using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Core.Toolkit.Collections;
using OpcRcw.Comn;
using OpcRcw.Da;

namespace Module.OPC.Models
{
    [Serializable]
    public class ConnectionGroup
    {
        private enum ServerWriteCapabilities
        {
            Unknown,
            None,
            Async,
            Sync
        }

        private OPCDataCallback _callback;
        private IOPCItemMgt _group;
        private readonly int _callbackCookie;
        private IOPCServer _server;
        private ServerWriteCapabilities _serverWriteCapabilities = ServerWriteCapabilities.Unknown;

        private const int OpcReadable = 1;
        private const int OpcWriteable = 2;

        public ConnectionGroup(string opcServer, string opcHost, AsyncObservableCollection<OpcPoint> channels)
        {            
            var t = opcHost.ToLowerInvariant() == "localhost" ? Type.GetTypeFromProgID(opcServer) : Type.GetTypeFromProgID(opcServer, opcHost);

            _server = (IOPCServer)Activator.CreateInstance(t);

            const int groupClientId = 1;
            var updateRate = 0;
            var tmpGuid = typeof(IOPCItemMgt).GUID;
            _server.AddGroup("fscdg", 1, updateRate, groupClientId, new IntPtr(), new IntPtr(), 0, out _, out updateRate, ref tmpGuid, out var groupObj);
            _group = (IOPCItemMgt)groupObj;
            
            var items = new OPCITEMDEF[channels.Count];
            for (var i = 0; i < channels.Count; i++)
            {
                items[i].bActive = 1;
                items[i].szItemID = channels[i].OpcChannel;
                items[i].hClient = channels[i].GetHashCode();
                items[i].dwBlobSize = 0;
                items[i].pBlob = IntPtr.Zero;
            }
            _group.AddItems(channels.Count, items, out var addResult, out var addErrors);

            var errors = new int[channels.Count];            
            Marshal.Copy(addErrors, errors, 0, channels.Count);

            for (var i = 0; i < channels.Count; i++)
            {
                if (errors[i] != 0)
                    continue;
                var pos = new IntPtr(addResult.ToInt64() + Marshal.SizeOf(typeof(OPCITEMRESULT)) * i);
                var res = (OPCITEMRESULT) Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));

                var readOnly = (res.dwAccessRights & OpcWriteable) != OpcWriteable;
                channels[i].Connect(this, res.hServer, readOnly);
            }

            Marshal.FreeCoTaskMem(addResult);
            Marshal.FreeCoTaskMem(addErrors);

            var cpc = (IConnectionPointContainer)groupObj;
            var dataCallbackGuid = typeof(IOPCDataCallback).GUID;
            cpc.FindConnectionPoint(ref dataCallbackGuid, out var cp);

            _callback = new OPCDataCallback(channels);
            cp.Advise(_callback, out _callbackCookie);
        }

        ~ConnectionGroup()
        {
            try
            {
                var cpc = (IConnectionPointContainer)_group;
                var dataCallbackGuid = typeof(IOPCDataCallback).GUID;
                cpc.FindConnectionPoint(ref dataCallbackGuid, out var cp);

                cp.Unadvise(_callbackCookie);
            }
            catch (COMException)
            {
            }

            _group = null;
            _callback = null;
            _server = null;
        }

        public bool WriteChannel(int channelHandle, object value)    
        {
            IOPCAsyncIO2 asyncIo = null;
            IOPCSyncIO syncIo = null;

            lock (this)
            {
                if (_serverWriteCapabilities == ServerWriteCapabilities.Unknown)
                {
                    try
                    {
                        asyncIo = (IOPCAsyncIO2)_group;
                        if (asyncIo != null)
                            _serverWriteCapabilities = ServerWriteCapabilities.Async;
                    }
                    catch (COMException)
                    {
                    }

                    if (asyncIo == null)
                    {
                        try
                        {
                            syncIo = (IOPCSyncIO)_group;
                            if (syncIo != null)
                                _serverWriteCapabilities = ServerWriteCapabilities.Sync;
                        }
                        catch (COMException)
                        {
                        }
                    }

                    if (asyncIo == null && syncIo == null)
                        _serverWriteCapabilities = ServerWriteCapabilities.None;
                }

                try
                {
                    switch (_serverWriteCapabilities)
                    {
                        case ServerWriteCapabilities.Async:
                            syncIo = (IOPCSyncIO)_group;
                            break;
                        case ServerWriteCapabilities.Sync:
                            syncIo = (IOPCSyncIO)_group;
                            break;
                        default:
                            return false;
                    }
                }
                catch (COMException)
                {
                }

                if (asyncIo == null && syncIo == null)
                    return false;

                if (asyncIo != null)
                {
                    asyncIo.Write(1, new int[] { channelHandle }, new object[] { value }, 0, out var cancelId, out var ppErrors);
                    Marshal.FreeCoTaskMem(ppErrors);
                }
                else
                {                    
                    syncIo.Write(1, new int[] { channelHandle }, new object[] { value }, out var ppErrors);
                    Marshal.FreeCoTaskMem(ppErrors);
                }

                return true;
            }
        }
    }
}

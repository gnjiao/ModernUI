using System;
using System.Collections.Generic;
using Core;
using Core.Toolkit.Collections;
using OpcRcw.Da;

namespace Module.OPC.Models
{
    class OPCDataCallback : IOPCDataCallback
    {
        Dictionary<int, OpcPoint> channels = new Dictionary<int, OpcPoint>();

        public OPCDataCallback(AsyncObservableCollection<OpcPoint> channels)
        {
            foreach (OpcPoint ch in channels)
            {
                this.channels[ch.GetHashCode()] = ch;
            }
        }

        #region IOPCDataCallback Members

        public void OnCancelComplete(int dwTransid, int hGroup)
        {
            throw new NotImplementedException();
        }

        const short Q_GOOD = 0x03 << 6;
        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
        {
            for (int i = 0; i < dwCount; i++)
            {
                OpcPoint ch;
                if (channels.TryGetValue(phClientItems[i], out ch))
                {
                    long ticks;
                    byte[] bticks = new byte[8];
                    BitConverter.GetBytes(pftTimeStamps[i].dwLowDateTime).CopyTo(bticks, 0);
                    BitConverter.GetBytes(pftTimeStamps[i].dwHighDateTime).CopyTo(bticks, 4);
                    ticks = BitConverter.ToInt64(bticks, 0);
                    DateTime dt = DateTime.FromFileTime(ticks);

                    PointStatusFlags status;
                    if ((pwQualities[i] & Q_GOOD) == Q_GOOD)
                        status = PointStatusFlags.Good;
                    else
                        status = PointStatusFlags.Bad;

                    ch.DoUpdate(pvValues[i], dt, status);

                    //Statistic.ChannelsCount++;
                    //Statistic.DoAnalysis();
                }
            }
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, FILETIME[] pftTimeStamps, int[] pErrors)
        {
            throw new NotImplementedException();
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            for (int i = 0; i < dwCount; i++)
            {
                OpcPoint ch;
                if (channels.TryGetValue(pClienthandles[i], out ch))
                    ch.StatusFlags = PointStatusFlags.Good;
            }
        }

        #endregion
    }
}

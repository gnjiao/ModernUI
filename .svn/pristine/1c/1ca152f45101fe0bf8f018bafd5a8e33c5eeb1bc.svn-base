using System;
using System.Reactive;
using Core;

namespace Module.OPC.Models
{
    [Serializable]
    public class OpcPoint : BasePoint
    {
        private ConnectionGroup _opcConnection;
        private int _opcHandle;

        public OpcPoint() : base($"variable", "", typeof(object), ReadWriteFlags.ReadWrite, "")
        {
        }
        
        public OpcPoint(
            string name, 
            string device,
            ReadWriteFlags readwrite,
            object defaultVal,
            object tag,

            string opcChannel, 
            string opcServer, 
            string opcHost)
            : base(name, device, typeof(object), readwrite, defaultVal)
        {
            this.OpcChannel = opcChannel;
            this.OpcHost = opcHost;
            this.OpcServer = opcServer;
            this.Tag = tag;
            base.ReadWrite = readwrite;
        }

        public string OpcChannel { get; set; }

        public string OpcServer { get; set; }

        public string OpcHost { get; set; }

        public override object Value
        {
            get => base.Value;
            set
            {
                if (ReadWrite != ReadWriteFlags.ReadOnly)
                {
                    _opcConnection?.WriteChannel(_opcHandle, value);
                    base.Value = value;
                }                
            }
        }
        

        internal void Connect(ConnectionGroup opcConnection, int opcHandle, bool readOnly)
        {
            _opcConnection = opcConnection;
            _opcHandle = opcHandle;
            ReadWrite = readOnly ? ReadWriteFlags.ReadOnly : ReadWriteFlags.ReadWrite;
        }

        internal void Disconnect()
        {
            _opcConnection = null;
            _opcHandle = 0;
        }

        public override void DoUpdate()
        {
        }        
    }
}

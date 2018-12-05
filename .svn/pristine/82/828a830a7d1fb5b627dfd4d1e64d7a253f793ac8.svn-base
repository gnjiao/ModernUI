using System;

namespace Core.Common
{
    public interface IDataAcquisitionPlug
    {
        event EventHandler ChannelsChanged;
        string Name { get; }
        IPoint[] Channels { get; }
        string PluginId { get; }
        bool IsConnected { get; }
        void Initialize();
        bool Connect();
        void Disconnect();        
    }

}

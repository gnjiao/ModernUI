using System;
using System.Collections;
using System.Linq;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using Core;
using Core.Common;
using Core.Patterns;
using Core.Serialization;
using Core.Toolkit.Collections;
using Microsoft.Practices.ServiceLocation;

namespace Module.OPC.Models
{    
    [Serializable]
    [ContentProperty("Points")]
    public class OpcPlugin : IDataAcquisitionPlug
    {
        private const string OpcPluginXaml = "OPC.Plugin.Settings.Xaml";
        public event EventHandler ChannelsChanged;

        public AsyncObservableCollection<OpcPoint> Points { get; set; } = new AsyncObservableCollection<OpcPoint>();
        // ReSharper disable once CollectionNeverQueried.Local
        public AsyncObservableCollection<ConnectionGroup> ConnectionGroups { get; set; } = new AsyncObservableCollection<ConnectionGroup>();
        
        ~OpcPlugin()
        {
            if (IsConnected)
                Disconnect();
        }

        #region IDataAcquisitionPlug Members


        public string Name => StringConstants.PluginName;
        
        public IPoint[] Channels
        {
            get => Points.ToArray<IPoint>();
            set
            {
                Points.Clear();
                Points.AddRange(Array.ConvertAll<IPoint, OpcPoint>(value, p => (OpcPoint)p));               
                Points.RemoveAll(ch => ch == null);
                FireChannelChangedEvent();
            }
        }

        public string PluginId => StringConstants.PluginId;

        public void Initialize()
        {
            LoadSettings();
        }

        public bool IsConnected { get; private set; }

        public bool Connect()
        {
            if (IsConnected)
                return false;

            ConnectionGroups.Clear();
            GC.Collect();

            if (Points.Count > 0)
            {
                var originalChannels = new AsyncObservableCollection<IPoint>();
                originalChannels.AddRange(Points);
                do
                {
                    var groupChannels = new AsyncObservableCollection<OpcPoint>();
                    var lhc = (OpcPoint)originalChannels[0];
                    groupChannels.Add(lhc);
                    originalChannels.RemoveAt(0);
                    for (var i = originalChannels.Count - 1; i >= 0; i--)
                    {
                        var rhc = (OpcPoint)originalChannels[i];
                        if (lhc.OpcServer != rhc.OpcServer || lhc.OpcHost != rhc.OpcHost) continue;
                        groupChannels.Add(rhc);
                        originalChannels.RemoveAt(i);
                    }
                    ConnectionGroups.Add(new ConnectionGroup(lhc.OpcServer, lhc.OpcHost, groupChannels));

                } while (originalChannels.Count > 0);
            }

            IsConnected = true;
            return IsConnected;
        }

        public void Disconnect()
        {
            IsConnected = false;

            foreach (var point in Points)
            {
                var ch = (OpcPoint) point;
                ch.Disconnect();
            }

            ConnectionGroups.Clear();

            GC.Collect();
        }

        #endregion
        

        public void SaveSettings()
        {            
            this.SerializeToXamlFile(OpcPluginXaml);
        }

        private void LoadSettings()
        {
            var opcPlugin = File.Exists(OpcPluginXaml) ? OpcPluginXaml.DeserializeFromXamlFile<OpcPlugin>() : null;

            if (opcPlugin != null)
                Channels = opcPlugin.Channels;
        }

        private void FireChannelChangedEvent()
        {
            ChannelsChanged?.Invoke(this, new EventArgs());
        }
    }

    
}

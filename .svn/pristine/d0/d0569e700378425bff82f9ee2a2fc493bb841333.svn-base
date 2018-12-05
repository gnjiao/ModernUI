using System.Linq;
using System.Windows;
using System.Windows.Input;
using Core;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Module.OPC.Models;

namespace Module.OPC.ViewModels
{
    public class OpcSettingsViewModel: BindableBase
    {
        public OpcPlugin OpcPluginViewModel { get; set; }
        public ICommand RunStopPluginCommand { get; set; }
        public ICommand SaveXamlCommand { get; set; }
        
        public OpcSettingsViewModel()
        {
            OpcPluginViewModel = ServiceLocator.Current.GetInstance<OpcPlugin>();
            OpcPluginViewModel.Initialize();

            var eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            eventAggregator.GetEvent<OpcPointsChangedEvent>().Subscribe(
                x =>
                {
                    var nCount = x.Count;
                    var opcPoints = new IPoint[nCount];

                    for (var i = 0; i < x.Count; i++)
                    {
                        var opcChannel = x[i];
                        
                        opcPoints[i] = ChannelFactory.CreateChannel(GetUniqueVariableName(opcPoints.ToArray<IPoint>()), "", ReadWriteFlags.ReadWrite,
                            opcChannel.Channel, opcChannel.ProgId, opcChannel.Host);                        
                    }


                    OpcPluginViewModel.Channels = opcPoints;
                });

            RunStopPluginCommand = new RelayCommand(o=>
            {
                if ((bool) o)
                {
                    if(!OpcPluginViewModel.IsConnected)
                        OpcPluginViewModel.Connect();
                }
                else
                {                    
                    OpcPluginViewModel.Disconnect();
                }
            });

            SaveXamlCommand = new DelegateCommand(() =>
            {
                if (OpcPluginViewModel.IsConnected)
                {
                    MessageBox.Show("Stop AutoRefresh first, please!");
                    return;
                }

                OpcPluginViewModel.SaveSettings();
                MessageBox.Show("SaveXamlCommand Ok!");
            });
        }

        private string GetUniqueVariableName(IPoint[] channels)
        {
            const string baseName = "variable_";

            var baseNumber = 1;

            for (; ; )
            {
                var newName = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName,
                    baseNumber);

                var exists = channels.Any(p => p?.Name == newName);

                if (exists == false)
                    return newName;

                baseNumber++;
            }
        }
    }    
}

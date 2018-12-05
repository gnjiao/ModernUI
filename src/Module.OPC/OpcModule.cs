using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common;
using Microsoft.Practices.Prism.Interactivity.DefaultPopupWindows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Module.OPC.Models;
using Module.OPC.ViewModels;

namespace Module.OPC
{
    public class OpcModule : IModule
    {
        public void Initialize()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType<IDataAcquisitionPlug, OpcPlugin>(nameof(OpcPlugin),new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType<OpcSettingsViewModel>(new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType<OpcImportViewModel>(new ContainerControlledLifetimeManager());
            
            Console.WriteLine($@"{nameof(OpcModule)} has been initialized at{DateTime.Now}.");
        }
    }
}

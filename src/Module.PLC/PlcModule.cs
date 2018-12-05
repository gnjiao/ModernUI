using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Module.PLC
{
    public class PlcModule : IModule
    {
        public void Initialize()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(PlcModule), new ContainerControlledLifetimeManager());

            Console.WriteLine($@"{nameof(PlcModule)} has been initialized at{DateTime.Now}.");
        }
    }
}

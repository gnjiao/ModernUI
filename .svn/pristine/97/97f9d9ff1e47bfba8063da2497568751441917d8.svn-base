using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CoreModule
{
    public class CoreModule : IModule
    {
        private readonly IUnityContainer _container;

        public CoreModule(IUnityContainer container)
        {
            _container = container ?? throw new ArgumentNullException($"{nameof(container)}");
        }

        public void Initialize()
        {
        }
    }
}

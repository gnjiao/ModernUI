using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using Core;
using CoreModule;
using FirstFloor.ModernUI.Presentation;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Platform.Main.Services;
using Platform.Main.Util;

namespace Platform.Main
{
    internal class Bootstrapper : UnityBootstrapper
    {
        private const string ModulesPath = @".\Modules";
        private const string SplashScreenPath = "SplashScreen.png";

        private SplashScreen _splashScreen;
        private LinkGroupCollection _linkGroupCollection;
        
        protected override IModuleCatalog CreateModuleCatalog()
        {
            _splashScreen = new SplashScreen(SplashScreenPath);
            _splashScreen.Show(false);
            
            return new DirectoryModuleCatalog { ModulePath = ModulesPath};
        }

        protected override void ConfigureModuleCatalog()
        {
            var directoryCatalog = (DirectoryModuleCatalog)ModuleCatalog;
            directoryCatalog.Initialize();

            _linkGroupCollection = new LinkGroupCollection();
            var typeFilter = new TypeFilter(InterfaceFilter);

            foreach (var module in directoryCatalog.Items)
            {
                var mi = (ModuleInfo)module;
                var asm = Assembly.LoadFrom(mi.Ref);

                foreach (var t in asm.GetTypes())
                {
                    var myInterfaces = t.FindInterfaces(typeFilter, typeof(ILinkGroupService).ToString());

                    if (myInterfaces.Length <= 0) continue;

                    var linkGroupService = (ILinkGroupService)asm.CreateInstance(t.FullName ?? throw new InvalidOperationException());
                    var linkGroup = linkGroupService?.GetLinkGroup();
                    _linkGroupCollection.Add(linkGroup);
                }
            }

            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(CoreModule.CoreModule));
        }

        private static bool InterfaceFilter(Type typeObj, object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());            
           
            var exe = typeof(Bootstrapper).Assembly;
            FileUtility.ApplicationRootPath = Path.GetDirectoryName(exe.Location);

            var container = new PlatformServiceContainer();
            container.AddFallbackProvider(ServiceSingleton.FallbackServiceProvider);
            container.AddService(typeof(ILoggingService), new Log4NetLoggingService());
            ServiceSingleton.ServiceProvider = container;

            var coreStartup = new CoreStartup("Core 5.x");
            var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "addin");
            var dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "data");
            var propertyService = new Services.PropertyService(DirectoryName.Create(configDirectory), DirectoryName.Create(dataDirectory), "custom");
            coreStartup.StartCoreServices(propertyService);

            PlatformServiceTools.ResourceService.Language = "en-US"; //language: zh-Hans simple chinese, en-US english
            PlatformServiceTools.ResourceService.RegisterNeutralStrings(new ResourceManager("Platform.Main.Properties.StringResources", exe));
            PlatformServiceTools.ResourceService.RegisterNeutralImages(new ResourceManager("Platform.Main.Properties.ImageResuorces", exe));

            CommandWrapper.LinkCommandCreator = link => new LinkCommand(link);
            CommandWrapper.WellKnownCommandCreator = Core.Presentation.MenuService.GetKnownCommand;
            CommandWrapper.RegisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested += eh);
            CommandWrapper.UnregisterConditionRequerySuggestedHandler = (eh => CommandManager.RequerySuggested -= eh);

            coreStartup.AddAddInsFromDirectory(FileUtility.ApplicationRootPath);
            coreStartup.RunInitialization();
        }
        
        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<Shell>();            

            if (_linkGroupCollection != null)
            {
                shell.AddLinkGroups(_linkGroupCollection);
            }

            shell.Loaded += (sender, e) =>
            {
                _splashScreen.Close(new TimeSpan(10));
                (sender as Shell)?.Activate();
            };

            return shell;
        }
        
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow?.Show();
        }        
        
    }
}

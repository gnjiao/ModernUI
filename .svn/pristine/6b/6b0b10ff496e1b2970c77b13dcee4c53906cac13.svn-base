using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Core;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Platform.Main.Annotations;
using Platform.Main.Util;
using Platform.Main.ViewModels;
using Platform.Main.Views.Aoi;
using Platform.Main.Views.Main;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using Xceed.Wpf.AvalonDock.Themes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class DesignPage : UserControl, INotifyPropertyChanged
    {
        public static DesignPage Instance { get; private set; } 

        private const string LayoutConfig = "Platform.config";
        private DockingManager DockManager { get; set; }
        private XmlLayoutSerializer Serializer { get; set; }
        private PlatformMainStatusBar _statusBar;
        private DispatcherTimer _dispatcherTimer;

        private PropertyItemBase _selectedPropertyGridItem;
        public ICommand OpenFileCommand { get; set; }
        public ICommand RunCommand { get; set; }
        
        public DesignPage()
        {
            RegisterTypes();

            CreateCommands();

            InitializeComponent();

            InitializeWindow();            
        }

        public static void StartUpMainWindow()
        {
            Instance = new DesignPage();
        }

        private static void RegisterTypes()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(ToolBoxView), new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(ImageViewer), new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(LogView), new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(PropertyBrowserView), new ContainerControlledLifetimeManager());
            ServiceLocator.Current.GetInstance<IUnityContainer>().RegisterType(typeof(ProjectView), new ContainerControlledLifetimeManager());            
        }

        private void CreateCommands()
        {
            OpenFileCommand = new RoutedCommand("OpenFileCommand", typeof(Shell));

            RunCommand = new DelegateCommand(() => { });
        }

        private void InitializeWindow()
        {
            _statusBar = new PlatformMainStatusBar { Background = System.Windows.Media.Brushes.Transparent};
            DockManager = new DockingManager { Theme = new GenericTheme() };
            Serializer = new XmlLayoutSerializer(DockManager);
            Serializer.LayoutSerializationCallback += (sender, args) =>
            {
                switch (args.Model.ContentId)
                {
                    case "LogView":
                        args.Content = ServiceLocator.Current.GetInstance<LogView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.LogViewTitle");
                        break;
                    case "ImageView":
                        args.Content = ServiceLocator.Current.GetInstance<ImageViewer>(); ;
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.ImageViewTitle");
                        args.Model.IsActive = true;
                        break;

                    case "ToolBoxView":
                        args.Content = ServiceLocator.Current.GetInstance<ToolBoxView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.ToolBoxViewTitle");
                        break;

                    case "PropertyBrowserView":
                        args.Content = ServiceLocator.Current.GetInstance<PropertyBrowserView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.PropertyBrowserTitle");
                        break;

                    case "ProjectView":
                        args.Content = ServiceLocator.Current.GetInstance<ProjectView>();
                        args.Model.Title = PlatformServiceTools.ResourceService.GetString("Base.ProjectTitle");
                        break;

                    default:
                        break;
                }
            };
           
            DockPanel.Children.Add(_statusBar);
            DockPanel.SetDock(_statusBar, Dock.Bottom);
            DockPanel.Children.Add(DockManager);

            Loaded += (sender, e) =>
            {
                if (File.Exists(LayoutConfig))
                    Serializer.Deserialize(LayoutConfig);

                _dispatcherTimer.Start();
            };

            Unloaded += (sender, e) =>
            {
                _dispatcherTimer?.Stop();

                Serializer.Serialize($"{LayoutConfig}.Bak");
            };

            _dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(1000) };

            _dispatcherTimer.Tick += (sender, e) =>
            { 
                _statusBar.TxtStatusBarPanel.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            };
            

            PlatformServiceTools.ResourceService.LanguageChanged += (sender, e) =>
            {
                ServiceLocator.Current.GetInstance<Shell>().Title = PlatformServiceTools.ResourceService.GetString("Base.WindowName");
            };
        }


//        private HDrawingObjectSerializable _editingDrawingObject;
//        public HDrawingObjectSerializable EditingDrawingObject
//        {
//            get => _editingDrawingObject;
//            set
//            {
//                if (Equals(value, _editingDrawingObject)) return;
//                _editingDrawingObject = value;
//                OnPropertyChanged();
//            }
//        }
//
//        private HDrawingObjectSerializable _selectedDrawingObject;
//        public HDrawingObjectSerializable SelectedDrawingObject
//        {
//            get => _selectedDrawingObject;
//            set
//            {
//                if (Equals(value, _selectedDrawingObject)) return;
//                _selectedDrawingObject = value;
//
//                EditingDrawingObject = _selectedDrawingObject;
//                OnPropertyChanged();
//            }
//        }
//
//        public PropertyItemBase SelectedPropertyGridItem
//        {
//            get => _selectedPropertyGridItem;
//            set
//            {
//                _selectedPropertyGridItem = value;
//
//                if (!(_selectedPropertyGridItem is PropertyItemBase))
//                {
//                }
//                else
//                {
//                    var propertyName = ((PropertyItemBase)_selectedPropertyGridItem).DisplayName;
//                    
//                }
//
//                if (SelectedDrawingObject != null)
//                {
//                }
//
//                OnPropertyChanged();
//            }
//        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

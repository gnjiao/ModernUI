using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Module.OPC.ViewModels;

namespace Module.OPC.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for OpcSettingsView.xaml
    /// </summary>
    public partial class OpcSettingsView : UserControl
    {
        public OpcSettingsView()
        {
            InitializeComponent();

            var opcSettingsViewModel = ServiceLocator.Current.GetInstance<IUnityContainer>().Resolve<OpcSettingsViewModel>();            

            DataContext = opcSettingsViewModel;
        }        
    }
}

using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Module.OPC.ViewModels;

namespace Module.OPC.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for OpcImportView.xaml
    /// </summary>
    public partial class OpcImportView : UserControl
    {
        public OpcImportView()
        {
            InitializeComponent();

            var opcImportViewModel = ServiceLocator.Current.GetInstance<IUnityContainer>().Resolve<OpcImportViewModel>();            

            WindowsFormsHostPanel.Child = opcImportViewModel.ChannelsTree;

            DataContext = opcImportViewModel;            
        }
    }
}

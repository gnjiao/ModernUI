using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Platform.Main.Annotations;
using Platform.Main.ViewModels;

namespace Platform.Main.Views
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for DesignPage.xaml
    /// </summary>
    public partial class AoiPage : UserControl
    {
        public AoiPage()
        {
            InitializeComponent();
           
            this.DataContext = new AoiViewModel();
        }        
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Platform.Main.Annotations;
using Platform.Main.ViewModels;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Platform.Main.Views.Aoi
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    /// Interaction logic for PropertyBrowserView.xaml
    /// </summary>
    public sealed partial class PropertyBrowserView :  INotifyPropertyChanged
    {
        public PropertyBrowserView()
        {
            InitializeComponent();

            BlockPropertyGrid.PropertyValueChanged += BlockPropertyGrid_PropertyValueChanged;            
        }

        private HDrawingObjectSerializable _selectedDrawingObjectSerializable;
        public HDrawingObjectSerializable SelectedDrawingObjectSerializable
        {
            get => _selectedDrawingObjectSerializable;
            set
            {
                //if (Equals(value, _selectedDrawingObjectSerializable)) return;
                _selectedDrawingObjectSerializable = value;
                OnPropertyChanged();
            }
        }

        private void BlockPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (!Equals(e.NewValue, e.OldValue))
            {
            }

            string s = _selectedDrawingObjectSerializable.ToString();
        }        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }    
}

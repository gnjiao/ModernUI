using System;
using System.Drawing;
using System.Windows.Forms;
using Core.Presentation;
using UserControl = System.Windows.Controls.UserControl;

namespace Platform.Main.Views.Aoi
{
    /// <summary>
    /// Interaction logic for ToolBoxView.xaml
    /// </summary>
    public partial class ToolBoxView : UserControl
    {
        private ToolBox _toolBox;
        public ToolBoxView()
        {
            InitializeComponent();

            CreateToolWindow();

            WindowsFormsHost.Child = _toolBox;
        }

        #region ToolBox Control

        private void CreateToolWindow()
        {
            _toolBox = new ToolBox
            {
                BorderStyle = BorderStyle.None, 
                ItemNormalColor = Color.BlanchedAlmond,
                ItemSelectedColor = Color.BurlyWood,
                ItemHoverColor = Color.BurlyWood,
            };
            
            _toolBox.AddTab($"Roi Control", -1);
                        
            _toolBox[$"Roi Control"].AddItem("Rectangle1", 0, 0, true, "Rectangle1");
            _toolBox[$"Roi Control"].AddItem("Rectangle2", 0, 0, true, "Rectangle2");
            _toolBox[$"Roi Control"].AddItem("Ellipse", 0, 0, true, "Ellipse");
            _toolBox[$"Roi Control"].AddItem("Circle", 0, 0, true, "Circle");

            _toolBox[0].Selected = true;
            _toolBox[0].SelectedItemIndex = 0;

        }

        #endregion
    }
}

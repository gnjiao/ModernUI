using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core.Serialization;
using HalconDotNet;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using Platform.Main.ViewModels;

namespace Platform.Main.Views.Aoi
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public sealed partial class ImageViewer : UserControl, INotifyPropertyChanged
    {
        public ICommand ZoomFitCommand { get; set; }
        public ICommand ZoomActualCommand { get; set; }
        public ICommand ImportImageCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _createOrder = null;
        private object _imageLock = new object();

        public Method method = new Method();

        public List<HDrawingObject> HDrawingObjects = new List<HDrawingObject>();
        public List<HDrawingObjectSerializable> HDrawingObjectSerializables = new List<HDrawingObjectSerializable>();
        //public List<HDrawingObjectSerializable> HDrawingObjectSerializables = new List<HDrawingObjectSerializable>();

        public HDrawingObjectFactory HDrawingObjectFactoryImp = new HDrawingObjectFactory();

        private HDrawingObject _selectedDrawingObject;
        

        public ImageViewer()
        {
            PreviewKeyUp += (sneder, e) =>
            {
                if (e.Key == Key.Delete && _selectedDrawingObject != null)
                {
                    if (MessageBox.Show("删除当点ROI?", "Waring", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {                        
                        HDrawingObjects.Remove(HDrawingObjects.FirstOrDefault(p => p.ID == _selectedDrawingObject.ID));
                        HDrawingObjectFactoryImp.Remove(HDrawingObjectFactoryImp.FirstOrDefault(p =>
                            p.DrawingObjectImp()?.ID == _selectedDrawingObject.ID));
                        
                        HWindowControlWpf.HalconWindow.DetachDrawingObjectFromWindow(_selectedDrawingObject);                        

                        HWindowControlWpf.HalconWindow.ClearWindow();

                        _selectedDrawingObject = null;
                    }
                }
            };

            ZoomFitCommand = new DelegateCommand(ZoomFit);

            ZoomActualCommand = new DelegateCommand(ZoomActual);

            ImportImageCommand = new DelegateCommand(() =>
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "*.tif|*.tif|*.tiff|*.tiff|*.*|*.*"
                };

                if (dialog.ShowDialog() != true)
                    return;

                lock (_imageLock)
                {
                    foreach (var dobj in HDrawingObjects)
                    {
                        HOperatorSet.ClearDrawingObject(dobj);
                    }
                    HDrawingObjects.Clear();
                    HDrawingObjectFactoryImp.Clear();
                }

                HWindowControlWpf.HalconWindow.ClearWindow();
                //_selectedDrawingObject = null;

                var lImage = new HImage();
                lImage.ReadImage(dialog.FileName);
                Image = lImage;

                ZoomFit();
                
            });

            RunCommand = new DelegateCommand(Run);

            SaveCommand = new DelegateCommand(Save);

            LoadCommand = new DelegateCommand(Load);
            InitializeComponent();

            ImageViewToolBar.Loaded += ImageViewToolBar_Loaded;

            HWindowControlWpf.HInitWindow += HWindowControl_HInitWindow;
        }

        private void HWindowControl_HInitWindow(object sender, EventArgs e)
        {
            HWindowControlWpf.HMouseMove += HWindowControl_HMouseMove;
            HWindowControlWpf.AddHandler(HSmartWindowControlWPF.DropEvent, new DragEventHandler(HSmartWindowControlWPFRoutedEventHandler), true);
        }

        private void HWindowControl_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
        {
            Column = e.Column;
            Row = e.Row;

            if (!string.IsNullOrEmpty(_createOrder))
            {
                HDrawingObject newDrawingObject;
                switch (_createOrder)
                {
                    case "Rectangle1":
                        newDrawingObject = HDrawingObject.CreateDrawingObject(
                            HDrawingObject.HDrawingObjectType.RECTANGLE1, Row, Column, Row + 200, Column + 200);
                        break;

                    case "Rectangle2":
                        newDrawingObject = HDrawingObject.CreateDrawingObject(
                            HDrawingObject.HDrawingObjectType.RECTANGLE2, Row, Column, 0, 100, 100);
                        break;

                    case "Ellipse":
                        newDrawingObject = HDrawingObject.CreateDrawingObject(
                            HDrawingObject.HDrawingObjectType.ELLIPSE, Row, Column, 0, 100, 50);
                        break;

                    case "Circle":
                        newDrawingObject = HDrawingObject.CreateDrawingObject(
                            HDrawingObject.HDrawingObjectType.CIRCLE, Row, Column, 100);
                        break;

                    default:
                        return;
                }

                
                HDrawingObjects.Add(newDrawingObject);
                HDrawingObjectFactoryImp.Add(new HDrawingObjectSerializable(newDrawingObject));
                _selectedDrawingObject = newDrawingObject;
                AttachDrawObjToWindow(_selectedDrawingObject);
                _createOrder = null;
            }
        }

        public void AttachDrawObjToWindow(HDrawingObject hDrawingObject)
        {
            if (Image != null && Image.Key != IntPtr.Zero)
            {
                _selectedDrawingObject = hDrawingObject;

                if (_selectedDrawingObject != null /*&& _selectedDrawingObject.Handle != IntPtr.Zero*/)
                {
                    _selectedDrawingObject.SetDrawingObjectParams("color", "green");
                    AttachDrawObj(_selectedDrawingObject);
                }
            }
        }
        
        private void HSmartWindowControlWPFRoutedEventHandler(object sender, DragEventArgs e)
        {
            var data = e.Data;

            if (data.GetDataPresent(typeof(string)))
            {
                _createOrder = data.GetData(typeof(string)) as string;
            }
        }

        private void SobelFilter(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                HImage image = Image;
                HRegion region = new HRegion(dobj.GetDrawingObjectIconic());
                hwin.SetWindowParam("flush", "false");
                hwin.ClearWindow();
                hwin.DispObj(image.ReduceDomain(region).SobelAmp("sum_abs", 11));
                hwin.SetWindowParam("flush", "true");
                hwin.FlushBuffer();
            }
            catch (HalconException)
            {
                //
            }
        }

        [Microsoft.Practices.Unity.Dependency]
        public PropertyBrowserView PropertyBrowserViewImp { get; set; }

        private void OnSelectDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            
            PropertyBrowserViewImp.SelectedDrawingObjectSerializable = HDrawingObjectFactoryImp.FirstOrDefault(p => p.DrawingObjectImp()?.ID == _selectedDrawingObject.ID)?.UpdateHDrawingObjectSerializable(_selectedDrawingObject);
            OnPropertyChanged();
            SobelFilter(dobj, hwin, type);
        }

        private void OnResizeDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            PropertyBrowserViewImp.SelectedDrawingObjectSerializable = HDrawingObjectFactoryImp.FirstOrDefault(p => p.DrawingObjectImp()?.ID == _selectedDrawingObject.ID)?.UpdateHDrawingObjectSerializable(_selectedDrawingObject);
            OnPropertyChanged();
            SobelFilter(dobj, hwin, type);
        }

        private void OnDragDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            _selectedDrawingObject = dobj;
            PropertyBrowserViewImp.SelectedDrawingObjectSerializable = HDrawingObjectFactoryImp.FirstOrDefault(p => p.DrawingObjectImp()?.ID == _selectedDrawingObject.ID)?.UpdateHDrawingObjectSerializable(_selectedDrawingObject);
            OnPropertyChanged();
            SobelFilter(dobj, hwin, type);
        }
        private void AttachDrawObj(HDrawingObject obj)
        {
            //_drawingObjects.Add(obj);

            obj.OnDrag(OnDragDrawingObject);
            obj.OnAttach(SobelFilter);
            obj.OnResize(OnResizeDrawingObject);
            obj.OnSelect(OnSelectDrawingObject);

            if (_selectedDrawingObject == null)
                _selectedDrawingObject = obj;

            PropertyBrowserViewImp.SelectedDrawingObjectSerializable = new HDrawingObjectSerializable(_selectedDrawingObject);
            HWindowControlWpf.HalconWindow.AttachDrawingObjectToWindow(obj);
        }

        #region Row
        public double Row
        {
            get { return (double)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        public static readonly DependencyProperty RowProperty = DependencyProperty.Register(
            "Row", typeof(double), typeof(ImageViewer));

        #endregion

        #region Column
        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column", typeof(double), typeof(ImageViewer));

        #endregion

        private void ImageViewToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            var toolBar = sender as ToolBar;

            if (toolBar?.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
                overflowGrid.Visibility = Visibility.Collapsed;

            if (toolBar?.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
                mainPanelBorder.Margin = new Thickness(0);
        }
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Show(HImage hImage)
        {
            Image = hImage;

            ZoomFit();
        }

        public void SmartShow(HImage hImage)
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;
            if (hImage != null)
                HWindowControlWpf.HalconWindow.DispImage(hImage);
        }

        public void ZoomFit()
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;

            X = 0;
            Y = 0;

            int width, height;
            Image.GetImageSize(out width, out height);

            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);

            var ratioImage = (double)width / (double)height;
            var ratioWindow = (double)widthExtent / (double)heightExtent;

            double finalRatio = 0;
            if (ratioImage > ratioWindow)
            {
                finalRatio = (double)widthExtent / (double)width;
                Y = 0 + (height - heightExtent / finalRatio) / 2;
            }
            else
            {
                finalRatio = (double)heightExtent / (double)height;
                X = 0 + (width - widthExtent / finalRatio) / 2;
            }

            Scale = finalRatio;

            Refresh();
        }

        public void ZoomActual()
        {
            X = 0;
            Y = 0;
            Scale = 1.0;

            Refresh();
        }

        public void ClearWindow()
        {

            if (_selectedDrawingObject != null)
            {
                HWindowControlWpf.HalconWindow.DetachDrawingObjectFromWindow(_selectedDrawingObject);
                _selectedDrawingObject.Dispose();
                _selectedDrawingObject = null;
            }

            HWindowControlWpf.HalconWindow.ClearWindow();
        }
        

        public bool HMoveContent
        {
            set
            {
                _contentLoaded = value;
                HWindowControlWpf.HMoveContent = true;
                OnPropertyChanged(nameof(HMoveContent));
                OnPropertyChanged(nameof(HHandContent));
            }

            get { return HWindowControlWpf?.HMoveContent != null && (bool) HWindowControlWpf?.HMoveContent; }
        }

        public bool HHandContent
        {
            set
            {
                _contentLoaded = value;
                HMoveContent = false;
                OnPropertyChanged(nameof(HMoveContent));
                OnPropertyChanged(nameof(HHandContent));
            }

            get { return HWindowControlWpf?.HMoveContent != null && (bool) !HWindowControlWpf?.HMoveContent; }
        }

        private void Refresh()
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;

            int width, height;
            Image.GetImageSize(out width, out height);

            int rowExtent, columnExtent, widthExtent, heightExtent;
            HWindowControlWpf.HalconWindow.GetWindowExtents(out rowExtent, out columnExtent,
                out widthExtent, out heightExtent);

            HWindowControlWpf.HImagePart = new Rect
            (
                X,
                Y,
                widthExtent / Scale,
                heightExtent / Scale
            );            
        }

        #region Image

        public HImage Image
        {
            get { return (HImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(HImage), typeof(ImageViewer), new FrameworkPropertyMetadata(OnImageChangedCallback));

        private static void OnImageChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var me = s as ImageViewer;
            if (me == null) return;

            me.HWindowControlWpf.HalconWindow.ClearWindow();

            var image = e.NewValue as HImage;
            if (image == null || image.Key == IntPtr.Zero)
            {
                me.HWindowControlWpf.HalconWindow.DetachBackgroundFromWindow();
            }
            else
            {
                me.HWindowControlWpf.HalconWindow.AttachBackgroundToWindow(image);
            }

            me.Refresh();
        }

        #endregion

        #region Scale

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(1.0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var me = s as ImageViewer;

            me?.Refresh();
        }

        #endregion

        #region X

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion

        #region Y

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(ImageViewer), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion

        private void Load()
        {
            if (Image == null || Image.Key == IntPtr.Zero)
            {
                MessageBox.Show("Import image first, please!");
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "*.xaml|*.xaml"
            };

            if (dialog.ShowDialog() != true)
                return;


            HDrawingObjectFactoryImp.Clear();
            HDrawingObjectFactoryImp = HDrawingObjectFactory.LoadFormXaml(dialog.FileName);

            for (int i = 0; i < HDrawingObjectFactoryImp.Count; i++)
            {
                method.PositionCorrection(Image, HDrawingObjectFactoryImp[i], out var hdrawingObjectFactoryImpout);
                HDrawingObjectFactoryImp[i] = hdrawingObjectFactoryImpout;

            }

            HDrawingObjectSerializables.Clear();
            HDrawingObjectSerializables = HDrawingObjectFactoryImp;

            HDrawingObjects.Clear();
            HDrawingObjects = HDrawingObjectFactoryImp.ConvertToDrawingObjectList();


            foreach (var newDrawingObject in HDrawingObjects)
            {
                _selectedDrawingObject = newDrawingObject;
                AttachDrawObjToWindow(_selectedDrawingObject);
            }

            PropertyBrowserViewImp.SelectedDrawingObjectSerializable = HDrawingObjectFactoryImp.FirstOrDefault(p => p.DrawingObjectImp()?.ID == _selectedDrawingObject.ID);
        }

        private void Save()
        {
            if (HDrawingObjects == null || HDrawingObjects.Count <= 0) return;

            var dialog = new SaveFileDialog
            {
                Filter = "*.xaml|*.xaml"
            };

            if (dialog.ShowDialog() != true)
                return;

            //var factory = new HDrawingObjectFactory();

            //factory.AddDrawingObjectList(HDrawingObjects);

            //factory.SaveToXaml(dialog.FileName);      

            var hDrawingObjectSerializable = HDrawingObjectFactoryImp.SingleOrDefault(x => x.RegionType.ToString() == "TemplateRegion");
            if (hDrawingObjectSerializable == null)
            {
                MessageBox.Show($"hDrawingObjectSerializable is null, Save error!");
                return;
            }

            var regionhobject = hDrawingObjectSerializable.HDrawingObjectDeserialize();
            HRegion region = new HRegion(regionhobject.GetDrawingObjectIconic());
            var imgreduce = Image.ReduceDomain(region).CropDomain();
            var templatepaht = hDrawingObjectSerializable.TemplateRegionPath;
            imgreduce.WriteImage("tiff",0, templatepaht);
            
            double rowout, colout, angleout;
            method.PositionOfTemplate(Image, hDrawingObjectSerializable.TemplateRegionPath, out rowout, out colout, out angleout);

            foreach (var hdrawingObjectFactoryImp in HDrawingObjectFactoryImp)
            {
                hdrawingObjectFactoryImp.CenterRow = rowout;
                hdrawingObjectFactoryImp.CenterCol = colout;
                hdrawingObjectFactoryImp.CenterAngle = angleout;
                hdrawingObjectFactoryImp .TemplateRegionPath= templatepaht;

                if (hdrawingObjectFactoryImp.RoiType.ToString()=="Spot")
                {
                    var regionhobject2 = hdrawingObjectFactoryImp.HDrawingObjectDeserialize();
                    HRegion region2 = new HRegion(regionhobject2.GetDrawingObjectIconic());
                    var imgreduce2 = Image.ReduceDomain(region2).CropDomain();
                    imgreduce2.WriteImage("tiff", 0, hdrawingObjectFactoryImp.ModelPath);
                }
            }

            HDrawingObjectFactoryImp.SaveToXaml(dialog.FileName);
        }

        private void Run()
        {
            Inspection();
            var hImage = Image;

            foreach (var hDrawingObject in HDrawingObjects)
            {
                string type = hDrawingObject.GetDrawingObjectParams("type");

                double row, column,row1, column1, row2, column2, phi, length1, length2, radius, radius1, radius2;

                switch (type)
                {
                    case "rectangle1":
                        row1 = hDrawingObject.GetDrawingObjectParams("row1");
                        column1 = hDrawingObject.GetDrawingObjectParams("column1");
                        row2 = hDrawingObject.GetDrawingObjectParams("row2");
                        column2 = hDrawingObject.GetDrawingObjectParams("column2");
                        break;

                    case "rectangle2":
                        row = hDrawingObject.GetDrawingObjectParams("row");
                        column = hDrawingObject.GetDrawingObjectParams("column");
                        phi = hDrawingObject.GetDrawingObjectParams("phi");
                        length1 = hDrawingObject.GetDrawingObjectParams("length1");
                        length2 = hDrawingObject.GetDrawingObjectParams("length2");
                        break;

                    case "circle":
                        row = hDrawingObject.GetDrawingObjectParams("row");
                        column = hDrawingObject.GetDrawingObjectParams("column");
                        radius = hDrawingObject.GetDrawingObjectParams("radius");
                        break;

                    case "ellipse":
                        row = hDrawingObject.GetDrawingObjectParams("row");
                        column = hDrawingObject.GetDrawingObjectParams("column");
                        phi = hDrawingObject.GetDrawingObjectParams("phi");
                        radius1 = hDrawingObject.GetDrawingObjectParams("radius1");
                        radius2 = hDrawingObject.GetDrawingObjectParams("radius2");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }

        private void Inspection()
        {
            var hImage = Image;
            bool isng = false;
            


            foreach (var hDrawingObjectSerializable in HDrawingObjectSerializables)
            {
                var type = hDrawingObjectSerializable.RoiType.ToString();


                switch (type)
                {
                    case "Spot"://检测螺丝的有无
                        method.InspectionOnScrew(hImage, hDrawingObjectSerializable,out isng);
                        ShowOkNG(hDrawingObjectSerializable,isng);
                        break;

                    case "Defect":


                        method.InspectionOnTinSolder(hImage, hDrawingObjectSerializable, out isng);
                        ShowOkNG(hDrawingObjectSerializable,isng);
                        break;


                    default:
                        break;
                }
                HWindowControlWpf.HalconWindow.DispImage(Image);

 

            }

        }

        private void ShowOkNG(HDrawingObjectSerializable hDrawingObjectSerializable,bool isNG)
        {
            if (isNG)
            {
                HTuple hv_Font, hv_FontWithSize;
                HOperatorSet.QueryFont(HWindowControlWpf.HalconWindow, out hv_Font);
                //Specify font name and size
                hv_FontWithSize = (hv_Font.TupleSelect(1)) + "-40";
                HOperatorSet.SetFont(HWindowControlWpf.HalconWindow, hv_FontWithSize);
                HOperatorSet.DispText(HWindowControlWpf.HalconWindow, "NG", "window", "top", "left",
                    "red", new HTuple(), new HTuple());
                var regionhobject2 = hDrawingObjectSerializable.HDrawingObjectDeserialize();
                HRegion region2 = new HRegion(regionhobject2.GetDrawingObjectIconic());
                HWindowControlWpf.HalconWindow.SetColor("red");
                region2.DispRegion(HWindowControlWpf.HalconWindow);

            }
            else
            {
                HTuple hv_Font, hv_FontWithSize;
                HOperatorSet.QueryFont(HWindowControlWpf.HalconWindow, out hv_Font);
                //Specify font name and size
                hv_FontWithSize = (hv_Font.TupleSelect(1)) + "-40";
                HOperatorSet.SetFont(HWindowControlWpf.HalconWindow, hv_FontWithSize);
                HOperatorSet.DispText(HWindowControlWpf.HalconWindow, "OK", "window", "top", "left", "green", new HTuple(), new HTuple());
                //DispText(HTuple windowHandle, HTuple stringVal, HTuple coordSystem, HTuple row, HTuple column, HTuple color, HTuple genParamName, HTuple genParamValue);
            }
        }


    }
}

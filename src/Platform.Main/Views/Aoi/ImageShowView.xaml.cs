using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using HalconDotNet;
using Platform.Main.Annotations;

namespace Platform.Main.Views.Aoi
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    /// Interaction logic for ImageShowView.xaml
    /// </summary>
    public partial class ImageShowView : UserControl, INotifyPropertyChanged
    {
        public ImageShowView()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Image

        public HImage Image
        {
            get => (HImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(HImage), typeof(ImageShowView), new FrameworkPropertyMetadata(OnImageChangedCallback));

        private static void OnImageChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            if (!(s is ImageShowView me)) return;

            me.HWindowControlWpf.HalconWindow.ClearWindow();

            if (!(e.NewValue is HImage image) || image.Key == IntPtr.Zero)
            {
                me.HWindowControlWpf.HalconWindow.DetachBackgroundFromWindow();
            }
            else
            {
                me.HWindowControlWpf.HalconWindow.AttachBackgroundToWindow(image);
            }

            me.Refresh();
        }

        private void Refresh()
        {
            if (Image == null)
                return;

            if (Image.Key == IntPtr.Zero)
                return;

            Image.GetImageSize(out var width, out int height);

            HWindowControlWpf.HalconWindow.GetWindowExtents(out var rowExtent, out var columnExtent,
                out var widthExtent, out var heightExtent);

            HWindowControlWpf.HImagePart = new Rect
            (
                X,
                Y,
                widthExtent / Scale,
                heightExtent / Scale
            );
        }

        #endregion

        #region Scale

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(ImageShowView), new FrameworkPropertyMetadata(1.0, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var me = s as ImageShowView;

            me?.Refresh();
        }

        #endregion

        #region X

        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(ImageShowView), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion

        #region Y

        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(ImageShowView), new FrameworkPropertyMetadata(PropertyChangedCallback));

        #endregion
    }
}

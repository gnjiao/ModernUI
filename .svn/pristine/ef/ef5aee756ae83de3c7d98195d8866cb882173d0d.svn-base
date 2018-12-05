using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Core;
using Core.Common;
using FirstFloor.ModernUI.Presentation;
using HalconDotNet;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Platform.Main.Models;
using Platform.Main.Views.Aoi;

namespace Platform.Main.ViewModels
{
    public class AoiViewModel : BindableBase
    {
        
        private const string StartPoint = "variable_1";
        private const string EndPoint   = "variable_2";
        private const string IsNgPoint  = "variable_3";
        private IPoint _endPointImp;
        private IPoint _isNgPointImp;

        private Cameras camera;
        private readonly HTuple _hvAcqHandle1 = null;
        public ICommand RunCommand { get; set; }
        public ICommand GrabImageCommand { get; set; }
        public ImageShowView ImageShowView1 { get; set; }

        public AoiViewModel()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>()
                .RegisterType<ImageShowView>("ImageShowView1", new ContainerControlledLifetimeManager());

            ImageShowView1 = ServiceLocator.Current.GetInstance<IUnityContainer>()
                .Resolve<ImageShowView>("ImageShowView1");

            RunCommand = new DelegateCommand(() =>
            {
                var plugs = ServiceLocator.Current.GetAllInstances<IDataAcquisitionPlug>();

                foreach (var plug in plugs)
                {
                    if (!plug.IsConnected)
                    {
                        plug.Initialize();
                        plug.Connect();
                    }

                    var startPointImp = plug.Channels.FirstOrDefault(p => p.Name == StartPoint);
                    _endPointImp = plug.Channels.FirstOrDefault(p => p.Name == EndPoint);
                    _isNgPointImp = plug.Channels.FirstOrDefault(p => p.Name == IsNgPoint);

                    if (startPointImp != null)
                        startPointImp.ValueChanged += Point_ValueChanged;
                    
                }
            });
            
            camera = new Cameras()
            {
                HFramegrabberImp = new HFramegrabber(),
                OpenFramegrabberName = "DirectShow",
                OpenFramegrabberHorizontalResolution = 1,
                OpenFramegrabberVerticalResolution = 1,
                OpenFramegrabberImageWidth = 0,
                OpenFramegrabberImageHeight = 0,
                OpenFramegrabberStartRow = 0,
                OpenFramegrabberStartColumn = 0,
                OpenFramegrabberField = "default",
                OpenFramegrabberBitsPerChannel = -1,
                OpenFramegrabberColorSpace = "rgb",
                OpenFramegrabberGeneric = "-1",
                OpenFramegrabberExternalTrigger = "default",
                OpenFramegrabberCameraType = "default",
                OpenFramegrabberDevice = "[0] Integrated Webcam",
                OpenFramegrabberPort = 0,
                OpenFramegrabberLineIn = -1,
            };

            //camera.HFramegrabberImp.SetFramegrabberParam("ExposureTime", "10000");
            //camera.HFramegrabberImp.SetFramegrabberParam("Gain", "10");

            camera.Initialize();
            

        }

        private readonly object _lockObject = new object();

        private void Point_ValueChanged(object sender, EventArgs e)
        {
            var point = (IPoint) sender;

            if (!int.TryParse(point.Value.ToString(), out var value) || value <= 0) return;

            lock (_lockObject)
            {
                var hImage1 = camera.HFramegrabberImp.GrabImage();                
                                
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _endPointImp.Value = point.Value;
                    ImageShowView1.Image = hImage1;

                    _isNgPointImp.Value = point.Value;
                }));
            }
        }

        private static void HobjectToHimage(HObject hobject, out HImage image)
        {
            image = new HImage();
            HOperatorSet.GetImagePointer1(hobject, out var pointer, out var type, out var width, out var height);
            image.GenImage1(type, width, height, pointer);
        }
    }
}

using HalconDotNet;

namespace Platform.Main.Models
{
    public class Cameras
    {
        public HFramegrabber HFramegrabberImp { get; set; }
        //        public HFramegrabber HFramegrabber;
        public void Initialize()
        {
            HFramegrabberImp.OpenFramegrabber(OpenFramegrabberName,
                                           OpenFramegrabberHorizontalResolution,
                                           OpenFramegrabberVerticalResolution,
                                           OpenFramegrabberImageWidth,
                                           OpenFramegrabberImageHeight,
                                           OpenFramegrabberStartRow,
                                           OpenFramegrabberStartColumn,
                                           OpenFramegrabberField,
                                           OpenFramegrabberBitsPerChannel,
                                           OpenFramegrabberColorSpace,
                                           OpenFramegrabberGeneric,
                                           OpenFramegrabberExternalTrigger,
                                           OpenFramegrabberCameraType,
                                           OpenFramegrabberDevice,
                                           OpenFramegrabberPort,
                                           OpenFramegrabberLineIn);
            HFramegrabberImp.GrabImageStart(5000);
        }

        public HImage GrabImage()
        {

            var image = HFramegrabberImp.GrabImage();
            return image;
        }
        //常用
        public HImage GrabImageAsync()
        {

            var image = HFramegrabberImp.GrabImageAsync(-1.0);
            return image;
        }

        public void Dispose()
        {
            //HOperatorSet.CloseFramegrabber(HFramegrabber);
            HFramegrabberImp.Dispose();
        }

        public string OpenFramegrabberName { get; set; } = "DirectShow";
        public int OpenFramegrabberHorizontalResolution { get; set; } = 1;
        public int OpenFramegrabberVerticalResolution { get; set; } = 1;
        public int OpenFramegrabberImageWidth { get; set; } = 0;
        public int OpenFramegrabberImageHeight { get; set; } = 0;
        public int OpenFramegrabberStartRow { get; set; } = 0;
        public int OpenFramegrabberStartColumn { get; set; } = 0;
        public string OpenFramegrabberField { get; set; } = "default";
        public int OpenFramegrabberBitsPerChannel { get; set; } = -1;
        public string OpenFramegrabberColorSpace { get; set; } = "default";
        public string OpenFramegrabberGeneric { get; set; } = "-1";
        public string OpenFramegrabberExternalTrigger { get; set; } = "default";
        public string OpenFramegrabberCameraType { get; set; } = "default";
        public string OpenFramegrabberDevice { get; set; } = "default";
        public int OpenFramegrabberPort { get; set; } = -1;
        public int OpenFramegrabberLineIn { get; set; } = -1;
    }
}

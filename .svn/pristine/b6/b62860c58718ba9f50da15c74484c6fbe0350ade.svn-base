using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Platform.Main.ViewModels;

namespace Platform.Main.Views.Aoi
{
    public class Method
    {
        public void InspectionOnTinSolder(HImage HImage, HDrawingObjectSerializable hDrawingObjectSerializable, out bool isNG)
        {
            var regionhobject = hDrawingObjectSerializable.HDrawingObjectDeserialize();
            HRegion region = new HRegion(regionhobject.GetDrawingObjectIconic());

            var imgcopy = HImage.CopyImage();
            var imgreduce = imgcopy.ReduceDomain(region);
            HImage imgR, imgG, imgB;
            imgR = imgreduce.Decompose3(out imgG, out imgB);

            double _threshold = 200;//rgb三色提取的阈值
            var _rArea = imgR.Threshold(hDrawingObjectSerializable.Threshold_R, 255).RegionFeatures("area");
            var _gArea = imgG.Threshold(hDrawingObjectSerializable.Threshold_G, 255).RegionFeatures("area");
            var _bArea = imgB.Threshold(hDrawingObjectSerializable.Threshold_B, 255).RegionFeatures("area");

            var g_rRatio = _gArea / _rArea;
            var b_rRatio = _bArea / _rArea;

            double OKGR_Ratio = 1;//经验值
            double OKBR_Ratio = 1;

            isNG = false;
            if (Math.Abs(g_rRatio - hDrawingObjectSerializable.G_R_radio) < hDrawingObjectSerializable.G_R_Tolerane && Math.Abs(b_rRatio - hDrawingObjectSerializable.B_R_radio) < hDrawingObjectSerializable.B_R_Tolerane)
            {
                isNG = true;

            }
            else
            {
                isNG = false;

            }

        }

        public void InspectionOnScrew(HImage HImage, HDrawingObjectSerializable hDrawingObjectSerializable, out bool isNG)
        {
            //检测有无用末班匹配去搜索，没搜到或者得分低的直接判断为没有螺丝
            var _himage = HImage.CopyImage();
            HImage modelimg = new HImage();
            modelimg.ReadImage(hDrawingObjectSerializable.ModelPath);
            HShapeModel model = new HShapeModel(modelimg.Rgb1ToGray().ZoomImageFactor(0.5,0.5, "constant").MeanImage(13, 13), 8, -3.14, 3.14, "auto", "auto", "use_polarity", "auto", "auto");
            HTuple Row, Column, Angle, Score;
            model.FindShapeModel(_himage.Rgb1ToGray().ZoomImageFactor(0.5, 0.5, "constant").MeanImage(13, 13), -3.14, 3.14, 0.3, 20, 0.5, "least_squares", 8, 0.9, out Row, out Column, out Angle, out Score);

            if (Score < 0.4 || Angle == null)
            {
                isNG = true;
            }
            else
            {
                isNG = false;

            }
            _himage.Dispose();
        }

        public void InspectionUsing(HImage HImage, string modelstr, out bool isNG)
        {
            isNG = true;
        }

        public void SaveModelImage(HImage HImage, string modelstr)
        {
            var _img = HImage.CopyImage();
            _img.WriteImage("tiff", 0, "b://123.tiff");
        }

        
        public void PositionCorrection(HImage HImage, HDrawingObjectSerializable hDrawingObjectSerializable, out HDrawingObjectSerializable hDrawingObjectSerializableout)
        {
            var _himage = HImage.CopyImage();
            HImage modelimg = new HImage();
            modelimg.ReadImage(hDrawingObjectSerializable.TemplateRegionPath);
            HShapeModel model = new HShapeModel(modelimg.Rgb1ToGray().ZoomImageFactor(0.5, 0.5, "constant").MeanImage(13, 13), 8, -3.14, 3.14, "auto", "auto", "use_polarity", "auto", "auto");
            HTuple HRow, HColumn, HAngle, HScore;
            model.FindShapeModel(_himage.Rgb1ToGray().ZoomImageFactor(0.5, 0.5, "constant").MeanImage(13, 13), -3.14, 3.14, 0.3, 20, 0.5, "least_squares", 8, 0.9, out HRow, out HColumn, out HAngle, out HScore);

            var maxIndex =  HScore.TupleFind(HScore.TupleMax());

            var relativeY = hDrawingObjectSerializable.Row - hDrawingObjectSerializable.CenterRow;
            var relativeX = hDrawingObjectSerializable.Column - hDrawingObjectSerializable.CenterCol;

            var lenght = Math.Sqrt(relativeX * relativeX + relativeY * relativeY);
            double theta2 = 0;
            if (relativeY!=0)
            {
                if (relativeY<0)
                {
                    theta2 = Math.PI / 2 - Math.Atan(relativeX / relativeY) + hDrawingObjectSerializable.CenterAngle - HAngle[maxIndex] + Math.PI;
                }
                else
                {
                    theta2 = Math.PI / 2 - Math.Atan(relativeX / relativeY) + hDrawingObjectSerializable.CenterAngle - HAngle[maxIndex]+ Math.PI*2;
                }
                    


                
            }
            else
            {
                theta2= hDrawingObjectSerializable.CenterAngle  - HAngle[maxIndex] + Math.PI;
            }
            var relativeXnew = lenght * Math.Cos(theta2);
            var relativeYnew = lenght * Math.Sin(theta2);

            hDrawingObjectSerializableout = hDrawingObjectSerializable;
            hDrawingObjectSerializableout.Row = HRow[maxIndex]*2 + relativeYnew;
            hDrawingObjectSerializableout.Column = HColumn[maxIndex]*2 + relativeXnew;
            hDrawingObjectSerializableout.Phi = HAngle[maxIndex];
        }
        
        public void PositionOfTemplate(HImage HImage, string ModelPath, out double RowOut, out double ColOut, out double AngleOut)
        {
            var _himage = HImage.CopyImage();
            HImage modelimg=new HImage(ModelPath);
            HShapeModel model = new HShapeModel(modelimg.Rgb1ToGray().ZoomImageFactor(0.5, 0.5, "constant").MeanImage(13, 13), 8, -3.14, 3.14, "auto", "auto", "use_polarity", "auto", "auto");
            HTuple HRow, HColumn, HAngle, HScore;
            model.FindShapeModel(_himage.Rgb1ToGray().ZoomImageFactor(0.5, 0.5, "constant").MeanImage(13, 13), -3.14, 3.14, 0.3, 20, 0.5, "least_squares", 8, 0.9, out HRow, out HColumn, out HAngle, out HScore);
            var maxIndex = HScore.TupleFind(HScore.TupleMax());
            RowOut = HRow[maxIndex] *2;
            ColOut = HColumn[maxIndex] *2;
            AngleOut = HAngle[maxIndex] ;
        }


    }
}

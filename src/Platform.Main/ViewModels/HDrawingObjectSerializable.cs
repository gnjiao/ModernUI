using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Platform.Main.Annotations;

namespace Platform.Main.ViewModels
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class HDrawingObjectSerializable : INotifyPropertyChanged
    {
        public enum RegionTypeEnum
        {
            Unknown,
            TemplateRegion,
            RoiRegion
        }

        public enum RoiTypeEnum
        {
            Unknown,
            Spot,
            Defect
        }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public string Type { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Row { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Column { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Row1 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Column1 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Row2 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Column2 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Phi { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Length1 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Length2 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Radius { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Radius1 { get; set; }

        [Category(BlockPropertyCategories.Runtime)]
        [Browsable(true)]
        [ReadOnly(true)]
        public double Radius2 { get; set; }

        [Category(BlockPropertyCategories.Common)]
        [Browsable(true)]
        [ReadOnly(false)]
        public RegionTypeEnum RegionType { get; set; }

        [Category(BlockPropertyCategories.Common)]
        [Browsable(true)]
        [ReadOnly(false)]
        public string TemplateRegionPath { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public int OPCPoint { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double B_R_radio { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double G_R_radio { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double B_R_Tolerane { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double G_R_Tolerane { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double Threshold_R { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double Threshold_G { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public double Threshold_B { get; set; }

        [Category(BlockPropertyCategories.Parameter)]
        [Browsable(true)]
        [ReadOnly(false)]
        public string ModelPath { get; set; }

        [Category(BlockPropertyCategories.Common)]
        [Browsable(true)]
        [ReadOnly(false)]
        public RoiTypeEnum RoiType { get; set; }

        private HDrawingObject _drawingObject;

        public HDrawingObjectSerializable()
        {
            Type = "";
            RegionType = RegionTypeEnum.Unknown;
            RoiType = RoiTypeEnum.Unknown;
        }


        public double CenterRow { set; get; }
        public double CenterCol { set; get; }
        public double CenterAngle { set; get; }
        

        public HDrawingObjectSerializable(HDrawingObject hDrawingObject)
        {
            if (hDrawingObject == null)
                return;

            _drawingObject = hDrawingObject;

            Type = hDrawingObject.GetDrawingObjectParams("type");

            switch (Type)
            {
                case "rectangle1":
                    Row1 = hDrawingObject.GetDrawingObjectParams("row1");
                    Column1 = hDrawingObject.GetDrawingObjectParams("column1");
                    Row2 = hDrawingObject.GetDrawingObjectParams("row2");
                    Column2 = hDrawingObject.GetDrawingObjectParams("column2");
                    break;

                case "rectangle2":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Phi = hDrawingObject.GetDrawingObjectParams("phi");
                    Length1 = hDrawingObject.GetDrawingObjectParams("length1");
                    Length2 = hDrawingObject.GetDrawingObjectParams("length2");
                    break;

                case "circle":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Radius = hDrawingObject.GetDrawingObjectParams("radius");
                    break;

                case "ellipse":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Phi = hDrawingObject.GetDrawingObjectParams("phi");
                    Radius1 = hDrawingObject.GetDrawingObjectParams("radius1");
                    Radius2 = hDrawingObject.GetDrawingObjectParams("radius2");
                    break;
            }
        }

        public HDrawingObjectSerializable UpdateHDrawingObjectSerializable(HDrawingObject hDrawingObject)
        {
            if (hDrawingObject == null)
                return new HDrawingObjectSerializable();

            _drawingObject = hDrawingObject;

            Type = hDrawingObject.GetDrawingObjectParams("type");

            switch (Type)
            {
                case "rectangle1":
                    Row1 = hDrawingObject.GetDrawingObjectParams("row1");
                    Column1 = hDrawingObject.GetDrawingObjectParams("column1");
                    Row2 = hDrawingObject.GetDrawingObjectParams("row2");
                    Column2 = hDrawingObject.GetDrawingObjectParams("column2");
                    break;

                case "rectangle2":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Phi = hDrawingObject.GetDrawingObjectParams("phi");
                    Length1 = hDrawingObject.GetDrawingObjectParams("length1");
                    Length2 = hDrawingObject.GetDrawingObjectParams("length2");
                    break;

                case "circle":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Radius = hDrawingObject.GetDrawingObjectParams("radius");
                    break;

                case "ellipse":
                    Row = hDrawingObject.GetDrawingObjectParams("row");
                    Column = hDrawingObject.GetDrawingObjectParams("column");
                    Phi = hDrawingObject.GetDrawingObjectParams("phi");
                    Radius1 = hDrawingObject.GetDrawingObjectParams("radius1");
                    Radius2 = hDrawingObject.GetDrawingObjectParams("radius2");
                    break;
            }

            return this;
        }

        public HDrawingObject DrawingObjectImp()
        {
            return _drawingObject;
        }

        public HDrawingObject HDrawingObjectDeserialize()
        {
            HDrawingObject newDrawingObject = null;

            switch (Type)
            {
                case "rectangle1":
                    newDrawingObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.RECTANGLE1, Row1, Column1, Row2, Column2);
                    break;

                case "rectangle2":
                    newDrawingObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.RECTANGLE2, Row, Column, Phi, Length1, Length2);
                    break;

                case "ellipse":
                    newDrawingObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.ELLIPSE, Row, Column, Phi, Radius1, Radius2);
                    break;

                case "circle":
                    newDrawingObject = HDrawingObject.CreateDrawingObject(
                        HDrawingObject.HDrawingObjectType.CIRCLE, Row, Column, Radius);
                    break;

                default:
                    break;
            }

            _drawingObject = newDrawingObject;

            return newDrawingObject;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}

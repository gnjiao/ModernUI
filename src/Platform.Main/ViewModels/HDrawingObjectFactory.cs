using System;
using System.Collections.Generic;
using System.Linq;
using Core.Serialization;
using HalconDotNet;

namespace Platform.Main.ViewModels
{
    [Serializable]
    public class HDrawingObjectFactory : List<HDrawingObjectSerializable>
    {
        public void AddDrawingObject(HDrawingObject hDrawingObject)
        {
            Add(new HDrawingObjectSerializable(hDrawingObject));
        }

        public void AddDrawingObjectList(List<HDrawingObject> hDrawingObjects)
        {          
            AddRange(Array.ConvertAll(hDrawingObjects.ToArray(), p => new HDrawingObjectSerializable(p)));
        }

        public List<HDrawingObject> ConvertToDrawingObjectList()
        {
            return Array.ConvertAll(ToArray(), p => p.HDrawingObjectDeserialize()).ToList();
        }

        public static HDrawingObjectFactory LoadFormXaml(string xaml)
        {
            return xaml.DeserializeFromXamlFile<HDrawingObjectFactory>();
        }

        public void SaveToXaml(string xmal)
        {
            this.SerializeToXamlFile(xmal);
        }

    }
}

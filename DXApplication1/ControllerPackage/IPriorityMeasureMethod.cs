using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DXApplication1.DataModelPackage;

namespace DXApplication1.ControllerPackage
{
    [XmlInclude(typeof(FirstPriorityMeasureMethod))]
    public  abstract class IPriorityMeasureMethod
    {
       

        

        [XmlElement]
        public SerializableDictionary<int, double> PriorityMeasureDict { get; set; }

        public IPriorityMeasureMethod()
        {
            this.PriorityMeasureDict = new SerializableDictionary<int, double>();
        }
 

        public abstract  void setPriorityMeasureValue();
    }

    [XmlRoot]
    public class FirstPriorityMeasureMethod : IPriorityMeasureMethod
    {
        public FirstPriorityMeasureMethod()
        {
            setPriorityMeasureValue();
        }
       
        public override void setPriorityMeasureValue()
        {
            this.PriorityMeasureDict.Clear();
            this.PriorityMeasureDict.Add(1, 10);
            this.PriorityMeasureDict.Add(2, 8);
            this.PriorityMeasureDict.Add(3, 4);
            this.PriorityMeasureDict.Add(4, 1);
        }
      
    }

}

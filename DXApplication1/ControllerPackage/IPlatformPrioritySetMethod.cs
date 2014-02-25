using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXApplication1.DataModelPackage;
using System.Xml.Serialization;


namespace DXApplication1.ControllerPackage
{
    public enum PriorityType
    {
        ALL_SET_TO_ONE,
        USE_DATA_FROME_EXCEL,

    }

    [XmlInclude(typeof(EqualPlatformPrioritySetMethod))]
    [XmlInclude(typeof(ExactPlatformPrioritySetMethod))]
    public abstract  class IPlatformPrioritySetMethod
    {
       

        [XmlElement]
        public PriorityType Prioritytype { get; set; }
       


        public abstract void setPlatformPriority();
       
    }

    [XmlRoot]
    public class EqualPlatformPrioritySetMethod : IPlatformPrioritySetMethod
    {
        public EqualPlatformPrioritySetMethod()
        {
            setPlatformPriority();
        }

        public override void setPlatformPriority( )
        {
            this.Prioritytype = PriorityType.ALL_SET_TO_ONE;
        }
       
    }

    [XmlRoot]
    public class ExactPlatformPrioritySetMethod : IPlatformPrioritySetMethod
    {
        public ExactPlatformPrioritySetMethod()
        {
            setPlatformPriority();
        }

        public override void setPlatformPriority( )
        {
            this.Prioritytype = PriorityType.USE_DATA_FROME_EXCEL;
        }
       
      
    }
}

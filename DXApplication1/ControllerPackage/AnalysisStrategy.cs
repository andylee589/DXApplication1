using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DXApplication1.DataModelPackage;

namespace DXApplication1.ControllerPackage
{
    [XmlRoot("Analysis Strategy")]
  public  class AnalysisStrategy
    {

        [XmlIgnore]
         //Project property
         public Project project { get; set; }

        // INFO property
        [XmlElement]
        public String StrategyName { get; set; }
       

        [XmlElement]
        public String StrategyDescription { get; set; }
       

        // the round number of the test process;
        [XmlElement]
        public int TestRounds { get; set; }
       

        [XmlElement]
        // Platform Weight Set Property
        public SerializableGenericObject<IPlatformWeightSetMethod> PlatformWeightSetMethod { get; set; }
        [XmlElement]
        public SerializableDictionary <string,IPlatformWeightSetMethod> PlatformWeightSetMethodDict { get; set; }

        private ManualAssignmentPlatformWeightSetMethod manualWeightSetMethod;

        private AutoAssignmentPlatformWeightSetMethod autoWeightSetMethod;

        //Platform Assign Method Property
        [XmlElement]
        public SerializableGenericObject<IPlatformAssignMethod> PlatformAssignMethod { get; set; }

        public SerializableDictionary<string, IPlatformAssignMethod> PlatformAssignMethodDict { get; set; }

        private RandomPlatformAssignMethod randomAssignMethod;
        private BalancedPlatformAssignMethod balancedAssignMethod;
       


        public AnalysisStrategy()
        {

            //Platform Weight filed
            this.PlatformWeightSetMethod =new SerializableGenericObject<IPlatformWeightSetMethod>();

            // the candidate platform weight set method
            this.manualWeightSetMethod = new ManualAssignmentPlatformWeightSetMethod();
            this.autoWeightSetMethod= new AutoAssignmentPlatformWeightSetMethod();
            // the add order is the same as the radio group present index order
            this.PlatformWeightSetMethodDict = new SerializableDictionary<string, IPlatformWeightSetMethod>();
            this.PlatformWeightSetMethodDict.Add(typeof(ManualAssignmentPlatformWeightSetMethod).FullName, this.manualWeightSetMethod);
            this.PlatformWeightSetMethodDict.Add(typeof(AutoAssignmentPlatformWeightSetMethod).FullName ,this.autoWeightSetMethod);

            //Platform Assign Method filed
            this.PlatformAssignMethod=new SerializableGenericObject<IPlatformAssignMethod>();
           // two candidate platform assign method
            this.randomAssignMethod = new RandomPlatformAssignMethod();
            this.balancedAssignMethod = new BalancedPlatformAssignMethod();
            // the add order is the same as the radio group present index order
            this.PlatformAssignMethodDict = new SerializableDictionary<string, IPlatformAssignMethod>();
            this.PlatformAssignMethodDict.Add(typeof(RandomPlatformAssignMethod).FullName,this.randomAssignMethod);
            this.PlatformAssignMethodDict.Add(typeof(BalancedPlatformAssignMethod).FullName ,this.balancedAssignMethod);
            InitDefaultValue();
        }
       
     

        private void InitDefaultValue(){


            //index 1 means auto assignment;
            this.PlatformWeightSetMethod.Value =this.PlatformWeightSetMethodDict[typeof(AutoAssignmentPlatformWeightSetMethod).FullName];

            this.PlatformAssignMethod.Value = this.PlatformAssignMethodDict[typeof(BalancedPlatformAssignMethod).FullName];
        }


        public void updateData(Project project)
        {
            this.project = project;
            // update the latest variable relation from the xml
            foreach (KeyValuePair<string, IPlatformWeightSetMethod> kv in this.PlatformWeightSetMethodDict)
            {
                kv.Value.updateDataFromVariableRelation(this);
                kv.Value.updateOtherData(this);
            }
        }

       
        public void initStrategyInfo(string name,string desc,int rounds)
        {
            this.StrategyName = name;
            this.StrategyDescription = desc;
            this.TestRounds = rounds;
        }


        public List<Dictionary<string, Dictionary<string, string>>> startAnalysis()
        {
            this.PlatformWeightSetMethod.Value.setPlatformWeight(this.TestRounds);
            return this.PlatformAssignMethod.Value.assignPlatformMethod(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DXApplication1.DataModelPackage;

namespace DXApplication1.ControllerPackage
{
    [XmlRoot]
    public class AutoAssignmentPlatformWeightSetMethod : IPlatformWeightSetMethod
    {

      

        [XmlElement]
        // the way priority measured 
        public SerializableGenericObject<IPriorityMeasureMethod> PriorityMeasureMethod { get; set; }
        [XmlElement]
        public SerializableDictionary<string, IPriorityMeasureMethod> priorityMeasureMethodDict { get; set; }
        // candidate method 
        private FirstPriorityMeasureMethod firstMeasureMethod { get; set; }



        [XmlElement]
        // the way languagePriority set
        public SerializableGenericObject<IFactorPrioritySetMethod> FactorPrioritySetMethod { get; set; }
        [XmlElement]
        public SerializableDictionary<string, IFactorPrioritySetMethod> FactorPrioritySetMethodDict { get; set; }
        // candidate language priority set method
        private EqualFactorPrioritySetMethod equalLanguagePrioritySetMethod;
        private CustomFactorPrioritySetMethod customLanguagePrioritySetMethod;

        [XmlElement]
        public SerializableGenericObject<IPlatformPrioritySetMethod> PlatformPrioritySetMethod { get; set; }
        [XmlElement]
        public SerializableDictionary <string,IPlatformPrioritySetMethod> PlatformPrioritySetMethodDict { get; set; }
        private EqualPlatformPrioritySetMethod equalPlatformPrioritySetMethod;
        private ExactPlatformPrioritySetMethod exactPlatformPrioritySetMethod { get; set; }

        public AutoAssignmentPlatformWeightSetMethod()
            : base()
        {
            this.FactorPrioritySetMethod = new SerializableGenericObject<IFactorPrioritySetMethod>();
            this.equalLanguagePrioritySetMethod = new EqualFactorPrioritySetMethod();
            this.customLanguagePrioritySetMethod = new CustomFactorPrioritySetMethod();
            // the add order is the same as the radio group present index order
            this.FactorPrioritySetMethodDict = new SerializableDictionary<string, IFactorPrioritySetMethod>();
            this.FactorPrioritySetMethodDict.Add(typeof(EqualFactorPrioritySetMethod).FullName, equalLanguagePrioritySetMethod);
            this.FactorPrioritySetMethodDict.Add(typeof(CustomFactorPrioritySetMethod).FullName ,customLanguagePrioritySetMethod);


            this.PlatformPrioritySetMethod = new SerializableGenericObject<IPlatformPrioritySetMethod>();
            this.equalPlatformPrioritySetMethod = new EqualPlatformPrioritySetMethod();
            this.exactPlatformPrioritySetMethod = new ExactPlatformPrioritySetMethod();
            // the add order is the same as the radio group present index order
            this.PlatformPrioritySetMethodDict = new SerializableDictionary<string, IPlatformPrioritySetMethod>();
            this.PlatformPrioritySetMethodDict.Add(typeof(EqualPlatformPrioritySetMethod).FullName ,this.equalPlatformPrioritySetMethod);
            this.PlatformPrioritySetMethodDict.Add(typeof(ExactPlatformPrioritySetMethod).FullName ,this.exactPlatformPrioritySetMethod);

            this.PriorityMeasureMethod = new SerializableGenericObject<IPriorityMeasureMethod>();
            this.firstMeasureMethod = new FirstPriorityMeasureMethod();
            //add order is the same as the radio group present index order
            this.priorityMeasureMethodDict = new SerializableDictionary<string, IPriorityMeasureMethod>();
            this.priorityMeasureMethodDict.Add(typeof(FirstPriorityMeasureMethod).FullName, this.firstMeasureMethod);

            this.InitDefaultValue();
        }


        public override void InitDefaultValue()
        {
            this.FactorPrioritySetMethod.Value = this.FactorPrioritySetMethodDict[typeof(EqualFactorPrioritySetMethod).FullName];
            this.PlatformPrioritySetMethod.Value = this.PlatformPrioritySetMethodDict[typeof(ExactPlatformPrioritySetMethod).FullName];
            this.PriorityMeasureMethod.Value = this.priorityMeasureMethodDict[typeof(FirstPriorityMeasureMethod).FullName];


            //initialize primary Variable Weight dictionary
            Controller controller = Controller.getInstance();
            VariableRelation variableRelationSetting = Controller.getInstance().XMLDeserializeVariableRelation();
            List<string> primaryVarialbeStrList = variableRelationSetting.PrimaryVariableList;
            if (controller.project != null)
            {
                foreach (string primaryVariableStr in primaryVarialbeStrList)
                {
                    Variable primaryVariable = controller.project.Variables[primaryVariableStr];
                    List<string> primaryItemStrList = primaryVariable.PlatformList;
                    SerializableDictionary<string, double> weightDict = new SerializableDictionary<string, double>();
                   
                    for (int i = 0; i < primaryItemStrList.Count; i++)
                    {
                        weightDict.Add(primaryItemStrList[i], 0);
                    }

                    this.PrimaryVariableWeightDict.Add(primaryVariableStr, weightDict);

                }
            }


            // we only init the auto secondary item weight set method
            this.SecondaryItemWeightSetMethod.Value = this.SecondaryItemWeightSetMethodDict[typeof(AutoSecondaryItemWeightSetMethod).FullName];
        }




        public override void setPlatformWeight(int testRouds)
        {
            this.SecondaryItemWeightSetMethod.Value = this.SecondaryItemWeightSetMethodDict[typeof(AutoSecondaryItemWeightSetMethod).FullName];
            // init the total test time
            Controller controller = Controller.getInstance();
            Project project = controller.project;
            List<string> languageList = project.Variables[project.MultiFactorVariableNameList[0]].FactorList;
            int languageNum = languageList.Count;
            int totalTimes = languageNum * testRouds;
                  
            // primary variable weight dictionary  set value

             foreach (KeyValuePair<string ,SerializableDictionary<string,double>> kv1 in this.PrimaryVariableWeightDict)
            {
                SerializableDictionary<string, double> itemWeightDict = kv1.Value;
                SerializableDictionary<string, double> doubleItemTestTimesDict = new SerializableDictionary<string, double>();


                List<string> kv2KeyList = new List<string>(itemWeightDict.Keys);
                 // firstly set weight double value  to language measure value multiply by platform measure value;
                double totalWeight = 0;
                for (int i = 0; i < itemWeightDict.Count;i++ )
                {
                    itemWeightDict[kv2KeyList[i]] = computeAutoAssignWeight(kv1.Key, kv2KeyList[i]);
                    totalWeight += itemWeightDict[kv2KeyList[i]];                  
                }
                 // secondary set weight double value to percentage weight;
                double totalPercentageWeight = 0;

                int index = 0;
                for (; index < itemWeightDict.Count - 1; index++)
                {
                    double tempWeight = itemWeightDict[kv2KeyList[index]];
                    string percentageStr=String.Format("{0:F}",tempWeight/totalWeight);
                    itemWeightDict[kv2KeyList[index]] = double.Parse(percentageStr);
                    totalPercentageWeight += itemWeightDict[kv2KeyList[index]];
                    doubleItemTestTimesDict.Add(kv2KeyList[index], itemWeightDict[kv2KeyList[index]] * totalTimes);
                    // set do
                }
                 // set the last item value to the rest 
                itemWeightDict[kv2KeyList[index]] = 1 - totalPercentageWeight;
                doubleItemTestTimesDict.Add(kv2KeyList[index], itemWeightDict[kv2KeyList[index]] * totalTimes);
             
                SerializableDictionary<string, int> itemTestTimesDict = this.getItemTestTimes(doubleItemTestTimesDict, totalTimes);

                if (this.PrimaryVariableTestTimesDict.ContainsKey(kv1.Key))
                {
                    this.PrimaryVariableTestTimesDict[kv1.Key] = itemTestTimesDict;
                  
                }
                else
                {                   
                   this.PrimaryVariableTestTimesDict.Add(kv1.Key, itemTestTimesDict);
                }
            }


            // secondary variable weight dictionary set value

             foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv1 in this.SecondaryItemWeightSetMethod.Value.DependenceRecordDict)
             {
                 SerializableDictionary<string, SerializableDictionary<string, int>> relationDict = new SerializableDictionary<string, SerializableDictionary<string, int>>();
                 VariableDependenceRecord record = kv1.Value;

                 foreach (KeyValuePair<string, SerializableDictionary<string, SecondaryItem>> kv2 in record.ItemDependencDict)
                 {

                     SerializableDictionary<string, double> doubleItemTestTimesDict = new SerializableDictionary<string, double>();
                     SerializableDictionary<string, SecondaryItem> itemDict = kv2.Value;
                     int itemTimes =this.PrimaryVariableTestTimesDict[kv1.Key.PrimaryVariableKey][kv2.Key];
                     List<string> kv3KeyList = new List<string>(itemDict.Keys);

                     // firstly set weight double value  to language measure value multiply by platform measure value;
                     double totalWeight = 0;
                     for (int i = 0; i < itemDict.Count; i++)
                     {
                         itemDict[kv3KeyList[i]].Weight = computeAutoAssignWeight(kv1.Key.SecondaryVariableKey, kv3KeyList[i]);
                         totalWeight += itemDict[kv3KeyList[i]].Weight;
                     }
                     // secondary set weight double value to percentage weight;
                     double totalPercentageWeight = 0;

                     int index = 0;
                     for (; index < itemDict.Count - 1; index++)
                     {
                         double tempWeight = itemDict[kv3KeyList[index]].Weight;
                         string percentageStr = String.Format("{0:F}", tempWeight / totalWeight);
                         itemDict[kv3KeyList[index]].Weight = double.Parse(percentageStr);
                         totalPercentageWeight += itemDict[kv3KeyList[index]].Weight;
                         doubleItemTestTimesDict.Add(kv3KeyList[index], itemDict[kv3KeyList[index]].Weight * itemTimes);
                         // set do
                     }

                     // set the last item value to the rest 
                     itemDict[kv3KeyList[index]].Weight = 1 - totalPercentageWeight;
                     doubleItemTestTimesDict.Add(kv3KeyList[index], itemDict[kv3KeyList[index]].Weight *itemTimes );
                     SerializableDictionary<string, int> itemTestTimesDict = this.getItemTestTimes(doubleItemTestTimesDict, itemTimes);                 
                     relationDict.Add(kv2.Key, itemTestTimesDict);

                 }

                 if (this.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict.ContainsKey(kv1.Key))
                 {
                     this.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict[kv1.Key] = relationDict;
                 }
                 else
                 {
                     this.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict.Add(kv1.Key, relationDict);
                 }
             }                        
        }

        public override void updateOtherData(AnalysisStrategy strategy)
        {
            updateFactorPriorityData();
        }

        public void updateFactorPriorityData()
        {
            this.FactorPrioritySetMethod.Value.updateFactorList();
            foreach (KeyValuePair<string ,IFactorPrioritySetMethod>  kv in this.FactorPrioritySetMethodDict)
            {
                kv.Value.updateFactorList();
            }
        }

        // 
        private double computeAutoAssignWeight(string variableStr, string itemStr)
        {
            Controller controller = Controller.getInstance();
            Project project = controller.project;
            Variable variable = project.Variables[variableStr];
            Dictionary<string, Dictionary<string, Priority>> platformPriorityMatrix = variable.PlatformPriorityMatrix;
            double sum = 0;
            foreach (KeyValuePair<string, Dictionary<string, Priority>> kv in platformPriorityMatrix)
            {
                int platformPriority = 1;
                switch (this.PlatformPrioritySetMethod.Value.Prioritytype)
                {
                    case PriorityType.USE_DATA_FROME_EXCEL:
                        platformPriority = kv.Value[itemStr].Value;
                        break;
                    case PriorityType.ALL_SET_TO_ONE:
                        platformPriority = 1;
                        break;

                }
                double platformMeasure = this.PriorityMeasureMethod.Value.PriorityMeasureDict[platformPriority];

                if (project.SingleFactorVariableNameList.Contains(variableStr))
                {
                    return sum = platformMeasure;
                }
                
                int factorPriority = this.FactorPrioritySetMethod.Value.FactorPriorityDict[kv.Key];
                double factorMeasure = this.PriorityMeasureMethod.Value.PriorityMeasureDict[factorPriority];
                sum += platformMeasure * factorMeasure;
            }           
            return sum;
        }
           
         
    }
}

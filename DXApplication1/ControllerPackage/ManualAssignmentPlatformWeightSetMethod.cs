using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DevExpress.XtraEditors;
using DXApplication1.DataModelPackage;
using DXApplication1.ViewPackage;
namespace DXApplication1.ControllerPackage
{
    [XmlRoot]
    public class ManualAssignmentPlatformWeightSetMethod : IPlatformWeightSetMethod
    {


        public ManualAssignmentPlatformWeightSetMethod( )
            : base()
        {
            InitDefaultValue();
        }
     

        public override void InitDefaultValue()
        {
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
                    double[] weightArray = ManualAssignmentPlatformWeightSetMethod.getAverageWeight(primaryItemStrList.Count);

                    for (int i = 0; i < primaryItemStrList.Count; i++)
                    {
                        weightDict.Add(primaryItemStrList[i], weightArray[i]);
                    }

                    this.PrimaryVariableWeightDict.Add(primaryVariableStr, weightDict);

                }
            }
           
            //initialize secondaryItemWeightSetMethod          

            this.SecondaryItemWeightSetMethod.Value = this.SecondaryItemWeightSetMethodDict[typeof(EqualSecondaryItemWeightSetMethod).FullName];
        }
  


        public static double[] getAverageWeight(int number)
        {
            if(number >200)
            {
                // avoid average item weight less than 0.005
                DevExpress.XtraEditors.XtraMessageBox.Show ("error input  for platform items "+ number);
               
                return  null;
            }
            double[] array = new double[number];
            double item = double.Parse((1.0 / number - 0.005).ToString("0.00"));
            double lastItem = (1 - item * (number - 1));

            for (int i = 0; i < number; i++)
            {
                array[i] = item;
            }
            array[number - 1] = lastItem;
             return array;
        }

        public override void setPlatformWeight(int testRouds)
        {
            Controller  controller = Controller.getInstance();
            Project project = controller.project;
            List<string> languageList = project.Variables[project.MultiFactorVariableNameList[0]].FactorList;
            int languageNum = languageList.Count;
            int totalTimes = languageNum * testRouds;
            
            // count primary variable test times 
            foreach (KeyValuePair<string ,SerializableDictionary<string,double>> kv1 in this.PrimaryVariableWeightDict)
            {
                SerializableDictionary<string, double> itemWeightDict = kv1.Value;
                SerializableDictionary<string, double> doubleItemTestTimesDict = new SerializableDictionary<string, double>();
                
                foreach (KeyValuePair<string, double> kv2 in itemWeightDict)
                {
                    doubleItemTestTimesDict.Add(kv2.Key, kv2.Value * totalTimes);
                }
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
            
            // count secondary variable test times
           // SerializableDictionary<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>>
            foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv1 in this.SecondaryItemWeightSetMethod.Value.DependenceRecordDict)
            {
                SerializableDictionary<string, SerializableDictionary<string, int>> relationDict = new SerializableDictionary<string, SerializableDictionary<string, int>>();
                VariableDependenceRecord record = kv1.Value;

                foreach (KeyValuePair<string, SerializableDictionary<string, SecondaryItem>> kv2 in record.ItemDependencDict)
                {
                    int itemTimes =this.PrimaryVariableTestTimesDict[kv1.Key.PrimaryVariableKey][kv2.Key];
                    SerializableDictionary<string, double> doubleItemTestTimesDict = new SerializableDictionary<string, double>();
                    SerializableDictionary<string, SecondaryItem> itemDict = kv2.Value;

                    foreach (KeyValuePair<string,SecondaryItem> kv3 in itemDict)
                    {
                        doubleItemTestTimesDict.Add(kv3.Key, kv3.Value.Weight * itemTimes);
                    }
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
    }
}

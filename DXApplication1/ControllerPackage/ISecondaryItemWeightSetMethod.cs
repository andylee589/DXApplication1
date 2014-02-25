using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXApplication1.DataModelPackage;
using System.Xml.Serialization;

namespace DXApplication1.ControllerPackage
{
    [XmlInclude(typeof(EqualSecondaryItemWeightSetMethod))]
    [XmlInclude(typeof(ManualSecoandaryItemWeightSetMethod))]
    [XmlInclude(typeof(AutoSecondaryItemWeightSetMethod))]
    public abstract class ISecondaryItemWeightSetMethod
    {
       
        [XmlElement]
        public SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> DependenceRecordDict { get; set; }
        [XmlIgnore]
        public SerializableDictionary<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>> DependenceRecordTestTimesDict { get; set; }

        public ISecondaryItemWeightSetMethod( )
        {
             this.DependenceRecordDict = new SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord>();
             this.DependenceRecordTestTimesDict = new SerializableDictionary<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>>();
             InitDefaultValue();
        }



        // init set equal weight
        public virtual void InitDefaultValue()
        {
            Controller controller = Controller.getInstance();
            VariableRelation variableRelationSetting = Controller.getInstance().XMLDeserializeVariableRelation();
            this.DependenceRecordDict = variableRelationSetting.DependenceRecordDict;
            foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv in this.DependenceRecordDict)
            {
                VariableDependenceRecord record = kv.Value;
                SerializableDictionary<string, SerializableDictionary<string, SecondaryItem>> itemDependecDict = record.ItemDependencDict;
                foreach (KeyValuePair<string, SerializableDictionary<string, SecondaryItem>> kv2 in itemDependecDict)
                {
                    SerializableDictionary<string, SecondaryItem> secondaryItemDict = kv2.Value;
                    int number = secondaryItemDict.Count;
                    double[] itemArray = ManualAssignmentPlatformWeightSetMethod.getAverageWeight(number);

                    foreach (KeyValuePair<string, SecondaryItem> kv3 in secondaryItemDict)
                    {
                        kv3.Value.Weight = itemArray[0];
                    }

                    string lastItemStr = new List<string>(secondaryItemDict.Keys)[secondaryItemDict.Count - 1];
                    secondaryItemDict[lastItemStr].Weight = itemArray[number - 1];

                }
            }       
        
        }

        public virtual void setSecondaryItemWeight( ) { }

        public virtual void updateSecondaryItemData() 
        {
            Controller controller = Controller.getInstance();
            VariableRelation variableRelationSetting = Controller.getInstance().XMLDeserializeVariableRelation();
            SerializableDictionary<VariableDependenceRecordKey,VariableDependenceRecord> newDependenceRecordDict = variableRelationSetting.DependenceRecordDict;

            // compare with the latest data from xml add the lack records and remove the redundant records.
            // add lack records
            foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv in newDependenceRecordDict)
            {
                if (!this.DependenceRecordDict.ContainsKey(kv.Key))
                {

                    VariableDependenceRecord record = kv.Value;

                    SerializableDictionary<string, SerializableDictionary<string, SecondaryItem>> itemDependecDict = record.ItemDependencDict;
                    foreach (KeyValuePair<string, SerializableDictionary<string, SecondaryItem>> kv2 in itemDependecDict)
                    {


                        SerializableDictionary<string, SecondaryItem> secondaryItemDict = kv2.Value;
                        int number = secondaryItemDict.Count;
                        double[] itemArray = ManualAssignmentPlatformWeightSetMethod.getAverageWeight(number);

                        foreach (KeyValuePair<string, SecondaryItem> kv3 in secondaryItemDict)
                        {
                            kv3.Value.Weight = itemArray[0];
                        }

                        string lastItemStr = new List<string>(secondaryItemDict.Keys)[secondaryItemDict.Count - 1];
                        secondaryItemDict[lastItemStr].Weight = itemArray[number - 1];

                    }
                    // add the new record
                    this.DependenceRecordDict.Add(kv.Key, kv.Value);
                }            
            }    
            
            // remove the redundant records
            List<VariableDependenceRecordKey> keyList = new List<VariableDependenceRecordKey>(this.DependenceRecordDict.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                if (!newDependenceRecordDict.ContainsKey(keyList[i]))
                {
                    this.DependenceRecordDict.Remove(keyList[i]);
                }
            }

           

        }
    }

    [XmlRoot]
   public class EqualSecondaryItemWeightSetMethod : ISecondaryItemWeightSetMethod
   {
        public EqualSecondaryItemWeightSetMethod( )
            : base()
        {

        }

      
   }

    [XmlRoot]
    public class ManualSecoandaryItemWeightSetMethod : ISecondaryItemWeightSetMethod
    {
        public ManualSecoandaryItemWeightSetMethod( )
            : base()
        {

        }
      

        public override void setSecondaryItemWeight( )
        {
            throw new NotImplementedException();
        }
    }

    [XmlRoot]
    public class AutoSecondaryItemWeightSetMethod : ISecondaryItemWeightSetMethod
    {
        public AutoSecondaryItemWeightSetMethod()
            : base()
        {
        }

        public override void InitDefaultValue()
        {
            Controller controller = Controller.getInstance();
            VariableRelation variableRelationSetting = Controller.getInstance().XMLDeserializeVariableRelation();
            this.DependenceRecordDict = variableRelationSetting.DependenceRecordDict;
            foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv in this.DependenceRecordDict)
            {
                VariableDependenceRecord record = kv.Value;
                SerializableDictionary<string, SerializableDictionary<string, SecondaryItem>> itemDependecDict = record.ItemDependencDict;
                foreach (KeyValuePair<string, SerializableDictionary<string, SecondaryItem>> kv2 in itemDependecDict)
                {
                    SerializableDictionary<string, SecondaryItem> secondaryItemDict = kv2.Value;                                      
                    foreach (KeyValuePair<string, SecondaryItem> kv3 in secondaryItemDict)
                    {
                        kv3.Value.Weight = 0;
                    }   
                }
            }
        }

        public override void setSecondaryItemWeight( )
        {
            throw new NotImplementedException();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace DXApplication1.DataModelPackage
{
 [XmlRoot("variable relation")]
 public   class VariableRelation
    {
       
        private List<string> primaryVariableList = new List<string>();
        
        private List<string> secondaryVariableList = new List<string>();
      
        private SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> dependenceRecordDict = new SerializableDictionary<VariableDependenceRecordKey,VariableDependenceRecord>();

        [XmlArray]
        public List<string> PrimaryVariableList
        {
            get
            {
                return primaryVariableList;
            }
            set
            {
                primaryVariableList = value;
            }
        }

        [XmlArray]
        public List<string> SecondaryVariableList
        {
            get
            {
                return secondaryVariableList;
            }

            set
            {
                secondaryVariableList = value;
            }
        }

         [XmlElement]
        public SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> DependenceRecordDict
        {
            get
            {
                return dependenceRecordDict;
            }

            set
            {
                dependenceRecordDict = value;
            }
        }

         public bool containRecord(string primaryVariableStr, string secondaryVariableStr)
         {
             //foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv in this.dependenceRecordDict)
             //{
             //    if (kv.Key.Equals(new VariableDependenceRecordKey(primaryVariableStr, secondaryVariableStr)))
             //    {
             //        return true;
             //    }
             //}
             //return false;

             return this.dependenceRecordDict.ContainsKey(new VariableDependenceRecordKey(primaryVariableStr, secondaryVariableStr));
         }

         public void removeRecordContainsSecondaryKey(string secondaryVariableStr)
         {
             List <VariableDependenceRecordKey>  keyList = new List<VariableDependenceRecordKey>(this.dependenceRecordDict.Keys);

             for(int i=0 ;i<keyList.Count;i++)
             {
                   if(keyList[i].SecondaryVariableKey.Equals(secondaryVariableStr))
                 {
                      this.dependenceRecordDict.Remove(keyList[i]);
                 }
             }
         }

         public void removeRecordContainsPrimaryKey(string primaryVariableStr)
         {
             List<VariableDependenceRecordKey> keyList = new List<VariableDependenceRecordKey>(this.dependenceRecordDict.Keys);

             for (int i = 0; i < keyList.Count; i++)
             {
                 if (keyList[i].PrimaryVariableKey.Equals(primaryVariableStr))
                 {
                     this.dependenceRecordDict.Remove(keyList[i]);
                 }
             }
         }

         public VariableDependenceRecord getRecord(string primaryVariableStr, string secondaryVariableStr)
         {
             //VariableDependenceRecord record = null;
             //foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv in this.dependenceRecordDict)
             //{
             //    if (kv.Key.Equals(new VariableDependenceRecordKey(primaryVariableStr, secondaryVariableStr)))
             //    {
             //        record = kv.Value;
             //    }
             //}
             //return record;

             return this.dependenceRecordDict[new VariableDependenceRecordKey(primaryVariableStr, secondaryVariableStr)];
         }

         

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXApplication1.DataModelPackage;
using System.Xml.Serialization;

namespace DXApplication1.ControllerPackage
{
    [XmlInclude(typeof(ManualAssignmentPlatformWeightSetMethod))]
    [XmlInclude(typeof(AutoAssignmentPlatformWeightSetMethod))]
   public  abstract class IPlatformWeightSetMethod
    {



        [XmlElement]
        public SerializableDictionary<string, SerializableDictionary<string, double>> PrimaryVariableWeightDict { get; set; }
        [XmlIgnore]
        public SerializableDictionary<string, SerializableDictionary<string, int>> PrimaryVariableTestTimesDict { get; set; }

       [XmlElement]
       public SerializableGenericObject<ISecondaryItemWeightSetMethod> SecondaryItemWeightSetMethod { get; set; }
       [XmlElement]
       public SerializableDictionary<string,ISecondaryItemWeightSetMethod> SecondaryItemWeightSetMethodDict { get; set; }
       private EqualSecondaryItemWeightSetMethod equalSecondItemWeightSetMethod;

       private ManualSecoandaryItemWeightSetMethod manualSecondItemWeightSetMethod;
       private AutoSecondaryItemWeightSetMethod autoSecondItemWeightSetMethod;

       public IPlatformWeightSetMethod()
       {
           this.PrimaryVariableWeightDict  = new SerializableDictionary<string, SerializableDictionary<string, double>>();
           this.PrimaryVariableTestTimesDict = new SerializableDictionary<string, SerializableDictionary<string, int>>();
           this.SecondaryItemWeightSetMethod = new SerializableGenericObject<ISecondaryItemWeightSetMethod>();
           // two candidate secondary item weight set method
           this.equalSecondItemWeightSetMethod = new EqualSecondaryItemWeightSetMethod();
           this.manualSecondItemWeightSetMethod = new ManualSecoandaryItemWeightSetMethod();
           this.autoSecondItemWeightSetMethod = new AutoSecondaryItemWeightSetMethod();
           // the add order is the same as the radio group present index order
           this.SecondaryItemWeightSetMethodDict = new SerializableDictionary<string, ISecondaryItemWeightSetMethod>();
           this.SecondaryItemWeightSetMethodDict.Add(typeof(EqualSecondaryItemWeightSetMethod).FullName,this.equalSecondItemWeightSetMethod);
           this.SecondaryItemWeightSetMethodDict.Add(typeof(ManualSecoandaryItemWeightSetMethod).FullName,this.manualSecondItemWeightSetMethod);
           this.SecondaryItemWeightSetMethodDict.Add(typeof(AutoSecondaryItemWeightSetMethod).FullName,this.autoSecondItemWeightSetMethod);
       }

       public virtual void updateDataFromVariableRelation(AnalysisStrategy strategy) 
       {
           Controller controller = Controller.getInstance();
           VariableRelation variableRelationSetting = Controller.getInstance().XMLDeserializeVariableRelation();
           strategy.project.VariableRelationSetting = variableRelationSetting;

           List<string> primaryVarialbeStrList = variableRelationSetting.PrimaryVariableList;

           // update the primary variable 
           //compare the read data with the older one

           // add new  primary variable 

           foreach (string primaryVariableStr in primaryVarialbeStrList)
           {
               if (!this.PrimaryVariableWeightDict.ContainsKey(primaryVariableStr))
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

           // delete the redundant primary variable
           List<string> keyList = new List<string>(this.PrimaryVariableWeightDict.Keys);
           for (int i = 0; i < keyList.Count; i++)
           {
               if (!primaryVarialbeStrList.Contains(keyList[i]))
               {
                   this.PrimaryVariableWeightDict.Remove(keyList[i]);
               }
           }


           // update the secondary dependence data
           foreach (KeyValuePair<string, ISecondaryItemWeightSetMethod> kv in this.SecondaryItemWeightSetMethodDict)
           {
               kv.Value.updateSecondaryItemData();
           }

       }

       public virtual void updateOtherData(AnalysisStrategy strategy)
       {

       }
     

       public abstract void InitDefaultValue();
       public abstract void setPlatformWeight(int testRouds);

       public SerializableDictionary<string, int> getItemTestTimes(Dictionary<string, double> doubleTestTimesDict, int totalTimes)
       {
           SerializableDictionary<string, int> itemTimesDict = new SerializableDictionary<string, int>();
           // the fill flag mean is the double value such as 1.6 should be fill to 1 but we set it to 1 and mark shall fill flag to 1 and if the value such as 1.4 ,we set it to 1 and mark fill flag to 0
           Dictionary<string, bool> itemFillFlagDict = new Dictionary<string, bool>();
           
           int sum=0;
           foreach(KeyValuePair<string,double> kv in doubleTestTimesDict)
           {
               int floorValue = (int) Math.Floor(kv.Value);
               sum+=floorValue;
               itemTimesDict.Add(kv.Key,floorValue);
               itemFillFlagDict.Add(kv.Key, (kv.Value - floorValue) >= 0.5);
           }

           int leftTime = totalTimes - sum;
           if (leftTime > 0)
           {
               // first find current value equal 0 and flag equal 1 ,if not enough, just random assign them ,else all assign 1.
               //then find the current value equal 0 and origin value larger than 0 ,if not enough random assign 1,if enough all assign 1, 
               //the last,if have rest value ,random assign for all.
               List<string> valueZeroFlagOneList = new List<string>();
               List<string> valueZeroFlagZeroList =new List<string> ();
               List<string> valueNotZeroFlagOneList = new List<string>();
               List<string> valueNotZeroFlagZeroList = new List<string>();
               foreach (KeyValuePair<string, int> kv in itemTimesDict)
               {
                   if (kv.Value == 0)
                   {
                       if (itemFillFlagDict[kv.Key])
                       {
                           valueZeroFlagOneList.Add(kv.Key);
                       }
                       else if(doubleTestTimesDict[kv.Key]>0)
                       {
                           valueZeroFlagZeroList.Add(kv.Key);
                       }
                   }

                   else
                   {
                       if (itemFillFlagDict[kv.Key])
                       {
                           valueNotZeroFlagOneList.Add(kv.Key);
                       }
                       else
                       {
                           valueNotZeroFlagZeroList.Add(kv.Key);
                       }
                       
                   }
               }

               int valueZeroFlagOneNum = valueZeroFlagOneList.Count;
               int valueZeroFlagZeroNum = valueZeroFlagZeroList.Count;
               int valueNotZeroFlagOneNum=valueNotZeroFlagOneList.Count;
               int valueNotZeroFlagZeroNum = valueNotZeroFlagZeroList.Count;
               if(leftTime<valueZeroFlagOneNum)
               {
                   //not enough for value=0 ,flag=1
                   List<int> selectFlagOneList = generateNumber(valueZeroFlagOneNum, leftTime);
                   foreach (int index in selectFlagOneList)
                   {
                       leftTime--;
                       itemTimesDict[valueZeroFlagOneList[index]]++;
                   }
               }
               else
               {
                   // enought for value=0 ,flag=1;

                   foreach (string str in valueZeroFlagOneList)
                   {
                       leftTime--;
                       itemTimesDict[str]++;
                   }

                   // not enough for value=0 ,flag =0
                   if (leftTime < valueZeroFlagZeroNum)
                   {
                       List<int> selectFlagZeroList = generateNumber(valueZeroFlagZeroNum, leftTime);
                       foreach (int index in selectFlagZeroList)
                       {
                           leftTime--;
                           itemTimesDict[valueZeroFlagZeroList[index]]++;
                       }

                   }
                   else
                   {
                       // enough for value =0 ,flag =0

                       foreach (string str in valueZeroFlagZeroList)
                       {
                           leftTime--;
                           itemTimesDict[str]++;
                       }
                       // then check the rest if 
                       if (leftTime<valueNotZeroFlagOneNum)
                       {

                           List<int> selectValueNotZeroFlagOneList = generateNumber(valueNotZeroFlagOneNum, leftTime);
                           foreach (int index in selectValueNotZeroFlagOneList)
                           {
                               leftTime--;
                               itemTimesDict[valueNotZeroFlagOneList[index]]++;
                           }
                       }
                       else
                       {
                           foreach (string str in valueNotZeroFlagOneList)
                           {
                               leftTime--;
                               itemTimesDict[str]++;
                           }

                           if (leftTime > 0)
                           {
                               List<int> selectValueNotZeroFlagZeroList = generateNumber(valueNotZeroFlagZeroNum, leftTime);
                               foreach (int index in selectValueNotZeroFlagZeroList)
                               {
                                   leftTime--;
                                   itemTimesDict[valueNotZeroFlagZeroList[index]]++;
                               }
                           }
                       }
                   }
               }


           }



           return itemTimesDict;
       }




       public static  List<int> generateNumber(int totalCount, int randomCount)
       {
           // store all index 
           List<int> container = new List<int>(totalCount);
           //store randomly selected index
           List<int> result = new List<int>(randomCount);
           Random random = new Random();
           for (int i = 0; i < totalCount;i++ )
           {
               container.Add(i);
           }
           int index = 0;
           int value = 0;
           for (int i = 1; i <= randomCount; i++)
           {
               // randomly select from container 
               index = random.Next(0, container.Count);
               value = container[index];
               result.Add(value);
               container.RemoveAt(index);
           }

           return result;
       }

    }

    // we have two sub class there ,ManualAssignmentPlatformWeightsetMethod，AutoAssignmentPlatformWeightSetMethod
    
}

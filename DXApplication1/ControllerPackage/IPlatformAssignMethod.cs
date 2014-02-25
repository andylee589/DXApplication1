using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DXApplication1.DataModelPackage;
using DXApplication1.ViewPackage;

namespace DXApplication1.ControllerPackage
{
    public enum PlatformAssignType
    {
        RANDOM_ASSIGN,
        BALANCE_ASSIGN,
    }


    [XmlInclude(typeof(RandomPlatformAssignMethod))]
    [XmlInclude(typeof(BalancedPlatformAssignMethod))]
    public abstract class IPlatformAssignMethod
    {
        public IPlatformAssignMethod() { }


       [XmlElement]
       public PlatformAssignType AssignType { get; set; }


       public abstract List<Dictionary<string, Dictionary<string, string>>> assignPlatformMethod(AnalysisStrategy strategy);
    }

    [XmlRoot]
   public class RandomPlatformAssignMethod : IPlatformAssignMethod
   {

        public RandomPlatformAssignMethod():base()
        {

            this.AssignType = PlatformAssignType.RANDOM_ASSIGN;
        }


        public override List<Dictionary<string, Dictionary<string, string>>> assignPlatformMethod(AnalysisStrategy strategy)
        {
            return null;
        }
   }

    public class BalancedPlatformAssignMethod : IPlatformAssignMethod
    {
        public BalancedPlatformAssignMethod()
        {
            this.AssignType = PlatformAssignType.BALANCE_ASSIGN;
        }

        public override List<Dictionary<string, Dictionary<string, string>>> assignPlatformMethod(AnalysisStrategy strategy)
       {
           //  language sorting
           strategy.project.variableImprotanceSorting();
           controlPrimaryLanuageRelatedList(strategy);
           controlPrimaryLanuageRelatelessList(strategy);
           return generateTestSet(strategy);
       }


        private void controlPrimaryLanuageRelatedList(AnalysisStrategy strategy)
        {
            List<string> primaryLanuageRelatedList = strategy.project.PrimaryMultiFactorList;
            
            foreach (string variableName in primaryLanuageRelatedList)
            {
                controlLanguageRelatedVarialbe(strategy, variableName);

            }           
        }

        private void controlPrimaryLanuageRelatelessList(AnalysisStrategy strategy)
        {
            List<string> primaryLanuageRelatelessList = strategy.project.PrimarySingleFactorList;
            foreach (string variableName in primaryLanuageRelatelessList)
            {
                controlLanaugeRelatelessVariable(strategy, variableName);

            }

        }

        //private void controlSecondaryLanuageRelatedList(AnalysisStrategy strategy)
        //{
        //    List<string> secondaryLanuageRelatedList = strategy.project.SecondaryLanuageRelatedList;
        //    foreach (string variableName in secondaryLanuageRelatedList)
        //    {
        //        controlSecondaryLanaugeRelateledVariable(strategy, variableName);

        //    }
        //}

        private void controlLanguageRelatedVarialbe(AnalysisStrategy strategy, string variableName)
        {
            int testRounds = strategy.TestRounds;
            Variable variable = strategy.project.Variables[variableName];
            variable.clearSelectedPlatform();
            List<Queue<string>> lanuageQueneList = getInitialQueueDict(strategy);
            // initialize the language Queue List
            for (int testIndex = 1; testIndex <= testRounds; testIndex++)
            {
                foreach (string languageName in variable.FactorList)
                {
                    Dictionary<string, Priority> itemDict = variable.PlatformPriorityMatrix[languageName];
                    KeyValuePair<string, Priority> priorityKV = getIndexLargestPriority(itemDict);
                    // key value pair has value
                    if (!priorityKV.Equals(new KeyValuePair<string, Priority>()))
                    {
                        lanuageQueneList[priorityKV.Value.Value].Enqueue(languageName);
                        variable.PlatformPriorityMatrix[languageName][priorityKV.Key].Enquued = true;
                    }
                    else
                    {
                        throw (new Exception("priority KV should not be null"));
                    }
                }
            }

            // start to select item
            for (int queueIndex = 1; queueIndex < lanuageQueneList.Count; queueIndex++)
            {
                Queue<string> queue = lanuageQueneList[queueIndex];

                for (int i = 0; i < queue.Count; i++)
                {
                    string lanuagName = queue.ElementAt(i);
                   
                    if (!variable.SelectedPlatform.ContainsKey(lanuagName))
                    {
                        variable.SelectedPlatform.Add(lanuagName, new List<string>());
                    }

                    Dictionary<string, Priority> itemDict = variable.PlatformPriorityMatrix[lanuagName];
                    Dictionary<string, int> testTimesDict = strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName];
                    List<string> candidateItemList = getCandidateItemList(itemDict, queueIndex, testTimesDict);
                    // the previous priority index is valid
                    if (candidateItemList.Count > 0)
                    {
                        
                        string selectItem = getSelectedItem(candidateItemList, testTimesDict);
                        strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName][selectItem]--;
                        variable.PlatformPriorityMatrix[lanuagName][selectItem].Selected = true;
                        // add select item to  variable
                        variable.SelectedPlatform[lanuagName].Add(selectItem);
                    }
                    // the previous priority index is invalid
                    else
                    {
                        KeyValuePair<string, Priority> priorityKV = getIndexLargestPriority(itemDict);
                        // key value pair has value
                        if (!priorityKV.Equals(new KeyValuePair<string, Priority>()))
                        {
                            lanuageQueneList[priorityKV.Value.Value].Enqueue(lanuagName);
                            variable.PlatformPriorityMatrix[lanuagName][priorityKV.Key].Enquued = true;
                        }
                        else
                        {

                            string specialCandidateItem = getSpecialCandidate(itemDict,  testTimesDict);
                            strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName][specialCandidateItem]--;
                            variable.PlatformPriorityMatrix[lanuagName][specialCandidateItem].Selected = true;
                            // add select item to  variable
                            variable.SelectedPlatform[lanuagName].Add(specialCandidateItem);

                            //priorityKV = getIndexLargestPriority(itemDict);
                            //throw (new Exception("priority KV should not be null"));
                        }
                    }
                }
            }
        }

        // in case of no match in such priority
        private string getSpecialCandidate(Dictionary<string, Priority> itemDict,  Dictionary<string, int> testTimesDict)
        {
            List<string> candidateList = new List<string>();
            foreach (KeyValuePair<string, Priority> kv in itemDict)
            {
                // select the rest item ,may be the one has been select 
                if ( testTimesDict[kv.Key] > 0)
                {
                    candidateList.Add(kv.Key);
                }
            }
            return candidateList[0];
        }


        private void controlLanaugeRelatelessVariable(AnalysisStrategy strategy, string variableName)
        {
            Variable variable = strategy.project.Variables[variableName];
            variable.clearSelectedPlatform();
            Dictionary<string, int> testTimesDict = strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName];
            // has only one fade language name
            string lanuageName = variable.FactorList[0];
            variable.SelectedPlatform.Add(lanuageName, new List<string>());


            List<string> itemList = new List<string>();


            foreach (KeyValuePair<string, Priority> kv in variable.PlatformPriorityMatrix[lanuageName])
            {
                while (strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName][kv.Key] > 0)
                {
                    strategy.PlatformWeightSetMethod.Value.PrimaryVariableTestTimesDict[variableName][kv.Key]--;
                    itemList.Add(kv.Key);
                }
            }

            List<int> selectIndexList = IPlatformWeightSetMethod.generateNumber(itemList.Count, itemList.Count);
            foreach (int index in selectIndexList)
            {
                string selectItem = itemList[index];
                variable.SelectedPlatform[lanuageName].Add(selectItem);
            }           
        }
        
        //private void controlSecondaryLanaugeRelateledVariable(AnalysisStrategy strategy, string variableName)
        //{
        //    int testRounds = strategy.TestRounds;
        //    Variable variable = strategy.project.Variables[variableName];
        //    variable.clearSecondarySelectPlatform();
        //    // first of all add the item times together.
        //    Dictionary<string, int> totalItemTimesDict = new Dictionary<string, int>();
        //    foreach (string itemStr in variable.PlatformList)
        //    {
        //        int times = getItemTotalTimes(strategy, variableName,itemStr);
        //        totalItemTimesDict.Add(itemStr, times);
        //    }

        //    List<Queue<string>> lanuageQueneList = getInitialQueueDict(strategy);
        //    // initialize the language Queue List
        //    for (int testIndex = 1; testIndex <= testRounds; testIndex++)
        //    {
        //        foreach (string languageName in variable.LangList)
        //        {
        //            Dictionary<string, Priority> itemDict = variable.PlatformPriorityMatrix[languageName];
        //            KeyValuePair<string, Priority> priorityKV = getIndexLargestPriority(itemDict);
        //            // key value pair has value
        //            if (!priorityKV.Equals(new KeyValuePair<string, Priority>()))
        //            {
        //                lanuageQueneList[priorityKV.Value.Value].Enqueue(languageName);
        //                variable.PlatformPriorityMatrix[languageName][priorityKV.Key].Enquued = true;
        //            }
        //            else
        //            {
        //                throw (new Exception("priority KV should not be null"));
        //            }

        //        }
        //    }

        //    // start to select item
        //    for (int queueIndex = 1; queueIndex < lanuageQueneList.Count; queueIndex++)
        //    {
        //        Queue<string> queue = lanuageQueneList[queueIndex];

        //        for (int i = 0; i < queue.Count; i++)
        //        {
        //            string lanuagName = queue.ElementAt(i);

        //            if (!variable.SelectedPlatform.ContainsKey(lanuagName))
        //            {
        //                variable.SelectedPlatform.Add(lanuagName, new List<string>());
        //            }

        //            Dictionary<string, Priority> itemDict = variable.PlatformPriorityMatrix[lanuagName];
        //            Dictionary<string, int> testTimesDict = totalItemTimesDict;
        //            List<string> candidateItemList = getCandidateItemList(itemDict, queueIndex, testTimesDict);
        //            // the previous priority index is valid
        //            if (candidateItemList.Count > 0)
        //            {

        //                string selectItem = getSelectedItem(candidateItemList, testTimesDict);
        //                totalItemTimesDict[selectItem]--;
        //                variable.PlatformPriorityMatrix[lanuagName][selectItem].Selected = true;
        //                // add select item to  variable
        //                variable.SelectedPlatform[lanuagName].Add(selectItem);
        //            }
        //            // the previous priority index is invalid
        //            else
        //            {
        //                KeyValuePair<string, Priority> priorityKV = getIndexLargestPriority(itemDict);
        //                // key value pair has value
        //                if (!priorityKV.Equals(new KeyValuePair<string, Priority>()))
        //                {
        //                    lanuageQueneList[priorityKV.Value.Value].Enqueue(lanuagName);
        //                    variable.PlatformPriorityMatrix[lanuagName][priorityKV.Key].Enquued = true;
        //                }
        //                else
        //                {
        //                    throw (new Exception("priority KV should not be null"));
        //                }
        //            }

        //        }
        //    }

        //    //set secondary Selected Platform according selected platform
        //    List<string> primayVariableList = getDependencePrimaryList(strategy, variableName);
        //    foreach (string primaryVariable in primayVariableList)
        //    {
        //        variable.SecondarySelectedPlatform.Add(primaryVariable, new Dictionary<string ,List<string>>());
        //        SerializableDictionary<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>> timesDict = strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict;
        //        SerializableDictionary<string, SerializableDictionary<string, int>> itemTimeDict = timesDict[new VariableDependenceRecordKey(primaryVariable, variableName)];
        //        foreach (KeyValuePair<string, SerializableDictionary<string, int>> kv1 in itemTimeDict)
        //        {
        //            string primaryItem = kv1.Key;
        //            variable.SecondarySelectedPlatform[primaryVariable].Add(primaryItem, new  List<string>());
        //            foreach (KeyValuePair<string, int> kv2 in kv1.Value)
        //            {
        //                string secondaryItem = kv2.Key;
        //                int testTime= kv2.Value;

        //                variable.SecondarySelectedPlatform[primaryVariable][primaryItem].Add()

        //            }
        //        }
        //    }

           
           
            
        //}


        //private int getItemTotalTimes(AnalysisStrategy strategy, string variableName,string itemStr)
        //{
        //    int sum = 0;
        //    SerializableDictionary<VariableDependenceRecordKey,SerializableDictionary<string,SerializableDictionary<string,int>>> timesDict =strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict;
        //    List<VariableDependenceRecordKey> recordKeyList = getDependenceRecordKeyList(strategy, variableName);
        //    foreach (VariableDependenceRecordKey recordKey in recordKeyList)
        //    {
        //        SerializableDictionary<string, SerializableDictionary<string, int>> totalTimesItemDict = timesDict[recordKey];
        //        foreach (KeyValuePair<string, SerializableDictionary<string, int>> kv in totalTimesItemDict)
        //        {
        //            if (kv.Value.ContainsKey(itemStr))
        //            {
        //                sum += kv.Value[itemStr];
        //            }
        //        }


        //    }
        //    return sum;
        //}




        private List<string> getDependencePrimaryList(AnalysisStrategy strategy, string variableName)
        {
            List<string> dependencePrimaryList = new List<string>();
            List<VariableDependenceRecordKey> dependenceRecordKeyList= new List<VariableDependenceRecordKey>(strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordDict.Keys) ;
            foreach (VariableDependenceRecordKey key in dependenceRecordKeyList)
            {
                if (key.SecondaryVariableKey.Equals(variableName))
                {
                    dependencePrimaryList.Add(key.PrimaryVariableKey);
                }
            }

            return dependencePrimaryList;
        }

        private List<VariableDependenceRecordKey> getDependenceRecordKeyList(AnalysisStrategy strategy, string variableName)
        {
            List<VariableDependenceRecordKey> dependenceRecordList = new List<VariableDependenceRecordKey>();
            List<VariableDependenceRecordKey> dependenceRecordKeyList = new List<VariableDependenceRecordKey>(strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordDict.Keys);
            foreach (VariableDependenceRecordKey key in dependenceRecordKeyList)
            {
                if (key.SecondaryVariableKey.Equals(variableName))
                {
                    dependenceRecordList.Add(key);
                }
            }

            return dependenceRecordList;
        }



        private List<string> getCandidateItemList(Dictionary<string, Priority> itemDict, int priorityValue, Dictionary<string, int> testTimesDict)
        {
            List<string> candidateList = new List<string>();
            foreach (KeyValuePair<string, Priority> kv in itemDict)
            {
                // select have same priority value and has not been selected
                if (kv.Value.Value == priorityValue && !kv.Value.Selected && testTimesDict[kv.Key]>0)
                {
                    candidateList.Add(kv.Key);
                }
            }

            return candidateList;
        }

        private string getSelectedItem(List<string> candidateItemList,  Dictionary<string, int> testTimesDict)
        {
            string selected = null;
            List<KeyValuePair<string, int>> testTimesList = new List<KeyValuePair<string, int>>();
            foreach (string key in candidateItemList)
            {
                if (testTimesDict.ContainsKey(key))
                {
                    KeyValuePair<string, int> kv = new KeyValuePair<string, int>(key, testTimesDict[key]);
                    testTimesList.Add(kv);
                }
            }
           
                              
            testTimesList.Sort(
                delegate(KeyValuePair<string, int> firstPair, KeyValuePair<string, int> nextPair)
                {
                    return nextPair.Value.CompareTo(firstPair.Value);
                }
            );

            int firstValue = testTimesList[0].Value;
            if (firstValue > 0)
            {
                selected = testTimesList[0].Key;
            }
           
            return selected;
        }


        private List<Queue<string>> getInitialQueueDict(AnalysisStrategy strategy)
        {
            List<Queue<string>> lanuageQueneList = new  List <Queue<string>>();
            int priorityNum = strategy.project.PriorityColorDict.Keys.Count;
            // create 1 more queue to solve the index start with 1 problem
            for (int i = 0; i <= priorityNum;i++ )
            {
                lanuageQueneList.Add(new Queue<string>());
            }
            return lanuageQueneList;
        }

        private KeyValuePair<string,Priority> getIndexLargestPriority(Dictionary<string, Priority> itemDict)
        {
            KeyValuePair<string, Priority> priorityKV = new KeyValuePair<string, Priority>();
            List<KeyValuePair<string, Priority>> itemPriorityList = itemDict.ToList();
            itemPriorityList.Sort(
               delegate(KeyValuePair<string, Priority> firstPair, KeyValuePair<string, Priority> nextPair)
               {
                   return firstPair.Value.Value.CompareTo(nextPair.Value.Value);
               }
           );

            for (int i = 0; i < itemPriorityList.Count; i++)
            {
                if (!itemPriorityList[i].Value.Enquued)
                {
                    priorityKV = itemPriorityList[i];

                    break;
                }
            }

            return priorityKV;
        }


        private List<Dictionary<string, Dictionary<string, string>>> generateTestSet(AnalysisStrategy strategy)
        {
            
            string temVariableName=  strategy.project.MultiFactorVariableNameList[0];
            Dictionary<string, Variable> variables = strategy.project.Variables;
            Variable tempVariable = variables[temVariableName];
            // total language List
            List<string> languageList = tempVariable.FactorList;
            // total test rounds
            int testRounds = strategy.TestRounds;
            // first index text iteration,secondary string language name,third string variable name,third name item name
            List<Dictionary<string, Dictionary<string, string>>> testSet = new List<Dictionary<string, Dictionary<string, string>>>();
            for (int testIndex = 0; testIndex < testRounds;testIndex++ )
            {
                testSet.Add(new Dictionary<string,  Dictionary<string,string>> ());
                foreach (string languageName in languageList)
                {
                    testSet[testIndex].Add(languageName, new Dictionary<string,string>());
                    // get select item for each variable in current language
                    foreach (string variableName in strategy.project.PrimaryMultiFactorList)
                    {
                        Variable variable = variables[variableName];
                        string selectItem = variable.SelectedPlatform[languageName][testIndex];
                        testSet[testIndex][languageName].Add(variableName, selectItem);
                        Dictionary<string, string> secondaryDict = getSecondaryItem(strategy, variableName, selectItem);
                        // has dependence record
                        if (secondaryDict.Count > 0)
                        {   
                            foreach(KeyValuePair<string,string> kv in secondaryDict)
                            {
                                testSet[testIndex][languageName].Add(kv.Key, kv.Value);
                            }
                            
                        }
                        
                    }

                    // add language relate less variable
                    foreach (string variableName in strategy.project.PrimarySingleFactorList)
                    {
                        Variable variable = variables[variableName];
                        string fadeLanuageName = variable.SelectedPlatform.First().Key;
                        string selectItem = variable.SelectedPlatform[fadeLanuageName].First();
                        variable.SelectedPlatform[fadeLanuageName].RemoveAt(0);
                        testSet[testIndex][languageName].Add(variableName, selectItem);
                        Dictionary<string, string> secondaryDict = getSecondaryItem(strategy, variableName, selectItem);
                        // has dependence record
                        if (secondaryDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> kv in secondaryDict)
                            {
                                testSet[testIndex][languageName].Add(kv.Key, kv.Value);
                            }

                        }
                    }


                }             
            }
            return testSet;
        }

        private Dictionary<string,string> getSecondaryItem(AnalysisStrategy strategy,string variableName,string itemName)
        {
            // first string variable name ,second string item string
            Dictionary<string,string> secondaryDict = new Dictionary<string,string>();
            SerializableDictionary<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>> timeDict = strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict;
            foreach (KeyValuePair<VariableDependenceRecordKey, SerializableDictionary<string, SerializableDictionary<string, int>>> kv in timeDict)
            {
                if (kv.Key.PrimaryVariableKey.Equals(variableName)&& kv.Value.ContainsKey(itemName))
                {
                    string secondaryVariable = kv.Key.SecondaryVariableKey;
                    SerializableDictionary<string, int> secondaryItemTimesDict = kv.Value[itemName];
                    foreach (KeyValuePair<string, int> kv1 in secondaryItemTimesDict)
                    {
                        if (kv1.Value > 0)
                        {
                            string secondaryItemName = kv1.Key;
                            strategy.PlatformWeightSetMethod.Value.SecondaryItemWeightSetMethod.Value.DependenceRecordTestTimesDict[kv.Key][itemName][secondaryItemName]--;
                            secondaryDict.Add(secondaryVariable, secondaryItemName);
                            break;
                        }
                    }
                }
            }


            return secondaryDict;
        }
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
namespace DXApplication1
{
    //public class PriorityRecord
    //{
    //    public string VariableName { get; set; }
    //    public string Type { get; set; }
    //    public string Platform { get; set; }
    //    public string DE { get; set; }
    //    public string FR { get; set; }
    //    public string ES { get; set; }
    //    public string JA { get; set; }
    //    public string SC { get; set; }
    //    public string KO { get; set; }
    //    public string TC { get; set; }
    //    public string RU { get; set; }


    //    public PriorityRecord(string  variableName,VariableType variablType, string platform,string de,string fr,string es,string ja,string sc,string ko,string tc,string ru )
    //        : this(variableName, variablType, platform) 
    //    {
            
    //        this.DE = de;
    //        this.FR = fr;
    //        this.ES = es;
    //        this.JA = ja;
    //        this.SC = sc;
    //        this.KO = ko;
    //        this.TC = tc;
    //        this.RU = ru;
    //    }

    //    public PriorityRecord(string  variableName,VariableType? variablType, string platform)
    //    {
    //        this.VariableName = variableName;
    //        if (variablType == VariableType.MULTI_FACTOR)
    //        {
    //            this.Type = StaticResource.multiFactorStr;
    //        }
    //        else if (variablType == VariableType.SINGLE_FACTOR)
    //        {
    //            this.Type = StaticResource.singleFactorStr;
    //        }
    //        //this.VariableType = variablType;
    //        this.Platform = platform;
    //    }

    //}


   
    public class Project
    {
        string name = null;
        Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
        Dictionary<int, Color> priorityColorDict = new Dictionary<int, Color>();
        Dictionary<int, Color> priorityFontColorDict = new Dictionary<int, Color>();
        VariableRelation variableRelation=new VariableRelation();
        List<AnalysisStrategy> strategyList = new List<AnalysisStrategy>();

        List<string> multiFactorVariableNameList = new List<string>();
        List<string> singleFactorVariableNameList = new List<string>();
        List<string> primaryMultiFactorList = new List<string>();
        List<string> primarySingleFactorList = new List<string>();
        List<string> secondaryMultiFactorList = new List<string>();
        List<string> secondarySingleFactorList = new List<string>();
        DataTable priorityDataTable;
        // single modern
      /*  private static Project instance = new Project();

        private Project() { }

        public static Project getInstance() 
        {
            if (instance == null)
            {
                instance = new Project();
            }

            return instance;
        }
           
       */ 


        #region  property
 
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public Dictionary<string, Variable> Variables
        {
            get
            {
                return variables;
            }
            set
            {
                variables = value;
            }
        }

        public Dictionary<int, Color> PriorityColorDict
        {
            get
            {
                return priorityColorDict;
            }

            set
            {
                priorityColorDict = value;
            }
        }

        public Dictionary<int, Color> PriorityFontColorDict
        {
            get
            {
                return priorityFontColorDict;
            }

            set
            {
                priorityFontColorDict = value;
            }
        }


        public VariableRelation VariableRelationSetting
        {
            get
            {
                return variableRelation;
            }

            set
            {
                variableRelation = value;
            }
        }

        public List<AnalysisStrategy> StrategyList
        {
            get
            {
                return strategyList;
            }
            set
            {
                strategyList = value;
            }
        }

        public List<string> MultiFactorVariableNameList
        {
            get
            {
                return this.multiFactorVariableNameList;
            }

            set
            {
                this.multiFactorVariableNameList = value;
            }
        }

        public List<string> SingleFactorVariableNameList
        {
            get
            {
                return this.singleFactorVariableNameList;
            }

            set
            {
                this.singleFactorVariableNameList = value;
            }
        }
       
        public List<string> PrimarySingleFactorList
        {
            get
            {
                return this.primarySingleFactorList;
            }

            set
            {
                this.primarySingleFactorList = value;
            }
        }

        public List<string> PrimaryMultiFactorList
        {
            get
            {
                return this.primaryMultiFactorList;
            }

            set
            {
                this.primaryMultiFactorList = value;
            }
        }

        public List<string> SecondarySingleFactorList
        {
            get
            {
                return this.secondarySingleFactorList;
            }

            set
            {
                this.secondarySingleFactorList = value;
            }
        }

        public List<string> SecondaryMultiFactorList
        {
            get
            {
                return this.secondaryMultiFactorList;
            }

            set
            {
                this.secondaryMultiFactorList = value;
            }
        }

        public DataTable PriorityDataTable
        {
            get
            {
                return this.priorityDataTable;
            }
            set
            {
                this.priorityDataTable = value;
            }
        }
 #endregion

        public void putPriorityColor(int priority, Color color)
        {
            if (!priorityColorDict.ContainsKey(priority))
            {
                priorityColorDict.Add(priority, color);
            }
        }

      

        public void putPriorityFontColor(int priority, Color color)
        {
            if (!priorityFontColorDict.ContainsKey(priority))
            {
                priorityFontColorDict.Add(priority, color);
            }
        }

        //public BindingList<PriorityRecord> getPriorityRecords()
        //{
        //    BindingList<PriorityRecord> recordsList = new BindingList<PriorityRecord>();
        //    Controller controller = Controller.getInstance();

        //    foreach (KeyValuePair<string, Variable> kv in variables)
        //    {

        //        Variable tempVariable = kv.Value;
        //        int platformCount = tempVariable.PlatformList.Count;
        //        int langCount = tempVariable.FactorList.Count;
        //        string name = tempVariable.Name;
        //        VariableType? type = tempVariable.Type;
        //        PriorityRecord[] recordGroup = new PriorityRecord[platformCount];
        //        for (int i = 0; i < platformCount; i++)
        //        {
        //            string platformName = tempVariable.PlatformList[i];
        //            PriorityRecord tempRecord = new PriorityRecord(name, type, platformName);
        //            tempRecord.DE = tempVariable.lookup("DE", platformName).Value.ToString();
        //            if (tempVariable.Type == VariableType.MULTI_FACTOR)
        //            {
        //                tempRecord.FR = tempVariable.lookup("FR", platformName).Value.ToString();
        //                tempRecord.ES = tempVariable.lookup("ES", platformName).Value.ToString();
        //                tempRecord.JA = tempVariable.lookup("JA", platformName).Value.ToString();
        //                tempRecord.SC = tempVariable.lookup("SC", platformName).Value.ToString();
        //                tempRecord.KO = tempVariable.lookup("KO", platformName).Value.ToString();
        //                tempRecord.TC = tempVariable.lookup("TC", platformName).Value.ToString();
        //                tempRecord.RU = tempVariable.lookup("RU", platformName).Value.ToString();
        //            }


        //            recordsList.Add(tempRecord);
        //        }
        //    }

        //    return recordsList;
        //}

        
        public DataTable getPriorityDataTable(){
            List<string> columnList = getPriorityColoumnList();
            List<List<string>> setList = getPriorityRowList();
            priorityDataTable = createTable(columnList, setList);
            return priorityDataTable;
        }


        private List<string> getPriorityColoumnList()
        {
            List<string> columnList = new List<string>();
            string variableType = StaticResource.varialbeTypeStr;
            string variableName = StaticResource.variableStr;
            string platformName = StaticResource.platformStr;
            
            columnList.Add(variableName);
            columnList.Add(variableType);
            columnList.Add(platformName);
            Variable fadeVariable = this.Variables[this.MultiFactorVariableNameList[0]];
            foreach (string factorName in fadeVariable.FactorList)
            {
                if (fadeVariable.FactorList.IndexOf(factorName) == 0)
                {
                    if(this.singleFactorVariableNameList.Count>0)
                    {
                        string fadeSingleVariableStr = this.singleFactorVariableNameList[0];
                        string singleFactorName = this.variables[fadeSingleVariableStr].FactorList[0];
                        columnList.Add(factorName+" / "+singleFactorName);
                    }                   
                }
                else
                {
                    columnList.Add(factorName);
                }
                
            }
            return columnList;
        }

        private List<List<string>> getPriorityRowList()
        {
            List<List<string>> setList = new List<List<string>>();

            foreach (KeyValuePair<string, Variable> kv in variables)
            {
                Variable tempVariable = kv.Value;
                int platformCount = tempVariable.PlatformList.Count;
                int factorCount = tempVariable.FactorList.Count;
                string name = tempVariable.Name;
                VariableType? type = tempVariable.Type;
                for (int i = 0; i < platformCount; i++)
                {
                    string platformName = tempVariable.PlatformList[i];

                    List<string> tempRecord = new List<string>();
                    tempRecord.Add(name);
                    tempRecord.Add(StaticResource.VariableTypeDict[type]);
                    tempRecord.Add(platformName);
                    Variable fadeVariable = this.Variables[this.MultiFactorVariableNameList[0]];
                    switch (tempVariable.Type)
                    {
                        case VariableType.SINGLE_FACTOR:
                            string fadeSingleVariableStr = this.singleFactorVariableNameList[0];
                            string singleFactorName = this.variables[fadeSingleVariableStr].FactorList[0];
                            tempRecord.Add(tempVariable.lookup(singleFactorName, platformName).Value.ToString());
                            break;
                        case VariableType.MULTI_FACTOR:
                            foreach (string factorName in fadeVariable.FactorList)
                            {
                                tempRecord.Add(tempVariable.lookup(factorName, platformName).Value.ToString());
                            }
                            break;
                    }

                    setList.Add(tempRecord);
                }
            }          
            return setList;
        }

        private DataTable createTable(List<string> columnList, List<List<string>> setList)
        {
            DataTable dataTable = new DataTable();
            foreach (string columnName in columnList)
            {
                dataTable.Columns.Add(columnName, columnName.GetType());
            }

            foreach (List<string> rowRecord in setList)
            {
                object[] objectArray = rowRecord.ToArray();
                dataTable.Rows.Add(objectArray);
            }

            return dataTable;
        }

        public  void setDefaultVariableRelation()
        {
            this.variableRelation.PrimaryVariableList.Clear();
            this.variableRelation.SecondaryVariableList.Clear();
            this.variableRelation.DependenceRecordDict.Clear();
           foreach (KeyValuePair<string, Variable> kv in this.Variables)
            {
                this.variableRelation.PrimaryVariableList.Add(kv.Key);
            }
        }

        public void setDefaultStrategyList()
        {
            this.strategyList.Clear();
            AnalysisStrategy strategy = new AnalysisStrategy();
            string strategyName =StaticResource.defaultStrategyNameStr;
            string description = StaticResource.defaultStrategDescripStr;
            int testRounds = 1;
            strategy.initStrategyInfo(strategyName, description, testRounds);
            this.strategyList.Add(strategy);
           
        }

        public void variablesTypeSorting()
        {
            foreach (KeyValuePair<string,Variable> kv in this.Variables)
            {
                switch (kv.Value.Type)
                {
                    case VariableType.MULTI_FACTOR:
                        this.multiFactorVariableNameList.Add(kv.Key);
                        break;
                    case VariableType.SINGLE_FACTOR:
                        this.singleFactorVariableNameList.Add(kv.Key);
                        break;
                }
            }
        }

        public void variableImprotanceSorting()
        {
            List<string> multiFactorList = this.MultiFactorVariableNameList;
            List<string> singleFactorList = this.SingleFactorVariableNameList;
            List<string> primaryList = this.VariableRelationSetting.PrimaryVariableList;
            List<string> secondaryList = this.VariableRelationSetting.SecondaryVariableList;
            this.secondaryMultiFactorList.Clear();
            this.secondarySingleFactorList.Clear();
            this.primaryMultiFactorList.Clear();
            this.primarySingleFactorList.Clear();

            foreach (string variabeName in primaryList)
            {
                if (multiFactorList.Contains(variabeName))
                {
                    primaryMultiFactorList.Add(variabeName);
                }
                else if (singleFactorList.Contains(variabeName))
                {
                    primarySingleFactorList.Add(variabeName);
                }
            }

            foreach (string variabeName in secondaryList)
            {
                if (multiFactorList.Contains(variabeName))
                {
                    secondaryMultiFactorList.Add(variabeName);
                }
                else if (singleFactorList.Contains(variabeName))
                {
                    secondarySingleFactorList.Add(variabeName);
                }
            }
        }
    
    }
}

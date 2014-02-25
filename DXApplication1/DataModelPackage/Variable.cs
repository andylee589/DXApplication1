using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DXApplication1
{
    public enum VariableType
    {
        MULTI_FACTOR,
        SINGLE_FACTOR,

    }
    public class Priority
    {
        int priorityValue=0;
        //measure value is the value we use to compute
        double measureValue = 0;
        bool isSelect = false;
        bool isEnquue = false;
       // Color color;
        public Priority()
        {

        }
       /*
        public Priority(int value, Color color)
        {
            this.priorityValue = value;
            this.color = color;

        }
        * */

        public Priority(int value)
        {
            this.priorityValue = value;
          
        }

        public int Value
        {
            get
            {
                return priorityValue;
            }
            set
            {
                priorityValue = value;
            }
        }

        public bool Selected
        {
            get
            {
                return this.isSelect;
            }
            set
            {
                this.isSelect = value;
            }
            

        }

        public bool Enquued
        {
            get
            {
                return this.isEnquue;
            }
            set
            {
                this.isEnquue = value;
            }
        }

        public double MeasureValue
        {
            get
            {
                return measureValue;
            }

            set
            {
                measureValue = value;
            }
        }
        /*
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }
         * */
    }
    public class Variable
    {
        string variableName;
        VariableType? type;
        List<string> platformList = new List<string>();
        List<string > factorList = new List<string>();
        // each language such as "DE" corresponds  with  a dictionary that has records of each platform's priority
        Dictionary<string ,Dictionary <string,Priority>> platformPriorityMatrix = new Dictionary<string,Dictionary<string,Priority>>();
        Dictionary<string, List<string>> selectedPlatform = new Dictionary<string, List<string>>();
        Dictionary<string, Dictionary<string, List<string>>> secondarySelectedPlatform = new Dictionary<string, Dictionary<string, List<string>>>();


        // the dictionary of each platform priority and  measure value
        Dictionary<int, double> platformPriorityMeasureDict = new Dictionary<int, double>();

        // different language have different priority in test process
        Dictionary<string, int> factorPriorityDict = new Dictionary<string, int>();
        // the dictionary of each language priority and measure value
        Dictionary<int, double> factorPriorityMeasureDict = new Dictionary<int, double>();


        #region  property
  public VariableType? Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string Name
        {
            get
            {
                return variableName;
            }
            set
            {
                variableName = value;
            }
        }

        public List<string> PlatformList
        {
            get
            {
                return platformList;
            }

            set
            {
                platformList = value;
            }
        }

        public List<string> FactorList
        {
            get
            {
                return factorList;
            }
            set
            {
                factorList = value;
            }
        }


        public Dictionary<string, Dictionary<string, Priority>> PlatformPriorityMatrix
        {
            get
            {
                return platformPriorityMatrix;
            }
            set
            {
                platformPriorityMatrix = value;
            }
        }

        public Dictionary<int, double> PlatformPriorityMeasureDict
        {
            get
            {
                return platformPriorityMeasureDict;
            }
            set
            {
                platformPriorityMeasureDict = value;
            }
        }

        public Dictionary<string, int> FactorPriorityDict
        {
            get
            {
                return factorPriorityDict;
            }

            set
            {
                factorPriorityDict = value;
            }
        }

        public Dictionary<int, double> FactorPriorityMeasureDict
        {
            get
            {
                return factorPriorityMeasureDict;
            }
            set
            {
                factorPriorityMeasureDict = value;
            }
        }

        public Dictionary<string, List<string>> SelectedPlatform
        {
            get
            {
                return this.selectedPlatform;
            }
            set
            {
                this.selectedPlatform = value;
            }
        }

        public Dictionary<string, Dictionary<string, List<string>>> SecondarySelectedPlatform
        {
            get
            {
                return this.secondarySelectedPlatform;
            }
            set
            {
                this.secondarySelectedPlatform = value;
            }
        }


  #endregion

       public Priority  lookup(string factor,string platform)
        {
            bool isContain = false;
            Priority priority=null;
            if (platformPriorityMatrix.ContainsKey(factor))
            {
                Dictionary<string, Priority> tempDict=platformPriorityMatrix[factor];
                if (tempDict.ContainsKey(platform))
                {
                    isContain = true;
                     priority= tempDict[platform];
                }
            }
            if (!isContain)
            {
                return new Priority();
            }
            else
            {
                return priority;
            }

        }

        public void  setup(string factor,string platform,Priority priority)
       {

           if (platformPriorityMatrix.ContainsKey(factor))
           {
               Dictionary<string, Priority> tempDict = platformPriorityMatrix[factor];
               if (!tempDict.ContainsKey(platform))
               {
                   tempDict.Add(platform, priority);
               }
           }
           else
           {
               Dictionary<string, Priority> tempDict=new Dictionary<string,Priority>();
               tempDict.Add(platform, priority);
               platformPriorityMatrix.Add(factor,tempDict);
           }
       }


        public void clearSelectedPlatform()
        {
            this.selectedPlatform.Clear();
            foreach (KeyValuePair<string, Dictionary<string, Priority>> kv in this.platformPriorityMatrix)
            {
                foreach (KeyValuePair<string, Priority> kv2 in kv.Value)
                {
                    kv2.Value.Enquued = false;
                    kv2.Value.Selected = false;
                }
            }
        }

        public void clearSecondarySelectPlatform()
        {
            this.secondarySelectedPlatform.Clear();
            foreach (KeyValuePair<string, Dictionary<string, Priority>> kv in this.platformPriorityMatrix)
            {
                foreach (KeyValuePair<string, Priority> kv2 in kv.Value)
                {
                    kv2.Value.Enquued = false;
                    kv2.Value.Selected = false;
                }
            }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DXApplication1.DataModelPackage;

namespace DXApplication1.ControllerPackage
{
    [XmlInclude(typeof(EqualFactorPrioritySetMethod))]
    [XmlInclude(typeof(CustomFactorPrioritySetMethod))]
    public abstract class   IFactorPrioritySetMethod
    {
        
        [XmlElement]
        public SerializableDictionary<string, int> FactorPriorityDict { get; set; }

        public IFactorPrioritySetMethod()
        {
            
            initFactorPrioritylDict();
        }
        public void updateFactorList()
        {
            if (this.FactorPriorityDict.Count == 0)
            {
                Controller controller = Controller.getInstance();
                string fadeVariableStr = controller.project.MultiFactorVariableNameList[0];
                Variable fadeVariable = controller.project.Variables[fadeVariableStr];
                foreach (string factorName in fadeVariable.FactorList)
                {
                    this.FactorPriorityDict.Add(factorName, 1);
                }
            }           
        }

        private void initFactorPrioritylDict(){
            this.FactorPriorityDict = new SerializableDictionary<string, int>();
            Controller controller = Controller.getInstance();
            Project project = controller.project;
            if (project != null)
            {
                string fadeVariableStr = controller.project.MultiFactorVariableNameList[0];
                Variable fadeVariable = controller.project.Variables[fadeVariableStr];
                foreach (string factorName in fadeVariable.FactorList)
                {
                    this.FactorPriorityDict.Add(factorName, 1);
                }
            }
            
     
        }

        public virtual void setFactorPriority(SerializableDictionary<string, int> dict) { }
        public virtual void setFactorPriority() { }
       
        
    }

    [XmlRoot]
    public class EqualFactorPrioritySetMethod : IFactorPrioritySetMethod
    {
        public EqualFactorPrioritySetMethod():base()
        {
           // setLanguagePriority();
        }

        public override void setFactorPriority()
        {
            Controller controller = Controller.getInstance();
            string fadeVariableStr = controller.project.MultiFactorVariableNameList[0];
            Variable fadeVariable = controller.project.Variables[fadeVariableStr];
            foreach (string factorName in fadeVariable.FactorList)
            {
                if (this.FactorPriorityDict.ContainsKey(factorName))
                {
                    this.FactorPriorityDict[factorName] = 1;
                }
                else
                {
                    this.FactorPriorityDict.Add(factorName, 1);
                }
               
            }

            
        }
       
    }

    [XmlRoot]
    public class CustomFactorPrioritySetMethod : IFactorPrioritySetMethod
    {
        public CustomFactorPrioritySetMethod()
            : base()
        {

        }

        public override void setFactorPriority( SerializableDictionary<string, int> dict )
        {
           // throw new NotImplementedException();
            this.FactorPriorityDict = dict;
        }       
    }

}

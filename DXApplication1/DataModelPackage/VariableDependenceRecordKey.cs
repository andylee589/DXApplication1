using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DXApplication1.DataModelPackage
{
    [XmlRoot]
   public class VariableDependenceRecordKey : IEquatable<VariableDependenceRecordKey>
    {
        private string primaryVariableStr;
        private string secondaryVariableStr;

        public VariableDependenceRecordKey()
        {

        }

        public VariableDependenceRecordKey(string primaryVariable, string secondaryVariable)
        {
            this.primaryVariableStr = primaryVariable;
            this.secondaryVariableStr = secondaryVariable;
        }

        [XmlElement]
        public String PrimaryVariableKey
        {
            get
            {
                return primaryVariableStr;
            }

            set
            {
                primaryVariableStr = value;
            }
        }

        [XmlElement]
        public string SecondaryVariableKey
        {
            get
            {
                return secondaryVariableStr;
            }

            set
            {
                secondaryVariableStr = value;
            }
        }

        public override bool Equals(object obj)
        {
            VariableDependenceRecordKey recordKey = obj as VariableDependenceRecordKey;

            return this.primaryVariableStr.Equals(recordKey.primaryVariableStr)&&this.secondaryVariableStr.Equals(recordKey.secondaryVariableStr);
        }

        public override int GetHashCode()
        {
            
            return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName+"#"+this.primaryVariableStr+"#"+this.secondaryVariableStr).GetHashCode();
        }

        bool IEquatable<VariableDependenceRecordKey>.Equals(VariableDependenceRecordKey other)
        {
            return this.primaryVariableStr.Equals(other.primaryVariableStr) && this.secondaryVariableStr.Equals(other.secondaryVariableStr);
        }
    }
}

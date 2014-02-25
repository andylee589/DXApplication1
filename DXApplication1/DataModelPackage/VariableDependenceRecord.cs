using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DXApplication1.DataModelPackage
{
    [XmlRoot]
    public class SecondaryItem
    {   
        [XmlElement]
        public string SecondaryItemStr { get; set; }
        [XmlElement]
        public double Weight { get; set; }

        public SecondaryItem()
        {

            Weight = 0;
        }

        public SecondaryItem(string secondaryItem)
        {
            this.SecondaryItemStr = secondaryItem;
            this.Weight = 0;
        }

        public SecondaryItem(string secondaryItem, double weight)
        {
            this.SecondaryItemStr = secondaryItem;
            this.Weight = weight;
        }

    }

    


    [XmlRoot]
 public    class VariableDependenceRecord
    {
        
        private string primaryVariable;
        private string secondaryVariable;
        // first key primaryItem  value a dictionary that has a key of secondary item and value of secondItem and weight
        private SerializableDictionary<string ,SerializableDictionary <string, SecondaryItem>> itemDependenceDict = new SerializableDictionary<string, SerializableDictionary <string,SecondaryItem>>();
       // private SerializableDictionary<string,List<string> > itemDependenceDict = new SerializableDictionary<string,List<string>>();
        [XmlElement]
        public string PrimaryVariable
        {
            get
            {
                return primaryVariable;
            }
            set
            {
                primaryVariable = value;
            }
        }

        [XmlElement]
        public string SecondaryVarialbe
        {
            get
            {
                return secondaryVariable;
            }
            set
            {
                secondaryVariable = value;
            }
        }

       [XmlElement]
        public SerializableDictionary<string, SerializableDictionary<string, SecondaryItem>> ItemDependencDict
        {
            get
            {
                return itemDependenceDict;
            }

            set
            {
                itemDependenceDict = value;
            }
        }
        
       // public SerializableDictionary<string,List<string>> ItemDependencDict 
       //{
       //    get 
       //    {
       //        return ItemDependencDict;
       //    }

       //    set 
       //    {
       //        ItemDependencDict = value;
       //    }
       //}
    }
}

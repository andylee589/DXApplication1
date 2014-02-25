using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using DXApplication1.ViewPackage;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using DXApplication1.ViewPackage;
namespace DXApplication1
{
   partial class Controller
    {
        public VariableDependenceOptionForm variableOptionForm { get; set; }
        string variableFileName = "variable relation.xml";
        // xml serializer  for variable relation object
        XmlSerializer variableRelationSerializer = new XmlSerializer(typeof(VariableRelation));
   
  
        // set the project default variable relation when load
        public void setProjectDefaultVaraibleRelation()
        {
           
            VariableRelation exsistRelation = XMLDeserializeVariableRelation();
            if (exsistRelation == null)
            {
                this.project.setDefaultVariableRelation();
                this.XMLSerializeVariableRelation(this.project.VariableRelationSetting);
            }
            else
            {
                // decide whether  the load excel variable names are same as the variable names read from xml file;
                List<string> keyList = (this.project.Variables.Keys.ToList());
                bool isEqual = true;

                List<string> exsistList = new List<string>(exsistRelation.PrimaryVariableList);
                exsistList.AddRange(exsistRelation.SecondaryVariableList);
                if (keyList.Count == exsistList.Count)
                {
                    foreach (string tempStr in keyList)
                    {
                        if (!exsistList.Contains(tempStr))
                        {
                            isEqual = false;
                            break;
                        }
                    }
                }
                else
                {
                    isEqual = false;
                }

                if (isEqual)
                {
                    this.project.VariableRelationSetting = exsistRelation;
                }
                else
                {
                    this.project.setDefaultVariableRelation();
                    this.XMLSerializeVariableRelation(this.project.VariableRelationSetting);
                }

            }
        }

        
        public void XMLSerializeVariableRelation(VariableRelation variableRelation)
        {                  
            try
            {
                Stream stream = new FileStream(workFolder + variableFileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                variableRelationSerializer.Serialize(stream, variableRelation);
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);              
            }         
        }


        public VariableRelation XMLDeserializeVariableRelation()
        {
            VariableRelation variableRelation = null;
            if (File.Exists(workFolder + variableFileName))
            {
                try
                {
                    Stream stream = new FileStream(workFolder + variableFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    variableRelation = variableRelationSerializer.Deserialize(stream) as VariableRelation;
                    stream.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace);
                }
            }
            else
            {  // configure file do not exsit ,just create and use the default relation
                this.project.setDefaultVariableRelation();
                this.XMLSerializeVariableRelation(this.project.VariableRelationSetting);
                variableRelation = this.project.VariableRelationSetting;
            }
            return variableRelation;
        }

    }
}

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
        public AnalysisStrategyOptionForm strategyOptionForm { get; set; }
        string strategyFileFoder = "Analysis Strategy\\";
        // xml serializer  for Analysis Strategy object
        XmlSerializer analysisStrategySerializer = new XmlSerializer(typeof(AnalysisStrategy));

        // set the project default Analysis Strategy when load
        public void setProjectDefaultAnalysisStrategy()
        {
            List<AnalysisStrategy> exsistStrategyList = XMLDeserializeAnalysisStrategy();
            VariableRelation exsistRelation = XMLDeserializeVariableRelation();
            if (exsistStrategyList == null)
            {
                this.project.setDefaultStrategyList();
                this.XMLSerializeAnalysisStrategyList(this.project.StrategyList);

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
                    this.project.StrategyList = exsistStrategyList;
                }
                else
                {
                    this.project.setDefaultVariableRelation();
                    this.project.setDefaultStrategyList();
                    this.XMLSerializeVariableRelation(this.project.VariableRelationSetting);
                    this.XMLSerializeAnalysisStrategyList(this.project.StrategyList);
                }

            }
        }

        public void XMLSerializeAnalysisStrategyList(List<AnalysisStrategy> strategyList)
        {
            foreach (AnalysisStrategy strategy in strategyList)
            {
                try
                {
                    Stream stream = new FileStream(workFolder + strategyFileFoder + strategyList.IndexOf(strategy) + strategy.StrategyName + ".xml", FileMode.Create, FileAccess.Write, FileShare.Read);
                    analysisStrategySerializer.Serialize(stream, strategy);
                    stream.Close();
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.StackTrace);

                }
            }
        }


        public void clearStrategyFileWithIndex(int index)
        {
            string[] allFileList = Directory.GetFiles(workFolder + strategyFileFoder);
            List<string> fileList = new List<string>();
            foreach (string file in allFileList)
            {
                string name = Regex.Split(file, "\\\\").Last();

                if (name.EndsWith(".xml") && name.StartsWith(index.ToString()))
                {
                    File.Delete(file);
                }
            }


        }

        public List<AnalysisStrategy> XMLDeserializeAnalysisStrategy()
        {
            List<AnalysisStrategy> strategyList = new List<AnalysisStrategy>();
            List<string> fileList = this.getXMLFileList(workFolder + strategyFileFoder);

            if (fileList.Count > 0)
            {
                foreach (string file in fileList)
                {
                    try
                    {
                        Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        AnalysisStrategy strategy = analysisStrategySerializer.Deserialize(stream) as AnalysisStrategy;
                        // we need to reload the variable relation in strategy 
                        if (this.project != null)
                        {
                            strategy.updateData(this.project);
                        }

                        strategyList.Add(strategy);
                        stream.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.StackTrace);
                    }
                }

            }
            else
            {  // configure file do not exsit ,just create and use the default strategy
                this.project.setDefaultStrategyList();
                this.XMLSerializeAnalysisStrategyList(this.project.StrategyList);
                strategyList = this.project.StrategyList;
            }

            return strategyList;
        }



        public List<string> getXMLFileList(string path)
        {
            string[] allFileList = Directory.GetFiles(path);
            List<string> fileList = new List<string>();
            foreach (string file in allFileList)
            {
                if (file.EndsWith(".xml"))
                {
                    fileList.Add(file);
                }
            }

            return fileList;
        }

        public List<string> getStrategyXMLFileList()
        {
            string[] allFileList = Directory.GetFiles(workFolder + strategyFileFoder);
            List<string> fileList = new List<string>();
            foreach (string file in allFileList)
            {
                if (file.EndsWith(".xml"))
                {
                    fileList.Add(file);
                }
            }

            return fileList;
        }


        public void updateStrategyGalleryItem(int index, string title, string descprition)
        {
            this.mainForm.updateStrategyItemCaption(index, title, descprition);
        }

    }
}

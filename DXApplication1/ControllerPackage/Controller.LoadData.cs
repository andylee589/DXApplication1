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
using System.Runtime.InteropServices;
namespace DXApplication1
{
   partial class Controller
    {
        public bool isNullRow(Excel.Range range, int rowIndex)
        {
            // range rows index start with  1 base not 0 
            object rowObj = range.Rows[rowIndex];
            // use reflection to get the cell obj array
            object[,] cellObjs = (object[,])rowObj.GetType().InvokeMember("Value2", System.Reflection.BindingFlags.GetProperty, null, rowObj, new object[] { });

            bool isNullRow = true;
            foreach (object oj in cellObjs)
            {
                if (oj != null)
                {
                    isNullRow = false;
                    break;
                }
            }
            return isNullRow;
        }

        public int getNonBlankRowCount(Excel.Range range)
        {
            int rowCount = range.Rows.Count;

            bool isNull = isNullRow(range, rowCount);
            while (isNull)
            {
                rowCount--;
                isNull = isNullRow(range, rowCount);
            }
            return rowCount;
        }

        public bool loadData(string filePath, string sheetName)
        {

            project = new Project();
            app = new Excel.Application();
            workbook = app.Workbooks.Open(filePath);
            sheets = workbook.Sheets;
            prioritySheet = sheets[1] as Excel.Worksheet;
            range = prioritySheet.UsedRange;
            int rowCount = 0;
            int colCount = 0;
            try
            {
                if (checkSheetName(prioritySheet) && checkSheetRange(range))
                {
                    rowCount = getNonBlankRowCount(range);
                    colCount = range.Columns.Count;
                    if (checkRowCount(rowCount))
                    {
                        Excel.Range projectFlagCell = range.Cells[2, 1] as Excel.Range;
                        string projectFlag = projectFlagCell.Value2 as string;
                        //get project name
                        Excel.Range projectNameCell = range.Cells[2, 2] as Excel.Range;
                        string projectName = projectNameCell.Value2 as string;
                        // set as project Name
                        project.Name = projectName;
                       
                        //get variables
                        //check the first variable 
                        Excel.Range variableFlagCell = range.Cells[3, 1] as Excel.Range;
                        string variableFlag = variableFlagCell.Value2 as string;
                        if (checkProjectFlag(projectFlag)&&checkProjectName(projectName)&& checkVariableFlag(variableFlag))
                        {
                            Dictionary<string, Variable> variableDict = processExcelData(range, rowCount);
                            if (checkVariableDict(variableDict))
                            {
                                // set up project variable list
                                project.Variables = variableDict;
                                project.variablesTypeSorting();
                                return true;
                            }
                        }
                    }
                }
            }
            finally
            {
                releaseResource();
            }
            return false;
        }


        private void releaseResource()
        {
            if (range != null)
            {
                Marshal.FinalReleaseComObject(range);
            }
            if (prioritySheet != null)
            {
                Marshal.FinalReleaseComObject(prioritySheet);
            }
            if (testSetSheet != null)
            {
                Marshal.FinalReleaseComObject(testSetSheet);
            }
            if (coverageSheet != null)
            {
                Marshal.FinalReleaseComObject(coverageSheet);
            }
            if (sheets != null)
            {
                Marshal.FinalReleaseComObject(sheets);
            }
            if (workbook != null)
            {
                workbook.Close();
                Marshal.FinalReleaseComObject(workbook);
            }
            app.Quit();
            Marshal.FinalReleaseComObject(app);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private bool checkSheetName(Excel.Worksheet sheet)
        {
            if (!sheet.Name.Equals(StaticResource.prioritySheetNameStr))
            {
                return showErrorMessage("WorkSheet has wrong name,Name should be 'Priority'!");            
            }
           return true;
        }

        private bool checkSheetRange(Excel.Range range)
        {
            if (range == null)
            {
                return showErrorMessage("The open sheet don't have any content!");
            }  
            return true;
        }

        private bool checkRowCount(int rowCount)
        {
            if (rowCount <= 2)
            {
                 return showErrorMessage("The open sheet has wrong content,Can't get Project Name!");
            }
            return true;
        }

        private bool checkProjectFlag(string projectFlag)
        {
            if (!projectFlag.Equals(StaticResource.projectNameFlagStr))
            {
                return showErrorMessage("There Cell in A2 should be 'Project Name'");
            }
            return true;
        }

        private bool checkProjectName(string projectName)
        {
            if (projectName == null)
            {
                return showErrorMessage("There Cell in B2 should has content as Project Name");
            }
            return true;
        }

        private bool checkVariableFlag(string variableFlag)
        {
            if (!variableFlag.Equals(StaticResource.variableNameFlagStr))
            {
                return showErrorMessage("The cell in A3 should be 'Variable Name'");
            }
            return true;
        }

        private bool checkVariableDict(Dictionary<string, Variable> variableDict)
        {
            if (variableDict == null)
            {
                return false;
            }
            return true;
        }

        private bool checkVariableProperty(string variableName, string typeName, VariableType? type,int index)
        {
            if (variableName != null && typeName != null && type != null)
            {
                return true;
            }
            else
            {                
                return showErrorMessage("Has error variable name or variable type in row" + index);;
            }

        }
       
       private Dictionary<string, Variable>  processExcelData(Excel.Range range, int rowCount)
        {
            Dictionary<string, Variable> variableDict = new Dictionary<string, Variable>();
            // lookout the row count not sure
            int factorIndex = 2;
            int platformIndex = 4;
            int platformCount = 0;
            bool isFirstFactor = true;
            bool isPriorityColorSet = false;
            for (int index = 2; index < rowCount; index++)
            {
                //update the row index
                index = platformIndex - 2;
                isFirstFactor = true;
                Variable tempVariable = new Variable();
                Excel.Range variableNameCell = range.Cells[index + 1, 2] as Excel.Range;
                string variableName = variableNameCell.Value2 as string;
                Excel.Range typeNameCell = range.Cells[index + 1, 4] as Excel.Range;
                string typeName = typeNameCell.Value2 as string;
                VariableType? type = null;
                if (typeName.Equals(StaticResource.VariableTypeDict[VariableType.SINGLE_FACTOR]))
                {
                    type = VariableType.SINGLE_FACTOR;
                }
                else if (typeName.Equals(StaticResource.VariableTypeDict[VariableType.MULTI_FACTOR]))
                {
                    type = VariableType.MULTI_FACTOR;
                }
                if (!checkVariableProperty(variableName, typeName, type,index))
                {
                    return null;
                }
                    tempVariable.Name = variableName;
                    tempVariable.Type = type;
                    // read the matrix in each  variable

                    Excel.Range factorCell = range.Cells[index + 2, factorIndex + 1] as Excel.Range;
                    string factorStr = factorCell.Value2 as string;
                    // check the first column of matrix is filled
                    if (factorStr == null)
                    {
                         showErrorMessage("The cell C" + (index + 2) + " shouldn't be empty");
                         return null;
                    }

                    //all the language string in this varabile;
                    List<string> factorList = new List<string>();
                    while (factorStr != null)
                    {
                        factorList.Add(factorStr);
                        Excel.Range platformCell = range.Cells[platformIndex + 1, 2] as Excel.Range;
                        string platformStr = platformCell.Value2 as string;
                        //detect whether the cell is next variable
                        Excel.Range detectCell = range.Cells[platformIndex + 1, 1] as Excel.Range;
                        string detectStr = detectCell.Value2 as string;

                        //check the first row of the matrix is filled
                        if (platformStr == null || detectStr != null)
                        {
                             showErrorMessage("The cell B" + (platformIndex + 1) + " shouldn't be empty");
                             return null;
                        }
                        //reset itemCount to zero
                        platformCount = 0;
                        // all the platform string 
                        List<string> platformList = new List<string>();
                        while (platformStr != null && detectStr == null)
                        {
                            //update platform string
                            platformCell = range.Cells[platformIndex + 1, 2] as Excel.Range;
                            platformStr = platformCell.Value2 as string;
                            // collect the platform string 
                            if (isFirstFactor && platformStr != null)
                            {
                                platformList.Add(platformStr);
                            }
                            // get cell information
                            Excel.Range priorityCell = range.Cells[platformIndex + 1, factorIndex + 1] as Excel.Range;
                            int priorityValue = Convert.ToInt32(priorityCell.Value2 as object);
                            if (priorityValue > 0 && platformStr != null)
                            {
                                Priority priority = new Priority();
                                priority.Value = priorityValue;
                                if (!isPriorityColorSet)
                                {
                                    setPriorityColor(priorityCell);
                                    isPriorityColorSet = true;
                                }

                                tempVariable.setup(factorStr, platformStr, priority);
                                //count the platform number in each variable
                                platformCount++;

                            }
                            else if (platformIndex < rowCount)
                            {
                                 showErrorMessage("The cell " + (platformIndex + 1) + ", " + (factorIndex + 1) + "and the cell" + (1 + platformIndex) + ", 2 shouldn't be empty");
                                 return null;
                            }

                            // get next platform information
                            platformIndex++;
                            detectCell = range.Cells[platformIndex + 1, 1] as Excel.Range;
                            detectStr = detectCell.Value2 as string;
                        }
                        if (platformIndex < rowCount && !detectStr.Equals(StaticResource.variableNameFlagStr))
                        {
                            showErrorMessage("format error ! should be 'Variable Name' in  cell" + (platformIndex + 1) + " " + (factorIndex + 1));
                            return null;
                        }

                        if (isFirstFactor)
                        {
                            isFirstFactor = false;
                            // set variable platform list
                            tempVariable.PlatformList = platformList;
                        }
                        // get next language information
                        factorIndex++;
                        factorCell = range.Cells[index + 2, factorIndex + 1] as Excel.Range;
                        factorStr = factorCell.Value2 as string;

                        //reset the platformIndex to the origin value
                        platformIndex -= platformCount;
                        //check the variable type if is a language relateless type ,we just need to read one column
                        if (tempVariable.Type == VariableType.SINGLE_FACTOR)
                        {
                            break;
                        }

                    }
                    // adjust the platformIndex to the  next variable 
                    platformIndex += platformCount;
                    //adjust the platformIndex to the next variable 's first platform index
                    platformIndex += 2;
                    //adjust the languageIndex to the next variable's first platform index
                    factorIndex = 2;
                    // adjust the row index to the next variable's row
                    index = platformIndex - 2;
                    tempVariable.FactorList = factorList;

                    variableDict.Add(tempVariable.Name, tempVariable);                                       
            }
             return variableDict;
        }




        public void setPriorityColor(Excel.Range priorityCell)
        {
            // get background color and translate to Color object

            int count = priorityCell.FormatConditions.Count;
            for (int i = 1; i <= count; i++)
            {
                Excel.FormatCondition formatObject = priorityCell.FormatConditions.Item(i) as Excel.FormatCondition;
                if (formatObject.Operator == (int)Excel.XlFormatConditionOperator.xlEqual)
                {
                    string formula = formatObject.Formula1;
                    int value = int.Parse(formula.Substring(1, formula.Length - 1));
                    Color filledColor = System.Drawing.ColorTranslator.FromOle(Convert.ToInt32(formatObject.Interior.Color as object));
                    Color fontColor = new Color();
                    object dynamicObj = formatObject.Font.Color;

                    // check DBNull type object 
                    if (!(dynamicObj is DBNull))
                    {
                        fontColor = System.Drawing.ColorTranslator.FromOle(Convert.ToInt32(formatObject.Font.Color as object));
                    }

                    this.project.putPriorityColor(value, filledColor);
                    this.project.putPriorityFontColor(value, fontColor);
                }

            }


        }
    }
}

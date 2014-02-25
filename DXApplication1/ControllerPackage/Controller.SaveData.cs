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
        Excel.Range range;
        Excel.Worksheet prioritySheet;
        Excel.Worksheet testSetSheet;
        Excel.Worksheet coverageSheet;
        Excel.Application app;
        Excel.Workbook workbook;
        Excel.Sheets sheets;
        public bool saveData(string path)
        {
            app = new Excel.Application();
            workbook = app.Workbooks.Open(path);
            sheets = workbook.Sheets;
            try
            {              
                testSetSheet = sheets.Add(Type.Missing,sheets[sheets.Count] as Excel.Worksheet, Type.Missing, Type.Missing) as Excel.Worksheet;
                coverageSheet = sheets.Add(Type.Missing, sheets[sheets.Count] as Excel.Worksheet, Type.Missing, Type.Missing) as Excel.Worksheet;
                saveTestSetToExcel();
                saveCoverageToExcel();
                workbook.Save();
            }
            finally
            {
                releaseResource();
            }
            return true;
        }

        private void saveTestSetToExcel()
        {
            Point titlePoint = new Point(1, 1);
            List<Point> iterationPointList = new List<Point>();
            // change point to range area
            Dictionary<string, string> factorRangeList = new Dictionary<string, string>();
            Dictionary<string, string> columnRangeList = new Dictionary<string, string>();


            testSetSheet.Name = StaticResource.testSetSheetFlagName+testSetSheet.Index ;

            testSetSheet.Cells[titlePoint.X, titlePoint.Y] = StaticResource.testsetSheetSlogonStr;
            // start index of the result;
            int rowIndex = 2;
            int columnFlagIndex=1;
            string iterationName="";
            string columnStartAdd = null;
            string columnEndAdd = null;
            string factorStartAdd = null;
            string factorEndAdd = null;
            foreach (DataRow row in this.mainForm.testSetDataTable.Rows)
            {
                if (!iterationName.Equals(row.ItemArray[0]))
                {

                    if (factorStartAdd != null)
                    {
                        factorEndAdd = (testSetSheet.Cells[rowIndex - 1, columnFlagIndex] as Excel.Range).Address.Replace("$", "");
                        factorRangeList.Add(factorStartAdd, factorEndAdd);
                    }
                    iterationName = row.ItemArray[0] as string ;
                    testSetSheet.Cells[rowIndex, columnFlagIndex] = iterationName;
                    iterationPointList.Add(new Point(rowIndex, columnFlagIndex));
                    rowIndex++;

                    for (int index = 1; index < row.ItemArray.Count(); index++)
                    {
                        testSetSheet.Cells[rowIndex, index] = this.mainForm.testSetDataTable.Columns[index].Caption;
                        // columnPointList.Add(new Point(rowIndex, index));
                        if (index == 1)
                        {
                            columnStartAdd = (testSetSheet.Cells[rowIndex, index] as Excel.Range).Address.Replace("$","");
                            
                        }
                        else if(index==row.ItemArray.Count()-1)
                        {
                            columnEndAdd = (testSetSheet.Cells[rowIndex, index] as Excel.Range).Address.Replace("$", "");
                        }
                    }
                    columnRangeList.Add(columnStartAdd, columnEndAdd);
                    rowIndex++;

                    //  factorPointList.Add(new Point(rowIndex, index));
                    factorStartAdd = (testSetSheet.Cells[rowIndex, columnFlagIndex] as Excel.Range).Address.Replace("$", "");

                }

                for (int index = 1; index < row.ItemArray.Count();index++ )
                {
                    testSetSheet.Cells[rowIndex, index] = row.ItemArray[index];                 
                }
                rowIndex++;
            }
            // add factor end address when the row is in the end
            factorEndAdd = (testSetSheet.Cells[rowIndex - 1, columnFlagIndex] as Excel.Range).Address.Replace("$", "");
            factorRangeList.Add(factorStartAdd, factorEndAdd);

            range = testSetSheet.UsedRange;
            range.Columns.AutoFit();

            // set for iteration points
            foreach (Point point in iterationPointList)
            {
                Excel.Range iterationCell = range.Cells[point.X, point.Y] as Excel.Range;
                iterationCell.Font.Bold =true;
            }

            foreach (KeyValuePair<string, string> kv in columnRangeList)
            {
                string rangStr = kv.Key + ":" + kv.Value;
                Excel.Range columnRange = testSetSheet.Range[rangStr] as Excel.Range;
                columnRange.Font.Bold = true;
                columnRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
                columnRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            }

            foreach (KeyValuePair<string, string> kv in factorRangeList)
            {
                string rangStr = kv.Key + ":" + kv.Value;
                Excel.Range factorRange = testSetSheet.Range[rangStr] as Excel.Range;
                factorRange.Font.Bold = true;
                factorRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
                factorRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            }
        }

      

        private void saveCoverageToExcel()
        {
            Point titlePoint = new Point(1, 1);
            List<Point> varaiblePointList = new List<Point>();
            // change point to range area
            Dictionary<string, string> platformRangeList = new Dictionary<string, string>();
            Dictionary<string, string> columnRangeList = new Dictionary<string, string>();
            
           // Dictionary<string,string>

            coverageSheet.Name = StaticResource.coverageSheetFlagName + coverageSheet.Index;
            coverageSheet.Cells[titlePoint.X, titlePoint.Y] = StaticResource.coverageSheetSlogonStr;
            // start index of the result;
            int rowIndex = 2;
            int columnFlagIndex = 1;
            string variableName = "";
            string columnStartAdd = null;
            string columnEndAdd = null;
            string platformStartAdd = null;
            string platformEndAdd = null;

            foreach (DataRow row in this.mainForm.coverageDataTable.Rows)
            {
                if (!variableName.Equals(row.ItemArray[1]))
                {
                    if (platformStartAdd != null)
                    {
                        platformEndAdd = (coverageSheet.Cells[rowIndex - 1, columnFlagIndex] as Excel.Range).Address.Replace("$", "");
                        platformRangeList.Add(platformStartAdd, platformEndAdd);
                    }

                    variableName = row.ItemArray[1] as string;
                    coverageSheet.Cells[rowIndex, columnFlagIndex] = variableName;
                    varaiblePointList.Add(new Point(rowIndex, columnFlagIndex));
                    rowIndex++;
                    for (int index = 1; index < row.ItemArray.Count()-1; index++)
                    {
                        coverageSheet.Cells[rowIndex, index] = this.mainForm.coverageDataTable.Columns[index+1].Caption;
                        if (index == 1)
                        {
                            columnStartAdd = (coverageSheet.Cells[rowIndex, index] as Excel.Range).Address.Replace("$", "");

                        }
                        else if (index == row.ItemArray.Count() - 2)
                        {
                            columnEndAdd = (coverageSheet.Cells[rowIndex, index] as Excel.Range).Address.Replace("$", "");
                        }
                    }
                    columnRangeList.Add(columnStartAdd, columnEndAdd);
                    rowIndex++;
                    //  factorPointList.Add(new Point(rowIndex, index));
                    platformStartAdd = (coverageSheet.Cells[rowIndex, columnFlagIndex] as Excel.Range).Address.Replace("$", "");
                }

                for (int index = 1; index < row.ItemArray.Count()-1; index++)
                {
                    coverageSheet.Cells[rowIndex, index] = row.ItemArray[index + 1];               
                }              
                rowIndex++;
            }
            // add factor end address when the row is in the end
            platformEndAdd = (coverageSheet.Cells[rowIndex - 1, columnFlagIndex] as Excel.Range).Address.Replace("$", "");
            platformRangeList.Add(platformStartAdd, platformEndAdd);

            range = coverageSheet.UsedRange;
            range.Columns.AutoFit();

            foreach (Point point in varaiblePointList)
            {
                Excel.Range iterationCell = range.Cells[point.X, point.Y] as Excel.Range;
                iterationCell.Font.Bold = true;
            }

            foreach (KeyValuePair<string, string> kv in columnRangeList)
            {
                string rangStr = kv.Key + ":" + kv.Value;
                Excel.Range columnRange = coverageSheet.Range[rangStr] as Excel.Range;
                columnRange.Font.Bold = true;
                columnRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
                columnRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            }

            //foreach (KeyValuePair<string, string> kv in platformRangeList)
            //{
            //    string rangStr = kv.Key + ":" + kv.Value;
            //    Excel.Range factorRange = coverageSheet.Range[rangStr] as Excel.Range;
            //    factorRange.Font.Bold = true;
            //    factorRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SteelBlue);
            //    factorRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            //}

        }

    }
}

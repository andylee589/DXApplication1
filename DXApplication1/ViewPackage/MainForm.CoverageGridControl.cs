using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraGrid.Views.Grid;
using DXApplication1.ViewPackage;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraCharts;
using System.Threading;


namespace DXApplication1
{
    public partial class MainForm : RibbonForm
    {
       public  DataTable coverageDataTable { get; set; }


        #region present coverage grid data function
        private void presentDataInCoverageGrid()
        {
            if (!this.initCoverageGridData())
            {
                MessageDxUnit.ShowWarning("initialize the coverage grid failed");
            }
        }

        public bool initCoverageGridData()
        {

            List<string> columnList = getCoverageColoumnList();
            List<List<string>> setList = getCoverageRowList();
            coverageDataTable = createTable(columnList, setList);
            this.coverateGridControl.DataSource = coverageDataTable;
            coverageGridView.Columns.Clear();
            for (int index = 0; index < coverageDataTable.Columns.Count; index++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = coverageGridView.Columns.AddVisible(coverageDataTable.Columns[index].ColumnName);
            }

            // set up certain column as group 
            coverageGridView.BeginSort();
            try
            {
                this.coverageGridView.ClearGrouping();
                this.coverageGridView.Columns[StaticResource.coverageVariableTypeStr].GroupIndex = 0;
                this.coverageGridView.Columns[StaticResource.coverageVariableStr].GroupIndex = 1;
            }
            finally
            {
                this.coverageGridView.EndSort();
                this.coverageGridView.OptionsView.ShowGroupPanel = true;
                this.coverageGridView.GroupPanelText = "Project Name : " + controller.project.Name;
                this.coverageGridView.BestFitColumns();
            }
            return true;
        }



        private List<string> getCoverageColoumnList()
        {
            List<string> columnList = new List<string>();
            string variableType = StaticResource.coverageVariableTypeStr;
            string variableName = StaticResource.coverageVariableStr;
            string platformName = StaticResource.coveragePlatformStr;
            columnList.Add(variableType);
            columnList.Add(variableName);
            columnList.Add(platformName);
            Variable fadeVariable = this.controller.project.Variables[this.controller.project.MultiFactorVariableNameList[0]];
            foreach (string languageName in fadeVariable.FactorList)
            {
                columnList.Add(languageName);
            }
            string sumName = StaticResource.coverageSumStr;
            columnList.Add(sumName);
            return columnList;
        }

        private List<List<string>> getCoverageRowList()
        {
            List<List<string>> setList = new List<List<string>>();
            int factorColumnIndex = 1;
            for (int index = 0; index < testSetDataTable.Columns.Count; index++)
            {
                string columnName = testSetDataTable.Columns[index].ColumnName;
                if (columnName.Equals(StaticResource.testSetFactorStr))
                {
                    factorColumnIndex = index;
                }
                //columnName is a variable name
                if (this.controller.project.Variables.ContainsKey(columnName))
                {
                    //List<string> variable
                    Variable variable = this.controller.project.Variables[columnName];
                    foreach (string platformName in variable.PlatformList)
                    {
                        List<string> platformRecord = getPlatformCoverage(columnName, platformName, index, factorColumnIndex);
                        setList.Add(platformRecord);
                    }
                }
            }
            return setList;
        }


        private List<string> getPlatformCoverage(string variableName, string platformName, int varaibleColumnIndex, int lanuageCoulumnIndex)
        {
            List<string> record = new List<string>();
            string varaibleType = null;
            if (this.controller.project.PrimaryMultiFactorList.Contains(variableName))
            {
                varaibleType = StaticResource.coveragePrimaryMultiTypeStr;
            }
            else if (this.controller.project.PrimarySingleFactorList.Contains(variableName))
            {
                varaibleType =StaticResource.coveragePrimaryMultiTypeStr;
            }
            else if (this.controller.project.SecondaryMultiFactorList.Contains(variableName))
            {
                varaibleType = StaticResource.coverageSecondaryMultiTypeStr;
            }
            else if (this.controller.project.SecondarySingleFactorList.Contains(variableName))
            {
                varaibleType = StaticResource.coverageSecondarySingleTypeStr;
            }
            record.Add(varaibleType);
            record.Add(variableName);
            record.Add(platformName);
            Variable fadeVariable = this.controller.project.Variables[this.controller.project.MultiFactorVariableNameList[0]];
            int sum = 0;
            foreach (string languageName in fadeVariable.FactorList)
            {
                int times = getPlatformInFactorCoverage(variableName, platformName, languageName, varaibleColumnIndex, lanuageCoulumnIndex);
                record.Add(times.ToString());
                sum += times;
            }
            record.Add(sum.ToString());
            return record;
        }


        private int getPlatformInFactorCoverage(string variableName, string platformName, string factorName, int variableColumnIndex, int factorColumnIndex)
        {
            int times = 0;
            foreach (DataRow row in testSetDataTable.Rows)
            {
                if (row[factorColumnIndex].Equals(factorName) && row[variableColumnIndex].Equals(platformName))
                {
                    times++;
                }

            }

            return times;
        }
 #endregion

        #region  coverage grid event handler function
        // set row cell style mainly color
        private void coverageGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            string fadeVariableName = this.controller.project.MultiFactorVariableNameList[0];
            Variable fadeVariable = this.controller.project.Variables[fadeVariableName];
            List<string> langList = fadeVariable.FactorList;
            if (langList.Contains(e.Column.FieldName))
            {
                Dictionary<int, Color> priorityColorDict = this.controller.project.PriorityColorDict;
                Dictionary<int, Color> priorityFontColorDict = this.controller.project.PriorityFontColorDict;
                int variableColumnIndex = 1;
                int platformColumnIndex = 2;
                string variableName = coverageDataTable.Rows[e.RowHandle][variableColumnIndex] as string;
                string platformName = coverageDataTable.Rows[e.RowHandle][platformColumnIndex] as string;
                string languageName = coverageDataTable.Columns[e.Column.ColumnHandle].ColumnName;
                if (this.controller.project.MultiFactorVariableNameList.Contains(variableName))
                {
                    int priority = this.controller.project.Variables[variableName].PlatformPriorityMatrix[languageName][platformName].Value;
                    e.Appearance.BackColor = priorityColorDict[priority];
                    e.Appearance.ForeColor = priorityFontColorDict[priority];
                }
            }
        }


        private void coverageItem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            coverageGridviewShow();
        }

        public void coverageGridviewShow()
        {
            this.coverageSplitContainerControl.Visible = true;
            this.priorityGridControl.Visible = false;
            this.testsetGridControl.Visible = false;
        }
 #endregion

        #region coverage splitcontainer event handler function
        private void coverageSplitContainerControl1_SizeChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.SplitContainerControl control = sender as DevExpress.XtraEditors.SplitContainerControl;
            int width = control.Size.Width;
            control.SplitterPosition = width / 2;

        }

        private void coverageSplitContainerControl1_SplitterPositionChanged(object sender, EventArgs e)
        {
            this.coverageGridView.BestFitColumns();
        } 
        #endregion

    }
}

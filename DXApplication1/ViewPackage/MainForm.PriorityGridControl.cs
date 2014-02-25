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
        #region initial function
        void InitPriorityGridProperty()
        {
            // initial configure for gridview1
            this.priorityGridView.RowCellStyle += this.priorityGridView_RowCellStyle;
            this.priorityGridView.OptionsView.ShowGroupPanel = true;
            this.priorityGridView.GroupPanelText = "Please Load Your Excel Data";
            this.priorityGridView.OptionsBehavior.Editable = false;
            // initial configure for gridview2
            this.testSetGridView.OptionsView.ShowGroupPanel = true;
            this.testSetGridView.GroupPanelText = "Please Apply Analysis Strategy To Generate TestSet";
            this.coverageGridView.OptionsView.ShowGroupPanel = true;
            this.coverageGridView.GroupPanelText = "Please Check Coverage After TestSet Generated";
        }
        #endregion


        #region present priority grid data function

        //present the data in data grid
        public void presentDataInPriorityGrid()
        {
            if (!this.initPriorityGridData())
            {
                MessageDxUnit.ShowWarning("initialize the priority grid failed");
            }
        }


        public bool initPriorityGridData()
        {
            if (controller.project == null)
            {
                controller.showErrorMessage("Data initialize error!");
            }

           // BindingList<PriorityRecord> recordList = controller.project.getPriorityRecords();
           //this.priorityGridControl.DataSource = recordList;
            DataTable priorityDataTable = controller.project.getPriorityDataTable();
            this.priorityGridControl.DataSource = priorityDataTable;

            // set up certain column as group 
            priorityGridView.BeginSort();
            try
            {
                this.priorityGridView.ClearGrouping();
                this.priorityGridView.Columns[StaticResource.varialbeTypeStr].GroupIndex = 0;
                this.priorityGridView.Columns[StaticResource.variableStr].GroupIndex = 1;
            }
            finally
            {
                this.priorityGridView.EndSort();
                this.priorityGridView.OptionsView.ShowGroupPanel = true;
                this.priorityGridView.GroupPanelText = "Project Name : " + controller.project.Name;
                this.priorityGridView.BestFitColumns();
            }


            return true;
        }
#endregion

        #region priority grid event handler function
        // set row cell style mainly color
        private void priorityGridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            string fadeVariableName = this.controller.project.MultiFactorVariableNameList[0];
            Variable fadeVariable = this.controller.project.Variables[fadeVariableName];
            List<string> factorList = fadeVariable.FactorList;
            if (factorList.Contains(e.Column.FieldName)||e.Column.FieldName.Contains(" / "))
            {
                Dictionary<int, Color> priorityColorDict = this.controller.project.PriorityColorDict;
                Dictionary<int, Color> priorityFontColorDict = this.controller.project.PriorityFontColorDict;
                foreach (KeyValuePair<int, Color> kv in priorityColorDict)
                {
                    if (e.CellValue != null)
                    {
                        if (e.CellValue.Equals(Convert.ToString(kv.Key)))
                        {
                            e.Appearance.BackColor = kv.Value;
                            e.Appearance.ForeColor = priorityFontColorDict[kv.Key];
                        }
                    }
                }
            }
        }

        private void gridView_EndGrouping(object sender, EventArgs e)
        {
            GridView gridView = (sender as GridView);
            gridView.ExpandAllGroups();
        }


        private void priority_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            priorityGridviewShow();
        }

        public void priorityGridviewShow()
        {
            this.priorityGridControl.Visible = true;
            this.testsetGridControl.Visible = false;
            this.coverageSplitContainerControl.Visible = false;
        }

        private void priorityGridControl_SizeChanged(object sender, EventArgs e)
        {
            this.priorityGridView.BestFitColumns();
        } 
        #endregion
    
    }
}

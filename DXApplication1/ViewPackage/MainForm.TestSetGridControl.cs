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
       public  DataTable testSetDataTable { get; set; }
        bool isTestSetGenerated = false;

        #region present testset grid data function

        public void presentDataInTestSetGrid(List<Dictionary<string, Dictionary<string, string>>> testSet)
        {
            if (!this.initTestSetGridData(testSet))
            {
                MessageDxUnit.ShowWarning("initialize the test set grid failed");
            }
            presentDataInCoverageGrid();
        }

        public bool initTestSetGridData(List<Dictionary<string, Dictionary<string, string>>> testSet)
        {
            if (testSet == null)
            {
                return false;
            }

            List<string> columnList = getTestSetColoumnList(testSet);
            List<List<string>> setList = getTestSetRowList(testSet);


            testSetDataTable = createTable(columnList, setList);
            this.testsetGridControl.DataSource = testSetDataTable;
            testSetGridView.Columns.Clear();
            for (int index = 0; index < testSetDataTable.Columns.Count; index++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = testSetGridView.Columns.AddVisible(testSetDataTable.Columns[index].ColumnName);

                string variableName = testSetDataTable.Columns[index].ColumnName;
                if (this.controller.project.Variables.ContainsKey(variableName))
                {

                    col.ColumnEdit = getLookUpEdit(variableName);
                }
            }

            // set up certain column as group 
            testSetGridView.BeginSort();
            try
            {
                this.testSetGridView.ClearGrouping();
                this.testSetGridView.Columns[StaticResource.testSetIterationStr].GroupIndex = 0;
            }
            finally
            {
                this.testSetGridView.EndSort();
                this.testSetGridView.OptionsView.ShowGroupPanel = true;
                this.testSetGridView.GroupPanelText = "Project Name : " + controller.project.Name;
                this.testSetGridView.BestFitColumns();
            }


            return true;
        }

        private List<string> getTestSetColoumnList(List<Dictionary<string, Dictionary<string, string>>> testSet)
        {
            List<string> columnList = new List<string>();
            columnList.Add(StaticResource.testSetIterationStr);
            columnList.Add(StaticResource.testSetFactorStr);
            foreach (KeyValuePair<string, Dictionary<string, string>> kv in testSet[0])
            {
                // get the first language and then initialize the column filed
                foreach (KeyValuePair<string, string> kv1 in kv.Value)
                {
                    columnList.Add(kv1.Key);
                }
                break;
            }

            return columnList;
        }


        private List<List<string>> getTestSetRowList(List<Dictionary<string, Dictionary<string, string>>> testSet)
        {
            List<List<string>> setList = new List<List<string>>();
            for (int iteratioanIndex = 1; iteratioanIndex <= testSet.Count; iteratioanIndex++)
            {
                string iterationName = StaticResource.testSetIterationStr + iteratioanIndex;

                foreach (KeyValuePair<string, Dictionary<string, string>> kv in testSet[iteratioanIndex - 1])
                {
                    List<string> tempList = new List<string>();
                    string lanuageName = kv.Key;
                    tempList.Add(iterationName);
                    tempList.Add(lanuageName);
                    foreach (KeyValuePair<string, string> kv1 in kv.Value)
                    {
                        string selectItem = kv1.Value;
                        tempList.Add(selectItem);
                    }
                    setList.Add(tempList);
                }
            }
            return setList;
        }

        private RepositoryItemLookUpEdit getLookUpEdit(string variableName)
        {
            RepositoryItemLookUpEdit lookUpEdit = new RepositoryItemLookUpEdit();
            DataTable tempData = new DataTable();
            tempData.Columns.Add(variableName, variableName.GetType());
            List<string> platformList = this.controller.project.Variables[variableName].PlatformList;
            foreach (string platformName in platformList)
            {
                tempData.Rows.Add(new object[1] { platformName });
            }
            lookUpEdit.ValueMember = variableName;
            lookUpEdit.DisplayMember = variableName;
            lookUpEdit.DataSource = tempData;
            return lookUpEdit;
        }

        private DataTable createTable(List<string> columnList, List<List<string>> setList)
        {
            DataTable dataTable = new DataTable();
            foreach (string columnName in columnList)
            {
                dataTable.Columns.Add(columnName, columnName.GetType());
            }

            foreach (List<string> rowRecord in setList)
            {
                object[] objectArray = rowRecord.ToArray();
                dataTable.Rows.Add(objectArray);
            }

            return dataTable;
        }
#endregion


        #region testSet grid event handler function

        private void testSetGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            presentDataInCoverageGrid();
        }

        
        private void testSetItem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            testSetGridviewShow();
        }

        public void testSetGridviewShow()
        {
            this.testsetGridControl.Visible = true;
            this.priorityGridControl.Visible = false;
            this.coverageSplitContainerControl.Visible = false;
        }

        private void testsetGridControl_SizeChanged(object sender, EventArgs e)
        {
            this.testSetGridView.BestFitColumns();
        }
        #endregion

    }
}

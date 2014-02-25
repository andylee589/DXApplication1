using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Repository;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.Text.RegularExpressions;

namespace DXApplication1.ViewPackage
{
    public partial class AnalysisStrategyOptionForm : DevExpress.XtraEditors.XtraForm
    {
        DataTable factorDataTable;
        #region initial function
        private void InitFactorGridView()
        {
            AutoAssignmentPlatformWeightSetMethod autoSet = this.Strategy.PlatformWeightSetMethodDict[typeof(AutoAssignmentPlatformWeightSetMethod).FullName] as AutoAssignmentPlatformWeightSetMethod;       
  
            loadFactorPriorityData(autoSet.FactorPrioritySetMethod.Value.FactorPriorityDict);
        } 
        #endregion

        #region factor priority radio group event handler function
        private void factorPriorityRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            int index = radioGroup.SelectedIndex;
            AutoAssignmentPlatformWeightSetMethod autoSet = this.Strategy.PlatformWeightSetMethod.Value as AutoAssignmentPlatformWeightSetMethod;
            Type factorPriorityType = StrategyOptionMapping.FactorPriorityIndexDict[index];
            autoSet.FactorPrioritySetMethod.Value = autoSet.FactorPrioritySetMethodDict[factorPriorityType.FullName];

            if (index == StrategyOptionMapping.FactorPriorityTypeDict[StrategyOptionMapping.equalFactorPriorityTypeName])
            {
                this.groupBox7.Enabled = false;
            }
            else if (index == StrategyOptionMapping.FactorPriorityTypeDict[StrategyOptionMapping.customFactorPriorotyTypeName])
            {
                this.groupBox7.Enabled = true;
                loadFactorPriorityData(autoSet.FactorPrioritySetMethod.Value.FactorPriorityDict);
            }
        }


        private void loadFactorPriorityData(SerializableDictionary<string, int> factorPriorityDict)
        {
            List<string> columnList = getFactorColoumnList();
            List<List<string>> setList = getFactorRowList(factorPriorityDict);
            factorDataTable= createTable(columnList, setList);
            this.factorPriorityGridControl.DataSource = factorDataTable;
            this.factorGridView.Columns.Clear();
            for (int index = 0; index < factorDataTable.Columns.Count; index++)
            {
                DevExpress.XtraGrid.Columns.GridColumn col = factorGridView.Columns.AddVisible(factorDataTable.Columns[index].ColumnName);
                
                if (index==1)
                {
                    col.ColumnEdit = getLookUpEdit();
                }
                this.factorGridView.Columns[index].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
            this.factorGridView.BestFitColumns();
        }

        private List<string> getFactorColoumnList()
        {
            List<string> columnList = new List<string>();
            columnList.Add(StaticResource.factorNameStr);
            columnList.Add(StaticResource.factorPriorityStr);
            return columnList;
        }

        private List<List<string>> getFactorRowList(SerializableDictionary<string, int> factorPriorityDict)
        {
            List<List<string>> setList = new List<List<string>>();
            foreach (KeyValuePair<string, int> kv in factorPriorityDict)
            {
                List<string> tempList = new List<string>();
                tempList.Add(kv.Key);
                tempList.Add(kv.Value.ToString());
                setList.Add(tempList);
            }             
            return setList;
        }

        private RepositoryItemLookUpEdit getLookUpEdit()
        {
            RepositoryItemLookUpEdit lookUpEdit = new RepositoryItemLookUpEdit();         
            DataTable tempData = new DataTable();
            string factorPriorityName = StaticResource.factorPriorityStr;
            tempData.Columns.Add(factorPriorityName, factorPriorityName.GetType());
            List<int> priorityList = new List<int>();
            foreach (KeyValuePair<int, Color> kv in this.controller.project.PriorityColorDict)
            {
                priorityList.Add(kv.Key);
               
            }
            priorityList.Reverse();
            foreach(int priority in priorityList)
            {
               tempData.Rows.Add(new object[1] { priority});
            }
            lookUpEdit.ValueMember = factorPriorityName;
            lookUpEdit.DisplayMember = factorPriorityName;
            lookUpEdit.DataSource = tempData;
            lookUpEdit.BestFitWidth = 70;
            lookUpEdit.BestFit();
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

        private void factorGridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;
            if (view.FocusedColumn.FieldName.Equals(StaticResource.factorNameStr))
            {
                e.Cancel = true;
            }
        }

        private void factorGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (e.Column.FieldName.Equals(StaticResource.factorPriorityStr))
            {
                string cellValue = e.Value.ToString();
                int priority = int.Parse(cellValue);
                string factorName = gridView.GetRowCellValue(e.RowHandle, gridView.Columns[StaticResource.factorNameStr]) as string;
                AutoAssignmentPlatformWeightSetMethod autoSet = this.Strategy.PlatformWeightSetMethod.Value as AutoAssignmentPlatformWeightSetMethod;
                autoSet.FactorPrioritySetMethod.Value.FactorPriorityDict[factorName] = priority;                
            }
        }


        #endregion



    }
}

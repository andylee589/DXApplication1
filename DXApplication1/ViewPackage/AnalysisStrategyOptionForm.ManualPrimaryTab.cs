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

        #region primary variable listbox event handler function
        private void primaryVariableListBoxControl_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            ListBoxControl listbox = sender as ListBoxControl;
            if (listbox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }
        }

        private void primaryPrimaryVariableListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int selectIndex = listBoxControl.SelectedIndex;
            string temVariableStr = listBoxControl.SelectedItem as string;
            if (selectIndex >= 0 && listBoxControl.Focused)
            {
                ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                SerializableDictionary<string, SerializableDictionary<string, double>> primaryWeightDict = manualSet.PrimaryVariableWeightDict;
                this.primaryPrimaryWeightGridControl.DataSource = this.getItemWeightRecordList(primaryWeightDict[temVariableStr]);
                this.setPrimaryGridView();
            }
        }

        public BindingList<ItemWeightRecord> getItemWeightRecordList(Dictionary<string, double> dict)
        {
            BindingList<ItemWeightRecord> bindingList = new BindingList<ItemWeightRecord>();
            foreach (KeyValuePair<string, double> kv in dict)
            {
                bindingList.Add(new ItemWeightRecord(kv.Key, kv.Value));
            }
            return bindingList;
        }

        private void setPrimaryGridView()
        {
            this.primaryGridView.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.primaryGridView.Columns[0].MaxWidth = 240;
            this.primaryGridView.Columns[1].MinWidth = 50;

            this.primaryGridView.BestFitColumns();
            this.primaryGridView.Columns[1].ColumnEdit = new MyRepositoryItemSpintEdit();

            this.primaryGridView.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.primaryGridView.Columns[1].SummaryItem.DisplayFormat = "SUM ={0:n2}";

        }

        private void primaryPrimaryVariableListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            ListBoxControl listbox = sender as ListBoxControl;
            listbox.SelectedIndex = -1;
        }

        private bool primaryPrimaryVariableListBoxControl_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            return checkPrimaryGridViewValid();
        }

        private bool checkPrimaryGridViewValid()
        {
            if (this.primaryGridView.DataSource != null)
            {
                double sumValue = double.Parse(this.primaryGridView.Columns[1].SummaryItem.SummaryValue.ToString());
                if (sumValue > 1)
                {
                    MessageDxUnit.ShowWarning("The current weight sum is " + sumValue + " and shouldn't larger than 1!");
                    return false;
                }
                else if (sumValue < 1)
                {
                    MessageDxUnit.ShowWarning("The current weight sum is " + sumValue + " and shouldn't less than 1!");
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region primary gridview event handler function
        private void primaryGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (e.Column.FieldName.Equals("Weight"))
            {
                string cellValue = e.Value.ToString();
                int selectIndex = primaryPrimaryVariableListBoxControl.SelectedIndex;
                string primaryVariableStr = primaryPrimaryVariableListBoxControl.SelectedItem as string;
                if (selectIndex >= 0)
                {
                    string primayItemStr = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["Item"]) as string;
                    ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                    SerializableDictionary<string, SerializableDictionary<string, double>> primaryWeightDict = manualSet.PrimaryVariableWeightDict;
                    primaryWeightDict[primaryVariableStr][primayItemStr] = (double)e.Value;
                }
            }
        }

        private void primaryPrimaryWeightGridControl_DataSourceChanged(object sender, EventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            if (gridControl.DataSource == null)
            {
                (gridControl.Views[0] as GridView).OptionsView.ShowFooter = false;
                this.primaryRestoreButton.Visible = false;
            }
            else
            {
                (gridControl.Views[0] as GridView).OptionsView.ShowFooter = true;
                this.primaryRestoreButton.Visible = true;
                    
            }
        }

        private void primaryPrimaryWeightGridControl_VisibleChanged(object sender, EventArgs e)
        {
            this.primaryPrimaryWeightGridControl.DataSource = null;
        }

        private void primaryRestoreButton_Click(object sender, EventArgs e)
        {

            BindingList<ItemWeightRecord> recordList = this.primaryPrimaryWeightGridControl.DataSource as BindingList<ItemWeightRecord>;
            int number = recordList.Count;
            double[] itemArray = ManualAssignmentPlatformWeightSetMethod.getAverageWeight(number);
            ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
            SerializableDictionary<string, SerializableDictionary<string, double>> primaryWeightDict = manualSet.PrimaryVariableWeightDict;
            string primaryVariableStr = primaryPrimaryVariableListBoxControl.SelectedItem as string;
            foreach (ItemWeightRecord record in recordList)              
            {
                if (recordList.IndexOf(record)!= recordList.Count - 1)
                {
                    record.Weight = itemArray[0];                    
                }
                else
                {
                    record.Weight = itemArray[number - 1];                   
                }
                string primayItemStr = record.Item;
                primaryWeightDict[primaryVariableStr][primayItemStr] = record.Weight;
                
            }    
            this.primaryPrimaryWeightGridControl.DataSource = recordList;
            this.setPrimaryGridView();
            this.primaryPrimaryWeightGridControl.RefreshDataSource();
        }

        #endregion


        #region gridview event handler function for primary and sencondary
        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            if (view.FocusedColumn.FieldName == "Item")

                e.Cancel = true;
        }

        private void gridView_DataSourceChanged(object sender, EventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView.DataSource == null)
            {
                gridView.OptionsView.ShowFooter = false;
            }
            else
            {
                gridView.OptionsView.ShowFooter = true;
            }
        }
        #endregion


        #region ItemWeightRecord Class for both primary and secondary
        // used for grid to present weight
        public class ItemWeightRecord
        {
            public string Item { get; set; }
            public double Weight { get; set; }

            public ItemWeightRecord(String item, Double weight)
            {
                this.Item = item;
                this.Weight = weight;
            }
        }
        #endregion
       
    }
}

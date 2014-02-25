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

        #region secondary variable radio group part event handler function
        private void secondaryVariableRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            int index = radioGroup.SelectedIndex;
            ManualAssignmentPlatformWeightSetMethod manualSet = this.Strategy.PlatformWeightSetMethod.Value as ManualAssignmentPlatformWeightSetMethod;
            Type secondaryItemType = StrategyOptionMapping.SecondaryItemWeightSetIndexDict[index];
            manualSet.SecondaryItemWeightSetMethod.Value = manualSet.SecondaryItemWeightSetMethodDict[secondaryItemType.FullName];
            if (index == StrategyOptionMapping.SecondaryItemWeightSetTypeDict[StrategyOptionMapping.equalSecondaryItemWeightSetTypeName])
            {
                this.groupBox5.Enabled = false;
            }
            else if (index == StrategyOptionMapping.SecondaryItemWeightSetTypeDict[StrategyOptionMapping.manualSecondaryItemWeightSetTypeName])
            {
                this.groupBox5.Enabled = true;
                loadSeconaryItemWeightData(manualSet.SecondaryItemWeightSetMethod.Value.DependenceRecordDict);
            }
        }

        private void loadSeconaryItemWeightData(Dictionary<VariableDependenceRecordKey, VariableDependenceRecord> dict)
        {
            List<string> dependenceItemStrList = getVariableDependenceItemList(dict);
            this.secondaryVariableDependenceListBoxControl.DataSource = dependenceItemStrList;
        }


        private List<string> getVariableDependenceItemList(Dictionary<VariableDependenceRecordKey, VariableDependenceRecord> dict)
        {
            List<string> dependenceItemStrList = new List<string>();
            Dictionary<VariableDependenceRecordKey, VariableDependenceRecord>.KeyCollection keyCollection = dict.Keys;
            foreach (VariableDependenceRecordKey key in keyCollection)
            {
                dependenceItemStrList.Add(key.PrimaryVariableKey + " --- " + key.SecondaryVariableKey);
            }

            return dependenceItemStrList;
        }
        #endregion

        #region secondary variable dependence Listbox event handler function
        private void secondaryVariableDependenceListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.secondaryVariableDependenceListBoxControl.SelectedIndex = -1;
        }

        private void secondaryVariableDependenceListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int selectIndex = listBoxControl.SelectedIndex;
            string temVariableStr = listBoxControl.SelectedItem as string;
            if (selectIndex >= 0 && listBoxControl.Focused)
            {
                string[] keyArray = Regex.Split(temVariableStr, " --- ", RegexOptions.IgnoreCase);
                if (keyArray.Length == 2)
                {
                    string primaryKey = keyArray[0];
                    string secondaryKey = keyArray[1];
                    ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                    SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> primaryWeightDict = manualSet.SecondaryItemWeightSetMethod.Value.DependenceRecordDict;
                    VariableDependenceRecord record = primaryWeightDict[new VariableDependenceRecordKey(primaryKey, secondaryKey)];
                    List<string> itemStrList = new List<string>(record.ItemDependencDict.Keys);
                    this.secondaryPrimaryVariableListBoxControl.DataSource = itemStrList;
                    this.secondaryPrimaryVariableListBoxControl.SelectedIndex = -1;
                    this.secondarySecondaryVariablGridControl.DataSource = null;
                }
                else
                {
                    throw new Exception("key array length error");
                }
            }
        }

        private void secondaryVariableDependenceListBoxControl_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            ListBoxControl listBox = sender as ListBoxControl;
            if (listBox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }
        }

        #endregion

        #region secondary priamry variable listbox event handler function
        private void secondaryPrimaryVariableListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.secondaryPrimaryVariableListBoxControl.DataSource = null;
        }

        private void secondaryPrimaryVariableListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int selectIndex = listBoxControl.SelectedIndex;
            string primaryItemStr = listBoxControl.SelectedItem as string;
            if (selectIndex >= 0 && listBoxControl.Focused)
            {

                ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> primaryWeightDict = manualSet.SecondaryItemWeightSetMethod.Value.DependenceRecordDict;
                string tempKeyStr = this.secondaryVariableDependenceListBoxControl.SelectedItem as string;
                string[] keyArray = Regex.Split(tempKeyStr, " --- ", RegexOptions.IgnoreCase);
                if (keyArray.Length == 2)
                {
                    string primaryKey = keyArray[0];
                    string secondaryKey = keyArray[1];
                    VariableDependenceRecord record = primaryWeightDict[new VariableDependenceRecordKey(primaryKey, secondaryKey)];
                    SerializableDictionary<string, SecondaryItem> secondaryItemDict = record.ItemDependencDict[primaryItemStr];
                    this.secondarySecondaryVariablGridControl.DataSource = this.getSecondaryItemRecords(secondaryItemDict);
                    this.setSecondaryGridView();
                }
                else
                {
                    throw new Exception("key array length error");
                }
            }
        }


        private BindingList<ItemWeightRecord> getSecondaryItemRecords(Dictionary<string, SecondaryItem> dict)
        {
            BindingList<ItemWeightRecord> secondaryItemRecordList = new BindingList<ItemWeightRecord>();

            foreach (KeyValuePair<string, SecondaryItem> kv in dict)
            {
                secondaryItemRecordList.Add(new ItemWeightRecord(kv.Value.SecondaryItemStr, kv.Value.Weight));
            }
            return secondaryItemRecordList;
        }

        private void setSecondaryGridView()
        {
            this.secondaryGridView.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.secondaryGridView.Columns[0].MaxWidth = 140;
            this.secondaryGridView.Columns[1].MinWidth = 50;

            this.secondaryGridView.BestFitColumns();
            this.secondaryGridView.Columns[1].ColumnEdit = new MyRepositoryItemSpintEdit();


            this.secondaryGridView.Columns[1].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
            this.secondaryGridView.Columns[1].SummaryItem.DisplayFormat = "SUM ={0:n2}";
        }

        private void secondaryPrimaryVariableListBoxControl_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            ListBoxControl listBox = sender as ListBoxControl;
            if (listBox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }
        }

        #endregion

        #region secondary variable grid event handler function in secondary tab
        private void secondarySecondaryVariablGridControl_VisibleChanged(object sender, EventArgs e)
        {
            this.secondarySecondaryVariablGridControl.DataSource = null;
        }

        
        private void secondaryGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (e.Column.FieldName.Equals("Weight"))
            {
                string cellValue = e.Value.ToString();

                int variableSelectIndex = this.secondaryVariableDependenceListBoxControl.SelectedIndex;
                string variableRelationKey = secondaryVariableDependenceListBoxControl.SelectedItem as string;
                if (variableSelectIndex >= 0)
                {

                    string[] keyArray = Regex.Split(variableRelationKey, " --- ", RegexOptions.IgnoreCase);
                    if (keyArray.Length == 2)
                    {
                        string primaryKey = keyArray[0];
                        string secondaryKey = keyArray[1];
                        ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                        SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> secondaryWeightDict = manualSet.SecondaryItemWeightSetMethod.Value.DependenceRecordDict;
                        VariableDependenceRecord record = secondaryWeightDict[new VariableDependenceRecordKey(primaryKey, secondaryKey)];

                        int itemSelectIndex = this.secondaryPrimaryVariableListBoxControl.SelectedIndex;
                        string itemRelationKey = this.secondaryPrimaryVariableListBoxControl.SelectedItem as string;
                        if (itemSelectIndex >= 0)
                        {
                            string secondaryItemStr = gridView.GetRowCellValue(e.RowHandle, gridView.Columns["Item"]) as string;
                            record.ItemDependencDict[itemRelationKey][secondaryItemStr].Weight = (double)e.Value;
                        }
                    }
                    else
                    {
                        throw new Exception("key array length error");
                    }
                }
            }
        }

        private void secondarySecondaryVariablGridControl_DataSourceChanged(object sender, EventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            if (gridControl.DataSource == null)
            {
                (gridControl.Views[0] as GridView).OptionsView.ShowFooter = false;
                this.secondaryRestireButton.Visible = false;
            }
            else
            {
                (gridControl.Views[0] as GridView).OptionsView.ShowFooter = true;
                this.secondaryRestireButton.Visible = true;

            }
        }

        private void secondaryRestoreButton_Click(object sender, EventArgs e)
        {
            BindingList<ItemWeightRecord> recordList = this.secondaryGridView.DataSource as BindingList<ItemWeightRecord>;
            int number = recordList.Count;
            double[] itemArray = ManualAssignmentPlatformWeightSetMethod.getAverageWeight(number);
            int variableSelectIndex = this.secondaryVariableDependenceListBoxControl.SelectedIndex;
            string variableRelationKey = secondaryVariableDependenceListBoxControl.SelectedItem as string;          
            string[] keyArray = Regex.Split(variableRelationKey, " --- ", RegexOptions.IgnoreCase);
            string primaryKey = keyArray[0];
            string secondaryKey = keyArray[1];
            ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
            SerializableDictionary<VariableDependenceRecordKey, VariableDependenceRecord> secondaryWeightDict = manualSet.SecondaryItemWeightSetMethod.Value.DependenceRecordDict;
            VariableDependenceRecord record = secondaryWeightDict[new VariableDependenceRecordKey(primaryKey, secondaryKey)];
            int itemSelectIndex = this.secondaryPrimaryVariableListBoxControl.SelectedIndex;
            string itemRelationKey = this.secondaryPrimaryVariableListBoxControl.SelectedItem as string;


            foreach (ItemWeightRecord itemRecord in recordList)
            {
                if (recordList.IndexOf(itemRecord)!=recordList.Count-1)
                {
                    itemRecord.Weight = itemArray[0];
                }
                else
                {
                    itemRecord.Weight = itemArray[number - 1];
                }              
                string secondaryItemStr = itemRecord.Item;
                record.ItemDependencDict[itemRelationKey][secondaryItemStr].Weight = itemRecord.Weight;
            }
            this.secondarySecondaryVariablGridControl.DataSource = recordList;
            this.setSecondaryGridView();
            this.secondarySecondaryVariablGridControl.RefreshDataSource();
        }

        #endregion
     
        #region listbox event handler function for all listbox in manual secondary tab 
        private void secondaryPublicListBoxControl_MouseMove(object sender, MouseEventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int index = listBoxControl.IndexFromPoint(new Point(e.X, e.Y));
            if (index != -1)
            {
                string item = listBoxControl.GetItem(index) as string;
                toolTipController1.ShowHint(item, listBoxControl.PointToScreen(new Point(e.X, e.Y)));
            }
            else
            {
                toolTipController1.HideHint();
            }
        }

        private void secondaryPublicListBoxControl_MouseLeave(object sender, EventArgs e)
        {
            toolTipController1.HideHint();
        }

        private bool secondaryPublicListBoxControl_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {

            return checkSecondaryGridViewValid();
        }

        private bool checkSecondaryGridViewValid()
        {
            if (this.secondaryGridView.DataSource != null)
            {
                double sumValue = double.Parse(this.secondaryGridView.Columns[1].SummaryItem.SummaryValue.ToString());
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
    }
}

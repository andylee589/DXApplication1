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
using DXApplication1.DataModelPackage;

namespace DXApplication1.ViewPackage
{
    public partial class VariableDependenceOptionForm : DevExpress.XtraEditors.XtraForm
    {



        #region  primary variable listbox event handler function
        private void settingPrimaryVaraibleListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.settingPrimaryVaraibleListBoxControl.SelectedIndex = -1;
        }

        private void settingPrimaryVaraibleListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int selectIndex = listBoxControl.SelectedIndex;
            string temVariableStr = listBoxControl.SelectedItem as string;
            if (selectIndex >= 0)
            {
                Variable temVariable = this.variables[temVariableStr];
                this.settingPrimaryItemListBoxControl.DataSource = temVariable.PlatformList;
                // this.saveCurrentDependenceRecord();
                this.settingPrimaryItemListBoxControl.SelectedIndex = -1;
            }
        }

        private bool settingPrimaryVaraibleListBoxControl_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            this.saveCurrentDependenceRecord();
            return checkDependenceRecordValid();
        }


        private void saveCurrentDependenceRecord()
        {
            if (this.settingPrimaryVaraibleListBoxControl.SelectedIndex >= 0 &&
                this.settingSecondaryVaraibleListBoxControl.SelectedIndex >= 0 &&
                this.settingPrimaryItemListBoxControl.SelectedIndex >= 0)
            {

                string primaryVariableStr = this.settingPrimaryVaraibleListBoxControl.SelectedItem as string;
                string secondVariableStr = this.settingSecondaryVaraibleListBoxControl.SelectedItem as string;
                string primaryItemStr = this.settingPrimaryItemListBoxControl.SelectedItem as string;

                SerializableDictionary<string, SecondaryItem> secondaryItemDict = new SerializableDictionary<string, SecondaryItem>();
                foreach (string temp in this.settingSecondaryItemListBoxControl.CheckedItems)
                {
                    if (!temp.Equals("Select All"))
                    {
                        secondaryItemDict.Add(temp, new SecondaryItem(temp));
                    }

                }
                // the secondaryItemList has checked items

                if (this.variableRelation.containRecord(primaryVariableStr, secondVariableStr))
                {
                    VariableDependenceRecord record = this.variableRelation.getRecord(primaryVariableStr, secondVariableStr);
                    if (record.ItemDependencDict.ContainsKey(primaryItemStr))
                    {
                        // in case of the checked item change to be no one checked ,so we use the last state secondaryItemList to decide whether is a empty  or not;
                        if (record.ItemDependencDict[primaryItemStr].Count > 0)
                        {
                            //if the secondaryItemList is not empty we just set it
                            if (secondaryItemDict.Count > 0)
                            {
                                record.ItemDependencDict[primaryItemStr] = secondaryItemDict;
                            }
                            // else the record item becomes empty ,we just need to delete it
                            else
                            {
                                record.ItemDependencDict.Remove(primaryItemStr);

                            }

                            // if the record key related record has no item relation just remove it
                            if (record.ItemDependencDict.Count == 0)
                            {
                                this.variableRelation.DependenceRecordDict.Remove(new VariableDependenceRecordKey(primaryVariableStr, secondVariableStr));
                            }

                        }

                    }
                    else
                    {
                        if (secondaryItemDict.Count > 0)
                        {
                            record.ItemDependencDict.Add(primaryItemStr, secondaryItemDict);
                        }

                    }
                }
                else
                {
                    if (secondaryItemDict.Count > 0)
                    {
                        VariableDependenceRecord record = new VariableDependenceRecord();
                        record.PrimaryVariable = primaryVariableStr;
                        record.SecondaryVarialbe = secondVariableStr;
                        record.ItemDependencDict.Add(primaryItemStr, secondaryItemDict);
                        // new added function : auto select all 
                        //List<string> primaryData = this.settingPrimaryItemListBoxControl.DataSource as List<string>;
                        //List<string> secondaryData = this.settingSecondaryItemListBoxControl.DataSource as List<string>;
                        //SerializableDictionary<string, SecondaryItem> allSecondaryItemDict = new SerializableDictionary<string, SecondaryItem>();
                        //foreach (string temp in secondaryData)
                        //{
                        //    if (!temp.Equals("Select All"))
                        //    {
                        //        allSecondaryItemDict.Add(temp, new SecondaryItem(temp));
                        //    }

                        //}
                        //foreach (string primaryItem in primaryData)
                        //{
                        //    if (primaryItem.Equals(primaryItemStr))
                        //    {
                        //        record.ItemDependencDict.Add(primaryItemStr, secondaryItemDict);
                        //    }
                        //    else
                        //    {
                        //        record.ItemDependencDict.Add(primaryItem, allSecondaryItemDict);
                        //    }
                        //}

                        this.variableRelation.DependenceRecordDict.Add(new VariableDependenceRecordKey(primaryVariableStr, secondVariableStr), record);
                    }


                }
            }
        }

        private bool checkDependenceRecordValid()
        {
            foreach (KeyValuePair<VariableDependenceRecordKey, VariableDependenceRecord> kv1 in this.variableRelation.DependenceRecordDict)
            {
                VariableDependenceRecord record = kv1.Value;
                String primaryVarialbeStr = kv1.Key.PrimaryVariableKey;
                String secondaryVariableStr = kv1.Key.SecondaryVariableKey;
                Variable primaryVariable = this.variables[primaryVarialbeStr];
                if (record.ItemDependencDict.Count < primaryVariable.PlatformList.Count)
                {
                    MessageDxUnit.ShowWarning("The current dependence record has some  primary item which doesn't has corresponding secondary item!");
                    return false;
                }
            }
            
            return true;
        }

        #endregion


        #region     secondary variable listbox event handler function
        private void settingSecondaryVaraibleListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.settingSecondaryVaraibleListBoxControl.SelectedIndex = -1;
        }

        private void settingSecondaryVaraibleListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBoxControl = sender as ListBoxControl;
            int selectIndex = listBoxControl.SelectedIndex;
            string temVariableStr = listBoxControl.SelectedItem as string;
            if (selectIndex >= 0)
            {
                Variable temVariable = this.variables[temVariableStr];
                List<string> dataList = new List<string>(temVariable.PlatformList);
                dataList.Add("Select All");
                this.settingSecondaryItemListBoxControl.DataSource = dataList;
                this.settingSecondaryItemListBoxControl.SelectedIndex = -1;



                // check if the settingPrimaryItemListBoxControl has  been selected,if selected make setttingSecondaryItemListBox enable ,else disable
                this.settingPrimaryItemListBoxControl.SelectedIndex = -1;
                this.settingSecondaryItemListBoxControl.Enabled = false;
            }
        }

        private bool settingSecondaryVaraibleListBoxControl_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            this.saveCurrentDependenceRecord();
            return checkDependenceRecordValid();
        }
   
        #endregion


        #region   primary item listbox event handler function
        private void settingPrimaryItemListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            // this.settingPrimaryItemListBoxControl.SelectedIndex = -1;
            this.settingPrimaryItemListBoxControl.DataSource = null;
        }

        private void settingPrimaryItemListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxControl listBox = sender as ListBoxControl;
            this.settingSecondaryItemListBoxControl.UnCheckAll();
            if (listBox.SelectedIndex >= 0)
            {
                this.settingSecondaryItemListBoxControl.Enabled = true;
                // load the according secondary checked item
                if (this.settingSecondaryVaraibleListBoxControl.SelectedIndex >= 0)
                {
                    string primaryVariableStr = this.settingPrimaryVaraibleListBoxControl.SelectedItem as string;
                    string secondVariableStr = this.settingSecondaryVaraibleListBoxControl.SelectedItem as string;
                    string primaryItemStr = (listBox.DataSource as List<string>)[listBox.SelectedIndex];
                    if (this.variableRelation.containRecord(primaryVariableStr, secondVariableStr))
                    {
                        VariableDependenceRecord record = this.variableRelation.getRecord(primaryVariableStr, secondVariableStr);
                        if (record.ItemDependencDict.ContainsKey(primaryItemStr))
                        {
                            SerializableDictionary<string, SecondaryItem> secondaryItemDict = record.ItemDependencDict[primaryItemStr];

                            foreach (KeyValuePair<string, SecondaryItem> kv in secondaryItemDict)
                            {
                                List<string> list = this.settingSecondaryItemListBoxControl.DataSource as List<string>;
                                int index = list.IndexOf(kv.Value.SecondaryItemStr);
                                this.settingSecondaryItemListBoxControl.SetItemChecked(index, true);
                            }
                            return;
                        }
                    }
                }

            }
            else
            {
                this.settingSecondaryItemListBoxControl.Enabled = false;

            }


            this.settingSecondaryItemListBoxControl.SelectedIndex = -1;
            return;
        }

        private bool settingPrimaryItemListBoxControl_BeforeSelect(object sender, BeforeSelectEventArgs e)
        {
            saveCurrentDependenceRecord();
            return true;

        }
   

       // private bool check


        #endregion


        #region secondary item listbox event handler function
        private void settingSecondaryItemListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            // this.settingSecondaryItemListBoxControl.SelectedIndex = -1;
            this.settingSecondaryItemListBoxControl.DataSource = null;
        }

        private void settingSecondaryItemListBoxControl_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            CheckedListBoxControl listBox = sender as CheckedListBoxControl;
            if (listBox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }

            if (e.Index == listBox.ItemCount - 1)
            {
                Font font = new Font(e.Appearance.Font, FontStyle.Bold);
                e.Appearance.Font = font;

            }
        }

        private void settingSecondaryItemListBoxControl_MouseMove(object sender, MouseEventArgs e)
        {
            CheckedListBoxControl listBoxControl = sender as CheckedListBoxControl;
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

        private void settingSecondaryItemListBoxControl_MouseLeave(object sender, EventArgs e)
        {
            toolTipController1.HideHint();
        }

        private void settingSecondaryItemListBoxControl_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {

            if (e.Index == 0)
            {
                return;
            }
            CheckedListBoxControl checkList = sender as CheckedListBoxControl;

            if (e.Index < checkList.ItemCount - 1 && e.State == CheckState.Unchecked)
            {

                if (checkList.GetItemChecked(checkList.ItemCount - 1))
                {
                    checkList.SetItemChecked(checkList.ItemCount - 1, false);
                }

            }
            else if (e.Index == checkList.ItemCount - 1)
            {
                if (e.State == CheckState.Checked)
                {
                    checkList.CheckAll();
                }
                else
                {
                    int checkCount = 0;
                    for (int index = 0; index < checkList.ItemCount - 1; index++)
                    {
                        if (checkList.GetItemChecked(index))
                        {
                            checkCount++;
                        }

                        if (checkCount == checkList.ItemCount - 1)
                        {
                            checkList.UnCheckAll();
                        }
                    }
                }
            }
        }
   

        #endregion      
       
    }
}

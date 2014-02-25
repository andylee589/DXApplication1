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
        #region shift button event handler function
        private void rightShiftButton_Click(object sender, EventArgs e)
        {
            string tempVariable = this.generalPrimaryVariableListBoxControl.SelectedItem as string;
            int selectedIndex = this.generalPrimaryVariableListBoxControl.SelectedIndex;
            // when primary variable change to secondary varaible we need to check the variable relation and remove related records
            this.variableRelation.removeRecordContainsPrimaryKey(tempVariable);

            if (selectedIndex >= 0)
            {
                variableRelation.PrimaryVariableList.RemoveAt(selectedIndex);
                variableRelation.SecondaryVariableList.Add(tempVariable);
                this.generalPrimaryVariableListBoxControl.SelectedIndex = -1;
                this.generalPrimaryVariableListBoxControl.Refresh();
                this.generalSecondaryVaraibleListBoxControl.Refresh();

            }
        }

        private void leftShiftButton_Click(object sender, EventArgs e)
        {
            string tempVariable = this.generalSecondaryVaraibleListBoxControl.SelectedItem as string;
            int selectedIndex = this.generalSecondaryVaraibleListBoxControl.SelectedIndex;

            // when change secondary variable to primary variable,we need to check the variable relation and remove related records
            this.variableRelation.removeRecordContainsSecondaryKey(tempVariable);
            if (selectedIndex >= 0)
            {
                variableRelation.SecondaryVariableList.RemoveAt(selectedIndex);
                variableRelation.PrimaryVariableList.Add(tempVariable);
                this.generalSecondaryVaraibleListBoxControl.SelectedIndex = -1;
                this.generalPrimaryVariableListBoxControl.Refresh();
                this.generalSecondaryVaraibleListBoxControl.Refresh();
            }
        }
        #endregion
      

        #region primary variable event handler function
        private void generalPrimaryVariableListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListBoxControl listBoxControl = sender as ListBoxControl;
            if (listBoxControl.SelectedIndex >= 0)
            {
                this.rightShiftButton.Enabled = true;
                this.leftShiftButton.Enabled = false;
                if (this.generalSecondaryVaraibleListBoxControl.Visible)
                {
                    this.generalSecondaryVaraibleListBoxControl.SelectedIndex = -1;
                }
            }

        }

        private void generalPrimaryVariableListBoxControl_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            ListBoxControl listBox = sender as ListBoxControl;
            if (listBox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }
        }

        private void generalPrimaryVariableListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.generalPrimaryVariableListBoxControl.SelectedIndex = -1;
        }

        private void generalPrimaryVariableListBoxControl_MouseMove(object sender, MouseEventArgs e)
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

        private void generalPrimaryVariableListBoxControl_MouseLeave(object sender, EventArgs e)
        {
            toolTipController1.HideHint();
        }

        #endregion

        #region secondary varialbe event handler funtion
        private void generalSecondaryVaraibleListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListBoxControl listBoxControl = sender as ListBoxControl;
            if (listBoxControl.SelectedIndex >= 0)
            {
                this.rightShiftButton.Enabled = false;
                this.leftShiftButton.Enabled = true;
                if (this.generalPrimaryVariableListBoxControl.Visible)
                {
                    this.generalPrimaryVariableListBoxControl.SelectedIndex = -1;
                }
            }
        }

        private void generalSecondaryVaraibleListBoxControl_VisibleChanged(object sender, EventArgs e)
        {
            this.generalSecondaryVaraibleListBoxControl.SelectedIndex = -1;
        }


        private void generalSecondaryVaraibleListBoxControl_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {
            ListBoxControl listBox = sender as ListBoxControl;
            if (listBox.Enabled && e.State != (DrawItemState.Focus & DrawItemState.Selected))
            {
                e.Appearance.BackColor = Color.SlateGray;
            }
        }

        #endregion

        
    }
}

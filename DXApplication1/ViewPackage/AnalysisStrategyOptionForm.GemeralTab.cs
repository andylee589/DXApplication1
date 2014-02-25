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
        
        #region initial function
        private void InitGeneralData(AnalysisStrategy strategy)
        {
            this.strategyNameTextEdit.Text = strategy.StrategyName;
            this.strategyDescTextEdit.Text = strategy.StrategyDescription;
            this.testRoudsSpinEdit.Value = strategy.TestRounds;
            // init the platform weight choose
            Type platformWeightSetType = strategy.PlatformWeightSetMethod.Value.GetType();
            this.platformWeightRadioGroup.SelectedIndex = StrategyOptionMapping.PlatformWeightSetTypeDict[platformWeightSetType];

            Type platformAssignType = strategy.PlatformAssignMethod.Value.GetType();
            this.platformAssignRadioGroup.SelectedIndex = StrategyOptionMapping.PlatformAssignTypeDict[platformAssignType];

        }
        #endregion

        #region Info part event handler function
        private void strategyNameTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textedit = sender as TextEdit;
            this.Strategy.StrategyName = textedit.Text;
        }

        private void strategyDescTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit textedit = sender as TextEdit;
            this.Strategy.StrategyDescription = textedit.Text;
        }

        private void testRoudsSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            SpinEdit spinEdit = sender as SpinEdit;
            this.Strategy.TestRounds = int.Parse(spinEdit.Text);
        }

        private bool checkInputValid()
        {
            bool isValid = true;
            if (this.strategyNameTextEdit.Text.Trim().Equals(""))
            {
                MessageDxUnit.ShowWarning("Strategy Name can not be empty");
                isValid = false;
            }
            else if (this.strategyDescTextEdit.Text.Trim().Equals(""))
            {
                MessageDxUnit.ShowWarning("Strategy Description can not be empty");
                isValid = false;
            }
            return isValid;
        }
        #endregion

        #region platform weight part event handler function
        private void platformWeightRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            int index = radioGroup.SelectedIndex;
            Type platformWeightSetType = StrategyOptionMapping.PlatformWeightSetIndexDict[index];
            this.Strategy.PlatformWeightSetMethod.Value = this.Strategy.PlatformWeightSetMethodDict[platformWeightSetType.FullName];

            if (radioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.manualWeightSetTypeName])
            {
                this.manualAssignNode.Expanded = true;
                this.autoAssignNode.Expanded = false;
                // load the manual assignment data
                //load primary variable data
                ManualAssignmentPlatformWeightSetMethod manualSet = (ManualAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                SerializableDictionary<string, SerializableDictionary<string, double>> primaryWeightDict = manualSet.PrimaryVariableWeightDict;
                SerializableDictionary<string, SerializableDictionary<string, double>>.KeyCollection primaryVarialbeStrList = primaryWeightDict.Keys;

                this.primaryPrimaryVariableListBoxControl.DataSource = primaryVarialbeStrList;
                // load the secondary variable
                Type secondaryItemType = manualSet.SecondaryItemWeightSetMethod.Value.GetType();
                this.secondaryVariableRadioGroup.SelectedIndex = StrategyOptionMapping.SecondaryItemWeightSetTypeDict[secondaryItemType];
            }
            else if (radioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.autoWeightSetTypeName])
            {
                this.autoAssignNode.Expanded = true;
                this.manualAssignNode.Expanded = false;
                // load the auto assignment data
                // language priority load
                AutoAssignmentPlatformWeightSetMethod autoSet = (AutoAssignmentPlatformWeightSetMethod)this.Strategy.PlatformWeightSetMethod.Value;
                Type languagePriorityType = autoSet.FactorPrioritySetMethod.Value.GetType();
                this.factorPriorityRadioGroup.SelectedIndex = StrategyOptionMapping.FactorPriorityTypeDict[languagePriorityType];
                // platform priority load
                Type platformPriorityType = autoSet.PlatformPrioritySetMethod.Value.GetType();
                this.platformPriorityRadioGroup.SelectedIndex = StrategyOptionMapping.PlatformPriorityTypeDict[platformPriorityType];
            }
        }
        #endregion

    }
}

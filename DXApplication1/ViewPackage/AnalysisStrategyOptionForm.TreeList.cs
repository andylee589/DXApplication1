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
        private TreeListNode generalNode;
        private TreeListNode platformWeightNode;
        private TreeListNode assignMethodNode;
        private TreeListNode manualAssignNode;
        private TreeListNode autoAssignNode;
        private TreeListNode primaryVariableManualAssignNode;

        private TreeListNode secondaryVariableManualAssignNode;
        private TreeListNode FactorPriorityNode;
        private TreeListNode platformPriorityNode;

        #region initial function
        private void InitTreeList()
        {
            generalNode = treeList1.AppendNode(null, null);
            generalNode.SetValue("name", StaticResource.strategyGeneralNodeStr);
            platformWeightNode = treeList1.AppendNode(null, null);
            platformWeightNode.SetValue("name",StaticResource.strategyPlatformWeightNodeStr );
            assignMethodNode = treeList1.AppendNode(null, null);
            assignMethodNode.SetValue("name", StaticResource.strategyAssignMethodNodeStr);
            //    TreeListNode variablePreffernceNode = treeList1.AppendNode(null, null);
            //    variablePreffernceNode.SetValue("name", "Variable Preference");

            manualAssignNode = treeList1.AppendNode(null, platformWeightNode);
            manualAssignNode.SetValue("name", StaticResource.strategyManualAssignNodeStr);
            autoAssignNode = treeList1.AppendNode(null, platformWeightNode);
            autoAssignNode.SetValue("name",StaticResource.strategyAutoAssignNodeStr );

            primaryVariableManualAssignNode = treeList1.AppendNode(null, manualAssignNode);
            primaryVariableManualAssignNode.SetValue("name",StaticResource.strategyPrimaryVariableManualAssignNodeStr );
            secondaryVariableManualAssignNode = treeList1.AppendNode(null, manualAssignNode);
            secondaryVariableManualAssignNode.SetValue("name", StaticResource.strategySecondaryVariableManualAssignNodeStr);

            FactorPriorityNode = treeList1.AppendNode(null, autoAssignNode);
            FactorPriorityNode.SetValue("name", StaticResource.strategyFactorPriorityNodeNodeStr);
            platformPriorityNode = treeList1.AppendNode(null, autoAssignNode);
            platformPriorityNode.SetValue("name", StaticResource.strategyPlatformPriorityNodeManualAssignNodeStr);
            //  TreeListNode priorityMeasureNode = treeList1.AppendNode(null, autoAssignNode);
            // priorityMeasureNode.SetValue("name", "Priority Measure");

            // expand all nodes
            this.treeList1.ExpandAll();
        }
        #endregion

        #region treelist event handler function 
        private void treeList1_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (this.platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.manualWeightSetTypeName])
            {
                this.manualAssignNode.Expanded = true;
                this.autoAssignNode.Expanded = false;

                if (e.Node.Equals(this.manualAssignNode) || e.Node.Equals(this.primaryVariableManualAssignNode) || e.Node.Equals(this.secondaryVariableManualAssignNode))
                {
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (e.Node.Equals(this.autoAssignNode) || e.Node.Equals(this.FactorPriorityNode) || e.Node.Equals(this.platformPriorityNode))
                {
                    e.Appearance.ForeColor = Color.DarkGray;
                }

            }
            else if (this.platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.autoWeightSetTypeName])
            {
                this.autoAssignNode.Expanded = true;
                this.manualAssignNode.Expanded = false;
                if (e.Node.Equals(this.manualAssignNode) || e.Node.Equals(this.primaryVariableManualAssignNode) || e.Node.Equals(this.secondaryVariableManualAssignNode))
                {
                    e.Appearance.ForeColor = Color.DarkGray;
                }
                else if (e.Node.Equals(this.autoAssignNode) || e.Node.Equals(this.FactorPriorityNode) || e.Node.Equals(this.platformPriorityNode))
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }

            if (e.Node.Focused)
            {
                e.Appearance.ForeColor = Color.White;
                e.Appearance.BackColor = Color.Black;
                // e.Appearance.Font.Bold = true;
            }
        }


        private void treeList1_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs e)
        {
            if (this.platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.manualWeightSetTypeName])
            {
                if (e.Node.Equals(this.manualAssignNode) || e.Node.Equals(this.primaryVariableManualAssignNode) || e.Node.Equals(this.secondaryVariableManualAssignNode))
                {
                    e.CanFocus = true;
                }
                else if (e.Node.Equals(this.autoAssignNode) || e.Node.Equals(this.FactorPriorityNode) || e.Node.Equals(this.platformPriorityNode))
                {
                    e.CanFocus = false;
                }
            }
            else if (this.platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.autoWeightSetTypeName])
            {
                if (e.Node.Equals(this.manualAssignNode) || e.Node.Equals(this.primaryVariableManualAssignNode) || e.Node.Equals(this.secondaryVariableManualAssignNode))
                {
                    e.CanFocus = false;
                }
                else if (e.Node.Equals(this.autoAssignNode) || e.Node.Equals(this.FactorPriorityNode) || e.Node.Equals(this.platformPriorityNode))
                {
                    e.CanFocus = true;
                }
            }

            if (!checkCurrentPageValid(sender, e))
            {
                e.CanFocus = false;
            }
        }

        private bool checkCurrentPageValid(object sender, BeforeFocusNodeEventArgs e)
        {
            if (e.CanFocus)
            {
                if (e.OldNode == this.primaryVariableManualAssignNode && e.Node != this.primaryVariableManualAssignNode)
                {
                    return checkPrimaryGridViewValid();
                }
                else if (e.OldNode == this.secondaryVariableManualAssignNode && e.Node != this.secondaryVariableManualAssignNode)
                {
                    return checkSecondaryGridViewValid();
                }
            }
            return true;
        }


        private bool checkCurrentPageValid()
        {
            if (this.configureTabControl.SelectedTabPage == this.manualPrimaryTabPage)
            {
                return checkPrimaryGridViewValid();
            }
            else if (this.configureTabControl.SelectedTabPage == this.manualSecondaryTabPage)
            {
                return checkSecondaryGridViewValid();
            }
            return true;
        }

        private void treeList1_AfterFocusNode(object sender, NodeEventArgs e)
        {
            TreeList treeList = sender as TreeList;
            TreeListHitInfo info = treeList.CalcHitInfo(treeList.PointToClient(MousePosition));
            if (info.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell)
            {
                switch (info.Node.GetValue("name") as string)
                {
                    case "General":
                        this.configureTabControl.SelectedTabPage = this.generalTabPage;
                        break;
                    case "Platform Weight":
                        // depends on the radio button value of general setting 
                        if (this.platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.manualWeightSetTypeName])
                        {

                            this.configureTabControl.SelectedTabPage = this.manualPrimaryTabPage;

                        }
                        else if (platformWeightRadioGroup.SelectedIndex == StrategyOptionMapping.PlatformWeightSetTypeDict[StrategyOptionMapping.autoWeightSetTypeName])
                        {
                            this.configureTabControl.SelectedTabPage = this.factorPriorityTabPage;
                        }

                        break;
                    case "Manual Assignment":
                        this.configureTabControl.SelectedTabPage = this.manualPrimaryTabPage;

                        break;
                    case "Primary Variable":
                        this.configureTabControl.SelectedTabPage = this.manualPrimaryTabPage;
                        break;
                    case "Secondary Variable":
                        this.configureTabControl.SelectedTabPage = this.manualSecondaryTabPage;
                        break;
                    case "Auto Assignment":
                        this.configureTabControl.SelectedTabPage = this.factorPriorityTabPage;
                        break;
                    case "Factor Priority":
                        this.configureTabControl.SelectedTabPage = this.factorPriorityTabPage;
                        break;
                    case "Platform Priority":
                        this.configureTabControl.SelectedTabPage = this.platformPriorityTabPage;
                        break;
                    case "Priority Measure":
                        this.configureTabControl.SelectedTabPage = this.priorityMeasureTabPage;
                        break;
                    case "Platform Assign Method":
                        this.configureTabControl.SelectedTabPage = this.assignMethodTabPage;
                        break;
                    case "Variable Preference":
                        this.configureTabControl.SelectedTabPage = this.variablePreferenceTabPage;
                        break;
                }
            }
        }
        #endregion

    }
}

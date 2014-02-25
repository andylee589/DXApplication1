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

        #region platform priority radio group event handler function
        private void platformPriorityRadioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup radioGroup = sender as RadioGroup;
            int index = radioGroup.SelectedIndex;
            Type platformPriorityType = StrategyOptionMapping.PlatformPriorityIndexDict[index];

            AutoAssignmentPlatformWeightSetMethod autoSet = this.Strategy.PlatformWeightSetMethod.Value as AutoAssignmentPlatformWeightSetMethod;
            autoSet.PlatformPrioritySetMethod.Value = autoSet.PlatformPrioritySetMethodDict[platformPriorityType.FullName];
        }
        #endregion
    }
}

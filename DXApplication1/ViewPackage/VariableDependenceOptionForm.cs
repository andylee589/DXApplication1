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
        Controller controller = Controller.getInstance();
        Dictionary<string, Variable> variables;
        VariableRelation variableRelation;
        public VariableDependenceOptionForm(VariableRelation variableRelation)
        {
            InitializeComponent();
            InitOptionTabPanel();
            this.variableRelation = variableRelation;
            this.variables = this.controller.project.Variables;
            InitComponentData(variableRelation);
            InitController();
        }

        #region  initial function
        private void InitController()
        {
            controller = Controller.getInstance();
            controller.variableOptionForm = this;
        }

        private void InitOptionTabPanel()
        {
            this.optionTabControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.optionTabControl.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.optionTabControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.optionTabControl.LookAndFeel.UseDefaultLookAndFeel = false;
            // this.configureTabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False; 
        }


        // set the compoment data stored in varaibleRelation object
        private void InitComponentData(VariableRelation variableRelation)
        {
            this.generalPrimaryVariableListBoxControl.DataSource = variableRelation.PrimaryVariableList;
            this.generalSecondaryVaraibleListBoxControl.DataSource = variableRelation.SecondaryVariableList;     
            this.settingPrimaryVaraibleListBoxControl.DataSource = variableRelation.PrimaryVariableList;
            this.settingSecondaryVaraibleListBoxControl.DataSource = variableRelation.SecondaryVariableList;
            this.rightShiftButton.Enabled = false;
            this.leftShiftButton.Enabled = false;
        }
        #endregion

        #region form event handler function

        private void VariableDependenceOptionForm_Load(object sender, EventArgs e)
        {
            TreeListNode generalNode = treeList1.AppendNode(null, null);
            generalNode.SetValue("name", "General");
            TreeListNode dependenceNode = treeList1.AppendNode(null, null);
            dependenceNode.SetValue("name", "Dependence Setting");
        }


        private void variableOptionOKButton_Click(object sender, EventArgs e)
        {
            this.saveCurrentDependenceRecord();        
            if (this.checkDependenceRecordValid())
            {
                this.controller.XMLSerializeVariableRelation(variableRelation);
                this.Close();
            }           
        }    
   
        #endregion     

        

       
    
    }
}
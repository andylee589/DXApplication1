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
        private Controller controller = Controller.getInstance();
        public AnalysisStrategy Strategy { get; set; }
        private int strategyIndex;
        public AnalysisStrategyOptionForm(AnalysisStrategy strategy,int strategyIndex)
        {
            this.Strategy = strategy;
            this.strategyIndex = strategyIndex;
            InitializeComponent();
            InitConfigureTabPanel();
            InitTreeList();      
            InitGeneralData(strategy);
            InitFactorGridView();
            InitController();
        }

        #region initial function
        private void InitController()
        {
            controller = Controller.getInstance();
            controller.strategyOptionForm = this;
        }


        private void InitConfigureTabPanel()
        {
            this.configureTabControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.configureTabControl.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.configureTabControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.configureTabControl.LookAndFeel.UseDefaultLookAndFeel = false;
           // this.configureTabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;          
        }
        #endregion
     
        #region configure tab panel event handler function
        private void configureOkButton_Click(object sender, EventArgs e)
        {
            if (checkInputValid()&& checkCurrentPageValid())
            {
                this.controller.updateStrategyGalleryItem(strategyIndex, this.Strategy.StrategyName, this.Strategy.StrategyDescription);
                this.controller.clearStrategyFileWithIndex(this.strategyIndex);
                this.controller.project.StrategyList[strategyIndex] = this.Strategy;
                this.controller.XMLSerializeAnalysisStrategyList(this.controller.project.StrategyList);
                this.Close();
            }    
        }

        private void ConfigureApplyButton_Click(object sender, EventArgs e)
        {
            if (checkInputValid() && checkCurrentPageValid())
            {
                this.controller.updateStrategyGalleryItem(strategyIndex, this.Strategy.StrategyName, this.Strategy.StrategyDescription);
                this.controller.clearStrategyFileWithIndex(this.strategyIndex);
                this.controller.project.StrategyList[strategyIndex] = this.Strategy;
                this.controller.XMLSerializeAnalysisStrategyList(this.controller.project.StrategyList);
                List<Dictionary<string, Dictionary<string, string>>> testSet= this.controller.project.StrategyList[strategyIndex].startAnalysis();
                this.controller.mainForm.presentDataInTestSetGrid(testSet);
                this.controller.mainForm.testSetGridviewShow();
                this.Close();
            }
        }
        #endregion

    

    
       

       

       
    }
}
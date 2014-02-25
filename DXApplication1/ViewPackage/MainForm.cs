using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraGrid.Views.Grid;
using DXApplication1.ViewPackage;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraCharts;
using System.Threading;


namespace DXApplication1
{
    public partial class MainForm : RibbonForm
    {
        Controller controller = Controller.getInstance();  
        AnalysisStrategyOptionForm strategyOptionForm ;
        VariableDependenceOptionForm variableOptionForm;   

        public MainForm()
        {
           
            InitializeComponent();           
            InitSkinGallery();
            InitStrategyGalleryItemList();
            InitPriorityGridProperty();
            InitStrategyGalleryDropDown();
            InitRibbonControlComponnetEvent();
            InitRibbonCompomentState();
            InitController();
            CheckForIllegalCrossThreadCalls = false;
           
        }
     

        #region initial function 
        private void InitController()
        {
            
            controller.mainForm = this;
        }


        void InitStrategyOptionDialog(AnalysisStrategy strategy,int index)
        {
            strategyOptionForm = new AnalysisStrategyOptionForm(strategy,index);
           
            strategyOptionForm.ShowDialog(this);
            
        }

        void InitVariableOptionDialog(VariableRelation variableRelation)
        {
            variableOptionForm = new VariableDependenceOptionForm(variableRelation);
            variableOptionForm.ShowDialog(this);
            
        }
        #endregion
     

        #region progress panel event handler function
        private void showProgressPanel()
        {
            this.progressPanel1.LookAndFeel.SetSkinStyle("DevExpress Style");
            this.progressPanel1.Visible = true;
        }

        private void hideProgressPanel()
        {
            this.progressPanel1.Visible = false;
        }

        private void progressPanel1_VisibleChanged(object sender, EventArgs e)
        {
            DevExpress.XtraWaitForm.ProgressPanel panel = sender as DevExpress.XtraWaitForm.ProgressPanel;
            if (panel.Visible)
            {
                Control parent = panel.Parent;
               // panel.Location = new Point(parent.Bounds.X + parent.Bounds.Width / 2 - panel.Width / 2- this.splitContainerControl.Panel1.Width, parent.Bounds.Y + parent.Bounds.Height / 2 - panel.Height / 2-this.ribbonControl.Height);
                panel.Location = new Point(parent.Bounds.X + parent.Bounds.Width / 2 - panel.Width / 2 - this.splitContainerControl.Panel1.Width, parent.Bounds.Y + parent.Bounds.Height / 2 - panel.Height / 2 - this.ribbonControl.Height/2);
            }
        }
        #endregion



    
       

    }
}
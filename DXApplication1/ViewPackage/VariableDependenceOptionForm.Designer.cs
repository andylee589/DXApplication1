namespace DXApplication1.ViewPackage
{
    using DXApplication1.ViewPackage;
    using DXApplication1.DataModelPackage;

    partial class VariableDependenceOptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.optionTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.generalTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.leftShiftButton = new DevExpress.XtraEditors.SimpleButton();
            this.rightShiftButton = new DevExpress.XtraEditors.SimpleButton();
            this.generalSecondaryVaraibleListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
            this.generalPrimaryVariableListBoxControl = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dependenceSettingTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.settingSecondaryVaraibleListBoxControl = new DXApplication1.DataModelPackage.MyListBoxControl();
            this.settingPrimaryVaraibleListBoxControl = new DXApplication1.DataModelPackage.MyListBoxControl();
            this.settingSecondaryItemListBoxControl = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.settingPrimaryItemListBoxControl = new DXApplication1.DataModelPackage.MyListBoxControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.variableOptionOKButton = new DevExpress.XtraEditors.SimpleButton();
            this.variableOptionCancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionTabControl)).BeginInit();
            this.optionTabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalSecondaryVaraibleListBoxControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.generalPrimaryVariableListBoxControl)).BeginInit();
            this.dependenceSettingTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingSecondaryVaraibleListBoxControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingPrimaryVaraibleListBoxControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingSecondaryItemListBoxControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingPrimaryItemListBoxControl)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerControl1.IsSplitterFixed = true;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.treeList1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.optionTabControl);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(658, 362);
            this.splitContainerControl1.SplitterPosition = 135;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsView.ShowColumns = false;
            this.treeList1.OptionsView.ShowHorzLines = false;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.Size = new System.Drawing.Size(135, 362);
            this.treeList1.TabIndex = 0;
            this.treeList1.NodeCellStyle += new DevExpress.XtraTreeList.GetCustomNodeCellStyleEventHandler(this.treeList1_NodeCellStyle);
            this.treeList1.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.treeList1_BeforeFocusNode);
            this.treeList1.AfterFocusNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList1_AfterFocusNode);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "treeListColumn1";
            this.treeListColumn1.FieldName = "name";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // optionTabControl
            // 
            this.optionTabControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.optionTabControl.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.optionTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionTabControl.Location = new System.Drawing.Point(0, 0);
            this.optionTabControl.Name = "optionTabControl";
            this.optionTabControl.SelectedTabPage = this.generalTabPage;
            this.optionTabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.optionTabControl.Size = new System.Drawing.Size(518, 362);
            this.optionTabControl.TabIndex = 0;
            this.optionTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.generalTabPage,
            this.dependenceSettingTabPage});
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.groupBox1);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new System.Drawing.Size(512, 356);
            this.generalTabPage.Text = "generalTabPage";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.leftShiftButton);
            this.groupBox1.Controls.Add(this.rightShiftButton);
            this.groupBox1.Controls.Add(this.generalSecondaryVaraibleListBoxControl);
            this.groupBox1.Controls.Add(this.generalPrimaryVariableListBoxControl);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(512, 356);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General ";
            // 
            // leftShiftButton
            // 
            this.leftShiftButton.Location = new System.Drawing.Point(204, 194);
            this.leftShiftButton.Name = "leftShiftButton";
            this.leftShiftButton.Size = new System.Drawing.Size(75, 23);
            this.leftShiftButton.TabIndex = 5;
            this.leftShiftButton.Text = "Left Shift";
            this.leftShiftButton.Click += new System.EventHandler(this.leftShiftButton_Click);
            // 
            // rightShiftButton
            // 
            this.rightShiftButton.Location = new System.Drawing.Point(204, 128);
            this.rightShiftButton.Name = "rightShiftButton";
            this.rightShiftButton.Size = new System.Drawing.Size(75, 23);
            this.rightShiftButton.TabIndex = 4;
            this.rightShiftButton.Text = "Right Shift";
            this.rightShiftButton.Click += new System.EventHandler(this.rightShiftButton_Click);
            // 
            // generalSecondaryVaraibleListBoxControl
            // 
            this.generalSecondaryVaraibleListBoxControl.HorizontalScrollbar = true;
            this.generalSecondaryVaraibleListBoxControl.Location = new System.Drawing.Point(309, 67);
            this.generalSecondaryVaraibleListBoxControl.Name = "generalSecondaryVaraibleListBoxControl";
            this.generalSecondaryVaraibleListBoxControl.Size = new System.Drawing.Size(136, 248);
            this.generalSecondaryVaraibleListBoxControl.TabIndex = 3;
            this.generalSecondaryVaraibleListBoxControl.SelectedIndexChanged += new System.EventHandler(this.generalSecondaryVaraibleListBoxControl_SelectedIndexChanged);
            this.generalSecondaryVaraibleListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.generalSecondaryVaraibleListBoxControl_DrawItem);
            this.generalSecondaryVaraibleListBoxControl.VisibleChanged += new System.EventHandler(this.generalSecondaryVaraibleListBoxControl_VisibleChanged);
            this.generalSecondaryVaraibleListBoxControl.MouseLeave += new System.EventHandler(this.generalPrimaryVariableListBoxControl_MouseLeave);
            this.generalSecondaryVaraibleListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.generalPrimaryVariableListBoxControl_MouseMove);
            // 
            // generalPrimaryVariableListBoxControl
            // 
            this.generalPrimaryVariableListBoxControl.HorizontalScrollbar = true;
            this.generalPrimaryVariableListBoxControl.Location = new System.Drawing.Point(35, 67);
            this.generalPrimaryVariableListBoxControl.Name = "generalPrimaryVariableListBoxControl";
            this.generalPrimaryVariableListBoxControl.Size = new System.Drawing.Size(138, 248);
            this.generalPrimaryVariableListBoxControl.TabIndex = 2;
            this.generalPrimaryVariableListBoxControl.SelectedIndexChanged += new System.EventHandler(this.generalPrimaryVariableListBoxControl_SelectedIndexChanged);
            this.generalPrimaryVariableListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.generalPrimaryVariableListBoxControl_DrawItem);
            this.generalPrimaryVariableListBoxControl.VisibleChanged += new System.EventHandler(this.generalPrimaryVariableListBoxControl_VisibleChanged);
            this.generalPrimaryVariableListBoxControl.MouseLeave += new System.EventHandler(this.generalPrimaryVariableListBoxControl_MouseLeave);
            this.generalPrimaryVariableListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.generalPrimaryVariableListBoxControl_MouseMove);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(317, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(92, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Secondary Variable";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(64, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Primary Variable";
            // 
            // dependenceSettingTabPage
            // 
            this.dependenceSettingTabPage.Controls.Add(this.groupBox2);
            this.dependenceSettingTabPage.Name = "dependenceSettingTabPage";
            this.dependenceSettingTabPage.Size = new System.Drawing.Size(512, 356);
            this.dependenceSettingTabPage.Text = "dependenceSettingTabPage";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.splitContainerControl2);
            this.groupBox2.Controls.Add(this.labelControl6);
            this.groupBox2.Controls.Add(this.labelControl5);
            this.groupBox2.Controls.Add(this.labelControl4);
            this.groupBox2.Controls.Add(this.labelControl3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 356);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dependence Setting";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.IsSplitterFixed = true;
            this.splitContainerControl2.Location = new System.Drawing.Point(6, 54);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.settingSecondaryVaraibleListBoxControl);
            this.splitContainerControl2.Panel1.Controls.Add(this.settingPrimaryVaraibleListBoxControl);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.settingSecondaryItemListBoxControl);
            this.splitContainerControl2.Panel2.Controls.Add(this.settingPrimaryItemListBoxControl);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(499, 284);
            this.splitContainerControl2.SplitterPosition = 199;
            this.splitContainerControl2.TabIndex = 4;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // settingSecondaryVaraibleListBoxControl
            // 
            this.settingSecondaryVaraibleListBoxControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingSecondaryVaraibleListBoxControl.HorizontalScrollbar = true;
            this.settingSecondaryVaraibleListBoxControl.Location = new System.Drawing.Point(101, 0);
            this.settingSecondaryVaraibleListBoxControl.Name = "settingSecondaryVaraibleListBoxControl";
            this.settingSecondaryVaraibleListBoxControl.Size = new System.Drawing.Size(98, 284);
            this.settingSecondaryVaraibleListBoxControl.TabIndex = 1;
            this.settingSecondaryVaraibleListBoxControl.BeforeSelect += new DXApplication1.DataModelPackage.MyListBoxControl.BeforeSelectEventHandler(this.settingSecondaryVaraibleListBoxControl_BeforeSelect);
            this.settingSecondaryVaraibleListBoxControl.SelectedIndexChanged += new System.EventHandler(this.settingSecondaryVaraibleListBoxControl_SelectedIndexChanged);
            this.settingSecondaryVaraibleListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.generalPrimaryVariableListBoxControl_DrawItem);
            this.settingSecondaryVaraibleListBoxControl.VisibleChanged += new System.EventHandler(this.settingSecondaryVaraibleListBoxControl_VisibleChanged);
            this.settingSecondaryVaraibleListBoxControl.MouseLeave += new System.EventHandler(this.generalPrimaryVariableListBoxControl_MouseLeave);
            this.settingSecondaryVaraibleListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.generalPrimaryVariableListBoxControl_MouseMove);
            // 
            // settingPrimaryVaraibleListBoxControl
            // 
            this.settingPrimaryVaraibleListBoxControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingPrimaryVaraibleListBoxControl.HorizontalScrollbar = true;
            this.settingPrimaryVaraibleListBoxControl.Location = new System.Drawing.Point(0, 0);
            this.settingPrimaryVaraibleListBoxControl.Name = "settingPrimaryVaraibleListBoxControl";
            this.settingPrimaryVaraibleListBoxControl.Size = new System.Drawing.Size(98, 284);
            this.settingPrimaryVaraibleListBoxControl.TabIndex = 0;
            this.settingPrimaryVaraibleListBoxControl.BeforeSelect += new DXApplication1.DataModelPackage.MyListBoxControl.BeforeSelectEventHandler(this.settingPrimaryVaraibleListBoxControl_BeforeSelect);
            this.settingPrimaryVaraibleListBoxControl.SelectedIndexChanged += new System.EventHandler(this.settingPrimaryVaraibleListBoxControl_SelectedIndexChanged);
            this.settingPrimaryVaraibleListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.generalPrimaryVariableListBoxControl_DrawItem);
            this.settingPrimaryVaraibleListBoxControl.VisibleChanged += new System.EventHandler(this.settingPrimaryVaraibleListBoxControl_VisibleChanged);
            this.settingPrimaryVaraibleListBoxControl.MouseLeave += new System.EventHandler(this.generalPrimaryVariableListBoxControl_MouseLeave);
            this.settingPrimaryVaraibleListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.generalPrimaryVariableListBoxControl_MouseMove);
            // 
            // settingSecondaryItemListBoxControl
            // 
            this.settingSecondaryItemListBoxControl.CheckOnClick = true;
            this.settingSecondaryItemListBoxControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingSecondaryItemListBoxControl.HorizontalScrollbar = true;
            this.settingSecondaryItemListBoxControl.Location = new System.Drawing.Point(144, 0);
            this.settingSecondaryItemListBoxControl.Name = "settingSecondaryItemListBoxControl";
            this.settingSecondaryItemListBoxControl.Size = new System.Drawing.Size(151, 284);
            this.settingSecondaryItemListBoxControl.TabIndex = 1;
            this.settingSecondaryItemListBoxControl.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.settingSecondaryItemListBoxControl_ItemCheck);
            this.settingSecondaryItemListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.settingSecondaryItemListBoxControl_DrawItem);
            this.settingSecondaryItemListBoxControl.VisibleChanged += new System.EventHandler(this.settingSecondaryItemListBoxControl_VisibleChanged);
            this.settingSecondaryItemListBoxControl.MouseLeave += new System.EventHandler(this.settingSecondaryItemListBoxControl_MouseLeave);
            this.settingSecondaryItemListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.settingSecondaryItemListBoxControl_MouseMove);
            // 
            // settingPrimaryItemListBoxControl
            // 
            this.settingPrimaryItemListBoxControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingPrimaryItemListBoxControl.HorizontalScrollbar = true;
            this.settingPrimaryItemListBoxControl.Location = new System.Drawing.Point(0, 0);
            this.settingPrimaryItemListBoxControl.Name = "settingPrimaryItemListBoxControl";
            this.settingPrimaryItemListBoxControl.Size = new System.Drawing.Size(138, 284);
            this.settingPrimaryItemListBoxControl.TabIndex = 0;
            this.settingPrimaryItemListBoxControl.BeforeSelect += new DXApplication1.DataModelPackage.MyListBoxControl.BeforeSelectEventHandler(this.settingPrimaryItemListBoxControl_BeforeSelect);
            this.settingPrimaryItemListBoxControl.SelectedIndexChanged += new System.EventHandler(this.settingPrimaryItemListBoxControl_SelectedIndexChanged);
            this.settingPrimaryItemListBoxControl.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.generalPrimaryVariableListBoxControl_DrawItem);
            this.settingPrimaryItemListBoxControl.VisibleChanged += new System.EventHandler(this.settingPrimaryItemListBoxControl_VisibleChanged);
            this.settingPrimaryItemListBoxControl.MouseLeave += new System.EventHandler(this.generalPrimaryVariableListBoxControl_MouseLeave);
            this.settingPrimaryItemListBoxControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.generalPrimaryVariableListBoxControl_MouseMove);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(397, 29);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(76, 13);
            this.labelControl6.TabIndex = 3;
            this.labelControl6.Text = "Secondary Item";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(240, 29);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(61, 13);
            this.labelControl5.TabIndex = 2;
            this.labelControl5.Text = "Primary Item";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(110, 29);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(92, 13);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Secondary Variable";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 29);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(77, 13);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Primary Variable";
            // 
            // variableOptionOKButton
            // 
            this.variableOptionOKButton.Location = new System.Drawing.Point(389, 376);
            this.variableOptionOKButton.Name = "variableOptionOKButton";
            this.variableOptionOKButton.Size = new System.Drawing.Size(75, 23);
            this.variableOptionOKButton.TabIndex = 1;
            this.variableOptionOKButton.Text = "OK";
            this.variableOptionOKButton.Click += new System.EventHandler(this.variableOptionOKButton_Click);
            // 
            // variableOptionCancelButton
            // 
            this.variableOptionCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.variableOptionCancelButton.Location = new System.Drawing.Point(523, 376);
            this.variableOptionCancelButton.Name = "variableOptionCancelButton";
            this.variableOptionCancelButton.Size = new System.Drawing.Size(75, 23);
            this.variableOptionCancelButton.TabIndex = 2;
            this.variableOptionCancelButton.Text = "Cancel";
            // 
            // VariableDependenceOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.variableOptionCancelButton;
            this.ClientSize = new System.Drawing.Size(658, 407);
            this.Controls.Add(this.variableOptionCancelButton);
            this.Controls.Add(this.variableOptionOKButton);
            this.Controls.Add(this.splitContainerControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VariableDependenceOptionForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Option";
            this.Load += new System.EventHandler(this.VariableDependenceOptionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionTabControl)).EndInit();
            this.optionTabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.generalSecondaryVaraibleListBoxControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.generalPrimaryVariableListBoxControl)).EndInit();
            this.dependenceSettingTabPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingSecondaryVaraibleListBoxControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingPrimaryVaraibleListBoxControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingSecondaryItemListBoxControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingPrimaryItemListBoxControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraEditors.SimpleButton variableOptionOKButton;
        private DevExpress.XtraEditors.SimpleButton variableOptionCancelButton;
        private DevExpress.XtraTab.XtraTabControl optionTabControl;
        private DevExpress.XtraTab.XtraTabPage generalTabPage;
        private DevExpress.XtraTab.XtraTabPage dependenceSettingTabPage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ListBoxControl generalSecondaryVaraibleListBoxControl;
        private DevExpress.XtraEditors.ListBoxControl generalPrimaryVariableListBoxControl;
        private DevExpress.XtraEditors.SimpleButton leftShiftButton;
        private DevExpress.XtraEditors.SimpleButton rightShiftButton;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckedListBoxControl settingSecondaryItemListBoxControl;
        private DXApplication1.DataModelPackage.MyListBoxControl settingPrimaryItemListBoxControl;
        private DXApplication1.DataModelPackage.MyListBoxControl settingPrimaryVaraibleListBoxControl;
        private DXApplication1.DataModelPackage.MyListBoxControl settingSecondaryVaraibleListBoxControl;
        private DevExpress.Utils.ToolTipController toolTipController1;
    }
}
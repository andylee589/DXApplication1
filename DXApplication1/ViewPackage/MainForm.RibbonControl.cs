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
        // the activated gallery item when right click and pop up a menu
        GalleryItem activatedGalleryItem = null;
        //the checked gallery item in the strategy items
        GalleryItem checkedStrategyGalleryItem = null;
        int checkedStrategyItemindex = -1;
        GalleryItem checkedDropDwonItem = null;
        //strategy item group
        GalleryItemGroup galleryItemGroup = null;
        GalleryItemGroup dropdownItemGroup = null;

        // flag  of the data load ,the variable dependence and analysis is 
        bool isDataLoad = false;
        private string loadFilePath;
        bool isSave = false;
        #region initial function

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void InitStrategyGalleryItemList()
        {
            this.galleryItemGroup = this.strategyRibbonGalleryBarItem.Gallery.Groups[0];
            List<string> fileList = this.controller.getStrategyXMLFileList();
            if (fileList.Count > 0)
            {
                List<AnalysisStrategy> strategyList = this.controller.XMLDeserializeAnalysisStrategy();
                foreach (AnalysisStrategy strategy in strategyList)
                {
                    GalleryItem galleryItem1 = new GalleryItem();
                    galleryItem1.Caption = strategy.StrategyName;
                    galleryItem1.Description = strategy.StrategyDescription;
                    galleryItem1.Hint = "Right Click To Manage This Strategy";
                    this.galleryItemGroup.Items.Add(galleryItem1);
                }
            }
            else
            {
                GalleryItem galleryItem1 = new GalleryItem();
                galleryItem1.Caption = "Default";
                galleryItem1.Description = "Default Setting";
                galleryItem1.Hint = "Right Click To Manage This Strategy";
                this.galleryItemGroup.Items.Add(galleryItem1);
            }
        }


        void InitStrategyGalleryDropDown()
        {

            this.galleryItemGroup = this.strategyRibbonGalleryBarItem.Gallery.Groups[0];
            this.checkedStrategyGalleryItem = this.galleryItemGroup.Items[0];
            this.checkedStrategyItemindex = 0;
            this.galleryDropDown1.Gallery.Groups.Add(new GalleryItemGroup(galleryItemGroup));
            this.dropdownItemGroup = this.galleryDropDown1.Gallery.Groups[0];
            this.checkedDropDwonItem = this.dropdownItemGroup.Items[0];
            //  this.checkedDropDwonItem.Checked = true;

            this.galleryDropDown1.Name = "galleryDropDown1";
            this.galleryDropDown1.Ribbon = this.ribbonControl;
        }

        void InitRibbonControlComponnetEvent()
        {
            //the strategy  pop up menu customization
            this.ribbonControl.ShowCustomizationMenu += new DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventHandler(this.ribbonControl_ShowCustomizationMenu);
            //strategy pop up menu bar items click event
            this.applyBarButtonItem.ItemClick += this.applyBarbuttonItem_ItemClick;
            this.modifyBarButtonItem.ItemClick += this.modifyBarButtonItem_ItemClick;
            this.createBarButtonItem.ItemClick += this.createBarButtonItem_ItemClick;
            this.removeBarButtonItem.ItemClick += this.removeBarButtonItem_ItemClick;

            // the strategy items click event;
            this.strategyRibbonGalleryBarItem.Gallery.ItemClick += this.strategyGallery_ItemClick;

            this.strategyRibbonGalleryBarItem.GalleryDropDown = galleryDropDown1;
            this.strategyRibbonGalleryBarItem.GalleryDropDown.GalleryItemClick += this.dropDownGallery_ItemClick;
        }


        void InitRibbonCompomentState()
        {
            isDataLoad = false;
            isTestSetGenerated = false;
            
            this.variableRelationRibbonPageGroup.Enabled = false;
            this.analysisStrategyRibbonGroup.Enabled = false;
            this.chartTypeRibbenGroup.Enabled = false;
            this.iSave.Enabled = false;
            this.iSaveAs.Enabled = false;
            this.clearDataItem.Enabled = false;
            this.progressBarEditItem1.Links[0].Visible = false;
        }
#endregion

        #region  gallery  events handler function
            #region strategy gallery menu events function
            private void ribbonControl_ShowCustomizationMenu(object sender, DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventArgs e)
            {

                if (e.HitInfo != null && e.HitInfo.InGalleryItem)
                {
                    // barButtonItem1.ImageIndex = e.HitInfo.GalleryItem.ImageIndex;
                    // show strategy popup menu when the gallery is strategy gallery otherwise show the default pop up menu
                    if (e.HitInfo.Gallery.Equals(this.strategyRibbonGalleryBarItem.Gallery))
                    {
                        e.ShowCustomizationMenu = false;
                        popupMenu1.ShowPopup(ribbonControl.PointToScreen(e.HitInfo.HitPoint));
                        this.activatedGalleryItem = e.HitInfo.GalleryItem;
                        // MessageBox.Show(e.HitInfo.GalleryItem.Caption);
                        // GalleryItemGroup group = this.strategyRibbonGalleryBarItem.Gallery.Groups[0];
                        if (this.activatedGalleryItem.Equals(this.galleryItemGroup.Items[0]))
                        {
                            this.removeBarButtonItem.Enabled = false;
                        }
                        else
                        {
                            this.removeBarButtonItem.Enabled = true;
                        }

                        if (this.activatedGalleryItem.Checked)
                        {
                            this.applyBarButtonItem.Enabled = false;
                        }
                        else
                        {
                            this.applyBarButtonItem.Enabled = true;
                        }

                    }
                    else
                    {
                        e.ShowCustomizationMenu = true;
                    }

                }
            }



            // strategy pop up menu bar items click event handler
            private void applyBarbuttonItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                int index = this.galleryItemGroup.Items.IndexOf(this.activatedGalleryItem);
                applayStrategy(index);
            }

            private void applayStrategy(int index)
            {
                if (!isTestSetGenerated)
                {
                    isTestSetGenerated = true;
                    this.chartTypeRibbenGroup.Enabled = true;
                    this.iSave.Enabled = true;
                    this.iSaveAs.Enabled = true;
                }
                List<AnalysisStrategy> strategyList = this.controller.XMLDeserializeAnalysisStrategy();
                AnalysisStrategy strategy = strategyList[index];
                List<Dictionary<string, Dictionary<string, string>>> testSet = strategy.startAnalysis();
                this.testSetGridviewShow();
                this.presentDataInTestSetGrid(testSet);
                isTestSetGenerated = false;
            }


            private void modifyBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                List<AnalysisStrategy> strategyList = this.controller.XMLDeserializeAnalysisStrategy();
                int index = this.galleryItemGroup.Items.IndexOf(this.activatedGalleryItem);
                AnalysisStrategy strategy = strategyList[index];
                InitStrategyOptionDialog(strategy, index);
                // modify both dropdown1 groups and strategy bar groups

            }

            private void createBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                int index = this.controller.project.StrategyList.Count;
                AnalysisStrategy strategy = new AnalysisStrategy();
                string strategyName = "Strategy" + index;
                string description = "Strategy" + index + " setting";
                int testRounds = 1;
                strategy.initStrategyInfo(strategyName, description, testRounds);
                this.controller.project.StrategyList.Add(strategy);
                this.controller.XMLSerializeAnalysisStrategyList(this.controller.project.StrategyList);

                this.galleryItemGroup.Items.Add(new GalleryItem(null, strategyName, description));
                updateDropDownGroups();
            }

            private void removeBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                int index = this.galleryItemGroup.Items.IndexOf(this.activatedGalleryItem);
                this.controller.project.StrategyList.RemoveAt(index);
                this.controller.clearStrategyFileWithIndex(index);
                this.controller.XMLSerializeAnalysisStrategyList(this.controller.project.StrategyList);
                // remove from gallery item group
                this.galleryItemGroup.Items.Remove(this.activatedGalleryItem);
                // set the checked item the default setting item
                if (this.activatedGalleryItem.Checked)
                {
                    this.checkedStrategyGalleryItem = this.galleryItemGroup.Items[0];
                    this.checkedStrategyItemindex = 0;

                    this.checkedStrategyGalleryItem.Checked = true;

                    this.checkedDropDwonItem = this.dropdownItemGroup.Items[checkedStrategyItemindex];
                    this.checkedDropDwonItem.Checked = true;
                }
                updateDropDownGroups();
            }
     #endregion


            #region strategy Gallery Update and click function
     // the click event of the strategy gallery items
            private void strategyGallery_ItemClick(object sender, GalleryItemClickEventArgs e)
            {
                this.checkedStrategyGalleryItem.Checked = false;
                this.checkedStrategyGalleryItem = e.Item;
                this.checkedStrategyItemindex = this.galleryItemGroup.Items.IndexOf(e.Item);
                // MessageBox.Show("" + checkedItemindex);
                this.checkedStrategyGalleryItem.Checked = true;
                applayStrategy(checkedStrategyItemindex);
                this.checkedDropDwonItem.Checked = false;
                this.checkedDropDwonItem = this.dropdownItemGroup.Items[checkedStrategyItemindex];
                this.checkedDropDwonItem.Checked = true;
            }

            private void updateDropDownGroups()
            {
                this.galleryDropDown1.Gallery.Groups.Clear();
                this.galleryDropDown1.Gallery.Groups.Add(new GalleryItemGroup(galleryItemGroup));

                updateDropDownCheckedItem();
            }

            private void updateDropDownCheckedItem()
            {
                this.dropdownItemGroup = this.galleryDropDown1.Gallery.Groups[0];
                this.checkedDropDwonItem = this.dropdownItemGroup.Items[checkedStrategyItemindex];
                this.checkedDropDwonItem.Checked = true;
            }


            // the click event of the dropdown gallery items
            private void dropDownGallery_ItemClick(object sender, GalleryItemClickEventArgs e)
            {
                this.checkedDropDwonItem.Checked = false;
                this.checkedDropDwonItem = e.Item;
                this.checkedStrategyItemindex = this.dropdownItemGroup.Items.IndexOf(e.Item);
                applayStrategy(checkedStrategyItemindex);
                this.checkedDropDwonItem.Checked = true;
                this.checkedStrategyGalleryItem.Checked = false;
                this.checkedStrategyGalleryItem = this.galleryItemGroup.Items[checkedStrategyItemindex];
                this.checkedStrategyGalleryItem.Checked = true;
                // this.strategyRibbonGalleryBarItem.Gallery.ScrollTo(this.checkedGalleryItem, true);
            }
     #endregion
        #endregion

        #region file group event handler function

            #region load data item click handler functin
            private void loadDataItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                string path = openFile();              
                if (path != null)
                {
                    this.loadFilePath = path;
                    priorityGridviewShow();
                    showProgressPanel();
                    CheckForIllegalCrossThreadCalls = false;
                    Thread loadDataThread = new Thread(new ParameterizedThreadStart(loadDataThreadRun));
                    loadDataThread.Start(path);
                }
            }


            private void loadDataThreadRun(object filePath)
            {
                string path = filePath as string;
                getDataFromExcel(path);
                if (isDataLoad)
                {
                    MethodInvoker invoker = new MethodInvoker(presentDataInPriorityGrid);
                    this.Invoke(invoker);
                    if (!this.priorityGridView.IsVisible)
                    {
                        priorityGridviewShow();
                    }
                }
                hideProgressPanel();
            }



            public string openFile()
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "txt files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    //  this.progressBar1.Visible = true;
                    string extension = Path.GetExtension(fileDialog.FileName);

                    string[] str = new string[] { ".xls", ".xlsx" };
                    bool isContain = false;
                    foreach (string tem in str)
                    {
                        if (tem.Equals(extension))
                        {
                            isContain = true;
                        }

                    }

                    if (!isContain)
                    {
                        MessageDxUnit.ShowTips("the excel file must be in .xls or .xlsx format!");

                    }
                    else if (fileDialog.FileName != null)
                    {
                        return fileDialog.FileName;
                    }
                }
                return null;
            }


            // import the data from excel  to the memory object
            public void getDataFromExcel(string path)
            {

                // load data from excel
                if (controller.loadData(path, "Priority"))
                {
                    // MessageDxUnit.ShowTips("load data form excel success");
                    isDataLoad = true;
                    this.variableRelationRibbonPageGroup.Enabled = true;
                    this.analysisStrategyRibbonGroup.Enabled = true;
                    this.clearDataItem.Enabled = true;
                    this.controller.setProjectDefaultVaraibleRelation();
                    this.controller.setProjectDefaultAnalysisStrategy();
                }
                else
                {
                    MessageDxUnit.ShowWarning("load data form excel failed");
                }
            }

            private void iSave_ItemClick(object sender, ItemClickEventArgs e)
            {
                Thread progressThread = new Thread(new ThreadStart(progressThreadRun));
                Thread saveDataThread = new Thread(new ThreadStart(saveDataThreadRun));
                saveDataThread.Start();
                progressThread.Start();
            }



            private void saveDataThreadRun()
            {
                if (this.controller.saveData(this.loadFilePath))
                {
                    iSave.Enabled = false;
                   // MessageDxUnit.ShowTips("Save succeed");
                }
                else
                {
                    MessageDxUnit.ShowError("Save Failed");
                }
                isSave = true;
            }

            private void progressThreadRun()
            {
                progressBegin();
                while (!isSave)
                {
                    // int value = this.progressBarEditItem1.ed
                    while (int.Parse(this.progressBarEditItem1.EditValue as string) < 90)
                    {
                        MethodInvoker invoker = new MethodInvoker(progressAdvance);
                        this.Invoke(invoker);
                        Thread.Sleep(1000);
                    }
                }
                progressComplete();

            }

            private void progressBegin()
            {
                this.progressBarEditItem1.Links[0].Visible = true;
            }
            private void progressAdvance()
            {
                
                this.progressBarEditItem1.EditValue = (int.Parse(this.progressBarEditItem1.EditValue as string) + 10).ToString();
                this.progressBarEditItem1.Caption = "Save progress " + this.progressBarEditItem1.EditValue + "%       ";
            }

            private void progressComplete()
            {
                this.progressBarEditItem1.EditValue = "100";
                this.progressBarEditItem1.Caption = "             Complete";
                this.progressBarEditItem1.Links[0].Visible = false;
            }
            private void iSaveAs_ItemClick(object sender, ItemClickEventArgs e)
            {
                iSaveAs.Enabled = false;
            }

            private void clearDataItem_ItemClick(object sender, ItemClickEventArgs e)
            {
                this.priorityGridControl.DataSource = null;
                this.testsetGridControl.DataSource = null;
                this.coverateGridControl.DataSource = null;
                this.InitRibbonCompomentState();
                this.clearDataItem.Enabled = false;

            }

    #endregion


            #region exit button click handler function
             private void iExit_ItemClick(object sender, ItemClickEventArgs e)
            {
                MessageBox.Show("OK");
            }
            #endregion

        #endregion

        #region variable relation group event handler function
        private void optionButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            VariableRelation variableRelation = this.controller.XMLDeserializeVariableRelation();

            InitVariableOptionDialog(variableRelation);
        }

        private void clearRelationButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.controller.project.setDefaultVariableRelation();
            this.controller.XMLSerializeVariableRelation(this.controller.project.VariableRelationSetting);
        }




        #endregion


        #region analysis strategy group event handler function

        public void updateStrategyItemCaption(int index, string cpation, string description)
        {
            GalleryItem item = this.galleryItemGroup.Items[index];
            item.Caption = cpation;
            item.Description = description;

        }
        #endregion

        #region chart group event handler funtion

        private void chartGalleryBarItem_GalleryItemClick(object sender, GalleryItemClickEventArgs e)
        {

            if (e.Item.Checked)
            {
                e.Item.Checked = false;
                this.coverageSplitContainerControl.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            }
            else
            {
                showChartType(e.Item.Caption);
                GalleryItemCollection items = this.chartGalleryBarItem.Gallery.Groups[0].Items;
                // this.checkedChartItemindex = items.IndexOf(e.Item);
                foreach (GalleryItem item in items)
                {
                    if (item.Equals(e.Item))
                    {
                        item.Checked = true;
                        coverageGridviewShow();
                        this.coverageSplitContainerControl.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                }
            }
        }


        private void showChartType(string chartType)
        {
            this.coverageSplitContainerControl.Panel2.Controls.Clear();
            int chartIndex = 0;
            List<ChartControl> chartList = new List<ChartControl>();
            foreach (DataColumn column in testSetDataTable.Columns)
            {
                string columnName = column.ColumnName;
                if (this.controller.project.Variables.ContainsKey(columnName))
                {
                    Variable variable = this.controller.project.Variables[columnName];
                    int platformColumnIndex = 2;
                    int sumColumnIndex = coverageDataTable.Columns.Count - 1;
                    int variableColumnIndex = 1;
                    Dictionary<string, int> platformDict = new Dictionary<string, int>();
                    foreach (DataRow row in coverageDataTable.Rows)
                    {
                        if (row.ItemArray[variableColumnIndex].Equals(variable.Name))
                        {
                            string platformName = row.ItemArray[platformColumnIndex] as string;
                            string timesStr = row.ItemArray[sumColumnIndex] as string;
                            int times = int.Parse(timesStr);
                            platformDict.Add(platformName, times);
                        }
                    }
                    chartIndex++;
                    ChartControl chartControl = null;
                    switch (chartType)
                    {
                        case "Pie":
                            chartControl = getPieChartControl(variable.Name, platformDict, chartIndex);
                            break;
                        case "Bar":
                            chartControl = getBarChartControl(variable.Name, platformDict, chartIndex);
                            break;
                    }
                    chartList.Add(chartControl);
                }
            }

            chartList.Reverse();
            foreach (ChartControl chartControl in chartList)
            {
                this.coverageSplitContainerControl.Panel2.Controls.Add(chartControl);
            }

        }


        private ChartControl getPieChartControl(string variableName, Dictionary<string, int> platformDict, int chartIndex)
        {
            ChartControl chartControl1 = new ChartControl();

            // normalConfigure
            chartControl1.Dock = System.Windows.Forms.DockStyle.Top;
            chartControl1.EmptyChartText.Text = "\r\n";
            chartControl1.Legend.Visible = false;
            chartControl1.Location = new System.Drawing.Point(0, 0);
            chartControl1.Name = "chartControl" + chartIndex;

            //configure pie series
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            series1.Label.PointOptions.PointView = PointView.ArgumentAndValues;
            series1.Label.PointOptions.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.Percent;
            series1.Label.LineVisible = true;
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.Name = "Series 1";

            List<SeriesPoint> seriesPointLIst = new List<SeriesPoint>();
            int pointIndex = 0;
            foreach (KeyValuePair<string, int> kv in platformDict)
            {
                DevExpress.XtraCharts.SeriesPoint seriesPoint = new DevExpress.XtraCharts.SeriesPoint(kv.Key, new object[] {
                ((object)(kv.Value))}, pointIndex);
                seriesPointLIst.Add(seriesPoint);
                pointIndex++;
            }
            series1.Points.AddRange(seriesPointLIst.ToArray());
            DevExpress.XtraCharts.PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();
            pieSeriesView1.RuntimeExploding = false;
            series1.View = pieSeriesView1;

            // configure chart series array
            chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
             series1};

            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            chartTitle1.Text = variableName;
            chartTitle1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});


            chartControl1.SeriesTemplate.Label.PointOptions.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
            chartControl1.SeriesTemplate.Label.LineVisible = true;

            //configure chart seriesTemplate View
            DevExpress.XtraCharts.PieSeriesView pieSeriesView2 = new DevExpress.XtraCharts.PieSeriesView();
            pieSeriesView2.RuntimeExploding = false;
            chartControl1.SeriesTemplate.View = pieSeriesView2;
            chartControl1.Size = new System.Drawing.Size(500, 500);
            chartControl1.TabIndex = chartIndex;

            return chartControl1;
        }



        private ChartControl getBarChartControl(string variableName, Dictionary<string, int> platformDict, int chartIndex)
        {
            ChartControl chartControl1 = new ChartControl();

            // normalConfigure
            chartControl1.Dock = System.Windows.Forms.DockStyle.Top;
            chartControl1.EmptyChartText.Text = "\r\n";
            chartControl1.Legend.Visible = false;
            chartControl1.Location = new System.Drawing.Point(0, 0);
            chartControl1.Name = "chartControl" + chartIndex;


            //configure pie series
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            series1.Label.LineVisible = true;
            series1.Name = "Series 1";
            List<SeriesPoint> seriesPointLIst = new List<SeriesPoint>();
            foreach (KeyValuePair<string, int> kv in platformDict)
            {
                DevExpress.XtraCharts.SeriesPoint seriesPoint = new DevExpress.XtraCharts.SeriesPoint(kv.Key, new object[] {
                ((object)(kv.Value))});
                seriesPointLIst.Add(seriesPoint);
            }
            series1.Points.AddRange(seriesPointLIst.ToArray());
            // configure chart series array
            chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
             series1};

            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            chartTitle1.Text = variableName;
            chartTitle1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});

            chartControl1.SeriesTemplate.Label.PointOptions.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
            chartControl1.SeriesTemplate.Label.LineVisible = true;

            //configure chart  size
            chartControl1.Size = new System.Drawing.Size(500, 500);
            chartControl1.TabIndex = chartIndex;

            return chartControl1;
        }

        #endregion
    
    }
}

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
        string lastFocusName;

        private void treeList1_NodeCellStyle(object sender, GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Node.Focused)
            {
                e.Appearance.ForeColor = Color.White;
                e.Appearance.BackColor = Color.Black;
                // e.Appearance.Font.Bold = true;
            }
        }

        private void treeList1_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs e)
        {
            if (e.Node.GetValue("name").Equals("General") && lastFocusName.Equals("Dependence Setting"))
            {
                this.saveCurrentDependenceRecord();
                if (checkDependenceRecordValid())
                {
                    e.CanFocus = true;
                }
                else
                {
                    e.CanFocus = false;
                }

            }
            else
            {
                e.CanFocus = true;
            }
            
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
                        this.optionTabControl.SelectedTabPage = this.generalTabPage;
                        break;
                    case "Dependence Setting":
                        this.optionTabControl.SelectedTabPage = this.dependenceSettingTabPage;
                        break;
                }
            }
            lastFocusName = e.Node.GetValue("name") as string;
        }

    }
}

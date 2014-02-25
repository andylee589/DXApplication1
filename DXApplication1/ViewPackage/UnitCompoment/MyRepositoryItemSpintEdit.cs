using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors.Repository;
namespace DXApplication1.ViewPackage
{
   public  class MyRepositoryItemSpintEdit : RepositoryItemSpinEdit
    {
       public MyRepositoryItemSpintEdit() :base()
       {
           
                    
           this.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
           this.EditMask = "p0";
           this.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            131072});
          
       }

    }
}

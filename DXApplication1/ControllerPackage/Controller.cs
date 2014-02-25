using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using DXApplication1.ViewPackage;
using DXApplication1.DataModelPackage;
using DXApplication1.ControllerPackage;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using DXApplication1.ViewPackage;
namespace DXApplication1
{
   partial class Controller
    {
        // a singleton class 
        private static Controller instance;
        //project data
        public Project project;
        public MainForm mainForm { get; set; }                
        // work folder
        string  workFolder ="c:\\xml\\";
              

      //  Xml Serializer for analysis strategy object
        private Controller()
        {
            createPath(workFolder);
            createPath(workFolder + strategyFileFoder);
        }
       
        public static Controller getInstance()
        {
            if (instance == null)
            {
                 instance=new Controller();
            }
            return instance;
        }


        // custom the show message dialog
        public bool showErrorMessage(string errorMessage)
        {
            
             MessageDxUnit.ShowError(errorMessage);
             return false;
        }

        public void createPath(string path)
        {
            // create file folder
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

    }
}

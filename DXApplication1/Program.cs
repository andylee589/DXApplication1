using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;


namespace DXApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            UserLookAndFeel.Default.SetSkinStyle("Office 2007 Black");
            MainForm mainForm = new MainForm();
            Application.Run(mainForm);
          //  Application.Run(new Form1());
           
        }
    }
}
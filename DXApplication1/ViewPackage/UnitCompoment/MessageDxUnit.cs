using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.LookAndFeel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
namespace DXApplication1.ViewPackage
{
    
   public static  class MessageDxUnit
    {
        ///<summary>
        ///a Message unit class for the encapsulation of  DevExpress Message Dialog
        /// <summary>
        /// show normal prompt message
        /// </summary>
        /// <param name="message">prompt message</param>
        public static DialogResult ShowTips(string message)
        {
            return XtraMessageBox.Show(message, "Prompt Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// show warning message
        /// </summary>
        /// <param name="message">warnning message</param>
        public static DialogResult ShowWarning(string message)
        {
            return XtraMessageBox.Show(message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// show error message
        /// </summary>
        /// <param name="message">error message</param>
        public static DialogResult ShowError(string message)
        {
            return XtraMessageBox.Show(message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// show query message for users  and show error icon
        /// </summary>
        /// <param name="message">error messsage</param>
        public static DialogResult ShowYesNoAndError(string message)
        {
            return XtraMessageBox.Show(message, "Error Message", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
        }

        /// <summary>
        ///show query message for users and show prompt icon
        /// </summary>
        /// <param name="message">Tip message</param>
        public static DialogResult ShowYesNoAndTips(string message)
        {
            return XtraMessageBox.Show(message, "Prompt Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        /// <summary>
        /// show query message for users and show warning icon
        /// </summary>
        /// <param name="message">warning message</param>
        public static DialogResult ShowYesNoAndWarning(string message)
        {
            return XtraMessageBox.Show(message, "Warning Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// show query message for users and show prompt icon
        /// </summary>
        /// <param name="message">Error Message</param>
        public static DialogResult ShowYesNoCancelAndTips(string message)
        {
            return XtraMessageBox.Show(message, "Error Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
        }

    }
}

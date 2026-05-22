using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Icondev.MedManage
{
    public partial class SelectDocumentTypeDialog : Form
    {
        public string DocumentType { get; set; }
        /// <summary>
        /// ok/cancel
        /// </summary>
        public SelectDocumentTypeDialog()
        {
            InitializeComponent();
            rbDocumentLinkToCase.Focus();
        }

        private void btnDocumentLinkOk_Click(object sender, EventArgs e)
        {
            OkClick();
        }

        private void btnDocumentLinkCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void rbDocumentLink_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                OkClick();
            }
        }

        private void OkClick()
        {
            if (rbDocumentLinkToCase.Checked)
            {
                DocumentType = "Case";
            }
            else
            {
                DocumentType = "Member";
            }
            
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


    }
}

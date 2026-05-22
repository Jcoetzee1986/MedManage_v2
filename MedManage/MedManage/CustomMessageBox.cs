using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Icondev.MedManage.MedManageLib
{
    public partial class CustomMessageBox : Form
    {
        private string caption;
        private string message;
        
        public CustomMessageBox(string text,string header)
        {
            caption = header;
            message = text;
            
            InitializeComponent();
            textBox1.Text = message;
            this.Text = caption;
            btnYes.Visible = false;
            btnNo.Visible = false;
        }

        public CustomMessageBox(string text, string header, bool YesNo)
        {
            caption = header;
            message = text;

            InitializeComponent();
            textBox1.Text = message;
            this.Text = caption;
            if (YesNo)
                OkButton.Visible = false;
            else
            {
                btnYes.Visible = false;
                btnNo.Visible = false;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        public string CustomMessage
        {
            get { return message; }
            set { message = value; }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}

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
    public partial class CustomMessageBoxDateSelector : Form
    {
        public CustomMessageBoxDateSelector(string message, string caption, DateTime date)
        {
            InitializeComponent();
            Text = caption;

            dateSelector.Value = date;
            lblMessage.Text = message;
        }
        public DateTime GetDate()
        {
            return dateSelector.Value;
        }

        private void btnCloseDialog_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

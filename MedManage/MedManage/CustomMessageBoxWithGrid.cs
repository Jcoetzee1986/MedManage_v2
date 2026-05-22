using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Icondev.MedManage.MedManageLib
{
    public partial class CustomMessageBoxWithGrid : Form
    {
        DataGridViewRow oDr;
        int index;

        /// <summary>
        /// yes/cancel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="header"></param>
        /// <param name="text"></param>
        public CustomMessageBoxWithGrid(DataTable data, string header, string text)
        {
            InitializeComponent();
            grdDisplay.DataSource = data;
            grdDisplay.Rows[0].Selected = true;
            lblMessage.Text = text;
            this.Text = header;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        public CustomMessageBoxWithGrid(DataTable data, string header, string text, bool AllowYes)
        {
            InitializeComponent();
            grdDisplay.DataSource = data;
            grdDisplay.Rows[0].Selected = true;
            lblMessage.Text = text;
            this.Text = header;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            if (!AllowYes)
            {
                btnClose.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            if (grdDisplay.SelectedRows == null)
            {
                oDr = grdDisplay.Rows[0];
                index = 0;
            }
            else
            {
                oDr = grdDisplay.Rows[grdDisplay.CurrentRow.Index];
                index = grdDisplay.CurrentRow.Index;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        public DataGridViewRow SelectedRow
        {
            get { return oDr; }
        }
        public int selectedIndex
        {
            get { return index; }
        }

        private void grdDisplay_DoubleClick(object sender, EventArgs e)
        {
            if (grdDisplay.SelectedRows == null)
            {
                oDr = grdDisplay.Rows[0];
                index = 0;
            }
            else
            {
                oDr = grdDisplay.Rows[grdDisplay.CurrentRow.Index];
                index = grdDisplay.CurrentRow.Index;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            index = -1;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void grdDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (grdDisplay.SelectedRows == null)
                {
                    oDr = grdDisplay.Rows[0];
                    index = 0;
                }
                else
                {
                    oDr = grdDisplay.Rows[grdDisplay.CurrentRow.Index -1];
                    index = grdDisplay.CurrentRow.Index -1;
                }
                if (btnClose.Enabled)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
                this.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;

namespace Icondev.MedManage
{
    public partial class Exclusions : Form
    {
        SharedObjects oShared;
        public Exclusions()
        {
            InitializeComponent();
            oShared = new SharedObjects(Program.oDb);
            datagridExclusions.DataSource = oShared.usp_Exclusion_Select();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            oShared.usp_Exclusion_Insert(txtExclusionName.Text, txtExclusionDescription.Text, Program.Username);
            datagridExclusions.DataSource = oShared.usp_Exclusion_Select();
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

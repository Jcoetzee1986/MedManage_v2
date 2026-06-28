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
    public partial class ChronicIllnesses : Form
    {
        SharedObjects oShared;
        public ChronicIllnesses()
        {
            InitializeComponent();
            oShared = new SharedObjects(Program.oDb);
            datadridChronicIllness.DataSource = oShared.usp_ChronicIllness_Select();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            oShared.usp_ChronicIllness_Insert(txtChronicIllnessName.Text, txtChronicIllnessDescription.Text, Program.Username);
            datadridChronicIllness.DataSource = oShared.usp_ChronicIllness_Select();
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

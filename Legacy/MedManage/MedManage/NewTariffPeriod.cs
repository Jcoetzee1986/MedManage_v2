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
    public partial class NewTariffPeriod : Form
    {
        public int TariffNameID { get; set; }
        public string TariffPeriodName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Multiplier { get; set; }
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public NewTariffPeriod(int tariffNameID)
        {
            InitializeComponent();
            TariffNameID = tariffNameID;
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            DataTable oDt = oShared.usp_Tariff_CheckPeriodExist(
                TariffNameID
                , txtNewTariffPeriodName.Text
                , dateStartDate.Value
                , dateEndDate.Value);
            if (oDt.Rows.Count > 0)
            {
                MessageBox.Show("Thie period either already exists, or the start and end dates overlap with another period for this tariff.");
            }
            else
            {
                TariffPeriodName = txtNewTariffPeriodName.Text;
                StartDate = dateStartDate.Value;
                EndDate = dateEndDate.Value;
                Multiplier = Convert.ToDecimal(txtMultiplier.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

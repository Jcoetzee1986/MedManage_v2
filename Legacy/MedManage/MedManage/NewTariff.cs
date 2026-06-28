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
    public partial class NewTariff : Form
    {
        public string TariffName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PeriodName { get; set; }

        public NewTariff()
        {
            InitializeComponent();
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            StartDate = dateStartDate.Value;
            EndDate = dateEndDate.Value;
            TariffName = txtNewTariffName.Text;
            PeriodName = txtNewPeriodName.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}

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
    public partial class BaseTariffAdd : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public BaseTariffAdd()
        {
            InitializeComponent();
            txtTariffCode.Text = oShared.usp_BaseTariff_Select_NewCustomCode().Rows[0][0].ToString();
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picAddCode_Click(object sender, EventArgs e)
        {
            try
            {
                oShared.usp_BaseTariff_InsertCustom(txtTariffCode.Text
                    , 0 //Allways custom/other
                    , txtTariffDesc.Text
                    , Program.Username);
            }
            catch (Exception err)
            {
                if (err.Message.Contains("Violation of PRIMARY KEY constraint"))
                    MessageBox.Show("This code already exists");
                else throw;
            }
            finally
            {
                this.Close();
            }
        }
    }
}

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
    public partial class TariffManagement : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        string SelectedCode { get; set; }
        public DataGridViewRow oDr { get; private set; }

        public TariffManagement()
        {
            InitializeComponent();

            comboTariffName.DisplayMember = "TariffName";
            comboTariffName.ValueMember = "TariffNameID";
            comboTariffName.DataSource = oShared.usp_TariffName_Select();

            comboHospitalType.DisplayMember = "Speciality";
            comboHospitalType.ValueMember = "SpecialityID";
            comboHospitalType.DataSource = oShared.usp_Speciality_Select_WhereThereAreTariffs();

            lblTariff.Visible = true;
            comboTariffName.Visible = true;

            comboSelectTariff.DisplayMember = "TariffName";
            comboSelectTariff.ValueMember = "TariffNameID";
            comboSelectTariff.DataSource = oShared.usp_TariffName_Select();

        }

        private void picNew_Click(object sender, EventArgs e)
        {
            NewTariff oNew = new NewTariff();
            DialogResult oResult = oNew.ShowDialog();
            if (oResult == System.Windows.Forms.DialogResult.OK)
            {
                oShared.usp_Tariff_Insert(oNew.TariffName, oNew.PeriodName, oNew.StartDate, oNew.EndDate, Program.Username);
                comboSelectTariff.DataSource = oShared.usp_TariffName_Select();
            }
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboSelectTariff_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboTariffPeriod.DisplayMember = "TariffPeriodName";
            comboTariffPeriod.ValueMember = "TariffPeriodName";
            comboTariffPeriod.DataSource = oShared.usp_Tariff_SelectPeriodName_ByTariffNameID(
                Convert.ToInt32(comboSelectTariff.SelectedValue));
        }

        private void datagridTariff_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            oShared.usp_Tariff_Update_Tariff(Convert.ToInt32(datagridTariff.Rows[e.RowIndex].Cells["TariffID"].Value), Convert.ToDecimal(datagridTariff.Rows[e.RowIndex].Cells["Tariff"].Value), Program.Username);
        }

        private void comboSpeciality_SelectedIndexChanged(object sender, EventArgs e)
        {
            datagridTariff.DataSource = oShared.usp_Tariff_Select_ByNameID_Period_Speciality(
                Convert.ToInt32(comboSelectTariff.SelectedValue)
                , comboTariffPeriod.SelectedValue.ToString()
                , Convert.ToInt32(comboType.SelectedValue));
            for (int i = 0; i < datagridTariff.Columns.Count; i++)
            {
                if (datagridTariff.Columns[i].Name != "Tariff")
                {
                    datagridTariff.Columns[i].ReadOnly = true;
                }
            }
            datagridTariff.Columns["TariffID"].Visible = false;
            datagridTariff.Columns["TariffName"].Visible = false;
        }

        private void comboTariffPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboType.DisplayMember = "Speciality";
            comboType.ValueMember = "SpecialityID";
            comboType.DataSource = oShared.usp_Speciality_Select_ByTariffNameID(Convert.ToInt32(comboSelectTariff.SelectedValue));
        }

        private void picNewPeriod_Click(object sender, EventArgs e)
        {
            NewTariffPeriod oNew = new NewTariffPeriod(Convert.ToInt32(comboSelectTariff.SelectedValue));
            DialogResult oResult = oNew.ShowDialog();
            if (oResult == System.Windows.Forms.DialogResult.OK)
            {
                oShared.usp_Tariff_InsertNewPeriod(
                    Convert.ToInt32(comboSelectTariff.SelectedValue)
                    , oNew.TariffPeriodName
                    , oNew.StartDate
                    , oNew.EndDate
                    , oNew.Multiplier
                    , Program.Username);
                comboSelectTariff.DataSource = oShared.usp_Tariff_SelectDistinctDetails();
            }
        }

        private void picAddCustomTariff_Click(object sender, EventArgs e)
        {
            BaseTariffAdd oAdd = new BaseTariffAdd();
            oAdd.ShowDialog();
        }

        private void tabTariffManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabTariffManagement.SelectedTab == tabNewPeriod)
            {
                if (Program.Username.ToLower() == "systemadmin")
                {
                }
                else
                {
                    tabTariffManagement.SelectedTab = tabSingleTariff;
                    MessageBox.Show("Only the system admin can access this screen");
                }
            }
        }

        private void picCodeLookup_Click(object sender, EventArgs e)
        {
            datagridSearchResults.DataSource = oShared.usp_Tariff_Select_ByFilters(txtCode.Text
                            , txtDesc.Text
                            , 0
                            , 0
                //, Convert.ToInt32(comboHospitalType.SelectedValue.ToString())
                            , Convert.ToInt32(comboTariffName.SelectedValue.ToString()));
        }

        private void picAddCode_Click(object sender, EventArgs e)
        {
            if (txtTariffCode.Text != "" || txtTariffDesc.Text == "")
            {

                try
                {
                    oShared.usp_BaseTariff_InsertCustom(txtTariffCode.Text
                        , Convert.ToInt32(comboHospitalType.SelectedValue)
                        , txtTariffDesc.Text
                        , Program.Username);
                    MessageBox.Show("Added Successfully");
                }
                catch (Exception err)
                {
                    if (err.Message.Contains("Violation of PRIMARY KEY constraint"))
                        MessageBox.Show("This code already exists");
                    else throw;
                }
            }
            else
            {
                MessageBox.Show("Please enter a tariff code and description");
            }
        }
    }
}

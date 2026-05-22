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
    public partial class CodeLookup : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        string CodeType;
        int CaseID;
        string SelectedCode { get; set; }
        public DataGridViewRow oDr { get; private set; }

        public CodeLookup(string codeType, int caseID)
        {
            InitializeComponent();
            CodeType = codeType;
            CaseID = caseID;
            switch (CodeType)
            {
                case "ICD":
                    {
                        this.Text = "ICD Lookup";
                        break;
                    }
                case "CPT":
                    {
                        this.Text = "CPT Lookup";
                        break;
                    }
                case "Tarrif":
                    {
                        this.Text = "Tarrif Lookup";
                        //comboHospitalType.DataSource = oShared.usp_Speciality_Select_WhereThereAreTariffs();
                        //comboHospitalType.DisplayMember = "Speciality";
                        //comboHospitalType.ValueMember = "SpecialityID";

                        comboTariffName.DisplayMember = "TariffName";
                        comboTariffName.ValueMember = "TariffNameID";
                        comboTariffName.DataSource = oShared.usp_TariffName_Select();

                        lblTariff.Visible = true;
                        comboTariffName.Visible = true;
                        //comboHospitalType.Visible = true;
                        //picAddBaseTariff.Visible = false;
                        break;
                    }

                default:
                    break;
            }
        }

        private void picCodeLookup_Click(object sender, EventArgs e)
        {
            switch (CodeType)
            {
                case "ICD":
                    {
                        datagridSearchResults.DataSource = oShared.usp_ICD_Select_ByFilters(txtCode.Text, txtDesc.Text);
                        break;
                    }
                case "CPT":
                    {
                        datagridSearchResults.DataSource = oShared.usp_CPT_Select_ByFilters(txtCode.Text, txtDesc.Text);
                        break;
                    }
                case "Tarrif":
                    {
                        //datagridSearchResults.DataSource = oShared.usp_Tariff_Select_ByFilters(txtCode.Text
                        //    , txtDesc.Text
                        //    , CaseID
                        //    , 0
                        //    //, Convert.ToInt32(comboHospitalType.SelectedValue.ToString())
                        //    , Convert.ToInt32(comboTariffName.SelectedValue.ToString()));
                        break;
                    }

                default:
                    break;
            }
        }

        private void picSelectCode_Click(object sender, EventArgs e)
        {
            oDr = datagridSearchResults.CurrentRow;
            this.Close();
        }

        //private void picAddBaseTariff_Click(object sender, EventArgs e)
        //{
        //    BaseTariffAdd oAdd = new BaseTariffAdd();
        //    oAdd.ShowDialog();
        //}

        private void datagridSearchResults_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            oDr = datagridSearchResults.CurrentRow;
            this.Close();
        }

    }
}

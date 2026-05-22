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
    public partial class TariffCodeLookup : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        string CodeType;
        int CaseID;
        string SelectedCode { get; set; }
        public DataGridViewRow oDr { get; private set; }
        int ServiceProviderID;
        DateTime TreatmentDate;

        public TariffCodeLookup(int caseID)
        {
            InitializeComponent();
            CaseID = caseID;

            DataTable oDtProviderDetails = oShared.usp_ProviderIDs_TreatmentDate_Select_ByCaseID(CaseID);
            ServiceProviderID = Convert.ToInt32(oDtProviderDetails.Rows[0]["ServiceProviderID"].ToString());
            TreatmentDate = Convert.ToDateTime(oDtProviderDetails.Rows[0]["DischargeDate"].ToString());

            this.Text = "Tarrif Lookup";
            comboHospitalType.DataSource = oShared.usp_Speciality_Select_WhereThereAreTariffs();
            comboHospitalType.DisplayMember = "Speciality";
            comboHospitalType.ValueMember = "SpecialityID";

            comboTariffName.DisplayMember = "TariffName";
            comboTariffName.ValueMember = "TariffNameID";
            comboTariffName.DataSource = oShared.usp_TariffName_Select();

            
            comboHospitalType.SelectedValue = oDtProviderDetails.Rows[0]["SpecialityID"].ToString();
            comboTariffName.SelectedValue = oDtProviderDetails.Rows[0]["TariffNameID"].ToString();


        }

        //private void picCodeLookup_Click(object sender, EventArgs e)
        //{

        //    datagridSearchResults.DataSource = oShared.usp_Tariff_Select_ByFilters(txtCode.Text
        //        , txtDesc.Text
        //        , CaseID
        //        , 0
        //        //, Convert.ToInt32(comboHospitalType.SelectedValue.ToString())
        //        , Convert.ToInt32(comboTariffName.SelectedValue.ToString()));

        //}

        private void picSelectCode_Click(object sender, EventArgs e)
        {
            oDr = datagridSearchResults.CurrentRow;
            this.Close();
        }

        private void datagridSearchResults_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            oDr = datagridSearchResults.CurrentRow;
            this.Close();
        }

        private void datagridSearchResults_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                
            }
        }

        private void datagridSearchResults_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void comboHospitalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboHospitalType.SelectedValue.ToString() != "System.Data.DataRowView")
            //{
            //    datagridSearchResults.DataSource = oShared.usp_ServiceProvider_Tariff_Select_ById(ServiceProviderID, Convert.ToInt32(comboHospitalType.SelectedValue),TreatmentDate);
            //    int LastIndex = datagridSearchResults.Rows.Count - 1;
            //    datagridSearchResults.Rows[LastIndex].ReadOnly = true;
            //}
        }

       
        private void datagridSearchResults_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

      
    }
}

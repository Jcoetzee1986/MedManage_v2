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
    public partial class rpt_Finance : Form
    {
        public rpt_Finance()
        {
            MedManageLib.CaseManagement oCM = new MedManageLib.CaseManagement(Program.oDb);
            SharedObjects oShared = new SharedObjects(Program.oDb);

            InitializeComponent();
            try
            {
                comboMedicalFunder.DisplayMember = "MedicalAidName";
                comboMedicalFunder.ValueMember = "MedicalAidID";
                comboMedicalFunder.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);
                //comboMedicalFunder.SelectedValue = Program.MainClientID;
            }
            catch
            {
            }
        }

        private void rpt_Finance_Load(object sender, EventArgs e)
        {
            //ReportsDataSet.EnforceConstraints = false;
            //// INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Finance_Select_Filters' table. You can move, or remove it, as needed.
            //this.usp_rpt_Finance_Select_FiltersTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Finance_Select_Filters
            //    , txtCaseNumber.Text
            //    , txtMemberNumber.Text
            //    , txtMemberName.Text
            //    , txtMemberSurname.Text
            //    , (chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
            //    , (chkDateCreatedSearch.Checked ? dateCaseCreated.Value : Program.ConvertIntToDate(19000101))
            //    , txtPracticeName.Text
            //    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
            //    , txtPrimaryIcd.Text
            //    , txtPrimaryCpt.Text);

            //this.reportViewer1.RefreshReport();
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Finance_Select_Filters' table. You can move, or remove it, as needed.
            this.usp_rpt_Finance_Select_FiltersTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Finance_Select_Filters
                , txtCaseNumber.Text
                , txtMemberNumber.Text
                , txtMemberName.Text
                , txtMemberSurname.Text
                , (chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
                , (chkDateCreatedSearch.Checked ? dateCaseCreated.Value : Program.ConvertIntToDate(19000101))
                , txtPracticeName.Text
                , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
                , txtPrimaryIcd.Text
                , txtPrimaryCpt.Text
                , Program.MainClientID);

            this.reportViewer1.RefreshReport();
        }
    }
}

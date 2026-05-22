using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib;
using Icondev.MedManage.MedManageLib.Shared;

namespace Icondev.MedManage
{
    public partial class rpt_Cases_BetweenAdmitDates : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public rpt_Cases_BetweenAdmitDates()
        {
            InitializeComponent();

            comboCaseStatus.DataSource = oCM.usp_CaseStatus_Select();
            comboCaseStatus.DisplayMember = "CaseStatus";
            comboCaseStatus.ValueMember = "CaseStatusID";

            comboMedicalFunder.DisplayMember = "MedicalAidName";
            comboMedicalFunder.ValueMember = "MedicalAidID";
            comboMedicalFunder.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);

            //comboMedicalFunder.SelectedValue = Program.MainClientID;
        }

        private void rpt_Cases_BetweenDates_Load(object sender, EventArgs e)
        {
            ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Cases_Select_Filters_BetweenDates' table. You can move, or remove it, as needed.
            this.usp_rpt_Cases_Select_Filters_BetweenAdmissionDatesTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_Filters_BetweenAdmissionDates
                , txtCaseNumber.Text
                    , txtMemberNumber.Text
                    , txtMemberName.Text
                    , txtMemberSurname.Text
                    , (chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
                    , dateFrom.Value
                    , dateTo.Value
                    , txtPracticeName.Text
                    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
                    , txtPrimaryIcd.Text
                    , txtPrimaryCpt.Text
                    , txtUser.Text
                    , (chkStatus.Checked ? Convert.ToInt32(comboCaseStatus.SelectedValue) : -1)
                , Program.MainClientID);

            this.reportViewer1.RefreshReport();
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            ReportsDataSet.EnforceConstraints = false;

            this.usp_rpt_Cases_Select_Filters_BetweenAdmissionDatesTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_Filters_BetweenAdmissionDates
                , txtCaseNumber.Text
                    , txtMemberNumber.Text
                    , txtMemberName.Text
                    , txtMemberSurname.Text
                    , (chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
                    , dateFrom.Value
                    , dateTo.Value
                    , txtPracticeName.Text
                    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
                    , txtPrimaryIcd.Text
                    , txtPrimaryCpt.Text
                    , txtUser.Text
                    , (chkStatus.Checked ? Convert.ToInt32(comboCaseStatus.SelectedValue) : -1)
                , Program.MainClientID);

            this.reportViewer1.RefreshReport();
        }
    }
}

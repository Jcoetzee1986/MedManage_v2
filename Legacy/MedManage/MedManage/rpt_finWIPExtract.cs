using Icondev.MedManage.MedManageLib;
using Icondev.MedManage.MedManageLib.Shared;
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
    public partial class rpt_finWIPExtract : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        SharedObjects oShared = new SharedObjects(Program.oDb);
        Finance oFin = new Finance(Program.oDb);

        AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
        AutoCompleteStringCollection numbersCollection = new AutoCompleteStringCollection();
        bool initialLoad = false;

        public rpt_finWIPExtract()
        {


            try
            {
                initialLoad = true;

                InitializeComponent();

                comboCaseStatus.DataSource = oCM.usp_CaseStatus_Select();
                comboCaseStatus.DisplayMember = "CaseStatus";
                comboCaseStatus.ValueMember = "CaseStatusID";
                comboCaseStatus.SelectedText = "Closed";

                comboBillingStatus.DataSource = oFin.usp_BillingStatus_Select();
                comboBillingStatus.DisplayMember = "BillingStatus";
                comboBillingStatus.ValueMember = "BillingStatusID";
                comboBillingStatus.SelectedText = "New";

                lblCurrentPracticeID.Text = "";
                dateFinalInvoiceAmountStart.Value = DateTime.Today.AddDays(-30);

                DataTable oDt = oShared.usp_ServiceProvider_Select_Autocomplete();
                string[] oResultName = new string[oDt.Rows.Count];
                for (int i = 0; i < oDt.Rows.Count; i++)
                {
                    oResultName[i] = oDt.Rows[i][1].ToString() + " - " + oDt.Rows[i][0].ToString();
                }
                string[] oResultNumber = new string[oDt.Rows.Count];
                for (int i = 0; i < oDt.Rows.Count; i++)
                {
                    oResultNumber[i] = oDt.Rows[i][0].ToString() + " - " + oDt.Rows[i][1].ToString();
                }

                numbersCollection.AddRange(oResultNumber);
                txtLookupPracticeNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
                //comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtLookupPracticeNumber.AutoCompleteCustomSource = numbersCollection;

                namesCollection.AddRange(oResultName);
                txtLookupPracticeName.AutoCompleteMode = AutoCompleteMode.Suggest;
                //comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtLookupPracticeName.AutoCompleteCustomSource = namesCollection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                initialLoad = false;
            }
        }

        //private void rpt_Finance_Load(object sender, EventArgs e)
        //{
        //    //ReportsDataSet.EnforceConstraints = false;
        //    //// INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Finance_Select_Filters' table. You can move, or remove it, as needed.
        //    //this.usp_rpt_Finance_Select_FiltersTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Finance_Select_Filters
        //    //    , txtCaseNumber.Text
        //    //    , txtMemberNumber.Text
        //    //    , txtMemberName.Text
        //    //    , txtMemberSurname.Text
        //    //    , (chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
        //    //    , (chkDateCreatedSearch.Checked ? dateCaseCreated.Value : Program.ConvertIntToDate(19000101))
        //    //    , txtPracticeName.Text
        //    //    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
        //    //    , txtPrimaryIcd.Text
        //    //    , txtPrimaryCpt.Text);

        //    //this.reportViewer1.RefreshReport();
        //}

        private void picRefresh_Click(object sender, EventArgs e)
        {
            ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Finance_Select_Filters' table. You can move, or remove it, as needed.

            if (txtLookupPracticeNumber.Text != ""
                && txtLookupPracticeNumber.AutoCompleteCustomSource.Contains(txtLookupPracticeNumber.Text))
            {
                lblSelectedPracticeName.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeNumber.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();
                //datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }
            else if (txtLookupPracticeName.Text != ""
                && txtLookupPracticeName.AutoCompleteCustomSource.Contains(txtLookupPracticeName.Text))
            {
                lblSelectedPracticeName.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeName.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();
                //datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }

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
            //    , txtPrimaryCpt.Text
            //    , Program.MainClientID);

            this.reportViewer1.RefreshReport();
        }
    }
}

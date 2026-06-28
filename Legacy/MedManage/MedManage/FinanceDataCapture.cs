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
    public partial class FinanceDataCapture : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        SharedObjects oShared = new SharedObjects(Program.oDb);
        Finance oFin = new Finance(Program.oDb);

        AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
        AutoCompleteStringCollection numbersCollection = new AutoCompleteStringCollection();
        

        public FinanceDataCapture()
        {
            InitializeComponent();

            lblBillingSelectedRecord.Text = "";
            lblCurrentPracticeID.Text = "";
            lblReceivedByName.Text = "";
            dateReceivedStart.Value = DateTime.Today.AddDays(-30);

            comboBillingStatus.DataSource = oFin.usp_BillingStatus_Select();
            comboBillingStatus.DisplayMember = "BillingStatus";
            comboBillingStatus.ValueMember = "BillingStatusID";

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
        private void BillingFormEnter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                BillingSave();
                txtBillingAccountNumber.Focus();
                txtBillingAccountNumber.SelectAll();
            }
        }

        private void picBillingSave_Click(object sender, EventArgs e)
        {
            BillingSave();
        }

        private void BillingSave()
        {
            //access
            //"Billing Auditing"
            //"System Administrator"
            //"Metadata Administrator"
            //"Imports"
            //"Case Manager"
            if (Program._GenericPrincipal.IsInRole("Billing Auditing"))
            {
                try
                {
                    if (chkBillingNewRecord.Checked
                        || grdBilling.CurrentRow == null)
                    {
                        DataTable oDt = oFin.usp_Case_Billing_CheckDuplicates(
                            -1
                            , txtBillingAccountNumber.Text
                            , Convert.ToInt32(lblCurrentPracticeID.Text));

                        DialogResult ores = new DialogResult();
                        ores = DialogResult.Yes;
                        if (oDt.Rows.Count > 0)
                        {
                            CustomMessageBoxWithGrid oMsg = new CustomMessageBoxWithGrid(oDt
                                , "Duplicate account"
                                , "This account number for the same provider has already been used in the cases listed below\n\rDo you want to continue?"
                                , false);

                            ores = oMsg.ShowDialog();
                        }

                        if (ores != DialogResult.Cancel)
                        {
                            DataTable oDtNewID = oFin.usp_Case_Billing_Insert(
                                Convert.ToInt32(lblCurrentPracticeID.Text)
                                , dateBillingAccountDateFrom.Value
                                , dateBillingAccountDateTo.Value
                                , txtBillingAccountNumber.Text
                                , txtBillingInvoiceNumber.Text
                                , dateBillingReceived.Value
                                , dateBillingSubmitted.Value
                                , Program.Username
                                , txtPatientInitials.Text
                                , txtPatientSurname.Text
                                , Convert.ToDecimal((txtBillingAmountDue.Text == "" ? "0.00" : txtBillingAmountDue.Text))
                                //, txtBillingComment.Text
                                , Convert.ToInt32(comboBillingStatus.SelectedValue.ToString())
                                , DateTime.Now
                                , Program.Username);

                            lblBillingSelectedRecord.Text = oDtNewID.Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        DataTable oDt = oFin.usp_Case_Billing_CheckDuplicates(
                           Convert.ToInt32(lblBillingSelectedRecord.Text)
                           , txtBillingAccountNumber.Text
                           , Convert.ToInt32(lblCurrentPracticeID.Text));

                        DialogResult ores = new DialogResult();
                        ores = DialogResult.Yes;
                        if (oDt.Rows.Count > 0)
                        {
                            CustomMessageBoxWithGrid oMsg = new CustomMessageBoxWithGrid(oDt
                                , "Duplicate account"
                                , "This account number for the same provider has already been used in the cases listed below\n\rDo you want to continue?");

                            ores = oMsg.ShowDialog();
                        }

                        if (ores != System.Windows.Forms.DialogResult.Cancel)
                            oFin.usp_Case_Billing_Update(
                                Convert.ToInt32(lblBillingSelectedRecord.Text)
                                , Convert.ToInt32(lblCurrentPracticeID.Text)
                                , dateBillingAccountDateFrom.Value
                                , dateBillingAccountDateTo.Value
                                , txtBillingAccountNumber.Text
                                , txtBillingInvoiceNumber.Text
                                , dateBillingReceived.Value
                                , dateBillingSubmitted.Value
                                , Program.Username
                                , txtPatientInitials.Text
                                , txtPatientSurname.Text
                                , Convert.ToDecimal((txtBillingAmountDue.Text == "" ? "0.00" : txtBillingAmountDue.Text))
                                //, txtBillingComment.Text
                                , Convert.ToInt32(comboBillingStatus.SelectedValue.ToString())
                                , DateTime.Now
                                , Program.Username);
                    }

                    SaveBillingComment();


                    BindBillingDataGrid();
                    //datagridBillingSmall.DataSource = oCM.usp_Case_Billing_Select_Summary(CaseID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Billing and paid amounts need to be numeric values");
                }
            }//Access
        }

        private void datagridBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grdBilling.CurrentRow.Index > -1)
            {
                try
                {
                    lblBillingSelectedRecord.Text = grdBilling.Rows[e.RowIndex].Cells["Case_BillingID"].Value.ToString();
                    dateBillingAccountDateFrom.Value = Convert.ToDateTime(grdBilling.Rows[e.RowIndex].Cells["AccountDate"].Value.ToString());
                    txtBillingAccountNumber.Text = grdBilling.Rows[e.RowIndex].Cells["AccountNumber"].Value.ToString();
                    txtBillingInvoiceNumber.Text = grdBilling.Rows[e.RowIndex].Cells["InvoiceNumber"].Value.ToString();
                    dateBillingReceived.Value = Convert.ToDateTime(grdBilling.Rows[e.RowIndex].Cells["DateReceived"].Value.ToString());
                    chkBillingSubmitted.Checked = Convert.ToBoolean(grdBilling.Rows[e.RowIndex].Cells["Submitted"].Value.ToString());
                    if (grdBilling.Rows[e.RowIndex].Cells["DateSubmitted"].Value.ToString() != "")
                        dateBillingSubmitted.Value = Convert.ToDateTime(grdBilling.Rows[e.RowIndex].Cells["DateSubmitted"].Value.ToString());
                    lblReceivedByName.Text = grdBilling.Rows[e.RowIndex].Cells["ReceivedByName"].Value.ToString();
                    
                    txtPatientInitials.Text = grdBilling.Rows[e.RowIndex].Cells["PatientInitials"].Value.ToString();
                    txtPatientSurname.Text = grdBilling.Rows[e.RowIndex].Cells["PatientSurname"].Value.ToString();

                    txtBillingAmountDue.Text = grdBilling.Rows[e.RowIndex].Cells["AmountDue"].Value.ToString();
                    txtBillingDiscount.Text = grdBilling.Rows[e.RowIndex].Cells["Discount"].Value.ToString();
                    txtBillingPenalty.Text = grdBilling.Rows[e.RowIndex].Cells["Penalty"].Value.ToString();
                    txtBillingRejectedAmount.Text = grdBilling.Rows[e.RowIndex].Cells["Rejected"].Value.ToString();
                    
                    txtBillingFinalInvoiceAmountDue.Text = grdBilling.Rows[e.RowIndex].Cells["FinalInvoiceAmountDue"].Value.ToString();
                    txtCaseID.Text = grdBilling.Rows[e.RowIndex].Cells["CaseID"].Value.ToString();
                    txtBillingRemittance.Text = grdBilling.Rows[e.RowIndex].Cells["Remittance"].Value.ToString();
                    comboBillingStatus.SelectedValue = grdBilling.Rows[e.RowIndex].Cells["BillingStatusID"].Value.ToString();
                    lblLinkedToCase.Text = txtCaseID.Text;

                    grdBillingComments.DataSource = oFin.usp_Case_Billing_Comment_Select(
                    Convert.ToInt32(lblBillingSelectedRecord.Text));

                    //chkBillingNewRecord.Checked = false;

                    if ((lblLinkedToCase.Text == "-1"
                            || lblLinkedToCase.Text == "")
                        )
                    {
                        txtCaseID.Text = "";
                        //picBillingDelete.Visible = true;
                        txtBillingAccountNumber.Enabled = true;
                    }
                    else
                    {
                        txtCaseID.Text = lblLinkedToCase.Text;
                        //picBillingDelete.Visible = false;
                        txtBillingAccountNumber.Enabled = false;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //private void datagridBillingSmall_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        //{
        //    decimal oInvoiceAmount = 0;
        //    string oAccountNumber = "";
        //    foreach (DataGridViewRow item in datagridBillingSmall.Rows)
        //    {
        //        oInvoiceAmount += Convert.ToDecimal(item.Cells["AmountDue"].Value);
        //        oAccountNumber += (oAccountNumber == "" ? "" : ", ") + item.Cells["AccountNumber"].Value;
        //    }

        //    txtFinalInvoiceAmount.Text = oInvoiceAmount.ToString();
        //    txtAccountNumber.Text = oAccountNumber;
        //}

        private void picRptBillingSummaryForCase_Click(object sender, EventArgs e)
        {
            rpt_BillingSummaryCase oSummary = new rpt_BillingSummaryCase(-1);//CaseID);
            //oSummary.CaseID = CaseID;
            oSummary.ShowDialog();
        }

        private void picRptBillingSummaryForMember_Click(object sender, EventArgs e)
        {
            rpt_BillingSummaryMember oSummary = new rpt_BillingSummaryMember(Convert.ToInt32(txtCaseID.ToString()));
            //oSummary.CaseID = CaseID;
            oSummary.ShowDialog();
        }

        private void picRptBillingSummaryForProvider_Click(object sender, EventArgs e)
        {
            MessageBox.Show("not implimented");
            //rpt_BillingSummary oSummary = new rpt_BillingSummary()//Convert.ToInt32(lblCurrentPracticeID.Text));
            ////oSummary.CaseID = CaseID;
            //oSummary.ShowDialog();
        }

        private void picBillingDelete_Click(object sender, EventArgs e)
        {
            if (grdBilling.CurrentRow.Index > -1)
            {
                oFin.usp_Case_Billing_Delete(Convert.ToInt32(lblBillingSelectedRecord.Text));

                BindBillingDataGrid();
                //datagridBillingSmall.DataSource = oCM.usp_Case_Billing_Select_Summary(CaseID);
                lblBillingSelectedRecord.Text = "";
                chkBillingNewRecord.Checked = true;
            }
            else
            {
                MessageBox.Show("Select a billing line to delete first.");
            }
        }

        private void txtLookupPracticeNumber_Leave(object sender, EventArgs e)
        {
            if (txtLookupPracticeNumber.Text != ""
                &&txtLookupPracticeNumber.AutoCompleteCustomSource.Contains(txtLookupPracticeNumber.Text))
            {
                lblPracticeNumberLookup.Text = "";
                lblPracticeNameLookup.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeNumber.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();

                BindBillingDataGrid();
            }
            else
            {
                lblPracticeNumberLookup.Text = "Please select a value from the list";
            }
        }

        private void txtLookupPracticeName_Leave(object sender, EventArgs e)
        {
            if (txtLookupPracticeName.Text != ""
                && txtLookupPracticeName.AutoCompleteCustomSource.Contains(txtLookupPracticeName.Text))
            {
                lblPracticeNumberLookup.Text = "";
                lblPracticeNameLookup.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeName.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();

                BindBillingDataGrid();
            }
            else
            {
                lblPracticeNameLookup.Text = "Please select a value from the list";
            }
        }

        void BindBillingDataGrid()
        {
            grdBilling.DataSource = oFin.usp_Case_Billing_Select(
                Convert.ToInt32(lblCurrentPracticeID.Text)
                , dateReceivedStart.Value, dateReceivedEnd.Value
                , txtAccountNumberSearch.Text);
        }

        private void chkBillingNewRecord_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBillingNewRecord.Checked)
            {
                txtCaseID.Text = "";
            }
            else
            {
                if ((lblLinkedToCase.Text == "-1"
                        || lblLinkedToCase.Text == "")
                    )
                {
                    txtCaseID.Text = "";
                    //picBillingDelete.Visible = true;
                    txtBillingAccountNumber.Enabled = true;
                }
                else
                {
                    txtCaseID.Text = lblLinkedToCase.Text;
                    //picBillingDelete.Visible = false;
                    txtBillingAccountNumber.Enabled = false;
                }
            }

        }

        private void dateReceivedStart_Leave(object sender, EventArgs e)
        {
            if (lblCurrentPracticeID.Text != "")
            {

                BindBillingDataGrid();
            }
           
        }

        private void dateReceivedEnd_Leave(object sender, EventArgs e)
        {
            if (lblCurrentPracticeID.Text != "")
            {
                BindBillingDataGrid();
            }
        }

        private void picAddComment_Click(object sender, EventArgs e)
        {
            SaveBillingComment();
        }

        private void SaveBillingComment()
        {
            if (txtBillingComment.Text.Length > 0)
            {
                oFin.usp_Case_Billing_Comment_Insert(
                    Convert.ToInt32(lblBillingSelectedRecord.Text)
                    , txtBillingComment.Text
                    , Program.Username);
            }
            txtBillingComment.Text = "";

            grdBillingComments.DataSource = oFin.usp_Case_Billing_Comment_Select(
                    Convert.ToInt32(lblBillingSelectedRecord.Text));
        }

        private void dateBillingAccountDateFrom_ValueChanged(object sender, EventArgs e)
        {
            dateBillingAccountDateTo.Value = dateBillingAccountDateFrom.Value;
        }

        private void txtBillingComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                SaveBillingComment();
            }
        }

        private void chkBillingReported_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBillingReported.Checked)
            {
                dateBillingReported.Visible = true;
            }
        }

        private void chkBillingSubmitted_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBillingSubmitted.Checked)
            {
                dateBillingSubmitted.Visible = true;
            }
        }

        private void tsbtnSearchByAccountNumber_Click(object sender, EventArgs e)
        {
            BindBillingDataGrid();
        }

        private void txtAccountNumberSearch_Leave(object sender, EventArgs e)
        {
            BindBillingDataGrid();
        }
    }
}

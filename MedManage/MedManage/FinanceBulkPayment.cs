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
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;

namespace Icondev.MedManage
{
    public partial class FinanceBulkPayment : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        SharedObjects oShared = new SharedObjects(Program.oDb);
        Finance oFin = new Finance(Program.oDb);

        AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
        AutoCompleteStringCollection numbersCollection = new AutoCompleteStringCollection();

        bool initialLoad = false;

        public FinanceBulkPayment()
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

        private void btnLoadRemittance_Click(object sender, EventArgs e)
        {
            if (txtRemittanceNumber.Text != "")
            {
                datagridRemittanceDetail.DataSource = oFin.usp_Cases_Select_ByRemittance(txtRemittanceNumber.Text);
            }

        }

        private void datagridRemittanceDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (datagridRemittanceDetail.Rows.Count > 0)
            {
                //decimal Overcharged = 0;
                decimal Payable = 0;
                foreach (DataGridViewRow item in datagridRemittanceDetail.Rows)
                {
                    //Overcharged += Convert.ToDecimal(item.Cells["TotalOvercharged"].Value.ToString());
                    Payable += Convert.ToDecimal(item.Cells["FinalInvoiceAmount"].Value.ToString());
                    //txtTotalValueRejected.Text = Overcharged.ToString();
                }
                txtTotalValueApproved.Text = Payable.ToString();
                if (datagridRemittanceDetail.Rows[0].Cells["BillingStatusID"].Value.ToString() == "2")
                {
                    btnMarkAllAsPaid.Enabled = false;
                }
            }
        }

        private void btnPrintRemittanceDetail_Click(object sender, EventArgs e)
        {
            rpt_ReportServer oReports = new rpt_ReportServer("Remittance Detail - Payment Detail", String.Format("&Remittance={0}", txtRemittanceNumber.Text));
            oReports.MdiParent = this.MdiParent;
            oReports.Text = "Remittance Detail - Payment Detail";
            oReports.Show();
        }

        private void btnMarkAllAsPaid_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in datagridRemittanceDetail.Rows)
            {
                oFin.usp_Case_Billing_UpdateToPaid(Convert.ToInt32(item.Cells["Case_BillingID"].Value.ToString())
                    , Convert.ToDecimal(item.Cells["FinalInvoiceAmount"].Value.ToString())
                    , dateMarkAsPaidDate.Value
                    , item.Cells["Comment"].Value.ToString()
                    , Program.Username);
            }
            datagridRemittanceDetail.DataSource = oFin.usp_Cases_Select_ByRemittance(txtRemittanceNumber.Text);
        }

        private void btnOutstandingCases_Click(object sender, EventArgs e)
        {
            rpt_ReportServer oReports = new rpt_ReportServer("Remittance Detail - Not Linked To Case", String.Format("&Remittance={0}", txtRemittanceNumber.Text));
            oReports.MdiParent = this.MdiParent;
            oReports.Text = "Remittance Detail - Not Linked To Case";
            oReports.Show();
        }

        private void txtRemittanceNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnLoadRemittance.PerformClick();
            }
        }

        private void dateFinalInvoiceAmountStart_Leave(object sender, EventArgs e)
        {
            if (lblCurrentPracticeID.Text != "")
            {
                datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }

        }

        private void dateFinalInvoiceAmountEnd_Leave(object sender, EventArgs e)
        {
            if (lblCurrentPracticeID.Text != "")
            {
                datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }
        }

        private void txtLookupPracticeNumber_Leave(object sender, EventArgs e)
        {
            BindDatagridCases();
        }

        private void txtLookupPracticeName_Leave(object sender, EventArgs e)
        {
            BindDatagridCases();
        }

        private void BindDatagridCases()
        {
            if (txtLookupPracticeNumber.Text != ""
                && txtLookupPracticeNumber.AutoCompleteCustomSource.Contains(txtLookupPracticeNumber.Text))
            {
                lblSelectedPracticeName.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeNumber.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();
                datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }
            else if (txtLookupPracticeName.Text != ""
                && txtLookupPracticeName.AutoCompleteCustomSource.Contains(txtLookupPracticeName.Text))
            {
                lblSelectedPracticeName.Text = "";
                DataTable oDt = oShared.usp_ServiceProvider_Select_AfterAutocomplete(txtLookupPracticeName.Text);
                lblSelectedPracticeName.Text = oDt.Rows[0][1].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0][0].ToString();
                datagridCases.DataSource = oFin.usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(Convert.ToInt32(lblCurrentPracticeID.Text), dateFinalInvoiceAmountStart.Value, dateFinalInvoiceAmountEnd.Value, Convert.ToInt32(comboCaseStatus.SelectedValue), Convert.ToInt32(comboBillingStatus.SelectedValue));
            }
            else
            {
                lblSelectedPracticeName.Text = "Please select a value from the list";
            }
        }

        private void datagridCases_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow item in datagridCases.Rows)
            {
                item.Cells["CaseID"].ReadOnly = true;
                item.Cells["BillingStatus"].ReadOnly = true;
                item.Cells["AuthNumber"].ReadOnly = true;
                item.Cells["AccountNr"].ReadOnly = true;
                item.Cells["FinalInvoiceAmount"].ReadOnly = true;
                item.Cells["CaseStatus"].ReadOnly = true;
                item.Cells["MemberNumber"].ReadOnly = true;
                item.Cells["Name"].ReadOnly = true;
                item.Cells["Surname"].ReadOnly = true;
                item.Cells["Initials"].ReadOnly = true;
                item.Cells["DateOfBirth"].ReadOnly = true;
                item.Cells["HasBooking"].ReadOnly = true;
                item.Cells["CurrentPractice"].ReadOnly = true;
                item.Cells["ServiceProviderName"].ReadOnly = true;
                item.Cells["ReferFrom"].ReadOnly = true;
                item.Cells["AdmissionDate"].ReadOnly = true;
                item.Cells["DischargeDate"].ReadOnly = true;
                item.Cells["MedicalAidName"].ReadOnly = true;
                item.Cells["MedAidProductName"].ReadOnly = true;
                item.Cells["Address1"].ReadOnly = true;
                item.Cells["Address2"].ReadOnly = true;
                item.Cells["Address3"].ReadOnly = true;
                item.Cells["Address4"].ReadOnly = true;
                item.Cells["AddressCode"].ReadOnly = true;
                item.Cells["ReferringPractice"].ReadOnly = true;
                item.Cells["Speciality"].ReadOnly = true;
                item.Cells["MBOD_RMA"].ReadOnly = true;
                item.Cells["ChangeToCaseDate"].ReadOnly = true;
                item.Cells["NumberOfAccounts"].ReadOnly = true;


                if (item.Cells["BillingStatus"].Value.ToString() == "Paid"
                    || item.Cells["BillingStatus"].Value.ToString() == "Superceded"
                    || item.Cells["Remittance"].Value.ToString() != "")
                {
                    item.Cells["Include"].ReadOnly = true;
                    item.Cells["Include"].Style.BackColor = Color.LightGray;
                }
            }
            if (comboBillingStatus.SelectedText == "Paid"
                    || comboBillingStatus.SelectedText == "Superceded")
            {
                btnCreateRemittance.Enabled = false;
            }
        }

        private void btnCreateRemittance_Click(object sender, EventArgs e)
        {
            string remittanceNumber = "";
            foreach (DataGridViewRow item in datagridCases.Rows)
            {
                if (item.Cells["Include"].Value.ToString() == "True")
                {
                    remittanceNumber = item.Cells["AccountNr"].Value.ToString().Substring(3);
                    break;
                }
            }

            SaveFileDialog oFile = new SaveFileDialog();
            oFile.AddExtension = true;
            oFile.DefaultExt = "xlsx";
            oFile.FileName = remittanceNumber + " - " + DateTime.Today.ToString("yyyy-MM-dd") + " - " + lblSelectedPracticeName.Text + ".xlsx";
            DialogResult saveFileDialogResult = oFile.ShowDialog();
            if (oFile.FileName != ""
                && saveFileDialogResult != DialogResult.Cancel)
            {
                if (File.Exists(oFile.FileName))
                {
                    File.Delete(oFile.FileName);
                }

                FileInfo newFile = new FileInfo(oFile.FileName);
                ExcelPackage oExcel = new ExcelPackage(newFile);
                ExcelWorksheet oSheet = oExcel.Workbook.Worksheets.Add("Remittance Detail");
                {
                    //oSheet.Row(1).Styl
                    oSheet.Cell(1, 1).Value = "UNIQUE NUMBER";
                    oSheet.Cell(1, 2).Value = "TYPE";
                    oSheet.Cell(1, 3).Value = "PRACTICE NUMBER";
                    oSheet.Cell(1, 4).Value = "PROVIDER";
                    oSheet.Cell(1, 5).Value = "INVOICE";
                    oSheet.Cell(1, 6).Value = "FIRST NAME";
                    oSheet.Cell(1, 7).Value = "SURNAME";
                    oSheet.Cell(1, 8).Value = "AMOUNT";
                    oSheet.Cell(1, 9).Value = "AUTHORIZATION NUMBER";
                    oSheet.Cell(1, 10).Value = "PAID";
                    oSheet.Cell(1, 11).Value = "DATE PAID";
                    oSheet.Cell(1, 12).Value = "SERVICE DATE";
                    oSheet.Cell(1, 13).Value = "COMMENTS";
                    oSheet.Cell(1, 14).Value = "SERVICE DATE FY";
                    oSheet.Cell(1, 15).Value = "COUNT INVOICE #";
                    //oSheet.Cell(1, 1).Value = "TYPE";
                    //oSheet.Cell(1, 2).Value = "YEAR";
                    //oSheet.Cell(1, 3).Value = "MONTH";
                    //oSheet.Cell(1, 4).Value = "DAY";
                    //oSheet.Cell(1, 17).Value = "DAYS TO SUBMIT";
                    //oSheet.Cell(1, 19).Value = "FORMATTED DATE";

                    int currentRow = 1;
                    foreach (DataGridViewRow item in datagridCases.Rows)
                    {
                        if (item.Cells["Include"].Value.ToString() == "True")
                        {
                            currentRow++;
                            oSheet.Cell(currentRow, 1).Value = item.Cells["Case_BillingID"].Value.ToString();
                            oSheet.Cell(currentRow, 2).Value = item.Cells["MedicalAidName"].Value.ToString();
                            oSheet.Cell(currentRow, 3).Value = item.Cells["CurrentPractice"].Value.ToString();
                            oSheet.Cell(currentRow, 4).Value = item.Cells["ServiceProviderName"].Value.ToString();
                            oSheet.Cell(currentRow, 5).Value = item.Cells["AccountNr"].Value.ToString();
                            oSheet.Cell(currentRow, 6).Value = item.Cells["Name"].Value.ToString();
                            oSheet.Cell(currentRow, 7).Value = item.Cells["Surname"].Value.ToString();
                            oSheet.Cell(currentRow, 8).Value = item.Cells["FinalInvoiceAmount"].Value.ToString();
                            oSheet.Cell(currentRow, 8).DataType = "CURRENCY";
                            oSheet.Cell(currentRow, 9).Value = item.Cells["AuthNumber"].Value.ToString();
                            oSheet.Cell(currentRow, 10).Value = "NO";// "PAID";
                            oSheet.Cell(currentRow, 11).Value = "";//.Value.ToString();"DATE PAID"
                            oSheet.Cell(currentRow, 12).Value = item.Cells["AdmissionDate"].Value.ToString().Substring(0, 10);
                            oSheet.Cell(currentRow, 13).Value = "";//.Value.ToString();"COMMENTS"
                            oSheet.Cell(currentRow, 14).Value = item.Cells["AdmissionDateFY"].Value.ToString();//.Value.ToString();SERVICE DATE FY 
                            oSheet.Cell(currentRow, 15).Value = item.Cells["NumberOfAccounts"].Value.ToString();//.Value.ToString();COUNT INVOICE #
                            oFin.usp_Case_Billing_UpdateRemittanceNumber(Convert.ToInt32(item.Cells["Case_BillingID"].Value)
                                , remittanceNumber
                                , Program.Username);
                        }
                    }
                    oExcel.Save();
                }
                oExcel.Dispose();
            }
            BindDatagridCases();

            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = oFile.FileName;
            //psi.Arguments = oDt.Rows[0]["UpdatePath"].ToString();
            //if you don't want a console window popping up then set this property
            //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //Create new process and set the starting information
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;

            //Set this so that you can tell when the process has completed
            //p.EnableRaisingEvents = true;
            p.Start();
        }

        private void comboCaseStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatagridCases();

        }

        private void comboBillingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatagridCases();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindDatagridCases();
        }

        private void btnImportRemittanceStatus_Click(object sender, EventArgs e)
        {
            try
            {

            
            OpenFileDialog oFile = new OpenFileDialog();
            DialogResult openFileDialogResult = oFile.ShowDialog();
            if (oFile.FileName != ""
                && openFileDialogResult != DialogResult.Cancel
                && File.Exists(oFile.FileName))
            {
                bool readNext = true;
                FileInfo newFile = new FileInfo(oFile.FileName);
                ExcelPackage oExcel = new ExcelPackage(newFile);
                ExcelWorksheet oSheet = oExcel.Workbook.Worksheets["Remittance Detail"];
                {
                    int cPaid = 0;
                    int cAuthNumber = 0;
                    int cDatePaid = 0;
                    int cComments = 0;
                    int cUnique = 0;
                    int cAmount = 0;

                    for (int i = 1; i < 20; i++)
                    {
                        if (oSheet.Cell(1, i).Value.ToUpper() == "PAID")
                        {
                            cPaid = i;
                        }
                        if (oSheet.Cell(1, i).Value.ToUpper() == "AUTHORIZATION NUMBER")
                        {
                            cAuthNumber = i;
                        }
                        if (oSheet.Cell(1, i).Value.ToUpper() == "DATE PAID")
                        {
                            cDatePaid = i;
                        }
                        if (oSheet.Cell(1, i).Value.ToUpper() == "COMMENTS")
                        {
                            cComments = i;
                        }
                        if (oSheet.Cell(1, i).Value.ToUpper() == "UNIQUE NUMBER")
                        {
                            cUnique = i;
                        }
                        if (oSheet.Cell(1, i).Value.ToUpper() == "AMOUNT")
                        {
                            cAmount = i;
                        }
                    }

                    int currentRow = 1;
                    if (cPaid != 0
                        && cAuthNumber != 0
                        && cDatePaid != 0
                        && cComments != 0
                        && cUnique != 0)
                    {
                        while(readNext)
                        {
                            currentRow++;
                            try
                            {
                                string oPaid = oSheet.Cell(currentRow, cPaid).Value;
                                string oAuthNumber = oSheet.Cell(currentRow, cAuthNumber).Value;
                                string oComments = oSheet.Cell(currentRow, cComments).Value;
                                int oUnique = Convert.ToInt32(oSheet.Cell(currentRow, cUnique).Value);
                                decimal oPaidAmount = Convert.ToDecimal(oSheet.Cell(currentRow, cAmount).Value);
                                DateTime oDatePaid = Convert.ToDateTime(oSheet.Cell(currentRow, cDatePaid).Value);//(oSheet.Cell(currentRow, cDatePaid).Value);

                                if (oPaid.ToUpper() != "NO")
                                {
                                    oFin.usp_Case_Billing_UpdateToPaid(oUnique
                                        , oPaidAmount
                                        , oDatePaid
                                        , oComments
                                        , Program.Username);
                                }
                                else
                                {
                                    MessageBox.Show(String.Format("Row {0} is not marked as paid", currentRow));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(String.Format("Please make sure the values in row {0} are correctly formatted\n\rThis row was skipped", currentRow));
                            }

                            if (oSheet.Cell(currentRow + 1,cUnique).Value == "")
                            {
                                readNext = false;
                            }
                        }
                        datagridRemittanceDetail.DataSource = oFin.usp_Cases_Select_ByRemittance(txtRemittanceNumber.Text);
                    }
                    else
                    {
                        MessageBox.Show("The excel file you are trying to import is not formatted correctly.");
                    }
                    oExcel.Save();
                }
                oExcel.Dispose();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
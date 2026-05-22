using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;
using Icondev.MedManage.MedManageLib;
using System.Diagnostics;

namespace Icondev.MedManage
{
    public partial class Case : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        CaseManagement oCM = new CaseManagement(Program.oDb);
        Finance oFin = new Finance(Program.oDb);
        //TariffCodeLookup oCodeTariff;
        CodeLookup oCodeICD;
        CodeLookup oCodeCPT;
        int CaseID;
        bool InitialLoad = true;
        bool formClosing = false;
        DataTable oDtCurrentCase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CaseID">When -1 then new else open old</param>
        public Case(int caseID)
        {


            InitializeComponent();
            //MessageBox.Show(this.Size.Width.ToString());
            this.Width = 1300;

            try
            {
                CaseID = caseID;

                //oCodeTariff = new TariffCodeLookup(CaseID);
                oCodeCPT = new CodeLookup("CPT", CaseID);
                oCodeICD = new CodeLookup("ICD", CaseID);
                //oCodeTariff.MdiParent = this.MdiParent;
                oCodeCPT.MdiParent = this.MdiParent;
                oCodeICD.MdiParent = this.MdiParent;

                #region data bindings
                comboTitle.DataSource = oShared.usp_Title_Select();
                comboTitle.DisplayMember = "Title";
                comboTitle.ValueMember = "TitleID";

                comboMemberLanguage.DataSource = oShared.usp_Language_Select();
                comboMemberLanguage.DisplayMember = "Language";
                comboMemberLanguage.ValueMember = "LanguageID";

                comboMemberStatus.DataSource = oShared.usp_MemberStatus_Select();
                comboMemberStatus.DisplayMember = "MemberStatus";
                comboMemberStatus.ValueMember = "MemberStatusID";

                comboMemberMedicalAid.DisplayMember = "MedicalAidName";
                comboMemberMedicalAid.ValueMember = "MedicalAidID";
                comboMemberMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);

                comboReferPracticeSpeciality.DataSource = oShared.usp_Speciality_Select();
                comboReferPracticeSpeciality.DisplayMember = "Speciality";
                comboReferPracticeSpeciality.ValueMember = "SpecialityID";

                comboCurrentPracticeSpeciality.DataSource = oShared.usp_Speciality_Select();
                comboCurrentPracticeSpeciality.DisplayMember = "Speciality";
                comboCurrentPracticeSpeciality.ValueMember = "SpecialityID";

                comboDaysInCareFacilityType.DataSource = oShared.usp_FacilityType_Select();
                comboDaysInCareFacilityType.DisplayMember = "FacilityType";
                comboDaysInCareFacilityType.ValueMember = "FacilityTypeID";
                //Default
                comboDaysInCareFacilityType.SelectedValue = 6;

                comboCaseStatus.DataSource = oCM.usp_CaseStatus_Select();
                comboCaseStatus.DisplayMember = "CaseStatus";
                comboCaseStatus.ValueMember = "CaseStatusID";

                comboExclusions.DataSource = oShared.usp_Exclusion_Select();
                comboExclusions.DisplayMember = "Exclusion";
                comboExclusions.ValueMember = "ExclusionID";

                comboCaseType.DataSource = oCM.usp_CaseType_Select();
                comboCaseType.DisplayMember = "CaseType";
                comboCaseType.ValueMember = "CaseTypeID";

                comboMedicalAidProduct.DisplayMember = "MedAidProductName";
                comboMedicalAidProduct.ValueMember = "MedAidProductID";
                comboMedicalAidProduct.DataSource = oShared.usp_MedicalAidProduct_Select(Program.MainClientID);

                comboCaseCategory.DisplayMember = "CaseCategory";
                comboCaseCategory.ValueMember = "CaseCategoryID";
                comboCaseCategory.DataSource = oCM.usp_CaseCategory_Select();

                lblMemberID.Text = "";

                if (CaseID == -1)
                {
                    //grpDaysInCare.Visible = false;

                    picCreateBooking.Enabled = false;

                    picLinkToCase.Visible = false;
                    picLinkToCase.Enabled = false;
                    lblLinkToCase.Visible = false;
                    picPrintLinkedCases.Visible = false;
                    picPrintLinkedCases.Enabled = false;

                    picUnlinkCase.Visible = false;
                    lblUnlinkCase.Visible = false;
                    picUnlinkCase.Enabled = false;

                    picLinkedDocumentsAdd.Visible = false;
                    picOpenFile.Visible = false;
                    picLinkedDocumentsDelete.Visible = false;

                }
                else
                {
                    //grpDaysInCare.Visible = true;
                    picLinkToCase.Visible = true;
                    picLinkToCase.Enabled = true;
                    lblLinkToCase.Visible = true;

                    picLinkedDocumentsAdd.Visible = true;
                    picOpenFile.Visible = true;
                    picLinkedDocumentsDelete.Visible = true;

                    picCreateBooking.Enabled = true;
                    tabControlMain.SelectedTab = tabNotes;

                    oDtCurrentCase = oCM.usp_Cases_Select(CaseID);
                    txtCaseID.Text = oDtCurrentCase.Rows[0]["CaseID"].ToString();
                    txtAuthNumber.Text = oDtCurrentCase.Rows[0]["AuthNumber"].ToString();
                    txtAccountNumber.Text = oDtCurrentCase.Rows[0]["AccountNr"].ToString();

                    lblCurrentPracticeID.Text = oDtCurrentCase.Rows[0]["ReferToID"].ToString();
                    BindCurrentPractice(oShared.usp_ServiceProvider_Select(Convert.ToInt32(oDtCurrentCase.Rows[0]["ReferToID"])));

                    lblReferringPracticeID.Text = oDtCurrentCase.Rows[0]["ReferFromID"].ToString();
                    BindReferingPractice(oShared.usp_ServiceProvider_Select(Convert.ToInt32(oDtCurrentCase.Rows[0]["ReferFromID"])));

                    dateAdmissionDate.Value = Convert.ToDateTime(oDtCurrentCase.Rows[0]["AdmissionDate"]);
                    timeAdmissionTime.Value = Convert.ToDateTime(oDtCurrentCase.Rows[0]["AdmissionTime"]);
                    dateDischargeDate.Value = Convert.ToDateTime(oDtCurrentCase.Rows[0]["DischargeDate"]);
                    timeDischargeTime.Value = Convert.ToDateTime(oDtCurrentCase.Rows[0]["DischargeTime"]);
                    comboCaseType.SelectedValue = oDtCurrentCase.Rows[0]["AuthtypeID"];
                    chkWCA_IOD.Checked = Convert.ToBoolean(oDtCurrentCase.Rows[0]["WCA_IOD"]);
                    txtCaseDescription.Text = oDtCurrentCase.Rows[0]["CaseDescription"].ToString();
                    txtInterimAmount.Text = oDtCurrentCase.Rows[0]["TotalAmount"].ToString();

                    if (oDtCurrentCase.Rows[0]["CaseCategoryID"].ToString() != "")
                    {
                        comboCaseCategory.SelectedValue = oDtCurrentCase.Rows[0]["CaseCategoryID"];
                    }
                    
                    lblMemberID.Text = oDtCurrentCase.Rows[0]["MemberID"].ToString();
                    BindMember(oShared.usp_Member_Select(Convert.ToInt32(oDtCurrentCase.Rows[0]["MemberID"]), dateAdmissionDate.Value));

                    if (oDtCurrentCase.Rows[0]["PenaltyPercentage"].ToString() != "")
                    {
                        numPenaltyPercentage.Value = Convert.ToDecimal(oDtCurrentCase.Rows[0]["PenaltyPercentage"]);
                    }
                    else
                    {
                        numPenaltyPercentage.Value = 0;
                    }
                    
                    txtFinalInvoiceAmount.Text = oDtCurrentCase.Rows[0]["FinalInvoiceAmount"].ToString();
                    
                    datagridCaseNotes.DataSource = oCM.usp_CaseNote_Select(CaseID);
                    datagridChecklist.DataSource = oCM.usp_Case_Checklist_Select(CaseID);
                    datagridCptCodes.DataSource = oCM.usp_Case_CPT_Select(CaseID);
                    datagridDaysInCare.DataSource = oCM.usp_Case_FacilityType_Select(CaseID);
                    datagridExclusionCodes.DataSource = oCM.usp_Case_Exclusion_Select(CaseID);
                    datagridIcd.DataSource = oCM.usp_Case_ICD_Select(CaseID);
                    datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                    datagridComments.DataSource = oCM.usp_CaseComment_Select(CaseID);
                    grdLinkedDocuments.DataSource = oCM.usp_Case_LinkedFile_SelectByCaseID(CaseID);
                    datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
                    //datagridBilling.DataSource = oCM.usp_Case_Billing_Select(CaseID);
                    //datagridBillingSmall.DataSource = oCM.usp_Case_Billing_Select_Summary(CaseID);

                    // Needs to happen after datagridChecklist id bound
                    comboCaseStatus.SelectedValue = oDtCurrentCase.Rows[0]["StatusID"];

                    chkWasBooking.Checked = Convert.ToBoolean(oDtCurrentCase.Rows[0]["HasBooking"]);
                    if (chkWasBooking.Checked
                        && comboCaseStatus.Text != "Booking")
                    {
                        lblChangeToCaseDate.Text = oDtCurrentCase.Rows[0]["ChangeToCaseDate"].ToString();
                    }

                    lblFinalInvoiceAmountUpdated.Text = oDtCurrentCase.Rows[0]["FinalInvoiceAmountUpdated"].ToString();

                    
                    //AutoComplete for account numbers
                    if (txtAccountNumber.Text == "" 
                        && Program.EnableBilling)
                    // ToDo: (Billing)taken out for Tariff change, need to put back
                    {
                        txtAccountNumber.Enabled = true;
                        AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
                        DataTable oDtNamesCollection = oFin.usp_Case_Billing_SelectAccountNumbers_ByServiceProviderID(Convert.ToInt32(lblCurrentPracticeID.Text));
                        string[] oResultName = new string[oDtNamesCollection.Rows.Count];
                        for (int i = 0; i < oDtNamesCollection.Rows.Count; i++)
                        {
                        oResultName[i] = oDtNamesCollection.Rows[i][0].ToString();
                        }
                        namesCollection.AddRange(oResultName);
                        txtAccountNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
                        //comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtAccountNumber.AutoCompleteCustomSource = namesCollection;
                    }
                    else if(Program._GenericPrincipal.IsInRole("Metadata Administrator")
                        || Program._GenericPrincipal.IsInRole("System Administrator") 
                        && Program.EnableBilling)
                    {
                        txtAccountNumber.Enabled = true;
                    }
                    else if (Program.EnableBilling)
                    {
                        txtAccountNumber.Enabled = false;
                    }

                    DataTable oDtLinks = oCM.usp_Case_Link_Select(CaseID);
                    if (oDtLinks.Rows.Count > 0)
                    {
                        picLinkToCase.Visible = false;
                        picLinkToCase.Enabled = false;
                        lblLinkToCase.Visible = false;
                        picPrintLinkedCases.Visible = true;
                        picPrintLinkedCases.Enabled = true;

                        picUnlinkCase.Visible = true;
                        lblUnlinkCase.Visible = true;
                        picUnlinkCase.Enabled = true;

                        //If case is copied, then should not be able to change Case Category
                        //TODO: find a better way to force this i the database
                        //if (Convert.ToInt32(oDtLinks.Rows[0]["CaseID"].ToString()) < CaseID)
                        //{
                        //    comboCaseCategory.Enabled = false;
                        //}
                        datagridCases.DataSource = oDtLinks;
                    }
                    else
                    {
                        picLinkToCase.Visible = true;
                        picLinkToCase.Enabled = true;
                        lblLinkToCase.Visible = true;
                        picPrintLinkedCases.Visible = false;
                        picPrintLinkedCases.Enabled = false;

                        picUnlinkCase.Visible = false;
                        lblUnlinkCase.Visible = false;
                        picUnlinkCase.Enabled = false;
                    }

                    //MainClient specific Rules
                    if (Program.MainClientName == "Botswana Local")
                    {
                        chkTariffPerUnit.Checked = false;
                    }
                    else
                    {
                        chkTariffPerUnit.Checked = true;
                    }
                }
                #endregion
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }

            InitialLoad = false;

            //access
            //"Billing Auditing"
            //"System Administrator"
            //"Metadata Administrator"
            //"Imports"
            //"Case Manager"
            if (Program._GenericPrincipal.IsInRole("Case Manager"))
            {
                picCopy.Visible = true;
                picIcdAdd.Visible = true;
                picIcdSearch.Visible = true;
                picICDDelete.Visible = true;
            }
            else
            {
                picCopy.Visible = false;
                picIcdAdd.Visible = false;
                picIcdSearch.Visible = false;
                picICDDelete.Visible = false;
            }
            if (Program._GenericPrincipal.IsInRole("Billing Auditing"))
            {
                picTarrifAdd.Visible = true;
                picTarrifDelete.Visible = true;
                //picBillingSave.Visible = true;
                //picBillingDelete.Visible = true;
            }
            else
            {
                picTarrifAdd.Visible = false;
                picTarrifDelete.Visible = false;
                //picBillingSave.Visible = false;
                //picBillingDelete.Visible = false;
            }

        }

        private void picMemberLookup_Click(object sender, EventArgs e)
        {
            MemberLookup oMemberLookup = new MemberLookup(true, txtMemberSurname.Text, txtMemberName.Text, txtPassportNumber.Text, txtIdNumber.Text);
            oMemberLookup.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //oMemberLookup.MdiParent = this.MdiParent;

            oMemberLookup.ShowDialog();
            switch (oMemberLookup.oReturn)
            {
                case "None":
                    break;
                case "Member":
                    {
                        DataTable oDtMember = oShared.usp_Member_Select(oMemberLookup.MemberID, dateAdmissionDate.Value);
                        BindMember(oDtMember);
                    }
                    break;
                case "New":
                    {
                        MemberAdd oMemberAdd = new MemberAdd(
                            oMemberLookup.Surname
                            , oMemberLookup.MemberName
                            , oMemberLookup.MemberNumber
                            , oMemberLookup.PassportNumber
                            , oMemberLookup.IDNumber
                            , oMemberLookup.DateOfBirth);

                        oMemberAdd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                        //oMemberAdd.MdiParent = this.MdiParent;
                        oMemberAdd.ShowDialog();

                        //Get member info from add
                        DataTable oDtMember = oShared.usp_Member_Select(oMemberAdd.MemberID, dateAdmissionDate.Value);
                        BindMember(oDtMember);
                    }
                    break;
                default:
                    break;
            }



        }

        private void BindMember(DataTable oDtMember)
        {
            if (oDtMember.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(oDtMember.Rows[0]["AllowServices"].ToString()))
                {
                    MessageBox.Show("Healthshare is not allowed to render any services for this patient.\n\rPlease check the patient's medical aid information");
                    picSave.Enabled = false;
                }
               
                gbMember.Text += " - " + oDtMember.Rows[0]["MemberNumber"].ToString();
                comboTitle.SelectedValue = oDtMember.Rows[0]["TitleID"].ToString();
                txtMemberSurname.Text = oDtMember.Rows[0]["Surname"].ToString();
                txtMemberInitials.Text = oDtMember.Rows[0]["Initials"].ToString();
                txtMemberName.Text = oDtMember.Rows[0]["Name"].ToString();
                txtIdNumber.Text = oDtMember.Rows[0]["IDNumber"].ToString();
                txtPassportNumber.Text = oDtMember.Rows[0]["PassportNumber"].ToString();
                dateMemberDateOfBirth.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateOfBirth"].ToString());
                comboMemberMedicalAid.SelectedValue = oDtMember.Rows[0]["MedicalAidID"].ToString();
                dateMemberDateJoined.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateJoined"].ToString());
                chkMemberIsPensioner.Checked = Convert.ToBoolean(oDtMember.Rows[0]["Pensioner"].ToString());
                comboMemberStatus.SelectedValue = oDtMember.Rows[0]["MemberStatusID"].ToString();
                lblMemberID.Text = oDtMember.Rows[0]["MemberID"].ToString();
                comboMedicalAidProduct.SelectedValue = oDtMember.Rows[0]["MedAidProductID"].ToString();

                chkMBOD_RMA1.Checked = Convert.ToBoolean(oDtMember.Rows[0]["MBOD_RMA"].ToString());
                chkMBOD_RMA2.Checked = Convert.ToBoolean(oDtMember.Rows[0]["MBOD_RMA"].ToString());

                if (CaseID != -1)
                {
                    if (oDtMember.Rows[0]["CasePrefix"].ToString() != txtAuthNumber.Text.Substring(0, 3))
                    {
                        MessageBox.Show("This member belongs to a different medical aid. The system will now update the Auth Number.");

                        txtAuthNumber.Text = txtAuthNumber.Text.Replace(txtAuthNumber.Text.Substring(0, 3), (oDtMember.Rows[0]["CasePrefix"].ToString()));

                        DataTable oDt = oCM.usp_Case_UpdateMemberMedicalAid(CaseID, txtAuthNumber.Text);
                        //if (oDt.Rows.Count > 0)
                        //{
                        //    txtCaseNumber.Text = oDt.Rows[0][0].ToString();
                        //}
                    }
                }
                if (Convert.ToBoolean(oDtMember.Rows[0]["Suspended"].ToString()))
                {
                    MessageBox.Show("This member was suspended on " + Convert.ToDateTime(oDtMember.Rows[0]["DateSuspended"].ToString()).ToShortDateString());
                }
                if (Convert.ToBoolean(oDtMember.Rows[0]["MedicalAidExhausted"].ToString()))
                {
                    MessageBox.Show("This member's medical aid is exhausted as of " + Convert.ToDateTime(oDtMember.Rows[0]["DateMedicalAidExhausted"].ToString()).ToShortDateString());
                }
                if (Convert.ToBoolean(oDtMember.Rows[0]["Deceased"].ToString()))
                {
                    MessageBox.Show("This is marked as deceased as of " + Convert.ToDateTime(oDtMember.Rows[0]["DeceasedDate"].ToString()).ToShortDateString());
                }

            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            DialogResult oResult = MessageBox.Show("Do you want to save before closing?", "Close", MessageBoxButtons.YesNo);
            if (oResult == System.Windows.Forms.DialogResult.Yes)
            {
                SaveCase();
            }

            GC.WaitForPendingFinalizers();

        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            if (txtAccountNumber.Focused)
                txtAccountNumber_Leave(sender, e);
            //to make sure a critical control does not have focus and all validations are applied

            formClosing = true;
            this.Close();
        }

        private void Case_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        private void picDaysInCareAdd_Click(object sender, EventArgs e)
        {
            DateTime admitted = Convert.ToDateTime(dateDaysInCareAdmitted.Value.ToString("yyyy/MM/dd ") + timeDaysInCareAdmitted.Value.ToString("hh:mm t\\M"));
            DateTime discharged = Convert.ToDateTime(dateDaysInCareDischarged.Value.ToString("yyyy/MM/dd ") + timeDaysInCareDischarged.Value.ToString("hh:mm t\\M"));

            try
            {
                oCM.usp_Case_FacilityType_Insert(
                Convert.ToInt32(comboDaysInCareFacilityType.SelectedValue)
                , CaseID
                , Convert.ToDateTime(admitted)
                , Convert.ToDateTime(discharged)
                , Convert.ToDecimal(numDaysInCareLOS.Value)
                , txtFacilityTypeCode.Text
                , Convert.ToInt32((txtMinutesOnVentilator.Text == "" ? "0" : txtMinutesOnVentilator.Text))
                , txtComments.Text
                , DateTime.Today
                , Program.Username
                );
                datagridDaysInCare.DataSource = oCM.usp_Case_FacilityType_Select(CaseID);
            }
            catch (Exception err)
            {
                if (err.Message.Contains("Violation of PRIMARY KEY constraint"))
                    MessageBox.Show("This record already exists");
                else throw;
            }

            //Total discharged rule - must not be earlier than the latest discharge date
            //Total admitted rule - Must not be later than the earliest admission date
            DateTime TotalDischarged = Convert.ToDateTime(dateDischargeDate.Value.ToString("yyyy/MM/dd ") + timeDischargeTime.Value.ToString("hh:mm t\\M"));
            DateTime TotalDischargedNew = TotalDischarged;

            DateTime TotalAdmitted = Convert.ToDateTime(dateAdmissionDate.Value.ToString("yyyy/MM/dd ") + timeAdmissionTime.Value.ToString("hh:mm t\\M"));
            DateTime TotalAdmittedNew = TotalAdmitted;
            decimal TotalLOS = 0;

            for (int i = 0; i < datagridDaysInCare.Rows.Count; i++)
            {
                if (TotalDischargedNew == TotalDischarged)
                {//GEt date from daagrid
                    TotalDischargedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value);
                }
                if (Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value) > TotalDischargedNew)
                {//Discharge
                    TotalDischargedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value);
                }
                if (Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateAdmitted"].Value) < TotalAdmittedNew)
                {//Admission
                    TotalAdmittedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateAdmitted"].Value);
                }
                TotalLOS += Convert.ToDecimal(datagridDaysInCare.Rows[i].Cells["LOS"].Value);
            }

            //Where the latest discharge is greater than what is currently selected
            if (TotalDischargedNew > TotalDischarged)
            {
                DialogResult oResult = MessageBox.Show("Warning: One of the discharge dates is later than the overall discharge date. "
                     + "\n\rDo you want to update the discharge date?"
                     , "Overall Discharge Date"
                     , MessageBoxButtons.YesNo);
                if (oResult == System.Windows.Forms.DialogResult.Yes)
                {
                    dateDischargeDate.Value = TotalDischargedNew;
                    timeDischargeTime.Value = TotalDischargedNew;
                }
            }
            //Where the latest discharge is SMALLER than what is currently selected
            if (TotalDischargedNew < TotalDischarged)
            {
                DialogResult oResult = MessageBox.Show("Warning: The latest discharge date in the admission is earlier than the current overall discharge date "
                     + "\n\rDo you want to update the discharge date?"
                     , "Overall Discharge Date"
                     , MessageBoxButtons.YesNo);
                if (oResult == System.Windows.Forms.DialogResult.Yes)
                {
                    dateDischargeDate.Value = TotalDischargedNew;
                    timeDischargeTime.Value = TotalDischargedNew;
                }
            }

            
            if (TotalAdmittedNew < TotalAdmitted)
            {
                DialogResult oResult = MessageBox.Show("Warning: One of the admission dates is earlier than the overall admission date. "
                     + "\n\rDo you want to update the admission date?"
                     , "Overall Admission Date"
                     , MessageBoxButtons.YesNo);
                if (oResult == System.Windows.Forms.DialogResult.Yes)
                {
                    dateAdmissionDate.Value = TotalAdmittedNew;
                    timeAdmissionTime.Value = TotalAdmittedNew;
                }
            }
        }

        private void picDaysInCareRemove_Click(object sender, EventArgs e)
        {
            if (datagridDaysInCare.CurrentRow.Index > -1)
                oCM.usp_Case_FacilityType_Delete(
                Convert.ToInt32(datagridDaysInCare["ID", datagridDaysInCare.CurrentRow.Index].Value)
                    );
            datagridDaysInCare.DataSource = oCM.usp_Case_FacilityType_Select(CaseID);
        }

        private void picExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (datagridIcd.Rows.Count > 0)
                {
                    rpt_CaseLetter oCaseLetter = new rpt_CaseLetter(CaseID, Convert.ToInt32(comboMemberMedicalAid.SelectedValue));
                    oCaseLetter.ShowDialog();
                }
                else
                {
                    MessageBox.Show("You must add the ICD codes first");
                }
            }
            catch
            {
                MessageBox.Show("You must add the ICD codes first");
                //Expected if there is no databinding
            }

        }

        private void picSave_Click(object sender, EventArgs e)
        {
            SaveCase();
        }

        private void SaveCase()
        {
            string oValidatorMessage = "";
            try
            {
                //Validations
                decimal oTestDecimal = 0;
                int oTestInt = 0;
                oValidatorMessage = "The Interim Amount field can only contain decimal values";
                oTestDecimal = (txtInterimAmount.Text == "" ? 0 : Convert.ToDecimal(txtInterimAmount.Text));
                oValidatorMessage = "The Final Amount field can only contain decimal values";
                oTestDecimal = (txtFinalInvoiceAmount.Text == "" ? 0 : Convert.ToDecimal(txtFinalInvoiceAmount.Text));
                oValidatorMessage = "You need to select a member before you can save";
                oTestInt = (lblMemberID.Text == "" ? 0 : Convert.ToInt32(lblMemberID.Text));
                oValidatorMessage = "You need to select a referring practice before you can save";
                oTestInt = (lblReferringPracticeID.Text == "" ? 0 : Convert.ToInt32(lblReferringPracticeID.Text));
                oValidatorMessage = "You need to select a current practice before you can save";
                oTestInt = (lblCurrentPracticeID.Text == "" ? 0 : Convert.ToInt32(lblCurrentPracticeID.Text));

                if (comboCaseType.SelectedValue.ToString() == "0")
                {
                    oValidatorMessage = "You have to select a case type before the case can be saved";
                    throw new Exception("Form validation error");
                }

                if (comboCaseCategory.Text == " Please Select")
                {
                    oValidatorMessage = "You have to select a category before the case can be saved";
                    throw new Exception("Form validation error");
                }

                oValidatorMessage = "";
                //
                Cursor.Current = Cursors.WaitCursor;
                ActionStatus oStatus = new ActionStatus("Saving");
                if (CaseID == -1)
                {
                    DataTable oDtDup = oCM.usp_Cases_Select_PossibleDuplicate(
                        Convert.ToInt32(lblMemberID.Text)
                        , Convert.ToInt32(lblCurrentPracticeID.Text)
                        , dateAdmissionDate.Value);

                    DialogResult oResult = System.Windows.Forms.DialogResult.Yes;

                    if (oDtDup.Rows.Count > 0)
                    {
                        CustomMessageBoxWithGrid oMessage = new CustomMessageBoxWithGrid(oDtDup, "Possible duplicate claim", "Do you want to create this new claim?");
                        oResult = oMessage.ShowDialog();
                    }

                    if (oResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        DataTable oDt =
                            oCM.usp_Cases_Insert(
                                txtAuthNumber.Text
                                , txtAccountNumber.Text
                                , Convert.ToInt32(lblMemberID.Text)
                                , Convert.ToInt32(lblCurrentPracticeID.Text)
                                , Convert.ToInt32(lblReferringPracticeID.Text == "" ? "0" : lblReferringPracticeID.Text)
                                , dateAdmissionDate.Value
                                , timeAdmissionTime.Value
                                , dateDischargeDate.Value
                                , timeDischargeTime.Value
                                , Convert.ToInt32(comboCaseType.SelectedValue)
                                , chkWCA_IOD.Checked
                                , 0
                                , (txtInterimAmount.Text == "" ? 0 : Convert.ToDecimal(txtInterimAmount.Text))
                                , (txtFinalInvoiceAmount.Text == "" ? 0 : Convert.ToDecimal(txtFinalInvoiceAmount.Text))
                                , lblFinalInvoiceAmountUpdated.Text
                                , Convert.ToInt32(comboCaseStatus.SelectedValue)
                                , txtCaseDescription.Text
                                , Program.Username
                                , ""
                                , ""
                                , ""
                                , chkWasBooking.Checked
                                , Convert.ToDateTime((lblChangeToCaseDate.Text == "N/A" ? null : lblChangeToCaseDate.Text))
                                , numPenaltyPercentage.Value
                                , Convert.ToInt32(comboCaseCategory.SelectedValue)
                                );
                        CaseID = Convert.ToInt32(oDt.Rows[0][0].ToString());
                        txtCaseID.Text = oDt.Rows[0][0].ToString();
                        txtAuthNumber.Text = oDt.Rows[0][1].ToString();
                        //grpDaysInCare.Visible = true;
                        grpEpisode.Visible = true;
                        picLinkToCase.Visible = true;
                        picLinkToCase.Enabled = true;
                        lblLinkToCase.Visible = true;


                        picLinkedDocumentsAdd.Visible = true;
                        picOpenFile.Visible = true;
                        picLinkedDocumentsDelete.Visible = true;

                        //ToDo: (Billing) Taken out for Tariff Change. Needs to be put back
                        if (Program.EnableBilling)
                        {
                            oFin.usp_Case_Billing_LinkToCase(CaseID);
                        }

                        //oCodeTariff = new TariffCodeLookup(CaseID);
                        oCodeCPT = new CodeLookup("CPT", CaseID);
                        oCodeICD = new CodeLookup("ICD", CaseID);

                        datagridCaseNotes.DataSource = oCM.usp_CaseNote_Select(CaseID);
                        datagridChecklist.DataSource = oCM.usp_Case_Checklist_Select(CaseID);
                        datagridCptCodes.DataSource = oCM.usp_Case_CPT_Select(CaseID);
                        datagridDaysInCare.DataSource = oCM.usp_Case_FacilityType_Select(CaseID);
                        datagridExclusionCodes.DataSource = oCM.usp_Case_Exclusion_Select(CaseID);
                        datagridIcd.DataSource = oCM.usp_Case_ICD_Select(CaseID);
                        datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                        grdLinkedDocuments.DataSource = oCM.usp_Case_LinkedFile_SelectByCaseID(CaseID);

                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Saved successfully");
                    }
                    else
                    {
                        MessageBox.Show("Case was not saved");
                    }

                }
                else
                {
                    oCM.usp_Cases_Update(
                        CaseID
                        , txtAuthNumber.Text
                        , txtAccountNumber.Text
                        , Convert.ToInt32(lblMemberID.Text)
                        , Convert.ToInt32(lblCurrentPracticeID.Text)
                        , Convert.ToInt32(lblReferringPracticeID.Text)
                        , dateAdmissionDate.Value
                        , timeAdmissionTime.Value
                        , dateDischargeDate.Value
                        , timeDischargeTime.Value
                        , Convert.ToInt32(comboCaseType.SelectedValue)
                        , chkWCA_IOD.Checked
                        , Convert.ToDecimal(txtTotalLOS.Text)
                        , (txtInterimAmount.Text == "" ? 0 : Convert.ToDecimal(txtInterimAmount.Text))
                        , (txtFinalInvoiceAmount.Text == "" ? 0 : Convert.ToDecimal(txtFinalInvoiceAmount.Text))
                        , lblFinalInvoiceAmountUpdated.Text
                        , Convert.ToInt32(comboCaseStatus.SelectedValue)
                        , txtCaseDescription.Text
                        , Program.Username
                        , ""
                        , ""
                        , ""
                        , chkWasBooking.Checked
                        , Convert.ToDateTime((lblChangeToCaseDate.Text == "N/A" ? null : lblChangeToCaseDate.Text))
                        , numPenaltyPercentage.Value
                        , Convert.ToInt32(comboCaseCategory.SelectedValue)
                        );

                    //ToDo: (Billing) Taken out for Tariff Change. Needs to be put back
                    if (Program.EnableBilling)
                    {
                        oFin.usp_Case_Billing_LinkToCase(CaseID);
                    }
                    

                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Saved successfully");
                    oDtCurrentCase = oCM.usp_Cases_Select(CaseID);
                }
                //Cursor.Current = Cursors.Default;
                //MessageBox.Show("Saved successfully");
            }
            catch(Exception err)
            {
                if (oValidatorMessage != "")
                    MessageBox.Show(oValidatorMessage);
                else MessageBox.Show("Please make sure that all fields are filled in");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void picReferPracticeLookup_Click(object sender, EventArgs e)
        {
            ServiceProviderLookup oSPLookup = new ServiceProviderLookup(true
                , (txtReferPracticeName.Text.Contains(" - ")?txtReferPracticeName.Text.Substring(txtReferPracticeName.Text.IndexOf(" - ") + 3):"")
                , txtReferPracticePersonSurname.Text
                , txtReferPracticePersonName.Text);
            oSPLookup.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //oSPLookup.MdiParent = this.MdiParent;

            oSPLookup.ShowDialog();
            switch (oSPLookup.oReturn)
            {
                case "None":
                    break;
                case "New":
                    {
                        ServiceProvider oServiceProvider = new ServiceProvider(-1);
                        oServiceProvider.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                        // oServiceProvider.MdiParent = this.MdiParent;
                        oServiceProvider.ShowDialog();

                        DataTable oDt = oShared.usp_ServiceProvider_Select(oServiceProvider.ServiceProviderID);
                        if (oDt.Rows.Count > 0)
                        {
                            BindReferingPractice(oDt);
                        }

                        break;
                    }
                case "ServiceProvider":
                    {
                        DataTable oDt = oShared.usp_ServiceProvider_Select(oSPLookup.ServiceProviderID);
                        BindReferingPractice(oDt);

                        break;
                    }
                default: break;
            }
        }

        private void BindReferingPractice(DataTable oDt)
        {
            if (oDt.Rows.Count > 0)
            {
                txtReferPracticeName.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                txtReferPracticePersonName.Text = oDt.Rows[0]["ServiceProviderName"].ToString();
                txtReferPracticePersonSurname.Text = oDt.Rows[0]["ServiceProviderSurname"].ToString();
                comboReferPracticeSpeciality.SelectedValue = oDt.Rows[0]["SpecialityID"].ToString();
                txtReferPracticeContactNumber.Text = oDt.Rows[0]["PhoneNumber"].ToString();
                lblReferringPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();
            }
        }

        private void picCurrPracticeLookup_Click(object sender, EventArgs e)
        {
            ServiceProviderLookup oSPLookup = new ServiceProviderLookup(true
                //, txtCurrentPracticeName.Text.Substring(txtCurrentPracticeName.Text.IndexOf(" - ") + 3)
                , (txtCurrentPracticeName.Text.Contains(" - ")?txtCurrentPracticeName.Text.Substring(txtCurrentPracticeName.Text.IndexOf(" - ") + 3):"")
                , txtCurrentPracticePersonSurname.Text
                , txtCurrentPracticePersonName.Text);
            oSPLookup.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //oSPLookup.MdiParent = this.MdiParent;
            oSPLookup.ShowDialog();
            switch (oSPLookup.oReturn)
            {
                case "None":
                    break;
                case "New":
                    {
                        ServiceProvider oServiceProvider = new ServiceProvider(-1);
                        oServiceProvider.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                        //oServiceProvider.MdiParent = this.MdiParent;
                        oServiceProvider.ShowDialog();

                        DataTable oDt = oShared.usp_ServiceProvider_Select(oServiceProvider.ServiceProviderID);

                        BindCurrentPractice(oDt);

                        break;
                    }
                case "ServiceProvider":
                    {
                        DataTable oDt = oShared.usp_ServiceProvider_Select(oSPLookup.ServiceProviderID);

                        BindCurrentPractice(oDt);

                        break;
                    }
                default: break;
            }
            
            
        }

        private void BindCurrentPractice(DataTable oDt)
        {
            if (oDt.Rows.Count > 0)
            {
                txtCurrentPracticeName.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                txtCurrentPracticePersonName.Text = oDt.Rows[0]["ServiceProviderName"].ToString();
                txtCurrentPracticePersonSurname.Text = oDt.Rows[0]["ServiceProviderSurname"].ToString();
                comboCurrentPracticeSpeciality.SelectedValue = oDt.Rows[0]["SpecialityID"].ToString();
                txtCurrentPracticeContactNumber.Text = oDt.Rows[0]["PhoneNumber"].ToString();
                lblCurrentPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();
            }

            //Do AutoComplete
            //ToDo: (Billing) Taken out for Tariff Change. Needs to be put back
            if (Program.EnableBilling)
            {
                //txtAccountNumber.Enabled = true;
                //txtAccountNumber.Text = "";
                AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
                DataTable oDtNamesCollection = oFin.usp_Case_Billing_SelectAccountNumbers_ByServiceProviderID(Convert.ToInt32(lblCurrentPracticeID.Text));
                string[] oResultName = new string[oDtNamesCollection.Rows.Count];
                for (int i = 0; i < oDtNamesCollection.Rows.Count; i++)
                {
                oResultName[i] = oDtNamesCollection.Rows[i][0].ToString();
                }
                namesCollection.AddRange(oResultName);
                txtAccountNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
                //comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtAccountNumber.AutoCompleteCustomSource = namesCollection;
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CaseID == -1)
            {
                if (tabControlMain.SelectedTab == tabCodes
                    || tabControlMain.SelectedTab == tabChecklist
                    || tabControlMain.SelectedTab == tabNappiCodes
                    || tabControlMain.SelectedTab == tabNotes
                    || tabControlMain.SelectedTab == tabTreatmentDates
                    || tabControlMain.SelectedTab == tabComments
                    //|| tabControlMain.SelectedTab == tabBilling
                    )
                {
                    MessageBox.Show("You have to save the case before you can add the rest of the information");
                    tabControlMain.SelectedTab = tabCaseDetail;
                }
            }
        }

        //private void picTarrifSearch_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        lblCaseID_TariffID.Text = "-1";

        //        oCodeTariff.ShowDialog();
        //        if (oCodeTariff.oDr != null)
        //        {
        //            if (oCodeTariff.oDr.DataBoundItem != null)
        //            {
        //                txtTarrifCode.Text = oCodeTariff.oDr.Cells["TariffCode"].Value.ToString();
        //                txtTarrifDescription.Text = oCodeTariff.oDr.Cells["TariffDescription"].Value.ToString();
        //                txtTarrifValue.Text = oCodeTariff.oDr.Cells["Tariff"].Value.ToString();
        //                lblTariffID.Text = oCodeTariff.oDr.Cells["TariffID"].Value.ToString();

        //                if (oCodeTariff.oDr.Cells["Exclusion"].Value.ToString() == "1")
        //                {
        //                    txtTarrifDescription.Text = "This item is not covered by the medical funder.\n\r" + txtTarrifDescription.Text;
        //                    txtTarrifValue.Text = "0.00";
        //                }

        //                DataTable oDt = oCM.usp_Case_Tariff_Insert(
        //                    Convert.ToInt32(lblCaseID_TariffID.Text)
        //                    , CaseID
        //                    , Convert.ToInt32(lblTariffID.Text)
        //                    , Convert.ToDecimal(txtTarrifValue.Text)
        //                    , Convert.ToInt32(numericTariffQty.Value)
        //                    , dateTarrifDateOfProcedure.Value
        //                    , DateTime.Today
        //                    , Program.Username);
        //                lblCaseID_TariffID.Text = oDt.Rows[0][0].ToString();
        //                datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
        //            }
        //        }
        //    }

        //    catch (Exception err)
        //    {
        //        MessageBox.Show(err.Message.ToString());
        //    }
        //}

        private void picCPTSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bool defaultPrimary = false;
                if (datagridCptCodes.Rows.Count == 0)
                {
                    defaultPrimary = true;
                    rbCptPrimary.Checked = true;
                    rbCptSecondary.Checked = false;
                }

                lblCaseID_CPTID.Text = "-1";
                oCodeCPT.ShowDialog();
                if (oCodeCPT.oDr != null)
                {
                    if (oCodeCPT.oDr.DataBoundItem != null)
                    {
                        txtCptCode.Text = oCodeCPT.oDr.Cells["CODE"].Value.ToString();
                        txtCptDescription.Text = oCodeCPT.oDr.Cells["LONG_DESCR"].Value.ToString();
                        lblCptID.Text = oCodeCPT.oDr.Cells["CPTID"].Value.ToString();

                        oCM.usp_Case_CPT_Insert(
                            Convert.ToInt32(lblCaseID_CPTID.Text)
                            , CaseID
                            , Convert.ToInt32(lblCptID.Text)
                            , dateCptDateOfProcedure.Value
                            , rbCptPrimary.Checked
                            , rbCptSecondary.Checked
                            , DateTime.Today
                            , Program.Username);
                        datagridCptCodes.DataSource = oCM.usp_Case_CPT_Select(CaseID);

                        if (defaultPrimary)
                        {
                            defaultPrimary = false;
                            rbCptPrimary.Checked = false;
                            rbCptSecondary.Checked = true;
                        }
                    }
                }
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void picIcdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //lblICDID.Text = "-1";
                oCodeICD.ShowDialog();
                if (oCodeICD.oDr != null)
                {
                    if (oCodeICD.oDr.DataBoundItem != null)
                    {
                        bool defaultPrimary = false;
                        if (datagridIcd.Rows.Count == 0)
                        {
                            defaultPrimary = true;
                            rbIcdPrimary.Checked = true;
                            rbIcdSecondary.Checked = false;
                            rbIcdCoMorbidity.Checked = false;
                        }

                        txtIcdCode.Text = oCodeICD.oDr.Cells["DiagnosisCode"].Value.ToString();
                        txtIcdDescription.Text = oCodeICD.oDr.Cells["DiagnosisDesc"].Value.ToString();
                        lblICDID.Text = oCodeICD.oDr.Cells["ICDID"].Value.ToString();

                        oCM.usp_Case_ICD_Insert(
                            CaseID
                            , Convert.ToInt32(lblICDID.Text)
                            , dateIcdDateOfProcedure.Value
                            , rbIcdPrimary.Checked
                            , rbIcdSecondary.Checked
                            , rbIcdCoMorbidity.Checked
                            , DateTime.Today
                            , Program.Username);
                        datagridIcd.DataSource = oCM.usp_Case_ICD_Select(CaseID);

                        if (defaultPrimary)
                        {
                            defaultPrimary = false;
                            rbIcdPrimary.Checked = false;
                            rbIcdSecondary.Checked = true;
                            rbIcdCoMorbidity.Checked = false;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void picTarrifAdd_Click(object sender, EventArgs e)
        {
            AddTariff();
        }

        private void picCPTAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool defaultPrimary = false;
                if (datagridCptCodes.Rows.Count == 0)
                {
                    defaultPrimary = true;
                    rbCptPrimary.Checked = true;
                    rbCptSecondary.Checked = false;
                }
                oCM.usp_Case_CPT_Insert(
                    Convert.ToInt32(lblCaseID_CPTID.Text)
                    , CaseID
                    , Convert.ToInt32(lblCptID.Text)
                    , dateCptDateOfProcedure.Value
                    , rbCptPrimary.Checked
                    , rbCptSecondary.Checked
                    , DateTime.Today
                    , Program.Username);
                datagridCptCodes.DataSource = oCM.usp_Case_CPT_Select(CaseID);

                if (defaultPrimary)
                {
                    defaultPrimary = false;
                    rbCptPrimary.Checked = false;
                    rbCptSecondary.Checked = true;
                }
            }
            catch
            {
                MessageBox.Show("Please select a CPT code first");
            }
        }

        private void picIcdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool defaultPrimary = false;
                if (datagridIcd.Rows.Count == 0)
                {
                    defaultPrimary = true;
                    rbIcdPrimary.Checked = true;
                    rbIcdSecondary.Checked = false;
                    rbIcdCoMorbidity.Checked = false;
                }

                oCM.usp_Case_ICD_Insert(
                    CaseID
                    , Convert.ToInt32(lblICDID.Text)
                    , dateIcdDateOfProcedure.Value
                    , rbIcdPrimary.Checked
                    , rbIcdSecondary.Checked
                    , rbIcdCoMorbidity.Checked
                    , DateTime.Today
                    , Program.Username);
                datagridIcd.DataSource = oCM.usp_Case_ICD_Select(CaseID);

                if (defaultPrimary)
                {
                    defaultPrimary = false;
                    rbIcdPrimary.Checked = false;
                    rbIcdSecondary.Checked = true;
                    rbIcdCoMorbidity.Checked = false;
                }
            }
            catch
            {
                MessageBox.Show("Please select an ICD code first");
            }
        }

        private void picExclusionsAdd_Click(object sender, EventArgs e)
        {
            oCM.usp_Case_Exclusion_Insert(
                CaseID
                , Convert.ToInt32(comboExclusions.SelectedValue)
                , txtExclusionsComments.Text
                , DateTime.Today
                , Program.Username);
            datagridExclusionCodes.DataSource = oCM.usp_Case_Exclusion_Select(CaseID);
        }

        private void picTarrifDelete_Click(object sender, EventArgs e)
        {
            try
            {
                oCM.usp_Case_Tariff_Delete(
                    Convert.ToInt32(datagridTarrifs.CurrentRow.Cells["Seq"].Value)
                    , Program.Username);
                datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
            }
            catch
            {
            }
        }

        private void picCPTDelete_Click(object sender, EventArgs e)
        {
            try
            {
                oCM.usp_Case_CPT_Delete(
                   Convert.ToInt32(datagridCptCodes.CurrentRow.Cells["Seq"].Value)
                    , Program.Username);
                datagridCptCodes.DataSource = oCM.usp_Case_CPT_Select(CaseID);
            }
            catch
            {
            }
        }

        private void picICDDelete_Click(object sender, EventArgs e)
        {
            try
            {
                oCM.usp_Case_ICD_Delete(
                    CaseID
                    , Convert.ToInt32(datagridIcd.CurrentRow.Cells["ICDID"].Value)
                    , Program.Username);
                datagridIcd.DataSource = oCM.usp_Case_ICD_Select(CaseID);
            }
            catch
            {
            }
        }

        private void picExclusionsDelete_Click(object sender, EventArgs e)
        {
            oCM.usp_Case_Exclusion_Delete(
                CaseID
                , Convert.ToInt32(datagridExclusionCodes.CurrentRow.Cells["ExclusionID"].Value)
                , Program.Username);
            datagridExclusionCodes.DataSource = oCM.usp_Case_Exclusion_Select(CaseID);
        }

        private void picCaseNoteAdd_Click(object sender, EventArgs e)
        {
            bool ValidAmount = true;
            try
            {
                decimal test = Convert.ToDecimal(txtUpdatesInterimAmount.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("The interim amount is not in the correct format");
                ValidAmount = false;
            }
            if (ValidAmount)
            {
                oCM.usp_CaseNote_Insert(
                    txtCaseNote.Text
                    , Program.Username
                    , Convert.ToDecimal(txtUpdatesInterimAmount.Text)
                    , txtUpdatesCaseNumber.Text
                    , dateCaseNoteDate.Value
                    , CaseID
                    , Convert.ToDecimal(txtInterimAccomodation.Text)
                    , Convert.ToDecimal(txtInterimHospital.Text)
                    , Convert.ToDecimal(txtInterimDialysis.Text)
                    , Convert.ToDecimal(txtInterimPhysio.Text)
                    , Convert.ToDecimal(txtInterimRadiology.Text)
                    , Convert.ToDecimal(txtInterimSpecialist.Text)
                    , Convert.ToDecimal(txtInterimTransport.Text)
                    , Convert.ToDecimal(txtInterimScript.Text));
                datagridCaseNotes.DataSource = oCM.usp_CaseNote_Select(CaseID);
                txtInterimAmount.Text = txtUpdatesInterimAmount.Text;
            }
            else
            {
                MessageBox.Show("Not Saved");
            }
        }

        private void datagridChecklistAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagridChecklist.Rows.Count; i++)
            {
                oCM.usp_Case_Checklist_Update(
                    CaseID
                    , Convert.ToInt32(datagridChecklist.Rows[i].Cells["ChecklistTemplateID"].Value)
                    , Convert.ToBoolean(datagridChecklist.Rows[i].Cells["Checked"].Value)
                    , Program.Username
                    , DateTime.Today
                    , datagridChecklist.Rows[i].Cells["Comments"].Value.ToString()
                    , Convert.ToBoolean(datagridChecklist.Rows[i].Cells["NotApplicable"].Value)
                    );
            }
            datagridChecklist.DataSource = oCM.usp_Case_Checklist_Select(CaseID);

            bool Completed = true;
            for (int i = 0; i < datagridChecklist.Rows.Count; i++)
            {
                if (!
                    (Convert.ToBoolean(datagridChecklist.Rows[i].Cells["Checked"].Value) == true
                    || Convert.ToBoolean(datagridChecklist.Rows[i].Cells["NotApplicable"].Value) == true)
                    )
                {
                    Completed = false;
                    break;
                }
            }
            if (Completed)
            {
                DialogResult oResult = MessageBox.Show("Do you want to update the status of this case to completed?", "Checklist Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (oResult == System.Windows.Forms.DialogResult.Yes)
                {
                    oCM.usp_Cases_Update_Status(CaseID, 3, Program.Username);//Status 3 = Completed
                    comboCaseStatus.SelectedValue = 3;
                }
            }
        }

        private void datagridChecklist_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            datagridChecklist.Columns["ChecklistPrompt"].ReadOnly = true;
            datagridChecklist.Columns["UserID"].ReadOnly = true;
            datagridChecklist.Columns["Date"].ReadOnly = true;
            datagridChecklist.Columns["CaseID"].ReadOnly = true;
            datagridChecklist.Columns["ChecklistTemplateID"].ReadOnly = true;
        }

        private void datagridTarrifs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblCaseID_TariffID.Text = datagridTarrifs.Rows[e.RowIndex].Cells["Seq"].Value.ToString();
                txtTarrifCode.Text = datagridTarrifs.Rows[e.RowIndex].Cells["TariffCode"].Value.ToString();
                txtTarrifDescription.Text = datagridTarrifs.Rows[e.RowIndex].Cells["TariffDescription"].Value.ToString();
                txtTarrifValue.Text = datagridTarrifs.Rows[e.RowIndex].Cells["Value"].Value.ToString();
                lblTariffID.Text = datagridTarrifs.Rows[e.RowIndex].Cells["TariffID"].Value.ToString();
                dateTarrifDateOfProcedure.Value = Convert.ToDateTime(datagridTarrifs.Rows[e.RowIndex].Cells["DateOfProcedure"].Value.ToString());
                numericTariffQty.Value = Convert.ToDecimal(datagridTarrifs.Rows[e.RowIndex].Cells["Qty"].Value);
                chkTariffPerUnit.Checked = Convert.ToBoolean(datagridTarrifs.Rows[e.RowIndex].Cells["ValueIsTotal"].Value.ToString());
                chkTariffRejectRecord.Checked = Convert.ToBoolean(datagridTarrifs.Rows[e.RowIndex].Cells["Rejected"].Value.ToString());
                txtTariffAgreedRateOverrride.Text = datagridTarrifs.Rows[e.RowIndex].Cells["AgreedRateOverride"].Value.ToString();
                if (txtTariffAgreedRateOverrride.Text == "")
                    txtTariffAgreedRateOverrride.Text = "0";
            }
            catch { }
        }

        private void datagridCptCodes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblCaseID_CPTID.Text = datagridCptCodes.Rows[e.RowIndex].Cells["Seq"].Value.ToString();
                txtCptCode.Text = datagridCptCodes.Rows[e.RowIndex].Cells["Code"].Value.ToString();
                txtCptDescription.Text = datagridCptCodes.Rows[e.RowIndex].Cells["Long_Descr"].Value.ToString();
                rbCptPrimary.Checked = Convert.ToBoolean(datagridCptCodes.Rows[e.RowIndex].Cells["PrimaryCode"].Value.ToString());
                rbCptSecondary.Checked = Convert.ToBoolean(datagridCptCodes.Rows[e.RowIndex].Cells["SecondaryCode"].Value.ToString());
                lblCptID.Text = datagridCptCodes.Rows[e.RowIndex].Cells["CPTID"].Value.ToString();
                dateCptDateOfProcedure.Value = Convert.ToDateTime(datagridCptCodes.Rows[e.RowIndex].Cells["DateOfProcedure"].Value.ToString());
            }
            catch { }
        }

        private void datagridIcd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtIcdCode.Text = datagridIcd.Rows[e.RowIndex].Cells["DiagnosisCode"].Value.ToString();
                txtIcdDescription.Text = datagridIcd.Rows[e.RowIndex].Cells["DiagnosisDesc"].Value.ToString();
                dateIcdDateOfProcedure.Value = Convert.ToDateTime(datagridIcd.Rows[e.RowIndex].Cells["DateOfProcedure"].Value.ToString());
                rbIcdCoMorbidity.Checked = Convert.ToBoolean(datagridIcd.Rows[e.RowIndex].Cells["CoMorbiditycode"].Value.ToString());
                rbIcdPrimary.Checked = Convert.ToBoolean(datagridIcd.Rows[e.RowIndex].Cells["PrimaryCode"].Value.ToString());
                rbIcdSecondary.Checked = Convert.ToBoolean(datagridIcd.Rows[e.RowIndex].Cells["SecondaryCode"].Value.ToString());
                lblICDID.Text = datagridIcd.Rows[e.RowIndex].Cells["ICDID"].Value.ToString();
            }
            catch { }
        }

        private void datagridExclusionCodes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboExclusions.SelectedValue = datagridExclusionCodes.Rows[e.RowIndex].Cells["ExclusionID"].Value;
            txtExclusionsComments.Text = datagridExclusionCodes.Rows[e.RowIndex].Cells["Comment"].Value.ToString();
        }

        private void datagridTarrifs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            txtTarrifTotalCaptured.Text = "0";
            decimal tarrifTotalCaptured = 0;
            decimal totalOvercharged = 0;
            decimal totalPenalty = 0;
            decimal total = 0;

            for (int i = 0; i < datagridTarrifs.Rows.Count; i++)
            {

                if (Convert.ToBoolean(datagridTarrifs.Rows[i].Cells["ValueIsTotal"].Value))
                {
                    tarrifTotalCaptured = tarrifTotalCaptured
                    + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Value"].Value);
                }
                else
                {
                    tarrifTotalCaptured = tarrifTotalCaptured
                    + (Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Value"].Value)
                    * Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Qty"].Value));
                }

                totalOvercharged += Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["TotalOvercharged"].Value);

                DataGridViewCellStyle oStyle = new DataGridViewCellStyle();
                oStyle.BackColor = System.Drawing.Color.FromName(datagridTarrifs.Rows[i].Cells["Colour"].Value.ToString());
                datagridTarrifs.Rows[i].DefaultCellStyle = oStyle;
            }
            totalPenalty = (tarrifTotalCaptured - totalOvercharged) * (numPenaltyPercentage.Value / 100);
            total = tarrifTotalCaptured - totalOvercharged - totalPenalty;

            txtTarrifTotal.Text = total.ToString();
            txtTarrifTotalCaptured.Text = tarrifTotalCaptured.ToString();
            txtTarrifTotalOvercharged.Text = totalOvercharged.ToString();
            txtTarrifTotalPenalty.Text = totalPenalty.ToString();
        }

        private void datagridDaysInCare_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            txtTotalLOS.Text = "0";

            for (int i = 0; i < datagridDaysInCare.Rows.Count; i++)
            {
                txtTotalLOS.Text = Convert.ToString(Convert.ToDecimal(txtTotalLOS.Text) + Convert.ToDecimal(datagridDaysInCare.Rows[i].Cells["LOS"].Value));
            }


        }
        
        private void datagridChecklist_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void datagridChecklist_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("{ENTER}");
        }

        private void comboCaseStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(comboCaseStatus.SelectedValue) == 3)
                {
                    bool Completed = true;
                    for (int i = 0; i < datagridChecklist.Rows.Count; i++)
                    {
                        if (!
                            (Convert.ToBoolean(datagridChecklist.Rows[i].Cells["Checked"].Value) == true
                            || Convert.ToBoolean(datagridChecklist.Rows[i].Cells["NotApplicable"].Value) == true)
                            )
                        {
                            Completed = false;
                            break;
                        }
                    }
                    if (Completed
                        && datagridChecklist.DataSource != null
                        && Convert.ToInt32(comboCaseStatus.SelectedValue) == 3)
                    {
                        oCM.usp_Cases_Update_Status(CaseID, 3, Program.Username);//Status 3 = Completed
                        comboCaseStatus.SelectedValue = 3;
                    }
                    else
                    {
                        MessageBox.Show("You cannot update this case as completed before the checklist is completed");
                        comboCaseStatus.SelectedValue = 1;
                    }
                }
            }
            catch
            {
                //Expected Failure with new cases
            }

            if (comboCaseStatus.Text != "Booking"
                && chkWasBooking.Checked
                && lblChangeToCaseDate.Text == "N/A")
            {
                lblChangeToCaseDate.Text = DateTime.Now.ToString();
            }
            else if (comboCaseStatus.Text == "Booking"
                && !chkWasBooking.Checked)
            {
                chkWasBooking.Checked = true;
            }
        }

        private void Case_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CaseID != -1)
            {
                oCM.usp_Session_User_Case_Delete(CaseID);
            }
        }

        private void picCreateBooking_Click(object sender, EventArgs e)
        {
            if (CaseID != -1)
            {
                BookingAdd oBookingsAdd = new BookingAdd(-1, CaseID,-1);
                oBookingsAdd.ShowDialog();
            }
            else if (lblMemberID.Text != "")
            {
                BookingAdd oBookingsAdd = new BookingAdd(-1, -1,Convert.ToInt32(lblMemberID.Text));
                oBookingsAdd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please save the case before adding a booking.");
            }

        }

        private void picDeleteCaseNote_Click(object sender, EventArgs e)
        {
            oCM.usp_CaseNote_Delete(
                Convert.ToInt32(datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["CaseNoteID"].Value));
            datagridCaseNotes.DataSource = oCM.usp_CaseNote_Select(CaseID);
        }

        private void picCaseNoteUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bool ValidAmount = true;
                try
                {
                    decimal test = Convert.ToDecimal(txtUpdatesInterimAmount.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("The interim amount is not in the correct format");
                    ValidAmount = false;
                }
                if (ValidAmount)
                {
                    DialogResult oResult = new System.Windows.Forms.DialogResult();
                    CustomMessageBox oCustom = new CustomMessageBox(
                        "Are you sure you want to update this note:\n\r" +
                        datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["UserID"].Value.ToString() + ":\n\r" +
                        datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["CaseNote"].Value.ToString()
                        , "Update Confirmation"
                        , true);
                    oCustom.ShowDialog();
                    oResult = oCustom.DialogResult;
                        //MessageBox.Show("Are you sure you want to update this note:\n\r" +
                        //datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["UserID"].Value.ToString() + ":\n\r" +
                        //datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["CaseNote"].Value.ToString()
                        //, "Update Confirmation"
                        //, MessageBoxButtons.YesNo);

                    if (oResult == DialogResult.Yes)
                    {
                        oCM.usp_CaseNote_Update(
                        Convert.ToInt32(datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["CaseNoteID"].Value.ToString())
                        , txtCaseNote.Text
                        , Program.Username
                        , Convert.ToDecimal(txtUpdatesInterimAmount.Text)
                        , txtUpdatesCaseNumber.Text
                        , DateTime.Now
                        , Convert.ToDecimal(txtInterimAccomodation.Text)
                        , Convert.ToDecimal(txtInterimHospital.Text)
                        , Convert.ToDecimal(txtInterimDialysis.Text)
                        , Convert.ToDecimal(txtInterimPhysio.Text)
                        , Convert.ToDecimal(txtInterimRadiology.Text)
                        , Convert.ToDecimal(txtInterimSpecialist.Text)
                        , Convert.ToDecimal(txtInterimTransport.Text)
                        , Convert.ToDecimal(txtInterimScript.Text));
                    }

                    int MaxID = 0;
                    for (int i = 0; i < datagridCaseNotes.Rows.Count; i++)
                    {
                        if (MaxID < Convert.ToInt32(datagridCaseNotes.Rows[i].Cells["CaseNoteID"].Value.ToString()))
                            MaxID = Convert.ToInt32(datagridCaseNotes.Rows[i].Cells["CaseNoteID"].Value.ToString());
                    }
                    if (MaxID == Convert.ToInt32(datagridCaseNotes.Rows[datagridCaseNotes.CurrentRow.Index].Cells["CaseNoteID"].Value.ToString()))
                    {
                        txtInterimAmount.Text = txtUpdatesInterimAmount.Text;
                    }
                    datagridCaseNotes.DataSource = oCM.usp_CaseNote_Select(CaseID);
                }
                else
                {
                    MessageBox.Show("Not Saved");
                }


            }
            catch
            {
                MessageBox.Show("You have to select a note before you can update");
            }

        }

        private void datagridCaseNotes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtCaseNote.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["CaseNote"].Value.ToString();
                txtUpdatesInterimAmount.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimAmount"].Value.ToString();
                dateCaseNoteDate.Value = Convert.ToDateTime(datagridCaseNotes.Rows[e.RowIndex].Cells["DateCreated"].Value.ToString());
                txtUpdatesCaseNumber.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["CaseNumber"].Value.ToString();
                txtInterimAccomodation.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimAccomodation"].Value.ToString();
                txtInterimHospital.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimHospital"].Value.ToString();
                txtInterimDialysis.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimPathology"].Value.ToString();
                txtInterimPhysio.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimPhysio"].Value.ToString();
                txtInterimRadiology.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimRadiology"].Value.ToString();
                txtInterimSpecialist.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimSpecialist"].Value.ToString();
                txtInterimTransport.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimTransport"].Value.ToString();
                txtInterimScript.Text = datagridCaseNotes.Rows[e.RowIndex].Cells["InterimScript"].Value.ToString();

                if (txtUpdatesInterimAmount.Text == "")
                {
                    txtUpdatesInterimAmount.Text = "0.00";
                }
            }
            catch
            {
                //Expected when the user does not click on a valid place on the grid.
            }
        }

        private void picExportCaseUpdates_Click(object sender, EventArgs e)
        {
            rpt_CaseUpdateExport oReport = new rpt_CaseUpdateExport(CaseID);
            oReport.ShowDialog();
        }

        private void picLinkToCase_Click(object sender, EventArgs e)
        {
            CaseLinkLookup oCl = new CaseLinkLookup(Convert.ToInt32(lblMemberID.Text), CaseID);
            oCl.ShowDialog();
            if (oCl.LinkCaseID != Convert.ToInt32(txtCaseID.Text))
            {
                if (oCl.AuthNumber != "")
                {
                    oCM.usp_Case_Link_Insert(oCl.LinkCaseID, CaseID);
                    picLinkToCase.Visible = false;
                    picLinkToCase.Enabled = false;
                    lblLinkToCase.Visible = false;
                    picPrintLinkedCases.Visible = true;
                    picPrintLinkedCases.Enabled = true;

                    picUnlinkCase.Visible = true;
                    lblUnlinkCase.Visible = true;
                    picUnlinkCase.Enabled = true;
                    DataTable oDtLinks = oCM.usp_Case_Link_Select(CaseID);
                    datagridCases.DataSource = oDtLinks;

                    //If case is copied, then should not be able to change Case Category
                    //TODO: find a better way to force this i the database - Create method from initial load, duplicated code
                    //if (Convert.ToInt32(oDtLinks.Rows[0]["CaseID"].ToString()) < CaseID)
                    //{
                    //    comboCaseCategory.Enabled = false;
                    //}
                    datagridCases.DataSource = oDtLinks;
                }
            }
            else
            {
                MessageBox.Show("You cannot link a case to itself");
            }

        }

        private void picUnlinkCase_Click(object sender, EventArgs e)
        {
            int CaseNumber = Convert.ToInt32(datagridCases.Rows[0].Cells["CaseID"].Value.ToString());
            oCM.usp_Case_Link_Delete(CaseNumber, CaseID);

            picLinkToCase.Visible = true;
            picLinkToCase.Enabled = true;
            lblLinkToCase.Visible = true;
            picPrintLinkedCases.Visible = false;
            picPrintLinkedCases.Enabled = false;

            picUnlinkCase.Visible = false;
            lblUnlinkCase.Visible = false;
            picUnlinkCase.Enabled = false;
            datagridCases.DataSource = new DataTable();
        }

        private void picPrintLinkedCases_Click(object sender, EventArgs e)
        {
            rpt_PrintLinkedCases orpt_PrintLinkedCases = new rpt_PrintLinkedCases(CaseID);
            try
            {
                orpt_PrintLinkedCases.MdiParent = this.MdiParent;
            }
            catch
            {
            }
            orpt_PrintLinkedCases.Show();
        }

        private void btnChecklistCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagridChecklist.Rows.Count; i++)
            {
                if (!
                    (Convert.ToBoolean(datagridChecklist.Rows[i].Cells["Checked"].Value) == true
                    || Convert.ToBoolean(datagridChecklist.Rows[i].Cells["NotApplicable"].Value) == true)
                    )
                {
                    datagridChecklist.Rows[i].Cells["Checked"].Value = true;
                }
            }
        }

        private void dateDaysInCareAdmitted_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!InitialLoad)
                {
                    DateTime admitted = Convert.ToDateTime(dateDaysInCareAdmitted.Value.ToString("yyyy/MM/dd ") + timeDaysInCareAdmitted.Value.ToString("hh:mm t\\M"));
                    DateTime discharged = Convert.ToDateTime(dateDaysInCareDischarged.Value.ToString("yyyy/MM/dd ") + timeDaysInCareDischarged.Value.ToString("hh:mm t\\M"));

                    if (admitted > discharged)
                    {
                        dateDaysInCareDischarged.Value = admitted.AddDays((Double)numDaysInCareLOS.Value);
                    }

                    TimeSpan ts = new DateTime(
                     discharged.Year
                     , discharged.Month
                     , discharged.Day
                        //, discharged.Hour
                        //, discharged.Minute
                        //, 0
                     )
                     -
                     new DateTime(
                     admitted.Year
                     , admitted.Month
                     , admitted.Day
                        //, admitted.Hour
                        //, admitted.Minute
                        //, 0
                     );

                    decimal days = 0;
                    days = Math.Truncate((Decimal)ts.TotalDays);
                    if (admitted.Hour < 12)
                    {
                        days += (decimal)1;
                    }
                    else if (admitted.Hour < 24)
                    {
                        days += (decimal)0.5;
                    }
                    if (discharged.Hour < 12)
                    {
                        days -= (decimal)0.5;
                    }

                    numDaysInCareLOS.Value = days;
                }
            }
            catch (Exception)
            {

                //Valid exception if the dates are being edited
            }
        }

        private void dateDaysInCareDischarged_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!InitialLoad)
                {
                    DateTime admitted = Convert.ToDateTime(dateDaysInCareAdmitted.Value.ToString("yyyy/MM/dd ") + timeDaysInCareAdmitted.Value.ToString("hh:mm t\\M"));
                    DateTime discharged = Convert.ToDateTime(dateDaysInCareDischarged.Value.ToString("yyyy/MM/dd ") + timeDaysInCareDischarged.Value.ToString("hh:mm t\\M"));

                    if (admitted > discharged)
                    {
                        dateDaysInCareDischarged.Value = admitted.AddDays((Double)numDaysInCareLOS.Value);
                        discharged = admitted.AddDays((Double)numDaysInCareLOS.Value);
                    }
                    
                    TimeSpan ts = new DateTime(
                    discharged.Year
                    , discharged.Month
                    , discharged.Day
                    //, discharged.Hour
                    //, discharged.Minute
                    //, 0
                    )
                    -
                    new DateTime(
                    admitted.Year
                    , admitted.Month
                    , admitted.Day
                    //, admitted.Hour
                    //, admitted.Minute
                    //, 0
                    );

                    decimal days = 0;
                    days = Math.Truncate((Decimal)ts.TotalDays);
                     if (admitted.Hour < 12)
                    {
                        days += (decimal)1;
                    }
                    else if (admitted.Hour < 24)
                    {
                        days += (decimal) 0.5;
                    }
                    if (discharged.Hour < 12)
                    {
                        days -= (decimal)0.5;
                    }

                    numDaysInCareLOS.Value = days;
                }
            }
            catch (Exception)
            {

                //Valid exception if the dates are being edited
            }
        }

        private void picDaysInCareEdit_Click(object sender, EventArgs e)
        {
            DialogResult oResultConfirm;
            oResultConfirm = MessageBox.Show("Are you sure you want to update the selected record?", "Confirmation", MessageBoxButtons.YesNo);
            if (oResultConfirm == System.Windows.Forms.DialogResult.Yes)
            {
                DateTime admitted = Convert.ToDateTime(dateDaysInCareAdmitted.Value.ToString("yyyy/MM/dd ") + timeDaysInCareAdmitted.Value.ToString("hh:mm t\\M"));
                DateTime discharged = Convert.ToDateTime(dateDaysInCareDischarged.Value.ToString("yyyy/MM/dd ") + timeDaysInCareDischarged.Value.ToString("hh:mm t\\M"));

                if (datagridDaysInCare.CurrentRow.Index >= 0)
                {
                    try
                    {
                        oCM.usp_Case_FacilityType_Update(
                        Convert.ToInt32(comboDaysInCareFacilityType.SelectedValue)
                        , CaseID
                        , Convert.ToDateTime(admitted)
                        , Convert.ToDateTime(discharged)
                        , Convert.ToDecimal(numDaysInCareLOS.Value)
                        , txtFacilityTypeCode.Text
                        , Convert.ToInt32((txtMinutesOnVentilator.Text == "" ? "0" : txtMinutesOnVentilator.Text))
                        , txtComments.Text
                        , DateTime.Today
                        , Program.Username
                        , Convert.ToInt32(datagridDaysInCare.CurrentRow.Cells["ID"].Value)
                        );
                        datagridDaysInCare.DataSource = oCM.usp_Case_FacilityType_Select(CaseID);
                    }
                    catch (Exception err)
                    {
                        if (err.Message.Contains("Violation of PRIMARY KEY constraint"))
                            MessageBox.Show("This record already exists");
                        else throw;
                    }

                    //Total discharged rule - must not be earlier than the latest discharge date
                    //Total admitted rule - Must not be later than the earliest admission date
                    DateTime TotalDischarged = Convert.ToDateTime(dateDischargeDate.Value.ToString("yyyy/MM/dd ") + timeDischargeTime.Value.ToString("hh:mm t\\M"));
                    DateTime TotalDischargedNew = TotalDischarged;

                    DateTime TotalAdmitted = Convert.ToDateTime(dateAdmissionDate.Value.ToString("yyyy/MM/dd ") + timeAdmissionTime.Value.ToString("hh:mm t\\M"));
                    DateTime TotalAdmittedNew = TotalAdmitted;
                    decimal TotalLOS = 0;

                    for (int i = 0; i < datagridDaysInCare.Rows.Count; i++)
                    {
                        if (TotalDischargedNew == TotalDischarged)
                        {//GEt date from daagrid
                            TotalDischargedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value);
                        }
                        if (Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value) > TotalDischargedNew)
                        {//Discharge
                            TotalDischargedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateDischarged"].Value);
                        }
                        if (Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateAdmitted"].Value) < TotalAdmittedNew)
                        {//Admission
                            TotalAdmittedNew = Convert.ToDateTime(datagridDaysInCare.Rows[i].Cells["DateAdmitted"].Value);
                        }
                        TotalLOS += Convert.ToDecimal(datagridDaysInCare.Rows[i].Cells["LOS"].Value);
                    }

                    //Where the latest discharge is greater than what is currently selected
                    if (TotalDischargedNew > TotalDischarged)
                    {
                        DialogResult oResult = MessageBox.Show("Warning: One of the discharge dates is later than the overall discharge date. "
                             + "\n\rDo you want to update the discharge date?"
                             , "Overall Discharge Date"
                             , MessageBoxButtons.YesNo);
                        if (oResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            dateDischargeDate.Value = TotalDischargedNew;
                            timeDischargeTime.Value = TotalDischargedNew;
                        }
                    }
                    //Where the latest discharge is SMALLER than what is currently selected
                    if (TotalDischargedNew < TotalDischarged)
                    {
                        DialogResult oResult = MessageBox.Show("Warning: The latest discharge date in the admission is earlier than the current overall discharge date "
                             + "\n\rDo you want to update the discharge date?"
                             , "Overall Discharge Date"
                             , MessageBoxButtons.YesNo);
                        if (oResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            dateDischargeDate.Value = TotalDischargedNew;
                            timeDischargeTime.Value = TotalDischargedNew;
                        }
                    }

                    if (TotalAdmittedNew < TotalAdmitted)
                    {
                        DialogResult oResult = MessageBox.Show("Warning: One of the admission dates is earlier than the overall admission date. "
                             + "\n\rDo you want to update the admission date?"
                             , "Overall Admission Date"
                             , MessageBoxButtons.YesNo);
                        if (oResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            dateAdmissionDate.Value = TotalAdmittedNew;
                            timeAdmissionTime.Value = TotalAdmittedNew;
                        }
                    }


                    //decimal LOS = 0;

                    //TimeSpan ts = new DateTime(
                    //discharged.Year
                    //, discharged.Month
                    //, discharged.Day
                    //, discharged.Hour
                    //, discharged.Minute
                    //, 0)
                    //-
                    //new DateTime(
                    //admitted.Year
                    //, admitted.Month
                    //, admitted.Day
                    //, admitted.Hour
                    //, admitted.Minute
                    //, 0);

                    //numDaysInCareLOS.Value = (Decimal)ts.TotalDays;

                    //LOS = (decimal)ts.Days;
                    //if (LOS < Convert.ToDecimal((txtTotalLOS.Text == "" ? "0" : txtTotalLOS.Text)))
                    //{
                    //    DialogResult oResult = MessageBox.Show("Warning: Total LOS is more days than the days between admission and discharge dates. "
                    //        + "\n\rDo you want to update the discharge date?"
                    //        , "Length of stay"
                    //        , MessageBoxButtons.YesNo);
                    //    if (oResult == System.Windows.Forms.DialogResult.Yes)
                    //    {
                    //        dateDischargeDate.Value = dateAdmissionDate.Value.AddDays(Convert.ToInt32(Math.Round(Convert.ToDecimal(txtTotalLOS.Text), 0)));
                    //    }
                    //}
                }
                else
                {
                    MessageBox.Show("You must select a record to update first.");
                }
            }
        }

        //private void txtDaysInCareDischarged_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

            
        //    if (!InitialLoad)
        //    {
        //        if (Convert.ToDateTime(txtDaysInCareAdmitted.Text) > Convert.ToDateTime(txtDaysInCareDischarged.Text))
        //        {
        //            txtDaysInCareDischarged.Text = Convert.ToDateTime(txtDaysInCareAdmitted.Text).AddDays((Double)numDaysInCareLOS.Value).ToString("yyyy/MM/dd hh:mm t\\M");
        //        }
        //        DateTime admitted = Convert.ToDateTime(txtDaysInCareAdmitted.Text);
        //        DateTime discharged = Convert.ToDateTime(txtDaysInCareDischarged.Text);
        //        TimeSpan ts = new DateTime(
        //        discharged.Year
        //        ,discharged.Month
        //        , discharged.Day
        //        , discharged.Hour
        //        ,discharged.Minute
        //        , 0)
        //        -
        //        new DateTime(
        //        admitted.Year
        //        , admitted.Month
        //        , admitted.Day
        //        , admitted.Hour
        //        , admitted.Minute
        //        , 0);
        //        numDaysInCareLOS.Value = (Decimal)ts.TotalDays;
        //    }
        //    }
        //    catch (Exception)
        //    {

        //       //Valid exception if the dates are being edited
        //    }
        //}

        //private void txtDaysInCareAdmitted_TextChanged(object sender, EventArgs e)
        //{
        //     try
        //    {

            
        //    if (!InitialLoad)
        //    {
        //        if (Convert.ToDateTime(txtDaysInCareAdmitted.Text) > Convert.ToDateTime(txtDaysInCareDischarged.Text))
        //        {
        //            txtDaysInCareDischarged.Text = Convert.ToDateTime(txtDaysInCareAdmitted.Text).AddDays((Double)numDaysInCareLOS.Value).ToString("yyyy/MM/dd hh:mm t\\M");
        //        }
        //        DateTime admitted = Convert.ToDateTime(txtDaysInCareAdmitted.Text);
        //        DateTime discharged = Convert.ToDateTime(txtDaysInCareDischarged.Text);
        //        TimeSpan ts = new DateTime(
        //        discharged.Year
        //        ,discharged.Month
        //        , discharged.Day
        //        , discharged.Hour
        //        ,discharged.Minute
        //        , 0)
        //        -
        //        new DateTime(
        //        admitted.Year
        //        , admitted.Month
        //        , admitted.Day
        //        , admitted.Hour
        //        , admitted.Minute
        //        , 0);
        //        numDaysInCareLOS.Value = (Decimal)ts.TotalDays;
        //    }
        //    }
        //     catch (Exception)
        //     {

        //         //Valid exception if the dates are being edited
        //     }
        //}

        private void dateDischargeDate_ValueChanged(object sender, EventArgs e)
        {
            if (!InitialLoad)
                if (dateAdmissionDate.Value > dateDischargeDate.Value)
                {
                    dateDischargeDate.Value = dateAdmissionDate.Value.AddDays(Convert.ToDouble((txtTotalLOS.Text == "" ? "0" : txtTotalLOS.Text)));
                }
        }

        private void dateAdmissionDate_ValueChanged(object sender, EventArgs e)
        {
            if (!InitialLoad)
                if (dateAdmissionDate.Value > dateDischargeDate.Value)
                {
                    dateDischargeDate.Value = dateAdmissionDate.Value.AddDays(Convert.ToDouble((txtTotalLOS.Text == "" ? "0" : txtTotalLOS.Text)));
                }
        }

        private void datagridDaysInCare_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                comboDaysInCareFacilityType.SelectedValue = datagridDaysInCare.Rows[e.RowIndex].Cells["FacilityTypeID"].Value.ToString();
                dateDaysInCareAdmitted.Value = Convert.ToDateTime(datagridDaysInCare.Rows[e.RowIndex].Cells["DateAdmitted"].Value);
                dateDaysInCareDischarged.Value = Convert.ToDateTime(datagridDaysInCare.Rows[e.RowIndex].Cells["DateDischarged"].Value);
                timeDaysInCareAdmitted.Value = Convert.ToDateTime(datagridDaysInCare.Rows[e.RowIndex].Cells["DateAdmitted"].Value);
                timeDaysInCareDischarged.Value = Convert.ToDateTime(datagridDaysInCare.Rows[e.RowIndex].Cells["DateDischarged"].Value);
                numDaysInCareLOS.Value = Convert.ToDecimal(datagridDaysInCare.Rows[e.RowIndex].Cells["LOS"].Value.ToString());
                txtFacilityTypeCode.Text = datagridDaysInCare.Rows[e.RowIndex].Cells["FacilityTypeCode"].Value.ToString();
                txtMinutesOnVentilator.Text = datagridDaysInCare.Rows[e.RowIndex].Cells["MinutesOnVentilator"].Value.ToString();
                txtComments.Text = datagridDaysInCare.Rows[e.RowIndex].Cells["Comments"].Value.ToString();
            }
            catch
            {
                //Valid exception if no rows are selected
            }

        }

        private void numDaysInCareLOS_ValueChanged(object sender, EventArgs e)
        {
            //InitialLoad = true;
            //DateTime admitted = Convert.ToDateTime(dateDaysInCareAdmitted.Value.ToString("yyyy/MM/dd ") + timeDaysInCareAdmitted.Value.ToString("hh:mm t\\M"));
            //DateTime discharged = Convert.ToDateTime(dateDaysInCareDischarged.Value.ToString("yyyy/MM/dd ") + timeDaysInCareDischarged.Value.ToString("hh:mm t\\M"));

            //dateDaysInCareDischarged.Value = admitted.AddDays((double)numDaysInCareLOS.Value);
            //timeDaysInCareDischarged.Value = dateDaysInCareDischarged.Value;

            //InitialLoad = false;
        }

        private void txtFinalInvoiceAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtFinalInvoiceAmount.Text != "")
            {
                lblFinalInvoiceAmountUpdated.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm t\\M");
                txtFinalInvoiceWithPenalty.Text = ((Convert.ToDecimal(1.00)- (numPenaltyPercentage.Value / 100)) * Convert.ToDecimal(txtFinalInvoiceAmount.Text)).ToString();
            }
        }

        private void picAddComment_Click(object sender, EventArgs e)
        {
            oCM.usp_CaseComment_Insert(
                txtComment.Text
                , Program.Username
                , DateTime.Now
                , CaseID);
            datagridComments.DataSource = oCM.usp_CaseComment_Select(CaseID);
        }

        private void picExportComments_Click(object sender, EventArgs e)
        {
            rpt_CaseCommentExport oReport = new rpt_CaseCommentExport(CaseID);
            oReport.ShowDialog();
        }

        private void picEditComment_Click(object sender, EventArgs e)
        {
            try
            {
              
                    DialogResult oResult = new System.Windows.Forms.DialogResult();
                    CustomMessageBox oCustom = new CustomMessageBox(
                        "Are you sure you want to update this Comment:\n\r" +
                        datagridComments.Rows[datagridComments.CurrentRow.Index].Cells["UserID"].Value.ToString() + ":\n\r" +
                        datagridComments.Rows[datagridComments.CurrentRow.Index].Cells["CaseComment"].Value.ToString()
                        , "Update Confirmation"
                        , true);
                    oCustom.ShowDialog();
                    oResult = oCustom.DialogResult;

                    if (oResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        oCM.usp_CaseComment_Update(
                        Convert.ToInt32(datagridComments.Rows[datagridComments.CurrentRow.Index].Cells["CaseCommentID"].Value.ToString())
                        , txtComment.Text
                        , Program.Username
                        , Convert.ToDateTime(datagridComments.Rows[datagridComments.CurrentRow.Index].Cells["DateCreated"].Value.ToString()));
                    }

                   
                    datagridComments.DataSource = oCM.usp_CaseComment_Select(CaseID);
              
            }
            catch
            {
                MessageBox.Show("You have to select a comment before you can update");
            }
        }

        private void picDeleteComment_Click(object sender, EventArgs e)
        {
            oCM.usp_CaseComment_Delete(
              Convert.ToInt32(datagridComments.Rows[datagridComments.CurrentRow.Index].Cells["CaseCommentID"].Value));
            datagridComments.DataSource = oCM.usp_CaseComment_Select(CaseID);
        }

        private void datagridComments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtComment.Text = datagridComments.Rows[e.RowIndex].Cells["CaseComment"].Value.ToString();
                
            }
            catch
            {
                //Expected when the user does not click on a valid place on the grid.
            }
        }

        private void txtTarrifCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }

        }

        private void txtTarrifValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void dateTarrifDateOfProcedure_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
            if (e.KeyChar == '\t')
            {
                AddTariff();
            }
        }

        private void numericTariffQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void AddTariff()
        {
            //access
            //"Billing Auditing"
            //"System Administrator"
            //"Metadata Administrator"
            //"Imports"
            //"Case Manager"
            string Detail = "";
            if (Program._GenericPrincipal.IsInRole("Billing Auditing"))
            {
                Detail += "Role is billing Auditing\n\r";
                if (txtTarrifValue.Text != "")
                {
                    Detail += "Tariff Value is not blank\n\r";
                    int updateTariffID = -1;
                    bool exists = false;
                    bool canInsertNew = true;
                    DialogResult oResult = System.Windows.Forms.DialogResult.Yes;

                    for (int i = 0; i < datagridTarrifs.Rows.Count; i++)
                    {
                        if ((datagridTarrifs.Rows[i].Cells["TariffCode"].Value.ToString() == txtTarrifCode.Text.Replace("*", "")
                                && (Convert.ToDateTime(datagridTarrifs.Rows[i].Cells["DateOfProcedure"].Value) == dateTarrifDateOfProcedure.Value
                                    )
                                )
                            )
                        {
                            updateTariffID = Convert.ToInt32(datagridTarrifs.Rows[i].Cells["Seq"].Value);
                            exists = true;
                            //if (datagridTarrifs.Rows[i].Cells["TariffCode"].Value.ToString() == txtTarrifCode.Text.Replace("*", "")
                            //    && Convert.ToDateTime(datagridTarrifs.Rows[i].Cells["DateOfProcedure"].Value) == dateTarrifDateOfProcedure.Value)
                            //{
                                canInsertNew = false;
                            //}
                        }
                    }

                    if (exists)
                    {
                        Detail += "Tariff line with same date and code already exists\n\r";
                        oResult = MessageBox.Show("Update existing tariff?", "Update or insert new", MessageBoxButtons.YesNo);
                    }

                    if (exists
                        && oResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        Detail += "You selected to update the existing tariff\n\r";
                        try
                        {
                            oCM.usp_Case_Tariff_Insert(
                                Convert.ToInt32(lblCaseID_TariffID.Text)
                                , CaseID
                                , Convert.ToInt32(lblTariffID.Text)
                                , Convert.ToDecimal(txtTarrifValue.Text)
                                , Convert.ToDecimal(numericTariffQty.Value)
                                    , Convert.ToDecimal(txtTariffAgreedRateOverrride.Text)
                                    , chkTariffPerUnit.Checked
                                    , chkTariffRejectRecord.Checked
                                , dateTarrifDateOfProcedure.Value
                                , DateTime.Today
                                , Program.Username);
                            datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Please select a tariff first\n\r" + (Program.DevMode == true? err.Message.ToString():""));
                        }

                    }
                    else if (exists)
                    {
                        Detail += "You selected not to update the existing tariff\n\r";
                        if (!canInsertNew)
                        {
                            MessageBox.Show("You cannot insert the same tariff more than once a day.\n\rRecord will not be created.");
                        }
                        else
                        {
                            try
                            {
                                oCM.usp_Case_Tariff_Insert(
                                    -1
                                    , CaseID
                                    , Convert.ToInt32(lblTariffID.Text)
                                    , Convert.ToDecimal(txtTarrifValue.Text)
                                    , Convert.ToDecimal(numericTariffQty.Value)
                                    , Convert.ToDecimal(txtTariffAgreedRateOverrride.Text)
                                    , chkTariffPerUnit.Checked
                                    , chkTariffRejectRecord.Checked
                                    , dateTarrifDateOfProcedure.Value
                                    , DateTime.Today
                                    , Program.Username);
                                datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show("Please select a tariff first\n\r\n\r" + (Program.DevMode == true ? err.Message.ToString() : ""));
                            }
                        }
                    }
                    else //Search for a tariff match in the database
                    {
                        Detail += "No existing tariff found, searching database\n\r";
                        int TariffID = -1;
                        int NumberOfRecords = 0;
                        DataTable oDtTariffSearchResult = oShared.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate(//MainClient specific Rules
                            txtTarrifCode.Text
                            , Convert.ToInt32(lblCurrentPracticeID.Text)
                            , dateTarrifDateOfProcedure.Value
                            , Program.MainClientID);

                        if (oDtTariffSearchResult.Rows.Count == 0 && !txtTarrifCode.Text.StartsWith("*"))
                        {//Add * for search
                            Detail += "No initial results found, searching with wildcard automatically\n\r";
                             oDtTariffSearchResult = oShared.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate(//MainClient specific Rules
                                "*" + txtTarrifCode.Text
                                , Convert.ToInt32(lblCurrentPracticeID.Text)
                                , dateTarrifDateOfProcedure.Value
                                , Program.MainClientID);
                        }

                        if (oDtTariffSearchResult.Rows.Count > 0)
                        {
                            Detail += "System counts the number of records found\n\r";
                            for (int i = 0; i < oDtTariffSearchResult.Rows.Count; i++)
                            {
                                if ((oDtTariffSearchResult.Rows[i]["SpSpeciality"].ToString() == "1"
                                    || oDtTariffSearchResult.Rows[i]["SpecialityID"].ToString() == comboCurrentPracticeSpeciality.SelectedValue.ToString())
                                    //Avoid a tariff being defaulted to a record with no value
                                    && Convert.ToDecimal(oDtTariffSearchResult.Rows[i]["FinalRate"].ToString()) != 0)
                                {
                                    TariffID = Convert.ToInt32(oDtTariffSearchResult.Rows[i]["TariffID"].ToString());
                                    NumberOfRecords++;
                                    break;
                                }
                            }

                            if (NumberOfRecords == 1)
                            {
                                Detail += "Record found that matches speciality\n\r";
                                oCM.usp_Case_Tariff_Insert(
                                -1
                                , CaseID
                                , TariffID
                                , Convert.ToDecimal(txtTarrifValue.Text)
                                , Convert.ToDecimal(numericTariffQty.Value)
                                    , Convert.ToDecimal(txtTariffAgreedRateOverrride.Text)
                                    , chkTariffPerUnit.Checked
                                    , chkTariffRejectRecord.Checked
                                , dateTarrifDateOfProcedure.Value
                                , DateTime.Today
                                , Program.Username);
                                datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                            }
                            else if (oDtTariffSearchResult.Rows.Count == 1)
                            {
                                Detail += "Only one record returned\n\r";
                                oCM.usp_Case_Tariff_Insert(
                                    -1
                                    , CaseID
                                    , Convert.ToInt32(oDtTariffSearchResult.Rows[0]["TariffID"].ToString())
                                    , Convert.ToDecimal(txtTarrifValue.Text)
                                    , Convert.ToDecimal(numericTariffQty.Value)
                                    , Convert.ToDecimal(txtTariffAgreedRateOverrride.Text)
                                    , chkTariffPerUnit.Checked
                                    , chkTariffRejectRecord.Checked
                                    , dateTarrifDateOfProcedure.Value
                                    , DateTime.Today
                                    , Program.Username);
                                datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                            }
                            else
                            {
                                Detail += "Multiple records returned. User needs to choose\n\r";
                                CustomMessageBoxWithGrid oMessageBox = new CustomMessageBoxWithGrid(oDtTariffSearchResult
                                    , "Select Tariff"
                                    , "This tariff exists for multiple specialities.\n\rPlease select the correct one and press enter or click yes.");
                                oMessageBox.ShowDialog();
                                if (oMessageBox.DialogResult == System.Windows.Forms.DialogResult.Yes)
                                {
                                    Detail += "You clicked 'Yes' for the record to be added\n\r";
                                    oCM.usp_Case_Tariff_Insert(
                                        -1
                                        , CaseID
                                        , Convert.ToInt32(oMessageBox.SelectedRow.Cells["TariffID"].Value)
                                        , Convert.ToDecimal(txtTarrifValue.Text)
                                        , Convert.ToDecimal(numericTariffQty.Value)
                                    , Convert.ToDecimal(txtTariffAgreedRateOverrride.Text)
                                    , chkTariffPerUnit.Checked
                                    , chkTariffRejectRecord.Checked
                                        , dateTarrifDateOfProcedure.Value
                                        , DateTime.Today
                                        , Program.Username);
                                    datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
                                }
                                else
                                {
                                    Detail += "You clicked 'No' for the record to be added\n\r";
                                    MessageBox.Show("Record was not added");

                                }
                            }

                        }
                        else if (oDtTariffSearchResult.Rows.Count == 0)
                        {
                            Detail += "No tariffs found\n\r";
                            MessageBox.Show("Tariff does not exist\n\rTry adding a '*' before or after the code to widen the search.");
                        }

                    }
                    txtTarrifCode.Focus();
                    txtTarrifCode.SelectAll();
                }
                else
                {
                    Detail += "Your tariff value is blank\n\r";
                    MessageBox.Show("Please enter a value");
                }
            }//Access
            if (Program.DevMode)
            {
                MessageBox.Show(Detail);
            }
        }

        private void picReportTariffs_Click(object sender, EventArgs e)
        {
            CaseTariffDetail ctd = new CaseTariffDetail(CaseID);
            ctd.ShowDialog();
            datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
        }

        private void txtTarrifCode_Enter(object sender, EventArgs e)
        {
            txtTarrifCode.SelectAll();
        }

        private void txtTarrifValue_Enter(object sender, EventArgs e)
        {
            txtTarrifValue.SelectAll();
        }

        private void numericTariffQty_Enter(object sender, EventArgs e)
        {
            numericTariffQty.Select(0, numericTariffQty.Value.ToString().Length);
        }

        private void txtTarrifCode_Click(object sender, EventArgs e)
        {
            txtTarrifCode.SelectAll();
        }

        private void txtTarrifDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void txtTarrifCode_TextChanged(object sender, EventArgs e)
        {
            numericTariffQty.Value = 1;
        }

        private void chkTariffPerUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void chkTariffRejectRecord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void txtTariffAgreedRateOverrride_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddTariff();
            }
        }

        private void picGallery_Click(object sender, EventArgs e)
        {
            Gallery oGal = new Gallery(CaseID);
            oGal.ShowDialog();
        }

        private void picLinkedDocumentsAdd_Click(object sender, EventArgs e)
        {
            SelectDocumentTypeDialog oTypeDialog = new SelectDocumentTypeDialog();
            //DialogResult oTypeResult = DialogResult.OK;
            oTypeDialog.ShowDialog();

            if (oTypeDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                OpenFileDialog oDialog = new OpenFileDialog();
                oDialog.CheckFileExists = true;
                oDialog.ShowDialog();

                if (oDialog.FileName != "")
                {
                    oCM.usp_Case_LinkedFile_Insert(CaseID
                        , Convert.ToInt32(lblMemberID.Text)
                        , oDialog.FileName
                        , oDialog.SafeFileName
                        , oTypeDialog.DocumentType
                        , Program.Username);
                }
                grdLinkedDocuments.DataSource = oCM.usp_Case_LinkedFile_SelectByCaseID(CaseID);
            }
        }

        private void picLinkedDocumentsDelete_Click(object sender, EventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                oCM.usp_Case_LinkedFile_DeleteByCase_LinkedFileID(Convert.ToInt32(grdLinkedDocuments.CurrentRow.Cells["Case_LinkedFileID"].Value));
            }
            grdLinkedDocuments.DataSource = oCM.usp_Case_LinkedFile_SelectByCaseID(CaseID);
        }

        private void grdLinkedDocuments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = grdLinkedDocuments.CurrentRow.Cells["FilePath"].Value.ToString();
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
        }

        private void picOpenFile_Click(object sender, EventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = grdLinkedDocuments.CurrentRow.Cells["FilePath"].Value.ToString();
                //psi.Arguments = oDt.Rows[0]["UpdatePath"].ToString();
                //if you don't want a console window popping up then set this property
                //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                //Create new process and set the starting information
                Process p = new Process
                {
                    StartInfo = psi
                };

                //Set this so that you can tell when the process has completed
                //p.EnableRaisingEvents = true;

                p.Start();

            }
        }

        private void AddNappi()
        {
            if (txtNappiValue.Text != "")
            {
                int updateNappiID = -1;
                int updateCaseIDNappiID = -1;
                bool exists = false;
                bool canInsertNew = true;
                DialogResult oResult = System.Windows.Forms.DialogResult.Yes;

                for (int i = 0; i < datagridNappiCodes.Rows.Count; i++)
                {
                    if (
                        (datagridNappiCodes.Rows[i].Cells["NappiCode"].Value.ToString() == txtNappiCode.Text.Replace("*", "")
                            && (Convert.ToDateTime(datagridNappiCodes.Rows[i].Cells["Date"].Value) == dateNappiDate.Value)
                            )
                        )
                    {
                        updateCaseIDNappiID = Convert.ToInt32(datagridNappiCodes.Rows[i].Cells["CaseID_NappiID"].Value);
                        updateNappiID = Convert.ToInt32(datagridNappiCodes.Rows[i].Cells["NappiID"].Value);
                        
                        exists = true;
                        if (datagridNappiCodes.Rows[i].Cells["NappiCode"].Value.ToString() == txtNappiCode.Text.Replace("*", "")
                            && Convert.ToDateTime(datagridNappiCodes.Rows[i].Cells["Date"].Value) == dateNappiDate.Value)
                        {
                            canInsertNew = false;
                        }
                    }
                }

                if (exists)
                {
                    oResult = MessageBox.Show("Update existing Nappi?", "Update or insert new", MessageBoxButtons.YesNo);
                }

                if (exists
                    && oResult == System.Windows.Forms.DialogResult.Yes)
                {

                    try
                    {
                        oCM.usp_Case_NappiCodes_Insert(
                            updateCaseIDNappiID//Convert.ToInt32(lblCaseID_NappiID.Text)
                            , CaseID
                            , updateNappiID//Convert.ToInt32(lblNappiID.Text)
                            , Convert.ToDecimal(txtNappiValue.Text)
                            , Convert.ToDecimal(numericNappiQty.Value)
                            , rbNappiDispensary.Checked
                            , rbNappiWard.Checked
                            , rbNappiTheater.Checked
                            , rbNappiTTO.Checked
                            , rbNappi0201.Checked
                            , dateNappiDate.Value
                            , Program.Username);
                        datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
                    }
                    catch
                    {
                        MessageBox.Show("Please select a Nappi first");
                    }

                }
                else if (exists)
                {
                    if (!canInsertNew)
                    {
                        MessageBox.Show("You cannot insert the same Nappi more than once a day.\n\rRecord will not be created.");
                    }
                    else
                    {
                        try
                        {
                            oCM.usp_Case_NappiCodes_Insert(
                            -1//updateCaseIDNappiID//Convert.ToInt32(lblCaseID_NappiID.Text)
                            , CaseID
                            , updateNappiID//Convert.ToInt32(lblNappiID.Text)
                            , Convert.ToDecimal(txtNappiValue.Text)
                            , Convert.ToDecimal(numericNappiQty.Value)
                            , rbNappiDispensary.Checked
                            , rbNappiWard.Checked
                            , rbNappiTheater.Checked
                            , rbNappiTTO.Checked
                            , rbNappi0201.Checked
                            , dateNappiDate.Value
                            , Program.Username);
                            datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
                        }
                        catch
                        {
                            MessageBox.Show("Please select a Nappi first");
                        }
                    }
                }
                else //Search for a Nappi match in the database
                {
                    //int NappiID = -1;
                    //int NumberOfRecords = 0;
                    DataTable oDtNappiSearchResult = oShared.usp_NappiCodes_Select_ByNappiCode_Description_Date(txtNappiCode.Text
                        , txtNappiDescription.Text
                        , dateNappiDate.Value);

                    if (oDtNappiSearchResult.Rows.Count > 0)
                    {
                        if (oDtNappiSearchResult.Rows.Count == 1)
                        {
                            oCM.usp_Case_NappiCodes_Insert(
                            -1
                            , CaseID
                            , Convert.ToInt32(oDtNappiSearchResult.Rows[0]["NappiID"].ToString())
                            , Convert.ToDecimal(txtNappiValue.Text)
                            , Convert.ToDecimal(numericNappiQty.Value)
                            , rbNappiDispensary.Checked
                            , rbNappiWard.Checked
                            , rbNappiTheater.Checked
                            , rbNappiTTO.Checked
                            , rbNappi0201.Checked
                            , dateNappiDate.Value
                            , Program.Username);

                            datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
                        }

                        else
                        {
                            CustomMessageBoxWithGrid oMessageBox = new CustomMessageBoxWithGrid(oDtNappiSearchResult
                                , "Select Nappi"
                                , "This Nappi exists for multiple specialities.\n\rPlease select the correct one and press enter or click yes.");
                            oMessageBox.ShowDialog();
                            if (oMessageBox.DialogResult == System.Windows.Forms.DialogResult.Yes)
                            {
                                oCM.usp_Case_NappiCodes_Insert(
                            -1
                            , CaseID
                            , Convert.ToInt32(oMessageBox.SelectedRow.Cells["NappiID"].Value)
                            , Convert.ToDecimal(txtNappiValue.Text)
                            , Convert.ToDecimal(numericNappiQty.Value)
                            , rbNappiDispensary.Checked
                            , rbNappiWard.Checked
                            , rbNappiTheater.Checked
                            , rbNappiTTO.Checked
                            , rbNappi0201.Checked
                            , dateNappiDate.Value
                            , Program.Username);

                                datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
                            }
                            else
                            {
                                MessageBox.Show("Record was not added");
                            }
                        }

                    }
                    else if (oDtNappiSearchResult.Rows.Count == 0)
                    {
                        MessageBox.Show("Nappi does not exist\n\rTry searching again");
                    }

                }
                txtNappiCode.Focus();
                txtNappiCode.SelectAll();
            }
            else
            {
                MessageBox.Show("Please enter a value");
            }
        }

        private void DeleteNappi()
        {
            try
            {
                oCM.usp_Case_NappiCodes_Delete(
                    Convert.ToInt32(datagridNappiCodes.CurrentRow.Cells["CaseID_NappiID"].Value));
                datagridNappiCodes.DataSource = oCM.usp_Case_NappiCodes_Select(CaseID);
            }
            catch
            {
            }
        }

        #region Wire up Nappi Events
        private void txtNappiCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void txtNappiDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void txtNappiValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void numericNappiQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void dateNappiDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void rbNappiDispensary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void rbNappiWard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void rbNappiTheater_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void rbNappiTTO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void rbNappi0201_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddNappi();
            }
        }

        private void txtNappiCode_Enter(object sender, EventArgs e)
        {
            txtNappiCode.SelectAll();
        }

        private void txtNappiDescription_Enter(object sender, EventArgs e)
        {
            txtNappiDescription.SelectAll();
        }

        private void txtNappiValue_Enter(object sender, EventArgs e)
        {
            txtNappiValue.SelectAll();
        }

        private void picNappiAdd_Click(object sender, EventArgs e)
        {
            AddNappi();
        }

        private void picNappiDelete_Click(object sender, EventArgs e)
        {
            DeleteNappi();
        }

        private void txtNappiCode_TextChanged(object sender, EventArgs e)
        {
            numericNappiQty.Value = 1;
            txtNappiValue.Text = "0";
        }

        private void txtNappiDescription_TextChanged(object sender, EventArgs e)
        {
            //numericNappiQty.Value = 1;
            //txtNappiValue.Text = "0";
        }

        private void datagridNappiCodes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblCaseID_NappiID.Text = datagridNappiCodes.Rows[e.RowIndex].Cells["CaseID_NappiID"].Value.ToString();
                lblNappiID.Text = datagridNappiCodes.Rows[e.RowIndex].Cells["NappiID"].Value.ToString();
                txtNappiCode.Text = datagridNappiCodes.Rows[e.RowIndex].Cells["NappiCode"].Value.ToString();
                txtNappiDescription.Text = datagridNappiCodes.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                txtNappiValue.Text = datagridNappiCodes.Rows[e.RowIndex].Cells["Value"].Value.ToString();
                numericNappiQty.Value = Convert.ToDecimal(datagridNappiCodes.Rows[e.RowIndex].Cells["Quantity"].Value.ToString());
                dateNappiDate.Value = Convert.ToDateTime(datagridNappiCodes.Rows[e.RowIndex].Cells["Date"].Value.ToString());
                rbNappi0201.Checked = Convert.ToBoolean(datagridNappiCodes.Rows[e.RowIndex].Cells["0201"].Value.ToString());
                rbNappiDispensary.Checked = Convert.ToBoolean(datagridNappiCodes.Rows[e.RowIndex].Cells["Dispensary"].Value.ToString());
                rbNappiTheater.Checked = Convert.ToBoolean(datagridNappiCodes.Rows[e.RowIndex].Cells["Theater"].Value.ToString());
                rbNappiTTO.Checked = Convert.ToBoolean(datagridNappiCodes.Rows[e.RowIndex].Cells["TTO"].Value.ToString());
                rbNappiWard.Checked = Convert.ToBoolean(datagridNappiCodes.Rows[e.RowIndex].Cells["Ward"].Value.ToString());

            }
            catch { }
        }
        #endregion

        private void chkMBOD_RMA2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can only change this value on the member detail screen\n\r(Where you create/update a member)");
        }

        private void chkMBOD_RMA1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can only change this value on the member detail screen\n\r(Where you create/update a member)");
        }

        private void label77_Click(object sender, EventArgs e)//chkMBOD_RMA1_Click - Label
        {
            MessageBox.Show("You can only change this value on the member detail screen\n\r(Where you create/update a member)");
        }

        private void picCopy_Click(object sender, EventArgs e)
        {
            if (CaseID > 0)
            {


                CopyCaseDialog oDialog = new CopyCaseDialog(CaseID, true);
                oDialog.ShowDialog();

                if (oDialog.NewCaseID != -1)
                {
                    //Linked to other cases - re-purpose the area
                    DataTable oDtLinks = oCM.usp_Case_Link_Select(CaseID);
                    if (oDtLinks.Rows.Count > 0)
                    {
                        picLinkToCase.Visible = false;
                        picLinkToCase.Enabled = false;
                        lblLinkToCase.Visible = false;
                        picPrintLinkedCases.Visible = true;
                        picPrintLinkedCases.Enabled = true;

                        picUnlinkCase.Visible = true;
                        lblUnlinkCase.Visible = true;
                        picUnlinkCase.Enabled = true;
                        //datagridCases.DataSource = oDtLinks;
                    }
                    else
                    {
                        picLinkToCase.Visible = true;
                        picLinkToCase.Enabled = true;
                        lblLinkToCase.Visible = true;
                        picPrintLinkedCases.Visible = false;
                        picPrintLinkedCases.Enabled = false;

                        picUnlinkCase.Visible = false;
                        lblUnlinkCase.Visible = false;
                        picUnlinkCase.Enabled = false;
                    }

                    OpenNewCaseDialog(
                    Convert.ToInt32(oDialog.NewCaseID)
                    , txtMemberSurname.Text);
                }
            }
            else
            {
                MessageBox.Show("Save the current case first");
            }
            
        }

        //From Mycases
        private void OpenNewCaseDialog(int CaseID, string Surname)
        {
            bool IsFormOpen = false;
            foreach (Form oForm in this.MdiParent.MdiChildren)
            {
                if (oForm.Text == "Case - " + CaseID.ToString() + " " + Surname)
                {
                    IsFormOpen = true;
                    oForm.Activate();
                    break;
                }
            }
            if (!IsFormOpen)
            {
                try
                {
                    try
                    {
                        if (CaseID != -1)
                        {
                            oCM.usp_Session_User_Case_Insert(CaseID, Program.Username);

                            Case oCase = new Case(CaseID);
                            oCase.Text = "Case - " + CaseID.ToString() + " " + Surname;
                            oCase.MdiParent = this.MdiParent;
                            oCase.WindowState = FormWindowState.Normal;
                            oCase.Show();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("This case is already opened by " + oCM.usp_Session_User_Case_Select_ByCaseID(CaseID).Rows[0][0].ToString());
                    }

                }
                catch
                {
                    //Catch error when the form is forcibly closed because of another user with the same case open
                }
            }
        }

        private void datagridCases_DoubleClick(object sender, EventArgs e)
        {
            if (datagridCases.CurrentRow != null)
            {
                OpenNewCaseDialog(
                    Convert.ToInt32(datagridCases.Rows[datagridCases.CurrentRow.Index].Cells["CaseID"].Value)
                    , txtMemberSurname.Text);
            }
        }

        //ToDo: (Billing) Taken out for Tariff Change. Needs to be put back
        private void txtAccountNumber_Leave(object sender, EventArgs e)
        {
            if (Program.EnableBilling)
            {
                if (!formClosing)
                {
                    if (txtAccountNumber.Text != ""
                        && txtAccountNumber.AutoCompleteCustomSource.Contains(txtAccountNumber.Text))
                    {
                        //todo: check if we need to auto populate final invoice amount
                        //DataTable oDt = oFin.usp_Case_Billing_SelectAfterAutoComplete(Convert.ToInt32(lblCurrentPracticeID.Text), txtAccountNumber.Text);
                        //txtFinalInvoiceAmount.Text = oDt.Rows[0][1].ToString();
                    }
                    else if (txtAccountNumber.Text != "")
                    {
                        if (oDtCurrentCase != null)
                        {
                            if (oDtCurrentCase.Rows[0]["AccountNr"].ToString() != txtAccountNumber.Text)
                            {
                                DialogResult oRes = MessageBox.Show("This account number does not exist\n\r", "Account does not exist", MessageBoxButtons.YesNo);
                                //DONE: (Billing) This needs to be put back once invoices are captured in the system
                                txtAccountNumber.Text = "";
                            }
                        }
                        else
                        {
                            DialogResult oRes = MessageBox.Show("This account number does not exist\n\r", "Account does not exist", MessageBoxButtons.YesNo);
                            //DONE: (Billing) This needs to be put back once invoices are captured in the system
                            txtAccountNumber.Text = "";
                        }


                        //DONE: (Billing) This needs to be removed once invoices are captured
                        #region to be removed later
                        //{
                        //    if (oRes == System.Windows.Forms.DialogResult.Yes)
                        //    {
                        //        DataTable oDt = oFin.usp_Case_Billing_CheckDuplicates(
                        //                    -1
                        //                    , txtAccountNumber.Text
                        //                    , Convert.ToInt32(lblCurrentPracticeID.Text));

                        //        DialogResult ores = new DialogResult();
                        //        ores = System.Windows.Forms.DialogResult.Yes;
                        //        if (oDt.Rows.Count > 0)
                        //        {
                        //            CustomMessageBoxWithGrid oMsg = new CustomMessageBoxWithGrid(oDt
                        //                , "Duplicate account"
                        //                , "This account number for the same provider has already been used in the cases listed below:"
                        //                , true);

                        //            ores = oMsg.ShowDialog();
                        //        }

                        //        if (ores != System.Windows.Forms.DialogResult.Cancel)
                        //            oFin.usp_Case_Billing_Insert(-1
                        //                , Convert.ToInt32(lblCurrentPracticeID.Text)
                        //                , DateTime.Now//dateBillingAccountDate.Value
                        //                , txtAccountNumber.Text
                        //                , DateTime.Now//dateBillingRecieved.Value
                        //                , Program.Username
                        //                , ""
                        //                , ""
                        //                , "Captured from Cases screen"//txtBillingComment.Text
                        //                , Convert.ToDecimal(0.00)//Convert.ToDecimal((txtBillingAmountDue.Text == "" ? "0.00" : txtBillingAmountDue.Text))
                        //                , false//chkBillingBank.Checked
                        //                , DateTime.Now//dateBillingDatePaid.Value
                        //                , ""//txtBillingRemittance.Text
                        //                , Convert.ToDecimal(0.00)//Convert.ToDecimal((txtBillingAmountPaid.Text == "" ? "0.00" : txtBillingAmountPaid.Text))
                        //                , 1//1 = new Convert.ToInt32(comboBillingStatus.SelectedValue.ToString())
                        //                , DateTime.Now
                        //                , Program.Username);
                        //        else
                        //        {
                        //            txtAccountNumber.Text = "";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        txtAccountNumber.Text = "";
                        //    }
                        //}
                        #endregion
                    }
                    else if (txtAccountNumber.Text == "")
                    {
                        //txtFinalInvoiceAmount.Text = "0";
                        //lblFinalInvoiceAmountUpdated.Text = "";
                    }

                    if (lblCurrentPracticeID.Text != "")
                    {
                        //AutoComplete for account numbers
                        AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();
                        DataTable oDtNamesCollection = oFin.usp_Case_Billing_SelectAccountNumbers_ByServiceProviderID(Convert.ToInt32(lblCurrentPracticeID.Text));
                        string[] oResultName = new string[oDtNamesCollection.Rows.Count];
                        for (int i = 0; i < oDtNamesCollection.Rows.Count; i++)
                        {
                            oResultName[i] = oDtNamesCollection.Rows[i][0].ToString();
                        }
                        namesCollection.AddRange(oResultName);
                        txtAccountNumber.AutoCompleteCustomSource = namesCollection;
                    }
                }
            }
        }

        private void datagridCaseNotes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (datagridCaseNotes.Rows.Count > 0)
            {
                txtUpdatesCaseNumber.Text = datagridCaseNotes.Rows[0].Cells["CaseNumber"].Value.ToString();
                txtCaseNumber.Text = txtUpdatesCaseNumber.Text;
            }
        }

        private void numPenaltyPercentage_ValueChanged(object sender, EventArgs e)
        {
            if (txtFinalInvoiceAmount.Text != "")
            {
                txtFinalInvoiceWithPenalty.Text = ((Convert.ToDecimal(1.00) - (numPenaltyPercentage.Value / 100)) * Convert.ToDecimal(txtFinalInvoiceAmount.Text)).ToString();
            }
        }

        private void txtInterimHospital_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();
        }

        private void AddUpInterimAmounts()
        {
            try
            {
                decimal totalInterim = 0;
                totalInterim = Convert.ToDecimal(txtInterimAccomodation.Text)
                    + Convert.ToDecimal(txtInterimHospital.Text)
                    + Convert.ToDecimal(txtInterimDialysis.Text)
                    + Convert.ToDecimal(txtInterimPhysio.Text)
                    + Convert.ToDecimal(txtInterimRadiology.Text)
                    + Convert.ToDecimal(txtInterimSpecialist.Text)
                    + Convert.ToDecimal(txtInterimTransport.Text)
                    + Convert.ToDecimal(txtInterimScript.Text);

                txtUpdatesInterimAmount.Text = totalInterim.ToString();
                txtInterimAmount.Text = txtUpdatesInterimAmount.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Please ensure the interim amounts are only numbers");
            }
        }

        private void txtInterimRadiology_TextChanged(object sender, EventArgs e)
        {

            AddUpInterimAmounts();
        }

        private void txtInterimPathology_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();

        }

        private void txtInterimSpecialist_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();

        }

        private void txtInterimPhysio_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();

        }

        private void txtInterimTransport_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();

        }

        private void txtInterimAccomodation_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();

        }
        
        private void txtInterimScript_TextChanged(object sender, EventArgs e)
        {
            AddUpInterimAmounts();
        }





        //private void BillingFormEnter_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == '\r')
        //    {
        //        BillingSave();
        //    }
        //}

        //private void picBillingSave_Click(object sender, EventArgs e)
        //{
        //    BillingSave();
        //}

        //private void BillingSave()
        //{
        //    //access
        //    //"Billing Auditing"
        //    //"System Administrator"
        //    //"Metadata Administrator"
        //    //"Imports"
        //    //"Case Manager"
        //    if (Program._GenericPrincipal.IsInRole("Billing Auditing"))
        //    {

        //        try
        //        {
        //            if (chkBillingNewRecord.Checked
        //                || datagridBilling.CurrentRow == null)
        //            {
        //                DataTable oDt = oCM.usp_Case_Billing_CheckDuplicates(
        //                    -1
        //                    , txtBillingAccountNumber.Text
        //                    , Convert.ToInt32(lblCurrentPracticeID.Text));

        //                DialogResult ores = new DialogResult();
        //                ores = System.Windows.Forms.DialogResult.Yes;
        //                if (oDt.Rows.Count > 0)
        //                {
        //                    CustomMessageBoxWithGrid oMsg = new CustomMessageBoxWithGrid(oDt
        //                        , "Duplicate account"
        //                        , "This account number for the same provider has already been used in the cases listed below\n\rDo you want to continue?");

        //                    ores = oMsg.ShowDialog();
        //                }

        //                if (ores != System.Windows.Forms.DialogResult.Cancel)
        //                    oCM.usp_Case_Billing_Insert(CaseID
        //                        , dateBillingAccountDate.Value
        //                        , txtBillingAccountNumber.Text
        //                        , dateBillingRecieved.Value
        //                        , txtBillingRecievedName.Text
        //                        , txtBillingRecievedInitials.Text
        //                        , txtBillingRecievedSurname.Text
        //                        , txtBillingComment.Text
        //                        , Convert.ToDecimal((txtBillingAmountDue.Text == "" ? "0.00" : txtBillingAmountDue.Text))
        //                        , chkBillingBank.Checked
        //                        , dateBillingDateDue.Value
        //                        , txtBillingRemittance.Text
        //                        , Convert.ToDecimal((txtBillingAmountPaid.Text == "" ? "0.00" : txtBillingAmountPaid.Text))
        //                        , DateTime.Now
        //                        , Program.Username);
        //            }
        //            else
        //            {
        //                DataTable oDt = oCM.usp_Case_Billing_CheckDuplicates(
        //                   Convert.ToInt32(lblBillingSelectedRecord.Text)
        //                   , txtBillingAccountNumber.Text
        //                   , Convert.ToInt32(lblCurrentPracticeID.Text));

        //                DialogResult ores = new DialogResult();
        //                ores = System.Windows.Forms.DialogResult.Yes;
        //                if (oDt.Rows.Count > 0)
        //                {
        //                    CustomMessageBoxWithGrid oMsg = new CustomMessageBoxWithGrid(oDt
        //                        , "Duplicate account"
        //                        , "This account number for the same provider has already been used in the cases listed below\n\rDo you want to continue?");

        //                    ores = oMsg.ShowDialog();
        //                }

        //                if (ores != System.Windows.Forms.DialogResult.Cancel)
        //                    oCM.usp_Case_Billing_Update(Convert.ToInt32(lblBillingSelectedRecord.Text)
        //                    , CaseID
        //                     , dateBillingAccountDate.Value
        //                     , txtBillingAccountNumber.Text
        //                     , dateBillingRecieved.Value
        //                     , txtBillingRecievedName.Text
        //                     , txtBillingRecievedInitials.Text
        //                     , txtBillingRecievedSurname.Text
        //                     , txtBillingComment.Text
        //                     , Convert.ToDecimal((txtBillingAmountDue.Text == "" ? "0.00" : txtBillingAmountDue.Text))
        //                     , chkBillingBank.Checked
        //                     , dateBillingDateDue.Value
        //                     , txtBillingRemittance.Text
        //                     , Convert.ToDecimal((txtBillingAmountPaid.Text == "" ? "0.00" : txtBillingAmountPaid.Text))
        //                     , DateTime.Now
        //                     , Program.Username);
        //            }
        //            datagridBilling.DataSource = oCM.usp_Case_Billing_Select(CaseID);
        //            datagridBillingSmall.DataSource = oCM.usp_Case_Billing_Select_Summary(CaseID);
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("Billing and paid amounts need to be numeric values");
        //        }
        //    }//Access
        //}

        //private void datagridBilling_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (datagridBilling.CurrentRow.Index > -1)
        //    {
        //        lblBillingSelectedRecord.Text = datagridBilling.Rows[e.RowIndex].Cells["Case_BillingID"].Value.ToString();
        //        dateBillingAccountDate.Value = Convert.ToDateTime(datagridBilling.Rows[e.RowIndex].Cells["AccountDate"].Value.ToString());
        //        txtBillingAccountNumber.Text = datagridBilling.Rows[e.RowIndex].Cells["AccountNumber"].Value.ToString();
        //        //txtBillingRecievedName.Text = datagridBilling.Rows[e.RowIndex].Cells[""].Value.ToString();
        //        dateBillingRecieved.Value = Convert.ToDateTime(datagridBilling.Rows[e.RowIndex].Cells["Recieved"].Value.ToString());
        //        txtBillingRecievedName.Text = datagridBilling.Rows[e.RowIndex].Cells["RecievedByName"].Value.ToString();
        //        txtBillingRecievedInitials.Text = datagridBilling.Rows[e.RowIndex].Cells["RecievedByInitials"].Value.ToString();
        //        txtBillingRecievedSurname.Text = datagridBilling.Rows[e.RowIndex].Cells["RecievedBySurname"].Value.ToString();
        //        txtBillingComment.Text = datagridBilling.Rows[e.RowIndex].Cells["Comment"].Value.ToString();
        //        txtBillingAmountDue.Text = datagridBilling.Rows[e.RowIndex].Cells["AmountDue"].Value.ToString();
        //        chkBillingBank.Checked = Convert.ToBoolean(datagridBilling.Rows[e.RowIndex].Cells["Bank"].Value.ToString());
        //        dateBillingDateDue.Value = Convert.ToDateTime(datagridBilling.Rows[e.RowIndex].Cells["DateDue"].Value.ToString());
        //        txtBillingAmountPaid.Text = datagridBilling.Rows[e.RowIndex].Cells["Amount"].Value.ToString();

        //        chkBillingNewRecord.Checked = false;
        //    }
        //}

        //private void datagridBillingSmall_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        //{
        //    decimal oInvoiceAmount = 0;
        //    string oAccountNumber = "";
        //    foreach (DataGridViewRow item in datagridBillingSmall.Rows)
        //    {
        //        oInvoiceAmount += Convert.ToDecimal(item.Cells["AmountDue"].Value);
        //        oAccountNumber += (oAccountNumber == ""?"":", ") + item.Cells["AccountNumber"].Value;
        //    }

        //    txtFinalInvoiceAmount.Text = oInvoiceAmount.ToString();
        //    txtAccountNumber.Text = oAccountNumber;
        //}

        //private void picRptBillingSummaryForCase_Click(object sender, EventArgs e)
        //{
        //    rpt_BillingSummaryCase oSummary = new rpt_BillingSummaryCase(CaseID);
        //    //oSummary.CaseID = CaseID;
        //    oSummary.ShowDialog();
        //}

        //private void picRptBillingSummaryForMember_Click(object sender, EventArgs e)
        //{
        //    rpt_BillingSummaryMember oSummary = new rpt_BillingSummaryMember(Convert.ToInt32(lblMemberID.Text));
        //    //oSummary.CaseID = CaseID;
        //    oSummary.ShowDialog();
        //}

        //private void picRptBillingSummaryForProvider_Click(object sender, EventArgs e)
        //{
        //    rpt_BillingSummaryProvider oSummary = new rpt_BillingSummaryProvider(Convert.ToInt32(lblCurrentPracticeID.Text));
        //    //oSummary.CaseID = CaseID;
        //    oSummary.ShowDialog();
        //}

        //private void picBillingDelete_Click(object sender, EventArgs e)
        //{
        //    if (datagridBilling.CurrentRow.Index > -1)
        //    {
        //        oCM.usp_Case_Billing_Delete(Convert.ToInt32(lblBillingSelectedRecord.Text));
        //        datagridBilling.DataSource = oCM.usp_Case_Billing_Select(CaseID);
        //        datagridBillingSmall.DataSource = oCM.usp_Case_Billing_Select_Summary(CaseID);
        //        lblBillingSelectedRecord.Text = "";
        //        chkBillingNewRecord.Checked = true;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select a billing line to delete first.");
        //    }
        //}

    }
}

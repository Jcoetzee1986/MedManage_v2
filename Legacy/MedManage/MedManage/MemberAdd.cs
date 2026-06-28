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
    public partial class MemberAdd : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        public int MemberID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID">When a value of -1 is sent through it indicates that a new member should be created</param>
        public MemberAdd(int MemberIDIn)
        {
            InitializeComponent();

            datePassportExpiryDate.Value = new DateTime(1900, 01, 01);

            MemberID = MemberIDIn;
            lblMemberID.Text = MemberID.ToString();

            comboGender.DisplayMember = "GenderDescription";
            comboGender.ValueMember = "GenderID";
            comboGender.DataSource = oShared.usp_Gender_Select();

            comboTitle.DisplayMember = "Title";
            comboTitle.ValueMember = "TitleID";
            comboTitle.DataSource = oShared.usp_Title_Select();

            comboMarritalStatus.DisplayMember = "MarritalStatus";
            comboMarritalStatus.ValueMember = "MarritalStatusID";
            comboMarritalStatus.DataSource = oShared.usp_MarritalStatus_Select();

            comboLanguage.DisplayMember = "Language";
            comboLanguage.ValueMember = "LanguageID";
            comboLanguage.DataSource = oShared.usp_Language_Select();

            comboRace.DisplayMember = "Race";
            comboRace.ValueMember = "RaceID";
            comboRace.DataSource = oShared.usp_Race_Select();

            comboCountry.DisplayMember = "CountryName";
            comboCountry.ValueMember = "CountryID";
            comboCountry.DataSource = oShared.usp_Country_Select();

            comboEmployerCountry.DisplayMember = "CountryName";
            comboEmployerCountry.ValueMember = "CountryID";
            comboEmployerCountry.DataSource = oShared.usp_Country_Select();

            comboPeriodInCountry.DisplayMember = "PeriodInCountry";
            comboPeriodInCountry.ValueMember = "PeriodInCountryID";
            comboPeriodInCountry.DataSource = oShared.usp_PeriodInCountry_Select();

            comboMedicalAid.DisplayMember = "MedicalAidName";
            comboMedicalAid.ValueMember = "MedicalAidID";
            comboMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);


            //comboMedicalAid.SelectedValue = Program.MainClientID;
            //comboMedicalAid.Enabled = false;

            comboMemberStatus.DisplayMember = "MemberStatus";
            comboMemberStatus.ValueMember = "MemberStatusID";
            comboMemberStatus.DataSource = oShared.usp_MemberStatus_Select();

            comboSuspendReason.DisplayMember = "SuspendedReason";
            comboSuspendReason.ValueMember = "SuspendedReasonID";
            comboSuspendReason.DataSource = oShared.usp_SuspendedReason_Select();

            comboMedicalAidProduct.DisplayMember = "MedAidProductName";
            comboMedicalAidProduct.ValueMember = "MedAidProductID";
            comboMedicalAidProduct.DataSource = oShared.usp_MedicalAidProduct_Select(Program.MainClientID);

            if (MemberID != -1)
            {
                grpMemberNotes.Enabled = true;
                txtMemberNumber.Enabled = false;
                DataTable oDtMember = oShared.usp_Member_Select(MemberID, DateTime.Now);

                txtMemberNumber.Text = oDtMember.Rows[0]["MemberNumber"].ToString();
                comboTitle.SelectedValue = oDtMember.Rows[0]["TitleID"].ToString();
                txtSurname.Text = oDtMember.Rows[0]["Surname"].ToString();
                txtInitials.Text = oDtMember.Rows[0]["Initials"].ToString();
                txtName.Text = oDtMember.Rows[0]["Name"].ToString();
                txtPassportNumber.Text = oDtMember.Rows[0]["PassportNumber"].ToString();
                datePassportExpiryDate.Value = Convert.ToDateTime(oDtMember.Rows[0]["PassportExpiryDate"].ToString());
                comboPeriodInCountry.SelectedValue = oDtMember.Rows[0]["PeriodInCountryID"].ToString();
                dateDateOfBirth.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateOfBirth"].ToString());
                comboGender.SelectedValue = oDtMember.Rows[0]["GenderID"].ToString();
                comboMedicalAid.SelectedValue = oDtMember.Rows[0]["MedicalAidID"].ToString();
                dateBenefitDate.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateOfBenefit"].ToString());
                dateJoinedDate.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateJoined"].ToString());

                chkSuspended.Checked = Convert.ToBoolean(oDtMember.Rows[0]["Suspended"].ToString());
                dateSuspendedDate.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateSuspended"].ToString());
                comboSuspendReason.SelectedValue = oDtMember.Rows[0]["SuspendedReasonID"].ToString();
                if (!chkSuspended.Checked)
                {
                    dateSuspendedDate.Enabled = false;
                }

                chkMedAidExhausted.Checked = Convert.ToBoolean(oDtMember.Rows[0]["MedicalAidExhausted"].ToString());
                dateExhaustedDate.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateMedicalaidExhausted"].ToString());
                if (!chkMedAidExhausted.Checked)
                    dateExhaustedDate.Enabled = false;

                chkDeceased.Checked = Convert.ToBoolean(oDtMember.Rows[0]["Deceased"].ToString());
                try
                {
                    dateDeceased.Value = Convert.ToDateTime(oDtMember.Rows[0]["DeceasedDate"].ToString());
                }
                catch
                {
                    //Expected when deceased date is not populated
                }
                if (!chkDeceased.Checked)
                    dateDeceased.Enabled = false;

                chkWaitingPeriod.Checked = Convert.ToBoolean(oDtMember.Rows[0]["WaitingPeriodApplicable"].ToString());
                comboMarritalStatus.SelectedValue = oDtMember.Rows[0]["MarritalStatusID"].ToString();
                comboEmployerCountry.SelectedValue = oDtMember.Rows[0]["EmployerCountryID"].ToString();
                txtEmployerAddress.Text = oDtMember.Rows[0]["EmployerAddress"].ToString();
                txtEmployerAddressCode.Text = oDtMember.Rows[0]["EmployerAddressCode"].ToString();
                txtEmployerPhoneNumber.Text = oDtMember.Rows[0]["EmployerPhoneNumber"].ToString();
                chkPensioner.Checked = Convert.ToBoolean(oDtMember.Rows[0]["Pensioner"].ToString());
                comboMemberStatus.SelectedValue = oDtMember.Rows[0]["MemberStatusID"].ToString();
                comboCountry.SelectedValue = oDtMember.Rows[0]["MemberCountryID"].ToString();
                txtAddress1.Text = oDtMember.Rows[0]["MemberAddress1"].ToString();
                txtAddress2.Text = oDtMember.Rows[0]["MemberAddress2"].ToString();
                txtAddress3.Text = oDtMember.Rows[0]["MemberAddress3"].ToString();
                txtAddressCode.Text = oDtMember.Rows[0]["MemberAddressCode"].ToString();
                txtPhoneNumber.Text = oDtMember.Rows[0]["MemberPhoneNumber"].ToString();
                txtCellNumber.Text = oDtMember.Rows[0]["MemberCellNumber"].ToString();
                txtNextOfKin.Text = oDtMember.Rows[0]["NextOfKinName"].ToString();
                txtRelationship.Text = oDtMember.Rows[0]["NextOfKinRelationship"].ToString();
                txtContactNumber.Text = oDtMember.Rows[0]["NextOfKinContactNumber"].ToString();
                comboLanguage.SelectedValue = oDtMember.Rows[0]["MemberLanguageID"].ToString();
                comboRace.SelectedValue = oDtMember.Rows[0]["MemberRaceID"].ToString();
                txtDependents.Text = oDtMember.Rows[0]["MemberDependents"].ToString();

                chkMemberReInstated.Checked = Convert.ToBoolean(oDtMember.Rows[0]["FundReinstated"].ToString());
                dateReInstated.Value = Convert.ToDateTime(oDtMember.Rows[0]["FundReinstatedDate"].ToString());
                if (!chkMemberReInstated.Checked)
                    dateReInstated.Enabled = false;

                try
                {
                    comboMedicalAidProduct.SelectedValue = oDtMember.Rows[0]["MedAidProductID"].ToString();
                }
                catch (Exception)
                {
                    if (((DataTable)comboMedicalAidProduct.DataSource).Rows.Count > 1)
                    {
                        comboMedicalAidProduct.SelectedValue = 0;
                        MessageBox.Show("Please select a valid Medical Aid Product");
                    }
                    else
                    {
                        comboMedicalAidProduct.SelectedIndex = 0;
                    }
                    datemedicalAidProductStart.Value = dateJoinedDate.Value;
                }
                
                chkMBOD_RMA1.Checked = Convert.ToBoolean(oDtMember.Rows[0]["MBOD_RMA"].ToString());


                datagridMemberComments.DataSource = oShared.usp_MemberNote_Select(MemberID);
                datagridMedicalAidProduct.DataSource = oShared.usp_Member_MedicalAidProduct_SelectByMemberID(Convert.ToInt32(lblMemberID.Text));
            }
            else
            {
                grpMemberNotes.Enabled = false;
                dateExhaustedDate.Enabled = false;
                dateSuspendedDate.Enabled = false;
                dateReInstated.Enabled = false;
                if (((DataTable)comboMedicalAidProduct.DataSource).Rows.Count > 1)
                {
                    comboMedicalAidProduct.SelectedValue = 0;
                    //MessageBox.Show("Please select a valid Medical Aid Product");
                }
                else
                {
                    comboMedicalAidProduct.SelectedIndex = 0;
                }
            }
        }

        public MemberAdd(string surname, string name, string memberNumber, string passportNumber, string idNumber, DateTime dateOfBirth)
        {
            InitializeComponent();

            datePassportExpiryDate.Value = new DateTime(1900, 01, 01);

            MemberID = -1;
            lblMemberID.Text = MemberID.ToString();

            txtSurname.Text = surname;
            txtName.Text = name;
            txtMemberNumber.Text = memberNumber;
            txtPassportNumber.Text = passportNumber;
            txtIDNumber.Text = idNumber;
            dateDateOfBirth.Value = dateOfBirth;

            comboGender.DisplayMember = "GenderDescription";
            comboGender.ValueMember = "GenderID";
            comboGender.DataSource = oShared.usp_Gender_Select();

            comboTitle.DisplayMember = "Title";
            comboTitle.ValueMember = "TitleID";
            comboTitle.DataSource = oShared.usp_Title_Select();

            comboMarritalStatus.DisplayMember = "MarritalStatus";
            comboMarritalStatus.ValueMember = "MarritalStatusID";
            comboMarritalStatus.DataSource = oShared.usp_MarritalStatus_Select();

            comboLanguage.DisplayMember = "Language";
            comboLanguage.ValueMember = "LanguageID";
            comboLanguage.DataSource = oShared.usp_Language_Select();

            comboRace.DisplayMember = "Race";
            comboRace.ValueMember = "RaceID";
            comboRace.DataSource = oShared.usp_Race_Select();

            comboCountry.DisplayMember = "CountryName";
            comboCountry.ValueMember = "CountryID";
            comboCountry.DataSource = oShared.usp_Country_Select();

            comboEmployerCountry.DisplayMember = "CountryName";
            comboEmployerCountry.ValueMember = "CountryID";
            comboEmployerCountry.DataSource = oShared.usp_Country_Select();

            comboPeriodInCountry.DisplayMember = "PeriodInCountry";
            comboPeriodInCountry.ValueMember = "PeriodInCountryID";
            comboPeriodInCountry.DataSource = oShared.usp_PeriodInCountry_Select();

            comboMedicalAid.DisplayMember = "MedicalAidName";
            comboMedicalAid.ValueMember = "MedicalAidID";
            comboMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);
            //comboMedicalAid.SelectedValue = Program.MainClientID;
            //comboMedicalAid.Enabled = false;

            comboMemberStatus.DisplayMember = "MemberStatus";
            comboMemberStatus.ValueMember = "MemberStatusID";
            comboMemberStatus.DataSource = oShared.usp_MemberStatus_Select();

            comboMedicalAidProduct.DisplayMember = "MedAidProductName";
            comboMedicalAidProduct.ValueMember = "MedAidProductID";
            comboMedicalAidProduct.DataSource = oShared.usp_MedicalAidProduct_Select(Program.MainClientID);
            


            grpMemberNotes.Enabled = false;
            dateExhaustedDate.Enabled = false;
            dateSuspendedDate.Enabled = false;
            dateReInstated.Enabled = false;
            if (((DataTable)comboMedicalAidProduct.DataSource).Rows.Count > 1)
            {
                comboMedicalAidProduct.SelectedValue = 0;
                //MessageBox.Show("Please select a valid Medical Aid Product");
            }
            else
            {
                comboMedicalAidProduct.SelectedIndex = 0;
            }
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void picSaveMember_Click(object sender, EventArgs e)
        {
            if (!(txtSurname.Text != ""
                && txtName.Text != ""
                && txtMemberNumber.Text != ""
                && comboMedicalAidProduct.SelectedValue != null))
            {
                MessageBox.Show("These fields are required:\n\rName\n\rSurname\n\rMember Number\n\rMedical Aid Product");
            }
            else if (lblMemberID.Text == "-1")
            {
                if (!oShared.usp_Member_Select_ExistingMemberNumber(txtMemberNumber.Text))
                {
                    DataTable oDt = oShared.usp_Member_Insert(txtMemberNumber.Text
                        , Convert.ToInt32(comboTitle.SelectedValue)
                        , txtSurname.Text
                        , txtInitials.Text
                        , txtName.Text
                        , txtIDNumber.Text
                        , txtPassportNumber.Text
                        , datePassportExpiryDate.Value
                        , Convert.ToInt32(comboPeriodInCountry.SelectedValue)
                        , dateDateOfBirth.Value
                        , Convert.ToInt32(comboGender.SelectedValue)
                        , true
                        , Convert.ToInt32(comboMedicalAid.SelectedValue)
                        , dateBenefitDate.Value
                        , dateJoinedDate.Value
                        , chkSuspended.Checked
                        , dateSuspendedDate.Value
                        , Convert.ToInt32(comboSuspendReason.SelectedValue)
                        , chkMedAidExhausted.Checked
                        , dateExhaustedDate.Value
                        , chkWaitingPeriod.Checked
                        , Convert.ToInt32(comboMarritalStatus.SelectedValue)
                        , Convert.ToInt32(comboEmployerCountry.SelectedValue)
                        , txtEmployerAddress.Text
                        , txtEmployerAddressCode.Text
                        , txtEmployerPhoneNumber.Text
                        , chkPensioner.Checked
                        , Convert.ToInt32(comboMemberStatus.SelectedValue)
                        , Convert.ToInt32(comboCountry.SelectedValue)
                        , txtAddress1.Text
                        , txtAddress2.Text
                        , txtAddress3.Text
                        , txtAddressCode.Text
                        , txtPhoneNumber.Text
                        , txtCellNumber.Text
                        , txtNextOfKin.Text
                        , txtRelationship.Text
                        , txtContactNumber.Text
                        , Convert.ToInt32(comboLanguage.SelectedValue)
                        , Convert.ToInt32(comboRace.SelectedValue)
                        , txtDependents.Text
                        , chkMemberReInstated.Checked
                        , dateReInstated.Value
                        , chkDeceased.Checked
                        , dateDeceased.Value
                        , Program.Username
                        , Convert.ToInt32(comboMedicalAidProduct.SelectedValue)
                        , chkMBOD_RMA1.Checked
                        , datemedicalAidProductStart.Value);
                    MemberID = Convert.ToInt32(oDt.Rows[0][0].ToString());

                    lblMemberID.Text = MemberID.ToString();
                    grpMemberNotes.Enabled = true;
                    datagridMemberComments.DataSource = oShared.usp_MemberNote_Select(MemberID);
                    datagridMedicalAidProduct.DataSource = oShared.usp_Member_MedicalAidProduct_SelectByMemberID(Convert.ToInt32(lblMemberID.Text));
                    DialogResult oResult = MessageBox.Show("Member info has been saved\n\r\n\rDo you want to add member notes?", "Success", MessageBoxButtons.YesNo);
                    if (oResult != System.Windows.Forms.DialogResult.Yes)
                    {
                        Close();
                    }

                }
                else
                {
                    MessageBox.Show("This member number already exist.");
                }
            }
            else
            {
                oShared.usp_Member_Update(
                    Convert.ToInt32(lblMemberID.Text)
                   , txtMemberNumber.Text
                   , Convert.ToInt32(comboTitle.SelectedValue)
                   , txtSurname.Text
                   , txtInitials.Text
                   , txtName.Text
                   , txtIDNumber.Text
                   , txtPassportNumber.Text
                   , datePassportExpiryDate.Value
                   , Convert.ToInt32(comboPeriodInCountry.SelectedValue)
                   , dateDateOfBirth.Value
                   , Convert.ToInt32(comboGender.SelectedValue)
                   , true
                   , Convert.ToInt32(comboMedicalAid.SelectedValue)
                   , dateBenefitDate.Value
                   , dateJoinedDate.Value
                   , chkSuspended.Checked
                   , dateSuspendedDate.Value
                    , Convert.ToInt32(comboSuspendReason.SelectedValue)
                    , chkMedAidExhausted.Checked
                    , dateExhaustedDate.Value
                    , chkWaitingPeriod.Checked
                    , Convert.ToInt32(comboMarritalStatus.SelectedValue)
                    , Convert.ToInt32(comboEmployerCountry.SelectedValue)
                    , txtEmployerAddress.Text
                    , txtEmployerAddressCode.Text
                    , txtEmployerPhoneNumber.Text
                    , chkPensioner.Checked
                    , Convert.ToInt32(comboMemberStatus.SelectedValue)
                    , Convert.ToInt32(comboCountry.SelectedValue)
                    , txtAddress1.Text
                    , txtAddress2.Text
                    , txtAddress3.Text
                    , txtAddressCode.Text
                    , txtPhoneNumber.Text
                    , txtCellNumber.Text
                    , txtNextOfKin.Text
                    , txtRelationship.Text
                    , txtContactNumber.Text
                    , Convert.ToInt32(comboLanguage.SelectedValue)
                    , Convert.ToInt32(comboRace.SelectedValue)
                    , txtDependents.Text
                    , chkMemberReInstated.Checked
                    , dateReInstated.Value
                    , chkDeceased.Checked
                    , dateDeceased.Value
                    , Program.Username
                    , Convert.ToInt32(comboMedicalAidProduct.SelectedValue)
                    , chkMBOD_RMA1.Checked
                    , datemedicalAidProductStart.Value);

                DialogResult oResult = MessageBox.Show("Member info has been saved\n\r\n\rDo you want to add member notes?", "Success", MessageBoxButtons.YesNo);
                if (oResult != System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                }
            }

            datagridMedicalAidProduct.DataSource = oShared.usp_Member_MedicalAidProduct_SelectByMemberID(Convert.ToInt32(lblMemberID.Text));

            
        }

        private void picMemberCommentsAdd_Click(object sender, EventArgs e)
        {
            if (txtMemberComments.Text != "")
            {
                oShared.usp_MemberNote_Insert(txtMemberComments.Text, MemberID, Program.Username);
                datagridMemberComments.DataSource = oShared.usp_MemberNote_Select(MemberID);
            }
        }

        private void picMemberCommentsDelete_Click(object sender, EventArgs e)
        {
            if (datagridMemberComments.SelectedRows != null)
            {
                DialogResult oResult = MessageBox.Show("Are you sure you want to delete this note?", "Delete Note", MessageBoxButtons.YesNo);
                if (oResult == System.Windows.Forms.DialogResult.Yes)
                {
                    oShared.usp_MemberNote_Delete(Convert.ToInt32(datagridMemberComments.Rows[datagridMemberComments.CurrentRow.Index].Cells["MemberNoteID"].Value.ToString()));
                    datagridMemberComments.DataSource = oShared.usp_MemberNote_Select(MemberID);
                }
            }
        }

        private void chkSuspended_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSuspended.Checked)
            {
                dateSuspendedDate.Enabled = true;
                comboSuspendReason.Enabled = true;
            }
            else
            {
                dateSuspendedDate.Enabled = false;
                comboSuspendReason.Enabled = false;
            }
        }

        private void chkMedAidExhausted_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMedAidExhausted.Checked)
                dateExhaustedDate.Enabled = true;
            else dateExhaustedDate.Enabled = false;
            
        }

        private void chkMemberReInstated_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMemberReInstated.Checked)
                dateReInstated.Enabled = true;
            else dateReInstated.Enabled = false;
            
        }

        private void chkDeceased_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeceased.Checked)
                dateDeceased.Enabled = true;
            else dateDeceased.Enabled = false;
        }
    }
}

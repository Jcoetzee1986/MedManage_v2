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
    public partial class BookingAdd : Form
    {
        int BookingID;
        SharedObjects oShared = new SharedObjects(Program.oDb);

        /// <summary>
        /// Populate the bookings form
        /// It needs one of the following parameters.
        /// The ones for which there is no valu must be set to -1
        /// </summary>
        /// <param name="bookingID"></param>
        /// <param name="CaseID"></param>
        /// <param name="MemberID"></param>
        public BookingAdd(int bookingID,int CaseID, int memberID)
        {
            InitializeComponent();

            BookingID = bookingID;

            //comboCurrentPracticeSpeciality.DataSource = oShared.usp_Speciality_Select();
            //comboCurrentPracticeSpeciality.DisplayMember = "Speciality";
            //comboCurrentPracticeSpeciality.ValueMember = "SpecialityID";

            #region Bind Data
            if (BookingID != -1)
            {
                DataTable oDt = oShared.usp_Bookings_Select_ByBookingID(BookingID);
                lblBookingID.Text = oDt.Rows[0]["BookingID"].ToString();
                lblCaseID.Text = oDt.Rows[0]["CaseID"].ToString();
                txtAuthNumber.Text = oDt.Rows[0]["AuthNumber"].ToString();
                txtComments.Text = oDt.Rows[0]["Comments"].ToString();
                txtCurrentPractice.Text = oDt.Rows[0]["CurrServiceProviderName"].ToString();
                txtHospital.Text = oDt.Rows[0]["Hospital"].ToString();
                txtMemberName.Text = oDt.Rows[0]["Name"].ToString();
                txtMemberNumber.Text = oDt.Rows[0]["MemberNumber"].ToString();
                txtMemberSurname.Text = oDt.Rows[0]["Surname"].ToString();
                lblMemberID.Text = oDt.Rows[0]["MemberID"].ToString();
                txtReferPracticeName.Text = oDt.Rows[0]["ReferServiceProviderName"].ToString();
                txtTISCH.Text = oDt.Rows[0]["TISCH"].ToString();
                dateAppointmentDate.Value = Convert.ToDateTime(oDt.Rows[0]["AppointmentDate"].ToString());
                dateTravelDate.Value = Convert.ToDateTime(oDt.Rows[0]["TravelDate"].ToString());
                timeTravelTime.Value = Convert.ToDateTime(oDt.Rows[0]["TravelTime"].ToString());
                chkAdmission.Checked = Convert.ToBoolean(oDt.Rows[0]["Admission"].ToString());
                chkArrived.Checked = Convert.ToBoolean(oDt.Rows[0]["Arrived"].ToString());
                chkConsultation.Checked = Convert.ToBoolean(oDt.Rows[0]["Consultation"].ToString());
            }
            else if (CaseID != -1)
            {
                DataTable oDt = oShared.usp_Bookings_Select_ByCaseID(CaseID);
                lblCaseID.Text = oDt.Rows[0]["CaseID"].ToString();
                txtAuthNumber.Text = oDt.Rows[0]["AuthNumber"].ToString();
                txtCurrentPractice.Text = oDt.Rows[0]["CurrServiceProviderName"].ToString();
                lblMemberID.Text = oDt.Rows[0]["MemberID"].ToString();
                txtMemberName.Text = oDt.Rows[0]["Name"].ToString();
                txtMemberNumber.Text = oDt.Rows[0]["MemberNumber"].ToString();
                txtMemberSurname.Text = oDt.Rows[0]["Surname"].ToString();
                txtReferPracticeName.Text = oDt.Rows[0]["ReferServiceProviderName"].ToString();
            }
            else if (memberID != -1)
            {
                DataTable oDt = oShared.usp_Member_Select(memberID,DateTime.Now);
                //lblCaseID.Text = oDt.Rows[0]["CaseID"].ToString();
                //txtAuthNumber.Text = oDt.Rows[0]["AuthNumber"].ToString();
                //txtCurrentPractice.Text = oDt.Rows[0]["CurrServiceProviderName"].ToString();
                txtMemberName.Text = oDt.Rows[0]["Name"].ToString();
                txtMemberNumber.Text = oDt.Rows[0]["MemberNumber"].ToString();
                txtMemberSurname.Text = oDt.Rows[0]["Surname"].ToString();
                lblMemberID.Text = oDt.Rows[0]["MemberID"].ToString();
                //txtReferPracticeName.Text = oDt.Rows[0]["ReferServiceProviderName"].ToString();
            }
            grdAllBookings.DataSource = oShared.usp_Bookings_Select_ALL_ByMemberNumber(txtMemberNumber.Text);
            #endregion
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            if (lblMemberID.Text != "-1")
            {
                BookingID = Convert.ToInt32(oShared.usp_Bookings_Insert(
                     BookingID
                     , dateTravelDate.Value
                     , timeTravelTime.Value
                     , dateAppointmentDate.Value
                     , Convert.ToInt32(lblReferringPracticeID.Text)
                     , Convert.ToInt32(lblMemberID.Text)
                     , Convert.ToInt32(lblCaseID.Text)
                     , ""
                     , chkConsultation.Checked
                     , chkAdmission.Checked
                     , Convert.ToInt32(lblCurrentPracticeID.Text)
                     , txtHospital.Text
                     , chkArrived.Checked
                     , txtTISCH.Text
                     , txtComments.Text).Rows[0][0]);
                lblBookingID.Text = BookingID.ToString();
                MessageBox.Show("Saved Successfully");
            }
            else
            {
                MessageBox.Show("Not Saved - Please select a member first");
            }
        }

        private void picMemberLookup_Click(object sender, EventArgs e)
        {
            MemberLookup oMemberLookup = new MemberLookup(true, txtMemberSurname.Text, txtMemberName.Text, "", "");
            oMemberLookup.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //oMemberLookup.MdiParent = this.MdiParent;

            oMemberLookup.ShowDialog();
            switch (oMemberLookup.oReturn)
            {
                case "None":
                    break;
                case "Member":
                    {
                        DataTable oDtMember = oShared.usp_Member_Select(oMemberLookup.MemberID,DateTime.Now);
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
                        DataTable oDtMember = oShared.usp_Member_Select(oMemberAdd.MemberID, DateTime.Now);
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
                //gbMember.Text += " - " + oDtMember.Rows[0]["MemberNumber"].ToString();
                //comboTitle.SelectedValue = oDtMember.Rows[0]["TitleID"].ToString();
                txtMemberSurname.Text = oDtMember.Rows[0]["Surname"].ToString();
                //txtMemberInitials.Text = oDtMember.Rows[0]["Initials"].ToString();
                txtMemberName.Text = oDtMember.Rows[0]["Name"].ToString();
                //txtIdNumber.Text = oDtMember.Rows[0]["IDNumber"].ToString();
                //txtPassportNumber.Text = oDtMember.Rows[0]["PassportNumber"].ToString();
                //dateMemberDateOfBirth.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateOfBirth"].ToString());
                //comboMemberMedicalAid.SelectedValue = oDtMember.Rows[0]["MedicalAidID"].ToString();
                //dateMemberDateJoined.Value = Convert.ToDateTime(oDtMember.Rows[0]["DateJoined"].ToString());
                //chkMemberIsPensioner.Checked = Convert.ToBoolean(oDtMember.Rows[0]["Pensioner"].ToString());
                //comboMemberStatus.SelectedValue = oDtMember.Rows[0]["MemberStatusID"].ToString();
                lblMemberID.Text = oDtMember.Rows[0]["MemberID"].ToString();
                //comboMedicalAidProduct.SelectedValue = oDtMember.Rows[0]["MedAidProductID"].ToString();
                txtMemberNumber.Text = oDtMember.Rows[0]["MemberNumber"].ToString();
                
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

        private void PicReferringPracticeLookup_Click(object sender, EventArgs e)
        {
            ServiceProviderLookup oSPLookup = new ServiceProviderLookup(true, txtReferPracticeName.Text, "", "");
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
                            txtReferPracticeName.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                            lblReferringPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();

                        }

                        break;
                    }
                case "ServiceProvider":
                    {
                        DataTable oDt = oShared.usp_ServiceProvider_Select(oSPLookup.ServiceProviderID);
                        txtReferPracticeName.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                        lblReferringPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();
                        break;
                    }
                default: break;
            }
        }

        private void PicCurrentPracticeLookup_Click(object sender, EventArgs e)
        {
            ServiceProviderLookup oSPLookup = new ServiceProviderLookup(true, txtCurrentPractice.Text, "", "");
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

                        txtCurrentPractice.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                        lblCurrentPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();

                        break;
                    }
                case "ServiceProvider":
                    {
                        DataTable oDt = oShared.usp_ServiceProvider_Select(oSPLookup.ServiceProviderID);

                        txtCurrentPractice.Text = oDt.Rows[0]["PracticeNr"].ToString() + " - " + oDt.Rows[0]["PracticeName"].ToString();
                        lblCurrentPracticeID.Text = oDt.Rows[0]["ServiceProviderID"].ToString();
                        
                        break;
                    }
                default: break;
            }
        }
    }
}

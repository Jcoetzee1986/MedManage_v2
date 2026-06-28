using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class CopyCaseDialog : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        private int CaseID;
        public int NewCaseID { get; set; }
        private DateTime admissionDate;

        public CopyCaseDialog(int caseID,bool fromParentCase)
        {
            InitializeComponent();
            CaseID = caseID;
            NewCaseID = -1;

            admissionDate = DateTime.Parse(oCM.usp_Cases_Select(caseID).Rows[0]["AdmissionDate"].ToString());
            dateAdmissionDate.Value = admissionDate;

            CustomMessageBoxDateSelector dateSelect = new CustomMessageBoxDateSelector("Please select the admission date for this case:"
                ,"Admission Date"
                ,admissionDate);
            dateSelect.ShowDialog();

            dateAdmissionDate.Value = dateSelect.GetDate();

            if (fromParentCase)
            {
                chkCopyCpt.Checked = true;
                chkCopyDaysInCare.Checked = true;
                chkCopyICD10.Checked = true;
                chkCopyTariff.Checked = true;
                chkLinkToParentCase.Checked = true;
                chkUseSameAuthNumber.Checked = false;

                chkCopyCpt.Enabled = false;
                //chkCopyDaysInCare.Enabled = false;
                chkCopyICD10.Enabled = false;
                chkCopyTariff.Enabled = false;
                chkLinkToParentCase.Enabled = false;
                chkUseSameAuthNumber.Enabled = false;
            }
        }

        private void chkUseSameAuthNumber_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseSameAuthNumber.Checked)
            {
                chkLinkToParentCase.Enabled = false;
            }
            else chkLinkToParentCase.Enabled = true;
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            NewCaseID = Convert.ToInt32(
                oCM.usp_Case_Copy(CaseID
                , chkUseSameAuthNumber.Checked
                , chkLinkToParentCase.Checked
                , chkCopyICD10.Checked
                , chkCopyCpt.Checked
                , chkCopyTariff.Checked
                , chkCopyDaysInCare.Checked
                , int.Parse((admissionDate.Date - dateAdmissionDate.Value.Date).TotalDays.ToString()) //(EndDate - StartDate).TotalDays
                , Program.Username
                ).Rows[0][0].ToString());

            Close();

        }
    }
}

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
    public partial class MyCases : Form
    {
        MedManageLib.CaseManagement oCM = new MedManageLib.CaseManagement(Program.oDb);
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public MyCases()
        {
            InitializeComponent();
            try
            {
                grdSearchResults.DataSource = oCM.usp_Cases_Select_Last30Cases(Program.Username,Program.MainClientID);
                comboMedicalFunder.DisplayMember = "MedicalAidName";
                comboMedicalFunder.ValueMember = "MedicalAidID";
                comboMedicalFunder.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);
                
                comboCaseType.DisplayMember = "CaseType";
                comboCaseType.ValueMember = "CaseTypeID";
                comboCaseType.DataSource = oCM.usp_CaseType_Select_ForFilters(); 
                
                //comboMedicalFunder.SelectedValue = Program.MainClientID;
                
                object[] oStatus = new object[2];
                oStatus[0] = "-1";
                oStatus[1] = "All";
                comboCaseStatus.DisplayMember = "CaseStatus";
                comboCaseStatus.ValueMember = "CaseStatusID";
                DataTable oDtStatus = oCM.usp_CaseStatus_Select();
                oDtStatus.Rows.Add(oStatus);
                comboCaseStatus.DataSource = oDtStatus;
                comboCaseStatus.SelectedValue = "-1";

                //access
                //"Billing Auditing"
                //"System Administrator"
                //"Metadata Administrator"
                //"Imports"
                //"Case Manager"
                if (Program._GenericPrincipal.IsInRole("Case Manager"))
                {
                    picCopy.Visible = true;
                    tsbtnNewCase.Visible = true;
                }
                else
                {
                    picCopy.Visible = false;
                    tsbtnNewCase.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void tsbtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (dateMemberDOB.Text != "")
                //    oDate = Program.ConvertIntToDate(dateMemberDOB.Text);
                //else oDate = Program.ConvertIntToDate(19000101);
                grdSearchResults.DataSource = oCM.usp_Cases_Select_Filters(
                    txtCaseNumber.Text
                    , txtMemberNumber.Text
                    , txtMemberName.Text
                    , txtMemberSurname.Text
                    , Program.ConvertIntToDate(19000101) //(chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
                    , (chkDateCreatedSearch.Checked ? dateCaseCreated.Value : Program.ConvertIntToDate(19000101))
                    , (chkDateCreatedSearch.Checked ? dateCaseCreatedEnd.Value : Program.ConvertIntToDate(20990101))
                    , txtPracticeName.Text
                    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
                    , ""//txtPrimaryIcd.Text
                    , ""//txtPrimaryCpt.Text
                    , Convert.ToInt32(comboCaseStatus.SelectedValue)
                    , Program.MainClientID
                    , Convert.ToInt32(comboCaseType.SelectedValue));
            }
            catch (Exception err)
            {
                MessageBox.Show("Error retrieving data. Check your search criteria\r\n" + err.Message.ToString(), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void tsbtnNewCase_Click(object sender, EventArgs e)
        {
            OpenNewCaseDialog();
        }

        private void grdSearchResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenNewCaseDialog(
                Convert.ToInt32(grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["CaseID"].Value)
                    , grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["Surname"].Value.ToString());
        }

        private void grdSearchResults_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                OpenNewCaseDialog(
                    Convert.ToInt32(grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["CaseID"].Value)
                    ,grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["Surname"].Value.ToString());
            }
        }
        private void OpenNewCaseDialog()
        {
            Case oCase = new Case(-1);
            oCase.MdiParent = this.MdiParent;
            oCase.WindowState = FormWindowState.Normal;
            oCase.Text = "New Case - " + Convert.ToString(DateTime.Now.TimeOfDay).Substring(0,5);
            oCase.Show();
        }
        private void OpenNewCaseDialog(int CaseID, string Surname)
        {
            bool IsFormOpen = false;
            foreach (Form oForm in this.MdiParent.MdiChildren)
            {
                if (oForm.Text =="Case - " + CaseID.ToString() + " " + Surname)
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

        private void picOpenCase_Click(object sender, EventArgs e)
        {
            if (grdSearchResults.CurrentRow != null)
                OpenNewCaseDialog(
                        Convert.ToInt32(grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["CaseID"].Value)
                        , grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["Surname"].Value.ToString());
            else MessageBox.Show("Please select a case first");
        }

        private void picExport_Click(object sender, EventArgs e)
        {
            rpt_MyCasesExport oReport = new rpt_MyCasesExport(txtCaseNumber.Text
                    , txtMemberNumber.Text
                    , txtMemberName.Text
                    , txtMemberSurname.Text
                    , Program.ConvertIntToDate(19000101)//(chkDOBSearch.Checked ? dateMemberDOB.Value : Program.ConvertIntToDate(19000101))
                    , (chkDateCreatedSearch.Checked ? dateCaseCreated.Value : Program.ConvertIntToDate(19000101))
                    , (chkDateCreatedSearch.Checked ? dateCaseCreatedEnd.Value : Program.ConvertIntToDate(20990101))
                    , txtPracticeName.Text
                    , (chkMedicalFunderSearch.Checked ? Convert.ToInt32(comboMedicalFunder.SelectedValue) : -1)
                    , ""//txtPrimaryIcd.Text
                    , ""//txtPrimaryCpt.Text
                    , Convert.ToInt32(comboCaseStatus.SelectedValue)
                    ,Convert.ToInt32(comboCaseType.SelectedValue));
            oReport.Show();
        }

        private void picCopy_Click(object sender, EventArgs e)
        {
            if (grdSearchResults.CurrentRow != null)
            {
                CopyCaseDialog oDialog = new CopyCaseDialog(Convert.ToInt32(grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["CaseID"].Value),false);
                oDialog.ShowDialog();
                if (oDialog.NewCaseID != -1)
                {
                    OpenNewCaseDialog(
                    Convert.ToInt32(oDialog.NewCaseID)
                    , grdSearchResults.Rows[grdSearchResults.CurrentRow.Index].Cells["Surname"].Value.ToString());
                }
            }
            else MessageBox.Show("Please select a case first");
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

    }
}

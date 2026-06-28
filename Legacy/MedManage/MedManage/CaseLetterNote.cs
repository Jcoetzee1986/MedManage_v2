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
    public partial class CaseLetterNote : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        int CaseID;

        public CaseLetterNote(int caseID)
        {
            InitializeComponent();
            CaseID = caseID;
            DataTable oDt = oCM.usp_CaseLetterNote_Select_ByCaseID(CaseID);
            if (oDt.Rows.Count > 0)
            {
                txtNote.Text = oDt.Rows[0]["Note"].ToString();
            }
        }

        private void picCaseNoteAdd_Click(object sender, EventArgs e)
        {
            oCM.usp_CaseLetterNote_Insert(CaseID, txtNote.Text,chkIncludeDischarge.Checked,chkDownReferral.Checked);
            this.Close();
        }
    }
}

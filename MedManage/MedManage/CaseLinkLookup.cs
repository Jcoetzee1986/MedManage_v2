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
    public partial class CaseLinkLookup : Form
    {
        public string AuthNumber { get; set; }
        public int LinkCaseID { get; set; }

        CaseManagement oCM = new CaseManagement(Program.oDb);

        public CaseLinkLookup(int MemberID,int CaseID)
        {
            InitializeComponent();
            AuthNumber = "";
            grdCaseLinkLookup.DataSource = oCM.usp_Cases_Select_ByMemberID_ExclCaseID(MemberID,CaseID);
        }

        private void grdCaseLinkLookup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(grdCaseLinkLookup.Rows[e.RowIndex].Cells["ChildCase"].Value))
                MessageBox.Show("This case is invalid as a 'parent' case as it is already a 'child' case to another case.\n\rPlesae select the same 'parent' case for all cases that should be linked together.");
            else
            {
                AuthNumber = grdCaseLinkLookup.Rows[e.RowIndex].Cells["AuthNumber"].Value.ToString();
                LinkCaseID = Convert.ToInt32(grdCaseLinkLookup.Rows[e.RowIndex].Cells["CaseID"].Value.ToString());
            }
            txtAuthNumber.Text = AuthNumber;
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            AuthNumber = "";
            this.Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            AuthNumber = txtAuthNumber.Text;
            this.Close();
        }
    }
}

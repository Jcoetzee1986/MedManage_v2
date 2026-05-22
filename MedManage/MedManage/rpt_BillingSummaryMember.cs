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
    public partial class rpt_BillingSummaryMember : Form
    {
        int MemberID;
        public rpt_BillingSummaryMember(int memberID)
        {
            InitializeComponent();
            MemberID = memberID;
        }

        private void rpt_CaseCommentExport_Load(object sender, EventArgs e)
        {
            this.ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_CaseComment_Select' table. You can move, or remove it, as needed.
            this.usp_rpt_Billing_MemberTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Billing_Member, MemberID);
            
            this.reportViewer1.RefreshReport();
        }
    }
}

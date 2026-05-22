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
    public partial class rpt_PrintLinkedCases : Form
    {
        int CaseID;

        public rpt_PrintLinkedCases(int caseID)
        {
            CaseID = caseID;
            InitializeComponent();
            
        }

        private void rpt_PrintLinkedCases_Load(object sender, EventArgs e)
        {
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Cases_SelectLinked_CaseID' table. You can move, or remove it, as needed.
            this.usp_rpt_Cases_SelectLinked_CaseIDTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_SelectLinked_CaseID,CaseID);

            this.reportViewer1.RefreshReport();
        }
    }
}

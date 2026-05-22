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
    public partial class RPT_CaseTariffDetail : Form
    {
        int CaseID;
        public RPT_CaseTariffDetail(int caseID)
        {
            CaseID = caseID;
            InitializeComponent();
        }

        private void RPT_CaseTariffDetail_Load(object sender, EventArgs e)
        {
            this.ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_RPT_Case_Tariff_Select' table. You can move, or remove it, as needed.
            this.usp_RPT_Case_Tariff_SelectTableAdapter.Fill(this.ReportsDataSet.usp_RPT_Case_Tariff_Select, CaseID);
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Cases_Select_CaseID' table. You can move, or remove it, as needed.
            this.usp_rpt_Cases_Select_CaseIDTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_CaseID, CaseID);
            this.reportViewer1.RefreshReport();
        }
    }
}

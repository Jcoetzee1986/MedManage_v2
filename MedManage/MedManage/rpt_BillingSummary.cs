using Icondev.MedManage.MedManageLib;
using Icondev.MedManage.MedManageLib.Shared;
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
    public partial class rpt_BillingSummary : Form
    {
        int ProviderID;
        //SharedObjects oShared = new SharedObjects(Program.oDb);
        CaseManagement oCM = new CaseManagement(Program.oDb);

        public rpt_BillingSummary()
        {
            InitializeComponent();
            comboCaseStatus.DataSource = oCM.usp_CaseStatus_Select();
            comboCaseStatus.DisplayMember = "CaseStatus";
            comboCaseStatus.ValueMember = "CaseStatusID";
            comboCaseStatus.SelectedText = "Closed";
        }

        private void rpt_CaseCommentExport_Load(object sender, EventArgs e)
        {
            this.ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_CaseComment_Select' table. You can move, or remove it, as needed.
            this.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter.Fill(ReportsDataSet.usp_rpt_Cases_Select_FinalInvoiceAmountUpdated
                , dateFinalInvoiceAmountStart.Value
                , dateFinalInvoiceAmountEnd.Value
                , Convert.ToInt32(comboCaseStatus.SelectedValue));

            this.reportViewer1.RefreshReport();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.ReportsDataSet.EnforceConstraints = false;
            // INFO: This line of code loads data into the 'ReportsDataSet.usp_CaseComment_Select' table. You can move, or remove it, as needed.
            this.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_FinalInvoiceAmountUpdated
                , dateFinalInvoiceAmountStart.Value
                , dateFinalInvoiceAmountEnd.Value
                , Convert.ToInt32(comboCaseStatus.SelectedValue));

            this.reportViewer1.RefreshReport();
        }
    }
}

namespace Icondev.MedManage
{
    partial class rpt_BillingSummaryMember
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.usp_rpt_BillingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.usp_rpt_Billing_MemberTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Billing_MemberTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_BillingBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // usp_rpt_BillingBindingSource
            // 
            this.usp_rpt_BillingBindingSource.DataMember = "usp_rpt_Billing_Member";
            this.usp_rpt_BillingBindingSource.DataSource = this.ReportsDataSet;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "BillingSummary";
            reportDataSource1.Value = this.usp_rpt_BillingBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.BillingSummaryCase.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(847, 506);
            this.reportViewer1.TabIndex = 1;
            // 
            // usp_rpt_Billing_MemberTableAdapter
            // 
            this.usp_rpt_Billing_MemberTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_BillingSummaryMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 506);
            this.Controls.Add(this.reportViewer1);
            this.Name = "rpt_BillingSummaryMember";
            this.Text = "rpt_BillingSummaryCase";
            this.Load += new System.EventHandler(this.rpt_CaseCommentExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_BillingBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_rpt_BillingBindingSource;
        private ReportsDataSet ReportsDataSet;
        private ReportsDataSetTableAdapters.usp_rpt_Billing_MemberTableAdapter usp_rpt_Billing_MemberTableAdapter;
    }
}
namespace Icondev.MedManage
{
    partial class RPT_CaseTariffDetail
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.usp_RPT_Case_Tariff_SelectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_RPT_Case_Tariff_SelectTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_RPT_Case_Tariff_SelectTableAdapter();
            this.usp_rpt_Cases_Select_CaseIDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_rpt_Cases_Select_CaseIDTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_CaseIDTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_RPT_Case_Tariff_SelectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_CaseIDBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "Data";
            reportDataSource1.Value = this.usp_RPT_Case_Tariff_SelectBindingSource;
            reportDataSource2.Name = "Case";
            reportDataSource2.Value = this.usp_rpt_Cases_Select_CaseIDBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.CaseTariffDetail.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(794, 491);
            this.reportViewer1.TabIndex = 1;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usp_RPT_Case_Tariff_SelectBindingSource
            // 
            this.usp_RPT_Case_Tariff_SelectBindingSource.DataMember = "usp_RPT_Case_Tariff_Select";
            this.usp_RPT_Case_Tariff_SelectBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usp_RPT_Case_Tariff_SelectTableAdapter
            // 
            this.usp_RPT_Case_Tariff_SelectTableAdapter.ClearBeforeFill = true;
            // 
            // usp_rpt_Cases_Select_CaseIDBindingSource
            // 
            this.usp_rpt_Cases_Select_CaseIDBindingSource.DataMember = "usp_rpt_Cases_Select_CaseID";
            this.usp_rpt_Cases_Select_CaseIDBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usp_rpt_Cases_Select_CaseIDTableAdapter
            // 
            this.usp_rpt_Cases_Select_CaseIDTableAdapter.ClearBeforeFill = true;
            // 
            // RPT_CaseTariffDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 491);
            this.Controls.Add(this.reportViewer1);
            this.Name = "RPT_CaseTariffDetail";
            this.Text = "RPT_CaseTariffDetail";
            this.Load += new System.EventHandler(this.RPT_CaseTariffDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_RPT_Case_Tariff_SelectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_CaseIDBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_RPT_Case_Tariff_SelectBindingSource;
        private ReportsDataSet ReportsDataSet;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_CaseIDBindingSource;
        private ReportsDataSetTableAdapters.usp_RPT_Case_Tariff_SelectTableAdapter usp_RPT_Case_Tariff_SelectTableAdapter;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_CaseIDTableAdapter usp_rpt_Cases_Select_CaseIDTableAdapter;
    }
}
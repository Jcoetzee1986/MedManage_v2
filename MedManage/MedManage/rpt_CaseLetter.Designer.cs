namespace Icondev.MedManage
{
    partial class rpt_CaseLetter
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.usp_rpt_Cases_Select_CaseIDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.uspCaseICDSelectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uspCaseTariffSelectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usprptCaseNoteSelectLastNoteCaseIDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uspCaseCPTSelectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rptCaseLetter = new Microsoft.Reporting.WinForms.ReportViewer();
            this.usp_rpt_Cases_Select_CaseIDTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_CaseIDTableAdapter();
            this.usp_Case_ICD_SelectTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_Case_ICD_SelectTableAdapter();
            this.usp_Case_Tariff_SelectTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_Case_Tariff_SelectTableAdapter();
            this.usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter();
            this.usp_Case_CPT_SelectTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_Case_CPT_SelectTableAdapter();
            this.uspCaseFacilityTypeSelectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_Case_FacilityType_SelectTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_Case_FacilityType_SelectTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_CaseIDBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseICDSelectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseTariffSelectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usprptCaseNoteSelectLastNoteCaseIDBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseCPTSelectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseFacilityTypeSelectBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // usp_rpt_Cases_Select_CaseIDBindingSource
            // 
            this.usp_rpt_Cases_Select_CaseIDBindingSource.DataMember = "usp_rpt_Cases_Select_CaseID";
            this.usp_rpt_Cases_Select_CaseIDBindingSource.DataSource = this.ReportsDataSet;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uspCaseICDSelectBindingSource
            // 
            this.uspCaseICDSelectBindingSource.DataMember = "usp_Case_ICD_Select";
            this.uspCaseICDSelectBindingSource.DataSource = this.ReportsDataSet;
            // 
            // uspCaseTariffSelectBindingSource
            // 
            this.uspCaseTariffSelectBindingSource.DataMember = "usp_Case_Tariff_Select";
            this.uspCaseTariffSelectBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usprptCaseNoteSelectLastNoteCaseIDBindingSource
            // 
            this.usprptCaseNoteSelectLastNoteCaseIDBindingSource.DataMember = "usp_rpt_CaseNote_SelectLastNote_CaseID";
            this.usprptCaseNoteSelectLastNoteCaseIDBindingSource.DataSource = this.ReportsDataSet;
            // 
            // uspCaseCPTSelectBindingSource
            // 
            this.uspCaseCPTSelectBindingSource.DataMember = "usp_Case_CPT_Select";
            this.uspCaseCPTSelectBindingSource.DataSource = this.ReportsDataSet;
            // 
            // rptCaseLetter
            // 
            this.rptCaseLetter.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "Case";
            reportDataSource1.Value = this.usp_rpt_Cases_Select_CaseIDBindingSource;
            reportDataSource2.Name = "ICD";
            reportDataSource2.Value = this.uspCaseICDSelectBindingSource;
            reportDataSource3.Name = "Tariff";
            reportDataSource3.Value = this.uspCaseTariffSelectBindingSource;
            reportDataSource4.Name = "LastCaseNote";
            reportDataSource4.Value = this.usprptCaseNoteSelectLastNoteCaseIDBindingSource;
            reportDataSource5.Name = "CPT";
            reportDataSource5.Value = this.uspCaseCPTSelectBindingSource;
            reportDataSource6.Name = "FacilityType";
            reportDataSource6.Value = this.uspCaseFacilityTypeSelectBindingSource;
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource1);
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource2);
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource3);
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource4);
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource5);
            this.rptCaseLetter.LocalReport.DataSources.Add(reportDataSource6);
            this.rptCaseLetter.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.CaseLetter.rdlc";
            this.rptCaseLetter.Location = new System.Drawing.Point(0, 0);
            this.rptCaseLetter.Name = "rptCaseLetter";
            this.rptCaseLetter.Size = new System.Drawing.Size(795, 562);
            this.rptCaseLetter.TabIndex = 0;
            // 
            // usp_rpt_Cases_Select_CaseIDTableAdapter
            // 
            this.usp_rpt_Cases_Select_CaseIDTableAdapter.ClearBeforeFill = true;
            // 
            // usp_Case_ICD_SelectTableAdapter
            // 
            this.usp_Case_ICD_SelectTableAdapter.ClearBeforeFill = true;
            // 
            // usp_Case_Tariff_SelectTableAdapter
            // 
            this.usp_Case_Tariff_SelectTableAdapter.ClearBeforeFill = true;
            // 
            // usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter
            // 
            this.usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter.ClearBeforeFill = true;
            // 
            // usp_Case_CPT_SelectTableAdapter
            // 
            this.usp_Case_CPT_SelectTableAdapter.ClearBeforeFill = true;
            // 
            // uspCaseFacilityTypeSelectBindingSource
            // 
            this.uspCaseFacilityTypeSelectBindingSource.DataMember = "usp_Case_FacilityType_Select";
            this.uspCaseFacilityTypeSelectBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usp_Case_FacilityType_SelectTableAdapter
            // 
            this.usp_Case_FacilityType_SelectTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_CaseLetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 562);
            this.Controls.Add(this.rptCaseLetter);
            this.Name = "rpt_CaseLetter";
            this.Text = "rpt_CaseLetter";
            this.Load += new System.EventHandler(this.rpt_CaseLetter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_CaseIDBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseICDSelectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseTariffSelectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usprptCaseNoteSelectLastNoteCaseIDBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseCPTSelectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uspCaseFacilityTypeSelectBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptCaseLetter;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_CaseIDBindingSource;
        private ReportsDataSet ReportsDataSet;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_CaseIDTableAdapter usp_rpt_Cases_Select_CaseIDTableAdapter;
        private System.Windows.Forms.BindingSource uspCaseICDSelectBindingSource;
        private ReportsDataSetTableAdapters.usp_Case_ICD_SelectTableAdapter usp_Case_ICD_SelectTableAdapter;
        private System.Windows.Forms.BindingSource uspCaseTariffSelectBindingSource;
        private ReportsDataSetTableAdapters.usp_Case_Tariff_SelectTableAdapter usp_Case_Tariff_SelectTableAdapter;
        private System.Windows.Forms.BindingSource usprptCaseNoteSelectLastNoteCaseIDBindingSource;
        private ReportsDataSetTableAdapters.usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter;
        private System.Windows.Forms.BindingSource uspCaseCPTSelectBindingSource;
        private ReportsDataSetTableAdapters.usp_Case_CPT_SelectTableAdapter usp_Case_CPT_SelectTableAdapter;
        private System.Windows.Forms.BindingSource uspCaseFacilityTypeSelectBindingSource;
        private ReportsDataSetTableAdapters.usp_Case_FacilityType_SelectTableAdapter usp_Case_FacilityType_SelectTableAdapter;
    }
}
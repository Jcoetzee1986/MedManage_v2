namespace Icondev.MedManage
{
    partial class rpt_PrintLinkedCases
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_rpt_Cases_SelectLinked_CaseIDTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_SelectLinked_CaseIDTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "LinkedCase";
            reportDataSource1.Value = this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.PrintLinkedCase.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(804, 465);
            this.reportViewer1.TabIndex = 0;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usp_rpt_Cases_SelectLinked_CaseIDBindingSource
            // 
            this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource.DataMember = "usp_rpt_Cases_SelectLinked_CaseID";
            this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usp_rpt_Cases_SelectLinked_CaseIDTableAdapter
            // 
            this.usp_rpt_Cases_SelectLinked_CaseIDTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_PrintLinkedCases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 465);
            this.Controls.Add(this.reportViewer1);
            this.Name = "rpt_PrintLinkedCases";
            this.Text = "rpt_PrintLinkedCases";
            this.Load += new System.EventHandler(this.rpt_PrintLinkedCases_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_SelectLinked_CaseIDBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_SelectLinked_CaseIDBindingSource;
        private ReportsDataSet ReportsDataSet;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_SelectLinked_CaseIDTableAdapter usp_rpt_Cases_SelectLinked_CaseIDTableAdapter;
    }
}
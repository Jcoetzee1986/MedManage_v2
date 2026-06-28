namespace Icondev.MedManage
{
    partial class rpt_MyCasesExport
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
            this.usp_rpt_Cases_Select_FiltersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_rpt_Cases_Select_FiltersTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_FiltersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_FiltersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "Cases";
            reportDataSource1.Value = this.usp_rpt_Cases_Select_FiltersBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.MyCasesExport.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(734, 413);
            this.reportViewer1.TabIndex = 0;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usp_rpt_Cases_Select_FiltersBindingSource
            // 
            this.usp_rpt_Cases_Select_FiltersBindingSource.DataMember = "usp_rpt_Cases_Select_Filters";
            this.usp_rpt_Cases_Select_FiltersBindingSource.DataSource = this.ReportsDataSet;
            // 
            // usp_rpt_Cases_Select_FiltersTableAdapter
            // 
            this.usp_rpt_Cases_Select_FiltersTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_MyCasesExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 413);
            this.Controls.Add(this.reportViewer1);
            this.Name = "rpt_MyCasesExport";
            this.Text = "rpt_MyCasesExport";
            this.Load += new System.EventHandler(this.rpt_MyCasesExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_FiltersBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_FiltersBindingSource;
        private ReportsDataSet ReportsDataSet;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_FiltersTableAdapter usp_rpt_Cases_Select_FiltersTableAdapter;
    }
}
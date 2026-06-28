namespace Icondev.MedManage
{
    partial class rpt_BillingSummary
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
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label3 = new System.Windows.Forms.Label();
            this.comboCaseStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountStart = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            reportDataSource1.Name = "BillingSummary";
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.CasesFinalInvoiceAmountUpdated.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 65);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1155, 622);
            this.reportViewer1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(590, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 137;
            this.label3.Text = "Case Status";
            // 
            // comboCaseStatus
            // 
            this.comboCaseStatus.FormattingEnabled = true;
            this.comboCaseStatus.Location = new System.Drawing.Point(681, 6);
            this.comboCaseStatus.Margin = new System.Windows.Forms.Padding(4);
            this.comboCaseStatus.Name = "comboCaseStatus";
            this.comboCaseStatus.Size = new System.Drawing.Size(160, 24);
            this.comboCaseStatus.TabIndex = 136;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(398, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 17);
            this.label6.TabIndex = 134;
            this.label6.Text = "And";
            // 
            // dateFinalInvoiceAmountEnd
            // 
            this.dateFinalInvoiceAmountEnd.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountEnd.Location = new System.Drawing.Point(444, 6);
            this.dateFinalInvoiceAmountEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dateFinalInvoiceAmountEnd.Name = "dateFinalInvoiceAmountEnd";
            this.dateFinalInvoiceAmountEnd.Size = new System.Drawing.Size(108, 22);
            this.dateFinalInvoiceAmountEnd.TabIndex = 130;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 17);
            this.label5.TabIndex = 135;
            this.label5.Text = "Final Invoice Amount Updated Between ";
            // 
            // dateFinalInvoiceAmountStart
            // 
            this.dateFinalInvoiceAmountStart.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountStart.Location = new System.Drawing.Point(277, 6);
            this.dateFinalInvoiceAmountStart.Margin = new System.Windows.Forms.Padding(4);
            this.dateFinalInvoiceAmountStart.Name = "dateFinalInvoiceAmountStart";
            this.dateFinalInvoiceAmountStart.Size = new System.Drawing.Size(108, 22);
            this.dateFinalInvoiceAmountStart.TabIndex = 128;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(989, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(153, 28);
            this.btnRefresh.TabIndex = 138;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "usp_rpt_Cases_Select_FinalInvoiceAmountUpdated";
            this.bindingSource1.DataSource = this.ReportsDataSet;
            // 
            // usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter
            // 
            this.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_BillingSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 687);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboCaseStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateFinalInvoiceAmountEnd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateFinalInvoiceAmountStart);
            this.Controls.Add(this.reportViewer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "rpt_BillingSummary";
            this.Text = "rpt_BillingSummary";
            this.Load += new System.EventHandler(this.rpt_CaseCommentExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ReportsDataSet ReportsDataSet;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboCaseStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountStart;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.BindingSource bindingSource1;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter usp_rpt_Cases_Select_FinalInvoiceAmountUpdatedTableAdapter;
    }
}
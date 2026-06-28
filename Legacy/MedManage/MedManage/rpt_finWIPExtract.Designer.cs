namespace Icondev.MedManage
{
    partial class rpt_finWIPExtract
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.usp_rpt_Finance_Select_FiltersTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Finance_Select_FiltersTableAdapter();
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter();
            this.comboMedicalFunder = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.usp_rpt_Finance_Select_FiltersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.comboBillingStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboCaseStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountStart = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLookupPracticeName = new System.Windows.Forms.TextBox();
            this.lblSelectedPracticeName = new System.Windows.Forms.Label();
            this.lblCurrentPracticeID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLookupPracticeNumber = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Finance_Select_FiltersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // usp_rpt_Finance_Select_FiltersTableAdapter
            // 
            this.usp_rpt_Finance_Select_FiltersTableAdapter.ClearBeforeFill = true;
            // 
            // usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter.ClearBeforeFill = true;
            // 
            // comboMedicalFunder
            // 
            this.comboMedicalFunder.FormattingEnabled = true;
            this.comboMedicalFunder.Location = new System.Drawing.Point(706, 62);
            this.comboMedicalFunder.Name = "comboMedicalFunder";
            this.comboMedicalFunder.Size = new System.Drawing.Size(178, 21);
            this.comboMedicalFunder.TabIndex = 74;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(620, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 79;
            this.label8.Text = "Medical Funder";
            this.label8.Visible = false;
            // 
            // usp_rpt_Finance_Select_FiltersBindingSource
            // 
            this.usp_rpt_Finance_Select_FiltersBindingSource.DataMember = "usp_rpt_Finance_Select_Filters";
            this.usp_rpt_Finance_Select_FiltersBindingSource.DataSource = this.ReportsDataSet;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.comboBillingStatus);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.comboCaseStatus);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.dateFinalInvoiceAmountEnd);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.dateFinalInvoiceAmountStart);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.txtLookupPracticeName);
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectedPracticeName);
            this.splitContainer1.Panel1.Controls.Add(this.lblCurrentPracticeID);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtLookupPracticeNumber);
            this.splitContainer1.Panel1.Controls.Add(this.comboMedicalFunder);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.picRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.reportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(887, 433);
            this.splitContainer1.SplitterDistance = 91;
            this.splitContainer1.TabIndex = 2;
            // 
            // picRefresh
            // 
            this.picRefresh.BackColor = System.Drawing.Color.White;
            this.picRefresh.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRefresh.Image = global::Icondev.MedManage.Properties.Resources.Magnifier2;
            this.picRefresh.Location = new System.Drawing.Point(840, 12);
            this.picRefresh.Name = "picRefresh";
            this.picRefresh.Size = new System.Drawing.Size(40, 33);
            this.picRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRefresh.TabIndex = 58;
            this.picRefresh.TabStop = false;
            this.picRefresh.Click += new System.EventHandler(this.picRefresh_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource3.Name = "CaseFinance";
            reportDataSource3.Value = this.usp_rpt_Finance_Select_FiltersBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.CaseFinances.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(887, 338);
            this.reportViewer1.TabIndex = 0;
            // 
            // usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataMember = "usp_rpt_Cases_Select_Filters_BetweenDates";
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataSource = this.ReportsDataSet;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(389, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 143;
            this.label7.Text = "Billing Status";
            // 
            // comboBillingStatus
            // 
            this.comboBillingStatus.FormattingEnabled = true;
            this.comboBillingStatus.Location = new System.Drawing.Point(457, 63);
            this.comboBillingStatus.Name = "comboBillingStatus";
            this.comboBillingStatus.Size = new System.Drawing.Size(121, 21);
            this.comboBillingStatus.TabIndex = 142;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(389, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 141;
            this.label3.Text = "Case Status";
            // 
            // comboCaseStatus
            // 
            this.comboCaseStatus.FormattingEnabled = true;
            this.comboCaseStatus.Location = new System.Drawing.Point(457, 36);
            this.comboCaseStatus.Name = "comboCaseStatus";
            this.comboCaseStatus.Size = new System.Drawing.Size(121, 21);
            this.comboCaseStatus.TabIndex = 140;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(301, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 138;
            this.label6.Text = "And";
            // 
            // dateFinalInvoiceAmountEnd
            // 
            this.dateFinalInvoiceAmountEnd.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountEnd.Location = new System.Drawing.Point(335, 6);
            this.dateFinalInvoiceAmountEnd.Name = "dateFinalInvoiceAmountEnd";
            this.dateFinalInvoiceAmountEnd.Size = new System.Drawing.Size(82, 20);
            this.dateFinalInvoiceAmountEnd.TabIndex = 132;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 13);
            this.label5.TabIndex = 139;
            this.label5.Text = "Final Invoice Amount Updated Between ";
            // 
            // dateFinalInvoiceAmountStart
            // 
            this.dateFinalInvoiceAmountStart.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountStart.Location = new System.Drawing.Point(210, 6);
            this.dateFinalInvoiceAmountStart.Name = "dateFinalInvoiceAmountStart";
            this.dateFinalInvoiceAmountStart.Size = new System.Drawing.Size(82, 20);
            this.dateFinalInvoiceAmountStart.TabIndex = 130;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 137;
            this.label4.Text = "Practice Name";
            // 
            // txtLookupPracticeName
            // 
            this.txtLookupPracticeName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLookupPracticeName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtLookupPracticeName.Location = new System.Drawing.Point(102, 62);
            this.txtLookupPracticeName.Name = "txtLookupPracticeName";
            this.txtLookupPracticeName.Size = new System.Drawing.Size(216, 20);
            this.txtLookupPracticeName.TabIndex = 134;
            // 
            // lblSelectedPracticeName
            // 
            this.lblSelectedPracticeName.AutoSize = true;
            this.lblSelectedPracticeName.Location = new System.Drawing.Point(454, 12);
            this.lblSelectedPracticeName.Name = "lblSelectedPracticeName";
            this.lblSelectedPracticeName.Size = new System.Drawing.Size(108, 13);
            this.lblSelectedPracticeName.TabIndex = 136;
            this.lblSelectedPracticeName.Text = "No Provider Selected";
            // 
            // lblCurrentPracticeID
            // 
            this.lblCurrentPracticeID.AutoSize = true;
            this.lblCurrentPracticeID.Location = new System.Drawing.Point(572, 12);
            this.lblCurrentPracticeID.Name = "lblCurrentPracticeID";
            this.lblCurrentPracticeID.Size = new System.Drawing.Size(101, 13);
            this.lblCurrentPracticeID.TabIndex = 135;
            this.lblCurrentPracticeID.Text = "lblCurrentPracticeID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 131;
            this.label2.Text = "Practice Number";
            // 
            // txtLookupPracticeNumber
            // 
            this.txtLookupPracticeNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLookupPracticeNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtLookupPracticeNumber.Location = new System.Drawing.Point(102, 36);
            this.txtLookupPracticeNumber.Name = "txtLookupPracticeNumber";
            this.txtLookupPracticeNumber.Size = new System.Drawing.Size(216, 20);
            this.txtLookupPracticeNumber.TabIndex = 133;
            // 
            // rpt_finWIPExtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 433);
            this.Controls.Add(this.splitContainer1);
            this.Name = "rpt_finWIPExtract";
            this.Text = "rpt_finWIPExtract";
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Finance_Select_FiltersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ReportsDataSetTableAdapters.usp_rpt_Finance_Select_FiltersTableAdapter usp_rpt_Finance_Select_FiltersTableAdapter;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter;
        private System.Windows.Forms.ComboBox comboMedicalFunder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.BindingSource usp_rpt_Finance_Select_FiltersBindingSource;
        private ReportsDataSet ReportsDataSet;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox picRefresh;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBillingStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboCaseStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLookupPracticeName;
        private System.Windows.Forms.Label lblSelectedPracticeName;
        private System.Windows.Forms.Label lblCurrentPracticeID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLookupPracticeNumber;
    }
}
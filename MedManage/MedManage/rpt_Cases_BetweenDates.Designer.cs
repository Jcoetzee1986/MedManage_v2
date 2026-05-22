namespace Icondev.MedManage
{
    partial class rpt_Cases_BetweenDates
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.chkMedicalFunderSearch = new System.Windows.Forms.CheckBox();
            this.chkDOBSearch = new System.Windows.Forms.CheckBox();
            this.txtPrimaryCpt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPrimaryIcd = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboMedicalFunder = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPracticeName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateMemberDOB = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMemberSurname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMemberName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMemberNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCaseNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTo = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.comboCaseStatus = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.chkStatus);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.comboCaseStatus);
            this.splitContainer1.Panel1.Controls.Add(this.dateTo);
            this.splitContainer1.Panel1.Controls.Add(this.label13);
            this.splitContainer1.Panel1.Controls.Add(this.dateFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.txtUser);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.picRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.chkMedicalFunderSearch);
            this.splitContainer1.Panel1.Controls.Add(this.chkDOBSearch);
            this.splitContainer1.Panel1.Controls.Add(this.txtPrimaryCpt);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.txtPrimaryIcd);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.comboMedicalFunder);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.txtPracticeName);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.dateMemberDOB);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.txtMemberSurname);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.txtMemberName);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.txtMemberNumber);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtCaseNumber);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.reportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(892, 673);
            this.splitContainer1.SplitterDistance = 143;
            this.splitContainer1.TabIndex = 0;
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
            // chkMedicalFunderSearch
            // 
            this.chkMedicalFunderSearch.AutoSize = true;
            this.chkMedicalFunderSearch.Location = new System.Drawing.Point(477, 97);
            this.chkMedicalFunderSearch.Name = "chkMedicalFunderSearch";
            this.chkMedicalFunderSearch.Size = new System.Drawing.Size(15, 14);
            this.chkMedicalFunderSearch.TabIndex = 52;
            this.chkMedicalFunderSearch.UseVisualStyleBackColor = true;
            // 
            // chkDOBSearch
            // 
            this.chkDOBSearch.AutoSize = true;
            this.chkDOBSearch.Location = new System.Drawing.Point(179, 98);
            this.chkDOBSearch.Name = "chkDOBSearch";
            this.chkDOBSearch.Size = new System.Drawing.Size(15, 14);
            this.chkDOBSearch.TabIndex = 46;
            this.chkDOBSearch.UseVisualStyleBackColor = true;
            // 
            // txtPrimaryCpt
            // 
            this.txtPrimaryCpt.Location = new System.Drawing.Point(573, 70);
            this.txtPrimaryCpt.Name = "txtPrimaryCpt";
            this.txtPrimaryCpt.Size = new System.Drawing.Size(100, 20);
            this.txtPrimaryCpt.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(532, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "CPT";
            // 
            // txtPrimaryIcd
            // 
            this.txtPrimaryIcd.Location = new System.Drawing.Point(429, 70);
            this.txtPrimaryIcd.Name = "txtPrimaryIcd";
            this.txtPrimaryIcd.Size = new System.Drawing.Size(100, 20);
            this.txtPrimaryIcd.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(367, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "ICD";
            // 
            // comboMedicalFunder
            // 
            this.comboMedicalFunder.FormattingEnabled = true;
            this.comboMedicalFunder.Location = new System.Drawing.Point(293, 95);
            this.comboMedicalFunder.Name = "comboMedicalFunder";
            this.comboMedicalFunder.Size = new System.Drawing.Size(178, 21);
            this.comboMedicalFunder.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(207, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 55;
            this.label8.Text = "Medical Funder";
            // 
            // txtPracticeName
            // 
            this.txtPracticeName.Location = new System.Drawing.Point(87, 70);
            this.txtPracticeName.Name = "txtPracticeName";
            this.txtPracticeName.Size = new System.Drawing.Size(274, 20);
            this.txtPracticeName.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "Practice";
            // 
            // dateMemberDOB
            // 
            this.dateMemberDOB.CustomFormat = "yyyy/MM/dd";
            this.dateMemberDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateMemberDOB.Location = new System.Drawing.Point(87, 96);
            this.dateMemberDOB.Name = "dateMemberDOB";
            this.dateMemberDOB.Size = new System.Drawing.Size(86, 20);
            this.dateMemberDOB.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 51;
            this.label5.Text = "DOB";
            // 
            // txtMemberSurname
            // 
            this.txtMemberSurname.Location = new System.Drawing.Point(429, 43);
            this.txtMemberSurname.Name = "txtMemberSurname";
            this.txtMemberSurname.Size = new System.Drawing.Size(100, 20);
            this.txtMemberSurname.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(367, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "Surname";
            // 
            // txtMemberName
            // 
            this.txtMemberName.Location = new System.Drawing.Point(573, 44);
            this.txtMemberName.Name = "txtMemberName";
            this.txtMemberName.Size = new System.Drawing.Size(100, 20);
            this.txtMemberName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(532, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Name";
            // 
            // txtMemberNumber
            // 
            this.txtMemberNumber.Location = new System.Drawing.Point(261, 43);
            this.txtMemberNumber.Name = "txtMemberNumber";
            this.txtMemberNumber.Size = new System.Drawing.Size(100, 20);
            this.txtMemberNumber.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Member No";
            // 
            // txtCaseNumber
            // 
            this.txtCaseNumber.Location = new System.Drawing.Point(87, 43);
            this.txtCaseNumber.Name = "txtCaseNumber";
            this.txtCaseNumber.Size = new System.Drawing.Size(100, 20);
            this.txtCaseNumber.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Case Number";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(573, 94);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 59;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(532, 97);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 60;
            this.label11.Text = "User";
            // 
            // dateFrom
            // 
            this.dateFrom.CustomFormat = "yyyy/MM/dd";
            this.dateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFrom.Location = new System.Drawing.Point(87, 12);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(84, 20);
            this.dateFrom.TabIndex = 61;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 62;
            this.label12.Text = "Date From";
            // 
            // dateTo
            // 
            this.dateTo.CustomFormat = "yyyy/MM/dd";
            this.dateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTo.Location = new System.Drawing.Point(261, 12);
            this.dateTo.Name = "dateTo";
            this.dateTo.Size = new System.Drawing.Size(84, 20);
            this.dateTo.TabIndex = 63;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(184, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 13);
            this.label13.TabIndex = 64;
            this.label13.Text = "Date To";
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource2.Name = "Cases";
            reportDataSource2.Value = this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.Cases_BetweenDates.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(892, 526);
            this.reportViewer1.TabIndex = 0;
            // 
            // comboCaseStatus
            // 
            this.comboCaseStatus.BackColor = System.Drawing.Color.Yellow;
            this.comboCaseStatus.FormattingEnabled = true;
            this.comboCaseStatus.Location = new System.Drawing.Point(429, 11);
            this.comboCaseStatus.Name = "comboCaseStatus";
            this.comboCaseStatus.Size = new System.Drawing.Size(143, 21);
            this.comboCaseStatus.TabIndex = 65;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(367, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 66;
            this.label6.Text = "Status";
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.BackColor = System.Drawing.Color.Yellow;
            this.chkStatus.Location = new System.Drawing.Point(578, 14);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(15, 14);
            this.chkStatus.TabIndex = 67;
            this.chkStatus.UseVisualStyleBackColor = false;
            // 
            // usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataMember = "usp_rpt_Cases_Select_Filters_BetweenDates";
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataSource = this.ReportsDataSet;
            // 
            // ReportsDataSet
            // 
            this.ReportsDataSet.DataSetName = "ReportsDataSet";
            this.ReportsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_Cases_BetweenDates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 673);
            this.Controls.Add(this.splitContainer1);
            this.Name = "rpt_Cases_BetweenDates";
            this.Text = "rpt_Cases_BetweenDates";
            this.Load += new System.EventHandler(this.rpt_Cases_BetweenDates_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource;
        private ReportsDataSet ReportsDataSet;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.CheckBox chkMedicalFunderSearch;
        private System.Windows.Forms.CheckBox chkDOBSearch;
        private System.Windows.Forms.TextBox txtPrimaryCpt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPrimaryIcd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboMedicalFunder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPracticeName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateMemberDOB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMemberSurname;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMemberName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMemberNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCaseNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label11;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboCaseStatus;
        private System.Windows.Forms.CheckBox chkStatus;
    }
}
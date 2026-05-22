namespace Icondev.MedManage
{
    partial class rpt_Finance
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
            this.usp_rpt_Finance_Select_FiltersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ReportsDataSet = new Icondev.MedManage.ReportsDataSet();
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.picRefresh = new System.Windows.Forms.PictureBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkMedicalFunderSearch = new System.Windows.Forms.CheckBox();
            this.chkDateCreatedSearch = new System.Windows.Forms.CheckBox();
            this.chkDOBSearch = new System.Windows.Forms.CheckBox();
            this.txtPrimaryCpt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPrimaryIcd = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboMedicalFunder = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPracticeName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dateCaseCreated = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
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
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter();
            this.usp_rpt_Finance_Select_FiltersTableAdapter = new Icondev.MedManage.ReportsDataSetTableAdapters.usp_rpt_Finance_Select_FiltersTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Finance_Select_FiltersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            // usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataMember = "usp_rpt_Cases_Select_Filters_BetweenDates";
            this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource.DataSource = this.ReportsDataSet;
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
            reportDataSource1.Name = "CaseFinance";
            reportDataSource1.Value = this.usp_rpt_Finance_Select_FiltersBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports.CaseFinances.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(889, 460);
            this.reportViewer1.TabIndex = 0;
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
            this.splitContainer1.Panel1.Controls.Add(this.chkMedicalFunderSearch);
            this.splitContainer1.Panel1.Controls.Add(this.chkDateCreatedSearch);
            this.splitContainer1.Panel1.Controls.Add(this.chkDOBSearch);
            this.splitContainer1.Panel1.Controls.Add(this.txtPrimaryCpt);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.txtPrimaryIcd);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.comboMedicalFunder);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.txtPracticeName);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.dateCaseCreated);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
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
            this.splitContainer1.Panel1.Controls.Add(this.picRefresh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.reportViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(889, 588);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 1;
            // 
            // chkMedicalFunderSearch
            // 
            this.chkMedicalFunderSearch.AutoSize = true;
            this.chkMedicalFunderSearch.Location = new System.Drawing.Point(662, 65);
            this.chkMedicalFunderSearch.Name = "chkMedicalFunderSearch";
            this.chkMedicalFunderSearch.Size = new System.Drawing.Size(15, 14);
            this.chkMedicalFunderSearch.TabIndex = 76;
            this.chkMedicalFunderSearch.UseVisualStyleBackColor = true;
            this.chkMedicalFunderSearch.Visible = false;
            // 
            // chkDateCreatedSearch
            // 
            this.chkDateCreatedSearch.AutoSize = true;
            this.chkDateCreatedSearch.Location = new System.Drawing.Point(371, 67);
            this.chkDateCreatedSearch.Name = "chkDateCreatedSearch";
            this.chkDateCreatedSearch.Size = new System.Drawing.Size(15, 14);
            this.chkDateCreatedSearch.TabIndex = 73;
            this.chkDateCreatedSearch.UseVisualStyleBackColor = true;
            // 
            // chkDOBSearch
            // 
            this.chkDOBSearch.AutoSize = true;
            this.chkDOBSearch.Location = new System.Drawing.Point(183, 67);
            this.chkDOBSearch.Name = "chkDOBSearch";
            this.chkDOBSearch.Size = new System.Drawing.Size(15, 14);
            this.chkDOBSearch.TabIndex = 70;
            this.chkDOBSearch.UseVisualStyleBackColor = true;
            // 
            // txtPrimaryCpt
            // 
            this.txtPrimaryCpt.Location = new System.Drawing.Point(577, 39);
            this.txtPrimaryCpt.Name = "txtPrimaryCpt";
            this.txtPrimaryCpt.Size = new System.Drawing.Size(100, 20);
            this.txtPrimaryCpt.TabIndex = 67;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(536, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 81;
            this.label10.Text = "CPT";
            // 
            // txtPrimaryIcd
            // 
            this.txtPrimaryIcd.Location = new System.Drawing.Point(433, 39);
            this.txtPrimaryIcd.Name = "txtPrimaryIcd";
            this.txtPrimaryIcd.Size = new System.Drawing.Size(100, 20);
            this.txtPrimaryIcd.TabIndex = 65;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(371, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 13);
            this.label9.TabIndex = 80;
            this.label9.Text = "ICD";
            // 
            // comboMedicalFunder
            // 
            this.comboMedicalFunder.FormattingEnabled = true;
            this.comboMedicalFunder.Location = new System.Drawing.Point(478, 63);
            this.comboMedicalFunder.Name = "comboMedicalFunder";
            this.comboMedicalFunder.Size = new System.Drawing.Size(178, 21);
            this.comboMedicalFunder.TabIndex = 74;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(392, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 79;
            this.label8.Text = "Medical Funder";
            this.label8.Visible = false;
            // 
            // txtPracticeName
            // 
            this.txtPracticeName.Location = new System.Drawing.Point(91, 39);
            this.txtPracticeName.Name = "txtPracticeName";
            this.txtPracticeName.Size = new System.Drawing.Size(274, 20);
            this.txtPracticeName.TabIndex = 64;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 78;
            this.label7.Text = "Practice";
            // 
            // dateCaseCreated
            // 
            this.dateCaseCreated.CustomFormat = "yyyy/MM/dd";
            this.dateCaseCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateCaseCreated.Location = new System.Drawing.Point(281, 65);
            this.dateCaseCreated.Name = "dateCaseCreated";
            this.dateCaseCreated.Size = new System.Drawing.Size(84, 20);
            this.dateCaseCreated.TabIndex = 72;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(204, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 77;
            this.label6.Text = "Date Created";
            // 
            // dateMemberDOB
            // 
            this.dateMemberDOB.CustomFormat = "yyyy/MM/dd";
            this.dateMemberDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateMemberDOB.Location = new System.Drawing.Point(91, 65);
            this.dateMemberDOB.Name = "dateMemberDOB";
            this.dateMemberDOB.Size = new System.Drawing.Size(86, 20);
            this.dateMemberDOB.TabIndex = 68;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 75;
            this.label5.Text = "DOB";
            // 
            // txtMemberSurname
            // 
            this.txtMemberSurname.Location = new System.Drawing.Point(433, 12);
            this.txtMemberSurname.Name = "txtMemberSurname";
            this.txtMemberSurname.Size = new System.Drawing.Size(100, 20);
            this.txtMemberSurname.TabIndex = 61;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(371, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "Surname";
            // 
            // txtMemberName
            // 
            this.txtMemberName.Location = new System.Drawing.Point(577, 13);
            this.txtMemberName.Name = "txtMemberName";
            this.txtMemberName.Size = new System.Drawing.Size(100, 20);
            this.txtMemberName.TabIndex = 63;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(536, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "Name";
            // 
            // txtMemberNumber
            // 
            this.txtMemberNumber.Location = new System.Drawing.Point(265, 12);
            this.txtMemberNumber.Name = "txtMemberNumber";
            this.txtMemberNumber.Size = new System.Drawing.Size(100, 20);
            this.txtMemberNumber.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(197, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "Member No";
            // 
            // txtCaseNumber
            // 
            this.txtCaseNumber.Location = new System.Drawing.Point(91, 12);
            this.txtCaseNumber.Name = "txtCaseNumber";
            this.txtCaseNumber.Size = new System.Drawing.Size(100, 20);
            this.txtCaseNumber.TabIndex = 59;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Case Number";
            // 
            // usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter
            // 
            this.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter.ClearBeforeFill = true;
            // 
            // usp_rpt_Finance_Select_FiltersTableAdapter
            // 
            this.usp_rpt_Finance_Select_FiltersTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_Finance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 588);
            this.Controls.Add(this.splitContainer1);
            this.Name = "rpt_Finance";
            this.Text = "rpt_Finance";
            this.Load += new System.EventHandler(this.rpt_Finance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Finance_Select_FiltersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReportsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRefresh)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picRefresh;
        private System.Windows.Forms.BindingSource usp_rpt_Cases_Select_Filters_BetweenDatesBindingSource;
        private ReportsDataSet ReportsDataSet;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ReportsDataSetTableAdapters.usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter usp_rpt_Cases_Select_Filters_BetweenDatesTableAdapter;
        private System.Windows.Forms.CheckBox chkMedicalFunderSearch;
        private System.Windows.Forms.CheckBox chkDateCreatedSearch;
        private System.Windows.Forms.CheckBox chkDOBSearch;
        private System.Windows.Forms.TextBox txtPrimaryCpt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPrimaryIcd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboMedicalFunder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPracticeName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateCaseCreated;
        private System.Windows.Forms.Label label6;
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
        private System.Windows.Forms.BindingSource usp_rpt_Finance_Select_FiltersBindingSource;
        private ReportsDataSetTableAdapters.usp_rpt_Finance_Select_FiltersTableAdapter usp_rpt_Finance_Select_FiltersTableAdapter;

    }
}
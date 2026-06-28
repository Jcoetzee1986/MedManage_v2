namespace Icondev.MedManage
{
    partial class TariffManagement
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
            this.tabTariffManagement = new System.Windows.Forms.TabControl();
            this.tabSingleTariff = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTariff = new System.Windows.Forms.Label();
            this.comboTariffName = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.picCodeLookup = new System.Windows.Forms.PictureBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.datagridSearchResults = new System.Windows.Forms.DataGridView();
            this.comboHospitalType = new System.Windows.Forms.ComboBox();
            this.picAddCode = new System.Windows.Forms.PictureBox();
            this.txtTariffDesc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTariffCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabNewPeriod = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.picAddCustomTariff = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picNewPeriod = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboTariffPeriod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picNew = new System.Windows.Forms.PictureBox();
            this.label59 = new System.Windows.Forms.Label();
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboSelectTariff = new System.Windows.Forms.ComboBox();
            this.datagridTariff = new System.Windows.Forms.DataGridView();
            this.tabTariffManagement.SuspendLayout();
            this.tabSingleTariff.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCodeLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddCode)).BeginInit();
            this.tabNewPeriod.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAddCustomTariff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridTariff)).BeginInit();
            this.SuspendLayout();
            // 
            // tabTariffManagement
            // 
            this.tabTariffManagement.Controls.Add(this.tabSingleTariff);
            this.tabTariffManagement.Controls.Add(this.tabNewPeriod);
            this.tabTariffManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTariffManagement.Location = new System.Drawing.Point(0, 0);
            this.tabTariffManagement.Name = "tabTariffManagement";
            this.tabTariffManagement.SelectedIndex = 0;
            this.tabTariffManagement.Size = new System.Drawing.Size(883, 580);
            this.tabTariffManagement.TabIndex = 0;
            this.tabTariffManagement.SelectedIndexChanged += new System.EventHandler(this.tabTariffManagement_SelectedIndexChanged);
            // 
            // tabSingleTariff
            // 
            this.tabSingleTariff.Controls.Add(this.label13);
            this.tabSingleTariff.Controls.Add(this.groupBox1);
            this.tabSingleTariff.Controls.Add(this.comboHospitalType);
            this.tabSingleTariff.Controls.Add(this.picAddCode);
            this.tabSingleTariff.Controls.Add(this.txtTariffDesc);
            this.tabSingleTariff.Controls.Add(this.label7);
            this.tabSingleTariff.Controls.Add(this.txtTariffCode);
            this.tabSingleTariff.Controls.Add(this.label8);
            this.tabSingleTariff.Controls.Add(this.label9);
            this.tabSingleTariff.Location = new System.Drawing.Point(4, 22);
            this.tabSingleTariff.Name = "tabSingleTariff";
            this.tabSingleTariff.Padding = new System.Windows.Forms.Padding(3);
            this.tabSingleTariff.Size = new System.Drawing.Size(875, 554);
            this.tabSingleTariff.TabIndex = 1;
            this.tabSingleTariff.Text = "Add Single Tariff";
            this.tabSingleTariff.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(575, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(26, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Add";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer2);
            this.groupBox1.Location = new System.Drawing.Point(11, 362);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(856, 184);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 16);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.datagridSearchResults);
            this.splitContainer2.Size = new System.Drawing.Size(850, 165);
            this.splitContainer2.SplitterDistance = 32;
            this.splitContainer2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTariff);
            this.groupBox2.Controls.Add(this.comboTariffName);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.picCodeLookup);
            this.groupBox2.Controls.Add(this.txtDesc);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtCode);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.MaximumSize = new System.Drawing.Size(0, 90);
            this.groupBox2.MinimumSize = new System.Drawing.Size(0, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(850, 90);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filters";
            // 
            // lblTariff
            // 
            this.lblTariff.AutoSize = true;
            this.lblTariff.Location = new System.Drawing.Point(6, 67);
            this.lblTariff.Name = "lblTariff";
            this.lblTariff.Size = new System.Drawing.Size(31, 13);
            this.lblTariff.TabIndex = 35;
            this.lblTariff.Text = "Tariff";
            this.lblTariff.Visible = false;
            // 
            // comboTariffName
            // 
            this.comboTariffName.FormattingEnabled = true;
            this.comboTariffName.Location = new System.Drawing.Point(74, 64);
            this.comboTariffName.Name = "comboTariffName";
            this.comboTariffName.Size = new System.Drawing.Size(240, 21);
            this.comboTariffName.TabIndex = 34;
            this.comboTariffName.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(207, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(323, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "If the code is filled in, the search will only be performed on the code";
            // 
            // picCodeLookup
            // 
            this.picCodeLookup.BackColor = System.Drawing.Color.White;
            this.picCodeLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCodeLookup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCodeLookup.Image = global::Icondev.MedManage.Properties.Resources.Binoculars;
            this.picCodeLookup.Location = new System.Drawing.Point(806, 12);
            this.picCodeLookup.Name = "picCodeLookup";
            this.picCodeLookup.Size = new System.Drawing.Size(38, 34);
            this.picCodeLookup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCodeLookup.TabIndex = 29;
            this.picCodeLookup.TabStop = false;
            this.picCodeLookup.Click += new System.EventHandler(this.picCodeLookup_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(74, 38);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(240, 20);
            this.txtDesc.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 41);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(74, 12);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(127, 20);
            this.txtCode.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Code";
            // 
            // datagridSearchResults
            // 
            this.datagridSearchResults.AllowUserToAddRows = false;
            this.datagridSearchResults.AllowUserToDeleteRows = false;
            this.datagridSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridSearchResults.Location = new System.Drawing.Point(0, 0);
            this.datagridSearchResults.Name = "datagridSearchResults";
            this.datagridSearchResults.ReadOnly = true;
            this.datagridSearchResults.Size = new System.Drawing.Size(850, 129);
            this.datagridSearchResults.TabIndex = 0;
            // 
            // comboHospitalType
            // 
            this.comboHospitalType.FormattingEnabled = true;
            this.comboHospitalType.Location = new System.Drawing.Point(104, 6);
            this.comboHospitalType.Name = "comboHospitalType";
            this.comboHospitalType.Size = new System.Drawing.Size(441, 21);
            this.comboHospitalType.TabIndex = 33;
            // 
            // picAddCode
            // 
            this.picAddCode.BackColor = System.Drawing.Color.White;
            this.picAddCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAddCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddCode.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picAddCode.Location = new System.Drawing.Point(611, 6);
            this.picAddCode.Name = "picAddCode";
            this.picAddCode.Size = new System.Drawing.Size(39, 34);
            this.picAddCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddCode.TabIndex = 40;
            this.picAddCode.TabStop = false;
            this.picAddCode.Click += new System.EventHandler(this.picAddCode_Click);
            // 
            // txtTariffDesc
            // 
            this.txtTariffDesc.Location = new System.Drawing.Point(104, 65);
            this.txtTariffDesc.Multiline = true;
            this.txtTariffDesc.Name = "txtTariffDesc";
            this.txtTariffDesc.Size = new System.Drawing.Size(441, 119);
            this.txtTariffDesc.TabIndex = 39;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Tariff Description";
            // 
            // txtTariffCode
            // 
            this.txtTariffCode.Location = new System.Drawing.Point(104, 32);
            this.txtTariffCode.Name = "txtTariffCode";
            this.txtTariffCode.Size = new System.Drawing.Size(147, 20);
            this.txtTariffCode.TabIndex = 37;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Tariff Code";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Speciality";
            // 
            // tabNewPeriod
            // 
            this.tabNewPeriod.Controls.Add(this.splitContainer1);
            this.tabNewPeriod.Location = new System.Drawing.Point(4, 22);
            this.tabNewPeriod.Name = "tabNewPeriod";
            this.tabNewPeriod.Padding = new System.Windows.Forms.Padding(3);
            this.tabNewPeriod.Size = new System.Drawing.Size(875, 554);
            this.tabNewPeriod.TabIndex = 0;
            this.tabNewPeriod.Text = "Add New Tariff Period";
            this.tabNewPeriod.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.picAddCustomTariff);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.picNewPeriod);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.comboTariffPeriod);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.comboType);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.picNew);
            this.splitContainer1.Panel1.Controls.Add(this.label59);
            this.splitContainer1.Panel1.Controls.Add(this.picCancel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.comboSelectTariff);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridTariff);
            this.splitContainer1.Size = new System.Drawing.Size(869, 548);
            this.splitContainer1.SplitterDistance = 92;
            this.splitContainer1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(775, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Custom";
            // 
            // picAddCustomTariff
            // 
            this.picAddCustomTariff.BackColor = System.Drawing.Color.White;
            this.picAddCustomTariff.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAddCustomTariff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddCustomTariff.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picAddCustomTariff.Location = new System.Drawing.Point(823, 54);
            this.picAddCustomTariff.Name = "picAddCustomTariff";
            this.picAddCustomTariff.Size = new System.Drawing.Size(40, 32);
            this.picAddCustomTariff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddCustomTariff.TabIndex = 62;
            this.picAddCustomTariff.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Period";
            // 
            // picNewPeriod
            // 
            this.picNewPeriod.BackColor = System.Drawing.Color.White;
            this.picNewPeriod.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picNewPeriod.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picNewPeriod.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picNewPeriod.Location = new System.Drawing.Point(345, 37);
            this.picNewPeriod.Name = "picNewPeriod";
            this.picNewPeriod.Size = new System.Drawing.Size(24, 21);
            this.picNewPeriod.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picNewPeriod.TabIndex = 60;
            this.picNewPeriod.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Tariff Period";
            // 
            // comboTariffPeriod
            // 
            this.comboTariffPeriod.FormattingEnabled = true;
            this.comboTariffPeriod.Location = new System.Drawing.Point(83, 37);
            this.comboTariffPeriod.Name = "comboTariffPeriod";
            this.comboTariffPeriod.Size = new System.Drawing.Size(256, 21);
            this.comboTariffPeriod.TabIndex = 58;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 57;
            this.label4.Text = "Type";
            // 
            // comboType
            // 
            this.comboType.FormattingEnabled = true;
            this.comboType.Location = new System.Drawing.Point(83, 64);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(256, 21);
            this.comboType.TabIndex = 56;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(375, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Tariff";
            // 
            // picNew
            // 
            this.picNew.BackColor = System.Drawing.Color.White;
            this.picNew.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picNew.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picNew.Location = new System.Drawing.Point(345, 10);
            this.picNew.Name = "picNew";
            this.picNew.Size = new System.Drawing.Size(24, 21);
            this.picNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picNew.TabIndex = 54;
            this.picNew.TabStop = false;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(784, 18);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(33, 13);
            this.label59.TabIndex = 53;
            this.label59.Text = "Close";
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(823, 5);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(40, 33);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 52;
            this.picCancel.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Tariff";
            // 
            // comboSelectTariff
            // 
            this.comboSelectTariff.FormattingEnabled = true;
            this.comboSelectTariff.Location = new System.Drawing.Point(83, 10);
            this.comboSelectTariff.Name = "comboSelectTariff";
            this.comboSelectTariff.Size = new System.Drawing.Size(256, 21);
            this.comboSelectTariff.TabIndex = 0;
            // 
            // datagridTariff
            // 
            this.datagridTariff.AllowUserToAddRows = false;
            this.datagridTariff.AllowUserToDeleteRows = false;
            this.datagridTariff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridTariff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridTariff.Location = new System.Drawing.Point(0, 0);
            this.datagridTariff.Name = "datagridTariff";
            this.datagridTariff.Size = new System.Drawing.Size(869, 452);
            this.datagridTariff.TabIndex = 0;
            // 
            // TariffManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 580);
            this.Controls.Add(this.tabTariffManagement);
            this.Name = "TariffManagement";
            this.Text = "TariffManagement";
            this.tabTariffManagement.ResumeLayout(false);
            this.tabSingleTariff.ResumeLayout(false);
            this.tabSingleTariff.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCodeLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddCode)).EndInit();
            this.tabNewPeriod.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAddCustomTariff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNewPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridTariff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabTariffManagement;
        private System.Windows.Forms.TabPage tabSingleTariff;
        private System.Windows.Forms.TabPage tabNewPeriod;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox picAddCustomTariff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picNewPeriod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboTariffPeriod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picNew;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.PictureBox picCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboSelectTariff;
        private System.Windows.Forms.DataGridView datagridTariff;
        private System.Windows.Forms.PictureBox picAddCode;
        private System.Windows.Forms.TextBox txtTariffDesc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTariffCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTariff;
        private System.Windows.Forms.ComboBox comboTariffName;
        private System.Windows.Forms.ComboBox comboHospitalType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox picCodeLookup;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView datagridSearchResults;
        private System.Windows.Forms.Label label13;

    }
}
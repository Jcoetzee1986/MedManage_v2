namespace Icondev.MedManage
{
    partial class CustomiseServiceProviderTariff
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
            this.comboHospitalType = new System.Windows.Forms.ComboBox();
            this.datagridSearchResults = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speciality = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchemeRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProviderRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TariffID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTariffEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.txtTariffCode = new System.Windows.Forms.TextBox();
            this.txtTariffAmount = new System.Windows.Forms.TextBox();
            this.btnSavePercentage = new System.Windows.Forms.Button();
            this.btnSaveSpecificTariffAmount = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTariffAmount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTariffCode = new System.Windows.Forms.Label();
            this.txtPercentageAdded = new System.Windows.Forms.TextBox();
            this.btnImportFromFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chkAllClients = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboSelectClient = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.comboSelectBaseTariff = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboHospitalType
            // 
            this.comboHospitalType.FormattingEnabled = true;
            this.comboHospitalType.Location = new System.Drawing.Point(88, 23);
            this.comboHospitalType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboHospitalType.Name = "comboHospitalType";
            this.comboHospitalType.Size = new System.Drawing.Size(333, 24);
            this.comboHospitalType.TabIndex = 33;
            this.comboHospitalType.SelectedIndexChanged += new System.EventHandler(this.comboHospitalType_SelectedIndexChanged);
            // 
            // datagridSearchResults
            // 
            this.datagridSearchResults.AllowUserToAddRows = false;
            this.datagridSearchResults.AllowUserToDeleteRows = false;
            this.datagridSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Speciality,
            this.Qty,
            this.Units,
            this.SchemeRate,
            this.ProviderRate,
            this.Description,
            this.StartDate,
            this.EndDate,
            this.TariffID});
            this.datagridSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridSearchResults.Location = new System.Drawing.Point(0, 0);
            this.datagridSearchResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.datagridSearchResults.Name = "datagridSearchResults";
            this.datagridSearchResults.Size = new System.Drawing.Size(1265, 487);
            this.datagridSearchResults.TabIndex = 0;
            this.datagridSearchResults.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridSearchResults_CellEndEdit);
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            // 
            // Speciality
            // 
            this.Speciality.DataPropertyName = "Speciality";
            this.Speciality.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.Speciality.HeaderText = "Speciality";
            this.Speciality.Name = "Speciality";
            this.Speciality.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.DataPropertyName = "Qty";
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            // 
            // Units
            // 
            this.Units.DataPropertyName = "Units";
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            // 
            // SchemeRate
            // 
            this.SchemeRate.DataPropertyName = "SchemeRate";
            this.SchemeRate.HeaderText = "DefaultRate";
            this.SchemeRate.Name = "SchemeRate";
            this.SchemeRate.ReadOnly = true;
            // 
            // ProviderRate
            // 
            this.ProviderRate.DataPropertyName = "ProviderRate";
            this.ProviderRate.HeaderText = "ProviderRate";
            this.ProviderRate.Name = "ProviderRate";
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.HeaderText = "StartDate";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            // 
            // EndDate
            // 
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.HeaderText = "EndDate";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            // 
            // TariffID
            // 
            this.TariffID.DataPropertyName = "TariffID";
            this.TariffID.HeaderText = "TariffID";
            this.TariffID.Name = "TariffID";
            this.TariffID.ReadOnly = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridSearchResults);
            this.splitContainer1.Size = new System.Drawing.Size(1265, 612);
            this.splitContainer1.SplitterDistance = 120;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTariffEffectiveDate);
            this.groupBox1.Controls.Add(this.txtTariffCode);
            this.groupBox1.Controls.Add(this.txtTariffAmount);
            this.groupBox1.Controls.Add(this.btnSavePercentage);
            this.groupBox1.Controls.Add(this.btnSaveSpecificTariffAmount);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblTariffAmount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblTariffCode);
            this.groupBox1.Controls.Add(this.txtPercentageAdded);
            this.groupBox1.Controls.Add(this.btnImportFromFile);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkAllClients);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboSelectClient);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.comboSelectBaseTariff);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboHospitalType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.MaximumSize = new System.Drawing.Size(0, 123);
            this.groupBox1.MinimumSize = new System.Drawing.Size(0, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1265, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // dateTariffEffectiveDate
            // 
            this.dateTariffEffectiveDate.CustomFormat = "yyyy/MM/dd";
            this.dateTariffEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTariffEffectiveDate.Location = new System.Drawing.Point(559, 23);
            this.dateTariffEffectiveDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTariffEffectiveDate.Name = "dateTariffEffectiveDate";
            this.dateTariffEffectiveDate.Size = new System.Drawing.Size(160, 22);
            this.dateTariffEffectiveDate.TabIndex = 69;
            this.dateTariffEffectiveDate.ValueChanged += new System.EventHandler(this.dateTariffEffectiveDate_ValueChanged);
            // 
            // txtTariffCode
            // 
            this.txtTariffCode.Location = new System.Drawing.Point(104, 89);
            this.txtTariffCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTariffCode.Name = "txtTariffCode";
            this.txtTariffCode.Size = new System.Drawing.Size(89, 22);
            this.txtTariffCode.TabIndex = 60;
            // 
            // txtTariffAmount
            // 
            this.txtTariffAmount.Location = new System.Drawing.Point(308, 89);
            this.txtTariffAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTariffAmount.Name = "txtTariffAmount";
            this.txtTariffAmount.Size = new System.Drawing.Size(89, 22);
            this.txtTariffAmount.TabIndex = 62;
            // 
            // btnSavePercentage
            // 
            this.btnSavePercentage.Location = new System.Drawing.Point(981, 89);
            this.btnSavePercentage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSavePercentage.Name = "btnSavePercentage";
            this.btnSavePercentage.Size = new System.Drawing.Size(233, 28);
            this.btnSavePercentage.TabIndex = 68;
            this.btnSavePercentage.Text = "Save BaseTariff and Percentage";
            this.btnSavePercentage.UseVisualStyleBackColor = true;
            this.btnSavePercentage.Click += new System.EventHandler(this.btnSavePercentage_Click);
            // 
            // btnSaveSpecificTariffAmount
            // 
            this.btnSaveSpecificTariffAmount.Location = new System.Drawing.Point(428, 86);
            this.btnSaveSpecificTariffAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveSpecificTariffAmount.Name = "btnSaveSpecificTariffAmount";
            this.btnSaveSpecificTariffAmount.Size = new System.Drawing.Size(100, 28);
            this.btnSaveSpecificTariffAmount.TabIndex = 64;
            this.btnSaveSpecificTariffAmount.Text = "Save";
            this.btnSaveSpecificTariffAmount.UseVisualStyleBackColor = true;
            this.btnSaveSpecificTariffAmount.Click += new System.EventHandler(this.btnSaveSpecificTariffAmount_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1219, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 20);
            this.label6.TabIndex = 67;
            this.label6.Text = "%";
            // 
            // lblTariffAmount
            // 
            this.lblTariffAmount.AutoSize = true;
            this.lblTariffAmount.Location = new System.Drawing.Point(211, 95);
            this.lblTariffAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTariffAmount.Name = "lblTariffAmount";
            this.lblTariffAmount.Size = new System.Drawing.Size(89, 17);
            this.lblTariffAmount.TabIndex = 63;
            this.lblTariffAmount.Text = "TariffAmount";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1139, 59);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 20);
            this.label5.TabIndex = 66;
            this.label5.Text = "+";
            // 
            // lblTariffCode
            // 
            this.lblTariffCode.AutoSize = true;
            this.lblTariffCode.Location = new System.Drawing.Point(21, 95);
            this.lblTariffCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTariffCode.Name = "lblTariffCode";
            this.lblTariffCode.Size = new System.Drawing.Size(74, 17);
            this.lblTariffCode.TabIndex = 61;
            this.lblTariffCode.Text = "TariffCode";
            // 
            // txtPercentageAdded
            // 
            this.txtPercentageAdded.Location = new System.Drawing.Point(1164, 59);
            this.txtPercentageAdded.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPercentageAdded.Name = "txtPercentageAdded";
            this.txtPercentageAdded.Size = new System.Drawing.Size(49, 22);
            this.txtPercentageAdded.TabIndex = 65;
            // 
            // btnImportFromFile
            // 
            this.btnImportFromFile.Location = new System.Drawing.Point(585, 86);
            this.btnImportFromFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportFromFile.Name = "btnImportFromFile";
            this.btnImportFromFile.Size = new System.Drawing.Size(135, 28);
            this.btnImportFromFile.TabIndex = 59;
            this.btnImportFromFile.Text = "Import From File";
            this.btnImportFromFile.UseVisualStyleBackColor = true;
            this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(16, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(289, 17);
            this.label4.TabIndex = 58;
            this.label4.Text = "Tariff Amounts here are excluding VAT";
            // 
            // chkAllClients
            // 
            this.chkAllClients.AutoSize = true;
            this.chkAllClients.Location = new System.Drawing.Point(1144, 27);
            this.chkAllClients.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAllClients.Name = "chkAllClients";
            this.chkAllClients.Size = new System.Drawing.Size(45, 21);
            this.chkAllClients.TabIndex = 57;
            this.chkAllClients.Text = "All";
            this.chkAllClients.UseVisualStyleBackColor = true;
            this.chkAllClients.CheckedChanged += new System.EventHandler(this.chkAllClients_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(747, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 56;
            this.label3.Text = "Select Client";
            // 
            // comboSelectClient
            // 
            this.comboSelectClient.FormattingEnabled = true;
            this.comboSelectClient.Location = new System.Drawing.Point(876, 25);
            this.comboSelectClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboSelectClient.Name = "comboSelectClient";
            this.comboSelectClient.Size = new System.Drawing.Size(259, 24);
            this.comboSelectClient.TabIndex = 55;
            this.comboSelectClient.SelectedIndexChanged += new System.EventHandler(this.comboSelectClient_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(747, 62);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(120, 17);
            this.label30.TabIndex = 54;
            this.label30.Text = "Select Base Tariff";
            // 
            // comboSelectBaseTariff
            // 
            this.comboSelectBaseTariff.FormattingEnabled = true;
            this.comboSelectBaseTariff.Location = new System.Drawing.Point(876, 58);
            this.comboSelectBaseTariff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboSelectBaseTariff.Name = "comboSelectBaseTariff";
            this.comboSelectBaseTariff.Size = new System.Drawing.Size(259, 24);
            this.comboSelectBaseTariff.TabIndex = 53;
            this.comboSelectBaseTariff.SelectedIndexChanged += new System.EventHandler(this.comboSelectBaseTariff_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(449, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 49;
            this.label1.Text = "Effective Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 34;
            this.label2.Text = "Speciality";
            // 
            // CustomiseServiceProviderTariff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 612);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CustomiseServiceProviderTariff";
            this.Text = "CustomiseServiceProviderTariff";
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboHospitalType;
        private System.Windows.Forms.DataGridView datagridSearchResults;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewComboBoxColumn Speciality;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchemeRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProviderRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TariffID;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ComboBox comboSelectBaseTariff;
        private System.Windows.Forms.CheckBox chkAllClients;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboSelectClient;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnImportFromFile;
        private System.Windows.Forms.Button btnSaveSpecificTariffAmount;
        private System.Windows.Forms.Label lblTariffAmount;
        private System.Windows.Forms.TextBox txtTariffAmount;
        private System.Windows.Forms.Label lblTariffCode;
        private System.Windows.Forms.TextBox txtTariffCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPercentageAdded;
        private System.Windows.Forms.Button btnSavePercentage;
        private System.Windows.Forms.DateTimePicker dateTariffEffectiveDate;
    }
}
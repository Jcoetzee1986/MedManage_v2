namespace Icondev.MedManage
{
    partial class TariffCodeLookup
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTariff = new System.Windows.Forms.Label();
            this.comboTariffName = new System.Windows.Forms.ComboBox();
            this.comboHospitalType = new System.Windows.Forms.ComboBox();
            this.picSelectCode = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.datagridSearchResults = new System.Windows.Forms.DataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Speciality = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchemeRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectCode)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTariff);
            this.groupBox1.Controls.Add(this.comboTariffName);
            this.groupBox1.Controls.Add(this.comboHospitalType);
            this.groupBox1.Controls.Add(this.picSelectCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.MaximumSize = new System.Drawing.Size(0, 50);
            this.groupBox1.MinimumSize = new System.Drawing.Size(0, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(754, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // lblTariff
            // 
            this.lblTariff.AutoSize = true;
            this.lblTariff.Location = new System.Drawing.Point(6, 22);
            this.lblTariff.Name = "lblTariff";
            this.lblTariff.Size = new System.Drawing.Size(31, 13);
            this.lblTariff.TabIndex = 35;
            this.lblTariff.Text = "Tariff";
            // 
            // comboTariffName
            // 
            this.comboTariffName.Enabled = false;
            this.comboTariffName.FormattingEnabled = true;
            this.comboTariffName.Location = new System.Drawing.Point(74, 19);
            this.comboTariffName.Name = "comboTariffName";
            this.comboTariffName.Size = new System.Drawing.Size(240, 21);
            this.comboTariffName.TabIndex = 34;
            // 
            // comboHospitalType
            // 
            this.comboHospitalType.Enabled = false;
            this.comboHospitalType.FormattingEnabled = true;
            this.comboHospitalType.Location = new System.Drawing.Point(334, 18);
            this.comboHospitalType.Name = "comboHospitalType";
            this.comboHospitalType.Size = new System.Drawing.Size(240, 21);
            this.comboHospitalType.TabIndex = 33;
            this.comboHospitalType.SelectedIndexChanged += new System.EventHandler(this.comboHospitalType_SelectedIndexChanged);
            // 
            // picSelectCode
            // 
            this.picSelectCode.BackColor = System.Drawing.Color.White;
            this.picSelectCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSelectCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelectCode.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSelectCode.Location = new System.Drawing.Point(707, 12);
            this.picSelectCode.Name = "picSelectCode";
            this.picSelectCode.Size = new System.Drawing.Size(39, 34);
            this.picSelectCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSelectCode.TabIndex = 31;
            this.picSelectCode.TabStop = false;
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridSearchResults);
            this.splitContainer1.Size = new System.Drawing.Size(754, 618);
            this.splitContainer1.SplitterDistance = 51;
            this.splitContainer1.TabIndex = 3;
            // 
            // datagridSearchResults
            // 
            this.datagridSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Speciality,
            this.Qty,
            this.Units,
            this.Rate,
            this.SchemeRate,
            this.Description});
            this.datagridSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridSearchResults.Location = new System.Drawing.Point(0, 0);
            this.datagridSearchResults.Name = "datagridSearchResults";
            this.datagridSearchResults.Size = new System.Drawing.Size(754, 563);
            this.datagridSearchResults.TabIndex = 0;
            this.datagridSearchResults.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridSearchResults_CellValueChanged);
            this.datagridSearchResults.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.datagridSearchResults_RowsAdded);
            this.datagridSearchResults.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.datagridSearchResults_RowsRemoved);
            // 
            // Code
            // 
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            // 
            // Speciality
            // 
            this.Speciality.HeaderText = "Speciality";
            this.Speciality.Name = "Speciality";
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            // 
            // Units
            // 
            this.Units.HeaderText = "Units";
            this.Units.Name = "Units";
            this.Units.ReadOnly = true;
            // 
            // Rate
            // 
            this.Rate.HeaderText = "Rate";
            this.Rate.Name = "Rate";
            // 
            // SchemeRate
            // 
            this.SchemeRate.HeaderText = "SchemeRate";
            this.SchemeRate.Name = "SchemeRate";
            this.SchemeRate.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // TariffCodeLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 618);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TariffCodeLookup";
            this.Text = "TariffCodeLookup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectCode)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTariff;
        private System.Windows.Forms.ComboBox comboTariffName;
        private System.Windows.Forms.ComboBox comboHospitalType;
        private System.Windows.Forms.PictureBox picSelectCode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView datagridSearchResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewComboBoxColumn Speciality;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Units;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchemeRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
    }
}
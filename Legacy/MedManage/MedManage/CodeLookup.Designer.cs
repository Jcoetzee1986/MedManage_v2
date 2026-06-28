namespace Icondev.MedManage
{
    partial class CodeLookup
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
            this.label1 = new System.Windows.Forms.Label();
            this.picSelectCode = new System.Windows.Forms.PictureBox();
            this.picCodeLookup = new System.Windows.Forms.PictureBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.datagridSearchResults = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCodeLookup)).BeginInit();
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
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.picSelectCode);
            this.groupBox1.Controls.Add(this.picCodeLookup);
            this.groupBox1.Controls.Add(this.txtDesc);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.MaximumSize = new System.Drawing.Size(0, 90);
            this.groupBox1.MinimumSize = new System.Drawing.Size(0, 90);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(753, 90);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
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
            // comboHospitalType
            // 
            this.comboHospitalType.FormattingEnabled = true;
            this.comboHospitalType.Location = new System.Drawing.Point(334, 63);
            this.comboHospitalType.Name = "comboHospitalType";
            this.comboHospitalType.Size = new System.Drawing.Size(240, 21);
            this.comboHospitalType.TabIndex = 33;
            this.comboHospitalType.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "If the code is filled in, the search will only be performed on the code";
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
            this.picSelectCode.Click += new System.EventHandler(this.picSelectCode_Click);
            // 
            // picCodeLookup
            // 
            this.picCodeLookup.BackColor = System.Drawing.Color.White;
            this.picCodeLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCodeLookup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCodeLookup.Image = global::Icondev.MedManage.Properties.Resources.Binoculars;
            this.picCodeLookup.Location = new System.Drawing.Point(654, 12);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(74, 12);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(127, 20);
            this.txtCode.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Code";
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
            this.splitContainer1.Size = new System.Drawing.Size(753, 570);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.TabIndex = 2;
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
            this.datagridSearchResults.Size = new System.Drawing.Size(753, 476);
            this.datagridSearchResults.TabIndex = 0;
            this.datagridSearchResults.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datagridSearchResults_CellMouseDoubleClick);
            // 
            // CodeLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(753, 570);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CodeLookup";
            this.Text = "Code Lookup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCodeLookup)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridSearchResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSelectCode;
        private System.Windows.Forms.PictureBox picCodeLookup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView datagridSearchResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboHospitalType;
        private System.Windows.Forms.Label lblTariff;
        private System.Windows.Forms.ComboBox comboTariffName;
    }
}
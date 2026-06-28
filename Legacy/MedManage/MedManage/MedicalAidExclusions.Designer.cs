namespace Icondev.MedManage
{
    partial class MedicalAidExclusions
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picSaveByHospitalType = new System.Windows.Forms.PictureBox();
            this.comboHospitalGroup = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboExclusion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picRemove = new System.Windows.Forms.PictureBox();
            this.comboMedicalAid = new System.Windows.Forms.ComboBox();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.datagridMedicalAidExclusions = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSaveByHospitalType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridMedicalAidExclusions)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.picSaveByHospitalType);
            this.splitContainer1.Panel1.Controls.Add(this.comboHospitalGroup);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.comboExclusion);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.picRemove);
            this.splitContainer1.Panel1.Controls.Add(this.comboMedicalAid);
            this.splitContainer1.Panel1.Controls.Add(this.picSave);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridMedicalAidExclusions);
            this.splitContainer1.Size = new System.Drawing.Size(454, 391);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 0;
            // 
            // picSaveByHospitalType
            // 
            this.picSaveByHospitalType.BackColor = System.Drawing.Color.White;
            this.picSaveByHospitalType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSaveByHospitalType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSaveByHospitalType.Image = global::Icondev.MedManage.Properties.Resources.down_64;
            this.picSaveByHospitalType.Location = new System.Drawing.Point(333, 65);
            this.picSaveByHospitalType.Name = "picSaveByHospitalType";
            this.picSaveByHospitalType.Size = new System.Drawing.Size(23, 19);
            this.picSaveByHospitalType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSaveByHospitalType.TabIndex = 36;
            this.picSaveByHospitalType.TabStop = false;
            this.picSaveByHospitalType.Click += new System.EventHandler(this.picSaveByHospitalType_Click);
            // 
            // comboHospitalGroup
            // 
            this.comboHospitalGroup.FormattingEnabled = true;
            this.comboHospitalGroup.Location = new System.Drawing.Point(12, 65);
            this.comboHospitalGroup.Name = "comboHospitalGroup";
            this.comboHospitalGroup.Size = new System.Drawing.Size(315, 21);
            this.comboHospitalGroup.TabIndex = 35;
            this.comboHospitalGroup.SelectedIndexChanged += new System.EventHandler(this.comboHospitalGroup_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Select Hospital Group";
            // 
            // comboExclusion
            // 
            this.comboExclusion.FormattingEnabled = true;
            this.comboExclusion.Location = new System.Drawing.Point(12, 107);
            this.comboExclusion.Name = "comboExclusion";
            this.comboExclusion.Size = new System.Drawing.Size(315, 21);
            this.comboExclusion.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Select Tariff Exclusion";
            // 
            // picRemove
            // 
            this.picRemove.BackColor = System.Drawing.Color.White;
            this.picRemove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picRemove.Image = global::Icondev.MedManage.Properties.Resources.DeleteRed;
            this.picRemove.Location = new System.Drawing.Point(420, 109);
            this.picRemove.Name = "picRemove";
            this.picRemove.Size = new System.Drawing.Size(22, 19);
            this.picRemove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRemove.TabIndex = 30;
            this.picRemove.TabStop = false;
            this.picRemove.Click += new System.EventHandler(this.picRemove_Click);
            // 
            // comboMedicalAid
            // 
            this.comboMedicalAid.FormattingEnabled = true;
            this.comboMedicalAid.Location = new System.Drawing.Point(12, 25);
            this.comboMedicalAid.Name = "comboMedicalAid";
            this.comboMedicalAid.Size = new System.Drawing.Size(315, 21);
            this.comboMedicalAid.TabIndex = 29;
            this.comboMedicalAid.SelectedIndexChanged += new System.EventHandler(this.comboMedicalAid_SelectedIndexChanged);
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.White;
            this.picSave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = global::Icondev.MedManage.Properties.Resources.down_64;
            this.picSave.Location = new System.Drawing.Point(333, 109);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(23, 19);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 25;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Medical Aid";
            // 
            // datagridMedicalAidExclusions
            // 
            this.datagridMedicalAidExclusions.AllowUserToAddRows = false;
            this.datagridMedicalAidExclusions.AllowUserToDeleteRows = false;
            this.datagridMedicalAidExclusions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridMedicalAidExclusions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridMedicalAidExclusions.Location = new System.Drawing.Point(0, 0);
            this.datagridMedicalAidExclusions.Name = "datagridMedicalAidExclusions";
            this.datagridMedicalAidExclusions.ReadOnly = true;
            this.datagridMedicalAidExclusions.Size = new System.Drawing.Size(454, 246);
            this.datagridMedicalAidExclusions.TabIndex = 0;
            this.datagridMedicalAidExclusions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridMedicalAidExclusions_CellContentClick);
            // 
            // MedicalAidExclusions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 391);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MedicalAidExclusions";
            this.Text = "MedicalAidExclusions";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSaveByHospitalType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridMedicalAidExclusions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView datagridMedicalAidExclusions;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.ComboBox comboExclusion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picRemove;
        private System.Windows.Forms.ComboBox comboMedicalAid;
        private System.Windows.Forms.ComboBox comboHospitalGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picSaveByHospitalType;
    }
}
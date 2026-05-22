namespace Icondev.MedManage
{
    partial class ChronicIllnesses
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
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtChronicIllnessDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChronicIllnessName = new System.Windows.Forms.TextBox();
            this.datadridChronicIllness = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datadridChronicIllness)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.picCancel);
            this.splitContainer1.Panel1.Controls.Add(this.picSave);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtChronicIllnessDescription);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtChronicIllnessName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datadridChronicIllness);
            this.splitContainer1.Size = new System.Drawing.Size(619, 575);
            this.splitContainer1.SplitterDistance = 124;
            this.splitContainer1.TabIndex = 0;
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(576, 88);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(40, 33);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 27;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.White;
            this.picSave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSave.Location = new System.Drawing.Point(576, 3);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(40, 33);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 23;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chrinic Illness Description";
            // 
            // txtChronicIllnessDescription
            // 
            this.txtChronicIllnessDescription.Location = new System.Drawing.Point(145, 32);
            this.txtChronicIllnessDescription.Multiline = true;
            this.txtChronicIllnessDescription.Name = "txtChronicIllnessDescription";
            this.txtChronicIllnessDescription.Size = new System.Drawing.Size(408, 89);
            this.txtChronicIllnessDescription.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Chrinic Illness Name";
            // 
            // txtChronicIllnessName
            // 
            this.txtChronicIllnessName.Location = new System.Drawing.Point(145, 3);
            this.txtChronicIllnessName.Name = "txtChronicIllnessName";
            this.txtChronicIllnessName.Size = new System.Drawing.Size(408, 20);
            this.txtChronicIllnessName.TabIndex = 0;
            // 
            // datadridChronicIllness
            // 
            this.datadridChronicIllness.AllowUserToAddRows = false;
            this.datadridChronicIllness.AllowUserToDeleteRows = false;
            this.datadridChronicIllness.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datadridChronicIllness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datadridChronicIllness.Location = new System.Drawing.Point(0, 0);
            this.datadridChronicIllness.Name = "datadridChronicIllness";
            this.datadridChronicIllness.ReadOnly = true;
            this.datadridChronicIllness.Size = new System.Drawing.Size(619, 447);
            this.datadridChronicIllness.TabIndex = 0;
            // 
            // ChronicIllnesses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChronicIllnesses";
            this.Text = "ChronicIllnesses";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datadridChronicIllness)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChronicIllnessDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtChronicIllnessName;
        private System.Windows.Forms.DataGridView datadridChronicIllness;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.PictureBox picCancel;
    }
}
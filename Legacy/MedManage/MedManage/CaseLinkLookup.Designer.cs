namespace Icondev.MedManage
{
    partial class CaseLinkLookup
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
            this.label41 = new System.Windows.Forms.Label();
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.label55 = new System.Windows.Forms.Label();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.txtAuthNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdCaseLinkLookup = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCaseLinkLookup)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.label41);
            this.splitContainer1.Panel1.Controls.Add(this.picCancel);
            this.splitContainer1.Panel1.Controls.Add(this.label55);
            this.splitContainer1.Panel1.Controls.Add(this.picSave);
            this.splitContainer1.Panel1.Controls.Add(this.txtAuthNumber);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdCaseLinkLookup);
            this.splitContainer1.Size = new System.Drawing.Size(591, 421);
            this.splitContainer1.SplitterDistance = 58;
            this.splitContainer1.TabIndex = 0;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(375, 23);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(40, 13);
            this.label41.TabIndex = 35;
            this.label41.Text = "Cancel";
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(419, 10);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(40, 33);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 34;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(501, 23);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(27, 13);
            this.label55.TabIndex = 33;
            this.label55.Text = "Link";
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.White;
            this.picSave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSave.Location = new System.Drawing.Point(539, 10);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(40, 33);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 32;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // txtAuthNumber
            // 
            this.txtAuthNumber.BackColor = System.Drawing.Color.White;
            this.txtAuthNumber.Enabled = false;
            this.txtAuthNumber.Location = new System.Drawing.Point(85, 10);
            this.txtAuthNumber.Name = "txtAuthNumber";
            this.txtAuthNumber.Size = new System.Drawing.Size(157, 20);
            this.txtAuthNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Tag = "";
            this.label1.Text = "AuthNumber";
            // 
            // grdCaseLinkLookup
            // 
            this.grdCaseLinkLookup.AllowUserToAddRows = false;
            this.grdCaseLinkLookup.AllowUserToDeleteRows = false;
            this.grdCaseLinkLookup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCaseLinkLookup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCaseLinkLookup.Location = new System.Drawing.Point(0, 0);
            this.grdCaseLinkLookup.Name = "grdCaseLinkLookup";
            this.grdCaseLinkLookup.ReadOnly = true;
            this.grdCaseLinkLookup.Size = new System.Drawing.Size(591, 359);
            this.grdCaseLinkLookup.TabIndex = 0;
            this.grdCaseLinkLookup.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCaseLinkLookup_CellClick);
            // 
            // CaseLinkLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 421);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CaseLinkLookup";
            this.Text = "CaseLinkLookup";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCaseLinkLookup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtAuthNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdCaseLinkLookup;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.PictureBox picCancel;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.PictureBox picSave;
    }
}
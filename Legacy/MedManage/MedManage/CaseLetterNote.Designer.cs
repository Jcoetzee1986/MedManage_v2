namespace Icondev.MedManage
{
    partial class CaseLetterNote
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
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picCaseNoteAdd = new System.Windows.Forms.PictureBox();
            this.chkIncludeDischarge = new System.Windows.Forms.CheckBox();
            this.chkDownReferral = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCaseNoteAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(12, 31);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNote.Size = new System.Drawing.Size(665, 212);
            this.txtNote.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please add your note below:";
            // 
            // picCaseNoteAdd
            // 
            this.picCaseNoteAdd.BackColor = System.Drawing.Color.White;
            this.picCaseNoteAdd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCaseNoteAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCaseNoteAdd.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picCaseNoteAdd.Location = new System.Drawing.Point(637, 250);
            this.picCaseNoteAdd.Name = "picCaseNoteAdd";
            this.picCaseNoteAdd.Size = new System.Drawing.Size(40, 33);
            this.picCaseNoteAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCaseNoteAdd.TabIndex = 23;
            this.picCaseNoteAdd.TabStop = false;
            this.picCaseNoteAdd.Click += new System.EventHandler(this.picCaseNoteAdd_Click);
            // 
            // chkIncludeDischarge
            // 
            this.chkIncludeDischarge.AutoSize = true;
            this.chkIncludeDischarge.Location = new System.Drawing.Point(492, 260);
            this.chkIncludeDischarge.Name = "chkIncludeDischarge";
            this.chkIncludeDischarge.Size = new System.Drawing.Size(138, 17);
            this.chkIncludeDischarge.TabIndex = 24;
            this.chkIncludeDischarge.Text = "Include Discharge Form";
            this.chkIncludeDischarge.UseVisualStyleBackColor = true;
            // 
            // chkDownReferral
            // 
            this.chkDownReferral.AutoSize = true;
            this.chkDownReferral.Location = new System.Drawing.Point(316, 260);
            this.chkDownReferral.Name = "chkDownReferral";
            this.chkDownReferral.Size = new System.Drawing.Size(158, 17);
            this.chkDownReferral.TabIndex = 25;
            this.chkDownReferral.Text = "Include Down Referral Form";
            this.chkDownReferral.UseVisualStyleBackColor = true;
            // 
            // CaseLetterNote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 295);
            this.Controls.Add(this.chkDownReferral);
            this.Controls.Add(this.chkIncludeDischarge);
            this.Controls.Add(this.picCaseNoteAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNote);
            this.Name = "CaseLetterNote";
            this.Text = "Letter Note";
            ((System.ComponentModel.ISupportInitialize)(this.picCaseNoteAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picCaseNoteAdd;
        private System.Windows.Forms.CheckBox chkIncludeDischarge;
        private System.Windows.Forms.CheckBox chkDownReferral;
    }
}
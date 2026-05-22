namespace Icondev.MedManage
{
    partial class CopyCaseDialog
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
            this.chkUseSameAuthNumber = new System.Windows.Forms.CheckBox();
            this.chkCopyICD10 = new System.Windows.Forms.CheckBox();
            this.chkCopyCpt = new System.Windows.Forms.CheckBox();
            this.chkCopyTariff = new System.Windows.Forms.CheckBox();
            this.chkLinkToParentCase = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.label55 = new System.Windows.Forms.Label();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.chkCopyDaysInCare = new System.Windows.Forms.CheckBox();
            this.dateAdmissionDate = new System.Windows.Forms.DateTimePicker();
            this.label25 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            this.SuspendLayout();
            // 
            // chkUseSameAuthNumber
            // 
            this.chkUseSameAuthNumber.AutoSize = true;
            this.chkUseSameAuthNumber.Location = new System.Drawing.Point(16, 15);
            this.chkUseSameAuthNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseSameAuthNumber.Name = "chkUseSameAuthNumber";
            this.chkUseSameAuthNumber.Size = new System.Drawing.Size(478, 21);
            this.chkUseSameAuthNumber.TabIndex = 1;
            this.chkUseSameAuthNumber.Text = "Use the same auth number (Will create a link to the case automatically)";
            this.chkUseSameAuthNumber.UseVisualStyleBackColor = true;
            this.chkUseSameAuthNumber.CheckedChanged += new System.EventHandler(this.chkUseSameAuthNumber_CheckedChanged);
            // 
            // chkCopyICD10
            // 
            this.chkCopyICD10.AutoSize = true;
            this.chkCopyICD10.Location = new System.Drawing.Point(15, 73);
            this.chkCopyICD10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCopyICD10.Name = "chkCopyICD10";
            this.chkCopyICD10.Size = new System.Drawing.Size(178, 21);
            this.chkCopyICD10.TabIndex = 2;
            this.chkCopyICD10.Text = "Copy ICD10 information";
            this.chkCopyICD10.UseVisualStyleBackColor = true;
            // 
            // chkCopyCpt
            // 
            this.chkCopyCpt.AutoSize = true;
            this.chkCopyCpt.Location = new System.Drawing.Point(15, 101);
            this.chkCopyCpt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCopyCpt.Name = "chkCopyCpt";
            this.chkCopyCpt.Size = new System.Drawing.Size(167, 21);
            this.chkCopyCpt.TabIndex = 3;
            this.chkCopyCpt.Text = "Copy CPT information";
            this.chkCopyCpt.UseVisualStyleBackColor = true;
            // 
            // chkCopyTariff
            // 
            this.chkCopyTariff.AutoSize = true;
            this.chkCopyTariff.Location = new System.Drawing.Point(15, 129);
            this.chkCopyTariff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCopyTariff.Name = "chkCopyTariff";
            this.chkCopyTariff.Size = new System.Drawing.Size(173, 21);
            this.chkCopyTariff.TabIndex = 4;
            this.chkCopyTariff.Text = "Copy Tariff information";
            this.chkCopyTariff.UseVisualStyleBackColor = true;
            // 
            // chkLinkToParentCase
            // 
            this.chkLinkToParentCase.AutoSize = true;
            this.chkLinkToParentCase.Location = new System.Drawing.Point(16, 44);
            this.chkLinkToParentCase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkLinkToParentCase.Name = "chkLinkToParentCase";
            this.chkLinkToParentCase.Size = new System.Drawing.Size(177, 21);
            this.chkLinkToParentCase.TabIndex = 5;
            this.chkLinkToParentCase.Text = "Link to this parent case";
            this.chkLinkToParentCase.UseVisualStyleBackColor = true;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(13, 274);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(51, 17);
            this.label41.TabIndex = 33;
            this.label41.Text = "Cancel";
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(13, 230);
            this.picCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(52, 40);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 32;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(443, 274);
            this.label55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(40, 17);
            this.label55.TabIndex = 35;
            this.label55.Text = "Save";
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.White;
            this.picSave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSave.Location = new System.Drawing.Point(439, 230);
            this.picSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(52, 40);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 34;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // chkCopyDaysInCare
            // 
            this.chkCopyDaysInCare.AutoSize = true;
            this.chkCopyDaysInCare.Location = new System.Drawing.Point(15, 158);
            this.chkCopyDaysInCare.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkCopyDaysInCare.Name = "chkCopyDaysInCare";
            this.chkCopyDaysInCare.Size = new System.Drawing.Size(236, 21);
            this.chkCopyDaysInCare.TabIndex = 36;
            this.chkCopyDaysInCare.Text = "Copy facility types (Days in care)";
            this.chkCopyDaysInCare.UseVisualStyleBackColor = true;
            // 
            // dateAdmissionDate
            // 
            this.dateAdmissionDate.CustomFormat = "yyyy/MM/dd";
            this.dateAdmissionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAdmissionDate.Location = new System.Drawing.Point(126, 188);
            this.dateAdmissionDate.Margin = new System.Windows.Forms.Padding(4);
            this.dateAdmissionDate.Name = "dateAdmissionDate";
            this.dateAdmissionDate.Size = new System.Drawing.Size(125, 22);
            this.dateAdmissionDate.TabIndex = 37;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(12, 193);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(106, 17);
            this.label25.TabIndex = 38;
            this.label25.Text = "Admission Date";
            // 
            // CopyCaseDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 306);
            this.Controls.Add(this.dateAdmissionDate);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.chkCopyDaysInCare);
            this.Controls.Add(this.label55);
            this.Controls.Add(this.picSave);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.picCancel);
            this.Controls.Add(this.chkLinkToParentCase);
            this.Controls.Add(this.chkCopyTariff);
            this.Controls.Add(this.chkCopyCpt);
            this.Controls.Add(this.chkCopyICD10);
            this.Controls.Add(this.chkUseSameAuthNumber);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CopyCaseDialog";
            this.Text = "Copy Case";
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseSameAuthNumber;
        private System.Windows.Forms.CheckBox chkCopyICD10;
        private System.Windows.Forms.CheckBox chkCopyCpt;
        private System.Windows.Forms.CheckBox chkCopyTariff;
        private System.Windows.Forms.CheckBox chkLinkToParentCase;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.PictureBox picCancel;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.CheckBox chkCopyDaysInCare;
        private System.Windows.Forms.DateTimePicker dateAdmissionDate;
        private System.Windows.Forms.Label label25;
    }
}
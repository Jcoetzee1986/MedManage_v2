namespace Icondev.MedManage
{
    partial class NewTariffPeriod
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
            this.txtNewTariffPeriodName = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.label55 = new System.Windows.Forms.Label();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.dateEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateStartDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMultiplier = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNewTariffPeriodName
            // 
            this.txtNewTariffPeriodName.Location = new System.Drawing.Point(89, 6);
            this.txtNewTariffPeriodName.Name = "txtNewTariffPeriodName";
            this.txtNewTariffPeriodName.Size = new System.Drawing.Size(121, 20);
            this.txtNewTariffPeriodName.TabIndex = 70;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(238, 59);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(40, 13);
            this.label59.TabIndex = 69;
            this.label59.Text = "Cancel";
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(280, 46);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(40, 33);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 68;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(242, 19);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(32, 13);
            this.label55.TabIndex = 67;
            this.label55.Text = "Save";
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.White;
            this.picSave.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSave.Location = new System.Drawing.Point(280, 6);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(40, 33);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSave.TabIndex = 66;
            this.picSave.TabStop = false;
            this.picSave.Click += new System.EventHandler(this.picSave_Click);
            // 
            // dateEndDate
            // 
            this.dateEndDate.CustomFormat = "yyyy/MM/dd";
            this.dateEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateEndDate.Location = new System.Drawing.Point(89, 59);
            this.dateEndDate.Name = "dateEndDate";
            this.dateEndDate.Size = new System.Drawing.Size(121, 20);
            this.dateEndDate.TabIndex = 65;
            // 
            // dateStartDate
            // 
            this.dateStartDate.CustomFormat = "yyyy/MM/dd";
            this.dateStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateStartDate.Location = new System.Drawing.Point(89, 33);
            this.dateStartDate.Name = "dateStartDate";
            this.dateStartDate.Size = new System.Drawing.Size(121, 20);
            this.dateStartDate.TabIndex = 64;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 63;
            this.label3.Text = "End Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Start Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Period (YYYY)";
            // 
            // txtMultiplier
            // 
            this.txtMultiplier.Location = new System.Drawing.Point(89, 85);
            this.txtMultiplier.Name = "txtMultiplier";
            this.txtMultiplier.Size = new System.Drawing.Size(121, 20);
            this.txtMultiplier.TabIndex = 72;
            this.txtMultiplier.Text = "1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "Multiplier";
            // 
            // NewTariffPeriod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 129);
            this.Controls.Add(this.txtMultiplier);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNewTariffPeriodName);
            this.Controls.Add(this.label59);
            this.Controls.Add(this.picCancel);
            this.Controls.Add(this.label55);
            this.Controls.Add(this.picSave);
            this.Controls.Add(this.dateEndDate);
            this.Controls.Add(this.dateStartDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewTariffPeriod";
            this.Text = "NewTariffPeriod";
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNewTariffPeriodName;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.PictureBox picCancel;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.DateTimePicker dateEndDate;
        private System.Windows.Forms.DateTimePicker dateStartDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMultiplier;
        private System.Windows.Forms.Label label4;
    }
}
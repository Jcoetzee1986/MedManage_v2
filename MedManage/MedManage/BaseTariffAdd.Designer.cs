namespace Icondev.MedManage
{
    partial class BaseTariffAdd
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpeciality = new System.Windows.Forms.TextBox();
            this.txtTariffCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTariffDesc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picAddCode = new System.Windows.Forms.PictureBox();
            this.picCancel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAddCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Speciality";
            // 
            // txtSpeciality
            // 
            this.txtSpeciality.Enabled = false;
            this.txtSpeciality.Location = new System.Drawing.Point(100, 6);
            this.txtSpeciality.Name = "txtSpeciality";
            this.txtSpeciality.Size = new System.Drawing.Size(113, 20);
            this.txtSpeciality.TabIndex = 1;
            this.txtSpeciality.Text = "CUSTOM/OTHER";
            // 
            // txtTariffCode
            // 
            this.txtTariffCode.Enabled = false;
            this.txtTariffCode.Location = new System.Drawing.Point(100, 32);
            this.txtTariffCode.Name = "txtTariffCode";
            this.txtTariffCode.Size = new System.Drawing.Size(113, 20);
            this.txtTariffCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tariff Code";
            // 
            // txtTariffDesc
            // 
            this.txtTariffDesc.Location = new System.Drawing.Point(100, 58);
            this.txtTariffDesc.Multiline = true;
            this.txtTariffDesc.Name = "txtTariffDesc";
            this.txtTariffDesc.Size = new System.Drawing.Size(179, 81);
            this.txtTariffDesc.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tariff Description";
            // 
            // picAddCode
            // 
            this.picAddCode.BackColor = System.Drawing.Color.White;
            this.picAddCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAddCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddCode.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picAddCode.Location = new System.Drawing.Point(240, 6);
            this.picAddCode.Name = "picAddCode";
            this.picAddCode.Size = new System.Drawing.Size(39, 34);
            this.picAddCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddCode.TabIndex = 32;
            this.picAddCode.TabStop = false;
            this.picAddCode.Click += new System.EventHandler(this.picAddCode_Click);
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(240, 145);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(39, 34);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 33;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // BaseTariffAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 185);
            this.Controls.Add(this.picCancel);
            this.Controls.Add(this.picAddCode);
            this.Controls.Add(this.txtTariffDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTariffCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSpeciality);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BaseTariffAdd";
            this.Text = "BaseTariffAdd";
            ((System.ComponentModel.ISupportInitialize)(this.picAddCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpeciality;
        private System.Windows.Forms.TextBox txtTariffCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTariffDesc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picAddCode;
        private System.Windows.Forms.PictureBox picCancel;
    }
}
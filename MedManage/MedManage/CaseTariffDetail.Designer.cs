namespace Icondev.MedManage
{
    partial class CaseTariffDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.picReportCaseTariffDetail = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.numericDiscount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPayable = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOvercharged = new System.Windows.Forms.TextBox();
            this.txtTarrifTotal = new System.Windows.Forms.TextBox();
            this.datagridTarrifs = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReportCaseTariffDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDiscount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridTarrifs)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.picReportCaseTariffDetail);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.txtPayable);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtOvercharged);
            this.splitContainer1.Panel1.Controls.Add(this.txtTarrifTotal);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.numericDiscount);
            this.splitContainer1.Panel2.Controls.Add(this.txtDiscount);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.datagridTarrifs);
            this.splitContainer1.Size = new System.Drawing.Size(639, 509);
            this.splitContainer1.SplitterDistance = 53;
            this.splitContainer1.TabIndex = 0;
            // 
            // picReportCaseTariffDetail
            // 
            this.picReportCaseTariffDetail.BackColor = System.Drawing.Color.White;
            this.picReportCaseTariffDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picReportCaseTariffDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picReportCaseTariffDetail.Image = global::Icondev.MedManage.Properties.Resources.Book3;
            this.picReportCaseTariffDetail.Location = new System.Drawing.Point(587, 12);
            this.picReportCaseTariffDetail.Name = "picReportCaseTariffDetail";
            this.picReportCaseTariffDetail.Size = new System.Drawing.Size(40, 33);
            this.picReportCaseTariffDetail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picReportCaseTariffDetail.TabIndex = 37;
            this.picReportCaseTariffDetail.TabStop = false;
            this.picReportCaseTariffDetail.Click += new System.EventHandler(this.picReportCaseTariffDetail_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(469, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Discount";
            this.label6.Visible = false;
            // 
            // txtDiscount
            // 
            this.txtDiscount.BackColor = System.Drawing.Color.White;
            this.txtDiscount.Enabled = false;
            this.txtDiscount.ForeColor = System.Drawing.Color.Black;
            this.txtDiscount.Location = new System.Drawing.Point(524, 40);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(100, 20);
            this.txtDiscount.TabIndex = 35;
            this.txtDiscount.Visible = false;
            // 
            // numericDiscount
            // 
            this.numericDiscount.Location = new System.Drawing.Point(525, 14);
            this.numericDiscount.Name = "numericDiscount";
            this.numericDiscount.Size = new System.Drawing.Size(78, 20);
            this.numericDiscount.TabIndex = 34;
            this.numericDiscount.Visible = false;
            this.numericDiscount.ValueChanged += new System.EventHandler(this.numericDiscount_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "%";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(417, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Payable";
            // 
            // txtPayable
            // 
            this.txtPayable.BackColor = System.Drawing.Color.White;
            this.txtPayable.Enabled = false;
            this.txtPayable.ForeColor = System.Drawing.Color.Black;
            this.txtPayable.Location = new System.Drawing.Point(472, 18);
            this.txtPayable.Name = "txtPayable";
            this.txtPayable.Size = new System.Drawing.Size(100, 20);
            this.txtPayable.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(469, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Discount";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Total Claimed";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Total Overcharged";
            // 
            // txtOvercharged
            // 
            this.txtOvercharged.BackColor = System.Drawing.Color.White;
            this.txtOvercharged.Enabled = false;
            this.txtOvercharged.ForeColor = System.Drawing.Color.Black;
            this.txtOvercharged.Location = new System.Drawing.Point(297, 18);
            this.txtOvercharged.Name = "txtOvercharged";
            this.txtOvercharged.Size = new System.Drawing.Size(100, 20);
            this.txtOvercharged.TabIndex = 26;
            // 
            // txtTarrifTotal
            // 
            this.txtTarrifTotal.BackColor = System.Drawing.Color.White;
            this.txtTarrifTotal.Enabled = false;
            this.txtTarrifTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTarrifTotal.Location = new System.Drawing.Point(89, 18);
            this.txtTarrifTotal.Name = "txtTarrifTotal";
            this.txtTarrifTotal.Size = new System.Drawing.Size(100, 20);
            this.txtTarrifTotal.TabIndex = 24;
            // 
            // datagridTarrifs
            // 
            this.datagridTarrifs.AllowUserToAddRows = false;
            this.datagridTarrifs.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridTarrifs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.datagridTarrifs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridTarrifs.DefaultCellStyle = dataGridViewCellStyle2;
            this.datagridTarrifs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridTarrifs.Location = new System.Drawing.Point(0, 0);
            this.datagridTarrifs.Name = "datagridTarrifs";
            this.datagridTarrifs.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridTarrifs.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.datagridTarrifs.Size = new System.Drawing.Size(639, 452);
            this.datagridTarrifs.TabIndex = 21;
            this.datagridTarrifs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.datagridTarrifs_DataBindingComplete);
            // 
            // CaseTariffDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 509);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CaseTariffDetail";
            this.Text = "CaseTariffDetail";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picReportCaseTariffDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDiscount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridTarrifs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView datagridTarrifs;
        private System.Windows.Forms.TextBox txtTarrifTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPayable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOvercharged;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericDiscount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.PictureBox picReportCaseTariffDetail;

    }
}
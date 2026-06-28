namespace Icondev.MedManage
{
    partial class FinanceBulkPayment
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
            this.txtRemittanceNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadRemittance = new System.Windows.Forms.Button();
            this.datagridRemittanceDetail = new System.Windows.Forms.DataGridView();
            this.txtTotalValueApproved = new System.Windows.Forms.TextBox();
            this.btnMarkAllAsPaid = new System.Windows.Forms.Button();
            this.btnPrintRemittanceDetail = new System.Windows.Forms.Button();
            this.btnOutstandingCases = new System.Windows.Forms.Button();
            this.tabFinance = new System.Windows.Forms.TabControl();
            this.tabCases = new System.Windows.Forms.TabPage();
            this.S = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBillingStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboCaseStatus = new System.Windows.Forms.ComboBox();
            this.btnCreateRemittance = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dateFinalInvoiceAmountStart = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLookupPracticeName = new System.Windows.Forms.TextBox();
            this.lblSelectedPracticeName = new System.Windows.Forms.Label();
            this.lblCurrentPracticeID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLookupPracticeNumber = new System.Windows.Forms.TextBox();
            this.datagridCases = new System.Windows.Forms.DataGridView();
            this.tabRemittance = new System.Windows.Forms.TabPage();
            this.btnImportRemittanceStatus = new System.Windows.Forms.Button();
            this.dateMarkAsPaidDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.datagridRemittanceDetail)).BeginInit();
            this.tabFinance.SuspendLayout();
            this.tabCases.SuspendLayout();
            this.S.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridCases)).BeginInit();
            this.tabRemittance.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRemittanceNumber
            // 
            this.txtRemittanceNumber.Location = new System.Drawing.Point(123, 10);
            this.txtRemittanceNumber.Name = "txtRemittanceNumber";
            this.txtRemittanceNumber.Size = new System.Drawing.Size(167, 20);
            this.txtRemittanceNumber.TabIndex = 0;
            this.txtRemittanceNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRemittanceNumber_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Remittance Number";
            // 
            // btnLoadRemittance
            // 
            this.btnLoadRemittance.Location = new System.Drawing.Point(296, 8);
            this.btnLoadRemittance.Name = "btnLoadRemittance";
            this.btnLoadRemittance.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRemittance.TabIndex = 2;
            this.btnLoadRemittance.Text = "Load";
            this.btnLoadRemittance.UseVisualStyleBackColor = true;
            this.btnLoadRemittance.Click += new System.EventHandler(this.btnLoadRemittance_Click);
            // 
            // datagridRemittanceDetail
            // 
            this.datagridRemittanceDetail.AllowUserToAddRows = false;
            this.datagridRemittanceDetail.AllowUserToDeleteRows = false;
            this.datagridRemittanceDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridRemittanceDetail.Location = new System.Drawing.Point(15, 36);
            this.datagridRemittanceDetail.Name = "datagridRemittanceDetail";
            this.datagridRemittanceDetail.ReadOnly = true;
            this.datagridRemittanceDetail.Size = new System.Drawing.Size(888, 445);
            this.datagridRemittanceDetail.TabIndex = 3;
            this.datagridRemittanceDetail.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.datagridRemittanceDetail_DataBindingComplete);
            // 
            // txtTotalValueApproved
            // 
            this.txtTotalValueApproved.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotalValueApproved.Enabled = false;
            this.txtTotalValueApproved.Location = new System.Drawing.Point(812, 10);
            this.txtTotalValueApproved.Name = "txtTotalValueApproved";
            this.txtTotalValueApproved.Size = new System.Drawing.Size(91, 20);
            this.txtTotalValueApproved.TabIndex = 4;
            // 
            // btnMarkAllAsPaid
            // 
            this.btnMarkAllAsPaid.Location = new System.Drawing.Point(793, 487);
            this.btnMarkAllAsPaid.Name = "btnMarkAllAsPaid";
            this.btnMarkAllAsPaid.Size = new System.Drawing.Size(110, 23);
            this.btnMarkAllAsPaid.TabIndex = 5;
            this.btnMarkAllAsPaid.Text = "Mark All As Paid";
            this.btnMarkAllAsPaid.UseVisualStyleBackColor = true;
            this.btnMarkAllAsPaid.Click += new System.EventHandler(this.btnMarkAllAsPaid_Click);
            // 
            // btnPrintRemittanceDetail
            // 
            this.btnPrintRemittanceDetail.Location = new System.Drawing.Point(203, 489);
            this.btnPrintRemittanceDetail.Name = "btnPrintRemittanceDetail";
            this.btnPrintRemittanceDetail.Size = new System.Drawing.Size(138, 23);
            this.btnPrintRemittanceDetail.TabIndex = 6;
            this.btnPrintRemittanceDetail.Text = "Print Remittance Detail";
            this.btnPrintRemittanceDetail.UseVisualStyleBackColor = true;
            this.btnPrintRemittanceDetail.Visible = false;
            this.btnPrintRemittanceDetail.Click += new System.EventHandler(this.btnPrintRemittanceDetail_Click);
            // 
            // btnOutstandingCases
            // 
            this.btnOutstandingCases.Location = new System.Drawing.Point(347, 489);
            this.btnOutstandingCases.Name = "btnOutstandingCases";
            this.btnOutstandingCases.Size = new System.Drawing.Size(138, 23);
            this.btnOutstandingCases.TabIndex = 8;
            this.btnOutstandingCases.Text = "View Outstanding Cases";
            this.btnOutstandingCases.UseVisualStyleBackColor = true;
            this.btnOutstandingCases.Visible = false;
            this.btnOutstandingCases.Click += new System.EventHandler(this.btnOutstandingCases_Click);
            // 
            // tabFinance
            // 
            this.tabFinance.Controls.Add(this.tabCases);
            this.tabFinance.Controls.Add(this.tabRemittance);
            this.tabFinance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFinance.Location = new System.Drawing.Point(0, 0);
            this.tabFinance.Name = "tabFinance";
            this.tabFinance.SelectedIndex = 0;
            this.tabFinance.Size = new System.Drawing.Size(919, 544);
            this.tabFinance.TabIndex = 9;
            // 
            // tabCases
            // 
            this.tabCases.Controls.Add(this.S);
            this.tabCases.Controls.Add(this.datagridCases);
            this.tabCases.Location = new System.Drawing.Point(4, 22);
            this.tabCases.Name = "tabCases";
            this.tabCases.Padding = new System.Windows.Forms.Padding(3);
            this.tabCases.Size = new System.Drawing.Size(911, 518);
            this.tabCases.TabIndex = 1;
            this.tabCases.Text = "Cases";
            this.tabCases.UseVisualStyleBackColor = true;
            // 
            // S
            // 
            this.S.Controls.Add(this.btnRefresh);
            this.S.Controls.Add(this.label7);
            this.S.Controls.Add(this.comboBillingStatus);
            this.S.Controls.Add(this.label3);
            this.S.Controls.Add(this.comboCaseStatus);
            this.S.Controls.Add(this.btnCreateRemittance);
            this.S.Controls.Add(this.label6);
            this.S.Controls.Add(this.dateFinalInvoiceAmountEnd);
            this.S.Controls.Add(this.label5);
            this.S.Controls.Add(this.dateFinalInvoiceAmountStart);
            this.S.Controls.Add(this.label4);
            this.S.Controls.Add(this.txtLookupPracticeName);
            this.S.Controls.Add(this.lblSelectedPracticeName);
            this.S.Controls.Add(this.lblCurrentPracticeID);
            this.S.Controls.Add(this.label2);
            this.S.Controls.Add(this.txtLookupPracticeNumber);
            this.S.Location = new System.Drawing.Point(6, 6);
            this.S.Name = "S";
            this.S.Size = new System.Drawing.Size(897, 105);
            this.S.TabIndex = 112;
            this.S.TabStop = false;
            this.S.Text = "Service Provider";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(776, 42);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(115, 23);
            this.btnRefresh.TabIndex = 130;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(442, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 129;
            this.label7.Text = "Billing Status";
            // 
            // comboBillingStatus
            // 
            this.comboBillingStatus.FormattingEnabled = true;
            this.comboBillingStatus.Location = new System.Drawing.Point(510, 76);
            this.comboBillingStatus.Name = "comboBillingStatus";
            this.comboBillingStatus.Size = new System.Drawing.Size(121, 21);
            this.comboBillingStatus.TabIndex = 128;
            this.comboBillingStatus.SelectedIndexChanged += new System.EventHandler(this.comboBillingStatus_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(442, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 127;
            this.label3.Text = "Case Status";
            // 
            // comboCaseStatus
            // 
            this.comboCaseStatus.FormattingEnabled = true;
            this.comboCaseStatus.Location = new System.Drawing.Point(510, 49);
            this.comboCaseStatus.Name = "comboCaseStatus";
            this.comboCaseStatus.Size = new System.Drawing.Size(121, 21);
            this.comboCaseStatus.TabIndex = 126;
            this.comboCaseStatus.SelectedIndexChanged += new System.EventHandler(this.comboCaseStatus_SelectedIndexChanged);
            // 
            // btnCreateRemittance
            // 
            this.btnCreateRemittance.Location = new System.Drawing.Point(776, 73);
            this.btnCreateRemittance.Name = "btnCreateRemittance";
            this.btnCreateRemittance.Size = new System.Drawing.Size(115, 23);
            this.btnCreateRemittance.TabIndex = 125;
            this.btnCreateRemittance.Text = "Create Remittance";
            this.btnCreateRemittance.UseVisualStyleBackColor = true;
            this.btnCreateRemittance.Click += new System.EventHandler(this.btnCreateRemittance_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(298, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 123;
            this.label6.Text = "And";
            // 
            // dateFinalInvoiceAmountEnd
            // 
            this.dateFinalInvoiceAmountEnd.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountEnd.Location = new System.Drawing.Point(332, 20);
            this.dateFinalInvoiceAmountEnd.Name = "dateFinalInvoiceAmountEnd";
            this.dateFinalInvoiceAmountEnd.Size = new System.Drawing.Size(82, 20);
            this.dateFinalInvoiceAmountEnd.TabIndex = 2;
            this.dateFinalInvoiceAmountEnd.Leave += new System.EventHandler(this.dateFinalInvoiceAmountEnd_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 13);
            this.label5.TabIndex = 124;
            this.label5.Text = "Final Invoice Amount Updated Between ";
            // 
            // dateFinalInvoiceAmountStart
            // 
            this.dateFinalInvoiceAmountStart.CustomFormat = "yyyy/MM/dd";
            this.dateFinalInvoiceAmountStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateFinalInvoiceAmountStart.Location = new System.Drawing.Point(207, 20);
            this.dateFinalInvoiceAmountStart.Name = "dateFinalInvoiceAmountStart";
            this.dateFinalInvoiceAmountStart.Size = new System.Drawing.Size(82, 20);
            this.dateFinalInvoiceAmountStart.TabIndex = 1;
            this.dateFinalInvoiceAmountStart.Leave += new System.EventHandler(this.dateFinalInvoiceAmountStart_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Practice Name";
            // 
            // txtLookupPracticeName
            // 
            this.txtLookupPracticeName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLookupPracticeName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtLookupPracticeName.Location = new System.Drawing.Point(99, 76);
            this.txtLookupPracticeName.Name = "txtLookupPracticeName";
            this.txtLookupPracticeName.Size = new System.Drawing.Size(216, 20);
            this.txtLookupPracticeName.TabIndex = 4;
            this.txtLookupPracticeName.Leave += new System.EventHandler(this.txtLookupPracticeName_Leave);
            // 
            // lblSelectedPracticeName
            // 
            this.lblSelectedPracticeName.AutoSize = true;
            this.lblSelectedPracticeName.Location = new System.Drawing.Point(451, 26);
            this.lblSelectedPracticeName.Name = "lblSelectedPracticeName";
            this.lblSelectedPracticeName.Size = new System.Drawing.Size(108, 13);
            this.lblSelectedPracticeName.TabIndex = 5;
            this.lblSelectedPracticeName.Text = "No Provider Selected";
            // 
            // lblCurrentPracticeID
            // 
            this.lblCurrentPracticeID.AutoSize = true;
            this.lblCurrentPracticeID.Location = new System.Drawing.Point(569, 26);
            this.lblCurrentPracticeID.Name = "lblCurrentPracticeID";
            this.lblCurrentPracticeID.Size = new System.Drawing.Size(101, 13);
            this.lblCurrentPracticeID.TabIndex = 4;
            this.lblCurrentPracticeID.Text = "lblCurrentPracticeID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Practice Number";
            // 
            // txtLookupPracticeNumber
            // 
            this.txtLookupPracticeNumber.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtLookupPracticeNumber.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtLookupPracticeNumber.Location = new System.Drawing.Point(99, 50);
            this.txtLookupPracticeNumber.Name = "txtLookupPracticeNumber";
            this.txtLookupPracticeNumber.Size = new System.Drawing.Size(216, 20);
            this.txtLookupPracticeNumber.TabIndex = 3;
            this.txtLookupPracticeNumber.Leave += new System.EventHandler(this.txtLookupPracticeNumber_Leave);
            // 
            // datagridCases
            // 
            this.datagridCases.AllowUserToAddRows = false;
            this.datagridCases.AllowUserToDeleteRows = false;
            this.datagridCases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridCases.Location = new System.Drawing.Point(6, 117);
            this.datagridCases.Name = "datagridCases";
            this.datagridCases.Size = new System.Drawing.Size(897, 371);
            this.datagridCases.TabIndex = 4;
            this.datagridCases.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.datagridCases_DataBindingComplete);
            // 
            // tabRemittance
            // 
            this.tabRemittance.Controls.Add(this.label8);
            this.tabRemittance.Controls.Add(this.dateMarkAsPaidDate);
            this.tabRemittance.Controls.Add(this.btnImportRemittanceStatus);
            this.tabRemittance.Controls.Add(this.datagridRemittanceDetail);
            this.tabRemittance.Controls.Add(this.btnOutstandingCases);
            this.tabRemittance.Controls.Add(this.txtRemittanceNumber);
            this.tabRemittance.Controls.Add(this.label1);
            this.tabRemittance.Controls.Add(this.btnPrintRemittanceDetail);
            this.tabRemittance.Controls.Add(this.btnLoadRemittance);
            this.tabRemittance.Controls.Add(this.btnMarkAllAsPaid);
            this.tabRemittance.Controls.Add(this.txtTotalValueApproved);
            this.tabRemittance.Location = new System.Drawing.Point(4, 22);
            this.tabRemittance.Name = "tabRemittance";
            this.tabRemittance.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemittance.Size = new System.Drawing.Size(911, 518);
            this.tabRemittance.TabIndex = 0;
            this.tabRemittance.Text = "Remittance";
            this.tabRemittance.UseVisualStyleBackColor = true;
            // 
            // btnImportRemittanceStatus
            // 
            this.btnImportRemittanceStatus.Location = new System.Drawing.Point(620, 8);
            this.btnImportRemittanceStatus.Name = "btnImportRemittanceStatus";
            this.btnImportRemittanceStatus.Size = new System.Drawing.Size(127, 23);
            this.btnImportRemittanceStatus.TabIndex = 9;
            this.btnImportRemittanceStatus.Text = "Import Status";
            this.btnImportRemittanceStatus.UseVisualStyleBackColor = true;
            this.btnImportRemittanceStatus.Click += new System.EventHandler(this.btnImportRemittanceStatus_Click);
            // 
            // dateMarkAsPaidDate
            // 
            this.dateMarkAsPaidDate.CustomFormat = "yyyy/MM/dd";
            this.dateMarkAsPaidDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateMarkAsPaidDate.Location = new System.Drawing.Point(705, 489);
            this.dateMarkAsPaidDate.Name = "dateMarkAsPaidDate";
            this.dateMarkAsPaidDate.Size = new System.Drawing.Size(82, 20);
            this.dateMarkAsPaidDate.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(645, 492);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Date Paid";
            // 
            // FinanceBulkPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 544);
            this.Controls.Add(this.tabFinance);
            this.Name = "FinanceBulkPayment";
            this.Text = "FinanceBulkPayment";
            ((System.ComponentModel.ISupportInitialize)(this.datagridRemittanceDetail)).EndInit();
            this.tabFinance.ResumeLayout(false);
            this.tabCases.ResumeLayout(false);
            this.S.ResumeLayout(false);
            this.S.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridCases)).EndInit();
            this.tabRemittance.ResumeLayout(false);
            this.tabRemittance.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRemittanceNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadRemittance;
        private System.Windows.Forms.DataGridView datagridRemittanceDetail;
        private System.Windows.Forms.TextBox txtTotalValueApproved;
        private System.Windows.Forms.Button btnMarkAllAsPaid;
        private System.Windows.Forms.Button btnPrintRemittanceDetail;
        private System.Windows.Forms.Button btnOutstandingCases;
        private System.Windows.Forms.TabControl tabFinance;
        private System.Windows.Forms.TabPage tabRemittance;
        private System.Windows.Forms.TabPage tabCases;
        private System.Windows.Forms.DataGridView datagridCases;
        private System.Windows.Forms.GroupBox S;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateFinalInvoiceAmountStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLookupPracticeName;
        private System.Windows.Forms.Label lblSelectedPracticeName;
        private System.Windows.Forms.Label lblCurrentPracticeID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLookupPracticeNumber;
        private System.Windows.Forms.Button btnCreateRemittance;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBillingStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboCaseStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnImportRemittanceStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateMarkAsPaidDate;
    }
}
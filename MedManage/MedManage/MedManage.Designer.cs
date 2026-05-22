namespace Icondev.MedManage
{
    partial class MedManageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MedManageForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeMyPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.caseManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForCasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceProviderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medicalAidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.caseDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.caseDetailsByAdmissionDatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.financeCasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.caseDetailsByParentCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dRDEmployeeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nappiCodesImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.financeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkPaymentCaptureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wipExtractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolAppNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.caseManagementToolStripMenuItem,
            this.metadataToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.devToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.importsToolStripMenuItem,
            this.financeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.windowToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1661, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Checked = true;
            this.fileToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.changeMyPasswordToolStripMenuItem,
            this.chooseClientToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // changeMyPasswordToolStripMenuItem
            // 
            this.changeMyPasswordToolStripMenuItem.Name = "changeMyPasswordToolStripMenuItem";
            this.changeMyPasswordToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.changeMyPasswordToolStripMenuItem.Text = "Change My Password";
            this.changeMyPasswordToolStripMenuItem.Click += new System.EventHandler(this.changeMyPasswordToolStripMenuItem_Click);
            // 
            // chooseClientToolStripMenuItem
            // 
            this.chooseClientToolStripMenuItem.Name = "chooseClientToolStripMenuItem";
            this.chooseClientToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.chooseClientToolStripMenuItem.Text = "Choose Client";
            this.chooseClientToolStripMenuItem.Click += new System.EventHandler(this.chooseClientToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(223, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(178, 6);
            // 
            // caseManagementToolStripMenuItem
            // 
            this.caseManagementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchForCasesToolStripMenuItem,
            this.bookingsToolStripMenuItem});
            this.caseManagementToolStripMenuItem.Name = "caseManagementToolStripMenuItem";
            this.caseManagementToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.caseManagementToolStripMenuItem.Text = "Case Management";
            this.caseManagementToolStripMenuItem.Visible = false;
            // 
            // searchForCasesToolStripMenuItem
            // 
            this.searchForCasesToolStripMenuItem.Name = "searchForCasesToolStripMenuItem";
            this.searchForCasesToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.searchForCasesToolStripMenuItem.Text = "Search For Cases";
            this.searchForCasesToolStripMenuItem.Visible = false;
            this.searchForCasesToolStripMenuItem.Click += new System.EventHandler(this.searchForCasesToolStripMenuItem_Click);
            // 
            // bookingsToolStripMenuItem
            // 
            this.bookingsToolStripMenuItem.Name = "bookingsToolStripMenuItem";
            this.bookingsToolStripMenuItem.Size = new System.Drawing.Size(194, 26);
            this.bookingsToolStripMenuItem.Text = "Bookings";
            this.bookingsToolStripMenuItem.Click += new System.EventHandler(this.bookingsToolStripMenuItem_Click);
            // 
            // metadataToolStripMenuItem
            // 
            this.metadataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memberToolStripMenuItem,
            this.serviceProviderToolStripMenuItem,
            this.medicalAidToolStripMenuItem});
            this.metadataToolStripMenuItem.Name = "metadataToolStripMenuItem";
            this.metadataToolStripMenuItem.Size = new System.Drawing.Size(85, 24);
            this.metadataToolStripMenuItem.Text = "Metadata";
            this.metadataToolStripMenuItem.Visible = false;
            // 
            // memberToolStripMenuItem
            // 
            this.memberToolStripMenuItem.Name = "memberToolStripMenuItem";
            this.memberToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.memberToolStripMenuItem.Text = "Member";
            this.memberToolStripMenuItem.Click += new System.EventHandler(this.memberToolStripMenuItem_Click);
            // 
            // serviceProviderToolStripMenuItem
            // 
            this.serviceProviderToolStripMenuItem.Name = "serviceProviderToolStripMenuItem";
            this.serviceProviderToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.serviceProviderToolStripMenuItem.Text = "Service Provider";
            this.serviceProviderToolStripMenuItem.Click += new System.EventHandler(this.serviceProviderToolStripMenuItem_Click);
            // 
            // medicalAidToolStripMenuItem
            // 
            this.medicalAidToolStripMenuItem.Name = "medicalAidToolStripMenuItem";
            this.medicalAidToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.medicalAidToolStripMenuItem.Text = "Medical Aid";
            this.medicalAidToolStripMenuItem.Click += new System.EventHandler(this.medicalAidToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemDataToolStripMenuItem,
            this.userManagementToolStripMenuItem,
            this.deleteUserSessionToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.adminToolStripMenuItem.Text = "Admin";
            this.adminToolStripMenuItem.Visible = false;
            // 
            // systemDataToolStripMenuItem
            // 
            this.systemDataToolStripMenuItem.Name = "systemDataToolStripMenuItem";
            this.systemDataToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.systemDataToolStripMenuItem.Text = "System Data";
            this.systemDataToolStripMenuItem.Visible = false;
            this.systemDataToolStripMenuItem.Click += new System.EventHandler(this.systemDataToolStripMenuItem_Click);
            // 
            // userManagementToolStripMenuItem
            // 
            this.userManagementToolStripMenuItem.Name = "userManagementToolStripMenuItem";
            this.userManagementToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.userManagementToolStripMenuItem.Text = "User Management";
            this.userManagementToolStripMenuItem.Click += new System.EventHandler(this.userManagementToolStripMenuItem_Click);
            // 
            // deleteUserSessionToolStripMenuItem
            // 
            this.deleteUserSessionToolStripMenuItem.Name = "deleteUserSessionToolStripMenuItem";
            this.deleteUserSessionToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.deleteUserSessionToolStripMenuItem.Text = "Delete User Session";
            this.deleteUserSessionToolStripMenuItem.Click += new System.EventHandler(this.deleteUserSessionToolStripMenuItem_Click);
            // 
            // devToolStripMenuItem
            // 
            this.devToolStripMenuItem.Enabled = false;
            this.devToolStripMenuItem.Name = "devToolStripMenuItem";
            this.devToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.devToolStripMenuItem.Text = "Dev";
            this.devToolStripMenuItem.Visible = false;
            this.devToolStripMenuItem.Click += new System.EventHandler(this.devToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.caseDetailsToolStripMenuItem,
            this.caseDetailsByAdmissionDatesToolStripMenuItem,
            this.financeCasesToolStripMenuItem,
            this.caseDetailsByParentCaseToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.reportsToolStripMenuItem.Text = "Reports";
            this.reportsToolStripMenuItem.Visible = false;
            // 
            // caseDetailsToolStripMenuItem
            // 
            this.caseDetailsToolStripMenuItem.Name = "caseDetailsToolStripMenuItem";
            this.caseDetailsToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.caseDetailsToolStripMenuItem.Text = "Case Details by Create Date";
            this.caseDetailsToolStripMenuItem.Click += new System.EventHandler(this.caseDetailsToolStripMenuItem_Click);
            // 
            // caseDetailsByAdmissionDatesToolStripMenuItem
            // 
            this.caseDetailsByAdmissionDatesToolStripMenuItem.Name = "caseDetailsByAdmissionDatesToolStripMenuItem";
            this.caseDetailsByAdmissionDatesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.caseDetailsByAdmissionDatesToolStripMenuItem.Text = "Case Details by Admission Dates";
            this.caseDetailsByAdmissionDatesToolStripMenuItem.Click += new System.EventHandler(this.caseDetailsByAdmissionDatesToolStripMenuItem_Click);
            // 
            // financeCasesToolStripMenuItem
            // 
            this.financeCasesToolStripMenuItem.Name = "financeCasesToolStripMenuItem";
            this.financeCasesToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.financeCasesToolStripMenuItem.Text = "Finance - Cases";
            this.financeCasesToolStripMenuItem.Click += new System.EventHandler(this.financeCasesToolStripMenuItem_Click);
            // 
            // caseDetailsByParentCaseToolStripMenuItem
            // 
            this.caseDetailsByParentCaseToolStripMenuItem.Name = "caseDetailsByParentCaseToolStripMenuItem";
            this.caseDetailsByParentCaseToolStripMenuItem.Size = new System.Drawing.Size(300, 26);
            this.caseDetailsByParentCaseToolStripMenuItem.Text = "Case Details by ParentCase";
            this.caseDetailsByParentCaseToolStripMenuItem.Click += new System.EventHandler(this.caseDetailsByParentCaseToolStripMenuItem_Click);
            // 
            // importsToolStripMenuItem
            // 
            this.importsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dRDEmployeeListToolStripMenuItem,
            this.nappiCodesImportToolStripMenuItem});
            this.importsToolStripMenuItem.Name = "importsToolStripMenuItem";
            this.importsToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.importsToolStripMenuItem.Text = "Imports";
            this.importsToolStripMenuItem.Visible = false;
            // 
            // dRDEmployeeListToolStripMenuItem
            // 
            this.dRDEmployeeListToolStripMenuItem.Name = "dRDEmployeeListToolStripMenuItem";
            this.dRDEmployeeListToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.dRDEmployeeListToolStripMenuItem.Text = "DRD Employee List";
            this.dRDEmployeeListToolStripMenuItem.Click += new System.EventHandler(this.dRDEmployeeListToolStripMenuItem_Click);
            // 
            // nappiCodesImportToolStripMenuItem
            // 
            this.nappiCodesImportToolStripMenuItem.Name = "nappiCodesImportToolStripMenuItem";
            this.nappiCodesImportToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.nappiCodesImportToolStripMenuItem.Text = "Nappi Codes Import";
            this.nappiCodesImportToolStripMenuItem.Click += new System.EventHandler(this.nappiCodesImportToolStripMenuItem_Click);
            // 
            // financeToolStripMenuItem
            // 
            this.financeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataCaptureToolStripMenuItem,
            this.bulkPaymentCaptureToolStripMenuItem,
            this.wipExtractToolStripMenuItem});
            this.financeToolStripMenuItem.Name = "financeToolStripMenuItem";
            this.financeToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.financeToolStripMenuItem.Text = "Finance";
            this.financeToolStripMenuItem.Visible = false;
            // 
            // dataCaptureToolStripMenuItem
            // 
            this.dataCaptureToolStripMenuItem.Name = "dataCaptureToolStripMenuItem";
            this.dataCaptureToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.dataCaptureToolStripMenuItem.Text = "Data Capture";
            this.dataCaptureToolStripMenuItem.Click += new System.EventHandler(this.dataCaptureToolStripMenuItem_Click);
            // 
            // bulkPaymentCaptureToolStripMenuItem
            // 
            this.bulkPaymentCaptureToolStripMenuItem.Name = "bulkPaymentCaptureToolStripMenuItem";
            this.bulkPaymentCaptureToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.bulkPaymentCaptureToolStripMenuItem.Text = "Bulk Payment Capture";
            this.bulkPaymentCaptureToolStripMenuItem.Visible = false;
            this.bulkPaymentCaptureToolStripMenuItem.Click += new System.EventHandler(this.bulkPaymentCaptureToolStripMenuItem_Click);
            // 
            // wipExtractToolStripMenuItem
            // 
            this.wipExtractToolStripMenuItem.Name = "wipExtractToolStripMenuItem";
            this.wipExtractToolStripMenuItem.Size = new System.Drawing.Size(228, 26);
            this.wipExtractToolStripMenuItem.Text = "WIP Extract";
            this.wipExtractToolStripMenuItem.Visible = false;
            this.wipExtractToolStripMenuItem.Click += new System.EventHandler(this.wipExtractToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAppNameLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 514);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1661, 25);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolAppNameLabel
            // 
            this.toolAppNameLabel.Name = "toolAppNameLabel";
            this.toolAppNameLabel.Size = new System.Drawing.Size(448, 20);
            this.toolAppNameLabel.Text = "Iconic Development - In hospital case management (MedManage)";
            // 
            // MedManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1661, 539);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MedManageForm";
            this.Text = "Med Manage";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MedManageForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolAppNameLabel;
        private System.Windows.Forms.ToolStripMenuItem changeMyPasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caseManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchForCasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serviceProviderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem medicalAidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem devToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserSessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bookingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caseDetailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caseDetailsByAdmissionDatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem financeCasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dRDEmployeeListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nappiCodesImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem financeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataCaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bulkPaymentCaptureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem caseDetailsByParentCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wipExtractToolStripMenuItem;
    }
}


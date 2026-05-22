namespace Icondev.MedManage
{
    partial class rpt_ReportServer
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
            this.webReportServer = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webReportServer
            // 
            this.webReportServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webReportServer.Location = new System.Drawing.Point(0, 0);
            this.webReportServer.MinimumSize = new System.Drawing.Size(20, 20);
            this.webReportServer.Name = "webReportServer";
            this.webReportServer.Size = new System.Drawing.Size(771, 324);
            this.webReportServer.TabIndex = 0;
            // 
            // rpt_ReportServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 324);
            this.Controls.Add(this.webReportServer);
            this.Name = "rpt_ReportServer";
            this.Text = "ReportServer";
            this.Load += new System.EventHandler(this.rpt_ReportServer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webReportServer;
    }
}
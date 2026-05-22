namespace Icondev.MedManage
{
    partial class CustomMessageBoxDateSelector
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
            this.dateSelector = new System.Windows.Forms.DateTimePicker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCloseDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dateSelector
            // 
            this.dateSelector.CustomFormat = "yyyy/MM/dd";
            this.dateSelector.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateSelector.Location = new System.Drawing.Point(16, 48);
            this.dateSelector.Margin = new System.Windows.Forms.Padding(4);
            this.dateSelector.Name = "dateSelector";
            this.dateSelector.Size = new System.Drawing.Size(176, 22);
            this.dateSelector.TabIndex = 48;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(13, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(79, 17);
            this.lblMessage.TabIndex = 49;
            this.lblMessage.Text = "lblMessage";
            // 
            // btnCloseDialog
            // 
            this.btnCloseDialog.Location = new System.Drawing.Point(284, 50);
            this.btnCloseDialog.Name = "btnCloseDialog";
            this.btnCloseDialog.Size = new System.Drawing.Size(75, 23);
            this.btnCloseDialog.TabIndex = 50;
            this.btnCloseDialog.Text = "Ok";
            this.btnCloseDialog.UseVisualStyleBackColor = true;
            this.btnCloseDialog.Click += new System.EventHandler(this.btnCloseDialog_Click);
            // 
            // CustomMessageBoxDateSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 94);
            this.ControlBox = false;
            this.Controls.Add(this.btnCloseDialog);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dateSelector);
            this.Name = "CustomMessageBoxDateSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomMessageBoxDateSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateSelector;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCloseDialog;
    }
}
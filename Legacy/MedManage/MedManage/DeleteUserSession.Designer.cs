namespace Icondev.MedManage
{
    partial class DeleteUserSession
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
            this.txtCaseID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picDelete = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDelete)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCaseID
            // 
            this.txtCaseID.Location = new System.Drawing.Point(102, 13);
            this.txtCaseID.Name = "txtCaseID";
            this.txtCaseID.Size = new System.Drawing.Size(100, 20);
            this.txtCaseID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "CaseID";
            // 
            // picDelete
            // 
            this.picDelete.BackColor = System.Drawing.Color.White;
            this.picDelete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDelete.Image = global::Icondev.MedManage.Properties.Resources.DeleteRed;
            this.picDelete.Location = new System.Drawing.Point(241, 12);
            this.picDelete.Name = "picDelete";
            this.picDelete.Size = new System.Drawing.Size(39, 34);
            this.picDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDelete.TabIndex = 32;
            this.picDelete.TabStop = false;
            this.picDelete.Click += new System.EventHandler(this.picDelete_Click);
            // 
            // DeleteUserSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 78);
            this.Controls.Add(this.picDelete);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCaseID);
            this.Name = "DeleteUserSession";
            this.Text = "DeleteUserSession";
            ((System.ComponentModel.ISupportInitialize)(this.picDelete)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCaseID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picDelete;
    }
}
namespace Icondev.MedManage
{
    partial class SelectDocumentTypeDialog
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
            this.btnDocumentLinkCancel = new System.Windows.Forms.Button();
            this.btnDocumentLinkOk = new System.Windows.Forms.Button();
            this.rbDocumentLinkToMember = new System.Windows.Forms.RadioButton();
            this.rbDocumentLinkToCase = new System.Windows.Forms.RadioButton();
            this.label97 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDocumentLinkCancel
            // 
            this.btnDocumentLinkCancel.Location = new System.Drawing.Point(263, 78);
            this.btnDocumentLinkCancel.Name = "btnDocumentLinkCancel";
            this.btnDocumentLinkCancel.Size = new System.Drawing.Size(75, 23);
            this.btnDocumentLinkCancel.TabIndex = 9;
            this.btnDocumentLinkCancel.Text = "Cancel";
            this.btnDocumentLinkCancel.UseVisualStyleBackColor = true;
            this.btnDocumentLinkCancel.Click += new System.EventHandler(this.btnDocumentLinkCancel_Click);
            // 
            // btnDocumentLinkOk
            // 
            this.btnDocumentLinkOk.Location = new System.Drawing.Point(13, 78);
            this.btnDocumentLinkOk.Name = "btnDocumentLinkOk";
            this.btnDocumentLinkOk.Size = new System.Drawing.Size(75, 23);
            this.btnDocumentLinkOk.TabIndex = 8;
            this.btnDocumentLinkOk.Text = "Ok";
            this.btnDocumentLinkOk.UseVisualStyleBackColor = true;
            this.btnDocumentLinkOk.Click += new System.EventHandler(this.btnDocumentLinkOk_Click);
            // 
            // rbDocumentLinkToMember
            // 
            this.rbDocumentLinkToMember.AutoSize = true;
            this.rbDocumentLinkToMember.Location = new System.Drawing.Point(22, 55);
            this.rbDocumentLinkToMember.Name = "rbDocumentLinkToMember";
            this.rbDocumentLinkToMember.Size = new System.Drawing.Size(98, 17);
            this.rbDocumentLinkToMember.TabIndex = 7;
            this.rbDocumentLinkToMember.Text = "Link to Member";
            this.rbDocumentLinkToMember.UseVisualStyleBackColor = true;
            this.rbDocumentLinkToMember.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rbDocumentLink_KeyPress);
            // 
            // rbDocumentLinkToCase
            // 
            this.rbDocumentLinkToCase.AutoSize = true;
            this.rbDocumentLinkToCase.Checked = true;
            this.rbDocumentLinkToCase.Location = new System.Drawing.Point(22, 32);
            this.rbDocumentLinkToCase.Name = "rbDocumentLinkToCase";
            this.rbDocumentLinkToCase.Size = new System.Drawing.Size(84, 17);
            this.rbDocumentLinkToCase.TabIndex = 6;
            this.rbDocumentLinkToCase.TabStop = true;
            this.rbDocumentLinkToCase.Text = "Link to Case";
            this.rbDocumentLinkToCase.UseVisualStyleBackColor = true;
            this.rbDocumentLinkToCase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rbDocumentLink_KeyPress);
            // 
            // label97
            // 
            this.label97.AutoSize = true;
            this.label97.Location = new System.Drawing.Point(9, 13);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(288, 13);
            this.label97.TabIndex = 5;
            this.label97.Text = "Should this document be linked to the Member or the case?";
            // 
            // SelectDocumentTypeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 117);
            this.Controls.Add(this.btnDocumentLinkCancel);
            this.Controls.Add(this.btnDocumentLinkOk);
            this.Controls.Add(this.rbDocumentLinkToMember);
            this.Controls.Add(this.rbDocumentLinkToCase);
            this.Controls.Add(this.label97);
            this.Name = "SelectDocumentTypeDialog";
            this.Text = "SelectDocumentTypeDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDocumentLinkCancel;
        private System.Windows.Forms.Button btnDocumentLinkOk;
        private System.Windows.Forms.RadioButton rbDocumentLinkToMember;
        private System.Windows.Forms.RadioButton rbDocumentLinkToCase;
        private System.Windows.Forms.Label label97;
    }
}
namespace Icondev.MedManage
{
    partial class Gallery
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
            this.treeGallery = new System.Windows.Forms.TreeView();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.btnRemoveImage = new System.Windows.Forms.Button();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlAddImage = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPnlFriendlyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPnlCancel = new System.Windows.Forms.Button();
            this.txtPnlFullName = new System.Windows.Forms.TextBox();
            this.btnPnlFindImage = new System.Windows.Forms.Button();
            this.btnPnlAddImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlAddImage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeGallery
            // 
            this.treeGallery.Location = new System.Drawing.Point(12, 40);
            this.treeGallery.Name = "treeGallery";
            this.treeGallery.Size = new System.Drawing.Size(193, 386);
            this.treeGallery.TabIndex = 0;
            this.treeGallery.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeGallery_AfterSelect);
            // 
            // btnAddImage
            // 
            this.btnAddImage.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddImage.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnAddImage.Location = new System.Drawing.Point(12, 5);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(33, 29);
            this.btnAddImage.TabIndex = 1;
            this.btnAddImage.Text = "+";
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // btnRemoveImage
            // 
            this.btnRemoveImage.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveImage.ForeColor = System.Drawing.Color.Maroon;
            this.btnRemoveImage.Location = new System.Drawing.Point(173, 5);
            this.btnRemoveImage.Name = "btnRemoveImage";
            this.btnRemoveImage.Size = new System.Drawing.Size(32, 29);
            this.btnRemoveImage.TabIndex = 2;
            this.btnRemoveImage.Text = "-";
            this.btnRemoveImage.UseVisualStyleBackColor = true;
            this.btnRemoveImage.Click += new System.EventHandler(this.btnRemoveImage_Click);
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(221, 11);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(519, 415);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 3;
            this.picImage.TabStop = false;
            // 
            // pnlAddImage
            // 
            this.pnlAddImage.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAddImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAddImage.Controls.Add(this.groupBox1);
            this.pnlAddImage.Location = new System.Drawing.Point(12, 5);
            this.pnlAddImage.Name = "pnlAddImage";
            this.pnlAddImage.Size = new System.Drawing.Size(553, 145);
            this.pnlAddImage.TabIndex = 4;
            this.pnlAddImage.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPnlFriendlyName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnPnlCancel);
            this.groupBox1.Controls.Add(this.txtPnlFullName);
            this.groupBox1.Controls.Add(this.btnPnlFindImage);
            this.groupBox1.Controls.Add(this.btnPnlAddImage);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Image";
            // 
            // txtPnlFriendlyName
            // 
            this.txtPnlFriendlyName.Location = new System.Drawing.Point(93, 45);
            this.txtPnlFriendlyName.Name = "txtPnlFriendlyName";
            this.txtPnlFriendlyName.Size = new System.Drawing.Size(365, 20);
            this.txtPnlFriendlyName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Friendly Name: ";
            // 
            // btnPnlCancel
            // 
            this.btnPnlCancel.Location = new System.Drawing.Point(6, 108);
            this.btnPnlCancel.Name = "btnPnlCancel";
            this.btnPnlCancel.Size = new System.Drawing.Size(75, 23);
            this.btnPnlCancel.TabIndex = 3;
            this.btnPnlCancel.Text = "Cancel";
            this.btnPnlCancel.UseVisualStyleBackColor = true;
            this.btnPnlCancel.Click += new System.EventHandler(this.btnPnlCancel_Click);
            // 
            // txtPnlFullName
            // 
            this.txtPnlFullName.Enabled = false;
            this.txtPnlFullName.Location = new System.Drawing.Point(7, 19);
            this.txtPnlFullName.Name = "txtPnlFullName";
            this.txtPnlFullName.Size = new System.Drawing.Size(451, 20);
            this.txtPnlFullName.TabIndex = 2;
            // 
            // btnPnlFindImage
            // 
            this.btnPnlFindImage.Location = new System.Drawing.Point(464, 17);
            this.btnPnlFindImage.Name = "btnPnlFindImage";
            this.btnPnlFindImage.Size = new System.Drawing.Size(75, 23);
            this.btnPnlFindImage.TabIndex = 1;
            this.btnPnlFindImage.Text = "Find Image";
            this.btnPnlFindImage.UseVisualStyleBackColor = true;
            this.btnPnlFindImage.Click += new System.EventHandler(this.btnPnlFindImage_Click);
            // 
            // btnPnlAddImage
            // 
            this.btnPnlAddImage.Location = new System.Drawing.Point(464, 108);
            this.btnPnlAddImage.Name = "btnPnlAddImage";
            this.btnPnlAddImage.Size = new System.Drawing.Size(75, 23);
            this.btnPnlAddImage.TabIndex = 0;
            this.btnPnlAddImage.Text = "Add";
            this.btnPnlAddImage.UseVisualStyleBackColor = true;
            this.btnPnlAddImage.Click += new System.EventHandler(this.btnPnlAddImage_Click);
            // 
            // Gallery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 438);
            this.Controls.Add(this.pnlAddImage);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.btnRemoveImage);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.treeGallery);
            this.Name = "Gallery";
            this.Text = "Gallery";
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlAddImage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeGallery;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.Button btnRemoveImage;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Panel pnlAddImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPnlFullName;
        private System.Windows.Forms.Button btnPnlFindImage;
        private System.Windows.Forms.Button btnPnlAddImage;
        private System.Windows.Forms.Button btnPnlCancel;
        private System.Windows.Forms.TextBox txtPnlFriendlyName;
        private System.Windows.Forms.Label label1;
    }
}
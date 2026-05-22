namespace Icondev.MedManage
{
    partial class ServiceProviderLookup
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picOpen = new System.Windows.Forms.PictureBox();
            this.picCancel = new System.Windows.Forms.PictureBox();
            this.picSelectServiceProvider = new System.Windows.Forms.PictureBox();
            this.picAddServiceProvider = new System.Windows.Forms.PictureBox();
            this.picServiceProviderLookup = new System.Windows.Forms.PictureBox();
            this.txtPracticeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProviderNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.datagridServiceProviders = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectServiceProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddServiceProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picServiceProviderLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridServiceProviders)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.datagridServiceProviders);
            this.splitContainer1.Size = new System.Drawing.Size(674, 593);
            this.splitContainer1.SplitterDistance = 94;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picOpen);
            this.groupBox1.Controls.Add(this.picCancel);
            this.groupBox1.Controls.Add(this.picSelectServiceProvider);
            this.groupBox1.Controls.Add(this.picAddServiceProvider);
            this.groupBox1.Controls.Add(this.picServiceProviderLookup);
            this.groupBox1.Controls.Add(this.txtPracticeName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtProviderNumber);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtSurname);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // picOpen
            // 
            this.picOpen.BackColor = System.Drawing.Color.White;
            this.picOpen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOpen.Image = global::Icondev.MedManage.Properties.Resources.FolderOpened_Yellow;
            this.picOpen.Location = new System.Drawing.Point(12, 19);
            this.picOpen.Name = "picOpen";
            this.picOpen.Size = new System.Drawing.Size(34, 33);
            this.picOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOpen.TabIndex = 86;
            this.picOpen.TabStop = false;
            this.picOpen.Click += new System.EventHandler(this.picOpen_Click);
            // 
            // picCancel
            // 
            this.picCancel.BackColor = System.Drawing.Color.White;
            this.picCancel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCancel.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picCancel.Location = new System.Drawing.Point(628, 55);
            this.picCancel.Name = "picCancel";
            this.picCancel.Size = new System.Drawing.Size(40, 33);
            this.picCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCancel.TabIndex = 85;
            this.picCancel.TabStop = false;
            this.picCancel.Click += new System.EventHandler(this.picCancel_Click);
            // 
            // picSelectServiceProvider
            // 
            this.picSelectServiceProvider.BackColor = System.Drawing.Color.White;
            this.picSelectServiceProvider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSelectServiceProvider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelectServiceProvider.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSelectServiceProvider.Location = new System.Drawing.Point(625, 12);
            this.picSelectServiceProvider.Name = "picSelectServiceProvider";
            this.picSelectServiceProvider.Size = new System.Drawing.Size(43, 34);
            this.picSelectServiceProvider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSelectServiceProvider.TabIndex = 31;
            this.picSelectServiceProvider.TabStop = false;
            this.picSelectServiceProvider.Click += new System.EventHandler(this.picSelectServiceProvider_Click);
            // 
            // picAddServiceProvider
            // 
            this.picAddServiceProvider.BackColor = System.Drawing.Color.White;
            this.picAddServiceProvider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAddServiceProvider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddServiceProvider.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picAddServiceProvider.Location = new System.Drawing.Point(573, 12);
            this.picAddServiceProvider.Name = "picAddServiceProvider";
            this.picAddServiceProvider.Size = new System.Drawing.Size(43, 34);
            this.picAddServiceProvider.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddServiceProvider.TabIndex = 30;
            this.picAddServiceProvider.TabStop = false;
            this.picAddServiceProvider.Click += new System.EventHandler(this.picAddServiceProvider_Click);
            // 
            // picServiceProviderLookup
            // 
            this.picServiceProviderLookup.BackColor = System.Drawing.Color.White;
            this.picServiceProviderLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picServiceProviderLookup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picServiceProviderLookup.Image = global::Icondev.MedManage.Properties.Resources.Binoculars;
            this.picServiceProviderLookup.Location = new System.Drawing.Point(522, 12);
            this.picServiceProviderLookup.Name = "picServiceProviderLookup";
            this.picServiceProviderLookup.Size = new System.Drawing.Size(43, 34);
            this.picServiceProviderLookup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picServiceProviderLookup.TabIndex = 29;
            this.picServiceProviderLookup.TabStop = false;
            this.picServiceProviderLookup.Click += new System.EventHandler(this.picServiceProviderLookup_Click);
            // 
            // txtPracticeName
            // 
            this.txtPracticeName.Location = new System.Drawing.Point(366, 38);
            this.txtPracticeName.Name = "txtPracticeName";
            this.txtPracticeName.Size = new System.Drawing.Size(127, 20);
            this.txtPracticeName.TabIndex = 23;
            this.txtPracticeName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Practice Name";
            // 
            // txtProviderNumber
            // 
            this.txtProviderNumber.Location = new System.Drawing.Point(366, 12);
            this.txtProviderNumber.Name = "txtProviderNumber";
            this.txtProviderNumber.Size = new System.Drawing.Size(127, 20);
            this.txtProviderNumber.TabIndex = 21;
            this.txtProviderNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(260, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Provider Number";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(121, 38);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(127, 20);
            this.txtName.TabIndex = 19;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(121, 12);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(127, 20);
            this.txtSurname.TabIndex = 18;
            this.txtSurname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(66, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Surname";
            // 
            // datagridServiceProviders
            // 
            this.datagridServiceProviders.AllowUserToAddRows = false;
            this.datagridServiceProviders.AllowUserToDeleteRows = false;
            this.datagridServiceProviders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridServiceProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridServiceProviders.Location = new System.Drawing.Point(0, 0);
            this.datagridServiceProviders.Name = "datagridServiceProviders";
            this.datagridServiceProviders.ReadOnly = true;
            this.datagridServiceProviders.Size = new System.Drawing.Size(674, 495);
            this.datagridServiceProviders.TabIndex = 0;
            this.datagridServiceProviders.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datagridServiceProviders_CellMouseDoubleClick);
            this.datagridServiceProviders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datagridServiceProviders_KeyPress);
            // 
            // ServiceProviderLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 593);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ServiceProviderLookup";
            this.Text = "ServiceProviderLookup";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectServiceProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddServiceProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picServiceProviderLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridServiceProviders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picSelectServiceProvider;
        private System.Windows.Forms.PictureBox picAddServiceProvider;
        private System.Windows.Forms.PictureBox picServiceProviderLookup;
        private System.Windows.Forms.TextBox txtPracticeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProviderNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView datagridServiceProviders;
        private System.Windows.Forms.PictureBox picCancel;
        private System.Windows.Forms.PictureBox picOpen;
    }
}
namespace Icondev.MedManage
{
    partial class Bookings
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
            this.dateTravelDateTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTravelDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.picOpen = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.picAdd = new System.Windows.Forms.PictureBox();
            this.picLookup = new System.Windows.Forms.PictureBox();
            this.txtMemberNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grdBookings = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBookings)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.dateTravelDateTo);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.dateTravelDateFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.picOpen);
            this.splitContainer1.Panel1.Controls.Add(this.picClose);
            this.splitContainer1.Panel1.Controls.Add(this.picAdd);
            this.splitContainer1.Panel1.Controls.Add(this.picLookup);
            this.splitContainer1.Panel1.Controls.Add(this.txtMemberNumber);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.txtName);
            this.splitContainer1.Panel1.Controls.Add(this.txtSurname);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdBookings);
            this.splitContainer1.Size = new System.Drawing.Size(862, 497);
            this.splitContainer1.SplitterDistance = 69;
            this.splitContainer1.TabIndex = 0;
            // 
            // dateTravelDateTo
            // 
            this.dateTravelDateTo.CustomFormat = "yyyy/MM/dd";
            this.dateTravelDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTravelDateTo.Location = new System.Drawing.Point(527, 42);
            this.dateTravelDateTo.Name = "dateTravelDateTo";
            this.dateTravelDateTo.Size = new System.Drawing.Size(121, 20);
            this.dateTravelDateTo.TabIndex = 107;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(495, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 106;
            this.label2.Text = "And";
            // 
            // dateTravelDateFrom
            // 
            this.dateTravelDateFrom.CustomFormat = "yyyy/MM/dd";
            this.dateTravelDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTravelDateFrom.Location = new System.Drawing.Point(368, 42);
            this.dateTravelDateFrom.Name = "dateTravelDateFrom";
            this.dateTravelDateFrom.Size = new System.Drawing.Size(121, 20);
            this.dateTravelDateFrom.TabIndex = 105;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 104;
            this.label1.Text = "Travel Date Between ";
            // 
            // picOpen
            // 
            this.picOpen.BackColor = System.Drawing.Color.White;
            this.picOpen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOpen.Image = global::Icondev.MedManage.Properties.Resources.FolderOpened_Yellow;
            this.picOpen.Location = new System.Drawing.Point(9, 14);
            this.picOpen.Name = "picOpen";
            this.picOpen.Size = new System.Drawing.Size(37, 33);
            this.picOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOpen.TabIndex = 103;
            this.picOpen.TabStop = false;
            this.picOpen.Click += new System.EventHandler(this.picOpen_Click);
            // 
            // picClose
            // 
            this.picClose.BackColor = System.Drawing.Color.White;
            this.picClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picClose.Image = global::Icondev.MedManage.Properties.Resources.block_64;
            this.picClose.Location = new System.Drawing.Point(805, 13);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(43, 34);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClose.TabIndex = 101;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // picAdd
            // 
            this.picAdd.BackColor = System.Drawing.Color.White;
            this.picAdd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAdd.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picAdd.Location = new System.Drawing.Point(753, 13);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(43, 34);
            this.picAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAdd.TabIndex = 100;
            this.picAdd.TabStop = false;
            this.picAdd.Visible = false;
            this.picAdd.Click += new System.EventHandler(this.picAdd_Click);
            // 
            // picLookup
            // 
            this.picLookup.BackColor = System.Drawing.Color.White;
            this.picLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLookup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLookup.Image = global::Icondev.MedManage.Properties.Resources.Binoculars;
            this.picLookup.Location = new System.Drawing.Point(702, 13);
            this.picLookup.Name = "picLookup";
            this.picLookup.Size = new System.Drawing.Size(43, 34);
            this.picLookup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLookup.TabIndex = 99;
            this.picLookup.TabStop = false;
            this.picLookup.Click += new System.EventHandler(this.picLookup_Click);
            // 
            // txtMemberNumber
            // 
            this.txtMemberNumber.Location = new System.Drawing.Point(368, 13);
            this.txtMemberNumber.Name = "txtMemberNumber";
            this.txtMemberNumber.Size = new System.Drawing.Size(127, 20);
            this.txtMemberNumber.TabIndex = 95;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(251, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 94;
            this.label8.Text = "Member Number";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(111, 39);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(127, 20);
            this.txtName.TabIndex = 93;
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(112, 13);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(127, 20);
            this.txtSurname.TabIndex = 92;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 91;
            this.label6.Text = "Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 90;
            this.label5.Text = "Surname";
            // 
            // grdBookings
            // 
            this.grdBookings.AllowUserToAddRows = false;
            this.grdBookings.AllowUserToDeleteRows = false;
            this.grdBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBookings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBookings.Location = new System.Drawing.Point(0, 0);
            this.grdBookings.Name = "grdBookings";
            this.grdBookings.ReadOnly = true;
            this.grdBookings.Size = new System.Drawing.Size(862, 424);
            this.grdBookings.TabIndex = 1;
            // 
            // Bookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 497);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Bookings";
            this.Text = "Bookings";
            this.Load += new System.EventHandler(this.Bookings_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBookings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grdBookings;
        private System.Windows.Forms.PictureBox picOpen;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.PictureBox picAdd;
        private System.Windows.Forms.PictureBox picLookup;
        private System.Windows.Forms.TextBox txtMemberNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTravelDateTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTravelDateFrom;

    }
}
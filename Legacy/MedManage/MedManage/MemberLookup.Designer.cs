namespace Icondev.MedManage
{
    partial class MemberLookup
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
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picOpen = new System.Windows.Forms.PictureBox();
            this.dateDOB = new System.Windows.Forms.DateTimePicker();
            this.picSelectMember = new System.Windows.Forms.PictureBox();
            this.picAddMember = new System.Windows.Forms.PictureBox();
            this.picMemberLookup = new System.Windows.Forms.PictureBox();
            this.label58 = new System.Windows.Forms.Label();
            this.txtPassportNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMemberNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.datagridMembers = new System.Windows.Forms.DataGridView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMemberLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridMembers)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.datagridMembers);
            this.splitContainer1.Size = new System.Drawing.Size(878, 463);
            this.splitContainer1.SplitterDistance = 74;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtIdNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.picOpen);
            this.groupBox1.Controls.Add(this.dateDOB);
            this.groupBox1.Controls.Add(this.picSelectMember);
            this.groupBox1.Controls.Add(this.picAddMember);
            this.groupBox1.Controls.Add(this.picMemberLookup);
            this.groupBox1.Controls.Add(this.label58);
            this.groupBox1.Controls.Add(this.txtPassportNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMemberNumber);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.txtSurname);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(878, 74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // txtIdNumber
            // 
            this.txtIdNumber.Location = new System.Drawing.Point(578, 38);
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(121, 20);
            this.txtIdNumber.TabIndex = 89;
            this.txtIdNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(504, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 88;
            this.label2.Text = "ID Number";
            // 
            // picOpen
            // 
            this.picOpen.BackColor = System.Drawing.Color.White;
            this.picOpen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOpen.Image = global::Icondev.MedManage.Properties.Resources.FolderOpened_Yellow;
            this.picOpen.Location = new System.Drawing.Point(12, 13);
            this.picOpen.Name = "picOpen";
            this.picOpen.Size = new System.Drawing.Size(37, 33);
            this.picOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOpen.TabIndex = 87;
            this.picOpen.TabStop = false;
            this.picOpen.Click += new System.EventHandler(this.picOpen_Click);
            // 
            // dateDOB
            // 
            this.dateDOB.CustomFormat = "yyyy/MM/dd";
            this.dateDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateDOB.Location = new System.Drawing.Point(578, 9);
            this.dateDOB.Name = "dateDOB";
            this.dateDOB.Size = new System.Drawing.Size(121, 20);
            this.dateDOB.TabIndex = 32;
            this.dateDOB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // picSelectMember
            // 
            this.picSelectMember.BackColor = System.Drawing.Color.White;
            this.picSelectMember.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSelectMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSelectMember.Image = global::Icondev.MedManage.Properties.Resources.tick_64;
            this.picSelectMember.Location = new System.Drawing.Point(808, 12);
            this.picSelectMember.Name = "picSelectMember";
            this.picSelectMember.Size = new System.Drawing.Size(43, 34);
            this.picSelectMember.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSelectMember.TabIndex = 31;
            this.picSelectMember.TabStop = false;
            this.picSelectMember.Click += new System.EventHandler(this.picSelectMember_Click);
            // 
            // picAddMember
            // 
            this.picAddMember.BackColor = System.Drawing.Color.White;
            this.picAddMember.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picAddMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAddMember.Image = global::Icondev.MedManage.Properties.Resources.plus_64;
            this.picAddMember.Location = new System.Drawing.Point(756, 12);
            this.picAddMember.Name = "picAddMember";
            this.picAddMember.Size = new System.Drawing.Size(43, 34);
            this.picAddMember.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAddMember.TabIndex = 30;
            this.picAddMember.TabStop = false;
            this.picAddMember.Click += new System.EventHandler(this.picAddMember_Click);
            // 
            // picMemberLookup
            // 
            this.picMemberLookup.BackColor = System.Drawing.Color.White;
            this.picMemberLookup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picMemberLookup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMemberLookup.Image = global::Icondev.MedManage.Properties.Resources.Binoculars;
            this.picMemberLookup.Location = new System.Drawing.Point(705, 12);
            this.picMemberLookup.Name = "picMemberLookup";
            this.picMemberLookup.Size = new System.Drawing.Size(43, 34);
            this.picMemberLookup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMemberLookup.TabIndex = 29;
            this.picMemberLookup.TabStop = false;
            this.picMemberLookup.Click += new System.EventHandler(this.picMemberLookup_Click);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(504, 13);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(68, 13);
            this.label58.TabIndex = 27;
            this.label58.Text = "Date Of Birth";
            // 
            // txtPassportNumber
            // 
            this.txtPassportNumber.Location = new System.Drawing.Point(365, 38);
            this.txtPassportNumber.Name = "txtPassportNumber";
            this.txtPassportNumber.Size = new System.Drawing.Size(127, 20);
            this.txtPassportNumber.TabIndex = 23;
            this.txtPassportNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(259, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Passport Number";
            // 
            // txtMemberNumber
            // 
            this.txtMemberNumber.Location = new System.Drawing.Point(365, 12);
            this.txtMemberNumber.Name = "txtMemberNumber";
            this.txtMemberNumber.Size = new System.Drawing.Size(127, 20);
            this.txtMemberNumber.TabIndex = 21;
            this.txtMemberNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Member Number";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 38);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(127, 20);
            this.txtName.TabIndex = 19;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(120, 12);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(127, 20);
            this.txtSurname.TabIndex = 18;
            this.txtSurname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filter_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(65, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Surname";
            // 
            // datagridMembers
            // 
            this.datagridMembers.AllowUserToAddRows = false;
            this.datagridMembers.AllowUserToDeleteRows = false;
            this.datagridMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridMembers.Location = new System.Drawing.Point(0, 0);
            this.datagridMembers.Name = "datagridMembers";
            this.datagridMembers.ReadOnly = true;
            this.datagridMembers.Size = new System.Drawing.Size(878, 385);
            this.datagridMembers.TabIndex = 0;
            this.datagridMembers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.datagridMembers_CellDoubleClick);
            this.datagridMembers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datagridMembers_KeyPress);
            // 
            // MemberLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 463);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MemberLookup";
            this.Text = "MemberLookup";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAddMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMemberLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datagridMembers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView datagridMembers;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassportNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMemberNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.PictureBox picMemberLookup;
        private System.Windows.Forms.PictureBox picAddMember;
        private System.Windows.Forms.PictureBox picSelectMember;
        private System.Windows.Forms.DateTimePicker dateDOB;
        private System.Windows.Forms.PictureBox picOpen;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.Label label2;
    }
}
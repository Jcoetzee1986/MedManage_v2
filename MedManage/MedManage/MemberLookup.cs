using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;

namespace Icondev.MedManage
{
    public partial class MemberLookup : Form
    {
        bool OpenedFromCase = false;
        public string oReturn { get; set; }
        public int MemberID { get; set; }
        public string Surname { get; private set; }
        public string MemberName { get; private set; }
        public string MemberNumber { get; private set; }
        public string PassportNumber { get; private set; }
        public string IDNumber { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public MemberLookup()
        {
            InitializeComponent();
            oReturn = "None";
        }

        public MemberLookup(bool openFromCase,string refSurname, string refName,string refPassport, string refID)
        {
            InitializeComponent();
            OpenedFromCase = openFromCase;
            if (!openFromCase)
            {
                picSelectMember.Visible = false;
            }

            if (refSurname != "")
            {
                txtIdNumber.Text = refID;
                txtName.Text = refName;
                txtPassportNumber.Text = refPassport;
                txtSurname.Text = refSurname;

                datagridMembers.DataSource = oShared.usp_Member_Select_ByFilters(
                    txtSurname.Text
                    , txtName.Text
                    , txtMemberNumber.Text
                    , txtPassportNumber.Text
                    , txtIdNumber.Text
                    , dateDOB.Value
                    , Program.MainClientID
                    );
            }
            
        }

        private void picMemberLookup_Click(object sender, EventArgs e)
        {
            datagridMembers.DataSource = oShared.usp_Member_Select_ByFilters(
                txtSurname.Text
                , txtName.Text
                , txtMemberNumber.Text
                , txtPassportNumber.Text
                , txtIdNumber.Text
                , dateDOB.Value
                , Program.MainClientID
                );
        }

        private void picSelectMember_Click(object sender, EventArgs e)
        {
            if (datagridMembers.CurrentRow != null)
            {
                try
                {
                    oReturn = "Member";
                    MemberID = Convert.ToInt32(datagridMembers.CurrentRow.Cells["MemberID"].Value);
                    Close();
                }
                catch
                { }
            }
            else MessageBox.Show("Please select a member first");
        }

        private void picAddMember_Click(object sender, EventArgs e)
        {
            if (OpenedFromCase)
            {
                oReturn = "New";
                Surname = txtSurname.Text;
                MemberName = txtName.Text;
                MemberNumber = txtMemberNumber.Text;
                PassportNumber = txtPassportNumber.Text;
                IDNumber = txtIdNumber.Text;
                DateOfBirth = dateDOB.Value;
                Close();
            }
            else
            {
                MemberAdd oMemberAdd = new MemberAdd(txtSurname.Text
                    ,txtName.Text
                    ,txtMemberNumber.Text
                    ,txtPassportNumber.Text
                    ,txtIdNumber.Text
                    ,dateDOB.Value);
                oMemberAdd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                oMemberAdd.ShowDialog();
            }
        }

        private void datagridMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MemberAdd oMemberAdd = new MemberAdd(Convert.ToInt32(datagridMembers.Rows[e.RowIndex].Cells["MemberID"].Value.ToString()));
            oMemberAdd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            oMemberAdd.ShowDialog();
        }

        private void picOpen_Click(object sender, EventArgs e)
        {
            if (datagridMembers.CurrentRow != null)
            {
                MemberAdd oMemberAdd = new MemberAdd(Convert.ToInt32(datagridMembers.Rows[datagridMembers.CurrentRow.Index].Cells["MemberID"].Value.ToString()));
                oMemberAdd.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                oMemberAdd.ShowDialog();
            }
            else MessageBox.Show("Please select a member first");
        }

        private void Filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                datagridMembers.DataSource = oShared.usp_Member_Select_ByFilters(
                txtSurname.Text
                , txtName.Text
                , txtMemberNumber.Text
                , txtPassportNumber.Text
                , txtIdNumber.Text
                , dateDOB.Value
                , Program.MainClientID
                );
                datagridMembers.Focus();
            }
        }

        private void datagridMembers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (datagridMembers.CurrentRow != null)
                {
                    try
                    {
                        if (datagridMembers.CurrentRow.Index == 0)
                        {
                            oReturn = "Member";
                            MemberID = Convert.ToInt32(datagridMembers.Rows[datagridMembers.CurrentRow.Index].Cells["MemberID"].Value);
                            Close();
                        }
                        else
                        {
                            oReturn = "Member";
                            MemberID = Convert.ToInt32(datagridMembers.Rows[datagridMembers.CurrentRow.Index - 1].Cells["MemberID"].Value);
                            Close();
                        }
                    }
                    catch
                    { }
                }
                else MessageBox.Show("Please select a member first");
            }
        }

        
    }
}

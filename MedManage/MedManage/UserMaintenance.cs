using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Security;
using System.Threading;

namespace Icondev.MedManage
{
    public partial class UserMaintenance : Form
    {
        public UserMaintenance()
        {
            InitializeComponent();
        }

        private void UserMaintenance_Load(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtPassword.Enabled = true;
            txtUserName.Enabled = true;
            btnNewUser.Visible = false;
            btnAddUser.Visible = true;
            btnUpdateUser.Visible = false;
            btnDeleteUser.Visible = false;
            btnResetPassword.Visible = false;

            MembershipUserCollection Members = Membership.GetAllUsers();

            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                this.comboUsers.Items.Add(user.UserName);
            }
            this.comboRoles.Items.AddRange(Roles.GetAllRoles());

            this.listUserRoles.Items.AddRange(Roles.GetRolesForUser(this.comboUsers.Text));
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                Membership.CreateUser(txtUserName.Text, txtPassword.Text, txtEmail.Text);
                this.comboUsers.Items.Clear();
                MembershipUserCollection Members = Membership.GetAllUsers();

                foreach (MembershipUser user in Membership.GetAllUsers())
                {
                    this.comboUsers.Items.Add(user.UserName);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtPassword.Enabled = true;
            txtUserName.Enabled = true;
            btnNewUser.Visible = false;
            btnAddUser.Visible = true;
            btnUpdateUser.Visible = false;
            btnDeleteUser.Visible = false;
            btnResetPassword.Visible = false;

        }



        private void btnAddRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Roles.RoleExists(this.comboRoles.Text))
                {
                    Roles.CreateRole(this.comboRoles.Text);
                }
                Roles.AddUsersToRole(new string[] { this.comboUsers.Text }, this.comboRoles.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }

            comboRoles.Items.Clear();
            comboUsers.Items.Clear();
            listUserRoles.Items.Clear();
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                this.comboUsers.Items.Add(user.UserName);
            }
            this.comboRoles.Items.AddRange(Roles.GetAllRoles());

            this.listUserRoles.Items.AddRange(Roles.GetRolesForUser(this.comboUsers.Text));
        }

        private void comboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnNewUser.Visible = true;
            btnAddUser.Visible = false;
            btnUpdateUser.Visible = true;
            txtPassword.Enabled = false;
            txtUserName.Enabled = false;
            btnDeleteUser.Visible = true;
            btnResetPassword.Visible = true;

            //Populate user details to update
            MembershipUser user = Membership.GetUser(comboUsers.Text);
            txtUserName.Text = user.UserName;
            txtPassword.Text = "";
            txtEmail.Text = user.Email;

            //Bind user roles
            listUserRoles.Items.Clear();
            this.listUserRoles.Items.AddRange(Roles.GetRolesForUser(this.comboUsers.Text));
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtPassword.Enabled = true;
            txtUserName.Enabled = true;
            btnNewUser.Visible = false;
            btnAddUser.Visible = true;
            btnUpdateUser.Visible = false;
            btnDeleteUser.Visible = false;
            btnResetPassword.Visible = false;
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser(comboUsers.Text);
            user.Email = txtEmail.Text;
            Membership.UpdateUser(user);

            btnNewUser.Visible = true;
            btnAddUser.Visible = false;
            btnUpdateUser.Visible = true;
            txtPassword.Enabled = false;
            txtUserName.Enabled = false;
            btnDeleteUser.Visible = true;
            btnResetPassword.Visible = true;
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will delete all data for the selected user\n\rAre you sure you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Membership.DeleteUser(comboUsers.Text, true);
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtEmail.Text = "";
                txtPassword.Enabled = true;
                txtUserName.Enabled = true;
                btnNewUser.Visible = false;
                btnAddUser.Visible = true;
                btnUpdateUser.Visible = false;
                btnDeleteUser.Visible = false;
                btnResetPassword.Visible = false;

                comboRoles.Items.Clear();
                comboUsers.Items.Clear();
                listUserRoles.Items.Clear();
                foreach (MembershipUser user in Membership.GetAllUsers())
                {
                    this.comboUsers.Items.Add(user.UserName);
                }
                this.comboRoles.Items.AddRange(Roles.GetAllRoles());

                this.listUserRoles.Items.AddRange(Roles.GetRolesForUser(this.comboUsers.Text));
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            MembershipUser user = Membership.GetUser(comboUsers.Text);

            string resetPassword = user.ResetPassword();

            Thread.Sleep(5000);

            string NewPassword = "pass" + DateTime.Today.Ticks.ToString().Substring(0, 4);
            user.ChangePassword(resetPassword, NewPassword);
            MessageBox.Show("The user's new password is : \n\r" + NewPassword);

            this.Cursor = Cursors.Default;

        }

        private void btnRemoveRoleFromUser_Click(object sender, EventArgs e)
        {
            try
            {
                Roles.RemoveUserFromRole(this.comboUsers.Text, this.comboRoles.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }

            comboRoles.Items.Clear();
            comboUsers.Items.Clear();
            listUserRoles.Items.Clear();
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                this.comboUsers.Items.Add(user.UserName);
            }
            this.comboRoles.Items.AddRange(Roles.GetAllRoles());

            this.listUserRoles.Items.AddRange(Roles.GetRolesForUser(this.comboUsers.Text));
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            DialogResult oResult = MessageBox.Show("Are you sure you want to delete thia complete user role?", "Warning", MessageBoxButtons.YesNo);
            if (oResult == DialogResult.Yes)
                try
                {
                    Roles.DeleteRole(this.comboRoles.Text);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message.ToString());
                }
        }
    }
}

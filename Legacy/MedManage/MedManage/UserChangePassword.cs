using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Security;


namespace Icondev.MedManage
{
    public partial class UserChangePassword : Form
    {
        public UserChangePassword()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNewPassword2.Text == txtNewPassword.Text)
            {
                MembershipUser user = Membership.GetUser(Program.Username);
                string name = user.UserName;
                user.ChangePassword(txtOldPassword.Text, txtNewPassword.Text);
                MessageBox.Show("Password Change Successfull");
                this.Close();
            }
            else
            {
                MessageBox.Show("Your passwords do not match");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

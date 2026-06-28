using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class DeleteUserSession : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);

        public DeleteUserSession()
        {
            InitializeComponent();
        }

        private void picDelete_Click(object sender, EventArgs e)
        {
            oCM.usp_Session_User_Case_Delete(Convert.ToInt32(txtCaseID.Text));
            this.Close();
        }
    }
}

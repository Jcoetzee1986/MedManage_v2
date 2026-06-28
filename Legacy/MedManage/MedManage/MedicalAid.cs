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
using System.IO;

namespace Icondev.MedManage
{
    public partial class MedicalAid : Form
    {
        SharedObjects oShared;

        public MedicalAid()
        {
            InitializeComponent();
            oShared = new SharedObjects(Program.oDb);
            datagridMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);

            comboMainClient.DisplayMember = "MainClientName";
            comboMainClient.ValueMember = "MainClientID";
            comboMainClient.DataSource = oShared.usp_MainClient_Select();
        }

        private void picExclusions_Click(object sender, EventArgs e)
        {
            MedicalAidExclusions oExclusions = new MedicalAidExclusions();
            oExclusions.ShowDialog();
        }

        private void picNew_Click(object sender, EventArgs e)
        {
            txtName.Text = "New Medical Aid";
            lblMedAidID.Text = "";
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            if (lblMedAidID.Text == "")
            {
                oShared.usp_MedicalAid_Insert(Convert.ToInt32(comboMainClient.SelectedValue),txtName.Text, dateInitiationDate.Value, dateReInstatementDate.Value, dateTerminationDate.Value, txtPrefix.Text, txtReportTemplate.Text, Program.Username);
            }
            else
            {
                oShared.usp_MedicalAid_Update(Convert.ToInt32(comboMainClient.SelectedValue),Convert.ToInt32(lblMedAidID.Text), txtName.Text, dateInitiationDate.Value, dateReInstatementDate.Value, dateTerminationDate.Value, txtPrefix.Text, txtReportTemplate.Text, Program.Username);
            }
            datagridMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            if (txtName.Text == "New Medical Aid")
            {
                txtName.Text = "";
            }
        }

        private void datagridMedicalAid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                txtName.Text = datagridMedicalAid.Rows[e.RowIndex].Cells["MedicalAidName"].Value.ToString();
                lblMedAidID.Text = datagridMedicalAid.Rows[e.RowIndex].Cells["MedicalAidId"].Value.ToString();
                dateInitiationDate.Value = Convert.ToDateTime(datagridMedicalAid.Rows[e.RowIndex].Cells["MedicalAidInitiationDate"].Value);
                dateReInstatementDate.Value = Convert.ToDateTime(datagridMedicalAid.Rows[e.RowIndex].Cells["MedicalAidReinstatedDate"].Value);
                dateTerminationDate.Value = Convert.ToDateTime(datagridMedicalAid.Rows[e.RowIndex].Cells["MedicalAidTerminatedDate"].Value);
                txtPrefix.Text = datagridMedicalAid.Rows[e.RowIndex].Cells["CasePrefix"].Value.ToString();
                txtReportTemplate.Text = datagridMedicalAid.Rows[e.RowIndex].Cells["ReportTemplate"].Value.ToString();
                comboMainClient.SelectedValue = datagridMedicalAid.Rows[e.RowIndex].Cells["MainClientID"].Value.ToString();
            }
        }

        private void btnAddMainClientImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.ShowDialog();

            if (oDialog.CheckFileExists)
            {
                byte[] oImageBytes;

                //string ext = oDialog.FileName.Substring(oDialog.FileName.LastIndexOf('.'));
                //string path = Application.StartupPath + "\\Images\\";
                //string finalPath = path + "LogoImage" + ext;

                //Images oImage = new Images();
                //oImage.resizeImage(oDialog.FileName, 250, finalPath);

                oImageBytes = File.ReadAllBytes(oDialog.FileName);
                oShared.usp_MainClient_UpdateImage(oImageBytes, Convert.ToInt32(comboMainClient.SelectedValue));
            }
        }
    }
}

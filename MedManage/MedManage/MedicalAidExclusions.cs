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
    public partial class MedicalAidExclusions : Form
    {
        SharedObjects oShared;

        public MedicalAidExclusions()
        {
            InitializeComponent();
            oShared = new SharedObjects(Program.oDb);

            comboMedicalAid.DisplayMember = "MedicalAidName";
            comboMedicalAid.ValueMember = "MedicalAidID";
            comboMedicalAid.DataSource = oShared.usp_MedicalAid_Select(Program.MainClientID);

            comboHospitalGroup.DisplayMember = "Speciality";
            comboHospitalGroup.ValueMember = "SpecialityID";
            comboHospitalGroup.DataSource = oShared.usp_Speciality_Select();            
        }

        private void comboMedicalAid_SelectedIndexChanged(object sender, EventArgs e)
        {
            datagridMedicalAidExclusions.DataSource = oShared.usp_MedicalAid_Exclusion_Select_ByMedicalAid(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()));
        }

        private void picRemove_Click(object sender, EventArgs e)
        {
            oShared.usp_MedicalAid_Exclusion_Delete(
                Convert.ToInt32(comboMedicalAid.SelectedValue.ToString())
                , datagridMedicalAidExclusions.Rows[datagridMedicalAidExclusions.CurrentRow.Index].Cells["BaseTariffID"].Value.ToString()
                , Program.Username);
            datagridMedicalAidExclusions.DataSource = oShared.usp_MedicalAid_Exclusion_Select_ByMedicalAid(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()));
        }

        private void datagridMedicalAidExclusions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //comboExclusion.SelectedValue = datagridMedicalAidExclusions.Rows[e.RowIndex].Cells["ExclusionID"].Value.ToString();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            if (comboExclusion.SelectedValue != null)
            {
                //Individual Tarriff
                oShared.usp_MedicalAid_Exclusion_Insert(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()), comboExclusion.SelectedValue.ToString(), Program.Username);
                datagridMedicalAidExclusions.DataSource = oShared.usp_MedicalAid_Exclusion_Select_ByMedicalAid(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()));
            }
            else
            {
                MessageBox.Show("Invalid Selection");
            }
        }

        private void comboHospitalGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboExclusion.DisplayMember = "TariffDescription";
            //comboExclusion.ValueMember = "BaseTariffID";
            //comboExclusion.DataSource = oShared.usp_Tariff_Select_Distinct_BySpeciality(Convert.ToInt32(comboHospitalGroup.SelectedValue.ToString()));
        }

        private void picSaveByHospitalType_Click(object sender, EventArgs e)
        {
            //Individual Tarriff
            oShared.usp_MedicalAid_Exclusion_Insert_BySpeciality(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()), Convert.ToInt32(comboHospitalGroup.SelectedValue.ToString()), Program.Username);
            datagridMedicalAidExclusions.DataSource = oShared.usp_MedicalAid_Exclusion_Select_ByMedicalAid(Convert.ToInt32(comboMedicalAid.SelectedValue.ToString()));
        
        }

    }
}

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
    public partial class rpt_CaseLetter : Form
    {
        int CaseID;
        int MedicalAidID;
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public rpt_CaseLetter(int caseID,int medicalAidID)
        {
            CaseID = caseID;
            CaseLetterNote oCaseLetterNote = new CaseLetterNote(CaseID);
            oCaseLetterNote.ShowDialog();

            MedicalAidID = medicalAidID;

            InitializeComponent();
        }

        private void rpt_CaseLetter_Load(object sender, EventArgs e)
        {
            DataTable oDt = oShared.usp_MedicalAid_Select_ByMedicalAidID(MedicalAidID);

            ReportsDataSet.EnforceConstraints = false;
            rptCaseLetter.LocalReport.ReportEmbeddedResource = "Icondev.MedManage.Reports." + oDt.Rows[0]["ReportTemplate"].ToString();
            
            usp_rpt_Cases_Select_CaseIDTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_CaseID,CaseID);
            usp_Case_ICD_SelectTableAdapter.Fill(this.ReportsDataSet.usp_Case_ICD_Select, CaseID);
            usp_Case_Tariff_SelectTableAdapter.Fill(this.ReportsDataSet.usp_Case_Tariff_Select, CaseID);
            usp_Case_CPT_SelectTableAdapter.Fill(this.ReportsDataSet.usp_Case_CPT_Select, CaseID);
            usp_Case_FacilityType_SelectTableAdapter.Fill(this.ReportsDataSet.usp_Case_FacilityType_Select, CaseID);
            usp_rpt_CaseNote_SelectLastNote_CaseIDTableAdapter.Fill(this.ReportsDataSet.usp_rpt_CaseNote_SelectLastNote_CaseID, CaseID);
            rptCaseLetter.RefreshReport();
        }
    }
}

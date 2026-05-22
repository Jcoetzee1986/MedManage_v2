using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Icondev.MedManage
{
    public partial class rpt_MyCasesExport : Form
    {
        string CaseNumber;
        string MemberNumber;
        string MemberName;
        string MemberSurname;
        DateTime MemberDOB;
        DateTime CaseCreated;
        DateTime CaseCreatedEnd;
        string PracticeName;
        int MedicalFunder;
        string PrimaryIcd;
        string PrimaryCpt;
        int StatusID;
        int CaseType;

        public rpt_MyCasesExport(string caseNumber, string memberNumber, string memberName, string memberSurname, DateTime memberDOB
            , DateTime caseCreated, DateTime caseCreatedEnd, string practiceName, int medicalFunder, string primaryIcd, string primaryCpt, int statusID
            ,int caseType)
        {
            InitializeComponent();

            CaseNumber = caseNumber;
            MemberNumber = memberNumber;
            MemberName = memberName;
            MemberSurname = memberSurname;
            MemberDOB = memberDOB;
            CaseCreated = caseCreated;
            CaseCreatedEnd = caseCreatedEnd;
            PracticeName = practiceName;
            MedicalFunder = medicalFunder;
            PrimaryIcd = primaryIcd;
            PrimaryCpt = primaryCpt;
            StatusID = statusID;
            CaseType = caseType;
        }

        private void rpt_MyCasesExport_Load(object sender, EventArgs e)
        {

            // INFO: This line of code loads data into the 'ReportsDataSet.usp_rpt_Cases_Select_Filters' table. You can move, or remove it, as needed.
            this.ReportsDataSet.EnforceConstraints = false;
            this.usp_rpt_Cases_Select_FiltersTableAdapter.Fill(this.ReportsDataSet.usp_rpt_Cases_Select_Filters
                ,CaseNumber
                ,MemberNumber
                ,MemberName
                ,MemberSurname
                ,MemberDOB
                , CaseCreated
                , CaseCreatedEnd
                , PracticeName
                ,MedicalFunder
                ,PrimaryIcd
                ,PrimaryCpt
                ,StatusID
                , Program.MainClientID
                ,CaseType);

            this.reportViewer1.RefreshReport();
        }
    }
}

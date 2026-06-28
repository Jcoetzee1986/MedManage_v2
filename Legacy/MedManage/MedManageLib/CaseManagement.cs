using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib
{
    public class CaseManagement
    {
        private Database oDb;
        public CaseManagement(Database oDatabase)
        {
            oDb = oDatabase;
        }

        public DataTable usp_Case_CPT_Insert(int CaseID_CPTID, int CaseID, int CPTID, DateTime DateOfProcedure, bool PrimaryCode, bool SecondaryCode, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_CPT_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID_CPTID", DbType.Int32, CaseID_CPTID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "CPTID", DbType.Int32, CPTID);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "PrimaryCode", DbType.Boolean, PrimaryCode);
            oDb.AddInParameter(sprocCmd, "SecondaryCode", DbType.Boolean, SecondaryCode);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Exclusion_Insert(int CaseID, int ExclusionID, string Comment, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Exclusion_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, ExclusionID);
            oDb.AddInParameter(sprocCmd, "Comment", DbType.String, Comment);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_FacilityType_Insert(int FacilityTypeID, int CaseID, DateTime DateAdmitted, DateTime DateDischarged, decimal LOS, string FacilityTypeCode, int MinutesOnVentilator, string Comments, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_FacilityType_Insert");
            oDb.AddInParameter(sprocCmd, "FacilityTypeID", DbType.Int32, FacilityTypeID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "DateAdmitted", DbType.DateTime2, DateAdmitted);
            oDb.AddInParameter(sprocCmd, "DateDischarged", DbType.DateTime2, DateDischarged);
            oDb.AddInParameter(sprocCmd, "LOS", DbType.Decimal, LOS);
            oDb.AddInParameter(sprocCmd, "FacilityTypeCode", DbType.String, FacilityTypeCode);
            oDb.AddInParameter(sprocCmd, "MinutesOnVentilator", DbType.Int32, MinutesOnVentilator);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_ICD_Insert(int CaseID, int ICDID, DateTime DateOfProcedure, bool PrimaryCode, bool SecondaryCode, bool CoMorbidityCode, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_ICD_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ICDID", DbType.Int32, ICDID);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "PrimaryCode", DbType.Boolean, PrimaryCode);
            oDb.AddInParameter(sprocCmd, "SecondaryCode", DbType.Boolean, SecondaryCode);
            oDb.AddInParameter(sprocCmd, "CoMorbidityCode", DbType.Boolean, CoMorbidityCode);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Tariff_Insert(int CaseID_TariffID, int CaseID, int TariffID, decimal Value, decimal Qty,decimal AgreedRateOverride, bool ValueIsTotal, bool Rejected, DateTime DateOfProcedure, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Case_Tariff_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID_TariffID", DbType.Int32, CaseID_TariffID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
            oDb.AddInParameter(sprocCmd, "Value", DbType.Decimal, Value);
            oDb.AddInParameter(sprocCmd, "Qty", DbType.Decimal, Qty);
            oDb.AddInParameter(sprocCmd, "AgreedRateOverride", DbType.Decimal, AgreedRateOverride);
            oDb.AddInParameter(sprocCmd, "ValueIsTotal", DbType.Boolean, ValueIsTotal);
            oDb.AddInParameter(sprocCmd, "Rejected", DbType.Boolean, Rejected);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_CPT_Update(int CaseID, int CPTID, DateTime DateOfProcedure, bool PrimaryCode, bool SecondaryCode, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_CPT_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "CPTID", DbType.Int32, CPTID);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "PrimaryCode", DbType.Boolean, PrimaryCode);
            oDb.AddInParameter(sprocCmd, "SecondaryCode", DbType.Boolean, SecondaryCode);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Exclusion_Update(int CaseID, int ExclusionID, string Comment, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Exclusion_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, ExclusionID);
            oDb.AddInParameter(sprocCmd, "Comment", DbType.String, Comment);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_FacilityType_Update(int FacilityTypeID, int CaseID, DateTime DateAdmitted, DateTime DateDischarged, decimal LOS, string FacilityTypeCode, int MinutesOnVentilator, string Comments, DateTime DateInserted, string UserID, int ID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_FacilityType_Update");
            oDb.AddInParameter(sprocCmd, "FacilityTypeID", DbType.Int32, FacilityTypeID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "DateAdmitted", DbType.DateTime2, DateAdmitted);
            oDb.AddInParameter(sprocCmd, "DateDischarged", DbType.DateTime2, DateDischarged);
            oDb.AddInParameter(sprocCmd, "LOS", DbType.Decimal, LOS);
            oDb.AddInParameter(sprocCmd, "FacilityTypeCode", DbType.String, FacilityTypeCode);
            oDb.AddInParameter(sprocCmd, "MinutesOnVentilator", DbType.Int32, MinutesOnVentilator);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "ID", DbType.Int32, ID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_ICD_Update(int CaseID, int ICDID, DateTime DateOfProcedure, bool PrimaryCode, bool SecondaryCode, bool CoMorbidityCode, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_ICD_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ICDID", DbType.Int32, ICDID);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "PrimaryCode", DbType.Boolean, PrimaryCode);
            oDb.AddInParameter(sprocCmd, "SecondaryCode", DbType.Boolean, SecondaryCode);
            oDb.AddInParameter(sprocCmd, "CoMorbidityCode", DbType.Boolean, CoMorbidityCode);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Tariff_Update(int CaseID, int TariffID, decimal Value, DateTime DateOfProcedure, DateTime DateInserted, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Case_Tariff_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
            oDb.AddInParameter(sprocCmd, "Value", DbType.Decimal, Value);
            oDb.AddInParameter(sprocCmd, "DateOfProcedure", DbType.Date, DateOfProcedure);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_Checklist_Delete(int CaseID, int ChecklistTemplateID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Checklist_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Checklist_Insert(int CaseID, int ChecklistTemplateID, bool Checked, string UserID, DateTime Date, string Comments, bool NotApplicable)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Checklist_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            oDb.AddInParameter(sprocCmd, "Checked", DbType.Boolean, Checked);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "Date", DbType.DateTime2, Date);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            oDb.AddInParameter(sprocCmd, "NotApplicable", DbType.Boolean, NotApplicable);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Checklist_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Checklist_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Checklist_Update(int CaseID, int ChecklistTemplateID, bool Checked, string UserID, DateTime Date, string Comments, bool NotApplicable)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Checklist_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            oDb.AddInParameter(sprocCmd, "Checked", DbType.Boolean, Checked);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "Date", DbType.DateTime2, Date);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            oDb.AddInParameter(sprocCmd, "NotApplicable", DbType.Boolean, NotApplicable);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Case_CPT_Delete(int CaseID_CPTID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_CPT_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID_CPTID", DbType.Int32, CaseID_CPTID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_CPT_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_CPT_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_ICD_Delete(int CaseID, int ICDID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_ICD_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ICDID", DbType.Int32, ICDID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_ICD_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_ICD_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_Tariff_Delete(int CaseID_TariffID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Case_Tariff_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID_TariffID", DbType.Int32, CaseID_TariffID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Tariff_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Case_Tariff_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_CaseNote_Delete(int CaseNoteID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseNote_Delete");
            oDb.AddInParameter(sprocCmd, "CaseNoteID", DbType.Int32, CaseNoteID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_CaseNote_Insert(string CaseNote, string UserID, decimal InterimAmount, string CaseNumber, DateTime DateCreated, int CaseID
            , decimal InterimAccomodation
            , decimal InterimHospital
            , decimal InterimDialysis
            , decimal InterimPhysio
            , decimal InterimRadiology
            , decimal InterimSpecialist
            , decimal InterimTransport
            , decimal InterimScript)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseNote_Insert");
            oDb.AddInParameter(sprocCmd, "CaseNote", DbType.String, CaseNote);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "InterimAmount", DbType.Decimal, InterimAmount);
            oDb.AddInParameter(sprocCmd, "DateCreated", DbType.DateTime2, DateCreated);
            oDb.AddInParameter(sprocCmd, "CaseNumber", DbType.String, CaseNumber);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "InterimAccomodation", DbType.Decimal, InterimAccomodation);
            oDb.AddInParameter(sprocCmd, "InterimHospital", DbType.Decimal, InterimHospital);
            oDb.AddInParameter(sprocCmd, "InterimDialysis", DbType.Decimal, InterimDialysis);
            oDb.AddInParameter(sprocCmd, "InterimPhysio", DbType.Decimal, InterimPhysio);
            oDb.AddInParameter(sprocCmd, "InterimRadiology", DbType.Decimal, InterimRadiology);
            oDb.AddInParameter(sprocCmd, "InterimSpecialist", DbType.Decimal, InterimSpecialist);
            oDb.AddInParameter(sprocCmd, "InterimTransport", DbType.Decimal, InterimTransport);
            oDb.AddInParameter(sprocCmd, "InterimScript", DbType.Decimal, InterimScript);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_CaseNote_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseNote_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CaseNote_Update(int CaseNoteID, string CaseNote, string UserID, decimal InterimAmount, string CaseNumber, DateTime DateCreated
            , decimal InterimAccomodation
            , decimal InterimHospital
            , decimal InterimDialysis
            , decimal InterimPhysio
            , decimal InterimRadiology
            , decimal InterimSpecialist
            , decimal InterimTransport
            , decimal InterimScript)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseNote_Update");
            oDb.AddInParameter(sprocCmd, "CaseNoteID", DbType.Int32, CaseNoteID);
            oDb.AddInParameter(sprocCmd, "CaseNote", DbType.String, CaseNote);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "InterimAmount", DbType.Decimal, InterimAmount);
            oDb.AddInParameter(sprocCmd, "CaseNumber", DbType.String, CaseNumber);
            oDb.AddInParameter(sprocCmd, "DateCreated", DbType.DateTime2, DateCreated);
            oDb.AddInParameter(sprocCmd, "InterimAccomodation", DbType.Decimal, InterimAccomodation);
            oDb.AddInParameter(sprocCmd, "InterimHospital", DbType.Decimal, InterimHospital);
            oDb.AddInParameter(sprocCmd, "InterimDialysis", DbType.Decimal, InterimDialysis);
            oDb.AddInParameter(sprocCmd, "InterimPhysio", DbType.Decimal, InterimPhysio);
            oDb.AddInParameter(sprocCmd, "InterimRadiology", DbType.Decimal, InterimRadiology);
            oDb.AddInParameter(sprocCmd, "InterimSpecialist", DbType.Decimal, InterimSpecialist);
            oDb.AddInParameter(sprocCmd, "InterimTransport", DbType.Decimal, InterimTransport);
            oDb.AddInParameter(sprocCmd, "InterimScript", DbType.Decimal, InterimScript);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_CaseComment_Delete(int CaseCommentID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseComment_Delete");
            oDb.AddInParameter(sprocCmd, "CaseCommentID", DbType.Int32, CaseCommentID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_CaseComment_Insert(string CaseComment, string UserID, DateTime DateCreated, int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseComment_Insert");
            oDb.AddInParameter(sprocCmd, "CaseComment", DbType.String, CaseComment);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "DateCreated", DbType.DateTime2, DateCreated);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_CaseComment_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseComment_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CaseComment_Update(int CaseCommentID, string CaseComment, string UserID, DateTime DateCreated)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseComment_Update");
            oDb.AddInParameter(sprocCmd, "CaseCommentID", DbType.Int32, CaseCommentID);
            oDb.AddInParameter(sprocCmd, "CaseComment", DbType.String, CaseComment);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "DateCreated", DbType.DateTime2, DateCreated);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Cases_Delete(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Insert(string AuthNumber, string AccountNr, int MemberID, int ReferToID
            , int ReferFromID, DateTime AdmissionDate, DateTime AdmissionTime, DateTime DischargeDate
            , DateTime DischargeTime, int AuthTypeID, bool WCA_IOD, decimal TotalLengthOfStay
            , decimal TotalAmount, decimal FinalInvoiceAmount, string FinalInvoiceAmountUpdated
            , int StatusID, string CaseDescription, string UserID, string Changes, string Limits
            , string Exclusions, bool HasBooking, DateTime ChangeToCaseDate, decimal PenaltyPercentage
            , int CaseCategoryID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Insert");
            oDb.AddInParameter(sprocCmd, "AuthNumber", DbType.String, AuthNumber);
            oDb.AddInParameter(sprocCmd, "AccountNr", DbType.String, AccountNr);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ReferToID", DbType.Int32, ReferToID);
            oDb.AddInParameter(sprocCmd, "ReferFromID", DbType.Int32, ReferFromID);
            oDb.AddInParameter(sprocCmd, "AdmissionDate", DbType.Date, AdmissionDate);
            oDb.AddInParameter(sprocCmd, "AdmissionTime", DbType.Time, AdmissionTime);
            oDb.AddInParameter(sprocCmd, "DischargeDate", DbType.Date, DischargeDate);
            oDb.AddInParameter(sprocCmd, "DischargeTime", DbType.Time, DischargeTime);
            oDb.AddInParameter(sprocCmd, "AuthTypeID", DbType.Int32, AuthTypeID);
            oDb.AddInParameter(sprocCmd, "WCA_IOD", DbType.Boolean, WCA_IOD);
            oDb.AddInParameter(sprocCmd, "TotalLengthOfStay", DbType.Decimal, TotalLengthOfStay);
            oDb.AddInParameter(sprocCmd, "TotalAmount", DbType.Decimal, TotalAmount);
            oDb.AddInParameter(sprocCmd, "FinalInvoiceAmount", DbType.Decimal, FinalInvoiceAmount);
            oDb.AddInParameter(sprocCmd, "FinalInvoiceAmountUpdated", DbType.String, FinalInvoiceAmountUpdated);
            
            oDb.AddInParameter(sprocCmd, "StatusID", DbType.Int32, StatusID);
            oDb.AddInParameter(sprocCmd, "CaseDescription", DbType.String, CaseDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "Changes", DbType.String, Changes);
            oDb.AddInParameter(sprocCmd, "Limits", DbType.String, Limits);
            oDb.AddInParameter(sprocCmd, "Exclusions", DbType.String, Exclusions);
            oDb.AddInParameter(sprocCmd, "HasBooking", DbType.Boolean, HasBooking);
            oDb.AddInParameter(sprocCmd, "ChangeToCaseDate", DbType.DateTime2, ChangeToCaseDate);
            oDb.AddInParameter(sprocCmd, "PenaltyPercentage", DbType.Decimal, PenaltyPercentage);

            oDb.AddInParameter(sprocCmd, "CaseCategoryID", DbType.Int32, CaseCategoryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Select_PossibleDuplicate(int MemberID, int ReferToID, DateTime AdmissionDate)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_PossibleDuplicate");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ReferToID", DbType.Int32, ReferToID);
            oDb.AddInParameter(sprocCmd, "AdmissionDate", DbType.Date, AdmissionDate);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Select_Filters(string AuthNumber, string memberNumber, string Name, string Surname, 
            DateTime DateOfBirth, DateTime DateCreated, DateTime DateCreatedEnd, string PracticeName, int MedicalFunder, string PrimaryICD, 
            string PrimaryCPT, int StatusID, int MainClientID, int CaseType)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_Filters");
            oDb.AddInParameter(sprocCmd, "AuthNumber", DbType.String, AuthNumber);
            oDb.AddInParameter(sprocCmd, "memberNumber", DbType.String, memberNumber);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "DateOfBirth", DbType.Date, DateOfBirth);
            oDb.AddInParameter(sprocCmd, "DateCreated", DbType.Date, DateCreated);
            oDb.AddInParameter(sprocCmd, "DateCreatedEnd", DbType.Date, DateCreatedEnd);
            oDb.AddInParameter(sprocCmd, "PracticeName", DbType.String, PracticeName);
            oDb.AddInParameter(sprocCmd, "MedicalFunder", DbType.Int32, MedicalFunder);
            oDb.AddInParameter(sprocCmd, "PrimaryICD", DbType.String, PrimaryICD);
            oDb.AddInParameter(sprocCmd, "PrimaryCPT", DbType.String, PrimaryCPT);
            oDb.AddInParameter(sprocCmd, "StatusID", DbType.Int32, StatusID);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "CaseType", DbType.Int32, CaseType);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Update(int CaseID, string AuthNumber, string AccountNr, int MemberID
            , int ReferToID, int ReferFromID, DateTime AdmissionDate, DateTime AdmissionTime
            , DateTime DischargeDate, DateTime DischargeTime, int AuthTypeID, bool WCA_IOD
            , decimal TotalLengthOfStay, decimal TotalAmount, decimal FinalInvoiceAmount
            , string FinalInvoiceAmountUpdated, int StatusID, string CaseDescription, string UserID
            , string Changes, string Limits, string Exclusions, bool HasBooking, DateTime ChangeToCaseDate
            , decimal PenaltyPercentage
            , int CaseCategoryID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "AuthNumber", DbType.String, AuthNumber);
            oDb.AddInParameter(sprocCmd, "AccountNr", DbType.String, AccountNr);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ReferToID", DbType.Int32, ReferToID);
            oDb.AddInParameter(sprocCmd, "ReferFromID", DbType.Int32, ReferFromID);
            oDb.AddInParameter(sprocCmd, "AdmissionDate", DbType.Date, AdmissionDate);
            oDb.AddInParameter(sprocCmd, "AdmissionTime", DbType.Time, AdmissionTime);
            oDb.AddInParameter(sprocCmd, "DischargeDate", DbType.Date, DischargeDate);
            oDb.AddInParameter(sprocCmd, "DischargeTime", DbType.Time, DischargeTime);
            oDb.AddInParameter(sprocCmd, "AuthTypeID", DbType.Int32, AuthTypeID);
            oDb.AddInParameter(sprocCmd, "WCA_IOD", DbType.Boolean, WCA_IOD);
            oDb.AddInParameter(sprocCmd, "TotalLengthOfStay", DbType.Decimal, TotalLengthOfStay);
            oDb.AddInParameter(sprocCmd, "TotalAmount", DbType.Decimal, TotalAmount);
            oDb.AddInParameter(sprocCmd, "FinalInvoiceAmount", DbType.Decimal, FinalInvoiceAmount);
            oDb.AddInParameter(sprocCmd, "FinalInvoiceAmountUpdated", DbType.String, FinalInvoiceAmountUpdated);
            
            oDb.AddInParameter(sprocCmd, "StatusID", DbType.Int32, StatusID);
            oDb.AddInParameter(sprocCmd, "CaseDescription", DbType.String, CaseDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "Changes", DbType.String, Changes);
            oDb.AddInParameter(sprocCmd, "Limits", DbType.String, Limits);
            oDb.AddInParameter(sprocCmd, "Exclusions", DbType.String, Exclusions);
            oDb.AddInParameter(sprocCmd, "HasBooking", DbType.Boolean, HasBooking);
            oDb.AddInParameter(sprocCmd, "ChangeToCaseDate", DbType.DateTime2, ChangeToCaseDate);
            oDb.AddInParameter(sprocCmd, "PenaltyPercentage", DbType.Decimal, PenaltyPercentage);

            oDb.AddInParameter(sprocCmd, "CaseCategoryID", DbType.Int32, CaseCategoryID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Cases_Select_Last30Cases(string UserID, int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_Last30Cases");
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Update_Status(int CaseID, int StatusID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Update_Status");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "StatusID", DbType.Int32, StatusID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Cases_Select_ByMemberID(int MemberID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_ByMemberID");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Cases_Select_ByMemberID_ExclCaseID(int MemberID,int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_ByMemberID_ExclCaseID");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_ChecklistTemplate_Delete(int ChecklistTemplateID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChecklistTemplate_Delete");
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ChecklistTemplate_Insert(string ChecklistPrompt)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChecklistTemplate_Insert");
            oDb.AddInParameter(sprocCmd, "ChecklistPrompt", DbType.String, ChecklistPrompt);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ChecklistTemplate_Select(int ChecklistTemplateID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChecklistTemplate_Select");
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ChecklistTemplate_Update(int ChecklistTemplateID, string ChecklistPrompt)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChecklistTemplate_Update");
            oDb.AddInParameter(sprocCmd, "ChecklistTemplateID", DbType.Int32, ChecklistTemplateID);
            oDb.AddInParameter(sprocCmd, "ChecklistPrompt", DbType.String, ChecklistPrompt);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_CaseStatus_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseStatus_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CaseType_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseType_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CaseType_Select_ForFilters()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseType_Select_ForFilters");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_FacilityType_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_FacilityType_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_FacilityType_Delete(int ID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_FacilityType_Delete");
            oDb.AddInParameter(sprocCmd, "ID", DbType.Int32, ID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Case_Exclusion_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Exclusion_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Case_Exclusion_Delete(int CaseID, int TariffID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Exclusion_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, TariffID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Session_User_Case_Select_ByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Session_User_Case_Select_ByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Session_User_Case_Insert(int CaseID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Session_User_Case_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Session_User_Case_Delete(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Session_User_Case_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_CaseLetterNote_Select_ByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseLetterNote_Select_ByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CaseLetterNote_Insert(int CaseID, string Note, bool IncludeDischargeForm, bool IncludeReferralLetter)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseLetterNote_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "Note", DbType.String, Note);
            oDb.AddInParameter(sprocCmd, "IncludeDischargeForm", DbType.Boolean, IncludeDischargeForm);
            oDb.AddInParameter(sprocCmd, "IncludeReferralLetter", DbType.Boolean, IncludeReferralLetter);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Case_UpdateMemberMedicalAid(int CaseID, string CaseNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_UpdateMemberMedicalAid");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "CaseNumber", DbType.String, CaseNumber);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_Link_Delete(int ParentCase, int ChildCase)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Link_Delete");
            oDb.AddInParameter(sprocCmd, "ParentCase", DbType.Int32, ParentCase);
            oDb.AddInParameter(sprocCmd, "ChildCase", DbType.Int32, ChildCase);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable() ;
        }
        public DataTable usp_Case_Link_Insert(int ParentCase, int ChildCase)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Link_Insert");
            oDb.AddInParameter(sprocCmd, "ParentCase", DbType.Int32, ParentCase);
            oDb.AddInParameter(sprocCmd, "ChildCase", DbType.Int32, ChildCase);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Link_Select(int Case)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Link_Select");
            oDb.AddInParameter(sprocCmd, "Case", DbType.Int32, Case);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_Copy(int CaseID, bool UseSameAuthNumber, bool LinkToParentCase, bool CopyICD10, bool CopyCPT
            , bool CopyTariff, bool CopyDaysInCare, int AdmissionDateDiff, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Copy");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "UseSameAuthNumber", DbType.Boolean, UseSameAuthNumber);
            oDb.AddInParameter(sprocCmd, "LinkToParentCase", DbType.Boolean, LinkToParentCase);
            oDb.AddInParameter(sprocCmd, "CopyICD10", DbType.Boolean, CopyICD10);
            oDb.AddInParameter(sprocCmd, "CopyCPT", DbType.Boolean, CopyCPT);
            oDb.AddInParameter(sprocCmd, "CopyTariff", DbType.Boolean, CopyTariff);
            oDb.AddInParameter(sprocCmd, "CopyDaysInCare", DbType.Boolean, CopyDaysInCare);
            oDb.AddInParameter(sprocCmd, "AdmissionDateDiff", DbType.Int32, AdmissionDateDiff);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_Case_LinkedFile_SelectByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_LinkedFile_SelectByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_LinkedFile_DeleteByCase_LinkedFileID(int Case_LinkedFileID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_LinkedFile_DeleteByCase_LinkedFileID");
            oDb.AddInParameter(sprocCmd, "Case_LinkedFileID", DbType.Int32, Case_LinkedFileID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_LinkedFile_Insert(int CaseID, int MemberID, string FilePath, string FileName, string FileType, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_LinkedFile_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "FilePath", DbType.String, FilePath);
            oDb.AddInParameter(sprocCmd, "FileName", DbType.String, FileName);
            oDb.AddInParameter(sprocCmd, "FileType", DbType.String, FileType);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Case_NappiCodes_Select(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_NappiCodes_Select");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_NappiCodes_Insert(int CaseID_NappiID, int CaseID, int NappiID, decimal Value, decimal Quantity
            , bool Dispensary, bool Ward, bool Theater, bool TTO, bool o201, DateTime Date, string UserId)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_NappiCodes_Insert");
            oDb.AddInParameter(sprocCmd, "CaseID_NappiID", DbType.Int32, CaseID_NappiID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "NappiID", DbType.Int32, NappiID);
            oDb.AddInParameter(sprocCmd, "Value", DbType.Decimal, Value);
            oDb.AddInParameter(sprocCmd, "Quantity", DbType.Decimal, Quantity);
            oDb.AddInParameter(sprocCmd, "Dispensary", DbType.Boolean, Dispensary);
            oDb.AddInParameter(sprocCmd, "Ward", DbType.Boolean, Ward);
            oDb.AddInParameter(sprocCmd, "Theater", DbType.Boolean, Theater);
            oDb.AddInParameter(sprocCmd, "TTO", DbType.Boolean, TTO);
            oDb.AddInParameter(sprocCmd, "0201", DbType.Boolean, o201);
            oDb.AddInParameter(sprocCmd, "Date", DbType.Date, Date);
            oDb.AddInParameter(sprocCmd, "UserId", DbType.String, UserId);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_NappiCodes_Delete(int CaseID_NappiID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_NappiCodes_Delete");
            oDb.AddInParameter(sprocCmd, "CaseID_NappiID", DbType.Int32, CaseID_NappiID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_CaseCategory_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CaseCategory_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

    }
}
  //public DataTable usp_Episode_Case_Delete(int CaseID, int EpisodeID)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Case_Delete");
  //          oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
  //          oDb.AddInParameter(sprocCmd, "EpisodeID", DbType.Int32, EpisodeID);
  //          oDb.ExecuteDataSet(sprocCmd);
  //          return new DataTable();
  //      }
  //      public DataTable usp_Episode_Case_Insert(int CaseID, int EpisodeID, string UserID, DateTime DateCreated)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Case_Insert");
  //          oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
  //          oDb.AddInParameter(sprocCmd, "EpisodeID", DbType.Int32, EpisodeID);
  //          oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
  //          oDb.AddInParameter(sprocCmd, "DateCreated", DbType.Date, DateCreated);
  //          oDb.ExecuteDataSet(sprocCmd);
  //          return new DataTable();
  //      }
  //      public DataTable usp_Episode_Case_Select_ByEpisodeID(int EpisodeID)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Case_Select_ByEpisodeID");
  //          oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, EpisodeID);
  //          return oDb.ExecuteDataSet(sprocCmd).Tables[0];
  //      }
  //      public DataSet usp_Episode_Select(int EpisodeID)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Select");
  //          oDb.AddInParameter(sprocCmd, "EpisodeID", DbType.Int32, EpisodeID);
  //          return oDb.ExecuteDataSet(sprocCmd);
  //      }
  //      public DataTable usp_Episode_Insert(string EpisodeDescription, int MemberID, string UserID, DateTime DateCreated)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Insert");
  //          oDb.AddInParameter(sprocCmd, "EpisodeDescription", DbType.String, EpisodeDescription);
  //          oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
  //          oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
  //          oDb.AddInParameter(sprocCmd, "DateCreated", DbType.Date, DateCreated);
  //          return oDb.ExecuteDataSet(sprocCmd).Tables[0];
  //      }
  //      public DataTable usp_Episode_Update(int EpisodeID, string EpisodeDescription, string UserID)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Update");
  //          oDb.AddInParameter(sprocCmd, "EpisodeID", DbType.Int32, EpisodeID);
  //          oDb.AddInParameter(sprocCmd, "EpisodeDescription", DbType.String, EpisodeDescription);
  //          oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
  //          oDb.ExecuteDataSet(sprocCmd);
  //          return new DataTable();
  //      }
  //      public DataTable usp_Episode_Select_By_Filters(int EpisodeID, string RelatedCaseNumber, string MemberName, string MemberSurname, string MemberNumber, string MemberIDNumber)
  //      {
  //          DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Episode_Select_By_Filters");
  //          oDb.AddInParameter(sprocCmd, "EpisodeID", DbType.Int32, EpisodeID);
  //          oDb.AddInParameter(sprocCmd, "RelatedCaseNumber", DbType.String, RelatedCaseNumber);
  //          oDb.AddInParameter(sprocCmd, "MemberName", DbType.String, MemberName);
  //          oDb.AddInParameter(sprocCmd, "MemberSurname", DbType.String, MemberSurname);
  //          oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
  //          oDb.AddInParameter(sprocCmd, "MemberIDNumber", DbType.String, MemberIDNumber);
  //          return oDb.ExecuteDataSet(sprocCmd).Tables[0];
  //      }

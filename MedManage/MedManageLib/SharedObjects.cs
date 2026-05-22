using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib.Shared
{
    /// <summary>
    /// All Stored Procedures
    /// </summary>
    public class SharedObjects
    {
        private Database oDb;

        /// <summary>
        ///Class Constructor
        /// </summary>
        /// <param name="oDatabase">Enterprise Library Database Object </param>
        public SharedObjects(Database oDatabase)
        {
            oDb = oDatabase;
        }
        public DataTable usp_ChronicIllness_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChronicIllness_Select");
            //oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Country_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Country_Select");
            //oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MainClient_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MainClient_Select");
            //oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Exclusion_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Exclusion_Select");
            //oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, ExclusionID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Gender_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Gender_Select");
            //oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Title_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Title_Select");
            //oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Language_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Language_Select");
            //oDb.AddInParameter(sprocCmd, "LanguageID", DbType.Int32, LanguageID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MarritalStatus_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MarritalStatus_Select");
            //oDb.AddInParameter(sprocCmd, "MarritalStatusID", DbType.Int32, MarritalStatusID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Select(int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Select");
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Exclusion_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Select");
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Exclusion_Select_ByMedicalAid(int MedicalAidID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Select_ByMedicalAid");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_Select(int MemberID, DateTime Date)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Select");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "Date", DbType.Date, Date);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_Select_ByFilters(string Surname, string Name, string MemberNumber, string PassportNumber, string IDNumber, DateTime DateOfBirth, int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Select_ByFilters");
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            oDb.AddInParameter(sprocCmd, "PassportNumber", DbType.String, PassportNumber);
            oDb.AddInParameter(sprocCmd, "IDNumber", DbType.String, IDNumber);
            oDb.AddInParameter(sprocCmd, "DateOfBirth", DbType.Date, DateOfBirth);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_ChronicIllness_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_ChronicIllness_Select");
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberNote_Select(int MemberID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberNote_Select");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberStatus_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberStatus_Select");
            //oDb.AddInParameter(sprocCmd, "MemberStatusID", DbType.Int32, MemberStatusID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_PeriodInCountry_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_PeriodInCountry_Select");
            //oDb.AddInParameter(sprocCmd, "PeriodInCountryID", DbType.Int32, PeriodInCountryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Race_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Race_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_SuspendedReason_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SuspendedReason_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Select(int ServiceProviderID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Select");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Select_ByFilters(string ServiceProviderName, string ServiceProviderSurname, string PracticeName, string PracticeNr, bool Visible)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Select_ByFilters");
            oDb.AddInParameter(sprocCmd, "ServiceProviderName", DbType.String, ServiceProviderName);
            oDb.AddInParameter(sprocCmd, "ServiceProviderSurname", DbType.String, ServiceProviderSurname);
            oDb.AddInParameter(sprocCmd, "PracticeName", DbType.String, PracticeName);
            oDb.AddInParameter(sprocCmd, "PracticeNr", DbType.String, PracticeNr);
            oDb.AddInParameter(sprocCmd, "Visible", DbType.Boolean, Visible);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Select_Autocomplete()
        {
            //IDataReader oReader = oDb.ExecuteReader(oDb.GetStoredProcCommand("usp_ServiceProvider_Select_Autocomplete", new object[1] { Filter }));

            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Select_Autocomplete");
            //oDb.AddInParameter(sprocCmd, "Filter", DbType.String, Filter);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
            
        }
        public DataTable usp_ServiceProvider_Select_AfterAutocomplete(string Filter)
        {
            //IDataReader oReader = oDb.ExecuteReader(oDb.GetStoredProcCommand("usp_ServiceProvider_Select_Autocomplete", new object[1] { Filter }));

            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Select_AfterAutocomplete");
            oDb.AddInParameter(sprocCmd, "Filter", DbType.String, Filter);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];

        }
        public DataTable usp_Speciality_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Select");
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        //public DataTable usp_Speciality_Select_WhereThereAreTariffs()
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Select_WhereThereAreTariffs");
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_SystemData_Select(int SystemDataID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SystemData_Select");
            oDb.AddInParameter(sprocCmd, "SystemDataID", DbType.Int32, SystemDataID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        //public DataTable usp_Tariff_SelectDistinctDetails()
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_SelectDistinctDetails");
        //    //oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Tariff_SelectPeriodName_ByTariffNameID(int TariffNameID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_SelectPeriodName_ByTariffNameID");
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Speciality_Select_ByTariffNameID(int TariffNameID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Select_ByTariffNameID");
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Tariff_Select_Distinct_BySpeciality(int SpecialityID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Select_Distinct_BySpeciality");
        //    oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_BaseTariff_Select()
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_BaseTariff_Select");
        //    //oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Tariff_Select_ByNameID_Period_Speciality(int TariffNameID, string TariffPeriodName, int SpecialityID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Select_ByNameID_Period_Speciality");
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
        //    oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.String, TariffPeriodName);
        //    oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_FacilityType_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_FacilityType_Select");
            //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_CPT_Select_ByFilters(string Code, string Description)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_CPT_Select_ByFilters");
            oDb.AddInParameter(sprocCmd, "Code", DbType.String, Code);
            oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ICD_Select_ByFilters(string Code, string Description)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ICD_Select_ByFilters");
            oDb.AddInParameter(sprocCmd, "Code", DbType.String, Code);
            oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        /// <summary>
        /// Returns true if the member number already exist
        /// </summary>
        /// <param name="MemberNumber"></param>
        /// <returns></returns>
        public bool usp_Member_Select_ExistingMemberNumber(string MemberNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Select_ExistingMemberNumber");
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            DataTable oDt = oDb.ExecuteDataSet(sprocCmd).Tables[0];
            if (oDt.Rows[0][0].ToString() == "1")
                return true;
            else
                return false;
        }
        /// <summary>
        /// Returns true if the member number already exist
        /// </summary>
        public bool usp_ServiceProvider_Select_ExistingPracticeNr(string PracticeNr)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Select_ExistingPracticeNr");
            oDb.AddInParameter(sprocCmd, "PracticeNr", DbType.String, PracticeNr);
            DataTable oDt = oDb.ExecuteDataSet(sprocCmd).Tables[0];
            if (oDt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //public DataTable usp_Tariff_Select_ByFilters(string Code, string Description, int CaseID, int SpecialityID,int TariffNameID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Select_ByFilters");
        //    oDb.AddInParameter(sprocCmd, "Code", DbType.String, Code);
        //    oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
        //    oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
        //    oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_TariffName_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_TariffName_Select");
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        //public DataTable usp_BaseTariff_Select_NewCustomCode()
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_BaseTariff_Select_NewCustomCode");
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Speciality_Select()
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Select");
        //    //oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_Bookings_Select_ByBookingID(int BookingID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Bookings_Select_ByBookingID");
            oDb.AddInParameter(sprocCmd, "BookingID", DbType.Int32, BookingID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Bookings_Select_ByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Bookings_Select_ByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Bookings_Select_ByFilters(string Surname, string Name, string MemberNumber, DateTime TravelDateFrom, DateTime TravelDateTo)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Bookings_Select_ByFilters");
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            oDb.AddInParameter(sprocCmd, "TravelDateFrom", DbType.Date, TravelDateFrom);
            oDb.AddInParameter(sprocCmd, "TravelDateTo", DbType.Date, TravelDateTo);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Bookings_Select_ALL_ByMemberNumber(string MemberNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Bookings_Select_ALL_ByMemberNumber");
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_MedicalAidProduct_SelectByMemberID(int MemberID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_MedicalAidProduct_SelectByMemberID");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Select_ByMedicalAidID(int MedicalAidID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Select_ByMedicalAidID");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAidProduct_Select(int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAidProduct_Select");
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.String, MainClientID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_TariffStructure_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_TariffStructure_Select");
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        
        public DataTable usp_ServiceProvider_Tariff_Select_ById(int ServiceProviderID,int SpecialityID, DateTime EffectiveDate,int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("[Tariff].[usp_ServiceProvider_Tariff_Select_ById]");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "EffectiveDate", DbType.Date, EffectiveDate);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        //public DataTable usp_SpecialityID_TariffID_Select_ByServiceProviderID(int ServiceProviderID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SpecialityID_TariffID_Select_ByServiceProviderID");
        //    oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_ProviderIDs_TreatmentDate_Select_ByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ProviderIDs_TreatmentDate_Select_ByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate(string TariffCode, int ServiceProviderID, DateTime TreatmentDate,int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate");
            oDb.AddInParameter(sprocCmd, "TariffCode", DbType.String, TariffCode);
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "TreatmentDate", DbType.DateTime, TreatmentDate);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_NappiCodes_Select_ByNappiCode_Description_Date(string NappiCode, string Description, DateTime Date)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_NappiCodes_Select_ByNappiCode_Description_Date");
            oDb.AddInParameter(sprocCmd, "NappiCode", DbType.String, NappiCode);
            oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
            oDb.AddInParameter(sprocCmd, "Date", DbType.DateTime, Date);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Tariff_SelectDistinctPeriods()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_SelectDistinctPeriods");
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }

        public DataTable usp_ChronicIllness_Delete(int ChronicIllnessID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChronicIllness_Delete");
            oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Country_Delete(int CountryID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Country_Delete");
            oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Exclusion_Delete(int ExclusionID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Exclusion_Delete");
            oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, ExclusionID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Gender_Delete(int GenderID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Gender_Delete");
            oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Language_Delete(int LanguageID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Language_Delete");
            oDb.AddInParameter(sprocCmd, "LanguageID", DbType.Int32, LanguageID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MarritalStatus_Delete(int MarritalStatusID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MarritalStatus_Delete");
            oDb.AddInParameter(sprocCmd, "MarritalStatusID", DbType.Int32, MarritalStatusID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Delete(int MedicalAidID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Delete");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Exclusion_Delete(int MedicalAidID, string BaseTariffID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Delete");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "BaseTariffID", DbType.String, BaseTariffID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Member_Delete(int MemberID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Delete");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_ChronicIllness_Delete(int MemberID, int ChronicIllnessID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_ChronicIllness_Delete");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberNote_Delete(int MemberNoteID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberNote_Delete");
            oDb.AddInParameter(sprocCmd, "MemberNoteID", DbType.Int32, MemberNoteID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_MemberStatus_Delete(int MemberStatusID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberStatus_Delete");
            oDb.AddInParameter(sprocCmd, "MemberStatusID", DbType.Int32, MemberStatusID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_PeriodInCountry_Delete(int PeriodInCountryID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_PeriodInCountry_Delete");
            oDb.AddInParameter(sprocCmd, "PeriodInCountryID", DbType.Int32, PeriodInCountryID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Race_Delete(int RaceID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Race_Delete");
            oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Delete(int ServiceProviderID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Delete");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Speciality_Delete(int SpecialityID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Delete");
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_SystemData_Delete(int SystemDataID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SystemData_Delete");
            oDb.AddInParameter(sprocCmd, "SystemDataID", DbType.Int32, SystemDataID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Tariff_Delete(int TariffID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Delete");
            oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_ChronicIllness_Insert(string ChronicIllnessName, string ChronicIllnessDescription, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChronicIllness_Insert");
            oDb.AddInParameter(sprocCmd, "ChronicIllnessName", DbType.String, ChronicIllnessName);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessDescription", DbType.String, ChronicIllnessDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();//
        }
        public DataTable usp_Country_Insert(string CountryName, string CountryISOCode, string CountryCurrencyCode, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Country_Insert");
            oDb.AddInParameter(sprocCmd, "CountryName", DbType.String, CountryName);
            oDb.AddInParameter(sprocCmd, "CountryISOCode", DbType.String, CountryISOCode);
            oDb.AddInParameter(sprocCmd, "CountryCurrencyCode", DbType.String, CountryCurrencyCode);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Exclusion_Insert(string Exclusion, string ExclusionDescription, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Exclusion_Insert");
            oDb.AddInParameter(sprocCmd, "Exclusion", DbType.String, Exclusion);
            oDb.AddInParameter(sprocCmd, "ExclusionDescription", DbType.String, ExclusionDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Gender_Insert(string GenderCode, string GenderDescription)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Gender_Insert");
            oDb.AddInParameter(sprocCmd, "GenderCode", DbType.String, GenderCode);
            oDb.AddInParameter(sprocCmd, "GenderDescription", DbType.String, GenderDescription);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Language_Insert(string Language)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Language_Insert");
            oDb.AddInParameter(sprocCmd, "Language", DbType.String, Language);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MarritalStatus_Insert(string MarritalStatus)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MarritalStatus_Insert");
            oDb.AddInParameter(sprocCmd, "MarritalStatus", DbType.String, MarritalStatus);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Insert(int MainClientID, string MedicalAidName, DateTime MedicalAidInitiationDate, DateTime MedicalAidReinstatedDate, DateTime MedicalAidTerminatedDate,string CasePrefix, string ReportTemplate, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Insert");
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "MedicalAidName", DbType.String, MedicalAidName);
            oDb.AddInParameter(sprocCmd, "MedicalAidInitiationDate", DbType.Date, MedicalAidInitiationDate);
            oDb.AddInParameter(sprocCmd, "MedicalAidReinstatedDate", DbType.Date, MedicalAidReinstatedDate);
            oDb.AddInParameter(sprocCmd, "MedicalAidTerminatedDate", DbType.Date, MedicalAidTerminatedDate);
            oDb.AddInParameter(sprocCmd, "CasePrefix", DbType.String, CasePrefix);
            oDb.AddInParameter(sprocCmd, "ReportTemplate", DbType.String, ReportTemplate);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);

            return new DataTable();
        }
        public DataTable usp_MedicalAid_Exclusion_Insert(int MedicalAidID, string BaseTariffID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Insert");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "BaseTariffID", DbType.String, BaseTariffID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_MedicalAid_Exclusion_Insert_BySpeciality(int MedicalAidID, int SpecialityID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Insert_BySpeciality");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Member_Insert(string MemberNumber, int TitleID, string Surname, string Initials
            , string Name, string IDNumber, string PassportNumber, DateTime PassportExpiryDate, int PeriodInCountryID
            , DateTime DateOfBirth, int GenderID, bool HasMedicalAid, int MedicalAidID, DateTime DateOfBenefit
            , DateTime DateJoined, bool Suspended, DateTime DateSuspended, int SuspensionReasonID
            , bool MedicalAidExhausted, DateTime DateMedicalAidExhausted, bool WaitingPeriodApplicable
            , int MarritalStatusID, int EmployerCountryID, string EmployerAddress, string EmployerAddressCode
            , string EmployerPhoneNumber, bool Pensioner, int MemberStatusID, int MemberCountryID, string MemberAddress1
            , string MemberAddress2, string MemberAddress3, string MemberAddressCode, string MemberPhoneNumber
            , string MemberCellNumber, string NextOfKinName, string NextOfKinRelationship, string NextOfKinContactNumber
            , int MemberLanguageID, int MemberRaceID, string MemberDependents, bool FundReinstated
            , DateTime FundReinstatedDate, bool Deceased, DateTime DeceasedDate, string UserID, int MedAidProductID
            , bool MBOD_RMA, DateTime MedicalAidProductStart)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Insert");
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            oDb.AddInParameter(sprocCmd, "TitleID", DbType.Int32, TitleID);
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "Initials", DbType.String, Initials);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "IDNumber", DbType.String, IDNumber);
            oDb.AddInParameter(sprocCmd, "PassportNumber", DbType.String, PassportNumber);
            oDb.AddInParameter(sprocCmd, "PassportExpiryDate", DbType.Date, PassportExpiryDate);
            oDb.AddInParameter(sprocCmd, "PeriodInCountryID", DbType.Int32, PeriodInCountryID);
            oDb.AddInParameter(sprocCmd, "DateOfBirth", DbType.Date, DateOfBirth);
            oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            oDb.AddInParameter(sprocCmd, "HasMedicalAid", DbType.Boolean, HasMedicalAid);
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "DateOfBenefit", DbType.Date, DateOfBenefit);
            oDb.AddInParameter(sprocCmd, "DateJoined", DbType.Date, DateJoined);
            oDb.AddInParameter(sprocCmd, "Suspended", DbType.Boolean, Suspended);
            oDb.AddInParameter(sprocCmd, "DateSuspended", DbType.Date, DateSuspended);
            oDb.AddInParameter(sprocCmd, "SuspendedReasonID", DbType.Int32, SuspensionReasonID);
            oDb.AddInParameter(sprocCmd, "MedicalAidExhausted", DbType.Boolean, MedicalAidExhausted);
            oDb.AddInParameter(sprocCmd, "DateMedicalAidExhausted", DbType.Date, DateMedicalAidExhausted);
            oDb.AddInParameter(sprocCmd, "WaitingPeriodApplicable", DbType.Boolean, WaitingPeriodApplicable);
            oDb.AddInParameter(sprocCmd, "MarritalStatusID", DbType.Int32, MarritalStatusID);
            oDb.AddInParameter(sprocCmd, "EmployerCountryID", DbType.Int32, EmployerCountryID);
            oDb.AddInParameter(sprocCmd, "EmployerAddress", DbType.String, EmployerAddress);
            oDb.AddInParameter(sprocCmd, "EmployerAddressCode", DbType.String, EmployerAddressCode);
            oDb.AddInParameter(sprocCmd, "EmployerPhoneNumber", DbType.String, EmployerPhoneNumber);
            oDb.AddInParameter(sprocCmd, "Pensioner", DbType.Boolean, Pensioner);
            oDb.AddInParameter(sprocCmd, "MemberStatusID", DbType.Int32, MemberStatusID);
            oDb.AddInParameter(sprocCmd, "MemberCountryID", DbType.Int32, MemberCountryID);
            oDb.AddInParameter(sprocCmd, "MemberAddress1", DbType.String, MemberAddress1);
            oDb.AddInParameter(sprocCmd, "MemberAddress2", DbType.String, MemberAddress2);
            oDb.AddInParameter(sprocCmd, "MemberAddress3", DbType.String, MemberAddress3);
            oDb.AddInParameter(sprocCmd, "MemberAddressCode", DbType.String, MemberAddressCode);
            oDb.AddInParameter(sprocCmd, "MemberPhoneNumber", DbType.String, MemberPhoneNumber);
            oDb.AddInParameter(sprocCmd, "MemberCellNumber", DbType.String, MemberCellNumber);
            oDb.AddInParameter(sprocCmd, "NextOfKinName", DbType.String, NextOfKinName);
            oDb.AddInParameter(sprocCmd, "NextOfKinRelationship", DbType.String, NextOfKinRelationship);
            oDb.AddInParameter(sprocCmd, "NextOfKinContactNumber", DbType.String, NextOfKinContactNumber);
            oDb.AddInParameter(sprocCmd, "MemberLanguageID", DbType.Int32, MemberLanguageID);
            oDb.AddInParameter(sprocCmd, "MemberRaceID", DbType.Int32, MemberRaceID);
            oDb.AddInParameter(sprocCmd, "MemberDependents", DbType.String, MemberDependents);
            oDb.AddInParameter(sprocCmd, "FundReinstated", DbType.Boolean, FundReinstated);
            oDb.AddInParameter(sprocCmd, "FundReinstatedDate", DbType.Date, FundReinstatedDate);
            oDb.AddInParameter(sprocCmd, "Deceased", DbType.Boolean, Deceased);
            oDb.AddInParameter(sprocCmd, "DeceasedDate", DbType.Date, DeceasedDate);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "MedAidProductID", DbType.Int32, MedAidProductID);
            oDb.AddInParameter(sprocCmd, "MBOD_RMA", DbType.Boolean, MBOD_RMA);
            oDb.AddInParameter(sprocCmd, "MedicalAidProductStart", DbType.Date, MedicalAidProductStart);

            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_ChronicIllness_Insert(int MemberID, int ChronicIllnessID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_ChronicIllness_Insert");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberNote_Insert(string MemberNote, int MemberID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberNote_Insert");
            oDb.AddInParameter(sprocCmd, "MemberNote", DbType.String, MemberNote);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_MemberStatus_Insert(string MemberStatus)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberStatus_Insert");
            oDb.AddInParameter(sprocCmd, "MemberStatus", DbType.String, MemberStatus);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_PeriodInCountry_Insert(string PeriodInCountry)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_PeriodInCountry_Insert");
            oDb.AddInParameter(sprocCmd, "PeriodInCountry", DbType.String, PeriodInCountry);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Race_Insert(string Race)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Race_Insert");
            oDb.AddInParameter(sprocCmd, "Race", DbType.String, Race);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Insert(string ServiceProviderName, string ServiceProviderSurname
            , string ServiceProviderInitials, string PracticeName, string GroupPracticeNr, string PracticeNr
            , int NoOfPartners, string ServiceArea, int SpecialityID, bool IsHospital, string PracticeAddress1
            , string PracticeAddress2, string PracticeAddress3, string PracticeAddress4, string PracticeAddressCode
            , string PracticePAddress1, string PracticePAddress2, string PracticePAddress3, string PracticePAddress4
            , string PracticePAddressCode, string PhoneNumber, string FaxNumber, string EmailAddress, int LanguageID
            , int CountryID, string BankName, string BankBranch, string BankBranchCode, string BankAccountType
            , string BankAccountNumber, string UserID, string TariffStructureID, bool TariffInclVAT, bool Visible
            , string CellNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Insert");
            oDb.AddInParameter(sprocCmd, "ServiceProviderName", DbType.String, ServiceProviderName);
            oDb.AddInParameter(sprocCmd, "ServiceProviderSurname", DbType.String, ServiceProviderSurname);
            oDb.AddInParameter(sprocCmd, "ServiceProviderInitials", DbType.String, ServiceProviderInitials);
            oDb.AddInParameter(sprocCmd, "PracticeName", DbType.String, PracticeName);
            oDb.AddInParameter(sprocCmd, "GroupPracticeNr", DbType.String, GroupPracticeNr);
            oDb.AddInParameter(sprocCmd, "PracticeNr", DbType.String, PracticeNr);
            oDb.AddInParameter(sprocCmd, "NoOfPartners", DbType.Int32, NoOfPartners);
            oDb.AddInParameter(sprocCmd, "ServiceArea", DbType.String, ServiceArea);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "IsHospital", DbType.Boolean, IsHospital);
            oDb.AddInParameter(sprocCmd, "PracticeAddress1", DbType.String, PracticeAddress1);
            oDb.AddInParameter(sprocCmd, "PracticeAddress2", DbType.String, PracticeAddress2);
            oDb.AddInParameter(sprocCmd, "PracticeAddress3", DbType.String, PracticeAddress3);
            oDb.AddInParameter(sprocCmd, "PracticeAddress4", DbType.String, PracticeAddress4);
            oDb.AddInParameter(sprocCmd, "PracticeAddressCode", DbType.String, PracticeAddressCode);
            oDb.AddInParameter(sprocCmd, "PracticePAddress1", DbType.String, PracticePAddress1);
            oDb.AddInParameter(sprocCmd, "PracticePAddress2", DbType.String, PracticePAddress2);
            oDb.AddInParameter(sprocCmd, "PracticePAddress3", DbType.String, PracticePAddress3);
            oDb.AddInParameter(sprocCmd, "PracticePAddress4", DbType.String, PracticePAddress4);
            oDb.AddInParameter(sprocCmd, "PracticePAddressCode", DbType.String, PracticePAddressCode);
            oDb.AddInParameter(sprocCmd, "PhoneNumber", DbType.String, PhoneNumber);
            oDb.AddInParameter(sprocCmd, "FaxNumber", DbType.String, FaxNumber);
            oDb.AddInParameter(sprocCmd, "EmailAddress", DbType.String, EmailAddress);
            oDb.AddInParameter(sprocCmd, "LanguageID", DbType.Int32, LanguageID);
            oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            oDb.AddInParameter(sprocCmd, "BankName", DbType.String, BankName);
            oDb.AddInParameter(sprocCmd, "BankBranch", DbType.String, BankBranch);
            oDb.AddInParameter(sprocCmd, "BankBranchCode", DbType.String, BankBranchCode);
            oDb.AddInParameter(sprocCmd, "BankAccountType", DbType.String, BankAccountType);
            oDb.AddInParameter(sprocCmd, "BankAccountNumber", DbType.String, BankAccountNumber);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "TariffStructureID", DbType.String, TariffStructureID);
            oDb.AddInParameter(sprocCmd, "TariffInclVAT", DbType.Boolean, TariffInclVAT);
            oDb.AddInParameter(sprocCmd, "Visible", DbType.Boolean, Visible);
            oDb.AddInParameter(sprocCmd, "CellNumber", DbType.String, CellNumber);
            
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Speciality_Insert(string Speciality, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Insert");
            oDb.AddInParameter(sprocCmd, "Speciality", DbType.String, Speciality);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_SystemData_Insert(int SystemCountryID, Guid SystemUniqueIdentifier, string SystemEmailAddress, string SMTPServer, bool SSL, string Username, string Password, string UserID, int SpecialICU, int ICU, int HighCare, int NeuroWard, int InIsolation, int GeneralWard, int Paediatric, int Meternity, int DayCase, int StepDown, int Psychiatric)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SystemData_Insert");
            oDb.AddInParameter(sprocCmd, "SystemCountryID", DbType.Int32, SystemCountryID);
            oDb.AddInParameter(sprocCmd, "SystemUniqueIdentifier", DbType.Guid, SystemUniqueIdentifier);
            oDb.AddInParameter(sprocCmd, "SystemEmailAddress", DbType.String, SystemEmailAddress);
            oDb.AddInParameter(sprocCmd, "SMTPServer", DbType.String, SMTPServer);
            oDb.AddInParameter(sprocCmd, "SSL", DbType.Boolean, SSL);
            oDb.AddInParameter(sprocCmd, "Username", DbType.String, Username);
            oDb.AddInParameter(sprocCmd, "Password", DbType.String, Password);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "SpecialICU", DbType.Int32, SpecialICU);
            oDb.AddInParameter(sprocCmd, "ICU", DbType.Int32, ICU);
            oDb.AddInParameter(sprocCmd, "HighCare", DbType.Int32, HighCare);
            oDb.AddInParameter(sprocCmd, "NeuroWard", DbType.Int32, NeuroWard);
            oDb.AddInParameter(sprocCmd, "InIsolation", DbType.Int32, InIsolation);
            oDb.AddInParameter(sprocCmd, "GeneralWard", DbType.Int32, GeneralWard);
            oDb.AddInParameter(sprocCmd, "Paediatric", DbType.Int32, Paediatric);
            oDb.AddInParameter(sprocCmd, "Meternity", DbType.Int32, Meternity);
            oDb.AddInParameter(sprocCmd, "DayCase", DbType.Int32, DayCase);
            oDb.AddInParameter(sprocCmd, "StepDown", DbType.Int32, StepDown);
            oDb.AddInParameter(sprocCmd, "Psychiatric", DbType.Int32, Psychiatric);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        //public DataTable usp_Tariff_Insert(string TariffName, string PeriodName, DateTime StartDate, DateTime EndDate, string UserID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_Insert");
        //    oDb.AddInParameter(sprocCmd, "TariffName", DbType.String, TariffName);
        //    oDb.AddInParameter(sprocCmd, "PeriodName", DbType.String, PeriodName);
        //    oDb.AddInParameter(sprocCmd, "StartDate", DbType.Date, StartDate);
        //    oDb.AddInParameter(sprocCmd, "EndDate", DbType.Date, EndDate);
        //    oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);

        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Tariff_CheckPeriodExist(int TariffNameID, string PeriodName, DateTime StartDate, DateTime EndDate)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_CheckPeriodExist");
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.String, TariffNameID);
        //    oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.String, PeriodName);
        //    oDb.AddInParameter(sprocCmd, "StartDate", DbType.Date, StartDate);
        //    oDb.AddInParameter(sprocCmd, "EndDate", DbType.Date, EndDate);

        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_Tariff_InsertNewPeriod(int TariffNameID, string PeriodName, DateTime StartDate, DateTime EndDate, decimal Multiplier, string UserID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_InsertNewPeriod");
        //    oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.String, TariffNameID);
        //    oDb.AddInParameter(sprocCmd, "PeriodName", DbType.String, PeriodName);
        //    oDb.AddInParameter(sprocCmd, "StartDate", DbType.Date, StartDate);
        //    oDb.AddInParameter(sprocCmd, "EndDate", DbType.Date, EndDate);
        //    oDb.AddInParameter(sprocCmd, "Multiplier", DbType.Decimal, Multiplier);
        //    oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);

        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        //public DataTable usp_BaseTariff_InsertCustom(string TariffCode, int SpecialityID, string TariffDescription, string UserID)
        //{
        //    DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_BaseTariff_InsertCustom");
        //    oDb.AddInParameter(sprocCmd, "TariffCode", DbType.String, TariffCode);
        //    oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
        //    oDb.AddInParameter(sprocCmd, "TariffDescription", DbType.String, TariffDescription);
        //    oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);

        //    return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        //}
        public DataTable usp_Bookings_Insert(int BookingID, DateTime TravelDate, DateTime TravelTime, DateTime AppointmentDate, int ReferringPracticeID, int MemberID, int CaseID, string Discipline, bool Consultation, bool Admission, int CurrentPracticeID, string Hospital, bool Arrived, string TISCH, string Comments)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Bookings_Insert");
            oDb.AddInParameter(sprocCmd, "BookingID", DbType.Int32, BookingID);
            oDb.AddInParameter(sprocCmd, "TravelDate", DbType.Date, TravelDate);
            oDb.AddInParameter(sprocCmd, "TravelTime", DbType.Time, TravelTime);
            oDb.AddInParameter(sprocCmd, "AppointmentDate", DbType.Date, AppointmentDate);
            oDb.AddInParameter(sprocCmd, "ReferringPracticeID", DbType.Int32, ReferringPracticeID);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "Discipline", DbType.String, Discipline);
            oDb.AddInParameter(sprocCmd, "Consultation", DbType.Boolean, Consultation);
            oDb.AddInParameter(sprocCmd, "Admission", DbType.Boolean, Admission);
            oDb.AddInParameter(sprocCmd, "CurrentPracticeID", DbType.Int32, CurrentPracticeID);
            oDb.AddInParameter(sprocCmd, "Hospital", DbType.String, Hospital);
            oDb.AddInParameter(sprocCmd, "Arrived", DbType.Boolean, Arrived);
            oDb.AddInParameter(sprocCmd, "TISCH", DbType.String, TISCH);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        
        public DataTable usp_ServiceProvider_Tariff_Custom_Insert(int ServiceProviderID, string BaseTariffID, decimal TariffAmount, int MainClientID, DateTime TariffPeriodName)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_ServiceProvider_Tariff_Custom_Insert");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "BaseTariffID", DbType.String, BaseTariffID);
            oDb.AddInParameter(sprocCmd, "TariffAmount", DbType.Decimal, TariffAmount);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.Date, TariffPeriodName);

            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        
        public DataTable usp_ServiceProvider_Tariff_Update(int ServiceProviderId, int TariffNameID, int MainClientID, DateTime TariffPeriodName, decimal PercentageAdded)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_ServiceProvider_Tariff_Update");
            oDb.AddInParameter(sprocCmd, "ServiceProviderId", DbType.Int32, ServiceProviderId);
            oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.Date, TariffPeriodName);
            oDb.AddInParameter(sprocCmd, "PercentageAdded", DbType.Decimal, PercentageAdded);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_ChronicIllness_Update(int ChronicIllnessID, string ChronicIllnessName, string ChronicIllnessDescription, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ChronicIllness_Update");
            oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessName", DbType.String, ChronicIllnessName);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessDescription", DbType.String, ChronicIllnessDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Country_Update(int CountryID, string CountryName, string CountryISOCode, string CountryCurrencyCode, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Country_Update");
            oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            oDb.AddInParameter(sprocCmd, "CountryName", DbType.String, CountryName);
            oDb.AddInParameter(sprocCmd, "CountryISOCode", DbType.String, CountryISOCode);
            oDb.AddInParameter(sprocCmd, "CountryCurrencyCode", DbType.String, CountryCurrencyCode);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Exclusion_Update(int ExclusionID, string Exclusion, string ExclusionDescription, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Exclusion_Update");
            oDb.AddInParameter(sprocCmd, "ExclusionID", DbType.Int32, ExclusionID);
            oDb.AddInParameter(sprocCmd, "Exclusion", DbType.String, Exclusion);
            oDb.AddInParameter(sprocCmd, "ExclusionDescription", DbType.String, ExclusionDescription);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Gender_Update(int GenderID, string GenderCode, string GenderDescription)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Gender_Update");
            oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            oDb.AddInParameter(sprocCmd, "GenderCode", DbType.String, GenderCode);
            oDb.AddInParameter(sprocCmd, "GenderDescription", DbType.String, GenderDescription);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Language_Update(int LanguageID, string Language)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Language_Update");
            oDb.AddInParameter(sprocCmd, "LanguageID", DbType.Int32, LanguageID);
            oDb.AddInParameter(sprocCmd, "Language", DbType.String, Language);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MarritalStatus_Update(int MarritalStatusID, string MarritalStatus)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MarritalStatus_Update");
            oDb.AddInParameter(sprocCmd, "MarritalStatusID", DbType.Int32, MarritalStatusID);
            oDb.AddInParameter(sprocCmd, "MarritalStatus", DbType.String, MarritalStatus);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MedicalAid_Update(int MainClientID, int MedicalAidID, string MedicalAidName, DateTime MedicalAidInitiationDate, DateTime MedicalAidReinstatedDate, DateTime MedicalAidTerminatedDate,string CasePrefix, string ReportTemplate, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Update");
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "MedicalAidName", DbType.String, MedicalAidName);
            oDb.AddInParameter(sprocCmd, "MedicalAidInitiationDate", DbType.Date, MedicalAidInitiationDate);
            oDb.AddInParameter(sprocCmd, "MedicalAidReinstatedDate", DbType.Date, MedicalAidReinstatedDate);
            oDb.AddInParameter(sprocCmd, "MedicalAidTerminatedDate", DbType.Date, MedicalAidTerminatedDate);
            oDb.AddInParameter(sprocCmd, "CasePrefix", DbType.String, CasePrefix);
            oDb.AddInParameter(sprocCmd, "ReportTemplate", DbType.String, ReportTemplate);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_MedicalAid_Exclusion_Update(int MedicalAidID, string TariffCode, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MedicalAid_Exclusion_Update");
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "TariffCode", DbType.String, TariffCode);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Member_Update(int MemberID, string MemberNumber, int TitleID, string Surname, string Initials
            , string Name, string IDNumber, string PassportNumber, DateTime PassportExpiryDate, int PeriodInCountryID
            , DateTime DateOfBirth, int GenderID, bool HasMedicalAid, int MedicalAidID, DateTime DateOfBenefit
            , DateTime DateJoined, bool Suspended, DateTime DateSuspended, int SuspensionReasonID
            , bool MedicalAidExhausted, DateTime DateMedicalAidExhausted, bool WaitingPeriodApplicable
            , int MarritalStatusID, int EmployerCountryID, string EmployerAddress, string EmployerAddressCode
            , string EmployerPhoneNumber, bool Pensioner, int MemberStatusID, int MemberCountryID, string MemberAddress1
            , string MemberAddress2, string MemberAddress3, string MemberAddressCode, string MemberPhoneNumber
            , string MemberCellNumber, string NextOfKinName, string NextOfKinRelationship, string NextOfKinContactNumber
            , int MemberLanguageID, int MemberRaceID, string MemberDependents, bool FundReinstated
            , DateTime FundReinstatedDate, bool Deceased, DateTime DeceasedDate, string UserID, int MedAidProductID
            , bool MBOD_RMA, DateTime MedicalAidProductStart)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_Update");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "MemberNumber", DbType.String, MemberNumber);
            oDb.AddInParameter(sprocCmd, "TitleID", DbType.Int32, TitleID);
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "Initials", DbType.String, Initials);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "IDNumber", DbType.String, IDNumber);
            oDb.AddInParameter(sprocCmd, "PassportNumber", DbType.String, PassportNumber);
            oDb.AddInParameter(sprocCmd, "PassportExpiryDate", DbType.Date, PassportExpiryDate);
            oDb.AddInParameter(sprocCmd, "PeriodInCountryID", DbType.Int32, PeriodInCountryID);
            oDb.AddInParameter(sprocCmd, "DateOfBirth", DbType.Date, DateOfBirth);
            oDb.AddInParameter(sprocCmd, "GenderID", DbType.Int32, GenderID);
            oDb.AddInParameter(sprocCmd, "HasMedicalAid", DbType.Boolean, HasMedicalAid);
            oDb.AddInParameter(sprocCmd, "MedicalAidID", DbType.Int32, MedicalAidID);
            oDb.AddInParameter(sprocCmd, "DateOfBenefit", DbType.Date, DateOfBenefit);
            oDb.AddInParameter(sprocCmd, "DateJoined", DbType.Date, DateJoined);
            oDb.AddInParameter(sprocCmd, "Suspended", DbType.Boolean, Suspended);
            oDb.AddInParameter(sprocCmd, "DateSuspended", DbType.Date, DateSuspended);
            oDb.AddInParameter(sprocCmd, "SuspendedReasonID", DbType.Int32, SuspensionReasonID);
            oDb.AddInParameter(sprocCmd, "MedicalAidExhausted", DbType.Boolean, MedicalAidExhausted);
            oDb.AddInParameter(sprocCmd, "DateMedicalAidExhausted", DbType.Date, DateMedicalAidExhausted);
            oDb.AddInParameter(sprocCmd, "WaitingPeriodApplicable", DbType.Boolean, WaitingPeriodApplicable);
            oDb.AddInParameter(sprocCmd, "MarritalStatusID", DbType.Int32, MarritalStatusID);
            oDb.AddInParameter(sprocCmd, "EmployerCountryID", DbType.Int32, EmployerCountryID);
            oDb.AddInParameter(sprocCmd, "EmployerAddress", DbType.String, EmployerAddress);
            oDb.AddInParameter(sprocCmd, "EmployerAddressCode", DbType.String, EmployerAddressCode);
            oDb.AddInParameter(sprocCmd, "EmployerPhoneNumber", DbType.String, EmployerPhoneNumber);
            oDb.AddInParameter(sprocCmd, "Pensioner", DbType.Boolean, Pensioner);
            oDb.AddInParameter(sprocCmd, "MemberStatusID", DbType.Int32, MemberStatusID);
            oDb.AddInParameter(sprocCmd, "MemberCountryID", DbType.Int32, MemberCountryID);
            oDb.AddInParameter(sprocCmd, "MemberAddress1", DbType.String, MemberAddress1);
            oDb.AddInParameter(sprocCmd, "MemberAddress2", DbType.String, MemberAddress2);
            oDb.AddInParameter(sprocCmd, "MemberAddress3", DbType.String, MemberAddress3);
            oDb.AddInParameter(sprocCmd, "MemberAddressCode", DbType.String, MemberAddressCode);
            oDb.AddInParameter(sprocCmd, "MemberPhoneNumber", DbType.String, MemberPhoneNumber);
            oDb.AddInParameter(sprocCmd, "MemberCellNumber", DbType.String, MemberCellNumber);
            oDb.AddInParameter(sprocCmd, "NextOfKinName", DbType.String, NextOfKinName);
            oDb.AddInParameter(sprocCmd, "NextOfKinRelationship", DbType.String, NextOfKinRelationship);
            oDb.AddInParameter(sprocCmd, "NextOfKinContactNumber", DbType.String, NextOfKinContactNumber);
            oDb.AddInParameter(sprocCmd, "MemberLanguageID", DbType.Int32, MemberLanguageID);
            oDb.AddInParameter(sprocCmd, "MemberRaceID", DbType.Int32, MemberRaceID);
            oDb.AddInParameter(sprocCmd, "MemberDependents", DbType.String, MemberDependents);
            oDb.AddInParameter(sprocCmd, "FundReinstatedDate", DbType.Date, FundReinstatedDate);
            oDb.AddInParameter(sprocCmd, "FundReinstated", DbType.Boolean, FundReinstated);
            oDb.AddInParameter(sprocCmd, "Deceased", DbType.Boolean, Deceased);
            oDb.AddInParameter(sprocCmd, "DeceasedDate", DbType.Date, DeceasedDate);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "MedAidProductID", DbType.Int32, MedAidProductID);
            oDb.AddInParameter(sprocCmd, "MBOD_RMA", DbType.Boolean, MBOD_RMA);
            oDb.AddInParameter(sprocCmd, "MedicalAidProductStart", DbType.Date, MedicalAidProductStart);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Member_ChronicIllness_Update(int MemberID, int ChronicIllnessID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Member_ChronicIllness_Update");
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberNote_Update(int MemberNoteID, string MemberNote, int MemberID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberNote_Update");
            oDb.AddInParameter(sprocCmd, "MemberNoteID", DbType.Int32, MemberNoteID);
            oDb.AddInParameter(sprocCmd, "MemberNote", DbType.String, MemberNote);
            oDb.AddInParameter(sprocCmd, "MemberID", DbType.Int32, MemberID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_MemberStatus_Update(int MemberStatusID, string MemberStatus)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MemberStatus_Update");
            oDb.AddInParameter(sprocCmd, "MemberStatusID", DbType.Int32, MemberStatusID);
            oDb.AddInParameter(sprocCmd, "MemberStatus", DbType.String, MemberStatus);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_PeriodInCountry_Update(int PeriodInCountryID, string PeriodInCountry)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_PeriodInCountry_Update");
            oDb.AddInParameter(sprocCmd, "PeriodInCountryID", DbType.Int32, PeriodInCountryID);
            oDb.AddInParameter(sprocCmd, "PeriodInCountry", DbType.String, PeriodInCountry);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Race_Update(int RaceID, string Race)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Race_Update");
            oDb.AddInParameter(sprocCmd, "RaceID", DbType.Int32, RaceID);
            oDb.AddInParameter(sprocCmd, "Race", DbType.String, Race);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_ServiceProvider_Update(int ServiceProviderID, string ServiceProviderName, string ServiceProviderSurname
            , string ServiceProviderInitials, string PracticeName, string GroupPracticeNr, string PracticeNr, int NoOfPartners
            , string ServiceArea, int SpecialityID, bool IsHospital, string PracticeAddress1, string PracticeAddress2
            , string PracticeAddress3, string PracticeAddress4, string PracticeAddressCode, string PracticePAddress1
            , string PracticePAddress2, string PracticePAddress3, string PracticePAddress4, string PracticePAddressCode
            , string PhoneNumber, string FaxNumber, string EmailAddress, int LanguageID, int CountryID, string BankName
            , string BankBranch, string BankBranchCode, string BankAccountType, string BankAccountNumber, string UserID
            , string TariffStructureID, bool TariffInclVAT, bool Visible, string CellNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_Update");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "ServiceProviderName", DbType.String, ServiceProviderName);
            oDb.AddInParameter(sprocCmd, "ServiceProviderSurname", DbType.String, ServiceProviderSurname);
            oDb.AddInParameter(sprocCmd, "ServiceProviderInitials", DbType.String, ServiceProviderInitials);
            oDb.AddInParameter(sprocCmd, "PracticeName", DbType.String, PracticeName);
            oDb.AddInParameter(sprocCmd, "GroupPracticeNr", DbType.String, GroupPracticeNr);
            oDb.AddInParameter(sprocCmd, "PracticeNr", DbType.String, PracticeNr);
            oDb.AddInParameter(sprocCmd, "NoOfPartners", DbType.Int32, NoOfPartners);
            oDb.AddInParameter(sprocCmd, "ServiceArea", DbType.String, ServiceArea);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "IsHospital", DbType.Boolean, IsHospital);
            oDb.AddInParameter(sprocCmd, "PracticeAddress1", DbType.String, PracticeAddress1);
            oDb.AddInParameter(sprocCmd, "PracticeAddress2", DbType.String, PracticeAddress2);
            oDb.AddInParameter(sprocCmd, "PracticeAddress3", DbType.String, PracticeAddress3);
            oDb.AddInParameter(sprocCmd, "PracticeAddress4", DbType.String, PracticeAddress4);
            oDb.AddInParameter(sprocCmd, "PracticeAddressCode", DbType.String, PracticeAddressCode);
            oDb.AddInParameter(sprocCmd, "PracticePAddress1", DbType.String, PracticePAddress1);
            oDb.AddInParameter(sprocCmd, "PracticePAddress2", DbType.String, PracticePAddress2);
            oDb.AddInParameter(sprocCmd, "PracticePAddress3", DbType.String, PracticePAddress3);
            oDb.AddInParameter(sprocCmd, "PracticePAddress4", DbType.String, PracticePAddress4);
            oDb.AddInParameter(sprocCmd, "PracticePAddressCode", DbType.String, PracticePAddressCode);
            oDb.AddInParameter(sprocCmd, "PhoneNumber", DbType.String, PhoneNumber);
            oDb.AddInParameter(sprocCmd, "FaxNumber", DbType.String, FaxNumber);
            oDb.AddInParameter(sprocCmd, "EmailAddress", DbType.String, EmailAddress);
            oDb.AddInParameter(sprocCmd, "LanguageID", DbType.Int32, LanguageID);
            oDb.AddInParameter(sprocCmd, "CountryID", DbType.Int32, CountryID);
            oDb.AddInParameter(sprocCmd, "BankName", DbType.String, BankName);
            oDb.AddInParameter(sprocCmd, "BankBranch", DbType.String, BankBranch);
            oDb.AddInParameter(sprocCmd, "BankBranchCode", DbType.String, BankBranchCode);
            oDb.AddInParameter(sprocCmd, "BankAccountType", DbType.String, BankAccountType);
            oDb.AddInParameter(sprocCmd, "BankAccountNumber", DbType.String, BankAccountNumber);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "TariffStructureID", DbType.String, TariffStructureID);
            oDb.AddInParameter(sprocCmd, "TariffInclVAT", DbType.Boolean, TariffInclVAT);
            oDb.AddInParameter(sprocCmd, "Visible", DbType.Boolean, Visible);
            oDb.AddInParameter(sprocCmd, "CellNumber", DbType.String, CellNumber);

            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Speciality_Update(int SpecialityID, string Speciality, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Speciality_Update");
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "Speciality", DbType.String, Speciality);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_SystemData_Update(int SystemDataID, int SystemCountryID, Guid SystemUniqueIdentifier, string SystemEmailAddress, string SMTPServer, bool SSL, string Username, string Password, string UserID, int SpecialICU, int ICU, int HighCare, int NeuroWard, int InIsolation, int GeneralWard, int Paediatric, int Maternity, int DayCase, int StepDown, int Psychiatric,string Address1, string Address2, string Address3, string Address4, string AddressCode, string Email, string Fax, string Website, string Telephone, byte[] Logo, int DefaultProviderID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_SystemData_Update");
            oDb.AddInParameter(sprocCmd, "SystemDataID", DbType.Int32, SystemDataID);
            oDb.AddInParameter(sprocCmd, "SystemCountryID", DbType.Int32, SystemCountryID);
            oDb.AddInParameter(sprocCmd, "SystemUniqueIdentifier", DbType.Guid, SystemUniqueIdentifier);
            oDb.AddInParameter(sprocCmd, "SystemEmailAddress", DbType.String, SystemEmailAddress);
            oDb.AddInParameter(sprocCmd, "SMTPServer", DbType.String, SMTPServer);
            oDb.AddInParameter(sprocCmd, "SSL", DbType.Boolean, SSL);
            oDb.AddInParameter(sprocCmd, "Username", DbType.String, Username);
            oDb.AddInParameter(sprocCmd, "Password", DbType.String, Password);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.AddInParameter(sprocCmd, "SpecialICU", DbType.Int32, SpecialICU);
            oDb.AddInParameter(sprocCmd, "ICU", DbType.Int32, ICU);
            oDb.AddInParameter(sprocCmd, "HighCare", DbType.Int32, HighCare);
            oDb.AddInParameter(sprocCmd, "NeuroWard", DbType.Int32, NeuroWard);
            oDb.AddInParameter(sprocCmd, "InIsolation", DbType.Int32, InIsolation);
            oDb.AddInParameter(sprocCmd, "GeneralWard", DbType.Int32, GeneralWard);
            oDb.AddInParameter(sprocCmd, "Paediatric", DbType.Int32, Paediatric);
            oDb.AddInParameter(sprocCmd, "Maternity", DbType.Int32, Maternity);
            oDb.AddInParameter(sprocCmd, "DayCase", DbType.Int32, DayCase);
            oDb.AddInParameter(sprocCmd, "StepDown", DbType.Int32, StepDown);
            oDb.AddInParameter(sprocCmd, "Psychiatric", DbType.Int32, Psychiatric);
            oDb.AddInParameter(sprocCmd, "Address1", DbType.String, Address1);
            oDb.AddInParameter(sprocCmd, "Address2", DbType.String, Address2);
            oDb.AddInParameter(sprocCmd, "Address3", DbType.String, Address3);
            oDb.AddInParameter(sprocCmd, "Address4", DbType.String, Address4);
            oDb.AddInParameter(sprocCmd, "AddressCode", DbType.String, AddressCode);
            oDb.AddInParameter(sprocCmd, "Email", DbType.String, Email);
            oDb.AddInParameter(sprocCmd, "Fax", DbType.String, Fax);
            oDb.AddInParameter(sprocCmd, "Telephone", DbType.String, Telephone);
            oDb.AddInParameter(sprocCmd, "Website", DbType.String, Website);
            oDb.AddInParameter(sprocCmd, "Logo", DbType.Binary, (object)Logo);
            oDb.AddInParameter(sprocCmd, "DefaultProviderID", DbType.Int32, DefaultProviderID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Tariff_Update(int TariffID, int TariffCode, int SpecialityID, string TariffDescription, decimal Tariff, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Update");
            oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
            oDb.AddInParameter(sprocCmd, "TariffCode", DbType.Int32, TariffCode);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "TariffDescription", DbType.String, TariffDescription);
            oDb.AddInParameter(sprocCmd, "Tariff", DbType.Decimal, Tariff);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }
        public DataTable usp_Tariff_Update_Tariff(int TariffID, decimal Tariff, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Tariff_Update_Tariff");
            oDb.AddInParameter(sprocCmd, "TariffID", DbType.Int32, TariffID);
            oDb.AddInParameter(sprocCmd, "Tariff", DbType.Decimal, Tariff);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_MainClient_UpdateImage(byte[] Logo, int MainClientID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_MainClient_UpdateImage");
            oDb.AddInParameter(sprocCmd, "Logo", DbType.Binary, (object)Logo);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        
        public DataTable usp_Tariff_SelectClients(int ServiceProviderID, DateTime TariffPeriodName)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_Tariff_SelectClients");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.Date, TariffPeriodName);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        
        public DataTable usp_ServiceProvider_Tariff_Custom_Insert_FromExcel(int ServiceProviderID, string TariffCode, decimal TariffAmount
            ,int MainClientID, DateTime TariffPeriodName, int TariffNameID, int SpecialityID
            ,string Description
            ,string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Tariff.usp_ServiceProvider_Tariff_Custom_Insert_FromExcel");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "TariffCode", DbType.String, TariffCode);
            oDb.AddInParameter(sprocCmd, "TariffAmount", DbType.Decimal, TariffAmount);
            oDb.AddInParameter(sprocCmd, "MainClientID", DbType.Int32, MainClientID);
            oDb.AddInParameter(sprocCmd, "TariffPeriodName", DbType.Date, TariffPeriodName);
            oDb.AddInParameter(sprocCmd, "TariffNameID", DbType.Int32, TariffNameID);
            oDb.AddInParameter(sprocCmd, "SpecialityID", DbType.Int32, SpecialityID);
            oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_LinkedFile_SelectByEntityID_EntityType(int EntityID, string EntityType)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_LinkedFile_SelectByEntityID_EntityType");
            oDb.AddInParameter(sprocCmd, "EntityID", DbType.Int32, EntityID);
            oDb.AddInParameter(sprocCmd, "EntityType", DbType.String, EntityType);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_LinkedFile_DeleteByLinkedFileID(int LinkedFileID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_LinkedFile_DeleteByLinkedFileID");
            oDb.AddInParameter(sprocCmd, "LinkedFileID", DbType.Int32, LinkedFileID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_LinkedFile_Insert(int EntityID, string EntityType, string FilePath, string FileName, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_LinkedFile_Insert");
            oDb.AddInParameter(sprocCmd, "EntityID", DbType.Int32, EntityID);
            oDb.AddInParameter(sprocCmd, "EntityType", DbType.String, EntityType);
            oDb.AddInParameter(sprocCmd, "FilePath", DbType.String, FilePath);
            oDb.AddInParameter(sprocCmd, "FileName", DbType.String, FileName);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
    }
}

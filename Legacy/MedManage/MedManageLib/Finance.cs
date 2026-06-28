using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib
{
    public class Finance
    {
        private Database oDb;
        public Finance(Database oDatabase)
        {
            oDb = oDatabase;
        }

        public DataTable usp_Case_Discount_Update(int CaseID, decimal Discount)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Discount_Update");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "Discount", DbType.Decimal, Discount);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_ServiceProvider_MainClient_Discount_Insert(int ServiceProviderID, decimal Discount)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_ServiceProvider_MainClient_Discount_Insert");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "Discount", DbType.Decimal, Discount);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
       
        public DataTable usp_BillingStatus_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_BillingStatus_Select");
            //oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }

        public DataTable usp_Case_Billing_Delete(int Case_BillingID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Delete");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Billing_Insert(int ServiceProviderID
                            , DateTime AccountDateFrom
                            , DateTime AccountDateTo
                            , string AccountNumber
                            , string InvoiceNumber
                            , DateTime DateReceived
                            , DateTime DateSubmitted
                            , string ReceivedByName
                            , string PatientInitials
                            , string PatientSurname
                            , decimal AmountDue
                            //, string Comment
                            , int BillingStatusID
                            , DateTime DateInserted
                            , string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Insert");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "AccountDate", DbType.Date, AccountDateFrom);
            oDb.AddInParameter(sprocCmd, "AccountToDate", DbType.Date, AccountDateTo);
            oDb.AddInParameter(sprocCmd, "AccountNumber", DbType.String, AccountNumber);
            oDb.AddInParameter(sprocCmd, "InvoiceNumber", DbType.String, InvoiceNumber);
            oDb.AddInParameter(sprocCmd, "DateReceived", DbType.Date, DateReceived);
            
            oDb.AddInParameter(sprocCmd, "DateSubmitted", DbType.Date, DateSubmitted);

            oDb.AddInParameter(sprocCmd, "ReceivedByName", DbType.String, ReceivedByName);
            oDb.AddInParameter(sprocCmd, "PatientInitials", DbType.String, PatientInitials);
            oDb.AddInParameter(sprocCmd, "PatientSurname", DbType.String, PatientSurname);

            oDb.AddInParameter(sprocCmd, "AmountDue", DbType.Decimal, AmountDue);
            //oDb.AddInParameter(sprocCmd, "Comment", DbType.String, Comment);

            oDb.AddInParameter(sprocCmd, "BillingStatusID", DbType.Int32, BillingStatusID);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.DateTime2, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd); 
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_Update(int Case_BillingID
                            , int ServiceProviderID
                            , DateTime AccountDateFrom
                            , DateTime AccountDateTo
                            , string AccountNumber
                            , string InvoiceNumber
                            , DateTime DateReceived
                            , DateTime DateSubmitted
                            , string ReceivedByName
                            , string PatientInitials
                            , string PatientSurname
                            , decimal AmountDue
                            //, string Comment
                            , int BillingStatusID
                            , DateTime DateInserted
                            , string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Update");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "AccountDate", DbType.Date, AccountDateFrom);
            oDb.AddInParameter(sprocCmd, "AccountToDate", DbType.Date, AccountDateTo);
            oDb.AddInParameter(sprocCmd, "AccountNumber", DbType.String, AccountNumber);
            oDb.AddInParameter(sprocCmd, "InvoiceNumber", DbType.String, InvoiceNumber);
            oDb.AddInParameter(sprocCmd, "DateReceived", DbType.Date, DateReceived);

            oDb.AddInParameter(sprocCmd, "DateSubmitted", DbType.Date, DateSubmitted);

            oDb.AddInParameter(sprocCmd, "ReceivedByName", DbType.String, ReceivedByName);
            oDb.AddInParameter(sprocCmd, "PatientInitials", DbType.String, PatientInitials);
            oDb.AddInParameter(sprocCmd, "PatientSurname", DbType.String, PatientSurname);

            oDb.AddInParameter(sprocCmd, "AmountDue", DbType.Decimal, AmountDue);
            //oDb.AddInParameter(sprocCmd, "Comment", DbType.String, Comment);

            oDb.AddInParameter(sprocCmd, "BillingStatusID", DbType.Int32, BillingStatusID);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.DateTime2, DateInserted);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Billing_Select(int ServiceProviderID
            , DateTime ReceivedStartDate
            , DateTime ReceivedEndDate
            , string AccountNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Select");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "ReceivedStartDate", DbType.Date, ReceivedStartDate);
            oDb.AddInParameter(sprocCmd, "ReceivedEndDate", DbType.Date, ReceivedEndDate);
            oDb.AddInParameter(sprocCmd, "AccountNumber", DbType.String, AccountNumber);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated(int ReferToID, DateTime DateFinalInvoiceUpdatedStart, DateTime DateFinalInvoiceUpdatedEnd, int CaseStatusID, int BillingStatusID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_ServiceProvider_FinalInvoiceAmountUpdated");
            oDb.AddInParameter(sprocCmd, "ReferToID", DbType.Int32, ReferToID);
            oDb.AddInParameter(sprocCmd, "DateFinalInvoiceUpdatedStart", DbType.Date, DateFinalInvoiceUpdatedStart);
            oDb.AddInParameter(sprocCmd, "DateFinalInvoiceUpdatedEnd", DbType.Date, DateFinalInvoiceUpdatedEnd);
            oDb.AddInParameter(sprocCmd, "CaseStatusID", DbType.Int32, CaseStatusID);
            oDb.AddInParameter(sprocCmd, "BillingStatusID", DbType.Int32, BillingStatusID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_Select_Summary(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Select_Summary");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_CheckDuplicates(int Case_BillingID, string AccountNumber, int ReferToID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_CheckDuplicates");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            oDb.AddInParameter(sprocCmd, "AccountNumber", DbType.String, AccountNumber);
            oDb.AddInParameter(sprocCmd, "ReferToID", DbType.Int32, ReferToID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_SelectAccountNumbers_ByServiceProviderID(int ServiceProviderID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_SelectAccountNumbers_ByServiceProviderID");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_SelectAfterAutoComplete(int ServiceProviderID, string AccountNumber)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_SelectAfterAutoComplete");
            oDb.AddInParameter(sprocCmd, "ServiceProviderID", DbType.Int32, ServiceProviderID);
            oDb.AddInParameter(sprocCmd, "AccountNumber", DbType.String, AccountNumber);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
        public DataTable usp_Case_Billing_LinkToCase(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_LinkToCase");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Billing_UpdateToPaid(int Case_BillingID, decimal Amount, DateTime DatePaid, string Comments, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_UpdateToPaid");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            oDb.AddInParameter(sprocCmd, "Amount", DbType.Decimal, Amount);
            oDb.AddInParameter(sprocCmd, "DatePaid", DbType.Date, DatePaid);
            oDb.AddInParameter(sprocCmd, "Comments", DbType.String, Comments);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Case_Billing_UpdateRemittanceNumber(int Case_BillingID, string Remittance, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_UpdateRemittanceNumber");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            oDb.AddInParameter(sprocCmd, "Remittance", DbType.String, Remittance);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Cases_Select_ByRemittance(string Remittance)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Cases_Select_ByRemittance");
            oDb.AddInParameter(sprocCmd, "Remittance", DbType.String, Remittance);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }

        public DataTable usp_Case_Billing_Comment_Select(int Case_BillingID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Finance.usp_Case_Billing_Comment_Select");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }

        public DataTable usp_Case_Billing_Comment_Insert(int Case_BillingID, string Comment, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Case_Billing_Comment_Insert");
            oDb.AddInParameter(sprocCmd, "Case_BillingID", DbType.Int32, Case_BillingID);
            oDb.AddInParameter(sprocCmd, "Comment", DbType.String, Comment);
            oDb.AddInParameter(sprocCmd, "DateInserted", DbType.Date, DateTime.Now);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib
{
    public class FileImports
    {
        private Database oDb;
        public FileImports(Database oDatabase)
        {
            oDb = oDatabase;
        }

        public DataTable usp_Members_DRD_InsertRecord(string M_no, string Title, string Surname, string Init, string Name, string ID_Pasport, string DOB, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Import.usp_Members_DRD_InsertRecord");
            oDb.AddInParameter(sprocCmd, "M_no", DbType.String, M_no);
            oDb.AddInParameter(sprocCmd, "Title", DbType.String, Title);
            oDb.AddInParameter(sprocCmd, "Surname", DbType.String, Surname);
            oDb.AddInParameter(sprocCmd, "Init", DbType.String, Init);
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "ID_Pasport", DbType.String, ID_Pasport);
            oDb.AddInParameter(sprocCmd, "DOB", DbType.String, DOB);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Members_DRD_Merge()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Import.usp_Members_DRD_Merge");
            
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Members_DRD_Truncate()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Import.usp_Members_DRD_Truncate");
            
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }

        public DataTable usp_Nappi_InsertRecord(string NappiCode, string Description, string Units,
            string Measure, string Price1, string Price2, DateTime Date)
        {
            if (Price2 == "")
                Price2 = "0";
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Import.usp_Nappi_InsertRecord");
            oDb.AddInParameter(sprocCmd, "NappiCode", DbType.String, NappiCode);
            oDb.AddInParameter(sprocCmd, "Description", DbType.String, Description);
            oDb.AddInParameter(sprocCmd, "Units", DbType.Int32, Units);
            oDb.AddInParameter(sprocCmd, "Measure", DbType.String, Measure);
            oDb.AddInParameter(sprocCmd, "Price1", DbType.Decimal, Price1);
            oDb.AddInParameter(sprocCmd, "Price2", DbType.Decimal, Price2);
            oDb.AddInParameter(sprocCmd, "Date", DbType.Date, Date);
            //oDb.AddInParameter(sprocCmd, "EndDate", DbType.Date, EndDate);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Nappi_InsertRecord()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("dbo.usp_Nappi_InsertRecord");
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
        public DataTable usp_Nappi_InsertRecord_Truncate()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("dbo.usp_Nappi_InsertRecord_Truncate");
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
    }
}
  
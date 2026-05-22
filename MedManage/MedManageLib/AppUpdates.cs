using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib
{
    public class AppUpdates
    {
        private Database oDb;

        public AppUpdates(Database oDatabase)
        {
            oDb = oDatabase;
        }

        public DataTable usp_AppUpdates_Select()
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("[Updates].[usp_AppUpdates_Select]");
            //oDb.AddInParameter(sprocCmd, "ChronicIllnessID", DbType.Int32, ChronicIllnessID);
            return oDb.ExecuteDataSet(sprocCmd).Tables[0];
        }

        public DataTable usp_AppUpdates_Insert(int UpdateID, string ClientComputerName)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("[Updates].[usp_AppUpdates_Insert]");
            oDb.AddInParameter(sprocCmd, "UpdateID", DbType.Int32, UpdateID);
            oDb.AddInParameter(sprocCmd, "ClientComputerName", DbType.String, ClientComputerName);
            oDb.ExecuteDataSet(sprocCmd);
            return new DataTable();
        }
    }
}

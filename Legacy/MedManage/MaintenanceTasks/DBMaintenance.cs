using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace MaintenanceTasks
{
    class DBMaintenance
    {
        public Database oDb;
        public DBMaintenance(Database oDbIn)
        {
            oDb = oDbIn;
        }

        public void CreateFullBackup(string DatabaseName, string BackupLocation)
        {
            oDb.CreateConnection();
            //
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Maintenance.[usp_FullBackup]");
            oDb.AddInParameter(sprocCmd, "DBName", DbType.String, DatabaseName);
            oDb.AddInParameter(sprocCmd, "BackupPath", DbType.String, BackupLocation);
            oDb.ExecuteDataSet(sprocCmd);
            //E:\Data\MedManage_Test 2012-04-19.bak
        }
        public void CreateDifferentialBackup(string DatabaseName, string BackupLocation)
        {
            oDb.CreateConnection();
            //
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Maintenance.[usp_DiffBackup]");
            oDb.AddInParameter(sprocCmd, "DBName", DbType.String, DatabaseName);
            oDb.AddInParameter(sprocCmd, "BackupPath", DbType.String, BackupLocation);
            oDb.ExecuteDataSet(sprocCmd);
            //E:\Data\MedManage_Test 2012-04-19.bak
        }
        public void CreateLogBackup(string DatabaseName, string BackupLocation)
        {
            oDb.CreateConnection();
            //
            DbCommand sprocCmd = oDb.GetStoredProcCommand("Maintenance.[usp_LogBackup]");
            oDb.AddInParameter(sprocCmd, "DBName", DbType.String, DatabaseName);
            oDb.AddInParameter(sprocCmd, "BackupPath", DbType.String, BackupLocation);
            oDb.ExecuteDataSet(sprocCmd);
            //E:\Data\MedManage_Test 2012-04-19.bak
        }

        
    }
}

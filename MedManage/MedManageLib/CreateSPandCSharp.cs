using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Icondev.MedManage.MedManageLib
{
    public class CreateSPandCSharp
    {
        string SQLUpdateOutput = "";
        string SQLInsertOutput = "";
        string SQLDeleteOutput = "";
        string SQLSelectOutput = "";
        string COutputUpdate = "";
        string COutputSelect = "";
        string COutputInsert = "";
        string COutputDelete = "";
        string SQLTriggerOutput = "";
        string trgInsert = "";
        string trgUpdate = "";
        string trgDelete = "";

        public CreateSPandCSharp(Database oDb)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_z_TableColumns_Select");

            // Add parameters
            //oDb.AddInParameter(sprocCmd, "CategoryDescription", DbType.Decimal, CategoryDescription);


            // Call the ExecuteDataSet method with the command
            DataTable oDt = oDb.ExecuteDataSet(sprocCmd).Tables[0];
            //return sprocDataSet;
            #region Variables
            string TableName = "";
            string SchemaName = "";
            string ColumnName = "";
            string WhereClause = "";
            string FromClause = "";
            string Variables = "";
            string Columns = "";
            string DataType = "";
            string CDataType = "";
            string NormalType = "";
            #endregion

            #region Triggers
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    if (SQLTriggerOutput != "")
                    {
                        SQLTriggerOutput += "" + trgInsert;
                        SQLTriggerOutput += "" + trgUpdate;
                        SQLTriggerOutput += "" + trgDelete;
                        SQLTriggerOutput += "\rGO\r";
                    }

                    SQLTriggerOutput += "\rALTER TRIGGER [CaseManagement].[trg_for_" + SchemaName + "_" + TableName + "]\n\r" +
                                        "   ON [CaseManagement].[" + SchemaName + "_" + TableName + "]\n\r" +
                                        "   AFTER INSERT,DELETE,UPDATE\n\r" +
                                        "AS \n\r" +
                                        "BEGIN\n\r" +
                                        "    SET NOCOUNT ON;\n\r" +
                                        "    DECLARE @type VarCHAR(11);-- 'U' for update, 'D' for delete, 'I' for insert \n\r" +
                                        "    IF EXISTS(SELECT * FROM inserted) \n\r" +
                                        "    BEGIN   \n\r" +
                                        "        IF EXISTS(SELECT * FROM deleted)   \n\r" +
                                        "        BEGIN      \n\r" +
                                        "            SET @type ='U';   \n\r" +
                                        "        END   \n\r" +
                                        "        ELSE   \n\r" +
                                        "        BEGIN      \n\r" +
                                        "            SET @type ='I';   \n\r" +
                                        "        END \n\r" +
                                        "    END \n\r" +
                                        "    ELSE \n\r" +
                                        "    BEGIN   \n\r" +
                                        "        SET @type = 'D'; \n\r" +
                                        "    END; \n\r" +
                                        "    DECLARE @AuditID INT\n\r";
                    //SQLUpdateOutput += "\rEND\rGO";
                    trgInsert = "    IF @type ='U'\n\r" + 
	                                "    BEGIN\n\r" + 
		                            "        INSERT INTO audit.CaseManagement_Audit\n\r" + 
		                            "        SELECT inserted.UserID,'" + SchemaName + "_" + TableName + "',str(inserted.[ID1]),[ID2],[ID3],@type,GetDate()\n\r" + 
		                            "        FROM Inserted\n\r" + 
		                            "        SELECT @AuditID = @@IDENTITY\n\r" + 
                                    "\n\r" + 
		                            "        IF UPDATE(" + ColumnName + ")\n\r" + 
			                        "            INSERT INTO audit.CaseManagement_AuditDetail\n\r" + 
			                        "            SELECT @AuditID\n\r" + 
				                    "                ,Deleted." + ColumnName + "\n\r" + 
				                    "                ,Inserted." + ColumnName + "\n\r" + 
			                        "            FROM Inserted INNER JOIN Deleted\n\r" + 
				                    "                [JoinClause]\n\r" + 
		                            "\n\r" + 
	                                "    END\n\r";

                    trgUpdate = "    ELSE IF @type ='I'\n\r" + 
	                            "    BEGIN\n\r" + 
		                        "        INSERT INTO audit.CaseManagement_Audit\n\r" + 
		                        "        SELECT inserted.UserID,'Case_Checklist',str(inserted.[ID1]),[ID2],[ID3],@type,GetDate()\n\r" + 
		                        "        FROM Inserted\n\r" + 
		                        "        SELECT @AuditID = @@IDENTITY\n\r" + 
		                        "\n\r" + 
		                        "        INSERT INTO audit.CaseManagement_AuditDetail\n\r" + 
		                        "        SELECT @AuditID,null," + ColumnName + " FROM Inserted\n\r" + 
	                            "    END\n\r";

                    trgDelete = "    ELSE IF @type = 'D'\n\r" + 
	                            "    BEGIN\n\r" + 
		                        "        INSERT INTO audit.CaseManagement_Audit\n\r" + 
		                        "        SELECT Deleted.UserID,'Case_Checklist',str(Deleted.[ID1]),[ID2],[ID3],@type,GetDate()\n\r" + 
		                        "        FROM Deleted\n\r" + 
		                        "        SELECT @AuditID = @@IDENTITY\n\r" + 
	                            "	\n\r" + 
		                        "        INSERT INTO audit.CaseManagement_AuditDetail\n\r" + 
		                        "        SELECT @AuditID," + ColumnName + ",null FROM deleted\n\r" + 
	                            "	\n\r" + 
	                            "    END\n\r";

                }

                //each column


            }
            //if (SQLTriggerOutput != "")
            //{
            //    SQLTriggerOutput += "" + Variables;
            //    SQLTriggerOutput += "AS BEGIN\r";
            //    SQLTriggerOutput += "" + Columns;
            //    SQLTriggerOutput += "" + WhereClause;
            //    SQLTriggerOutput += "\rEND\rGO\r";
            //}
            #endregion

            #region Update Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    if (SQLUpdateOutput != "")
                    {
                        SQLUpdateOutput += "" + Variables;
                        SQLUpdateOutput += "AS BEGIN\r";
                        SQLUpdateOutput += "" + Columns;
                        SQLUpdateOutput += "" + WhereClause;
                        SQLUpdateOutput += "\rEND\rGO\r";
                    }

                    SQLUpdateOutput += "\rCREATE PROCEDURE usp_" + TableName + "_Update \r";
                    //SQLUpdateOutput += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                if (oDt.Rows[i]["IDENTITY"].ToString() == "1")
                {
                    if (WhereClause == "")
                        WhereClause += "WHERE " + ColumnName + " = @" + ColumnName + "\r";
                    else
                        WhereClause += "    AND " + ColumnName + " = @" + ColumnName + "\r";
                    if (Variables == "")
                        Variables = "    @" + ColumnName + " " + DataType + "\r";
                    else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
                }
                else
                {
                    if (Variables == "")
                        Variables = "    @" + ColumnName + " " + DataType + "\r";
                    else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
                    if (Columns == "")
                        Columns = "UPDATE " + SchemaName + "." + TableName + "\rSET   " + ColumnName + " = @" + ColumnName + "\r";
                    else Columns += "    ," + ColumnName + " = @" + ColumnName + "\r";
                }

            }
            if (SQLUpdateOutput != "")
            {
                SQLUpdateOutput += "" + Variables;
                SQLUpdateOutput += "AS BEGIN\r";
                SQLUpdateOutput += "" + Columns;
                SQLUpdateOutput += "" + WhereClause;
                SQLUpdateOutput += "\rEND\rGO\r";
            }
            #endregion
            #region Insert Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    if (SQLInsertOutput != "")
                    {
                        SQLInsertOutput += "" + Variables;
                        SQLInsertOutput += "AS BEGIN\r";
                        SQLInsertOutput += "" + Columns + ")";
                        SQLInsertOutput += "" + WhereClause;
                        SQLInsertOutput += "END\rGO\r";
                    }

                    SQLInsertOutput += "\rCREATE PROCEDURE usp_" + TableName + "_Insert\r";
                    //SQLInsertOutput += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                if (oDt.Rows[i]["IDENTITY"].ToString() == "1")
                {
                    //if (WhereClause == "")
                    //    WhereClause += "WHERE " + ColumnName + " = @" + ColumnName + "\r";
                    //else
                    //    WhereClause += "    AND " + ColumnName + " = @" + ColumnName + "\r";
                }
                else
                {
                    if (Variables == "")
                        Variables = "    @" + ColumnName + " " + DataType + "\r";
                    else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
                    if (Columns == "")
                        Columns = "INSERT INTO " + SchemaName + "." + TableName + "\rVALUES(@" + ColumnName + "\r";
                    else Columns += "    ,@" + ColumnName + "\r";
                }

            }
            if (SQLInsertOutput != "")
            {
                SQLInsertOutput += "" + Variables;
                SQLInsertOutput += "AS BEGIN\r";
                SQLInsertOutput += "" + Columns + ")";
                SQLInsertOutput += "" + WhereClause;
                SQLInsertOutput += "END\rGO\r";
            }
            #endregion
            #region Delete Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    if (SQLDeleteOutput != "")
                    {
                        SQLDeleteOutput += "" + Variables;
                        SQLDeleteOutput += "AS BEGIN\r";
                        SQLDeleteOutput += "" + Columns;
                        SQLDeleteOutput += "" + WhereClause;
                        SQLDeleteOutput += "END\rGO\r";
                    }

                    SQLDeleteOutput += "\rCREATE PROCEDURE usp_" + TableName + "_Delete \r";
                    //SQLDeleteOutput += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                if (oDt.Rows[i]["IDENTITY"].ToString() == "1" || (TableName.Contains('_') && ColumnName.EndsWith("ID")))
                {
                    if (Columns == "")
                        Columns = "DELETE FROM " + SchemaName + "." + TableName + "\r";
                    if (ColumnName != "UserID")
                    {
                        if (WhereClause == "")
                            WhereClause += "WHERE " + ColumnName + " = @" + ColumnName + "\r";
                        else
                            WhereClause += "    AND " + ColumnName + " = @" + ColumnName + "\r";
                    }
                    if (Variables == "")
                        Variables = "    @" + ColumnName + " " + DataType + "\r";
                    else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
                }
                else
                {
                    //if (Variables == "")
                    //    Variables = "    @" + ColumnName + " " + DataType + "\r";
                    //else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
                    //VALUES(" + ColumnName + "\r";
                    //else Columns += "    ," + ColumnName + "\r";
                }

            }
            if (SQLDeleteOutput != "")
            {
                SQLDeleteOutput += "" + Variables;
                SQLDeleteOutput += "AS BEGIN\r";
                SQLDeleteOutput += "" + Columns;
                SQLDeleteOutput += "" + WhereClause;
                SQLDeleteOutput += "END\rGO\r";
            }
            #endregion
            #region Select Statements
            //for (int i = 0; i < oDt.Rows.Count; i++)
            //{
            //    ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
            //    DataType = oDt.Rows[i]["DATA_TYPE"].ToString();

            //    if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
            //    {
            //        TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
            //        SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

            //        if (SQLSelectOutput != "")
            //        {
            //            SQLSelectOutput += "" + Variables;
            //            SQLSelectOutput += "AS BEGIN\r";
            //            SQLSelectOutput += "" + Columns;
            //            SQLSelectOutput += "" + FromClause;
            //            SQLSelectOutput += "" + WhereClause;
            //            SQLSelectOutput += "END\rGO\r";
            //        }

            //        SQLSelectOutput += "\rCREATE PROCEDURE usp_" + TableName + "_Select \r";
            //        //SQLSelectOutput += "\rEND\rGO";
            //        Variables = "";
            //        Columns = "";
            //        WhereClause = "";
            //        FromClause = "FROM " + SchemaName + "." + TableName + "\r";
            //    }
            //    if (oDt.Rows[i]["IDENTITY"].ToString() == "1")
            //    {
            //        if (WhereClause == "")
            //            WhereClause += "WHERE " + ColumnName + " = @" + ColumnName + "\r";
            //        else
            //            WhereClause += "    AND " + ColumnName + " = @" + ColumnName + "\r";
            //        if (Variables == "")
            //            Variables = "    @" + ColumnName + " " + DataType + "\r";
            //        else Variables += "    ,@" + ColumnName + " " + DataType + "\r";
            //        if (Columns == "")
            //            Columns = "SELECT " + ColumnName + "\r";
            //        else Columns += "    ," + ColumnName + "\r";
                    
            //    }
            //    else
            //    {
                    

            //        if (Columns == "")
            //            Columns = "SELECT " + ColumnName + "\r";
            //        else Columns += "    ," + ColumnName + "\r";
            //    }

            //}
            //if (SQLSelectOutput != "")
            //{
            //    SQLSelectOutput += "" + Variables;
            //    SQLSelectOutput += "AS BEGIN\r";
            //    SQLSelectOutput += "" + Columns;
            //    SQLSelectOutput += "" + FromClause;
            //    SQLSelectOutput += "" + WhereClause;
            //    SQLSelectOutput += "END\rGO\r";
            //}
            #endregion
            #region C# Code Generator
            #region Update Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();
                switch (DataType.Substring(0, 3).ToLower())
                {
                    case "var": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "char": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "dat":
                        if (DataType.ToUpper().Contains("DATETIME2"))
                        {
                            CDataType = "DbType.DateTime2";
                            NormalType = "DateTime";
                        }
                        else
                        {
                            CDataType = "DbType.Date";
                            NormalType = "DateTime";
                        }
                        break;
                    case "bit": CDataType = "DbType.Boolean";
                        NormalType = "bool";
                        break;
                    case "int": CDataType = "DbType.Int32";
                        NormalType = "int";
                        break;
                    case "uni": CDataType = "DbType.Guid";
                        NormalType = "Guid";
                        break;
                    case "tim": CDataType = "DbType.Time";
                        NormalType = "DateTime";
                        break;
                    case "dec": CDataType = "DbType.Decimal";
                        NormalType = "decimal";
                        break;
                    default: CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                }

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {

                    if (COutputUpdate != "")
                    {
                        COutputUpdate += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Update\");\r";
                        COutputUpdate += "" + Variables;
                        COutputUpdate += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                        COutputUpdate += "}";
                    }

                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    COutputUpdate += "\rpublic DataTable usp_" + TableName + "_Update(";

                    //COutputUpdate += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
                if (Columns == "")
                    Columns += NormalType + " " + ColumnName;
                else 
                    Columns += "," + NormalType + " " + ColumnName;

            }
            if (COutputUpdate != "")
            {
                COutputUpdate += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Update\");\r";
                COutputUpdate += "" + Variables;
                COutputUpdate += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                COutputUpdate += "}";
            }
            #endregion
            #region Insert Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();
                switch (DataType.Substring(0, 3).ToLower())
                {
                    case "var": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "char": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "dat":
                        if (DataType.ToUpper().Contains("DATETIME2"))
                        {
                            CDataType = "DbType.DateTime2";
                            NormalType = "DateTime";
                        }
                        else
                        {
                            CDataType = "DbType.Date";
                            NormalType = "DateTime";
                        }
                        break;
                    case "bit": CDataType = "DbType.Boolean";
                        NormalType = "bool";
                        break;
                    case "int": CDataType = "DbType.Int32";
                        NormalType = "int";
                        break;
                    case "uni": CDataType = "DbType.Guid";
                        NormalType = "Guid";
                        break;
                    case "tim": CDataType = "DbType.Time";
                        NormalType = "DateTime";
                        break;
                    case "dec": CDataType = "DbType.Decimal";
                        NormalType = "decimal";
                        break;
                    default: CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                }

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    if (COutputInsert != "")
                    {
                        COutputInsert += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Insert\");\r";
                        COutputInsert += "" + Variables;
                        COutputInsert += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                        COutputInsert += "}";
                    }

                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    COutputInsert += "\rpublic DataTable usp_" + TableName + "_Insert(";

                    //COutputInsert += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                if (oDt.Rows[i]["IDENTITY"].ToString() == "1")
                {//oDb.AddInParameter(sprocCmd, "CategoryDescription", DbType.String, CategoryDescription);
                    //Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
                }
                else
                {
                    Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
                    if (Columns == "")
                        Columns += NormalType + " " + ColumnName;
                    else
                        Columns += "," + NormalType + " " + ColumnName;
                }

            }
            if (COutputInsert != "")
            {
                COutputInsert += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Insert\");\r";
                COutputInsert += "" + Variables;
                COutputInsert += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                COutputInsert += "}";
            }
            #endregion
            #region Delete Statements
            for (int i = 0; i < oDt.Rows.Count; i++)
            {
                ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
                DataType = oDt.Rows[i]["DATA_TYPE"].ToString();
                switch (DataType.Substring(0, 3).ToLower())
                {
                    case "var": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "char": CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                    case "dat":
                        if (DataType.ToUpper().Contains("DATETIME2"))
                        {
                            CDataType = "DbType.DateTime2";
                            NormalType = "DateTime";
                        }
                        else
                        {
                            CDataType = "DbType.Date";
                            NormalType = "DateTime";
                        }
                        break;
                    case "bit": CDataType = "DbType.Boolean";
                        NormalType = "bool";
                        break;
                    case "int": CDataType = "DbType.Int32";
                        NormalType = "int";
                        break;
                    case "uni": CDataType = "DbType.Guid";
                        NormalType = "Guid";
                        break;
                    case "tim": CDataType = "DbType.Time";
                        NormalType = "DateTime";
                        break;
                    case "dec": CDataType = "DbType.Decimal";
                        NormalType = "decimal";
                        break;
                    default: CDataType = "DbType.String";
                        NormalType = "string";
                        break;
                }

                if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
                {
                    if (COutputDelete != "")
                    {
                        COutputDelete += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Delete\");\r";
                        COutputDelete += "" + Variables;
                        COutputDelete += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                        COutputDelete += "}";
                    }

                    TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
                    SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

                    COutputDelete += "\rpublic DataTable usp_" + TableName + "_Delete(";

                    //COutputDelete += "\rEND\rGO";
                    Variables = "";
                    Columns = "";
                    WhereClause = "";
                }
                if (oDt.Rows[i]["IDENTITY"].ToString() == "1" || (TableName.Contains('_') && ColumnName.EndsWith("ID")))
                {//oDb.AddInParameter(sprocCmd, "CategoryDescription", DbType.String, CategoryDescription);
                    Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
                    if (Columns == "")
                        Columns += NormalType + " " + ColumnName;
                    else
                        Columns += "," + NormalType + " " + ColumnName;
                }
                else
                {
                    // Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
                }

            }
            if (COutputDelete != "")
            {
                COutputDelete += Columns + ")\r"
                    + "{\r"
                    + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Delete\");\r";
                COutputDelete += "" + Variables;
                COutputDelete += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
                COutputDelete += "}";
            }
            #endregion
            #region Select Statements
            //for (int i = 0; i < oDt.Rows.Count; i++)
            //{
            //    ColumnName = oDt.Rows[i]["COLUMN_NAME"].ToString();
            //    DataType = oDt.Rows[i]["DATA_TYPE"].ToString();
            //    switch (DataType.Substring(0, 3).ToLower())
            //    {
            //        case "var": CDataType = "DbType.String";
            //            NormalType = "string";
            //            break;
            //        case "char": CDataType = "DbType.String";
            //            NormalType = "string";
            //            break;
            //        case "dat":
            //            if (DataType.ToUpper().Contains("DATETIME2"))
            //            {
            //                CDataType = "DbType.DateTime2";
            //                NormalType = "DateTime";
            //            }
            //            else
            //            {
            //                CDataType = "DbType.Date";
            //                NormalType = "DateTime";
            //            }
            //            break;
            //        case "bit": CDataType = "DbType.Boolean";
            //            NormalType = "bool";
            //            break;
            //        case "int": CDataType = "DbType.Int32";
            //            NormalType = "int";
            //            break;
            //        case "uni": CDataType = "DbType.Guid";
            //            NormalType = "Guid";
            //            break;
            //        case "tim": CDataType = "DbType.Time";
            //            NormalType = "DateTime";
            //            break;
            //        case "dec": CDataType = "DbType.Decimal";
            //            NormalType = "decimal";
            //            break;
            //        default: CDataType = "DbType.String";
            //            NormalType = "string";
            //            break;
            //    }

            //    if (TableName != oDt.Rows[i]["TABLE_NAME"].ToString())
            //    {
            //        if (COutputSelect != "")
            //        {
            //            COutputSelect += Columns + ")\r"
            //        + "{\r"
            //        + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Select\");\r";
            //            COutputSelect += "" + Variables;
            //            COutputSelect += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
            //            COutputSelect += "}";
            //        }

            //        TableName = oDt.Rows[i]["TABLE_NAME"].ToString();
            //        SchemaName = oDt.Rows[i]["TABLE_SCHEMA"].ToString();

            //        COutputSelect += "\rpublic DataTable usp_" + TableName + "_Select(";

            //        //COutputSelect += "\rEND\rGO";
            //        Variables = "";
            //        Columns = "";
            //        WhereClause = "";
            //    }
            //    if (oDt.Rows[i]["IDENTITY"].ToString() == "1")
            //    {//oDb.AddInParameter(sprocCmd, "CategoryDescription", DbType.String, CategoryDescription);
            //        Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
            //        if (Columns == "")
            //            Columns += NormalType + " " + ColumnName;
            //        else
            //            Columns += "," + NormalType + " " + ColumnName;
            //    }
            //    else
            //    {
            //        //Variables += "oDb.AddInParameter(sprocCmd, \"" + ColumnName + "\", " + CDataType + ",  " + ColumnName + ");\r";
            //    }

            //}
            //if (COutputSelect != "")
            //{
            //    COutputSelect += Columns + ")\r"
            //        + "{\r"
            //        + " DbCommand sprocCmd = oDb.GetStoredProcCommand(\"usp_" + TableName + "_Select\");\r";
            //    COutputSelect += "" + Variables;
            //    COutputSelect += "return  oDb.ExecuteDataSet(sprocCmd).Tables[0];\r";
            //    COutputSelect += "}";
            //}
            #endregion
            #endregion

            string SQLFinalOutput = SQLDeleteOutput + "\n\r" + SQLInsertOutput + "\n\r" + SQLSelectOutput + "\n\r" + SQLUpdateOutput;
            string CFinalOutput = COutputSelect + "\n\r" + COutputDelete + "\n\r" + COutputInsert + "\n\r" + COutputUpdate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class ImportDRDEmployeeFiles : Form
    {
        OpenFileDialog oOpenFile = new OpenFileDialog();
        FileImports oFileImport = new FileImports(Program.oDb);

        public ImportDRDEmployeeFiles()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            
            oOpenFile.ShowDialog();
            if (oOpenFile.CheckFileExists)
            {
                txtFileName.Text = oOpenFile.FileName;
            }
            else
            {
                MessageBox.Show("No File Selected");
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (oOpenFile.CheckFileExists)
            {
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+oOpenFile.FileName+";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";

                string sql = "select * from [" + txtSheetName.Text + "$]";
                OleDbDataAdapter cmd = new OleDbDataAdapter(sql, connectionString);

                DataSet excelDataSet = new DataSet();
                cmd.Fill(excelDataSet);
                cmd.Dispose();

                oFileImport.usp_Members_DRD_Truncate();

                for (int i = 0; i < excelDataSet.Tables[0].Rows.Count; i++)
                {
                    oFileImport.usp_Members_DRD_InsertRecord(
                        excelDataSet.Tables[0].Rows[i]["M_no"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["Title"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["Surname"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["Init"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["Name"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["EB NUMBER"].ToString()
                        , excelDataSet.Tables[0].Rows[i]["DOB"].ToString()
                        , Program.Username);
                }

                oFileImport.usp_Members_DRD_Merge();
                MessageBox.Show("Done");
            }
            else
            {
                MessageBox.Show("Please select a file first");
            }
        }

        private void ImportDRDEmployeeFiles_Load(object sender, EventArgs e)
        {

        }
    }
}

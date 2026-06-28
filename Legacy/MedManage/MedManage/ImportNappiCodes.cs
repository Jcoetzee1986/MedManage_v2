using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib;
using System.Data.OleDb;

namespace Icondev.MedManage
{
    public partial class ImportNappiCodes : Form
    {
        OpenFileDialog oOpenFile = new OpenFileDialog();
        FileImports oFileImport = new FileImports(Program.oDb);

        public ImportNappiCodes()
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
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + oOpenFile.FileName + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";

                string sql = "select * from [" + txtSheetName.Text + "$]";
                OleDbDataAdapter cmd = new OleDbDataAdapter(sql, connectionString);

                DataSet excelDataSet = new DataSet();
                cmd.Fill(excelDataSet);
                cmd.Dispose();

                oFileImport.usp_Nappi_InsertRecord_Truncate();

                for (int i = 0; i < excelDataSet.Tables[0].Rows.Count; i++)
                {
                    oFileImport.usp_Nappi_InsertRecord(
                        excelDataSet.Tables[0].Rows[i][1].ToString() // Nappi Code
                        , excelDataSet.Tables[0].Rows[i][2].ToString() // Description
                        , excelDataSet.Tables[0].Rows[i][3].ToString() // Units
                        , excelDataSet.Tables[0].Rows[i][4].ToString() // Measure
                        , excelDataSet.Tables[0].Rows[i][14].ToString() // Price1
                        , excelDataSet.Tables[0].Rows[i][15].ToString() // Price2
                        , Convert.ToDateTime(oOpenFile.SafeFileName.Substring(0,oOpenFile.SafeFileName.LastIndexOf('.'))) // Date
                        );
                    txtLog.Text += ".";
                }
                
                oFileImport.usp_Nappi_InsertRecord();
                MessageBox.Show("Done");
            }
            else
            {
                MessageBox.Show("Please select a file first");
            }
        }
    }
}

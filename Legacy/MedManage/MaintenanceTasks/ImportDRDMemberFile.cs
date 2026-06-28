using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using OfficeOpenXml;
using System.Data;

namespace MaintenanceTasks
{
    class ImportDRDMemberFile
    {
        public ImportDRDMemberFile()
        {
            string FileName = "";
            FileName = "";// Server.MapPath("TempFiles") + "\\";// +uplStudyGroup.FileName;
            FileName = FileName.Replace("Training\\", "");
            //uplStudyGroup.SaveAs(FileName);
            FileInfo newFile = new FileInfo(FileName);
            //ExcelPackage oExcel = new ExcelPackage(newFile);

            //foreach (ExcelWorksheet oSheet in oExcel.Workbook.Worksheets)
            //        {
            //            FarmerID = -1;

            //            if (oSheet.Name.Contains("Sheet"))
            //            {
            //                string Surname = oSheet.Cell(7, 2).Value;
            //                if (Surname.Replace(" ", "").Length > 1)
            //                {
            //                    if (oSheet.Cell(1, 26).Value != "")
            //                        throw new Exception("Error on sheet " + oSheet.Name + "; in workbook " + uplStudyGroup.FileName);
            //                    string Initials = oSheet.Cell(7, 3).Value;
            //                    string IDNumber = oSheet.Cell(7, 4).Value;
        }
    }
}

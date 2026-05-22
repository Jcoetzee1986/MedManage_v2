using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Diagnostics;
using System.Data;
using Icondev.MedManage.MedManageLib;
using System.Security.Principal;

namespace Icondev.MedManage
{
    static class Program
    {
        public static string Username { get; set; }
        public static int MainClientID { get; set; }
        public static string MainClientName { get; set; }
        public static bool DevMode { get; set; }
        public static Database oDb { get; set; }
        public static GenericPrincipal _GenericPrincipal { get; set; }
        public static bool EnableBilling { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MedManageForm());
        }
        public static string GetProperStringDate(DateTime Date)
        {
            string StringDate = "";
            StringDate += Date.Year.ToString() + "/";
            StringDate += Date.Month.ToString().PadLeft(2, '0') + "/";
            StringDate += Date.Day.ToString().PadLeft(2, '0');

            return StringDate;
        }
        public static DateTime GetProperDateTimeDate(DateTime Date)
        {
            DateTime oDate = new DateTime(Date.Year, Date.Month, Date.Day);
            return oDate;
        }
        public static int ConvertDateToInt(DateTime date)
        {
            string stringDate = "";
            int intDate = 01;
            stringDate += date.Year.ToString();
            stringDate += date.Month.ToString().PadLeft(2, '0');
            stringDate += date.Day.ToString().PadLeft(2, '0');
            return intDate = Convert.ToInt32(stringDate);
        }
        public static DateTime ConvertIntToDate(int intDate)
        {
            try
            {
                string stringDate = intDate.ToString();
                DateTime date = new DateTime(Convert.ToInt32(stringDate.Substring(0, 4)), Convert.ToInt32(stringDate.Substring(4, 2)), Convert.ToInt32(stringDate.Substring(6, 2)));
                return date;
            }
            catch
            {
                try
                {
                    return Convert.ToDateTime(intDate);
                }
                catch
                {
                    return DateTime.Now;
                }

            }
        }
        public static bool IsUpdatesAvailable()
        {
            try
            {
                bool updated = false;
                string path = System.Windows.Forms.Application.StartupPath.ToString() + "\\GetUpdates.exe";
                ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                AppUpdates oUpdates = new AppUpdates(Program.oDb);
                DataTable oDt = oUpdates.usp_AppUpdates_Select();

                if (oDt != null)
                    for (int i = 0; i < oDt.Rows.Count; i++)
                    {
                        if (oDt.Rows[i]["ClientComputerName"].ToString() == System.Windows.Forms.SystemInformation.ComputerName)
                        {
                            updated = true;
                        }
                    }
                if (oDt.Rows.Count > 0)
                {
                    if (oDt.Rows[0]["AppVersion"].ToString() == version && !updated)
                    {
                        oUpdates.usp_AppUpdates_Insert(Convert.ToInt32(oDt.Rows[0]["UpdateID"].ToString())
                            ,System.Windows.Forms.SystemInformation.ComputerName);
                        return false;
                    }
                    else if (oDt.Rows[0]["AppVersion"].ToString() != version)
                    {
                        DialogResult result = MessageBox.Show("There are updates available for your application\n\r" +
                            "The application will now close to install these updates. After the process is finished you can re-open the application.", "Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {

                            //specify the name and the arguements you want to pass to the command prompt
                            psi.FileName = path;
                            psi.Arguments = oDt.Rows[0]["UpdatePath"].ToString();
                            //if you don't want a console window popping up then set this property
                            //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                            //Create new process and set the starting information
                            System.Diagnostics.Process p = new System.Diagnostics.Process();
                            p.StartInfo = psi;


                            //Set this so that you can tell when the process has completed
                            //p.EnableRaisingEvents = true;

                            p.Start();
                            return true;
                        }
                        else return false;
                    }
                    else return false;

                }
                else return false;

            }
            catch (Exception err)
            {
                err.Message.ToString();
                return false;

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Icondev.MedManage
{
    public partial class rpt_ReportServer : Form
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public rpt_ReportServer(string ReportName,string Parameters)
        {
            InitializeComponent();
            webReportServer.Url = new Uri(String.Format("{0}?%2f{1}%2f{2}&rs:Command=Render{3}"
                ,config.AppSettings.Settings["ReportServer"].Value
                , config.AppSettings.Settings["ReportServerMainFolder"].Value
                , ReportName
                , Parameters));
        }

        private void rpt_ReportServer_Load(object sender, EventArgs e)
        {
            
        }
    }
}

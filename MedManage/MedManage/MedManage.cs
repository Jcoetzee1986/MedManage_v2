using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Security;
using System.Security.Principal;
using System.Web.Security;
using Icondev.MedManage.MedManageLib;
using System.Configuration;
using System.Net.NetworkInformation;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Icondev.MedManage
{
    public partial class MedManageForm : Form
    {
        private bool authenticated;		// Data provided by user resulted in authenticated identity
        // Providers
        //private IAuthenticationProvider authenticationProvider;
        private IAuthorizationProvider ruleProvider;
        //private IRolesProvider rolesProvider;
        //private IProfileProvider profileProvider;
        private IIdentity identity;		// Identity for authenticated users
        //private IToken token;					// Token for valid identity
        private ISecurityCacheProvider cache;	// Security cache to handle tokens
        private string[] oRoles;   //All the roles that the users are in
        private GenericPrincipal oGenericPrincipal;
        string username = "";
        string password = "";
        public Database oDb { get; set; }

        MyCases oMyCases;// = new MyCases();

        public MedManageForm()
        {
            InitializeComponent();
            
        }

        private void MedManageForm_Load(object sender, EventArgs e)
        {
            try // Set up datebase connection
            {
                //Create configuration variale
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //Read the connection string
                string connection = config.ConnectionStrings.ConnectionStrings["DQLDB.MedManage"].ToString();

                if (connection.ToLower().Contains("_dev"))
                {
                    this.BackColor = Color.Green;
                    Program.DevMode = true;
                }

                //Check if billing is switched on
                if (config.AppSettings.Settings["EnableBilling"].Value == "true")
                {
                    Program.EnableBilling = true;
                }
                else
                {
                    Program.EnableBilling = false;
                }

                DialogResult oResult = System.Windows.Forms.DialogResult.No;
                //if (!config.AppSettings.Settings["LiveServer"].Value.StartsWith("."))
                //{
                //    if (!CanSeeServer(config.AppSettings.Settings["LiveServer"].Value))
                //    {
                //        oResult = MessageBox.Show("The application cannot see the database server. Are you working on the VPN?", "Network error", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                //        if (oResult == System.Windows.Forms.DialogResult.No)
                //        {
                //            MessageBox.Show("The application will now close. Please check your network connection and try again", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //            this.Close();
                //            return;
                //        }
                //        else if (oResult == System.Windows.Forms.DialogResult.Yes)
                //        {
                //            if (!CanSeeServer(config.AppSettings.Settings["VPNServer"].Value))
                //            {
                //                MessageBox.Show("The server cannot be reached.\n\rThe application will now close. Please check your network connection and try again", "Network error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //                this.Close();
                //                return;
                //            }
                //            else
                //            {
                //                connection = ReplaceDataSource(connection, config.AppSettings.Settings["VPNServer"].Value);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        connection = ReplaceDataSource(connection, config.AppSettings.Settings["LiveServer"].Value);
                //    }
                //}

                //// Update connection strings in memory only — do NOT save to disk
                //SetConnectionStringInMemory("DQLDB.MedManage", connection);
                //SetConnectionStringInMemory("Icondev.MedManage.Properties.Settings.MedManageConnectionString", connection);

                //Initialize database Connection
                oDb = DatabaseFactory.CreateDatabase();

                Program.oDb = oDb;
                // Initializes the Enterprise Library authorization and security caching providers
                // The ASP.NET Membership and Profile providers do not need to be initialized in this way
                this.ruleProvider = AuthorizationFactory.GetAuthorizationProvider("RuleProvider");
                this.cache = SecurityCacheFactory.GetSecurityCacheProvider("Caching Store Provider");
                
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                throw;
            }  
            
            CheckForUpdates();
            
            loginToolStripMenuItem.PerformClick();
            
        }

        /// <summary>
        /// Sets a connection string value in memory without writing to disk.
        /// Unlocks the read-only flags on both the collection and the individual element.
        /// </summary>
        private void SetConnectionStringInMemory(string name, string connectionString)
        {
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            var readOnlyField = typeof(ConfigurationElementCollection).GetField("bReadOnly", flags);
            var elementReadOnlyField = typeof(ConfigurationElement).GetField("_bReadOnly", flags);

            // Unlock the collection
            readOnlyField.SetValue(ConfigurationManager.ConnectionStrings, false);

            var connStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connStringSettings != null)
            {
                // Unlock the individual element
                elementReadOnlyField.SetValue(connStringSettings, false);
                connStringSettings.ConnectionString = connectionString;
            }
        }

        private string ReplaceDataSource(string connectionString, string newDataSource)
        {
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            builder.DataSource = newDataSource;
            return builder.ConnectionString;
        }

        private void CheckForUpdates()
        {
            if (Program.IsUpdatesAvailable())
                Close();
        }

        public bool CanSeeServer(string address)
        {
            // Extract the host portion, stripping any named instance (e.g., "server\instance")
            //string host = address;
            //int port = 1433; // Default SQL Server port

            //int backslashIndex = address.IndexOf('\\');
            //if (backslashIndex >= 0)
            //{
            //    host = address.Substring(0, backslashIndex);
            //}

            //try
            //{
            //    using (var client = new System.Net.Sockets.TcpClient())
            //    {
            //        var connectTask = client.BeginConnect(host, port, null, null);
            //        bool connected = connectTask.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
            //        if (connected && client.Connected)
            //        {
            //            client.EndConnect(connectTask);
            //            return true;
            //        }
            //    }
            //}
            //catch { }

            //return false;
            return true; //TODO: Remove this when done testing
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserMaintenance oUserMaintenance = new UserMaintenance();
            oUserMaintenance.MdiParent = this;
            oUserMaintenance.Show();
        }

        private void systemDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemData oSystemDatae = new SystemData();
            oSystemDatae.MdiParent = this;
            oSystemDatae.Show();
            //MedManageLib.CreateSPandCSharp oC = new CreateSPandCSharp(oDb);
        }

        private void chronicIllnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChronicIllnesses oChronicIllnesses = new ChronicIllnesses();
            oChronicIllnesses.MdiParent = this;
            oChronicIllnesses.Show();
        }

        private void exclusionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exclusions oExclusions = new Exclusions();
            oExclusions.MdiParent = this;
            oExclusions.Show();
        }

        private void medicalAidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MedicalAid oMedicalAid = new MedicalAid();
            oMedicalAid.MdiParent = this;
            oMedicalAid.Show();
        }

        //private void tariffsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    TariffManagement oTariffManagement = new TariffManagement();
        //    oTariffManagement.MdiParent = this;
        //    oTariffManagement.Show();
        //}

        private void serviceProviderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServiceProviderLookup oServiceProviderLookup = new ServiceProviderLookup(false,"","","");
            oServiceProviderLookup.MdiParent = this;
            oServiceProviderLookup.Show();
        }

        private void memberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberLookup oMemberLookup = new MemberLookup(false,"","","","");
            oMemberLookup.MdiParent = this;
            oMemberLookup.Show();
        }

        private void devToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateSPandCSharp oCreate = new CreateSPandCSharp(Program.oDb);
        }

        private void searchForCasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyCases oMyCases = new MyCases();
            oMyCases.MdiParent = this;
            oMyCases.WindowState = FormWindowState.Normal;
            oMyCases.StartPosition = FormStartPosition.Manual;
            oMyCases.Show();
        }

        private void addANewCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Case oCase = new Case(-1);
            oCase.MdiParent = this;
            oCase.WindowState = FormWindowState.Normal;
            oCase.Text = "New Case - " + Convert.ToString(DateTime.Now.TimeOfDay).Substring(0, 5);
            oCase.Show();
        }

        private void deleteUserSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUserSession oDelete = new DeleteUserSession();
            oDelete.ShowDialog();
        }

        private void changeMyPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserChangePassword oUserChangePassword = new UserChangePassword();
            oUserChangePassword.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bookingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bookings oBookings = new Bookings();
            oBookings.MdiParent = this.MdiParent;
            oBookings.Show();
        }

        private void caseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rpt_Cases_BetweenDates orpt_Cases_BetweenDates = new rpt_Cases_BetweenDates();
            orpt_Cases_BetweenDates.MdiParent = this.MdiParent;
            orpt_Cases_BetweenDates.Show();
        }

        private void caseDetailsByAdmissionDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rpt_Cases_BetweenAdmitDates orpt_Cases_BetweenDates = new rpt_Cases_BetweenAdmitDates();
            orpt_Cases_BetweenDates.MdiParent = this.MdiParent;
            orpt_Cases_BetweenDates.Show();
        }

        private void financeCasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rpt_Finance orpt_Finance = new rpt_Finance();
            orpt_Finance.MdiParent = this.MdiParent;
            orpt_Finance.Show();
        }

        private void dRDEmployeeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportDRDEmployeeFiles oImportDRDEmployeeFiles = new ImportDRDEmployeeFiles();
            oImportDRDEmployeeFiles.MdiParent = this.MdiParent;
            oImportDRDEmployeeFiles.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                authenticated = false;
                while (authenticated == false)
                {
                    Login oLogin = new Login();
                    oLogin.ShowDialog(this);
                    username = oLogin.UserName;
                    password = oLogin.Password;
                    if (username == null)
                        Close();
                    //Membership.
                    authenticated = Membership.ValidateUser(username, password);

                }
                Program.Username = username;
                identity = new GenericIdentity(username, Membership.Provider.Name);

                oRoles = Roles.GetRolesForUser(username);
                oGenericPrincipal = new GenericPrincipal(new GenericIdentity(username), oRoles);
                Program._GenericPrincipal = oGenericPrincipal; //Expose so other items can use it
                reportsToolStripMenuItem.Visible = true;

                if (oGenericPrincipal.IsInRole("Case Manager"))
                {
                    caseManagementToolStripMenuItem.Visible = true;
                }
                if (oGenericPrincipal.IsInRole("System Administrator"))
                {
                    adminToolStripMenuItem.Visible = true;
                    metadataToolStripMenuItem.Visible = true;
                }
                if (oGenericPrincipal.IsInRole("Metadata Administrator"))
                {
                    metadataToolStripMenuItem.Visible = true;
                    systemDataToolStripMenuItem.Visible = true;
                }
                if (oGenericPrincipal.IsInRole("Imports"))
                {
                    importsToolStripMenuItem.Visible = true;
                }
                if (oGenericPrincipal.IsInRole("Billing Auditing"))
                {
                    //DONE: (Billing) Taken out for Tariff Change. Needs to be put back
                    financeToolStripMenuItem.Visible = true;
                }

                ChooseClient oChooseClient = new ChooseClient();
                //oChooseClient.MdiParent = this;
                oChooseClient.ShowDialog();

                this.Text = "Med Manage - " + Program.MainClientName;

                if(this.MdiChildren.Contains(oMyCases))
                    oMyCases.Close();
                oMyCases = new MyCases();
                oMyCases.MdiParent = this;
                oMyCases.WindowState = FormWindowState.Normal;
                oMyCases.StartPosition = FormStartPosition.Manual;
                oMyCases.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void chooseClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChooseClient oChooseClient = new ChooseClient();
            //oChooseClient.MdiParent = this;
            oChooseClient.ShowDialog();

            this.Text = "Med Manage - " + Program.MainClientName;

            if (this.MdiChildren.Contains(oMyCases))
                oMyCases.Close();
            oMyCases = new MyCases();
            oMyCases.MdiParent = this;
            oMyCases.WindowState = FormWindowState.Normal;
            oMyCases.StartPosition = FormStartPosition.Manual;
            oMyCases.Show();
        }

        private void nappiCodesImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportNappiCodes oImportNappiCodes = new ImportNappiCodes();
            oImportNappiCodes.MdiParent = this;
            oImportNappiCodes.Show();
        }

        private void dataCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinanceDataCapture oFinanceDataCapture = new FinanceDataCapture();
            oFinanceDataCapture.MdiParent = this;
            oFinanceDataCapture.Show();
        }

        private void bulkPaymentCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FinanceBulkPayment oFinanceBulkPayment = new FinanceBulkPayment();
            oFinanceBulkPayment.MdiParent = this;
            oFinanceBulkPayment.Show();
        }

        private void caseDetailsByParentCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rpt_Cases_BetweenDates_Primary orpt_Cases_BetweenDates = new rpt_Cases_BetweenDates_Primary();
            orpt_Cases_BetweenDates.MdiParent = this.MdiParent;
            orpt_Cases_BetweenDates.Show();
        }

        private void wipExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rpt_BillingSummary orpt_BillingSummary = new rpt_BillingSummary();
            orpt_BillingSummary.MdiParent = this;
            orpt_BillingSummary.Show();
        }
    }
}

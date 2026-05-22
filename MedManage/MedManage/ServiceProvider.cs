using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class ServiceProvider : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        Finance oFinance = new Finance(Program.oDb);
        public int ServiceProviderID { get; set; }
        int SPBaseTariff;
        bool PromprForServiceproviderChange;

        public ServiceProvider(int ServiceProviderIDIn)
        {
            InitializeComponent();
            PromprForServiceproviderChange = false;

            if (Program._GenericPrincipal.IsInRole("System Administrator"))
            {
                grpTariffs.Visible = true;
            }
            else
            {
                grpTariffs.Visible = false;
            }


            ServiceProviderID = ServiceProviderIDIn;

            comboCountry.DataSource = oShared.usp_Country_Select();
            comboCountry.DisplayMember = "CountryName";
            comboCountry.ValueMember = "CountryID";

            comboLanguage.DataSource = oShared.usp_Language_Select();
            comboLanguage.DisplayMember = "Language";
            comboLanguage.ValueMember = "LanguageID";

            comboSpeciality.DataSource = oShared.usp_Speciality_Select();
            comboSpeciality.DisplayMember = "Speciality";
            comboSpeciality.ValueMember = "SpecialityID";

            comboSelectBaseTariff.DataSource = oShared.usp_TariffStructure_Select();
            comboSelectBaseTariff.DisplayMember = "TariffStructureID";
            comboSelectBaseTariff.ValueMember = "TariffStructureID";

            //comboSelectBaseTariff.DataSource = oShared.usp_TariffName_Select();
            //comboSelectBaseTariff.DisplayMember = "TariffName";
            //comboSelectBaseTariff.ValueMember = "TariffNameID";

            if (ServiceProviderID != -1)
            {
                grpLinkedDocuments.Visible = true;

                grdLinkedDocuments.DataSource = oShared.usp_LinkedFile_SelectByEntityID_EntityType(ServiceProviderID, "ServiceProvider");

                DataTable oDt = oShared.usp_ServiceProvider_Select(ServiceProviderID);
                txtName.Text = oDt.Rows[0]["ServiceProviderName"].ToString();
                txtSurname.Text = oDt.Rows[0]["ServiceProviderSurname"].ToString();
                txtPracticeName.Text = oDt.Rows[0]["PracticeName"].ToString();
                txtPracticeGroupNumber.Text = oDt.Rows[0]["GroupPracticeNr"].ToString();
                txtProviderNumber.Text = oDt.Rows[0]["PracticeNr"].ToString();
                numNoOfPartners.Value = (oDt.Rows[0]["NoOfPartners"].ToString() == "" ? 0 : Convert.ToInt32(oDt.Rows[0]["NoOfPartners"].ToString()));
                txtServiceArea.Text = oDt.Rows[0]["ServiceArea"].ToString();
                comboSpeciality.SelectedValue = Convert.ToInt32(oDt.Rows[0]["SpecialityID"].ToString());
                chkIsHospital.Checked = (oDt.Rows[0]["IsHospital"].ToString() == "" ? false : Convert.ToBoolean(oDt.Rows[0]["IsHospital"].ToString()));
                txtAddress1.Text = oDt.Rows[0]["PracticeAddress1"].ToString();
                txtAddress2.Text = oDt.Rows[0]["PracticeAddress2"].ToString();
                txtAddress3.Text = oDt.Rows[0]["PracticeAddress3"].ToString();
                txtAddress4.Text = oDt.Rows[0]["PracticeAddress4"].ToString();
                txtAddressCode.Text = oDt.Rows[0]["PracticeAddressCode"].ToString();
                txtPostalAddress1.Text = oDt.Rows[0]["PracticePAddress1"].ToString();
                txtPostalAddress2.Text = oDt.Rows[0]["PracticePAddress2"].ToString();
                txtPostalAddress3.Text = oDt.Rows[0]["PracticePAddress3"].ToString();
                txtPostalAddress4.Text = oDt.Rows[0]["PracticePAddress4"].ToString();
                txtPostalAddressCode.Text = oDt.Rows[0]["PracticePAddressCode"].ToString();
                txtPhone.Text = oDt.Rows[0]["PhoneNumber"].ToString();
                txtFax.Text = oDt.Rows[0]["FaxNumber"].ToString();
                txtEmail.Text = oDt.Rows[0]["EmailAddress"].ToString();
                comboLanguage.SelectedValue = (oDt.Rows[0]["LanguageID"].ToString() == "" ? 1 : Convert.ToInt32(oDt.Rows[0]["LanguageID"].ToString())); //Convert.ToInt32(oDt.Rows[0]["LanguageID"].ToString());
                comboCountry.SelectedValue = (oDt.Rows[0]["CountryID"].ToString() == "" ? 2 : Convert.ToInt32(oDt.Rows[0]["CountryID"].ToString())); //Convert.ToInt32(oDt.Rows[0]["CountryID"].ToString());
                txtBankName.Text = oDt.Rows[0]["BankName"].ToString();
                txtBranchName.Text = oDt.Rows[0]["BankBranch"].ToString();
                txtBranchCode.Text = oDt.Rows[0]["BankBranchCode"].ToString();
                txtAccountType.Text = oDt.Rows[0]["BankAccountType"].ToString();
                txtAccountNumber.Text = oDt.Rows[0]["BankAccountNumber"].ToString();
                comboSelectBaseTariff.SelectedValue = oDt.Rows[0]["TariffStructureID"].ToString();
                chkVatIncl.Checked = Convert.ToBoolean(oDt.Rows[0]["TariffInclVAT"].ToString());
                chkVisible.Checked = Convert.ToBoolean(oDt.Rows[0]["Visible"].ToString());
                SPBaseTariff = 0;// = Convert.ToInt32(oDt.Rows[0]["TariffNameID"].ToString());
                txtCellNumber.Text = oDt.Rows[0]["CellNumber"].ToString();

            }
            else
            {
                comboSelectBaseTariff.SelectedValue = "RSA";
                chkVatIncl.Checked = true;
                chkVisible.Checked = true;
            }


            PromprForServiceproviderChange = true;
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            if (txtPracticeName.Text == "" 
                || txtProviderNumber.Text == "")
            {
                MessageBox.Show("The provider name and number fields are mandatory.\n\rThe entry will not be saved");
            }
            else if (!oShared.usp_ServiceProvider_Select_ExistingPracticeNr(txtProviderNumber.Text))
            {
                MessageBox.Show("The provider number already exists.\n\rThe entry will not be saved");
            }
            else if (ServiceProviderID == -1)
            {
                DataTable oDt =
                    oShared.usp_ServiceProvider_Insert(
                        txtName.Text
                        , txtSurname.Text
                        , ""
                        , txtPracticeName.Text
                        , txtPracticeGroupNumber.Text
                        , txtProviderNumber.Text
                        , Convert.ToInt32(numNoOfPartners.Value)
                        , txtServiceArea.Text
                        , Convert.ToInt32(comboSpeciality.SelectedValue)
                        , chkIsHospital.Checked
                        , txtAddress1.Text
                        , txtAddress2.Text
                        , txtAddress3.Text
                        , txtAddress4.Text
                        , txtAddressCode.Text
                        , txtPostalAddress1.Text
                        , txtPostalAddress2.Text
                        , txtPostalAddress3.Text
                        , txtPostalAddress4.Text
                        , txtPostalAddressCode.Text
                        , txtPhone.Text
                        , txtFax.Text
                        , txtEmail.Text
                        , Convert.ToInt32(comboLanguage.SelectedValue)
                        , Convert.ToInt32(comboCountry.SelectedValue)
                        , txtBankName.Text
                        , txtBranchName.Text
                        , txtBranchCode.Text
                        , txtAccountType.Text
                        , txtAccountNumber.Text
                        , Program.Username
                        , comboSelectBaseTariff.SelectedValue.ToString()
                        , chkVatIncl.Checked
                        , chkVisible.Checked
                        , txtCellNumber.Text);
                ServiceProviderID = Convert.ToInt32(oDt.Rows[0][0].ToString());
                oFinance.usp_ServiceProvider_MainClient_Discount_Insert(ServiceProviderID
                    , Convert.ToDecimal(numericDiscount.Value));
                
            }
            else
            {
                oShared.usp_ServiceProvider_Update(
                    ServiceProviderID
                    , txtName.Text
                    , txtSurname.Text
                    , ""
                    , txtPracticeName.Text
                    , txtPracticeGroupNumber.Text
                    , txtProviderNumber.Text
                    , Convert.ToInt32(numNoOfPartners.Value)
                    , txtServiceArea.Text
                    , Convert.ToInt32(comboSpeciality.SelectedValue)
                    , chkIsHospital.Checked
                    , txtAddress1.Text
                    , txtAddress2.Text
                    , txtAddress3.Text
                    , txtAddress4.Text
                    , txtAddressCode.Text
                    , txtPostalAddress1.Text
                    , txtPostalAddress2.Text
                    , txtPostalAddress3.Text
                    , txtPostalAddress4.Text
                    , txtPostalAddressCode.Text
                    , txtPhone.Text
                    , txtFax.Text
                    , txtEmail.Text
                    , Convert.ToInt32(comboLanguage.SelectedValue)
                    , Convert.ToInt32(comboCountry.SelectedValue)
                    , txtBankName.Text
                    , txtBranchName.Text
                    , txtBranchCode.Text
                    , txtAccountType.Text
                    , txtAccountNumber.Text
                    , Program.Username
                    , comboSelectBaseTariff.SelectedValue.ToString()
                    , chkVatIncl.Checked
                    , chkVisible.Checked
                    , txtCellNumber.Text);
                oFinance.usp_ServiceProvider_MainClient_Discount_Insert(ServiceProviderID, Convert.ToDecimal(numericDiscount.Value));
            }

            this.Close();
        }

        private void comboSelectBaseTariff_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ServiceProviderID > 0 && PromprForServiceproviderChange)
            //{
            //    try
            //    {
            //        DialogResult oResult = MessageBox.Show("Are you sure you want to change the base tariff for this provider?"
            //            , "Warning", MessageBoxButtons.YesNo);
            //        if (oResult == System.Windows.Forms.DialogResult.Yes)
            //        {
            //            oShared.usp_ServiceProvider_Tariff_Update(ServiceProviderID, Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString()));
            //        }
            //        else comboSelectBaseTariff.SelectedValue = SPBaseTariff;
            //    }
            //    catch (Exception err)
            //    {
            //        MessageBox.Show(err.Message.ToString());
            //    }
            //}
            

        }

        private void btnCustomiseTariff_Click(object sender, EventArgs e)
        {
            CustomiseServiceProviderTariff oCustomise = new CustomiseServiceProviderTariff(ServiceProviderID, SPBaseTariff, Convert.ToInt32(comboSpeciality.SelectedValue));
            oCustomise.ShowDialog();
        }

        private void grdLinkedDocuments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = grdLinkedDocuments.CurrentRow.Cells["FilePath"].Value.ToString();
                //psi.Arguments = oDt.Rows[0]["UpdatePath"].ToString();
                //if you don't want a console window popping up then set this property
                //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                //Create new process and set the starting information
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = psi;

                //Set this so that you can tell when the process has completed
                //p.EnableRaisingEvents = true;

                p.Start();

            }
        }

        private void picLinkedDocumentsAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.CheckFileExists = true;
            oDialog.ShowDialog();

            if (oDialog.FileName != "")
            {
                oShared.usp_LinkedFile_Insert(ServiceProviderID, "ServiceProvider", oDialog.FileName, oDialog.SafeFileName, Program.Username);
            }
            grdLinkedDocuments.DataSource = oShared.usp_LinkedFile_SelectByEntityID_EntityType(ServiceProviderID, "ServiceProvider");
        }

        private void picOpenFile_Click(object sender, EventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = grdLinkedDocuments.CurrentRow.Cells["FilePath"].Value.ToString();
                //psi.Arguments = oDt.Rows[0]["UpdatePath"].ToString();
                //if you don't want a console window popping up then set this property
                //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                //Create new process and set the starting information
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = psi;

                //Set this so that you can tell when the process has completed
                //p.EnableRaisingEvents = true;

                p.Start();

            }
        }

        private void picLinkedDocumentsDelete_Click(object sender, EventArgs e)
        {
            if (grdLinkedDocuments.CurrentRow.Index >= 0)
            {
                oShared.usp_LinkedFile_DeleteByLinkedFileID(Convert.ToInt32(grdLinkedDocuments.CurrentRow.Cells["LinkedFileID"].Value));
            }
            grdLinkedDocuments.DataSource = oShared.usp_LinkedFile_SelectByEntityID_EntityType(ServiceProviderID, "ServiceProvider");
        }

    }
}

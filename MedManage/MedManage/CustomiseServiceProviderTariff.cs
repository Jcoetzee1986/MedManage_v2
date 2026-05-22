using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;
using System.IO;
using OfficeOpenXml;

namespace Icondev.MedManage
{
    public partial class CustomiseServiceProviderTariff : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        int ServiceProviderId;
        int TariffNameID; 
        int SpecialityID;
        string SelectedCode { get; set; }
        public DataGridViewRow oDr { get; private set; }
        bool updateControls;

        public CustomiseServiceProviderTariff(int serviceProviderId, int tariffNameID,int specialityID)
        {
            updateControls = false;

            SpecialityID = specialityID;
            ServiceProviderId = serviceProviderId;
            TariffNameID = tariffNameID;

            InitializeComponent();
           
            dateTariffEffectiveDate.Value = new DateTime(DateTime.Today.Year, 01, 01);

            this.Speciality.DataSource = oShared.usp_Speciality_Select();
            this.Speciality.ValueMember = "SpecialityID";
            this.Speciality.DisplayMember = "Speciality";
            this.Speciality.ReadOnly = true;

            //comboTariffPeriod.DataSource = oShared.usp_Tariff_SelectDistinctPeriods();
            //comboTariffPeriod.ValueMember = "TariffPeriodName";
            //comboTariffPeriod.DisplayMember = "TariffPeriodName";

            comboSelectClient.DataSource = oShared.usp_Tariff_SelectClients(ServiceProviderId, dateTariffEffectiveDate.Value);
            comboSelectClient.ValueMember = "MainClientID";
            comboSelectClient.DisplayMember = "MainClientName";

            //Check if service provider uses same tariff for all clients
            bool SameValues = true;
            for (int i = 1; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
            {
                if (((DataTable)comboSelectClient.DataSource).Rows[i - 1]["TariffNameID"].ToString() != ((DataTable)comboSelectClient.DataSource).Rows[i]["TariffNameID"].ToString())
                {
                    SameValues = false;
                }
            }
            chkAllClients.Checked = SameValues;
            comboSelectClient.Enabled = !chkAllClients.Checked;

            comboHospitalType.SelectedValue = SpecialityID;
            comboHospitalType.DataSource = oShared.usp_Speciality_Select();
            comboHospitalType.ValueMember = "SpecialityID";
            comboHospitalType.DisplayMember = "Speciality";
            updateControls = false;
            comboHospitalType.SelectedValue = SpecialityID;

            comboSelectBaseTariff.DataSource = oShared.usp_TariffName_Select();
            comboSelectBaseTariff.DisplayMember = "TariffName";
            comboSelectBaseTariff.ValueMember = "TariffNameID";
            updateControls = false;
            comboSelectBaseTariff.SelectedValue = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["TariffNameID"].ToString();

            txtPercentageAdded.Text = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["PercentageAdded"].ToString();

            updateControls = true;

            BindDatagridData();

            if (Program._GenericPrincipal.IsInRole("System Administrator"))
            {
                txtTariffAmount.Visible = true;
                txtTariffCode.Visible = true;
                btnSaveSpecificTariffAmount.Visible = true;
                lblTariffAmount.Visible = true;
                lblTariffCode.Visible = true;
            }
            else
            {
                txtTariffAmount.Visible = false;
                txtTariffCode.Visible = false;
                btnSaveSpecificTariffAmount.Visible = false;
                lblTariffAmount.Visible = false;
                lblTariffCode.Visible = false;
            }

        }

        private void comboHospitalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatagridData();
        }

        private void BindDatagridData()
        {
            try
            {
                if (updateControls)
                    try
                    {
                        if (comboHospitalType.SelectedValue != null)
                        {
                            if (comboHospitalType.SelectedValue.ToString() != "System.Data.DataRowView")
                            {
                                datagridSearchResults.DataSource = oShared.usp_ServiceProvider_Tariff_Select_ById(ServiceProviderId, Convert.ToInt32(comboHospitalType.SelectedValue), dateTariffEffectiveDate.Value, Convert.ToInt32(comboSelectClient.SelectedValue.ToString()));
                                int LastIndex = datagridSearchResults.Rows.Count - 1;
                                datagridSearchResults.Rows[LastIndex].ReadOnly = true;
                            }
                        }

                    }
                    catch (Exception)
                    {

                        //throw;
                    }
               
            }
            catch (Exception)
            {

                //throw;
            }
        }

       
        private void comboTariffPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (updateControls)
            //{
            //    updateControls = false;
            //    comboSelectClient.DataSource = oShared.usp_Tariff_SelectClients(ServiceProviderId, Convert.ToInt32(comboTariffPeriod.SelectedValue));
            //    comboSelectClient.ValueMember = "MainClientID";
            //    comboSelectClient.DisplayMember = "MainClientName";

            //    //Check if service provider uses same tariff for all clients
            //    bool SameValues = true;
            //    for (int i = 1; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
            //    {
            //        if (((DataTable)comboSelectClient.DataSource).Rows[i - 1]["TariffNameID"].ToString() != ((DataTable)comboSelectClient.DataSource).Rows[i]["TariffNameID"].ToString())
            //        {
            //            SameValues = false;
            //        }
            //    }
            //    chkAllClients.Checked = SameValues;
            //    comboSelectClient.Enabled = !chkAllClients.Checked;

            //    comboHospitalType.SelectedValue = SpecialityID;
            //    comboHospitalType.DataSource = oShared.usp_Speciality_Select();
            //    comboHospitalType.ValueMember = "SpecialityID";
            //    comboHospitalType.DisplayMember = "Speciality";
            //    updateControls = false;
            //    comboHospitalType.SelectedValue = SpecialityID;

            //    comboSelectBaseTariff.DataSource = oShared.usp_TariffName_Select();
            //    comboSelectBaseTariff.DisplayMember = "TariffName";
            //    comboSelectBaseTariff.ValueMember = "TariffNameID";
            //    updateControls = false;
            //    comboSelectBaseTariff.SelectedValue = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["TariffNameID"].ToString();// TariffNameID;

            //    updateControls = true;

            //    BindDatagridData();
            //}
        }

        private void chkAllClients_CheckedChanged(object sender, EventArgs e)
        {
            comboSelectClient.Enabled = !chkAllClients.Checked;
        }

        private void comboSelectClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateControls = false;
            comboSelectBaseTariff.SelectedValue = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["TariffNameID"].ToString();
            updateControls = true;
            txtPercentageAdded.Text = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["PercentageAdded"].ToString();

            BindDatagridData();
        } 
        
        private void datagridSearchResults_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (chkAllClients.Checked)
                {
                    for (int i = 0; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
                    {
                        oShared.usp_ServiceProvider_Tariff_Custom_Insert(ServiceProviderId
                            , datagridSearchResults.Rows[e.RowIndex].Cells["BaseTariffID"].Value.ToString()
                            , Convert.ToDecimal(datagridSearchResults.Rows[e.RowIndex].Cells["ProviderRate"].Value)
                            , Convert.ToInt32(((DataTable)comboSelectClient.DataSource).Rows[i]["MainClientID"].ToString())
                            , dateTariffEffectiveDate.Value);
                    }
                }
                else
                {
                    oShared.usp_ServiceProvider_Tariff_Custom_Insert(ServiceProviderId
                            , datagridSearchResults.Rows[e.RowIndex].Cells["BaseTariffID"].Value.ToString()
                            , Convert.ToDecimal(datagridSearchResults.Rows[e.RowIndex].Cells["ProviderRate"].Value)
                            ,  Convert.ToInt32(comboSelectClient.SelectedValue.ToString())
                            , dateTariffEffectiveDate.Value); 
                   
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void btnImportFromFile_Click(object sender, EventArgs e)
        {
            try
            {


                OpenFileDialog oFile = new OpenFileDialog();
                oFile.ShowDialog();
                if (oFile.CheckFileExists)
                {
                    if (oFile.FileName.EndsWith(".xlsx"))
                    {
                        FileInfo newFile = new FileInfo(oFile.FileName);
                        ExcelPackage oExcel = new ExcelPackage(newFile);
                        foreach (ExcelWorksheet oSheet in oExcel.Workbook.Worksheets)
                        {
                            if (oSheet.Cell(1, 1).Value == "Speciality")
                            {
                                int Counter = 2;//Start at first data record

                                while (Counter != -1)
                                {
                                    if (oSheet.Cell(Counter, 2).Value.ToString() != "")//While the code is populated
                                    {
                                        try
                                        {
                                            if (Convert.ToDecimal(oSheet.Cell(Counter, 4).Value.Trim()) / (Decimal)1.14 > 0)
                                            {
                                                if (chkAllClients.Checked)
                                                {
                                                    for (int i = 0; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
                                                    {
                                                        oShared.usp_ServiceProvider_Tariff_Custom_Insert_FromExcel(ServiceProviderId
                                                        , oSheet.Cell(Counter, 2).Value.Trim()//TariffCode
                                                        , Convert.ToDecimal(oSheet.Cell(Counter, 4).Value.Trim())
                                                        , Convert.ToInt32(((DataTable)comboSelectClient.DataSource).Rows[i]["MainClientID"].ToString())
                                                        , dateTariffEffectiveDate.Value
                                                        , Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString())
                                                        , Convert.ToInt32((oSheet.Cell(Counter, 1).Value.Trim() == "" ? "-1" : oSheet.Cell(Counter, 1).Value.Trim()))
                                                        , oSheet.Cell(Counter, 3).Value.Trim()//Description
                                                        , Program.Username
                                                        );
                                                    }
                                                }
                                                else
                                                {
                                                    oShared.usp_ServiceProvider_Tariff_Custom_Insert_FromExcel(ServiceProviderId
                                                        , oSheet.Cell(Counter, 2).Value.Trim()//TariffCode
                                                        , Convert.ToDecimal(oSheet.Cell(Counter, 4).Value.Trim())
                                                        , Convert.ToInt32(comboSelectClient.SelectedValue.ToString())
                                                        , dateTariffEffectiveDate.Value
                                                        , Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString())
                                                        , Convert.ToInt32((oSheet.Cell(Counter, 1).Value.Trim() == "" ? "-1" : oSheet.Cell(Counter, 1).Value.Trim()))
                                                        , oSheet.Cell(Counter, 3).Value.Trim()//Description
                                                        , Program.Username
                                                        );
                                                }
                                            }
                                        }
                                        catch (Exception err)
                                        {
                                            if (Program.DevMode == true)
                                            {
                                                MessageBox.Show(err.Message.ToString());
                                            }
                                        }
                                        Counter++;
                                    }
                                    else
                                    {
                                        MessageBox.Show("File imported up to row " + Counter.ToString());
                                        Counter = -1;
                                        BindDatagridData();
                                        oExcel.Dispose();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The file has to be in the new excel version (.xlsx)");
                        
                    }
                }
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
               
            }
        }

        private void btnSaveSpecificTariffAmount_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable oDt = new DataTable();
                if (chkAllClients.Checked)
                {
                    oDt = oShared.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate(
                    txtTariffCode.Text
                    , ServiceProviderId
                    , dateTariffEffectiveDate.Value
                    , -1);
                }
                else
                {
                    oDt = oShared.usp_Tariff_Select_ByTariffCode_ProviderID_TreatmentDate(
                    txtTariffCode.Text
                    , ServiceProviderId
                    , dateTariffEffectiveDate.Value
                    , Program.MainClientID);
                }

                for (int i = 0; i < oDt.Rows.Count; i++)
                {
                    try
                    {
                        if (txtTariffCode.Text == oDt.Rows[i]["Code"].ToString())
                        {
                            oShared.usp_ServiceProvider_Tariff_Custom_Insert(ServiceProviderId
                                , oDt.Rows[i]["BaseTariffID"].ToString()
                                , Convert.ToDecimal(txtTariffAmount.Text)
                                , Convert.ToInt32(oDt.Rows[i]["MainClientID"].ToString())
                                , dateTariffEffectiveDate.Value);
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message.ToString());
                    }
                    
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
            BindDatagridData();
        }

        private void btnSavePercentage_Click(object sender, EventArgs e)
        {
            if (updateControls)
            {
                try
                {
                    //Update does not work
                    DialogResult oResult = MessageBox.Show("Are you sure you want to change the base tariff for this provider?"
                        + "\n\rThis change will not update tariffs that were previously captured."
                        , "Warning", MessageBoxButtons.YesNo);
                    if (oResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (chkAllClients.Checked)
                        {
                            for (int i = 0; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
                            {
                                oShared.usp_ServiceProvider_Tariff_Update(ServiceProviderId
                                    , Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString())
                                    , Convert.ToInt32(((DataTable)comboSelectClient.DataSource).Rows[i]["MainClientID"].ToString())
                                    , dateTariffEffectiveDate.Value
                                    , Convert.ToDecimal((txtPercentageAdded.Text == "" ? "0" : txtPercentageAdded.Text)));
                                TariffNameID = Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString());
                            }
                            BindDatagridData();
                        }
                        else
                        {
                            oShared.usp_ServiceProvider_Tariff_Update(ServiceProviderId
                                , Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString())
                                , Convert.ToInt32(comboSelectClient.SelectedValue.ToString())
                                , dateTariffEffectiveDate.Value
                                , Convert.ToDecimal((txtPercentageAdded.Text == "" ? "0" : txtPercentageAdded.Text)));
                            TariffNameID = Convert.ToInt32(comboSelectBaseTariff.SelectedValue.ToString());
                            BindDatagridData();
                        }
                    }
                    else comboSelectBaseTariff.SelectedValue = TariffNameID;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message.ToString());
                }

            }
        }

        private void dateTariffEffectiveDate_ValueChanged(object sender, EventArgs e)
        {
            if (updateControls)
            {
                updateControls = false;
                comboSelectClient.DataSource = oShared.usp_Tariff_SelectClients(ServiceProviderId, dateTariffEffectiveDate.Value);
                comboSelectClient.ValueMember = "MainClientID";
                comboSelectClient.DisplayMember = "MainClientName";

                //Check if service provider uses same tariff for all clients
                bool SameValues = true;
                for (int i = 1; i < ((DataTable)comboSelectClient.DataSource).Rows.Count; i++)
                {
                    if (((DataTable)comboSelectClient.DataSource).Rows[i - 1]["TariffNameID"].ToString() != ((DataTable)comboSelectClient.DataSource).Rows[i]["TariffNameID"].ToString())
                    {
                        SameValues = false;
                    }
                }
                chkAllClients.Checked = SameValues;
                comboSelectClient.Enabled = !chkAllClients.Checked;

                comboHospitalType.SelectedValue = SpecialityID;
                comboHospitalType.DataSource = oShared.usp_Speciality_Select();
                comboHospitalType.ValueMember = "SpecialityID";
                comboHospitalType.DisplayMember = "Speciality";
                updateControls = false;
                comboHospitalType.SelectedValue = SpecialityID;

                comboSelectBaseTariff.DataSource = oShared.usp_TariffName_Select();
                comboSelectBaseTariff.DisplayMember = "TariffName";
                comboSelectBaseTariff.ValueMember = "TariffNameID";
                updateControls = false;
                comboSelectBaseTariff.SelectedValue = ((DataTable)comboSelectClient.DataSource).Rows[comboSelectClient.SelectedIndex]["TariffNameID"].ToString();// TariffNameID;

                updateControls = true;

                BindDatagridData();
            }
        }

        private void comboSelectBaseTariff_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPercentageAdded.Text = "0";
        }
    }
}

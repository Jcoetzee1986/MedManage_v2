using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib.Shared;

namespace Icondev.MedManage
{
    public partial class ServiceProviderLookup : Form
    {
        private bool OpenFromCase;
        SharedObjects oShared = new SharedObjects(Program.oDb);
        public string oReturn { get; set; }
        public int ServiceProviderID { get; set; }

        public ServiceProviderLookup(bool openFromCase, string refPracticeName, string refSurname, string refName)
        {
            InitializeComponent();
            OpenFromCase = openFromCase;
            if(!OpenFromCase)
            {
                picSelectServiceProvider.Visible = false;
            }
            
            if (refPracticeName != "")
            {
                txtName.Text = refName;
                txtPracticeName.Text = refPracticeName;
                txtSurname.Text = refSurname;
                datagridServiceProviders.DataSource = oShared.usp_ServiceProvider_Select_ByFilters(txtName.Text
                    , txtSurname.Text
                    , txtPracticeName.Text
                    , txtProviderNumber.Text
                    , !Program._GenericPrincipal.IsInRole("System Administrator"));
            }
        }

        private void picServiceProviderLookup_Click(object sender, EventArgs e)
        {
            datagridServiceProviders.DataSource = oShared.usp_ServiceProvider_Select_ByFilters(txtName.Text
                    , txtSurname.Text
                    , txtPracticeName.Text
                    , txtProviderNumber.Text
                    , !Program._GenericPrincipal.IsInRole("System Administrator"));
        }

        private void picAddServiceProvider_Click(object sender, EventArgs e)
        {
            oReturn = "New";
            if (OpenFromCase)
                this.Close();
            else
            {
                ServiceProvider oServiceProvider = new ServiceProvider(-1);
                oServiceProvider.ShowDialog();
            }
        }

        private void picSelectServiceProvider_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceProviderID = Convert.ToInt32(datagridServiceProviders.Rows[datagridServiceProviders.CurrentRow.Index].Cells["ServiceProviderID"].Value);
                oReturn = "ServiceProvider";
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No provider selected");
            }
            
        }

        private void datagridServiceProviders_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                ServiceProvider oServiceProvider = new ServiceProvider(Convert.ToInt32(datagridServiceProviders.Rows[e.RowIndex].Cells["ServiceProviderID"].Value));
                oServiceProvider.ShowDialog();
            }
        }

        private void picCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picOpen_Click(object sender, EventArgs e)
        {
            if (datagridServiceProviders.CurrentRow != null)
            {
                ServiceProvider oServiceProvider = new ServiceProvider(Convert.ToInt32(datagridServiceProviders.Rows[datagridServiceProviders.CurrentRow.Index].Cells["ServiceProviderID"].Value));
                oServiceProvider.ShowDialog();
            }
            else MessageBox.Show("Please select a service provider first");
        }

        private void Filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                datagridServiceProviders.DataSource = oShared.usp_ServiceProvider_Select_ByFilters(txtName.Text
                    , txtSurname.Text
                    , txtPracticeName.Text
                    , txtProviderNumber.Text
                    , !Program._GenericPrincipal.IsInRole("System Administrator"));
                datagridServiceProviders.Focus();
            }
        }

        private void datagridServiceProviders_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (datagridServiceProviders.CurrentRow != null)
                {
                    try
                    {
                        if (datagridServiceProviders.CurrentRow.Index == 0)
                        {
                            ServiceProviderID = Convert.ToInt32(datagridServiceProviders.Rows[datagridServiceProviders.CurrentRow.Index].Cells["ServiceProviderID"].Value);
                            oReturn = "ServiceProvider";
                            this.Close();
                        }
                        else
                        {
                            ServiceProviderID = Convert.ToInt32(datagridServiceProviders.Rows[datagridServiceProviders.CurrentRow.Index - 1].Cells["ServiceProviderID"].Value);
                            oReturn = "ServiceProvider";
                            this.Close();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please select a service provider first");
                    }
                }
                else MessageBox.Show("Please select a service provider first");
            }
        }
    }
}

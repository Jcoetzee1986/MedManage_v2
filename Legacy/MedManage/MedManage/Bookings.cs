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
    public partial class Bookings : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);

        public Bookings()
        {
            InitializeComponent();
        }

        private void Bookings_Load(object sender, EventArgs e)
        {
            dateTravelDateTo.Value = DateTime.Today.AddDays(14);
            dateTravelDateFrom.Value = DateTime.Today.AddDays(-7);

            grdBookings.DataSource = oShared.usp_Bookings_Select_ByFilters(
                txtSurname.Text
                , txtName.Text
                , txtMemberNumber.Text
                , dateTravelDateFrom.Value
                , dateTravelDateTo.Value);

        }

        private void picOpen_Click(object sender, EventArgs e)
        {
            if (grdBookings.CurrentRow.Index > -1)
            {
                BookingAdd oBookingAdd = new BookingAdd(
                    Convert.ToInt32(grdBookings.Rows[grdBookings.CurrentRow.Index].Cells["BookingID"].Value)
                    , -1
                    ,-1);
                oBookingAdd.ShowDialog();
            }
        }

        private void picLookup_Click(object sender, EventArgs e)
        {
            grdBookings.DataSource = oShared.usp_Bookings_Select_ByFilters(
                txtSurname.Text
                , txtName.Text
                , txtMemberNumber.Text
                , dateTravelDateFrom.Value
                , dateTravelDateTo.Value);
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picAdd_Click(object sender, EventArgs e)
        {
            BookingAdd oBookingAdd = new BookingAdd(-1
                    , -1
                    , -1);
            oBookingAdd.ShowDialog();
        }
    }
}

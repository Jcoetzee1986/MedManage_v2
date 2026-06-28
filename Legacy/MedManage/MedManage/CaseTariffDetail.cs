using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class CaseTariffDetail : Form
    {
        CaseManagement oCM = new CaseManagement(Program.oDb);
        Finance oFin = new Finance(Program.oDb);
        int CaseID;
        public CaseTariffDetail(int caseID)
        {
            InitializeComponent();
            CaseID = caseID;
            datagridTarrifs.DataSource = oCM.usp_Case_Tariff_Select(CaseID);
        }

        private void datagridTarrifs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (datagridTarrifs.RowCount > 0)
            {
                numericDiscount.Text = datagridTarrifs.Rows[0].Cells["Discount"].Value.ToString();
                PopulateValues();
            }
        }

        private void numericDiscount_ValueChanged(object sender, EventArgs e)
        {
            oFin.usp_Case_Discount_Update(CaseID, Convert.ToDecimal(numericDiscount.Value));
            PopulateValues();
        }

        private void PopulateValues()
        {
            if (datagridTarrifs.RowCount > 0)
            {
                txtTarrifTotal.Text = "0";
                txtOvercharged.Text = "0";
                txtPayable.Text = "0";
                txtDiscount.Text = "0";

                for (int i = 0; i < datagridTarrifs.Rows.Count; i++)
                {
                    //Overcharged = 0;
                    if (Convert.ToBoolean(datagridTarrifs.Rows[i].Cells["ValueIsTotal"].Value))
                    {
                        txtTarrifTotal.Text = Convert.ToString(Convert.ToDecimal(txtTarrifTotal.Text)
                        + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Value"].Value));
                    }
                    else
                    {
                        txtTarrifTotal.Text = Convert.ToString(Convert.ToDecimal(txtTarrifTotal.Text)
                        + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Value"].Value)
                        * Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["Qty"].Value));
                    }

                    txtOvercharged.Text = Convert.ToString(Convert.ToDecimal(txtOvercharged.Text)
                        + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["TotalOvercharged"].Value));

                    txtPayable.Text = Convert.ToString(Convert.ToDecimal(txtPayable.Text)
                        + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["TotalPayable"].Value));

                    txtDiscount.Text = Convert.ToString(Convert.ToDecimal(txtDiscount.Text)
                        + Convert.ToDecimal(datagridTarrifs.Rows[i].Cells["DiscountValue"].Value));

                    DataGridViewCellStyle oStyle = new DataGridViewCellStyle();
                    oStyle.BackColor = System.Drawing.Color.FromName(datagridTarrifs.Rows[i].Cells["Colour"].Value.ToString());
                    datagridTarrifs.Rows[i].DefaultCellStyle = oStyle;
                }
            }
        }

        private void picReportCaseTariffDetail_Click(object sender, EventArgs e)
        {
            RPT_CaseTariffDetail oReport = new RPT_CaseTariffDetail(CaseID);
            oReport.MdiParent = this.MdiParent;
            oReport.Show();
        }
    }
}

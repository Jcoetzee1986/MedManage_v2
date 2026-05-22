using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
using Icondev.MedManage.MedManageLib.Shared;
using Icondev.MedManage.MedManageLib;

namespace Icondev.MedManage
{
    public partial class SystemData : Form
    {
        SharedObjects oShared = new SharedObjects(Program.oDb);
        byte[] oImageBytes;

        public SystemData()
        {
            InitializeComponent();
            comboCountry.DisplayMember = "CountryName";
            comboCountry.ValueMember = "CountryID";
            comboCountry.DataSource = oShared.usp_Country_Select();

            DataTable oDt = oShared.usp_SystemData_Select(1);
            try
            {
                oImageBytes = (byte[])oDt.Rows[0]["Logo"];
                System.Drawing.Image img;

                MemoryStream strm = new MemoryStream();
                strm.Write(oImageBytes, 0, oImageBytes.Length);
                strm.Position = 0;

                img = System.Drawing.Image.FromStream(strm);

                picLogo.Image = img;
            }
            catch
            {
                //Not a valid Image, Load nothing
            }

             comboCountry.SelectedValue = oDt.Rows[0]["SystemCountryID"].ToString();
             txtSystemEmailAddress.Text = oDt.Rows[0]["SystemEmailAddress"].ToString();
             txtSMTPServer.Text = oDt.Rows[0]["SMTPServer"].ToString();
             chkSSL.Checked = Convert.ToBoolean(oDt.Rows[0]["SSL"].ToString());
             txtUserName.Text = oDt.Rows[0]["Username"].ToString();
             txtPassword.Text = oDt.Rows[0]["Password"].ToString();
             txtSpecialICU.Text = oDt.Rows[0]["SpecialICU"].ToString();
             txtICU.Text = oDt.Rows[0]["ICU"].ToString();
             txtHighCare.Text = oDt.Rows[0]["HighCare"].ToString();
             txtNeuroWard.Text = oDt.Rows[0]["NeuroWard"].ToString();
             txtIsolation.Text = oDt.Rows[0]["InIsolation"].ToString();
             txtGeneralWard.Text = oDt.Rows[0]["GeneralWard"].ToString();
             txtPaediatric.Text = oDt.Rows[0]["Paediatric"].ToString();
             txtMaternity.Text = oDt.Rows[0]["Maternity"].ToString();
             txtDayCase.Text = oDt.Rows[0]["DayCase"].ToString();
             txtStepDown.Text = oDt.Rows[0]["StepDown"].ToString();
             txtPsychiatric.Text = oDt.Rows[0]["Psychiatric"].ToString();
             txtAddress1.Text = oDt.Rows[0]["Address1"].ToString();
             txtAddress2.Text = oDt.Rows[0]["Address2"].ToString();
             txtAddress3.Text = oDt.Rows[0]["Address3"].ToString();
             txtAddress4.Text = oDt.Rows[0]["Address4"].ToString();
             tstAddressCode.Text = oDt.Rows[0]["AddressCode"].ToString();
             txtEmail.Text = oDt.Rows[0]["Email"].ToString();
             txtFax.Text = oDt.Rows[0]["Fax"].ToString();
             txtWebsite.Text = oDt.Rows[0]["Website"].ToString();
             txtTelephone.Text = oDt.Rows[0]["Telephone"].ToString();
             txtDefaultProviderID.Text = oDt.Rows[0]["DefaultProviderID"].ToString();

        }

        private void picLogoLookup_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog();
            DialogResult oResult = oDialog.ShowDialog();
            if (oDialog.FileName != "")
            {
                string ext = oDialog.FileName.Substring(oDialog.FileName.LastIndexOf('.'));
                string path = Application.StartupPath + "\\Images\\";
                string finalPath = path + "LogoImage" + ext;

                Images oImage = new Images(Program.oDb);
                oImage.resizeImage(oDialog.FileName, 250, finalPath);

                oImageBytes = File.ReadAllBytes(finalPath);

                System.Drawing.Image img;

                MemoryStream strm = new MemoryStream();
                strm.Write(oImageBytes, 0, oImageBytes.Length);
                strm.Position = 0;

                img = System.Drawing.Image.FromStream(strm);

                picLogo.Image = img;
            }
        }

        private void picSave_Click(object sender, EventArgs e)
        {
            oShared.usp_SystemData_Update(
                1
                , Convert.ToInt32(comboCountry.SelectedValue)
                , Guid.NewGuid()
                , txtSystemEmailAddress.Text
                , txtSMTPServer.Text
                , chkSSL.Checked
                , txtUserName.Text
                , txtPassword.Text
                , Program.Username
                , Convert.ToInt32(txtSpecialICU.Text)
                , Convert.ToInt32(txtICU.Text)
                , Convert.ToInt32(txtHighCare.Text)
                , Convert.ToInt32(txtNeuroWard.Text)
                , Convert.ToInt32(txtIsolation.Text)
                , Convert.ToInt32(txtGeneralWard.Text)
                , Convert.ToInt32(txtPaediatric.Text)
                , Convert.ToInt32(txtMaternity.Text)
                , Convert.ToInt32(txtDayCase.Text)
                , Convert.ToInt32(txtStepDown.Text)
                , Convert.ToInt32(txtPsychiatric.Text)
                , txtAddress1.Text
                , txtAddress2.Text
                , txtAddress3.Text
                , txtAddress4.Text
                , tstAddressCode.Text
                , txtEmail.Text
                , txtFax.Text
                , txtWebsite.Text
                , txtTelephone.Text
                , oImageBytes
                , Convert.ToInt32(txtDefaultProviderID.Text)
                );
            MessageBox.Show("Saved Successfully");
        }

        //private void GetImage()
        //{
            
        //    OpenFileDialog oDialog = new OpenFileDialog();

        //    string ext = oDialog.FileName.Substring(oDialog.FileName.LastIndexOf('.'));
        //    string path = Application.StartupPath + "Images\\";
        //    string finalPath = path + "LogoImage" + ext;

        //    Images oImage = new Images();
        //    oImage.resizeImage(oDialog.FileName, 250, finalPath);

        //    oImageBytes = File.ReadAllBytes(finalPath);

        //    try
        //    {
        //        StackPanel imgStackPnl = new StackPanel();
        //        imgStackPnl.Orientation = Orientation.Horizontal;
        //        if (oDs.Tables[1].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < oDs.Tables[1].Rows.Count; i++)
        //            {
        //                Image curImage = new Image();
        //                //curImage.Source = (ImageSource)"Binding";
        //                byte[] obytes = (byte[])oDs.Tables[1].Rows[i]["AssesmentImage"];
        //                //string oFolder = Application.ResourceAssembly.Location.ToString().Replace("EscudoAuditCapture.exe", "") + "Images\\tmpImg" + oAssesDetail.AssesmentDetailKey.ToString() + "_" + oDs.Tables[4].Rows[i]["AssesmentImageKey"].ToString() + ".jpg";

        //                try
        //                {
        //                    curImage.MouseDown += new MouseButtonEventHandler(CurImage_MouseDown);
        //                    curImage.MouseUp += new MouseButtonEventHandler(CurImage_MouseUp);

        //                    //File.WriteAllBytes(oFolder, obytes);
        //                    BitmapImage bm = new BitmapImage();
        //                    //System.Drawing.Icon ic;
        //                    System.Drawing.Image img;

        //                    MemoryStream strm = new MemoryStream();
        //                    strm.Write(obytes, 0, obytes.Length);
        //                    strm.Position = 0;

        //                    img = System.Drawing.Image.FromStream(strm);
        //                    System.Drawing.Bitmap b = (System.Drawing.Bitmap)img;
        //                    //ic = System.Drawing.Icon.FromHandle(b.GetHicon());
        //                    //string imagePath = "Icons/go-down-8.ico";
        //                    //System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath);

        //                    //BitmapImage bi = new BitmapImage();
        //                    bm.BeginInit();
        //                    bm.DecodePixelWidth = 128;
        //                    //MemoryStream ms = new MemoryStream();
        //                    strm.Seek(0, SeekOrigin.Begin);
        //                    bm.StreamSource = strm;
        //                    bm.EndInit();

        //                    curImage.Source = bm;
        //                    curImage.Tag = oDs.Tables[1].Rows[i]["AssesmentImageKey"].ToString();
        //                    imgStackPnl.Children.Add(curImage);

        //                }
        //                catch
        //                {
        //                }
        //                //imgFaultImage.ImageUrl = "~/Images/tmpImg" + oAssesDetail.AssesmentDetailKey.ToString() + ".jpg";
        //            }
        //        }
        //}



    }
}

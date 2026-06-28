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
using System.IO;

namespace Icondev.MedManage
{
    public partial class Gallery : Form
    {
        //SharedObjects oShared = new SharedObjects(Program.oDb);
        //CaseManagement oCM = new CaseManagement(Program.oDb);
        Images oImage = new Images(Program.oDb);
        //TariffCodeLookup oCodeTariff;
        //CodeLookup oCodeICD;
        //CodeLookup oCodeCPT;
        int CaseID;
        byte[] oImageBytesNew;

        public Gallery(int caseID)
        {
            CaseID = caseID;
            InitializeComponent();
            BindTreeView();
        }
        public void BindTreeView()
        {
            DataTable oDt = oImage.usp_Images_CaseUpdate_SelectByCaseID(CaseID).Tables[0];
            int LastCat = -1;
            int LastDate = -1;
            treeGallery.Nodes.Clear();
            for (int i = 0; i < oDt.Rows.Count; i++)
            {

                if (LastCat == -1
                    || oDt.Rows[LastCat]["ImageCategory"].ToString() != oDt.Rows[i]["ImageCategory"].ToString())
                {
                    treeGallery.Nodes.Add(i.ToString(), oDt.Rows[i]["ImageCategory"].ToString());
                    LastCat = i;
                }
                if (LastDate == -1
                    || oDt.Rows[LastDate]["DateAdded"].ToString() != oDt.Rows[i]["DateAdded"].ToString()
                    || LastCat == i)
                {
                    //treeGallery.Nodes.Add(i.ToString(), oDt.Rows[i]["DateAdded"].ToString());
                    treeGallery.Nodes[LastCat].Nodes.Add(i.ToString(), oDt.Rows[i]["DateAdded"].ToString().Substring(0,10));
                    LastDate = i;
                }
                treeGallery.Nodes[LastCat].Nodes[LastDate].Nodes.Add(i.ToString(), oDt.Rows[i]["Name"].ToString());
                treeGallery.Nodes[LastCat].Nodes[LastDate].Nodes[i.ToString()].Tag = oDt.Rows[i]["ImageID"].ToString();
            }
            treeGallery.ExpandAll();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            pnlAddImage.Visible = true;
        }

        private void btnRemoveImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeGallery.SelectedNode.Tag.ToString() != "")
                {
                    oImage.usp_Images_CaseUpdate_Delete(Convert.ToInt32(treeGallery.SelectedNode.Tag));
                }
                BindTreeView();
            }
            catch { };
        }

        private void treeGallery_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeGallery.SelectedNode.Tag.ToString() != "")
                {
                    byte[] oImageBytes;
                    DataTable oDt = oImage.usp_Images_CaseUpdate_SelectImageByImageID(Convert.ToInt32(treeGallery.SelectedNode.Tag));
                    oImageBytes = (byte[])oDt.Rows[0]["Image"];
                    System.Drawing.Image img;

                    MemoryStream strm = new MemoryStream();
                    strm.Write(oImageBytes, 0, oImageBytes.Length);
                    strm.Position = 0;

                    img = System.Drawing.Image.FromStream(strm);

                    picImage.Image = img;
                }
            }
            catch (Exception err)
            {
            }
        }

        private void btnPnlFindImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.CheckFileExists = true;

            oDialog.ShowDialog();
            if (oDialog.FileName != "")
            {
                
                MemoryStream oStream = oImage.resizeImage(oDialog.FileName, 800);

                oImageBytesNew = oStream.ToArray();

                txtPnlFriendlyName.Text = oDialog.SafeFileName;
                txtPnlFullName.Text = oDialog.FileName;
            }
        }

        private void btnPnlAddImage_Click(object sender, EventArgs e)
        {
            oImage.usp_Images_CaseUpdate_Insert(txtPnlFriendlyName.Text
                , oImageBytesNew
                , CaseID
                , Program.Username);

            BindTreeView();
            
            pnlAddImage.Visible = false;
        }

        private void btnPnlCancel_Click(object sender, EventArgs e)
        {
            pnlAddImage.Visible = false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;


using System.Windows.Forms;namespace Icondev.MedManage.MedManageLib
{
    public class Images
    {
        private Database oDb;
        public Images(Database oDatabase)
        {
            oDb = oDatabase;
        }

        public void NewThumbnail(string filePath, string saveLocation, string thumbLocation, string bigLocation)
        {
            try
            {
                System.Drawing.Image image = new Bitmap(filePath);
                double x = Convert.ToDouble(image.Width);
                double y = Convert.ToDouble(image.Height);
                double z;
                if (x > 100)
                {
                    z = 100 / x;
                    int NewY = Convert.ToInt32(Math.Round(y * z));
                    System.Drawing.Image pThumbnail = image.GetThumbnailImage(100, NewY, null, new IntPtr());
                    pThumbnail.Save(saveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(saveLocation, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        public bool DeleteImage(string ImagePath)
        {
            try
            {
                File.Delete(ImagePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void NewThumbnail(string filePath, double Height, double Width)
        {
            try
            {
                System.Drawing.Image image = new Bitmap(filePath);
                double x = Convert.ToDouble(image.Width);
                double y = Convert.ToDouble(image.Height);
                //double z;
                if (x > Width || y > Height)
                {
                    //    z = Width / x;
                    //    int NewY = Convert.ToInt32(Math.Round(y * z));
                    System.Drawing.Image pThumbnail = image.GetThumbnailImage((int)Math.Round(Width), (int)Math.Round(Height), null, new IntPtr());
                    pThumbnail.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        public void NewThumbnail(string filePath, double Height)
        {
            try
            {
                System.Drawing.Image image = new Bitmap(filePath);
                double x = Convert.ToDouble(image.Width);
                double y = Convert.ToDouble(image.Height);
                double z;
                //DeleteImage(filePath);
                if (y > Height)
                {
                    z = y / Height;
                    int NewX = Convert.ToInt32(Math.Round(x / z));
                    System.Drawing.Image pThumbnail = image.GetThumbnailImage(NewX, (int)Math.Round(Height), null, new IntPtr());
                    pThumbnail.Save(filePath.Replace("_Thumb_", ""), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }

        public void resizeImage(string filePath, double Height, string DestinationPath)
        {
            System.Drawing.Image image = new Bitmap(filePath);
            double x = Convert.ToDouble(image.Width);
            double y = Convert.ToDouble(image.Height);
            double z;
            //DeleteImage(filePath);
            if (y > Height)
            {
                z = y / Height;
                int Width = Convert.ToInt32(Math.Round(x / z));
                Bitmap b = new Bitmap(image, Width, (int)Math.Round(Height));
                b.Save(DestinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            else
            {
                image.Save(DestinationPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        public MemoryStream resizeImage(string filePath, double Height)
        {
            System.Drawing.Image image = new Bitmap(filePath);
            double x = Convert.ToDouble(image.Width);
            double y = Convert.ToDouble(image.Height);
            double z;
            Bitmap b = new Bitmap(image);
            MemoryStream oStream = new MemoryStream();
            //DeleteImage(filePath);
            if (y > Height)
            {
                z = y / Height;
                int Width = Convert.ToInt32(Math.Round(x / z));
                b = new Bitmap(b, Width, (int)Math.Round(Height));

                b.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return oStream;

            }
            else
            {
                b.Save(oStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return oStream;
            }
        }

        public DataSet usp_Images_CaseUpdate_SelectByCaseID(int CaseID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Images_CaseUpdate_SelectByCaseID");
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs;
        }
        public void usp_Images_CaseUpdate_Insert(string Name, byte[] image,int CaseID, string UserID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Images_CaseUpdate_Insert");
            oDb.AddInParameter(sprocCmd, "Name", DbType.String, Name);
            oDb.AddInParameter(sprocCmd, "image", DbType.Binary, (object)image);
            oDb.AddInParameter(sprocCmd, "CaseID", DbType.Int32, CaseID);
            oDb.AddInParameter(sprocCmd, "UserID", DbType.String, UserID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            //return oDs;
        }

        public void usp_Images_CaseUpdate_Delete(int ImageID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Images_CaseUpdate_Delete");
            oDb.AddInParameter(sprocCmd, "ImageID", DbType.Int32, ImageID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            //return oDs;
        }

        public DataTable usp_Images_CaseUpdate_SelectImageByImageID(int ImageID)
        {
            DbCommand sprocCmd = oDb.GetStoredProcCommand("usp_Images_CaseUpdate_SelectImageByImageID");
            oDb.AddInParameter(sprocCmd, "ImageID", DbType.Int32, ImageID);
            DataSet oDs = oDb.ExecuteDataSet(sprocCmd);
            return oDs.Tables[0];
        }
    }
}

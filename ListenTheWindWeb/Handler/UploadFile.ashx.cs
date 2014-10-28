using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;


using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HDS.QMS.Handler
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                string s =  context.Request.Form["Filedata"].ToString();

                var base64Data = Regex.Match(s, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var binData = Convert.FromBase64String(base64Data);

                string sNewFileName = DateTime.Now.ToString("yyyyMMddhhmmsfff");

                MemoryStream ms = new MemoryStream(binData);

                //original image
                MemoryStream msPass = new MemoryStream();
                
                MemoryStream msOut = new MemoryStream();

                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                
                int height = img.Height;
                int width = img.Width;
                if (img.Width >= 1024)
                {
                    height = (int)((1000 / img.Width) * img.Height);
                    if (height >= 768)
                    {
                        height = 720;
                        width = (int)((720 / height) * 1000);
                    }
                    else
                        width = 1000;
                }
                if (img.Height >= 768)
                {
                    height = 720;
                    width = (int)((720 / height) * img.Width);
                    if (width > 1024)
                    {
                        width = 1000;
                        height = (int)((1000 / width) * 720);
                    }
                    else
                        width = 1000;
                }

                MakeThumbnail(img, 60, 50, "HW", out msOut);
                if(width != img.Width && height != img.Height)
                    MakeThumbnail(img, width, height, "HW", out msPass);
                else
                    msPass = new MemoryStream(binData);
                img.Dispose();
                string uploadImageType = System.Configuration.ConfigurationManager.AppSettings["UploadImageType"];
                string uploadImagePath = "localuploadimage";
                if (uploadImageType == "test")
                {
                    uploadImagePath = "testuploadimage";
                }
                else if (uploadImageType == "production")
                {
                    uploadImagePath = "productionuploadimage";
                }
                string sExtension =".png";

                //upload file to store
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["AzureBlobStorage"]);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(uploadImagePath);
                container.CreateIfNotExists();
                BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Container;

                //Set the permission policy on the container.
                container.SetPermissions(containerPermissions);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(sNewFileName + sExtension);

                using (msPass)
                {
                    //var callback = new AsyncCallback(OnUploadCompleted);
                    //IAsyncResult result = blockBlob.BeginUploadFromStream(ms, callback, new object[] { blockBlob, context });
                    //blockBlob.EndUploadFromStream(result);
                    msPass.Position = 0;
                    blockBlob.UploadFromStream(msPass);
                }

                CloudBlockBlob blockBlobSmallImage = container.GetBlockBlobReference(sNewFileName+"S" + sExtension);
                using (msOut)
                {
                    msOut.Position = 0;
                    blockBlobSmallImage.UploadFromStream(msOut);
                }

                string imgUrl = blockBlobSmallImage.Uri.AbsoluteUri;
                context.Response.Write(imgUrl);
                context.Response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
                context.Response.End();
            }
        }
        /*
        public void OnUploadCompleted(IAsyncResult result)
        {
            Object[] states = (Object[])result.AsyncState;
            CloudBlockBlob blob = (CloudBlockBlob)states[0];
            string imgUrl = blob.Uri.AbsoluteUri;

            HttpContext _httpContext = (HttpContext)states[1];
            _httpContext.Response.Write(imgUrl);
           // _httpContext.Response.StatusCode = 200;

        }
        */
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        ///<summary> 
  /// 生成缩略图 
  /// </summary> 
  /// <param name="originalImagePath">源图路径（物理路径）</param> 
  /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
  /// <param name="width">缩略图宽度</param> 
  /// <param name="height">缩略图高度</param> 
  /// <param name="mode">生成缩略图的方式</param>     
        public void MakeThumbnail(System.Drawing.Image originalImage, int width, int height, string mode,out MemoryStream ms)
        {
            System.Drawing.Image _originalImage = originalImage;
            ms = new MemoryStream();

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":                
                    break;
                case "W":                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut":                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //outthumbnailPath = thumbnailPath;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                //originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}
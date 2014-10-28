<%@ WebHandler Language="C#" Class="UploadFile" %>
using System;
using System.Web;
using System.IO;
using System.Data;


public class UploadFile : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";
        context.Response.Expires = -1;
        try
        {
            HttpPostedFile postedFile = context.Request.Files["Filedata"];
            string savepath = "";
            savepath = context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]) ;//System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
            string filename = postedFile.FileName;
            string sExtension = filename.Substring(filename.LastIndexOf('.'));
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);
            string sNewFileName = DateTime.Now.ToString("yyyyMMddhhmmsfff");
            postedFile.SaveAs(savepath + @"\" + sNewFileName + sExtension);
            context.Response.Write(savepath + "\\" + sNewFileName + sExtension);
            context.Response.StatusCode = 200;
        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
            context.Response.End();
        }
    }

    public bool IsReusable 
    {
        get {
            return false;
        }
    }

}
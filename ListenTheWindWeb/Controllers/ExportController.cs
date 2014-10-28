using BizLogic;
using ComLib.Export;
using DataAccess.DC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Energizer.User;
using WebModel.Col;

namespace HDS.QMS.Controllers
{
    /// <summary>
    /// Created By: Peter Sun
    /// Created Date: 11/16/2012
    /// </summary>
    public class ExportController : Controller
    {
        //
        // GET: /Export/
        public ExportBizLogic exportBiz = new ExportBizLogic();
        User user = UserHelper.CurrentUser;
        string permissions = ",";
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ExportQuoteList(string mainId, string fullSearch, string colInfos)
        {
            try
            {
                BuildExportParamater("Quote", mainId, fullSearch, colInfos);
                return PartialView("Export");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ExportPOSList(string mainId, string fullSearch, string colInfos)
        {
            try
            {
                BuildExportParamater("POS", mainId, fullSearch, colInfos);
                return PartialView("Export");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void BuildExportParamater(string dataType, string mainId, string fullSearch, string colInfos)
        {

            List<string[]> hashTemp = new List<string[]>();

            var listCol = JsonConvert.DeserializeObject(colInfos, typeof(List<colInfoModel>)) as List<colInfoModel>;
            foreach (colInfoModel col in listCol)
            {
                if ((col.disType == "text" || col.disType == "date") && col.isHidden.Trim() != "true") // 
                {
                    //Hashtable colInfo = new Hashtable();
                    string[] str = { col.columnName, col.disName_en };

                    hashTemp.Add(str);
                }
            }
            ViewData["DataType"] = dataType;
            ViewData["MainId"] = mainId;
            ViewData["SearchField"] = "";
            ViewData["SearchString"] = fullSearch;
            ViewData["SearchOper"] = "";
            ViewData["Sidx"] = "";
            ViewData["Sord"] = "";
            //ViewData["ExportFields"] = colInfo;
            ViewData["ExportFields"] = hashTemp;
            ViewData["ExportFieldsMataData"] = colInfos;
        }

        public string ExportFile(string dataType, string exportType, string exportFields, string fullSreach, string exportFieldsMataData)
        {
            //Get Export Data
            DataTable dtExportData = new DataTable();
            switch (dataType)
            {
                case "Quote":
                    //dtExportData = exportBiz.GetQuoteExportData(fullSreach);
                    break;
                case "POS":
                    //dtExportData = exportBiz.GetPOSExportData(fullSreach);
                    break;
            }

            //Filter Columns
            string[] exportFieldList = exportFields.Split(',');


            var listCol = JsonConvert.DeserializeObject(exportFieldsMataData, typeof(List<colInfoModel>)) as List<colInfoModel>;
            List<string[]> listTemp = new List<string[]>();
            foreach (colInfoModel col in listCol)
            {
                if ((col.disType == "text" || col.disType == "date") && col.isHidden.Trim() != "true") // 
                {
                    foreach (string s in exportFieldList)
                    {
                        if (s == col.columnName)
                        {
                            string[] str = { col.columnName, col.disName_en };
                            listTemp.Add(str);
                            break;
                        }
                    }
                }
            }

            for (int i = dtExportData.Columns.Count - 1; i >= 0; i--)
            {
                bool exportFalg = false;
                foreach (string fieldName in exportFieldList)
                {
                    if (dtExportData.Columns[i].ColumnName == fieldName)
                    {
                        exportFalg = true;
                        break;
                    }
                }

                if (!exportFalg)
                    dtExportData.Columns.Remove(dtExportData.Columns[i]);
            }

            //Generate Export File
            string fileName = "ExportData_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + (new Random()).Next(100, 999).ToString();
            ExportModel exportObj = new ExportModel();
            Export exportController = new Export();
            exportObj.virtualPath = ConfigurationManager.AppSettings["DownLoadVirtualPath"];

            exportObj.xlsFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".xls";
            exportObj.csvFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".csv";
            exportObj.zipFileName = Server.MapPath(exportObj.virtualPath) + fileName + ".zip";

            exportObj.fileName = fileName;
            fileName = exportController.GenerateExportFile(exportType, dtExportData, exportObj, listTemp);
            return "../DownLoadData/" + fileName;
        }
        /// <summary>
        /// This function is the control of export detail, with two types, one is excel and another one is pdf
        /// </summary>
        /// <param name="pageType">Individual page of export.</param>
        /// <param name="exportType">Export type.</param>
        /// <returns>Full file path of file generated.</returns>
        [Authorize]
        public FileStreamResult ExportDetailPage(string pageType, string exportType, int oid)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public FileStreamResult DownLoadFile(string pageType, string exportType, int oid)
        {

            string filePath = string.Empty;

            string[] s = filePath.Split('|');
            return File(new FileStream(s[0], FileMode.Open), "application/octet-stream", Server.UrlEncode(s[1]));
        }
    }
}

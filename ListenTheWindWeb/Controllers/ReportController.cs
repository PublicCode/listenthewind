using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataAccessLayer;
using IDataAccessLayer;
using Newtonsoft.Json;
using T2VSoft.MVC.Core;
using WebModel.Col;

namespace HDS.QMS.Controllers
{
    public class ReportController : T2VController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSubject()
        {
            IReportManager ireport = new ReportManager();
            return Json(new { result = ireport.GetSubjectAndSearchFormData() });
        }

        [T2VAuthorize]
        public ActionResult EncryptParam(string param)
        {
            Dictionary<string, string> ctrlValue = new Dictionary<string, string>();
            var listCol = JsonConvert.DeserializeObject(param, typeof(List<colModel>)) as List<colModel>;
            foreach (colModel colmodel in listCol)
            {
                ctrlValue.Add(colmodel.columnName, colmodel.columnValue);
            }

            return Json(new { result = string.Join(";", ctrlValue.Select(c => c.Key + ":" + Convert.ToBase64String(Encoding.Default.GetBytes(c.Value)))) });
        }

        [T2VAuthorize]
        public ActionResult GetReport(int currentPageIndex, int pageSize, string sortName, string sortOrder, string SearchParam)
        {
            //Get a claim biz logic instance.
            Dictionary<string, string> ctrlValue = new Dictionary<string,string>();
            string strSubject = "";
            string fullsearch = string.Empty;
            if (SearchParam != "")
            {
                var listCol = JsonConvert.DeserializeObject(SearchParam, typeof(List<colModel>)) as List<colModel>;
                foreach (colModel colmodel in listCol)
                {
                    if(colmodel.columnName == "subject")
                    {
                        strSubject = colmodel.columnValue;
                        continue;
                    }
                    ctrlValue.Add(colmodel.columnName, colmodel.columnValue);
                }
            }
            try
            {
                IReportManager report = new ReportManager();
                var res = report.GetReport(strSubject, ctrlValue, "", "", currentPageIndex, pageSize);
                return JsonNet(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

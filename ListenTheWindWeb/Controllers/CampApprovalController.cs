using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Energizer.User;
using Handler;
using BizLogic;
using DataAccess.DC;
using T2VSoft.MVC.Core;
using System.Configuration;
using System.IO;
using DataAccessLayer;
using IDataAccessLayer;
using Newtonsoft.Json;
using WebModel.Camp;
using WebModel.Account;
using WebModel.ApprovalCamp;

namespace HDS.QMS.Controllers
{
    public class CampApprovalController : Controller
    {
        ApprovalCampBizLogic bizLogic;
        public CampApprovalController()
        {
            bizLogic = new ApprovalCampBizLogic(UserHelper.CurrentUser);
        }

        public ActionResult Index()
        {
            ViewBag.lstObj = bizLogic.GetApprovalCampLstModel(1, 12);
            return View();
        }
        public ActionResult GetApprovalCampLstModel(int page, int limit)
        {
            return Json(new { data = bizLogic.GetApprovalCampLstModel(page, limit) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ApprovalDetail(int campID, DateTime? dt)
        {
            var campmodel = bizLogic.GetCamp(campID, dt);
            campmodel.paramDate = dt;
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View();
        }
        public ActionResult ApprovalEdit(int campID, DateTime? dt)
        {
            var campmodel = bizLogic.GetCamp(campID, dt);
            campmodel.paramDate = dt;
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View();
        }
        public ActionResult SaveApprovalCamp(approvalcampModel info, string ops)
        {
            if (ops == "submitBy2")
            {
                info.ApprovalStatus = "待审批";
            }
            return Json(new { campID = info.CampID });
        }
    }
}

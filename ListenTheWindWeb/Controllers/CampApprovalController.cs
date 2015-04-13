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
    }
}

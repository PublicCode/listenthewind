using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T2VSoft.MVC.Core;
using Web.Energizer.User;
using DataAccessLayer;

using ComLib.HTTPResultHelpers;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;
using ComLib.Extension;

using IDataAccessLayer;

using Newtonsoft.Json;
using System.Collections;

namespace HDS.QMS.Controllers
{


    [T2VAuthorize]
    public class AdminController : T2VController
    {

        public ActionResult Index()
        {
            return View();
        }

        [T2VAuthorize]
        public new ActionResult User()
        {
            return View();
        }

        [T2VAuthorize]
        public ActionResult UserProfile()
        {
            return View();
        }

        [T2VAuthorize]
        public ActionResult UserPermission()
        {
            return View();
        }

        [T2VAuthorize]
        public ActionResult Permission()
        {
            return View();
        }


        [T2VAuthorize]
        public ActionResult GetTimeZoneList()
        {
            var list = TimeZoneInfo.GetSystemTimeZones().ToList().Select(c => c.Id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [T2VAuthorize]
        public ActionResult UserEdit(int userID)
        {
            if (userID == 0)
            { 
                return PartialView("~/Views/Admin/UserEdit.cshtml", new User());
            }
            else
                return PartialView("~/Views/Admin/UserEdit.cshtml");
        }

    }
}

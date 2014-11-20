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

namespace AdestoSolution.Controllers
{
    public class HomeController : T2VController
    {
        [Authorize]
        public ActionResult Index()
        {
            HomeBizLogic h = new HomeBizLogic();
            string num = h.TestMysqlDB();
            ViewBag.Message = "Welcome to HDS!";
            return View();
        }
        /// <summary>
        /// In this function, we get all activitis for this system for specific user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>

        public ActionResult About()
        {
            return View();
        }
        public ActionResult ReleaseNote()
        {
            return View("~/Views/Home/CampList.cshtml");
        }
        public ActionResult CampDetail()
        {
            return View("~/Views/Home/CampDetail.cshtml");
        }
        public ActionResult CampBook()
        {
            return View("~/Views/Home/CampBook.cshtml");
        }
        public ActionResult ComingSoon()
        {
            return View("~/Views/shared/ComingSoon.cshtml");
        }

        public string GetNewNumber(string TypeName,string strFirstChar)
        {
            IHomeManager homeManager = new HomeManager();
            return homeManager.GetNewNumber(TypeName,strFirstChar);
        }
    }
}

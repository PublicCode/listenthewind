﻿using System;
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

namespace AdestoSolution.Controllers
{
    public class HomeController : T2VController
    {
        HomeBizLogic bizLogic;

        public HomeController()
        {
            bizLogic = new HomeBizLogic();
        }

        [Authorize]
        public ActionResult Index()
        {
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
        public ActionResult CampDetail(int CampID)
        {
            campModel campmodel = bizLogic.GetCamp(CampID);
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View("~/Views/Home/CampDetail.cshtml");
        }

        public string CampCollect(int CampID)
        {
            return bizLogic.CampCollect(CampID);
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
        //public ActionResult GetClient()
        //{

        //}
    }
}

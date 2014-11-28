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

namespace AdestoSolution.Controllers
{
    public class HomeController : T2VController
    {
        HomeBizLogic bizLogic;
        HomeManager hoManger;
        public HomeController()
        {
            bizLogic = new HomeBizLogic();
            hoManger = new HomeManager();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CityInfo = JsonConvert.SerializeObject(hoManger.GetCitys());
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
            string strParam = Request.Form["param"];
            if (string.IsNullOrEmpty(strParam))
                return RedirectToAction("Index", "Home");

            var lst = hoManger.GetCampList(Convert.ToInt32(strParam.Split('/')[0]), Convert.ToDateTime(strParam.Split('/')[1]));
            ViewBag.lstInfo = JsonConvert.SerializeObject(lst);
            return View("~/Views/Home/CampList.cshtml");
            //return RedirectToAction("CampList", "Home");
        }
        public ActionResult CampDetail(int CampID)
        {
            campModel campmodel = bizLogic.GetCamp(CampID,null);
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View("~/Views/Home/CampDetail.cshtml");
        }

        public string CampCollect(int CampID)
        {
            return bizLogic.CampCollect(CampID);
        }

        public ActionResult CampBook(int CampID)
        {
            campModel campmodel = bizLogic.GetCamp(CampID, null);
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
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

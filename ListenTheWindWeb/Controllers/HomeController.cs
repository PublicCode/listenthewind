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
using DataAccessLayer.DTO;

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

            var info = new CampListSeachDTO { LocationID = Convert.ToInt32(strParam.Split('/')[0]), JoinCampDate = strParam.Split('/')[1] };
            var lst = hoManger.GetCampList(info, 1, 12);
            ViewBag.lstInfo = JsonConvert.SerializeObject(lst);
            ViewBag.CityInfo = JsonConvert.SerializeObject(hoManger.GetCitys());
            ViewBag.BasicData = JsonConvert.SerializeObject(hoManger.GetBasicData());
            return View("~/Views/Home/CampList.cshtml");
        }
        public ActionResult AjaxCampList(string searchInfo, int page, int limit)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var info = js.Deserialize<CampListSeachDTO>(searchInfo);

            var lst = hoManger.GetCampList(info, page, limit);
            return Json(lst, JsonRequestBehavior.AllowGet);
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

        public ActionResult CampBook(int CampID, int PileID)
        {
            campModel campmodel = bizLogic.GetCamp(CampID, null);
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            ViewBag.PileID = PileID;
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
        public ActionResult getReservedDateForPile(int PileId)
        {
            IHomeManager homeManager = new HomeManager();
            return Json(homeManager.GetListOfReserveForPile(PileId));
        }
    }
}

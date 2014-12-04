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
        public HomeController()
        {
            bizLogic = new HomeBizLogic();
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CityInfo = JsonConvert.SerializeObject(bizLogic.GetCitys());
            ViewBag.BasicData = JsonConvert.SerializeObject(bizLogic.GetBasicData());
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

            var info = new CampListSeachModel { LocationID = Convert.ToInt32(strParam.Split('/')[0]), JoinCampDate = strParam.Split('/')[1], CampLOD = strParam.Split('/')[2] };
            var lst = bizLogic.GetCampList(info, 1, 12);
            ViewBag.lstInfo = JsonConvert.SerializeObject(lst);
            ViewBag.CityInfo = JsonConvert.SerializeObject(bizLogic.GetCitys());
            ViewBag.BasicData = JsonConvert.SerializeObject(bizLogic.GetBasicData());
            return View("~/Views/Home/CampList.cshtml");
        }
        public ActionResult AjaxCampList(string searchInfo, int page, int limit)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var info = js.Deserialize<CampListSeachModel>(searchInfo);

            var lst = bizLogic.GetCampList(info, page, limit);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CampDetail(int CampID, DateTime? dt)
        {
            campModel campmodel = bizLogic.GetCamp(CampID, dt);
            campmodel.paramDate = dt;
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View("~/Views/Home/CampDetail.cshtml");
        }

        public string CampCollect(int CampID)
        {
            return bizLogic.CampCollect(CampID);
        }

        public ActionResult CampBook(int CampID, int PileID, DateTime? BookDate)
        {
            campModel campmodel = bizLogic.GetCamp(CampID, BookDate);
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            ViewBag.PileID = PileID;
            var listOfBasic = bizLogic.GetBasicDataForCamp();
            foreach (campitemModel itemModel in campmodel.ModelListcampitem)
            { 
               var basicModel =  listOfBasic.FirstOrDefault(m=>m.DataName == itemModel.CampItemName.ToLower());
               if (basicModel != null)
                    basicModel.Checked =true;
            }
            ViewBag.BasicData = JsonConvert.SerializeObject(listOfBasic);
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

        public string SaveReserve(List<DateTime> SelectedDate, campModel Camp, int PileID)
        {
            List<camppriceModel> listOfCampPrice = Camp.ModelListcampprice;
            return bizLogic.SaveReserve(SelectedDate, listOfCampPrice, Camp.CampID, PileID);
        }
    }
}

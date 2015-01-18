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

namespace HDS.QMS.Controllers
{
    public class UserController : T2VController
    {
        UserBizLogic bizLogic;
        public UserController()
        {
            bizLogic = new UserBizLogic(UserHelper.CurrentUser);
        }
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPayBook()
        {
            return View();
        }

        public ActionResult UserCollection()
        {
            ViewBag.CampCollectByUser = JsonConvert.SerializeObject(bizLogic.GetCampCollectByUser());
            return View();
        }

        public string DeleteCampCollect(int CampID)
        {
            return bizLogic.DeleteCampCollect(CampID);
        }
        

    }
}

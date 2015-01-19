using System;
using System.IO;
using System.Web.Mvc;
using BizLogic;
using Newtonsoft.Json;
using T2VSoft.MVC.Core;
using Web.Energizer.User;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

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

        public ActionResult UserIntegral()
        {
            ViewBag.UsedIntegralSum = JsonConvert.SerializeObject(bizLogic.GetUseIntegralNumber());
            ViewBag.UserIntegralList = JsonConvert.SerializeObject(bizLogic.GetIntegralList(1,20));
            return View();
        }

        public ActionResult UserHeaderImg()
        {
            return View();
        }

        public ActionResult UserFileUpload()
        {
            string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]) + "TempFile";
            string strTemp = string.Empty;
            var fileName = DateTime.Now.Ticks.ToString() + ".jpg";
            try
            {
                using (var inputStream = Request.Files.Count > 0 ? Request.Files[0].InputStream : Request.InputStream)
                {
                    using (var flieStream = new FileStream(savePath + @"\" + fileName, FileMode.Create))
                    {
                        inputStream.CopyTo(flieStream);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, "text/plain");
                throw ex;
            }
            return Json(new { success = true, fileName = fileName }, "text/plain");
        }
        public string SaveUserPhotos(string tmpFileName)
        {
            string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]);
            var oFile = savePath + "TempFile\\" + tmpFileName;
            var nFile = savePath + "User\\" + tmpFileName;
            System.IO.File.Move(oFile, nFile);

            bizLogic.SaveUserHeadPhoto(tmpFileName);
            return true.ToString();
        }

        public object GetIntegralList(int page, int limit)
        {
            return JsonConvert.SerializeObject(bizLogic.GetIntegralList(page, limit));
        }
    }
}

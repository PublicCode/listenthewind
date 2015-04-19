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
using WebModel.Account;
using WebModel.ApprovalCamp;

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
        public ActionResult Edit(int userId)
        {
            UserModel userModel;
            if (userId == 0)
                userModel = new UserModel();
            else
                userModel = bizLogic.GetUserInfo(userId);
            ViewBag.UserModel = JsonConvert.SerializeObject(new { usermodel = userModel, loginflag = true });
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

        public ActionResult UserInfo()
        {
            ViewBag.UserAllInfo = JsonConvert.SerializeObject(bizLogic.GetUserAllInfo());
            return View();
        }
         [T2VAuthorize]
        public ActionResult UserManagement()
        {
            return View();
        }
        [T2VAuthorize]
        public ActionResult GetAllUserList(int page, int limit)
        {
            return Json(new { listOfUsers = bizLogic.GetAllUserForList(page, limit) });
        }
        [T2VAuthorize]
        public ActionResult AssignManager()
        {
            return View();
        }
        [T2VAuthorize]
        public ActionResult GetAllCampList(int page, int limit)
        {
            return Json(new { listOfCamps = bizLogic.GetAllCampList(page, limit) });
        }

        [T2VAuthorize]
        public ActionResult GetAllManagerList()
        {
            return Json(new { managerList = bizLogic.GetAllManagerList() });
        }
        [T2VAuthorize]
        public ActionResult ChooseManager(approvalcampModel apprvedCamp)
        {
            return Json(new { Code = bizLogic.ChooseManager(apprvedCamp) });
        }
        public ActionResult PassValidate(int userId)
        {
            return Json(bizLogic.PassValidate(userId));
        }
        public ActionResult DeleteUser(int userId)
        {
            int res = bizLogic.DeleteUser(userId);
            switch(res)
            {
                case 1:
                    return Json(new { Code = "Sucess", Message = "删除成功." });
                case 2:
                    return Json(new { Code = "Fail", Message = "此用户有管理的营地，请重新分配营地管理员后删除." });
                case 3: default:
                    return Json(new { Code = "Fail", Message = "有异常，请联系管理员." });
            }
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
            DataAccess.DC.User user = Session["user"] as DataAccess.DC.User;
            user.HeadPhoto = tmpFileName;
            Session["user"] = user;
            return true.ToString();
        }

        public string saveIDNumberInfo(UserModel userModel)
        {
            string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]);
            if (userModel.IDNumberImg1 != null && userModel.IDNumberImg1.IndexOf("TempFile") > -1)
            {
                var oFile = savePath + userModel.IDNumberImg1;
                var nFile = savePath + "User\\" + userModel.IDNumberImg1.Replace("TempFile\\", "");
                System.IO.File.Move(oFile, nFile);
            }
            if (userModel.IDNumberImg2 != null && userModel.IDNumberImg2.IndexOf("TempFile") > -1)
            {
                var oFile = savePath + userModel.IDNumberImg2;
                var nFile = savePath + "User\\" + userModel.IDNumberImg2.Replace("TempFile\\", "");
                System.IO.File.Move(oFile, nFile);
            }

            return bizLogic.saveIDNumberInfo(userModel);
        }
        public ActionResult CreateUser(UserModel userModel)
        {
            int res = bizLogic.CreateUser(userModel);
            if (res == -1)
                return Json(new { Code = "Fail", Message = "用户名或邮箱已存在" });
            else if (res>0)
                return Json(new { Code = "Success", Message = "创建成功" });
            else
                return Json(new { Code = "Fail", Message = "程序错误" });
        }

        public string saveUserBasicInfo(UserModel userModel)
        {
            return bizLogic.saveUserBasicInfo(userModel);
        }

        public string saveUserAuthInfo(UserModel userModel)
        {
            return bizLogic.saveUserAuthInfo(userModel);
        }

        

        public object GetIntegralList(int page, int limit)
        {
            return JsonConvert.SerializeObject(bizLogic.GetIntegralList(page, limit));
        }

        public ActionResult UserIDNumFileUpload()
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
            return Json(new { success = true, fileName = "TempFile\\" + fileName }, "text/plain");
        }
    }
}

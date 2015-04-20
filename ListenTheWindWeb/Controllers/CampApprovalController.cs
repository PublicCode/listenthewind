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
using WebModel.ApprovalCamp;

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
        public ActionResult ApprovalDetail(int campID, DateTime? dt)
        {
            var campmodel = bizLogic.GetCamp(campID, dt);
            campmodel.paramDate = dt;
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View();
        }
        public ActionResult ApprovalEdit(int campID, DateTime? dt)
        {
            var campmodel = bizLogic.GetCamp(campID, dt);
            campmodel.paramDate = dt;
            ViewBag.CampInfo = JsonConvert.SerializeObject(campmodel);
            return View();
        }
        public ActionResult CampFileUpload()
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
        public ActionResult CampFileUpload1()
        {
            string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]) + "Camp";
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
        public ActionResult CampFileUpload2()
        {
            string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]) + "Camp";
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
        public void MoveCampPhotos(List<approvalcampphotoModel> lst)
        {
            foreach (var info in lst)
            {
                if (info.CampPhotoID == 0)
                {
                    string savePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UpLoadPath"]);
                    var oFile = savePath + "TempFile\\" + info.CampPhoteFile;
                    var nFile = savePath + "Camp\\" + info.CampPhoteFile;
                    System.IO.File.Move(oFile, nFile);
                }
            }
        }
        public void UpdateCommentRes(int id, string cres, string type)
        {
            bizLogic.UpdateComment(id, cres, type);
        }
        public ActionResult SaveApprovalCamp(approvalcampModel info, string ops)
        {
            if (ops == "submitBy2")
            {
                info.ApprovalStatus = "待审批";
                info.RejectReason = "";
            }
            else if (info.CampID == 0)
            {
                info.ApprovalStatus = "待审批";
            }
            bizLogic.SaveCamp(info);
            MoveCampPhotos(info.ModelListcampphoto);
            return Json(new { campID = info.CampID });
        }
        public ActionResult ApprovalCamp(approvalcampModel info, string ops)
        {
            if (ops == "submitBy3")
            {
                info.ApprovalStatus = "已审批";
                info.RejectReason = "";
            }
            bizLogic.SaveCamp(info);
            bizLogic.ApprovalCamp(info);
            return Json(new { campID = info.CampID });
        }
        public ActionResult RejectCamp(approvalcampModel info)
        {
            info.ApprovalStatus = "拒绝";
            bizLogic.RejectCamp(info);
            return Json(new { campID = info.CampID });
        }
    }
}

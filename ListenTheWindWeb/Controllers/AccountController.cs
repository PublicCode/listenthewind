using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BizLogic;
using BizLogic.MailServiceBiz;
using ComLib.Extension;
using ComLib.Mail;
using DataAccess.DC;
using Newtonsoft.Json;
using WebModel.Account;

namespace HDS.QMS.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn
        public ActionResult LogOn(string q)
        {
            return View();
        }

        public ActionResult MobileLogOn()
        {
            return View("~/Views/MobileAccount/LogOn.cshtml");
        }

        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult LogOn(string UserName, string Pwd, string returnUrl)
        {
            Session.RemoveAll();
            AccountHelper accountHelper = new AccountHelper();
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? (Request["returnUrl"] ?? "") : returnUrl;
            if (ModelState.IsValid)
            {
                int returnResut = accountHelper.UserLogon(UserName, Pwd);
                if (returnResut ==1)
                {
                    Session["ReleaseNoteFlag"] = false;
                    //get user 
                    DataAccess.DC.User user = accountHelper.GetUserByName(UserName);
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserName",UserName));
                    HttpContext.Response.Cookies.Add(new HttpCookie("UserID", user.UserID.ToString()));
                    Session["user"] = user;
                    return Json("Login Sucess");
                }
                else if (returnResut == 2)
                {
                    ModelState.AddModelError("", "User account is inactive, please contact your Administrator.");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password.");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult getClientInfo(int id)
        {
            AccountHelper accountHelper = new AccountHelper();
            return Json(accountHelper.GetUserByID(id));
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        #region 20141228
        [HttpPost]
        public string UserLogOn(string strJson)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var info = js.Deserialize<UserModel>(strJson);
            Session.RemoveAll();
            AccountHelper accountHelper = new AccountHelper();

            int returnResut = accountHelper.UserLogon(info.UserName, info.Pwd);
            if (returnResut == 1)
            {
                DataAccess.DC.User user = accountHelper.GetUserByName(info.UserName);
                user.Pwd = "";
                Session["user"] = user;
                ModelConverter.Convert<User, UserModel>(user, info);
            }
            else if (returnResut == 2)
            {
                info.errPwd = true;
                info.errUserName = false;
            }
            else if (returnResut == 3)
            {
                info.errPwd = false;
                info.errUserName = true;
            }
            info.Pwd = "";
            return JsonConvert.SerializeObject(info);
        }
        [HttpPost]
        public string UserLogOff()
        {
            try {
                Session.RemoveAll();
                return true.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        [HttpPost]
        public ActionResult UserRegister(string strJson)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var info = js.Deserialize<UserModel>(strJson);
            var createFlag = false;
            Session.RemoveAll();
            AccountHelper accountHelper = new AccountHelper();

            var uInfo = accountHelper.GetUserByName(info.UserName);
            if (uInfo == null)
            {
                uInfo = new User
                {
                    UserName = info.UserName,
                    Pwd = info.Pwd,
                    Mail = info.Email,
                    CreateTime = DateTime.Now,
                    Name = info.UserName
                };
                uInfo = accountHelper.CreateUser(uInfo);
                createFlag = true;
                ModelConverter.Convert<User, UserModel>(uInfo, info); 
            }
            else
            {
                info.errUserName = true;
            }
            info.Pwd = string.Empty;
            info.RePwd = string.Empty;
            return Json(new { success = createFlag, data = info }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UserResetPws(string email)
        {
            AccountHelper accountHelper = new AccountHelper();

            var uInfo = accountHelper.GetUserByEmail(email);
            try
            {
                if (uInfo != null)
                {
                    var pwd = GetNewPassword();
                    var mInfo = new MailObject
                    {
                        MailReceiver = email,
                        MailBody = "您的新密码为:" + pwd,
                        MailSubject = "修改听风用户密码",
                        MailSender = ConfigurationManager.AppSettings["username"].ToString()
                    };
                    BizLogic.MailServiceBiz.MailFactoryLoader.Send_Mail(mInfo);
                    accountHelper.UpdatePwd(email, pwd);
                    return Json(new { success = true, mailServer = "mail." + email.Split('@')[1].ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mailServer = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        private static string GetNewPassword()
        {
            string allchars = "!@#$%ABCDEFGHIJKLMNOPQRSTUVXYZ ";
            StringBuilder sb = new StringBuilder(8);
            Random rand = new Random();
            for (int i = 0; i < 2; i++)
            {
                sb.Append(allchars[rand.Next(11, allchars.Length)]);
            }
            for (int i = 0; i < 4; i++)
            {
                sb.Append(allchars[rand.Next(10)]);
            }
            return sb.ToString();
        } 
        #endregion
    }
}

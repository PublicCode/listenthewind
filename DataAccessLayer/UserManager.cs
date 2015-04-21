using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataAccess.DC;
using IDataAccessLayer;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;
using WebModel.Camp;
using ComLib.Extension;
using Newtonsoft.Json;
using System.IO;
using WebModel.Account;

namespace DataAccessLayer
{
    public class UserManager : BaseManager, IUserManager
    {
        protected User _user;
        public UserManager(User user)
        {
            _user = user;
        }

        public void aa() {
            File.Move("C:\\Test.txt", "D:\\Test.txt");
        }

        public object GetCampCollectByUser()
        {
            bool flag = true;
            List<camp> listcampcollect = null;
            if (_user == null)
            {
                flag = false;
            }
            else
            {
                var camIds = dc.campcollects.Where(c=>c.UserID  == _user.UserID).Select(c=>c.CampID).ToList();
                listcampcollect = dc.camps.Include("Listcampitem").Where(c => camIds.Contains(c.CampID)).ToList();
            }

            return new
            {
                campcollect = listcampcollect,
                loginflag = flag
            };
        }

        public string DeleteCampCollect(int CampID)
        {
            string strRet = "1";
            try
            {
                if (_user == null)
                {
                    strRet = "-1";
                }
                else
                {
                   campcollect coll = dc.campcollects.FirstOrDefault(c => c.UserID == _user.UserID && c.CampID == CampID);
                   if (coll != null)
                   {
                       dc.campcollects.Remove(coll);
                       dc.SaveChanges();
                   }
                }

                return strRet;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int GetUseIntegralNumber()
        {
            DC dc = DCLoader.GetMyDC();
            int? num = 0;
            if (_user != null)
            {
                num = dc.UserIntegralHistorys.Where(c => c.UserID == _user.UserID && c.SpentIntegral < 0).Sum(c=>c.SpentIntegral);
            }
            return (num.HasValue ? num.Value : 0);
        }

        public object GetIntegralList(int page, int limit)
        {
            DC dc = DCLoader.GetMyDC();
            IEnumerable<UserIntegralHistory> lstEF = null;
            bool userflag = true;
            if (_user == null)
            {
                userflag = false;
            }
            else
            {
                lstEF = dc.UserIntegralHistorys.Where(c => c.UserID == _user.UserID).OrderByDescending(c=>c.HappenedDateTime);
            }

            return GetIntegralListObj(lstEF, page, limit, userflag);
        }

        public object GetIntegralListObj(IEnumerable<UserIntegralHistory> lstEF, int page, int limit, bool userflag)
        {
            if (userflag)
            {
                IEnumerable<UserIntegralHistory> res = lstEF;
                if (page > 0)
                {
                    int skipPages = page - 1;
                    res = lstEF.Skip(skipPages * limit);
                }
                if (limit > 0)
                {
                    res = res.Take(limit);
                }
                int count = lstEF.Count();
                return new
                {
                    total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                    page = page,
                    records = count,
                    rows = res.ToList(),
                    loginflag = userflag
                };
            }
            return new
            {
                userflag = userflag
            };
            
        }

        public int CreateUser(User user)
        {
            var userCheck = dc.Users.FirstOrDefault(c=>c.UserName == user.UserName|| c.Email == user.Email);
            if (userCheck != null)
            {
                return -1;
            }
            user.CreateTime = DateTime.Now;
            user.Active = 1;
            dc.Users.Add(user);
            dc.SaveChanges();
            UserIntegralHistory newUserHistory = new UserIntegralHistory();
            newUserHistory.UserID = user.UserID;
            newUserHistory.HappenedDateTime = DateTime.Now;
            newUserHistory.SpentIntegral = 500;
            return user.UserID;
        }
        public bool PassValidate(int userId)
        {
            try
            {
                var user = dc.Users.FirstOrDefault(m => m.UserID == userId);
                user.IDNumberFlag = 1;
                dc.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int DeleteUser(int userId)
        {
            try
            {
                var campManaged = dc.approvalcamps.FirstOrDefault(c => c.ManagedByID == userId);
                if (campManaged == null)
                {
                    var user = dc.Users.FirstOrDefault(m => m.UserID == userId);
                    user.Active = 0;
                    dc.SaveChanges();
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception ex)
            {
                return 3;
            }
        }
        public void SaveUserHeadPhoto(string fileName)
        {
            var u = dc.Users.FirstOrDefault(c => c.UserID == _user.UserID);
            u.HeadPhoto = fileName;
            dc.SaveChanges();
        }

        public string saveUserBasicInfo(UserModel userModel)
        {
            try
            {
                var u = dc.Users.FirstOrDefault(c => c.UserID == userModel.UserID);
                u.Sex = userModel.Sex;
                u.Birth = userModel.Birth;
                u.Intro = userModel.Intro;
                dc.SaveChanges();
                return "True";
            }
            catch (Exception ex)
            {
                return "False";
            }
        }
        public int SaveUserInfo(UserModel userModel)
        {
            try
            {
                User user = dc.Users.FirstOrDefault(m => m.UserID == userModel.UserID);
                if (user != null)
                {
                    ModelConverter.Convert<UserModel, User>(userModel, user);
                }
                dc.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public string saveUserAuthInfo(UserModel userModel)
        {
            try
            {
                var u = dc.Users.FirstOrDefault(c => c.UserID == userModel.UserID);
                if (!u.MailFlag.HasValue || u.MailFlag.Value == 0)
                {
                    u.Email = userModel.Email;
                }
                if (!u.MobileFlag.HasValue || u.MobileFlag.Value == 0)
                {
                    u.Mobile = userModel.Mobile;
                }
                dc.SaveChanges();
                return "True";
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

        public string saveIDNumberInfo(UserModel userModel)
        {
            try
            {
                var u = dc.Users.FirstOrDefault(c => c.UserID == userModel.UserID);
                if (!u.IDNumberFlag.HasValue || u.IDNumberFlag.Value == 0)
                {
                    u.IDNumber = userModel.IDNumber;
                    u.Name = userModel.Name;
                    if (userModel.IDNumberImg1 != null && userModel.IDNumberImg1.IndexOf("TempFile") > -1)
                    {
                        u.IDNumberImg1 = userModel.IDNumberImg1.Replace("TempFile\\", "");
                    }
                    if (userModel.IDNumberImg2 != null && userModel.IDNumberImg2.IndexOf("TempFile") > -1)
                    {
                        u.IDNumberImg2 = userModel.IDNumberImg2.Replace("TempFile\\", "");
                    }
                    u.IDNumberFlag = 0;
                    dc.SaveChanges();
                }
                return "True";
            }
            catch (Exception ex)
            {
                return "False";
            }
        }

        public object GetUserAllInfo()
        {
            bool flag = true;
            UserModel usermodel = new UserModel();
            if (_user == null)
            {
                flag = false;
            }
            else
            {
                User user = dc.Users.FirstOrDefault(c => c.UserID == _user.UserID);
                if (user != null)
                {
                    ModelConverter.Convert<User, UserModel>(user, usermodel);
                }
            
            }

            return new
            {
                usermodel = usermodel,
                loginflag = flag
            };
        }
        public object GetAllUserForList(int page, int limit, bool includeCurrentUser = false)
        {
            IEnumerable<User> lstEF = dc.Users.OrderBy(m=>m.IDNumberFlag);
            if (includeCurrentUser == false)
            {
                lstEF = dc.Users.Where(m => m.UserID != _user.UserID);
            }
            IEnumerable<User> res = lstEF;
             if (page > 0)
            {
                int skipPages = page - 1;
                res = lstEF.Skip(skipPages * limit);
            }
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            int count = lstEF.Count();
            return new
            {
                total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                page = page,
                records = count,
                rows = res.ToList(),
            };
        }
        public object GetAllCampList(int page, int limit)
        {
            IEnumerable<approvalcamp> lstEF = dc.approvalcamps;
            IEnumerable<approvalcamp> res = lstEF;
            if (page > 0)
            {
                int skipPages = page - 1;
                res = lstEF.Skip(skipPages * limit);
            }
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            int count = lstEF.Count();
            return new
            {
                total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                page = page,
                records = count,
                rows = res.ToList(),
            };
        }
        public List<User> GetAllManagerList()
        {
            var managerList = dc.Users.Where(m=>m.UserType == "3").ToList();
            return managerList;
        }
        public int ChooseManager(WebModel.ApprovalCamp.approvalcampModel approvedCamp)
        {
            try
            {
                var campInDb = dc.approvalcamps.FirstOrDefault(m => m.CampID == approvedCamp.CampID);
                campInDb.ManagedByID = approvedCamp.ManagedByID;
                campInDb.ManagedByName = approvedCamp.ManagedByName;
                dc.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public User GetUserInfo(int userId)
        {
            User user = dc.Users.FirstOrDefault(c => c.UserID == userId);
            return user;
        }
    }
}

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

        public void SaveUserHeadPhoto(string fileName)
        {
            var u = dc.Users.FirstOrDefault(c => c.UserID == _user.UserID);
            u.HeadPhoto = fileName;
            dc.SaveChanges();
        }
    }
}

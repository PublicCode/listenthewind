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

namespace DataAccessLayer
{
    public class UserManager : BaseManager, IUserManager
    {
        protected User _user;
        public UserManager(User user)
        {
            _user = user;
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


    }
}

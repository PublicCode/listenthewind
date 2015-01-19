using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using IDataAccessLayer;
using DataAccess.DC;
using WebModel.Camp;
using ComLib.Extension;

namespace BizLogic
{
    public class UserBizLogic
    {
        private IUserManager userBase;

        public UserBizLogic(User user = null)
        {
            userBase = new UserManager(user);
        }

        public object GetCampCollectByUser()
        {
            return userBase.GetCampCollectByUser();
        }

        public string DeleteCampCollect(int CampID)
        {
            return userBase.DeleteCampCollect(CampID);
        }

        public object GetIntegralList(int page, int limit)
        {
            return userBase.GetIntegralList(page, limit);
        }

        public int GetUseIntegralNumber()
        {
            return userBase.GetUseIntegralNumber() * -1;
        }

        public void SaveUserHeadPhoto(string fileName)
        {
            userBase.SaveUserHeadPhoto(fileName);
        }
    }
}

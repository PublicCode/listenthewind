using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using IDataAccessLayer;
using DataAccess.DC;
using WebModel.Camp;
using ComLib.Extension;
using WebModel.Account;

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

        public string saveUserBasicInfo(UserModel userModel)
        {
            return userBase.saveUserBasicInfo(userModel);
        }

        public string saveUserAuthInfo(UserModel userModel)
        {
            return userBase.saveUserAuthInfo(userModel);
        }

        public string saveIDNumberInfo(UserModel userModel)
        {
            return userBase.saveIDNumberInfo(userModel);
        }

        

        public object GetUserAllInfo()
        {
            return userBase.GetUserAllInfo();
        }
    }
}

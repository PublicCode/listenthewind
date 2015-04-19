using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Account;
using WebModel.Camp;

namespace IDataAccessLayer
{
    public interface IUserManager
    {
        object GetCampCollectByUser();
        string DeleteCampCollect(int CampID);
        object GetIntegralList(int page, int limit);
        int GetUseIntegralNumber();
        void SaveUserHeadPhoto(string fileName);
        string saveUserBasicInfo(UserModel userModel);
        object GetUserAllInfo();
        string saveUserAuthInfo(UserModel userModel);
        string saveIDNumberInfo(UserModel userModel);

        object GetAllUserForList(int page, int limit);
        User GetUserInfo(int userId);

        int CreateUser(User user);

        bool PassValidate(int userId);

        int DeleteUser(int userId);

        object GetAllCampList(int page, int limit);

        List<User> GetAllManagerList();

        int ChooseManager(WebModel.ApprovalCamp.approvalcampModel approvedCamp);
    }
}

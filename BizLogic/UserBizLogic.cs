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
using WebModel.ApprovalCamp;

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

        public UserModel GetUserInfo(int userId)
        {
            User user = userBase.GetUserInfo(userId); 
            UserModel userModel = new UserModel();
            ModelConverter.Convert<User, UserModel>(user, userModel);
            if (user.UserType == "0")
            {
                userModel.UserTypeShow = "一般用户";
            }
            else if (user.UserType == "1")
            {
                userModel.UserTypeShow = "营主";
            }
            else if (user.UserType == "2")
            {
                userModel.UserTypeShow = "营长";
            }
            else if (user.UserType == "3")
            {
                userModel.UserTypeShow = "管理员";
            }
            return userModel;
        }
        
        public object GetUserAllInfo()
        {
            return userBase.GetUserAllInfo();
        }

        public object GetAllUserForList(int page, int limit)
        {
            return userBase.GetAllUserForList(page, limit);
        }

        public int CreateUser(UserModel userModel)
        {
            User user = new User();
            ModelConverter.Convert<UserModel, User>(userModel, user);
            return userBase.CreateUser(user);
        }

        public bool PassValidate(int userId)
        {
            return userBase.PassValidate(userId); 
        }

        public int  DeleteUser(int userId)
        {
            return userBase.DeleteUser(userId); 
        }

        public object GetAllCampList(int page, int limit)
        {
            return userBase.GetAllCampList(page, limit);
        }

        public List<UserModel> GetAllManagerList()
        {
            List<UserModel> managerList = new List<UserModel>();
            var managerInDb = userBase.GetAllManagerList();
            foreach(User user in managerInDb)
            {
                UserModel userModel = new UserModel();
                ModelConverter.Convert<User, UserModel>(user, userModel);
                managerList.Add(userModel);
            }
            return managerList;
        }

        public int ChooseManager(approvalcampModel approvedCamp)
        {
            return userBase.ChooseManager(approvedCamp);
        }

        public int SaveUserInfo(UserModel userModel)
        {
            return userBase.SaveUserInfo(userModel);
        }
    }
}

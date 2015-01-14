using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.DC;
namespace IDataAccessLayer
{
    public interface IAccountManager
    {
        int ValidateUser(string userName, string passWord, bool fromMobile = false);

        User GetUserByName(string userName);

        string GetCurrentUserName();

        string GetCurrentUserID();

        string GetCurrentDisplayName();

        User GetUserById(int id);

        User CreateUser(User uInfo);

        User GetUserByEmail(string email);

        void UpdatePwd(string email, string pwd);
    }
}

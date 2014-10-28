using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDataAccessLayer;
using DataAccessLayer;
using DataAccess.DC;
namespace BizLogic
{
    public class AccountHelper
    {
        public int UserLogon(string userId, string passWord, bool fromMobile = false)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.ValidateUser(userId, passWord, fromMobile);
         
        }

        public User GetUserByName(string userName)
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetUserByName(userName);
        }


        public string GetCurrentUserName()
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetCurrentUserName();
        }

        public string GetCurrentUserID()
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetCurrentUserID();
        }


        public string GetCurrentDisplayName()
        {
            IAccountManager IAccount = new AccountManager();
            return IAccount.GetCurrentDisplayName();
        }
    }
}

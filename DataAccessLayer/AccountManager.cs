﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using DataAccess.DC;

using IDataAccessLayer;
using System.Data.Entity;
using System.Web;
using System.Collections.ObjectModel;


namespace DataAccessLayer
{
    public class AccountManager : IAccountManager
    {
        public DC dcObj;
        /// <summary>
        /// validate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>1:ok,2:pwd error,3:no user</returns>
        public int ValidateUser(string userName, string passWord, bool fromMobile = false)
        {
            var user = new User();
            dcObj = DCLoader.GetMyDC();
            user = dcObj.Users.FirstOrDefault(c => c.UserName== userName && c.Active == 1);
            if (user != null && user.Pwd == passWord)
            {
                if (!fromMobile)
                    return 1;
                else
                    return 4;
            }
            else if (user != null)
            {
                return 2;
            }
            return 3;
        }
        
        public User GetUserByName(string userName)
        {
            User user = new User();
            dcObj = DCLoader.GetMyDC();
            user = dcObj.Users.FirstOrDefault(c => c.UserName == userName);
            return user;

        }

        public User GetUserById(int id)
        {
            dcObj = DCLoader.GetMyDC();
            var user = dcObj.Users.FirstOrDefault(u=>u.UserID == id);
            return user;
        }

        public User GetUserByEmail(string strEmail)
        {
            User user = new User();
            dcObj = DCLoader.GetMyDC();
            user = dcObj.Users.FirstOrDefault(u => u.Email == strEmail);
            return user;
        }

        public string GetCurrentUserID()
        {
            if (HttpContext.Current.Request.Cookies["UserID"] != null)
            {
                return HttpContext.Current.Request.Cookies["UserID"].Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetCurrentUserName()
        {
            if (HttpContext.Current.Request.Cookies["UserName"] != null)
            {
                return HttpContext.Current.Request.Cookies["UserName"].Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetCurrentDisplayName()
        {
            if (HttpContext.Current.Request.Cookies["UserID"] != null)
            {
                int intCurrentUserID = int.Parse(GetCurrentUserID());
                User user = new User();
                dcObj = DCLoader.GetMyDC();
                user = dcObj.Users.FirstOrDefault(c => c.UserID == intCurrentUserID);
                return user.Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public User CreateUser(User uInfo)
        {
            dcObj = DCLoader.GetMyDC();
            dcObj.Users.Add(uInfo);
            dcObj.SaveChanges();
            return uInfo;
        }
        public void UpdatePwd(string email, string pwd)
        {
            dcObj = DCLoader.GetMyDC();
            var info = dcObj.Users.FirstOrDefault(c => c.Email == email);
            info.Pwd = pwd;
            dcObj.SaveChanges();
        }
    }
}

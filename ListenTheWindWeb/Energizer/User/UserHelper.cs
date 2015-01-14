using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccess.DC;
using DataAccessLayer;
using Newtonsoft.Json;

namespace Web.Energizer.User
{
    public static class UserHelper
    {
        public static DataAccess.DC.User CurrentUser
        {
            get
            {
                var user = HttpContext.Current.Session["User"] as DataAccess.DC.User;
                if (user != null)
                {


                    return user;
                }


                if (HttpContext.Current.Session["User"] == null &&
                    HttpContext.Current.User.Identity.IsAuthenticated)
                {
                }
                return user;
            }
        }
        public static string getCurrentUser
        {
            get
            {
                var user = HttpContext.Current.Session["User"] as DataAccess.DC.User;
                if (user != null)
                {
                    return JsonConvert.SerializeObject(user);
                }
                return JsonConvert.SerializeObject(new WebModel.Account.UserModel());
            }
        }
    }
}
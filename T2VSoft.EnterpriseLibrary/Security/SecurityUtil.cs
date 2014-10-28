using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web;
using T2VSoft.EnterpriseLibrary.Common;
using T2VSoft.EnterpriseLibrary.Data;
namespace T2VSoft.EnterpriseLibrary.Security
{
    public class SecurityUtil
    {
        public static string GetPassword(string userName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = @"SELECT password FROM t_security_user WHERE LoginName=@LoginName OR Email=@Email";
            try
            {
                using (DbCommand command = db.GetSqlStringCommand(sql))
                {
                    db.AddInParameter(command, "@LoginName", DbType.String, userName.Trim());
                    db.AddInParameter(command, "@Email", DbType.String, userName.Trim());
                    return DataConvert.ConvertToTrimString(db.ExecuteScalar(command));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetEmailAddress(string userName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string sql = @"SELECT email FROM t_security_user WHERE LoginName=@LoginName OR Email=@Email";
            try
            {
                using (DbCommand command = db.GetSqlStringCommand(sql))
                {
                    db.AddInParameter(command, "@LoginName", DbType.String, userName.Trim());
                    db.AddInParameter(command, "@Email", DbType.String, userName.Trim());
                    return DataConvert.ConvertToTrimString(db.ExecuteScalar(command));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Authenticate(string username)
        {
            return Authenticate(username, false);
        }
        public static bool Authenticate(string username, bool persist)
        {
            if (ValidateUser(username))
            {
                SetTicket(username, persist);
                return true;
            }
            return false;
        }
        public static bool ValidateUser(string loginName)
        {
            string sql = @"SELECT count(*) FROM t_security_user WHERE Email = @LoginName";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(command, "@LoginName", DbType.String, loginName);
                int count = Convert.ToInt32(db.ExecuteScalar(command));

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static void SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
        }
        private static void SetTicket(string username, bool persist)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(username, persist);
        }

        public static bool ValidateUser(string loginName, string password)
        {
            string sql = @"SELECT count(*) FROM [user] WHERE UserName = @LoginName AND Password = @Password";
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                db.AddInParameter(command, "@LoginName", DbType.String, loginName);
                db.AddInParameter(command, "@PassWord", DbType.String, password);
                int count = Convert.ToInt32(db.ExecuteScalar(command));

                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
       
        
    }
}

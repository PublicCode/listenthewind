using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.DC;
using System.Configuration;

namespace BizLogic
{
    public class BaseBizLogic
    {
        protected User u;
        protected string siteType;
        public BaseBizLogic(User u)
        {
            this.u = u;
            this.siteType = ConfigurationManager.AppSettings["EnvType"].ToString();
        }
        public BaseBizLogic()
        {
            this.siteType = ConfigurationManager.AppSettings["EnvType"].ToString();
        }
    }
}

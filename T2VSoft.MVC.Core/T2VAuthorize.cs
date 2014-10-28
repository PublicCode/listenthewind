using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace T2VSoft.MVC.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class T2VAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            OnAuthorizationHelp(filterContext);
        }

        internal void OnAuthorizationHelp(AuthorizationContext filterContext)
        {
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}

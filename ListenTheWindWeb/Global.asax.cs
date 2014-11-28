using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataAccess.DC;
using ComLib.Mail;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using HDS.QMS.Energizer.SignalRs;
using System.Web.Security;
using T2VSoft.MVC.Core;

namespace HDS.QMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{*id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonDotNetValueProviderFactory());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

        }
        protected void Application_BeginRequest()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            int sendPort = int.Parse(ConfigurationManager.AppSettings["SendPort"]);
            string useSSL = ConfigurationManager.AppSettings["usessl"].ToString();
            string userName = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            var mailservice = new MailServiceFactory(hostName,sendPort, port, useSSL,userName,password);
            HttpContext.Current.Items["MailService"] = mailservice;
        }

        protected void Application_EndRequest()
        {

        }

        protected void Session_Start()
        {
            //HttpContext.Current.Session["ReleaseNoteFlag"] = false;
        }
    }
}
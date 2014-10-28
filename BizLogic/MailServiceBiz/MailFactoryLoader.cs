using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Mail;
using System.Configuration;
using System.Web;
using System.Threading;

namespace BizLogic.MailServiceBiz
{
    public static class MailFactoryLoader
    {
        public const string mailstyle = "<div><p class=\"small grey\" style=\"color: grey; line-height: 16px; font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 12px; margin-top: 0px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px;\">Reply above the line</p></div><div style= \"width: 100%; padding-top: 15px; padding-right: 0px; padding-bottom: 15px; padding-left: 0px; margin-top: 19px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px; border-top-color: #cccccc; border-bottom-color: #cccccc; border-top-width: 1px; border-bottom-width: 1px; border-top-style: solid; border-bottom-style: solid; background-color: rgb(255, 255, 255);\"><span style = \"text-align: left; color: #000000; line-height: 19px; font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 14px; vertical-align: top; background-color: rgb(255, 255, 255);\">";
        public const string mailstyleocz = "<div style= \"width: 100%; padding-top: 15px; padding-right: 0px; padding-bottom: 15px; padding-left: 0px; margin-top: 19px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px; background-color: rgb(255, 255, 255);\"><span style = \"text-align: left; color: #000000; line-height: 19px; font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 14px; vertical-align: top; background-color: rgb(255, 255, 255);\">";
        public const string maildeclare = "</div><div style =\"color: grey; line-height: 16px; font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 12px; margin-top: 0px; margin-right: 0px; margin-bottom: 8px; margin-left: 0px;\"><span>You can reply this email to add message for this quote, text max length limit to 140, the rest will be truncated.</span><div>";
        public static MailServiceFactory GetNewMailService()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            int sendPort = int.Parse(ConfigurationManager.AppSettings["SendPort"]);
            string useSSL = ConfigurationManager.AppSettings["usessl"].ToString();
            string userName = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            var mailservice = new MailServiceFactory(hostName, sendPort, port, useSSL, userName, password);
            return mailservice;
        }

        public static MailServiceFactory GetMyMailService()
        {
            var mailservice = HttpContext.Current == null
                        ? GetNewMailService()
                        : (HttpContext.Current.Items["MailService"] == null ? GetNewMailService() : HttpContext.Current.Items["MailService"] as MailServiceFactory);
            return mailservice;
        }

        public static void SendEmail(object mailObj)
        {
            MailServiceFactory mailservice = GetMyMailService();
            mailservice.Send((MailObject)mailObj);
        }
        public static void SendEmailNewThread(List<MailObject> mailObjs)
        {
            foreach (MailObject mailObj in mailObjs)
            {
                ParameterizedThreadStart ParStart = new ParameterizedThreadStart(MailFactoryLoader.SendEmail);
                Thread myThread = new Thread(ParStart);
                myThread.Start(mailObj);
            }
        }
        public static void SendEmailNewThread(object mailObj)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(MailFactoryLoader.SendEmail);
            Thread myThread = new Thread(ParStart);
            myThread.Start(mailObj);
        }

    }
}

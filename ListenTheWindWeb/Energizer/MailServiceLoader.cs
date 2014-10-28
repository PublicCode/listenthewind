using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComLib.Mail;
using System.Configuration;
using System.Threading;
using DataAccess.DC;
using System.Text;

namespace HDS.QMS.Energizer
{
    public static class MailFactoryLoader
    {
        public const string mailstyle = "<div><p class=\"small grey\" style=\" line-height: 16px;font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 12px; margin-top: 0px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px;\">Reply above the line</p></div><div style= \"width: 100%; padding-top: 15px; padding-right: 0px; padding-bottom: 15px; padding-left: 0px; margin-top: 19px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px; border-top-color: #cccccc; border-bottom-color: #cccccc; border-top-width: 1px; border-bottom-width: 1px; border-top-style: solid; border-bottom-style: solid; background-color: rgb(255, 255, 255);\"><span style = \"text-align: left; color: #000000; line-height: 19px;font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 14px; vertical-align: top; background-color: rgb(255, 255, 255);\">";
        public const string mailstyleocz = "<div style= \"width: 100%; padding-top: 15px; padding-right: 0px; padding-bottom: 15px; padding-left: 0px; margin-top: 19px; margin-right: 0px; margin-bottom: 19px; margin-left: 0px; background-color: rgb(255, 255, 255);\"><span style = \"text-align: left; color: #000000; line-height: 19px;font-family: 'Helvetica Neue', helvetica, sans-serif; font-size: 14px; vertical-align: top; background-color: rgb(255, 255, 255);\">";
        public const string maildeclare = "</div><div style ='font-family: Helvetica Neue, helvetica, sans-serif; font-size: 14px;'><span>Accept, reject or request a modification to the request by replying to this email and adding a hashtag of #accept, #reject or #modify at the end of the subject.</span><div><br /><br />";
        
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
                        : (HttpContext.Current.Items["MailService"] == null ? GetNewMailService(): HttpContext.Current.Items["MailService"] as MailServiceFactory);
            return mailservice;
        }

        public static void SendEmail(object mailObj)
        {
            MailServiceFactory mailservice = GetMyMailService();
            mailservice.Send((MailObject)mailObj);
        }
        public static void SendEmailNewThread(List<MailObject> mailObjs)
        {
            string sender = ConfigurationManager.AppSettings["mailaddress"].ToString();
            foreach (MailObject mailObj in mailObjs)
            {
                mailObj.MailSender = sender;
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
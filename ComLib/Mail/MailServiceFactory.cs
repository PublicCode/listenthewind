using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPop.Pop3;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web;
using OpenPop.Mime.Header;
using OpenPop.Mime;

namespace ComLib.Mail
{
    /// <summary>
    /// Mail service to send and read mail.
    /// </summary>
    public class MailServiceFactory : IMailService
    {
        public string HostName { get; set; }
        public int Post { get; set; }
        public int SendPort { get; set; }
        public string UseSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MailServiceFactory()
        {
        }
        public MailServiceFactory(string hostName, int sendPort, int post, string useSSL, string userName, string password)
        {
            this.HostName = hostName; this.SendPort = SendPort; this.Post = post; this.UseSSL = useSSL; this.UserName = userName; this.Password = password;
        }
        /// <summary>
        /// Function to send email.
        /// </summary>
        /// <param name="mailObj">Mail object, with subject, receiver, sender and body.</param>
        public void Send(MailObject mailObj)
        {
            try
            {
                SmtpClient smtp = new SmtpClient(HostName, SendPort);
                MailMessage message = new MailMessage();
                mailObj.MailBCC = "Faye.Gu@t2vsoft.com";
                message.IsBodyHtml = true;
                message.From = new MailAddress(mailObj.MailSender);
                if (!string.IsNullOrEmpty(mailObj.MailAttachment))
                {
                    Attachment attachment = new Attachment(mailObj.MailAttachment);
                    message.Attachments.Add(attachment);
                }
                if (mailObj.MailReceiver.IndexOf(';') != -1)
                {
                    string[] strMailArray = mailObj.MailReceiver.Split(';');
                    foreach (string strEmail in strMailArray)
                    {
                        if (strEmail.Trim() != "" && !message.To.Contains(new MailAddress(strEmail.Trim())))
                        {
                            message.To.Add(new MailAddress(strEmail));
                        }
                    }
                }
                else
                {
                    message.To.Add(new MailAddress(mailObj.MailReceiver));
                }
                
                if (mailObj.MailCC != null && mailObj.MailCC != "")
                {
                    if (mailObj.MailCC.IndexOf(';') != -1)
                    {
                        string[] strMailArray = mailObj.MailCC.Split(';');
                        foreach (string strEmail in strMailArray)
                        {
                            if (strEmail.Trim() != "" && !message.To.Contains(new MailAddress(strEmail.Trim())) && !message.CC.Contains(new MailAddress(strEmail.Trim())))
                            {
                                message.CC.Add(new MailAddress(strEmail));
                            }
                        }
                    }
                    else
                    {
                        if (mailObj.MailCC != "" && !message.To.Contains(new MailAddress(mailObj.MailCC)) && !message.CC.Contains(new MailAddress(mailObj.MailCC)))
                        {
                            message.CC.Add(new MailAddress(mailObj.MailCC));
                        }
                    }
                    
                }
                if (mailObj.MailBCC != null)
                {
                    message.Bcc.Add(new MailAddress(mailObj.MailBCC));
                }
                
                message.Subject = mailObj.MailSubject;
                message.Body = mailObj.MailBody;
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                smtp.Credentials = new NetworkCredential(this.UserName, this.Password);
                smtp.EnableSsl = true;
                smtp.Timeout = 300000;
                smtp.Send(message);
                smtp.Dispose();
                message.Dispose();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        public bool Read()
        {
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(this.HostName, this.SendPort, Boolean.Parse(this.UseSSL));

                // Authenticate ourselves towards the server
                client.Authenticate(this.UserName, this.Password);

                int messageCount = client.GetMessageCount();

                // Run trough each of these messages and download the headers
                for (int messageItem = messageCount; messageItem > 0; messageItem--)
                {
                    MessageHeader headers = client.GetMessageHeaders(messageItem);
                    RfcMailAddress from = headers.From;
                    Message msg = client.GetMessage(messageItem);
                }
                return true;
            }
        }
    }
}

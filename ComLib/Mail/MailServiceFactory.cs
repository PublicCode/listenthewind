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
            this.HostName = hostName; this.SendPort = sendPort; this.Post = post; this.UseSSL = useSSL; this.UserName = userName; this.Password = password;
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
                //mailObj.MailBCC = "Faye.Gu@t2vsoft.com";
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
                smtp.EnableSsl = false;
                smtp.Timeout = 300000;
                smtp.Send(message);
                smtp.Dispose();
                message.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
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
        public void Send_Mail(MailObject mailObj)
        {
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HostName);

                MailAddress from = new MailAddress(UserName, UserName, Encoding.UTF8);

                MailAddress to = new MailAddress(mailObj.MailReceiver, mailObj.MailReceiver, Encoding.UTF8);

                MailMessage message = new MailMessage(from, to);

                message.Subject = mailObj.MailSubject;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Body = mailObj.MailBody;
                message.BodyEncoding = Encoding.UTF8;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = false;

                client.EnableSsl = false;

                client.UseDefaultCredentials = false;
                string username = UserName;
                string passwd = Password;

                NetworkCredential myCredentials = new NetworkCredential(username, passwd);
                client.Credentials = myCredentials;

                client.Timeout = 300000;
                client.Send(message);
                client.Dispose();
                message.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

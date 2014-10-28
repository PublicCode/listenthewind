using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailService.Mail
{
    public class MailObject
    {
        public string MailSender { get; set; }
        public string MailReceiver { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string MailAttachFileName { get; set; }
        public string SaveFileName { get; set; }
    }
}

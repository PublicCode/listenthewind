using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.MailFail
{
   public  class MailFailModel
    {
        public int ID { get; set; }

        public string QuoteNumber { get; set; }

        public string MailSender { get; set; }

        public string ErrorMessage { get; set; }

        public string MailSubject { get; set; }

        public string MailAttathFileName { get; set; }

        public string MailAttathTickName { get; set; }

        public string MailBody { get; set; }
        public string MailType { get; set; }

        public DateTime CreateTime { get; set; }

        public int QuoteExistFlag { get; set; }

        public string TIQ { get; set; }

        public string Description { get; set; }

        public int QuoteID { get; set; }
    }
}

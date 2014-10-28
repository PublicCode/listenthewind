using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class MailFail : ILoggedEntity
    {
        [Key]
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

        long ILoggedEntity.Id
        {
            get { return ID; }
        }

        public string LoggedType { get { return "MailFail"; } }

        [NotMapped]
        public string BatchID { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Mail
{
    public interface IMailService
    {
        void Send(MailObject mailObj);
        bool Read();
    }
}

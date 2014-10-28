using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailService.Mail
{
    public interface IMailService
    {
        void Send(MailObject mailObj);
        List<MailObject> Read(string fileDir, string fileType);
    }
}

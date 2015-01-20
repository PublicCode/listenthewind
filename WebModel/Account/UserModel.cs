using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Account
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public bool errUserName { get; set; }
        public string Pwd { get; set; }
        public bool errPwd { get; set; }
        public string RePwd { get; set; }
        public string Mobile { get; set; }
        public string UserIntegral { get; set; }
        public string Email { get; set; }
        public string ValidCode { get; set; }
        public bool errValidCode { get; set; }

        public string HeadPhoto { get; set; }
        public string Name { get; set; }

        public string Sex { get; set; }

        public string Birth { get; set; }

        public string Intro { get; set; }

        public string UserType { get; set; }

        public int? MailFlag { get; set; }
        public int? MobileFlag { get; set; }

        public string IDNumber { get; set; }


        public string IDNumberImg1 { get; set; }

        public string IDNumberImg2 { get; set; }

        public int? IDNumberFlag { get; set; }

    }
}

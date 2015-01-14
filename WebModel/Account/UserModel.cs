﻿using System;
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
        public string Email { get; set; }
        public string ValidCode { get; set; }
        public bool errValidCode { get; set; }
    }
}
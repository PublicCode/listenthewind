using System;
using System.Collections.Generic;
using System.Text;

namespace T2VSoft.EnterpriseLibrary.Validation
{
    public class ValidRuler
    {
        public ValidRuler(ValidationType validType, string errorInfo)
        {
            this.ValidType = validType;
            this.ErrorInfo = errorInfo;
        }

        public ValidationType ValidType { get; set; }

        public string ValidRegular { get; set; }

        public string ErrorInfo { get; set; }
    }
}

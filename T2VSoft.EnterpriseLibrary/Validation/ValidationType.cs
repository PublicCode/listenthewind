using System;
using System.Collections.Generic;
using System.Text;

namespace T2VSoft.EnterpriseLibrary.Validation
{
    public enum ValidationType
    {
        Required,
        Number,
        Plus,
        PlusIntegerNotZero,
        DateTimeAfterNow,
        DateTimeBeforeNow,
        EmptyOrPlusIntegerNotZero,
        Email,
        MulitEmail,
        RequiredDropDown,
        RequiredDropDownEmail
    }
}

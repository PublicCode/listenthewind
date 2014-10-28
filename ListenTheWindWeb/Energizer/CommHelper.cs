using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDS.QMS.Energizer
{
    public static class CommHelper
    {
        public static string GetFiscalQtr(DateTime date)
        {
            string qtr = string.Empty;
            if (3 <= date.Month && date.Month <= 5)
                qtr = "Q01";
            else if (6 <= date.Month && date.Month <= 8)
                qtr = "Q02";
            else if (9 <= date.Month && date.Month <= 11)
                qtr = "Q03";
            else if (12 <= date.Month || (1 <= date.Month && date.Month <= 2))
                qtr = "Q04";
            return qtr;
        }
    }
}
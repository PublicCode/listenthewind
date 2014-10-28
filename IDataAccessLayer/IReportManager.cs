using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDataAccessLayer
{
    public interface IReportManager
    {
        string GetSubjectAndSearchFormData();
        object GetReport(string strSubject,Dictionary<string,string> ctrlValue, string username, string permissions, int currentPageIndex, int pageSize);
    }
}

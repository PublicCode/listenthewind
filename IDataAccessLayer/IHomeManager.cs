using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDataAccessLayer
{
    public interface IHomeManager
    {
        string GetNewNumber(string TypeName,string strFirstChar);
        List<UserOperationLog> GetOperationLogList(string[] objectType, string objectValue);
        string TestMysqlDB();
    }
}

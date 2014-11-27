using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Camp;

namespace IDataAccessLayer
{
    public interface IHomeManager
    {
        string GetNewNumber(string TypeName,string strFirstChar);
        List<UserOperationLog> GetOperationLogList(string[] objectType, string objectValue);
        campModel GetCamp(int CampID);
        bool CheckCampCollect(int CampID);
        string AddCampCollect(int CampID);
    }
}

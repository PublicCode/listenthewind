using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using IDataAccessLayer;
using DataAccess.DC;
using WebModel.Camp;

namespace BizLogic
{
    public class HomeBizLogic
    {
         private IHomeManager homeBase;

         public HomeBizLogic()
        {
            homeBase = new HomeManager();
        }

         public string GetNewNumber(string TypeName, string strFirstChar)
         {
             return homeBase.GetNewNumber(TypeName, strFirstChar);
         }
         public List<UserOperationLog> GetUserOperationList(string[] objectTypes, string objectValue)
         {
             return homeBase.GetOperationLogList(objectTypes, objectValue);
         }

         public campModel GetCamp(int CampID)
         {
             return homeBase.GetCamp(CampID);
         }

         public string CampCollect(int CampID)
         {
             if (homeBase.CheckCampCollect(CampID))
             {
                 return homeBase.AddCampCollect(CampID);
             }
             else
             {
                 return "已经添加收藏";
             }
         }
    }
}

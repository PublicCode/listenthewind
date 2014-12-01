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

        public campModel GetCamp(int CampID, DateTime? dt)
        {
            return homeBase.GetCamp(CampID, dt);
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
        public List<string> getListOfReserveDate(int PileId)
        {
            return homeBase.GetListOfReserveForPile(PileId);
        }

        public string SaveReserve(List<DateTime> SelectedDate, List<int> SelectedItemId, int CampID, int PileID)
        {
            return homeBase.SaveReserve(SelectedDate, SelectedItemId, CampID, PileID);
        }

        public List<CityModel> GetCitys()
        {
            return homeBase.GetCitys();
        }
        public object GetCampList(CampListSeachModel info, int page, int limit)
        {
            return homeBase.GetCampList(info, page, limit);
        }
        public List<basicdatacollect> GetBasicData()
        {
            return homeBase.GetBasicData();
        }
    }
}

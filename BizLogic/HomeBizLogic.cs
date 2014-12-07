using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using IDataAccessLayer;
using DataAccess.DC;
using WebModel.Camp;
using ComLib.Extension;

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
        public List<string> getListOfReserveDate(int PileId, int userId)
        {
            return homeBase.GetListOfReserveForPile(PileId, userId);
        }

        public string SaveReserve(List<DateTime> SelectedDate, List<camppriceModel> listOfCampPrice, int CampID, int PileID)
        {
            return homeBase.SaveReserve(SelectedDate, listOfCampPrice, CampID, PileID);
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

        public string SaveComments(campcommentModel campCommentModel)
        {
            return homeBase.SaveComments(campCommentModel);
        }
        public List<basidatacollectforcampModel> GetBasicDataForCamp()
        {
            var basicData = homeBase.GetBasicData();
            var listOfBasicDataForCamp = new List<basidatacollectforcampModel>();
            foreach (basicdatacollect basic in basicData)
            {
                basidatacollectforcampModel basicModal = new basidatacollectforcampModel();
                ModelConverter.Convert<basicdatacollect, basidatacollectforcampModel>(basic, basicModal);
                listOfBasicDataForCamp.Add(basicModal);
            }
            return listOfBasicDataForCamp;
        }
    }
}

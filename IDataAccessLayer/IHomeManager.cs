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
        campModel GetCamp(int CampID, DateTime? dt);
        bool CheckCampCollect(int CampID);
        string AddCampCollect(int CampID);
        List<string> GetListOfReserveForPile(int PileId, int userID);
        string SaveReserve(List<DateTime> SelectedDate, List<camppriceModel> SelectedItem, int CampID, int PileID);
        List<CityModel> GetCitys();
        object GetCampList(CampListSeachModel info, int page, int limit);
        List<basicdatacollect> GetBasicData();
        object SaveComments(int CampID, string CommentCon);
    }
}

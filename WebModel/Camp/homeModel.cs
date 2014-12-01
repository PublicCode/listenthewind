using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.DC;

namespace WebModel.Camp
{
    public class homeModel
    {

    }
    public class CityModel
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public List<CityLocationModel> Locations { get; set; }
    }
    public class CityLocationModel
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int CityID { get; set; }
        public static CityLocationModel FromEFToDTO(CityLocation ef)
        {
            var dto = new CityLocationModel
            {
                LocationID = ef.LocationID,
                LocationName = ef.LocationName,
                CityID = ef.CityID
            };
            return dto;
        }
    }
    public class CampListSeachModel
    {
        public int LocationID { get; set; }
        public string JoinCampDate { get; set; }
        public DateTime DBJoinCampDate { get { return string.IsNullOrEmpty(JoinCampDate) ? DateTime.Now : Convert.ToDateTime(JoinCampDate); } }
        public string CampLOD { get; set; }
        public int? PriceStart { get; set; }
        public int? PriceEnd { get; set; }
        public List<int> SpecialContents { get; set; }
        public List<int> CampType { get; set; }
        public List<int> HostLang { get; set; }
        public string KeyContent { get; set; }
    }
}

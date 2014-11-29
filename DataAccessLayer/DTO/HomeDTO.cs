using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.DC;

namespace DataAccessLayer.DTO
{
    public class HomeDTO
    {

    }
    public class CityDTO
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public List<CityLocationDTO> Locations { get; set; }
    }
    public class CityLocationDTO
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int CityID { get; set; }
        public static CityLocationDTO FromEFToDTO(CityLocation ef) {
            var dto = new CityLocationDTO {
                LocationID = ef.LocationID,
                LocationName = ef.LocationName,
                CityID = ef.CityID
            };
            return dto;
        }
    }
    public class CampListSeachDTO
    {
        public int LocationID { get; set; }
        public string JoinCampDate { get; set; }
        public DateTime DBJoinCampDate { get { return string.IsNullOrEmpty(JoinCampDate) ? DateTime.Now : Convert.ToDateTime(JoinCampDate); } }
        public int? PriceStart { get; set; }
        public int? PriceEnd { get; set; }
        public List<int> SpecialContents { get; set; }
        public List<int> CampType { get; set; }
        public List<int> HostLang { get; set; }
    }
}

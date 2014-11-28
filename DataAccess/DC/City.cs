using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    [Table("codecity")]
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
    [Table("codecitylocation")]
    public class CityLocation
    {
        [Key]
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int CityID { get; set; }
    }
}

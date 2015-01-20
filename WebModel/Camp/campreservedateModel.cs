using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class campreservedateModel
    {
        public int CampReserveDateID { get; set; }
        public int CampReserveID { get; set; }
        public int CampPileID { get; set; }
        public int CampID { get; set; }
        public DateTime? CampReserveDate { get; set; }
        public string CampReserveDateForDisplay { get; set; }
    }
}

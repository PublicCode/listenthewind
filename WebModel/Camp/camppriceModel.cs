using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class camppriceModel
    {
        public int CampPriceID { get; set; }

        public int CampID { get; set; }

        public string ItemName { get; set; }

        public string ItemUnit { get; set; }
        public decimal? ItemPrice { get; set; }
    }

}

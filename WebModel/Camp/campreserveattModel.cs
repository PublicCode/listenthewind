using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class campreserveattModel
    {
        public int CampReserveAttID { get; set; }

        public int CampReserveID { get; set; }

        public int CampItemID { get; set; }

        public string CampItemName { get; set; }

        public decimal? CampItemUnitPrice { get; set; }

        public decimal? CampItemDiscount { get; set; }

        public decimal? CampItemFinalPrice { get; set; }

        public int? Qty { get; set; }

        public decimal? CampItemPriceAmt { get; set; }
    }
}

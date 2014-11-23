using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DataAccess.DC
{
    public class campreserveatt : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampReserveAttID { get; set; }

        public string LoggedType { get { return "campreserveatt"; } }

        public int CampReserveID { get; set; }

        public int CampItemID { get; set; }

        public string CampItemName { get; set; }

        public decimal? CampItemUnitPrice { get; set; }

        public decimal? CampItemDiscount { get; set; }

        public decimal? CampItemFinalPrice { get; set; }

        public int? Qty { get; set; }

        public decimal? CampItemPriceAmt { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampReserveAttID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}

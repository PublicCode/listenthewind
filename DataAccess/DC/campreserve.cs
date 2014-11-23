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
    public class campreserve : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampReserveID { get; set; }

        public string LoggedType { get { return "campreserve"; } }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public int CampPileID { get; set; }

        public decimal? PilePrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? FinalPilePrice { get; set; }

        public int? Days { get; set; }

        public decimal? PilePriceAmt { get; set; }

        public string ReserveStatus { get; set; }

        public DateTime? Createtime { get; set; }

        public DateTime? PayTime { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampReserveID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}

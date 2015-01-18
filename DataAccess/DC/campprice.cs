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
    public class campprice : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampPriceID { get; set; }

        public string LoggedType { get { return "Camp Price"; } }

        public int CampID { get; set; }

        public string ItemName { get; set; }

        public string ItemUnit { get; set; }
        public decimal? ItemPrice { get; set; }

        public string ItemImage { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampPriceID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}
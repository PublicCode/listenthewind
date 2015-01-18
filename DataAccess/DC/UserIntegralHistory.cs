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
    public class UserIntegralHistory : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int UserIntegralHistoryID { get; set; }

        public string LoggedType { get { return "UserIntegralHistory"; } }

        public string IntegralInfo { get; set; }

        public int? CampID { get; set; }
        public int? SpentIntegral { get; set; }
        public int? CampReserveID { get; set; }
        public int? AdminID { get; set; }
        public int? UserID { get; set; }
        public DateTime? HappenedDateTime { get; set; }


        long ILoggedEntity.Id
        {
            get { return UserIntegralHistoryID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}
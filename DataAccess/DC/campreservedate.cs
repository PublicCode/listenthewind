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
    public class campreservedate : ILoggedEntity
    {
        [Key]
        public int CampReserveDateID { get; set; }

        public string LoggedType { get { return "campreservedate"; } }

        public int CampReserveID { get; set; }
        public int CampPileID { get; set; }
        public int CampID { get; set; }
        public DateTime? CampReserveDate { get; set; }

        

        long ILoggedEntity.Id
        {
            get { return CampReserveDateID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
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
    public class camppile : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int PileID { get; set; }

        public string LoggedType { get { return "camppile"; } }

        public int CampID { get; set; }

        public string PileNumber { get; set; }

        public int? Active { get; set; }

        long ILoggedEntity.Id
        {
            get { return PileID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}
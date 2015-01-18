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
    public class campcollect : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampCollectID { get; set; }

        public string LoggedType { get { return "campcollect"; } }

        public int CampID { get; set; }

        public int UserID { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampCollectID; }
        }

        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
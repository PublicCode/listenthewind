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
    public class campitem : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampItemID { get; set; }

        public string LoggedType { get { return "campitem"; } }

        public int CampID { get; set; }

        public string CampItemName { get; set; }

        public string CampItemIcon { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampItemID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}

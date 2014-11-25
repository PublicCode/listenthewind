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
    public class camphost : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampHostID { get; set; }

        public string LoggedType { get { return "camphost"; } }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public string CampHostPhoto { get; set; }

        public string CampHostIntro { get; set; }

        [ForeignKey("CampHostID")]
        public virtual ICollection<camphostlanguage> Listcamphostlanguage { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampHostID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
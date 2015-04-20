using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DataAccess.DC
{
    public class camptype : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CampTypeID { get; set; }

        public string LoggedType { get { return "camptype"; } }

        public int CampID { get; set; }

        public string CampTypeName { get; set; }

        public int BasicID { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampTypeID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}

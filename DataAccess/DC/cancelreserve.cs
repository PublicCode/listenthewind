using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class cancelreserve : ILoggedEntity
    {
        [Key]
        public int ID { get; set; }
        public int CampReserveID { get; set; }
        public string LoggedType { get { return "cancelreserve"; } }
        public DateTime CancelledOn { get; set; }

        long ILoggedEntity.Id
        {
            get { return ID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}

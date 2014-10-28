using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Extension;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DC
{
    public class TableNumberEnt : ILoggedEntity
    {
        public string LoggedType { get { return "TableNumberEnt"; } }
        [Key]
        public System.Int32 ID { get; set; }
        public System.String Type { get; set; }
        public System.Int32 NumberID { get; set; }
        public System.String FirstChar { get; set; }

        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }


        long ILoggedEntity.Id
        {
            get { return ID; }
        }
    }
}
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

    public class basicdatacollect : ILoggedEntity
    {
        [Key]
        public int BasicDataCollectID { get; set; }

        public string LoggedType { get { return "basicdatacollect"; } }

        public string DataType { get; set; }

        public string DataName { get; set; }

        public string DataIcon { get; set; }

        public int? DataSort { get; set; }

        long ILoggedEntity.Id
        {
            get { return BasicDataCollectID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}

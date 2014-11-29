﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DataAccess.DC
{
    public class camphostlanguage : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampHostLanguageID { get; set; }

        public string LoggedType { get { return "camphostlanguage"; } }

        public int CampHostID { get; set; }

        public string Language { get; set; }

        public int BasicID { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampHostLanguageID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}

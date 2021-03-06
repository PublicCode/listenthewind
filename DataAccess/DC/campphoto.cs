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
    public class campphoto : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CampPhotoID { get; set; }

        public string LoggedType { get { return "campphoto"; } }

        public int CampID { get; set; }

        public string CampPhoteFile { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampPhotoID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }
    }
}
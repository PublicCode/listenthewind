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
    public class campcomment : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampCommentID { get; set; }

        public string LoggedType { get { return "campcomment"; } }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public string CommentCon { get; set; }

        public string CommentRes { get; set; }

        public DateTime? CommentTime { get; set; }

        public DateTime? ResponseTime { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampCommentID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
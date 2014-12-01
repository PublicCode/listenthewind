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
    public class camp : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        [Key]
        public int CampID { get; set; }

        public string LoggedType { get { return "camp"; } }

        public int CampHostID { get; set; }

        public string CampNum { get; set; }

        public string CampName { get; set; }

        public int? CampLevel { get; set; }

        public int? LocID { get; set; }

        public decimal? PilePrice { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string CampPic { get; set; }        

        public string CampAddress { get; set; }

        public string CampIntro { get; set; }

        public string CampPhoto { get; set; }

        public string CampLOD { get; set; }

        public int? Active { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        [ForeignKey("CampID")]
        public virtual ICollection<campcomment> Listcampcomment { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<camphost> Listcamphost { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<campitem> Listcampitem { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<campphoto> Listcampphoto { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<camppile> Listcamppile { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<campprice> Listcampprice { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<camptype> Listcamptype { get; set; }

        long ILoggedEntity.Id
        {
            get { return CampID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}

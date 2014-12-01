using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class campModel
    {
        public int CampID { get; set; }

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

        public string CampLOD { get; set; }

        public int? Active { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public virtual List<campcommentModel> ModelListcampcomment { get; set; }
        public virtual List<camphostModel> ModelListcamphost { get; set; }
        public virtual List<campitemModel> ModelListcampitem { get; set; }
        public virtual List<campphotoModel> ModelListcampphoto { get; set; }
        public virtual List<camppileModel> ModelListcamppile { get; set; }
        public virtual List<camppriceModel> ModelListcampprice { get; set; }
        public virtual List<camptypeModel> ModelListcamptype { get; set; }
    }
}

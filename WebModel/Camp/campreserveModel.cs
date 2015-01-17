using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class campreserveModel
    {

        public string ordernumber { get { return this.CampReserveID.ToString().PadLeft(4, '0'); } }

        public int CampReserveID { get; set; }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public int CampPileID { get; set; }

        public decimal? PilePrice { get; set; }

        public decimal? Discount { get; set; }

        public decimal? FinalPilePrice { get; set; }

        public int? Days { get; set; }

        public decimal? PilePriceAmt { get; set; }

        public int ReserveStatus { get; set; }

        public DateTime Createtime { get; set; }

        public string CreateOn { get { return this.Createtime.ToShortDateString(); } }

        public DateTime? PayTime { get; set; }
        public DateTime? FinishedOn { get; set; }

        public List<campreserveattModel> Listcampreserveatt { get; set; }
        public virtual campInfoModel campInfo { get; set; }

        public string Choosed { get; set; }

        public decimal TotalAmt { get; set; }

    }
    public class campInfoModel
    {
        public int CampId {get;set;}
        public string CampName {get;set;}
        public int PileID {get;set;}
        public string PileNumber {get;set;}
    }
}

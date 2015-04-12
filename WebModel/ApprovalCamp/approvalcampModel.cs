using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.ApprovalCamp
{
    public class approvalcampModel
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
        public string CampPhoto { get; set; }
        public string CampLOD { get; set; }
        public int? Active { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual List<approvalcampcommentModel> ModelListcampcomment { get; set; }
        public virtual List<approvalcamphostModel> ModelListcamphost { get; set; }
        public virtual List<approvalcampitemModel> ModelListcampitem { get; set; }
        public virtual List<approvalcampphotoModel> ModelListcampphoto { get; set; }
        public virtual List<approvalcamppileModel> ModelListcamppile { get; set; }
        public virtual List<approvalcamppriceModel> ModelListcampprice { get; set; }
        public virtual List<approvalcamptypeModel> ModelListcamptype { get; set; }
    }
    public class approvalcampcollectModel
    {
        public int CampCollectID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
    }
    public class approvalcampcommentModel
    {
        public int CampCommentID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string CommentCon { get; set; }
        public string CommentRes { get; set; }
        public DateTime? CommentTime { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
    public class approvalcamphostModel
    {
        public int CampHostID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
        public string CampHostPhoto { get; set; }
        public string CampHostIntro { get; set; }
        public virtual List<approvalcamphostlanguageModel> ModelListcamphostlanguage { get; set; }
    }
    public class approvalcamphostlanguageModel
    {
        public int CampHostLanguageID { get; set; }
        public int CampHostID { get; set; }
        public string Language { get; set; }
        public int BasicID { get; set; }
    }
    public class approvalcampitemModel
    {
        public int CampItemID { get; set; }
        public int CampID { get; set; }
        public string CampItemName { get; set; }
        public string CampItemIcon { get; set; }
        public int CampItemSort { get; set; }
        public int BasicID { get; set; }
    }
    public class approvalcampphotoModel
    {
        public int CampPhotoID { get; set; }
        public int CampID { get; set; }
        public string CampPhoteFile { get; set; }
    }
    public class approvalcamppileModel
    {
        public int PileID { get; set; }
        public int CampID { get; set; }
        public string PileNumber { get; set; }
        public bool Flag { get; set; }
        public int? Active { get; set; }
    }
    public class approvalcamppriceModel
    {
        public int CampPriceID { get; set; }
        public int CampID { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal? ItemPrice { get; set; }
        public string ItemImage { get; set; }
        public int Qty { get; set; }
        public bool Checked { get; set; }
    }
    public class approvalcampreserveModel
    {
        public string ordernumber { get { return this.CampReserveID.ToString().PadLeft(9, '0'); } }
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

        public List<approvalcampreserveattModel> ModelListcampreserveatt { get; set; }
        public List<approvalcampreservedateModel> ModelListcampreservedate { get; set; }
        public virtual approvalCampInfoModel ModelApprovalCampInfo { get; set; }

        public string Choosed { get; set; }
        public decimal TotalAmt { get; set; }
    }
    public class approvalCampInfoModel
    {
        public int CampId { get; set; }
        public string CampName { get; set; }
        public int PileID { get; set; }
        public string PileNumber { get; set; }
        public string CampPhoto { get; set; }
        public string CampIntro { get; set; }
    }
    public class approvalcampreserveattModel
    {
        public int CampReserveAttID { get; set; }
        public int CampReserveID { get; set; }
        public int CampItemID { get; set; }
        public string CampItemName { get; set; }
        public decimal? CampItemUnitPrice { get; set; }
        public decimal? CampItemDiscount { get; set; }
        public decimal? CampItemFinalPrice { get; set; }
        public int? Qty { get; set; }
        public decimal? CampItemPriceAmt { get; set; }
    }
    public class approvalcampreservedateModel
    {
        public int CampReserveDateID { get; set; }
        public int CampReserveID { get; set; }
        public int CampPileID { get; set; }
        public int CampID { get; set; }
        public DateTime? CampReserveDate { get; set; }
        public string CampReserveDateForDisplay { get; set; }
    }
    public class approvalcamptypeModel
    {
        public int CampTypeID { get; set; }
        public int CampID { get; set; }
        public string CampTypeName { get; set; }
        public int BasicID { get; set; }
    }
}

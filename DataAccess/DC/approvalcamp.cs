using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    [Table("approvalcamp")]
    public class approvalcamp
    {
        [Key]
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

        [ForeignKey("CampID")]
        public virtual ICollection<approvalcampcomment> Listapprovalcampcomment { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcamphost> Listapprovalcamphost { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcampitem> Listapprovalcampitem { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcampphoto> Listapprovalcampphoto { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcamppile> Listapprovalcamppile { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcampprice> Listapprovalcampprice { get; set; }
        [ForeignKey("CampID")]
        public virtual ICollection<approvalcamptype> Listapprovalcamptype { get; set; }
    }
    [Table("approvalcampcollect")]
    public class approvalcampcollect
    {
        [Key]
        public int CampCollectID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
    }
    [Table("approvalcampcomment")]
    public class approvalcampcomment
    {
        [Key]
        public int CampCommentID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string CommentCon { get; set; }
        public string CommentRes { get; set; }
        public DateTime? CommentTime { get; set; }
        public DateTime? ResponseTime { get; set; }
    }
    [Table("approvalcamphost")]
    public class approvalcamphost
    {
        [Key]
        public int CampHostID { get; set; }
        public int CampID { get; set; }
        public int UserID { get; set; }
        public string CampHostPhoto { get; set; }
        public string CampHostIntro { get; set; }
        [ForeignKey("CampHostID")]
        public virtual ICollection<approvalcamphostlanguage> Listapprovalcamphostlanguage { get; set; }
    }
    [Table("approvalcamphostlanguage")]
    public class approvalcamphostlanguage
    {
        [Key]
        public int CampHostLanguageID { get; set; }
        public int CampHostID { get; set; }
        public string Language { get; set; }
        public int BasicID { get; set; }
    }
    [Table("approvalcampitem")]
    public class approvalcampitem
    {
        [Key]
        public int CampItemID { get; set; }
        public int CampID { get; set; }
        public string CampItemName { get; set; }
        public string CampItemIcon { get; set; }
        public int CampItemSort { get; set; }
        public int BasicID { get; set; }
    }
    [Table("approvalcampphoto")]
    public class approvalcampphoto
    {
        [Key]
        public int CampPhotoID { get; set; }
        public int CampID { get; set; }
        public string CampPhoteFile { get; set; }
    }
    [Table("approvalcamppile")]
    public class approvalcamppile
    {
        [Key]
        public int PileID { get; set; }
        public int CampID { get; set; }
        public string PileNumber { get; set; }
        public int? Active { get; set; }
        public virtual approvalcamp MyApprovalCamp { get; set; }
        public virtual ICollection<approvalcampreserve> listOfApprovalReserve { get; set; }
    }
    [Table("approvalcampprice")]
    public class approvalcampprice
    {
        [Key]
        public int CampPriceID { get; set; }
        public int CampID { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal? ItemPrice { get; set; }
        public string ItemImage { get; set; }
    }
    [Table("approvalcampreserve")]
    public class approvalcampreserve
    {
        [Key]
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
        public DateTime? Createtime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? FinishedOn { get; set; }
        [ForeignKey("CampReserveID")]
        public virtual ICollection<approvalcampreserveatt> Listapprovalcampreserveatt { get; set; }
        [ForeignKey("CampReserveID")]
        public virtual ICollection<approvalcampreservedate> Listapprovalcampreservedate { get; set; }

        [ForeignKey("CampPileID")]
        public virtual approvalcamppile approvalReservePile { get; set; }
    }
    [Table("approvalcampreserveatt")]
    public class approvalcampreserveatt
    {
        [Key]
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
    [Table("approvalcampreservedate")]
    public class approvalcampreservedate
    {
        [Key]
        public int CampReserveDateID { get; set; }
        public int CampReserveID { get; set; }
        public int CampPileID { get; set; }
        public int CampID { get; set; }
        public DateTime? CampReserveDate { get; set; }
        public approvalcampreserve myApprovalReserve { get; set; }
    }
    [Table("approvalcamptype")]
    public class approvalcamptype
    {
        [Key]
        public int CampTypeID { get; set; }
        public int CampID { get; set; }
        public string CampTypeName { get; set; }
        public int BasicID { get; set; }
    }
    [Table("vapprovalcamp")]
    public class approvalcamplist
    {
        [Key]
        public int CampID { get; set; }
        public string CampName { get; set; }
        public string RejectReason { get; set; }
        public int CreateByID { get; set; }
        public string CreateByName { get; set; }
        public int ManagedByID { get; set; }
        public string ManagedByName { get; set; }
        public int ApprovalStatus { get; set; }
    }
}

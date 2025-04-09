using FrameWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class Advertisement : BaseEntity
    {
        public Advertisement()
        {
            AdvertisementFeaturesValues = new HashSet<AdvertisementFeaturesValues>();
            //Projects = new HashSet<Projects>();
            AdvertisementFiles = new HashSet<AdvertisementFiles>();
            AdvertisementPricesHistories = new HashSet<AdvertisementPricesHistories>();
            AdvertisementCallers = new HashSet<AdvertisementCallers>();
            AdvertisementViewers = new HashSet<AdvertisementViewers>();
            AdvertisementOwners = new HashSet<AdvertisementOwners>();
        }

        [Key]
        public long AdvertisementId { get; set; }
        public string? PublishType {  get; set; }

        //[ForeignKey("PropertyTypes")]
        public int PropertyTypeId { get; set; }

        //[ForeignKey("AdvertisementTypes")]
        //public int AdvertisementTypeId { get; set; }


        //[ForeignKey("TypeOfUses")]
        public int? TypeOfUseId { get; set; }

        //[ForeignKey("DocumentTypes")]
        public int? DocumentTypeId { get; set; }

        //[ForeignKey("DocumentOwnershipTypes")]
        public int? DocumentOwnershipTypeId { get; set; }

        //[ForeignKey("DocumentRootTypes")]
        public int? DocumentRootTypeId { get; set; }

        [StringLength(50)]
        public string? AdvertisementTitle { get; set; }

        [StringLength(50)]
        public string Area { get; set; }//مساحت

        //[StringLength(50)]
        //public string Intermediary { get; set; }//واسطه

        //[StringLength(50)]
        //public string IntermediaryPhone { get; set; }//شماره تماس واسطه

        //public long? IntermediaryPersonId { get; set; }//واسطه

        //public long? OwnerPersonId { get; set; }//مالک

        //مشاور
        public long? ConsultantUserId { get; set; }

        //آگهی دهنده
        public long? AdvertiserId { get; set; }

        public int? BuiltInYear { get; set; }

        public int? BuiltInYearFa { get; set; }

        public int? RebuiltInYear { get; set; }

        public int? RebuiltInYearFa { get; set; }

        public int? StatusId { get; set; }

        public string? RejectionReason {  get; set; }

        ////قیمت پیشنهادی
        //public long? OfferPrice { get; set; }

        ////نوع قیمت پیشنهادی
        ////0 = متری
        ////1 = کل
        //public int? OfferPriceType { get; set; }

        ////قیمت محاسبه شده
        //public long? CalculatedOfferPrice { get; set; }

        //public long? OfferPrice { get; set; }

        //public int? OfferPriceType { get; set; }

        //public long? CalculatedOfferPrice { get; set; }

        //public bool IsPrivate { get; set; }//خصوصی/عمومی

        //[ForeignKey("PropertyStates")]
        //public int PropertyStateId { get; set; }

        [StringLength(500)]
        public string? AdvertisementDescriptions { get; set; }

        //اطلاعات آگهی
        public virtual AdvertisementDetails? AdvertisementDetails { get; set; }

        public virtual ICollection<AdvertisementFeaturesValues> AdvertisementFeaturesValues { get; set; }

        //public virtual InvestorsFavorites InvestorsFavorites { get; set; }

        //public virtual ICollection<Projects> Projects { get; set; }

        public virtual ICollection<AdvertisementPricesHistories> AdvertisementPricesHistories { get; set; }

        //public virtual PropertyStates PropertyStates { get; set; }

        //public virtual AdvertisementTypes? AdvertisementTypes { get; set; }

        //public virtual TypeOfUses? TypeOfUses { get; set; }

        public virtual AdvertisementAddress? AdvertisementAddress { get; set; }
        public virtual AdvertisementFavorites? AdvertisementFavourite {  get; set; }
        public virtual AdvertisementSelectedCallers? AdvertisementSelectedCaller {  get; set; }
        public virtual ICollection<AdvertisementCallers>? AdvertisementCallers { get; set; }
        public virtual ICollection<AdvertisementViewers>? AdvertisementViewers { get; set; }
        public virtual ICollection<AdvertisementOwners> AdvertisementOwners { get; set; }

        //public virtual DocumentOwnershipTypes? DocumentOwnershipTypes { get; set; }

        //public virtual DocumentRootTypes? DocumentRootTypes { get; set; }

        //public virtual DocumentTypes? DocumentTypes { get; set; }

        public virtual ICollection<AdvertisementFiles>? AdvertisementFiles { get; set; }

        //public virtual Persons? IntermediaryPersons { get; set; }

        //public virtual Persons? OwnerPersons { get; set; }
    }
}

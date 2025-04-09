using APIs.Public.Models.Entities;
using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Properties : BaseEntity
    {
        public Properties()
        {
            FeaturesValues = new HashSet<FeaturesValues>();
            //Projects = new HashSet<Projects>();
            PropertyFiles = new HashSet<PropertyFiles>();
            PropertyOwners = new HashSet<PropertyOwners>();
            PropertiesPricesHistories = new HashSet<PropertiesPricesHistories>();
        }

        [Key]
        public long PropertyId { get; set; }

        [ForeignKey("PropertyTypes")]
        public int PropertyTypeId { get; set; }

        [ForeignKey("TypeOfUses")]
        public int? TypeOfUseId { get; set; }

        [ForeignKey("DocumentTypes")]
        public int? DocumentTypeId { get; set; }

        [ForeignKey("DocumentOwnershipTypes")]
        public int? DocumentOwnershipTypeId { get; set; }

        [ForeignKey("DocumentRootTypes")]
        public int? DocumentRootTypeId { get; set; }

        [StringLength(50)]
        public string? PropertyCodeName { get; set; }

        [StringLength(50)]
        public string Area { get; set; }//مساحت

        //[StringLength(50)]
        //public string Intermediary { get; set; }//واسطه

        //[StringLength(50)]
        //public string IntermediaryPhone { get; set; }//شماره تماس واسطه

        public long? ConsultantUserId { get; set; }//واسطه

        //آگهی دهنده
        public long? AdvertiserId { get; set; }
        //public long? OwnerPersonId { get; set; }//مالک

        public int? BuiltInYear { get; set; }

        public int? BuiltInYearFa { get; set; }

        public int? RebuiltInYear { get; set; }

        public int? RebuiltInYearFa { get; set; }

        public int? StatusId { get; set; }
        public string? RejectionReason { get; set; }

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
        public string? PropertyDescriptions { get; set; }

        public virtual ICollection<FeaturesValues> FeaturesValues { get; set; }

        public virtual InvestorsFavorites InvestorsFavorites { get; set; }

        //public virtual ICollection<Projects> Projects { get; set; }

        public virtual ICollection<PropertiesPricesHistories> PropertiesPricesHistories { get; set; }

        //public virtual PropertyStates PropertyStates { get; set; }

        public virtual PropertyTypes? PropertyTypes { get; set; }

        public virtual TypeOfUses? TypeOfUses { get; set; }

        public virtual PropertyAddress? PropertyAddress { get; set; }

        public virtual DocumentOwnershipTypes? DocumentOwnershipTypes { get; set; }

        public virtual DocumentRootTypes? DocumentRootTypes { get; set; }

        public virtual DocumentTypes? DocumentTypes { get; set; }

        public virtual ICollection<PropertyFiles>? PropertyFiles { get; set; }

        public virtual ICollection<PropertyOwners>? PropertyOwners { get; set; }

        //public virtual Persons? IntermediaryPersons { get; set; }

        //public virtual Persons? OwnerPersons { get; set; }

        //نمایش در ملکاوان
        public bool? ShowInMelkavan { get; set; }
    }
}

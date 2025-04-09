using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementPricesHistories : BaseEntity
    {
        [Key]
        public int AdvertisementPriceHistoryId { get; set; }

        [ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }

        public long? OfferPrice { get; set; }//قیمت پیشنهادی

        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }

        public long? CalculatedOfferPrice { get; set; }//قیمت محاسبه شده

        //public long? OwnerPrice { get; set; }//قیمت مالک

        //public long? RepresentativePrice { get; set; }//قیمت املاکی

        //public long? OwnerUnitPrice { get; set; }//قیمت واحد

        //public long? RepresentativeUnitPrice { get; set; }//قیمت واحد

        public long? DepositPrice { get; set; }
        public long? RentPrice { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}

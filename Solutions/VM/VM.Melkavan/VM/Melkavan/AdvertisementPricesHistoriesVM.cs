using FrameWork;

namespace VM.Melkavan
{
    public class AdvertisementPricesHistoriesVM : BaseEntity
    {
        public int AdvertisementPriceHistoryId { get; set; }

        public long AdvertisementId { get; set; }

        //قیمت پیشنهادی
        public long? OfferPrice { get; set; }
        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره

        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        //قیمت محاسبه شده
        public long? CalculatedOfferPrice { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }


        ////قیمت مالک
        //public long? CurrentPrice { get; set; }

        ////قیمت املاکی
        //public long? RepresentativePrice { get; set; }

        ////قیمت واحد
        //public long? OwnerUnitPrice { get; set; }

        ////قیمت واحد
        //public long? RepresentativeUnitPrice { get; set; }

        public virtual AdvertisementVM AdvertisementVM { get; set; }

    }
}

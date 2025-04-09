using FrameWork;


namespace VM.Teniaco
{
    public class MyPropertiesPricesHistoriesVM : BaseEntity
    {
        public int PropertyPriceHistoryId { get; set; }

        public long PropertyId { get; set; }

        //قیمت پیشنهادی
        public long? OfferPrice { get; set; }

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

        public virtual MyPropertiesVM MyPropertiesVM { get; set; }
    }
}

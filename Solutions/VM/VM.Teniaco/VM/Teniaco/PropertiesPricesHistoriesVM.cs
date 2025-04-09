using FrameWork;

namespace VM.Teniaco
{
    public class PropertiesPricesHistoriesVM : BaseEntity
    {
        public int PropertyPriceHistoryId { get; set; }

        public long PropertyId { get; set; }

        //قیمت پیشنهادی
        public long? OfferPrice { get; set; }

        public string StrOfferPrice { get; set; }

        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره

        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        //قیمت محاسبه شده
        public long? CalculatedOfferPrice { get; set; }

        public string StrCalculatedOfferPrice { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }

        public string CreateDate
        {
            set { }
            get
            {
                if (this.CreateEnDate.HasValue)
                    return DateManager.ConvertFromDate("fa", this.CreateEnDate.Value);

                return "";
            }
        }


        public virtual PropertiesVM PropertiesVM { get; set; }
    }
}

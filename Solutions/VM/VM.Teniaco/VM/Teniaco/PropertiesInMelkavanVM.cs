using FrameWork;
using Newtonsoft.Json;


namespace VM.Teniaco
{
    public class PropertiesInMelkavanVM : BaseEntity
    {
        //ملک
        public long PropertyId { get; set; }

        //کد اختصاصی
        public string? PropertyCodeName { get; set; }

        //نوع آگهی
        public int AdvertisementTypeId { get; set; }

        //قابل تبدیل
        //فقط در اجاره
        public bool? Convertable { get; set; }


        public string? PropertyDescriptions { get; set; }
        public string? SecondPropertyDescriptions {  get; set; }


        //وضعیت تاهل
        //0:هردو
        //1:متاهل
        //2:مجرد
        public int? MaritalStatusId { get; set; }


        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }



        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        //قیمت محاسبه شده
        public long? CalculatedOfferPrice { get; set; }

        //قیمت پیشنهادی
        public long? OfferPrice { get; set; }

        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره


        public long StateId { get; set; }

        public long CityId { get; set; }

        public long ZoneId { get; set; }

        public string TownName { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        //User
        //public string UserName { get; set; }
        //public string? Family { get; set; }

        //public string? Password { get; set; }


        //آگهی دهنده
        public long? AdvertiserId { get; set; } //ایدی آگهی دهنده
        public int? AdvertiserNumType { get; set; } 
        // شماره تماس آگهی دهنده
        // 0 :آژانس املاک
        // 1: آگهی دهنده
        // 2: هردو

    }
}

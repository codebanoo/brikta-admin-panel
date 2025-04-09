using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Melkavan;

namespace VM.Teniaco
{
    public class PropertiesDetailsVM : BaseEntity
    {

        //کلید
        public int PropertiesDetailsId { get; set; }

        //آگهی
        public long PropertyId { get; set; }

        public int? AdvertisementTypeId { get; set; }

        //تائید یا رد آگهی
        public bool VerifyAdvertisement { get; set; }


        //نمایش در پیشنهاد ویژه
        public bool ShowInSpecialSuggestion { get; set; }

        //قابل تبدیل
        //فقط در اجاره
        public bool Convertable { get; set; }


        //وضعیت تاهل
        //0:هردو
        //1:متاهل
        //2:مجرد
        public int? MaritalStatusId { get; set; }

        //تگ های یک آگهی
        public string? TagId { get; set; }

        public int? BuildingLifeId { get; set; }
    
        public string? BuildingLifeTitle { get; set; }

        //زیر بنا
        public string? Foundation { get; set; }

        //نمایش در ملکاوان
        public double? ShowInMelkavan { get; set; }


        //قابل مشارکت
        public bool Participable { get; set; }

        //قابل معاوضه
        public bool Exchangeable { get; set; }

        // توضیحات دومی اضافی
        public string? SecondPropertyDescriptions {  get; set; }


        //public virtual BuildingLifesVM? BuildingLifesVM { get; set; }

    }
}

using FrameWork;
using System.Runtime;

namespace VM.Melkavan
{
    public class AdvertisementDetailsVM : BaseEntity
    {

        //کلید
        public int AdvertisementDetailsId { get; set; }

        //آگهی
        public long AdvertisementId { get; set; }

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
       
        //عمر بنا
        public int? BuildingLifeId { get; set; }

        //زیر بنا
        public string? Foundation { get; set; }


        //قابل مشارکت
        public bool Participable { get; set; }

        //قابل معاوضه
        public bool Exchangeable { get; set; }
        // لینک اینستاگرام
        public string? InstagramLink {  get; set; }


        //لیبل ها
        public string? TagId { get; set; }
        // توضیحات دوم اضافی
        public string? SecondAdvertisementDescriptions {  get; set; }
        public virtual BuildingLifesVM? BuildingLifesVM { get; set; }
    }
}

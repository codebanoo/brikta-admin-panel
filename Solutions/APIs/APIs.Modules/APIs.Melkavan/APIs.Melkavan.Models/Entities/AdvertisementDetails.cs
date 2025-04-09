using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementDetails : BaseEntity
    {
        [Key]
        public int AdvertisementDetailsId { get; set; }

        //[ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }

        //[ForeignKey("AdvertisementTypes")]
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
        public string? InstagramLink { get; set; }

        public string? TagId { get; set; }
        public string? SecondAdvertisementDescriptions { get; set; }
    }
}

using FrameWork;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VM.Public;
using VM.Teniaco;

namespace VM.Melkavan
{
    public class AdvertisementsListForMelkavanPropertiesVM : BaseEntity
    {
        public AdvertisementsListForMelkavanPropertiesVM()
        {
            //AdvertisementFeaturesValuesVM = new List<AdvertisementFeaturesValuesVM>();
            AdvertisementFeaturesValuesVM = new HashSet<AdvertisementFeaturesValuesVM>();

            AdvertisementOwnersVM = new AdvertisementOwnersVM();
            //AdvertisementFilesVM = new HashSet<AdvertisementFilesVM>();

            //AdvertismentPricesHistoriesVM = new HashSet<AdvertismentPricesHistoriesVM>();
        }

        [NotMapped]
        public string? RecordType { get; set; }
        public string? PublishType { get; set; }

        //کلید
        public long AdvertisementId { get; set; }

    
        //نوع ملک
        [Required(ErrorMessage = "نوع ملک را انتخاب کنید")]
        public int PropertyTypeId { get; set; }

        public string? PropertyTypeTilte { get; set; }

        //نوع کاربری
        [Required(ErrorMessage = "نوع کاربری را انتخاب کنید")]
        public int? TypeOfUseId { get; set; }

        public string? TypeUseTitle { get; set; }

        //نوع سند
        //[Required(ErrorMessage = "نوع سند را انتخاب کنید")]
        public int? DocumentTypeId { get; set; }

        public string? DocumentTypeTitle { get; set; }

        //نوع مالکیت سند
        //[Required(ErrorMessage = "نوع مالکیت را انتخاب کنید")]
        public int? DocumentOwnershipTypeId { get; set; }

        public string? DocumentOwnershipTypeTitle { get; set; }

        //نوع ریشه سند
        public int? DocumentRootTypeId { get; set; }

        public string? DocumentRootTypeTitle { get; set; }

        //کد اختصاصی
        //public string? PropertyCodeName { get; set; }

        //عنوان آگهی
        public string? AdvertisementTitle{ get; set; }

        //مساحت
        [Required(ErrorMessage = "مساحت را انتخاب کنید")]
        public string Area { get; set; }

        ////واسطه
        //public string? Intermediary { get; set; }

        ////شماره تماس واسطه
        //public string? IntermediaryPhone { get; set; }

        ////واسطه
        //public long? IntermediaryPersonId { get; set; }

        ////مشاور
        public long? ConsultantUserId { get; set; }


        //آگهی دهنده
        public long? AdvertiserId { get; set; }
        // نام آگهی دهنده
        public string? AdvertiserName { get; set; }


        ////مالک
        public long? OwnerId { get; set; }

        //تاریخ ساخت
        public int? BuiltInYear { get; set; }

        //تاریخ ساخت
        public int? BuiltInYearFa { get; set; }

        //سال بازسازی
        public int? RebuiltInYear { get; set; }

        //سال بازسازی
        //[Required(ErrorMessage = "سال ساخت را وارد کنید")]
        public int? RebuiltInYearFa { get; set; }

        //قیمت پیشنهادی ===> اجاره - ودیعه
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

        //خصوصی/عمومی
        //public bool IsPrivate { get; set; }

        //public int PropertyStateId { get; set; }

        public string? AdvertisementDescriptions { get; set; }

        public string? NewAdvertisementDescriptions
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AdvertisementDescriptions))
                {
                    if (this.AdvertisementDescriptions.Length > 50)
                        return this.AdvertisementDescriptions.Substring(0, 50);
                    else
                        return this.AdvertisementDescriptions;
                }
                else { return string.Empty; }
            }
        }

        //اطلاعات آگهی
        public virtual AdvertisementDetailsVM? AdvertisementDetailsVM { get; set; }

        //شماره موبایل برای تماس
        public virtual AdvertisementSelectedCallersVM? AdvertisementSelectedCallersVM { get; set; }

        public ICollection<AdvertisementFeaturesValuesVM>? AdvertisementFeaturesValuesVM { get; set; }

        public string StrAdvertisementFeaturesValuesVM 
        {
            get {
                return "";
            }
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    AdvertisementFeaturesValuesVM = JsonConvert.DeserializeObject<List<AdvertisementFeaturesValuesVM>>(value);
                }
            }
        }

        //مالک
        //public virtual InvestorsFavoritesVM? InvestorsFavoritesVM { get; set; }

        //پروژه ها
        //public virtual ICollection<ProjectsVM>? ProjectsVM { get; set; }

        //سوابق قیمت
        //public virtual ICollection<AdvertisementPricesHistoriesVM>? AdvertisementPricesHistoriesVM { get; set; }

        public virtual AdvertisementPricesHistoriesVM? AdvertisementPricesHistoriesVM { get; set; }

        //public virtual PropertyStatesVM? PropertyStatesVM { get; set; }

        //نوع ملک
        public virtual PropertyTypesVM? PropertyTypesVM { get; set; }

        //نوع کاربری
        public virtual TypeOfUsesVM? TypeUsesVM { get; set; }

        //آدرس
        public virtual AdvertisementAddressVM? AdvertisementAddressVM { get; set; }

        //فایلها
        public virtual List<AdvertisementFilesVM>? AdvertisementFilesVM { get; set; }

        // وضعیت آگهی
        public int? StatusId { get; set; }

        // علت رد آگهی
        public string? RejectionReason {  get; set; }

        //مالکین
        public virtual AdvertisementOwnersVM? AdvertisementOwnersVM { get; set; }


        public int MyProperty { get; set; }

        public List<IFormFile>? Files { get; set; }

        //public virtual PersonsVM? IntermediaryPersons { get; set; }

        //public virtual PersonsVM? OwnerPersons { get; set; }

        public string RegisterOrEditDate
        {
            get
            {
                if (EditEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(EditEnDate.Value);
                }
                else if (CreateEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(CreateEnDate.Value);
                }
                else
                    return "تاریخ ندارد";
            }
        }


        public int? CountOfMaps { get; set; }
        public int? CountOfDocs { get; set; }
        public int? CountOfMedia { get; set; }
        public int? CountOfPrices { get; set; }

        public DateTime? CurrentDate { get; set; }
        public bool? IsFavorite { get; set; }
        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره

        public long? ViewersTotalCount { get; set; }
        [NotMapped]
        public IList<int>? DeletedPhotosIDs { get; set; }
        [NotMapped]
        public IList<string>? DeletedPhotosPaths {  get; set; }
        [NotMapped]
        public bool? IsMainChanged { get; set; }
        [NotMapped]
        public int? DistanceFromMyProperty { get; set; }
    }
}

using FrameWork;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VM.Console;
using VM.Melkavan;
using VM.Public;
using VM.Teniaco.VM.Teniaco;

namespace VM.Teniaco
{
    [NotMapped]
    public class PropertiesVM : BaseEntity
    {
        public PropertiesVM()
        {
            FeaturesValuesVM = new HashSet<FeaturesValuesVM>();
            //ProjectsVM = new HashSet<ProjectsVM>();
            //PropertyFilesVM = new HashSet<PropertyFilesVM>();
            PropertiesPricesHistoriesVM = new HashSet<PropertiesPricesHistoriesVM>();
        }

        //کلید
        public long PropertyId { get; set; }

        //نوع ملک
        [Required(ErrorMessage = "نوع ملک را انتخاب کنید")]
        public int PropertyTypeId { get; set; }

        public string? PropertyTypeTilte { get; set; }

        //نوع کاربری
        [Required(ErrorMessage = "نوع کاربری را انتخاب کنید")]
        public int? TypeOfUseId { get; set; }

        public string? TypeUseTitle { get; set; }

        //نوع سند
        [Required(ErrorMessage = "نوع سند را انتخاب کنید")]
        public int? DocumentTypeId { get; set; }

        public string? DocumentTypeTitle { get; set; }

        //نوع مالکیت سند
        [Required(ErrorMessage = "نوع مالکیت سند را انتخاب کنید")]
        public int? DocumentOwnershipTypeId { get; set; }

        public string? DocumentOwnershipTypeTitle { get; set; }

        //نوع ریشه سند
        [Required(ErrorMessage = "نوع ریشه سند را انتخاب کنید")]
        public int? DocumentRootTypeId { get; set; }

        public string? DocumentRootTypeTitle { get; set; }

        //کد اختصاصی
        public string? PropertyCodeName { get; set; }

        //مساحت
        [Required(ErrorMessage = "مساحت را وارد کنید")]
        public string Area { get; set; }

        ////واسطه
        //public string? Intermediary { get; set; }

        ////شماره تماس واسطه
        //public string? IntermediaryPhone { get; set; }

        //واسطه
        public long? ConsultantUserId { get; set; }

        //مالک
        //public long? OwnerId { get; set; }
        public List<PropertyOwnersVM> PropertyOwnersVM { get; set; }

        //تاریخ ساخت
        public int? BuiltInYear { get; set; }

        //تاریخ ساخت
        public int? BuiltInYearFa { get; set; }

        //تاریخ بازسازی
        public int? RebuiltInYear { get; set; }

        //تاریخ بازسازی
        public int? RebuiltInYearFa { get; set; }

        //قیمت پیشنهادی
        public long? OfferPrice { get; set; }

        public string? StrOfferPrice { get; set; }

        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        //قیمت محاسبه شده
        public long? CalculatedOfferPrice { get; set; }

        public string? StrCalculatedOfferPrice { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }

        //خصوصی/عمومی
        //public bool IsPrivate { get; set; }

        //public int PropertyStateId { get; set; }

        public string? PropertyDescriptions { get; set; }

        public string? NewPropertyDescriptions
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PropertyDescriptions))
                {
                    if (this.PropertyDescriptions.Length > 50)
                        return this.PropertyDescriptions.Substring(0, 50);
                    else
                        return this.PropertyDescriptions;
                }
                else { return string.Empty; }
            }
        }

        public virtual PropertiesDetailsVM? PropertiesDetailsVM { get; set; }
        public virtual ICollection<FeaturesValuesVM>? FeaturesValuesVM { get; set; }

        //public virtual InvestorsFavoritesVM? InvestorsFavoritesVM { get; set; }

        //پروژه ها
        //public virtual ICollection<ProjectsVM>? ProjectsVM { get; set; }

        //سوابق قیمت
        public virtual ICollection<PropertiesPricesHistoriesVM>? PropertiesPricesHistoriesVM { get; set; }

        //public virtual PropertyStatesVM? PropertyStatesVM { get; set; }

        //نوع ملک
        public virtual PropertyTypesVM? PropertyTypesVM { get; set; }

        //نوع کاربری
        public virtual TypeOfUsesVM? TypeUsesVM { get; set; }

        //آدرس
        public virtual PropertyAddressVM? PropertyAddressVM { get; set; }

        //فایلها
        //public virtual ICollection<PropertyFilesVM>? PropertyFilesVM { get; set; }

        public virtual List<PropertyFilesVM>? PropertyFilesVM { get; set; }

        public virtual PersonsVM? IntermediaryPersons { get; set; }

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

        public string StrPropertyOwnersVM
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    PropertyOwnersVM = JsonConvert.DeserializeObject<List<PropertyOwnersVM>>(value);
                }
            }
        }
        public List<PropertyBuyersVM> PropertyBuyersVM { get; set; }
        public string StrPropertyBuyersVM
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    PropertyBuyersVM = JsonConvert.DeserializeObject<List<PropertyBuyersVM>>(value);
                }
            }
        }

        //نمایش در ملکاوان
        public bool? ShowInMelkavan { get; set; }
        // وضعیت آگهی
        public int? StatusId {  get; set; }
        // علت رد آگهی
        public string? RejectionReason {  get; set; }
        //تماس با آگهی دهنده
        public virtual PropertySelectedCallersVM? PropertySelectedCallersVM { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }
    }
}

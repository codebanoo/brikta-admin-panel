using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Public;

namespace VM.Teniaco
{
    public class MyPropertiesVM : BaseEntity
    {
        public MyPropertiesVM()
        {
            FeaturesValuesVM = new HashSet<FeaturesValuesVM>();
            //ProjectsVM = new HashSet<ProjectsVM>();
            MyPropertyFilesVM = new HashSet<MyPropertyFilesVM>();
            MyPropertiesPricesHistoriesVM = new HashSet<MyPropertiesPricesHistoriesVM>();
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
        public string? PropertyCodeName { get; set; }

        //مساحت
        [Required(ErrorMessage = "مساحت را انتخاب کنید")]
        public string Area { get; set; }

        ////واسطه
        //public string? Intermediary { get; set; }

        ////شماره تماس واسطه
        //public string? IntermediaryPhone { get; set; }

        //واسطه
        public long? ConsultantUserId { get; set; }

        //مالک
        public long? OwnerId { get; set; }

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

        public virtual ICollection<FeaturesValuesVM>? FeaturesValuesVM { get; set; }

        //مالک
        public virtual InvestorsFavoritesVM? InvestorsFavoritesVM { get; set; }

        //پروژه ها
        //public virtual ICollection<ProjectsVM>? ProjectsVM { get; set; }

        //سوابق قیمت
        public virtual ICollection<MyPropertiesPricesHistoriesVM>? MyPropertiesPricesHistoriesVM { get; set; }

        //public virtual PropertyStatesVM? PropertyStatesVM { get; set; }

        //نوع ملک
        public virtual MyPropertyTypesVM? MyPropertyTypesVM { get; set; }

        //نوع کاربری
        public virtual TypeOfUsesVM? TypeUsesVM { get; set; }

        //آدرس
        public virtual MyPropertyAddressVM? MyPropertyAddressVM { get; set; }

        //فایلها
        public virtual ICollection<MyPropertyFilesVM>? MyPropertyFilesVM { get; set; }

        public virtual PersonsVM? IntermediaryPersons { get; set; }

        public virtual PersonsVM? OwnerPersons { get; set; }

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
    }

}

using FrameWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Console;
using VM.Public;
using VM.Teniaco;

namespace VM.Projects
{
    public class ConstructionProjectsVM : BaseEntity
    {
        public long ConstructionProjectId { get; set; }



        [Required(ErrorMessage = "عنوان پروژه را وارد کنید")]
        public string ConstructionProjectTitle { get; set; }



        //نوع پروژه
        [Required(ErrorMessage = "نوع پروژه را انتخاب کنید")]
        public int ConstructionProjectTypeId { get; set; }


        //کارگاه
        public string? WorkshopName { get; set; }



        [Required(ErrorMessage = "ملک را انتخاب کنید")]
        public long PropertyId { get; set; }


        //نماینده
        //[Required(ErrorMessage = "نماینده را انتخاب کنید")]
        public long? UserId { get; set; }



        //زیر بنا
        public int? Foundation { get; set; }

        public string? ConstructionProjectDescription { get; set; }

        //شروع اجرا
        public string? ExecutionStartDate { get; set; }

        //شروع طراحی
        public string? DesignStartDate { get; set; }


        // تاریخ شروع پروژه
        public DateTime? StartDateEn { get; set; }


        // ماه پیش بینی کل پروژه
        //مدت پروژه
        public string? MonthsLeftUntilTheEnd { get; set; }

        // نمایش در داشبورد
        public bool? ShowInDashboard {  get; set; }
        // عنوان پروژه در صفحه پیشنهادات تنیاکو
        public string? SuggestionTitle {  get; set; }

        public virtual PersonsVM? PersonsVM { get; set; }
        public virtual PropertiesVM? PropertiesVM { get; set; }
        public virtual ConstructionProjectTypesVM? ConstructionProjectTypesVM { get; set; }
        //مالکین
        public List<ConstructionProjectOwnerUsersVM> ConstructionProjectOwnerUsersVM { get; set; }

        //نمایندگان
        public List<ConstructionProjectRepresentativesVM> ConstructionProjectRepresentativesVM { get; set; }

        public virtual CustomUsersVM CustomUsersVM { get; set; }

        //[Required(ErrorMessage = "انتخاب حداقل یک مالک اجباری است")]
        public string StrConstructionProjectOwnerUsersVM
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ConstructionProjectOwnerUsersVM = JsonConvert.DeserializeObject<List<ConstructionProjectOwnerUsersVM>>(value);
                }
            }
        }


        public string StrConstructionProjectRepresentativesVM
        {
            get
            {
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ConstructionProjectRepresentativesVM = JsonConvert.DeserializeObject<List<ConstructionProjectRepresentativesVM>>(value);
                }
            }
        }

        public string? NewConstructionProjectDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ConstructionProjectDescription))
                {
                    if (this.ConstructionProjectDescription.Length > 50)
                        return this.ConstructionProjectDescription.Substring(0, 50);
                    else
                        return this.ConstructionProjectDescription;
                }
                else { return string.Empty; }
            }
        }


        public string? StartDate
        {
            get
            {
                if (this.StartDateEn.HasValue)
                {
                    return PersianDate.PersianDateFrom(this.StartDateEn.Value);
                }
                else
                {
                    return string.Empty;
                }


            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.StartDateEn = PersianDate.ToGregorianDate(PersianType.ToEnglishDigits(value));
                }
                else
                {
                    this.StartDateEn = (DateTime?)null;
                }
            }
        }


        public bool? HasSuggestion { get; set; }


        #region PriceHistories

        //ارزش جاری پروژه
        public long? CurrentValueOfProject { get; set; }

        //ارزش برآورد پایان پروژه
        public long? ProjectEstimate { get; set; }

        //برآورد هزینه ساخت
        public long? PrevisionOfCost { get; set; }


        //ارزش جاری پروژه
        public string? StrCurrentValueOfProject { get; set; }

        //ارزش برآورد پایان پروژه
        public string? StrProjectEstimate { get; set; }

        //برآورد هزینه ساخت
        public string? StrPrevisionOfCost { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }


        //سوابق قیمت
        public virtual ICollection<ConstructionProjectPriceHistoriesVM>? ConstructionProjectPriceHistoriesVM { get; set; }
        #endregion




        #region DataTypeCounts
        public int? PartnershipAgreementsCount { get; set; }
        public int? ContractAgreementsCount { get; set; }
        public int? ConfirmationAgreementsCount { get; set; }
        public int? ContractorsAgreementsCount { get; set; }
        public int? InitialPlansCount { get; set; }
        public int? PitchDecksCount { get; set; }
        public int? MeetingBoardsCount { get; set; }
        public int? ProgressPicturesCount { get; set; }

        public int? BillDelaysCount { get; set; }

        #endregion



        #region outerDashboard
        // روز مانده تا پایان پروژه
        public string? DaysLeftUntilTheEnd { get; set; }

        // براورد کل هزینه پروژه
        public string? EstimateTotalCost { get; set; }

        // تاریخ پایان پروژه
        public string? EndDate { get; set; }

        public string? EndFaDate
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EndDate))
                {
                    return PersianDate.PersianDateFrom(DateTime.Parse(this.EndDate));
                }
                else
                {
                    return string.Empty;
                }


            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.EndDate = PersianDate.ToGregorianDate(PersianType.ToEnglishDigits(value)).ToShortDateString();
                }
                else
                {
                    this.EndDate = null;
                }
            }
        }

        public long? ProjectTotalCost { get; set; }

        public long? ProjectPublicTotalCost { get; set; }

        public long? ProjectShareOfGeneralCost { get; set; }

        public decimal? ProjectShareOfGeneralCostPercent { get; set; }

        public long? ProjectPrivateTotalCost { get; set; }

        public decimal? ProjectRemainingCost { get; set; }

        public decimal? ShareOfTotalProjectCost { get; set; }

        public double? ShareOfTotalProjectCostPercent { get; set; }

        public long? ProjectPaidShare { get; set; }

        public long? FundsCeiling { get; set; }

        public long? RemainingDebt { get; set; }

         //سهامدار نام خانوادگی
        public string? OwnerUserFamiy { get; set; }

        //سهامدار
        public long? OwnerUserId { get; set; }

        //نماینده
        public long? ConstructionProjectRepresentativeId { get; set; }


        //تعهدات
        public long? Obligations { get; set; }

        //تاریخ آخرین آپدیت
        public string? LastProgessUpdateDate { get; set; }
        #endregion

        public ProjectProgressDataVM? ProjectProgressDataVM { get; set; }
    }
}

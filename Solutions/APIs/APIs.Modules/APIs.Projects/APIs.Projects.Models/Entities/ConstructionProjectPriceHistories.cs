using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectPriceHistories : BaseEntity
    {
        [Key]
        public long ConstructionProjectPriceHistoryId { get; set; }

        public long ConstructionProjectId { get; set; }

        //ارزش جاری پروژه
        public long CurrentValueOfProject { get; set; }

        //ارزش برآورد پایان پروژه
        public long ProjectEstimate { get; set; }

        //برآورد هزینه ساخت
        public long PrevisionOfCost { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }

        //spublic virtual ConstructionProjects ConstructionProjects { get; set; }
    }
}

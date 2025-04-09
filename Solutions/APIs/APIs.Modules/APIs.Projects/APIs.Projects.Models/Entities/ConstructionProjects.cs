using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjects : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConstructionProjectId { get; set; }

        public string ConstructionProjectTitle { get; set; }

        public int ConstructionProjectTypeId { get; set; }

        public long PropertyId { get; set; }

        public long? UserId { get; set; }

        public string? ConstructionProjectDescription { get; set; }

        //شروع اجرا
        public string? ExecutionStartDate { get; set; }

        //شروع طراحی
        public string? DesignStartDate { get; set; }

        public int? Foundation { get; set; }

        public string? WorkshopName { get; set; }

        // تاریخ شروع پروژه
        public DateTime? StartDateEn { get; set; }


        // ماه پیش بینی کل پروژه
        //مدت پروژه
        public string? MonthsLeftUntilTheEnd { get; set; }
        // نمایش در داشبورد
        public bool? ShowInDashboard { get; set; }
        // عنوان پروژه در صفحه پیشنهادات تنیاکو
        public string? SuggestionTitle { get; set; }
    }
}

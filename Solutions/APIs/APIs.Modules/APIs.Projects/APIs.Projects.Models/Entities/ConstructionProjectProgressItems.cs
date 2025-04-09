using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectProgressItems : BaseEntity
    {
        [Key]
        public long ConstructionProjectProgressItemId { get; set; }

        public long ConstructionProjectId { get; set; }

        public string WBS { get; set; }//WBS

        public string ConstructionProjectProgressItemTitle { get; set; }//شرح ردیف برنامه زمان بندی

        public long? ConstructionProjectProgressParentItemId { get; set; }

        public string PeriodicProgram { get; set; }//پیشرفت دوره ای - برنامه

        public string PeriodicOperation { get; set; }//پیشرفت دوره ای - عملکرد

        public string PeriodicDeviation { get; set; }//پیشرفت دوره ای - انحراف

        public string CumulativeProgram { get; set; }//پیشرفت تجمعی - برنامه

        public string CumulativeOperation { get; set; }//پیشرفت تجمعی - عملکرد

        public string CumulativeDeviation { get; set; }//پیشرفت تجمعی - انحراف

        public string SummaryOfActivity { get; set; }//خلاصه فعالیت

        public string ActivityStart { get; set; }//شروع

        public string ActivityEnd { get; set; }//پایان

        public string Weight { get; set; }//وزن

        public string DelayWeight { get; set; }//وزن تاخیر
    }
}

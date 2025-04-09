using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class TimeTables : BaseEntity
    {
        [Key]
        public long TimeTableId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? TimeTableTitle { get; set; }
        public string? TimeTableDescription { get; set; }
        public long? TimeTableNumber { get; set; }
        public string? TimeTableFilePath { get; set; }
        public string? TimeTableFileExt { get; set; }
        public int? TimeTableFileOrder { get; set; }
        public string? TimeTableFileType { get; set; }
        //public int? ParentTimeTableId { get; set; }
        //public bool? IsConfirm { get; set; }
        //public long? UserIdConfirmation { get; set; }
        //public DateTime? ConfirmationDate { get; set; }
        //public string? ConfirmationTime { get; set; }
        //public bool? IsView { get; set; }
        //public long? UserIdViewer { get; set; }
        //public DateTime? ViewDate { get; set; }
        //public string? ViewTime { get; set; }
        //public bool? IsSend { get; set; }
        //public long? UserIdSender { get; set; }
        //public DateTime? SendDate { get; set; }
        //public string? SendTime { get; set; }
    }
}

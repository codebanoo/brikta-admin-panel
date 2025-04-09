using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class MeetingBoards : BaseEntity
    {
        [Key]
        public long MeetingBoardId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? MeetingBoardTitle { get; set; }
        public string? MeetingBoardDescription { get; set; }
        public long? MeetingBoardNumber { get; set; }
        public string? MeetingBoardFilePath { get; set; }
        public string? MeetingBoardFileExt { get; set; }
        public int? MeetingBoardFileOrder { get; set; }
        public string? MeetingBoardFileType { get; set; }

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

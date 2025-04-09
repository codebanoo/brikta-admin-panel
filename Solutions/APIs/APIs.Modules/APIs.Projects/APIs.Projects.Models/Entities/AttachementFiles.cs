using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class AttachementFiles : BaseEntity
    {
        [Key]
        public long AttachementId { get; set; }
        public string? AttachementTitle { get; set; }
        public string? AttachementDescription { get; set; }
        public long? AttachementParentId { get; set; }
        public string? AttachementTableTitle { get; set; }
        public string? AttachementFilePath { get; set; }
        public string? AttachementFileExt { get; set; }
        public int? AttachementFileOrder { get; set; }
        public string? AttachementFileType { get; set; }
        public long? ReplyAttachementId { get; set; }
        //public long? OperationUserId { get; set; }
        //public DateTime? OperationEnDate { get; set; }
        //public string OperationTime { get; set; }
        //public int AttachementStateId { get; set; }
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

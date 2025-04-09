using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ConfirmationAgreements : BaseEntity
    {
        [Key]
        public long ConfirmationAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public int ConfirmationTypeId { get; set; }
        public string? ConfirmationAgreementTitle { get; set; }
        public string? ConfirmationAgreementDescription { get; set; }
        public long? ConfirmationAgreementNumber { get; set; }
        public string? ConfirmationAgreementFilePath { get; set; }
        public string? ConfirmationAgreementFileExt { get; set; }
        public int? ConfirmationAgreementFileOrder { get; set; }
        public string? ConfirmationAgreementFileType { get; set; }
        //public bool? IsConfirm { get; set; }
        //public long? UserId { get; set; }
        //public string? ConfirmationTime { get; set; }
        //public DateTime? ConfirmationDate { get; set; }
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

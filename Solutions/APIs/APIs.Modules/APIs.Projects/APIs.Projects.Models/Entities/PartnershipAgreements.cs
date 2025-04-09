using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class PartnershipAgreements : BaseEntity
    {
        [Key]
        public long PartnershipAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? PartnershipAgreementTitle { get; set; }
        public string? PartnershipAgreementDescription { get; set; }
        public long? PartnershipAgreementNumber { get; set; }
        public string? PartnershipAgreementFilePath { get; set; }
        public string? PartnershipAgreementFileExt { get; set; }
        public int? PartnershipAgreementFileOrder { get; set; }
        public string? PartnershipAgreementFileType { get; set; }
        //public int? ParentPartnershipAgreementId { get; set; }
        public bool? IsConfirm { get; set; }
        public long? UserIdConfirmation { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string? ConfirmationTime { get; set; }
        public bool? IsView { get; set; }
        public long? UserIdViewer { get; set; }
        public DateTime? ViewDate { get; set; }
        public string? ViewTime { get; set; }
        public bool? IsSend { get; set; }
        public long? UserIdSender { get; set; }
        public DateTime? SendDate { get; set; }
        public string? SendTime { get; set; }
    }

}

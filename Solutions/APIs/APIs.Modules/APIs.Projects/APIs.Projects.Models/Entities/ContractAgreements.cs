using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ContractAgreements : BaseEntity
    {
        [Key]
        public long ContractAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? ContractAgreementTitle { get; set; }
        public string? ContractAgreementDescription { get; set; }
        public long? ContractAgreementNumber { get; set; }
        public string? ContractAgreementFilePath { get; set; }
        public string? ContractAgreementFileExt { get; set; }
        public int? ContractAgreementFileOrder { get; set; }
        public string? ContractAgreementFileType { get; set; }

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

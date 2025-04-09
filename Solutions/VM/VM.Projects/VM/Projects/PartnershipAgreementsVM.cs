using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class PartnershipAgreementsVM : BaseEntity
    {
        public long PartnershipAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? PartnershipAgreementTitle { get; set; }
        public string? PartnershipAgreementDescription { get; set; }
        public string? NewPartnershipAgreementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PartnershipAgreementDescription))
                {
                    if (this.PartnershipAgreementDescription.Length > 50)
                        return this.PartnershipAgreementDescription.Substring(0, 50);
                    else
                        return this.PartnershipAgreementDescription;
                }
                else { return string.Empty; }
            }
        }
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
        //تعداد پیوست های یک قرارداد
        public int? AttachementsCount { get; set; }

        public int? ConversationIsReadCount { get; set; }
    }
}

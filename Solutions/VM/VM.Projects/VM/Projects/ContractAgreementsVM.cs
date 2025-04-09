using FrameWork;
using System;

namespace VM.Projects
{
    public class ContractAgreementsVM : BaseEntity
    {
        public long ContractAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? ContractAgreementTitle { get; set; }
        public string? ContractAgreementDescription { get; set; }
        public string? NewContractAgreementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ContractAgreementDescription))
                {
                    if (this.ContractAgreementDescription.Length > 50)
                        return this.ContractAgreementDescription.Substring(0, 50);
                    else
                        return this.ContractAgreementDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? ContractAgreementNumber { get; set; }
        public string? ContractAgreementFilePath { get; set; }
        public string? ContractAgreementFileExt { get; set; }
        public int? ContractAgreementFileOrder { get; set; }
        public string? ContractAgreementFileType { get; set; }

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

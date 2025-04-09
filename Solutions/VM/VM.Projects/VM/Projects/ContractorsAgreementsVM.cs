using FrameWork;
using System;

namespace VM.Projects
{
    public class ContractorsAgreementsVM : BaseEntity
    {
        public long ContractorsAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? ContractorsAgreementTitle { get; set; }
        public string? ContractorsAgreementDescription { get; set; }
        public string? NewContractorsAgreementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ContractorsAgreementDescription))
                {
                    if (this.ContractorsAgreementDescription.Length > 50)
                        return this.ContractorsAgreementDescription.Substring(0, 50);
                    else
                        return this.ContractorsAgreementDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? ContractorsAgreementNumber { get; set; }
        public string? ContractorsAgreementFilePath { get; set; }
        public string? ContractorsAgreementFileExt { get; set; }
        public int? ContractorsAgreementFileOrder { get; set; }
        public string? ContractorsAgreementFileType { get; set; }

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

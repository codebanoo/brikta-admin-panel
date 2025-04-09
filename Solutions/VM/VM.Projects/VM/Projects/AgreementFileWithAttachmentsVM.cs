using FrameWork;
using System.Collections.Generic;

namespace VM.Projects
{
    public class AgreementFileWithAttachmentsVM : BaseEntity
    {
        public long AgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string AgreementType { get; set; }
        public string? AgreementTitle { get; set; }
        public string? AgreementDescription { get; set; }
        public string? NewAgreementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AgreementDescription))
                {
                    if (this.AgreementDescription.Length > 50)
                        return this.AgreementDescription.Substring(0, 50);
                    else
                        return this.AgreementDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? AgreementNumber { get; set; }
        public string? AgreementFilePath { get; set; }
        public string? AgreementFileExt { get; set; }
        public int? AgreementFileOrder { get; set; }
        public string? AgreementFileType { get; set; }
        //public int? ParentAgreementId { get; set; }
        public bool? IsConfirm { get; set; }

        public bool? IsView { get; set; }
        public int? ConversationIsReadCount { get; set; }

        public List<AttachementFilesVM>? AttachmentFilesVMs { get; set; }
        public List<ConversationLogsVM>? ConversationLogVms { get; set; }

    }
}

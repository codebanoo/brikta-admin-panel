using FrameWork;
using System;
using System.Collections.Generic;

namespace VM.Projects
{
    public class AttachementFilesVM : BaseEntity
    {
        public long AttachementId { get; set; }
        public string? AttachementTitle { get; set; }
        public string? AttachementDescription { get; set; }
        public string? NewAttachementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AttachementDescription))
                {
                    if (this.AttachementDescription.Length > 50)
                        return this.AttachementDescription.Substring(0, 50);
                    else
                        return this.AttachementDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? AttachementParentId { get; set; }
        public string? AttachementTableTitle { get; set; }
        public string? AttachementFilePath { get; set; }
        public string? AttachementFileExt { get; set; }
        public int? AttachementFileOrder { get; set; }
        public string? AttachementFileType { get; set; }
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
        public long? ConstructionProjectId { get; set; }
        public int? ConversationIsReadCount { get; set; }
        public List<ConversationLogsVM>? ConversationLogVms { get; set; }
    }
}

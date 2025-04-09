using FrameWork;
using System;

namespace VM.Projects
{
    public class InitialPlansVM : BaseEntity
    {
        public long InitialPlanId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? InitialPlanTitle { get; set; }
        public string? InitialPlanDescription { get; set; }
        public string? NewInitialPlanDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.InitialPlanDescription))
                {
                    if (this.InitialPlanDescription.Length > 50)
                        return this.InitialPlanDescription.Substring(0, 50);
                    else
                        return this.InitialPlanDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? InitialPlanNumber { get; set; }
        public string? InitialPlanFilePath { get; set; }
        public string? InitialPlanFileExt { get; set; }
        public int? InitialPlanFileOrder { get; set; }
        public string? InitialPlanFileType { get; set; }

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

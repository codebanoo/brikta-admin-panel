using FrameWork;
using System;

namespace VM.Projects
{
    public class MeetingBoardsVM : BaseEntity
    {
        public long MeetingBoardId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? MeetingBoardTitle { get; set; }
        public string? MeetingBoardDescription { get; set; }
        public string? NewMeetingBoardDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.MeetingBoardDescription))
                {
                    if (this.MeetingBoardDescription.Length > 50)
                        return this.MeetingBoardDescription.Substring(0, 50);
                    else
                        return this.MeetingBoardDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? MeetingBoardNumber { get; set; }
        public string? MeetingBoardFilePath { get; set; }
        public string? MeetingBoardFileExt { get; set; }
        public int? MeetingBoardFileOrder { get; set; }
        public string? MeetingBoardFileType { get; set; }

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

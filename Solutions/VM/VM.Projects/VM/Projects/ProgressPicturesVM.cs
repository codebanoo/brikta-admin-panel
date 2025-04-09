using FrameWork;
using System;

namespace VM.Projects
{
    public class ProgressPicturesVM : BaseEntity
    {
        public long ProgressPictureId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? ProgressPictureTitle { get; set; }
        public string? ProgressPictureDescription { get; set; }
        public string? NewProgressPictureDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ProgressPictureDescription))
                {
                    if (this.ProgressPictureDescription.Length > 50)
                        return this.ProgressPictureDescription.Substring(0, 50);
                    else
                        return this.ProgressPictureDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? ProgressPictureNumber { get; set; }
        public string? ProgressPictureFilePath { get; set; }
        public string? ProgressPictureFileExt { get; set; }
        public int? ProgressPictureFileOrder { get; set; }
        public string? ProgressPictureFileType { get; set; }

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

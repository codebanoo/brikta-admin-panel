using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class PitchDecksVM : BaseEntity
    {
        public long PitchDeckId { get; set; }
        public long ConstructionProjectId { get; set; }
        public string? PitchDeckTitle { get; set; }
        public string? PitchDeckDescription { get; set; }
        public string? NewPitchDeckDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PitchDeckDescription))
                {
                    if (this.PitchDeckDescription.Length > 50)
                        return this.PitchDeckDescription.Substring(0, 50);
                    else
                        return this.PitchDeckDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? PitchDeckNumber { get; set; }
        public string? PitchDeckFilePath { get; set; }
        public string? PitchDeckFileExt { get; set; }
        public int? PitchDeckFileOrder { get; set; }
        public string? PitchDeckFileType { get; set; }

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

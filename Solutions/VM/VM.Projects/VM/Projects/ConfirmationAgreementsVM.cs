using FrameWork;
using System;
using VM.Public;

namespace VM.Projects
{
    public class ConfirmationAgreementsVM : BaseEntity
    {
        public long ConfirmationAgreementId { get; set; }
        public long ConstructionProjectId { get; set; }
        public int ConfirmationTypeId { get; set; }
        public string? ConfirmationAgreementTitle { get; set; }
        public string? ConfirmationAgreementDescription { get; set; }
        public string? NewConfirmationAgreementDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ConfirmationAgreementDescription))
                {
                    if (this.ConfirmationAgreementDescription.Length > 50)
                        return this.ConfirmationAgreementDescription.Substring(0, 50);
                    else
                        return this.ConfirmationAgreementDescription;
                }
                else { return string.Empty; }
            }
        }
        public long? ConfirmationAgreementNumber { get; set; }
        public string? ConfirmationAgreementFilePath { get; set; }
        public string? ConfirmationAgreementFileExt { get; set; }
        public int? ConfirmationAgreementFileOrder { get; set; }
        public string? ConfirmationAgreementFileType { get; set; }
        //public bool? IsConfirm { get; set; }
        //public long? UserId { get; set; }
        //public string? ConfirmationTime { get; set; }
        //public DateTime? ConfirmationDate { get; set; }
        //public string PersianConfirmationDate
        //{
        //    get
        //    {
        //        if (ConfirmationDate.HasValue)
        //        {
        //            return PersianDate.PersianDateFrom(ConfirmationDate.Value);
        //        }
        //        else
        //            return "تاریخ ندارد";
        //    }
        //}
        public virtual PersonsVM? PersonsVM { get; set; }
        public virtual ConfirmationTypesVM? ConfirmationTypesVM { get; set; }

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

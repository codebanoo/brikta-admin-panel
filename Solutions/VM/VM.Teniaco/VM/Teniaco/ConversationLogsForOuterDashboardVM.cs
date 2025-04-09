using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Console;
using VM.Projects;

namespace VM.Teniaco
{
    public class ConversationLogsForOuterDashboardVM : BaseEntity
    {
        public long ConversationLogId { get; set; }

        public string? ConversationLogTitle { get; set; }

        public string? ConversationLogDescription { get; set; }

        public string? TableTitle { get; set; }

        public long RecordId { get; set; }

        public DateTime? OperationEnDate { get; set; }

        [NotMapped]
        public string? OperationDateFa
        {
            get
            {
                if (OperationEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(OperationEnDate.Value);
                }

                return null;
            }
        }

        public string? OperationTime { get; set; }

        public long? OperatorUserId { get; set; }

        public long? ReplyConversationLogId { get; set; }

        public bool? IsRead { get; set; }

        public string? ProjectTitle { get; set; }
        public string? ConversationTitle { get; set; }
        public string? SenderName {  get; set; }
        public string? picture {  get; set; }
    }
}

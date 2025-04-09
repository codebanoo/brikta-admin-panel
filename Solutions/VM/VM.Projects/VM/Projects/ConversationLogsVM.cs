using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Console;

namespace VM.Projects
{
    public class ConversationLogsVM : BaseEntity
    {
        public long ConversationLogId { get; set; }

        public string ConversationLogTitle { get; set; }

        public string ConversationLogDescription { get; set; }

        public string TableTitle { get; set; }

        public long RecordId { get; set; }

        public DateTime? OperationEnDate { get; set; }
        public string? OperationDateFa
        {
            get
            {
                if (OperationEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(OperationEnDate.Value);
                }
                else
                {
                    return null;
                }
            }
        }
        public string OperationTime { get; set; }
        public string OperationTimeFa { get; set; }

        public long? OperatorUserId { get; set; }
        public bool? IsRead { get; set; }

        public string? RecordTitle { get; set; }


        //public int? ConversationLogStateId { get; set; }

        public long? ReplyConversationLogId { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }
    }
}

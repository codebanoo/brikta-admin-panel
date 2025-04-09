using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConversationLogs : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConversationLogId { get; set; }

        public string ConversationLogTitle { get; set; }

        public string ConversationLogDescription { get; set; }

        public string TableTitle { get; set; }

        public long RecordId { get; set; }

        public DateTime? OperationEnDate { get; set; }

        public string OperationTime { get; set; }

        public long? OperatorUserId { get; set; }

        //public int? ConversationLogStateId { get; set; }

        public long? ReplyConversationLogId { get; set; }

        public bool IsRead { get; set; }
    }
}

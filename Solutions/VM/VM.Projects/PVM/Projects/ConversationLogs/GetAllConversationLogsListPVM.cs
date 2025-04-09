using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetAllConversationLogsListPVM : BPVM
    {
        public string ConversationLogTitle { get; set; }

        public string ConversationLogDescription { get; set; }

        public string TableTitle { get; set; }

        public long? RecordId { get; set; }
    }
}

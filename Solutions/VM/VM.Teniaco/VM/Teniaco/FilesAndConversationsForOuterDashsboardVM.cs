using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;

namespace VM.Teniaco
{
    public class FilesAndConversationsForOuterDashsboardVM
    {
        public List<ConversationLogsVM> Conversations { get; set; }
        public List<FileStatesLogsVM> Files { get; set; }
    }
}

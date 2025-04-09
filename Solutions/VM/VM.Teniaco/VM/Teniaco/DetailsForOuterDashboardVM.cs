using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;

namespace VM.Teniaco
{
    public class DetailsForOuterDashboardVM
    {
        public int UnverifiedFilesCount { get; set; }
        public int UnreadConversationsCount { get; set; }
        public int NearAdvertisementsCount { get; set; }
        public string? MaxDeviation { get; set; }
        public string? MaxDeviationProjectName { get; set; }
        public string? MinDeviation { get; set; }
        public string? MinDeviationProjectName { get; set; }
        [NotMapped]
        public List<ConversationLogsForOuterDashboardVM> Conversations { get; set; }
        [NotMapped]
        public List<FileStatesLogsForOuterDashboardVM> Files { get; set; }
    }
}

using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;

namespace VM.Teniaco
{
    public class FileStatesLogsForOuterDashboardVM : BaseEntity
    {
        private static readonly Dictionary<string, string> FaTableNames = new()
        {
            {"Attachments","الحاقیه ها" },
            {"InitialPlan" ,"نقشه ها"},
            {"ConfirmationAgreement" ,"تاییدیه ها"},
            {"ContractAgreement" ,"قرارداد های پروژه"},
            {"MeetingBoard" ,"صورت جلسات"},
            {"PartnershipAgreement" ,"درخواست ها"},
            {"PitchDeck" ,"معرفی پروژه"}
        };

        public long FileStateLogId { get; set; }
        public string? TableTitle { get; set; }

        [NotMapped]
        public string FaTableTitle => FaTableNames.TryGetValue(TableTitle, out var faTitle) ? faTitle : TableTitle;

        public long RecordId { get; set; }
        public int FileStateId { get; set; }
        public string? ProjectTitle { get; set; }
        public string? FileTitle { get; set; }
    }
}

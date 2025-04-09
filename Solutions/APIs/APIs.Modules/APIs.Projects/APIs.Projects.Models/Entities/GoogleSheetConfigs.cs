using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Projects.Models.Entities
{
    public partial class GoogleSheetConfigs : BaseEntity
    {
        [Key]
        public long GoogleSheetConfigId { get; set; }
        //public long ConstructionProjectId { get; set; }
        public string GoogleSheetId { get; set; }
        public string ApiKey { get; set; }
        public string ConfigJson { get; set; }
        public string GoogleSheetTitle { get; set; }
        public string GoogleDriveTitle { get; set; }
        public string GoogleDriveId { get; set; }
        public string CredentialsJson { get; set; }
        public string GmailAddress { get; set; }
        public string GmailPassword { get; set; }
    }
}

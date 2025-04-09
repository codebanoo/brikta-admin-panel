using FrameWork;

namespace VM.Projects
{
    public class GoogleSheetConfigsVM : BaseEntity
    {
        public long GoogleSheetConfigId { get; set; }
        //public long? ConstructionProjectId { get; set; }
        public string GoogleSheetId { get; set; }
        public string ApiKey { get; set; }
        public string ConfigJson { get; set; }
        public string? NewConfigJson
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ConfigJson))
                {
                    if (this.ConfigJson.Length > 50)
                        return this.ConfigJson.Substring(0, 50);
                    else
                        return this.ConfigJson;
                }
                else { return string.Empty; }
            }
        }
        public string GoogleSheetTitle { get; set; }
        public string GoogleDriveTitle { get; set; }
        public string GoogleDriveId { get; set; }
        public string CredentialsJson { get; set; }
        public string? NewCredentialsJson
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CredentialsJson))
                {
                    if (this.CredentialsJson.Length > 50)
                        return this.CredentialsJson.Substring(0, 50);
                    else
                        return this.CredentialsJson;
                }
                else { return string.Empty; }
            }
        }
        public string GmailAddress { get; set; }
        public string GmailPassword { get; set; }
    }
}

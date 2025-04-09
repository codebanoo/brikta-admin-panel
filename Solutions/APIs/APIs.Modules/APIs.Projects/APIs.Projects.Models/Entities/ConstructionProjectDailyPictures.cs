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
    public class ConstructionProjectDailyPictures : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConstructionProjectDailyPictureId { get; set; }

        public long ConstructionProjectId { get; set; }

        public string ProjectName { get; set; }

        //public long ConstructionProjectId { get; set; }

        #region Update

        public int UpdateId { get; set; }

        public string? UpdateType { get; set; }

        #endregion

        #region Message

        public int MessageId { get; set; }

        public string? Caption { get; set; }

        public string? Text { get; set; }

        public DateTime? EnDate { get; set; }

        public string? Time { get; set; }

        public string? MessageType { get; set; }

        #endregion

        #region MessageFile

        public string FileName { get; set; }

        public string FileExt { get; set; }

        public string FileType { get; set; }

        #endregion

        #region From

        public string? FromFirstName { get; set; }

        public string? FromLastName { get; set; }

        public long? FromId { get; set; }

        #endregion

        #region Chat

        public long ChatId { get; set; }

        public string? Title { get; set; }

        public string? ChatType { get; set; }

        public string? ChatUsername { get; set; }

        #endregion

        #region Reply

        public int? ReplyMessageId { get; set; }

        public string? ReplyCaption { get; set; }

        public string? ReplyText { get; set; }

        public DateTime? ReplyEnDate { get; set; }

        public string? ReplyTime { get; set; }

        public string? ReplyMessageType { get; set; }

        #endregion

        #region ForwardedFrom

        public string? ForwardedFromFirstName { get; set; }

        public string? ForwardedFromLastName { get; set; }

        public long? ForwardedFromId { get; set; }

        #endregion

        public bool Transferred { get; set; }

        public string SubFolderName { get; set; }

        public bool FileDownloaded { get; set; }
    }
}

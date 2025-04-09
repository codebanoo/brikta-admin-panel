using FrameWork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace APIs.Public.Models.Entities
{
    public class SmsSenders : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SmsSenderId { get; set; }
        public string SmsSenderTitle { get; set; }
        public string SmsSenderNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDefault { get; set; }
    }
}

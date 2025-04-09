using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Console;

namespace VM.Melkavan
{
    public class MelkavanLoginVM : BaseEntity
    {
        //public LoginVM LoginVM { get; set; }


        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string CaptchaCode { get; set; }

        public int DomainSettingId { get; set; }

        public long? ParentUserId { get; set; }

        public CustomUsersVM? CustomUsersVM { get; set; }
    }
}

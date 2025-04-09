using FrameWork;

namespace VM.Public
{
    public class SmsSendersVM : BaseEntity
    {
        public long SmsSenderId { get; set; }
        public string SmsSenderTitle { get; set; }
        public string SmsSenderNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDefault { get; set; }

        //    public List<SmsesVm> SmsesVms { get; set; }
    }
}

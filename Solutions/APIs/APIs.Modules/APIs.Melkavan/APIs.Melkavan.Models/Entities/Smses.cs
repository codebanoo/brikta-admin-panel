using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class Smses : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SmsId { get; set; }
        public long SmsSenderId { get; set; }
        public long UserId { get; set; }

        public string SmsMessage { get; set; }
        public SmsSendingStatus SmsSendingStatus { get; set; }

    }
    public enum SmsSendingStatus
    {
        NotFound = -1,
        WithoutStatus = 0,
        Success = 1,
        NotArrived = 2,
        NotSendToCommunication = 3,
        SendToCommunication = 4,
        CommunicationBlockList = 5,
        SendToOperator = 8,
        CombackCost = 9,
        SendQueue = 10,
        Spam = 11,
        Expire = 12,
        WithoutRoot = 13,
        Rejected = 14,
        NotArrivedToCommunication = 16,
        WithoutDeliveryStatus = 17,

    }
}

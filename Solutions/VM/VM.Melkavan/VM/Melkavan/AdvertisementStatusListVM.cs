using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan.VM.Melkavan
{
    [NotMapped]
    public class AdvertisementStatusListVM
    {
        public string RecordType { get; set; }
        public long AdvertisementId { get; set; }
        public long? UserIdCreator { get; set; }
        public long? ConsultantUserId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusTitle { get; set; }
        public string? RejectionReason {  get; set; }
        public int? AdvertisementTypeId { get; set; }
        public string? AdvertisementTitle { get; set; }
        public int? CountOfMedia { get; set; }
        public DateTime? CreateEnDate { get; set; }
        public string CreateFaDate
        {
            get
            {
                if (CreateEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(CreateEnDate.Value);
                }
                else
                    return "تاریخ ندارد";
            }
        }
        public string AdvertiserDetails
        {
            get
            {
                string Result = Name + " " + Family + "," + Mobile;
                return Result;
            }
        }
        //public string Labels
        //{
        //    get
        //    {
        //        string[] Result = new string[4];
        //        if(TagId != null)
        //        {
        //            if (TagId.Contains("1"))
        //            {
        //                Result.Append("فوری");
        //            }
        //            if (TagId.Contains("2"))
        //            {
        //                Result.Append("مشارکت");
        //            }
        //            if (TagId.Contains("3"))
        //            {
        //                Result.Append("معاوضه");
        //            }
        //            if (TagId.Contains("4"))
        //            {
        //                Result.Append("زیرقیمت");
        //            }
        //            if (TagId.Contains("5"))
        //            {
        //                Result.Append("کارشناسی");
        //            }
        //        }
        //        return string.Join(",", Result);
        //    }
        //}
        public string? AdvertisementDescriptions { get; set; }
        public string? Mobile {  get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }
        public string? TagId { get; set; }
        public long? LastPrice { get; set; }
    }
}

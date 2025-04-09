using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace VM.Melkavan
{
    [NotMapped]
    public class AdvertisementAdvanceSearchVM /*: BaseEntity*/
    {

        //[Key, Column(Order = 0)]
        public string RecordType { get; set; }
        public string? InstagramLink { get; set; }

        public int? StatusId { get; set; }
        public string? RejectionReason { get; set; }

        //[Key, Column(Order = 1)]
        public long AdvertisementId { get; set; }

        public string? AdvertisementTitle { get; set; }

        public bool? ShowInMelkavan { get; set; }

        public long? StateId { get; set; }

        public string? StateName { get; set; }

        public long? CityId { get; set; }

        public string? CityName { get; set; }

        public long? ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public long? DistrictId { get; set; }

        public string? DistrictName { get; set; }

        public string? TownName { get; set; }

        public long? UserIdCreator { get; set; }

        public DateTime? CreateEnDate { get; set; }

        //public DateTime? CurrentDate { get; set; }

        public DateTime? EditEnDate { get; set; }
        public string RegisterOrEditDate
        {
            get
            {
                if (EditEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(EditEnDate.Value);
                }
                else if (CreateEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(CreateEnDate.Value);
                }
                else
                    return "تاریخ ندارد";
            }
        }

        public string UserCreatorName { get; set; }

        public bool? IsActivated { get; set; }
        public bool? IsDeleted { get; set; }
        public int? AdvertisementTypeId { get; set; }

        public long? OfferPrice { get; set; }
        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره

        public string? TagId { get; set; }

        public DateTime? CurrentDate = DateTime.Now;
        public bool? IsFavorite { get; set; }


    

        [NotMapped]
        public virtual List<AdvertisementFilesVM>? AdvertisementFilesVM { get; set; }
    }
}

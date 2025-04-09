using FrameWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Console;
using VM.Melkavan;

namespace VM.Teniaco
{
    [NotMapped]
    public class PropertiesAdvanceSearchVM/*: BaseEntity*/
    {
        //[Key, Column(Order = 0)]
        public string RecordType { get; set; }

        //[Key, Column(Order = 1)]
        public long AdvertisementId { get; set; }

        //نوع ملک
        public int PropertyTypeId { get; set; }

        public string? PropertyTypeTilte { get; set; }

        //نوع کاربری
        public int? TypeOfUseId { get; set; }

        public string? TypeUseTitle { get; set; }

        //نوع سند
        public int? DocumentTypeId { get; set; }

        public string? DocumentTypeTitle { get; set; }

        //نوع مالکیت سند
        public int? DocumentOwnershipTypeId { get; set; }

        public string? DocumentOwnershipTypeTitle { get; set; }

        //نوع ریشه سند
        public int? DocumentRootTypeId { get; set; }

        public string? DocumentRootTypeTitle { get; set; }

        public string? AdvertisementTitle { get; set; }

        public string? Area { get; set; }

        public string? AdvertisementDescriptions { get; set; }

        public int? CountOfMaps { get; set; }

        public int? CountOfDocs { get; set; }

        public int? CountOfMedia { get; set; }

        public int? CountOfPrices { get; set; }

        public bool? ShowInMelkavan { get; set; }        

        //مالک
        public long? OwnerId { get; set; }

        //مشاور
        public long? ConsultantUserId { get; set; }

        //نام مشاور
        public string? ConsultantName { get; set; }

        //آگهی دهنده
        public long? AdvertiserId { get; set; }
        // نام آگهی دهنده
        public string? AdvertiserName { get; set; }


        public long? StateId { get; set; }

        public string? StateName { get; set; }

        public long? CityId { get; set; }

        public string? CityName { get; set; }

        public long? ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public long? DistrictId { get; set; }

        public string? DistrictName { get; set; }


        public int? BuildingLifeId { get; set; }

        //زیر بنا
        public string? Foundation { get; set; }

        public long? UserIdCreator { get; set; }

        public DateTime? CreateEnDate { get; set; }


        //تاریخ بازسازی
        public int? RebuiltInYearFa { get; set; }

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


        public bool? Participable { get; set; }

        public bool? Exchangeable { get; set; }

        [NotMapped]
        public List<PropertyOwnersVM>? PropertyOwnersVM { get; set; }



        [NotMapped]
        public List<AdvertisementOwnersVM>? AdvertisementOwnersVM { get; set; }


        [NotMapped]
        public long? LastPrice { get; set; }

        #region Comments
        ////قیمت پیشنهادی
        //public long? OfferPrice { get; set; }

        ////نوع قیمت پیشنهادی
        ////0 = متری
        ////1 = کل
        //public int? OfferPriceType { get; set; }

        ////قیمت محاسبه شده
        //public long? CalculatedOfferPrice { get; set; }

        //public long? DepositPrice { get; set; }//قیمت ودیعه

        //public long? RentPrice { get; set; }//قیمت اجاره

        //public virtual CustomUsersVM? CustomUsersVM { get; set; }
        #endregion

    }
}









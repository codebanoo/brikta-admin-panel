using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertiesPricesHistories : BaseEntity
    {
        [Key]
        public int PropertyPriceHistoryId { get; set; }

        [ForeignKey("Properties")]
        public long PropertyId { get; set; }

        public long? OfferPrice { get; set; }//قیمت پیشنهادی

        //نوع قیمت پیشنهادی
        //0 = متری
        //1 = کل
        public int? OfferPriceType { get; set; }

        ////نوع ثبت قیمت
        ///0 = اصلاح قیمت قبلی
        ///1 = ثبت قیمت جدید
        public int? PriceTypeRegister { get; set; }

        public long? CalculatedOfferPrice { get; set; }//قیمت محاسبه شده

        public long? DepositPrice { get; set; }//قیمت ودیعه
        public long? RentPrice { get; set; }//قیمت اجاره

        //public long? OwnerPrice { get; set; }//قیمت مالک

        //public long? RepresentativePrice { get; set; }//قیمت املاکی

        //public long? OwnerUnitPrice { get; set; }//قیمت واحد

        //public long? RepresentativeUnitPrice { get; set; }//قیمت واحد

        public virtual Properties Properties { get; set; }
    }
}

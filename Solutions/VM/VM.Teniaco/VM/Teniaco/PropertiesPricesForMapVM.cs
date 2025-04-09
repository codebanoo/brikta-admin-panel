using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    [NotMapped]
    public class PropertiesPricesForMapVM
    {
        [Key]
        public long PropertyId { get; set; }

        public string? PropertyCodeName { get; set; }

        public long? OfferPrice { get; set; }

        public long? CalculatedOfferPrice { get; set; }

        public int? OfferPriceType { get; set; }

        public long? LastPrice { get; set; }
        public long? RentPrice { get; set; }
        public long? DepositPrice { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }

        public long? DistrictId { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public int? TypeOfUseId { get; set; }

        public int PropertyTypeId { get; set; }
        public int AdvertisementTypeId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetListOfAdvertisementWithMoreDetailPVM : BPVM
    {
        public int? AdvertisementTypeId { get; set; }
        public int? PropertyTypeId { get; set; }

        public string PropertyCodeName { get; set; }

        public int? TypeOfUseId { get; set; }

        public int? DocumentTypeId { get; set; }


        public string? NeighborhoodName { get; set; }

        public string Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public long? ConsultantUserId { get; set; }

        public long? OwnerId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }

        public long? DistrictId { get; set; }

        public string? AdvertisementTitle { get; set; }

        //آگهی دهنده
        public long? AdvertiserId { get; set; }

        public bool HaveCallers { get; set; }
        public bool HaveAddress { get; set; }
        public bool HaveFeatures { get; set; }
        public bool HaveFavorites { get; set; }
        public bool HaveViewers { get; set; }
        public bool HaveDetails { get; set; }
        public bool HaveFiles { get; set; }
        public bool HaveTags { get; set; }
    }
}

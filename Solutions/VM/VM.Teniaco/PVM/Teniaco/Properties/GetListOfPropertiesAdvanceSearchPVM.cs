using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfPropertiesAdvanceSearchPVM : BPVM
    {
        //0=ملکاون
        //1=داخلی
        public List<int>? Platform { get; set; }

        public int? PropertyTypeId { get; set; }

        public int? Price { get; set; }

        public double? PriceFrom { get; set; }

        public double? PriceTo { get; set; }

        public int? Area { get; set;}

        public double? AreaFrom { get; set; }

        public double? AreaTo { get; set; }

        public string? Address { get; set; }

        public string? FeaturesAndDesc { get; set; }

        public int? TypeOfUseId { get; set; }

        public int? DocumentTypeId { get; set; }

        public int? DocumentRootTypeId { get; set; }

        public int? DocumentOwnershipTypeId { get; set; }

        public string? PropertyCodeName { get; set; }

        public long? ConsultantUserId { get; set; }

        public long? OwnerId { get; set; }

        public long? InvestorId { get; set; }

        //آگهی دهنده
        public long? AdvertiserId { get; set; }
        public Dictionary<string, string>? Features { get; set; }
        //public List<string>? Features { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }

        public long? DistrictId { get; set; }

        public int? BuildingLifeId { get; set; }

        //زیر بنا
        public int? Foundation { get; set; }

        //تاریخ بازسازی
        public int? RebuiltInYearFa { get; set; }

        public long? ThisUserId { get; set; }

        public bool? Participable { get; set; }

        public bool? Exchangeable { get; set; }
    }
}

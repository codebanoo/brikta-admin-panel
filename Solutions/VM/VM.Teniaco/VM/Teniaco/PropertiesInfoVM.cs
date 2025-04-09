using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class PropertiesInfoVM : BaseEntity
    {
        public long PropertyId { get; set; }
        public int PropertyTypeId { get; set; }
        public int? TypeOfUseId { get; set; }
        public int? DocumentTypeId { get; set; }
        public long? ConsultantUserId { get; set; }
        public string? PropertyCodeName { get; set; }
        public long? OwnerId { get; set; }
        public string? PropertyDescriptions { get; set; }
        public long? UserIdCreator { get; set; }
        public string? Area { get; set; }
        public int? OfferPriceType { get; set; }
        public long? OfferPrice { get; set; }  
        public ProppertyAddressInfo? ProppertyAddressInfo { get; set; } = new ProppertyAddressInfo();
        //  public List<FeatureInfo> FeaturesInfo { get; set; } = new List<FeatureInfo>();
        public List<PropertyFilesVM> PropertyFiles { get; set; } = new List<PropertyFilesVM>();
    }

    public class ProppertyAddressInfo
    {
        public long? StateId { get; set; }

       
        public long? CityId { get; set; }

        public long? ZoneId { get; set; }
        public long? DistrictId { get; set; }
        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }
        
    }
    public class FeatureInfo
    {
        public int FeatureId { get; set; }
        public string FeatureTitle { get; set; }
        public int ElementTypeId { get; set; }
        public bool IsRequired { get; set; }
        public string? FeatureValue { get; set; }
        public string? DefaultValue { get; set; }
        public List<FeatureOptionInfo> FeatureOptionsInfo { get; set; } = new List<FeatureOptionInfo>();
    }
    public class FeatureOptionInfo
    {
        public int FeatureOptionId { get; set; }
        public int FeatureOptionValue { get; set; }
        public string? FeatureOptionText { get; set; }
    }


}

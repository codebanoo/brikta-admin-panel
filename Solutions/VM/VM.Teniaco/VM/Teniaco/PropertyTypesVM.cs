using FrameWork;

namespace VM.Teniaco
{
    public class PropertyTypesVM : BaseEntity
    {
        public PropertyTypesVM()
        {
            FeaturesValuesVM = new HashSet<FeaturesValuesVM>();
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int PropertyTypeId { get; set; }

        public string PropertyTypeTilte { get; set; }

        public virtual ICollection<FeaturesValuesVM> FeaturesValuesVM { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}

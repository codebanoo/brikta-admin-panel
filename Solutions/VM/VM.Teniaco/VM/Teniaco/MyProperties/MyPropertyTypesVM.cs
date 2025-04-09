using FrameWork;


namespace VM.Teniaco
{
    public class MyPropertyTypesVM : BaseEntity
    {
        public MyPropertyTypesVM()
        {
            FeaturesValuesVM = new HashSet<FeaturesValuesVM>();
            MyPropertiesVM = new HashSet<MyPropertiesVM>();
        }

        public int PropertyTypeId { get; set; }

        public string PropertyTypeTilte { get; set; }

        public virtual ICollection<FeaturesValuesVM> FeaturesValuesVM { get; set; }

        public virtual ICollection<MyPropertiesVM> MyPropertiesVM { get; set; }
    }
}

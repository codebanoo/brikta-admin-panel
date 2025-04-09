using FrameWork;

namespace VM.Teniaco
{
    public class PropertyStatesVM : BaseEntity
    {
        public PropertyStatesVM()
        {
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int PropertyStateId { get; set; }

        public string PropertyStateTitle { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}

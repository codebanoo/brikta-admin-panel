using FrameWork;

namespace APIs.Teniaco.Models.Entities
{
    public class PropertiesCallers : BaseEntity
    {
        public int PropertiesCallersId { get; set; }
        public long PropertyId { get; set; }
        public string PropertiesCallerType { get; set; }
    }
}

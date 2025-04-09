using FrameWork;


namespace VM.Teniaco
{
    public class PropertySelectedCallersVM : BaseEntity
    {
        //key
        public int PropertySelectedCallersId { get; set; }

        //properties
        public long PropertyId { get; set; }
        public string? CallerType { get; set; }
        public string? AdvertiserNumber { get; set; }
        public string? AgencyNumber { get; set; }
    }
}

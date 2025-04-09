using FrameWork;

namespace VM.Teniaco
{
    public class InvestorsFavoritesVM : BaseEntity
    {
        public int InvestorFavoiteId { get; set; }

        public long PropertyId { get; set; }

        public int InvestorId { get; set; }

        public virtual InvestorsVM InvestorsVM { get; set; }

        public virtual PropertiesVM PropertiesVM { get; set; }
    }
}

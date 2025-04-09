using FrameWork;

namespace VM.Teniaco
{
    public class IntroductionMethodsVM : BaseEntity
    {
        public IntroductionMethodsVM()
        {
            InvestorsVM = new HashSet<InvestorsVM>();
        }

        public int IntroductionMethodId { get; set; }

        public string IntroductionMethodTitle { get; set; }

        public virtual ICollection<InvestorsVM> InvestorsVM { get; set; }
    }
}

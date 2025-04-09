using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Teniaco
{
    public class ProposedProjectsVM : BaseEntity
    {
        public ProposedProjectsVM()
        {
            InvestorsRequestsVM = new HashSet<InvestorsRequestsVM>();
        }

        public int ProposedProjectId { get; set; }

        public int ProjectId { get; set; }

        public int InvestorId { get; set; }

        public virtual InvestorsVM InvestorsVM { get; set; }

        public virtual ICollection<InvestorsRequestsVM> InvestorsRequestsVM { get; set; }

        //public virtual ProjectsVM ProjectsVM { get; set; }
    }
}

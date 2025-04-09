using FrameWork;
using VM.Console;
using VM.Public;

namespace VM.Teniaco
{
    public class InvestorsVM : BaseEntity
    {

        public int InvestorId { get; set; }
        //public long PersonId { get; set; }
        public long? UserId { get; set; }

        //public InvestorsVM()
        //{
        //    InvestorsFavoritesVM = new HashSet<InvestorsFavoritesVM>();
        //    InvestorsRequestsVM = new HashSet<InvestorsRequestsVM>();
        //    ProposedProjectsVM = new HashSet<ProposedProjectsVM>();
        //}

        //public string? Email { get; set; }

        public int? GuildCategoryId { get; set; }
        public string? Job { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyAddress { get; set; }
        public string? DescriptionOfCompany { get; set; }

        public string? NewDescriptionOfCompany
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DescriptionOfCompany))
                {
                    if (this.DescriptionOfCompany.Length > 50)
                        return this.DescriptionOfCompany.Substring(0, 50);
                    else
                        return this.DescriptionOfCompany;
                }
                else { return string.Empty; }
            }
        }

        //معرف
        public int? RepresentativeId { get; set; }

        //عدد کل سرمایه
        public int? FundId { get; set; }

        //عدد قابل معامله
        public int? TradableNumber { get; set; }

        //شرح سرمایه
        public string? DescriptionOfFund { get; set; }
        public string? NewDescriptionOfFund
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DescriptionOfFund))
                {
                    if (this.DescriptionOfFund.Length > 50)
                        return this.DescriptionOfFund.Substring(0, 50);
                    else
                        return this.DescriptionOfFund;
                }
                else { return string.Empty; }
            }
        }

        //تعداد املاک
        public int? CountOfProperties { get; set; }

        //اشخاص مرتبط
        public string? RelatedPersons { get; set; }

        public string? NewRelatedPersons
        {
            get
            {
                if (!string.IsNullOrEmpty(this.RelatedPersons))
                {
                    if (this.RelatedPersons.Length > 50)
                        return this.RelatedPersons.Substring(0, 50);
                    else
                        return this.RelatedPersons;
                }
                else { return string.Empty; }
            }
        }


        //روحیه تجاری
        public string? BusinessSpirit { get; set; }


        public string? NewBusinessSpirit
        {
            get
            {
                if (!string.IsNullOrEmpty(this.BusinessSpirit))
                {
                    if (this.BusinessSpirit.Length > 50)
                        return this.BusinessSpirit.Substring(0, 50);
                    else
                        return this.BusinessSpirit;
                }
                else { return string.Empty; }
            }
        }
        //روحیه شخصی
        public string? PersonalSpirit { get; set; }

        public string? NewPersonalSpirit
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PersonalSpirit))
                {
                    if (this.PersonalSpirit.Length > 50)
                        return this.PersonalSpirit.Substring(0, 50);
                    else
                        return this.PersonalSpirit;
                }
                else { return string.Empty; }
            }
        }
        //معاملات گذشته
        public string? PastTransactions { get; set; }

        public string? NewPastTransactions
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PastTransactions))
                {
                    if (this.PastTransactions.Length > 50)
                        return this.PastTransactions.Substring(0, 50);
                    else
                        return this.PastTransactions;
                }
                else { return string.Empty; }
            }
        }

        public int? Score{ get; set; }
        public virtual PersonsVM? PersonsVM { get; set; }
        public virtual FundsVM? FundsVM { get; set; }
        public virtual GuildCategoriesVM? GuildCategoriesVM { get; set; }
        public virtual CustomUsersVM? CustomUsersVM { get; set; }


        //public virtual IntroductionMethodsVM IntroductionMethodsVM { get; set; }

        //public virtual ICollection<InvestorsFavoritesVM> InvestorsFavoritesVM { get; set; }

        //public virtual ICollection<InvestorsRequestsVM> InvestorsRequestsVM { get; set; }

        //public virtual ICollection<ProposedProjectsVM> ProposedProjectsVM { get; set; }

    }
}

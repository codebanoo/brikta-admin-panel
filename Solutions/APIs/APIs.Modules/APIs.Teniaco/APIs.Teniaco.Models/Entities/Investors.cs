using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Investors : BaseEntity
    {
        [Key]
        public int InvestorId { get; set; }
        //public long PersonId { get; set; } //persons
        public long? UserId { get; set; } //users
        public int? GuildCategoryId { get; set; } //categories
        public string? Job { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyAddress { get; set; }
        public string? DescriptionOfCompany { get; set; }
        public int? RepresentativeId { get; set; } //persons

        [ForeignKey("Funds")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? FundId { get; set; } //funds
        public int? TradableNumber { get; set; }//funds
        public string? DescriptionOfFund { get; set; }
        public int? CountOfProperties { get; set; }
        public string? RelatedPersons { get; set; }
        public string? BusinessSpirit { get; set; }
        public string? PersonalSpirit { get; set; }
        public string? PastTransactions { get; set; }
        public virtual Funds? Funds { get; set; }
    }
}

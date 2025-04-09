using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Automation.Models.Entities
{
    public partial class MyCompanies : BaseEntity
    {
        public MyCompanies()
        {
            // MyDepartments = new HashSet<MyDepartments>();
        }

        [Key]
        public int MyCompanyId { get; set; }
        public int? ParentMyCompanyId { get; set; }
        public int? OrgChartNodeId { get; set; }
        public int? DomainSettingId { get; set; }
        //[CustomRequired]
        public string MyCompanyName { get; set; }
        public string MyCompanyRealName { get; set; }
        public DateTime? EstablishedEnDate { get; set; }//تاریخ تاسیس
        //public long? MyCompanyDirectorId { get; set; }//آی دی مدیر شرکت
        public string Industry { get; set; }
        public string PositionX { get; set; }
        public string PositionY { get; set; }
        public string Address { get; set; }
        public string Phones { get; set; }
        public string Faxes { get; set; }
        public string CompanyLogo { get; set; }
        public string NationalCode { get; set; }
        public string CompanyMap { get; set; }
        public string Header { get; set; }
        public string RealHeader { get; set; }
        public string Footer { get; set; }
        public string RealFooter { get; set; }
        public string WaterMarkText { get; set; }
        public string WaterMarkImage { get; set; }
        public bool CentralOffice { get; set; }
        public string CommercialCode1 { get; set; }
        public string CommercialCode2 { get; set; }
        public string RegisterNumber { get; set; }
        public string PostalCode { get; set; }
        public int? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public bool HasLimited { get; set; }//just child user seen defined company and so on
        //public ICollection<MyDepartments> MyDepartments { get; set; }
    }
}

using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VM.Automation
{
    public class MyCompaniesVM : BaseEntity
    {
        private string lang = "fa";

        public MyCompaniesVM()
        {
            MyDepartmentsVMList = new HashSet<MyDepartmentsVM>();
        }

        public MyCompaniesVM(string _lang)
        {
            lang = _lang;
            MyDepartmentsVMList = new HashSet<MyDepartmentsVM>();
        }

        public int MyCompanyId { get; set; }

        public int? ParentMyCompanyId { get; set; }

        public int? OrgChartNodeId { get; set; }

        public string Lang { get; set; }

        public int? DomainSettingId { get; set; }

        public string DomainName { get; set; }

        [Required]
        public string MyCompanyName { get; set; }

        public string MyCompanyRealName { get; set; }

        public DateTime? EstablishedEnDate { get; set; }//تاریخ تاسیس

        public string EstablishedDateTime
        {
            get
            {
                if (this.EstablishedEnDate.HasValue)
                    return DateManager.ConvertFromDate(lang, this.EstablishedEnDate.Value);
                else
                    return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(lang) && !string.IsNullOrEmpty(value))
                    this.EstablishedEnDate = DateManager.ConvertToDate(lang, value);
            }
        }

        //public long? MyCompanyDirectorId { get; set; }//آی دی مدیر شرکت

        public string DirectorName { get; set; }
        public string NationalCode { get; set; }

        public string Industry { get; set; }

        public string UserCreatorName { get; set; }

        public string PositionX { get; set; }

        public string PositionY { get; set; }

        public string Address { get; set; }

        public string Phones { get; set; }

        public string Faxes { get; set; }

        public string CompanyLogo { get; set; }

        public string CompanyMap { get; set; }

        public string Header { get; set; }

        public string RealHeader { get; set; }

        public string Footer { get; set; }

        public string RealFooter { get; set; }

        public string WaterMarkText { get; set; }

        public string WaterMarkImage { get; set; }

        public string CommercialCode1 { get; set; }

        public string CommercialCode2 { get; set; }

        public string RegisterNumber { get; set; }

        public string PostalCode { get; set; }

        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public long? StateId { get; set; }

        public string StateName { get; set; }

        public long? CityId { get; set; }

        public string CityName { get; set; }

        public bool CentralOffice { get; set; }

        public ICollection<MyDepartmentsVM> MyDepartmentsVMList { get; set; }
    }
}

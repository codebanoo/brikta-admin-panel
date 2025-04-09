using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllMyPropertiesForInvestorsListPVM : BPVM
    {
        public int? PropertyTypeId { get; set; }

        public string PropertyCodeName { get; set; }

        public int? TypeOfUseId { get; set; }

        public int? DocumentTypeId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }

        public long? DistrictId { get; set; }

        public long? ConsultantUserId { get; set; }

        public long? OwnerId { get; set; }
    }
}

using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectDailyTitles : BaseEntity
    {
        [Key]
        public long ConstructionProjectDailyTitleId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public string CellTitle { get; set; }

        public int CellIndex { get; set; }
    }
}

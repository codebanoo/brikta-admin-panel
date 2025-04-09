using FrameWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Console;
using VM.Public;
using VM.Teniaco.VM.Teniaco;
using VM.Teniaco;

namespace VM.Projects
{
    public class SuggestedConstructionProjectsVM
    {
        public long ConstructionProjectId { get; set; }

        public string? ConstructionProjectTitle { get; set; }

        public DateTime? StartDateEn { get; set; }
        public DateTime? EndDateEn { get; set; }

        public string? MonthsLeftUntilTheEnd { get; set; }

        public string? SuggestionTitle { get; set; }
        public string? Operation { get; set; }

        public ConstructionProjectPriceHistoriesVM? constructionProjectPriceHistoriesVM { get; set; }
        public PropertyAddressVM? propertyAddressVM { get; set; }
        public List<TeniacoSuggestionFilesVM> suggestionFiles { get; set; }

    }
}

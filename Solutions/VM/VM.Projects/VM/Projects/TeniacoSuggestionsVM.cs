using FrameWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class TeniacoSuggestionFilesVM 
    {
        public long? SuggestionFileId { get; set; }
        public long? ConstructionProjectId { get; set; }
        public string? SuggestionFileDescription { get; set; }
        public string? SuggestionFileTitle { get; set; }
        public string? SuggestionFilePath { get; set; }
        public string? SuggestionFileExt { get; set; }
        public string SuggestionFileType { get; set; } = "media";
        public int? SuggestionFileOrder { get; set; }
        public IFormFile? File { get; set; }
    }
}

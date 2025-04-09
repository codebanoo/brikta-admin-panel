using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class TeniacoSuggestionFiles : BaseEntity
    {
        [Key]
        public long SuggestionFileId { get; set; }
        [Required]
        public long ConstructionProjectId { get; set; }
        public string? SuggestionFileDescription {  get; set; }
        public string? SuggestionFileTitle { get; set; }
        public string? SuggestionFilePath { get; set; }
        public string? SuggestionFileExt { get; set; }
        public string SuggestionFileType { get; set; } = "media";
        public int? SuggestionFileOrder { get; set; }
    }
}

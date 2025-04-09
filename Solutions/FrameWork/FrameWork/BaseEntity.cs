using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FrameWork
{
    public partial class BaseEntity
    {
        public long? UserIdCreator { get; set; }
        //[AutoMapper.IgnoreMap()]
        [NotMapped]
        public string UserCreatorName { get; set; }
        public DateTime? CreateEnDate { get; set; }
        public string CreateTime { get; set; }
        public long? UserIdEditor { get; set; }
        public DateTime? EditEnDate { get; set; }
        public string EditTime { get; set; }
        public long? UserIdRemover { get; set; }
        public DateTime? RemoveEnDate { get; set; }
        public string RemoveTime { get; set; }
        public bool? IsActivated { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

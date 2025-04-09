using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class FileStatesVM : BaseEntity
    {
        [Key]
        public int FileStateId { get; set; }

        /// <summary>
        /// 0=آپلود شده
        /// 1=ارسال شده
        /// 2=دیده شده
        /// 3=تایید شده
        /// 4=لغو شده
        /// </summary>
        public string FileStateTitle { get; set; }
    }
}

﻿using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class FileStatesLogs : BaseEntity
    {
        [Key]
        public long FileStateLogId { get; set; }

        public string TableTitle { get; set; }

        public long RecordId { get; set; }

        /// <summary>
        /// 0=آپلود شده
        /// 1=ارسال شده
        /// 2=دیده شده
        /// 3=تایید شده
        /// 4=لغو شده
        /// </summary>
        public int FileStateId { get; set; }
    }
}

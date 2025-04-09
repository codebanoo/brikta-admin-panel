using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace FrameWork
{
    public static class ExcelWriter
    {
        public static Exception WriteDataTableToExcel(DataTable dt, string filePath)
        {
            if ((System.IO.Path.GetExtension(filePath) == ".xls") || (System.IO.Path.GetExtension(filePath) == ".xlsx"))
            {
                try
                {
                    dt.WriteXml(filePath);
                    return null;
                }
                catch (Exception exc)
                {
                    return exc;
                }
            }
            return null;
        }
    }
}
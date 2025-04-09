using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameWork
{
    public static class PreventInjection
    {
        private static string SafeSql(string sql)
        {
            return sql.Replace("'", "''");
        }
    }
}
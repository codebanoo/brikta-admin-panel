using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FrameWork
{
    public static class UrlValidator
    {
        public static bool IsValid(string url)
        {
            string expression = @"^((http|https)://)?([\w-]+\.)+[\w]+(/[\w- ./?]*)?$";

            Match match = Regex.Match(url, expression, RegexOptions.IgnoreCase);
            if (match.Success)
                return true;
            return false;
        }
    }
}
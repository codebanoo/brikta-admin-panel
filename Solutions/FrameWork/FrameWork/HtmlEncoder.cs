using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameWork
{
    public static class HtmlEncoder
    {
        public static string Decode(string html = "")
        {
            // return real html tags
            return HttpUtility.HtmlDecode(html);
        }

        public static string Encode(string html = "")
        {
            //return coded html tags
            return HttpUtility.HtmlEncode(html);
        }
    }
}
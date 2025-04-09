using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FrameWork
{
    public static class UrlEncoder
    {
        public static string Decode(string url = "")
        {
            return HttpUtility.UrlDecode(url);
        }

        public static string Encode(string url = "")
        {
            return HttpUtility.UrlEncode(url);
        }
    }
}
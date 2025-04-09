//using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Util;

namespace FrameWork
{
    public static class AntiXssEncoder
    {
        /// <summary>
        /// add this tag to system.web (<httpRuntime encoderType="AntiXssEncoder, AssemblyName"/> OR 
        /// <httpRuntime encoderType="System.Web.Security.AntiXss.AntiXssEncoder" /> for override methods)
        /// DataAnnotation: [RegularExpression(@"^[^\<\>]*$", ErrorMessage = "May not contain <,>")] OR [RegularExpression(@"^[^\u003C\u003E]*$", ErrorMessage = "...")]
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        //public static string HtmlEncode(string inputString)
        //{
        //    var s = Microsoft.Security.Application.Sanitizer.GetSafeHtml("YourHtml");
        //    return Encoder.HtmlEncode(inputString);
        //}

        //public static string HtmlAttributeEncode(string inputString)
        //{
        //    return Encoder.HtmlAttributeEncode(inputString);
        //}

        //public static string CssEncode(string inputString)
        //{
        //    return Encoder.CssEncode(inputString);
        //}

        //public static string HtmlFormUrlEncode(string inputString)
        //{
        //    return Encoder.HtmlFormUrlEncode(inputString);
        //}

        //public static string JavaScriptEncode(string inputString)
        //{
        //    return Encoder.JavaScriptEncode(inputString);
        //}

        //public static string UrlEncode(string inputString)
        //{
        //    return Encoder.UrlEncode(inputString);
        //}

        //public static string UrlPathEncode(string inputString)
        //{
        //    return Encoder.UrlPathEncode(inputString);
        //}

        //public static string XmlAttributeEncode(string inputString)
        //{
        //    return Encoder.XmlAttributeEncode(inputString);
        //}

        //public static string XmlEncode(string inputString)
        //{
        //    return Encoder.XmlEncode(inputString);
        //}
    }
}
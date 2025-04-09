using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
//using System.Web.Routing;
using System.Net;
using System.Net.Http;
//using System.ServiceModel.Channels;

namespace FrameWork
{
    public static class UserInfo
    {
        //public static string GetIp()
        //{
        //    return HttpContext.Current.Request.UserHostAddress;
        //}

        //public static string GetHostName()
        //{
        //    return HttpContext.Current.Request.UserHostName;
        //}

        //public static string GetUserAgent()
        //{
        //    return HttpContext.Current.Request.UserAgent;
        //}

        //public static RouteData GetUserRouteData()
        //{
        //    return HttpContext.Current.Request.RequestContext.RouteData;
        //}

        //public static NameValueCollection GetUserQueryString()
        //{
        //    return HttpContext.Current.Request.QueryString;
        //}

        //public static string GetContentType()
        //{
        //    return HttpContext.Current.Request.ContentType;
        //}

        //public static string GetApiIpAddress(HttpRequestMessage request)
        //{
        //    if (request.Properties.ContainsKey("MS_HttpContext"))
        //    {
        //        return ((HttpContext)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        //    }
        //    else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
        //    {
        //        RemoteEndpointMessageProperty prop;
        //        prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
        //        return prop.Address;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static string GetVisitorIPAddress(bool getLan = false)
        //{
        //    string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //    if (String.IsNullOrEmpty(visitorIPAddress))
        //        visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        //    if (string.IsNullOrEmpty(visitorIPAddress))
        //        visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

        //    if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
        //    {
        //        getLan = true;
        //        visitorIPAddress = string.Empty;
        //    }

        //    if (getLan)
        //    {
        //        if (string.IsNullOrEmpty(visitorIPAddress))
        //        {
        //            //This is for Local(LAN) Connected ID Address
        //            string stringHostName = Dns.GetHostName();
        //            //Get Ip Host Entry
        //            IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
        //            //Get Ip Address From The Ip Host Entry Address List
        //            IPAddress[] arrIpAddress = ipHostEntries.AddressList;

        //            try
        //            {
        //                visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
        //            }
        //            catch
        //            {
        //                try
        //                {
        //                    visitorIPAddress = arrIpAddress[0].ToString();
        //                }
        //                catch
        //                {
        //                    try
        //                    {
        //                        arrIpAddress = Dns.GetHostAddresses(stringHostName);
        //                        visitorIPAddress = arrIpAddress[0].ToString();
        //                    }
        //                    catch
        //                    {
        //                        visitorIPAddress = "127.0.0.1";
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return visitorIPAddress;
        //}
    }
}
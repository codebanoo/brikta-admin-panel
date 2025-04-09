using Services.Xml;
using Services.Xml.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Public.Areas.UserPublic.ViewComponents
{
    //public class AdminDashboardViewComponent : ViewComponent
    //{
    //    IHostingEnvironment hostEnvironment;
    //    IActionContextAccessor actionContextAccessor;

    //    public AdminDashboardViewComponent(IHostEnvironment _hostEnvironment,
    //        IActionContextAccessor _actionContextAccessor)
    //    {
    //        hostEnvironment = _hostEnvironment;
    //        actionContextAccessor = _actionContextAccessor;
    //    }

    //    public IViewComponentResult Invoke()
    //    {
    //        string direction = "rtl";
    //        string arrow = "left";
    //        string currentLanguage = "fa";
    //        List<Texts> pageTexts = new List<Texts>();
    //        string themeName = "";

    //        string areaName = "";
    //        string controllerName = "";
    //        string actionName = "";

    //        try
    //        {
    //            //if (ViewData["Theme"] != null)
    //            //{
    //            //    theme = ViewData["Theme"] as Themes;
    //            //}
    //            if (ViewData["ThemeName"] != null)
    //            {
    //                themeName = ViewData["ThemeName"] as string;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        if (ViewData["Direction"] != null)
    //        {
    //            direction = ViewData["Direction"] as string;
    //            arrow = direction == "rtl" ? "left" : "right";
    //            ViewData["Arrow"] = arrow;
    //        }

    //        try
    //        {
    //            if (ViewData["CurrentLanguage"] != null)
    //            {
    //                currentLanguage = ViewData["CurrentLanguage"] as string;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        #region get area name from route data

    //        if (actionContextAccessor.ActionContext.RouteData.Values["area"] != null)
    //            areaName = actionContextAccessor.ActionContext.RouteData.Values["area"].ToString();
    //        else
    //            if (actionContextAccessor.ActionContext.RouteData.DataTokens["area"] != null)
    //            areaName = actionContextAccessor.ActionContext.RouteData.DataTokens["area"].ToString();

    //        #endregion

    //        #region get controller name from route data

    //        if (actionContextAccessor.ActionContext.RouteData.Values["controller"] != null)
    //            controllerName = actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString();
    //        else
    //            if (actionContextAccessor.ActionContext.RouteData.DataTokens["controller"] != null)
    //            controllerName = actionContextAccessor.ActionContext.RouteData.DataTokens["controller"].ToString();

    //        #endregion

    //        #region get action name from route data

    //        if (actionContextAccessor.ActionContext.RouteData.Values["action"] != null)
    //            actionName = actionContextAccessor.ActionContext.RouteData.Values["action"].ToString();
    //        else
    //            if (actionContextAccessor.ActionContext.RouteData.DataTokens["action"] != null)
    //            actionName = actionContextAccessor.ActionContext.RouteData.DataTokens["action"].ToString();

    //        #endregion

    //        try
    //        {
    //            if (ViewData["CurrentLanguage"] != null)
    //            {
    //                currentLanguage = ViewData["CurrentLanguage"] as string;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        try
    //        {
    //            if (ViewData["DashboardTexts"] == null)
    //            {
    //                string type = areaName.ToLower() + "dashboard";

    //                TextServices textServices = new TextServices(hostEnvironment,
    //                    areaName,
    //                    "",
    //                    "",
    //                    areaName);

    //                pageTexts = textServices.GetMany(tooltip => tooltip.Type == type, tooltip => tooltip.Language == currentLanguage).ToList();

    //                ViewData["DashboardTexts"] = pageTexts;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        //Themes theme = new Themes();
    //        //if (ViewData["Theme"] != null)
    //        //{
    //        //    theme = ViewData["Theme"] as Themes;
    //        //}
    //        return View(/*theme.ThemeName*/ themeName + /*"ltr"*/direction + "AdminDashboard");
    //    }
    //}
}

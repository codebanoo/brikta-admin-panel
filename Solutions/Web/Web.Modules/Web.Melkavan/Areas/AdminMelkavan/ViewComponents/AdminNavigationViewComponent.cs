using Services.Xml.Entities;
using VM.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Melkavan.Areas.AdminPublic.ViewComponents
{
    //public class AdminNavigationViewComponent : ViewComponent
    //{
    //    IHostingEnvironment hostEnvironment;

    //    public AdminNavigationViewComponent(IHostingEnvironment _hostEnvironment)
    //    {
    //        hostEnvironment = _hostEnvironment;
    //    }

    //    public IViewComponentResult Invoke()
    //    {
    //        string currentLanguage = "fa";
    //        string direction = "rtl";
    //        string arrow = "left";
    //        //Themes theme = new Themes();
    //        string themeName = "";
    //        int? roleId = 0;
    //        int? levelId = 0;
    //        string domain = "";
    //        string roleName = "";

    //        try
    //        {
    //            if (ViewData["Direction"] != null)
    //            {
    //                direction = ViewData["Direction"] as string;
    //                arrow = direction == "rtl" ? "left" : "right";
    //                ViewData["Arrow"] = arrow;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

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

    //        try
    //        {
    //            if (ViewData["RoleId"] != null)
    //            {
    //                roleId = int.Parse(ViewData["RoleId"].ToString());
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        try
    //        {
    //            if (ViewData["LevelId"] != null)
    //            {
    //                levelId = int.Parse(ViewData["LevelId"].ToString());
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        try
    //        {
    //            if (ViewData["Domain"] != null)
    //            {
    //                domain = ViewData["Domain"] as string;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        try
    //        {
    //            if (ViewData["RoleName"] != null)
    //            {
    //                roleName = ViewData["RoleName"] as string;
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

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
    //            string menuFilePath = hostEnvironment.WebRootPath + "\\Files\\TextFiles\\Menus\\" + domain + "\\" +
    //                roleName + "\\" + themeName/*theme.ThemeName*/ + "\\";
    //            string mainMenuFileName = roleId.ToString() + "_" + levelId.ToString() + "_mainmenu_" + currentLanguage + ".txt";

    //            if (System.IO.File.Exists(menuFilePath + mainMenuFileName))
    //            {
    //                if (ViewData["MainMenu"] == null)
    //                {
    //                    ViewData["MainMenu"] = System.IO.File.ReadAllText(menuFilePath + mainMenuFileName);
    //                }
    //            }

    //            string topMenuFileName = roleId.ToString() + "_" + levelId.ToString() + "_topmenu_" + currentLanguage + ".txt";

    //            if (System.IO.File.Exists(menuFilePath + topMenuFileName))
    //            {
    //                if (ViewData["TopMenu"] == null)
    //                {
    //                    ViewData["TopMenu"] = System.IO.File.ReadAllText(menuFilePath + topMenuFileName);
    //                }
    //            }
    //        }
    //        catch (Exception exc)
    //        { }

    //        return View(/*theme.ThemeName*/ themeName + /*"ltr"*/direction + "AdminNavigation");
    //    }
    //}
}

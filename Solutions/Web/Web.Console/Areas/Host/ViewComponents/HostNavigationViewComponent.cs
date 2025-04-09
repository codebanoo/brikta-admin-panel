using Services.Xml.Entities;
using VM.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;

namespace Web.Console.Areas.Host.ViewComponents
{
    public class HostNavigationViewComponent : ViewComponent
    {
        IHostEnvironment hostEnvironment;
        IConsoleBusiness consoleBusiness;

        public HostNavigationViewComponent(IHostEnvironment _hostEnvironment,
            IConsoleBusiness _consoleBusiness)
        {
            hostEnvironment = _hostEnvironment;
            consoleBusiness = _consoleBusiness;
        }

        public IViewComponentResult Invoke()
        {
            int? roleId = 0;
            List<int> roleIds = new List<int>();
            int? levelId = 0;
            List<int> levelIds = new List<int>();
            string domain = "";
            string roleName = "";
            string roleNames = "";

            try
            {
                if (ViewData["RoleId"] != null)
                {
                    roleId = int.Parse(ViewData["RoleId"].ToString());
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["RoleIds"] != null)
                {
                    roleIds = ViewData["RoleIds"].ToString().Split(',').Select(int.Parse).ToList();
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["LevelId"] != null)
                {
                    levelId = int.Parse(ViewData["LevelId"].ToString());
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["LevelIds"] != null)
                {
                    levelIds = ViewData["LevelIds"].ToString().Split(',').Select(int.Parse).ToList();
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["Domain"] != null)
                {
                    domain = ViewData["Domain"] as string;
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["RoleName"] != null)
                {
                    roleName = ViewData["RoleName"] as string;
                }
            }
            catch (Exception exc)
            { }

            try
            {
                if (ViewData["RoleNames"] != null)
                {
                    roleNames = ViewData["RoleNames"] as string;
                }
            }
            catch (Exception exc)
            { }

            if ((roleIds.Count() == 1) && levelIds.Count() == 1)
            {
                try
                {
                    string menuFilePath = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus\\" + domain + "\\" +
                        roleNames + "\\";
                    string mainMenuFileName = roleIds.FirstOrDefault().ToString() + "_" + 
                        levelIds.FirstOrDefault().ToString() + "_mainmenu.txt";

                    if (System.IO.File.Exists(menuFilePath + mainMenuFileName))
                    {
                        if (ViewData["MainMenu"] == null)
                        {
                            ViewData["MainMenu"] = System.IO.File.ReadAllText(menuFilePath + mainMenuFileName);
                        }
                    }

                    string topMenuFileName = roleIds.FirstOrDefault().ToString() + "_" + 
                        levelIds.FirstOrDefault().ToString() + "_topmenu.txt";

                    if (System.IO.File.Exists(menuFilePath + topMenuFileName))
                    {
                        if (ViewData["TopMenu"] == null)
                        {
                            ViewData["TopMenu"] = System.IO.File.ReadAllText(menuFilePath + topMenuFileName);
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            else
            {
                try
                {
                    Dictionary<string, string> strMainMenu = new Dictionary<string, string>();
                    Dictionary<string, string> strTopMenu = new Dictionary<string, string>();

                    List<RolesVM> roles = consoleBusiness.GetRolesWithRoleIds(roleIds);
                    List<LevelsVM> levels = consoleBusiness.GetLevelsWithLevelIds(levelIds);

                    if ((ViewData["MainMenu"] == null) || (ViewData["TopMenu"] == null))
                    {
                        foreach (var role in roles)
                        {
                            string menuFilePath = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus\\" + domain + "\\" +
                                role.RoleName + "\\";

                            foreach (var level in levels)
                            {
                                string mainMenuFileName = role.RoleId.ToString() + "_" + level.LevelId.ToString() + "_mainmenu.txt";
                                if (System.IO.File.Exists(menuFilePath + mainMenuFileName))
                                {
                                    strMainMenu.Add(role.RoleId.ToString() + "_" + level.LevelId.ToString(),
                                        System.IO.File.ReadAllText(menuFilePath + mainMenuFileName));
                                }

                                string topMenuFileName = role.RoleId.ToString() + "_" + level.LevelId.ToString() + "_topmenu.txt";

                                if (System.IO.File.Exists(menuFilePath + topMenuFileName))
                                {
                                    strTopMenu.Add(role.RoleId.ToString() + "_" + level.LevelId.ToString(),
                                        System.IO.File.ReadAllText(menuFilePath + topMenuFileName));
                                }
                            }
                        }

                        ViewData["MainMenu"] = strMainMenu;
                        ViewData["TopMenu"] = strTopMenu;
                        ViewData["Roles"] = roles;
                        ViewData["Levels"] = levels;
                    }
                }
                catch (Exception exc)
                { }
            }

            return View("HostNavigation");
        }
    }
}

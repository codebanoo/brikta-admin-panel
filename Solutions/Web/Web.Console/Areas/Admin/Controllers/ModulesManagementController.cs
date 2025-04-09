using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Controllers;
using CustomAttributes;
using FrameWork;
using Models.Business;
using VM.Console;
using Services.Business;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Web.Core.Ext;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Services.Xml;
using Services.Xml.Entities;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Web.Console.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ModulesManagementController : ExtraAdminController
    {
        public ModulesManagementController(IHostEnvironment _hostEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IMapper _mapper,
            IConsoleBusiness _consoleBusiness,
            IPublicServicesBusiness _publicServicesBusiness,
            IMemoryCache _memoryCache,
            IDistributedCache _distributedCache) :
            base(_hostEnvironment,
            _httpContextAccessor,
            _actionContextAccessor,
            _configurationRoot,
            _mapper,
            _consoleBusiness,
            _publicServicesBusiness,
            _memoryCache,
            _distributedCache)
        {

        }

        public IActionResult Index()
        {
            ViewData["Title"] = "لیست ماژولها";

            return View("Index");
        }

        public IActionResult TemporaryUpdateModules()
        {
            ViewData["Title"] = "بروز رسانی موقت";

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList(this.userId.Value);

            if (ViewData["LevelsList"] == null)
                ViewData["LevelsList"] = consoleBusiness.GetAllLevelsList(this.userId.Value);

            if (ViewData["DomainsSettingsList"] == null)
                ViewData["DomainsSettingsList"] = consoleBusiness.GetAllDomainsSettingsList(this.userId.Value);

            //return View(themeName /*this.theme.ThemeName*/ + "TemporaryUpdateModules");

            return View("Update");
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TemporaryUpdateModules(int slcDomainSettingId,
            bool replaceOldModule,
            bool updateOldModule,
            bool reCreateMenus,
            bool reCreateMenusForUsers,
            string[] slcRoleIds,
            string[] slcLevelIds)
        {
            try
            {
                List<int> levelIds = new List<int>();
                foreach (var levelId in slcLevelIds)
                {
                    levelIds.Add(int.Parse(levelId));
                }

                List<int> roleIds = new List<int>();
                foreach (var roleId in slcRoleIds)
                {
                    roleIds.Add(int.Parse(roleId));
                }

                var roles = consoleBusiness.GetAllRolesList().Where(r => roleIds.Contains(r.RoleId)).ToList();
                var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(slcDomainSettingId, this.userId.Value);

                List<LevelsVM> tmplevels = new List<LevelsVM>();

                foreach (var role in roles)
                {
                    if (levelIds.Count == 0)
                    {
                        tmplevels.AddRange(consoleBusiness.GetAllLevelsList(this.userId.Value,
                        role.RoleId).ToList());
                    }
                    else
                    {
                        tmplevels.AddRange(consoleBusiness.GetAllLevelsList(this.userId.Value,
                        role.RoleId).Where(l => levelIds.Contains(l.LevelId)).ToList());
                    }
                }

                if (levelIds.Count == 0)
                {
                    levelIds = tmplevels.Select(l => l.LevelId).ToList();
                }

                if (replaceOldModule)
                {
                    consoleBusiness.RemoveAllAreas(roleIds);
                }

                var allAreaRootPath = hostEnvironment.ContentRootPath + "\\Areas\\";
                var allAreaDirectories = Directory.GetDirectories(allAreaRootPath);

                foreach (var role in roles)
                {
                    List<LevelsVM> levels = tmplevels.Where(l => l.RoleId.Equals(role.RoleId)).ToList();

                    foreach (var areaDirectory in allAreaDirectories)
                    {
                        if (System.IO.File.Exists(areaDirectory + "\\Accesses.xml"))
                        {
                            AccessServices accessesServices = new AccessServices(hostEnvironment, areaDirectory, "Accesses.xml");
                            var accesses = accessesServices.GetAll().ToList();

                            #region Add All Areas

                            foreach (var access in accesses)
                            {
                                int areaId = 0;
                                string areaName = access.AreaName;
                                string areaNameFa = access.AreaNameFa;
                                int areaSort = access.AreaSort;
                                string areaClass = access.AreaClass;
                                bool showAreaNameInMenu = access.ShowAreaNameInMenu;
                                bool showControllerNameInMenu = access.ShowControllerNameInMenu;
                                int controllerSort = access.ControllerSort;
                                string controllerClass = access.ControllerClass;
                                string roleName = access.RoleName;
                                string controllerName = access.ControllerName;
                                string controllerNameFa = access.ControllerNameFa;
                                bool isActivated = access.IsActivated;
                                bool isInstalledModule = access.IsInstalledModule;
                                bool isDynamicView = access.IsDynamicView;
                                bool showPublicErrorPage = access.ShowPublicErrorPage;
                                bool isStandAlone = access.IsStandAlone;
                                string areaImage = access.AreaImage;
                                string controllerImage = access.ControllerImage;
                                bool hasCharts = access.HasCharts;
                                bool hasNotifications = access.HasNotifications;

                                //if (role.RoleName.Equals("Hosts"))
                                //{
                                //    access.RoleId = consoleBusiness.GetRoleIdWithRoleName("Hosts"/*, domainSetting.DomainName*/);

                                //    areaId = consoleBusiness.GetAreaIdWithAreaNameAndRoleId(areaName, access.RoleId/*access.RoleId*/);
                                //    if (areaId == 0)
                                //    {
                                //        areaId = consoleBusiness.AddToArea(domainSetting.DomainSettingId,
                                //            areaName,
                                //            areaSort,
                                //            role.RoleId,
                                //            areaClass,
                                //            showAreaNameInMenu,
                                //            isActivated,
                                //            isInstalledModule,
                                //            isDynamicView,
                                //            showPublicErrorPage,
                                //            isStandAlone,
                                //            areaImage,
                                //            hasCharts,
                                //            hasNotifications);
                                //    }
                                //    else
                                //    {
                                //        if (updateOldModule)
                                //        {
                                //            consoleBusiness.UpdateArea(areaId,
                                //                areaSort,
                                //                role.RoleId,
                                //                areaName,
                                //                areaClass,
                                //                showAreaNameInMenu,
                                //                isActivated,
                                //                isInstalledModule,
                                //                isDynamicView,
                                //                showPublicErrorPage,
                                //                isStandAlone,
                                //                hasCharts,
                                //                hasNotifications,
                                //                areaImage);
                                //        }
                                //    }

                                //    #region Add All Controllers For This Area

                                //    int controllerId = 0;
                                //    if (areaId != 0)
                                //    {
                                //        controllerId = consoleBusiness.GetControllerIdWithControllerNameAndAreaId(controllerName, areaId);
                                //        if (controllerId == 0)
                                //        {
                                //            controllerId = consoleBusiness.AddToController(areaId,
                                //                controllerName,
                                //                controllerSort,
                                //                controllerClass,
                                //                showControllerNameInMenu,
                                //                isActivated,
                                //                controllerImage);
                                //        }
                                //        else
                                //        {
                                //            if (updateOldModule)
                                //            {
                                //                consoleBusiness.UpdateController(controllerId,
                                //                    areaId,
                                //                    controllerName,
                                //                    controllerSort,
                                //                    controllerClass,
                                //                    showControllerNameInMenu,
                                //                    isActivated,
                                //                    controllerImage);
                                //            }
                                //        }

                                //        #region Add All Actions For This Controller

                                //        if (controllerId != 0)
                                //        {
                                //            List<ListOfActions> listOfActions = new List<ListOfActions>();
                                //            listOfActions = access.ListOfActions.ToList();
                                //            Dictionary<int, string> actionIds = new Dictionary<int, string>();
                                //            //int tmpActionId = 0;
                                //            foreach (var _action in listOfActions)
                                //            {
                                //                int actionId = 0;
                                //                string actionName = _action.ActionName;
                                //                string actionClass = _action.ActionClass;
                                //                int actionSort = _action.ActionSort;
                                //                string styles = _action.Styles;
                                //                string scripts = _action.Scripts;
                                //                string metaTags = _action.MetaTags;
                                //                string descTags = _action.DescTags;
                                //                string queryString = _action.QueryString;
                                //                string queryStringValues = _action.QueryStringValues;
                                //                string relatedActions = _action.RelatedActions;
                                //                string menusGroups = _action.MenusGroups;
                                //                bool actionIsActivated = _action.IsActivated;
                                //                string actionImage = _action.ActionImage;
                                //                bool isAjax = _action.IsAjax;

                                //                actionId = consoleBusiness.GetActionIdWithActionNameAndControllerId(actionName, controllerId);
                                //                if (actionId == 0)
                                //                {
                                //                    actionId = consoleBusiness.AddToAction(controllerId,
                                //                        actionName,
                                //                        actionSort,
                                //                        actionClass,
                                //                        styles,
                                //                        scripts,
                                //                        metaTags,
                                //                        descTags,
                                //                        queryString,
                                //                        queryStringValues,
                                //                        relatedActions,
                                //                        menusGroups,
                                //                        actionIsActivated,
                                //                        actionImage,
                                //                        isAjax);
                                //                }
                                //                else
                                //                {
                                //                    if (updateOldModule)
                                //                    {
                                //                        consoleBusiness.UpdateAction(controllerId,
                                //                        actionId,
                                //                        actionName,
                                //                        actionSort,
                                //                        actionClass,
                                //                        styles,
                                //                        scripts,
                                //                        metaTags,
                                //                        descTags,
                                //                        queryString,
                                //                        queryStringValues,
                                //                        relatedActions,
                                //                        menusGroups,
                                //                        actionIsActivated,
                                //                        actionImage,
                                //                        isAjax);
                                //                    }
                                //                }

                                //                if (reCreateMenus || reCreateMobileMenus)
                                //                {
                                //                    if (actionId != 0)
                                //                    {
                                //                        #region Add Access For This Actions

                                //                        if (access.RoleId.Equals(role.RoleId))
                                //                        {
                                //                            actionIds.Add(actionId, actionName);

                                //                            //foreach (var level in levels)
                                //                            //{
                                //                            //    if (!level.LevelId.Equals(this.levelId.Value))
                                //                            //    {
                                //                            //        if (!consoleBusiness.ExistAccessWithActionIdAndLevelId(actionId, this.levelId.Value))
                                //                            //        {
                                //                            //            consoleBusiness.AddToAccesses(actionId, this.levelId.Value, userId.Value);
                                //                            //        }
                                //                            //    }

                                //                            //    if (!consoleBusiness.ExistAccessWithActionIdAndLevelId(actionId, level.LevelId))
                                //                            //    {
                                //                            //        consoleBusiness.AddToAccesses(actionId, level.LevelId, userId.Value);
                                //                            //    }
                                //                            //}
                                //                        }

                                //                        #endregion
                                //                    }
                                //                }

                                //                //if (actionName.Equals("RemoveUserPicture"))
                                //                //{
                                //                //    tmpActionId = actionId;
                                //                //}
                                //            }

                                //            //if (actionIds.Contains(tmpActionId))
                                //            //{
                                //            //    int i = 0;
                                //            //}

                                //            if (reCreateMenus || reCreateMobileMenus)
                                //            {
                                //                if (actionIds.Count > 0)
                                //                {
                                //                    if (levels.Count > 0)
                                //                        consoleBusiness.AddToAccesses(actionIds,
                                //                            levels.Select(l => l.LevelId).ToList(),
                                //                            //oldActionNames,
                                //                            userId.Value);

                                //                    if (!levels.Select(l => l.LevelId).ToList().Contains(this.levelId.Value))
                                //                        consoleBusiness.AddToAccesses(actionIds.Keys.ToList(),
                                //                            this.levelId.Value,
                                //                            userId.Value);
                                //                }
                                //            }
                                //        }

                                //        #endregion
                                //    }

                                //    #endregion
                                //}
                                //else
                                if (access.RoleName.Equals(role.RoleName))
                                {
                                    access.RoleId = consoleBusiness.GetRoleIdWithRoleName(access.RoleName/*, domainSetting.DomainName*/);

                                    areaId = consoleBusiness.GetAreaIdWithAreaNameAndRoleId(areaName, access.RoleId/*access.RoleId*/);
                                    if (areaId == 0)
                                    {
                                        areaId = consoleBusiness.AddToArea(domainSetting.DomainSettingId,
                                            areaName,
                                            areaNameFa,
                                            areaSort,
                                            role.RoleId,
                                            areaClass,
                                            showAreaNameInMenu,
                                            isActivated,
                                            isInstalledModule,
                                            isDynamicView,
                                            showPublicErrorPage,
                                            isStandAlone,
                                            areaImage,
                                            hasCharts,
                                            hasNotifications);
                                    }
                                    else
                                    {
                                        if (updateOldModule)
                                        {
                                            consoleBusiness.UpdateArea(areaId,
                                                areaSort,
                                                role.RoleId,
                                                areaName,
                                                areaNameFa,
                                                areaClass,
                                                showAreaNameInMenu,
                                                isActivated,
                                                isInstalledModule,
                                                isDynamicView,
                                                showPublicErrorPage,
                                                isStandAlone,
                                                hasCharts,
                                                hasNotifications,
                                                areaImage);
                                        }
                                    }

                                    #region Add All Controllers For This Area

                                    int controllerId = 0;
                                    if (areaId != 0)
                                    {
                                        controllerId = consoleBusiness.GetControllerIdWithControllerNameAndAreaId(controllerName, areaId);
                                        if (controllerId == 0)
                                        {
                                            controllerId = consoleBusiness.AddToController(areaId,
                                                controllerName,
                                                controllerNameFa,
                                                controllerSort,
                                                controllerClass,
                                                showControllerNameInMenu,
                                                isActivated,
                                                controllerImage);
                                        }
                                        else
                                        {
                                            if (updateOldModule)
                                            {
                                                consoleBusiness.UpdateController(controllerId,
                                                    areaId,
                                                    controllerName,
                                                    controllerNameFa,
                                                    controllerSort,
                                                    controllerClass,
                                                    showControllerNameInMenu,
                                                    isActivated,
                                                    controllerImage);
                                            }
                                        }

                                        #region Add All Actions For This Controller

                                        if (controllerId != 0)
                                        {
                                            List<ListOfActions> listOfActions = new List<ListOfActions>();
                                            listOfActions = access.ListOfActions.ToList();
                                            //Dictionary<int, string> actionIds = new Dictionary<int, string>();
                                            List<AccessesWithDataAccessTypesVM> actionIds = new List<AccessesWithDataAccessTypesVM>();
                                            //int tmpActionId = 0;
                                            foreach (var _action in listOfActions)
                                            {
                                                int actionId = 0;
                                                string actionName = _action.ActionName;
                                                string actionNameFa = _action.ActionNameFa;
                                                string actionClass = _action.ActionClass;
                                                int actionSort = _action.ActionSort;
                                                string styles = _action.Styles;
                                                string scripts = _action.Scripts;
                                                string metaTags = _action.MetaTags;
                                                string descTags = _action.DescTags;
                                                string queryString = _action.QueryString;
                                                string queryStringValues = _action.QueryStringValues;
                                                string relatedActions = _action.RelatedActions;
                                                string menusGroups = _action.MenusGroups;
                                                bool actionIsActivated = _action.IsActivated;
                                                string actionImage = _action.ActionImage;
                                                bool isAjax = _action.IsAjax;
                                                string dataAccessTypes = _action.DataAccessTypes;

                                                actionId = consoleBusiness.GetActionIdWithActionNameAndControllerId(actionName, controllerId);
                                                if (actionId == 0)
                                                {
                                                    actionId = consoleBusiness.AddToAction(controllerId,
                                                        actionName,
                                                        actionNameFa,
                                                        actionSort,
                                                        actionClass,
                                                        styles,
                                                        scripts,
                                                        metaTags,
                                                        descTags,
                                                        queryString,
                                                        queryStringValues,
                                                        relatedActions,
                                                        menusGroups,
                                                        actionIsActivated,
                                                        actionImage,
                                                        isAjax);
                                                }
                                                else
                                                {
                                                    if (updateOldModule)
                                                    {
                                                        consoleBusiness.UpdateAction(controllerId,
                                                        actionId,
                                                        actionName,
                                                        actionNameFa,
                                                        actionSort,
                                                        actionClass,
                                                        styles,
                                                        scripts,
                                                        metaTags,
                                                        descTags,
                                                        queryString,
                                                        queryStringValues,
                                                        relatedActions,
                                                        menusGroups,
                                                        actionIsActivated,
                                                        actionImage,
                                                        isAjax);
                                                    }
                                                }

                                                if (reCreateMenus)
                                                {
                                                    if (actionId != 0)
                                                    {
                                                        #region Add Access For This Actions

                                                        if (access.RoleId.Equals(role.RoleId))
                                                        {
                                                            //actionIds.Add(actionId, actionName);

                                                            int dataAccessTypeId = 1;

                                                            switch (dataAccessTypes)
                                                            {
                                                                case "selfandchilds"://1
                                                                    dataAccessTypeId = 1;
                                                                    break;
                                                                case "justself"://2
                                                                    dataAccessTypeId = 2;
                                                                    break;
                                                                case "justchilds"://3
                                                                    dataAccessTypeId = 3;
                                                                    break;
                                                                case "justparent"://4
                                                                    dataAccessTypeId = 4;
                                                                    break;
                                                                case "parentandchilds"://5
                                                                    dataAccessTypeId = 5;
                                                                    break;
                                                                case "departments"://6
                                                                    dataAccessTypeId = 6;
                                                                    break;
                                                                case "domainadmin"://7
                                                                    dataAccessTypeId = 7;
                                                                    break;
                                                                case "domainadminandchilds"://8
                                                                    dataAccessTypeId = 8;
                                                                    break;
                                                                case "samelevel"://9
                                                                    dataAccessTypeId = 9;
                                                                    break;
                                                                case "samelevelsandchilds"://10
                                                                    dataAccessTypeId = 10;
                                                                    break;
                                                                case "all"://11
                                                                    dataAccessTypeId = 11;
                                                                    break;
                                                            }

                                                            actionIds.Add(new AccessesWithDataAccessTypesVM()
                                                            {
                                                                ActionId = actionId,
                                                                ActionName = actionName,
                                                                DataAccessTypes = dataAccessTypes,
                                                                DataAccessTypeId = dataAccessTypeId
                                                            });

                                                            //foreach (var level in levels)
                                                            //{
                                                            //    if (!level.LevelId.Equals(this.levelId.Value))
                                                            //    {
                                                            //        if (!consoleBusiness.ExistAccessWithActionIdAndLevelId(actionId, this.levelId.Value))
                                                            //        {
                                                            //            consoleBusiness.AddToAccesses(actionId, this.levelId.Value, userId.Value);
                                                            //        }
                                                            //    }

                                                            //    if (!consoleBusiness.ExistAccessWithActionIdAndLevelId(actionId, level.LevelId))
                                                            //    {
                                                            //        consoleBusiness.AddToAccesses(actionId, level.LevelId, userId.Value);
                                                            //    }
                                                            //}
                                                        }

                                                        #endregion
                                                    }
                                                }

                                                //if (actionName.Equals("RemoveUserPicture"))
                                                //{
                                                //    tmpActionId = actionId;
                                                //}
                                            }

                                            //if (actionIds.Contains(tmpActionId))
                                            //{
                                            //    int i = 0;
                                            //}

                                            if (reCreateMenus)
                                            {
                                                if (actionIds.Count > 0)
                                                {
                                                    if (levels.Count > 0)
                                                        consoleBusiness.AddToAccesses(actionIds,
                                                            levels.Select(l => l.LevelId).ToList(),
                                                            //1,
                                                            //oldActionNames,
                                                            userId.Value);

                                                    if (!levels.Select(l => l.LevelId).ToList().Contains(this.levelId.Value))
                                                        consoleBusiness.AddToAccesses(actionIds,
                                                            this.levelIds,
                                                            //1,
                                                            userId.Value);
                                                }
                                            }
                                        }

                                        #endregion
                                    }

                                    #endregion
                                }
                            }

                            #endregion
                        }
                    }
                }

                //message += "5,";

                if (reCreateMenus)
                {
                    //message += "6,";
                    //consoleBusiness.RemoveAllMenus(/*slcThemes.ToList(), */roleIds);

                    string rootMenuFolder = "";
                    string rootDomainNameMenuFolder = "";

                    string rootMobileMenuFolder = "";
                    string rootDomainNameMobileMenuFolder = "";

                    if (reCreateMenus)
                    {
                        #region Create Needed Folders For Menus

                        rootMenuFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus";
                        System.IO.Directory.CreateDirectory(rootMenuFolder + "\\" + domain);
                        System.Threading.Thread.Sleep(100);
                        rootDomainNameMenuFolder = rootMenuFolder + "\\" + domain;

                        #endregion
                    }

                    foreach (var role in roles)
                    {
                        if (reCreateMenus)
                        {
                            System.IO.Directory.CreateDirectory(rootDomainNameMenuFolder + "\\" + role.RoleName);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (reCreateMenus)
                        {
                            System.IO.Directory.CreateDirectory(rootDomainNameMenuFolder + "\\" + role.RoleName);
                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    //message += "10,";

                    #region Fill Menu Table From Areas And Controllers And Actions

                    //foreach (var theme in themes)
                    //{
                    //foreach (var Language in Languages)
                    //{

                    //    #region Get All Areas

                    //    List<AreasVM> areasVM = consoleBusiness.GetAllAreas().Where(a => roleIds.Contains(a.RoleId)).
                    //        Where(a => a.IsActivated.Equals(true)).ToList();
                    //    foreach (var areaVM in areasVM)
                    //    {

                    //        List<string> menusGroups = new ThemeServices(hostEnvironment).Get(t => t.ThemeName == theme.ThemeName).
                    //            MenusGroups.Split(new char[] { ',' }).ToList();

                    //        var levels = consoleBusiness.GetAllLevelsList(this.userId.Value, /*domainSetting.DomainSettingId,*/ 0).
                    //            Where(l => roleIds.Contains(l.RoleId)).ToList();

                    //        foreach (var level in levels)
                    //        {
                    //            if (consoleBusiness.ExistActionInAreaForMenu(/*domain, */areaVM.AreaId, level.LevelId/*, menusGroups*/))
                    //            {

                    //                foreach (var menuGroup in menusGroups)
                    //                {

                    //                    long areaMenuId = 0;
                    //                    long controllerMenuId = 0;

                    //                    string link = "/" + areaVM.AreaName;

                    //                    if (!consoleBusiness.ExistMenuWithLink(link, /*domain, */Language.Language, level.LevelId/*, theme.ThemeName*/))
                    //                    {

                    //                        #region Add To Menu For This Area

                    //                        MenusVM menusVMForArea = new MenusVM();
                    //                        menusVMForArea.CreateEnDate = DateTime.Now;
                    //                        menusVMForArea.CreateTime = PersianDate.TimeNow;
                    //                        menusVMForArea.CssClass = areaVM.AreaClass;
                    //                        //menusVMForArea.DomainSettingId = domainSetting.DomainSettingId;
                    //                        //menusVMForArea.MenusGroups = menuGroup;
                    //                        menusVMForArea.IsActivated = areaVM.IsActivated;
                    //                        menusVMForArea.IsPanelMenu = true;
                    //                        menusVMForArea.Lang = Language.Language;
                    //                        menusVMForArea.LevelId = level.LevelId;
                    //                        menusVMForArea.Link = link;
                    //                        menusVMForArea.MenuImage = areaVM.AreaImage;
                    //                        menusVMForArea.MenuOrder = areaVM.AreaSort;
                    //                        menusVMForArea.MenusGroups = "";
                    //                        menusVMForArea.Show = areaVM.ShowAreaNameInMenu;
                    //                        try
                    //                        {
                    //                            menusVMForArea.Title = new TextServices(hostEnvironment, areaVM.AreaName,
                    //                                "",
                    //                                "",
                    //                                areaVM.AreaName).Get(t => t.PropertyName == "AreaTitle",
                    //                                t => t.Language == Language.Language).Value;
                    //                        }
                    //                        catch (Exception exc)
                    //                        { }
                    //                        menusVMForArea.UserIdCreator = userId.Value;
                    //                        //menusVMForArea.ThemeName = theme.ThemeName;
                    //                        menusVMForArea.MenuType = "area";

                    //                        areaMenuId = consoleBusiness.AddToMenu(menusVMForArea);

                    //                        #endregion

                    //                    }
                    //                    else
                    //                    {

                    //                        #region Get Area Menu Id

                    //                        areaMenuId = consoleBusiness.GetMenuIdWithLink(link,
                    //                            /*domain, */Language.Language,
                    //                            level.LevelId/*,
                    //                            theme.ThemeName*/);

                    //                        #endregion

                    //                    }

                    //                    #region Get All Controllers

                    //                    List<ControllersVM> controllersVM = consoleBusiness.
                    //                        GetAllControllersInViewWithAreaId(areaVM.AreaId).
                    //                        Where(c => c.IsActivated == true).ToList();
                    //                    foreach (var controllerVM in controllersVM)
                    //                    {

                    //                        if (consoleBusiness.ExistActionInControllerForMenu(/*domain, */controllerVM.ControllerId,
                    //                            level.LevelId,
                    //                            menusGroups))
                    //                        {

                    //                            link = "/" + areaVM.AreaName +
                    //                                "/" + controllerVM.ControllerName;

                    //                            if (!consoleBusiness.ExistMenuWithLink(link,
                    //                                /*domain, */Language.Language,
                    //                                level.LevelId/*,
                    //                                theme.ThemeName*/))
                    //                            {

                    //                                #region Add To Menu For This Controller

                    //                                MenusVM menusVMForController = new MenusVM();
                    //                                menusVMForController.CreateEnDate = DateTime.Now;
                    //                                menusVMForController.CreateTime = PersianDate.TimeNow;
                    //                                menusVMForController.CssClass = controllerVM.ControllerClass;
                    //                                //menusVMForController.DomainSettingId = domainSetting.DomainSettingId;
                    //                                menusVMForController.MenusGroups = menuGroup;
                    //                                menusVMForController.IsActivated = controllerVM.IsActivated;
                    //                                menusVMForController.IsPanelMenu = true;
                    //                                menusVMForController.Lang = Language.Language;
                    //                                menusVMForController.LevelId = level.LevelId;
                    //                                menusVMForController.Link = link;
                    //                                menusVMForController.MenuImage = controllerVM.ControllerImage;
                    //                                menusVMForController.MenuOrder = controllerVM.ControllerSort;
                    //                                menusVMForController.MenusGroups = "";
                    //                                menusVMForController.ParentId = areaMenuId;
                    //                                menusVMForController.Show = controllerVM.ShowControllerNameInMenu;
                    //                                try
                    //                                {
                    //                                    menusVMForController.Title = new TextServices(hostEnvironment, areaVM.AreaName,
                    //                                        controllerVM.ControllerName,
                    //                                        "",
                    //                                        controllerVM.ControllerName).Get(t => t.PropertyName == "ControllerTitle",
                    //                                        t => t.Language == Language.Language).Value;
                    //                                }
                    //                                catch (Exception exc)
                    //                                { }
                    //                                menusVMForController.UserIdCreator = userId.Value;
                    //                                //menusVMForController.ThemeName = theme.ThemeName;
                    //                                menusVMForController.MenuType = "controller";

                    //                                controllerMenuId = consoleBusiness.AddToMenu(menusVMForController);

                    //                                #endregion

                    //                            }
                    //                            else
                    //                            {

                    //                                #region Get Controller Menu Id

                    //                                controllerMenuId = consoleBusiness.GetMenuIdWithLink(link,
                    //                                    /*domain, */Language.Language,
                    //                                    level.LevelId/*,
                    //                                    theme.ThemeName*/);

                    //                                #endregion

                    //                            }

                    //                            #region Get All Actions

                    //                            List<ActionsVM> actionsVM = consoleBusiness.GetAllActionInControllersWithControllerId(controllerVM.ControllerId).
                    //                                Where(a => a.IsActivated.Equals(true) && a.MenusGroups.Equals(menuGroup)).ToList();
                    //                            foreach (var actionVM in actionsVM)
                    //                            {

                    //                                if (consoleBusiness.ExistActionForMenu(/*domain, */actionVM.ActionId,
                    //                                    level.LevelId,
                    //                                    menusGroups))
                    //                                {

                    //                                    link = "/" + areaVM.AreaName +
                    //                                        "/" + controllerVM.ControllerName +
                    //                                        "/" + actionVM.ActionName;

                    //                                    if (!consoleBusiness.ExistMenuWithLink(link,
                    //                                        /*domain, */Language.Language,
                    //                                        level.LevelId/*,
                    //                                        theme.ThemeName*/))
                    //                                    {

                    //                                        #region Add To Menu For This Action

                    //                                        MenusVM menusVMForAction = new MenusVM();
                    //                                        menusVMForAction.CreateEnDate = DateTime.Now;
                    //                                        menusVMForAction.CreateTime = PersianDate.TimeNow;
                    //                                        menusVMForAction.CssClass = actionVM.ActionClass;
                    //                                        //menusVMForAction.DomainSettingId = domainSetting.DomainSettingId;
                    //                                        menusVMForAction.IsActivated = actionVM.IsActivated;
                    //                                        menusVMForAction.IsPanelMenu = true;
                    //                                        menusVMForAction.Lang = Language.Language;
                    //                                        menusVMForAction.LevelId = level.LevelId;
                    //                                        menusVMForAction.Link = link;
                    //                                        menusVMForAction.MenuImage = actionVM.ActionImage;
                    //                                        menusVMForAction.MenuOrder = actionVM.ActionSort;
                    //                                        menusVMForAction.MenusGroups = menuGroup;
                    //                                        menusVMForAction.ParentId = controllerMenuId;
                    //                                        menusVMForAction.Show = actionVM.IsActivated;
                    //                                        string type = areaVM.AreaName + controllerVM.ControllerName + actionVM.ActionName;
                    //                                        type = type.ToLower();
                    //                                        try
                    //                                        {
                    //                                            menusVMForAction.Title = new TextServices(hostEnvironment, areaVM.AreaName,
                    //                                                controllerVM.ControllerName,
                    //                                                actionVM.ActionName,
                    //                                                controllerVM.ControllerName).Get(t => t.PropertyName == "PageTitle",
                    //                                                t => t.Language == Language.Language, t => t.Type == type).Value;
                    //                                        }
                    //                                        catch (Exception exc)
                    //                                        { }
                    //                                        menusVMForAction.UserIdCreator = userId.Value;
                    //                                        //menusVMForAction.ThemeName = theme.ThemeName;
                    //                                        menusVMForAction.MenuType = "action";

                    //                                        consoleBusiness.AddToMenu(menusVMForAction);

                    //                                        #endregion

                    //                                    }

                    //                                }

                    //                            }

                    //                            #endregion

                    //                        }

                    //                    }

                    //                    #endregion

                    //                }

                    //            }
                    //        }

                    //    }

                    //    #endregion

                    //}
                    //}

                    #endregion

                    #region Create Panel And Mobile Menu Files

                    #region For All Active Themes

                    foreach (var role in roles)
                    {
                        //var levelIds = business.GetAllLevelsList(0, /*0, */role.RoleId).Select(l => l.LevelId);

                        var levels = consoleBusiness.GetAllLevelsList(this.userId.Value, /*0, */role.RoleId);

                        if (levelIds.Count == 0)
                        {
                            levelIds = levels.Select(l => l.LevelId).ToList();
                        }

                        foreach (var levelId in levelIds)
                            if (reCreateMenus)
                            {
                                var level = levels.Where(l => l.LevelId.Equals(levelId)).FirstOrDefault();

                                PanelMenus.Create(hostEnvironment, consoleBusiness.CmsDb,
                                    level,
                                    role,
                                    domainSetting.DomainName,
                                    true);
                            }
                    }

                    #endregion

                    #endregion
                }

                return Json(new
                {
                    Result = "OK",
                    Message = "Success"// + " " + message
                });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"// + " " + message
            });
        }
    }
}
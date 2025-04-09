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

using Services.Xml.Entities;
using Services.Xml;
using Web.Core.Ext;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Newtonsoft.Json;

namespace Web.Console.Areas.Host.Controllers
{
    [Area("Host")]
    public class LevelsManagementController : ExtraHostController
    {
        public LevelsManagementController(IHostEnvironment _hostEnvironment,
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
            //if (ViewData["DomainsSettingsList"] == null)
            //    ViewData["DomainsSettingsList"] = business.GetAllDomainsSettingsList();

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList(this.userId.Value);

            if (ViewData["DefaultPagesList"] == null)
                ViewData["DefaultPagesList"] = "";//consoleBusiness.GetDefaultPages/*WithDomainName*/(/*domain, */"fa");

            //return View(themeName /*this.theme.ThemeName*/ + "Index");

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfLevels(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
            int domainSettingIdSearch = 0,
            string levelNameSearch = null,
            int roleIdSearch = 0)
        {
            try
            {
                int listCount = 0;

                var levelsList = consoleBusiness.GetLevelsList(jtStartIndex,
                    jtPageSize,
                    ref listCount,
                    jtSorting,
                    //domainSettingIdSearch,
                    levelNameSearch,
                    roleIdSearch,
                    this.userId);

                return Json(new { Result = "OK", Records = levelsList, TotalRecordCount = listCount });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToLevels(LevelsVM levelVM)
        {
            string returnMessage = "";

            try
            {
                //if (this.roleId.Value.Equals(levelVM.RoleId))
                //    levelVM.UserIdCreator = this.userId.Value;
                //else
                //{
                //    levelVM.UserIdCreator = business.GetDomainAdminId(this.domainsSettings.DomainSettingId);
                //}
                //var domainSetting = business.GetAllDomainsSettingsList(this.userId.Value).FirstOrDefault(d => d.DomainSettingId.Equals(levelVM.DomainSettingId));
                //if (domainSetting != null)
                //    levelVM.UserIdCreator = domainSetting.UserIdCreator.Value;

                levelVM.UserIdCreator = this.userId.Value;
                levelVM.CreateEnDate = DateTime.Now;
                levelVM.CreateTime = PersianDate.TimeNow;
                //levelVM.UserIdCreator = this.userId.Value;

                if (ModelState.IsValid)
                {
                    levelVM.LevelId = consoleBusiness.AddToLevels(levelVM, ref returnMessage);

                    if (levelVM.LevelId > 0)
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "Success",
                            Record = levelVM
                        });
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = !string.IsNullOrEmpty(returnMessage) ? returnMessage : "ErrorMessage",
                //pageTexts.Where(t => t.PropertyName == returnMessage).FirstOrDefault().Value :
                //pageTexts.Where(t => t.PropertyName == "ErrorMessage").FirstOrDefault().Value,
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateLevel(LevelsVM levelVM)
        {
            string returnMessage = "";
            try
            {
                //levelVM.UserIdCreator = this.userId.Value;
                levelVM.EditEnDate = DateTime.Now;
                levelVM.EditTime = PersianDate.TimeNow;
                levelVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateLevel(levelVM, this.userId.Value, ref returnMessage))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Record = levelVM
                        });
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = !string.IsNullOrEmpty(returnMessage) ? returnMessage : "ErrorMessage"
                //pageTexts.Where(t => t.PropertyName == returnMessage).FirstOrDefault().Value :
                //pageTexts.Where(t => t.PropertyName == "ErrorMessage").FirstOrDefault().Value,
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationLevel(int LevelId = 0)
        {
            try
            {
                if (consoleBusiness.ToggleActivationLevel(LevelId, this.userId.Value))
                {
                    return Json(new
                    {
                        Result = "OK",
                    });
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleTemporaryDeleteLevel(int LevelId = 0)
        {
            try
            {
                if (consoleBusiness.ToggleTemporaryDeleteLevel(LevelId, this.userId.Value))
                {
                    return Json(new
                    {
                        Result = "OK",
                    });
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult CompleteDeleteLevel(int LevelId = 0)
        {
            try
            {
                if (LevelId != this.levelId.Value)
                {
                    if (consoleBusiness.CompleteDeleteLevel(LevelId, this.userId.Value))
                    {
                        return Json(new
                        {
                            Result = "OK",
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentLevelErrorMessage"
                    });
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult LevelDetails(int Id = 0)
        {
            if (consoleBusiness.CheckLevelId/*InDomain*/(Id,
                this.userId.Value,
                this.parentUserId.Value,
                this.areaName,
                this.controllerName,
                this.actionName,
                this.domainsSettings,
                this.roleIds,
                this.levelIds
                /*, domain*/))
            {
                if (ViewData["DataAccessTypesList"] == null)
                    ViewData["DataAccessTypesList"] = consoleBusiness.GetAllDataAccessTypesList();

                bool justloadForCurrentLevel = false;
                LevelDetailsVM levelDetailsVM = consoleBusiness.GetLevelDetailsWithLevelIdForAccesses(Id,
                    this.userId.Value,
                    "fa",
                    justloadForCurrentLevel);
                if (levelDetailsVM.AccessesIdsVMList.Count > 0)
                {
                    levelDetailsVM.AccessesWithDataAccessTypesVMList = new List<AccessesWithDataAccessTypesVM>();

                    foreach (var access in levelDetailsVM.AccessesIdsVMList)
                    {
                        if (access.LevelId.Equals(Id))
                            levelDetailsVM.AccessesWithDataAccessTypesVMList.Add(new AccessesWithDataAccessTypesVM()
                            {
                                ActionId = access.ActionId,
                                DataAccessTypeId = access.DataAccessTypeId,
                            });

                        //if (access.LevelId.Equals(Id))
                        //    levelDetailsVM.ActionIds += access.ActionId + ",";
                    }
                    //if (!string.IsNullOrEmpty(levelDetailsVM.ActionIds))
                    //    levelDetailsVM.ActionIds = StringModify.RemoveLastCharacters(levelDetailsVM.ActionIds, 1);
                }
                //return View(themeName /*this.theme.ThemeName*/ + "LevelDetails", levelDetailsVM);

                if (ViewData["DataBackUrl"] == null)
                {
                    ViewData["DataBackUrl"] = "/Host/LevelsManagement/Index";
                }

                dynamic expando = new ExpandoObject();
                expando = levelDetailsVM;

                return View("Update", levelDetailsVM);
            }

            return RedirectToAction("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult LevelDetails(int LevelId,
            bool reCreateMenus,
            bool reCreateMobileMenus,
            bool reCreateMenusForUsers,
            string strAccessesWithDataAccessTypesVMList
            //string strAccesses
            /*List<AccessesWithDataAccessTypesVM> accessesWithDataAccessTypesVMList*/)
        {
            try
            {
                List<AccessesWithDataAccessTypesVM> accessesWithDataAccessTypesVMList = new List<AccessesWithDataAccessTypesVM>();

                accessesWithDataAccessTypesVMList = JsonConvert.DeserializeObject<List<AccessesWithDataAccessTypesVM>>(strAccessesWithDataAccessTypesVMList);

                if (consoleBusiness.CheckLevelId/*InDomain*/(LevelId, this.userId.Value,
                        this.parentUserId.Value,
                        this.areaName,
                        this.controllerName,
                        this.actionName,
                        this.domainsSettings,
                        this.roleIds,
                        this.levelIds
                    /*, domain*/))
                    if (accessesWithDataAccessTypesVMList != null)
                    {
                        if (accessesWithDataAccessTypesVMList.Count > 0)
                        {
                            if (consoleBusiness.RemoveFromAccessesWithLevelId(LevelId))
                                if (consoleBusiness.AddListToAccesses(accessesWithDataAccessTypesVMList, LevelId, this.userId.Value))
                                {
                                    if (reCreateMenus || reCreateMobileMenus)
                                    {
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

                                        if (reCreateMobileMenus)
                                        {
                                            #region Create Needed Folders For Mobile Menus

                                            rootMobileMenuFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\MobileMenus";
                                            System.IO.Directory.CreateDirectory(rootMobileMenuFolder + "\\" + domain);
                                            System.Threading.Thread.Sleep(100);
                                            rootDomainNameMobileMenuFolder = rootMobileMenuFolder + "\\" + domain;

                                            #endregion
                                        }

                                        var role = consoleBusiness.GetRoleWithLevelId(LevelId);

                                        if (reCreateMenus)
                                        {
                                            System.IO.Directory.CreateDirectory(rootDomainNameMenuFolder + "\\" + role.RoleName);
                                            System.Threading.Thread.Sleep(100);

                                            //System.IO.Directory.CreateDirectory(rootDomainNameMenuFolder + "\\" + role.RoleName);
                                            //System.Threading.Thread.Sleep(100);
                                        }

                                        if (reCreateMobileMenus)
                                        {
                                            System.IO.Directory.CreateDirectory(rootDomainNameMobileMenuFolder + "\\" + role.RoleName);
                                            System.Threading.Thread.Sleep(100);

                                            //System.IO.Directory.CreateDirectory(rootDomainNameMobileMenuFolder + "\\" + role.RoleName);
                                            //System.Threading.Thread.Sleep(100);
                                        }

                                        #region Create Panel And Mobile Menu Files

                                        #region For All Active Themes

                                        var level = consoleBusiness.GetLevelDetailWithLevelId(LevelId);


                                        if (reCreateMenus)
                                        {

                                            PanelMenus.Create(hostEnvironment, consoleBusiness.CmsDb,
                                                level,
                                                role,
                                                this.domain,
                                                true);
                                        }

                                        #endregion

                                        #endregion
                                    }

                                    return Json(new
                                    {
                                        Result = "OK",
                                        Message = "SuccessAssignAccesses"
                                    });
                                }
                        }
                        else
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "NoAssignAccesses"
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "NoAssignAccesses"
                        });
                    }
            }
            catch (Exception exc)
            { }
            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult LevelShortCutImages(int Id = 0)
        {
            //if (consoleBusiness.CheckLevelId/*InDomain*/(Id, this.userId.Value/*, domain*/))
            //{
            //    List<MenusVM> menusVMList = consoleBusiness.GetMenusWithLevelIdForLevelShortCutImages(Id,
            //        this."fa"/*, 
            //        "sb_admin_2"*/);
            //    if (menusVMList.Count > 0)
            //        return View(themeName /*this.theme.ThemeName*/ + "LevelShortCutImages", menusVMList);
            //}

            return RedirectToAction("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult LevelShortCutImages(int LevelId, string strIds/*, List<LevelShortCutImagesVM> levelShortCutImagesVMList*/)
        {
            try
            {
                if (consoleBusiness.CheckLevelId/*InDomain*/(LevelId, this.userId.Value,
                        this.parentUserId.Value,
                        this.areaName,
                        this.controllerName,
                        this.actionName,
                        this.domainsSettings,
                        this.roleIds,
                        this.levelIds
                    /*, domain*/))
                    if (!string.IsNullOrEmpty(strIds))
                    {
                        if (consoleBusiness.RemoveFromLevelShortCutImagesWithLevelId(LevelId))
                            if (consoleBusiness.AddListToLevelShortCutImages(LevelId, strIds/*levelShortCutImagesVMList*/))
                                return Json(new
                                {
                                    Result = "OK",
                                    Message = "SuccessShortCutImages"
                                });
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "NoAssignAccesses"
                        });
                    }
            }
            catch (Exception exc)
            { }
            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }
    }
}
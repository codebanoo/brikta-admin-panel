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

using Services.Xml.Entities;
using Services.Xml;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Newtonsoft.Json;
using Models.Entities.Console;

namespace Web.Console.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LevelsManagementController : ExtraAdminController
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
            ViewData["Title"] = "لیست دسترسی ها";

            //if (ViewData["DomainsSettingsList"] == null)
            //    ViewData["DomainsSettingsList"] = business.GetAllDomainsSettingsList();

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList(/*this.userId.Value*/).Where(r => !r.RoleName.Equals("Hosts")).ToList();

            //if (ViewData["DefaultPagesList"] == null)
            //    ViewData["DefaultPagesList"] = "";// consoleBusiness.GetDefaultPages/*WithDomainName*/(/*domain, */"fa");

            //return View(themeName /*this.theme.ThemeName*/ + "Index");
            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }
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

                //List<int> RoleIds = null;
                //List<int> LevelIds = null;

                var levelsList = consoleBusiness.GetLevelsList(this.areaName,
                    this.controllerName,
                    this.actionName,
                    this.domainsSettings,
                    this.userId.Value,
                    this.parentUserId.Value,
                    jtStartIndex,
                    jtPageSize,
                    ref listCount,
                    jtSorting,
                    //domainSettingIdSearch,
                    levelNameSearch,
                    roleIdSearch,
                    this.userId,
                    this.roleIds,
                    this.levelIds);

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
                Message = !string.IsNullOrEmpty(returnMessage) ? returnMessage : "ErrorMessage"
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
                        var role = consoleBusiness.GetRoleWithLevelId(levelVM.LevelId);

                        #region remove txt files

                        try
                        {
                            if (role != null)
                            {
                                string menuFilePath = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus\\" + domain + "\\" +
                                    role.RoleName + "\\";

                                //var files = (new System.IO.DirectoryInfo(@menuFilePath)).GetFiles("*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                var filesNames = FileManagement.GetFilesNames(menuFilePath, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                foreach (var file in filesNames)
                                {
                                    try
                                    {
                                        if (file.Contains("_" + levelVM.LevelId.ToString() + "_"))
                                        {
                                            System.Threading.Thread.Sleep(100);
                                            System.IO.File.Delete(@menuFilePath + file);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc) { }
                                }
                            }
                        }
                        catch (Exception exc) { }

                        #endregion

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
                if (!this.levelIds.Contains(LevelId))
                //if (LevelId != this.levelId.Value)
                {
                    var role = consoleBusiness.GetRoleWithLevelId(LevelId);

                    if (consoleBusiness.CompleteDeleteLevel(LevelId, this.userId.Value))
                    {
                        #region remove txt files

                        try
                        {
                            if (role != null)
                            {
                                string menuFilePath = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus\\" + domain + "\\" +
                                    role.RoleName + "\\";

                                //var files = (new System.IO.DirectoryInfo(@menuFilePath)).GetFiles("*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                var filesNames = FileManagement.GetFilesNames(menuFilePath, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                foreach (var file in filesNames)
                                {
                                    try
                                    {
                                        if (file.Contains("_" + LevelId.ToString() + "_"))
                                        {
                                            System.Threading.Thread.Sleep(100);
                                            System.IO.File.Delete(@menuFilePath + file);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc) { }
                                }
                            }
                        }
                        catch (Exception exc) { }

                        #endregion

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
            ViewData["Title"] = "انتساب دسترسی";

            if (ViewData["DataAccessTypesList"] == null)
                ViewData["DataAccessTypesList"] = consoleBusiness.GetAllDataAccessTypesList();

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
                bool justloadForCurrentLevel = true;
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
                    ViewData["DataBackUrl"] = "/Admin/LevelsManagement/Index";
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
                                            //var role = consoleBusiness.GetRoleWithLevelId(LevelId);

                                            #region remove txt files

                                            try
                                            {
                                                if (role != null)
                                                {
                                                    string menuFilePath = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TextFiles\\Menus\\" + domain + "\\" +
                                                        role.RoleName + "\\";

                                                    //var files = (new System.IO.DirectoryInfo(@menuFilePath)).GetFiles("*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                                    var filesNames = FileManagement.GetFilesNames(menuFilePath, "*.txt", System.IO.SearchOption.TopDirectoryOnly);
                                                    foreach (var file in filesNames)
                                                    {
                                                        try
                                                        {
                                                            if (file.Contains("_" + LevelId.ToString() + "_"))
                                                            {
                                                                System.Threading.Thread.Sleep(100);
                                                                System.IO.File.Delete(@menuFilePath + file);
                                                                System.Threading.Thread.Sleep(100);
                                                            }
                                                        }
                                                        catch (Exception exc) { }
                                                    }
                                                }
                                            }
                                            catch (Exception exc) { }

                                            #endregion

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
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
using Microsoft.Web.Administration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using Newtonsoft.Json;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Dynamic;

namespace Web.Console.Areas.Host.Controllers
{
    [Area("Host")]
    public class UsersManagementController : ExtraHostController
    {
        public UsersManagementController(IHostEnvironment _HostingingEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IMapper _mapper,
            IConsoleBusiness _consoleBusiness,
            IPublicServicesBusiness _publicServicesBusiness,
            IMemoryCache _memoryCache,
            IDistributedCache _distributedCache) :
            base(_HostingingEnvironment,
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
            if (ViewData["DomainsSettingsList"] == null)
                ViewData["DomainsSettingsList"] = consoleBusiness.GetAllDomainsSettingsList();

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList();

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            if (ViewData["ExcelExport"] == null)
            {
                ViewData["ExcelExport"] = "OK";
            }

            if (ViewData["MultiSelect"] == null)
            {
                ViewData["MultiSelect"] = "OK";
            }

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfUsers(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
            long ParentId = 0,
            int DomainSettingId = 0,
            List<int> RoleIds = null,
            List<int> LevelIds = null,
            string UserName = "",
            string Name = "",
            string Family = "",
            string Email = "",
            string Phone = "",
            string Mobile = "",
            string Address = "",
            string CertificateId = "",
            string NationalCode = "",
            string BirthDateTimeFrom = "",
            string BirthDateTimeTo = "",
            string RegisterDateTimeFrom = "",
            string RegisterDateTimeTo = "",
            bool? IsActivated = null)
        {
            try
            {
                DateTime? BirthDateTimeEnFrom = null;
                if (!string.IsNullOrEmpty(BirthDateTimeFrom))
                    BirthDateTimeEnFrom = PersianDate.ToGregorianDate(BirthDateTimeFrom.ToEnglishDigits());

                DateTime? BirthDateTimeEnTo = null;
                if (!string.IsNullOrEmpty(BirthDateTimeTo))
                    BirthDateTimeEnTo = PersianDate.ToGregorianDate(BirthDateTimeTo.ToEnglishDigits());

                DateTime? RegisterDateTimeEnFrom = null;
                if (!string.IsNullOrEmpty(RegisterDateTimeFrom))
                    RegisterDateTimeEnFrom = PersianDate.ToGregorianDate(RegisterDateTimeFrom.ToEnglishDigits());

                DateTime? RegisterDateTimeEnTo = null;
                if (!string.IsNullOrEmpty(RegisterDateTimeTo))
                    RegisterDateTimeEnTo = PersianDate.ToGregorianDate(RegisterDateTimeTo.ToEnglishDigits());

                int listCount = 0;
                var usersList = consoleBusiness.GetUsersList(
                    this.areaName,
                    this.controllerName,
                    this.actionName,
                    this.domainsSettings,
                    jtStartIndex,
                    jtPageSize,
                    ref listCount,
                    jtSorting,
                    //ParentId,
                    (ParentId == 0) ? this.userId.Value : ParentId,
                    this.userId.Value,
                    DomainSettingId,
                    //(DomainSettingId != 0) ? DomainSettingId : this.domainsSettings.DomainSettingId,
                    RoleIds,
                    LevelIds,
                    this.roleIds,
                    this.levelIds,
                    UserName,
                    Name,
                    Family,
                    Email,
                    Phone,
                    Mobile,
                    Address,
                    CertificateId,
                    NationalCode,
                    BirthDateTimeEnFrom,
                    BirthDateTimeEnTo,
                    RegisterDateTimeEnFrom,
                    RegisterDateTimeEnTo,
                    //!string.IsNullOrEmpty(BirthDateTime) ? PersianDate.ToGregorianDate(BirthDateTime) : null,
                    IsActivated,
                    "fa");

                return Json(new { Result = "OK", Records = usersList, TotalRecordCount = listCount });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult CreateUser()
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/UsersManagement/Index";
            }

            if (ViewData["DomainsSettingsList"] == null)
                ViewData["DomainsSettingsList"] = consoleBusiness.GetAllDomainsSettingsList();

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList();

            //if (ViewData["LevelsList"] == null)
            //    ViewData["LevelsList"] = business.GetAllLevelsList();

            //if (ViewData["UserId"] == null)
            //    ViewData["UserId"] = this.userId.Value;

            CustomUsersVM customUsersVM = new CustomUsersVM();
            customUsersVM.Sexuality = true;

            //return View(themeName /*this.theme.ThemeName*/ + "CreateUser", customUsersVM);

            dynamic expando = new ExpandoObject();
            expando = customUsersVM;

            return View("AddTo", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult CreateUser(CustomUsersVM customUsersVM)
        {
            try
            {
                if (!consoleBusiness.ValidCreateNewUserInDomain(customUsersVM.DomainSettingId))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "LimitCreateNewUser"
                    });
                }

                //var domainSettings = business.GetDomainsSettingsWithUserId(customUsersVM.ParentUserId.Value);
                //if (!customUsersVM.DomainSettingId.Equals(domainSettings.DomainSettingId))
                //{
                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = publicTexts.Where(t => t.PropertyName == "ParentIsNotInSelectedDomain").FirstOrDefault().Value
                //    });
                //}

                if (consoleBusiness.ExistUserWithUserName(customUsersVM.UserName, customUsersVM.DomainSettingId))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DuplicateUserName"
                    });
                }

                if (string.IsNullOrEmpty(customUsersVM.Password) || string.IsNullOrEmpty(customUsersVM.ReplyPassword))
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "EnterPasswordAndReplyPassword"
                    });

                if (!string.IsNullOrEmpty(customUsersVM.Password) && !string.IsNullOrEmpty(customUsersVM.ReplyPassword))
                    if (!customUsersVM.Password.Equals(customUsersVM.ReplyPassword))
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "NotEqualPasswordAndReplyPassword"
                        });

                if (!customUsersVM.ParentUserId.HasValue)
                    customUsersVM.ParentUserId = this.userId.Value;

                if (ModelState.IsValid)
                {
                    customUsersVM.UserId = consoleBusiness.CreateUser(customUsersVM);
                    if (customUsersVM.UserId > 0)
                    {
                        #region create user folders

                        var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(customUsersVM.DomainSettingId, this.userId.Value);

                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId))
                        {
                            System.IO.Directory.CreateDirectory(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages"))
                        {
                            System.IO.Directory.CreateDirectory(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages");
                            System.Threading.Thread.Sleep(100);
                        }

                        #endregion

                        #region if CreateSubDomainPerNewUser config for user domain is activated then create sub domain per user

                        if (domainSettings.CreateSubDomainPerNewUser)
                        {
                            try
                            {
                                string siteName = customUsersVM.UserName + "." + domainSettings.DomainName;
                                string ipAddress = "";
                                string tcpPort = "";
                                string AdminHeader = "";
                                string protocol = "";

                                if (string.IsNullOrEmpty(siteName))
                                {

                                }
                                //get the server manager instance

                                //using (ServerManager mgr = new ServerManager(@"\\qasql01\IISSharedConfig\applicationAdmin.config"))
                                using (ServerManager mgr = ServerManager.OpenRemote(@"\\Qasql01\c$\Windows\System32\inetsrv\config\applicationAdmin.config"))
                                //using (ServerManager mgr = new ServerManager())
                                {
                                    SiteCollection sites = mgr.Sites;
                                    Site site = mgr.Sites[siteName];
                                    if (site != null)
                                    {
                                        string bind = ipAddress + ":" + tcpPort + ":" + AdminHeader;
                                        //check the binding exists or not
                                        foreach (Binding b in site.Bindings)
                                        {
                                            if (b.Protocol == protocol && b.BindingInformation == bind)
                                            {
                                                throw new Exception("A binding with the same ip, port and Admin header already exists.");
                                            }
                                        }
                                        Binding newBinding = site.Bindings.CreateElement();
                                        newBinding.Protocol = protocol;
                                        newBinding.BindingInformation = bind;
                                        site.Bindings.Add(newBinding);
                                        mgr.CommitChanges();
                                    }
                                    else
                                    { }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            //CreateAppPool("IIS://LocalAdmin/W3SVC/AppPools", "MyAppPool");
                            //CreateVDir("IIS://LocalAdmin/W3SVC/1/Root", "MyVDir", "D:\\Inetpub\\Wwwroot");
                            //AssignVDirToAppPool("IIS://LocalAdmin/W3SVC/1/Root/MyVDir", "MyAppPool");
                        }
                        #endregion

                        return Json(new
                        {
                            Result = "OK",
                            UserId = customUsersVM.UserId,
                            Message = "Success"
                        });
                    }
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

        public IActionResult EditUser(long Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/UsersManagement/Index";
            }

            //if (!consoleBusiness.GetChildUserIdsWithStoredProcedure(Id).Contains(this.userId.Value))
            //    return RedirectToAction("Index");

            if (ViewData["DomainsSettingsList"] == null)
                ViewData["DomainsSettingsList"] = consoleBusiness.GetAllDomainsSettingsList();

            if (ViewData["RolesList"] == null)
                ViewData["RolesList"] = consoleBusiness.GetAllRolesList();

            if (Id.Equals(this.userId.Value))
                ViewData["IsAdmin"] = true;

            //if (ViewData["LevelsList"] == null)
            //    ViewData["LevelsList"] = business.GetAllLevelsList();

            CustomUsersVM customUsersVM = consoleBusiness.GetMultiLevelsUserWithUserId(Id);
            //customUsersVM.ReplyPassword = customUsersVM.Password;
            //return View(themeName /*this.theme.ThemeName*/ + "EditUser", customUsersVM);

            dynamic expando = new ExpandoObject();
            expando = customUsersVM;

            return View("Update", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult EditUser(CustomUsersVM customUsersVM)
        {
            try
            {
                if (!consoleBusiness.ValidCreateNewUserInDomain(customUsersVM.DomainSettingId))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "LimitCreateNewUser"
                    });
                }

                if (!customUsersVM.ParentUserId.HasValue)
                {
                    if (customUsersVM.UserId.Equals(this.userId.Value))
                    {
                        customUsersVM.ParentUserId = (long?)null;
                    }
                    else
                    {
                        customUsersVM.ParentUserId = this.userId.Value;
                    }
                }

                if (!string.IsNullOrEmpty(customUsersVM.Password) && !string.IsNullOrEmpty(customUsersVM.ReplyPassword))
                    if (!customUsersVM.Password.Equals(customUsersVM.ReplyPassword))
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "NotEqualPasswordAndReplyPassword"
                        });

                if (string.IsNullOrEmpty(customUsersVM.Password) && string.IsNullOrEmpty(customUsersVM.ReplyPassword))
                {
                    ModelState["Password"].Errors.Clear();
                    ModelState["ReplyPassword"].Errors.Clear();
                    ModelState["Password"].ValidationState = ModelValidationState.Valid;
                    ModelState["ReplyPassword"].ValidationState = ModelValidationState.Valid;
                }

                customUsersVM.EditEnDate = DateTime.Now;
                customUsersVM.EditTime = PersianDate.TimeNow;
                customUsersVM.UserIdEditor = this.userId.Value;

                string returnMessage = "";

                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateMultiLevelsUser(customUsersVM, ref returnMessage))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            UserId = customUsersVM.UserId,
                            Message = !string.IsNullOrEmpty(returnMessage) ? returnMessage : "Success",
                            Record = customUsersVM
                        });
                    }
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
        public IActionResult ToggleActivationUser(int UserId = 0)
        {
            try
            {
                if (consoleBusiness.ToggleActivationUser(UserId, this.userId.Value))
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
        public IActionResult ToggleTemporaryDeleteUser(int UserId = 0)
        {
            try
            {
                if (consoleBusiness.ToggleTemporaryDeleteUser(UserId, this.userId.Value))
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
        public IActionResult CompleteDeleteUser(int UserId = 0)
        {
            try
            {
                if (UserId != userId.Value)
                {
                    var customUsersVM = consoleBusiness.GetUserWithUserId(UserId);

                    string picture = consoleBusiness.GetUserProfilePicture(UserId);

                    if (consoleBusiness.CompleteDeleteUser(UserId))
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(customUsersVM.DomainSettingId, this.userId.Value);

                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\" + domainSettings.DomainName
                            + "\\" + customUsersVM.UserId;

                        #region Delete user files and folders

                        try
                        {
                            if (System.IO.Directory.Exists(userFolder))
                            {
                                DirectoryInfo directory = new DirectoryInfo(userFolder);
                                FolderManager.EmptyFolder(directory);
                            }
                        }
                        catch (Exception exc)
                        { }

                        try
                        {
                            if (System.IO.Directory.Exists(userFolder))
                            {
                                System.IO.Directory.Delete(userFolder, true);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        catch (Exception exc)
                        { }

                        //try
                        //{
                        //    if (System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" +
                        //        customUsersVM.UserId))
                        //    {
                        //        System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" +
                        //            customUsersVM.UserId, true);
                        //        System.Threading.Thread.Sleep(100);
                        //    }
                        //}
                        //catch (Exception exc)
                        //{ }

                        #endregion

                        //if (!string.IsNullOrEmpty(picture))
                        //{
                        //    try
                        //    {
                        //        if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picture))
                        //        {
                        //            System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picture);
                        //            System.Threading.Thread.Sleep(100);
                        //        }
                        //    }
                        //    catch (Exception exc)
                        //    { }

                        //    try
                        //    {
                        //        if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picture))
                        //        {
                        //            System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picture);
                        //            System.Threading.Thread.Sleep(100);
                        //        }
                        //    }
                        //    catch (Exception exc)
                        //    { }
                        //}

                        //#region Delete User Images

                        //if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages"))
                        //{
                        //    System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages");
                        //    System.Threading.Thread.Sleep(100);
                        //}

                        //if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId))
                        //{
                        //    System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId);
                        //    System.Threading.Thread.Sleep(100);
                        //}

                        //#endregion

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
                        Message = "DeleteUserErrorMessage"
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
        public IActionResult CompleteDeleteUsers(List<long> UserIds)
        {
            try
            {
                if (UserIds.Count > 0)
                {
                    var childsUserIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.userId.Value);

                    //if (childsUserIds.All(x => UserIds.Any(y => (x == y))))
                    if (childsUserIds.Any(x => UserIds.Any(y => x.Equals(y))))
                    {
                        var customUsersVM = consoleBusiness.GetUsersWithUserIds(UserIds);

                        foreach (var customUserVM in customUsersVM)
                        {
                            string picture = consoleBusiness.GetUserProfilePicture(customUserVM.UserId);

                            if (consoleBusiness.CompleteDeleteUser(customUserVM.UserId))
                            {
                                var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(customUserVM.DomainSettingId, this.userId.Value);

                                string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\" + domainSettings.DomainName
                                    + "\\" + customUserVM.UserId;

                                #region Delete user files and folders

                                try
                                {
                                    if (System.IO.Directory.Exists(userFolder))
                                    {
                                        DirectoryInfo directory = new DirectoryInfo(userFolder);
                                        FolderManager.EmptyFolder(directory);
                                    }
                                }
                                catch (Exception exc)
                                { }

                                try
                                {
                                    if (System.IO.Directory.Exists(userFolder))
                                    {
                                        System.IO.Directory.Delete(userFolder, true);
                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
                                catch (Exception exc)
                                { }

                                //try
                                //{
                                //    if (System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" +
                                //        customUsersVM.UserId))
                                //    {
                                //        System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" +
                                //            customUsersVM.UserId, true);
                                //        System.Threading.Thread.Sleep(100);
                                //    }
                                //}
                                //catch (Exception exc)
                                //{ }

                                #endregion

                                //if (!string.IsNullOrEmpty(picture))
                                //{
                                //    //try
                                //    //{
                                //    //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                //    //        customUsersVM.UserId + "\\" + picture))
                                //    //    {
                                //    //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                //    //            customUsersVM.UserId + "\\" + picture);
                                //    //        System.Threading.Thread.Sleep(100);
                                //    //    }
                                //    //}
                                //    //catch (Exception exc)
                                //    //{ }

                                //    //try
                                //    //{
                                //    //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                //    //        customUsersVM.UserId + "\\thumb_" + picture))
                                //    //    {
                                //    //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                //    //            customUsersVM.UserId + "\\thumb_" + picture);
                                //    //        System.Threading.Thread.Sleep(100);
                                //    //    }
                                //    //}
                                //    //catch (Exception exc)
                                //    //{ }
                                //}

                                //#region Delete User Images

                                ////if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                ////    customUsersVM.UserId + "\\UserImages"))
                                ////{
                                ////    System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages");
                                ////    System.Threading.Thread.Sleep(100);
                                ////}

                                ////if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + 
                                ////    customUsersVM.UserId))
                                ////{
                                ////    System.IO.Directory.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + customUsersVM.UserId);
                                ////    System.Threading.Thread.Sleep(100);
                                ////}

                                //#endregion
                            }
                        }

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
                        Message = "DeleteUsersErrorMessage"
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

        public IActionResult GetUserDetails(int Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/UsersManagement/Index";
            }

            CustomUsersVM customUsersVM = consoleBusiness.GetUserWithUserId(Id);
            //return View(themeName /*this.theme.ThemeName*/ + "GetUserDetails", customUsersVM);

            dynamic expando = new ExpandoObject();
            expando = customUsersVM;

            return View("Update", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetUserDetails(int UserId, string strAccesses)
        {
            try
            { }
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
        public IActionResult GetUsersList(int DomainSettingId, List<int> RoleIds)
        {
            try
            {
                //var usersList = business.GetUsersList(DomainSettingId, RoleId, 0);

                List<CustomUsersVM> usersList = new List<CustomUsersVM>();

                usersList = consoleBusiness.GetUsersList(/*this.domainsSettings.DomainSettingId*/
                    this.areaName,
                    this.controllerName,
                    this.actionName,
                    this.domainsSettings,
                    DomainSettingId,
                    RoleIds,
                    levelIds,
                    this.userId.Value,
                    this.parentUserId.Value);

                //long domainAdminId = 0;
                //if (DomainSettingId == 0)
                //{
                //    usersList = business.GetUsersList(/*this.domainsSettings.DomainSettingId*/0, RoleId, this.userId.Value);
                //}
                //else
                //{
                //    domainAdminId = business.GetDomainAdminId(DomainSettingId);
                //    usersList = business.GetUsersList(DomainSettingId, RoleId, domainAdminId);
                //}

                long domainAdminId = 0;
                domainAdminId = consoleBusiness.GetDomainAdminId(DomainSettingId);
                if (!usersList.Any(u => u.UserId.Equals(this.userId.Value)) && domainAdminId != this.userId.Value)
                    usersList.Add(consoleBusiness.GetUserWithUserId(this.userId.Value));

                if (domainAdminId != 0)
                    if (!usersList.Any(u => u.UserId.Equals(domainAdminId))/* && domainAdminId != this.userId.Value*/)
                        usersList.Add(consoleBusiness.GetUserWithUserId(domainAdminId));

                return Json(new { Result = "OK", Records = usersList });
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
        public IActionResult GetLevelsList(int DomainSettingId, List<int> RoleIds, long UserId = 0)
        {
            try
            {
                //var usersList = business.GetUsersList(DomainSettingId, RoleId, 0);

                List<LevelsVM> levelsList = new List<LevelsVM>();

                levelsList = consoleBusiness.GetAllLevelsList(this.userId.Value, RoleIds/*DomainSettingId, RoleId, this.userId.Value*/);

                //if (UserId != 0)
                //    levelsList = business.GetAllLevelsList(UserId, RoleId/*DomainSettingId, RoleId, this.userId.Value*/);
                //else
                //if (DomainSettingId == 0)
                //    levelsList = business.GetAllLevelsList(this.userId.Value, RoleId/*DomainSettingId, RoleId, this.userId.Value*/);
                //else
                //    levelsList = business.GetAllLevelsList(business.GetDomainAdminId(this.domainsSettings.DomainSettingId), RoleId/*DomainSettingId, RoleId, this.userId.Value*/);

                return Json(new { Result = "OK", Records = levelsList });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult HierarchyOfUsers(long Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/UsersManagement/Index";
            }

            string profileTitle = "پروفایل";
            string roleTitle = "نقش";
            string levelTitle = "دسترسی";
            string notEnteredTitle = "وارد نشده";
            string domainNameTitle = "دامنه";
            string emailTitle = "ایمیل";
            string registerDateTitle = "تاریخ ثبت";
            string mobileTitle = "همراه";
            string nationalCodeTitle = "کد ملی";
            string moreInformationTitle = "اطلاعات بیشتر";
            string nameFamilyTitle = "اسم";
            string userNameTitle = "نام کاربری";
            string birthDateTitle = "تاریخ تولد";
            string isActivatedText = "فعال";
            string isDeactivatedText = "غیر فعال";
            //List<CustomUsersVM> usersList = new List<CustomUsersVM>();
            try
            {
                if (consoleBusiness.ExistUserWithUserId(Id, this.domain))
                {
                    var user = consoleBusiness.GetUserWithUserId(Id);
                    ViewData["ParentUser"] = user;

                    ViewData["Root"] = JsonConvert.SerializeObject(consoleBusiness.GetRootOfSpaceTree(Id,
                        profileTitle,
                        roleTitle,
                        levelTitle,
                        notEnteredTitle,
                        domainNameTitle,
                        emailTitle,
                        registerDateTitle,
                        mobileTitle,
                        nationalCodeTitle,
                        moreInformationTitle,
                        nameFamilyTitle,
                        userNameTitle,
                        birthDateTitle,
                        "fa",
                        isActivatedText,
                        isDeactivatedText));

                    //var role = business.GetRoleWithUserId/*AndDomainName*/(Id/*, user.DomainSettingId*/);
                    //usersList = business.GetUsersList(0/*user.DomainSettingId*/, 0, user.UserId);

                    //string nameFamily = "";
                    //if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Family))
                    //    nameFamily = nameFamilyTitle + "<br />" + notEnteredTitle;
                    //else
                    //    nameFamily = nameFamilyTitle + "<br />" + user.Name + " " + user.Family;

                    //string levelName = user.LevelName;

                    //string dataContent = domainNameTitle + " : " + (string.IsNullOrEmpty(user.DomainName) ? notEnteredTitle : user.DomainName) +
                    //    "<br />" + emailTitle + " : " + (string.IsNullOrEmpty(user.Email) ? notEnteredTitle : user.Email) +
                    //    "<br />" + birthDateTitle + " : " + (string.IsNullOrEmpty(user.BirthDateTime) ? notEnteredTitle : user.BirthDateTime) +
                    //    "<br />" + nationalCodeTitle + " : " + (string.IsNullOrEmpty(user.NationalCode) ? notEnteredTitle : user.NationalCode) +
                    //    "<br />" + mobileTitle + " : " + (string.IsNullOrEmpty(user.Mobile) ? notEnteredTitle : user.Mobile);

                    //string activeateDeactivateStatusText = "";

                    //if (user.IsActivated)
                    //    activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsActivated")).Value;
                    //else
                    //    activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsDeactivated")).Value;

                    //string hierarchyOfUsersDataString = "";

                    //string userImage = "";
                    //if (!string.IsNullOrEmpty(user.Picture))
                    //{
                    //    userImage = "<img src='/Files/UserFiles/" + user.DomainName + "/" +
                    //                    user.UserId + "/thumb_" + user.Picture + "' class='userThumbImage' />";
                    //}
                    //else
                    //{
                    //    userImage = "<i class='fa fa-4x fa-user'></i>";
                    //}

                    ////string html = "\"<div class='row'><div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                    ////    userImage + "</div>" +
                    ////    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding' data-toggle='popover' data-html='True'" +
                    ////    "data-content='" + dataContent + "' data-original-title='" + moreInformationTitle + "'" +
                    ////    " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                    ////    nameFamily + "<br />" + user.UserName + "</div></div><div class='parent-show-profile'><a " +
                    ////    "class='show-profile' data-userId='1'>" + profileTitle + "</a></div>\", ";

                    //string html = "\"<div class='card kl-card kl-xl kl-reveal kl-overlay kl-show kl-slide-in kl-shine kl-hide kl-spin'>" +
                    //                         "<div class='kl-card-block kl-lg bg-success kl-shadow-br kl-overlay'>" +
                    //            "<div class='kl-background'>" +
                    //                "<div class='row'>" +
                    //                    "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                    //                        userImage +
                    //                    "</div>" +
                    //                    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'><a class='showMoreInfo' data-toggle='popover' " +
                    //                            "tabindex='0' data-trigger='focus' data-html='True' " +
                    //                            "data-content='" + dataContent + "' data-container='body' data-original-title='" + moreInformationTitle + "'" +
                    //                            " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                    //                        registerDateTitle + ":" +
                    //                        "<br />" +
                    //                        user.RegisterDate +
                    //                        "<br />" +
                    //                        (!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "09122060766") +
                    //                    "</a></div>" +
                    //                "</div>" +
                    //            //userImage +
                    //            "</div>" +
                    //            "<div class='kl-card-overlay kl-card-overlay-split-v-4 kl-dark kl-inverse kl-bottom-in'>" +
                    //                "<div class='kl-card-overlay-item'></div>" +
                    //                "<div class='kl-card-overlay-item'></div>" +
                    //                "<div class='kl-card-overlay-item'></div>" +
                    //                "<div class='kl-card-overlay-item'></div>" +
                    //            "</div>" +
                    //            "<div class='kl-card-item kl-pbl kl-show text-white kl-txt-shadow'>" +
                    //            //"<div class='kl-figure-block'>" +
                    //            //    "<span class='kl-figure'>109</span>" +
                    //            //    "<span class='kl-title text-white'>Following</span>" +
                    //            //"</div>" +
                    //            "</div>" +
                    //            "<div class='kl-card-item kl-pbr kl-show text-white kl-txt-shadow'>" +
                    //            //"<div class='kl-figure-block'>" +
                    //            //    "<span class='kl-figure>26k</span>" +
                    //            //    "<span class='kl-title text-white'>Followers</span>" +
                    //            //"</div>" +
                    //            "</div>" +
                    //            "<div class='kl-card-item kl-pm kl-show text-white kl-txt-shadow text-center'>" +
                    //            //birthDateTitle + ":" +
                    //            //user.BirthDateTime +
                    //            //"<br />" +
                    //            //(!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "") +
                    //            //"<h3 class='mb-0'>Joe Bloggs</h3>" +
                    //            //"<div class='text-white small'>asdasdasd</div>" +
                    //            "</div>" +
                    //            //"<div class='kl-card-item kl-ptl kl-v kl-card-social kl-slide-in'>" +
                    //            //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEa0lEQVRYR8VXa2hbZRh+3vckXbu09cKKOJyC29y8IZQmduiw9YesDjbc7HqTIV46GV5okjrdhAZRytY2HbMiE0FhNqdtZAiiGwPpREGb1NUfq0xnHdNZ53602GzrJT3fKydb2zTXs2xgfoXwvM/zfO/75vnOIfzPH8pVv7qvT3P8tbrokiZ2x9i/kU98lVO5cFk2UOHrty0vKnxCkWwmhUeUqHuY2T4vauAfEIaE6BgMrbdnV+moFUNZDZjCtxc7GiHqDYBXWCFVUIrBujK4pff1spFMNRkNNLT/cJ8SDoDxkBXhJIySaWF6a23E6ff5SKXiSGugtm1gI7P0CXhpTuJxRULqcP7EZEOqPUlpwBQnps8B2K5XfK6eSB0pdNg2f7ijLBrPmWTAbLsQwjfi5InmBdLV43n4lbQGYgvncAzmPHMr7SJU6W7X0fnOxNfU+UM7IXjfCk+uGFHy2/lLl+897qucNTnmRxA7fVHB71b/avEGFGhcgwQEmFBQTzF4LRTGwLg15eYLngl4Xd2LDNR3hJ8UyJdpTybyoiJpZfCyeIxBOGePqnXdu8rPmb83Hhy0RyJSQSSrBGo/wHlJnITjuttVuchArT90kASN6QywXd00O2Ur0bSoLtCccTPcGfC4Pkisq20LVwgbwUTDJs4MKm0muizw5vrx+RHUtYdOgnB/2g4QKnS36xufT/jXotBWBdouSj0KDTW97vJjyQYGThPTqnR8BNoY8Di/ihkwLxY+e+fkomxfNGQl0HjvmgnnnkWJJkLVwSAHt20z4uHVvuE8myMyBea0QSdC7h6vszMGeLZz6OZpFR3PuNmEB3S3a9jK9td0DpayUj9mwbbqHtfuKx04cKLEFp29kKmAgK8jhbzpix1ll7OZqG8P7xaSdzPiBB261+W90gFff/50kWMyGzFAL82uOPNRYsvj66oOnF5SHB37hUF3Zeloi+52vb2whPtC56HhtoxdIDlU6NCeT8zz+Jq6jvA7gOzJdhgR2t7jdR5aMNAWOgLGhmyFCuqUTbC121v+c9Lmd4RqSCk90/LN1ShFD/Y2O0/OG6htDzcRiT+VAQO4oCk6Ck1GWKnPEsVjKVq41H01qDjbIQCM6m7nHSCSBQN7TywX28yfDE5JIILvQfhY2PiuAPl/TxuGjVjuFiWPK8ZzLFhtQTgGIaA94HE1X/2+UFbXEfoUQINVohxxs7M2Xhl8reyPJAM1+wZXMhnDYFqSI7mVsv26x9U0B0xKqjp/uBki+6wwXTtGRtgupd2vlk+kNWBm/anigSAJb7l2gQwVSi6SjdYHmlw/xaNSZrUZTDPFBYdFuOqGmDDFWTYFPOX9iXxpLwvzXp+4aPgJ9PL1mZARYno68eRpR5AoVucPbRBD3st0taYxaD5ydbFdtcTP3HIH4oGx17JCR40wXoChHsuSdKMEBCC2roC39Gy27mV9NUskqG/99hbk5a9TgjVEUgJBHhgTouiMCIZ6vWXDZsJlE7Y8AqtEueL+Az80sTB3CtXzAAAAAElFTkSuQmCC'></a>" +
                    //            //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAD9klEQVRYR8WXX2hbdRTHv+fcpOscisJG16gvTnQqIig+JFm1vsjmwOGwf7INrTa9lakPGw6GCA2+CKL1XxXS1E3QpUkKwxedzAer7CYTJxPZcMKmzOncFBxWx7o29xy50bZJ1uT+murM04Wc8/1+fuece+69hP/5R436d3TkrF+Xnb/SPa9Baxn9Mf7OY5ONaBkDtLcnAtOha+9XkQ3qcpQIN4ERnDUVOUvgw2DaD1DWyfSeNgHyBfCMp1pDNlR3Any9iSgEAgujEBnIZ/tP1MupC9DWsevWaauYZtAdRsZVQQJcJNBzhdU/DiKRkPk0agJEukfWA5oDcEUj5uU5Ctm7pLlp83xzMi+AZy7Q9xkILNZ8Jl9B+6aucjd8Odw/Xa55CYBXdtdyv/g3Tl4Nr9ChQsZ+uiaAN3CTK0OHjHsuclaZxgA+CdIiAy2q2Fmvakq0rjAa/2gmpqICkdjwVii9aVJ2l+RbLlqRwlj8t5n4cCy5mpS/qZ8vx5vO/HzL+Hii6MXNApRut5Wt3xnfalA7n7FT5WZmAACpbnGy9p4KgOim1AMq+MDk9F6MCK05mIs73nWke7gH4AERt4mZQ34aCh0vZOz7KgAi3ckkwLZf8lzv+G4n03uoBNCVeh2EiuGqqyMQDkwvP5Deem62BeGO1BGycJsfALnytTJ/HCDrtc8yj5/y4sPdww+RUlQJywl41E+jdHLGeifd92EJwHuw/ES/X6jY7TVUSDXpZO0n5vs70jlyD1g/NQIAbXcy8VdKAO09u6+emiyeM0qsAxCOpWKkSJvoKPSFQsZ+tgTQ1rFrhWu5v5gkwnvqMR0X0nhhtP/Y3zMwvJ0IG0W1lYhvMNJRvJzP9j0zU4HmqcniBaPEf4IIc0MY7UwNKePJBeUrDTjZ+POzQxjpTJ4Bc4upyGIBAHokn4m/OwsQ7UztU8baywagdHs+Gz8yB9A9sk2hg5cDQEROH8zZ13k7sQzg7ZCKnAKDTSAW0wKCvuRk7B0Vm7A0zbHUe1Bs/i8BBCgGXFl1YKz/h0sBupKrhPgoA0v8IBqtACledbJ92+ZWepVTtCu1Qwkv+gFAMC2M0nseC4ImrRPghFpL7/x8z5aJmgBIJDh8rHWMwBt9IRYQIMCfzNqWT9tflafN+07Y3rO7+eKku5eg6xbgUTPUM7eEH3RyvZ9UB9V8K77LTgabJmiQQE8tBsIrO7M+XH3y2i2ocgvHRtaSum8AfONCQLxptxRDxcDSgfKeG1egPLD0WdYS6hJCnKD3Alyzct6SsZjSyjyUT8dP+kH7fppVC6zZ9NY1imAYQjcLZAUpNRFoQgnfQ3E4n+096m04P2PjFpgKNRr3F+G5pDBOJtRGAAAAAElFTkSuQmCC'></a>" +
                    //            //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEdElEQVRYR8WXXYiUZRTH/+e8szOLzrBKmlF2EUaRy9K+874QYtJ2U5qSEG5dWN1URlIXRkKIYRghRZ9oF3WZpahhEZTRTUNd6T7PvIHuRbhbVGRfhDszC+rs7DnxLLvLOM774Rr0XM57Pn7n63nOEP7nQwv1Pzw87J09e7ZERD19fX2NSqVycSG2MgMMDQ3lJicn7wOwWVXXishtzNwz51RE/vQ8LwLw9fT09JEois5lAUoFcI4bjcY2EXmRmW/OYhSAADgMYI+1djxJJxHA9/3VzHwIwJ0ZHXeKXQKw21r71izUFWZiAYIg2CgiR5l50QKdz6sR0fFisbi1W590BZh1/hkz567V+Zy+qp4gos3W2ql2m1cAuLQDGPkvIu+EV9UD1Wr1uViA2YYz11Dz1ISp6oZqtfrVnOBlGQiCYDuA97pZERGXukEiupuIdonITQDOMbOb/6KIrGDmgy5KAJuI6OUYmrFSqXRHpVJpue/zAC76Wq32Y9yoich4FEW3OqUgCBaVSiVpb6r+/v786Oho030vl8sPE9GRuHSo6qPVavXjywDCMHxAVb+IUxKRf6IoWpaaYwBhGG5R1WMJtipRFN3bCfC+qm6LUyKi3caYV7MAuMz6vv8dM6+NkZdms7ns9OnT5+dLEATBGQD9CdTXR1H0d0YAV4aniOiDhIA2GmO+nAFwD8vY2NiF9ru9Q1GttZzV+Wwf3E9E893eqUtEzxtj3p4BGBwcXOJ53vkUB0ustbWsEEEQDAM4miC/z1q7awbA9/3lzPxXivFN1trYJu3U9X1/LzO/lFDSN6MoemEGYGhoqLfRaFxIAiAiUywW12V594Mg6AMwCsDdFXHHvZR755vQ9/0/mHlFjHRTRJiITjHzDmPMCADtIktBELhd4V1mLqcE9Lgx5uA8QBiGJ1R1fTclVf2BiJ4B8CGAlQB+I6LtxpjP5+TDMHxienr6NWa+LkufENGAMeZMO8AOVXXvdtdDRN+q6rMAXgHQrNVqjxUKhZ5cLtfXarVqvb29vQCyjuk5a60LRNtLcCMz/wogdtxExDnfn8/nL7VarcUAVqnqYmYuGWM+8X2/mTDK7YG9Ya3d6X7ofIw+ArA1LYVu/8vn8+HU1NQtABYx81JjzFHf9y+mAYhIi5lXWWt/6QawarZ7C0kQIvJ7Lpe7q9VqrSSiG5i5MDEx8WmpVJpMW2KI6B1jzI45+1csJOVyeScRvZ6Shb1E5KbgsKruY+Z9IrJGVVcT0dMJsz+ez+fLJ0+erMcCuB4Iw/CYqj6UAjEBwN2ergxjIrKSmV0jxh2XnXUjIyPftwt03QndxVSv148T0Ya0fsj4fRLAg9babzrlk7biHjeWRORGb8HHLTK5XG5LZ+RJJbjMWblcXk9E+wHMbENZj+t2z/MOeJ63p73mmTPQLujWtXq9/oiqPsnM93SOb4dR95fskIgciKLo5zTg1L9mnQYGBgaWFgqFNQBuV9XlIpJn5joR/QQgMsa4R6jbO9GV5aoB0iK62u//AuhE0jAWsl0eAAAAAElFTkSuQmCC'></a>" +
                    //            //"</div>" +
                    //            "<div class='kl-card-item kl-pb kl-show online_status'>" +
                    //                (user.IsActivated ? "<span class='badge badge-success green'>" : "<span class='badge badge-success red'>") +
                    //                    //"<span class='badge badge-success green'>" + 
                    //                    activeateDeactivateStatusText +
                    //                "</span>" +
                    //            "</div>" +
                    //            "<div class='kl-card-avatar kl-xl kl-pm kl-slow kl-hide'>" +
                    //                "<div class='row'>" +
                    //                    "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                    //                        userImage +
                    //                    "</div>" +
                    //                    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'>" +
                    //                        nameFamily +
                    //                        "<br />" +
                    //                        /*userNameTitle + 
                    //                        "<br />" + */
                    //                        user.UserName +
                    //                    "</div>" +
                    //                "</div>" +
                    //            "</div>" +
                    //        "</div>" +
                    //    "</div>\", ";

                    //hierarchyOfUsersDataString += " { id: \"" + user.UserId.ToString() + "\",";
                    //hierarchyOfUsersDataString += " name: " + html;
                    //hierarchyOfUsersDataString += " data: {}, ";
                    //hierarchyOfUsersDataString += " children: [  ";

                    ////hierarchyOfUsersDataString += hierarchyOfUsersData(user.UserId,
                    ////    usersList,
                    ////    profileTitle,
                    ////    roleTitle,
                    ////    levelTitle,
                    ////    notEnteredTitle,
                    ////    domainNameTitle,
                    ////    emailTitle,
                    ////    birthDateTitle,
                    ////    registerDateTitle,
                    ////    mobileTitle,
                    ////    nationalCodeTitle,
                    ////    moreInformationTitle);

                    //hierarchyOfUsersDataString += " ] } ";
                    //ViewData["HierarchyOfUsersDataString"] = hierarchyOfUsersDataString;
                }
            }
            catch (Exception exc)
            { }
            //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfUsers");

            return View("Index");
        }

        public IActionResult HierarchyOfTreeViewUsers(long Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/UsersManagement/Index";
            }

            try
            {
                List<long> childUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.userId.Value);
                if (childUsersIds.Contains(Id))
                {
                    var user = consoleBusiness.GetUserWithUserId(Id);
                    ViewData["ParentUser"] = user;
                    var role = consoleBusiness.GetRoleWithUserId/*AndDomainName*/(Id/*, user.DomainSettingId*/);

                    string domainNameTitle = "دامنه";
                    string nameFamilyTitle = "اسم";

                    ViewData["Root"] = JsonConvert.SerializeObject(consoleBusiness.GetRootOfTree(Id,
                        domainNameTitle,
                        nameFamilyTitle,
                        "Admin"));

                    //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfTreeViewUsers");

                    return View("Index");
                }
            }
            catch (Exception exc)
            { }
            return RedirectToAction("Index");
        }

        public string GetChildUsers(long UserId = 0)
        {
            try
            {
                string domainNameTitle = "دامنه";
                string nameFamilyTitle = "اسم";

                var childUsers = consoleBusiness.GetDirectUsersWithParentIdForTree(UserId,
                    domainNameTitle,
                    nameFamilyTitle,
                    "Admin");

                if (childUsers != null)
                {
                    return JsonConvert.SerializeObject(childUsers);
                    //.Replace("\"{ tooltip = true }\"", "{ \"tooltip\": \"\" }")
                    //.Replace("folder = true", "\"folder\": \"true\"")
                    //.Replace("lazy = true", "\"lazy\": \"true\"")
                    //.Replace("--DOMAINNAME--", publicTexts.Where(t => t.PropertyName == "DomainNameTitle").FirstOrDefault().Value)
                    //.Replace("--NAMEFAMILY--:", publicTexts.Where(t => t.PropertyName == "NameFamilyTitle").FirstOrDefault().Value);
                    //.Replace("\"{ children = true }\"", "{ \"children\": [] }");
                }
            }
            catch (Exception exc)
            { }

            return "";
        }

        public string GetChildUsersForSpaceTree(long UserId = 0)
        {
            try
            {

                string profileTitle = "پروفایل";
                string roleTitle = "نقش";
                string levelTitle = "دسترسی";
                string notEnteredTitle = "وارد نشده";
                string domainNameTitle = "دامنه";
                string emailTitle = "ایمیل";
                string registerDateTitle = "تاریخ ثبت";
                string mobileTitle = "همراه";
                string nationalCodeTitle = "کد ملی";
                string moreInformationTitle = "اطلاعات بیشتر";
                string nameFamilyTitle = "اسم";
                string userNameTitle = "نام کاربری";
                string birthDateTitle = "تاریخ تولد";
                string isActivatedText = "فعال";
                string isDeactivatedText = "غیر فعال";
                //List<CustomUsersVM> usersList = new List<CustomUsersVM>();

                if (consoleBusiness.ExistUserWithUserId((UserId == 0 ? this.userId.Value : UserId), this.domain))
                {
                    var user = consoleBusiness.GetUserWithUserId(UserId);
                    //usersList = business.GetUsersList(0/*user.DomainSettingId*/, 0, user.UserId);
                    var usersList = consoleBusiness.GetDirectUsersWithParentIdForSpaceTree((user.UserId == 0) ? UserId : user.UserId,
                        profileTitle,
                        roleTitle,
                        levelTitle,
                        notEnteredTitle,
                        domainNameTitle,
                        emailTitle,
                        registerDateTitle,
                        mobileTitle,
                        nationalCodeTitle,
                        moreInformationTitle,
                        nameFamilyTitle,
                        userNameTitle,
                        birthDateTitle,
                        "fa",
                        isActivatedText,
                        isDeactivatedText);
                    return JsonConvert.SerializeObject(usersList);
                    //var result = JsonConvert.SerializeObject(usersList);
                    //return result;
                }
            }
            catch (Exception exc)
            { }








            //string profileTitle = publicTexts.Where(t => t.PropertyName == "ProfileTitle").FirstOrDefault().Value;
            //string roleTitle = publicTexts.Where(t => t.PropertyName == "RoleTitle").FirstOrDefault().Value;
            //string levelTitle = publicTexts.Where(t => t.PropertyName == "LevelTitle").FirstOrDefault().Value;
            //string notEnteredTitle = publicTexts.Where(t => t.PropertyName == "NotEnteredTitle").FirstOrDefault().Value;
            //string domainNameTitle = publicTexts.Where(t => t.PropertyName == "DomainNameTitle").FirstOrDefault().Value;
            //string emailTitle = publicTexts.Where(t => t.PropertyName == "EmailTitle").FirstOrDefault().Value;
            //string registerDateTitle = publicTexts.Where(t => t.PropertyName == "RegisterDateTitle").FirstOrDefault().Value;
            //string mobileTitle = publicTexts.Where(t => t.PropertyName == "MobileTitle").FirstOrDefault().Value;
            //string nationalCodeTitle = publicTexts.Where(t => t.PropertyName == "NationalCodeTitle").FirstOrDefault().Value;
            //string moreInformationTitle = publicTexts.Where(t => t.PropertyName == "MoreInformationTitle").FirstOrDefault().Value;
            //string nameFamilyTitle = publicTexts.Where(t => t.PropertyName == "NameFamilyTitle").FirstOrDefault().Value;
            //string userNameTitle = publicTexts.Where(t => t.PropertyName == "NameFamilyTitle").FirstOrDefault().Value;
            //string birthDateTitle = publicTexts.Where(t => t.PropertyName == "BirthDateTitle").FirstOrDefault().Value;
            //List<CustomUsersVM> usersList = new List<CustomUsersVM>();
            //try
            //{
            //    if (business.ExistUserWithUserId(UserId, this.domain))
            //    {
            //        var user = business.GetUserWithUserId(UserId);
            //        ViewData["ParentUser"] = user;
            //        var role = business.GetRoleWithUserId/*AndDomainName*/(UserId/*, user.DomainSettingId*/);
            //        usersList = business.GetUsersList(0/*user.DomainSettingId*/, 0, user.UserId);

            //        string nameFamily = "";
            //        if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Family))
            //            nameFamily = nameFamilyTitle + "<br />" + notEnteredTitle;
            //        else
            //            nameFamily = nameFamilyTitle + "<br />" + user.Name + " " + user.Family;

            //        string levelName = user.LevelName;

            //        string dataContent = domainNameTitle + " : " + (string.IsNullOrEmpty(user.DomainName) ? notEnteredTitle : user.DomainName) +
            //            "<br />" + emailTitle + " : " + (string.IsNullOrEmpty(user.Email) ? notEnteredTitle : user.Email) +
            //            "<br />" + birthDateTitle + " : " + (string.IsNullOrEmpty(user.BirthDateTime) ? notEnteredTitle : user.BirthDateTime) +
            //            "<br />" + nationalCodeTitle + " : " + (string.IsNullOrEmpty(user.NationalCode) ? notEnteredTitle : user.NationalCode) +
            //            "<br />" + mobileTitle + " : " + (string.IsNullOrEmpty(user.Mobile) ? notEnteredTitle : user.Mobile);

            //        string activeateDeactivateStatusText = "";

            //        if (user.IsActivated)
            //            activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsActivated")).Value;
            //        else
            //            activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsDeactivated")).Value;

            //        string hierarchyOfUsersDataString = "";

            //        string userImage = "";
            //        if (!string.IsNullOrEmpty(user.Picture))
            //        {
            //            userImage = "<img src='/Files/UserFiles/" + user.DomainName + "/" +
            //                            user.UserId + "/thumb_" + user.Picture + "' class='userThumbImage' />";
            //        }
            //        else
            //        {
            //            userImage = "<i class='fa fa-4x fa-user'></i>";
            //        }

            //        //string html = "\"<div class='row'><div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
            //        //    userImage + "</div>" +
            //        //    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding' data-toggle='popover' data-html='True'" +
            //        //    "data-content='" + dataContent + "' data-original-title='" + moreInformationTitle + "'" +
            //        //    " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
            //        //    nameFamily + "<br />" + user.UserName + "</div></div><div class='parent-show-profile'><a " +
            //        //    "class='show-profile' data-userId='1'>" + profileTitle + "</a></div>\", ";

            //        string html = "\"<div class='card kl-card kl-xl kl-reveal kl-overlay kl-show kl-slide-in kl-shine kl-hide kl-spin'>" +
            //                                 "<div class='kl-card-block kl-lg bg-success kl-shadow-br kl-overlay'>" +
            //                    "<div class='kl-background'>" +
            //                        "<div class='row'>" +
            //                            "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
            //                                userImage +
            //                            "</div>" +
            //                            "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'><a class='showMoreInfo' data-toggle='popover' " +
            //                                    "tabindex='0' data-trigger='focus' data-html='True' " +
            //                                    "data-content='" + dataContent + "' data-container='body' data-original-title='" + moreInformationTitle + "'" +
            //                                    " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
            //                                registerDateTitle + ":" +
            //                                "<br />" +
            //                                user.RegisterDate +
            //                                "<br />" +
            //                                (!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "09122060766") +
            //                            "</a></div>" +
            //                        "</div>" +
            //                    //userImage +
            //                    "</div>" +
            //                    "<div class='kl-card-overlay kl-card-overlay-split-v-4 kl-dark kl-inverse kl-bottom-in'>" +
            //                        "<div class='kl-card-overlay-item'></div>" +
            //                        "<div class='kl-card-overlay-item'></div>" +
            //                        "<div class='kl-card-overlay-item'></div>" +
            //                        "<div class='kl-card-overlay-item'></div>" +
            //                    "</div>" +
            //                    "<div class='kl-card-item kl-pbl kl-show text-white kl-txt-shadow'>" +
            //                    //"<div class='kl-figure-block'>" +
            //                    //    "<span class='kl-figure'>109</span>" +
            //                    //    "<span class='kl-title text-white'>Following</span>" +
            //                    //"</div>" +
            //                    "</div>" +
            //                    "<div class='kl-card-item kl-pbr kl-show text-white kl-txt-shadow'>" +
            //                    //"<div class='kl-figure-block'>" +
            //                    //    "<span class='kl-figure>26k</span>" +
            //                    //    "<span class='kl-title text-white'>Followers</span>" +
            //                    //"</div>" +
            //                    "</div>" +
            //                    "<div class='kl-card-item kl-pm kl-show text-white kl-txt-shadow text-center'>" +
            //                    //birthDateTitle + ":" +
            //                    //user.BirthDateTime +
            //                    //"<br />" +
            //                    //(!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "") +
            //                    //"<h3 class='mb-0'>Joe Bloggs</h3>" +
            //                    //"<div class='text-white small'>asdasdasd</div>" +
            //                    "</div>" +
            //                    //"<div class='kl-card-item kl-ptl kl-v kl-card-social kl-slide-in'>" +
            //                    //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEa0lEQVRYR8VXa2hbZRh+3vckXbu09cKKOJyC29y8IZQmduiw9YesDjbc7HqTIV46GV5okjrdhAZRytY2HbMiE0FhNqdtZAiiGwPpREGb1NUfq0xnHdNZ53602GzrJT3fKydb2zTXs2xgfoXwvM/zfO/75vnOIfzPH8pVv7qvT3P8tbrokiZ2x9i/kU98lVO5cFk2UOHrty0vKnxCkWwmhUeUqHuY2T4vauAfEIaE6BgMrbdnV+moFUNZDZjCtxc7GiHqDYBXWCFVUIrBujK4pff1spFMNRkNNLT/cJ8SDoDxkBXhJIySaWF6a23E6ff5SKXiSGugtm1gI7P0CXhpTuJxRULqcP7EZEOqPUlpwBQnps8B2K5XfK6eSB0pdNg2f7ijLBrPmWTAbLsQwjfi5InmBdLV43n4lbQGYgvncAzmPHMr7SJU6W7X0fnOxNfU+UM7IXjfCk+uGFHy2/lLl+897qucNTnmRxA7fVHB71b/avEGFGhcgwQEmFBQTzF4LRTGwLg15eYLngl4Xd2LDNR3hJ8UyJdpTybyoiJpZfCyeIxBOGePqnXdu8rPmb83Hhy0RyJSQSSrBGo/wHlJnITjuttVuchArT90kASN6QywXd00O2Ur0bSoLtCccTPcGfC4Pkisq20LVwgbwUTDJs4MKm0muizw5vrx+RHUtYdOgnB/2g4QKnS36xufT/jXotBWBdouSj0KDTW97vJjyQYGThPTqnR8BNoY8Di/ihkwLxY+e+fkomxfNGQl0HjvmgnnnkWJJkLVwSAHt20z4uHVvuE8myMyBea0QSdC7h6vszMGeLZz6OZpFR3PuNmEB3S3a9jK9td0DpayUj9mwbbqHtfuKx04cKLEFp29kKmAgK8jhbzpix1ll7OZqG8P7xaSdzPiBB261+W90gFff/50kWMyGzFAL82uOPNRYsvj66oOnF5SHB37hUF3Zeloi+52vb2whPtC56HhtoxdIDlU6NCeT8zz+Jq6jvA7gOzJdhgR2t7jdR5aMNAWOgLGhmyFCuqUTbC121v+c9Lmd4RqSCk90/LN1ShFD/Y2O0/OG6htDzcRiT+VAQO4oCk6Ck1GWKnPEsVjKVq41H01qDjbIQCM6m7nHSCSBQN7TywX28yfDE5JIILvQfhY2PiuAPl/TxuGjVjuFiWPK8ZzLFhtQTgGIaA94HE1X/2+UFbXEfoUQINVohxxs7M2Xhl8reyPJAM1+wZXMhnDYFqSI7mVsv26x9U0B0xKqjp/uBki+6wwXTtGRtgupd2vlk+kNWBm/anigSAJb7l2gQwVSi6SjdYHmlw/xaNSZrUZTDPFBYdFuOqGmDDFWTYFPOX9iXxpLwvzXp+4aPgJ9PL1mZARYno68eRpR5AoVucPbRBD3st0taYxaD5ydbFdtcTP3HIH4oGx17JCR40wXoChHsuSdKMEBCC2roC39Gy27mV9NUskqG/99hbk5a9TgjVEUgJBHhgTouiMCIZ6vWXDZsJlE7Y8AqtEueL+Az80sTB3CtXzAAAAAElFTkSuQmCC'></a>" +
            //                    //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAD9klEQVRYR8WXX2hbdRTHv+fcpOscisJG16gvTnQqIig+JFm1vsjmwOGwf7INrTa9lakPGw6GCA2+CKL1XxXS1E3QpUkKwxedzAer7CYTJxPZcMKmzOncFBxWx7o29xy50bZJ1uT+murM04Wc8/1+fuece+69hP/5R436d3TkrF+Xnb/SPa9Baxn9Mf7OY5ONaBkDtLcnAtOha+9XkQ3qcpQIN4ERnDUVOUvgw2DaD1DWyfSeNgHyBfCMp1pDNlR3Any9iSgEAgujEBnIZ/tP1MupC9DWsevWaauYZtAdRsZVQQJcJNBzhdU/DiKRkPk0agJEukfWA5oDcEUj5uU5Ctm7pLlp83xzMi+AZy7Q9xkILNZ8Jl9B+6aucjd8Odw/Xa55CYBXdtdyv/g3Tl4Nr9ChQsZ+uiaAN3CTK0OHjHsuclaZxgA+CdIiAy2q2Fmvakq0rjAa/2gmpqICkdjwVii9aVJ2l+RbLlqRwlj8t5n4cCy5mpS/qZ8vx5vO/HzL+Hii6MXNApRut5Wt3xnfalA7n7FT5WZmAACpbnGy9p4KgOim1AMq+MDk9F6MCK05mIs73nWke7gH4AERt4mZQ34aCh0vZOz7KgAi3ckkwLZf8lzv+G4n03uoBNCVeh2EiuGqqyMQDkwvP5Deem62BeGO1BGycJsfALnytTJ/HCDrtc8yj5/y4sPdww+RUlQJywl41E+jdHLGeifd92EJwHuw/ES/X6jY7TVUSDXpZO0n5vs70jlyD1g/NQIAbXcy8VdKAO09u6+emiyeM0qsAxCOpWKkSJvoKPSFQsZ+tgTQ1rFrhWu5v5gkwnvqMR0X0nhhtP/Y3zMwvJ0IG0W1lYhvMNJRvJzP9j0zU4HmqcniBaPEf4IIc0MY7UwNKePJBeUrDTjZ+POzQxjpTJ4Bc4upyGIBAHokn4m/OwsQ7UztU8baywagdHs+Gz8yB9A9sk2hg5cDQEROH8zZ13k7sQzg7ZCKnAKDTSAW0wKCvuRk7B0Vm7A0zbHUe1Bs/i8BBCgGXFl1YKz/h0sBupKrhPgoA0v8IBqtACledbJ92+ZWepVTtCu1Qwkv+gFAMC2M0nseC4ImrRPghFpL7/x8z5aJmgBIJDh8rHWMwBt9IRYQIMCfzNqWT9tflafN+07Y3rO7+eKku5eg6xbgUTPUM7eEH3RyvZ9UB9V8K77LTgabJmiQQE8tBsIrO7M+XH3y2i2ocgvHRtaSum8AfONCQLxptxRDxcDSgfKeG1egPLD0WdYS6hJCnKD3Alyzct6SsZjSyjyUT8dP+kH7fppVC6zZ9NY1imAYQjcLZAUpNRFoQgnfQ3E4n+096m04P2PjFpgKNRr3F+G5pDBOJtRGAAAAAElFTkSuQmCC'></a>" +
            //                    //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEdElEQVRYR8WXXYiUZRTH/+e8szOLzrBKmlF2EUaRy9K+874QYtJ2U5qSEG5dWN1URlIXRkKIYRghRZ9oF3WZpahhEZTRTUNd6T7PvIHuRbhbVGRfhDszC+rs7DnxLLvLOM774Rr0XM57Pn7n63nOEP7nQwv1Pzw87J09e7ZERD19fX2NSqVycSG2MgMMDQ3lJicn7wOwWVXXishtzNwz51RE/vQ8LwLw9fT09JEois5lAUoFcI4bjcY2EXmRmW/OYhSAADgMYI+1djxJJxHA9/3VzHwIwJ0ZHXeKXQKw21r71izUFWZiAYIg2CgiR5l50QKdz6sR0fFisbi1W590BZh1/hkz567V+Zy+qp4gos3W2ql2m1cAuLQDGPkvIu+EV9UD1Wr1uViA2YYz11Dz1ISp6oZqtfrVnOBlGQiCYDuA97pZERGXukEiupuIdonITQDOMbOb/6KIrGDmgy5KAJuI6OUYmrFSqXRHpVJpue/zAC76Wq32Y9yoich4FEW3OqUgCBaVSiVpb6r+/v786Oho030vl8sPE9GRuHSo6qPVavXjywDCMHxAVb+IUxKRf6IoWpaaYwBhGG5R1WMJtipRFN3bCfC+qm6LUyKi3caYV7MAuMz6vv8dM6+NkZdms7ns9OnT5+dLEATBGQD9CdTXR1H0d0YAV4aniOiDhIA2GmO+nAFwD8vY2NiF9ru9Q1GttZzV+Wwf3E9E893eqUtEzxtj3p4BGBwcXOJ53vkUB0ustbWsEEEQDAM4miC/z1q7awbA9/3lzPxXivFN1trYJu3U9X1/LzO/lFDSN6MoemEGYGhoqLfRaFxIAiAiUywW12V594Mg6AMwCsDdFXHHvZR755vQ9/0/mHlFjHRTRJiITjHzDmPMCADtIktBELhd4V1mLqcE9Lgx5uA8QBiGJ1R1fTclVf2BiJ4B8CGAlQB+I6LtxpjP5+TDMHxienr6NWa+LkufENGAMeZMO8AOVXXvdtdDRN+q6rMAXgHQrNVqjxUKhZ5cLtfXarVqvb29vQCyjuk5a60LRNtLcCMz/wogdtxExDnfn8/nL7VarcUAVqnqYmYuGWM+8X2/mTDK7YG9Ya3d6X7ofIw+ArA1LYVu/8vn8+HU1NQtABYx81JjzFHf9y+mAYhIi5lXWWt/6QawarZ7C0kQIvJ7Lpe7q9VqrSSiG5i5MDEx8WmpVJpMW2KI6B1jzI45+1csJOVyeScRvZ6Shb1E5KbgsKruY+Z9IrJGVVcT0dMJsz+ez+fLJ0+erMcCuB4Iw/CYqj6UAjEBwN2ergxjIrKSmV0jxh2XnXUjIyPftwt03QndxVSv148T0Ya0fsj4fRLAg9babzrlk7biHjeWRORGb8HHLTK5XG5LZ+RJJbjMWblcXk9E+wHMbENZj+t2z/MOeJ63p73mmTPQLujWtXq9/oiqPsnM93SOb4dR95fskIgciKLo5zTg1L9mnQYGBgaWFgqFNQBuV9XlIpJn5joR/QQgMsa4R6jbO9GV5aoB0iK62u//AuhE0jAWsl0eAAAAAElFTkSuQmCC'></a>" +
            //                    //"</div>" +
            //                    "<div class='kl-card-item kl-pb kl-show online_status'>" +
            //                        (user.IsActivated ? "<span class='badge badge-success green'>" : "<span class='badge badge-success red'>") +
            //                            //"<span class='badge badge-success green'>" + 
            //                            activeateDeactivateStatusText +
            //                        "</span>" +
            //                    "</div>" +
            //                    "<div class='kl-card-avatar kl-xl kl-pm kl-slow kl-hide'>" +
            //                        "<div class='row'>" +
            //                            "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
            //                                userImage +
            //                            "</div>" +
            //                            "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'>" +
            //                                nameFamily +
            //                                "<br />" +
            //                                /*userNameTitle + 
            //                                "<br />" + */
            //                                user.UserName +
            //                            "</div>" +
            //                        "</div>" +
            //                    "</div>" +
            //                "</div>" +
            //            "</div>\", ";

            //        hierarchyOfUsersDataString += " { id: \"" + user.UserId.ToString() + "\",";
            //        hierarchyOfUsersDataString += " name: " + html;
            //        hierarchyOfUsersDataString += " data: {}, ";
            //        hierarchyOfUsersDataString += " children: [  ";

            //        //hierarchyOfUsersDataString += hierarchyOfUsersData(user.UserId,
            //        //    usersList,
            //        //    profileTitle,
            //        //    roleTitle,
            //        //    levelTitle,
            //        //    notEnteredTitle,
            //        //    domainNameTitle,
            //        //    emailTitle,
            //        //    birthDateTitle,
            //        //    registerDateTitle,
            //        //    mobileTitle,
            //        //    nationalCodeTitle,
            //        //    moreInformationTitle);

            //        hierarchyOfUsersDataString += " ] } ";
            //        //ViewData["HierarchyOfUsersDataString"] = hierarchyOfUsersDataString;

            //        return hierarchyOfUsersDataString;
            //    }
            //}
            //catch (Exception exc)
            //{ }
            return "[]";
            //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfUsers", usersList);
        }

        public IActionResult ChangeParentId(long childUserId, long newParentUserId)
        {
            try
            {
                if (consoleBusiness.ChangeParentId(childUserId, newParentUserId))
                    return Json(new
                    {
                        Result = "OK",
                    });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> UploadFile(IFormFile file, long UserId)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        string fileName = "";
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(UserId);

                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        #region Remove Old User Picture

                        string picture = consoleBusiness.GetUserProfilePicture(UserId);

                        if (!string.IsNullOrEmpty(picture))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picture))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picture);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picture))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picture);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }
                        }

                        #endregion

                        string ext = Path.GetExtension(file.FileName);
                        fileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + fileName, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                            fileName,
                            60,
                            60);

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            if (consoleBusiness.UpdateUserPicture(UserId, fileName))
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                    userPicture = fileName,
                                    Message = "Success"
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                    Message = "ErrorUploadPicture"
                                });
                            }
                        }
                    }
                }

                return Json(new
                {
                    Result = "ERROR",
                    Message = ""
                });
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
        public IActionResult RemoveUserPicture(long Id)
        {
            try
            {
                var userProfilePicture = consoleBusiness.GetUserProfilePicture(Id);
                var user = consoleBusiness.GetUserWithUserId(Id);
                var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(user.DomainSettingId);

                var childsUSerIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.userId.Value);
                if (childsUSerIds.Contains(user.UserId))
                {
                    if (!string.IsNullOrEmpty(userProfilePicture))
                    {
                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        if (System.IO.File.Exists(userFolder + "\\" + domainSetting.DomainName + "\\" + Id + "\\" + userProfilePicture))
                        {
                            System.IO.File.Delete(userFolder + "\\" + domainSetting.DomainName + "\\" + Id + "\\" + userProfilePicture);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (System.IO.File.Exists(userFolder + "\\" + domainSetting.DomainName + "\\" + Id + "\\thumb_" + userProfilePicture))
                        {
                            System.IO.File.Delete(userFolder + "\\" + domainSetting.DomainName + "\\" + Id + "\\thumb_" + userProfilePicture);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (consoleBusiness.RemoveUserProfilePicture(Id, this.userId.Value))
                            return Json(new
                            {
                                Result = "OK",
                                //Message = "Success"
                            });
                    }
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

        [HttpPost]
        [AjaxOnly]
        public IActionResult CreateExcel(long ParentId = 0,
            int DomainSettingId = 0,
            List<int> RoleIds = null,
            List<int> LevelIds = null,
            string UserName = "",
            string Name = "",
            string Family = "",
            string Email = "",
            string Phone = "",
            string Mobile = "",
            string Address = "",
            string CertificateId = "",
            string NationalCode = "",
            string BirthDateTimeFrom = "",
            string BirthDateTimeTo = "",
            string RegisterDateTimeFrom = "",
            string RegisterDateTimeTo = "",
            bool? IsActivated = null)
        {
            try
            {
                string userExcelFilesFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ExcelsFiles\\" + this.userId.Value;

                if (!System.IO.Directory.Exists(userExcelFilesFolder))
                {
                    System.IO.Directory.CreateDirectory(userExcelFilesFolder);
                    System.Threading.Thread.Sleep(1500);
                }

                string usersListFile = userExcelFilesFolder + "\\UsersList.xls";

                if (System.IO.File.Exists(usersListFile))
                {
                    System.IO.File.Delete(usersListFile);
                    System.Threading.Thread.Sleep(1500);
                }
                using (System.IO.FileStream fs = new System.IO.FileStream(usersListFile, System.IO.FileMode.Create))
                {
                }

                DateTime? BirthDateTimeEnFrom = null;
                if (!string.IsNullOrEmpty(BirthDateTimeFrom))
                    BirthDateTimeEnFrom = PersianDate.ToGregorianDate(BirthDateTimeFrom.ToEnglishDigits());

                DateTime? BirthDateTimeEnTo = null;
                if (!string.IsNullOrEmpty(BirthDateTimeTo))
                    BirthDateTimeEnTo = PersianDate.ToGregorianDate(BirthDateTimeTo.ToEnglishDigits());

                DateTime? RegisterDateTimeEnFrom = null;
                if (!string.IsNullOrEmpty(RegisterDateTimeFrom))
                    RegisterDateTimeEnFrom = PersianDate.ToGregorianDate(RegisterDateTimeFrom.ToEnglishDigits());

                DateTime? RegisterDateTimeEnTo = null;
                if (!string.IsNullOrEmpty(RegisterDateTimeTo))
                    RegisterDateTimeEnTo = PersianDate.ToGregorianDate(RegisterDateTimeTo.ToEnglishDigits());

                string separator = " ";

                var usersList = consoleBusiness.GetUsersList((ParentId == 0) ? this.userId.Value : ParentId,
                    DomainSettingId,
                    RoleIds,
                    LevelIds,
                    UserName,
                    Name,
                    Family,
                    Email,
                    Phone,
                    Mobile,
                    Address,
                    CertificateId,
                    NationalCode,
                    BirthDateTimeEnFrom,
                    BirthDateTimeEnTo,
                    RegisterDateTimeEnFrom,
                    RegisterDateTimeEnTo,
                    IsActivated,
                    "fa",
                    separator);

                //if ("fa" != "en")
                //{
                //    foreach (var user in usersList)
                //    {
                //        user.Lang = "fa";
                //        //if (educationalGroup.EstablishedEnDate.HasValue)
                //        //{

                //        //}
                //    }
                //}

                if (usersList.Count > 100)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "UserRecordsCountError"
                    });
                }


                string userNameTitle = "نام کاربری";
                string emailTitle = "ایمیل";
                string nameTitle = "نام";
                string familyTitle = "نام خانوادگی";
                string nameFamilyOfUserCreatorTitle = "نام و نام خانوادگی والد";
                string domainNameTitle = "نام دامنه";
                string roleNameTitle = "نام نقش";
                string levelNameTitle = "نام سطح دسترسی";
                string phoneTitle = "شماره تماس";
                string mobileTitle = "موبایل";
                string addressTitle = "آدرس";
                string certificateIdTitle = "شماره شناسنامه";
                string nationalCodeTitle = "کد ملی";
                string birthDateTimeTitle = "تاریخ تولد";
                string registerDateTitle = "تاریخ ثبت";
                string statusTitle = "وضعیت";
                string isActivatedTitle = "فعال";
                string isDeactivateTitle = "غیر فعال";

                DataTable dt = CreateDataTable(userNameTitle,
                    emailTitle,
                    nameTitle,
                    familyTitle,
                    nameFamilyOfUserCreatorTitle,
                    domainNameTitle,
                    roleNameTitle,
                    levelNameTitle,
                    phoneTitle,
                    mobileTitle,
                    addressTitle,
                    certificateIdTitle,
                    nationalCodeTitle,
                    birthDateTimeTitle,
                    registerDateTitle,
                    statusTitle);

                foreach (var user in usersList)
                {
                    try
                    {
                        string statusText = "";

                        switch (user.IsActivated)
                        {
                            case true:
                                statusText = isActivatedTitle;
                                break;
                            case false:
                                statusText = isDeactivateTitle;
                                break;
                        }

                        DataRow row = dt.NewRow();
                        row[userNameTitle] = user.UserName;
                        row[emailTitle] = user.Email;
                        row[nameTitle] = user.Name;
                        row[familyTitle] = user.Family;
                        row[nameFamilyOfUserCreatorTitle] = user.NameFamilyOfUserCreator;
                        row[domainNameTitle] = user.DomainName;
                        row[roleNameTitle] = user.RoleName;
                        row[levelNameTitle] = user.LevelName;
                        row[phoneTitle] = user.Phone;
                        row[mobileTitle] = user.Mobile;
                        row[addressTitle] = user.Address;
                        row[certificateIdTitle] = user.CertificateId;
                        row[nationalCodeTitle] = user.NationalCode;
                        row[birthDateTimeTitle] = user.BirthDateTime;
                        row[registerDateTitle] = user.RegisterDate;
                        row[statusTitle] = statusText;
                        dt.Rows.Add(row);
                    }
                    catch (Exception exc)
                    { }
                }

                var result = ExcelWriter.WriteDataTableToExcel(dt, usersListFile);

                if (result == null)
                {
                    if (System.IO.File.Exists(userExcelFilesFolder + "\\UsersList.xls"))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            FileName = "UsersList"
                        });
                    }
                }
                else
                {
                    return Json(new { Result = "OK", Message = result.Message + " - " + ((result.InnerException != null) ? result.InnerException.Message : "") });
                }
            }
            catch (Exception exc)
            {
                return Json(new { Result = "OK", Message = exc.Message + " - " + exc.InnerException != null ? exc.InnerException.Message : "" });
            }

            return Json(new { Result = "ERROR", Message = "خطا" });
        }

        public async Task<IActionResult> Download(string fileName)
        {
            if (fileName == null)
                return Content("FileNotFound");

            string userExcelFilesFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ExcelsFiles\\" + this.userId.Value + "\\";

            if (System.IO.File.Exists(userExcelFilesFolder + fileName + ".xls"))
            {
                //var path = Path.Combine(
                //               Directory.GetCurrentDirectory(),
                //               "wwwroot", fileName);

                var filePath = userExcelFilesFolder + fileName + ".xls";

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath/*path*/, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, ContentTypeManagement.GetContentType(filePath), Path.GetFileName(filePath));
            }

            //if (filename == null)
            //    return Content("filename not present");

            //var path = Path.Combine(
            //               Directory.GetCurrentDirectory(),
            //               "wwwroot", filename);

            //var memory = new MemoryStream();
            //using (var stream = new FileStream(path, FileMode.Open))
            //{
            //    await stream.CopyToAsync(memory);
            //}
            //memory.Position = 0;
            //return File(memory, GetContentType(path), Path.GetFileName(path));
            return null;
        }

        #region Needed Methods

        private DataTable CreateDataTable(string userNameTitle,
                    string emailTitle,
                    string nameTitle,
                    string familyTitle,
                    string nameFamilyOfUserCreatorTitle,
                    string domainNameTitle,
                    string roleNameTitle,
                    string levelNameTitle,
                    string phoneTitle,
                    string mobileTitle,
                    string addressTitle,
                    string certificateIdTitle,
                    string nationalCodeTitle,
                    string birthDateTimeTitle,
                    string registerDateTitle,
                    string statusTitle)
        {
            DataTable dt = new DataTable();
            dt.TableName = "UsersList";

            DataColumn col = new DataColumn(userNameTitle, typeof(string));
            col.Caption = userNameTitle;//"نام کاربری";
            dt.Columns.Add(col);

            col = new DataColumn(emailTitle, typeof(string));
            col.Caption = emailTitle;//"ایمیل";
            dt.Columns.Add(col);

            col = new DataColumn(nameTitle, typeof(string));
            col.Caption = nameTitle;//"نام";
            dt.Columns.Add(col);

            col = new DataColumn(familyTitle, typeof(string));
            col.Caption = familyTitle;//"نام خانوادگی";
            dt.Columns.Add(col);

            col = new DataColumn(nameFamilyOfUserCreatorTitle, typeof(string));
            col.Caption = nameFamilyOfUserCreatorTitle;//"نام و نام خانوادگی والد";
            dt.Columns.Add(col);

            col = new DataColumn(domainNameTitle, typeof(string));
            col.Caption = domainNameTitle;//"نام دامنه";
            dt.Columns.Add(col);

            col = new DataColumn(roleNameTitle, typeof(string));
            col.Caption = roleNameTitle;//"نام نقش";
            dt.Columns.Add(col);

            col = new DataColumn(levelNameTitle, typeof(string));
            col.Caption = levelNameTitle;//"نام سطح دسترسی";
            dt.Columns.Add(col);

            col = new DataColumn(phoneTitle, typeof(string));
            col.Caption = phoneTitle;//"شماره تماس";
            dt.Columns.Add(col);

            col = new DataColumn(mobileTitle, typeof(string));
            col.Caption = mobileTitle;//"موبایل";
            dt.Columns.Add(col);

            col = new DataColumn(addressTitle, typeof(string));
            col.Caption = addressTitle;//"آدرس";
            dt.Columns.Add(col);

            col = new DataColumn(certificateIdTitle, typeof(string));
            col.Caption = certificateIdTitle;//"شماره شناسنامه";
            dt.Columns.Add(col);

            col = new DataColumn(nationalCodeTitle, typeof(string));
            col.Caption = nationalCodeTitle;//"کد ملی";
            dt.Columns.Add(col);

            col = new DataColumn(birthDateTimeTitle, typeof(string));
            col.Caption = birthDateTimeTitle;//"تاریخ تولد";
            dt.Columns.Add(col);

            col = new DataColumn(registerDateTitle, typeof(string));
            col.Caption = registerDateTitle;//"تاریخ ثبت";
            dt.Columns.Add(col);

            col = new DataColumn(statusTitle, typeof(string));
            col.Caption = statusTitle;//"وضعیت";
            dt.Columns.Add(col);

            return dt;
        }

        //private string GetContentType(string path)
        //{
        //    var types = GetMimeTypes();
        //    var ext = Path.GetExtension(path).ToLowerInvariant();
        //    return types[ext];
        //}

        //private Dictionary<string, string> GetMimeTypes()
        //{
        //    return new Dictionary<string, string>
        //    {
        //        {".txt", "text/plain"},
        //        {".pdf", "application/pdf"},
        //        {".doc", "application/vnd.ms-word"},
        //        {".docx", "application/vnd.ms-word"},
        //        {".xls", "application/vnd.ms-excel"},
        //        {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
        //        {".png", "image/png"},
        //        {".jpg", "image/jpeg"},
        //        {".jpeg", "image/jpeg"},
        //        {".gif", "image/gif"},
        //        {".csv", "text/csv"}
        //    };
        //}

        //private string hierarchyOfUsersData(long parentUserId, List<CustomUsersVM> usersList,
        //    string profileTitle,
        //    string roleTitle,
        //    string levelTitle,
        //    string notEnteredTitle,
        //    string domainNameTitle,
        //    string emailTitle,
        //    string birthDateTitle,
        //    string registerDateTitle,
        //    string mobileTitle,
        //    string nationalCodeTitle,
        //    string moreInformationTitle)
        //{
        //    string hierarchyOfUsersDataString = "";
        //    if (usersList.Any(u => u.ParentUserId.Equals(parentUserId)))
        //    {
        //        var childUsersList = usersList.Where(u => u.ParentUserId.Equals(parentUserId)).ToList();
        //        foreach (var childUser in childUsersList)
        //        {
        //            string nameFamily = "";
        //            if (string.IsNullOrEmpty(childUser.Name) && string.IsNullOrEmpty(childUser.Family))
        //                nameFamily = notEnteredTitle;
        //            else
        //                nameFamily = childUser.Name + " " + childUser.Family;

        //            string levelName = childUser.LevelName;

        //            string dataContent = domainNameTitle + " : " + (string.IsNullOrEmpty(childUser.DomainName) ? notEnteredTitle : childUser.DomainName) +
        //                "<br />" + emailTitle + " : " + (string.IsNullOrEmpty(childUser.Email) ? notEnteredTitle : childUser.Email) +
        //                "<br />" + birthDateTitle + " : " + (string.IsNullOrEmpty(childUser.BirthDateTime) ? notEnteredTitle : childUser.BirthDateTime) +
        //                "<br />" + nationalCodeTitle + " : " + (string.IsNullOrEmpty(childUser.NationalCode) ? notEnteredTitle : childUser.NationalCode) +
        //                "<br />" + mobileTitle + " : " + (string.IsNullOrEmpty(childUser.Mobile) ? notEnteredTitle : childUser.Mobile);

        //            string activeateDeactivateStatusText = "";

        //            if (childUser.IsActivated)
        //                activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsActivated")).Value;
        //            else
        //                activeateDeactivateStatusText = publicTexts.FirstOrDefault(p => p.PropertyName.Equals("IsDeactivated")).Value;

        //            hierarchyOfUsersDataString += " { id: \"" + childUser.UserId.ToString() + "\",";

        //            string userImage = "";
        //            if (!string.IsNullOrEmpty(childUser.Picture))
        //            {
        //                userImage = "<img src='/Files/UserFiles/" + childUser.DomainName + "/" +
        //                                childUser.UserId + "/thumb_" + childUser.Picture + "' class='userThumbImage' />";
        //            }
        //            else
        //            {
        //                userImage = "<i class='fa fa-4x fa-user'></i>";
        //            }

        //            //string html = "\"<div class='row'><div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
        //            //    userImage + "</div>" +
        //            //    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding' data-toggle='popover' data-html='True'" +
        //            //    "data-content='" + dataContent + "' data-original-title='" + moreInformationTitle + "'" +
        //            //    " data-placement='top' title='" + roleTitle + " : " + childUser.RoleName + " " + levelTitle + " : " + levelName + "'>" +
        //            //    nameFamily + "<br />" + childUser.UserName + "</div></div><div class='parent-show-profile'><a " +
        //            //    "class='show-profile' data-userId='1'>" + profileTitle + "</a></div>\", ";

        //            string html = "\"<div class='card kl-card kl-xl kl-reveal kl-overlay kl-show kl-slide-in kl-shine kl-hide kl-spin'>" +
        //                                     "<div class='kl-card-block kl-lg bg-success kl-shadow-br kl-overlay'>" +
        //                        "<div class='kl-background'>" +
        //                            "<div class='row'>" +
        //                                "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
        //                                    userImage +
        //                                "</div>" +
        //                                "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'><a class='showMoreInfo' data-toggle='popover'" +
        //                                        " tabindex='0' data-trigger='focus' data-html='True' " +
        //                                        "data-content='" + dataContent + "' data-container='body' data-original-title='" + moreInformationTitle + "'" +
        //                                        " data-placement='top' title='" + roleTitle + " : " + childUser.RoleName + " " + levelTitle + " : " + levelName + "'>" +
        //                                    registerDateTitle + ":" +
        //                                    "<br />" +
        //                                    childUser.RegisterDate +
        //                                    "<br />" +
        //                                    (!string.IsNullOrEmpty(childUser.Mobile) ? childUser.Mobile : "09122060766") +
        //                                "</a></div>" +
        //                            "</div>" +
        //                        //userImage +
        //                        "</div>" +
        //                        "<div class='kl-card-overlay kl-card-overlay-split-v-4 kl-dark kl-inverse kl-bottom-in'>" +
        //                            "<div class='kl-card-overlay-item'></div>" +
        //                            "<div class='kl-card-overlay-item'></div>" +
        //                            "<div class='kl-card-overlay-item'></div>" +
        //                            "<div class='kl-card-overlay-item'></div>" +
        //                        "</div>" +
        //                        "<div class='kl-card-item kl-pbl kl-show text-white kl-txt-shadow'>" +
        //                        //"<div class='kl-figure-block'>" +
        //                        //    "<span class='kl-figure'>109</span>" +
        //                        //    "<span class='kl-title text-white'>Following</span>" +
        //                        //"</div>" +
        //                        "</div>" +
        //                        "<div class='kl-card-item kl-pbr kl-show text-white kl-txt-shadow'>" +
        //                        //"<div class='kl-figure-block'>" +
        //                        //    "<span class='kl-figure>26k</span>" +
        //                        //    "<span class='kl-title text-white'>Followers</span>" +
        //                        //"</div>" +
        //                        "</div>" +
        //                        "<div class='kl-card-item kl-pm kl-show text-white kl-txt-shadow text-center'>" +
        //                        //birthDateTitle + ":" +
        //                        //user.BirthDateTime +
        //                        //"<br />" +
        //                        //(!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "") +
        //                        //"<h3 class='mb-0'>Joe Bloggs</h3>" +
        //                        //"<div class='text-white small'>asdasdasd</div>" +
        //                        "</div>" +
        //                        //"<div class='kl-card-item kl-ptl kl-v kl-card-social kl-slide-in'>" +
        //                        //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEa0lEQVRYR8VXa2hbZRh+3vckXbu09cKKOJyC29y8IZQmduiw9YesDjbc7HqTIV46GV5okjrdhAZRytY2HbMiE0FhNqdtZAiiGwPpREGb1NUfq0xnHdNZ53602GzrJT3fKydb2zTXs2xgfoXwvM/zfO/75vnOIfzPH8pVv7qvT3P8tbrokiZ2x9i/kU98lVO5cFk2UOHrty0vKnxCkWwmhUeUqHuY2T4vauAfEIaE6BgMrbdnV+moFUNZDZjCtxc7GiHqDYBXWCFVUIrBujK4pff1spFMNRkNNLT/cJ8SDoDxkBXhJIySaWF6a23E6ff5SKXiSGugtm1gI7P0CXhpTuJxRULqcP7EZEOqPUlpwBQnps8B2K5XfK6eSB0pdNg2f7ijLBrPmWTAbLsQwjfi5InmBdLV43n4lbQGYgvncAzmPHMr7SJU6W7X0fnOxNfU+UM7IXjfCk+uGFHy2/lLl+897qucNTnmRxA7fVHB71b/avEGFGhcgwQEmFBQTzF4LRTGwLg15eYLngl4Xd2LDNR3hJ8UyJdpTybyoiJpZfCyeIxBOGePqnXdu8rPmb83Hhy0RyJSQSSrBGo/wHlJnITjuttVuchArT90kASN6QywXd00O2Ur0bSoLtCccTPcGfC4Pkisq20LVwgbwUTDJs4MKm0muizw5vrx+RHUtYdOgnB/2g4QKnS36xufT/jXotBWBdouSj0KDTW97vJjyQYGThPTqnR8BNoY8Di/ihkwLxY+e+fkomxfNGQl0HjvmgnnnkWJJkLVwSAHt20z4uHVvuE8myMyBea0QSdC7h6vszMGeLZz6OZpFR3PuNmEB3S3a9jK9td0DpayUj9mwbbqHtfuKx04cKLEFp29kKmAgK8jhbzpix1ll7OZqG8P7xaSdzPiBB261+W90gFff/50kWMyGzFAL82uOPNRYsvj66oOnF5SHB37hUF3Zeloi+52vb2whPtC56HhtoxdIDlU6NCeT8zz+Jq6jvA7gOzJdhgR2t7jdR5aMNAWOgLGhmyFCuqUTbC121v+c9Lmd4RqSCk90/LN1ShFD/Y2O0/OG6htDzcRiT+VAQO4oCk6Ck1GWKnPEsVjKVq41H01qDjbIQCM6m7nHSCSBQN7TywX28yfDE5JIILvQfhY2PiuAPl/TxuGjVjuFiWPK8ZzLFhtQTgGIaA94HE1X/2+UFbXEfoUQINVohxxs7M2Xhl8reyPJAM1+wZXMhnDYFqSI7mVsv26x9U0B0xKqjp/uBki+6wwXTtGRtgupd2vlk+kNWBm/anigSAJb7l2gQwVSi6SjdYHmlw/xaNSZrUZTDPFBYdFuOqGmDDFWTYFPOX9iXxpLwvzXp+4aPgJ9PL1mZARYno68eRpR5AoVucPbRBD3st0taYxaD5ydbFdtcTP3HIH4oGx17JCR40wXoChHsuSdKMEBCC2roC39Gy27mV9NUskqG/99hbk5a9TgjVEUgJBHhgTouiMCIZ6vWXDZsJlE7Y8AqtEueL+Az80sTB3CtXzAAAAAElFTkSuQmCC'></a>" +
        //                        //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAD9klEQVRYR8WXX2hbdRTHv+fcpOscisJG16gvTnQqIig+JFm1vsjmwOGwf7INrTa9lakPGw6GCA2+CKL1XxXS1E3QpUkKwxedzAer7CYTJxPZcMKmzOncFBxWx7o29xy50bZJ1uT+murM04Wc8/1+fuece+69hP/5R436d3TkrF+Xnb/SPa9Baxn9Mf7OY5ONaBkDtLcnAtOha+9XkQ3qcpQIN4ERnDUVOUvgw2DaD1DWyfSeNgHyBfCMp1pDNlR3Any9iSgEAgujEBnIZ/tP1MupC9DWsevWaauYZtAdRsZVQQJcJNBzhdU/DiKRkPk0agJEukfWA5oDcEUj5uU5Ctm7pLlp83xzMi+AZy7Q9xkILNZ8Jl9B+6aucjd8Odw/Xa55CYBXdtdyv/g3Tl4Nr9ChQsZ+uiaAN3CTK0OHjHsuclaZxgA+CdIiAy2q2Fmvakq0rjAa/2gmpqICkdjwVii9aVJ2l+RbLlqRwlj8t5n4cCy5mpS/qZ8vx5vO/HzL+Hii6MXNApRut5Wt3xnfalA7n7FT5WZmAACpbnGy9p4KgOim1AMq+MDk9F6MCK05mIs73nWke7gH4AERt4mZQ34aCh0vZOz7KgAi3ckkwLZf8lzv+G4n03uoBNCVeh2EiuGqqyMQDkwvP5Deem62BeGO1BGycJsfALnytTJ/HCDrtc8yj5/y4sPdww+RUlQJywl41E+jdHLGeifd92EJwHuw/ES/X6jY7TVUSDXpZO0n5vs70jlyD1g/NQIAbXcy8VdKAO09u6+emiyeM0qsAxCOpWKkSJvoKPSFQsZ+tgTQ1rFrhWu5v5gkwnvqMR0X0nhhtP/Y3zMwvJ0IG0W1lYhvMNJRvJzP9j0zU4HmqcniBaPEf4IIc0MY7UwNKePJBeUrDTjZ+POzQxjpTJ4Bc4upyGIBAHokn4m/OwsQ7UztU8baywagdHs+Gz8yB9A9sk2hg5cDQEROH8zZ13k7sQzg7ZCKnAKDTSAW0wKCvuRk7B0Vm7A0zbHUe1Bs/i8BBCgGXFl1YKz/h0sBupKrhPgoA0v8IBqtACledbJ92+ZWepVTtCu1Qwkv+gFAMC2M0nseC4ImrRPghFpL7/x8z5aJmgBIJDh8rHWMwBt9IRYQIMCfzNqWT9tflafN+07Y3rO7+eKku5eg6xbgUTPUM7eEH3RyvZ9UB9V8K77LTgabJmiQQE8tBsIrO7M+XH3y2i2ocgvHRtaSum8AfONCQLxptxRDxcDSgfKeG1egPLD0WdYS6hJCnKD3Alyzct6SsZjSyjyUT8dP+kH7fppVC6zZ9NY1imAYQjcLZAUpNRFoQgnfQ3E4n+096m04P2PjFpgKNRr3F+G5pDBOJtRGAAAAAElFTkSuQmCC'></a>" +
        //                        //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEdElEQVRYR8WXXYiUZRTH/+e8szOLzrBKmlF2EUaRy9K+874QYtJ2U5qSEG5dWN1URlIXRkKIYRghRZ9oF3WZpahhEZTRTUNd6T7PvIHuRbhbVGRfhDszC+rs7DnxLLvLOM774Rr0XM57Pn7n63nOEP7nQwv1Pzw87J09e7ZERD19fX2NSqVycSG2MgMMDQ3lJicn7wOwWVXXishtzNwz51RE/vQ8LwLw9fT09JEois5lAUoFcI4bjcY2EXmRmW/OYhSAADgMYI+1djxJJxHA9/3VzHwIwJ0ZHXeKXQKw21r71izUFWZiAYIg2CgiR5l50QKdz6sR0fFisbi1W590BZh1/hkz567V+Zy+qp4gos3W2ql2m1cAuLQDGPkvIu+EV9UD1Wr1uViA2YYz11Dz1ISp6oZqtfrVnOBlGQiCYDuA97pZERGXukEiupuIdonITQDOMbOb/6KIrGDmgy5KAJuI6OUYmrFSqXRHpVJpue/zAC76Wq32Y9yoich4FEW3OqUgCBaVSiVpb6r+/v786Oho030vl8sPE9GRuHSo6qPVavXjywDCMHxAVb+IUxKRf6IoWpaaYwBhGG5R1WMJtipRFN3bCfC+qm6LUyKi3caYV7MAuMz6vv8dM6+NkZdms7ns9OnT5+dLEATBGQD9CdTXR1H0d0YAV4aniOiDhIA2GmO+nAFwD8vY2NiF9ru9Q1GttZzV+Wwf3E9E893eqUtEzxtj3p4BGBwcXOJ53vkUB0ustbWsEEEQDAM4miC/z1q7awbA9/3lzPxXivFN1trYJu3U9X1/LzO/lFDSN6MoemEGYGhoqLfRaFxIAiAiUywW12V594Mg6AMwCsDdFXHHvZR755vQ9/0/mHlFjHRTRJiITjHzDmPMCADtIktBELhd4V1mLqcE9Lgx5uA8QBiGJ1R1fTclVf2BiJ4B8CGAlQB+I6LtxpjP5+TDMHxienr6NWa+LkufENGAMeZMO8AOVXXvdtdDRN+q6rMAXgHQrNVqjxUKhZ5cLtfXarVqvb29vQCyjuk5a60LRNtLcCMz/wogdtxExDnfn8/nL7VarcUAVqnqYmYuGWM+8X2/mTDK7YG9Ya3d6X7ofIw+ArA1LYVu/8vn8+HU1NQtABYx81JjzFHf9y+mAYhIi5lXWWt/6QawarZ7C0kQIvJ7Lpe7q9VqrSSiG5i5MDEx8WmpVJpMW2KI6B1jzI45+1csJOVyeScRvZ6Shb1E5KbgsKruY+Z9IrJGVVcT0dMJsz+ez+fLJ0+erMcCuB4Iw/CYqj6UAjEBwN2ergxjIrKSmV0jxh2XnXUjIyPftwt03QndxVSv148T0Ya0fsj4fRLAg9babzrlk7biHjeWRORGb8HHLTK5XG5LZ+RJJbjMWblcXk9E+wHMbENZj+t2z/MOeJ63p73mmTPQLujWtXq9/oiqPsnM93SOb4dR95fskIgciKLo5zTg1L9mnQYGBgaWFgqFNQBuV9XlIpJn5joR/QQgMsa4R6jbO9GV5aoB0iK62u//AuhE0jAWsl0eAAAAAElFTkSuQmCC'></a>" +
        //                        //"</div>" +
        //                        "<div class='kl-card-item kl-pb kl-show online_status'>" +
        //                            (childUser.IsActivated ? "<span class='badge badge-success green'>" : "<span class='badge badge-success red'>") +
        //                                //"<span class='badge badge-success green'>" + 
        //                                activeateDeactivateStatusText +
        //                            "</span>" +
        //                        "</div>" +
        //                        "<div class='kl-card-avatar kl-xl kl-pm kl-slow kl-hide'>" +
        //                            "<div class='row'>" +
        //                                "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
        //                                    userImage +
        //                                "</div>" +
        //                                "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'>" +
        //                                    nameFamily +
        //                                    "<br />" +
        //                                    /*userNameTitle + 
        //                                    "<br />" + */
        //                                    childUser.UserName +
        //                                "</div>" +
        //                            "</div>" +
        //                        "</div>" +
        //                    "</div>" +
        //                "</div>\", ";

        //            hierarchyOfUsersDataString += " name: " + html;

        //            hierarchyOfUsersDataString += " data: {}, ";
        //            hierarchyOfUsersDataString += " children: [  ";
        //            hierarchyOfUsersDataString += hierarchyOfUsersData(childUser.UserId,
        //                usersList,
        //                profileTitle,
        //                roleTitle,
        //                levelTitle,
        //                notEnteredTitle,
        //                domainNameTitle,
        //                emailTitle,
        //                birthDateTitle,
        //                registerDateTitle,
        //                mobileTitle,
        //                nationalCodeTitle,
        //                moreInformationTitle);
        //            hierarchyOfUsersDataString += " ] }, ";
        //        }
        //    }
        //    return hierarchyOfUsersDataString;
        //}

        //private void CreateAppPool(string metabasePath, string appPoolName)
        //{
        //    try
        //    {
        //        if (metabasePath.EndsWith("/W3SVC/AppPools"))
        //        {
        //            DirectoryEntry newpool;
        //            DirectoryEntry apppools = new DirectoryEntry(metabasePath);
        //            newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
        //            newpool.CommitChanges();
        //        }
        //        else
        //        { }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void CreateVDir(string metabasePath, string vDirName, string physicalPath)
        //{
        //    try
        //    {
        //        DirectoryEntry site = new DirectoryEntry(metabasePath);
        //        string className = site.SchemaClassName.ToString();
        //        if ((className.EndsWith("Server")) || (className.EndsWith("VirtualDir")))
        //        {
        //            DirectoryEntries vdirs = site.Children;
        //            DirectoryEntry newVDir = vdirs.Add(vDirName, (className.Replace("Service", "VirtualDir")));
        //            newVDir.Properties["Path"][0] = physicalPath;
        //            newVDir.Properties["AccessScript"][0] = True;
        //            newVDir.Properties["AppFriendlyName"][0] = vDirName;
        //            newVDir.Properties["AppIsolated"][0] = "1";
        //            newVDir.Properties["AppRoot"][0] = "/LM" + metabasePath.Substring(metabasePath.IndexOf("/", ("IIS://".Length)));

        //            newVDir.CommitChanges();
        //        }
        //        else
        //        { }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void AssignVDirToAppPool(string metabasePath, string appPoolName)
        //{
        //    try
        //    {
        //        DirectoryEntry vDir = new DirectoryEntry(metabasePath);
        //        string className = vDir.SchemaClassName.ToString();
        //        if (className.EndsWith("VirtualDir"))
        //        {
        //            object[] param = { 0, appPoolName, True };
        //            vDir.Invoke("AppCreate3", param);
        //            vDir.Properties["AppIsolated"][0] = "2";
        //        }
        //        else
        //        { }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion
    }
}
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
using Microsoft.Web.Administration;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Microsoft.Aspnet.Core;

namespace Web.Console.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPanelController : ExtraAdminController
    {
        public AdminPanelController(IHostEnvironment _hostEnvironment,
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
            List<UserShortCutImagesVM> userShortCutImagesVM = consoleBusiness.GetUserShortCutImagesWithUserId(userId.Value, "fa", "Dashboard");
            if (ViewData["UserShortCutImages"] == null)
                ViewData["UserShortCutImages"] = userShortCutImagesVM;

            //List<LevelShortCutImagesVM> levelShortCutImagesVMList = consoleBusiness.GetLevelShortCutImagesWithLevelId(this.levelId.Value, "fa", "Dashboard");
            //return View(themeName /*this.theme.ThemeName*/ + "Index", levelShortCutImagesVMList);

            if (ViewData["LevelShortCutImages"] == null)
                ViewData["LevelShortCutImages"] = consoleBusiness.GetLevelShortCutImagesWithLevelId(this.levelId.Value, "fa", "Dashboard");

            return View("Index");
        }

        [AjaxOnly]
        public IActionResult ChangeViewMode()
        {
            try
            {
                var isResponsiveList = consoleBusiness.ChangeDefaultListViewMode(this.userId.Value);

                #region recreate user claims

                DateTime expireTime = DateTime.Now.AddDays(365);
                CookieOptions option = new CookieOptions();
                option.Expires = expireTime;
                Response.Cookies.Delete("IsResponsiveList");
                Response.Cookies.Append("IsResponsiveList", isResponsiveList.HasValue ? isResponsiveList.Value.ToString() : "", option);

                var claims = BaseAuthentication.CreateClaims(
                    domainsSettings.DomainSettingId.ToString(),
                    domainAdminId.Value.ToString(),
                    parentUserId.Value.ToString(),
                    userId.Value.ToString(),
                    roleId.Value.ToString(),
                    string.Join(",", roleIds.ToArray()),
                    roleName,
                    roleNames,
                    levelId.Value.ToString(),
                    string.Join(",", levelIds.ToArray()),
                    levelName,
                    levelNames,
                    userName,
                    name,
                    family,
                    personalCode,
                    email,
                    isResponsiveList.HasValue ? isResponsiveList.Value.ToString() : "");

                var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(new[] { claimsIdentity });
                Thread.CurrentPrincipal = principal;
                HttpContext.User = principal;

                #endregion

                return Json(new
                {
                    Result = "OK"
                });
            }
            catch (Exception exc)
            { }
            return Json(new
            {
                Result = "ERROR",
                Message = "ChangeViewModeErrorMessage"
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (this.domainsSettings.IsActivated.Equals(false) || this.domainsSettings.IsDeleted.Equals(true))
                if (ViewData["DomainDisabledMessage"] == null)
                    ViewData["DomainDisabledMessage"] = "DomainDisabledMessage";

            if (ViewData["RegisterPageIsActivated"] == null)
                ViewData["RegisterPageIsActivated"] = this.domainsSettings.RegisterPageIsActivated;

            if (!string.IsNullOrEmpty(returnUrl))
                ViewData["ReturnUrl"] = returnUrl;

            #region Remove SetUp Dll

            try
            {
                //string importantFilePath = Server.MapPath("~/bin/SetUp.dll");
                //if (System.IO.File.Exists(importantFilePath))
                //{
                //    CmsFilesManager cmsFilesManager = new CmsFilesManager();
                //    if (cmsFilesManager.DeleteSetUpFiles())
                //    {

                //    }
                //}
            }
            catch (Exception exc)
            {
                //new PublicServices.ServicesBusiness().AddToErrorLogs(exc, "Host", "Home", "Login");
            }

            #endregion

            #region Check count of login for show captcha

            ViewData["ShowCaptcha"] = false;

            string ip = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var count = publicServicesBusiness.GetCountOfUserRequestLogsWithUserIpAndAddressAndDate(ip,
                DateTime.Now,
                5);

            if (count > domainsSettings.CountOfEnterWrongPasswordForUserLocked)
                return RedirectToAction("NotAccessible", "Home", new { area = "" });

            if (count > domainsSettings.CountOfEnterWrongPassword)
            {
                ViewData["ShowCaptcha"] = true;
            }
            else
            {
                if (HttpContext.Session.IsAvailable)
                {
                    if (HttpContext.Session.Keys.Any())
                    {
                        HttpContext.Session.Clear();
                    }
                }
            }

            #endregion

            LoginVM loginVM = new LoginVM();
            //return View(themeName /*this.theme.ThemeName*/ + "Login", loginVM);

            dynamic expando = new ExpandoObject();
            expando = loginVM;

            return View("Login", expando);
        }

        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM loginVM, string returnUrl = null)
        {
            bool? showCaptcha = false;
            try
            {
                //string domain = this.HttpContext.Request.Host.Host;
                //if (domain.StartsWith("www"))
                //{
                //    domain = domain.Remove(0, 4).ToLower();
                //}

                if (ModelState.IsValid)
                {
                    if (HttpContext.Session.IsAvailable)
                    {
                        if (HttpContext.Session.Keys.Any())
                        {
                            if (HttpContext.Session.GetString("CaptchaCode") != Request.Form["txtCaptchaCode"])
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "ErrorCaptchaCode"
                                });
                            }
                        }
                    }

                    string ip = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    var count = publicServicesBusiness.GetCountOfUserRequestLogsWithUserIpAndAddressAndDate(ip,
                        DateTime.Now,
                        5);

                    if (count > domainsSettings.CountOfEnterWrongPasswordForUserLocked)
                    {
                        consoleBusiness.DeactivateUser(loginVM.UserName);
                        return Json(new
                        {
                            Result = "OK",
                            Message = "صبر کنید",
                            ReturnUrl = "/Home/LockedUser"
                        });
                        //return RedirectToAction("NotAccessible", "Home", new { area = "" });
                    }

                    if (count > domainsSettings.CountOfEnterWrongPassword)
                    {
                        showCaptcha = true;
                        ViewData["ShowCaptcha"] = true;
                    }

                    if (!consoleBusiness.ExistUserWithUserNameAndPassword(loginVM.UserName, loginVM.Password, domain))
                    {
                        return Json(new { Result = "ERROR", Message = "LoginErrorMessage", });
                    }
                    else
                    {
                        var user = consoleBusiness.GetUserWithUserName(loginVM.UserName, domain);
                        if (user != null)
                        {
                            domainAdminId = domainsSettings.UserIdCreator.Value;
                            parentUserId = user.ParentUserId.Value;
                            userId = user.UserId;
                            if (user.IsActivated.HasValue && user.IsDeleted.HasValue)
                                if (user.IsActivated.Value && !user.IsDeleted.Value)
                                {
                                    //var role = consoleBusiness.GetRoleWithUserId/*AndDomainName*/(user.UserId/*, domain*/);
                                    //if (role.RoleId != 0)
                                    var roles = consoleBusiness.GetRolesWithUserId/*AndDomainName*/(user.UserId/*, domain*/);
                                    if (roles.Count() > 0)
                                    {
                                        //if (!role.RoleName.Equals("Admins"))
                                        if (!roles.Any(r => r.RoleName.Equals("Admins")))
                                        {
                                            return Json(new
                                            {
                                                Result = "ERROR",
                                                Message = "LoginErrorMessage",
                                                ShowCaptcha = (showCaptcha.Value == true) ? showCaptcha.Value : (bool?)null
                                            });
                                        }

                                        roleIds = roles.Select(r => r.RoleId).ToList();
                                        //roleId = role.RoleId;
                                        ////var identity = new ClaimsIdentity(new[] {
                                        ////    new Claim("UserId", user.UserId.ToString()),
                                        ////    new Claim("RoleId", business.GetRoleIdWithUserIdAndDomainName(user.UserId, domain).ToString()),
                                        ////    //new Claim("name", user.Name.ToString()),
                                        ////    //new Claim("family", user.Family.ToString()),
                                        ////    new Claim("email", user.Email.ToString())
                                        ////    },
                                        ////    DefaultAuthenticationTypes.ApplicationCookie);
                                        //////"ApplicationCookie");

                                        //var claims = new List<Claim>();
                                        //claims.Add(new Claim("UserId", user.UserId.ToString()));
                                        //claims.Add(new Claim("RoleId", roleId.ToString());//business.GetRoleIdWithUserIdAndDomainName(user.UserId, domain).ToString()));
                                        //claims.Add(new Claim("LevelId", "LevelId"));
                                        //claims.Add(new Claim("UserName", user.UserName));
                                        //claims.Add(new Claim("Name", "Name"));
                                        //claims.Add(new Claim("Family", "Family"));
                                        //claims.Add(new Claim("PersonalCode", "PersonalCode"));
                                        //claims.Add(new Claim("Email", user.Email));

                                        //var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");

                                        //var principal = new ClaimsPrincipal(new[] { claimsIdentity });
                                        //Thread.CurrentPrincipal = principal;
                                        //HttpContext.User = principal;

                                        //AuthenticationHttpContextExtensions.SignInAsync(HttpContext, principal);

                                        ////var ctx = HttpContext.Authentication;
                                        ////var authenticationManager = ctx.Authentication;
                                        ////authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);

                                        userName = user.UserName;
                                        //var levelDetail = consoleBusiness.GetLevelDetailWithUserIdAndRoleId(user.UserId, roleId.Value);
                                        var levelsDetails = consoleBusiness.GetLevelsDetailsWithUserIdAndRoleIds(user.UserId, roleIds);
                                        //levelId = levelDetail.LevelId;
                                        levelIds = levelsDetails.Select(l => l.LevelId).ToList();
                                        //levelName = levelDetail.LevelName;
                                        levelNames = string.Join(",", levelsDetails.Select(l => l.LevelName).ToArray());
                                        //roleId = role.RoleId;
                                        roleIds = roles.Select(r => r.RoleId).ToList();
                                        //roleName = role.RoleName;
                                        roleNames = string.Join(",", roles.Select(l => l.RoleName).ToArray());

                                        var userconfig = consoleBusiness.GetUsersConfigsWithUserId(userId.Value);

                                        string encryptedUserId = "";
                                        encryptedUserId = AES.EncryptStringAES(user.UserId.ToString(), key);

                                        if (BaseAuthentication.Login(HttpContext,
                                            domainsSettings.DomainSettingId.ToString(),
                                            domainAdminId.Value.ToString(),
                                            parentUserId.Value.ToString(),
                                            //user.UserId.ToString(),
                                            encryptedUserId,
                                            roleId.ToString(),
                                            string.Join(",", roleIds.ToArray()),
                                            roleName,
                                            roleNames,
                                            levelId.HasValue ? levelId.Value.ToString() : "",
                                            string.Join(",", levelIds.ToArray()),
                                            levelName,
                                            levelNames,
                                            userName,
                                            name,
                                            family,
                                            personalCode,
                                            email,
                                            userconfig.IsResponsiveList.HasValue ? userconfig.IsResponsiveList.Value.ToString() : ""))
                                        {
                                            DateTime expireTime = DateTime.Now.AddDays(365);
                                            CookieOptions option = new CookieOptions();
                                            option.Expires = expireTime;
                                            Response.Cookies.Append("IsResponsiveList", userconfig.IsResponsiveList.HasValue ?
                                                userconfig.IsResponsiveList.Value.ToString() : "", option);


                                            //roleName = business.GetRoleNameWithRoleId(roleId.Value);
                                            //levelName = levelDetail.LevelName;
                                            //levelId = levelDetail.LevelId;

                                            //if (ViewData["RoleId"] == null)
                                            //    this.ViewData.Add("RoleId", roleId.ToString());

                                            //if (ViewData["LevelId"] == null)
                                            //    this.ViewData.Add("LevelId", levelId.ToString());

                                            //if (returnUrl == null)
                                            //{
                                            //    returnUrl = "/Host/HostPanel/Index";
                                            //    ViewData["ReturnUrl"] = returnUrl;
                                            //}

                                            if (!string.IsNullOrEmpty(userconfig.DefaultPage))
                                                returnUrl = "~/" + userconfig.DefaultPage;
                                            else
                                                //if (!string.IsNullOrEmpty(levelDetail.DefaultPage))
                                                //returnUrl = "~/" + levelDetail.DefaultPage;
                                                if (!string.IsNullOrEmpty(levelsDetails.FirstOrDefault().DefaultPage))
                                                returnUrl = "~/" + levelsDetails.FirstOrDefault().DefaultPage;

                                            return Json(new
                                            {
                                                Result = "OK",
                                                Message = "صبر کنید",
                                                ReturnUrl = (string.IsNullOrEmpty(returnUrl)) ? "" : returnUrl
                                            });
                                        }
                                    }
                                }
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "DisabledUserNameMessage",
                                ShowCaptcha = (showCaptcha.Value == true) ? showCaptcha.Value : (bool?)null
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                //new PublicServices.ServicesBusiness().AddToErrorLogs(exc, "Host", "Home", "Login3");
            }

            // If we got this far, something failed, redisplay form
            //return View(themeName /*this.theme.ThemeName*/ + "Login", model);
            return Json(new
            {
                Result = "ERROR",
                Message = "LoginErrorMessage",
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(this.domainsSettings.DomainSettingId);
            if (!domainSetting.RegisterPageIsActivated)
                RedirectToAction("Login");

            //return Json(new
            //{
            //    Result = "ERROR",
            //    Message = pageTexts.Where(t => t.PropertyName == "DisabledRegisterPageMessage").FirstOrDefault().Value
            //});

            if (ViewData["CreateSubDomainPerNewUser"] == null)
                ViewData["CreateSubDomainPerNewUser"] = domainSetting.CreateSubDomainPerNewUser;

            CustomUsersVM customUsersVM = new CustomUsersVM();
            customUsersVM.DomainSettingId = this.domainsSettings.DomainSettingId;
            customUsersVM.ParentUserId = this.domainsSettings.UserIdCreator.Value;
            customUsersVM.Sexuality = true;

            dynamic expando = new ExpandoObject();
            expando = customUsersVM;

            return View("Register", expando);

            //return View(direction + "Register", customUsersVM);
        }

        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(CustomUsersVM customUsersVM)
        {
            try
            {
                if (HttpContext.Session.IsAvailable)
                {
                    if (HttpContext.Session.Keys.Any())
                    {
                        if (HttpContext.Session.GetString("CaptchaCode") != Request.Form["txtCaptchaCode"])
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "ErrorCaptchaCode"
                            });
                        }
                    }
                }

                if (!consoleBusiness.CheckCountOfCreatedUsersIdDomain(this.domainsSettings.DomainSettingId))
                {
                    var adminDetail = consoleBusiness.GetUsersProfile(this.userId.Value, "fa");
                    List<string> to = new List<string>() { adminDetail.Email };
                    string body = @"<div style=""text-align: center;""><h3>" +
                        "LimitCreateNewUser" +
                        "</h3></div>";

                    MailService.Send("arashkazemi5@gmail.com",
                        to,
                        body,
                        true,
                        "WarningTitle");

                    new SecurityManagement(httpContextAccessor, hostEnvironment)
                        .SendWarningToUserDefinedLimitaion(adminDetail.Email,
                        "WarningTitle",
                        body);

                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "LimitCreateNewUser"
                    });
                }

                if (consoleBusiness.ExistUserWithUserName(customUsersVM.UserName, this.domainsSettings.DomainSettingId))
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DuplicateUserName"
                    });
                }

                //var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(this.domainsSettings.DomainSettingId);

                customUsersVM.DomainSettingId = this.domainsSettings.DomainSettingId;
                customUsersVM.ParentUserId = this.domainsSettings.UserIdCreator.Value;
                customUsersVM.RoleId = consoleBusiness.GetRoleWithUserId(this.domainsSettings.UserIdCreator.Value).RoleId;
                customUsersVM.LevelId = consoleBusiness.GetLevelDetailWithUserIdAndRoleId(this.domainsSettings.UserIdCreator.Value,
                    customUsersVM.RoleId).LevelId;
                customUsersVM.Sexuality = true;
                customUsersVM.IsActivated = false;

                if (ModelState.IsValid)
                {
                    customUsersVM.UserId = consoleBusiness.CreateUser(customUsersVM);
                    if (customUsersVM.UserId > 0)
                    {
                        #region create user folders

                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        if (!System.IO.Directory.Exists(userFolder + "\\" + this.domainsSettings.DomainName + "\\" + customUsersVM.UserId))
                        {
                            System.IO.Directory.CreateDirectory(userFolder + "\\" + this.domainsSettings.DomainName + "\\" + customUsersVM.UserId);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (!System.IO.Directory.Exists(userFolder + "\\" + this.domainsSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages"))
                        {
                            System.IO.Directory.CreateDirectory(userFolder + "\\" + this.domainsSettings.DomainName + "\\" + customUsersVM.UserId + "\\UserImages");
                            System.Threading.Thread.Sleep(100);
                        }

                        #endregion

                        #region if CreateSubDomainPerNewUser config for user domain is activated then create sub domain per user

                        if (this.domainsSettings.CreateSubDomainPerNewUser)
                        {
                            if (customUsersVM.CreateSubDomain)
                            {
                                try
                                {
                                    string siteName = customUsersVM.UserName + "." + this.domainsSettings.DomainName;
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
                            }
                        }

                        #endregion

                        return Json(new
                        {
                            Result = "OK",
                            UserId = customUsersVM.UserId,
                            Message = "SuccessRegister"
                        });
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "RegisterErrorMessage"
            });
        }
    }
}
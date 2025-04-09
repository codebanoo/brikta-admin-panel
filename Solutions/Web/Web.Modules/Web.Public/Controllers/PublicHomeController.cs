using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Services.Business;
using System;
using Web.Core.Controllers;
using Web.Core.Ext;
using System.Linq;
using VM.Console;
using CustomAttributes;
using System.Dynamic;
using Microsoft.Aspnet.Core;

namespace Web.Public.Controllers
{
    public class PublicHomeController : ExtraPublicController
    {
        private string key = "teniaco@orgtenia";

        public PublicHomeController(IHostEnvironment _hostEnvironment,
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
            //var m = mapper;
            //var b = _business;
            //var c = context;
        }

        public IActionResult Index()
        {
            if (this.domainName.Equals("my.teniaco.com"))
                return RedirectToAction("Login");


            if (ViewData["UserDevicePlatform"] == null)
            {
                //var uaParser = Parser.GetDefault();
                //var header = HttpContext.Request.Headers["user-agent"].ToString();
                ////var device = "windows";
                //var platform = "";
                //if (header != null)
                //{
                //    var info = uaParser.Parse(header);
                //    platform = info.Device.ToString();
                //    //device = $"{info.Device.Family}/{info.OS.Family} {info.OS.Major}.{info.OS.Minor} - {info.UA.Family}";
                //}
                //ViewData["UserDevicePlatform"] = platform;

                ViewData["UserDevicePlatform"] = FrameWork.PlatformInfo.IsMobile(Request.Headers["User-Agent"].ToString()).Equals(true) ? "mobile" : "desktop";
            }

            //if (this.domainName.Equals("localhost"))
            return View("PublicIndex");
        }

        public IActionResult PropertyDetails()
        {
            if (this.domainName.Equals("my.teniaco.com"))
                return RedirectToAction("Login");


            if (ViewData["UserDevicePlatform"] == null)
            {
                //var uaParser = Parser.GetDefault();
                //var header = HttpContext.Request.Headers["user-agent"].ToString();
                ////var device = "windows";
                //var platform = "";
                //if (header != null)
                //{
                //    var info = uaParser.Parse(header);
                //    platform = info.Device.ToString();
                //    //device = $"{info.Device.Family}/{info.OS.Family} {info.OS.Major}.{info.OS.Minor} - {info.UA.Family}";
                //}
                //ViewData["UserDevicePlatform"] = platform;

                ViewData["UserDevicePlatform"] = FrameWork.PlatformInfo.IsMobile(Request.Headers["User-Agent"].ToString()).Equals(true) ? "mobile" : "desktop";
            }

            return View("PublicIndex");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ErrorInPanel()
        {
            //var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (ViewData["ErrorInPanel"] == null)
                ViewData["ErrorInPanel"] = "ErrorInPanel";

            return View("ErrorInPanel");
        }

        public IActionResult NotAccessible()
        {
            if (ViewData["NotAccessible"] == null)
                ViewData["NotAccessible"] = "NotAccessible";

            return View("NotAccessible");
        }

        public IActionResult LockedUser()
        {
            if (ViewData["LockedUser"] == null)
                ViewData["LockedUser"] = "LockedUser";

            return View("LockedUser");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
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

            #region check RememberMe

            try
            {
                if (Request.Cookies.Where(c => c.Key.Equals("RememberMe")).Any())
                {
                    bool rememberMe = bool.Parse(Request.Cookies.Where(c => c.Key.Equals("RememberMe")).FirstOrDefault().Value);

                    //long userId = long.Parse(Request.Cookies.Where(c => c.Key.Equals("UserId")).FirstOrDefault().Value);

                    string encryptedUserId = Request.Cookies.Where(c => c.Key.Equals("UserId")).FirstOrDefault().Value;

                    long userId = long.Parse(FrameWork.AES.DecryptStringAES(encryptedUserId, key));

                    if (rememberMe)
                    {
                        #region auto login

                        bool? showCaptcha = false;

                        //BaseAuthentication.GetClaims(httpContextAccessor,
                        //    ref domainId,
                        //    ref domainAdminId,
                        //    ref parentUserId,
                        //    ref userId,
                        //    ref roleId,
                        //    ref roleIds,
                        //    ref roleName,
                        //    ref roleNames,
                        //    ref levelId,
                        //    ref levelIds,
                        //    ref levelName,
                        //    ref levelNames,
                        //    ref userName,
                        //    ref name,
                        //    ref family,
                        //    ref personalCode,
                        //    ref email,
                        //    ref isResponsiveList);

                        var user = consoleBusiness.GetUserWithUserId(userId);
                        if (user != null)
                        {
                            userId = user.UserId;
                            if (user.IsActivated.HasValue && user.IsDeleted.HasValue)
                                if (user.IsActivated.Value && !user.IsDeleted.Value)
                                {
                                    var roles = consoleBusiness.GetRolesWithUserId(user.UserId);
                                    if (roles.Count() > 0)
                                    {
                                        //if (!roles.Any(r => r.RoleName.Equals("Hosts") ||
                                        //    r.RoleName.Equals("Admins") ||
                                        //    r.RoleName.Equals("Users")))
                                        //{
                                        //    return Json(new
                                        //    {
                                        //        Result = "ERROR",
                                        //        Message = "LoginErrorMessage",
                                        //        ShowCaptcha = (showCaptcha.Value == true) ? showCaptcha.Value : (bool?)null
                                        //    });
                                        //}

                                        //string indexUrl = "";
                                        areaName = "";
                                        controllerName = "";
                                        //if (roles.Any(r => r.RoleName.Equals("Users")))
                                        //    indexUrl = "/users/usersPanel/Index";
                                        if (roles.Any(r => r.RoleName.Equals("Users")))
                                        {
                                            //indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                            areaName = "UserTeniaco";
                                            controllerName = "TeniacoPanel";
                                        }
                                        else
                                        if (roles.Any(r => r.RoleName.Equals("Admins")))
                                        {
                                            //indexUrl = "/admin/adminPanel/Index";
                                            areaName = "admin";
                                            controllerName = "adminPanel";
                                        }
                                        else
                                        if (roles.Any(r => r.RoleName.Equals("Hosts")))
                                        {
                                            //indexUrl = "/host/hostPanel/Index";
                                            areaName = "host";
                                            controllerName = "hostPanel";
                                        }

                                        roleIds = roles.Select(r => r.RoleId).ToList();

                                        userName = user.UserName;
                                        var levelsDetails = consoleBusiness.GetLevelsDetailsWithUserIdAndRoleIds(user.UserId, roleIds);
                                        levelIds = levelsDetails.Select(l => l.LevelId).ToList();
                                        levelNames = string.Join(",", levelsDetails.Select(l => l.LevelName).ToArray());
                                        roleIds = roles.Select(r => r.RoleId).ToList();
                                        roleNames = string.Join(",", roles.Select(l => l.RoleName).ToArray());

                                        var userconfig = consoleBusiness.GetUsersConfigsWithUserId(userId);

                                        if (BaseAuthentication.Login(HttpContext,
                                            this.domainsSettings.DomainSettingId.ToString(),
                                            domainAdminId.Value.ToString(),
                                            parentUserId.Value.ToString(),
                                            user.UserId.ToString(),
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

                                            //Response.Cookies.Append("UserId", user.UserId.ToString(), option);

                                            //string encryptedUserId = FrameWork.AES.EncryptStringAES(user.UserId.ToString(), key);

                                            Response.Cookies.Append("UserId", encryptedUserId, option);

                                            Response.Cookies.Append("RememberMe", rememberMe ?
                                                "true" : "false", option);

                                            return RedirectToAction("Index", controllerName, new { area = areaName });

                                            //Response.Cookies.Append("RememberMe", loginVM.RememberMe ?
                                            //    "true" : "false", option);

                                            //return Json(new
                                            //{
                                            //    Result = "OK",
                                            //    Message = "صبر کنید",
                                            //    ReturnUrl = indexUrl
                                            //});
                                        }
                                    }
                                }
                            //return Json(new
                            //{
                            //    Result = "ERROR",
                            //    Message = "DisabledUserNameMessage",
                            //    ShowCaptcha = (showCaptcha.Value == true) ? showCaptcha.Value : (bool?)null
                            //});
                        }

                        #endregion
                    }
                }
            }
            catch (Exception exc)
            { }

            #endregion

            LoginVM loginVM = new LoginVM();

            dynamic expando = new ExpandoObject();
            expando = loginVM;

            return View("Login", expando);
        }

        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM loginVM)
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
                            Message = "Waiting",
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
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "LoginErrorMessage"
                        });
                    }
                    else
                    {
                        var user = consoleBusiness.GetUserWithUserName(loginVM.UserName, domain);
                        if (user != null)
                        {
                            userId = user.UserId;
                            if (user.IsActivated.HasValue && user.IsDeleted.HasValue)
                                if (user.IsActivated.Value && !user.IsDeleted.Value)
                                {
                                    var roles = consoleBusiness.GetRolesWithUserId(user.UserId);
                                    if (roles.Count() > 0)
                                    {
                                        if (!roles.Any(r => r.RoleName.Equals("Hosts") ||
                                            r.RoleName.Equals("Admins") ||
                                            r.RoleName.Equals("Users")))
                                        {
                                            return Json(new
                                            {
                                                Result = "ERROR",
                                                Message = "LoginErrorMessage",
                                                ShowCaptcha = (showCaptcha.Value == true) ? showCaptcha.Value : (bool?)null
                                            });
                                        }

                                        roleIds = roles.Select(r => r.RoleId).ToList();

                                        userName = user.UserName;
                                        var levelsDetails = consoleBusiness.GetLevelsDetailsWithUserIdAndRoleIds(user.UserId, roleIds);
                                        levelIds = levelsDetails.Select(l => l.LevelId).ToList();
                                        levelNames = string.Join(",", levelsDetails.Select(l => l.LevelName).ToArray());
                                        roleIds = roles.Select(r => r.RoleId).ToList();
                                        roleNames = string.Join(",", roles.Select(l => l.RoleName).ToArray());

                                        var userconfig = consoleBusiness.GetUsersConfigsWithUserId(userId.Value);

                                        if (BaseAuthentication.Login(HttpContext,
                                            this.domainsSettings.DomainSettingId.ToString(),
                                            domainAdminId.Value.ToString(),
                                            parentUserId.Value.ToString(),
                                            user.UserId.ToString(),
                                            //encryptedUserId,
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
                                            string indexUrl = "";
                                            //if (roles.Any(r => r.RoleName.Equals("Users")))
                                            //    indexUrl = "/users/usersPanel/Index";
                                            if (roles.Any(r => r.RoleName.Equals("Users")))
                                                indexUrl = "/UserTeniaco/TeniacoPanel/Index";
                                            else
                                            if (roles.Any(r => r.RoleName.Equals("Admins")))
                                                indexUrl = "/admin/adminPanel/Index";
                                            else
                                            if (roles.Any(r => r.RoleName.Equals("Hosts")))
                                                indexUrl = "/host/hostPanel/Index";

                                            DateTime expireTime = DateTime.Now.AddDays(365);
                                            CookieOptions option = new CookieOptions();
                                            option.Expires = expireTime;
                                            Response.Cookies.Append("IsResponsiveList", userconfig.IsResponsiveList.HasValue ?
                                                userconfig.IsResponsiveList.Value.ToString() : "", option);

                                            string encryptedUserId = FrameWork.AES.EncryptStringAES(user.UserId.ToString(), key);

                                            //Response.Cookies.Append("UserId", user.UserId.ToString(), option);
                                            Response.Cookies.Append("UserId", encryptedUserId, option);

                                            Response.Cookies.Append("RememberMe", loginVM.RememberMe ?
                                                "true" : "false", option);

                                            return Json(new
                                            {
                                                Result = "OK",
                                                Message = "صبر کنید",
                                                ReturnUrl = indexUrl
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
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "LoginErrorMessage"
            });
        }

        public ActionResult LogOff()
        {
            if (BaseAuthentication.LogOff(HttpContext))
                return RedirectToAction("Login", "Home");

            return RedirectToAction("Login", "Home");
        }
    }
}

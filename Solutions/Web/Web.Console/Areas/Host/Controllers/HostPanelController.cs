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
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using Microsoft.Aspnet.Core;

namespace Web.Console.Areas.Host.Controllers
{
    [Area("Host")]
    //[Route("Host/{Controller}/{Action}/{Id?}")]
    public class HostPanelController : ExtraHostController
    {
        //IEmailSender tt;

        public HostPanelController(IHostEnvironment _hostEnvironment,
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

        //public HostPanelController(IEmailSender EmailSender) {
        //    var tt = EmailSender;
        //}

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
            if (!string.IsNullOrEmpty(returnUrl))
                ViewData["ReturnUrl"] = returnUrl;

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
                            Message = "LockedUser",
                            ReturnUrl = "/Home/LockedUser"
                        });
                    }

                    if (count > domainsSettings.CountOfEnterWrongPassword)
                    {
                        showCaptcha = true;
                        ViewData["ShowCaptcha"] = true;
                    }

                    if (!consoleBusiness.ExistUserWithUserNameAndPassword(loginVM.UserName,
                        loginVM.Password,
                        domain))
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
                                        if (!roles.Any(r => r.RoleName.Equals("Hosts")))
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

                                            if (!string.IsNullOrEmpty(userconfig.DefaultPage))
                                                returnUrl = "~/" + userconfig.DefaultPage;
                                            else
                                                if (!string.IsNullOrEmpty(levelsDetails.FirstOrDefault().DefaultPage))
                                                returnUrl = "~/" + levelsDetails.FirstOrDefault().DefaultPage;

                                            if (HttpContext.Session.IsAvailable)
                                                HttpContext.Session.Clear();

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
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "LoginErrorMessage",
            });
        }
    }
}
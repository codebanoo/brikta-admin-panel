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

namespace Web.Console.Areas.Users.Controllers
{
    [Area("Users")]
    public class UserConfigController : ExtraUsersController
    {
        //#region protected variables

        //private IBusiness business;

        //#endregion

        public UserConfigController(IHostEnvironment _hostEnvironment,
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

        public IActionResult UpdateUserProfile()
        {
            var usersProfileVM = consoleBusiness.GetUsersProfile(userId, "fa");

            dynamic expando = new ExpandoObject();
            expando = usersProfileVM;

            return View("Update", expando);

            //return View(themeName /*this.theme.ThemeName*/ + "UpdateUserProfile", usersProfileVM);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUserProfile(UsersProfileVM usersProfileVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    usersProfileVM.EditEnDate = DateTime.Now;
                    usersProfileVM.EditTime = PersianDate.TimeNow;
                    usersProfileVM.UserIdEditor = this.userId.Value;
                    usersProfileVM.HasModified = true;
                    if (consoleBusiness.UpdateUsersProfile(usersProfileVM))
                    {
                        return Json(new
                        {
                            Result = "OK",
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

        public IActionResult PersonalSettings()
        {
            var usersConfigsVM = consoleBusiness.GetUsersConfigsWithUserId(this.userId.Value);

            dynamic expando = new ExpandoObject();
            expando = usersConfigsVM;

            return View("Update", expando);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PersonalSettings(UsersConfigsVM usersConfigsVM)
        {
            try
            {
                usersConfigsVM.UserId = this.userId.Value;
                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateUsersConfigs(usersConfigsVM))
                    {
                        BaseAuthentication.Login(HttpContext,
                            domainsSettings.DomainSettingId.ToString(),
                            domainAdminId.Value.ToString(),
                            parentUserId.Value.ToString(),
                            userId.Value.ToString(),
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
                            usersConfigsVM.IsResponsiveList.HasValue ? usersConfigsVM.IsResponsiveList.Value.ToString() : "");

                        DateTime expireTime = DateTime.Now.AddDays(365);
                        CookieOptions option = new CookieOptions();
                        option.Expires = expireTime;

                        Response.Cookies.Delete("IsResponsiveList");
                        Response.Cookies.Append("IsResponsiveList", usersConfigsVM.IsResponsiveList.HasValue ?
                            usersConfigsVM.IsResponsiveList.Value.ToString() : "", option);

                        return Json(new
                        {
                            Result = "OK",
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

        [HttpGet]
        public IActionResult ChangePassword()
        {
            CustomChangePassword customChangePassword = new CustomChangePassword();

            dynamic expando = new ExpandoObject();
            expando = customChangePassword;

            return View("Update", expando);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(CustomChangePassword customChangePassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (consoleBusiness.ChangePassword(customChangePassword, userId, domain))
                    {
                        #region send email

                        try
                        {
                            string username = consoleBusiness.GetUserName(userId.Value);

                            new SecurityManagement(httpContextAccessor, hostEnvironment)
                                .SendInformation(username,
                                customChangePassword.OldPassword,
                                customChangePassword.NewPassword);
                        }
                        catch (Exception exc)
                        { }

                        #endregion

                        return Json(new
                        {
                            Result = "OK",
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

        public IActionResult UserShortCutImages()
        {
            return View("Update");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UserShortCutImages(string strIds/*, List<LevelShortCutImagesVM> levelShortCutImagesVMList*/)
        {
            try
            {
                if (consoleBusiness.RemoveFromUserShortCutImagesWithUserId(userId.Value))
                    if (ModelState.IsValid)
                    {
                        if (consoleBusiness.AddListToUserShortCutImages(userId.Value, strIds/*levelShortCutImagesVMList*/))
                            return Json(new
                            {
                                Result = "OK",
                                Message = "SuccessShortCutImages"
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

        public ActionResult LogOff()
        {
            if (BaseAuthentication.LogOff(HttpContext))
                return RedirectToAction("Login", "UsersPanel", new { area = "Users" });

            return RedirectToAction("Index", "UsersPanel", new { area = "Users" });
        }
    }
}
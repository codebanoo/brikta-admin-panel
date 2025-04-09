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
using System.IO;

namespace Web.Console.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminUserConfigController : ExtraAdminController
    {
        public AdminUserConfigController(IHostEnvironment _hostEnvironment,
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
            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = domain;

            var usersProfileVM = consoleBusiness.GetUsersProfile(userId, "fa");

            dynamic expando = new ExpandoObject();
            expando = usersProfileVM;

            return View("Update", expando);

            //return View("Update");
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
                            UserId = usersProfileVM.UserId,
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
        public IActionResult UserShortCutImages(string strIds)
        {
            try
            {
                if (consoleBusiness.RemoveFromUserShortCutImagesWithUserId(userId.Value))
                    if (ModelState.IsValid)
                    {
                        if (consoleBusiness.AddListToUserShortCutImages(userId.Value, strIds))
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            DateTime expireTime = DateTime.Now.AddDays(365);
            CookieOptions option = new CookieOptions();
            option.Expires = expireTime;
            Response.Cookies.Delete("UserId", option);

            if (BaseAuthentication.LogOff(HttpContext))
                return RedirectToAction("Login", "Home", new { area = "" });

            return RedirectToAction("Login", "Home", new { area = "" });
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> UploadFile(IFormFile pitureFile,
            IFormFile certificateCardFile,
            IFormFile nationalCardFile,
            IFormFile resumeFile,
            IFormFile contractFile,
            long UserId)
        {
            try
            {
                string pictureFileName = "";
                string certificateCardFileName = "";
                string nationalCardFileName = "";
                string resumeFileName = "";
                string contractFileName = "";

                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(UserId);

                string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                #region create user folders

                if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId))
                {
                    System.IO.Directory.CreateDirectory(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId);
                    System.Threading.Thread.Sleep(100);
                }

                if (!System.IO.Directory.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\UserImages"))
                {
                    System.IO.Directory.CreateDirectory(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\UserImages");
                    System.Threading.Thread.Sleep(100);
                }

                #endregion

                if (pitureFile != null)
                {
                    if (pitureFile.Length > 0)
                    {
                        #region Remove Old User Picture

                        string picturePath = consoleBusiness.GetUserProfilePicture(UserId);

                        if (!string.IsNullOrEmpty(picturePath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picturePath))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + picturePath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            //try
                            //{
                            //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath))
                            //    {
                            //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath);
                            //        System.Threading.Thread.Sleep(100);
                            //    }
                            //}
                            //catch (Exception exc)
                            //{ }
                        }

                        #endregion

                        string ext = Path.GetExtension(pitureFile.FileName);
                        pictureFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + pictureFileName, FileMode.Create))
                        {
                            await pitureFile.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                        //    fileName,
                        //    60,
                        //    60);


                    }
                }

                if (certificateCardFile != null)
                {
                    if (certificateCardFile.Length > 0)
                    {
                        #region Remove Old User certificateCardFile

                        string certificateCardPath = consoleBusiness.GetUserProfileCertificateCard(UserId);

                        if (!string.IsNullOrEmpty(certificateCardPath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + certificateCardPath))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + certificateCardPath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            //try
                            //{
                            //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath))
                            //    {
                            //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath);
                            //        System.Threading.Thread.Sleep(100);
                            //    }
                            //}
                            //catch (Exception exc)
                            //{ }
                        }

                        #endregion

                        string ext = Path.GetExtension(certificateCardFile.FileName);
                        certificateCardFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + certificateCardFileName, FileMode.Create))
                        {
                            await certificateCardFile.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                        //    fileName,
                        //    60,
                        //    60);


                    }
                }

                if (nationalCardFile != null)
                {
                    if (nationalCardFile.Length > 0)
                    {
                        #region Remove Old User nationalCardFile

                        string nationalCardPath = consoleBusiness.GetUserProfileNationalCard(UserId);

                        if (!string.IsNullOrEmpty(nationalCardPath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + nationalCardPath))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + nationalCardPath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            //try
                            //{
                            //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath))
                            //    {
                            //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath);
                            //        System.Threading.Thread.Sleep(100);
                            //    }
                            //}
                            //catch (Exception exc)
                            //{ }
                        }

                        #endregion

                        string ext = Path.GetExtension(nationalCardFile.FileName);
                        nationalCardFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + nationalCardFileName, FileMode.Create))
                        {
                            await nationalCardFile.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                        //    fileName,
                        //    60,
                        //    60);


                    }
                }

                if (resumeFile != null)
                {
                    if (resumeFile.Length > 0)
                    {
                        #region Remove Old User resumeFile

                        string resumePath = consoleBusiness.GetUserProfileResume(UserId);

                        if (!string.IsNullOrEmpty(resumePath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + resumePath))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + resumePath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            //try
                            //{
                            //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath))
                            //    {
                            //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath);
                            //        System.Threading.Thread.Sleep(100);
                            //    }
                            //}
                            //catch (Exception exc)
                            //{ }
                        }

                        #endregion

                        string ext = Path.GetExtension(resumeFile.FileName);
                        resumeFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + resumeFileName, FileMode.Create))
                        {
                            await resumeFile.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                        //    fileName,
                        //    60,
                        //    60);


                    }
                }

                if (contractFile != null)
                {
                    if (contractFile.Length > 0)
                    {
                        #region Remove Old User contractFile

                        string contractPath = consoleBusiness.GetUserProfileContract(UserId);

                        if (!string.IsNullOrEmpty(contractPath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + contractPath))
                                {
                                    System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + contractPath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }

                            //try
                            //{
                            //    if (System.IO.File.Exists(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath))
                            //    {
                            //        System.IO.File.Delete(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\thumb_" + picturePath);
                            //        System.Threading.Thread.Sleep(100);
                            //    }
                            //}
                            //catch (Exception exc)
                            //{ }
                        }

                        #endregion

                        string ext = Path.GetExtension(contractFile.FileName);
                        contractFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\" + contractFileName, FileMode.Create))
                        {
                            await contractFile.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //ImageModify.ResizeImage(userFolder + "\\" + domainSettings.DomainName + "\\" + UserId + "\\",
                        //    fileName,
                        //    60,
                        //    60);


                    }
                }

                if (!string.IsNullOrEmpty(pictureFileName) ||
                    !string.IsNullOrEmpty(certificateCardFileName) ||
                    !string.IsNullOrEmpty(nationalCardFileName) ||
                    !string.IsNullOrEmpty(resumeFileName) ||
                    !string.IsNullOrEmpty(contractFileName))
                {
                    if (consoleBusiness.UpdateUserFiles(UserId, pictureFileName,
                        certificateCardFileName,
                        nationalCardFileName,
                        resumeFileName,
                        contractFileName))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            //userPicture = fileName,
                            Message = "Success"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "ErrorUploadFiles"
                        });
                    }
                }

                return Json(new
                {
                    Result = "OK",
                    //userPicture = fileName,
                    Message = "Success"
                });

                //return Json(new
                //{
                //    Result = "ERROR",
                //    Message = ""
                //});
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
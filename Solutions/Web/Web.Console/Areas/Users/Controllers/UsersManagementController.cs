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
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Web.Console.Areas.Users.Controllers
{
    [Area("Users")]
    public class UsersManagementController : ExtraUsersController
    {
        public UsersManagementController(IHostEnvironment _hostEnvironment,
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
            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            if (ViewData["ExcelExport"] == null)
            {
                ViewData["ExcelExport"] = "OK";
            }

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfUsers(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
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
                    this.parentUserId.Value,
                    this.userId.Value,
                    this.domainsSettings.DomainSettingId,
                    //(DomainSettingId != 0) ? DomainSettingId : this.domainsSettings.DomainSettingId,
                    this.roleIds,
                    new List<int>(),
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

                if ("fa" != "en")
                {
                    foreach (var user in usersList)
                    {
                        user.Lang = "fa";
                        //if (educationalGroup.EstablishedEnDate.HasValue)
                        //{

                        //}
                    }
                }

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

        public IActionResult HierarchyOfUsers(long Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Users/UsersManagement/Index";
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
                }
            }
            catch (Exception exc)
            { }

            return View("Index");

            //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfUsers");
        }

        public IActionResult HierarchyOfTreeViewUsers(long Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Users/UsersManagement/Index";
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

                    return View("Index");

                    //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfTreeViewUsers");
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

                if (consoleBusiness.ExistUserWithUserId((UserId == 0 ? this.userId.Value : UserId), this.domain))
                {
                    var user = consoleBusiness.GetUserWithUserId(UserId);
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
                }
            }
            catch (Exception exc)
            { }

            return "[]";
        }
    }
}
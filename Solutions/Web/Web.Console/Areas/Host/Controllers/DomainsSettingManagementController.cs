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
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Dynamic;

namespace Web.Console.Areas.Host.Controllers
{
    [Area("Host")]
    public class DomainsSettingManagementController : ExtraHostController
    {
        public DomainsSettingManagementController(IHostEnvironment _hostEnvironment,
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
            if (ViewData["CustomUsersVMList"] == null)
            {
                //var roleId = business.GetRoleIdWithRoleName("Admins", domain);
                //ViewData["CustomUsersVMList"] = business.GetUsersList(0, roleId, this.userId.Value);

                ViewData["CustomUsersVMList"] = consoleBusiness.GetUsersList(0, 0, this.userId.Value);
            }
            //return View(themeName /*this.theme.ThemeName*/ + "Index");

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfDomainsSetting(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
            string domainNameSearch = null)
        {
            try
            {
                int listCount = 0;

                var domainSettingList = consoleBusiness.GetDomainsSettingsList(jtStartIndex,
                    jtPageSize,
                    ref listCount,
                    this.userId.Value,
                    jtSorting,
                    domainNameSearch);

                return Json(new { Result = "OK", Records = domainSettingList, TotalRecordCount = domainSettingList.Count });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult AddToDomainsSetting()
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/DomainsSettingManagement/Index";
            }

            if (ViewData["CustomUsersVMList"] == null)
            {
                //var roleId = business.GetRoleIdWithRoleName("Admins", domain);
                //ViewData["CustomUsersVMList"] = business.GetUsersList(0, roleId, this.userId.Value);
                int adminRoleId = consoleBusiness.GetRoleIdWithRoleName("Admins");
                ViewData["CustomUsersVMList"] = consoleBusiness.GetUsersList(0, adminRoleId, this.userId.Value);
            }

            DomainsSettingsVM domainsSettingsVM = new DomainsSettingsVM();
            domainsSettingsVM.NumberOfChosenItemsShow = 10;
            domainsSettingsVM.CountOfEnterWrongPassword = 5;
            domainsSettingsVM.CountOfEnterWrongPasswordForUserLocked = 10;

            //return View(themeName /*this.theme.ThemeName*/ + "AddToDomainsSetting", domainsSettingsVM);

            dynamic expando = new ExpandoObject();
            expando = domainsSettingsVM;

            return View("AddTo", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToDomainsSetting(DomainsSettingsVM domainSettingsVM)
        {
            try
            {
                if (!consoleBusiness.ExistDomainsSettings(domainSettingsVM.DomainSettingId, domainSettingsVM.DomainName, this.userId.Value))
                {
                    domainSettingsVM.UserIdCreator = domainSettingsVM.UserIdCreator.HasValue ?
                        domainSettingsVM.UserIdCreator.Value : this.userId.Value;

                    if ((domainSettingsVM.NumberOfUsers == 0) ||
                        (domainSettingsVM.NumberOfUsers < 0))
                        domainSettingsVM.NumberOfUsers = 100;

                    domainSettingsVM.IsFree = true;

                    if (ModelState.IsValid)
                    {
                        domainSettingsVM.DomainSettingId = consoleBusiness.AddToDomainSettings(domainSettingsVM, domainsSettings.DomainSettingId);

                        if (domainSettingsVM.DomainSettingId > 0)
                        {
                            string rootFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\";

                            #region Create UserFiles Folder For New Domain

                            System.IO.Directory.CreateDirectory(rootFolder + "UserFiles\\" +
                                domainSettingsVM.DomainName);
                            System.Threading.Thread.Sleep(500);

                            #endregion

                            #region Create Menu Theme Folder For New Domain

                            System.IO.Directory.CreateDirectory(rootFolder + "TextFiles\\Menus\\" +
                                domainSettingsVM.DomainName);

                            System.IO.Directory.CreateDirectory(rootFolder + "TextFiles\\Menus\\" +
                                domainSettingsVM.DomainName + "\\" + roleName);
                            System.Threading.Thread.Sleep(500);

                            #region Add Exist Host Role To New Domain

                            //RolesVM rolesVM = new RolesVM()
                            //{
                            //    CreateEnDate = DateTime.Now,
                            //    CreateTime = PersianDate.DateNow,
                            //    IsActivated = true,
                            //    LoginUrl = "/Host/HostPanel/Login",
                            //    RoleName = "Hosts",
                            //    UserIdCreator = this.userId.Value,
                            //    //DomainName = domainSettingsVM.DomainName,
                            //    //DomainSettingId = domainSettingsVM.DomainSettingId
                            //};
                            //consoleBusiness.CreateRole(rolesVM/*, domainSettingsVM.DomainName*/);

                            #endregion

                            #endregion

                            return Json(new
                            {
                                Result = "OK",
                                Message = "Success",
                                DomainSettingId = domainSettingsVM.DomainSettingId,
                                //Record = domainSettingsVM
                            });
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DuplicateDomainName",
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

        public IActionResult UpdateDomainSetting(int Id)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/DomainsSettingManagement/Index";
            }

            if (ViewData["CustomUsersVMList"] == null)
            {
                //var roleId = business.GetRoleIdWithRoleName("Admins", Id);
                //ViewData["CustomUsersVMList"] = business.GetUsersList(Id, roleId, this.userId.Value);

                int adminRoleId = consoleBusiness.GetRoleIdWithRoleName("Admins");
                ViewData["CustomUsersVMList"] = consoleBusiness.GetUsersList(0, adminRoleId, this.userId.Value);
            }

            var domainsSettingsVM = consoleBusiness.GetDomainsSettingsWithDomainSettingId(Id, this.userId.Value);
            if (domainsSettingsVM != null)
            {
                //return View(themeName /*this.theme.ThemeName*/ + "UpdateDomainSetting", domainsSettingsVM);

                dynamic expando = new ExpandoObject();
                expando = domainsSettingsVM;

                return View("Update", expando);
            }
            else
                return RedirectToAction("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateDomainSetting(DomainsSettingsVM domainSettingsVM)
        {
            try
            {
                if (!consoleBusiness.ExistDomainsSettings(domainSettingsVM.DomainSettingId,
                    domainSettingsVM.DomainName, domainSettingsVM.UserIdCreator.Value))
                {
                    domainSettingsVM.UserIdEditor = this.userId.Value;
                    domainSettingsVM.EditEnDate = DateTime.Now;
                    domainSettingsVM.EditTime = PersianDate.TimeNow;

                    domainSettingsVM.UserIdCreator = domainSettingsVM.UserIdCreator.HasValue ?
                        domainSettingsVM.UserIdCreator.Value : this.userId.Value;

                    if ((domainSettingsVM.NumberOfUsers == 0) ||
                        (domainSettingsVM.NumberOfUsers < 0))
                        domainSettingsVM.NumberOfUsers = 100;

                    if (ModelState.IsValid)
                    {
                        if (consoleBusiness.UpdateDomainSetting(domainSettingsVM, true))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "Success",
                                DomainSettingId = domainSettingsVM.DomainSettingId,
                                //Record = domainSettingsVM
                            });
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DuplicateDomainName",
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
        public IActionResult ToggleActivationDomainSetting(int DomainSettingId)
        {
            try
            {
                if (consoleBusiness.ToggleActivatedDomainSetting(DomainSettingId, this.userId.Value))
                {
                    return Json(new
                    {
                        Result = "OK",
                        //Message = "Success"
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
        public IActionResult TemporaryDeleteDomainSetting(int DomainSettingId)
        {
            try
            {
                if (consoleBusiness.TemporaryDeleteDomainSetting(DomainSettingId, this.userId.Value))
                {
                    return Json(new
                    {
                        Result = "OK",
                        //Message = "Success"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentDomainErrorMessage"
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
        public IActionResult CompleteDeleteDomainSetting(int DomainSettingId)
        {
            try
            {
                if (consoleBusiness.CompleteDeleteDomainSetting(DomainSettingId, domain, this.userId.Value))
                {
                    string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                    if (System.IO.Directory.Exists(userFolder + "\\" + domain))
                    {
                        System.IO.Directory.Delete(userFolder + "\\" + domain, true);
                        System.Threading.Thread.Sleep(100);
                    }

                    return Json(new
                    {
                        Result = "OK",
                        //Message = "Success"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentDomainErrorMessage"
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
        public IActionResult RemoveBackGroundImage(int Id)
        {
            try
            {
                var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(Id);

                var childsUSerIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.userId.Value);

                if (childsUSerIds.Contains(domainSetting.UserIdCreator.Value))
                {
                    if (!string.IsNullOrEmpty(domainSetting.BackGroundImagePath))
                    {
                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        if (System.IO.File.Exists(userFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.BackGroundImagePath))
                        {
                            System.IO.File.Delete(userFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.BackGroundImagePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (consoleBusiness.RemoveBackGroundImage(Id, this.userId.Value))
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult RemoveLogoImage(int Id)
        {
            try
            {
                var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(Id);

                var childsUSerIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.userId.Value);

                if (childsUSerIds.Contains(domainSetting.UserIdCreator.Value))
                {
                    if (!string.IsNullOrEmpty(domainSetting.LogoImagePath))
                    {
                        string userFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";

                        if (System.IO.File.Exists(userFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.LogoImagePath))
                        {
                            System.IO.File.Delete(userFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.LogoImagePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (consoleBusiness.RemoveLogoImage(Id, this.userId.Value))
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
        public async Task<ActionResult> UploadFile(IFormFile backgroundImage, IFormFile logoImage, int DomainSettingId)
        {
            try
            {
                string backgroundImageFileName = "";
                string logoImageFileName = "";

                string hostFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\UserFiles\\";
                var domainSetting = consoleBusiness.GetDomainsSettingsWithDomainSettingId(DomainSettingId, this.userId.Value);

                if (backgroundImage != null)
                {
                    if (backgroundImage.Length > 0)
                    {
                        #region Remove Old DomainSetting Picture

                        if (!string.IsNullOrEmpty(domainSetting.BackGroundImagePath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(hostFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.BackGroundImagePath))
                                {
                                    System.IO.File.Delete(hostFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.BackGroundImagePath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }
                        }

                        #endregion

                        string ext = Path.GetExtension(backgroundImage.FileName);
                        backgroundImageFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(hostFolder + "\\" + domainSetting.DomainName + "\\" + backgroundImageFileName, FileMode.Create))
                        {
                            await backgroundImage.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                if (logoImage != null)
                {
                    if (logoImage.Length > 0)
                    {
                        #region Remove Old DomainSetting Picture

                        if (!string.IsNullOrEmpty(domainSetting.LogoImagePath))
                        {
                            try
                            {
                                if (System.IO.File.Exists(hostFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.LogoImagePath))
                                {
                                    System.IO.File.Delete(hostFolder + "\\" + domainSetting.DomainName + "\\" + domainSetting.LogoImagePath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (Exception exc)
                            { }
                        }

                        #endregion

                        string ext = Path.GetExtension(logoImage.FileName);
                        logoImageFileName = Guid.NewGuid().ToString() + ext;
                        using (var fileStream = new FileStream(hostFolder + "\\" + domainSetting.DomainName + "\\" + logoImageFileName, FileMode.Create))
                        {
                            await backgroundImage.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(backgroundImageFileName) || !string.IsNullOrEmpty(logoImageFileName))
                {
                    if (consoleBusiness.UpdateDomainSettingPictures(DomainSettingId, 
                        backgroundImageFileName, 
                        logoImageFileName, 
                        this.userId.Value))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            backgroundImage = backgroundImageFileName,
                            logoImage = logoImageFileName,
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
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult Hierarchy(int Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/DomainsSettingManagement/Index";
            }

            try
            {
                if (consoleBusiness.HasParentDomainUser(Id, this.userId.Value))
                {
                    var domain = consoleBusiness.GetDomainsSettingsWithDomainSettingId(Id);
                    var ParentNode = new SpaceTreeNodeVM()
                    {
                        Id = (long)domain.DomainSettingId,
                        NodeId = domain.DomainSettingId,
                        Name = domain.DomainName,
                        NodeType = "Domain"
                    };
                    ViewData["ParentNode"] = ParentNode;

                    ViewData["Root"] = JsonConvert.SerializeObject(consoleBusiness.GetRootOfSpaceTreeForDomain(Id));
                }
            }
            catch (Exception exc)
            { }
            //return View(direction + "HierarchyOfNodes");

            return View("Index");
        }

        public string GetChildNodesForSpaceTree(long NodeId = 0, string NodeType = "", long ParentId = 0)
        {
            try
            {
                switch (NodeType)
                {
                    case "Domain":
                        var roles = consoleBusiness.GetAllRolesListForSpaceTree(this.userId.Value, (int)ParentId);
                        return JsonConvert.SerializeObject(roles);
                        break;
                    case "Role":
                        var levels = consoleBusiness.GetAllLevelsListForSpaceTree(this.userId.Value, (int)NodeId);
                        break;
                    case "Level":
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

                            var usersList = consoleBusiness.GetDirectUsersWithLevelIdForSpaceTree((int)NodeId,
                                this.userId.Value,
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
                        break;
                    case "User":
                        if (consoleBusiness.ExistUserWithUserId((NodeId == 0 ? this.userId.Value : NodeId), this.domain))
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

                            var user = consoleBusiness.GetUserWithUserId(NodeId);
                            var usersList = consoleBusiness.GetDirectUsersWithParentIdForSpaceTree((user.UserId == 0) ? NodeId : user.UserId,
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
                        break;
                }
            }
            catch (Exception exc)
            { }
            return "[]";
        }

        public IActionResult TreeViews(int Id = 0)
        {
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/Host/DomainsSettingManagement/Index";
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
            string userNameTitle = "NameFamilyTitle";
            string birthDateTitle = "تاریخ تولد";
            List<CustomUsersVM> usersList = new List<CustomUsersVM>();
            try
            {
                if (consoleBusiness.ExistUserWithUserId(Id, this.domain))
                {
                    var user = consoleBusiness.GetUserWithUserId(Id);
                    ViewData["ParentUser"] = user;
                    var role = consoleBusiness.GetRoleWithUserId/*AndDomainName*/(Id/*, user.DomainSettingId*/);
                    usersList = consoleBusiness.GetUsersList(0/*user.DomainSettingId*/, 0, user.UserId);

                    string nameFamily = "";
                    if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Family))
                        nameFamily = nameFamilyTitle + "<br />" + notEnteredTitle;
                    else
                        nameFamily = nameFamilyTitle + "<br />" + user.Name + " " + user.Family;

                    string levelName = user.LevelName;

                    string dataContent = domainNameTitle + " : " + (string.IsNullOrEmpty(user.DomainName) ? notEnteredTitle : user.DomainName) +
                        "<br />" + emailTitle + " : " + (string.IsNullOrEmpty(user.Email) ? notEnteredTitle : user.Email) +
                        "<br />" + birthDateTitle + " : " + (string.IsNullOrEmpty(user.BirthDateTime) ? notEnteredTitle : user.BirthDateTime) +
                        "<br />" + nationalCodeTitle + " : " + (string.IsNullOrEmpty(user.NationalCode) ? notEnteredTitle : user.NationalCode) +
                        "<br />" + mobileTitle + " : " + (string.IsNullOrEmpty(user.Mobile) ? notEnteredTitle : user.Mobile);

                    string activeateDeactivateStatusText = "";

                    if (user.IsActivated.HasValue)
                        if (user.IsActivated.Value)
                            activeateDeactivateStatusText = "IsActivated";
                        else
                            activeateDeactivateStatusText = "IsDeactivated";
                    else
                        activeateDeactivateStatusText = "IsDeactivated";

                    string hierarchyOfUsersDataString = "";

                    string userImage = "";
                    if (!string.IsNullOrEmpty(user.Picture))
                    {
                        userImage = "<img src='/Files/UserFiles/" + user.DomainName + "/" +
                                        user.UserId + "/thumb_" + user.Picture + "' class='userThumbImage' />";
                    }
                    else
                    {
                        userImage = "<i class='fa fa-4x fa-user'></i>";
                    }

                    //string html = "\"<div class='row'><div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                    //    userImage + "</div>" +
                    //    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding' data-toggle='popover' data-html='True'" +
                    //    "data-content='" + dataContent + "' data-original-title='" + moreInformationTitle + "'" +
                    //    " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                    //    nameFamily + "<br />" + user.UserName + "</div></div><div class='parent-show-profile'><a " +
                    //    "class='show-profile' data-userId='1'>" + profileTitle + "</a></div>\", ";

                    string html = "\"<div class='card kl-card kl-xl kl-reveal kl-overlay kl-show kl-slide-in kl-shine kl-hide kl-spin'>" +
                                             "<div class='kl-card-block kl-lg bg-success kl-shadow-br kl-overlay'>" +
                                "<div class='kl-background'>" +
                                    "<div class='row'>" +
                                        "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                                            userImage +
                                        "</div>" +
                                        "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'><a class='showMoreInfo' data-toggle='popover' " +
                                                "tabindex='0' data-trigger='focus' data-html='True' " +
                                                "data-content='" + dataContent + "' data-container='body' data-original-title='" + moreInformationTitle + "'" +
                                                " data-placement='top' title='" + roleTitle + " : " + user.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                                            registerDateTitle + ":" +
                                            "<br />" +
                                            user.RegisterDate +
                                            "<br />" +
                                            (!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "09122060766") +
                                        "</a></div>" +
                                    "</div>" +
                                //userImage +
                                "</div>" +
                                "<div class='kl-card-overlay kl-card-overlay-split-v-4 kl-dark kl-inverse kl-bottom-in'>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pbl kl-show text-white kl-txt-shadow'>" +
                                //"<div class='kl-figure-block'>" +
                                //    "<span class='kl-figure'>109</span>" +
                                //    "<span class='kl-title text-white'>Following</span>" +
                                //"</div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pbr kl-show text-white kl-txt-shadow'>" +
                                //"<div class='kl-figure-block'>" +
                                //    "<span class='kl-figure>26k</span>" +
                                //    "<span class='kl-title text-white'>Followers</span>" +
                                //"</div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pm kl-show text-white kl-txt-shadow text-center'>" +
                                //birthDateTitle + ":" +
                                //user.BirthDateTime +
                                //"<br />" +
                                //(!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "") +
                                //"<h3 class='mb-0'>Joe Bloggs</h3>" +
                                //"<div class='text-white small'>asdasdasd</div>" +
                                "</div>" +
                                //"<div class='kl-card-item kl-ptl kl-v kl-card-social kl-slide-in'>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEa0lEQVRYR8VXa2hbZRh+3vckXbu09cKKOJyC29y8IZQmduiw9YesDjbc7HqTIV46GV5okjrdhAZRytY2HbMiE0FhNqdtZAiiGwPpREGb1NUfq0xnHdNZ53602GzrJT3fKydb2zTXs2xgfoXwvM/zfO/75vnOIfzPH8pVv7qvT3P8tbrokiZ2x9i/kU98lVO5cFk2UOHrty0vKnxCkWwmhUeUqHuY2T4vauAfEIaE6BgMrbdnV+moFUNZDZjCtxc7GiHqDYBXWCFVUIrBujK4pff1spFMNRkNNLT/cJ8SDoDxkBXhJIySaWF6a23E6ff5SKXiSGugtm1gI7P0CXhpTuJxRULqcP7EZEOqPUlpwBQnps8B2K5XfK6eSB0pdNg2f7ijLBrPmWTAbLsQwjfi5InmBdLV43n4lbQGYgvncAzmPHMr7SJU6W7X0fnOxNfU+UM7IXjfCk+uGFHy2/lLl+897qucNTnmRxA7fVHB71b/avEGFGhcgwQEmFBQTzF4LRTGwLg15eYLngl4Xd2LDNR3hJ8UyJdpTybyoiJpZfCyeIxBOGePqnXdu8rPmb83Hhy0RyJSQSSrBGo/wHlJnITjuttVuchArT90kASN6QywXd00O2Ur0bSoLtCccTPcGfC4Pkisq20LVwgbwUTDJs4MKm0muizw5vrx+RHUtYdOgnB/2g4QKnS36xufT/jXotBWBdouSj0KDTW97vJjyQYGThPTqnR8BNoY8Di/ihkwLxY+e+fkomxfNGQl0HjvmgnnnkWJJkLVwSAHt20z4uHVvuE8myMyBea0QSdC7h6vszMGeLZz6OZpFR3PuNmEB3S3a9jK9td0DpayUj9mwbbqHtfuKx04cKLEFp29kKmAgK8jhbzpix1ll7OZqG8P7xaSdzPiBB261+W90gFff/50kWMyGzFAL82uOPNRYsvj66oOnF5SHB37hUF3Zeloi+52vb2whPtC56HhtoxdIDlU6NCeT8zz+Jq6jvA7gOzJdhgR2t7jdR5aMNAWOgLGhmyFCuqUTbC121v+c9Lmd4RqSCk90/LN1ShFD/Y2O0/OG6htDzcRiT+VAQO4oCk6Ck1GWKnPEsVjKVq41H01qDjbIQCM6m7nHSCSBQN7TywX28yfDE5JIILvQfhY2PiuAPl/TxuGjVjuFiWPK8ZzLFhtQTgGIaA94HE1X/2+UFbXEfoUQINVohxxs7M2Xhl8reyPJAM1+wZXMhnDYFqSI7mVsv26x9U0B0xKqjp/uBki+6wwXTtGRtgupd2vlk+kNWBm/anigSAJb7l2gQwVSi6SjdYHmlw/xaNSZrUZTDPFBYdFuOqGmDDFWTYFPOX9iXxpLwvzXp+4aPgJ9PL1mZARYno68eRpR5AoVucPbRBD3st0taYxaD5ydbFdtcTP3HIH4oGx17JCR40wXoChHsuSdKMEBCC2roC39Gy27mV9NUskqG/99hbk5a9TgjVEUgJBHhgTouiMCIZ6vWXDZsJlE7Y8AqtEueL+Az80sTB3CtXzAAAAAElFTkSuQmCC'></a>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAD9klEQVRYR8WXX2hbdRTHv+fcpOscisJG16gvTnQqIig+JFm1vsjmwOGwf7INrTa9lakPGw6GCA2+CKL1XxXS1E3QpUkKwxedzAer7CYTJxPZcMKmzOncFBxWx7o29xy50bZJ1uT+murM04Wc8/1+fuece+69hP/5R436d3TkrF+Xnb/SPa9Baxn9Mf7OY5ONaBkDtLcnAtOha+9XkQ3qcpQIN4ERnDUVOUvgw2DaD1DWyfSeNgHyBfCMp1pDNlR3Any9iSgEAgujEBnIZ/tP1MupC9DWsevWaauYZtAdRsZVQQJcJNBzhdU/DiKRkPk0agJEukfWA5oDcEUj5uU5Ctm7pLlp83xzMi+AZy7Q9xkILNZ8Jl9B+6aucjd8Odw/Xa55CYBXdtdyv/g3Tl4Nr9ChQsZ+uiaAN3CTK0OHjHsuclaZxgA+CdIiAy2q2Fmvakq0rjAa/2gmpqICkdjwVii9aVJ2l+RbLlqRwlj8t5n4cCy5mpS/qZ8vx5vO/HzL+Hii6MXNApRut5Wt3xnfalA7n7FT5WZmAACpbnGy9p4KgOim1AMq+MDk9F6MCK05mIs73nWke7gH4AERt4mZQ34aCh0vZOz7KgAi3ckkwLZf8lzv+G4n03uoBNCVeh2EiuGqqyMQDkwvP5Deem62BeGO1BGycJsfALnytTJ/HCDrtc8yj5/y4sPdww+RUlQJywl41E+jdHLGeifd92EJwHuw/ES/X6jY7TVUSDXpZO0n5vs70jlyD1g/NQIAbXcy8VdKAO09u6+emiyeM0qsAxCOpWKkSJvoKPSFQsZ+tgTQ1rFrhWu5v5gkwnvqMR0X0nhhtP/Y3zMwvJ0IG0W1lYhvMNJRvJzP9j0zU4HmqcniBaPEf4IIc0MY7UwNKePJBeUrDTjZ+POzQxjpTJ4Bc4upyGIBAHokn4m/OwsQ7UztU8baywagdHs+Gz8yB9A9sk2hg5cDQEROH8zZ13k7sQzg7ZCKnAKDTSAW0wKCvuRk7B0Vm7A0zbHUe1Bs/i8BBCgGXFl1YKz/h0sBupKrhPgoA0v8IBqtACledbJ92+ZWepVTtCu1Qwkv+gFAMC2M0nseC4ImrRPghFpL7/x8z5aJmgBIJDh8rHWMwBt9IRYQIMCfzNqWT9tflafN+07Y3rO7+eKku5eg6xbgUTPUM7eEH3RyvZ9UB9V8K77LTgabJmiQQE8tBsIrO7M+XH3y2i2ocgvHRtaSum8AfONCQLxptxRDxcDSgfKeG1egPLD0WdYS6hJCnKD3Alyzct6SsZjSyjyUT8dP+kH7fppVC6zZ9NY1imAYQjcLZAUpNRFoQgnfQ3E4n+096m04P2PjFpgKNRr3F+G5pDBOJtRGAAAAAElFTkSuQmCC'></a>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEdElEQVRYR8WXXYiUZRTH/+e8szOLzrBKmlF2EUaRy9K+874QYtJ2U5qSEG5dWN1URlIXRkKIYRghRZ9oF3WZpahhEZTRTUNd6T7PvIHuRbhbVGRfhDszC+rs7DnxLLvLOM774Rr0XM57Pn7n63nOEP7nQwv1Pzw87J09e7ZERD19fX2NSqVycSG2MgMMDQ3lJicn7wOwWVXXishtzNwz51RE/vQ8LwLw9fT09JEois5lAUoFcI4bjcY2EXmRmW/OYhSAADgMYI+1djxJJxHA9/3VzHwIwJ0ZHXeKXQKw21r71izUFWZiAYIg2CgiR5l50QKdz6sR0fFisbi1W590BZh1/hkz567V+Zy+qp4gos3W2ql2m1cAuLQDGPkvIu+EV9UD1Wr1uViA2YYz11Dz1ISp6oZqtfrVnOBlGQiCYDuA97pZERGXukEiupuIdonITQDOMbOb/6KIrGDmgy5KAJuI6OUYmrFSqXRHpVJpue/zAC76Wq32Y9yoich4FEW3OqUgCBaVSiVpb6r+/v786Oho030vl8sPE9GRuHSo6qPVavXjywDCMHxAVb+IUxKRf6IoWpaaYwBhGG5R1WMJtipRFN3bCfC+qm6LUyKi3caYV7MAuMz6vv8dM6+NkZdms7ns9OnT5+dLEATBGQD9CdTXR1H0d0YAV4aniOiDhIA2GmO+nAFwD8vY2NiF9ru9Q1GttZzV+Wwf3E9E893eqUtEzxtj3p4BGBwcXOJ53vkUB0ustbWsEEEQDAM4miC/z1q7awbA9/3lzPxXivFN1trYJu3U9X1/LzO/lFDSN6MoemEGYGhoqLfRaFxIAiAiUywW12V594Mg6AMwCsDdFXHHvZR755vQ9/0/mHlFjHRTRJiITjHzDmPMCADtIktBELhd4V1mLqcE9Lgx5uA8QBiGJ1R1fTclVf2BiJ4B8CGAlQB+I6LtxpjP5+TDMHxienr6NWa+LkufENGAMeZMO8AOVXXvdtdDRN+q6rMAXgHQrNVqjxUKhZ5cLtfXarVqvb29vQCyjuk5a60LRNtLcCMz/wogdtxExDnfn8/nL7VarcUAVqnqYmYuGWM+8X2/mTDK7YG9Ya3d6X7ofIw+ArA1LYVu/8vn8+HU1NQtABYx81JjzFHf9y+mAYhIi5lXWWt/6QawarZ7C0kQIvJ7Lpe7q9VqrSSiG5i5MDEx8WmpVJpMW2KI6B1jzI45+1csJOVyeScRvZ6Shb1E5KbgsKruY+Z9IrJGVVcT0dMJsz+ez+fLJ0+erMcCuB4Iw/CYqj6UAjEBwN2ergxjIrKSmV0jxh2XnXUjIyPftwt03QndxVSv148T0Ya0fsj4fRLAg9babzrlk7biHjeWRORGb8HHLTK5XG5LZ+RJJbjMWblcXk9E+wHMbENZj+t2z/MOeJ63p73mmTPQLujWtXq9/oiqPsnM93SOb4dR95fskIgciKLo5zTg1L9mnQYGBgaWFgqFNQBuV9XlIpJn5joR/QQgMsa4R6jbO9GV5aoB0iK62u//AuhE0jAWsl0eAAAAAElFTkSuQmCC'></a>" +
                                //"</div>" +
                                "<div class='kl-card-item kl-pb kl-show online_status'>" +
                                    (user.IsActivated.HasValue ?
                                    user.IsActivated.Value ?
                                    "<span class='badge badge-success green'>" :
                                    "<span class='badge badge-success red'>" :
                                    "<span class='badge badge-success red'>") +
                                        //"<span class='badge badge-success green'>" + 
                                        activeateDeactivateStatusText +
                                    "</span>" +
                                "</div>" +
                                "<div class='kl-card-avatar kl-xl kl-pm kl-slow kl-hide'>" +
                                    "<div class='row'>" +
                                        "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                                            userImage +
                                        "</div>" +
                                        "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'>" +
                                            nameFamily +
                                            "<br />" +
                                            /*userNameTitle + 
                                            "<br />" + */
                                            user.UserName +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>\", ";

                    hierarchyOfUsersDataString += " { id: \"" + user.UserId.ToString() + "\",";
                    hierarchyOfUsersDataString += " name: " + html;
                    hierarchyOfUsersDataString += " data: {}, ";
                    hierarchyOfUsersDataString += " children: [  ";

                    hierarchyOfUsersDataString += hierarchyOfUsersData(user.UserId,
                        usersList,
                        profileTitle,
                        roleTitle,
                        levelTitle,
                        notEnteredTitle,
                        domainNameTitle,
                        emailTitle,
                        birthDateTitle,
                        registerDateTitle,
                        mobileTitle,
                        nationalCodeTitle,
                        moreInformationTitle);

                    hierarchyOfUsersDataString += " ] } ";
                    ViewData["HierarchyOfUsersDataString"] = hierarchyOfUsersDataString;
                }
            }
            catch (Exception exc)
            { }
            //return View(themeName /*this.theme.ThemeName*/ + "HierarchyOfUsers", usersList);

            return View("Index");
        }

        public string GetChildNodesForTree(long UserId = 0, string NodeType = "")
        {
            try
            {
                string domainNameTitle = "دامنه";
                string nameFamilyTitle = "اسم";

                var childUsers = consoleBusiness.GetDirectUsersWithParentIdForTree(UserId,
                    domainNameTitle,
                    nameFamilyTitle,
                    "Host");

                if (childUsers != null)
                {
                    return JsonConvert.SerializeObject(childUsers)
                        //.Replace("\"{ tooltip = true }\"", "{ \"tooltip\": \"\" }")
                        //.Replace("folder = true", "\"folder\": \"true\"")
                        //.Replace("lazy = true", "\"lazy\": \"true\"")
                        .Replace("--DOMAINNAME--", "DomainNameTitle")
                        .Replace("--NAMEFAMILY--:", "NameFamilyTitle");
                    //.Replace("\"{ children = true }\"", "{ \"children\": [] }");
                }
            }
            catch (Exception exc)
            { }

            return "";
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

        #region Needed Methods

        private string hierarchyOfUsersData(long parentUserId, List<CustomUsersVM> usersList,
            string profileTitle,
            string roleTitle,
            string levelTitle,
            string notEnteredTitle,
            string domainNameTitle,
            string emailTitle,
            string birthDateTitle,
            string registerDateTitle,
            string mobileTitle,
            string nationalCodeTitle,
            string moreInformationTitle)
        {
            string hierarchyOfUsersDataString = "";
            if (usersList.Any(u => u.ParentUserId.Equals(parentUserId)))
            {
                var childUsersList = usersList.Where(u => u.ParentUserId.Equals(parentUserId)).ToList();
                foreach (var childUser in childUsersList)
                {
                    string nameFamily = "";
                    if (string.IsNullOrEmpty(childUser.Name) && string.IsNullOrEmpty(childUser.Family))
                        nameFamily = notEnteredTitle;
                    else
                        nameFamily = childUser.Name + " " + childUser.Family;

                    string levelName = childUser.LevelName;

                    string dataContent = domainNameTitle + " : " + (string.IsNullOrEmpty(childUser.DomainName) ? notEnteredTitle : childUser.DomainName) +
                        "<br />" + emailTitle + " : " + (string.IsNullOrEmpty(childUser.Email) ? notEnteredTitle : childUser.Email) +
                        "<br />" + birthDateTitle + " : " + (string.IsNullOrEmpty(childUser.BirthDateTime) ? notEnteredTitle : childUser.BirthDateTime) +
                        "<br />" + nationalCodeTitle + " : " + (string.IsNullOrEmpty(childUser.NationalCode) ? notEnteredTitle : childUser.NationalCode) +
                        "<br />" + mobileTitle + " : " + (string.IsNullOrEmpty(childUser.Mobile) ? notEnteredTitle : childUser.Mobile);

                    string activeateDeactivateStatusText = "";

                    if (childUser.IsActivated.HasValue)
                        if (childUser.IsActivated.Value)
                            activeateDeactivateStatusText = "IsActivated";
                        else
                            activeateDeactivateStatusText = "IsDeactivated";
                    else
                        activeateDeactivateStatusText = "IsDeactivated";

                    hierarchyOfUsersDataString += " { id: \"" + childUser.UserId.ToString() + "\",";

                    string userImage = "";
                    if (!string.IsNullOrEmpty(childUser.Picture))
                    {
                        userImage = "<img src='/Files/UserFiles/" + childUser.DomainName + "/" +
                                        childUser.UserId + "/thumb_" + childUser.Picture + "' class='userThumbImage' />";
                    }
                    else
                    {
                        userImage = "<i class='fa fa-4x fa-user'></i>";
                    }

                    //string html = "\"<div class='row'><div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                    //    userImage + "</div>" +
                    //    "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding' data-toggle='popover' data-html='True'" +
                    //    "data-content='" + dataContent + "' data-original-title='" + moreInformationTitle + "'" +
                    //    " data-placement='top' title='" + roleTitle + " : " + childUser.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                    //    nameFamily + "<br />" + childUser.UserName + "</div></div><div class='parent-show-profile'><a " +
                    //    "class='show-profile' data-userId='1'>" + profileTitle + "</a></div>\", ";

                    string html = "\"<div class='card kl-card kl-xl kl-reveal kl-overlay kl-show kl-slide-in kl-shine kl-hide kl-spin'>" +
                                             "<div class='kl-card-block kl-lg bg-success kl-shadow-br kl-overlay'>" +
                                "<div class='kl-background'>" +
                                    "<div class='row'>" +
                                        "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                                            userImage +
                                        "</div>" +
                                        "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'><a class='showMoreInfo' data-toggle='popover'" +
                                                " tabindex='0' data-trigger='focus' data-html='True' " +
                                                "data-content='" + dataContent + "' data-container='body' data-original-title='" + moreInformationTitle + "'" +
                                                " data-placement='top' title='" + roleTitle + " : " + childUser.RoleName + " " + levelTitle + " : " + levelName + "'>" +
                                            registerDateTitle + ":" +
                                            "<br />" +
                                            childUser.RegisterDate +
                                            "<br />" +
                                            (!string.IsNullOrEmpty(childUser.Mobile) ? childUser.Mobile : "09122060766") +
                                        "</a></div>" +
                                    "</div>" +
                                //userImage +
                                "</div>" +
                                "<div class='kl-card-overlay kl-card-overlay-split-v-4 kl-dark kl-inverse kl-bottom-in'>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                    "<div class='kl-card-overlay-item'></div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pbl kl-show text-white kl-txt-shadow'>" +
                                //"<div class='kl-figure-block'>" +
                                //    "<span class='kl-figure'>109</span>" +
                                //    "<span class='kl-title text-white'>Following</span>" +
                                //"</div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pbr kl-show text-white kl-txt-shadow'>" +
                                //"<div class='kl-figure-block'>" +
                                //    "<span class='kl-figure>26k</span>" +
                                //    "<span class='kl-title text-white'>Followers</span>" +
                                //"</div>" +
                                "</div>" +
                                "<div class='kl-card-item kl-pm kl-show text-white kl-txt-shadow text-center'>" +
                                //birthDateTitle + ":" +
                                //user.BirthDateTime +
                                //"<br />" +
                                //(!string.IsNullOrEmpty(user.Mobile) ? user.Mobile : "") +
                                //"<h3 class='mb-0'>Joe Bloggs</h3>" +
                                //"<div class='text-white small'>asdasdasd</div>" +
                                "</div>" +
                                //"<div class='kl-card-item kl-ptl kl-v kl-card-social kl-slide-in'>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEa0lEQVRYR8VXa2hbZRh+3vckXbu09cKKOJyC29y8IZQmduiw9YesDjbc7HqTIV46GV5okjrdhAZRytY2HbMiE0FhNqdtZAiiGwPpREGb1NUfq0xnHdNZ53602GzrJT3fKydb2zTXs2xgfoXwvM/zfO/75vnOIfzPH8pVv7qvT3P8tbrokiZ2x9i/kU98lVO5cFk2UOHrty0vKnxCkWwmhUeUqHuY2T4vauAfEIaE6BgMrbdnV+moFUNZDZjCtxc7GiHqDYBXWCFVUIrBujK4pff1spFMNRkNNLT/cJ8SDoDxkBXhJIySaWF6a23E6ff5SKXiSGugtm1gI7P0CXhpTuJxRULqcP7EZEOqPUlpwBQnps8B2K5XfK6eSB0pdNg2f7ijLBrPmWTAbLsQwjfi5InmBdLV43n4lbQGYgvncAzmPHMr7SJU6W7X0fnOxNfU+UM7IXjfCk+uGFHy2/lLl+897qucNTnmRxA7fVHB71b/avEGFGhcgwQEmFBQTzF4LRTGwLg15eYLngl4Xd2LDNR3hJ8UyJdpTybyoiJpZfCyeIxBOGePqnXdu8rPmb83Hhy0RyJSQSSrBGo/wHlJnITjuttVuchArT90kASN6QywXd00O2Ur0bSoLtCccTPcGfC4Pkisq20LVwgbwUTDJs4MKm0muizw5vrx+RHUtYdOgnB/2g4QKnS36xufT/jXotBWBdouSj0KDTW97vJjyQYGThPTqnR8BNoY8Di/ihkwLxY+e+fkomxfNGQl0HjvmgnnnkWJJkLVwSAHt20z4uHVvuE8myMyBea0QSdC7h6vszMGeLZz6OZpFR3PuNmEB3S3a9jK9td0DpayUj9mwbbqHtfuKx04cKLEFp29kKmAgK8jhbzpix1ll7OZqG8P7xaSdzPiBB261+W90gFff/50kWMyGzFAL82uOPNRYsvj66oOnF5SHB37hUF3Zeloi+52vb2whPtC56HhtoxdIDlU6NCeT8zz+Jq6jvA7gOzJdhgR2t7jdR5aMNAWOgLGhmyFCuqUTbC121v+c9Lmd4RqSCk90/LN1ShFD/Y2O0/OG6htDzcRiT+VAQO4oCk6Ck1GWKnPEsVjKVq41H01qDjbIQCM6m7nHSCSBQN7TywX28yfDE5JIILvQfhY2PiuAPl/TxuGjVjuFiWPK8ZzLFhtQTgGIaA94HE1X/2+UFbXEfoUQINVohxxs7M2Xhl8reyPJAM1+wZXMhnDYFqSI7mVsv26x9U0B0xKqjp/uBki+6wwXTtGRtgupd2vlk+kNWBm/anigSAJb7l2gQwVSi6SjdYHmlw/xaNSZrUZTDPFBYdFuOqGmDDFWTYFPOX9iXxpLwvzXp+4aPgJ9PL1mZARYno68eRpR5AoVucPbRBD3st0taYxaD5ydbFdtcTP3HIH4oGx17JCR40wXoChHsuSdKMEBCC2roC39Gy27mV9NUskqG/99hbk5a9TgjVEUgJBHhgTouiMCIZ6vWXDZsJlE7Y8AqtEueL+Az80sTB3CtXzAAAAAElFTkSuQmCC'></a>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAD9klEQVRYR8WXX2hbdRTHv+fcpOscisJG16gvTnQqIig+JFm1vsjmwOGwf7INrTa9lakPGw6GCA2+CKL1XxXS1E3QpUkKwxedzAer7CYTJxPZcMKmzOncFBxWx7o29xy50bZJ1uT+murM04Wc8/1+fuece+69hP/5R436d3TkrF+Xnb/SPa9Baxn9Mf7OY5ONaBkDtLcnAtOha+9XkQ3qcpQIN4ERnDUVOUvgw2DaD1DWyfSeNgHyBfCMp1pDNlR3Any9iSgEAgujEBnIZ/tP1MupC9DWsevWaauYZtAdRsZVQQJcJNBzhdU/DiKRkPk0agJEukfWA5oDcEUj5uU5Ctm7pLlp83xzMi+AZy7Q9xkILNZ8Jl9B+6aucjd8Odw/Xa55CYBXdtdyv/g3Tl4Nr9ChQsZ+uiaAN3CTK0OHjHsuclaZxgA+CdIiAy2q2Fmvakq0rjAa/2gmpqICkdjwVii9aVJ2l+RbLlqRwlj8t5n4cCy5mpS/qZ8vx5vO/HzL+Hii6MXNApRut5Wt3xnfalA7n7FT5WZmAACpbnGy9p4KgOim1AMq+MDk9F6MCK05mIs73nWke7gH4AERt4mZQ34aCh0vZOz7KgAi3ckkwLZf8lzv+G4n03uoBNCVeh2EiuGqqyMQDkwvP5Deem62BeGO1BGycJsfALnytTJ/HCDrtc8yj5/y4sPdww+RUlQJywl41E+jdHLGeifd92EJwHuw/ES/X6jY7TVUSDXpZO0n5vs70jlyD1g/NQIAbXcy8VdKAO09u6+emiyeM0qsAxCOpWKkSJvoKPSFQsZ+tgTQ1rFrhWu5v5gkwnvqMR0X0nhhtP/Y3zMwvJ0IG0W1lYhvMNJRvJzP9j0zU4HmqcniBaPEf4IIc0MY7UwNKePJBeUrDTjZ+POzQxjpTJ4Bc4upyGIBAHokn4m/OwsQ7UztU8baywagdHs+Gz8yB9A9sk2hg5cDQEROH8zZ13k7sQzg7ZCKnAKDTSAW0wKCvuRk7B0Vm7A0zbHUe1Bs/i8BBCgGXFl1YKz/h0sBupKrhPgoA0v8IBqtACledbJ92+ZWepVTtCu1Qwkv+gFAMC2M0nseC4ImrRPghFpL7/x8z5aJmgBIJDh8rHWMwBt9IRYQIMCfzNqWT9tflafN+07Y3rO7+eKku5eg6xbgUTPUM7eEH3RyvZ9UB9V8K77LTgabJmiQQE8tBsIrO7M+XH3y2i2ocgvHRtaSum8AfONCQLxptxRDxcDSgfKeG1egPLD0WdYS6hJCnKD3Alyzct6SsZjSyjyUT8dP+kH7fppVC6zZ9NY1imAYQjcLZAUpNRFoQgnfQ3E4n+096m04P2PjFpgKNRr3F+G5pDBOJtRGAAAAAElFTkSuQmCC'></a>" +
                                //    "<a href='#'><img class='kl-b-1 kl-b-white kl-b-circle kl-spin' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAEdElEQVRYR8WXXYiUZRTH/+e8szOLzrBKmlF2EUaRy9K+874QYtJ2U5qSEG5dWN1URlIXRkKIYRghRZ9oF3WZpahhEZTRTUNd6T7PvIHuRbhbVGRfhDszC+rs7DnxLLvLOM774Rr0XM57Pn7n63nOEP7nQwv1Pzw87J09e7ZERD19fX2NSqVycSG2MgMMDQ3lJicn7wOwWVXXishtzNwz51RE/vQ8LwLw9fT09JEois5lAUoFcI4bjcY2EXmRmW/OYhSAADgMYI+1djxJJxHA9/3VzHwIwJ0ZHXeKXQKw21r71izUFWZiAYIg2CgiR5l50QKdz6sR0fFisbi1W590BZh1/hkz567V+Zy+qp4gos3W2ql2m1cAuLQDGPkvIu+EV9UD1Wr1uViA2YYz11Dz1ISp6oZqtfrVnOBlGQiCYDuA97pZERGXukEiupuIdonITQDOMbOb/6KIrGDmgy5KAJuI6OUYmrFSqXRHpVJpue/zAC76Wq32Y9yoich4FEW3OqUgCBaVSiVpb6r+/v786Oho030vl8sPE9GRuHSo6qPVavXjywDCMHxAVb+IUxKRf6IoWpaaYwBhGG5R1WMJtipRFN3bCfC+qm6LUyKi3caYV7MAuMz6vv8dM6+NkZdms7ns9OnT5+dLEATBGQD9CdTXR1H0d0YAV4aniOiDhIA2GmO+nAFwD8vY2NiF9ru9Q1GttZzV+Wwf3E9E893eqUtEzxtj3p4BGBwcXOJ53vkUB0ustbWsEEEQDAM4miC/z1q7awbA9/3lzPxXivFN1trYJu3U9X1/LzO/lFDSN6MoemEGYGhoqLfRaFxIAiAiUywW12V594Mg6AMwCsDdFXHHvZR755vQ9/0/mHlFjHRTRJiITjHzDmPMCADtIktBELhd4V1mLqcE9Lgx5uA8QBiGJ1R1fTclVf2BiJ4B8CGAlQB+I6LtxpjP5+TDMHxienr6NWa+LkufENGAMeZMO8AOVXXvdtdDRN+q6rMAXgHQrNVqjxUKhZ5cLtfXarVqvb29vQCyjuk5a60LRNtLcCMz/wogdtxExDnfn8/nL7VarcUAVqnqYmYuGWM+8X2/mTDK7YG9Ya3d6X7ofIw+ArA1LYVu/8vn8+HU1NQtABYx81JjzFHf9y+mAYhIi5lXWWt/6QawarZ7C0kQIvJ7Lpe7q9VqrSSiG5i5MDEx8WmpVJpMW2KI6B1jzI45+1csJOVyeScRvZ6Shb1E5KbgsKruY+Z9IrJGVVcT0dMJsz+ez+fLJ0+erMcCuB4Iw/CYqj6UAjEBwN2ergxjIrKSmV0jxh2XnXUjIyPftwt03QndxVSv148T0Ya0fsj4fRLAg9babzrlk7biHjeWRORGb8HHLTK5XG5LZ+RJJbjMWblcXk9E+wHMbENZj+t2z/MOeJ63p73mmTPQLujWtXq9/oiqPsnM93SOb4dR95fskIgciKLo5zTg1L9mnQYGBgaWFgqFNQBuV9XlIpJn5joR/QQgMsa4R6jbO9GV5aoB0iK62u//AuhE0jAWsl0eAAAAAElFTkSuQmCC'></a>" +
                                //"</div>" +
                                "<div class='kl-card-item kl-pb kl-show online_status'>" +
                                    (childUser.IsActivated.HasValue ?
                                    childUser.IsActivated.Value ?
                                    "<span class='badge badge-success green'>" :
                                    "<span class='badge badge-success red'>" :
                                    "<span class='badge badge-success red'>") +
                                        //"<span class='badge badge-success green'>" + 
                                        activeateDeactivateStatusText +
                                    "</span>" +
                                "</div>" +
                                "<div class='kl-card-avatar kl-xl kl-pm kl-slow kl-hide'>" +
                                    "<div class='row'>" +
                                        "<div class='col-lg-5 col-md-5 col-xs-5 col-sm-5 no-padding'>" +
                                            userImage +
                                        "</div>" +
                                        "<div class='col-lg-7 col-md-7 col-xs-7 col-sm-7 no-padding'>" +
                                            nameFamily +
                                            "<br />" +
                                            /*userNameTitle + 
                                            "<br />" + */
                                            childUser.UserName +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>\", ";

                    hierarchyOfUsersDataString += " name: " + html;

                    hierarchyOfUsersDataString += " data: {}, ";
                    hierarchyOfUsersDataString += " children: [  ";
                    hierarchyOfUsersDataString += hierarchyOfUsersData(childUser.UserId,
                        usersList,
                        profileTitle,
                        roleTitle,
                        levelTitle,
                        notEnteredTitle,
                        domainNameTitle,
                        emailTitle,
                        birthDateTitle,
                        registerDateTitle,
                        mobileTitle,
                        nationalCodeTitle,
                        moreInformationTitle);
                    hierarchyOfUsersDataString += " ] }, ";
                }
            }
            return hierarchyOfUsersDataString;
        }

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

        //[AjaxOnly]
        //[HttpPost]
        //public IActionResult ToggleDeleteDomainSetting(int domainSettingsId)
        //{
        //    try
        //    {
        //        if (business.ToggleDeleteDomainSetting(domainSettingsId,
        //            userId.Value))
        //        {
        //            return Json(new
        //            {
        //                Result = "OK",
        //                Message = "Success"
        //            });
        //        }
        //    }
        //    catch (Exception exc)
        //    { }

        //    return Json(new
        //    {
        //        Result = "ERROR",
        //        Message = "ErrorMessage"
        //    });
        //}

        //[AjaxOnly]
        //[HttpPost]
        //public IActionResult ToggleActivatedDomainSetting(int domainSettingsId)
        //{
        //    try
        //    {
        //        if (business.ToggleActivatedDomainSetting(domainSettingsId,
        //            userId.Value))
        //        {
        //            return Json(new
        //            {
        //                Result = "OK",
        //                Message = "Success"
        //            });
        //        }
        //    }
        //    catch (Exception exc)
        //    { }

        //    return Json(new
        //    {
        //        Result = "ERROR",
        //        Message = "ErrorMessage"
        //    });
        //}
    }
}
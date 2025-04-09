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
using Microsoft.Extensions.Configuration;

namespace Web.Console.Areas.Host.Controllers
{
    [Area("Host")]
    public class RolesManagementController : ExtraHostController
    {
        public RolesManagementController(IHostEnvironment _hostEnvironment,
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

            if (ViewData["CustomUsersVMList"] == null)
            {
                ViewData["CustomUsersVMList"] = consoleBusiness.GetUsersList(0, 0, this.userId.Value);
            }

            //return View(themeName /*this.theme.ThemeName*/ + "Index");

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfRoles(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
            string roleNameSearch = null,
            //int domainSettingIdSearch = 0,
            string loginUrlSearch = null)
        {
            try
            {
                int listCount = 0;

                var rolesList = consoleBusiness.GetRolesList(jtStartIndex, 
                    jtPageSize, 
                    ref listCount, 
                    jtSorting, 
                    roleNameSearch, 
                    //domainSettingIdSearch,
                    loginUrlSearch, 
                    this.userId);
                return Json(new { Result = "OK", Records = rolesList, TotalRecordCount = listCount });
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
        public IActionResult AddToRoles(RolesVM roleVM)
        {
            try
            {
                //var domainSetting = business.GetAllDomainsSettingsList(this.userId.Value).FirstOrDefault(d => d.DomainSettingId.Equals(roleVM.DomainSettingId));
                //if(domainSetting!=null)
                //    roleVM.UserIdCreator = domainSetting.UserIdCreator.Value;

                roleVM.CreateEnDate = DateTime.Now;
                roleVM.CreateTime = PersianDate.TimeNow;
                //roleVM.UserIdCreator = this.userId.Value;

                if (ModelState.IsValid)
                {
                    roleVM.RoleId = consoleBusiness.CreateRole(roleVM);

                    if (roleVM.RoleId > 0)
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "Success",
                            Record = roleVM
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
        public IActionResult UpdateRole(RolesVM roleVM)
        {
            try
            {
                roleVM.EditEnDate = DateTime.Now;
                roleVM.EditTime = PersianDate.TimeNow;
                roleVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateRole(roleVM))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Record = roleVM
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
        public IActionResult CompleteDeleteRole(int RoleId)
        {
            try
            {
                if (RoleId != this.roleId.Value)
                {
                    if (consoleBusiness.CompleteDeleteRole(RoleId, this.userId.Value/*this.domain*/))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            //Message = "Success"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentRoleErrorMessage"
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

        //public JsonResult GetDomainsSettings()
        //{
        //    try
        //    {
        //        var domainsSettingsList = business.GetAllDomainsSettingsList();
        //        return Json(new { Result = "OK", Options = domainsSettingsList });
        //    }
        //    catch (Exception exc)
        //    {
        //        return Json(new { Result = "ERROR", Message = "خطا" });
        //    }
        //}

        //[AjaxOnly]
        //[HttpPost]
        //public IActionResult ToggleActivatedRole(int RoleId)
        //{
        //    try
        //    {
        //        if (business.ToggleActivatedRole(RoleId))
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
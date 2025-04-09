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

namespace Web.Console.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesManagementController : ExtraAdminController
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
            ViewData["Title"] = "لیست نقشها";

            //if (ViewData["DomainsSettingsList"] == null)
            //    ViewData["DomainsSettingsList"] = business.GetAllDomainsSettingsList(this.userId.Value);

            //return View(themeName /*this.theme.ThemeName*/ + "Index");
              if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }
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
                    //this.domainsSettings.DomainSettingId,
                    loginUrlSearch,
                    this.userId).Where(r => !r.RoleName.Equals("Host")).ToList();

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
        public IActionResult ToggleActivationRole(int RoleId = 0)
        {
            try
            {
                if (consoleBusiness.ToggleActivationRole(RoleId, this.userId.Value))
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
    }
}
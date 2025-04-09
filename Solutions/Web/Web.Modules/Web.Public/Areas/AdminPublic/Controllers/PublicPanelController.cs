using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Core.Ext;
using Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Models.Business;
using AutoMapper;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using CustomAttributes;
using Services.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using VM.Console;
using Models.Business.ConsoleBusiness;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class PublicPanelController : ExtraAdminController
    {
        public PublicPanelController(IHostEnvironment _hostEnvironment,
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

            List<LevelShortCutImagesVM> levelShortCutImagesVMList = consoleBusiness.GetLevelShortCutImagesWithLevelId(this.levelId.Value, "fa", "Dashboard");
            return View("Index", levelShortCutImagesVMList);
        }
    }
}
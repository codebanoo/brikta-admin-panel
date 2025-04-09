using AutoMapper;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VM.Console;
using Web.Core.Controllers;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class AutomationPanelController : ExtraAdminController
    {
        public AutomationPanelController(IHostEnvironment _hostEnvironment,
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

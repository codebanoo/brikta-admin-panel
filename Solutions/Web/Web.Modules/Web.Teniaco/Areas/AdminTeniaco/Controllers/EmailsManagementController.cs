﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Services.Business;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class EmailsManagementController : ExtraAdminController
    {
        public EmailsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult RedirectToEmailPanel()
        {
            //"my.teniaco.com:7000/"
            return Redirect("http://my.teniaco.com:7000");
        }
    }
}

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
    public class ShortCutKeysManagementController : ExtraHostController
    {
        public ShortCutKeysManagementController(IHostEnvironment _hostEnvironment,
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
            //return View(direction + "Index");

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfShortCutKeys(int jtStartIndex,
            int jtPageSize)
        {
            try
            {
                int listCount = 0;
                var shortCutKeysList = consoleBusiness.GetShortCutKeysList(jtStartIndex, jtPageSize, out listCount);
                return Json(new { Result = "OK", Records = shortCutKeysList, TotalRecordCount = listCount });
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
        public IActionResult ToggleActivationShortCutKeys(int ShortCutKeyId)
        {
            try
            {
                if (consoleBusiness.ToggleActivationShortCutKeys(ShortCutKeyId))
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToShortCutKeys(ShortCutKeysVM shortCutKeysVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    shortCutKeysVM.ShortCutKeyId = consoleBusiness.AddToShortCutKeys(shortCutKeysVM);

                    if (shortCutKeysVM.ShortCutKeyId > 0)
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "Success",
                            Record = shortCutKeysVM
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
        public IActionResult UpdateShortCutKeys(ShortCutKeysVM shortCutKeysVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateShortCutKeys(shortCutKeysVM))
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Record = shortCutKeysVM
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
        public IActionResult CompleteDeleteShortCutKeys(int ShortCutKeyId = 0)
        {
            try
            {
                if (consoleBusiness.CompleteDeleteShortCutKeys(ShortCutKeyId))
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
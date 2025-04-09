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
    public class LimitationsManagementController : ExtraHostController
    {
        public LimitationsManagementController(IHostEnvironment _hostEnvironment,
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
            if (ViewData["DomainsSettingsList"] == null)
                ViewData["DomainsSettingsList"] = consoleBusiness.GetAllDomainsSettingsList();

            //return View(themeName /*this.theme.ThemeName*/ + "Index");

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfLimitations(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null,
            int domainSettingIdSearch = 0,
            string limitTitleSearch = null)
        {
            try
            {
                int listCount = 0;

                var limitationsList = consoleBusiness.GetLimitationsList(jtStartIndex,
                    jtPageSize,
                    ref listCount,
                    jtSorting,
                    domainSettingIdSearch,
                    limitTitleSearch,
                    "fa");

                return Json(new { Result = "OK", Records = limitationsList, TotalRecordCount = listCount });
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
        public IActionResult AddToLimitations(DomainLimitationsVM domainLimitationsVM)
        {
            try
            {
                string returnMessage = "";

                if (ModelState.IsValid)
                {
                    domainLimitationsVM.DomainLimitationsId = consoleBusiness.AddToLimitations(domainLimitationsVM, ref returnMessage);

                    if (domainLimitationsVM.DomainLimitationsId > 0)
                    {
                        return Json(new
                        {
                            Result = "OK",
                            Message = "Success",
                            Record = domainLimitationsVM
                        });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(returnMessage))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "Success",
                                Record = domainLimitationsVM
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = returnMessage
                            });
                        }
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
        public IActionResult UpdateLimitations(DomainLimitationsVM domainLimitationsVM)
        {
            try
            {
                string returnMessage = "";

                if (ModelState.IsValid)
                {
                    if (consoleBusiness.UpdateLimitations(domainLimitationsVM, ref returnMessage))
                    {
                        if (string.IsNullOrEmpty(returnMessage))
                            return Json(new
                            {
                                Result = "OK",
                                Record = domainLimitationsVM
                            });
                        else
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = returnMessage
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
        public IActionResult CompleteDeleteLimitations(int DomainLimitationsId = 0)
        {
            try
            {
                if (consoleBusiness.CompleteDeleteLimitations(DomainLimitationsId))
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

        public JsonResult GetListOfAreas(int DomainSettingId)
        {
            try
            {
                var areasList = consoleBusiness.GetAllAreas(DomainSettingId);
                return Json(new { Result = "OK", Records = areasList });
            }
            catch (Exception exc)
            {
                return Json(new { Result = "ERROR", Message = "خطا" });
            }
        }

        public JsonResult GetListOfControllers(int AreaId)
        {
            try
            {
                var controllersList = consoleBusiness.GetAllControllersInViewWithAreaId(AreaId);
                return Json(new { Result = "OK", Records = controllersList });
            }
            catch (Exception exc)
            {
                return Json(new { Result = "ERROR", Message = "خطا" });
            }
        }

        public JsonResult GetListOfActions(int ControllerId)
        {
            try
            {
                var actionsList = consoleBusiness.GetAllActionInControllersWithControllerId(ControllerId);
                return Json(new { Result = "OK", Records = actionsList });
            }
            catch (Exception exc)
            {
                return Json(new { Result = "ERROR", Message = "خطا" });
            }
        }
    }
}
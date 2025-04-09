using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Threading.Tasks;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class AgencyLocationManagementController : ExtraAdminController
    {
        public AgencyLocationManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult UpdateAgencylocation(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "AgenciesManagement");

            ViewData["Title"] = "موقعیت";

            AgenciesVM agenciesVM = new AgenciesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgenciesManagement/GetAgencyWithAgencyId";

                GetAgencyWithAgencyIdPVM getAgencyWithAgencyIdPVM = new GetAgencyWithAgencyIdPVM()
                {
                    AgencyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAgencyWithAgencyId(getAgencyWithAgencyIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<AgenciesVM>();


                            if (record != null)
                            {
                                agenciesVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["AgenciesList"] = agenciesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/ColleaguesManagement/ListOfAgencies/";
            }

            return View("Update");
        }


        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> UpdateAgencylocation(AgencyLocationVM agencyLocationVM)
        {
            
            JsonResultWithRecordObjectVM jsonResultWithRecord   = new JsonResultWithRecordObjectVM(new object() { });
            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyLocationManagement/UpdateAgencylocation";

                UpdateAgencyLocationPVM updateAgencyLocationPVM = new UpdateAgencyLocationPVM()
                {
                    AgencyLocationVM = agencyLocationVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateAgencyLocation(updateAgencyLocationPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecord = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecord != null)
                    {
                        if (jsonResultWithRecord.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "تعیین موقعیت انجام شد",
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
                Message = "خطا"
            });
        }
    }
}

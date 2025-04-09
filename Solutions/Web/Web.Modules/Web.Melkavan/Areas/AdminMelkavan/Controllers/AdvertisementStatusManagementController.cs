using ApiCallers.MelkavanApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Console;
using VM.Melkavan;
using VM.Melkavan.PVM.Melkavan.Advertisement;
using VM.Melkavan.VM.Melkavan;
using VM.PVM.Melkavan;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class AdvertisementStatusManagementController : ExtraAdminController
    {
        public AdvertisementStatusManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "مدیریت تعیین وضعیت آگهی ها";
            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAdvertisementWithAdvertisementId(long id,string? type)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string ServiceUrl = melkavanApiUrl + "/api/AdvertisementManagement/GetAdvertisementWithAdvertisementId";

                GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM =
                    new GetAdvertisementWithAdvertisementIdPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementStatusManagement", "GetAdvertisementWithAdvertisementId", this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        AdvertisementId = id,
                        Type = type,
                        UserId = this.userId.Value
                    };
                responseApiCaller = new MelkavanApiCaller(ServiceUrl).GetAdvertisementWithAdvertisementId(getAdvertisementWithAdvertisementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var Record = jObject.ToObject<AdvertisementVM>();


                            return Json(new
                            {
                                jsonResultWithRecordObjectVM.Result,
                                Record
                            });


                        }
                    }
                }
            }
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfAdvertisementsForStatus(int jtStartIndex = 0,
            int jtPageSize = 10,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {

                string serviceUrl = melkavanApiUrl + "/api/AdvertisementStatusManagement/GetListOfAdvertisementsForStatus";

                GetListOfPropertiesAdvanceSearchPVM getListOfPropertiesAdvanceSearchPVM = new GetListOfPropertiesAdvanceSearchPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementStatusManagement", "GetListOfAdvertisementsForStatus", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    jtSorting = jtSorting,
                    ThisUserId = this.userId.Value
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfPropertiesAdvanceSearch(getListOfPropertiesAdvanceSearchPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<AdvertisementStatusListVM>>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordsObjectVM.Result,
                                Records = records,
                                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> UpdateAdvertisementStatusId(AdvertisementVM advertisementVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string ServiceUrl = melkavanApiUrl + "/api/AdvertisementStatusManagement/UpdateAdvertisementStatusId";

                UpdateAdvertisementStatusPVM updateAdvertisementStatusPVM =
                    new UpdateAdvertisementStatusPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementStatusManagement", "UpdateAdvertisementStatusId", this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        AdvertisementId = advertisementVM.AdvertisementId,
                        Type = advertisementVM.RecordType,
                        StatusId = advertisementVM.StatusId,
                        RejectionReason = advertisementVM.RejectionReason,
                        UserId = this.userId.Value
                    };
                responseApiCaller = new MelkavanApiCaller(ServiceUrl).UpdateAdvertisementStatus(updateAdvertisementStatusPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK"
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> UpdateAdvertisementTagId(AdvertisementVM advertisementVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string ServiceUrl = melkavanApiUrl + "/api/AdvertisementStatusManagement/UpdateAdvertisementTagId";

                UpdateAdvertisementTagIdPVM updateAdvertisementTagIdPVM =
                    new UpdateAdvertisementTagIdPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementStatusManagement", "UpdateAdvertisementTagId", this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        AdvertisementId = advertisementVM.AdvertisementId,
                        Type = advertisementVM.RecordType,
                        TagId = advertisementVM.AdvertisementDetailsVM.TagId,
                        UserId = this.userId.Value
                    };
                responseApiCaller = new MelkavanApiCaller(ServiceUrl).UpdateAdvertisementTagId(updateAdvertisementTagIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK"
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

    }
}

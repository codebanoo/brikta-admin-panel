using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
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
using System.Collections.Generic;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class MyPropertiesFeaturesManagementController : ExtraAdminController
    {
        public MyPropertiesFeaturesManagementController(IHostEnvironment _hostEnvironment,
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



        public IActionResult UpdateMyPropertyFeatures(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "MyPropertiesManagement");

            ViewData["Title"] = "امکانات";

            MyPropertiesVM propertiesVM = new MyPropertiesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/GetMyPropertyWithMyPropertyId";

                GetMyPropertyWithMyPropertyIdPVM getPropertyWithPropertyIdPVM = new GetMyPropertyWithMyPropertyIdPVM()
                {
                    PropertyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyWithMyPropertyId(getPropertyWithPropertyIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertiesVM>();


                            if (record != null)
                            {
                                propertiesVM = record;


                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["MyPropertiesVM"] = propertiesVM;


            MyPropertyFeaturesValuesVM propertyFeaturesValuesVM = new MyPropertyFeaturesValuesVM();

            try
            {
                serviceUrl = teniacoApiUrl + "/api/MyPropertiesFeaturesManagement/GetMyPropertyFeaturesValues";

                GetMyPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM = new GetMyPropertyFeaturesValuesPVM()
                {
                    PropertyId = propertiesVM.PropertyId,
                    PropertyTypeId = propertiesVM.PropertyTypeId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyFeaturesValues(getPropertyFeaturesValuesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertyFeaturesValuesVM>();


                            if (record != null)
                            {
                                propertyFeaturesValuesVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["MyPropertyFeaturesValuesVM"] = propertyFeaturesValuesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MyPropertiesManagement/Index/";
            }

            return View("Update");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateMyPropertyFeatures(List<FeaturesValuesVM> featuresValuesVMList, int propertyId)
        {
            try
            {
                JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

                foreach (FeaturesValuesVM featuresValuesVM in featuresValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.IsActivated = true;
                    featuresValuesVM.IsDeleted = false;

                    featuresValuesVM.PropertyId = propertyId;

                }

                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFeaturesManagement/UpdateMyPropertyFeatures";

                UpdateMyPropertyFeaturesPVM updatePropertyFeaturesPVM = new UpdateMyPropertyFeaturesPVM()
                {
                    FeaturesValuesVMList = featuresValuesVMList,
                    UserId = this.userId.Value,
                    PropertyId = propertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateMyPropertyFeatures(updatePropertyFeaturesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "بروز رسانی انجام شد"
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

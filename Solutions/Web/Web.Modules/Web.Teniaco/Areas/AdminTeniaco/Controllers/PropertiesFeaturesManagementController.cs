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
    public class PropertiesFeaturesManagementController : ExtraAdminController
    {
        public PropertiesFeaturesManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult UpdatePropertyFeatures(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "PropertiesManagement");

            ViewData["Title"] = "امکانات";

            PropertiesVM propertiesVM = new PropertiesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetPropertyWithPropertyId";

                GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM = new GetPropertyWithPropertyIdPVM()
                {
                    PropertyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetPropertyWithPropertyId(getPropertyWithPropertyIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<PropertiesVM>();


                            if (record != null)
                            {
                                propertiesVM = record;

                                #region Fill UserCreatorName

                                //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                //foreach (var record in records)
                                //{
                                //    if (record.UserIdCreator.HasValue)
                                //    {
                                //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                //        if (customUser != null)
                                //        {
                                //            record.UserCreatorName = customUser.UserName;

                                //            if (!string.IsNullOrEmpty(customUser.Name))
                                //                record.UserCreatorName += " " + customUser.Name;

                                //            if (!string.IsNullOrEmpty(customUser.Family))
                                //                record.UserCreatorName += " " + customUser.Family;
                                //        }
                                //    }
                                //}

                                //statesVMList = records;

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["PropertiesVM"] = propertiesVM;

            //if (ViewData["ElementTypesList"] == null)
            //{
            //    List<ElementTypesVM> elementTypesVMList = new List<ElementTypesVM>();

            //    try
            //    {
            //        string serviceUrl = teniacoApiUrl + "/api/ElementTypesManagement/GetAllElementTypesList";

            //        responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllElementTypesList();

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    var records = jArray.ToObject<List<ElementTypesVM>>();

            //                    if (records.Count > 0)
            //                    {
            //                        #region Fill UserCreatorName

            //                        #endregion
            //                    }

            //                    elementTypesVMList = records;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["ElementTypesList"] = elementTypesVMList;
            //}

            PropertyFeaturesValuesVM propertyFeaturesValuesVM = new PropertyFeaturesValuesVM();

            try
            {
                serviceUrl = teniacoApiUrl + "/api/PropertiesFeaturesManagement/GetPropertyFeaturesValues";

                GetPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM = new GetPropertyFeaturesValuesPVM()
                {
                    PropertyId = propertiesVM.PropertyId,
                    PropertyTypeId = propertiesVM.PropertyTypeId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetPropertyFeaturesValues(getPropertyFeaturesValuesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<PropertyFeaturesValuesVM>();


                            if (record != null)
                            {
                                propertyFeaturesValuesVM = record;

                                #region Fill UserCreatorName

                                //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                //foreach (var record in records)
                                //{
                                //    if (record.UserIdCreator.HasValue)
                                //    {
                                //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                //        if (customUser != null)
                                //        {
                                //            record.UserCreatorName = customUser.UserName;

                                //            if (!string.IsNullOrEmpty(customUser.Name))
                                //                record.UserCreatorName += " " + customUser.Name;

                                //            if (!string.IsNullOrEmpty(customUser.Family))
                                //                record.UserCreatorName += " " + customUser.Family;
                                //        }
                                //    }
                                //}

                                //statesVMList = records;

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["PropertyFeaturesValuesVM"] = propertyFeaturesValuesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index/";
            }

            return View("Update");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdatePropertyFeatures(List<FeaturesValuesVM> featuresValuesVMList, int propertyId)
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

                    //featuresValuesVM.FeaturesVM = new FeaturesVM();
                    //featuresValuesVM.PropertiesVM = new PropertiesVM();
                    //featuresValuesVM.PropertyTypesVM = new PropertyTypesVM();
                }

                string serviceUrl = teniacoApiUrl + "/api/PropertiesFeaturesManagement/UpdatePropertyFeatures";

                UpdatePropertyFeaturesPVM updatePropertyFeaturesPVM = new UpdatePropertyFeaturesPVM()
                {
                    FeaturesValuesVMList = featuresValuesVMList,
                    UserId = this.userId.Value,
                    PropertyId = propertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //IsActivated = true,
                    //IsDeleted = false,

                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdatePropertyFeatures(updatePropertyFeaturesPVM);

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

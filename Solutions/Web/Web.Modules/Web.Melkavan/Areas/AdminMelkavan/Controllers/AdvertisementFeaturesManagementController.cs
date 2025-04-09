using ApiCallers.MelkavanApiCaller;
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
using System.Linq;
using VM.Base;
using VM.Melkavan;
using VM.PVM.Melkavan;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class AdvertisementFeaturesManagementController : ExtraAdminController
    {
        public AdvertisementFeaturesManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "مدیریت امکانات";




            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (ViewData["PropertyTypesVMs"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/PropertyTypesManagement/GetAllPropertyTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertyTypesList(getAllPropertyTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<PropertyTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertyTypesVMs"] = propertyTypesVMList;
            }


            return View("Index");

        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetFeaturesListByPropertyTypeId(
           int PropertyTypeId = 0)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            ManageAdvertisementFeaturesValuesVM manageAdvertisementFeaturesValuesVM = new ManageAdvertisementFeaturesValuesVM();

            if (PropertyTypeId.Equals(0))
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "خطا"
                });
            }

            try
            {


                string serviceUrl = melkavanApiUrl + "/api/AdvertisementFeaturesManagement/GetListOfFeaturesByPropertyTypeId";

                GetListOfFeaturesByPropertyTypeIdPVM getListOfFeaturesByPropertyTypeIdPVM =
                    new GetListOfFeaturesByPropertyTypeIdPVM()
                    {
                        PropertyTypeId = PropertyTypeId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetListOfFeaturesByPropertyTypeId(getListOfFeaturesByPropertyTypeIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {

                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            manageAdvertisementFeaturesValuesVM = jObject.ToObject<ManageAdvertisementFeaturesValuesVM>();


                            if (manageAdvertisementFeaturesValuesVM != null)
                                return Json(new
                                {
                                    jsonResultWithRecordObjectVM.Result,
                                    jsonResultWithRecordObjectVM.Record
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





        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateAdvertisementFeatures(UpdateAdvertisementFeaturesPVM updateAdvertisementFeaturesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = melkavanApiUrl + "/api/AdvertisementFeaturesManagement/UpdateAdvertisementFeatures";

                updateAdvertisementFeaturesPVM.UserId = this.userId.Value;
                updateAdvertisementFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);
                //updateAdvertisementFeaturesPVM.ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId);

                responseApiCaller = new MelkavanApiCaller(serviceUrl).UpdateAdvertisementFeatures(updateAdvertisementFeaturesPVM);

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
                                Message = "ویرایش انجام شد",
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


        //SINA CODES - HANDLING MELKAVAN FEATURES IN ADMIN TENIACO (GET)
        public IActionResult UpdateAdvertisementFeatures(int Id = 0)
        {
            if (Id.Equals(0))
                return Redirect("/AdminTeniaco/PropertiesManagement/Index");

            ViewData["Title"] = "امکانات";

            AdvertisementVM advertisementVM = new AdvertisementVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = melkavanApiUrl + "/api/AdvertisementManagement/GetAdvertisementWithAdvertisementId";

                GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM = new GetAdvertisementWithAdvertisementIdPVM()
                {
                    AdvertisementId = Id,
                    Type = "Advertisement",
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementManagement", "GetAdvertisementWithAdvertisementId", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAdvertisementWithAdvertisementId(getAdvertisementWithAdvertisementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<AdvertisementVM>();


                            if (record != null)
                            {
                                advertisementVM = record;

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

            ViewData["AdvertisementVM"] = advertisementVM;

            #region Comments


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
            #endregion
            AdvertisementFeaturesValuesVM advertisementFeaturesValuesVM = new AdvertisementFeaturesValuesVM();

            try
            {
                serviceUrl = melkavanApiUrl + "/api/AdvertisementFeatureValuesManagement/GetAdvertisementFeaturesValues";

                GetAdvertisementFeaturesValuesPVM getAdvertisementFeaturesValuesPVM = new GetAdvertisementFeaturesValuesPVM()
                {
                    AdvertisementId = advertisementVM.AdvertisementId,
                    PropertyTypeId = advertisementVM.PropertyTypeId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementFeatureValuesManagement", "GetAdvertisementFeaturesValues", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAdvertisementFeaturesValues(getAdvertisementFeaturesValuesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<AdvertisementFeaturesValuesVM>();


                            if (record != null)
                            {
                                advertisementFeaturesValuesVM = record;

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

            ViewData["AdvertisementFeaturesValuesVM"] = advertisementFeaturesValuesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index/";
            }

            return View("Update");
        }

        
        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateAdvertisementFeaturesForMelkavanInnerDashBoard(List<AdvertisementFeaturesValuesVM> featuresValuesVMList, int advertisementId)
        {
            try
            {
                JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

                foreach (AdvertisementFeaturesValuesVM featuresValuesVM in featuresValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.IsActivated = true;
                    featuresValuesVM.IsDeleted = false;

                    featuresValuesVM.AdvertisementId = advertisementId;

                    //featuresValuesVM.FeaturesVM = new FeaturesVM();
                    //featuresValuesVM.PropertiesVM = new PropertiesVM();
                    //featuresValuesVM.PropertyTypesVM = new PropertyTypesVM();
                }

                string serviceUrl = melkavanApiUrl + "/api/AdvertisementFeatureValuesManagement/UpdateAdvertisementFeatureValues";

                UpdateAdvertisementFeatureValuesPVM updateAdvertisementFeatureValuesPVM = new UpdateAdvertisementFeatureValuesPVM()
                {
                    AdvertisementFeaturesValuesVMList = featuresValuesVMList.Where(fv => fv.FeatureValue!=null).ToList(),
                    UserId = this.userId.Value,
                    AdvertisementId = advertisementId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //IsActivated = true,
                    //IsDeleted = false,

                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).UpdateAdvertisementFeatureValues(updateAdvertisementFeatureValuesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

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

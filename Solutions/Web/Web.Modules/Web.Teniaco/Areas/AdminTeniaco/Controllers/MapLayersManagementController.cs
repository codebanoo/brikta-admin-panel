using ApiCallers.PublicApiCaller;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class MapLayersManagementController : ExtraAdminController
    {
        public MapLayersManagementController(IHostEnvironment _hostEnvironment,
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



        #region MapLayer

        public IActionResult Index(int id = 0)
        {

            if (id.Equals(0))
            {
                return RedirectToAction("Index", "MapLayerCategoriesManagement");
            }
            ViewData["Title"] = "لیست نقشه ها ";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["StatesList"] == null)
            {
                List<StatesVM> statesVMList = new List<StatesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

                    GetListOfStatesPVM getListOfStatesPVM = new GetListOfStatesPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getListOfStatesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                statesVMList = jArray.ToObject<List<StatesVM>>();


                                if (statesVMList != null)
                                    if (statesVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["StatesList"] = statesVMList;
            }

            if (ViewData["CitiesList"] == null)
            {
                List<CitiesVM> citiesVMList = new List<CitiesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/CitiesManagement/GetAllCitiesList";

                    GetAllCitiesListPVM getAllCitiesListPVM = new GetAllCitiesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllCitiesList(getAllCitiesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                citiesVMList = jArray.ToObject<List<CitiesVM>>();


                                if (citiesVMList != null)
                                    if (citiesVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["CitiesList"] = citiesVMList;
            }

            if (ViewData["ZonesList"] == null)
            {
                List<ZonesVM> zonesVMList = new List<ZonesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetAllZonesList";

                    GetAllZonesListPVM getAllZonesListPVM = new GetAllZonesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllZonesList(getAllZonesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                zonesVMList = jArray.ToObject<List<ZonesVM>>();


                                if (zonesVMList != null)
                                    if (zonesVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ZonesList"] = zonesVMList;
            }

            if (ViewData["DistrictsList"] == null)
            {
                List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/DistrictsManagement/GetAllDistrictsList";

                    GetAllDistrictsListPVM getAllDistrictsListPVM = new GetAllDistrictsListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllDistrictsList(getAllDistrictsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                districtsVMList = jArray.ToObject<List<DistrictsVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DistrictsList"] = districtsVMList;
            }

            if (ViewData["MapLayerCategory"] == null)
            {
                MapLayerCategoriesVM mapLayerCategoryVMList = new MapLayerCategoriesVM();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetMapLayerCategoryWithMapLayerCategoryId";

                    GetMapLayerCategoryWithMapLayerCategoryIdPVM getMapLayerCategoryWithMapLayerCategoryIdPVM = new GetMapLayerCategoryWithMapLayerCategoryIdPVM()
                    {
                        MapLayerCategoryId = id,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        // this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMapLayerCategoryWithMapLayerCategoryId(getMapLayerCategoryWithMapLayerCategoryIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<MapLayerCategoriesVM>();

                                if (record != null)
                                {
                                    mapLayerCategoryVMList = record;


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["MapLayerCategory"] = mapLayerCategoryVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MapLayerCategoriesManagement/Index/";
            }

            return View("Index");
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllMapLayersList(
            int? mapLayerCategoryId = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetAllMapLayersList";

                GetAllMapLayersListPVM getAllMapLayersListPVM = new GetAllMapLayersListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                   MapLayerCategoryId = mapLayerCategoryId,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMapLayersList(getAllMapLayersListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MapLayersVM>>();

                            if (records.Count > 0)
                            {
                                var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                foreach (var record in records)
                                {
                                    if (record.UserIdCreator.HasValue)
                                    {
                                        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                        if (customUser != null)
                                        {
                                            record.UserCreatorName = customUser.UserName;

                                            if (!string.IsNullOrEmpty(customUser.Name))
                                                record.UserCreatorName += " " + customUser.Name;

                                            if (!string.IsNullOrEmpty(customUser.Family))
                                                record.UserCreatorName += " " + customUser.Family;
                                        }
                                    }
                                }
                            }

                            #endregion

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
            catch (Exception)
            {

                throw;
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfMapLayers(
            int jtStartIndex = 0,
            int jtPageSize = 10,
            int? mapLayerCategoryId = null,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetListOfMapLayers";

                GetListOfMapLayersPVM getListOfMapLayersPVM = new GetListOfMapLayersPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    MapLayerCategoryId = mapLayerCategoryId,
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfMapLayers(getListOfMapLayersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MapLayersVM>>();

                            if (records.Count > 0)
                            {
                                var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                foreach (var record in records)
                                {
                                    if (record.UserIdCreator.HasValue)
                                    {
                                        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                        if (customUser != null)
                                        {
                                            record.UserCreatorName = customUser.UserName;

                                            if (!string.IsNullOrEmpty(customUser.Name))
                                                record.UserCreatorName += " " + customUser.Name;

                                            if (!string.IsNullOrEmpty(customUser.Family))
                                                record.UserCreatorName += " " + customUser.Family;
                                        }
                                    }
                                }
                            }

                            #endregion

                            return Json(new
                            {
                                Result = jsonResultWithRecordsObjectVM.Result,
                                Records = records,//jsonResultWithRecordsObjectVM.Records,
                                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {

                throw;
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToMapLayers(MapLayersVM mapLayersVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                mapLayersVM.CreateEnDate = DateTime.Now;
                mapLayersVM.CreateTime = PersianDate.TimeNow;
                mapLayersVM.UserIdCreator = this.domainsSettings.UserIdCreator.Value;
                mapLayersVM.IsActivated = true;
                mapLayersVM.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/AddToMapLayers";

                    AddToMapLayersPVM addToMapLayersPVM = new AddToMapLayersPVM()
                    {
                        MapLayersVM = mapLayersVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMapLayers(addToMapLayersPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<MapLayersVM>();

                                if (record != null)
                                {
                                    mapLayersVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = mapLayersVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateMapLayer"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateMapLayers(MapLayersVM mapLayersVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                mapLayersVM.EditEnDate = DateTime.Now;
                mapLayersVM.EditTime = PersianDate.TimeNow;
                mapLayersVM.UserIdEditor = this.userId.Value;



                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/UpdateMapLayers";

                    UpdateMapLayersPVM updateMapLayersPVM = new UpdateMapLayersPVM()
                    {
                        MapLayersVM = mapLayersVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //    this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateMapLayers(updateMapLayersPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            mapLayersVM = jObject.ToObject<MapLayersVM>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = mapLayersVM,
                            });
                        }
                        else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateMapLayer"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "رکورد تکراری است"
                            });
                        }
                    }

                }
                return Json(new
                {
                    Result = jsonResultWithRecordObjectVM.Result,
                    Record = mapLayersVM,
                });
            }
            catch (Exception)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationMapLayers(int mapLayerId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/ToggleActivationMapLayers";

                ToggleActivationMapLayersPVM toggleActivationMapLayersPVM =
                    new ToggleActivationMapLayersPVM()
                    {
                        MapLayerId = mapLayerId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationMapLayers(toggleActivationMapLayersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
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
        public IActionResult TemporaryDeleteMapLayers(int mapLayerId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/TemporaryDeleteMapLayers";

                TemporaryDeleteMapLayersPVM temporaryDeleteMapLayersPVM =
                    new TemporaryDeleteMapLayersPVM()
                    {
                        MapLayerId = mapLayerId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteMapLayers(temporaryDeleteMapLayersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
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


        [HttpPost]
        [AjaxOnly]
        public IActionResult CompleteDeleteMapLayers(int mapLayerId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/CompleteDeleteMapLayers";

                CompleteDeleteMapLayersPVM completeDeleteMapLayersPVM =
                  new CompleteDeleteMapLayersPVM()
                  {
                      MapLayerId = mapLayerId,
                      //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                      //  this.domainsSettings.DomainSettingId),
                      ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                      UserId = this.userId.Value
                  };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteMapLayers(completeDeleteMapLayersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult CompleteDeleteMapLayerIds(List<int> Ids)
        {
            if (Ids.Count == 0)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "رکوردی انتخاب نشده است"
                });
            }

            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/CompleteDeleteMapLayersIds";

                CompleteDeleteMapLayersIdsPVM completeDeleteMapLayersIdsPVM =
                  new CompleteDeleteMapLayersIdsPVM()
                  {
                      MapLayerIds = Ids,
                      //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                      //  this.domainsSettings.DomainSettingId),
                      ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                      UserId = this.domainsSettings.UserIdCreator.Value
                  };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteMapLayersIds(completeDeleteMapLayersIdsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                        if (jsonResultObjectVM != null)
                        {
                            if (jsonResultObjectVM.Result.Equals("OK"))
                            {
                                return Json(new { Result = "OK" });
                            }
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

        #endregion

        #region MapView
        public IActionResult ReDrawOnTheMap(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "اصلاح لایه روی نقشه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MapLayerCategoriesList"] == null)
            {
                List<MapLayerCategoriesVM> mapLayerCategoriesVMList = new List<MapLayerCategoriesVM>();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";

                    GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM = new GetAllMapLayerCategoriesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMapLayerCategoriesList(getAllMapLayerCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                mapLayerCategoriesVMList = jArray.ToObject<List<MapLayerCategoriesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["MapLayerCategoriesList"] = mapLayerCategoriesVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MapLayersManagement/Index";
            }
            MapLayersVM mapLayersVM = new MapLayersVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetMapLayerWithMapLayerId";

                GetMapLayerWithMapLayerIdPVM getMapLayerWithMapLayerIdPVM = new GetMapLayerWithMapLayerIdPVM()
                {
                    MapLayerId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMapLayerWithMapLayerId(getMapLayerWithMapLayerIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MapLayersVM>();


                            if (record != null)
                            {
                                mapLayersVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            dynamic expando = new ExpandoObject();
            expando = mapLayersVM;

            return View("Update", expando);
        }
        public IActionResult FeaturesOnTheMap(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "انتقال و چرخش لایه روی نقشه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MapLayersManagement/Index";
            }
            MapLayersVM mapLayersVM = new MapLayersVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetMapLayerWithMapLayerId";

                GetMapLayerWithMapLayerIdPVM getMapLayerWithMapLayerIdPVM = new GetMapLayerWithMapLayerIdPVM()
                {
                    MapLayerId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMapLayerWithMapLayerId(getMapLayerWithMapLayerIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MapLayersVM>();


                            if (record != null)
                            {
                                mapLayersVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            dynamic expando = new ExpandoObject();
            expando = mapLayersVM;

            return View("Update", expando);
        }

        public IActionResult ShowAllMapLayers(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "نمایش تمامی های لایه های نقشه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MapLayersManagement/Index";
            }
            List<MapLayersVM> mapLayersVM = new List<MapLayersVM>();

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetAllMapLayersList";

                GetAllMapLayersListPVM getAllMapLayersListPVM = new GetAllMapLayersListPVM()
                {
                    MapLayerCategoryId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMapLayersList(getAllMapLayersListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MapLayersVM>>();

                            if (records != null)
                            {
                                mapLayersVM = records;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            dynamic expando = new ExpandoObject();
            expando = mapLayersVM;

            return View("Index", expando);
        }

        #endregion

        #region MapLayerFiles

        public IActionResult AddToMapLayerFiles(int id)
        {
            ViewData["Title"] = "آپلود فایل GeoJson";

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["MapLayerCategoriesList"] == null)
            {
                MapLayerCategoriesVM mapLayerCategoriesVMList = new MapLayerCategoriesVM();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetMapLayerCategoryWithMapLayerCategoryId";

                    GetMapLayerCategoryWithMapLayerCategoryIdPVM getMapLayerCategoryWithMapLayerCategoryIdPVM = new GetMapLayerCategoryWithMapLayerCategoryIdPVM()
                    {
                        MapLayerCategoryId = id,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        // this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMapLayerCategoryWithMapLayerCategoryId(getMapLayerCategoryWithMapLayerCategoryIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<MapLayerCategoriesVM>();

                                if (record != null)
                                {
                                    mapLayerCategoriesVMList = record;


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["MapLayerCategoriesList"] = mapLayerCategoriesVMList;
            }


            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MapLayersManagement/Index";
            }

            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToMapLayerFiles(List<PropertyFileUploadPVM> mapLayerFileUploadPVMList, int mapLayerId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            List<MapLayerFilesVM> mapLayerFilesVMList = new List<MapLayerFilesVM>();

            try
            {

                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/AddToMapLayerFiles";

                AddToMapLayerFilesPVM addToMapLayerFilesPVM = new AddToMapLayerFilesPVM()
                {
                    MapLayerFilesVMList = mapLayerFilesVMList,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMapLayerFiles(addToMapLayerFilesPVM);

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
                                Message = "آپلود انجام شد",
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


        [HttpPost]
        [AjaxOnly]
        public ActionResult UploadJsonFile(UploadGeographicDataPVM uploadGeographicDataPVM)
        {

            if (uploadGeographicDataPVM.File.Length > 0)
            {

                var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadGeographicDataPVM.File.CopyTo(stream);
                }
                string text = System.IO.File.ReadAllText(filePath);
                //text = text.Replace("/", ".");

                var geoJsonDataVm = JsonConvert.DeserializeObject<GeoJsonDataVm>(
                                        text, new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore
                                        });

                List<string> _result = new List<string>();
                foreach (var item in geoJsonDataVm?.features[0].geometry.coordinates)
                {
                    var it = item;
                    //string res = JsonConvert.SerializeObject(item.geometry.coordinates);
                    string res = JsonConvert.SerializeObject(it[0]);
                    _result.Add(res);
                }
                string Err = AddToMaplayerJsonData(uploadGeographicDataPVM.MapLayerCategoryId, _result, uploadGeographicDataPVM);
                return Json(new
                {
                    Result = Err.Equals("") ? "OK" : "ERROR",
                    Message = Err
                });
            }
            return Json(new
            {
                Result = "ERROR",
                Message = "فایل  ارسالی معتبر نیست"
            });

        }
        private string AddToMaplayerJsonData(int _MapLayerCategoryId, List<string> _StrPolygon, UploadGeographicDataPVM uploadGeographicDataPVM)
        {


            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            MapLayersVM mapLayersVM = new MapLayersVM();
            try
            {
                mapLayersVM.CreateEnDate = DateTime.Now;
                mapLayersVM.CreateTime = PersianDate.TimeNow;
                mapLayersVM.UserIdCreator = this.domainsSettings.UserIdCreator.Value;
                mapLayersVM.IsActivated = true;
                mapLayersVM.IsDeleted = false;
                mapLayersVM.MapLayerCategoryId = _MapLayerCategoryId;
                mapLayersVM.StrPolygon = string.Empty;
                //mapLayersVM.CityId = uploadGeographicDataPVM.CityId;
                //mapLayersVM.ZoneId = uploadGeographicDataPVM.ZoneId;
                mapLayersVM.DistrictId = uploadGeographicDataPVM.DistrictId;

                AddToMapLayersJsonDataPVM addToMapLayersJsonDataPVM = new AddToMapLayersJsonDataPVM()
                {
                    MapLayersVM = mapLayersVM,
                    StrPolygonList = _StrPolygon,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/AddToMapLayersWithJsonData";
                responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMapLayersWithJsonData(addToMapLayersJsonDataPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                        }
                    }
                }
                else return "خطا زخ داده است";


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        #endregion
    }
}

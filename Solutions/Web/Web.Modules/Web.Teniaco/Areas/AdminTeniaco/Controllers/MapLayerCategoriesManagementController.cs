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
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;

using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class MapLayerCategoriesManagementController : ExtraAdminController
    {
        public MapLayerCategoriesManagementController(IHostEnvironment _hostEnvironment,
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

        #region MapLayerCategories

        public IActionResult Index()
        {
            ViewData["Title"] = "لیست دسته بندی لایه های نقشه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MapLayerCategoriesList"] == null)
            {
                List<MapLayerCategoriesVM> mapLayerCategoriesVMList = new List<MapLayerCategoriesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";

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

            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllMapLayerCategoriesList(
           string mapLayerCategoryTitle = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";

                GetAllMapLayerCategoriesListPVM getAllInvestorsListPVM = new GetAllMapLayerCategoriesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    
                    MapLayerCategoryTitle = mapLayerCategoryTitle,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMapLayerCategoriesList(getAllInvestorsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MapLayerCategoriesVM>>();

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
        public IActionResult GetListOfMapLayerCategories(int EvaluationId,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string mapLayerCategoryTitle = "",
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetListOfMapLayerCategories";

                GetListOfMapLayerCategoriesPVM getListOfMapLayerCategoriesPVM = new GetListOfMapLayerCategoriesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    MapLayerCategoryTitle = (!string.IsNullOrEmpty(mapLayerCategoryTitle) ? mapLayerCategoryTitle.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfMapLayerCategories(getListOfMapLayerCategoriesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MapLayerCategoriesVM>>();

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



        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToMapLayerCategories(MapLayerCategoriesVM mapLayerCategoriesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                mapLayerCategoriesVM.CreateEnDate = DateTime.Now;
                mapLayerCategoriesVM.CreateTime = PersianDate.TimeNow;
                mapLayerCategoriesVM.UserIdCreator = this.domainsSettings.UserIdCreator.Value;
                mapLayerCategoriesVM.IsActivated = true;
                mapLayerCategoriesVM.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/AddToMapLayerCategories";

                    AddToMapLayerCategoriesPVM addToMapLayerCategoriesPVM = new AddToMapLayerCategoriesPVM() 
                    {
                        MapLayerCategoriesVM = mapLayerCategoriesVM ,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMapLayerCategories(addToMapLayerCategoriesPVM);


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
                                    mapLayerCategoriesVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = mapLayerCategoriesVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateMapLayerCategory"))
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
        public IActionResult UpdateMapLayerCategories(MapLayerCategoriesVM mapLayerCategoriesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                mapLayerCategoriesVM.EditEnDate = DateTime.Now;
                mapLayerCategoriesVM.EditTime = PersianDate.TimeNow;
                mapLayerCategoriesVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/UpdateMapLayerCategories";

                    UpdateMapLayerCategoriesPVM updateMapLayerCategoriesPVM = new UpdateMapLayerCategoriesPVM()
                    {
                        MapLayerCategoriesVM = mapLayerCategoriesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateMapLayerCategories(updateMapLayerCategoriesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            mapLayerCategoriesVM = jObject.ToObject<MapLayerCategoriesVM>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = mapLayerCategoriesVM,
                            });
                        }
                        else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateMapLayerCategory"))
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
                    Record = mapLayerCategoriesVM,
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
        public IActionResult ToggleActivationMapLayerCategories(int mapLayerCategoryId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/ToggleActivationMapLayerCategories";

                ToggleActivationMapLayerCategoriesPVM toggleActivationMapLayerCategoriesPVM =
                    new ToggleActivationMapLayerCategoriesPVM()
                    {
                        MapLayerCategoryId = mapLayerCategoryId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.domainsSettings.UserIdCreator.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationMapLayerCategories(toggleActivationMapLayerCategoriesPVM);

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
        public IActionResult TemporaryDeleteMapLayerCategories(int mapLayerCategoryId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/TemporaryDeleteMapLayerCategories";

                TemporaryDeleteMapLayerCategoriesPVM temporaryDeleteMapLayerCategoriesPVM =
                    new TemporaryDeleteMapLayerCategoriesPVM()
                    {
                        MapLayerCategoryId = mapLayerCategoryId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.domainsSettings.UserIdCreator.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteMapLayerCategories(temporaryDeleteMapLayerCategoriesPVM);

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

        #endregion

        #region CompareMapLayers

        public IActionResult CompareMapLayers()
        {
            ViewData["Title"] = "لایه ها";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MapLayerCategoriesList"] == null)
            {
                List<MapLayerCategoriesVM> mapLayerCategoriesVMList = new List<MapLayerCategoriesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";

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

            if (ViewData["MapLayersList"] == null)
            {
                List<MapLayersVM> mapLayersVM = new List<MapLayersVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetAllMapLayersList";

                    GetAllMapLayersListPVM getAllMapLayersListPVM = new GetAllMapLayersListPVM()
                    {
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

                ViewData["MapLayersList"] = mapLayersVM;
            }

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


            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllMapLayersList(int mapLayerCategoryId)
        {
            
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayersManagement/GetAllMapLayersList";

                GetAllMapLayersListPVM getAllMapLayersListPVM = new GetAllMapLayersListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
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
                            return Json(new
                            {
                                jsonResultWithRecordsObjectVM.Result,
                                jsonResultWithRecordsObjectVM.Records,
                                jsonResultWithRecordsObjectVM.TotalRecordCount
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
        public IActionResult GetZoneWithZoneId(int zoneId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetZoneWithZoneId";

                GetZoneWithZoneIdPVM getZoneWithZoneIdPVM  = new GetZoneWithZoneIdPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ZoneId = zoneId,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetZoneWithZoneId(getZoneWithZoneIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
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
        public IActionResult GetDistrictWithDistrictId(int districtId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictsManagement/GetDistrictWithDistrictId";

                GetDistrictWithDistrictIdPVM getDistrictWithDistrictIdPVM = new GetDistrictWithDistrictIdPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    DistrictId = districtId,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictWithDistrictId(getDistrictWithDistrictIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
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
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        #endregion

        #region TreeMapLayer

        [HttpPost]
        [AjaxOnly]
        public IActionResult CompleteDeleteMapLayerCategories(int mapLayerCategoryId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/CompleteDeleteMapLayerCategories";

                CompleteDeleteMapLayerCategoriesPVM completeDeleteMapLayerCategoriesPVM =
                    new CompleteDeleteMapLayerCategoriesPVM()
                    {
                        MapLayerCategoryId = mapLayerCategoryId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.domainsSettings.UserIdCreator.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteMapLayerCategories(completeDeleteMapLayerCategoriesPVM);

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

        public IActionResult ShowTreeMapLayer()
        {
            ViewData["Title"] = "ساختار نقشه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MapLayerCatList"] == null)
            {
                List<EvaluationCategoriesVM> evaluationCategoriesVMList = new List<EvaluationCategoriesVM>();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";
                    GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM = new GetAllMapLayerCategoriesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
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
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<MapLayerCategoriesVM>>();
                                if (records.Count > 0)
                                {
                                    var finall = GetHierarchicalData(records);
                                    ViewData["MapLayerCatList"] = finall;
                                }
                            }
                        }
                    }
                }
                catch
                { }
            }
            return View("Index");
        }

        private List<MapLayerCategoriesNodeWithDataVM> result = new List<MapLayerCategoriesNodeWithDataVM>();

        public List<MapLayerCategoriesNodeWithDataVM> GetHierarchicalData(List<MapLayerCategoriesVM> list)
        {
            result = list.Select(s => new MapLayerCategoriesNodeWithDataVM
            {
                id = s.MapLayerCategoryId.ToString(),
                parent = s.ParentMapLayerCategoryId == null || s.ParentMapLayerCategoryId == 0 ? "#" : s.ParentMapLayerCategoryId.ToString(),

                title = s.MapLayerCategoryTitle
            }).ToList();

            List<MapLayerCategoriesNodeWithDataVM> res1 = new List<MapLayerCategoriesNodeWithDataVM>();
            foreach (var item in result.Where(w => w.parent == "#"))
            {
                item.folder = true;
                item.children = GetNodesCategory(item.id.ToString());
                res1.Add(item);
            }

            return res1;
        }

        private List<MapLayerCategoriesNodeWithDataVM> GetNodesCategory(string id)
        {
            var res = result.Where(w => w.parent == id).ToList();
            if (res.Any())
                foreach (var item in res)
                {
                    item.children = GetNodesCategory(item.id);
                }
            else
                res = null;
            return res;
        }

        #endregion

        #region MapPriceLayer

        public IActionResult PriceLayerIndex()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["PropertyTypesList"] == null)
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


                                if (propertyTypesVMList != null)
                                    if (propertyTypesVMList.Count > 0)
                                    {
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
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM = new GetAllTypeOfUsesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/TypeOfUsesManagement/GetAllTypeOfUsesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllTypeOfUsesList(getAllTypeOfUsesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                typeOfUsesVMList = jArray.ToObject<List<TypeOfUsesVM>>();


                                if (typeOfUsesVMList != null)
                                    if (typeOfUsesVMList.Count > 0)
                                    {
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
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["TypeOfUsesList"] = typeOfUsesVMList;
            }

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

            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfPropertiesPricesForMap(int platform,//all, teniaco, melkavan
            long? priceFrom,
            long? priceTo,
            int? stateId,
            int? cityId,
            int? zoneId,
            int? districtId,
            int? typeOfUseId,
            int? propertyTypeId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetListOfPropertiesPricesForMap";

                GetListOfPropertiesPricesForMapPVM getListOfPropertiesPricesForMapPVM = new GetListOfPropertiesPricesForMapPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    Platform = platform,
                    PriceFrom = priceFrom,
                    PriceTo = priceTo,
                    StateId = stateId,
                    CityId = cityId,
                    ZoneId = zoneId,
                    DistrictId = districtId,
                    PropertyTypeId = propertyTypeId
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfPropertiesPricesForMap(getListOfPropertiesPricesForMapPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<PropertiesPricesForMapVM>>();

                            //if (records.Count > 0)
                            //{
                            //    var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                            //    var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                            //    foreach (var record in records)
                            //    {
                            //        if (record.UserIdCreator.HasValue)
                            //        {
                            //            var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                            //            if (customUser != null)
                            //            {
                            //                record.UserCreatorName = customUser.UserName;

                            //                if (!string.IsNullOrEmpty(customUser.Name))
                            //                    record.UserCreatorName += " " + customUser.Name;

                            //                if (!string.IsNullOrEmpty(customUser.Family))
                            //                    record.UserCreatorName += " " + customUser.Family;
                            //            }
                            //        }
                            //    }
                            //}

                            #endregion

                            return Json(new
                            {
                                Result = jsonResultWithRecordsObjectVM.Result,
                                Records = records,
                                //TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
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

        #endregion

    }
}

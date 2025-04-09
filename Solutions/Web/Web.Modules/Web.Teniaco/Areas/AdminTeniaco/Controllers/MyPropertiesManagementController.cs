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
using System.Dynamic;
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
    public class MyPropertiesManagementController : ExtraAdminController
    {
        public MyPropertiesManagementController(IHostEnvironment _hostEnvironment,
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


        #region properties management

        public IActionResult Index()
        {
            ViewData["Title"] = "لیست املاک من";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MyPropertyTypesList"] == null)
            {
                List<MyPropertyTypesVM> propertyTypesVMList = new List<MyPropertyTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MyPropertyTypesManagement/GetAllMyPropertyTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertyTypesList(new GetAllPropertyTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<MyPropertyTypesVM>>();


                                if (propertyTypesVMList != null)
                                    if (propertyTypesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["MyPropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/TypeOfUsesManagement/GetAllTypeOfUsesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllTypeOfUsesList(new GetAllTypeOfUsesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

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

            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(new GetAllDocumentTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentTypesVMList = jArray.ToObject<List<DocumentTypesVM>>();


                                if (documentTypesVMList != null)
                                    if (documentTypesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }


            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                if (personsVMList != null)
                                    if (personsVMList.Count > 0)
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

                ViewData["PersonsList"] = personsVMList;
            }

            if (ViewData["StatesList"] == null)
            {
                List<StatesVM> statesVMList = new List<StatesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

                    GetListOfStatesPVM getListOfStatesPVM = new GetListOfStatesPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
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

                ViewData["StatesList"] = statesVMList;
            }

            if (ViewData["CitiesList"] == null)
            {
                List<CitiesVM> citiesVMList = new List<CitiesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/CitiesManagement/GetAllCitiesListWithOutStrPolygon";

                    GetAllCitiesListPVM getAllCitiesListPVM = new GetAllCitiesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
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

                ViewData["CitiesList"] = citiesVMList;
            }

            if (ViewData["ZonesList"] == null)
            {
                List<ZonesVM> zonesVMList = new List<ZonesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetAllZonesListWithOutStrPolygon";

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

                ViewData["ZonesList"] = zonesVMList;
            }

            if (ViewData["DistrictsList"] == null)
            {
                List<DistrictsVM> districtsVMList = new List<DistrictsVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/DistrictsManagement/GetAllDistrictsListWithOutStrPolygon";

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

            if (ViewData["PersonTypesList"] == null)
            {
                List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/PersonTypesManagement/GetAllPersonTypesList";

                    GetAllPersonTypesListPVM getAllPersonTypesListPVM =
                        new GetAllPersonTypesListPVM()
                        {
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonTypesList(getAllPersonTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<PersonTypesVM>>();

                                if (records.Count > 0)
                                {


                                }

                                personTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonTypesList"] = personTypesVMList;
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllMyPropertiesList(
               
             int? propertyTypeId = null,
             int? typeOfUseId = null,
             int? documentTypeId = null,
             long? consultantUserId = null,
             long? OwnerId = null,
             string propertyCodeName = null,
             long? stateId = null,
             long? cityId = null,
             long? zoneId = null,
             long? districtId = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/GetAllMyPropertiesList";

                GetAllMyPropertiesListPVM getAllMyPropertyFilesListPVM = new GetAllMyPropertiesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    PropertyTypeId = propertyTypeId,
                    TypeOfUseId = typeOfUseId,
                    DocumentTypeId = documentTypeId,
                    ConsultantUserId  = consultantUserId,
                    OwnerId = OwnerId,
                    PropertyCodeName = propertyCodeName,
                    StateId = stateId,
                    CityId = cityId,
                    ZoneId = zoneId,
                    DistrictId = districtId
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertiesList(getAllMyPropertyFilesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MyPropertiesVM>>();

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
        public IActionResult GetListOfMyProperties(int jtStartIndex = 0,
            int jtPageSize = 10,
            int? propertyTypeId = null,
            int? typeOfUseId = null,
            int? documentTypeId = null,
            string propertyCodeName = null,
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
            long? districtId = null,
            long? consultantUserId = null,
            long? OwnerId = null,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/GetListOfMyProperties";

                GetListOfMyPropertiesPVM getListOfPropertiesPVM = new GetListOfMyPropertiesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    PropertyTypeId = ((propertyTypeId.HasValue) ? propertyTypeId.Value : (int?)0),
                    TypeOfUseId = ((typeOfUseId.HasValue) ? typeOfUseId.Value : (int?)0),
                    DocumentTypeId = ((documentTypeId.HasValue) ? documentTypeId.Value : (int?)0),
                    PropertyCodeName = (!string.IsNullOrEmpty(propertyCodeName) ? propertyCodeName.Trim() : ""),
                    StateId = ((stateId.HasValue) ? stateId.Value : (long?)0),
                    CityId = ((cityId.HasValue) ? cityId.Value : (long?)0),
                    ZoneId = ((zoneId.HasValue) ? zoneId.Value : (long?)0),
                    DistrictId = ((districtId.HasValue) ? districtId.Value : (long?)0),
                    ConsultantUserId = ((consultantUserId.HasValue) ? consultantUserId.Value : (long?)0),
                    OwnerId = ((OwnerId.HasValue) ? OwnerId.Value : (long?)0),
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfMyProperties(getListOfPropertiesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MyPropertiesVM>>();

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
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }



        public IActionResult GetMyPropertyDetailsWithPropertyId(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "جزئیات ملک";

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index";
            }

            PropertiesVM propertiesVM = new PropertiesVM();
            propertiesVM.PropertyAddressVM = new PropertyAddressVM();
            propertiesVM.PropertiesPricesHistoriesVM = new List<PropertiesPricesHistoriesVM>();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetPropertyDetailsWithPropertyId";

                GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM = new GetPropertyWithPropertyIdPVM()
                {
                    PropertyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetPropertyDetailsWithPropertyId(getPropertyWithPropertyIdPVM);

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

                                GetPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM = new GetPropertyFeaturesValuesPVM()
                                {
                                    PropertyTypeId = record.PropertyTypeId,
                                    PropertyId = Id,
                                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                                };

                                string serviceUrlFeature = teniacoApiUrl + "/api/PropertiesFeaturesManagement/GetPropertyFeaturesValues";
                                var responseApiCallerFeature = new TeniacoApiCaller(serviceUrlFeature).GetPropertyFeaturesValues(getPropertyFeaturesValuesPVM).JsonResultWithRecordObjectVM;

                                if (responseApiCallerFeature != null)
                                {
                                    if (responseApiCallerFeature.Result.Equals("OK"))
                                    {
                                        var recordCalerFeature = responseApiCallerFeature.Record;
                                        var recordFeature = recordCalerFeature.ToObject<PropertyFeaturesValuesVM>();
                                        if (recordFeature != null)
                                        {
                                            ViewData["DataFeature"] = recordFeature;
                                        }
                                    }
                                }

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

            dynamic expando = new ExpandoObject();
            expando = propertiesVM;

            return View("Index", expando);
        }

        public IActionResult AddToMyProperties()
        {
            ViewData["Title"] = "تعریف ملک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MyPropertyTypesList"] == null)
            {
                List<MyPropertyTypesVM> propertyTypesVMList = new List<MyPropertyTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MyPropertyTypesManagement/GetAllMyPropertyTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertyTypesList(new GetAllPropertyTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<MyPropertyTypesVM>>();


                                if (propertyTypesVMList != null)
                                    if (propertyTypesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["MyPropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/TypeOfUsesManagement/GetAllTypeOfUsesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllTypeOfUsesList(new GetAllTypeOfUsesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

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

            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(new GetAllDocumentTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentTypesVMList = jArray.ToObject<List<DocumentTypesVM>>();


                                if (documentTypesVMList != null)
                                    if (documentTypesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }


            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                if (personsVMList != null)
                                    if (personsVMList.Count > 0)
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

                ViewData["PersonsList"] = personsVMList;
            }

            //if (ViewData["StatesList"] == null)
            //{
            //    List<StatesVM> statesVMList = new List<StatesVM>();

            //    try
            //    {
            //        serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

            //        GetListOfStatesPVM getListOfStatesPVM = new GetListOfStatesPVM()
            //        {
            //            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
            //            //    this.domainsSettings.DomainSettingId),
            //        };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getListOfStatesPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    statesVMList = jArray.ToObject<List<StatesVM>>();


            //                    if (statesVMList != null)
            //                        if (statesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["StatesList"] = statesVMList;
            //}

            //if (ViewData["CitiesList"] == null)
            //{
            //    List<CitiesVM> citiesVMList = new List<CitiesVM>();

            //    try
            //    {
            //        serviceUrl = publicApiUrl + "/api/CitiesManagement/GetAllCitiesList";

            //        GetAllCitiesListPVM getAllCitiesListPVM = new GetAllCitiesListPVM()
            //        {
            //            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
            //            //    this.domainsSettings.DomainSettingId),
            //        };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetAllCitiesList(getAllCitiesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    citiesVMList = jArray.ToObject<List<CitiesVM>>();


            //                    if (citiesVMList != null)
            //                        if (citiesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["CitiesList"] = citiesVMList;
            //}

            //if (ViewData["ZonesList"] == null)
            //{
            //    List<ZonesVM> zonesVMList = new List<ZonesVM>();

            //    try
            //    {
            //        string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetAllZonesList";

            //        GetAllZonesListPVM getAllZonesListPVM = new GetAllZonesListPVM();

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetAllZonesList(getAllZonesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    zonesVMList = jArray.ToObject<List<ZonesVM>>();


            //                    if (zonesVMList != null)
            //                        if (zonesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["ZonesList"] = zonesVMList;
            //}

            if (ViewData["PersonTypesList"] == null)
            {
                List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/PersonTypesManagement/GetAllPersonTypesList";

                    GetAllPersonTypesListPVM getAllPersonTypesListPVM =
                        new GetAllPersonTypesListPVM()
                        {
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonTypesList(getAllPersonTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<PersonTypesVM>>();

                                if (records.Count > 0)
                                {

                                    #region Fill UserCreatorName



                                    #endregion
                                }

                                personTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonTypesList"] = personTypesVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MyPropertiesManagement/Index";
            }

            MyPropertiesVM propertiesVM = new MyPropertiesVM();
            propertiesVM.MyPropertyAddressVM = new MyPropertyAddressVM();


            dynamic expando = new ExpandoObject();
            expando = propertiesVM;

            return View("AddTo", expando);
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToMyProperties(MyPropertiesVM propertiesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                propertiesVM.CreateEnDate = DateTime.Now;
                propertiesVM.CreateTime = PersianDate.TimeNow;
                propertiesVM.UserIdCreator = this.userId.Value;
                propertiesVM.IsActivated = true;
                propertiesVM.IsDeleted = false;

                //propertiesVM.MyPropertyAddressVM.CreateEnDate = DateTime.Now;
                //propertiesVM.MyPropertyAddressVM.CreateTime = PersianDate.TimeNow;
                //propertiesVM.MyPropertyAddressVM.UserIdCreator = this.userId.Value;
                //propertiesVM.MyPropertyAddressVM.IsActivated = true;
                //propertiesVM.MyPropertyAddressVM.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/AddToMyProperties";

                    AddToMyPropertiesPVM addToFormPVM = new AddToMyPropertiesPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        MyPropertiesVM = propertiesVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMyProperties(addToFormPVM);

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

                                    #region create needed folders

                                    if (propertiesVM.PropertyId > 0)
                                    {
                                        #region create domain folder

                                        string propertiesFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PropertiesFiles\\";

                                        //var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(this.domainsSettings.DomainSettingId, this.userId.Value);

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName);
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create root folder for this property

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId);
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create maps folder

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Maps"))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Maps");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create docs folder

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Docs"))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Docs");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create media folder

                                        if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Media"))
                                        {
                                            System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Media");
                                            System.Threading.Thread.Sleep(100);
                                        }

                                        #endregion
                                    }

                                    #endregion

                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = propertiesVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateProperty"))
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
            catch (Exception exc)
            {
                //string message = "";

                //message += exc.Message;

                //if (exc.InnerException != null)
                //    message += " " + exc.InnerException.Message;

                //return Json(new
                //{
                //    Result = "ERROR",
                //    Message = message
                //});
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        public IActionResult UpdateMyProperties(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "ویرایش ملک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MyPropertyTypesList"] == null)
            {
                List<MyPropertyTypesVM> propertyTypesVMList = new List<MyPropertyTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MyPropertyTypesManagement/GetAllMyPropertyTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertyTypesList(new GetAllPropertyTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<MyPropertyTypesVM>>();


                                if (propertyTypesVMList != null)
                                    if (propertyTypesVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["MyPropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/TypeOfUsesManagement/GetAllTypeOfUsesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllTypeOfUsesList(new GetAllTypeOfUsesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

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

            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(new GetAllDocumentTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentTypesVMList = jArray.ToObject<List<DocumentTypesVM>>();


                                if (documentTypesVMList != null)
                                    if (documentTypesVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }



            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                if (personsVMList != null)
                                    if (personsVMList.Count > 0)
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

                ViewData["PersonsList"] = personsVMList;
            }

            //if (ViewData["StatesList"] == null)
            //{
            //    List<StatesVM> statesVMList = new List<StatesVM>();

            //    try
            //    {
            //        serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

            //        GetListOfStatesPVM getListOfStatesPVM = new GetListOfStatesPVM()
            //        {
            //            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
            //            //    this.domainsSettings.DomainSettingId),
            //        };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getListOfStatesPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    statesVMList = jArray.ToObject<List<StatesVM>>();


            //                    if (statesVMList != null)
            //                        if (statesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["StatesList"] = statesVMList;
            //}

            //if (ViewData["CitiesList"] == null)
            //{
            //    List<CitiesVM> citiesVMList = new List<CitiesVM>();

            //    try
            //    {
            //        serviceUrl = publicApiUrl + "/api/CitiesManagement/GetAllCitiesList";

            //        GetAllCitiesListPVM getAllCitiesListPVM = new GetAllCitiesListPVM()
            //        {
            //            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
            //            //    this.domainsSettings.DomainSettingId),
            //        };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetAllCitiesList(getAllCitiesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    citiesVMList = jArray.ToObject<List<CitiesVM>>();


            //                    if (citiesVMList != null)
            //                        if (citiesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["CitiesList"] = citiesVMList;
            //}

            //if (ViewData["ZonesList"] == null)
            //{
            //    List<ZonesVM> zonesVMList = new List<ZonesVM>();

            //    try
            //    {
            //        string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetAllZonesList";

            //        GetAllZonesListPVM getAllZonesListPVM = new GetAllZonesListPVM();

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetAllZonesList(getAllZonesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    zonesVMList = jArray.ToObject<List<ZonesVM>>();


            //                    if (zonesVMList != null)
            //                        if (zonesVMList.Count > 0)
            //                        {

            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["ZonesList"] = zonesVMList;
            //}

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MyPropertiesManagement/Index";
            }

            MyPropertiesVM propertiesVM = new MyPropertiesVM();
            propertiesVM.MyPropertyAddressVM = new MyPropertyAddressVM();
            propertiesVM.MyPropertiesPricesHistoriesVM = new List<MyPropertiesPricesHistoriesVM>();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

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

                                #region Fill UserCreatorName



                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            dynamic expando = new ExpandoObject();
            expando = propertiesVM;

            return View("Update", expando);
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult UpdateMyProperties(MyPropertiesVM propertiesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                propertiesVM.EditEnDate = DateTime.Now;
                propertiesVM.EditTime = PersianDate.TimeNow;
                propertiesVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/UpdateMyProperties";

                    UpdateMyPropertiesPVM updateFormPVM = new UpdateMyPropertiesPVM()
                    {
                        MyPropertiesVM = propertiesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateMyProperties(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                propertiesVM = jObject.ToObject<MyPropertiesVM>();

                                #region Fill UserCreatorName

                                if (propertiesVM.UserIdCreator.HasValue)
                                {
                                    var customUser = consoleBusiness.GetCustomUser(propertiesVM.UserIdCreator.Value);

                                    if (customUser != null)
                                    {
                                        propertiesVM.UserCreatorName = customUser.UserName;

                                        if (!string.IsNullOrEmpty(customUser.Name))
                                            propertiesVM.UserCreatorName += " " + customUser.Name;

                                        if (!string.IsNullOrEmpty(customUser.Family))
                                            propertiesVM.UserCreatorName += " " + customUser.Family;
                                    }
                                }

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = propertiesVM,
                                    Message = "ویرایش انجام شد",
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateProperty"))
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
                        Record = propertiesVM,
                    });
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
        public IActionResult ToggleActivationMyProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/ToggleActivationMyProperties";

                ToggleActivationMyPropertiesPVM toggleActivationPropertiesPVM =
                    new ToggleActivationMyPropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationMyProperties(toggleActivationPropertiesPVM);

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
        public IActionResult TemporaryDeleteMyProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/TemporaryDeleteMyProperties";

                TemporaryDeleteMyPropertiesPVM toggleActivationPropertiesPVM =
                    new TemporaryDeleteMyPropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteMyProperties(toggleActivationPropertiesPVM);

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
        public IActionResult CompleteDeleteMyProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/CompleteDeleteMyProperties";

                CompleteDeleteMyPropertiesPVM deletePropertiesPVM =
                    new CompleteDeleteMyPropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteMyProperties(deletePropertiesPVM);

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

        #endregion

        #region persons management

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToPersons(PersonsVM personsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                personsVM.CreateEnDate = DateTime.Now;
                personsVM.CreateTime = PersianDate.TimeNow;
                personsVM.UserIdCreator = this.userId.Value;
                personsVM.IsActivated = true;
                personsVM.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/PersonsManagement/AddToPersons";

                    AddToPersonsPVM addToFormPVM = new AddToPersonsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        PersonsVM = personsVM
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).AddToPersons(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                personsVM = jObject.ToObject<PersonsVM>();

                                #region Fill UserCreatorName

                                //if (zonesVM.UserIdCreator.HasValue)
                                //{
                                //    var customUser = consoleBusiness.GetCustomUser(zonesVM.UserIdCreator.Value);

                                //    if (customUser != null)
                                //    {
                                //        zonesVM.UserCreatorName = customUser.UserName;

                                //        if (!string.IsNullOrEmpty(customUser.Name))
                                //            zonesVM.UserCreatorName += " " + customUser.Name;

                                //        if (!string.IsNullOrEmpty(customUser.Family))
                                //            zonesVM.UserCreatorName += " " + customUser.Family;
                                //    }
                                //}

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = personsVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicatePerson"))
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
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllPersonsList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

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

        #endregion

        #region features management

        public IActionResult ListOfFeatures()
        {
            ViewData["Title"] = "تعاریف امکانات";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["MyPropertyTypesList"] == null)
            {
                List<MyPropertyTypesVM> propertyTypesVMList = new List<MyPropertyTypesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/MyPropertyTypesManagement/GetAllMyPropertyTypesList";
                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertyTypesList(new GetAllPropertyTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<MyPropertyTypesVM>>();


                                if (propertyTypesVMList != null)
                                    if (propertyTypesVMList.Count > 0)
                                    {
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

                ViewData["MyPropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["ElementTypesList"] == null)
            {
                List<ElementTypesVM> elementTypesVMList = new List<ElementTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/ElementTypesManagement/GetAllElementTypesList";

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllElementTypesList(new GetAllElementTypesListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<ElementTypesVM>>();

                                if (records.Count > 0)
                                {
                                    #region Fill UserCreatorName

                                    #endregion
                                }

                                elementTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ElementTypesList"] = elementTypesVMList;
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfFeatures(int jtStartIndex = 0,
            int jtPageSize = 0,
            int? propertyTypeId = null,
            string featureTitleSearch = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/GetListOfFeatures";

                GetListOfFeaturesPVM getListOfFeaturesPVM = new GetListOfFeaturesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtPageSize = jtPageSize,
                    jtStartIndex = jtStartIndex,
                    PropertyTypeId = propertyTypeId,
                    FeatureTitleSearch = featureTitleSearch
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfFeatures(getListOfFeaturesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<FeaturesVM>>();

                            //JObject jObject = jsonResultWithRecordsObjectVM.Records;
                            //var records = jObject.ToObject<List<FeaturesVM>>();

                            if (records.Count > 0)
                            {
                                #region Fill UserCreatorName

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
                            }


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
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToFeatures(FeaturesVM featuresVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                featuresVM.CreateEnDate = DateTime.Now;
                featuresVM.CreateTime = PersianDate.TimeNow;
                featuresVM.UserIdCreator = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/AddToFeatures";

                    AddToFeaturesPVM addToFormPVM = new AddToFeaturesPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        FeaturesVM = featuresVM
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToFeatures(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                featuresVM = jObject.ToObject<FeaturesVM>();

                                #region Fill UserCreatorName

                                //if (zonesVM.UserIdCreator.HasValue)
                                //{
                                //    var customUser = consoleBusiness.GetCustomUser(zonesVM.UserIdCreator.Value);

                                //    if (customUser != null)
                                //    {
                                //        zonesVM.UserCreatorName = customUser.UserName;

                                //        if (!string.IsNullOrEmpty(customUser.Name))
                                //            zonesVM.UserCreatorName += " " + customUser.Name;

                                //        if (!string.IsNullOrEmpty(customUser.Family))
                                //            zonesVM.UserCreatorName += " " + customUser.Family;
                                //    }
                                //}

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = featuresVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateFeature"))
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
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult UpdateFeatures(FeaturesVM featuresVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                featuresVM.EditEnDate = DateTime.Now;
                featuresVM.EditTime = PersianDate.TimeNow;
                featuresVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/UpdateFeatures";

                    UpdateFeaturesPVM updateFormPVM = new UpdateFeaturesPVM()
                    {
                        FeaturesVM = featuresVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateFeatures(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                featuresVM = jObject.ToObject<FeaturesVM>();

                                #region Fill UserCreatorName

                                //if (zonesVM.UserIdCreator.HasValue)
                                //{
                                //    var customUser = consoleBusiness.GetCustomUser(zonesVM.UserIdCreator.Value);

                                //    if (customUser != null)
                                //    {
                                //        zonesVM.UserCreatorName = customUser.UserName;

                                //        if (!string.IsNullOrEmpty(customUser.Name))
                                //            zonesVM.UserCreatorName += " " + customUser.Name;

                                //        if (!string.IsNullOrEmpty(customUser.Family))
                                //            zonesVM.UserCreatorName += " " + customUser.Family;
                                //    }
                                //}

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = featuresVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateFeature"))
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
                        Record = featuresVM,
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
        public IActionResult ToggleActivationFeatures(int FeatureId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/ToggleActivationFeatures";

                ToggleActivationFeaturesPVM toggleActivationFeaturesPVM =
                    new ToggleActivationFeaturesPVM()
                    {
                        FeatureId = FeatureId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationFeatures(toggleActivationFeaturesPVM);

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
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult TemporaryDeleteFeatures(int FeatureId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //if (institutionsBusiness.CompleteDeleteFeatures(EducationLevelId, this.userId.Value))
                //{
                //    return Json(new
                //    {
                //        Result = "OK",
                //    });
                //}
                //else
                //{
                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentFeaturesErrorMessage").FirstOrDefault().Value
                //    });
                //}

                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/TemporaryDeleteFeatures";

                TemporaryDeleteFeaturesPVM temporaryDeleteFeaturesPVM =
                    new TemporaryDeleteFeaturesPVM()
                    {
                        FeatureId = FeatureId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).
                    TemporaryDeleteFeatures(temporaryDeleteFeaturesPVM);

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

                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentFeaturesErrorMessage"
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

        [HttpPost]
        [AjaxOnly]
        public IActionResult CompleteDeleteFeatures(int FeatureId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/CompleteDeleteFeatures";

                CompleteDeleteFeaturesPVM deleteFeaturesPVM =
                    new CompleteDeleteFeaturesPVM()
                    {
                        FeatureId = FeatureId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteFeatures(deleteFeaturesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
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
                Message = "ErrorMessage"
            });
        }

        #endregion
    }
}

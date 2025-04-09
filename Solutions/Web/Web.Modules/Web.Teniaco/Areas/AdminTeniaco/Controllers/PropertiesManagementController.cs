using ApiCallers.MelkavanApiCaller;
using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Models.Entities.Console;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using VM.Base;
using VM.Console;
using VM.Melkavan;
using VM.Public;
using VM.PVM.Melkavan;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class PropertiesManagementController : ExtraAdminController
    {
        public PropertiesManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست املاک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["PropertyTypesList"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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

            //نوع سند
            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                GetAllDocumentTypesListPVM getAllDocumentTypesListPVM = new GetAllDocumentTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(getAllDocumentTypesListPVM);

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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }


            //نوع ریشه سند
            if (ViewData["DocumentRootTypesList"] == null)
            {
                List<DocumentRootTypesVM> documentRootTypesVMList = new List<DocumentRootTypesVM>();

                GetAllDocumentRootTypesListPVM getAllDocumentRootTypesListPVM = new GetAllDocumentRootTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentRootTypesManagement/GetAllDocumentRootTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentRootTypesList(getAllDocumentRootTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentRootTypesVMList = jArray.ToObject<List<DocumentRootTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentRootTypesList"] = documentRootTypesVMList;
            }


            //نوع مالکیت 
            if (ViewData["DocumentOwnershipTypesList"] == null)
            {
                List<DocumentOwnershipTypesVM> documentOwnershipTypesVMList = new List<DocumentOwnershipTypesVM>();

                GetAllDocumentOwnershipTypesListPVM getAllDocumentOwnershipTypesListPVM = new GetAllDocumentOwnershipTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentOwnershipTypesManagement/GetAllDocumentOwnershipTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentOwnershipTypesList(getAllDocumentOwnershipTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentOwnershipTypesVMList = jArray.ToObject<List<DocumentOwnershipTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentOwnershipTypesList"] = documentOwnershipTypesVMList;
            }


            //عمر بنا
            if (ViewData["BuildingLifesList"] == null)
            {
                List<BuildingLifesVM> buildingLifesList = new List<BuildingLifesVM>();

                GetAllBuildingLifesListPVM getAllBuildingLifesListPVM = new GetAllBuildingLifesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };


                try
                {
                    serviceUrl = melkavanApiUrl + "/api/BuildingLifesManagement/GetAllBuildingLifesList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllBuildingLifesList(getAllBuildingLifesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                buildingLifesList = jArray.ToObject<List<BuildingLifesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["BuildingLifesList"] = buildingLifesList;
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
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

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
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                            //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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


            //مالک(اشخاص)
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
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminPublic", "PersonsManagement", "GetAllPersonsList", this.userId.Value, this.parentUserId.Value,
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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonsList"] = personsVMList;
            }



            //مالک(کاربر(
            if (ViewData["OwnerUsers"] == null)
            {


                GetListofOwnerUsersList();

            }


            // مشاور
            // گارمندان بنگاه
            if (ViewData["AgencyStaffsList"] == null)
            {
                GetListOfCounsultants();

            }



            //مالک (سرمایه گذار)
            if (ViewData["InvestorsList"] == null)
            {
                List<InvestorsVM> investorsVMList = new List<InvestorsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                    GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetAllInvestorsList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                investorsVMList = jArray.ToObject<List<InvestorsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["InvestorsList"] = investorsVMList;
            }



            //نوع آگهی
            if (ViewData["AdvertisementTypesList"] == null)
            {
                List<AdvertisementTypesVM> advertisementTypesList = new List<AdvertisementTypesVM>();

                GetAllAdvertisementTypesListPVM getAllAdvertisementTypesListPVM = new GetAllAdvertisementTypesListPVM()
                {

                };
                try
                {
                    serviceUrl = melkavanApiUrl + "/api/AdvertisementTypesManagement/GetAllAdvertisementTypesList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllAdvertisementTypesList(getAllAdvertisementTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                advertisementTypesList = jArray.ToObject<List<AdvertisementTypesVM>>();



                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["AdvertisementTypesList"] = advertisementTypesList;
            }


            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }


            return View("Index");

        }


        public void GetListOfCounsultants1()
        {
            List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            try
            {
                List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                
                //گرفتن ایدی های دسترسی مشاورین
                var levelIds = consoleBusiness.CmsDb.Levels.Where(c =>
                c.LevelName.Contains(("مدیر املاک")) ||
                c.LevelName.Contains("مشاور املاک - کاربر و زیر گروه ها") ||
                c.LevelName.Contains("مشاور املاک - فقط خود کاربر") ||
                c.LevelName.Contains("مشاور املاک - فقط زیر گروه ها") ||
                c.LevelName.Contains("مشاور املاک - والد و زیر گروه ها")).Select(c => c.LevelId).ToList();

                //گرفتن ایدی کاربرانی که دسترسی مشاورین دارند
                var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

                //گرفتن لیست کاربران
                //var usersList = consoleBusiness.CmsDb.Users.Where(c => c.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                //                userIds.Contains(c.UserId)).AsQueryable();

                var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

                var levelIdsForThisUserId = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(this.userId.Value)).ToList().Select(c=>c.LevelId);

                var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIdsForThisUserId.Contains(c.LevelId)).Select(c => c.LevelName).ToList();

                //اعمال شرط دسترسی
                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        if(levelNames.Contains("مشاور املاک - والد و زیر گروه ها"))
                        {
                            usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) ||
                            childsUsersIds.Contains(c.ParentUserId.Value) || 
                            childsUsersIds.Contains(c.UserId) ||
                            c.ParentUserId.Equals(this.parentUserId.Value));
                        }
                        else
                        {
                            usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));
                        }

                        
                        ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                        {
                            UserId = s.UserId,
                            Name = s.UsersProfileUser.Name,
                            Family = s.UsersProfileUser.Family,
                            UserName = s.UserName,
                            Phone = s.UsersProfileUser.Phone,
                            Mobile = s.UsersProfileUser.Mobile,

                        }).ToList();
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));
                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();
                            }
                        }
                        else if (childsUsersIds.Count == 0)
                        {
                            List<CustomUsersVM> usersList2 = new List<CustomUsersVM>();

                            ViewData["AgencyStaffsList"] = usersList2;
                        }
                    }
                }


               

            }

            catch (Exception exc)
            { }
          

        }
        public void GetListOfCounsultants()
        {
            List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            try
            {
                List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);


                var levelIdsForThisUserId = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(this.userId.Value)).ToList().Select(c => c.LevelId);

                var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIdsForThisUserId.Contains(c.LevelId)).Select(c => c.LevelName).ToList();

                //اعمال شرط دسترسی
                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {

                        #region load data for consultants

                     
                        if (levelNames.Contains("مدیر املاک"))
                        {
                             //لود لیست مشاورین آژانس من => parentUserId
                             //لود خودم به عنوان مدیر املاک => UserId
                             //لود مشاورینی که من ثبت کرده ام => userIdCreator

                            var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                            c.ParentUserId.Equals(this.userId.Value) ||
                            c.UserIdCreator.Equals(this.userId.Value) ||
                            c.UserId.Equals(this.userId.Value)).AsQueryable();

                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();

                        }else if (levelNames.Contains("مشاور املاک - کاربر و زیر گروه ها"))
                        {

                            var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                            c.ParentUserId.Equals(this.userId.Value) ||
                            c.UserIdCreator.Equals(this.userId.Value) ||
                            c.UserId.Equals(this.userId.Value)).AsQueryable();


                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();

                        }else if (levelNames.Contains("مشاور املاک - فقط خود کاربر"))
                        {
                            var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                            c.UserId.Equals(this.userId.Value)).AsQueryable();

                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();

                        }else if (levelNames.Contains("مشاور املاک - فقط زیر گروه ها"))
                        {
                            var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                           c.ParentUserId.Equals(this.userId.Value)).AsQueryable();


                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();

                        }else if (levelNames.Contains("مشاور املاک - والد و زیر گروه ها"))
                        {
                            var usersList = consoleBusiness.CmsDb.Users.Where(c =>c.ParentUserId.Equals(this.parentUserId.Value));

                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();
                        }
                        else
                        {
                            //گرفتن ایدی های دسترسی مشاورین
                            var levelIds = consoleBusiness.CmsDb.Levels.Where(c =>
                            c.LevelName.Contains(("مدیر املاک")) ||
                            c.LevelName.Contains("مشاور املاک - کاربر و زیر گروه ها") ||
                            c.LevelName.Contains("مشاور املاک - فقط خود کاربر") ||
                            c.LevelName.Contains("مشاور املاک - فقط زیر گروه ها") ||
                            c.LevelName.Contains("مشاور املاک - والد و زیر گروه ها")).Select(c => c.LevelId).ToList();

                            //گرفتن ایدی کاربرانی که دسترسی مشاورین دارند
                            var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

                            //گرفتن لیست کاربران
                            //var usersList = consoleBusiness.CmsDb.Users.Where(c => c.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                            //                userIds.Contains(c.UserId)).AsQueryable();

                            var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

                            usersList = usersList.Where(c => 
                            childsUsersIds.Contains(c.UserIdCreator.Value) ||
                            childsUsersIds.Contains(c.ParentUserId.Value) || 
                            childsUsersIds.Contains(c.UserId));

                            ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                            {
                                UserId = s.UserId,
                                Name = s.UsersProfileUser.Name,
                                Family = s.UsersProfileUser.Family,
                                UserName = s.UserName,
                                Phone = s.UsersProfileUser.Phone,
                                Mobile = s.UsersProfileUser.Mobile,

                            }).ToList();
                        }
                        #endregion

                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {

                            #region load data for consultants


                            if (levelNames.Contains("مدیر املاک"))
                            {
                                //لود لیست مشاورین آژانس من => parentUserId
                                //لود خودم به عنوان مدیر املاک => UserId
                                //لود مشاورینی که من ثبت کرده ام => userIdCreator

                                var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                                c.ParentUserId.Equals(this.userId.Value) ||
                                c.UserIdCreator.Equals(this.userId.Value) ||
                                c.UserId.Equals(this.userId.Value)).AsQueryable();

                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();

                            }
                            else if (levelNames.Contains("مشاور املاک - کاربر و زیر گروه ها"))
                            {

                                var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                                c.ParentUserId.Equals(this.userId.Value) ||
                                c.UserIdCreator.Equals(this.userId.Value) ||
                                c.UserId.Equals(this.userId.Value)).AsQueryable();


                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();

                            }
                            else if (levelNames.Contains("مشاور املاک - فقط خود کاربر"))
                            {
                                var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                                c.UserId.Equals(this.userId.Value)).AsQueryable();

                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();

                            }
                            else if (levelNames.Contains("مشاور املاک - فقط زیر گروه ها"))
                            {
                                var usersList = consoleBusiness.CmsDb.Users.Where(c =>
                               c.ParentUserId.Equals(this.userId.Value)).AsQueryable();


                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();

                            }
                            else if (levelNames.Contains("مشاور املاک - والد و زیر گروه ها"))
                            {
                                var usersList = consoleBusiness.CmsDb.Users.Where(c => c.ParentUserId.Equals(this.parentUserId.Value));

                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();
                            }
                            else
                            {
                                //گرفتن ایدی های دسترسی مشاورین
                                var levelIds = consoleBusiness.CmsDb.Levels.Where(c =>
                                c.LevelName.Contains(("مدیر املاک")) ||
                                c.LevelName.Contains("مشاور املاک - کاربر و زیر گروه ها") ||
                                c.LevelName.Contains("مشاور املاک - فقط خود کاربر") ||
                                c.LevelName.Contains("مشاور املاک - فقط زیر گروه ها") ||
                                c.LevelName.Contains("مشاور املاک - والد و زیر گروه ها")).Select(c => c.LevelId).ToList();

                                //گرفتن ایدی کاربرانی که دسترسی مشاورین دارند
                                var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

                                //گرفتن لیست کاربران
                                //var usersList = consoleBusiness.CmsDb.Users.Where(c => c.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                                //                userIds.Contains(c.UserId)).AsQueryable();

                                var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

                                usersList = usersList.Where(c =>
                                childsUsersIds.Contains(c.UserIdCreator.Value) ||
                                childsUsersIds.Contains(c.ParentUserId.Value) ||
                                childsUsersIds.Contains(c.UserId));

                                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
                                {
                                    UserId = s.UserId,
                                    Name = s.UsersProfileUser.Name,
                                    Family = s.UsersProfileUser.Family,
                                    UserName = s.UserName,
                                    Phone = s.UsersProfileUser.Phone,
                                    Mobile = s.UsersProfileUser.Mobile,

                                }).ToList();
                            }
                            #endregion

                        }
                        else if (childsUsersIds.Count == 0)
                        {
                            List<CustomUsersVM> usersList2 = new List<CustomUsersVM>();

                            ViewData["AgencyStaffsList"] = usersList2;
                        }
                    }
                }




            }

            catch (Exception exc)
            { }


        }

        public void GetListofOwnerUsersList()
        {
            List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            try
            {
                List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                //گرفتن ایدی های دسترسی مالک
                var levelIds = consoleBusiness.CmsDb.Levels.Where(c => c.LevelName.Contains("مالک")).Select(c => c.LevelId).ToList();

                //گرفتن ایدی کاربرانی که دسترسی مالک دارند
                var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

                //گرفتن لیست کاربران

                var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

                //اعمال شرط دسترسی
                if (childsUsersIds != null)
                {
                    if (childsUsersIds.Count > 1)
                    {
                        usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));
                    }
                    else
                    {
                        if (childsUsersIds.Count == 1)
                        {
                            if (childsUsersIds.FirstOrDefault() > 0)
                            {
                                usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));

                            }
                        }
                       
                    }
                }


                ViewData["OwnerUsers"] = usersList.Select(s => new CustomUsersVM
                {
                    UserId = s.UserId,
                    Name = s.UsersProfileUser.Name,
                    Family = s.UsersProfileUser.Family,
                    UserName = s.UserName,
                    Phone = s.UsersProfileUser.Phone,
                    Mobile = s.UsersProfileUser.Mobile,

                }).ToList();

            }

            catch (Exception exc)
            { }

        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllPropertiesList(

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
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetAllPropertiesList";

                GetAllPropertiesListPVM getAllPropertyFilesListPVM = new GetAllPropertiesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    PropertyTypeId = propertyTypeId,
                    TypeOfUseId = typeOfUseId,
                    DocumentTypeId = documentTypeId,
                    ConsultantUserId = consultantUserId,
                    OwnerId = OwnerId,
                    PropertyCodeName = propertyCodeName,
                    StateId = stateId,
                    CityId = cityId,
                    ZoneId = zoneId,
                    DistrictId = districtId
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertiesList(getAllPropertyFilesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<PropertiesVM>>();

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
        public IActionResult GetListOfProperties(int jtStartIndex = 0,
            int jtPageSize = 10,
            int? propertyTypeId = null,
            int? typeOfUseId = null,
            int? documentTypeId = null,
            //int? documentOwnershipTypeId = null,
            //int? documentRootTypeId = null,
            string propertyCodeName = null,
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
            long? districtId = null,
            long? consultantUserId = null,
            long? OwnerId = null,
            //bool? isPrivate = null,
            string jtSorting = null,
            long? priceFrom = null,
            long? priceTo = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetListOfProperties";

                GetListOfPropertiesPVM getListOfPropertiesPVM = new GetListOfPropertiesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),

                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    PropertyTypeId = ((propertyTypeId.HasValue) ? propertyTypeId.Value : (int?)0),
                    TypeOfUseId = ((typeOfUseId.HasValue) ? typeOfUseId.Value : (int?)0),
                    DocumentTypeId = ((documentTypeId.HasValue) ? documentTypeId.Value : (int?)0),
                    //DocumentOwnershipTypeId = ((documentOwnershipTypeId.HasValue) ? documentOwnershipTypeId.Value : (int?)0),
                    //DocumentRootTypeId = ((documentRootTypeId.HasValue) ? documentRootTypeId.Value : (int?)0),
                    PropertyCodeName = (!string.IsNullOrEmpty(propertyCodeName) ? propertyCodeName.Trim() : ""),
                    StateId = ((stateId.HasValue) ? stateId.Value : (long?)0),
                    CityId = ((cityId.HasValue) ? cityId.Value : (long?)0),
                    ZoneId = ((zoneId.HasValue) ? zoneId.Value : (long?)0),
                    DistrictId = ((districtId.HasValue) ? districtId.Value : (long?)0),
                    ConsultantUserId = ((consultantUserId.HasValue) ? consultantUserId.Value : (long?)0),
                    OwnerId = ((OwnerId.HasValue) ? OwnerId.Value : (long?)0),
                    //IsPrivate = isPrivate,//((isPrivate.HasValue) ? isPrivate.Value : (bool?)false),
                    jtSorting = jtSorting,
                    PriceFrom = ((priceFrom.HasValue) ? priceFrom.Value : (long?)0),
                    PriceTo = ((priceTo.HasValue) ? priceTo.Value : (long?)0),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfProperties(getListOfPropertiesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<PropertiesVM>>();

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

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfPropertiesAdvanceSearch(int jtStartIndex = 0,
            int jtPageSize = 10,
            List<int>? platform = null,
            int? propertyTypeId = null,
            int? price = null,
            double? priceFrom = null,
            double? priceTo = null,
            int? area = null,
            double? areaFrom = null,
            double? areaTo = null,
            string? address = null,
            string? featuresAndDesc = null,
            int? typeOfUseId = null,
            int? documentTypeId = null,
            int? documentRootTypeId = null,
            int? documentOwnershipTypeId = null,
            string? propertyCodeName = null,
            long? consultantUserId = null,
            long? OwnerId = null,
            long? InvestorId = null,
            long? AdvertiserId = null,
            string? tmpFeatures = null,
            bool? Participable = false,
            bool? Exchangeable = false,
            long? stateId = null,
            long? cityId = null,
            long? zoneId = null,
            long? districtId = null,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            Dictionary<string, string> Features = new Dictionary<string, string>();

            #region comments
            //Features = json[0].ToObject<Dictionary<string, string>>();// < Dictionary<string, string>>(json.ToString());

            //var a = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(json.ToString()));

            //Features =  JsonConvert.DeserializeObject<Dictionary<string, string>>(json.ToString());

            //if (Features.Where(c => c.Key.Equals("jtStartIndex")).Any())
            //{
            //    Features = null;
            //}
            #endregion

            try
            {
                if (tmpFeatures != null)
                {
                    JArray json = JArray.Parse(tmpFeatures);

                    for (int i = 0; i < json.Count; i++)
                    {
                        var dic = json[i];
                        Features.Add(dic["Key"].ToString(), dic["Value"].ToString());
                    }

                }
                #region ChildUsers

                List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);


                #endregion

                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetListOfPropertiesAdvanceSearch";

                GetListOfPropertiesAdvanceSearchPVM getListOfPropertiesAdvanceSearchPVM = new GetListOfPropertiesAdvanceSearchPVM()
                {

                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PropertiesManagement", "GetListOfPropertiesAdvanceSearch", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),


                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    //ChildsUsersIds = childsUsersIds,
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    Platform = ((platform != null) ? platform : null),
                    PropertyTypeId = ((propertyTypeId.HasValue) ? propertyTypeId.Value : (int?)0),
                    Price = ((price.HasValue) ? price.Value : (int?)0),
                    PriceFrom = ((priceFrom.HasValue) ? priceFrom.Value : (double?)0),
                    PriceTo = ((priceTo.HasValue) ? priceTo.Value : (double?)0),
                    Area = ((area.HasValue) ? area.Value : (int?)0),
                    AreaFrom = ((areaFrom.HasValue) ? areaFrom.Value : (double?)0),
                    AreaTo = ((areaTo.HasValue) ? areaTo.Value : (double?)0),
                    Address = (!string.IsNullOrEmpty(address) ? address.Trim() : ""),
                    FeaturesAndDesc = (!string.IsNullOrEmpty(featuresAndDesc) ? featuresAndDesc.Trim() : ""),
                    TypeOfUseId = ((typeOfUseId.HasValue) ? typeOfUseId.Value : (int?)0),
                    DocumentTypeId = ((documentTypeId.HasValue) ? documentTypeId.Value : (int?)0),
                    DocumentRootTypeId = ((documentRootTypeId.HasValue) ? documentRootTypeId.Value : (int?)0),
                    DocumentOwnershipTypeId = ((documentOwnershipTypeId.HasValue) ? documentOwnershipTypeId.Value : (int?)0),
                    PropertyCodeName = (!string.IsNullOrEmpty(propertyCodeName) ? propertyCodeName.Trim() : ""),
                    ConsultantUserId = ((consultantUserId.HasValue) ? consultantUserId.Value : (long?)0),
                    OwnerId = ((OwnerId.HasValue) ? OwnerId.Value : (long?)0),
                    InvestorId = ((InvestorId.HasValue) ? InvestorId.Value : (long?)0),
                    AdvertiserId = ((AdvertiserId.HasValue) ? AdvertiserId.Value : (long?)0),
                    Features = ((Features != null) ? Features : null),
                    StateId = ((stateId.HasValue) ? stateId.Value : (long?)0),
                    CityId = ((cityId.HasValue) ? cityId.Value : (long?)0),
                    ZoneId = ((zoneId.HasValue) ? zoneId.Value : (long?)0),
                    DistrictId = ((districtId.HasValue) ? districtId.Value : (long?)0),
                    ThisUserId = this.userId.Value,
                    Participable  = Participable,
                    Exchangeable = Exchangeable,
                    jtSorting = jtSorting,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfPropertiesAdvanceSearch(getListOfPropertiesAdvanceSearchPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<PropertiesAdvanceSearchVM>>();

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

        public IActionResult AddToProperties()
        {
            ViewData["Title"] = "تعریف ملک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            //خریدار
            if (ViewData["BuyersList"] == null)
            {
                List<PersonsVM> buyersVMList = new List<PersonsVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetBuyersList";

                    GetAllPersonsListPVM getBuyersListPVM = new GetAllPersonsListPVM()
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PersonsManagement", "GetBuyersList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getBuyersListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                buyersVMList = jArray.ToObject<List<PersonsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["BuyersList"] = buyersVMList;
            }


            //نوع ملک
            if (ViewData["PropertyTypesList"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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

            //نوع کاربری
            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();
                GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM = new GetAllTypeOfUsesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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

            //نوع سند
            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                GetAllDocumentTypesListPVM getAllDocumentTypesListPVM = new GetAllDocumentTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(getAllDocumentTypesListPVM);

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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }


            //نوع ریشه سند
            if (ViewData["DocumentRootTypesList"] == null)
            {
                List<DocumentRootTypesVM> documentRootTypesVMList = new List<DocumentRootTypesVM>();

                GetAllDocumentRootTypesListPVM getAllDocumentRootTypesListPVM = new GetAllDocumentRootTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentRootTypesManagement/GetAllDocumentRootTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentRootTypesList(getAllDocumentRootTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentRootTypesVMList = jArray.ToObject<List<DocumentRootTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentRootTypesList"] = documentRootTypesVMList;
            }


            //نوع مالکیت 
            if (ViewData["DocumentOwnershipTypesList"] == null)
            {
                List<DocumentOwnershipTypesVM> documentOwnershipTypesVMList = new List<DocumentOwnershipTypesVM>();

                GetAllDocumentOwnershipTypesListPVM getAllDocumentOwnershipTypesListPVM = new GetAllDocumentOwnershipTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentOwnershipTypesManagement/GetAllDocumentOwnershipTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentOwnershipTypesList(getAllDocumentOwnershipTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentOwnershipTypesVMList = jArray.ToObject<List<DocumentOwnershipTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentOwnershipTypesList"] = documentOwnershipTypesVMList;
            }


            //عمر بنا
            if (ViewData["BuildingLifesList"] == null)
            {
                List<BuildingLifesVM> buildingLifesList = new List<BuildingLifesVM>();

                GetAllBuildingLifesListPVM getAllBuildingLifesListPVM = new GetAllBuildingLifesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };


                try
                {
                    serviceUrl = melkavanApiUrl + "/api/BuildingLifesManagement/GetAllBuildingLifesList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllBuildingLifesList(getAllBuildingLifesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                buildingLifesList = jArray.ToObject<List<BuildingLifesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["BuildingLifesList"] = buildingLifesList;
            }

            //مالک(اشخاص)
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
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminPublic", "PersonsManagement", "GetAllPersonsList", this.userId.Value, this.parentUserId.Value,
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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonsList"] = personsVMList;
            }

            //مالک (سرمایه گذار)
            if (ViewData["InvestorsList"] == null)
            {
                List<InvestorsVM> investorsVMList = new List<InvestorsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                    GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetAllInvestorsList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                investorsVMList = jArray.ToObject<List<InvestorsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["InvestorsList"] = investorsVMList;
            }



            // مشاور
            // گارمندان بنگاه
            if (ViewData["AgencyStaffsList"] == null)
            {
                GetListOfCounsultants();

            }

            #region comments



            //// مشاور
            //// گارمندان بنگاه
            //if (ViewData["AgencyStaffsList"] == null)
            //{

            //    List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            //    try
            //    {

            //        #region ChildUsers

            //        List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
            //                this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

            //        //گرفتن ایدی های دسترسی مشاورین
            //        var levelIds = consoleBusiness.CmsDb.Levels.Where(c =>
            //        c.LevelName.Contains(("مدیر املاک")) ||
            //        c.LevelName.Contains("مشاور املاک - کاربر و زیر گروه ها") ||
            //        c.LevelName.Contains("مشاور املاک - فقط خود کاربر") ||
            //        c.LevelName.Contains("مشاور املاک - فقط زیر گروه ها") ||
            //        c.LevelName.Contains("مشاور املاک - والد و زیر گروه ها")).Select(c => c.LevelId).ToList();

            //        //گرفتن ایدی کاربرانی که دسترسی مشاورین دارند
            //        var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

            //        //گرفتن لیست کاربران
            //        //var usersList = consoleBusiness.CmsDb.Users.Where(c => c.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //                userIds.Contains(c.UserId)).AsQueryable();

            //        var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

            //        //اعمال شرط دسترسی
            //        if (childsUsersIds != null)
            //        {
            //            if (childsUsersIds.Count > 1)
            //            {
            //                usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));

            //                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
            //                {
            //                    UserId = s.UserId,
            //                    Name = s.UsersProfileUser.Name,
            //                    Family = s.UsersProfileUser.Family,
            //                    UserName = s.UserName,
            //                    Phone = s.UsersProfileUser.Phone,
            //                    Mobile = s.UsersProfileUser.Mobile,

            //                }).ToList();
            //            }
            //            else
            //            {
            //                if (childsUsersIds.Count == 1)
            //                {
            //                    if (childsUsersIds.FirstOrDefault() > 0)
            //                    {
            //                        usersList = usersList.Where(c =>
            //                        childsUsersIds.Contains(c.UserIdCreator.Value) ||
            //                        childsUsersIds.Contains(c.ParentUserId.Value) ||
            //                        childsUsersIds.Contains(c.UserId));

            //                        ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
            //                        {
            //                            UserId = s.UserId,
            //                            Name = s.UsersProfileUser.Name,
            //                            Family = s.UsersProfileUser.Family,
            //                            UserName = s.UserName,
            //                            Phone = s.UsersProfileUser.Phone,
            //                            Mobile = s.UsersProfileUser.Mobile,

            //                        }).ToList();
            //                    }

            //                }
            //                else if (childsUsersIds.Count == 0)
            //                {
            //                    List<CustomUsersVM> usersList2 = new List<CustomUsersVM>();

            //                    ViewData["AgencyStaffsList"] = usersList2;
            //                }
            //            }
            //        }



            //        #endregion





            //        #region code - comments

            //        //var getLevelId1 = consoleBusiness.GetLevelDetailWithLevelName("مدیر املاک");
            //        //var getLevelId2 = consoleBusiness.GetLevelDetailWithLevelName("مشاور املاک - کاربر و زیر گروه ها");
            //        //var getLevelId3 = consoleBusiness.GetLevelDetailWithLevelName("مشاور املاک - فقط خود کاربر");
            //        //var getLevelId4 = consoleBusiness.GetLevelDetailWithLevelName("مشاور املاک - فقط زیر گروه ها");
            //        //var getLevelId5 = consoleBusiness.GetLevelDetailWithLevelName("مشاور املاک - والد و زیر گروه ها");


            //        //var usersIdsInUserLevels1 = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId1.LevelId)).Select(f => f.UserId).ToList();
            //        //var usersIdsInUserLevels2 = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId2.LevelId)).Select(f => f.UserId).ToList();
            //        //var usersIdsInUserLevels3 = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId3.LevelId)).Select(f => f.UserId).ToList();
            //        //var usersIdsInUserLevels4 = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId4.LevelId)).Select(f => f.UserId).ToList();
            //        //var usersIdsInUserLevels5 = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId5.LevelId)).Select(f => f.UserId).ToList();


            //        //var usersList1 = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //        usersIdsInUserLevels1.Contains(u.UserId)).AsQueryable();


            //        //var usersList2 = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //    usersIdsInUserLevels2.Contains(u.UserId)).AsQueryable();

            //        //var usersList3 = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //        usersIdsInUserLevels3.Contains(u.UserId)).AsQueryable();

            //        //var usersList4 = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //    usersIdsInUserLevels4.Contains(u.UserId)).AsQueryable();


            //        //var usersList5 = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //    usersIdsInUserLevels5.Contains(u.UserId)).AsQueryable();

            //        //if (childsUsersIds != null)
            //        //{
            //        //    if (childsUsersIds.Count > 1)
            //        //    {
            //        //        usersList1 = usersList1.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //        usersList2 = usersList2.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //        usersList3 = usersList3.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //        usersList4 = usersList4.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //        usersList5 = usersList5.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //    }
            //        //    else
            //        //    {
            //        //        if (childsUsersIds.Count == 1)
            //        //        {
            //        //            if (childsUsersIds.FirstOrDefault() > 0)
            //        //            {
            //        //                usersList1 = usersList1.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //                usersList2 = usersList2.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //                usersList3 = usersList3.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //                usersList4 = usersList4.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
            //        //                usersList5 = usersList5.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));

            //        //            }
            //        //        }
            //        //    }
            //        //}


            //        //ViewData["CustomUsersList"] = usersList1.Select(s => new CustomUsersVM
            //        //{
            //        //    UserId = s.UserId,
            //        //    Name = s.UsersProfileUser.Name,
            //        //    Family = s.UsersProfileUser.Family,
            //        //    UserName = s.UserName,
            //        //    Phone = s.UsersProfileUser.Phone,
            //        //    Mobile = s.UsersProfileUser.Mobile,

            //        //}).ToList();

            //        //ViewData["CustomUsersList"] = usersList2.Select(s => new CustomUsersVM
            //        //{
            //        //    UserId = s.UserId,
            //        //    Name = s.UsersProfileUser.Name,
            //        //    Family = s.UsersProfileUser.Family,
            //        //    UserName = s.UserName,
            //        //    Phone = s.UsersProfileUser.Phone,
            //        //    Mobile = s.UsersProfileUser.Mobile,

            //        //}).ToList();


            //        //ViewData["CustomUsersList"] = usersList3.Select(s => new CustomUsersVM
            //        //{
            //        //    UserId = s.UserId,
            //        //    Name = s.UsersProfileUser.Name,
            //        //    Family = s.UsersProfileUser.Family,
            //        //    UserName = s.UserName,
            //        //    Phone = s.UsersProfileUser.Phone,
            //        //    Mobile = s.UsersProfileUser.Mobile,

            //        //}).ToList();


            //        //ViewData["CustomUsersList"] = usersList4.Select(s => new CustomUsersVM
            //        //{
            //        //    UserId = s.UserId,
            //        //    Name = s.UsersProfileUser.Name,
            //        //    Family = s.UsersProfileUser.Family,
            //        //    UserName = s.UserName,
            //        //    Phone = s.UsersProfileUser.Phone,
            //        //    Mobile = s.UsersProfileUser.Mobile,

            //        //}).ToList();


            //        //ViewData["CustomUsersList"] = usersList5.Select(s => new CustomUsersVM
            //        //{
            //        //    UserId = s.UserId,
            //        //    Name = s.UsersProfileUser.Name,
            //        //    Family = s.UsersProfileUser.Family,
            //        //    UserName = s.UserName,
            //        //    Phone = s.UsersProfileUser.Phone,
            //        //    Mobile = s.UsersProfileUser.Mobile,

            //        //}).ToList();

            //        #endregion

            //        #region comments - agencyStaffs

            //        //if (ViewData["AgencyStaffsList"] == null)
            //        //{
            //        //    List<AgencyStaffsVM> agencyStaffsList = new List<AgencyStaffsVM>();

            //        //    try
            //        //    {
            //        //        serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/GetAllAgencyStaffsList";

            //        //        GetAllAgencyStaffsListPVM getAllAgencyStaffsListPVM  = new GetAllAgencyStaffsListPVM()
            //        //        {
            //        //            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "AgencyStaffsManagement", "GetAllAgencyStaffsList", this.userId.Value, this.parentUserId.Value,
            //        //                this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
            //        //        };

            //        //        responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllAgencyStaffsList(getAllAgencyStaffsListPVM);

            //        //        if (responseApiCaller.IsSuccessStatusCode)
            //        //        {
            //        //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //        //            if (jsonResultWithRecordsObjectVM != null)
            //        //            {
            //        //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //        //                {
            //        //                    #region Fill UserCreatorName

            //        //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //        //                    agencyStaffsList = jArray.ToObject<List<AgencyStaffsVM>>();

            //        //                    #endregion
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //    catch (Exception exc)
            //        //    { }

            //        //    ViewData["AgencyStaffsList"] = agencyStaffsList;
            //        //}

            //        #endregion

            //    }

            //    catch (Exception exc)
            //    { }



            //}


            #endregion



            if (ViewData["PersonTypesList"] == null)
            {
                List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/PersonTypesManagement/GetAllPersonTypesList";

                    GetAllPersonTypesListPVM getAllPersonTypesListPVM =
                        new GetAllPersonTypesListPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index";
            }


            #region comments - states

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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            #endregion

            PropertiesVM propertiesVM = new PropertiesVM();
            propertiesVM.PropertyAddressVM = new PropertyAddressVM();
            //propertiesVM.PropertiesPricesHistoriesVM = new List<PropertiesPricesHistoriesVM>();

            dynamic expando = new ExpandoObject();
            expando = propertiesVM;

            return View("AddTo", expando);
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToProperties(PropertiesVM propertiesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                propertiesVM.CreateEnDate = DateTime.Now;
                propertiesVM.CreateTime = PersianDate.TimeNow;
                propertiesVM.UserIdCreator = this.userId.Value;
                propertiesVM.IsActivated = true;
                propertiesVM.IsDeleted = false;
                propertiesVM.OfferPrice = long.Parse(propertiesVM.StrOfferPrice.Replace(",", ""));
                propertiesVM.CalculatedOfferPrice = long.Parse(propertiesVM.StrCalculatedOfferPrice.Replace(",", ""));

                try
                {
                    if (propertiesVM.PropertiesDetailsVM != null)
                        if (!propertiesVM.PropertiesDetailsVM.BuildingLifeId.HasValue)
                            propertiesVM.PropertiesDetailsVM.BuildingLifeId = 0;
                    ModelState.Remove("PropertiesDetailsVM.BuildingLifeId");
                }
                catch (Exception exc)
                {
                    propertiesVM.PropertiesDetailsVM.BuildingLifeId = 0;
                    ModelState.Remove("PropertiesDetailsVM.BuildingLifeId");
                }

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/AddToProperties";

                    AddToPropertiesPVM addToFormPVM = new AddToPropertiesPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        PropertiesVM = propertiesVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToProperties(addToFormPVM);

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

                                    #region create needed folders

                                    if (propertiesVM.PropertyId > 0)
                                    {
                                        #region create domain folder

                                        string propertiesFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PropertiesFiles\\";

                                        //var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(this.domainsSettings.DomainSettingId, this.userId.Value);

                                        //try
                                        //{
                                        //    if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName))
                                        //    {
                                        //        System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName);
                                        //        System.Threading.Thread.Sleep(100);
                                        //    }
                                        //}
                                        //catch (Exception exc)
                                        //{ }



                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + "my.teniaco.com"))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + "my.teniaco.com");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create root folder for this property

                                        //try
                                        //{
                                        //    if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId))
                                        //    {
                                        //        System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId);
                                        //        System.Threading.Thread.Sleep(100);
                                        //    }
                                        //}
                                        //catch (Exception exc)
                                        //{ }




                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId);
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }
                                        #endregion

                                        #region create maps folder

                                        //try
                                        //{
                                        //    if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Maps"))
                                        //    {
                                        //        System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Maps");
                                        //        System.Threading.Thread.Sleep(100);
                                        //    }
                                        //}
                                        //catch (Exception exc)
                                        //{ }

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Maps"))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Maps");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }
                                        #endregion

                                        #region create docs folder

                                        //try
                                        //{
                                        //    if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Docs"))
                                        //    {
                                        //        System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Docs");
                                        //        System.Threading.Thread.Sleep(100);
                                        //    }
                                        //}
                                        //catch (Exception exc)
                                        //{ }


                                        try
                                        {
                                            if (!System.IO.Directory.Exists(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Docs"))
                                            {
                                                System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Docs");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create media folder

                                        //if (!System.IO.Directory.Exists(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Media"))
                                        //{
                                        //    System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + domainsSettings.DomainName + "\\" + propertiesVM.PropertyId + "\\Media");
                                        //    System.Threading.Thread.Sleep(100);
                                        //}


                                        if (!System.IO.Directory.Exists(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Media"))
                                        {
                                            System.IO.Directory.CreateDirectory(propertiesFolder + "\\" + "my.teniaco.com" + "\\" + propertiesVM.PropertyId + "\\Media");
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
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        public IActionResult UpdateProperties(long Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "ویرایش ملک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });



            if (ViewData["BuyersList"] == null)
            {
                List<PersonsVM> buyersVMList = new List<PersonsVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetBuyersList";

                    GetAllPersonsListPVM getBuyersListPVM = new GetAllPersonsListPVM()
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PersonsManagement", "GetBuyersList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getBuyersListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                buyersVMList = jArray.ToObject<List<PersonsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["BuyersList"] = buyersVMList;
            }


            if (ViewData["PropertyTypesList"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();
                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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

            //نوع سند
            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                GetAllDocumentTypesListPVM getAllDocumentTypesListPVM = new GetAllDocumentTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentTypesManagement/GetAllDocumentTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentTypesList(getAllDocumentTypesListPVM);

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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentTypesList"] = documentTypesVMList;
            }


            //نوع ریشه سند
            if (ViewData["DocumentRootTypesList"] == null)
            {
                List<DocumentRootTypesVM> documentRootTypesVMList = new List<DocumentRootTypesVM>();

                GetAllDocumentRootTypesListPVM getAllDocumentRootTypesListPVM = new GetAllDocumentRootTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentRootTypesManagement/GetAllDocumentRootTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentRootTypesList(getAllDocumentRootTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentRootTypesVMList = jArray.ToObject<List<DocumentRootTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentRootTypesList"] = documentRootTypesVMList;
            }


            //نوع مالکیت 
            if (ViewData["DocumentOwnershipTypesList"] == null)
            {
                List<DocumentOwnershipTypesVM> documentOwnershipTypesVMList = new List<DocumentOwnershipTypesVM>();

                GetAllDocumentOwnershipTypesListPVM getAllDocumentOwnershipTypesListPVM = new GetAllDocumentOwnershipTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ////ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    ////       this.domainsSettings.DomainSettingId),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/DocumentOwnershipTypesManagement/GetAllDocumentOwnershipTypesList";

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDocumentOwnershipTypesList(getAllDocumentOwnershipTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                documentOwnershipTypesVMList = jArray.ToObject<List<DocumentOwnershipTypesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["DocumentOwnershipTypesList"] = documentOwnershipTypesVMList;
            }


            //عمر بنا
            if (ViewData["BuildingLifesList"] == null)
            {
                List<BuildingLifesVM> buildingLifesList = new List<BuildingLifesVM>();

                GetAllBuildingLifesListPVM getAllBuildingLifesListPVM = new GetAllBuildingLifesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };


                try
                {
                    serviceUrl = melkavanApiUrl + "/api/BuildingLifesManagement/GetAllBuildingLifesList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllBuildingLifesList(getAllBuildingLifesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                buildingLifesList = jArray.ToObject<List<BuildingLifesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["BuildingLifesList"] = buildingLifesList;
            }


            //مالک (اشخاص)
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
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminPublic", "PersonsManagement", "GetAllPersonsList", this.userId.Value, this.parentUserId.Value,
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

            //مالک (سرمایه گذار)
            if (ViewData["InvestorsList"] == null)
            {
                List<InvestorsVM> investorsVMList = new List<InvestorsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                    GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetAllInvestorsList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                investorsVMList = jArray.ToObject<List<InvestorsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["InvestorsList"] = investorsVMList;
            }



            // مشاور
            // گارمندان بنگاه
            if (ViewData["AgencyStaffsList"] == null)
            {
                GetListOfCounsultants();

            }


            #region Comments


            //// مشاور
            //// گارمندان بنگاه
            //if (ViewData["AgencyStaffsList"] == null)
            //{

            //    List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            //    try
            //    {

            //        #region ChildUsers

            //        List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
            //                this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

            //        //گرفتن ایدی های دسترسی مشاورین
            //        var levelIds = consoleBusiness.CmsDb.Levels.Where(c =>
            //        c.LevelName.Contains(("مدیر املاک")) ||
            //        c.LevelName.Contains("مشاور املاک - کاربر و زیر گروه ها") ||
            //        c.LevelName.Contains("مشاور املاک - فقط خود کاربر") ||
            //        c.LevelName.Contains("مشاور املاک - فقط زیر گروه ها") ||
            //        c.LevelName.Contains("مشاور املاک - والد و زیر گروه ها")).Select(c => c.LevelId).ToList();

            //        //گرفتن ایدی کاربرانی که دسترسی مشاورین دارند
            //        var userIds = consoleBusiness.CmsDb.UsersLevels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.UserId).ToList();

            //        //گرفتن لیست کاربران
            //        //var usersList = consoleBusiness.CmsDb.Users.Where(c => c.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
            //        //                userIds.Contains(c.UserId)).AsQueryable();

            //        var usersList = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).AsQueryable();

            //        //اعمال شرط دسترسی
            //        if (childsUsersIds != null)
            //        {
            //            if (childsUsersIds.Count > 1)
            //            {
            //                usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value) || childsUsersIds.Contains(c.ParentUserId.Value) || childsUsersIds.Contains(c.UserId));

            //                ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
            //                {
            //                    UserId = s.UserId,
            //                    Name = s.UsersProfileUser.Name,
            //                    Family = s.UsersProfileUser.Family,
            //                    UserName = s.UserName,
            //                    Phone = s.UsersProfileUser.Phone,
            //                    Mobile = s.UsersProfileUser.Mobile,

            //                }).ToList();
            //            }
            //            else
            //            {
            //                if (childsUsersIds.Count == 1)
            //                {
            //                    if (childsUsersIds.FirstOrDefault() > 0)
            //                    {
            //                        usersList = usersList.Where(c =>
            //                        childsUsersIds.Contains(c.UserIdCreator.Value) ||
            //                        childsUsersIds.Contains(c.ParentUserId.Value) ||
            //                        childsUsersIds.Contains(c.UserId));

            //                        ViewData["AgencyStaffsList"] = usersList.Select(s => new CustomUsersVM
            //                        {
            //                            UserId = s.UserId,
            //                            Name = s.UsersProfileUser.Name,
            //                            Family = s.UsersProfileUser.Family,
            //                            UserName = s.UserName,
            //                            Phone = s.UsersProfileUser.Phone,
            //                            Mobile = s.UsersProfileUser.Mobile,

            //                        }).ToList();
            //                    }

            //                }
            //                else if (childsUsersIds.Count == 0)
            //                {
            //                    List<CustomUsersVM> usersList2 = new List<CustomUsersVM>();

            //                    ViewData["AgencyStaffsList"] = usersList2;
            //                }
            //            }
            //        }



            //        #endregion


            //    }

            //    catch (Exception exc)
            //    { }



            //}


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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
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
            #endregion

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

                                propertiesVM.StrOfferPrice = propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().OfferPrice.ToString();
                                propertiesVM.StrCalculatedOfferPrice = propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().CalculatedOfferPrice.ToString();

                                propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().StrOfferPrice = propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().OfferPrice.ToString();
                                propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().StrCalculatedOfferPrice = propertiesVM.PropertiesPricesHistoriesVM.FirstOrDefault().CalculatedOfferPrice.ToString();

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

            return View("Update", expando);
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult UpdateProperties(PropertiesVM propertiesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                propertiesVM.EditEnDate = DateTime.Now;
                propertiesVM.EditTime = PersianDate.TimeNow;
                propertiesVM.UserIdEditor = this.userId.Value;
                propertiesVM.OfferPrice = long.Parse(propertiesVM.StrOfferPrice.Replace(",", ""));
                propertiesVM.CalculatedOfferPrice = long.Parse(propertiesVM.StrCalculatedOfferPrice.Replace(",", ""));
                try
                {
                    if (propertiesVM.PropertiesDetailsVM != null)
                        if (!propertiesVM.PropertiesDetailsVM.BuildingLifeId.HasValue)
                            propertiesVM.PropertiesDetailsVM.BuildingLifeId = 0;
                    ModelState.Remove("PropertiesDetailsVM.BuildingLifeId");
                    
                }
                catch (Exception exc)
                {
                    propertiesVM.PropertiesDetailsVM.BuildingLifeId = 0;
                    ModelState.Remove("PropertiesDetailsVM.BuildingLifeId");
                }
                ModelState.Remove("PropertiesDetailsVM.Participable");
                ModelState.Remove("PropertiesDetailsVM.Exchangeable");
                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/UpdateProperties";

                    UpdatePropertiesPVM updateFormPVM = new UpdatePropertiesPVM()
                    {
                        PropertiesVM = propertiesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateProperties(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                propertiesVM = jObject.ToObject<PropertiesVM>();

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
        public IActionResult ToggleActivationProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/ToggleActivationProperties";

                ToggleActivationPropertiesPVM toggleActivationPropertiesPVM =
                    new ToggleActivationPropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationProperties(toggleActivationPropertiesPVM);

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
        public IActionResult TemporaryDeleteProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/TemporaryDeleteProperties";

                TemporaryDeletePropertiesPVM toggleActivationPropertiesPVM =
                    new TemporaryDeletePropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteProperties(toggleActivationPropertiesPVM);

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
        public IActionResult CompleteDeleteProperties(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/CompleteDeleteProperties";

                CompleteDeletePropertiesPVM deletePropertiesPVM =
                    new CompleteDeletePropertiesPVM()
                    {
                        PropertyId = PropertyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteProperties(deletePropertiesPVM);

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

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetPropertyWithPropertyId(long propertyId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetPropertyWithPropertyId";

                GetPropertyWithPropertyIdPVM getPropertyWithPropertyIdPVM = new GetPropertyWithPropertyIdPVM()
                {
                    PropertyId = propertyId,

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
                            var Record = jObject.ToObject<PropertiesVM>();

                            #region Fill UserCreatorName

                            var userIdCreator = Convert.ToInt64(jsonResultWithRecordObjectVM.Record.UserIdCreator);
                            //var customUser = consoleBusiness.GetCustomUser(userIdCreator);
                            CustomUsersVM user = consoleBusiness.GetMultiLevelsUserWithUserId(userIdCreator);

                            //if (user.LevelName.Equals("آگهی دهنده") && user.RoleName.Equals("Users") && user.DomainName.Equals("melkavan.com"))
                            if (
                                (user.LevelName.Equals("آگهی دهنده") ||
                                ((user.LevelNames != null) ? user.LevelNames.Contains("آگهی دهنده") : false)) &&
                                user.RoleName.Equals("Users"))
                            {


                                Record.CustomUsersVM = new CustomUsersVM();
                                Record.CustomUsersVM.UserName = user.UserName;
                                Record.CustomUsersVM.Family = user.Family;


                            }

                            #endregion

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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                propertyTypesVMList = jArray.ToObject<List<PropertyTypesVM>>();


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

                ViewData["PropertyTypesList"] = propertyTypesVMList;
            }

            if (ViewData["ElementTypesList"] == null)
            {
                List<ElementTypesVM> elementTypesVMList = new List<ElementTypesVM>();

                GetAllElementTypesListPVM getAllElementTypesListPVM = new GetAllElementTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    string serviceUrl = publicApiUrl + "/api/ElementTypesManagement/GetAllElementTypesList";

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllElementTypesList(getAllElementTypesListPVM);

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

            if (ViewData["FeaturesCategoriesList"] == null)
            {
                List<FeaturesCategoriesVM> featuresCategoriesVMList = new List<FeaturesCategoriesVM>();

                GetAllFeaturesCategoriesListPVM getAllFeaturesCategoriesListPVM = new GetAllFeaturesCategoriesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/FeaturesCategoriesManagement/GetAllFeaturesCategoriesList";
                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllFeaturesCategoriesList(getAllFeaturesCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                featuresCategoriesVMList = jArray.ToObject<List<FeaturesCategoriesVM>>();


                                if (featuresCategoriesVMList != null)
                                    if (featuresCategoriesVMList.Count > 0)
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

                ViewData["featuresCategoriesList"] = featuresCategoriesVMList;
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
            string? featureTitleSearch = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/GetListOfFeatures";

                GetListOfFeaturesPVM getListOfFeaturesPVM = new GetListOfFeaturesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
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
        public IActionResult GetAllFeaturesList(
            int? propertyTypeId = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/GetAllFeaturesList";

                GetAllFeaturesListPVM getAllFeaturesListPVM = new GetAllFeaturesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    PropertyTypeId = propertyTypeId,
                    FeatureTitleSearch = ""
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllFeaturesList(getAllFeaturesListPVM);

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
        public IActionResult GetFeatureWithFeatureId(int FeatureId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/GetFeatureWithFeatureId";

                GetFeatureWithFeatureIdPVM getFeatureWithFeatureIdPVM = new GetFeatureWithFeatureIdPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "FeaturesManagement", "GetFeatureWithFeatureId", this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    FeatureId = FeatureId,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetFeatureWithFeatureId(getFeatureWithFeatureIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<FeaturesVM>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = record,
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
                featuresVM.IsDeleted = false;
                featuresVM.IsActivated = true;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/AddToFeatures";

                    AddToFeaturesPVM addToFormPVM = new AddToFeaturesPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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
                featuresVM.IsDeleted = false;
                featuresVM.IsActivated = true;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/FeaturesManagement/UpdateFeatures";

                    UpdateFeaturesPVM updateFormPVM = new UpdateFeaturesPVM()
                    {
                        FeaturesVM = featuresVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
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


        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToFeaturesCategories(FeaturesCategoriesVM featuresCategoriesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                featuresCategoriesVM.CreateEnDate = DateTime.Now;
                featuresCategoriesVM.CreateTime = PersianDate.TimeNow;
                featuresCategoriesVM.UserIdCreator = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/FeaturesCategoriesManagement/CreateFeaturesCategories";

                    CreateFeaturesCategoriesPVM CreateFeaturesCategoriesPVM = new CreateFeaturesCategoriesPVM()
                    {
                        FeaturesCategoriesVM = featuresCategoriesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).CreateFeaturesCategories(CreateFeaturesCategoriesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
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
                                    Result = "OK",
                                    Id = jsonResultWithRecordObjectVM.Record
                                });
                            }
                            else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR"))
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
                        Record = featuresCategoriesVM,
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

        #endregion

        #region CompareProperties management

        public IActionResult CompareProperties()
        {
            ViewData["Title"] = "مقایسه املاک";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["ComparePropertiesListByPersonId"] == null)
            {
                List<ComparePropertiesByPersonIdVM> comparePropertiesByPersonIdVMList = new List<ComparePropertiesByPersonIdVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/ComparePropertiesManagement/GetAllComparePropertiesListByPersonId";
                    GetAllComparePropertiesListByPersonIdPVM getAllComparePropertiesListByPersonIdPVM = new GetAllComparePropertiesListByPersonIdPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllComparePropertiesListByPersonId(getAllComparePropertiesListByPersonIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                comparePropertiesByPersonIdVMList = jArray.ToObject<List<ComparePropertiesByPersonIdVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ComparePropertiesListByPersonId"] = comparePropertiesByPersonIdVMList;
            }


            //if (ViewData["PropertyTypesList"] == null)
            //{
            //    List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

            //    try
            //    {
            //        serviceUrl = teniacoApiUrl + "/api/PropertyTypesManagement/GetAllPropertyTypesList";

            //        responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertyTypesList();

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    propertyTypesVMList = jArray.ToObject<List<PropertyTypesVM>>();


            //                    if (propertyTypesVMList != null)
            //                        if (propertyTypesVMList.Count > 0)
            //                        {
            //                            //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                            //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                            //foreach (var record in records)
            //                            //{
            //                            //    if (record.UserIdCreator.HasValue)
            //                            //    {
            //                            //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                            //        if (customUser != null)
            //                            //        {
            //                            //            record.UserCreatorName = customUser.UserName;

            //                            //            if (!string.IsNullOrEmpty(customUser.Name))
            //                            //                record.UserCreatorName += " " + customUser.Name;

            //                            //            if (!string.IsNullOrEmpty(customUser.Family))
            //                            //                record.UserCreatorName += " " + customUser.Family;
            //                            //        }
            //                            //    }
            //                            //}

            //                            //statesVMList = records;
            //                        }

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["PropertyTypesList"] = propertyTypesVMList;
            //}



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

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertyTypesList"] = propertyTypesVMList;
            }


            ViewData["SearchTitle"] = "OK";

            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfComparePropertiesForBasicInfo(long propertyId)
        {

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            //JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/ComparePropertiesManagement/GetListOfComparePropertiesForBasicInfo";

                GetListOfComparePropertiesForBasicInfoPVM getListOfComparePropertiesForBasicInfoPVM = new GetListOfComparePropertiesForBasicInfoPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    PropertyId = propertyId
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfComparePropertiesForBasicInfo(getListOfComparePropertiesForBasicInfoPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {

                            //JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            //var records = jArray.ToObject<List<ComparePropertiesForBasicInfoVM>>();


                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var _record = jObject.ToObject<ComparePropertiesForBasicInfoVM>();

                            return Json(new { Result = "OK", Record = _record });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json(new { Result = "Err" });
            }
            return Json(new { Result = "Error" });

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfCompareFeatureValues(long propertyId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/ComparePropertiesManagement/GetListOfCompareFeatureValues";

                GetListOfCompareFeatureValuesPVM getListOfCompareFeatureValuesPVM = new GetListOfCompareFeatureValuesPVM()
                {
                    PropertyId = propertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfCompareFeatureValues(getListOfCompareFeatureValuesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var _record = jArray.ToObject<List<CompareFeatureValuesVM>>();


                            return Json(new { Result = "OK", Record = _record });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json(new { Result = "Err" });
            }
            return Json(new { Result = "Error" });


        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfComparePropertiesAddress(long propertyId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/ComparePropertiesManagement/GetListOfComparePropertiesAddress";

                GetListOfComparePropertiesAddressPVM getListOfComparePropertiesAddressPVM = new GetListOfComparePropertiesAddressPVM()
                {
                    PropertyId = propertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfComparePropertiesAddress(getListOfComparePropertiesAddressPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var _record = jArray.ToObject<List<ComparePropertiesAddressVM>>();


                            return Json(new { Result = "OK", Record = _record });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json(new { Result = "Err" });
            }
            return Json(new { Result = "Error" });


        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfComparePropertiesPricesHistories(long propertyId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/ComparePropertiesManagement/GetListOfComparePropertiesPricesHistories";

                GetListOfComparePropertiesPricesHistoriesPVM getListOfComparePropertiesPricesHistoriesPVM = new GetListOfComparePropertiesPricesHistoriesPVM()
                {
                    PropertyId = propertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfComparePropertiesPricesHistories(getListOfComparePropertiesPricesHistoriesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var _record = jArray.ToObject<List<ComparePropertiesPricesHistoriesVM>>();


                            return Json(new { Result = "OK", Record = _record });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json(new { Result = "Err" });
            }
            return Json(new { Result = "Error" });


        }



        #endregion

        #region Show In Melkavan

        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationShowInMelkavan(long PropertyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/ToggleActivationShowInMelkavan";

                ToggleActivationShowInMelkavanPVM toggleActivationShowInMelkavanPVM =
                    new ToggleActivationShowInMelkavanPVM()
                    {
                        PropertyId = PropertyId,
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationShowInMelkavan(toggleActivationShowInMelkavanPVM);

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
        public IActionResult AddPropertiesInMelkavan(PropertiesInMelkavanVM propertiesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                #region comments
                //if(propertiesVM.CustomUsersVM.UserName != null || propertiesVM.CustomUsersVM.UserName != "")
                //{

                //    #region Add User

                //    long userId = 0;
                //    var getLevel = consoleBusiness.GetLevelDetailWithLevelName("آگهی دهنده");
                //    var getUsersRoleId = consoleBusiness.GetRoleIdWithRoleName("Users");


                //    var customUsersVM = new CustomUsersVM()
                //    {
                //        UserName = propertiesVM.CustomUsersVM.UserName,
                //        DomainSettingId = this.domainsSettings.DomainSettingId,
                //        ParentUserId = this.userId,
                //        Email = "",
                //        Password = RandomCode(),
                //        ReplyPassword = RandomCode(),
                //        HasModified = true,
                //        Sexuality = true,
                //        LevelId = getLevel.LevelId,
                //        RoleId = getUsersRoleId,
                //        IsActivated = true,
                //        IsDeleted = false
                //    };

                //    customUsersVM.UserId = consoleBusiness.CreateUser(customUsersVM);

                //    userId = customUsersVM.UserId;

                //    var UserProfieVM = new UsersProfileVM()
                //    {
                //        UserId = userId,
                //        Name = propertiesVM.CustomUsersVM.UserName,
                //        Family = propertiesVM.CustomUsersVM.Family != "" ? propertiesVM.CustomUsersVM.Family : propertiesVM.CustomUsersVM.UserName,
                //        Email = "",
                //        Phone = propertiesVM.CustomUsersVM.UserName,
                //        Mobile = propertiesVM.CustomUsersVM.UserName,
                //        Address = "",
                //        Age = 0,
                //        BirthDateTimeEn = DateTime.Now,
                //        CertificateId = "",
                //        CreateEnDate = DateTime.Now,
                //        CreateTime = PersianDate.TimeNow,
                //        CreditCardNumber = "",
                //        HasModified = false,
                //        IsActivated = true,
                //        IsDeleted = false,
                //        NationalCode = "",
                //        Picture = "",
                //        PostalCode = "",
                //        Sexuality = false,
                //        SocialNetworkAddress = "",
                //        UniqueKey = "",
                //        UserIdCreator = userId,
                //    };

                //    consoleBusiness.AddToUsersProfile(UserProfieVM);

                //    propertiesVM.UserIdCreator = userId;

                //    #endregion

                //}
                #endregion

                propertiesVM.CreateEnDate = DateTime.Now;
                propertiesVM.CreateTime = PersianDate.TimeNow;
                propertiesVM.IsActivated = true;
                propertiesVM.IsDeleted = false;
                propertiesVM.UserIdEditor = this.userId.Value;

                ModelState.Remove("Convertable");

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/AddPropertiesInMelkavan";

                    AddPropertiesInMelkavanPVM addPropertiesInMelkavanPVM = new AddPropertiesInMelkavanPVM()
                    {
                        PropertiesInMelkavanVM = propertiesVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddPropertiesInMelkavan(addPropertiesInMelkavanPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<PropertiesInMelkavanVM>();

                                if (record != null)
                                {
                                    propertiesVM = record;

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
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }
        #endregion

        //public string RandomCode()
        //{
        //    Random rd = new Random();
        //    int randomNumber = rd.Next(1000, 9999);
        //    return randomNumber.ToString();
        //}
    }
}

using ApiCallers.MelkavanApiCaller;
using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Authorization;
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
using System.IO;
using System.Linq;
using VM.Base;
using VM.Console;
using VM.Melkavan;
using VM.Melkavan.PVM.Melkavan.Tags;
using VM.Public;
using VM.PVM.Melkavan;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class AdvertisementQuickRegistrationManagementController : ExtraAdminController
    {
        public AdvertisementQuickRegistrationManagementController(IHostEnvironment _hostEnvironment,
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


                ViewData["OwnersList"] = usersList.OrderByDescending(c => c.CreateEnDate).Select(s => new CustomUsersVM
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

        public void GetListOfCounsultants()
        {
            List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
            try
            {
                List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                //گرفتن ایدی های دسترسی مشاور
                var levelIds = consoleBusiness.CmsDb.Levels.Where(c => c.LevelName.Contains("مشاور")).Select(c => c.LevelId).ToList();

                //گرفتن ایدی کاربرانی که دسترسی مشاور دارند
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


                ViewData["ConsultantsList"] = usersList.OrderByDescending(c => c.CreateEnDate).Select(s => new CustomUsersVM
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


        public IActionResult Index()
        {


            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            ViewData["Title"] = "ثبت فوری ملک";

            // مالک
            if (ViewData["OwnersList"] == null)
            {
                GetListofOwnerUsersList();
            }


            //مشاور
            if (ViewData["ConsultantsList"] == null)
            {
                GetListOfCounsultants();
            }


            //لیبل ها
            if (ViewData["TagsList"] == null)
            {
                List<TagsVM> tagsVMList = new List<TagsVM>();

                GetAllTagsListPVM getAllTagsListPVM = new GetAllTagsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };
                try
                {
                    serviceUrl = melkavanApiUrl + "/api/TagsManagement/GetAllTagsList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllTagsList(getAllTagsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                tagsVMList = jArray.ToObject<List<TagsVM>>();

                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["TagsList"] = tagsVMList;
            }


            //نوع آگهی
            if (ViewData["AdvertisementTypesList"] == null)
            {
                List<AdvertisementTypesVM> advertisementTypesList = new List<AdvertisementTypesVM>();

                GetAllAdvertisementTypesListPVM getAllAdvertisementTypesListPVM = new GetAllAdvertisementTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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


            //نوع ملک
            if (ViewData["PropertyTypesList"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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


            //نوع کاربری
            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM = new GetAllTypeOfUsesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["TypeOfUsesList"] = typeOfUsesVMList;
            }


            //استان
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


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["StatesList"] = statesVMList;
            }


            //بخش
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

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["CitiesList"] = citiesVMList;
            }


            //شهر یا منطقه
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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //   this.domainsSettings.DomainSettingId),
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

            //نوع سند
            if (ViewData["DocumentTypesList"] == null)
            {
                List<DocumentTypesVM> documentTypesVMList = new List<DocumentTypesVM>();

                GetAllDocumentTypesListPVM getAllDocumentTypesListPVM = new GetAllDocumentTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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


            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index/";
            }
            return View("Index");
        }



        #region AddOwnerOrConcultant

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddOwnerOrConcultant(string? Name, string? Family, string? Phone, string? Type)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            CustomUsersVM customUsersVM = new CustomUsersVM();
            try
            {


                string serviceUrl = melkavanApiUrl + "/api/AdvertisementQuickRegistrationManagement/AddOwnerOrConcultant";

                AddOwnerOrConcultantPVM addToUsersPVM = new AddOwnerOrConcultantPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminMelkavan", "AdvertisementQuickRegistrationManagement", "AddOwnerOrConcultant", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    Name = Name,
                    Family = Family,
                    Phone = Phone,
                    Type = Type,
                    UserIdCreator = this.userId.Value,
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).AddOwnerOrConcultant(addToUsersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            customUsersVM = jObject.ToObject<CustomUsersVM>();


                            return Json(new
                            {
                                Result = "OK",
                                Message = jsonResultWithRecordObjectVM.Message,
                                Records = customUsersVM,
                            });
                        }
                        else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                            jsonResultWithRecordObjectVM.Message.Equals("DuplicateUser"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "کاربر تکراری است"
                            });
                        }
                        else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                            jsonResultWithRecordObjectVM.Message.Equals("ERRORConsultant"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "شماره موبایل وارد شده در پلتفرم به عنوان مشاور تعریف شده است. امکان ثبت مالک را ندارید."
                            });
                        }
                        else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                            jsonResultWithRecordObjectVM.Message.Equals("DupliacateConsultant"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "شماره موبایل وارد شده در پلتفرم به عنوان مشاور تعریف شده است. امکان ثبت دوباره آن وجود ندارد."
                            });
                        }
                        else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                            jsonResultWithRecordObjectVM.Message.Equals("DuplicateOwner"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "شماره موبایل وارد شده در پلتفرم به عنوان مالک تعریف شده است. امکان ثبت دوباره آن وجود ندارد."
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطایی رخ داده است"
                            });
                        }
                    }
                }

            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "خطا",
                Message = "خطایی رخ داده است"
            });
        }

        #endregion

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllOwnersList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {

                GetAllOwnersListPVM getAllOwnersListPVM = new GetAllOwnersListPVM() { };

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


                    getAllOwnersListPVM.CustomUsersVMList = usersList.OrderByDescending(c=>c.CreateEnDate).Select(s => new CustomUsersVM
                    {
                        UserId = s.UserId,
                        Name = s.UsersProfileUser.Name,
                        Family = s.UsersProfileUser.Family,
                        UserName = s.UserName,
                        Phone = s.UsersProfileUser.Phone,
                        Mobile = s.UsersProfileUser.Mobile,

                    }).ToList();


                    //ViewData["OwnersList"] = usersList.Select(s => new CustomUsersVM
                    //{
                    //    UserId = s.UserId,
                    //    Name = s.UsersProfileUser.Name,
                    //    Family = s.UsersProfileUser.Family,
                    //    UserName = s.UserName,
                    //    Phone = s.UsersProfileUser.Phone,
                    //    Mobile = s.UsersProfileUser.Mobile,

                    //}).ToList();



                    return Json(new
                    {
                        Result = "OK",
                        Records = getAllOwnersListPVM.CustomUsersVMList

                    });

                }

                catch (Exception exc)
                { }

                
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
        public IActionResult GetAllConsultantsList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {

                GetAllCosultantsListPVM getAllCosultantsListPVM = new GetAllCosultantsListPVM() { };
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
                try
                {
                    List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                    //گرفتن ایدی های دسترسی مشاور
                    var levelIds = consoleBusiness.CmsDb.Levels.Where(c => c.LevelName.Contains("مشاور")).Select(c => c.LevelId).ToList();

                    //گرفتن ایدی کاربرانی که دسترسی مشاور دارند
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


                    getAllCosultantsListPVM.CustomUsersVMList = usersList.OrderByDescending(c => c.CreateEnDate).Select(s => new CustomUsersVM
                    {
                        UserId = s.UserId,
                        Name = s.UsersProfileUser.Name,
                        Family = s.UsersProfileUser.Family,
                        UserName = s.UserName,
                        Phone = s.UsersProfileUser.Phone,
                        Mobile = s.UsersProfileUser.Mobile,

                    }).ToList();


                    return Json(new
                    {
                        Result = "OK",
                        Records = getAllCosultantsListPVM.CustomUsersVMList,

                    });

                }

                catch (Exception exc)
                { }

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
        [AllowAnonymous]
        public IActionResult AdvertisementQuickRegistration([FromForm] AdvertisementVM advertisementVM/*, List<IFormFile> filesList*/)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            var advertisementFilesVM = new AdvertisementFilesVM();

            try
            {
                var files = advertisementVM.Files;
                string fileName = "";
                string ext = "";

                List<TemporaryAdvertisementFilesVM> myFiles = new List<TemporaryAdvertisementFilesVM>();

                if (files != null)
                {
                    if (files.Count > 0)
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);
                        domainSettings.DomainName = "melkavan.com";

                        advertisementVM.AdvertisementFilesVM = new List<AdvertisementFilesVM>();

                        foreach (var file in files)
                        {
                            try
                            {
                                ext = Path.GetExtension(file.FileName).ToLower();
                                fileName = Guid.NewGuid().ToString() + ext;

                                advertisementFilesVM = new AdvertisementFilesVM()
                                {
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = this.userId.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                    AdvertisementFileExt = ext,
                                    AdvertisementFilePath = fileName,
                                    AdvertisementFileTitle = file.FileName,
                                    AdvertisementFileType = "media",
                                    AdvertisementId = 1,
                                    AdvertisementFileOrder = 1,
                                };


                                advertisementVM.AdvertisementFilesVM.Add(advertisementFilesVM);

                                myFiles.Add(new TemporaryAdvertisementFilesVM()
                                {
                                    AdvertisementFilePath = advertisementFilesVM.AdvertisementFilePath,
                                    MyFile = file
                                });
                            }
                            catch (Exception exc)
                            { }
                        }
                    }
                }

                advertisementVM.PublishType = "Quick";
                advertisementVM.CreateEnDate = DateTime.Now;
                advertisementVM.CreateTime = PersianDate.TimeNow;
                advertisementVM.UserIdCreator = this.userId.Value;
                advertisementVM.IsActivated = true;
                advertisementVM.IsDeleted = false;


                if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId == 2)//فروش
                {
                    ModelState.Remove("AdvertisementDetailsVM.MaritalStatusId");
                    ModelState.Remove("AdvertisementPricesHistoriesVM.RentPrice");
                    ModelState.Remove("AdvertisementPricesHistoriesVM.DepositPrice");
                    ModelState.Remove("AdvertisementDetailsVM.Convertable");

                    if (advertisementVM.DocumentOwnershipTypeId == null)
                    {
                        advertisementVM.DocumentOwnershipTypeId = 1;
                    } 

                }
                else //اجاره
                {
                    ModelState.Remove("AdvertisementPricesHistoriesVM.OfferPrice");
                }



                ModelState.Remove("BuiltInYear");
                ModelState.Remove("BuiltInYearFa");
                ModelState.Remove("AdvertisementDetailsVM.BuildingLifeId");
                ModelState.Remove("AdvertisementDetailsVM.Foundation");
                ModelState.Remove("AdvertisementAddressVM.Address");
                ModelState.Remove("Files");
                ModelState.Remove("AdvertisementPricesHistoriesVM.CalculatedOfferPrice");
                ModelState.Remove("AdvertisementDetailsVM.MaritalStatusId");
                ModelState.Remove("DocumentTypeId");
                ModelState.Remove("DocumentRootTypeId");
                ModelState.Remove("DocumentOwnershipTypeId");


                advertisementVM.Files = null;
                advertisementVM.AdvertisementOwnersVM.OwnerId = advertisementVM.AdvertisementOwnersVM.OwnerId;
                advertisementVM.AdvertisementOwnersVM.Share = 6;
                advertisementVM.AdvertisementOwnersVM.SharePercent = 100;


                if (ModelState.IsValid)
                {
                    string serviceUrl = melkavanApiUrl + "/api/AdvertisementQuickRegistrationManagement/AdvertisementQuickRegistration";

                    AddToAdvertisementPVM addToAdvertisementPVM = new AddToAdvertisementPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        AdvertisementVM = advertisementVM
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).AddToAdvertisement(addToAdvertisementPVM);

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

                                    #region create needed folders

                                    if (advertisementVM.AdvertisementId > 0)
                                    {
                                        #region create root folder for this advertisement

                                        try
                                        {
                                            if (myFiles != null)
                                            {
                                                if (myFiles.Count > 0)
                                                {
                                                    advertisementFilesVM.AdvertisementId = advertisementVM.AdvertisementId;

                                                    foreach (var item in advertisementVM.AdvertisementFilesVM)
                                                    {
                                                        item.AdvertisementId = advertisementVM.AdvertisementId;
                                                    }


                                                    //var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                                                    string advertisementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\AdvertisementsFiles\\" + "melkavan.com" + "\\" + advertisementVM.AdvertisementId + "\\Media";

                                                    if (!Directory.Exists(advertisementFolder))
                                                    {
                                                        Directory.CreateDirectory(advertisementFolder);
                                                    }

                                                    foreach (var myFile in myFiles)
                                                    {
                                                        try
                                                        {
                                                            using (var fileStream = new FileStream(advertisementFolder + "\\" + myFile.AdvertisementFilePath, FileMode.Create))
                                                            {
                                                                myFile.MyFile.CopyToAsync(fileStream);
                                                                System.Threading.Thread.Sleep(100);
                                                            }

                                                            string tmpExt = Path.GetExtension(myFile.AdvertisementFilePath);

                                                            if (tmpExt.Equals(".jpeg") ||
                                                                 tmpExt.Equals(".jpg") ||
                                                                 tmpExt.Equals(".png") ||
                                                                 tmpExt.Equals(".gif") ||
                                                                 tmpExt.Equals(".bmp"))
                                                            {
                                                                ImageModify.ResizeImage(advertisementFolder + "\\",
                                                                    myFile.AdvertisementFilePath,
                                                                    120,
                                                                    120);
                                                            }

                                                        }
                                                        catch (Exception exc)
                                                        { }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                    }

                                    #endregion

                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = advertisementVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateAdvertisement"))
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



        //صفحه ی  ویرایش آگهی
        [AllowAnonymous]
        public IActionResult UpdateAdvertisement(int Id)
        {
            ViewData["Title"] = "ویرایش آگهی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            string userDevicePlatform = FrameWork.PlatformInfo.IsMobile(Request.Headers["User-Agent"].ToString()).Equals(true) ? "mobile" : "desktop";
            if (ViewData["UserDevicePlatform"] == null)
            {
                ViewData["UserDevicePlatform"] = userDevicePlatform;
            }

            if (ViewData["CurrentYear"] == null)
            {
                ViewData["CurrentYear"] = PersianDate.ThisYear;
            }


            // مالک
            if (ViewData["OwnersList"] == null)
            {
                GetListofOwnerUsersList();
            }


            //مشاور
            if (ViewData["ConsultantsList"] == null)
            {
                GetListOfCounsultants();
            }

            //لیبل ها
            if (ViewData["TagsList"] == null)
            {
                List<TagsVM> tagsVMList = new List<TagsVM>();

                GetAllTagsListPVM getAllTagsListPVM = new GetAllTagsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };
                try
                {
                    serviceUrl = melkavanApiUrl + "/api/TagsManagement/GetAllTagsList";

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllTagsList(getAllTagsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                tagsVMList = jArray.ToObject<List<TagsVM>>();

                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["TagsList"] = tagsVMList;
            }

            //نوع آگهی
            if (ViewData["AdvertisementTypesList"] == null)
            {
                List<AdvertisementTypesVM> advertisementTypesList = new List<AdvertisementTypesVM>();

                GetAllAdvertisementTypesListPVM getAllAdvertisementTypesListPVM = new GetAllAdvertisementTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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


            //نوع ملک
            if (ViewData["PropertyTypesList"] == null)
            {
                List<PropertyTypesVM> propertyTypesVMList = new List<PropertyTypesVM>();

                GetAllPropertyTypesListPVM getAllPropertyTypesListPVM = new GetAllPropertyTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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


            //نوع کاربری
            if (ViewData["TypeOfUsesList"] == null)
            {
                List<TypeOfUsesVM> typeOfUsesVMList = new List<TypeOfUsesVM>();

                GetAllTypeOfUsesListPVM getAllTypeOfUsesListPVM = new GetAllTypeOfUsesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //       this.domainsSettings.DomainSettingId),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
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


            AdvertisementVM advertisementVM = new AdvertisementVM();


            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = melkavanApiUrl + "/api/AdvertisementManagement/getAdvertisementWithAdvertisementId";

                GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM = new GetAdvertisementWithAdvertisementIdPVM()
                {
                    AdvertisementId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    Type = "Advertisement"
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
                                //advertisementVM.AdvertisementFilesVM = advertisementVM.AdvertisementFilesVM.OrderByDescending(a => a.AdvertisementFileId).ToList();
                                advertisementVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            advertisementVM.AdvertisementId = Id;

            ViewData["Advertisement"] = advertisementVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index/";
            }

            return View("Index");

        }


        [HttpPost]
        [AjaxOnly]
        [AllowAnonymous]
        public IActionResult UpdateAdvertisement([FromForm] AdvertisementVM advertisementVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                advertisementVM.UserIdEditor = this.userId.Value;

                if (advertisementVM.AdvertisementDetailsVM.AdvertisementTypeId == 2)//فروش
                {
                    ModelState.Remove("AdvertisementDetailsVM.MaritalStatusId");
                    ModelState.Remove("AdvertisementPricesHistoriesVM.RentPrice");
                    ModelState.Remove("AdvertisementPricesHistoriesVM.DepositPrice");
                    ModelState.Remove("AdvertisementDetailsVM.Convertable");

                    if (advertisementVM.DocumentOwnershipTypeId == null)
                    {
                        advertisementVM.DocumentOwnershipTypeId = 1;
                    }

                }
                else //اجاره
                {
                    ModelState.Remove("AdvertisementPricesHistoriesVM.OfferPrice");
                }



                ModelState.Remove("BuiltInYear");
                ModelState.Remove("BuiltInYearFa");
                ModelState.Remove("AdvertisementDetailsVM.BuildingLifeId");
                ModelState.Remove("AdvertisementDetailsVM.Foundation");
                ModelState.Remove("AdvertisementAddressVM.Address");
                ModelState.Remove("Files");
                ModelState.Remove("AdvertisementPricesHistoriesVM.CalculatedOfferPrice");
                ModelState.Remove("AdvertisementDetailsVM.MaritalStatusId");
                ModelState.Remove("DocumentTypeId");
                ModelState.Remove("DocumentRootTypeId");
                ModelState.Remove("DocumentOwnershipTypeId");


                advertisementVM.Files = null;
                advertisementVM.AdvertisementFilesVM = null;
                advertisementVM.AdvertisementAddressVM = null;
                advertisementVM.AdvertisementFeaturesValuesVM = null;
                advertisementVM.RecordType = "Advertisement";


                if (ModelState.IsValid)
                {
                    string serviceUrl = melkavanApiUrl + "/api/AdvertisementManagement/UpdateAdvertisement";

                    AddToAdvertisementPVM addToAdvertisementPVM = new AddToAdvertisementPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                         this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        AdvertisementVM = advertisementVM
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).AddToAdvertisement(addToAdvertisementPVM);

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

                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = advertisementVM,
                                        Message = "ویرایش انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateAdvertisement"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = ""
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


    }
}

using ApiCallers.PublicApiCaller;
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
using Web.Core.Controllers;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class ZonesManagementController : ExtraAdminController
    {
        public ZonesManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست مناطق";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

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
                    serviceUrl = publicApiUrl + "/api/CitiesManagement/GetAllCitiesList";

                    GetAllCitiesListPVM getAllCitiesListPVM = new GetAllCitiesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId)
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
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId)
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

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllZonesList(
             long? stateId = null,
            long? cityId = null,
            string searchTitle = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetAllZonesList";

                GetAllZonesListPVM getAllZonesListPVM = new GetAllZonesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),


                    StateId = stateId,
                    CityId = cityId,
                    SearchTitle = searchTitle,
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
                            var records = jArray.ToObject<List<ZonesVM>>();

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
        public IActionResult GetListOfZones(int jtStartIndex = 0,
            int jtPageSize = 10,
            long? stateId = null,
            long? cityId = null,
            string searchTitle = null,
            string jtSorting = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetListOfZones";

                GetListOfZonesPVM getListOfZonesPVM = new GetListOfZonesPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    StateId = ((stateId.HasValue) ? stateId.Value : (long?)0),
                    CityId = ((cityId.HasValue) ? cityId.Value : (long?)0),
                    SearchTitle = (!string.IsNullOrEmpty(searchTitle) ? searchTitle.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfZones(getListOfZonesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ZonesVM>>();

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
        public IActionResult GetListOfZonesWithOutStrPolygon(int jtStartIndex = 0,
           int jtPageSize = 10,
           long? stateId = null,
           long? cityId = null,
           string searchTitle = null,
           string jtSorting = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetListOfZonesWithOutStrPolygon";

                GetListOfZonesPVM getListOfZonesPVM = new GetListOfZonesPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    StateId = ((stateId.HasValue) ? stateId.Value : (long?)0),
                    CityId = ((cityId.HasValue) ? cityId.Value : (long?)0),
                    SearchTitle = (!string.IsNullOrEmpty(searchTitle) ? searchTitle.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfZones(getListOfZonesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ZonesVM>>();

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
        public IActionResult AddToZones(ZonesVM zonesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                zonesVM.CreateEnDate = DateTime.Now;
                zonesVM.CreateTime = PersianDate.TimeNow;
                zonesVM.UserIdCreator = this.userId.Value;

                //zonesVM.ZonesType = "text";

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/ZonesManagement/AddToZones";

                    AddToZonesPVM addToFormPVM = new AddToZonesPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        ZonesVM = zonesVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).AddToZones(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<ZonesVM>();

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


                                if (record != null)
                                {
                                    zonesVM = record;

                                    #region create needed folders

                                    if (zonesVM.ZoneId > 0)
                                    {
                                        #region create domain folder

                                        string zonesFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ZoneFiles\\";

                                        var domainSettings = consoleBusiness.GetDomainsSettingsWithDomainSettingId(this.domainsSettings.DomainSettingId, this.userId.Value);

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(zonesFolder + "\\" + domainSettings.DomainName))
                                            {
                                                System.IO.Directory.CreateDirectory(zonesFolder + "\\" + domainSettings.DomainName);
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create root folder for this district

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId))
                                            {
                                                System.IO.Directory.CreateDirectory(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId);
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create maps folder

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Maps"))
                                            {
                                                System.IO.Directory.CreateDirectory(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Maps");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create docs folder

                                        try
                                        {
                                            if (!System.IO.Directory.Exists(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Docs"))
                                            {
                                                System.IO.Directory.CreateDirectory(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Docs");
                                                System.Threading.Thread.Sleep(100);
                                            }
                                        }
                                        catch (Exception exc)
                                        { }

                                        #endregion

                                        #region create media folder

                                        if (!System.IO.Directory.Exists(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Media"))
                                        {
                                            System.IO.Directory.CreateDirectory(zonesFolder + "\\" + domainSettings.DomainName + "\\" + zonesVM.ZoneId + "\\Media");
                                            System.Threading.Thread.Sleep(100);
                                        }

                                        #endregion
                                    }

                                    #endregion


                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = zonesVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }


                                //return Json(new
                                //{
                                //    Result = jsonResultWithRecordObjectVM.Result,
                                //    Record = zonesVM,
                                //});


                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateZone"))
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

        [HttpPost]
        [AjaxOnly]
        public IActionResult UpdateZones(ZonesVM zonesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                zonesVM.EditEnDate = DateTime.Now;
                zonesVM.EditTime = PersianDate.TimeNow;
                zonesVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/ZonesManagement/UpdateZones";

                    UpdateZonesPVM updateFormPVM = new UpdateZonesPVM()
                    {
                        ZonesVM = zonesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).UpdateZones(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                zonesVM = jObject.ToObject<ZonesVM>();

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
                                    Record = zonesVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateZone"))
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
                        Record = zonesVM,
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
        public IActionResult ToggleActivationZones(int ZoneId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/ToggleActivationZones";

                ToggleActivationZonesPVM toggleActivationZonesPVM =
                    new ToggleActivationZonesPVM()
                    {
                        ZoneId = ZoneId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationZones(toggleActivationZonesPVM);

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
        public IActionResult TemporaryDeleteZones(int ZoneId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/TemporaryDeleteZones";

                TemporaryDeleteZonesPVM toggleActivationZonesPVM =
                    new TemporaryDeleteZonesPVM()
                    {
                        ZoneId = ZoneId,
                        UserId = this.userId.Value,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).TemporaryDeleteZones(toggleActivationZonesPVM);

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
        public IActionResult CompleteDeleteZones(int ZoneId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/CompleteDeleteZones";

                CompleteDeleteZonesPVM deleteZonesPVM =
                    new CompleteDeleteZonesPVM()
                    {
                        ZoneId = ZoneId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeleteZones(deleteZonesPVM);

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
    }
}

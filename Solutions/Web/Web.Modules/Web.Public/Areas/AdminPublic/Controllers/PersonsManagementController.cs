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
    public class PersonsManagementController : ExtraAdminController
    {
        public PersonsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست مالکین/واسطه ها";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            //if (ViewData["StatesList"] == null)
            //{
            //    //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

            //    List<StatesVM> statesVMList = new List<StatesVM>();

            //    try
            //    {
            //        string serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

            //        GetListOfStatesPVM getAllStatesListPVM =
            //            new GetListOfStatesPVM()
            //            {
            //                //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
            //            };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getAllStatesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    //var records = jsonResultWithRecordsObjectVM.Records as List<StatesVM>;
            //                    //var records = JsonConvert.DeserializeObject<List<StatesVM>>(jsonResultWithRecordsObjectVM.Records);

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    var records = jArray.ToObject<List<StatesVM>>();

            //                    if (records.Count > 0)
            //                    {
            //                        var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                        var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                        foreach (var record in records)
            //                        {
            //                            if (record.UserIdCreator.HasValue)
            //                            {
            //                                var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                                if (customUser != null)
            //                                {
            //                                    record.UserCreatorName = customUser.UserName;

            //                                    if (!string.IsNullOrEmpty(customUser.Name))
            //                                        record.UserCreatorName += " " + customUser.Name;

            //                                    if (!string.IsNullOrEmpty(customUser.Family))
            //                                        record.UserCreatorName += " " + customUser.Family;
            //                                }
            //                            }
            //                        }
            //                    }

            //                    #endregion

            //                    statesVMList = records;
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
            //        string serviceUrl = publicApiUrl + "/api/CitiesManagement/GetListOfCitiesWithOutStrPolygon";

            //        GetListOfCitiesPVM getAllCitiesListPVM =
            //            new GetListOfCitiesPVM()
            //            {
            //                ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
            //                this.domainsSettings.DomainSettingId),
            //            };

            //        responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfCities(getAllCitiesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    //var records = jsonResultWithRecordsObjectVM.Records as List<CitiesVM>;
            //                    //var records = JsonConvert.DeserializeObject<List<CitiesVM>>(jsonResultWithRecordsObjectVM.Records);

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    var records = jArray.ToObject<List<CitiesVM>>();

            //                    if (records.Count > 0)
            //                    {
            //                        var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
            //                        var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

            //                        foreach (var record in records)
            //                        {
            //                            if (record.UserIdCreator.HasValue)
            //                            {
            //                                var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
            //                                if (customUser != null)
            //                                {
            //                                    record.UserCreatorName = customUser.UserName;

            //                                    if (!string.IsNullOrEmpty(customUser.Name))
            //                                        record.UserCreatorName += " " + customUser.Name;

            //                                    if (!string.IsNullOrEmpty(customUser.Family))
            //                                        record.UserCreatorName += " " + customUser.Family;
            //                                }
            //                            }
            //                        }
            //                    }

            //                    #endregion

            //                    citiesVMList = records;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["CitiesList"] = citiesVMList;
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

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllPersonsList(
            string searchText)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    SearchText = searchText,
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
                            var records = jArray.ToObject<List<PersonsVM>>();

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
        public IActionResult GetListOfPersons(string searchText, int jtStartIndex = 0, int jtPageSize = 0)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/GetListOfPersons";

                GetListOfPersonsPVM getListOfPersonsPVM = new GetListOfPersonsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtPageSize = jtPageSize,
                    jtStartIndex = jtStartIndex,
                    SearchText = searchText,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfPersons(getListOfPersonsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            //var records = jsonResultWithRecordsObjectVM.Records as List<PersonssVM>;
                            //var records = JsonConvert.DeserializeObject<List<PersonssVM>>(jsonResultWithRecordsObjectVM.Records);

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<PersonsVM>>();

                            //JObject jObject = jsonResultWithRecordsObjectVM.Records;
                            //var records = jObject.ToObject<List<PersonsVM>>();

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
                Message = "ErrorMessage"
            });
        }

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
                //menusVM.Lang = this.currentLanguage;

                //menusVM.PersonsType = "text";

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
        public IActionResult UpdatePersons(PersonsVM personsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                personsVM.EditEnDate = DateTime.Now;
                personsVM.EditTime = PersianDate.TimeNow;
                personsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/PersonsManagement/UpdatePersons";

                    UpdatePersonsPVM updateFormPVM = new UpdatePersonsPVM()
                    {
                        PersonsVM = personsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).UpdatePersons(updateFormPVM);

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

                    return Json(new
                    {
                        Result = jsonResultWithRecordObjectVM.Result,
                        Record = personsVM,
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
        public IActionResult ToggleActivationPersons(int PersonId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/ToggleActivationPersons";

                ToggleActivationPersonsPVM toggleActivationPersonsPVM =
                    new ToggleActivationPersonsPVM()
                    {
                        PersonId = PersonId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationPersons(toggleActivationPersonsPVM);

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
        public IActionResult TemporaryDeletePersons(int PersonId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //if (institutionsBusiness.CompleteDeletePersons(EducationLevelId, this.userId.Value))
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
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentPersonsErrorMessage").FirstOrDefault().Value
                //    });
                //}

                string serviceUrl = publicApiUrl + "/api/PersonsManagement/TemporaryDeletePersons";

                TemporaryDeletePersonsPVM temporaryDeletePersonsPVM =
                    new TemporaryDeletePersonsPVM()
                    {
                        PersonId = PersonId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).
                    TemporaryDeletePersons(temporaryDeletePersonsPVM);

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
                        Message = "DeleteCurrentPersonsErrorMessage"
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
        public IActionResult CompleteDeletePersons(int PersonId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/CompleteDeletePersons";

                CompleteDeletePersonsPVM deletePersonsPVM =
                    new CompleteDeletePersonsPVM()
                    {
                        PersonId = PersonId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeletePersons(deletePersonsPVM);

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
                Message = "ErrorMessage"
            });
        }
    }
}

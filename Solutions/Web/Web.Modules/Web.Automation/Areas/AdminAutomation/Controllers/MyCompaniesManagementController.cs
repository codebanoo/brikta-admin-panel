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
//using ApiCallers.AutomationApiCaller;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VM.Console;
using VM.Automation;
using VM.PVM.Automation;
using Web.Core.Controllers;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using ApiCallers.PublicApiCaller;
using System.Dynamic;
using System.IO;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class MyCompaniesManagementController : ExtraAdminController
    {
        public MyCompaniesManagementController(IHostEnvironment _hostEnvironment,
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
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["StatesList"] == null)
            {
                //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                List<StatesVM> statesVMList = new List<StatesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

                    GetListOfStatesPVM getAllStatesListPVM =
                        new GetListOfStatesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getAllStatesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<StatesVM>;
                                //var records = JsonConvert.DeserializeObject<List<StatesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<StatesVM>>();

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

                                statesVMList = records;
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
                    string serviceUrl = publicApiUrl + "/api/CitiesManagement/GetListOfCities";

                    GetListOfCitiesPVM getAllCitiesListPVM =
                        new GetListOfCitiesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            //this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfCities(getAllCitiesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<CitiesVM>;
                                //var records = JsonConvert.DeserializeObject<List<CitiesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<CitiesVM>>();

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

                                citiesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["CitiesList"] = citiesVMList;
            }



            if (ViewData["SearchTitle"] == null)
                ViewData["SearchTitle"] = "OK";


            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetListOfMyCompanies(
            string address,
            string commercialCode,
            string myCompanyName,
            string phones,
            string myCompanyRealName,
            string postalCode,
            string faxes,
            string registerNumber,
            string nationalCode,
            long? cityId,
            long? stateId,
            int jtStartIndex,
            int jtPageSize,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();

                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetListOfMyCompanies";

                //GetListOfMyCompaniesPVM getListOfMyCompaniesPVM = new GetListOfMyCompaniesPVM()
                //{
                //    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId),
                //    jtStartIndex = jtStartIndex,
                //    jtPageSize = jtPageSize,
                //    jtSorting = jtSorting,
                //    "fa",
                //    Address = address,
                //    CommercialCode = commercialCode,
                //    MyCompanyName = myCompanyName,
                //    Phones = phones,
                //    MyCompanyRealName = myCompanyRealName,
                //    PostalCode = postalCode,
                //    Faxes = faxes,
                //    RegisterNumber = registerNumber,
                //    NationalCode = nationalCode,
                //    CityId = cityId,
                //    StateId = stateId
                //};

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetListOfMyCompanies(getListOfMyCompaniesPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<MyCompaniesVM>>();

                //            if (records != null)
                //            {
                //                if (records.Count > 0)
                //                {
                //                    var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();

                //                    var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //                    foreach (var record in records)
                //                    {
                //                        if (record.UserIdCreator.HasValue)
                //                        {
                //                            var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                //                            if (customUser != null)
                //                            {
                //                                record.UserCreatorName = customUser.UserName;

                //                                if (!string.IsNullOrEmpty(customUser.Name))
                //                                    record.UserCreatorName += " " + customUser.Name;

                //                                if (!string.IsNullOrEmpty(customUser.Family))
                //                                    record.UserCreatorName += " " + customUser.Family;
                //                            }
                //                        }

                //                    }
                //                }

                //                myCompaniesVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = myCompaniesVMList,
                //                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                //            });
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult AddToMyCompanies()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["StatesList"] == null)
            {
                //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                List<StatesVM> statesVMList = new List<StatesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

                    GetListOfStatesPVM getAllStatesListPVM =
                        new GetListOfStatesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getAllStatesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<StatesVM>;
                                //var records = JsonConvert.DeserializeObject<List<StatesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<StatesVM>>();

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

                                statesVMList = records;
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
                    string serviceUrl = publicApiUrl + "/api/CitiesManagement/GetListOfCities";

                    GetListOfCitiesPVM getAllCitiesListPVM =
                        new GetListOfCitiesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            //this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfCities(getAllCitiesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<CitiesVM>;
                                //var records = JsonConvert.DeserializeObject<List<CitiesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<CitiesVM>>();

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

                                citiesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["CitiesList"] = citiesVMList;
            }


            if (ViewData["UsersList"] == null)
            {

                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }


            if (ViewData["DomainName"] == null)
            {
                ViewData["DomainName"] = this.domain;
            }

            MyCompaniesVM myCompaniesVM = new MyCompaniesVM();
            myCompaniesVM.IsActivated = true;
            myCompaniesVM.IsDeleted = false;

            //return View(themeName /*this.theme.ThemeName*/ + direction + "AddToNewsCategory", cityiesVM);
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminAutomation/MyCompaniesManagement/Index";
            }
            dynamic expando = new ExpandoObject();
            expando = myCompaniesVM;

            return View("AddTo", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToMyCompanies(MyCompaniesVM myCompaniesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                myCompaniesVM.CreateEnDate = DateTime.Now;
                myCompaniesVM.CreateTime = PersianDate.TimeNow;
                myCompaniesVM.UserIdCreator = this.userId.Value;

                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/AddToMyCompanies";

                AddToMyCompaniesPVM addToMyCompaniesPVM = new AddToMyCompaniesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //            this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    //
                    MyCompaniesVM = myCompaniesVM
                };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).AddToMyCompanies(addToMyCompaniesPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            int? record = (int?)jsonResultWithRecordObjectVM.Record;

                //            if (record != null)
                //                if (record.Value > 0)
                //                {
                //                    myCompaniesVM.MyCompanyId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                MyCompanyId = myCompaniesVM.MyCompanyId
                //            });
                //        }
                //    }
                //}
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
        public IActionResult GetMyCompanyWithMyCompanyId(int MyCompanyId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            MyCompaniesVM myCompaniesVM = new MyCompaniesVM();

            try
            {
                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetMyCompanyWithMyCompanyId";

                GetMyCompanyWithMyCompanyIdPVM getMyCompanyWithMyCompanyIdPVM =
                    new GetMyCompanyWithMyCompanyIdPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //        this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        MyCompanyId = MyCompanyId,
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetMyCompanyWithMyCompanyId(getMyCompanyWithMyCompanyIdPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                //            var record = jObject.ToObject<MyCompaniesVM>();

                //            if (record != null)
                //            {
                //                myCompaniesVM = record;

                //                #region Fill UserCreatorName

                //                if (myCompaniesVM.UserIdCreator.HasValue)
                //                {
                //                    var customUser = consoleBusiness.GetCustomUser(myCompaniesVM.UserIdCreator.Value);

                //                    if (customUser != null)
                //                    {
                //                        myCompaniesVM.UserCreatorName = customUser.UserName;

                //                        if (!string.IsNullOrEmpty(customUser.Name))
                //                            myCompaniesVM.UserCreatorName += " " + customUser.Name;

                //                        if (!string.IsNullOrEmpty(customUser.Family))
                //                            myCompaniesVM.UserCreatorName += " " + customUser.Family;
                //                    }
                //                }

                //                #endregion
                //            }
                //            return Json(new
                //            {
                //                Result = "OK",
                //                MyCompaniesVM = myCompaniesVM
                //            }); ;
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult UpdateMyCompanies(int Id = 0)
        {
            MyCompaniesVM myCompaniesVM = new MyCompaniesVM();
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetMyCompanyWithMyCompanyId";

                GetMyCompanyWithMyCompanyIdPVM getMyCompanyWithMyCompanyIdPVM =
                    new GetMyCompanyWithMyCompanyIdPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //        this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        MyCompanyId = Id,
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetMyCompanyWithMyCompanyId(getMyCompanyWithMyCompanyIdPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                //            var record = jObject.ToObject<MyCompaniesVM>();

                //            if (record != null)
                //            {
                //                myCompaniesVM = record;

                //                #region Fill UserCreatorName

                //                if (myCompaniesVM.UserIdCreator.HasValue)
                //                {
                //                    var customUser = consoleBusiness.GetCustomUser(myCompaniesVM.UserIdCreator.Value);

                //                    if (customUser != null)
                //                    {
                //                        myCompaniesVM.UserCreatorName = customUser.UserName;

                //                        if (!string.IsNullOrEmpty(customUser.Name))
                //                            myCompaniesVM.UserCreatorName += " " + customUser.Name;

                //                        if (!string.IsNullOrEmpty(customUser.Family))
                //                            myCompaniesVM.UserCreatorName += " " + customUser.Family;
                //                    }
                //                }

                //                #endregion
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }

            if (ViewData["StatesList"] == null)
            {
                //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                List<StatesVM> statesVMList = new List<StatesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/StatesManagement/GetListOfStates";

                    GetListOfStatesPVM getAllStatesListPVM =
                        new GetListOfStatesPVM()
                        {
                            ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfStates(getAllStatesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<StatesVM>;
                                //var records = JsonConvert.DeserializeObject<List<StatesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<StatesVM>>();

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

                                statesVMList = records;
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
                    string serviceUrl = publicApiUrl + "/api/CitiesManagement/GetListOfCities";

                    GetListOfCitiesPVM getAllCitiesListPVM =
                        new GetListOfCitiesPVM()
                        {
                            ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            this.domainsSettings.DomainSettingId),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfCities(getAllCitiesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                //var records = jsonResultWithRecordsObjectVM.Records as List<CitiesVM>;
                                //var records = JsonConvert.DeserializeObject<List<CitiesVM>>(jsonResultWithRecordsObjectVM.Records);

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<CitiesVM>>();

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

                                citiesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["CitiesList"] = citiesVMList;
            }


            if (ViewData["UsersList"] == null)
            {
                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }

            if (ViewData["DomainName"] == null)
            {
                ViewData["DomainName"] = this.domain;
            }
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminAutomation/MyCompaniesManagement/Index";
            }
            dynamic expando = new ExpandoObject();
            expando = myCompaniesVM;

            return View("Update", expando);

            //return View(themeName /*this.theme.ThemeName*/ + direction + "UpdateNewsCategory", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateMyCompanies(MyCompaniesVM myCompaniesVM)
        {
            try
            {
                myCompaniesVM.EditEnDate = DateTime.Now;
                myCompaniesVM.EditTime = PersianDate.TimeNow;
                myCompaniesVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/UpdateMyCompanies";

                    UpdateMyCompaniesPVM updateMyCompaniesPVM =
                        new UpdateMyCompaniesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            //this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            MyCompaniesVM = myCompaniesVM,
                        };

                    //responseApiCaller = new AutomationApiCaller(serviceUrl).
                    //    UpdateMyCompanies(updateMyCompaniesPVM);

                    //if (responseApiCaller.IsSuccessStatusCode)
                    //{
                    //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    //    if (jsonResultWithRecordObjectVM != null)
                    //    {
                    //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                    //        {
                    //            //var record = jsonResultWithRecordObjectVM.Record as AlumnusCoursesVM;
                    //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                    //            var record = jObject.ToObject<MyCompaniesVM>();

                    //            if (record != null)
                    //            {
                    //                myCompaniesVM = record;
                    //                return Json(new
                    //                {
                    //                    Result = "OK",
                    //                    Message = "Success",
                    //                    MyCompanyId = myCompaniesVM.MyCompanyId
                    //                });
                    //            }
                    //        }
                    //    }
                    //}
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
        public IActionResult ToggleActivationMyCompanies(int myCompanyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl +
                    "/api/MyCompaniesManagement/ToggleActivationMyCompanies";

                ToggleActivationMyCompaniesPVM toggleActivationMyCompaniesPVM =
                    new ToggleActivationMyCompaniesPVM()
                    {
                        MyCompanyId = myCompanyId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    ToggleActivationMyCompanies(toggleActivationMyCompaniesPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }
                //}
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
        public IActionResult TemporaryDeleteMyCompanies(int myCompanyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/TemporaryDeleteMyCompanies";

                TemporaryDeleteMyCompaniesPVM temporaryDeleteMyCompaniesPVM =
                    new TemporaryDeleteMyCompaniesPVM()
                    {
                        MyCompanyId = myCompanyId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    TemporaryDeleteMyCompanies(temporaryDeleteMyCompaniesPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }

                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentAlumnusCoursesErrorMessage").FirstOrDefault().Value
                //    });
                //}
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
        public IActionResult CompleteDeleteMyCompanies(int myCompanyId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/CompleteDeleteMyCompanies";

                CompleteDeleteMyCompaniesPVM completeDeleteMyCompaniesPVM =
                    new CompleteDeleteMyCompaniesPVM()
                    {
                        MyCompanyId = myCompanyId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    CompleteDeleteMyCompanies(completeDeleteMyCompaniesPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }

                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentMyCompaniesErrorMessage").FirstOrDefault().Value
                //    });
                //}
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
        public async Task<ActionResult> UploadFile(IFormFile CompanyLogo, IFormFile WaterMarkImage, int MyCompanyId)
        {
            try
            {
                if (CompanyLogo != null || WaterMarkImage != null)
                {
                    string companyLogoName = "";
                    string waterMarkImageName = "";
                    string ext = "";

                    string myCompanyFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\MyCompanies\\";

                    string oldCompanyLogoName = "";
                    string oldWaterMarkImageName = "";

                    try
                    {   //TODO
                        serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetMyCompaniesImages";

                        GetMyCompaniesImagesPVM getMyCompaniesImagesPVM = new GetMyCompaniesImagesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            MyCompanyId = MyCompanyId
                        };

                        //responseApiCaller = new AutomationApiCaller(serviceUrl).GetMyCompaniesImages(getMyCompaniesImagesPVM);

                        //if (responseApiCaller.IsSuccessStatusCode)
                        //{
                        //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        //    if (jsonResultWithRecordObjectVM != null)
                        //    {
                        //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        //        {

                        //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                        //            var record = jObject.ToObject<MyCompaniesVM>();

                        //            oldCompanyLogoName = record.CompanyLogo;
                        //            oldWaterMarkImageName = record.WaterMarkImage;
                        //        }
                        //    }
                        //}
                    }
                    catch (Exception exc)
                    { }

                    if (CompanyLogo != null)
                        if (CompanyLogo.Length > 0)
                        {
                            #region Remove Old company Logo

                            if (CompanyLogo != null)
                            {
                                if (!string.IsNullOrEmpty(oldCompanyLogoName))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" + MyCompanyId + "\\" + oldCompanyLogoName))
                                        {
                                            System.IO.File.Delete(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                MyCompanyId + "\\" + oldCompanyLogoName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }

                                    try
                                    {
                                        if (System.IO.File.Exists(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" + MyCompanyId + "\\thumb_" + oldCompanyLogoName))
                                        {
                                            System.IO.File.Delete(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                MyCompanyId + "\\thumb_" + oldCompanyLogoName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }
                                }
                            }

                            #endregion

                            #region image
                            string path = myCompanyFolder + this.domainsSettings.DomainName + "\\" + MyCompanyId + "\\";

                            ext = Path.GetExtension(CompanyLogo.FileName);
                            companyLogoName = Guid.NewGuid().ToString() + ext;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(path + companyLogoName, FileMode.Create))
                            {
                                await CompanyLogo.CopyToAsync(fileStream);
                                System.Threading.Thread.Sleep(100);
                            }

                            ImageModify.ResizeImage(path, companyLogoName, 40, 40);

                            #endregion
                        }


                    if (WaterMarkImage != null)
                        if (WaterMarkImage.Length > 0)
                        {
                            #region Remove Old waterMark Image

                            if (WaterMarkImage != null)
                            {
                                if (!string.IsNullOrEmpty(oldWaterMarkImageName))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" + MyCompanyId + "\\" + oldWaterMarkImageName))
                                        {
                                            System.IO.File.Delete(myCompanyFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                MyCompanyId + "\\" + oldWaterMarkImageName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }

                                }
                            }

                            #endregion

                            #region files

                            string path = myCompanyFolder + this.domainsSettings.DomainName + "\\" + MyCompanyId + "\\";

                            ext = Path.GetExtension(WaterMarkImage.FileName);
                            waterMarkImageName = Guid.NewGuid().ToString() + ext;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(path + waterMarkImageName, FileMode.Create))
                            {
                                await WaterMarkImage.CopyToAsync(fileStream);
                                System.Threading.Thread.Sleep(100);
                            }

                            ImageModify.ResizeImage(path, waterMarkImageName, 40, 40);

                            #endregion
                        }
                    if (!string.IsNullOrEmpty(companyLogoName) || !string.IsNullOrEmpty(waterMarkImageName))
                    {
                        try
                        {
                            serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/UpdateCompanyPictures";

                            VM.PVM.Automation.UpdateCompanyPicturesPVM UpdateCompanyPicturesPVM = new VM.PVM.Automation.UpdateCompanyPicturesPVM()
                            {
                                //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                                ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                                this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                                MyCompanyId = MyCompanyId,
                                CompanyLogo = companyLogoName,
                                WaterMarkImage = waterMarkImageName
                            };

                            //responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateCompanyPictures(UpdateCompanyPicturesPVM);

                            //if (responseApiCaller.IsSuccessStatusCode)
                            //{
                            //    var jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                            //    if (jsonResultObjectVM != null)
                            //    {
                            //        if (jsonResultObjectVM.Result.Equals("OK"))
                            //        {
                            //            return Json(new
                            //            {
                            //                Result = "OK",
                            //                CompanyLogo = companyLogoName,
                            //                Message = "Success"
                            //            });
                            //        }
                            //    }
                            //}

                        }
                        catch (Exception exc)
                        { }
                    }
                }
                return Json(new
                {
                    Result = "ERROR",
                    Message = ""
                });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }
    }
}

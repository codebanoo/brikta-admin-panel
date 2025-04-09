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

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class MyDepartmentsManagementController : ExtraAdminController
    {
        public MyDepartmentsManagementController(IHostEnvironment _hostEnvironment,
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

            if (ViewData["MyCompaniesList"] == null)
            {
                //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();

                try
                {
                    string serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetAllMyCompaniesList";

                    GetAllMyCompaniesListPVM getAllMyCompaniesListPVM =
                        new GetAllMyCompaniesListPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    //responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllMyCompaniesList(getAllMyCompaniesListPVM);

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

                    //            //if (records.Count > 0)
                    //            //{
                    //            //    var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                    //            //    var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                    //            //    foreach (var record in records)
                    //            //    {
                    //            //        if (record.UserIdCreator.HasValue)
                    //            //        {
                    //            //            var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                    //            //            if (customUser != null)
                    //            //            {
                    //            //                record.UserCreatorName = customUser.UserName;

                    //            //                if (!string.IsNullOrEmpty(customUser.Name))
                    //            //                    record.UserCreatorName += " " + customUser.Name;

                    //            //                if (!string.IsNullOrEmpty(customUser.Family))
                    //            //                    record.UserCreatorName += " " + customUser.Family;
                    //            //            }
                    //            //        }
                    //            //    }
                    //            //}

                    //            #endregion

                    //            myCompaniesVMList = records;
                    //        }
                    //    }
                    //}
                }
                catch (Exception exc)
                { }

                ViewData["MyCompaniesList"] = myCompaniesVMList;
            }

            if (ViewData["UsersList"] == null)
            {
                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }

            return View("Index");
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllMyDepartmentsList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                List<MyDepartmentsVM> myDepartmentsVMList = new List<MyDepartmentsVM>();

                serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/GetAllMyDepartmentsList";

                GetAllMyDepartmentsListPVM getAllMyDepartmentsListPVM =
                    new GetAllMyDepartmentsListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetAllMyDepartmentsList(getAllMyDepartmentsListPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<MyDepartmentsVM>>();

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

                //                myDepartmentsVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = myDepartmentsVMList,
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
        public IActionResult GetListOfMyDepartments(int jtStartIndex, int jtPageSize, string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                List<MyDepartmentsVM> myDepartmentsVMList = new List<MyDepartmentsVM>();

                serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/GetListOfMyDepartments";

                GetListOfMyDepartmentsPVM getListOfMyDepartmentsPVM =
                    new GetListOfMyDepartmentsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        jtSorting = jtSorting,
                        
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetListOfMyDepartments(getListOfMyDepartmentsPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            //var records = jsonResultWithRecordsObjectVM.Records as List<AlumnusCoursesVM>;
                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<MyDepartmentsVM>>();

                //            //var records = JsonConvert.DeserializeObject<List<AlumnusCoursesVM>>(jsonResultWithRecordsObjectVM.Records);

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

                //                myDepartmentsVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = myDepartmentsVMList,
                //                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                //            });
                //        }
                //    }
                //}

                if (ViewData["MyCompaniesList"] == null)
                {
                    //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                    List<MyCompaniesVM> myCompaniesVMList = new List<MyCompaniesVM>();

                    try
                    {
                        string serviceUrl = automationApiUrl + "/api/MyCompaniesManagement/GetAllMyCompaniesList";

                        GetAllMyCompaniesListPVM getAllMyCompaniesListPVM =
                            new GetAllMyCompaniesListPVM()
                            {
                                //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                                ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            };

                        //responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllMyCompaniesList(getAllMyCompaniesListPVM);

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


                        //            #endregion

                        //            myCompaniesVMList = records;
                        //        }
                        //    }
                        //}
                    }
                    catch (Exception exc)
                    { }

                    ViewData["StatesList"] = myCompaniesVMList;
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
        public IActionResult AddToMyDepartments(MyDepartmentsVM myDepartmentsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/AddToMyDepartments";

                myDepartmentsVM.CreateEnDate = DateTime.Now;
                myDepartmentsVM.CreateTime = PersianDate.TimeNow;
                myDepartmentsVM.UserIdCreator = this.userId.Value;

                AddToMyDepartmentsPVM addToMyDepartmentsPVM =
                    new AddToMyDepartmentsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //        this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        MyDepartmentsVM = myDepartmentsVM
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).AddToMyDepartments(addToMyDepartmentsPVM);

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
                //            {
                //                if (record.Value > 0)
                //                {
                //                    myDepartmentsVM.MyDepartmentId = record.Value;
                //                }

                //                return Json(new
                //                {
                //                    Result = "OK",
                //                    Record = myDepartmentsVM,
                //                    Message = "Success",
                //                });
                //                //return Json(new
                //                //{
                //                //    Result = "OK",
                //                //    Message = "Success",
                //                //    MyDepartmentId = myDepartmentsVM.MyDepartmentId
                //                //});
                //            }
                //            else
                //            {
                //                return Json(new
                //                {
                //                    Result = "ERROR",
                //                    Message = "Duplicate"
                //                });
                //            }
                //            #endregion


                //        }
                //    }

                //}

                //int listCount = 0;

                //var EducationalMaterialsList = institutionsBusiness.GetEducationalMaterialsList(jtStartIndex,
                //    jtPageSize,
                //    ref listCount,
                //    jtSorting,
                //    this.userId.Value);

                //return Json(new { Result = "OK", Records = EducationalMaterialsList, TotalRecordCount = listCount });
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
        public IActionResult UpdateMyDepartments(MyDepartmentsVM myDepartmentsVM)
        {
            try
            {
                myDepartmentsVM.EditEnDate = DateTime.Now;
                myDepartmentsVM.EditTime = PersianDate.TimeNow;
                myDepartmentsVM.UserIdEditor = this.userId.Value;
                myDepartmentsVM.UserIdCreator = this.userId.Value;
                if (ModelState.IsValid)
                {
                    serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/UpdateMyDepartments";

                    UpdateMyDepartmentsPVM updateMyDepartmentsPVM =
                        new UpdateMyDepartmentsPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            //this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            MyDepartmentsVM = myDepartmentsVM,
                        };

                    //responseApiCaller = new AutomationApiCaller(serviceUrl).
                    //    UpdateMyDepartments(updateMyDepartmentsPVM);

                    //if (responseApiCaller.IsSuccessStatusCode)
                    //{
                    //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    //    if (jsonResultWithRecordObjectVM != null)
                    //    {
                    //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                    //        {
                    //            //var record = jsonResultWithRecordObjectVM.Record as AlumnusCoursesVM;
                    //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                    //            var record = jObject.ToObject<MyDepartmentsVM>();

                    //            if (record != null)
                    //            {
                    //                myDepartmentsVM = record;

                    //                return Json(new
                    //                {
                    //                    Result = jsonResultWithRecordObjectVM.Result,
                    //                    Record = myDepartmentsVM,
                    //                });

                    //                //return Json(new
                    //                //{
                    //                //    Result = "OK",
                    //                //    Message = "Success",
                    //                //    TimesLimitationId = myDepartmentsVM.MyDepartmentId
                    //                //});
                    //            }
                    //        }
                    //    }
                    //}

                    //if (institutionsBusiness.UpdateAlumnusCourses(ref AlumnusCoursesVM, this.userId.Value))
                    //{
                    //    return Json(new
                    //    {
                    //        Result = "OK",
                    //        Record = AlumnusCoursesVM
                    //    });
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
        public IActionResult ToggleActivationMyDepartments(int myDepartmentId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl +
                    "/api/MyDepartmentsManagement/ToggleActivationMyDepartments";

                ToggleActivationMyDepartmentsPVM toggleActivationMyDepartmentsPVM =
                    new ToggleActivationMyDepartmentsPVM()
                    {
                        MyDepartmentId = myDepartmentId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    ToggleActivationMyDepartments(toggleActivationMyDepartmentsPVM);

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
        public IActionResult TemporaryDeleteMyDepartments(int myDepartmentId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/TemporaryDeleteMyDepartments";

                TemporaryDeleteMyDepartmentsPVM temporaryDeleteMyDepartmentsPVM =
                    new TemporaryDeleteMyDepartmentsPVM()
                    {
                        MyDepartmentId = myDepartmentId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    TemporaryDeleteMyDepartments(temporaryDeleteMyDepartmentsPVM);

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
                //        Message = "DeleteCurrentMyDepartmentsErrorMessage"
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
        public IActionResult CompleteDeleteMyDepartments(int myDepartmentId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsManagement/CompleteDeleteMyDepartments";

                CompleteDeleteMyDepartmentsPVM completeDeleteMyDepartmentsPVM =
                    new CompleteDeleteMyDepartmentsPVM()
                    {
                        MyDepartmentId = myDepartmentId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    CompleteDeleteMyDepartments(completeDeleteMyDepartmentsPVM);

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
                //        Message = "DeleteCurrentMyDepartmentsErrorMessage"
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
    }
}

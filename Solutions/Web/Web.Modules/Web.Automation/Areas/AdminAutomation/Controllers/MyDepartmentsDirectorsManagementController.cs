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
    public class MyDepartmentsDirectorsManagementController : ExtraAdminController
    {
        public MyDepartmentsDirectorsManagementController(IHostEnvironment _hostEnvironment,
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllMyDepartmentsDirectorsList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                List<MyDepartmentsDirectorsVM> myDepartmentsDirectorsVMList = new List<MyDepartmentsDirectorsVM>();

                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/GetAllMyDepartmentsDirectorsList";

                GetAllMyDepartmentsDirectorsListPVM getAllMyDepartmentsDirectorsListPVM =
                    new GetAllMyDepartmentsDirectorsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetAllMyDepartmentsDirectorsList(getAllMyDepartmentsDirectorsListPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<MyDepartmentsDirectorsVM>>();

                //            if (records != null)
                //            {
                //                if (records.Count > 0)
                //                {
                //                    //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                //                    //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //                    //foreach (var record in records)
                //                    //{
                //                    //    if (record.UserIdCreator.HasValue)
                //                    //    {
                //                    //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                //                    //        if (customUser != null)
                //                    //        {
                //                    //            record.UserCreatorName = customUser.UserName;

                //                    //            if (!string.IsNullOrEmpty(customUser.Name))
                //                    //                record.UserCreatorName += " " + customUser.Name;

                //                    //            if (!string.IsNullOrEmpty(customUser.Family))
                //                    //                record.UserCreatorName += " " + customUser.Family;
                //                    //        }
                //                    //    }
                //                    //}
                //                }

                //                myDepartmentsDirectorsVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = myDepartmentsDirectorsVMList,
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
        public IActionResult GetListOfMyDepartmentsDirectors(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                List<MyDepartmentsDirectorsVM> myDepartmentsDirectorsVMList = new List<MyDepartmentsDirectorsVM>();

                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/GetListOfMyDepartmentsDirectors";

                GetListOfMyDepartmentsDirectorsPVM getListOfMyDepartmentsDirectorsPVM =
                    new GetListOfMyDepartmentsDirectorsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        jtSorting = jtSorting,
                        
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetListOfMyDepartmentsDirectors(getListOfMyDepartmentsDirectorsPVM);

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
                //            var records = jArray.ToObject<List<MyDepartmentsDirectorsVM>>();

                //            //var records = JsonConvert.DeserializeObject<List<AlumnusCoursesVM>>(jsonResultWithRecordsObjectVM.Records);

                //            if (records != null)
                //            {
                //                if (records.Count > 0)
                //                {
                //                    //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                //                    //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //                    //foreach (var record in records)
                //                    //{
                //                    //    if (record.UserIdCreator.HasValue)
                //                    //    {
                //                    //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                //                    //        if (customUser != null)
                //                    //        {
                //                    //            record.UserCreatorName = customUser.UserName;

                //                    //            if (!string.IsNullOrEmpty(customUser.Name))
                //                    //                record.UserCreatorName += " " + customUser.Name;

                //                    //            if (!string.IsNullOrEmpty(customUser.Family))
                //                    //                record.UserCreatorName += " " + customUser.Family;
                //                    //        }
                //                    //    }
                //                    //}
                //                }

                //                myDepartmentsDirectorsVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = myDepartmentsDirectorsVMList,
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

        [AjaxOnly]
        [HttpPost]

        public IActionResult AddToMyDepartmentsDirectors(MyDepartmentsDirectorsVM myDepartmentsDirectorsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/AddToMyDepartmentsDirectors";

                AddToMyDepartmentsDirectorsPVM addToMyDepartmentsDirectorsPVM =
                    new AddToMyDepartmentsDirectorsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //        this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        MyDepartmentsDirectorsVM = myDepartmentsDirectorsVM
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    AddToMyDepartmentsDirectors(addToMyDepartmentsDirectorsPVM);

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
                //                    myDepartmentsDirectorsVM.UserId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                MyDepartmentDirectorId = myDepartmentsDirectorsVM.UserId
                //            });
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
        public IActionResult GetMyDepartmentDirectorWithMyDepartmentDirectorId(int timesLimitationId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            MyDepartmentsDirectorsVM myDepartmentsDirectorsVM = new MyDepartmentsDirectorsVM();

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/GetMyDepartmentDirectorWithMyDepartmentDirectorId";

                GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM =
                    new GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //        this.domainsSettings.DomainSettingId),

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = timesLimitationId,
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetMyDepartmentDirectorWithMyDepartmentDirectorId(getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM);

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
                //                    myDepartmentsDirectorsVM.UserId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                MyDepartmentsDirectorsVM = myDepartmentsDirectorsVM
                //            }); ;
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
        public IActionResult UpdateMyDepartmentsDirectors(MyDepartmentsDirectorsVM myDepartmentsDirectorsVM)
        {
            try
            {
                myDepartmentsDirectorsVM.EditEnDate = DateTime.Now;
                myDepartmentsDirectorsVM.EditTime = PersianDate.TimeNow;
                myDepartmentsDirectorsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/UpdateMyDepartmentsDirectors";

                    UpdateMyDepartmentsDirectorsPVM updateMyDepartmentsDirectorsPVM =
                        new UpdateMyDepartmentsDirectorsPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            //this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            MyDepartmentsDirectorsVM = myDepartmentsDirectorsVM,
                        };

                    //responseApiCaller = new AutomationApiCaller(serviceUrl).
                    //    UpdateMyDepartmentsDirectors(updateMyDepartmentsDirectorsPVM);

                    //if (responseApiCaller.IsSuccessStatusCode)
                    //{
                    //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    //    if (jsonResultWithRecordObjectVM != null)
                    //    {
                    //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                    //        {
                    //            //var record = jsonResultWithRecordObjectVM.Record as AlumnusCoursesVM;
                    //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                    //            var record = jObject.ToObject<MyDepartmentsDirectorsVM>();

                    //            if (record != null)
                    //            {
                    //                myDepartmentsDirectorsVM = record;
                    //                return Json(new
                    //                {
                    //                    Result = "OK",
                    //                    Message = "Success",
                    //                    MyDepartmentDirectorId = myDepartmentsDirectorsVM.UserId
                    //                });
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
        public IActionResult ToggleActivationMyDepartmentsDirectors(int myDepartmentDirectorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl +
                    "/api/MyDepartmentsDirectorsManagement/ToggleActivationMyDepartmentsDirectors";

                ToggleActivationMyDepartmentsDirectorsPVM toggleActivationMyDepartmentsDirectorsPVM =
                    new ToggleActivationMyDepartmentsDirectorsPVM()
                    {
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    ToggleActivationMyDepartmentsDirectors(toggleActivationMyDepartmentsDirectorsPVM);

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
        public IActionResult TemporaryDeleteMyDepartmentsDirectors(int myDepartmentDirectorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/TemporaryDeleteMyDepartmentsDirectors";

                TemporaryDeleteMyDepartmentsDirectorsPVM temporaryDeleteMyDepartmentsDirectorsPVM =
                    new TemporaryDeleteMyDepartmentsDirectorsPVM()
                    {
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    TemporaryDeleteMyDepartmentsDirectors(temporaryDeleteMyDepartmentsDirectorsPVM);

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
                //        Message = "DeleteCurrentMyDepartmentsDirectorsErrorMessage"
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
        public IActionResult CompleteDeleteMyDepartmentsDirectors(int myDepartmentDirectorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                serviceUrl = automationApiUrl + "/api/MyDepartmentsDirectorsManagement/CompleteDeleteMyDepartmentsDirectors";

                CompleteDeleteMyDepartmentsDirectorsPVM completeDeleteMyDepartmentsDirectorsPVM =
                    new CompleteDeleteMyDepartmentsDirectorsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    CompleteDeleteMyDepartmentsDirectors(completeDeleteMyDepartmentsDirectorsPVM);

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
                //        Message = "DeleteCurrentMyDepartmentsDirectorsErrorMessage"
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

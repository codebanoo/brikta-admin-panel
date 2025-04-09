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
using ApiCallers.AutomationApiCaller;
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

namespace Web.Automation.Areas.UserAutomation.Controllers
{
    [Area("UserAutomation")]
    public class DepartmentsStaffManagementController : ExtraAdminController
    {
        public DepartmentsStaffManagementController(IHostEnvironment _hostEnvironment,
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
        public IActionResult GetAllDepartmentsStaffList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //List<DepartmentsStaffVM> departmentsStaffVMList = new List<DepartmentsStaffVM>();

                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/GetAllDepartmentsStaffList";

                //GetAllDepartmentsStaffListPVM getAllDepartmentsStaffListPVM =
                //    new GetAllDepartmentsStaffListPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId),
                //        
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetAllDepartmentsStaffList(getAllDepartmentsStaffListPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<DepartmentsStaffVM>>();

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

                //                departmentsStaffVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = departmentsStaffVMList,
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
        public IActionResult GetListOfDepartmentsStaff(int jtStartIndex,
            int jtPageSize,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //List<DepartmentsStaffVM> departmentsStaffVMList = new List<DepartmentsStaffVM>();

                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/GetListOfDepartmentsStaff";

                //GetListOfDepartmentsStaffPVM getListOfDepartmentsStaffPVM =
                //    new GetListOfDepartmentsStaffPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId),
                //        jtStartIndex = jtStartIndex,
                //        jtPageSize = jtPageSize,
                //        jtSorting = jtSorting,
                //        
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetListOfDepartmentsStaff(getListOfDepartmentsStaffPVM);

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
                //            var records = jArray.ToObject<List<DepartmentsStaffVM>>();

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

                //                departmentsStaffVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = departmentsStaffVMList,
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

        public IActionResult AddToDepartmentsStaff(DepartmentsStaffVM departmentsStaffVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/AddToDepartmentsStaff";

                //AddToDepartmentsStaffPVM addToDepartmentsStaffPVM =
                //    new AddToDepartmentsStaffPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        DepartmentsStaffVM = departmentsStaffVM
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    AddToDepartmentsStaff(addToDepartmentsStaffPVM);

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
                //                    departmentsStaffVM.DepartmentStaffId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                DepartmentStaffId = departmentsStaffVM.DepartmentStaffId
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
        public IActionResult GetDepartmentStaffWithDepartmentStaffId(int timesLimitationId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            DepartmentsStaffVM departmentsStaffVM = new DepartmentsStaffVM();

            try
            {
                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/GetDepartmentStaffWithDepartmentStaffId";

                //GetDepartmentStaffWithDepartmentStaffIdPVM getDepartmentStaffWithDepartmentStaffIdPVM =
                //    new GetDepartmentStaffWithDepartmentStaffIdPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        DepartmentStaffId = timesLimitationId,
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetDepartmentStaffWithDepartmentStaffId(getDepartmentStaffWithDepartmentStaffIdPVM);

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
                //                    departmentsStaffVM.DepartmentStaffId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                DepartmentsStaffVM = departmentsStaffVM
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateDepartmentsStaff(DepartmentsStaffVM departmentsStaffVM)
        {
            try
            {
                departmentsStaffVM.EditEnDate = DateTime.Now;
                departmentsStaffVM.EditTime = PersianDate.TimeNow;
                departmentsStaffVM.UserIdEditor = this.userId.Value;

                //if (ModelState.IsValid)
                //{
                //    serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/UpdateDepartmentsStaff";

                //    UpdateDepartmentsStaffPVM updateDepartmentsStaffPVM =
                //        new UpdateDepartmentsStaffPVM()
                //        {
                //            ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //            this.domainsSettings.DomainSettingId),
                //            DepartmentsStaffVM = departmentsStaffVM,
                //        };

                //    responseApiCaller = new AutomationApiCaller(serviceUrl).
                //        UpdateDepartmentsStaff(updateDepartmentsStaffPVM);

                //    if (responseApiCaller.IsSuccessStatusCode)
                //    {
                //        var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //        if (jsonResultWithRecordObjectVM != null)
                //        {
                //            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //            {
                //                //var record = jsonResultWithRecordObjectVM.Record as AlumnusCoursesVM;
                //                JObject jObject = jsonResultWithRecordObjectVM.Record;
                //                var record = jObject.ToObject<DepartmentsStaffVM>();

                //                if (record != null)
                //                {
                //                    departmentsStaffVM = record;
                //                    return Json(new
                //                    {
                //                        Result = "OK",
                //                        Message = "Success",
                //                        DepartmentStaffId = departmentsStaffVM.DepartmentStaffId
                //                    });
                //                }
                //            }
                //        }
                //    }

                //    //if (institutionsBusiness.UpdateAlumnusCourses(ref AlumnusCoursesVM, this.userId.Value))
                //    //{
                //    //    return Json(new
                //    //    {
                //    //        Result = "OK",
                //    //        Record = AlumnusCoursesVM
                //    //    });
                //    //}
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
        public IActionResult ToggleActivationDepartmentsStaff(int departmentStaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl +
                //    "/api/DepartmentsStaffManagement/ToggleActivationDepartmentsStaff";

                //ToggleActivationDepartmentsStaffPVM toggleActivationDepartmentsStaffPVM =
                //    new ToggleActivationDepartmentsStaffPVM()
                //    {
                //        DepartmentStaffId = departmentStaffId,
                //        UserId = this.userId.Value,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    ToggleActivationDepartmentsStaff(toggleActivationDepartmentsStaffPVM);

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
        public IActionResult TemporaryDeleteDepartmentsStaff(int departmentStaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/TemporaryDeleteDepartmentsStaff";

                //TemporaryDeleteDepartmentsStaffPVM temporaryDeleteDepartmentsStaffPVM =
                //    new TemporaryDeleteDepartmentsStaffPVM()
                //    {
                //        DepartmentStaffId = departmentStaffId,
                //        UserId = this.userId.Value,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    TemporaryDeleteDepartmentsStaff(temporaryDeleteDepartmentsStaffPVM);

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
                //        Message = "DeleteCurrentAlumnusCoursesErrorMessage"
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
        public IActionResult CompleteDeleteDepartmentsStaff(int departmentStaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/DepartmentsStaffManagement/CompleteDeleteDepartmentsStaff";

                //CompleteDeleteDepartmentsStaffPVM completeDeleteDepartmentsStaffPVM =
                //    new CompleteDeleteDepartmentsStaffPVM()
                //    {
                //        DepartmentStaffId = departmentStaffId,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    CompleteDeleteDepartmentsStaff(completeDeleteDepartmentsStaffPVM);

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
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentDepartmentsStaffErrorMessage").FirstOrDefault().Value
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

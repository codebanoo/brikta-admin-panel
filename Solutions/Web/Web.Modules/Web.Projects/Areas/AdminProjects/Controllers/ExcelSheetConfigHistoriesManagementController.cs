using ApiCallers.ProjectsApiCaller;
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
using System.Threading.Tasks;
using VM.Base;
using VM.Projects;
using VM.PVM.Projects;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class ExcelSheetConfigHistoriesManagementController : ExtraAdminController
    {
        public ExcelSheetConfigHistoriesManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index", "GoogleSheetConfigsManagement");
            }
            ViewData["ExcelSheetConfigId"] = Id;

            ViewData["Title"] = "لیست تاریخچه کانفیگ های شیت اکسل";

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/GoogleSheetConfigsManagement/Index/";
            }
            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllExcelSheetConfigHistoriesList(
            long? ExcelSheetConfigId = 0)
        {

            try
            {
                List<ExcelSheetConfigHistoriesVM> excelSheetConfigHistoriesVMList = new List<ExcelSheetConfigHistoriesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/GetAllExcelSheetConfigHistoriesList";

                    GetAllExcelSheetConfigHistoriesListPVM getAllExcelSheetConfigHistoriesListPVM = new GetAllExcelSheetConfigHistoriesListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        ExcelSheetConfigId = ExcelSheetConfigId
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllExcelSheetConfigHistoriesList(getAllExcelSheetConfigHistoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                excelSheetConfigHistoriesVMList = jArray.ToObject<List<ExcelSheetConfigHistoriesVM>>();


                                if (excelSheetConfigHistoriesVMList != null)
                                    if (excelSheetConfigHistoriesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ExcelSheetConfigHistoriesVM>>();

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

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }
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
        public IActionResult GetListOfExcelSheetConfigHistories(
            int ExcelSheetConfigId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string excelSheetConfigTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<ExcelSheetConfigHistoriesVM> excelSheetConfigHistoriesVMList = new List<ExcelSheetConfigHistoriesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/GetListOfExcelSheetConfigHistories";
                    GetListOfExcelSheetConfigHistoriesPVM getListOfExcelSheetConfigHistoriesPVM = new GetListOfExcelSheetConfigHistoriesPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ExcelSheetConfigId = ExcelSheetConfigId,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfExcelSheetConfigHistories(getListOfExcelSheetConfigHistoriesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                excelSheetConfigHistoriesVMList = jArray.ToObject<List<ExcelSheetConfigHistoriesVM>>();


                                if (excelSheetConfigHistoriesVMList != null)
                                    if (excelSheetConfigHistoriesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ExcelSheetConfigHistoriesVM>>();

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

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }
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



        #region Not Needed
        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToExcelSheetConfigHistories(ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });


            try
            {
                if (excelSheetConfigHistoriesVM != null)
                {

                    string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/AddToExcelSheetConfigHistories";
                    excelSheetConfigHistoriesVM.CreateEnDate = DateTime.Now;
                    excelSheetConfigHistoriesVM.CreateTime = PersianDate.TimeNow;
                    excelSheetConfigHistoriesVM.UserIdCreator = this.userId.Value;
                    excelSheetConfigHistoriesVM.IsActivated = true;
                    excelSheetConfigHistoriesVM.IsDeleted = false;
                    AddToExcelSheetConfigHistoriesPVM addToExcelSheetConfigHistoriesPVM1 = new AddToExcelSheetConfigHistoriesPVM()
                    {
                        ExcelSheetConfigHistoriesVM = excelSheetConfigHistoriesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToExcelSheetConfigHistories(addToExcelSheetConfigHistoriesPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<ConstructionProjectsVM>();

                                if (record != null)
                                {
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = excelSheetConfigHistoriesVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
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
        public IActionResult UpdateExcelSheetConfigHistories(ExcelSheetConfigHistoriesVM excelSheetConfigHistoriesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                excelSheetConfigHistoriesVM.EditEnDate = DateTime.Now;
                excelSheetConfigHistoriesVM.EditTime = PersianDate.TimeNow;
                excelSheetConfigHistoriesVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/UpdateExcelSheetConfigHistories";

                UpdateExcelSheetConfigHistoriesPVM updateExcelSheetConfigHistoriesPVM = new UpdateExcelSheetConfigHistoriesPVM()
                {
                    ExcelSheetConfigHistoriesVM = excelSheetConfigHistoriesVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateExcelSheetConfigHistories(updateExcelSheetConfigHistoriesPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ExcelSheetConfigHistoriesVM>();

                        if (record != null)
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Record = record,
                                Message = "تعریف انجام شد",
                            });
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





        [HttpPost]
        [AjaxOnly]
        public IActionResult ToggleActivationExcelSheetConfigHistories(int ExcelSheetConfigHistoryId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/ToggleActivationExcelSheetConfigHistories";

                ToggleActivationExcelSheetConfigHistoriesPVM toggleActivationExcelSheetConfigHistoriesPVM =
                    new ToggleActivationExcelSheetConfigHistoriesPVM()
                    {
                        ExcelSheetConfigHistoryId = ExcelSheetConfigHistoryId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationExcelSheetConfigHistories(toggleActivationExcelSheetConfigHistoriesPVM);

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
        public IActionResult TemporaryDeleteExcelSheetConfigHistories(int ExcelSheetConfigHistoryId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/TemporaryDeleteExcelSheetConfigHistories";
                TemporaryDeleteExcelSheetConfigHistoriesPVM temporaryDeleteExcelSheetConfigHistoriesPVM = new TemporaryDeleteExcelSheetConfigHistoriesPVM
                {
                    ExcelSheetConfigHistoryId = ExcelSheetConfigHistoryId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteExcelSheetConfigHistories(temporaryDeleteExcelSheetConfigHistoriesPVM);

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
        public IActionResult CompleteDeleteExcelSheetConfigHistories(long ExcelSheetConfigHistoryId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            try
            {
                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigHistoriesManagement/CompleteDeleteExcelSheetConfigHistories";

                CompleteDeleteExcelSheetConfigHistoriesPVM completeDeleteExcelSheetConfigHistoriesPVM = new CompleteDeleteExcelSheetConfigHistoriesPVM()
                {
                    ExcelSheetConfigHistoryId = ExcelSheetConfigHistoryId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteExcelSheetConfigHistories(completeDeleteExcelSheetConfigHistoriesPVM);

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
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "خطا"
                });
            }

        }

        #endregion
    }
}

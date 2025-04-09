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
    public class ExcelSheetConfigsManagementController : ExtraAdminController
    {
        public ExcelSheetConfigsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["GoogleSheetConfigId"] = Id;

            ViewData["Title"] = "لیست کانفیگ های شیت اکسل";

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/GoogleSheetConfigsManagement/Index/";
            }
            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }
            return View("Index");

        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllExcelSheetConfigsList(
             string? ExcelSheetConfigName = "",
            long? GoogleSheetConfigId = 0)
        {

            try
            {
                List<ExcelSheetConfigsVM> excelSheetConfigsVMList = new List<ExcelSheetConfigsVM>();

                serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/GetAllExcelSheetConfigsList";
                GetAllExcelSheetConfigsListPVM getAllExcelSheetConfigsListPVM = new GetAllExcelSheetConfigsListPVM
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                  
                    ExcelSheetConfigName = ExcelSheetConfigName,
                    GoogleSheetConfigId= GoogleSheetConfigId,

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllExcelSheetConfigsList(getAllExcelSheetConfigsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            excelSheetConfigsVMList = jArray.ToObject<List<ExcelSheetConfigsVM>>();


                            if (excelSheetConfigsVMList != null)
                                if (excelSheetConfigsVMList.Count >= 0)
                                {

                                    #region Fill UserCreatorName

                                    var records = jArray.ToObject<List<ExcelSheetConfigsVM>>();

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
            catch (Exception)
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
        public IActionResult GetListOfExcelSheetConfigs(
            int GoogleSheetConfigId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string ExcelSheetConfigName = "",
            string jtSorting = null)
        {

            try
            {
                List<ExcelSheetConfigsVM> excelSheetConfigsVMList = new List<ExcelSheetConfigsVM>();

                serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/GetListOfExcelSheetConfigs";
                GetListOfExcelSheetConfigsPVM getListOfExcelSheetConfigsPVM = new GetListOfExcelSheetConfigsPVM
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    ExcelSheetConfigName = ExcelSheetConfigName,
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    GoogleSheetConfigId = GoogleSheetConfigId,
                    jtSorting = jtSorting,
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfExcelSheetConfigs(getListOfExcelSheetConfigsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            excelSheetConfigsVMList = jArray.ToObject<List<ExcelSheetConfigsVM>>();


                            if (excelSheetConfigsVMList != null)
                                if (excelSheetConfigsVMList.Count >= 0)
                                {

                                    #region Fill UserCreatorName
                                    
                                    var records = jArray.ToObject<List<ExcelSheetConfigsVM>>();

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
            catch (Exception)
            {

            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }




        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToExcelSheetConfigs(ExcelSheetConfigsVM excelSheetConfigsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });


            try
            {
                if (excelSheetConfigsVM != null)
                {

                    string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/AddToExcelSheetConfigs";
                    excelSheetConfigsVM.CreateEnDate = DateTime.Now;
                    excelSheetConfigsVM.CreateTime = PersianDate.TimeNow;
                    excelSheetConfigsVM.UserIdCreator = this.userId.Value;
                    excelSheetConfigsVM.IsActivated = true;
                    excelSheetConfigsVM.IsDeleted = false;
                    AddToExcelSheetConfigsPVM addToExcelSheetConfigsPVM1 = new AddToExcelSheetConfigsPVM()
                    {
                        ExcelSheetConfigsVM = excelSheetConfigsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToExcelSheetConfigs(addToExcelSheetConfigsPVM1);

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
                                        Record = excelSheetConfigsVM,
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
        public IActionResult UpdateExcelSheetConfigs(ExcelSheetConfigsVM excelSheetConfigsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                excelSheetConfigsVM.EditEnDate = DateTime.Now;
                excelSheetConfigsVM.EditTime = PersianDate.TimeNow;
                excelSheetConfigsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/UpdateExcelSheetConfigs";

                UpdateExcelSheetConfigsPVM updateExcelSheetConfigsPVM = new UpdateExcelSheetConfigsPVM()
                {
                    ExcelSheetConfigsVM = excelSheetConfigsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateExcelSheetConfigs(updateExcelSheetConfigsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ExcelSheetConfigsVM>();

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
        public IActionResult ToggleActivationExcelSheetConfigs(int ExcelSheetConfigId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/ToggleActivationExcelSheetConfigs";

                ToggleActivationExcelSheetConfigsPVM toggleActivationExcelSheetConfigsPVM =
                    new ToggleActivationExcelSheetConfigsPVM()
                    {
                        ExcelSheetConfigId = ExcelSheetConfigId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationExcelSheetConfigs(toggleActivationExcelSheetConfigsPVM);

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
        public IActionResult TemporaryDeleteExcelSheetConfigs(int ExcelSheetConfigId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/TemporaryDeleteExcelSheetConfigs";
                TemporaryDeleteExcelSheetConfigsPVM temporaryDeleteExcelSheetConfigsPVM = new TemporaryDeleteExcelSheetConfigsPVM
                {
                    ExcelSheetConfigId = ExcelSheetConfigId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteExcelSheetConfigs(temporaryDeleteExcelSheetConfigsPVM);

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
        public IActionResult CompleteDeleteExcelSheetConfigs(long ExcelSheetConfigId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            try
            {
                string serviceUrl = projectsApiUrl + "/api/ExcelSheetConfigsManagement/CompleteDeleteExcelSheetConfigs";

                CompleteDeleteExcelSheetConfigsPVM completeDeleteExcelSheetConfigsPVM = new CompleteDeleteExcelSheetConfigsPVM()
                {
                    ExcelSheetConfigId = ExcelSheetConfigId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteExcelSheetConfigs(completeDeleteExcelSheetConfigsPVM);

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


    }
}

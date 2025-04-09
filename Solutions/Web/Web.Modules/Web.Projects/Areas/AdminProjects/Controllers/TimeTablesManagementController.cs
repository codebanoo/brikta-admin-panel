using ApiCallers.ProjectsApiCaller;
using AutoMapper;
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
using System.Collections.Generic;
using System;
using VM.Projects;
using VM.PVM.Projects;
using Web.Core.Controllers;
using CustomAttributes;
using FrameWork;
using System.IO;
using System.Threading.Tasks;
using VM.Base;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class TimeTablesManagementController : ExtraAdminController
    {
        public TimeTablesManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("Index", "ConstructionProjectsManagement");
            }
            ViewData["ConstructionProjectId"] = Id;
            ViewData["Title"] = "لیست فایلهای جدول زمان بندی";

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/ConstructionProjectsManagement/Index/";
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }
            return View("Index");

        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllTimeTablesList(
            long? constructionProjectId)
        {

            try
            {
                List<TimeTablesVM> TimeTablesVMList = new List<TimeTablesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/GetAllTimeTablesList";

                    GetAllTimeTablesListPVM getAllTimeTablesListPVM = new GetAllTimeTablesListPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        this.domainsSettings.DomainSettingId),


                         ConstructionProjectId = constructionProjectId
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllTimeTablesList(getAllTimeTablesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                TimeTablesVMList = jArray.ToObject<List<TimeTablesVM>>();


                                if (TimeTablesVMList != null)
                                    if (TimeTablesVMList.Count >= 0)
                                    {
                                        return Json(new
                                        {
                                            jsonResultWithRecordsObjectVM.Result,
                                            jsonResultWithRecordsObjectVM.Records,
                                            jsonResultWithRecordsObjectVM.TotalRecordCount
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
        public IActionResult GetListOfTimeTables(
            int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string jtSorting = null)
        {

            try
            {
                List<TimeTablesVM> TimeTablesVMList = new List<TimeTablesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/GetListOfTimeTables";
                    GetListOfTimeTablesPVM getListOfTimeTablesPVM = new GetListOfTimeTablesPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        this.domainsSettings.DomainSettingId),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ConstructionProjectId = ConstructionProjectId,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfTimeTables(getListOfTimeTablesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                TimeTablesVMList = jArray.ToObject<List<TimeTablesVM>>();


                                if (TimeTablesVMList != null)
                                    if (TimeTablesVMList.Count >= 0)
                                    {
                                        return Json(new
                                        {
                                            jsonResultWithRecordsObjectVM.Result,
                                            jsonResultWithRecordsObjectVM.Records,
                                            jsonResultWithRecordsObjectVM.TotalRecordCount
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

        public IActionResult AddToTimeTables(int Id)
        {
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["ConstructionProjectId"] = Id;
            ViewData["Title"] = "آپلود جدول زمان بندی";

            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToTimeTables(AddToTimeTablesPVM addToTimeTablesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            TimeTablesVM TimeTablesVM = new TimeTablesVM();

            try
            {
                if (addToTimeTablesPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string fileType = "";

                    string ext = Path.GetExtension(addToTimeTablesPVM.File.FileName);
                    fileName = Guid.NewGuid().ToString() + ext;

                    if (ext.Equals(".xlsx") ||
                        ext.Equals(".xlsx"))
                    {
                        fileType = "excel";

                    }

                    TimeTablesVM = new TimeTablesVM()
                    {
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        //IsConfirm = false,
                        TimeTableFileExt = ext,
                        TimeTableFilePath = fileName,
                        ConstructionProjectId = addToTimeTablesPVM.TimeTablesVM.ConstructionProjectId,
                        TimeTableDescription = addToTimeTablesPVM.TimeTablesVM.TimeTableDescription,
                        TimeTableNumber = addToTimeTablesPVM.TimeTablesVM.TimeTableNumber,
                        TimeTableFileOrder = addToTimeTablesPVM.TimeTablesVM.TimeTableFileOrder,
                        TimeTableFileType = fileType,
                        //TimeTableLink = addToTimeTablesPVM.TimeTablesVM.TimeTableLink,
                        TimeTableTitle = addToTimeTablesPVM.TimeTablesVM.TimeTableTitle,
                    };

                    string serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/AddToTimeTables";

                    AddToTimeTablesPVM addToTimeTablesPVM1 = new AddToTimeTablesPVM()
                    {
                        TimeTablesVM = TimeTablesVM,
                        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                            this.domainsSettings.DomainSettingId),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToTimeTables(addToTimeTablesPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                TimeTablesVM.TimeTableId = jObject.ToObject<TimeTablesVM>().TimeTableId;
                            }
                        }
                    }

                    try
                    {
                        if (TimeTablesVM.TimeTableId == 0)
                        {

                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                        string TimeTableFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TimeTableFiles\\" + domainSettings.DomainName + "\\" + TimeTablesVM.TimeTableId + "\\Image";
                        if (!Directory.Exists(TimeTableFolder))
                        {
                            Directory.CreateDirectory(TimeTableFolder);
                        }
                        using (var fileStream = new FileStream(TimeTableFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToTimeTablesPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }


                        return Json(new
                        {
                            Result = "OK",
                            Message = "آپلود انجام شد",
                        });

                    }
                    catch (Exception exc)
                    { }
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
        public IActionResult UpdateTimeTables(TimeTablesVM TimeTablesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                TimeTablesVM.EditEnDate = DateTime.Now;
                TimeTablesVM.EditTime = PersianDate.TimeNow;
                TimeTablesVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/UpdateTimeTables";

                UpdateTimeTablesPVM updateTimeTablesPVM = new UpdateTimeTablesPVM()
                {
                    TimeTablesVM = TimeTablesVM,
                    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateTimeTables(updateTimeTablesPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<TimeTablesVM>();

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
        public IActionResult ToggleActivationTimeTables(int TimeTableId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/ToggleActivationTimeTables";

                ToggleActivationTimeTablesPVM toggleActivationTimeTablesPVM =
                    new ToggleActivationTimeTablesPVM()
                    {
                        TimeTableId = TimeTableId,
                        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationTimeTables(toggleActivationTimeTablesPVM);

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
        public IActionResult TemporaryDeleteTimeTables(int TimeTableId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/TemporaryDeleteTimeTables";
                TemporaryDeleteTimeTablesPVM temporaryDeleteTimeTablesPVM = new TemporaryDeleteTimeTablesPVM
                {
                    TimeTableId = TimeTableId,
                    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteTimeTables(temporaryDeleteTimeTablesPVM);

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
        public IActionResult CompleteDeleteTimeTables(int TimeTableId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            TimeTablesVM TimeTablesVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/GetTimeTablesWithTimeTableId";

                GetTimeTableWithTimeTableIdPVM getTimeTablesWithTimeTableIdPVM = new GetTimeTableWithTimeTableIdPVM()
                {
                    TimeTableId = TimeTableId,
                    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetTimeTableWithTimeTableId(getTimeTablesWithTimeTableIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<TimeTablesVM>();


                            if (record != null)
                            {
                                TimeTablesVM = record;

                            }
                            else
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
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "خطا"
                    });
                }
            }
            catch (Exception exc)
            { }

            try
            {


                serviceUrl = projectsApiUrl + "/api/TimeTablesManagement/CompleteDeleteTimeTables";
                CompleteDeleteTimeTablesPVM completeDeleteTimeTablesPVM = new CompleteDeleteTimeTablesPVM
                {
                    TimeTableId = TimeTableId,
                    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteTimeTables(completeDeleteTimeTablesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                        }
                        else
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = jsonResultObjectVM.Message
                            });
                        }
                    }
                }


                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string TimeTableFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TimeTableFiles\\" + domainSettings.DomainName + "\\" + TimeTablesVM.TimeTableId + "\\Image";

                if (!string.IsNullOrEmpty(TimeTablesVM.TimeTableFilePath))
                {
                    if (System.IO.File.Exists(TimeTableFolder + "\\" + TimeTablesVM.TimeTableFilePath))
                    {
                        System.IO.File.Delete(TimeTableFolder + "\\" + TimeTablesVM.TimeTableFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (TimeTablesVM.TimeTableFileExt.ToLower().Equals(".jpg") ||
                    //    TimeTablesVM.TimeTableFileExt.ToLower().Equals(".jpeg") ||
                    //    TimeTablesVM.TimeTableFileExt.ToLower().Equals(".png") ||
                    //    TimeTablesVM.TimeTableFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(TimeTableFolder + "\\thumb_" + TimeTablesVM.TimeTableFilePath))
                    //    {
                    //        System.IO.File.Delete(TimeTableFolder + "\\thumb_" + TimeTablesVM.TimeTableFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(TimeTableFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(TimeTableFolder + "\\..");
                    System.Threading.Thread.Sleep(100);

                }
            }

            catch (Exception exc)
            { }

            return Json(new { Result = "OK" });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using System.Linq;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;
using System.Threading.Tasks;
using System.IO;
using VM.Projects;
using VM.PVM.Projects;
using ApiCallers.ProjectsApiCaller;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class ProgressPicturesManagementController : ExtraAdminController
    {
        public ProgressPicturesManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(long Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index", "ConstructionProjectsManagement");
            }
            ViewData["ConstructionProjectId"] = Id;


            //ViewData["Title"] = "لیست تصاویر پیشرفت";


            ViewData["Title"] = "لیست تصاویر ماهانه";

            if (ViewData["ConstructionProject"] == null)
            {
                ConstructionProjectsVM constructionProjectsVM = new ConstructionProjectsVM();
                try
                {
                    JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                    new JsonResultWithRecordObjectVM(new object() { });
                    string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/GetConstructionProjectWithConstructionProjectId";

                    GetConstructionProjectWithConstructionProjectIdPVM getConstructionProjectWithConstructionProjectIdPVM = new GetConstructionProjectWithConstructionProjectIdPVM()
                    {
                        ConstructionProjectId = Id,
                    };
                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConstructionProjectWithConstructionProjectId(getConstructionProjectWithConstructionProjectIdPVM);
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
                                    constructionProjectsVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                ViewData["ConstructionProject"] = constructionProjectsVM;
            }

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
        public IActionResult GetAllProgressPicturesList(
            string? ProgressPictureTitle = "",
            long? ConstructionProjectId = 0)
        {

            try
            {
                List<ProgressPicturesVM> progressPicturesVMList = new List<ProgressPicturesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/GetAllProgressPicturesList";

                    GetAllProgressPicturesListPVM getAllProgressPicturesListPVM = new GetAllProgressPicturesListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        ProgressPictureTitle = ProgressPictureTitle,
                        ConstructionProjectId = ConstructionProjectId
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllProgressPicturesList(getAllProgressPicturesListPVM
                        );

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                progressPicturesVMList = jArray.ToObject<List<ProgressPicturesVM>>();


                                if (progressPicturesVMList != null)
                                    if (progressPicturesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ProgressPicturesVM>>();

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
        public IActionResult GetListOfProgressPictures(
            int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string progressPictureTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<ProgressPicturesVM> progressPicturesVMList = new List<ProgressPicturesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/GetListOfProgressPictures";
                    GetListOfProgressPicturesPVM getListOfProgressPicturesPVM = new GetListOfProgressPicturesPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ProgressPictureTitle = progressPictureTitle,
                        ConstructionProjectId = ConstructionProjectId,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfProgressPictures(getListOfProgressPicturesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                progressPicturesVMList = jArray.ToObject<List<ProgressPicturesVM>>();


                                if (progressPicturesVMList != null)
                                    if (progressPicturesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ProgressPicturesVM>>();

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

        public IActionResult AddToProgressPictures(long Id)
        {
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;


            ViewData["ConstructionProjectId"] = Id;

            //ViewData["Title"] = "آپلود تصاویر پیشرفت";

            ViewData["Title"] = "آپلود تصویر ماهانه";

            if (ViewData["ConstructionProject"] == null)
            {
                ConstructionProjectsVM constructionProjectsVM = new ConstructionProjectsVM();
                try
                {
                    JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                    new JsonResultWithRecordObjectVM(new object() { });
                    string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/GetConstructionProjectWithConstructionProjectId";
                    GetConstructionProjectWithConstructionProjectIdPVM getConstructionProjectWithConstructionProjectIdPVM = new GetConstructionProjectWithConstructionProjectIdPVM()
                    {
                        ConstructionProjectId = Id,
                    };
                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConstructionProjectWithConstructionProjectId(getConstructionProjectWithConstructionProjectIdPVM);
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
                                    constructionProjectsVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                ViewData["ConstructionProject"] = constructionProjectsVM;
            }
            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToProgressPictures(AddToProgressPicturesPVM addToProgressPicturesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ProgressPicturesVM progressPicturesVM = new ProgressPicturesVM();

            try
            {
                if (addToProgressPicturesPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string fileType = "";

                    string ext = Path.GetExtension(addToProgressPicturesPVM.File.FileName);
                    fileName = Guid.NewGuid().ToString() + ext;

                    if (ext.Equals(".jpeg") ||
ext.Equals(".jpg") ||
ext.Equals(".png") ||
ext.Equals(".mp4") ||
ext.Equals(".mkv") ||
ext.Equals(".mov"))
                    {
                        fileType = "media";

                    }
                    else if (ext.Equals(".pdf")) //pdf
                    {
                        fileType = "pdf";
                    }
                    else if (ext.Equals(".pptx")) //powerPoint
                    {
                        fileType = "powerPoint";
                    }
                    else if (ext.Equals(".xls") || //excel
                          ext.Equals(".xlsx"))
                    {
                        fileType = "excel";

                    }
                    else if (ext.Equals(".docx") || //word
                          ext.Equals(".doc"))
                    {
                        fileType = "word";
                    }
                    else if (ext.Equals(".mpp")) //microdoft project
                    {
                        fileType = "mpp";
                    }
                    else if (ext.Equals(".txt")) //microdoft project
                    {
                        fileType = "text";
                    }
                    else if (ext.Equals(".rar") ||
                        ext.Equals(".zip"))
                    {
                        fileType = "rar";
                    }
                    else if (ext.Equals(".dwg") ||
                        ext.Equals(".skp"))
                    {
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "لطفا فایل آپلودی خود را فشرده سازید."
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Result = "ERROR",
                            Message = "خطا"
                        });
                    }


                    progressPicturesVM = new ProgressPicturesVM()
                    {
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        ProgressPictureFileExt = ext,
                        ProgressPictureFilePath = fileName,
                        ConstructionProjectId = addToProgressPicturesPVM.ProgressPicturesVM.ConstructionProjectId,
                        ProgressPictureDescription = addToProgressPicturesPVM.ProgressPicturesVM.ProgressPictureDescription,
                        ProgressPictureNumber = addToProgressPicturesPVM.ProgressPicturesVM.ProgressPictureNumber,
                        ProgressPictureFileOrder = addToProgressPicturesPVM.ProgressPicturesVM.ProgressPictureFileOrder,
                        ProgressPictureFileType = fileType,
                        //ProgressPictureLink = addToProgressPicturesPVM.ProgressPicturesVM.ProgressPictureLink,
                        ProgressPictureTitle = addToProgressPicturesPVM.ProgressPicturesVM.ProgressPictureTitle,

                    };

                    string serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/AddToProgressPictures";

                    AddToProgressPicturesPVM addToProgressPicturesPVM1 = new AddToProgressPicturesPVM()
                    {
                        ProgressPicturesVM = progressPicturesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToProgressPictures(addToProgressPicturesPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                progressPicturesVM.ProgressPictureId = jObject.ToObject<ProgressPicturesVM>().ProgressPictureId;
                            }
                        }
                    }

                    try
                    {
                        if (progressPicturesVM.ProgressPictureId == 0)
                        {

                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                        string progressPictureFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainSettings.DomainName +
                            "\\" + progressPicturesVM.ConstructionProjectId + "\\ProgressPictureFiles\\" + progressPicturesVM.ProgressPictureId + "\\Image";
                        if (!Directory.Exists(progressPictureFolder))
                        {
                            Directory.CreateDirectory(progressPictureFolder);
                        }
                        using (var fileStream = new FileStream(progressPictureFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToProgressPicturesPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //if (ext.Equals(".jpeg") ||
                        //    ext.Equals(".jpg") ||
                        //    ext.Equals(".png") ||
                        //    ext.Equals(".gif") ||
                        //    ext.Equals(".bmp"))
                        //{
                        //    ImageModify.ResizeImage(progressPictureFolder + "\\",
                        //        fileName,
                        //        120,
                        //        120);
                        //}
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
        public IActionResult UpdateProgressPictures(ProgressPicturesVM progressPicturesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                progressPicturesVM.EditEnDate = DateTime.Now;
                progressPicturesVM.EditTime = PersianDate.TimeNow;
                progressPicturesVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/UpdateProgressPictures";

                UpdateProgressPicturesPVM updateProgressPicturesPVM = new UpdateProgressPicturesPVM()
                {
                    ProgressPicturesVM = progressPicturesVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateProgressPictures(updateProgressPicturesPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ProgressPicturesVM>();

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
        public IActionResult ToggleActivationProgressPictures(int ProgressPictureId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/ToggleActivationProgressPictures";

                ToggleActivationProgressPicturesPVM toggleActivationProgressPicturesPVM =
                    new ToggleActivationProgressPicturesPVM()
                    {
                        ProgressPictureId = ProgressPictureId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationProgressPictures(toggleActivationProgressPicturesPVM);

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
        public IActionResult TemporaryDeleteProgressPictures(int ProgressPictureId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/TemporaryDeleteProgressPictures";
                TemporaryDeleteProgressPicturesPVM temporaryDeleteProgressPicturesPVM = new TemporaryDeleteProgressPicturesPVM
                {
                    ProgressPictureId = ProgressPictureId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteProgressPictures(temporaryDeleteProgressPicturesPVM);

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
        public IActionResult CompleteDeleteProgressPictures(int ProgressPictureId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            ProgressPicturesVM progressPicturesVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/GetProgressPicturesWithProgressPictureId";

                GetProgressPicturesWithProgressPictureIdPVM getProgressPicturesWithProgressPictureIdPVM = new GetProgressPicturesWithProgressPictureIdPVM()
                {
                    ProgressPictureId = ProgressPictureId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetProgressPicturesWithProgressPictureId(getProgressPicturesWithProgressPictureIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ProgressPicturesVM>();


                            if (record != null)
                            {
                                progressPicturesVM = record;

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


                serviceUrl = projectsApiUrl + "/api/ProgressPicturesManagement/CompleteDeleteProgressPictures";
                CompleteDeleteProgressPicturesPVM completeDeleteProgressPicturesPVM = new CompleteDeleteProgressPicturesPVM
                {
                    ProgressPictureId = ProgressPictureId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteProgressPictures(completeDeleteProgressPicturesPVM);

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
                            if (jsonResultObjectVM.Message == "ERROR_DEPENDENCY")
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "لطفا ابتدا پیوست های این قرارداد را حذف کنید."
                                });
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                    }
                }


                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string progressPictureFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ProgressPictureFiles\\" + domainSettings.DomainName + "\\" + progressPicturesVM.ProgressPictureId + "\\Image";

                if (!string.IsNullOrEmpty(progressPicturesVM.ProgressPictureFilePath))
                {
                    if (System.IO.File.Exists(progressPictureFolder + "\\" + progressPicturesVM.ProgressPictureFilePath))
                    {
                        System.IO.File.Delete(progressPictureFolder + "\\" + progressPicturesVM.ProgressPictureFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (progressPicturesVM.ProgressPictureFileExt.ToLower().Equals(".jpg") ||
                    //    progressPicturesVM.ProgressPictureFileExt.ToLower().Equals(".jpeg") ||
                    //    progressPicturesVM.ProgressPictureFileExt.ToLower().Equals(".png") ||
                    //    progressPicturesVM.ProgressPictureFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(progressPictureFolder + "\\thumb_" + progressPicturesVM.ProgressPictureFilePath))
                    //    {
                    //        System.IO.File.Delete(progressPictureFolder + "\\thumb_" + progressPicturesVM.ProgressPictureFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(progressPictureFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(progressPictureFolder + "\\..");
                    System.Threading.Thread.Sleep(100);

                }
            }

            catch (Exception exc)
            { }

            return Json(new { Result = "OK" });
        }



        #region Download File

        public async Task<IActionResult> Download(string constructionProjectId, long fileId, string fileName, string type)
        {
            if (fileName == null)
                return Content("FileNotFound");
            string fileLocation = "";
            if (type == "Attachments")
            {
                fileLocation = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainsSettings.DomainName + "\\"
                    + constructionProjectId + "\\AttachementFiles\\" + fileId + "\\Image\\";
            }
            else
            {
                fileLocation = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainsSettings.DomainName + "\\"
                    + constructionProjectId + "\\" + type + "Files" + "\\" + fileId + "\\Image\\";
            }
            if (System.IO.File.Exists(fileLocation + fileName))
            {
                try
                {
                    serviceUrl = projectsApiUrl + "/api/FileStatesLogsManagement/AddToFileStatesLogs";
                    var fileStatesLogsVM = new FileStatesLogsVM
                    {
                        TableTitle = type,
                        RecordId = fileId,
                        FileStateId = 3,
                    };
                    AddToFileStatesLogsPVM addToFileStatesLogsPVM = new AddToFileStatesLogsPVM()
                    {
                        FileStatesLogsVM = fileStatesLogsVM
                    };
                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToFileStatesLogs(addToFileStatesLogsPVM);
                }
                catch (Exception exc)
                {
                    // Handle exception
                }
                var filePath = fileLocation + fileName;
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                string contentType = "";
                string extension = Path.GetExtension(fileName).ToLowerInvariant();
                switch (extension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".mp4":
                        contentType = "video/mp4";
                        break;
                    case ".mkv":
                        contentType = "video/x-matroska";
                        break;
                    case ".mov":
                        contentType = "video/quicktime";
                        break;
                    case ".xlsx":
                    case ".xls":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".doc":
                    case ".docx":
                        contentType = "application/msword";
                        break;
                    case ".pptx":
                        contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        break;
                    case ".mpp":
                        contentType = "application/vnd.ms-project";
                        break;
                    case ".txt":
                        contentType = "text/plain";
                        break;
                    case ".rar":
                    case ".zip":
                        contentType = "application/octet-stream"; // Default to binary if the type is unknown
                        break;

                        //default:
                        //    contentType = "application/octet-stream"; // Default to binary if the type is unknown
                        //break;

                        //case ".gif":
                        //    contentType = "image/gif";
                        //    break;

                        //case ".dwg":
                        //    //contentType = "application/octet-stream"; // Adjust as needed
                        //    contentType = "application/acad";
                        //    break;
                        //case ".skp":
                        //    contentType = "application/octet-stream"; // Adjust as needed
                        //    break;
                }
                return File(memory, contentType, Path.GetFileName(filePath));
            }




            return Content("FileNotFound");
        }

        #endregion

    }
}

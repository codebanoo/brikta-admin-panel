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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Projects;
using VM.PVM.Projects;
using Web.Core.Controllers;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class AttachementFilesManagementController : ExtraAdminController
    {
        public AttachementFilesManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(
            long ParentId = 0,
            string TableName = "",
            long ConstructionProjectId = 0)
        {
            if (ParentId == 0 || String.IsNullOrEmpty(TableName) || ConstructionProjectId == 0)
            {
                return RedirectToAction("Index", "ConstructionProjectsManagement");
            }



            ViewData["ParentId"] = ParentId;

            ViewData["TableName"] = TableName;

            ViewData["Title"] = "لیست  پیوست";

            ViewData["ConstructionProjectId"] = ConstructionProjectId;

            ViewData["UserId"] = this.userId;


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
                        ConstructionProjectId = ConstructionProjectId,
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
                //ViewData["DataBackUrl"] = "/AdminProjects/ConstructionProjectsManagement/Index/";
                ViewData["DataBackUrl"] = "/AdminProjects/" + TableName + "Management/Index/" + ConstructionProjectId.ToString();
            }

            //if (ViewData["SearchTitle"] == null)
            //{
            //    ViewData["SearchTitle"] = "OK";
            //}
            return View("Index");
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllAttachementFilesList(
              int? attachementParentId = 0,
              string? AttachementTableTitle = "")
        {

            try
            {
                List<AttachementFilesVM> attachementFilesVMList = new List<AttachementFilesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/GetAllAttachementFilesList";

                    GetAllAttachementFilesListPVM getAllAttachementFilesListPVM = new GetAllAttachementFilesListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        AttachementParentId = attachementParentId,
                        AttachementTableTitle = AttachementTableTitle
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllAttachementFilesList(getAllAttachementFilesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                attachementFilesVMList = jArray.ToObject<List<AttachementFilesVM>>();


                                if (attachementFilesVMList != null)
                                    if (attachementFilesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<AttachementFilesVM>>();

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
        public IActionResult GetListOfAttachementFiles(
            int attachementParentId = 0,
            string attachementTableTitle = "",
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string jtSorting = null)
        {

            try
            {
                List<AttachementFilesVM> attachementFilesVMList = new List<AttachementFilesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/GetListOfAttachementFiles";
                    GetListOfAttachementFilesPVM getListOfAttachementFilesPVM = new GetListOfAttachementFilesPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        AttachementTableTitle = attachementTableTitle,
                        AttachementParentId = attachementParentId,
                        jtSorting = jtSorting,
                        UserId = this.userId.Value,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfAttachementFiles(getListOfAttachementFilesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                attachementFilesVMList = jArray.ToObject<List<AttachementFilesVM>>();


                                if (attachementFilesVMList != null)
                                    if (attachementFilesVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<AttachementFilesVM>>();

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

        public IActionResult AddToAttachementFiles(long ParentId = 0, string TableName = "", long ConstructionProjectId = 0)
        {
            if (ParentId == 0 || String.IsNullOrEmpty(TableName) || ConstructionProjectId == 0)
            {
                return RedirectToAction("Index", "ConstructionProjectsManagement");
            }
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["Title"] = "آپلود  پیوست";
            ViewData["ParentId"] = ParentId;
            ViewData["TableName"] = TableName;
            ViewData["ConstructionProjectId"] = ConstructionProjectId;

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
                        ConstructionProjectId = ConstructionProjectId,
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

            //if (ViewData["DataBackUrl"] == null)
            //{
            //    ViewData["DataBackUrl"] = "/AdminProjects/AttachementFilesManagement/Index?ParentId=" + ParentId + "&TableName=" + TableName;
            //}

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/AttachementFilesManagement/Index?ParentId=" + ParentId + "&TableName=" + TableName + "&ConstructionProjectId=" + ConstructionProjectId;
            }

            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToAttachementFiles(AddToAttachementFilesPVM addToAttachementFilesPVM, int constructionProjectId = 0)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            AttachementFilesVM attachementFilesVM = new AttachementFilesVM();

            try
            {
                if (addToAttachementFilesPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string fileType = "";

                    string ext = Path.GetExtension(addToAttachementFilesPVM.File.FileName);
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

                    attachementFilesVM = new AttachementFilesVM()
                    {
                        AttachementParentId = addToAttachementFilesPVM.AttachementFilesVM.AttachementParentId,
                        AttachementTableTitle = addToAttachementFilesPVM.AttachementFilesVM.AttachementTableTitle,
                        IsConfirm = false,
                        IsSend = false,
                        IsView = false,
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        AttachementFileExt = ext,
                        AttachementFilePath = fileName,
                        AttachementDescription = addToAttachementFilesPVM.AttachementFilesVM.AttachementDescription,
                        AttachementFileOrder = addToAttachementFilesPVM.AttachementFilesVM.AttachementFileOrder,
                        AttachementFileType = fileType,
                        AttachementTitle = addToAttachementFilesPVM.AttachementFilesVM.AttachementTitle,

                    };

                    string serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/AddToAttachementFiles";

                    AddToAttachementFilesPVM addToAttachementFilesPVM1 = new AddToAttachementFilesPVM()
                    {
                        AttachementFilesVM = attachementFilesVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToAttachementFiles(addToAttachementFilesPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                attachementFilesVM.AttachementId = jObject.ToObject<AttachementFilesVM>().AttachementId;
                            }
                        }
                    }

                    try
                    {
                        if (attachementFilesVM.AttachementId == 0)
                        {

                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                        //string attachementFileFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\AttachementFiles\\" + domainSettings.DomainName + "\\" + attachementFilesVM.AttachementId + "\\Image";
                        string attachementFileFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainSettings.DomainName +
                            "\\" + constructionProjectId + "\\AttachementFiles\\" + attachementFilesVM.AttachementId + "\\Image";
                        if (!Directory.Exists(attachementFileFolder))
                        {
                            Directory.CreateDirectory(attachementFileFolder);
                        }
                        using (var fileStream = new FileStream(attachementFileFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToAttachementFilesPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //if (ext.Equals(".jpeg") ||
                        //    ext.Equals(".jpg") ||
                        //    ext.Equals(".png") ||
                        //    ext.Equals(".gif") ||
                        //    ext.Equals(".bmp"))
                        //{
                        //    ImageModify.ResizeImage(attachementFileFolder + "\\",
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
        public IActionResult UpdateAttachementFiles(AttachementFilesVM attachementFilesVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                attachementFilesVM.EditEnDate = DateTime.Now;
                attachementFilesVM.EditTime = PersianDate.TimeNow;
                attachementFilesVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/UpdateAttachementFiles";

                UpdateAttachementFilesPVM updateAttachementFilesPVM = new UpdateAttachementFilesPVM()
                {
                    AttachementFilesVM = attachementFilesVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateAttachementFiles(updateAttachementFilesPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<AttachementFilesVM>();

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
        public IActionResult ToggleActivationAttachementFiles(int attachementFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/ToggleActivationAttachementFiles";

                ToggleActivationAttachementFilesPVM toggleActivationAttachementFilesPVM =
                    new ToggleActivationAttachementFilesPVM()
                    {
                        AttachementFileId = attachementFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationAttachementFiles(toggleActivationAttachementFilesPVM);

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
        public IActionResult TemporaryDeleteAttachementFiles(int AttachementFileId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/TemporaryDeleteAttachementFiles";
                TemporaryDeleteAttachementFilesPVM temporaryDeleteAttachementFilesPVM = new TemporaryDeleteAttachementFilesPVM
                {
                    AttachementFileId = AttachementFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteAttachementFiles(temporaryDeleteAttachementFilesPVM);

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
        public IActionResult CompleteDeleteAttachementFiles(int AttachementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            AttachementFilesVM attachementFilesVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/GetAttachementFilesWithAttachementFileId";

                GetAttachementFilesWithAttachementFileIdPVM getAttachementFilesWithAttachementFileIdPVM = new GetAttachementFilesWithAttachementFileIdPVM()
                {
                    AttachementFileId = AttachementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAttachementFilesWithAttachementFileId(getAttachementFilesWithAttachementFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<AttachementFilesVM>();


                            if (record != null)
                            {
                                attachementFilesVM = record;

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


                serviceUrl = projectsApiUrl + "/api/AttachementFilesManagement/CompleteDeleteAttachementFiles";
                CompleteDeleteAttachementFilesPVM completeDeleteAttachementFilesPVM = new CompleteDeleteAttachementFilesPVM
                {
                    AttachementFilesId = AttachementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteAttachementFiles(completeDeleteAttachementFilesPVM);

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
                                Message = "خطا"
                            });
                        }
                    }
                }


                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string attachementFileFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\AttachementFiles\\" + domainSettings.DomainName + "\\" + attachementFilesVM.AttachementId + "\\Image";

                if (!string.IsNullOrEmpty(attachementFilesVM.AttachementFilePath))
                {
                    if (System.IO.File.Exists(attachementFileFolder + "\\" + attachementFilesVM.AttachementFilePath))
                    {
                        System.IO.File.Delete(attachementFileFolder + "\\" + attachementFilesVM.AttachementFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (attachementFilesVM.AttachementFileExt.ToLower().Equals(".jpg") ||
                    //    attachementFilesVM.AttachementFileExt.ToLower().Equals(".jpeg") ||
                    //    attachementFilesVM.AttachementFileExt.ToLower().Equals(".png") ||
                    //    attachementFilesVM.AttachementFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(attachementFileFolder + "\\thumb_" + attachementFilesVM.AttachementFilePath))
                    //    {
                    //        System.IO.File.Delete(attachementFileFolder + "\\thumb_" + attachementFilesVM.AttachementFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(attachementFileFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(attachementFileFolder + "\\..");
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


        #region  Chats


        //اضافه کردن مکالمه
        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToConversationLogs(AddToConversationLogsPVM addToConversationLogsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            try
            {

                AddToConversationLogsPVM addToConversationLogs = new AddToConversationLogsPVM()
                {
                    ConversationLogsVM = new ConversationLogsVM()
                    {
                        ConversationLogDescription = addToConversationLogsPVM.ConversationLogsVM.ConversationLogDescription,
                        RecordId = addToConversationLogsPVM.ConversationLogsVM.RecordId,
                        TableTitle = addToConversationLogsPVM.ConversationLogsVM.TableTitle,
                        UserIdCreator = this.userId.Value,
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        IsActivated = true,
                        IsDeleted = false,
                    },

                    UserId = this.userId.Value
                };


                serviceUrl = projectsApiUrl + "/api/ConversationLogsManagement/AddToConversationLogs";

                responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToConversationLogs(addToConversationLogs);
                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            return Json(jsonResultWithRecordObjectVM);
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


        //لیستی از مکالمات 
        [AjaxOnly]
        [HttpPost]
        public IActionResult GetConversationDataByAgreementTypeAndRecordId(GetConversationDataByAgreementTypeAndRecordIdPVM
          getConversationDataByAgreementTypeAndRecordIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                GetConversationDataByAgreementTypeAndRecordIdPVM conversationDataByAgreementTypeAndRecordIdPVM = new GetConversationDataByAgreementTypeAndRecordIdPVM
                {
                    UserId = this.userId,
                    ContractType = getConversationDataByAgreementTypeAndRecordIdPVM.ContractType,
                    RecordId = getConversationDataByAgreementTypeAndRecordIdPVM.RecordId,
                };
                serviceUrl = projectsApiUrl + "/api/ConversationLogsManagement/GetConversationDataByAgreementTypeAndRecordId";
                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConversationDataByAgreementTypeAndRecordId(conversationDataByAgreementTypeAndRecordIdPVM);
                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;
                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            return Json(jsonResultWithRecordsObjectVM);
                        }
                    }
                }
            }
            catch (Exception exc)
            { }
            return View("UserIndex");
        }
        #endregion
    }
}

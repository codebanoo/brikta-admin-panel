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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class DistrictsDocsManagementController : ExtraAdminController
    {
        public DistrictsDocsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "DistrictsManagement");

            ViewData["Title"] = "لیست اسناد";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;

            DistrictsVM districtsVM = new DistrictsVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictsManagement/GetDistrictWithDistrictId";

                GetDistrictWithDistrictIdPVM getDistrictWithDistrictIdPVM = new GetDistrictWithDistrictIdPVM()
                {
                    DistrictId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictWithDistrictId(getDistrictWithDistrictIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<DistrictsVM>();


                            if (record != null)
                            {
                                districtsVM = record;


                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["DistrictsList"] = districtsVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminPublic/DistrictsManagement/Index/";
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View();
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfDistrictFiles(int jtStartIndex = 0,
             int jtPageSize = 10,
             int? districtId = null,
             string districtFileTitle = "",
             string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/GetListOfDistrictFiles";

                GetListOfDistrictFilesPVM getListOfDistrictFilesPVM = new GetListOfDistrictFilesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    DistrictId = ((districtId.HasValue) ? districtId.Value : 0),
                    DistrictFileTitle = districtFileTitle,
                    DistrictFileType = "docs",
                    jtSorting = jtSorting
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfDistrictFiles(getListOfDistrictFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {

                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<DistrictFilesVM>>();

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
                Message = "خطا"
            });
        }




        public IActionResult AddToDistrictFiles(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "DistrictsManagement");

            ViewData["Title"] = "آپلود اسناد";

            DistrictsVM districtsVM = new DistrictsVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictsManagement/GetDistrictWithDistrictId";

                GetDistrictWithDistrictIdPVM getDistrictWithDistrictIdPVM = new GetDistrictWithDistrictIdPVM()
                {
                    DistrictId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictWithDistrictId(getDistrictWithDistrictIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<DistrictsVM>();


                            if (record != null)
                            {
                                districtsVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["DistrictsList"] = districtsVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminPublic/DistrictsDocsManagement/Index/" + districtsVM.DistrictId;

            }

            return View("AddTo");
        }



        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToDistrictFiles(List<DistrictFileUploadPVM> districtFileUploadPVMList, int districtId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            List<DistrictFilesVM> districtFilesVM = new List<DistrictFilesVM>();

            try
            {
                if (districtFileUploadPVMList != null)
                {
                    if (districtFileUploadPVMList.Count > 0)
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                        string districtFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\DistrictFiles\\" + domainSettings.DomainName + "\\" + districtId + "\\Docs";

                        foreach (var districtFileUpload in districtFileUploadPVMList)
                        {
                            try
                            {
                                string fileName = "";

                                string ext = Path.GetExtension(districtFileUpload.File.FileName);
                                fileName = Guid.NewGuid().ToString() + ext;
                                using (var fileStream = new FileStream(districtFolder + "\\" + fileName, FileMode.Create))
                                {
                                    await districtFileUpload.File.CopyToAsync(fileStream);
                                    System.Threading.Thread.Sleep(100);
                                }

                                if (ext.Equals(".jpeg") ||
                                    ext.Equals(".jpg") ||
                                    ext.Equals(".png") ||
                                    ext.Equals(".gif") ||
                                    ext.Equals(".bmp"))
                                {
                                    ImageModify.ResizeImage(districtFolder + "\\",
                                        fileName,
                                        120,
                                        120);
                                }
                                else
                                    if (ext.Equals(".mp4"))
                                {

                                }

                                var districtFiles = new DistrictFilesVM()
                                {
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = this.userId.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                    DistrictFileExt = ext,
                                    DistrictFilePath = fileName,
                                    DistrictFileTitle = districtFileUpload.DistrictFileTitle,
                                    DistrictFileOrder = districtFileUpload.DistrictFileOrder,
                                    DistrictFileType = "docs",
                                    DistrictId = districtId,
                                };

                                districtFilesVM.Add(districtFiles);
                            }
                            catch (Exception exc)
                            { }
                        }
                    }
                }

                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/AddToDistrictFiles";

                AddToDistrictFilesPVM addToDistrictFilesPVM = new AddToDistrictFilesPVM()
                {
                    DistrictFilesVM = districtFilesVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).AddToDistrictFiles(addToDistrictFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "آپلود انجام شد",
                            });
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
        public async Task<ActionResult> UpdateDistrictFiles(DistrictFileUploadPVM districtFileUploadPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            DistrictFilesVM districtFilesVM = new DistrictFilesVM();
            DistrictFilesVM oldDistrictFilesVM = new DistrictFilesVM();

            #region get old file

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/GetDistrictFileWithDistrictFileId";

                GetDistrictFileWithDistrictFileIdPVM getDistrictFileWithDistrictFileIdPVM = new GetDistrictFileWithDistrictFileIdPVM()
                {
                    DistrictFileId = districtFileUploadPVM.DistrictFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictFileWithDistrictFileId(getDistrictFileWithDistrictFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<DistrictFilesVM>();


                            if (record != null)
                            {
                                oldDistrictFilesVM = record;

                                districtFilesVM.DistrictFileId = oldDistrictFilesVM.DistrictFileId;
                                districtFilesVM.DistrictFileExt = oldDistrictFilesVM.DistrictFileExt;
                                districtFilesVM.DistrictFilePath = oldDistrictFilesVM.DistrictFilePath;
                                districtFilesVM.DistrictFileType = "docs";
                                districtFilesVM.DistrictId = oldDistrictFilesVM.DistrictId;


                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            #endregion

            try
            {
                districtFilesVM.DistrictFileTitle = districtFileUploadPVM.DistrictFileTitle;
                districtFilesVM.DistrictFileOrder = districtFileUploadPVM.DistrictFileOrder;
                districtFilesVM.DistrictId = oldDistrictFilesVM.DistrictId;
                districtFilesVM.EditEnDate = DateTime.Now;
                districtFilesVM.EditTime = PersianDate.TimeNow;
                districtFilesVM.UserIdEditor = this.userId.Value;
                districtFilesVM.IsActivated = true;
                districtFilesVM.IsDeleted = false;

                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/UpdateDistrictFiles";

                UpdateDistrictFilesPVM updateDistrictFilesPVM = new UpdateDistrictFilesPVM()
                {
                    DistrictFilesVM = districtFilesVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).UpdateDistrictFiles(updateDistrictFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "ویرایش انجام شد",
                            });
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



        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationDistrictFiles(int DistrictFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/ToggleActivationDistrictFiles";

                ToggleActivationDistrictFilesPVM toggleActivationDistrictFilesPVM =
                    new ToggleActivationDistrictFilesPVM()
                    {
                        DistrictFileId = DistrictFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationDistrictFiles(toggleActivationDistrictFilesPVM);

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




        [AjaxOnly]
        [HttpPost]
        public IActionResult TemporaryDeleteDistrictFiles(int DistrictFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/TemporaryDeleteDistrictFiles";

                TemporaryDeleteDistrictFilesPVM temporaryDeleteDistrictFilesPVM =
                    new TemporaryDeleteDistrictFilesPVM()
                    {
                        DistrictFileId = DistrictFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).TemporaryDeleteDistrictFiles(temporaryDeleteDistrictFilesPVM);

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
        public IActionResult CompleteDeleteDistrictFiles(int DistrictFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            DistrictFilesVM districtFilesVM = new DistrictFilesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/GetDistrictFileWithDistrictFileId";

                GetDistrictFileWithDistrictFileIdPVM getDistrictFileWithDistrictFileIdPVM = new GetDistrictFileWithDistrictFileIdPVM()
                {
                    DistrictFileId = DistrictFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictFileWithDistrictFileId(getDistrictFileWithDistrictFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<DistrictFilesVM>();


                            if (record != null)
                            {
                                districtFilesVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            try
            {
                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string districtFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\DistrictFiles\\" + domainSettings.DomainName + "\\" + districtFilesVM.DistrictId + "\\Docs";

                if (districtFilesVM != null)
                {
                    if (!string.IsNullOrEmpty(districtFilesVM.DistrictFilePath))
                    {
                        if (System.IO.File.Exists(districtFolder + "\\" + districtFilesVM.DistrictFilePath))
                        {
                            System.IO.File.Delete(districtFolder + "\\" + districtFilesVM.DistrictFilePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        switch (districtFilesVM.DistrictFileExt.ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                            case ".png":
                            case ".bmp":
                                break;
                            case ".mp4":
                                break;
                        }
                        if (districtFilesVM.DistrictFileExt.ToLower().Equals(".jpg") ||
                            districtFilesVM.DistrictFileExt.ToLower().Equals(".jpeg") ||
                            districtFilesVM.DistrictFileExt.ToLower().Equals(".png") ||
                            districtFilesVM.DistrictFileExt.ToLower().Equals(".bmp"))
                        {
                            if (System.IO.File.Exists(districtFolder + "\\thumb_" + districtFilesVM.DistrictFilePath))
                            {
                                System.IO.File.Delete(districtFolder + "\\thumb_" + districtFilesVM.DistrictFilePath);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }

                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/CompleteDeleteDistrictFiles";

                CompleteDeleteDistrictFilesPVM completeDeleteDistrictFilesPVM =
                    new CompleteDeleteDistrictFilesPVM()
                    {
                        DistrictFileId = DistrictFileId,
                        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        this.domainsSettings.DomainSettingId),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeleteDistrictFiles(completeDeleteDistrictFilesPVM);

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
                Message = "خطا"
            });
        }




        public async Task<IActionResult> FileDownload(int Id = 0)
        {
            if (Id == null)
                return Content("FileNotFound");

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            DistrictFilesVM districtFilesVM = new DistrictFilesVM();

            #region get old file

            try
            {
                string serviceUrl = publicApiUrl + "/api/DistrictFilesManagement/GetDistrictFileWithDistrictFileId";

                GetDistrictFileWithDistrictFileIdPVM getDistrictFileWithDistrictFileIdPVM = new GetDistrictFileWithDistrictFileIdPVM()
                {
                    DistrictFileId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetDistrictFileWithDistrictFileId(getDistrictFileWithDistrictFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<DistrictFilesVM>();


                            if (record != null)
                            {
                                districtFilesVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            #endregion

            if (!string.IsNullOrEmpty(districtFilesVM.DistrictFilePath))
            {

                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string districtFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\DistrictFiles\\" + domainSettings.DomainName + "\\" + districtFilesVM.DistrictId + "\\Docs";

                if (System.IO.File.Exists(districtFolder + "\\" + districtFilesVM.DistrictFilePath))
                {

                    var filePath = districtFolder + "\\" + districtFilesVM.DistrictFilePath;

                    var memory = new MemoryStream();
                    using (var stream = new FileStream(filePath/*path*/, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, ContentTypeManagement.GetContentType(filePath), Path.GetFileName(filePath));
                }
            }

            return Content("FileNotFound");
        }
    }
}

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
    public class ZonesMapsManagementController : ExtraAdminController
    {
        public ZonesMapsManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("Index", "ZonesManagement");

            ViewData["Title"] = "لیست نقشه ها";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;

            ZonesVM ZonesVM = new ZonesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetZoneWithZoneId";

                GetZoneWithZoneIdPVM getZoneWithZoneIdPVM = new GetZoneWithZoneIdPVM()
                {
                    ZoneId = Id,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetZoneWithZoneId(getZoneWithZoneIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ZonesVM>();


                            if (record != null)
                            {
                                ZonesVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["ZonesList"] = ZonesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminPublic/ZonesManagement/Index/";
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfZoneFiles(int jtStartIndex = 0,
            int jtPageSize = 10,
            int? ZoneId = null,
            string ZoneFileTitle = "",
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/GetListOfZoneFiles";

                GetListOfZoneFilesPVM getListOfZoneFilesPVM = new GetListOfZoneFilesPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    ZoneId = ((ZoneId.HasValue) ? ZoneId.Value : 0),
                    ZoneFileTitle = ZoneFileTitle,
                    ZoneFileType = "maps",
                    jtSorting = jtSorting
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfZoneFiles(getListOfZoneFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ZoneFilesVM>>();

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




        public IActionResult AddToZoneFiles(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "ZonesManagement");

            ViewData["Title"] = "آپلود نقشه";

            ZonesVM ZonesVM = new ZonesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZonesManagement/GetZoneWithZoneId";

                GetZoneWithZoneIdPVM getZoneWithZoneIdPVM = new GetZoneWithZoneIdPVM()
                {
                    ZoneId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetZoneWithZoneId(getZoneWithZoneIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ZonesVM>();


                            if (record != null)
                            {
                                ZonesVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["ZonesList"] = ZonesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminPublic/ZonesMapsManagement/Index/" + ZonesVM.ZoneId;

            }

            return View("AddTo");
        }




        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToZoneFiles(List<ZoneFileUploadPVM> ZoneFileUploadPVMList, int ZoneId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            List<ZoneFilesVM> ZoneFilesVM = new List<ZoneFilesVM>();

            try
            {
                if (ZoneFileUploadPVMList != null)
                {
                    if (ZoneFileUploadPVMList.Count > 0)
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                        string ZoneFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ZoneFiles\\" + domainSettings.DomainName + "\\" + ZoneId + "\\Maps";

                        foreach (var ZoneFileUpload in ZoneFileUploadPVMList)
                        {
                            try
                            {
                                string fileName = "";

                                string ext = Path.GetExtension(ZoneFileUpload.File.FileName);
                                fileName = Guid.NewGuid().ToString() + ext;
                                using (var fileStream = new FileStream(ZoneFolder + "\\" + fileName, FileMode.Create))
                                {
                                    await ZoneFileUpload.File.CopyToAsync(fileStream);
                                    System.Threading.Thread.Sleep(100);
                                }

                                if (ext.Equals(".jpeg") ||
                                    ext.Equals(".jpg") ||
                                    ext.Equals(".png") ||
                                    ext.Equals(".gif") ||
                                    ext.Equals(".bmp"))
                                {
                                    ImageModify.ResizeImage(ZoneFolder + "\\",
                                        fileName,
                                        120,
                                        120);
                                }
                                else
                                    if (ext.Equals(".mp4"))
                                {

                                }

                                var ZoneFiles = new ZoneFilesVM()
                                {
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = this.userId.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                    ZoneFileExt = ext,
                                    ZoneFilePath = fileName,
                                    ZoneFileTitle = ZoneFileUpload.ZoneFileTitle,
                                    ZoneFileOrder = ZoneFileUpload.ZoneFileOrder,
                                    ZoneFileType = "maps",
                                    ZoneId = ZoneId,
                                };

                                ZoneFilesVM.Add(ZoneFiles);
                            }
                            catch (Exception exc)
                            { }
                        }
                    }
                }

                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/AddToZoneFiles";

                AddToZoneFilesPVM addToZoneFilesPVM = new AddToZoneFilesPVM()
                {
                    ZoneFilesVM = ZoneFilesVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).AddToZoneFiles(addToZoneFilesPVM);

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
        public async Task<ActionResult> UpdateZoneFiles(ZoneFileUploadPVM ZoneFileUploadPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ZoneFilesVM ZoneFilesVM = new ZoneFilesVM();
            ZoneFilesVM oldZoneFilesVM = new ZoneFilesVM();

            #region get old file

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/GetZoneFileWithZoneFileId";

                GetZoneFileWithZoneFileIdPVM getZoneFileWithZoneFileIdPVM = new GetZoneFileWithZoneFileIdPVM()
                {
                    ZoneFileId = ZoneFileUploadPVM.ZoneFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetZoneFileWithZoneFileId(getZoneFileWithZoneFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ZoneFilesVM>();


                            if (record != null)
                            {
                                oldZoneFilesVM = record;

                                ZoneFilesVM.ZoneFileId = oldZoneFilesVM.ZoneFileId;
                                ZoneFilesVM.ZoneFileExt = oldZoneFilesVM.ZoneFileExt;
                                ZoneFilesVM.ZoneFilePath = oldZoneFilesVM.ZoneFilePath;
                                ZoneFilesVM.ZoneFileType = "maps";
                                ZoneFilesVM.ZoneId = oldZoneFilesVM.ZoneId;


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
                ZoneFilesVM.ZoneFileTitle = ZoneFileUploadPVM.ZoneFileTitle;
                ZoneFilesVM.ZoneFileOrder = ZoneFileUploadPVM.ZoneFileOrder;
                ZoneFilesVM.ZoneId = oldZoneFilesVM.ZoneId;
                ZoneFilesVM.EditEnDate = DateTime.Now;
                ZoneFilesVM.EditTime = PersianDate.TimeNow;
                ZoneFilesVM.UserIdEditor = this.userId.Value;
                ZoneFilesVM.IsActivated = true;
                ZoneFilesVM.IsDeleted = false;

                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/UpdateZoneFiles";

                UpdateZoneFilesPVM updateZoneFilesPVM = new UpdateZoneFilesPVM()
                {
                    ZoneFilesVM = ZoneFilesVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).UpdateZoneFiles(updateZoneFilesPVM);

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
        public IActionResult ToggleActivationZoneFiles(int ZoneFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/ToggleActivationZoneFiles";

                ToggleActivationZoneFilesPVM toggleActivationZoneFilesPVM =
                    new ToggleActivationZoneFilesPVM()
                    {
                        ZoneFileId = ZoneFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationZoneFiles(toggleActivationZoneFilesPVM);

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
        public IActionResult TemporaryDeleteZoneFiles(int ZoneFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/TemporaryDeleteZoneFiles";

                TemporaryDeleteZoneFilesPVM temporaryDeleteZoneFilesPVM =
                    new TemporaryDeleteZoneFilesPVM()
                    {
                        ZoneFileId = ZoneFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).TemporaryDeleteZoneFiles(temporaryDeleteZoneFilesPVM);

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
        public IActionResult CompleteDeleteZoneFiles(int ZoneFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            ZoneFilesVM ZoneFilesVM = new ZoneFilesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/GetZoneFileWithZoneFileId";

                GetZoneFileWithZoneFileIdPVM getZoneFileWithZoneFileIdPVM = new GetZoneFileWithZoneFileIdPVM()
                {
                    ZoneFileId = ZoneFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetZoneFileWithZoneFileId(getZoneFileWithZoneFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ZoneFilesVM>();


                            if (record != null)
                            {
                                ZoneFilesVM = record;

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

                string ZoneFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ZoneFiles\\" + domainSettings.DomainName + "\\" + ZoneFilesVM.ZoneId + "\\Maps";

                if (ZoneFilesVM != null)
                {
                    if (!string.IsNullOrEmpty(ZoneFilesVM.ZoneFilePath))
                    {
                        if (System.IO.File.Exists(ZoneFolder + "\\" + ZoneFilesVM.ZoneFilePath))
                        {
                            System.IO.File.Delete(ZoneFolder + "\\" + ZoneFilesVM.ZoneFilePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        switch (ZoneFilesVM.ZoneFileExt.ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                            case ".png":
                            case ".bmp":
                                break;
                            case ".mp4":
                                break;
                        }
                        if (ZoneFilesVM.ZoneFileExt.ToLower().Equals(".jpg") ||
                            ZoneFilesVM.ZoneFileExt.ToLower().Equals(".jpeg") ||
                            ZoneFilesVM.ZoneFileExt.ToLower().Equals(".png") ||
                            ZoneFilesVM.ZoneFileExt.ToLower().Equals(".bmp"))
                        {
                            if (System.IO.File.Exists(ZoneFolder + "\\thumb_" + ZoneFilesVM.ZoneFilePath))
                            {
                                System.IO.File.Delete(ZoneFolder + "\\thumb_" + ZoneFilesVM.ZoneFilePath);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }

                string serviceUrl = publicApiUrl + "/api/ZoneFilesManagement/CompleteDeleteZoneFiles";

                CompleteDeleteZoneFilesPVM completeDeleteZoneFilesPVM =
                    new CompleteDeleteZoneFilesPVM()
                    {
                        ZoneFileId = ZoneFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeleteZoneFiles(completeDeleteZoneFilesPVM);

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

    }
}

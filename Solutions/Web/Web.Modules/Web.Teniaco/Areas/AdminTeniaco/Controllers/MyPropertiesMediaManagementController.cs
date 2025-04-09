using ApiCallers.TeniacoApiCaller;
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
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class MyPropertiesMediaManagementController : ExtraAdminController
    {
        public MyPropertiesMediaManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("Index", "MyPropertiesManagement");

            ViewData["Title"] = "لیست فیلم/عکس";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;

            MyPropertiesVM propertiesVM = new MyPropertiesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/GetMyPropertyWithMyPropertyId";

                GetMyPropertyWithMyPropertyIdPVM getPropertyWithPropertyIdPVM = new GetMyPropertyWithMyPropertyIdPVM()
                {
                    PropertyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyWithMyPropertyId(getPropertyWithPropertyIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertiesVM>();


                            if (record != null)
                            {
                                propertiesVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["MyPropertiesVM"] = propertiesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MyPropertiesManagement/Index/";
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllMyPropertyFilesList(
            int? propertyId = null,
            string propertyFileTitle = "",
            string propertyFileType = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/GetAllMyPropertyFilesList";

                GetAllMyPropertyFilesListPVM getAllMyPropertyFilesListPVM = new GetAllMyPropertyFilesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    PropertyId = propertyId.Value,
                    PropertyFileTitle = propertyFileTitle,
                    PropertyFileType = propertyFileType,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMyPropertyFilesList(getAllMyPropertyFilesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MyPropertyFilesVM>>();

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
                                Records = records,
                                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                            });
                        }
                    }
                }
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
        public IActionResult GetListOfMyPropertyFiles(int jtStartIndex = 0,
            int jtPageSize = 10,
            int? propertyId = null,
            string propertyFileTitle = "",
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/GetListOfMyPropertyFiles";

                GetListOfMyPropertyFilesPVM getListOfPropertyFilesPVM = new GetListOfMyPropertyFilesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    PropertyId = ((propertyId.HasValue) ? propertyId.Value : 0),
                    PropertyFileTitle = propertyFileTitle,
                    PropertyFileType = "media",
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfMyPropertyFiles(getListOfPropertyFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {

                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<MyPropertyFilesVM>>();

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

        public IActionResult AddToMyPropertyFiles(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "MyPropertiesManagement");

            ViewData["Title"] = "آپلود فیلم/عکس";

            MyPropertiesVM propertiesVM = new MyPropertiesVM();
            propertiesVM.MyPropertyAddressVM = new MyPropertyAddressVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesManagement/GetMyPropertyWithMyPropertyId";

                GetMyPropertyWithMyPropertyIdPVM getPropertyWithPropertyIdPVM = new GetMyPropertyWithMyPropertyIdPVM()
                {
                    PropertyId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyWithMyPropertyId(getPropertyWithPropertyIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertiesVM>();


                            if (record != null)
                            {
                                propertiesVM = record;


                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["MyPropertiesVM"] = propertiesVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/MyPropertiesMediaManagement/Index/" + propertiesVM.PropertyId;
            }

            return View("AddTo");
        }


        //[DisableRequestSizeLimit]
        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToMyPropertyFiles(List<MyPropertyFileUploadPVM> propertyFileUploadPVMList, int propertyId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            List<MyPropertyFilesVM> propertyFilesVMList = new List<MyPropertyFilesVM>();

            try
            {
                if (propertyFileUploadPVMList != null)
                {
                    if (propertyFileUploadPVMList.Count > 0)
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                        string propertyFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PropertiesFiles\\" + domainSettings.DomainName + "\\" + propertyId + "\\Media";

                        foreach (var propertyFileUploadPVM in propertyFileUploadPVMList)
                        {
                            try
                            {
                                string fileName = "";

                                string ext = Path.GetExtension(propertyFileUploadPVM.File.FileName);
                                fileName = Guid.NewGuid().ToString() + ext;
                                using (var fileStream = new FileStream(propertyFolder + "\\" + fileName, FileMode.Create))
                                {
                                    await propertyFileUploadPVM.File.CopyToAsync(fileStream);
                                    System.Threading.Thread.Sleep(100);
                                }

                                if (ext.Equals(".jpeg") ||
                                    ext.Equals(".jpg") ||
                                    ext.Equals(".png") ||
                                    ext.Equals(".gif") ||
                                    ext.Equals(".bmp"))
                                {
                                    ImageModify.ResizeImage(propertyFolder + "\\",
                                        fileName,
                                        120,
                                        120);
                                }
                                else
                                    if (ext.Equals(".mp4"))
                                {

                                }

                                var propertyFilesVM = new MyPropertyFilesVM()
                                {
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = this.userId.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                    PropertyFileExt = ext,
                                    PropertyFilePath = fileName,
                                    PropertyFileTitle = propertyFileUploadPVM.PropertyFileTitle,
                                    PropertyFileOrder = propertyFileUploadPVM.PropertyFileOrder,
                                    PropertyFileType = "media",
                                    PropertyId = propertyId,
                                };
                                //propertyFilesVM.PropertiesVM = new PropertiesVM();
                                //propertyFilesVM.PropertiesVM.PropertyAddressVM = new PropertyAddressVM();

                                propertyFilesVMList.Add(propertyFilesVM);
                            }
                            catch (Exception exc)
                            { }
                        }
                    }
                }

                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/AddToMyPropertyFiles";

                AddToMyPropertyFilesPVM addToPropertyFilesPVM = new AddToMyPropertyFilesPVM()
                {
                    MyPropertyFilesVMList = propertyFilesVMList,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToMyPropertyFiles(addToPropertyFilesPVM);

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
        public async Task<ActionResult> UpdateMyPropertyFiles(MyPropertyFileUploadPVM propertyFileUploadPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            MyPropertyFilesVM propertyFilesVM = new MyPropertyFilesVM();
            MyPropertyFilesVM oldPropertyFilesVM = new MyPropertyFilesVM();

            #region get old file

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/GetMyPropertyFileWithMyPropertyFileId";

                GetMyPropertyFileWithMyPropertyFileIdPVM getPropertyFileWithPropertyFileIdPVM = new GetMyPropertyFileWithMyPropertyFileIdPVM()
                {
                    PropertyFileId = propertyFileUploadPVM.PropertyFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyFileWithMyPropertyFileId(getPropertyFileWithPropertyFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertyFilesVM>();


                            if (record != null)
                            {
                                oldPropertyFilesVM = record;
                                propertyFilesVM.PropertyFileId = oldPropertyFilesVM.PropertyFileId;
                                propertyFilesVM.PropertyFileExt = oldPropertyFilesVM.PropertyFileExt;
                                propertyFilesVM.PropertyFilePath = oldPropertyFilesVM.PropertyFilePath;
                                //propertyFilesVM.PropertyFileTitle = oldPropertyFilesVM.PropertyFileTitle;
                                propertyFilesVM.PropertyFileType = "media";
                                propertyFilesVM.PropertyId = oldPropertyFilesVM.PropertyId;

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
                propertyFilesVM.PropertyFileTitle = propertyFileUploadPVM.PropertyFileTitle;
                propertyFilesVM.PropertyFileOrder = propertyFileUploadPVM.PropertyFileOrder;
                propertyFilesVM.PropertyId = oldPropertyFilesVM.PropertyId;
                propertyFilesVM.EditEnDate = DateTime.Now;
                propertyFilesVM.EditTime = PersianDate.TimeNow;
                propertyFilesVM.UserIdEditor = this.userId.Value;
                propertyFilesVM.IsActivated = true;
                propertyFilesVM.IsDeleted = false;

                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/UpdateMyPropertyFiles";

                UpdateMyPropertyFilesPVM updatePropertyFilesPVM = new UpdateMyPropertyFilesPVM()
                {
                    MyPropertyFilesVM = propertyFilesVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateMyPropertyFiles(updatePropertyFilesPVM);

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
                                //Message = "آپلود انجام شد",
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
        public IActionResult ToggleActivationMyPropertyFiles(int PropertyFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/ToggleActivationMyPropertyFiles";

                ToggleActivationMyPropertyFilesPVM toggleActivationPropertyFilesPVM =
                    new ToggleActivationMyPropertyFilesPVM()
                    {
                        PropertyFileId = PropertyFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationMyPropertyFiles(toggleActivationPropertyFilesPVM);

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
        public IActionResult TemporaryDeleteMyPropertyFiles(int PropertyFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/TemporaryDeleteMyPropertyFiles";

                TemporaryDeleteMyPropertyFilesPVM temporaryDeletePropertyFilesPVM =
                    new TemporaryDeleteMyPropertyFilesPVM()
                    {
                        PropertyFileId = PropertyFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteMyPropertyFiles(temporaryDeletePropertyFilesPVM);

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
        public IActionResult CompleteDeleteMyPropertyFiles(int PropertyFileId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            MyPropertyFilesVM propertyFilesVM = new MyPropertyFilesVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/GetMyPropertyFileWithMyPropertyFileId";

                GetMyPropertyFileWithMyPropertyFileIdPVM getPropertyFileWithPropertyFileIdPVM = new GetMyPropertyFileWithMyPropertyFileIdPVM()
                {
                    PropertyFileId = PropertyFileId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetMyPropertyFileWithMyPropertyFileId(getPropertyFileWithPropertyFileIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<MyPropertyFilesVM>();


                            if (record != null)
                            {
                                propertyFilesVM = record;


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

                string propertyFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PropertiesFiles\\" + domainSettings.DomainName + "\\" + propertyFilesVM.PropertyId + "\\Media";

                if (propertyFilesVM != null)
                {
                    if (!string.IsNullOrEmpty(propertyFilesVM.PropertyFilePath))
                    {
                        if (System.IO.File.Exists(propertyFolder + "\\" + propertyFilesVM.PropertyFilePath))
                        {
                            System.IO.File.Delete(propertyFolder + "\\" + propertyFilesVM.PropertyFilePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        switch (propertyFilesVM.PropertyFileExt.ToLower())
                        {
                            case ".jpg":
                            case ".jpeg":
                            case ".png":
                            case ".bmp":
                                break;
                            case ".mp4":
                                break;
                        }
                        if (propertyFilesVM.PropertyFileExt.ToLower().Equals(".jpg") ||
                            propertyFilesVM.PropertyFileExt.ToLower().Equals(".jpeg") ||
                            propertyFilesVM.PropertyFileExt.ToLower().Equals(".png") ||
                            propertyFilesVM.PropertyFileExt.ToLower().Equals(".bmp"))
                        {
                            if (System.IO.File.Exists(propertyFolder + "\\thumb_" + propertyFilesVM.PropertyFilePath))
                            {
                                System.IO.File.Delete(propertyFolder + "\\thumb_" + propertyFilesVM.PropertyFilePath);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                    }
                }

                string serviceUrl = teniacoApiUrl + "/api/MyPropertiesFilesManagement/CompleteDeleteMyPropertyFiles";

                CompleteDeleteMyPropertyFilesPVM completeDeletePropertyFilesPVM =
                    new CompleteDeleteMyPropertyFilesPVM()
                    {
                        PropertyFileId = PropertyFileId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteMyPropertyFiles(completeDeletePropertyFilesPVM);

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

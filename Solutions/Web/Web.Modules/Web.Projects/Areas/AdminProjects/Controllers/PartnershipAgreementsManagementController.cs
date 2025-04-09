using ApiCallers.ProjectsApiCaller;
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
using VM.Projects;
using VM.PVM.Projects;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class PartnershipAgreementsManagementController : ExtraAdminController
    {
        public PartnershipAgreementsManagementController(IHostEnvironment _hostEnvironment,
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
            //ViewData["Title"] = "لیست قرارداد مشارکت";
            ViewData["Title"] = "لیست درخواست ها";
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
        public IActionResult GetAllPartnershipAgreementsList(
            string? PartnershipAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            try
            {
                List<PartnershipAgreementsVM> partnershipAgreementsVMList = new List<PartnershipAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/GetAllPartnershipAgreementsList";

                    GetAllPartnershipAgreementsListPVM getAllPartnershipAgreementsListPVM = new GetAllPartnershipAgreementsListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        PartnershipAgreementTitle = PartnershipAgreementTitle,
                        ConstructionProjectId = ConstructionProjectId
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllPartnershipAgreementsList(getAllPartnershipAgreementsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                partnershipAgreementsVMList = jArray.ToObject<List<PartnershipAgreementsVM>>();


                                if (partnershipAgreementsVMList != null)
                                    if (partnershipAgreementsVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<PartnershipAgreementsVM>>();

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
        public IActionResult GetListOfPartnershipAgreements(
            int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string partnershipAgreementTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<PartnershipAgreementsVM> partnershipAgreementsVMList = new List<PartnershipAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/GetListOfPartnershipAgreements";
                    GetListOfPartnershipAgreementsPVM getListOfPartnershipAgreementsPVM = new GetListOfPartnershipAgreementsPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("PartnershipAgreementsManagement", "GetListOfPartnershipAgreements", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        PartnershipAgreementTitle = partnershipAgreementTitle,
                        ConstructionProjectId = ConstructionProjectId,
                        UserId = this.userId.Value,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfPartnershipAgreements(getListOfPartnershipAgreementsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                partnershipAgreementsVMList = jArray.ToObject<List<PartnershipAgreementsVM>>();


                                if (partnershipAgreementsVMList != null)
                                    if (partnershipAgreementsVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<PartnershipAgreementsVM>>();

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

        public IActionResult AddToPartnershipAgreements(long Id)
        {
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["ConstructionProjectId"] = Id;
            //ViewData["Title"] = "آپلود قرارداد مشارکت";
            ViewData["Title"] = "آپلود درخواست جدید";
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


        #region oldcodes - comments


        //[AjaxOnly]
        //[HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        //[RequestSizeLimit(long.MaxValue)]
        //public async Task<ActionResult> AddToPartnershipAgreements(AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM)
        //{
        //    JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

        //    PartnershipAgreementsVM partnershipAgreementsVM = new PartnershipAgreementsVM();

        //    try
        //    {
        //        if (addToPartnershipAgreementsPVM != null)
        //        {
        //            var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


        //            string fileName = "";
        //            string fileType = "";
        //            string ext = Path.GetExtension(addToPartnershipAgreementsPVM.File.FileName);
        //            fileName = Guid.NewGuid().ToString() + ext;

        //            if (ext.Equals(".jpeg") ||
        //                ext.Equals(".jpg") ||
        //                ext.Equals(".png") ||
        //                ext.Equals(".gif") ||
        //                ext.Equals(".bmp"))
        //            {
        //                fileType = "media";

        //            }
        //            else if (ext.Equals(".pdf"))
        //            {
        //                fileType = "pdf";
        //            }
        //            else
        //            {
        //                fileType = "other";
        //            }


        //            partnershipAgreementsVM = new PartnershipAgreementsVM()
        //            {
        //                CreateEnDate = DateTime.Now,
        //                CreateTime = PersianDate.TimeNow,
        //                UserIdCreator = this.userId.Value,
        //                IsActivated = true,
        //                IsDeleted = false,
        //                PartnershipAgreementFileExt = ext,
        //                PartnershipAgreementFilePath = fileName,
        //                ConstructionProjectId = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.ConstructionProjectId,
        //                PartnershipAgreementDescription = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementDescription,
        //                PartnershipAgreementNumber = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementNumber,
        //                PartnershipAgreementFileOrder = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementFileOrder,
        //                PartnershipAgreementFileType = fileType,
        //                //PartnershipAgreementLink = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementLink,
        //                PartnershipAgreementTitle = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementTitle,
        //            };

        //            string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/AddToPartnershipAgreements";

        //            AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM1 = new AddToPartnershipAgreementsPVM()
        //            {
        //                PartnershipAgreementsVM = partnershipAgreementsVM,
        //                //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
        //                //    this.domainsSettings.DomainSettingId),
        //                ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
        //                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

        //                UserId = this.userId.Value
        //            };

        //            responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToPartnershipAgreements(addToPartnershipAgreementsPVM1);

        //            if (responseApiCaller.IsSuccessStatusCode)
        //            {
        //                jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

        //                if (jsonResultWithRecordObjectVM != null)
        //                {
        //                    if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
        //                    {
        //                        JObject jObject = jsonResultWithRecordObjectVM.Record;
        //                        partnershipAgreementsVM.PartnershipAgreementId = jObject.ToObject<PartnershipAgreementsVM>().PartnershipAgreementId;
        //                    }
        //                }
        //            }

        //            try
        //            {
        //                if (partnershipAgreementsVM.PartnershipAgreementId == 0)
        //                {

        //                    return Json(new
        //                    {
        //                        Result = "ERROR",
        //                        Message = "خطا"
        //                    });
        //                }
        //                //string partnershipAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PartnershipAgreementFiles\\" + domainSettings.DomainName + "\\" + partnershipAgreementsVM.PartnershipAgreementId + "\\Image";
        //                string partnershipAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainSettings.DomainName +
        //                    "\\" + partnershipAgreementsVM.ConstructionProjectId + "\\PartnershipAgreementFiles\\" + partnershipAgreementsVM.PartnershipAgreementId + "\\Image";
        //                if (!Directory.Exists(partnershipAgreementFolder))
        //                {
        //                    Directory.CreateDirectory(partnershipAgreementFolder);
        //                }
        //                using (var fileStream = new FileStream(partnershipAgreementFolder + "\\" + fileName, FileMode.Create))
        //                {
        //                    await addToPartnershipAgreementsPVM.File.CopyToAsync(fileStream);
        //                    System.Threading.Thread.Sleep(100);
        //                }

        //                //if (ext.Equals(".jpeg") ||
        //                //    ext.Equals(".jpg") ||
        //                //    ext.Equals(".png") ||
        //                //    ext.Equals(".gif") ||
        //                //    ext.Equals(".bmp"))
        //                //{
        //                //    ImageModify.ResizeImage(partnershipAgreementFolder + "\\",
        //                //        fileName,
        //                //        120,
        //                //        120);
        //                //}
        //                return Json(new
        //                {
        //                    Result = "OK",
        //                    Message = "آپلود انجام شد",
        //                });

        //            }
        //            catch (Exception exc)
        //            { }
        //        }



        //    }
        //    catch (Exception exc)
        //    { }

        //    return Json(new
        //    {
        //        Result = "ERROR",
        //        Message = "خطا"
        //    });
        //}


        #region old codes
        //[AjaxOnly]
        //[HttpPost]
        //public IActionResult AddToPartnershipAgreements(AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM)
        //{
        //    JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

        //    PartnershipAgreementsVM partnershipAgreementsVM = new PartnershipAgreementsVM();

        //    try
        //    {
        //        if (addToPartnershipAgreementsPVM != null)
        //        {
        //            var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


        //            partnershipAgreementsVM = new PartnershipAgreementsVM()
        //            {
        //                CreateEnDate = DateTime.Now,
        //                CreateTime = PersianDate.TimeNow,
        //                UserIdCreator = this.userId.Value,
        //                IsActivated = true,
        //                IsDeleted = false,
        //                ConstructionProjectId = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.ConstructionProjectId,
        //                PartnershipAgreementTitle = addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementTitle,
        //            };

        //            string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/AddToPartnershipAgreements";

        //            AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM1 = new AddToPartnershipAgreementsPVM()
        //            {
        //                PartnershipAgreementsVM = partnershipAgreementsVM,
        //                //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
        //                //    this.domainsSettings.DomainSettingId),
        //                ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
        //                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

        //                UserId = this.userId.Value
        //            };

        //            responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToPartnershipAgreements(addToPartnershipAgreementsPVM1);

        //            if (responseApiCaller.IsSuccessStatusCode)
        //            {
        //                jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

        //                if (jsonResultWithRecordObjectVM != null)
        //                {
        //                    if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
        //                    {
        //                        JObject jObject = jsonResultWithRecordObjectVM.Record;
        //                        partnershipAgreementsVM.PartnershipAgreementId = jObject.ToObject<PartnershipAgreementsVM>().PartnershipAgreementId;

        //                    }
        //                }
        //            }
        //        }



        //    }
        //    catch (Exception exc)
        //    { }

        //    return Json(new
        //    {
        //        Result = "ERROR",
        //        Message = "خطا"
        //    });
        //}
        #endregion

        #endregion


        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToPartnershipAgreements(PartnershipAgreementsVM partnershipAgreementsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                partnershipAgreementsVM.CreateEnDate = DateTime.Now;
                partnershipAgreementsVM.CreateTime = PersianDate.TimeNow;
                partnershipAgreementsVM.UserIdCreator = this.userId.Value;
                partnershipAgreementsVM.IsActivated = true;
                partnershipAgreementsVM.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/AddToPartnershipAgreements";

                    AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM1 = new AddToPartnershipAgreementsPVM()
                    {
                        PartnershipAgreementsVM = partnershipAgreementsVM,

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToPartnershipAgreements(addToPartnershipAgreementsPVM1);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<PartnershipAgreementsVM>();

                                if (record != null)
                                {
                                    partnershipAgreementsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = partnershipAgreementsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicatePartnerShip"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
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
        public IActionResult UpdatePartnershipAgreements(PartnershipAgreementsVM partnershipAgreementsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                partnershipAgreementsVM.EditEnDate = DateTime.Now;
                partnershipAgreementsVM.EditTime = PersianDate.TimeNow;
                partnershipAgreementsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/UpdatePartnershipAgreements";

                UpdatePartnershipAgreementsPVM updatePartnershipAgreementsPVM = new UpdatePartnershipAgreementsPVM()
                {
                    PartnershipAgreementsVM = partnershipAgreementsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdatePartnershipAgreements(updatePartnershipAgreementsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<PartnershipAgreementsVM>();

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
        public IActionResult ToggleActivationPartnershipAgreements(int PartnershipAgreementId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/ToggleActivationPartnershipAgreements";

                ToggleActivationPartnershipAgreementsPVM toggleActivationPartnershipAgreementsPVM =
                    new ToggleActivationPartnershipAgreementsPVM()
                    {
                        PartnershipAgreementId = PartnershipAgreementId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationPartnershipAgreements(toggleActivationPartnershipAgreementsPVM);

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
        public IActionResult TemporaryDeletePartnershipAgreements(int PartnershipAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/TemporaryDeletePartnershipAgreements";
                TemporaryDeletePartnershipAgreementsPVM temporaryDeletePartnershipAgreementsPVM = new TemporaryDeletePartnershipAgreementsPVM
                {
                    PartnershipAgreementId = PartnershipAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeletePartnershipAgreements(temporaryDeletePartnershipAgreementsPVM);

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
        public IActionResult CompleteDeletePartnershipAgreements(int PartnershipAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            PartnershipAgreementsVM partnershipAgreementsVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/GetPartnershipAgreementsWithPartnershipAgreementId";

                GetPartnershipAgreementsWithPartnershipAgreementIdPVM getPartnershipAgreementsWithPartnershipAgreementIdPVM = new GetPartnershipAgreementsWithPartnershipAgreementIdPVM()
                {
                    PartnershipAgreementId = PartnershipAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetPartnershipAgreementsWithPartnershipAgreementId(getPartnershipAgreementsWithPartnershipAgreementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<PartnershipAgreementsVM>();


                            if (record != null)
                            {
                                partnershipAgreementsVM = record;

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


                serviceUrl = projectsApiUrl + "/api/PartnershipAgreementsManagement/CompleteDeletePartnershipAgreements";
                CompleteDeletePartnershipAgreementsPVM completeDeletePartnershipAgreementsPVM = new CompleteDeletePartnershipAgreementsPVM
                {
                    PartnershipAgreementId = PartnershipAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeletePartnershipAgreements(completeDeletePartnershipAgreementsPVM);

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

                string partnershipAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\PartnershipAgreementFiles\\" + domainSettings.DomainName + "\\" + partnershipAgreementsVM.PartnershipAgreementId + "\\Image";

                if (!string.IsNullOrEmpty(partnershipAgreementsVM.PartnershipAgreementFilePath))
                {
                    if (System.IO.File.Exists(partnershipAgreementFolder + "\\" + partnershipAgreementsVM.PartnershipAgreementFilePath))
                    {
                        System.IO.File.Delete(partnershipAgreementFolder + "\\" + partnershipAgreementsVM.PartnershipAgreementFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (partnershipAgreementsVM.PartnershipAgreementFileExt.ToLower().Equals(".jpg") ||
                    //    partnershipAgreementsVM.PartnershipAgreementFileExt.ToLower().Equals(".jpeg") ||
                    //    partnershipAgreementsVM.PartnershipAgreementFileExt.ToLower().Equals(".png") ||
                    //    partnershipAgreementsVM.PartnershipAgreementFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(partnershipAgreementFolder + "\\thumb_" + partnershipAgreementsVM.PartnershipAgreementFilePath))
                    //    {
                    //        System.IO.File.Delete(partnershipAgreementFolder + "\\thumb_" + partnershipAgreementsVM.PartnershipAgreementFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(partnershipAgreementFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(partnershipAgreementFolder + "\\..");
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
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".dwg":
                        contentType = "application/octet-stream"; // Adjust as needed
                        break;
                    case ".skp":
                        contentType = "application/octet-stream"; // Adjust as needed
                        break;
                    case ".mp4":
                        contentType = "video/mp4";
                        break;
                    case ".mkv":
                        contentType = "video/x-matroska";
                        break;
                    case ".xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".doc":
                    case ".docx":
                        contentType = "application/msword";
                        break;
                    default:
                        contentType = "application/octet-stream"; // Default to binary if the type is unknown
                        break;
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

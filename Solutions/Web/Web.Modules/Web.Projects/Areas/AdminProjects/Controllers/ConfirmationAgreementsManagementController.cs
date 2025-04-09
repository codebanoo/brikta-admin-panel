using Microsoft.AspNetCore.Mvc;
using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Http;
using VM.Projects;
using VM.PVM.Projects;
using ApiCallers.ProjectsApiCaller;
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
using System.Net.Mime;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class ConfirmationAgreementsManagementController : ExtraAdminController
    {
        public ConfirmationAgreementsManagementController(IHostEnvironment _hostEnvironment,
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

            ViewData["Title"] = "لیست قرارداد تائیدیه";

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



            List<ConfirmationTypesVM> confirmationTypesVMList = new List<ConfirmationTypesVM>();

            try
            {
                serviceUrl = projectsApiUrl + "/api/ConfirmationTypesManagement/GetAllConfirmationTypesList";
                GetAllConfirmationTypesListPVM getListOfConfirmationTypesPVM = new GetAllConfirmationTypesListPVM
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConfirmationTypesList(getListOfConfirmationTypesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            confirmationTypesVMList = jArray.ToObject<List<ConfirmationTypesVM>>();


                            if (confirmationTypesVMList != null)
                                if (confirmationTypesVMList.Count >= 0)
                                {
                                    ViewData["ConfirmationTypesVMList"] = confirmationTypesVMList;

                                }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception exc)
            { }



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
        public IActionResult GetAllConfirmationAgreementsList(
            string? ConfirmationAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            try
            {
                List<ConfirmationAgreementsVM> confirmationAgreementsVMList = new List<ConfirmationAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/GetAllConfirmationAgreementsList";

                    GetAllConfirmationAgreementsListPVM getAllConfirmationAgreementsListPVM = new GetAllConfirmationAgreementsListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                       
                        ConfirmationAgreementTitle = ConfirmationAgreementTitle,
                        ConstructionProjectId = ConstructionProjectId,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConfirmationAgreementsList(getAllConfirmationAgreementsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                confirmationAgreementsVMList = jArray.ToObject<List<ConfirmationAgreementsVM>>();


                                if (confirmationAgreementsVMList != null)
                                    if (confirmationAgreementsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConfirmationAgreementsVM>>();

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
        public IActionResult GetListOfConfirmationAgreements(
            int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string confirmationAgreementTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<ConfirmationAgreementsVM> confirmationAgreementsVMList = new List<ConfirmationAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/GetListOfConfirmationAgreements";
                    GetListOfConfirmationAgreementsPVM getListOfConfirmationAgreementsPVM = new GetListOfConfirmationAgreementsPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ConfirmationAgreementTitle = confirmationAgreementTitle,
                        ConstructionProjectId = ConstructionProjectId,
                        jtSorting = jtSorting,
                        UserId = this.userId.Value,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfConfirmationAgreements(getListOfConfirmationAgreementsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                confirmationAgreementsVMList = jArray.ToObject<List<ConfirmationAgreementsVM>>();


                                if (confirmationAgreementsVMList != null)
                                    if (confirmationAgreementsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConfirmationAgreementsVM>>();

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



        public IActionResult AddToConfirmationAgreements(long Id)
        {
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["ConstructionProjectId"] = Id;
            ViewData["Title"] = "آپلود قرارداد تائیدیه";
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

               
            List<ConfirmationTypesVM> confirmationTypesVMList = new List<ConfirmationTypesVM>();
            try
            {
                serviceUrl = projectsApiUrl + "/api/ConfirmationTypesManagement/GetAllConfirmationTypesList";
                GetAllConfirmationTypesListPVM getListOfConfirmationTypesPVM = new GetAllConfirmationTypesListPVM
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConfirmationTypesList(getListOfConfirmationTypesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            confirmationTypesVMList = jArray.ToObject<List<ConfirmationTypesVM>>();


                            if (confirmationTypesVMList != null)
                                if (confirmationTypesVMList.Count >= 0)
                                {
                                    ViewData["ConfirmationTypesVMList"] = confirmationTypesVMList;

                                }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception exc)
            { }
            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToConfirmationAgreements(AddToConfirmationAgreementsPVM addToConfirmationAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ConfirmationAgreementsVM confirmationAgreementsVM = new ConfirmationAgreementsVM();

            try
            {
                if (addToConfirmationAgreementsPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string fileType = "";

                    string ext = Path.GetExtension(addToConfirmationAgreementsPVM.File.FileName);
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

                    confirmationAgreementsVM = new ConfirmationAgreementsVM()
                    {
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        IsConfirm = false,
                        ConfirmationAgreementFileExt = ext,
                        ConfirmationAgreementFilePath = fileName,
                        ConstructionProjectId = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConstructionProjectId,
                        ConfirmationAgreementDescription = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementDescription,
                        ConfirmationAgreementNumber = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementNumber,
                        ConfirmationAgreementFileOrder = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementFileOrder,
                        ConfirmationAgreementFileType = fileType,
                        //ConfirmationAgreementLink = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementLink,
                        ConfirmationAgreementTitle = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementTitle,
                        ConfirmationTypeId = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationTypeId,

                    };

                    string serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/AddToConfirmationAgreements";

                    AddToConfirmationAgreementsPVM addToConfirmationAgreementsPVM1 = new AddToConfirmationAgreementsPVM()
                    {
                        ConfirmationAgreementsVM = confirmationAgreementsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToConfirmationAgreements(addToConfirmationAgreementsPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                confirmationAgreementsVM.ConfirmationAgreementId = jObject.ToObject<ConfirmationAgreementsVM>().ConfirmationAgreementId;
                            }
                        }
                    }

                    try
                    {
                        if (confirmationAgreementsVM.ConfirmationAgreementId == 0)
                        {

                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                        //string confirmationAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConfirmationAgreementFiles\\" + domainSettings.DomainName + "\\" + confirmationAgreementsVM.ConfirmationAgreementId + "\\Image";
                        string confirmationAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainSettings.DomainName +
                            "\\" + confirmationAgreementsVM.ConstructionProjectId + "\\ConfirmationAgreementFiles\\" + confirmationAgreementsVM.ConfirmationAgreementId + "\\Image";
                        if (!Directory.Exists(confirmationAgreementFolder))
                        {
                            Directory.CreateDirectory(confirmationAgreementFolder);
                        }
                        using (var fileStream = new FileStream(confirmationAgreementFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToConfirmationAgreementsPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //if (ext.Equals(".jpeg") ||
                        //    ext.Equals(".jpg") ||
                        //    ext.Equals(".png") ||
                        //    ext.Equals(".gif") ||
                        //    ext.Equals(".bmp"))
                        //{
                        //    ImageModify.ResizeImage(confirmationAgreementFolder + "\\",
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
        public IActionResult UpdateConfirmationAgreements(ConfirmationAgreementsVM confirmationAgreementsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                confirmationAgreementsVM.EditEnDate = DateTime.Now;
                confirmationAgreementsVM.EditTime = PersianDate.TimeNow;
                confirmationAgreementsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/UpdateConfirmationAgreements";

                UpdateConfirmationAgreementsPVM updateConfirmationAgreementsPVM = new UpdateConfirmationAgreementsPVM()
                {
                    ConfirmationAgreementsVM = confirmationAgreementsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateConfirmationAgreements(updateConfirmationAgreementsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ConfirmationAgreementsVM>();

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
        public IActionResult ToggleActivationConfirmationAgreements(int ConfirmationAgreementId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/ToggleActivationConfirmationAgreements";

                ToggleActivationConfirmationAgreementsPVM toggleActivationConfirmationAgreementsPVM =
                    new ToggleActivationConfirmationAgreementsPVM()
                    {
                        ConfirmationAgreementId = ConfirmationAgreementId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationConfirmationAgreements(toggleActivationConfirmationAgreementsPVM);

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
        public IActionResult TemporaryDeleteConfirmationAgreements(int ConfirmationAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/TemporaryDeleteConfirmationAgreements";
                TemporaryDeleteConfirmationAgreementsPVM temporaryDeleteConfirmationAgreementsPVM = new TemporaryDeleteConfirmationAgreementsPVM
                {
                    ConfirmationAgreementId = ConfirmationAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteConfirmationAgreements(temporaryDeleteConfirmationAgreementsPVM);

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
        public IActionResult CompleteDeleteConfirmationAgreements(int ConfirmationAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            ConfirmationAgreementsVM confirmationAgreementsVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/GetConfirmationAgreementsWithConfirmationAgreementId";

                GetConfirmationAgreementsWithConfirmationAgreementIdPVM getConfirmationAgreementsWithConfirmationAgreementIdPVM = new GetConfirmationAgreementsWithConfirmationAgreementIdPVM()
                {
                    ConfirmationAgreementId = ConfirmationAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConfirmationAgreementsWithConfirmationAgreementId(getConfirmationAgreementsWithConfirmationAgreementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ConfirmationAgreementsVM>();


                            if (record != null)
                            {
                                confirmationAgreementsVM = record;

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


                serviceUrl = projectsApiUrl + "/api/ConfirmationAgreementsManagement/CompleteDeleteConfirmationAgreements";
                CompleteDeleteConfirmationAgreementsPVM completeDeleteConfirmationAgreementsPVM = new CompleteDeleteConfirmationAgreementsPVM
                {
                    ConfirmationAgreementId = ConfirmationAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteConfirmationAgreements(completeDeleteConfirmationAgreementsPVM);

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

                string confirmationAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConfirmationAgreementFiles\\" + domainSettings.DomainName + "\\" + confirmationAgreementsVM.ConfirmationAgreementId + "\\Image";

                if (!string.IsNullOrEmpty(confirmationAgreementsVM.ConfirmationAgreementFilePath))
                {
                    if (System.IO.File.Exists(confirmationAgreementFolder + "\\" + confirmationAgreementsVM.ConfirmationAgreementFilePath))
                    {
                        System.IO.File.Delete(confirmationAgreementFolder + "\\" + confirmationAgreementsVM.ConfirmationAgreementFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (confirmationAgreementsVM.ConfirmationAgreementFileExt.ToLower().Equals(".jpg") ||
                    //    confirmationAgreementsVM.ConfirmationAgreementFileExt.ToLower().Equals(".jpeg") ||
                    //    confirmationAgreementsVM.ConfirmationAgreementFileExt.ToLower().Equals(".png") ||
                    //    confirmationAgreementsVM.ConfirmationAgreementFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(confirmationAgreementFolder + "\\thumb_" + confirmationAgreementsVM.ConfirmationAgreementFilePath))
                    //    {
                    //        System.IO.File.Delete(confirmationAgreementFolder + "\\thumb_" + confirmationAgreementsVM.ConfirmationAgreementFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(confirmationAgreementFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(confirmationAgreementFolder + "\\..");
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

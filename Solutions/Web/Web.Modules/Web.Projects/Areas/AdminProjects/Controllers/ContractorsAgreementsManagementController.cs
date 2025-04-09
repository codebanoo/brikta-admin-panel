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
    public class ContractorsAgreementsManagementController : ExtraAdminController
    {
        public ContractorsAgreementsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست قرارداد پیمانکاران";
            if(ViewData["ConstructionProject"] == null)
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
        public IActionResult GetAllContractorsAgreementsList(
            string? ContractorsAgreementTitle = "",
            long? ConstructionProjectId = 0)
        {

            try
            {
                List<ContractorsAgreementsVM> contractorsAgreementsVMList = new List<ContractorsAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/GetAllContractorsAgreementsList";

                    GetAllContractorsAgreementsListPVM getAllContractorsAgreementsListPVM = new GetAllContractorsAgreementsListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                      
                        ConstructionProjectId = ConstructionProjectId,
                        ContractorsAgreementTitle = ContractorsAgreementTitle,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllContractorsAgreementsList(getAllContractorsAgreementsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                contractorsAgreementsVMList = jArray.ToObject<List<ContractorsAgreementsVM>>();


                                if (contractorsAgreementsVMList != null)
                                    if (contractorsAgreementsVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ContractorsAgreementsVM>>();

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
        public IActionResult GetListOfContractorsAgreements(
            int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string contractorsAgreementTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<ContractorsAgreementsVM> contractorsAgreementsVMList = new List<ContractorsAgreementsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/GetListOfContractorsAgreements";
                    GetListOfContractorsAgreementsPVM getListOfContractorsAgreementsPVM = new GetListOfContractorsAgreementsPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ContractorsAgreementTitle = contractorsAgreementTitle,
                        ConstructionProjectId = ConstructionProjectId,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfContractorsAgreements(getListOfContractorsAgreementsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                contractorsAgreementsVMList = jArray.ToObject<List<ContractorsAgreementsVM>>();


                                if (contractorsAgreementsVMList != null)
                                    if (contractorsAgreementsVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ContractorsAgreementsVM>>();

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

        public IActionResult AddToContractorsAgreements(long Id)
        {
            ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["ConstructionProjectId"] = Id;
            ViewData["Title"] = "آپلود قرارداد پیمانکاران";

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
        public async Task<ActionResult> AddToContractorsAgreements(AddToContractorsAgreementsPVM addToContractorsAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ContractorsAgreementsVM contractorsAgreementsVM = new ContractorsAgreementsVM();

            try
            {
                if (addToContractorsAgreementsPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string fileType = "";

                    string ext = Path.GetExtension(addToContractorsAgreementsPVM.File.FileName);
                    fileName = Guid.NewGuid().ToString() + ext;

                    if (ext.Equals(".jpeg") ||
                        ext.Equals(".jpg") ||
                        ext.Equals(".png") ||
                        ext.Equals(".gif") ||
                        ext.Equals(".bmp"))
                    {
                        fileType = "media";

                    }
                    else if (ext.Equals(".pdf"))
                    {
                        fileType = "pdf";
                    }
                    else
                    {
                        fileType = "other";
                    }

                    contractorsAgreementsVM = new ContractorsAgreementsVM()
                    {
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        ContractorsAgreementFileExt = ext,
                        ContractorsAgreementFilePath = fileName,
                        ConstructionProjectId = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ConstructionProjectId,
                        ContractorsAgreementDescription = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementDescription,
                        ContractorsAgreementNumber = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementNumber,
                        ContractorsAgreementFileOrder = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementFileOrder,
                        ContractorsAgreementFileType = fileType,
                        //ContractorsAgreementLink = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementLink,
                        ContractorsAgreementTitle = addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementTitle,

                    };

                    string serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/AddToContractorsAgreements";

                    AddToContractorsAgreementsPVM addToContractorsAgreementsPVM1 = new AddToContractorsAgreementsPVM()
                    {
                        ContractorsAgreementsVM = contractorsAgreementsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToContractorsAgreements(addToContractorsAgreementsPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                contractorsAgreementsVM.ContractorsAgreementId = jObject.ToObject<ContractorsAgreementsVM>().ContractorsAgreementId;
                            }
                        }
                    }

                    try
                    {
                        if (contractorsAgreementsVM.ContractorsAgreementId == 0)
                        {

                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                        //string contractorsAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ContractorsAgreementFiles\\" + domainSettings.DomainName + "\\" + contractorsAgreementsVM.ContractorsAgreementId + "\\Image";
                        string contractorsAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles\\" + domainSettings.DomainName +
                            "\\" + contractorsAgreementsVM.ConstructionProjectId + "\\ContractorsAgreementFiles\\" + contractorsAgreementsVM.ContractorsAgreementId + "\\Image";
                        if (!Directory.Exists(contractorsAgreementFolder))
                        {
                            Directory.CreateDirectory(contractorsAgreementFolder);
                        }
                        using (var fileStream = new FileStream(contractorsAgreementFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToContractorsAgreementsPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        //if (ext.Equals(".jpeg") ||
                        //    ext.Equals(".jpg") ||
                        //    ext.Equals(".png") ||
                        //    ext.Equals(".gif") ||
                        //    ext.Equals(".bmp"))
                        //{
                        //    ImageModify.ResizeImage(contractorsAgreementFolder + "\\",
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
        public IActionResult UpdateContractorsAgreements(ContractorsAgreementsVM contractorsAgreementsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                contractorsAgreementsVM.EditEnDate = DateTime.Now;
                contractorsAgreementsVM.EditTime = PersianDate.TimeNow;
                contractorsAgreementsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/UpdateContractorsAgreements";

                UpdateContractorsAgreementsPVM updateContractorsAgreementsPVM = new UpdateContractorsAgreementsPVM()
                {
                    ContractorsAgreementsVM = contractorsAgreementsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateContractorsAgreements(updateContractorsAgreementsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ContractorsAgreementsVM>();

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
        public IActionResult ToggleActivationContractorsAgreements(int ContractorsAgreementId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/ToggleActivationContractorsAgreements";

                ToggleActivationContractorsAgreementsPVM toggleActivationContractorsAgreementsPVM =
                    new ToggleActivationContractorsAgreementsPVM()
                    {
                        ContractorsAgreementId = ContractorsAgreementId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationContractorsAgreements(toggleActivationContractorsAgreementsPVM);

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
        public IActionResult TemporaryDeleteContractorsAgreements(int ContractorsAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/TemporaryDeleteContractorsAgreements";
                TemporaryDeleteContractorsAgreementsPVM temporaryDeleteContractorsAgreementsPVM = new TemporaryDeleteContractorsAgreementsPVM
                {
                    ContractorsAgreementId = ContractorsAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteContractorsAgreements(temporaryDeleteContractorsAgreementsPVM);

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
        public IActionResult CompleteDeleteContractorsAgreements(int ContractorsAgreementId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            ContractorsAgreementsVM contractorsAgreementsVM = null;
            try
            {
                string serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/GetContractorsAgreementsWithContractorsAgreementId";

                GetContractorsAgreementsWithContractorsAgreementIdPVM getContractorsAgreementsWithContractorsAgreementIdPVM = new GetContractorsAgreementsWithContractorsAgreementIdPVM()
                {
                    ContractorsAgreementId = ContractorsAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetContractorsAgreementsWithContractorsAgreementId(getContractorsAgreementsWithContractorsAgreementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ContractorsAgreementsVM>();


                            if (record != null)
                            {
                                contractorsAgreementsVM = record;

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


                serviceUrl = projectsApiUrl + "/api/ContractorsAgreementsManagement/CompleteDeleteContractorsAgreements";
                CompleteDeleteContractorsAgreementsPVM completeDeleteContractorsAgreementsPVM = new CompleteDeleteContractorsAgreementsPVM
                {
                    ContractorsAgreementId = ContractorsAgreementId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteContractorsAgreements(completeDeleteContractorsAgreementsPVM);

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

                string contractorsAgreementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ContractorsAgreementFiles\\" + domainSettings.DomainName + "\\" + contractorsAgreementsVM.ContractorsAgreementId + "\\Image";

                if (!string.IsNullOrEmpty(contractorsAgreementsVM.ContractorsAgreementFilePath))
                {
                    if (System.IO.File.Exists(contractorsAgreementFolder + "\\" + contractorsAgreementsVM.ContractorsAgreementFilePath))
                    {
                        System.IO.File.Delete(contractorsAgreementFolder + "\\" + contractorsAgreementsVM.ContractorsAgreementFilePath);
                        System.Threading.Thread.Sleep(100);
                    }

                    //if (contractorsAgreementsVM.ContractorsAgreementFileExt.ToLower().Equals(".jpg") ||
                    //    contractorsAgreementsVM.ContractorsAgreementFileExt.ToLower().Equals(".jpeg") ||
                    //    contractorsAgreementsVM.ContractorsAgreementFileExt.ToLower().Equals(".png") ||
                    //    contractorsAgreementsVM.ContractorsAgreementFileExt.ToLower().Equals(".bmp"))
                    //{
                    //    if (System.IO.File.Exists(contractorsAgreementFolder + "\\thumb_" + contractorsAgreementsVM.ContractorsAgreementFilePath))
                    //    {
                    //        System.IO.File.Delete(contractorsAgreementFolder + "\\thumb_" + contractorsAgreementsVM.ContractorsAgreementFilePath);
                    //        System.Threading.Thread.Sleep(100);
                    //    }
                    //}
                    Directory.Delete(contractorsAgreementFolder);
                    System.Threading.Thread.Sleep(100);
                    Directory.Delete(contractorsAgreementFolder + "\\..");
                    System.Threading.Thread.Sleep(100);

                }
            }

            catch (Exception exc)
            { }

            return Json(new { Result = "OK" });
        }


    }
}

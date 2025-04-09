using ApiCallers.ProjectsApiCaller;
using AutoMapper;
using CustomAttributes;
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
using System.Linq;
using System;
using VM.Projects;
using VM.PVM.Projects;
using Web.Core.Controllers;
using PVM.Projects.TeniacoSuggestions;
using VM.Projects.TeniacoSuggestions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using System.IO;
using VM.PVM.Teniaco;
using VM.Base;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class TeniacoSuggestionsManagementController : ExtraAdminController
    {
        public TeniacoSuggestionsManagementController(IHostEnvironment _hostEnvironment,
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
            // نام پروژه
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
                ViewData["DataBackUrl"] = "/AdminProjects/ConstructionProjectsManagement/Index";
            }

            ViewData["ConstructionProjectId"] = Id;
            ViewData["Title"] = "پیشنهادات تنیاکو";
            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfTeniacoSuggestionFilesWithConstructionProjectId(long ConstructionProjectId = 0)
        {

            try
            {
                List<TeniacoSuggestionFilesVM> teniacoSuggestionsVMList = new List<TeniacoSuggestionFilesVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/TeniacoSuggestionsManagement/GetListOfTeniacoSuggestionFilesWithConstructionProjectId";
                    GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM = new GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        ConstructionProjectId = ConstructionProjectId,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfTeniacoSuggestionFilesWithConstructionProjectId(getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                teniacoSuggestionsVMList = jArray.ToObject<List<TeniacoSuggestionFilesVM>>();


                                if (teniacoSuggestionsVMList != null)
                                    if (teniacoSuggestionsVMList.Count >= 0)
                                    {

                                        var records = jArray.ToObject<List<TeniacoSuggestionFilesVM>>();


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


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToTeniacoSuggestionFiles(List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM,string SuggestionPageTitle=null)
        {

            try
            {
                string projectFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TeniacoSuggestionFiles\\my.teniaco.com\\" + teniacoSuggestionFilesVM[0].ConstructionProjectId + "\\Media";

                if(!Directory.Exists(projectFolder))
                {
                    Directory.CreateDirectory(projectFolder);
                }
                
                string fileName = "";

                foreach(var suggestionFile in teniacoSuggestionFilesVM)
                {
                    if(suggestionFile.SuggestionFileId > 0)
                        continue;

                    string ext = Path.GetExtension(suggestionFile.File.FileName).ToLower();
                    fileName = Guid.NewGuid().ToString() + ext;
                    using (var fileStream = new FileStream(projectFolder + "\\" + fileName, FileMode.Create))
                    {
                        await suggestionFile.File.CopyToAsync(fileStream);
                        System.Threading.Thread.Sleep(100);
                    }

                    suggestionFile.SuggestionFilePath = fileName;
                    suggestionFile.SuggestionFileExt = ext;
                    suggestionFile.SuggestionFileTitle = suggestionFile.File.FileName;
                    suggestionFile.File = null;
                }


                serviceUrl = projectsApiUrl + "/api/TeniacoSuggestionsManagement/AddToTeniacoSuggestionFiles";
                AddToTeniacoSuggestionFilesPVM addToTeniacoSuggestionFilesPVM = new AddToTeniacoSuggestionFilesPVM
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    teniacoSuggestionFilesVM = teniacoSuggestionFilesVM,
                    UserId = this.userId.Value,
                    SuggestionPageTitle = SuggestionPageTitle

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToTeniacoSuggestionFiles(addToTeniacoSuggestionFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))

                        {
                            return Json(new
                            {
                                Result = "OK"
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


        [HttpPost]
        [AjaxOnly]
        public IActionResult DeleteTeniacoSuggestionFile(long SuggestionFileId = 0)
        {

            try
            {
                serviceUrl = projectsApiUrl + "/api/TeniacoSuggestionsManagement/DeleteTeniacoSuggestionFile";
                DeleteTeniacoSuggestionFilePVM deleteTeniacoSuggestionFilePVM = new DeleteTeniacoSuggestionFilePVM
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    SuggestionFileId = SuggestionFileId,
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).DeleteTeniacoSuggestionFile(deleteTeniacoSuggestionFilePVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {

                            return Json(new
                            {
                                Result = "OK"
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
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> EditTeniacoSuggestionFile(List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM)
        {

            try
            {
                string projectFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\TeniacoSuggestionFiles\\my.teniaco.com\\" + teniacoSuggestionFilesVM[0].ConstructionProjectId + "\\Media";

                if (!Directory.Exists(projectFolder))
                {
                    Directory.CreateDirectory(projectFolder);
                }

                string fileName = "";

                foreach (var suggestionFile in teniacoSuggestionFilesVM)
                {
                    if (suggestionFile.File is null)
                        continue;

                    string ext = Path.GetExtension(suggestionFile.File.FileName).ToLower();
                    fileName = Guid.NewGuid().ToString() + ext;
                    using (var fileStream = new FileStream(projectFolder + "\\" + fileName, FileMode.Create))
                    {
                        await suggestionFile.File.CopyToAsync(fileStream);
                        System.Threading.Thread.Sleep(100);
                    }

                    suggestionFile.SuggestionFilePath = fileName;
                    suggestionFile.SuggestionFileExt = ext;
                    suggestionFile.SuggestionFileTitle = suggestionFile.File.FileName;
                    suggestionFile.File = null;
                }


                serviceUrl = projectsApiUrl + "/api/TeniacoSuggestionsManagement/EditTeniacoSuggestionFile";
                EditTeniacoSuggestionFilePVM editTeniacoSuggestionFilePVM = new EditTeniacoSuggestionFilePVM
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    teniacoSuggestionFilesVM = teniacoSuggestionFilesVM
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).EditTeniacoSuggestionFile(editTeniacoSuggestionFilePVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK"
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


    }
}

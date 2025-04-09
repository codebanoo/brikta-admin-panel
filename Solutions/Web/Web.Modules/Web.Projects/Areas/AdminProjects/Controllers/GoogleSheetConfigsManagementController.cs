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
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Projects;
using VM.PVM.Projects;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class GoogleSheetConfigsManagementController : ExtraAdminController
    {
        public GoogleSheetConfigsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index()
        {

            ViewData["Title"] = "لیست کانفیگ های گوگل شیت";


            return View("Index");

        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllGoogleSheetConfigsList()
        {

            try
            {
                List<GoogleSheetConfigsVM> googleSheetConfigsVMList = new List<GoogleSheetConfigsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/GetAllGoogleSheetConfigsList";

                    GetAllGoogleSheetConfigsListPVM getAllGoogleSheetConfigsListPVM = new GetAllGoogleSheetConfigsListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                      
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllGoogleSheetConfigsList(getAllGoogleSheetConfigsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                googleSheetConfigsVMList = jArray.ToObject<List<GoogleSheetConfigsVM>>();


                                if (googleSheetConfigsVMList != null)
                                    if (googleSheetConfigsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<GoogleSheetConfigsVM>>();

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
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfGoogleSheetConfigs(
            //int ConstructionProjectId = 0,
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string googleSheetConfigTitle = "",
            string jtSorting = null)
        {

            try
            {
                List<GoogleSheetConfigsVM> googleSheetConfigsVMList = new List<GoogleSheetConfigsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/GetListOfGoogleSheetConfigs";
                    GetListOfGoogleSheetConfigsPVM getListOfGoogleSheetConfigsPVM = new GetListOfGoogleSheetConfigsPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        //ConstructionProjectId = ConstructionProjectId,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfGoogleSheetConfigs(getListOfGoogleSheetConfigsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                googleSheetConfigsVMList = jArray.ToObject<List<GoogleSheetConfigsVM>>();


                                if (googleSheetConfigsVMList != null)
                                    if (googleSheetConfigsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<GoogleSheetConfigsVM>>();

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
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        //public IActionResult AddToGoogleSheetConfigs(int Id)
        //{
        //    ViewData["DomainName"] = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
        //    ViewData["ConstructionProjectId"] = Id;
        //    ViewData["Title"] = "آپلود کانفیگ گوگل شیت";

        //    return View("AddTo");
        //}


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToGoogleSheetConfigs(GoogleSheetConfigsVM googleSheetConfigsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });


            try
            {
                if (googleSheetConfigsVM != null)
                {

                    string serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/AddToGoogleSheetConfigs";
                    googleSheetConfigsVM.CreateEnDate = DateTime.Now;
                    googleSheetConfigsVM.CreateTime = PersianDate.TimeNow;
                    googleSheetConfigsVM.UserIdCreator = this.userId.Value;
                    googleSheetConfigsVM.IsActivated = true;
                    googleSheetConfigsVM.IsDeleted = false;
                    AddToGoogleSheetConfigsPVM addToGoogleSheetConfigsPVM1 = new AddToGoogleSheetConfigsPVM()
                    {
                        GoogleSheetConfigsVM = googleSheetConfigsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToGoogleSheetConfigs(addToGoogleSheetConfigsPVM1);

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
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = googleSheetConfigsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
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
        public IActionResult UpdateGoogleSheetConfigs(GoogleSheetConfigsVM googleSheetConfigsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                googleSheetConfigsVM.EditEnDate = DateTime.Now;
                googleSheetConfigsVM.EditTime = PersianDate.TimeNow;
                googleSheetConfigsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/UpdateGoogleSheetConfigs";

                UpdateGoogleSheetConfigsPVM updateGoogleSheetConfigsPVM = new UpdateGoogleSheetConfigsPVM()
                {
                    GoogleSheetConfigsVM = googleSheetConfigsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateGoogleSheetConfigs(updateGoogleSheetConfigsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<GoogleSheetConfigsVM>();

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
        public IActionResult ToggleActivationGoogleSheetConfigs(int GoogleSheetConfigId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/ToggleActivationGoogleSheetConfigs";

                ToggleActivationGoogleSheetConfigsPVM toggleActivationGoogleSheetConfigsPVM =
                    new ToggleActivationGoogleSheetConfigsPVM()
                    {
                        GoogleSheetConfigId = GoogleSheetConfigId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationGoogleSheetConfigs(toggleActivationGoogleSheetConfigsPVM);

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
        public IActionResult TemporaryDeleteGoogleSheetConfigs(int GoogleSheetConfigId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/TemporaryDeleteGoogleSheetConfigs";
                TemporaryDeleteGoogleSheetConfigsPVM temporaryDeleteGoogleSheetConfigsPVM = new TemporaryDeleteGoogleSheetConfigsPVM
                {
                    GoogleSheetConfigId = GoogleSheetConfigId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteGoogleSheetConfigs(temporaryDeleteGoogleSheetConfigsPVM);

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
        public IActionResult CompleteDeleteGoogleSheetConfigs(long GoogleSheetConfigId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            try
            {
                string serviceUrl = projectsApiUrl + "/api/GoogleSheetConfigsManagement/CompleteDeleteGoogleSheetConfigs";

                CompleteDeleteGoogleSheetConfigsPVM completeDeleteGoogleSheetConfigsPVM = new CompleteDeleteGoogleSheetConfigsPVM()
                {
                    GoogleSheetConfigId = GoogleSheetConfigId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteGoogleSheetConfigs(completeDeleteGoogleSheetConfigsPVM);

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

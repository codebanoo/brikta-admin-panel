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
using VM.Base;
using VM.Projects;
using VM.PVM.Projects;
using Web.Core.Controllers;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class ConstructionProjectDelaysManagementController : ExtraAdminController
    {
        public ConstructionProjectDelaysManagementController(IHostEnvironment _hostEnvironment,
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


            ViewData["Title"] = "لایحه تاخیرات";

            //علل لایحه تاخیرات
            if (ViewData["ConstructionProjectBillDelaysList"] == null)
            {
                JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

                List<ConstructionProjectBillDelaysVM> constructionProjectBillDelaysVMList = new List<ConstructionProjectBillDelaysVM>();

                try
                {
                    string serviceUrl = projectsApiUrl + "/api/ConstructionProjectBillDelaysManagement/GetAllConstructionProjectBillDelaysList";

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectBillDelaysList(new GetAllConstructionProjectBillDelaysListPVM() { });

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<ConstructionProjectBillDelaysVM>>();

                                constructionProjectBillDelaysVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ConstructionProjectBillDelaysList"] = constructionProjectBillDelaysVMList;
            }

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
        public IActionResult GetAllConstructionProjectDelays(
            int? constructionProjectBillDelayId = 0,
            long constructionProjectId = 0)
        {

            try
            {
                List<ConstructionProjectDelaysVM> constructionProjectDelaysVMList = new List<ConstructionProjectDelaysVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/GetAllConstructionProjectDelays";

                    GetAllConstructionProjectDelaysListPVM getAllConstructionProjectDelaysListPVM = new GetAllConstructionProjectDelaysListPVM
                    {

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        ConstructionProjectBillDelayId = constructionProjectBillDelayId,
                        ConstructionProjectId = constructionProjectId,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectDelays(getAllConstructionProjectDelaysListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectDelaysVMList = jArray.ToObject<List<ConstructionProjectDelaysVM>>();


                                if (constructionProjectDelaysVMList != null)
                                    if (constructionProjectDelaysVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConstructionProjectDelaysVM>>();

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
                                                            record.UserCreatorName += "" + customUser.Name;

                                                        if (!string.IsNullOrEmpty(customUser.Family))
                                                            record.UserCreatorName += "" + customUser.Family;
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
        public IActionResult GetListOfConstructionProjectDelays(
            int jtStartIndex = 0,
            int jtPageSize = 10,
            long ConstructionProjectId = 0,
            int? constructionProjectBillDelayId = 0,
            string jtSorting = null)
        {

            try
            {
                List<ConstructionProjectDelaysVM> constructionProjectDelaysVMList = new List<ConstructionProjectDelaysVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/GetListOfConstructionProjectDelays";
                    GetListOfConstructionProjectDelaysPVM getListOfConstructionProjectDelaysPVM = new GetListOfConstructionProjectDelaysPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ConstructionProjectId = ConstructionProjectId,
                        ConstructionProjectBillDelayId = constructionProjectBillDelayId.Value,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfConstructionProjectDelays(getListOfConstructionProjectDelaysPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectDelaysVMList = jArray.ToObject<List<ConstructionProjectDelaysVM>>();


                                if (constructionProjectDelaysVMList != null)
                                    if (constructionProjectDelaysVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConstructionProjectDelaysVM>>();

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



        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToConstructionProjectDelays(ConstructionProjectDelaysVM constructionProjectDelaysVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectDelaysVM.CreateEnDate = DateTime.Now;
                constructionProjectDelaysVM.CreateTime = PersianDate.TimeNow;
                constructionProjectDelaysVM.UserIdCreator = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/AddToConstructionProjectDelays";

                AddToConstructionProjectDelaysPVM addToConstructionProjectDelaysPVM = new AddToConstructionProjectDelaysPVM()
                {
                    constructionProjectDelaysVM = constructionProjectDelaysVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToConstructionProjectDelays(addToConstructionProjectDelaysPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ConstructionProjectsVM>();

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



        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateConstructionProjectDelays(ConstructionProjectDelaysVM constructionProjectDelaysVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectDelaysVM.EditEnDate = DateTime.Now;
                constructionProjectDelaysVM.EditTime = PersianDate.TimeNow;
                constructionProjectDelaysVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/UpdateConstructionProjectDelays";

                UpdateConstructionProjectDelaysPVM updateConstructionProjectDelaysPVM = new UpdateConstructionProjectDelaysPVM()
                {
                    constructionProjectDelaysVM = constructionProjectDelaysVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateConstructionProjectDelays(updateConstructionProjectDelaysPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {

                        if (jsonResultWithRecordObjectVM.Result == "OK")
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
        public IActionResult ToggleActivationConstructionProjectDelays(long ConstructionProjectsDelayId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/ToggleActivationConstructionProjectDelays";

                ToggleActivationConstructionProjectDelaysPVM toggleActivationConstructionProjectDelaysPVM = new ToggleActivationConstructionProjectDelaysPVM()
                {
                    ConstructionProjectDelayId = ConstructionProjectsDelayId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationConstructionProjectDelays(toggleActivationConstructionProjectDelaysPVM);

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
        public IActionResult TemporaryDeleteConstructionProjectDelays(long ConstructionProjectsDelayId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/TemporaryDeleteConstructionProjectDelays";
                TemporaryDeleteConstructionProjectDelaysPVM temporaryDeleteConstructionProjectDelaysPVM = new TemporaryDeleteConstructionProjectDelaysPVM
                {
                    ConstructionProjectDelayId = ConstructionProjectsDelayId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteConstructionProjectDelays(temporaryDeleteConstructionProjectDelaysPVM);

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
        public IActionResult CompleteDeleteConstructionProjectDelays(long ConstructionProjectsDelayId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/CompleteDeleteConstructionProjectDelays";

                CompleteDeleteConstructionProjectDelaysPVM completeDeleteConstructionProjectDelaysPVM = new CompleteDeleteConstructionProjectDelaysPVM()
                {
                    ConstructionProjectDelayId = ConstructionProjectsDelayId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteConstructionProjectDelays(completeDeleteConstructionProjectDelaysPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
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
        public IActionResult GetConstructionProjectDelayById(long ConstructionProjectsDelayId)
        {

            try
            {
                ConstructionProjectDelaysVM constructionProjectDelay = new ConstructionProjectDelaysVM();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectDelaysManagement/GetConstructionProjectDelayById";
                    GetConstructionProjectDelayByIdPVM getConstructionProjectDelayByIdPVM = new GetConstructionProjectDelayByIdPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        ConstructionProjectsDelayId = ConstructionProjectsDelayId
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConstructionProjectDelayById(getConstructionProjectDelayByIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JObject Jobject = jsonResultWithRecordObjectVM.Record;
                                constructionProjectDelay = Jobject.ToObject<ConstructionProjectDelaysVM>();


                                if (constructionProjectDelay != null)
                                    return Json(new
                                    {
                                        Result = jsonResultWithRecordObjectVM.Result,
                                        Record = constructionProjectDelay
                                    });

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

    }
}

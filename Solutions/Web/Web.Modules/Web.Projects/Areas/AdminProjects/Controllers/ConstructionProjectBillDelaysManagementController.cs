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
    public class ConstructionProjectBillDelaysManagementController : ExtraAdminController
    {
        public ConstructionProjectBillDelaysManagementController(IHostEnvironment _hostEnvironment,
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


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllConstructionProjectBillDelays()
        {

            try
            {
                List<ConstructionProjectBillDelaysVM> constructionProjectBillDelaysVMList = new List<ConstructionProjectBillDelaysVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectBillDelaysManagement/GetListOfConstructionProjectBillDelays";
                    GetAllConstructionProjectBillDelaysListPVM getListOfConstructionProjectBillDelaysPVM = new GetAllConstructionProjectBillDelaysListPVM
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminProjects", "ConstructionProjectBillDelays", "GetAllConstructionProjectBillDelays", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds)
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectBillDelaysList(getListOfConstructionProjectBillDelaysPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectBillDelaysVMList = jArray.ToObject<List<ConstructionProjectBillDelaysVM>>();


                                if (constructionProjectBillDelaysVMList != null)
                                    if (constructionProjectBillDelaysVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConstructionProjectBillDelaysVM>>();

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
        public IActionResult AddToConstructionProjectBillDelays(ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectBillDelaysVM.CreateEnDate = DateTime.Now;
                constructionProjectBillDelaysVM.CreateTime = PersianDate.TimeNow;
                constructionProjectBillDelaysVM.UserIdCreator = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectBillDelaysManagement/AddToConstructionProjectBillDelays";

                AddToConstructionProjectBillDelaysPVM addToConstructionProjectBillDelaysPVM = new AddToConstructionProjectBillDelaysPVM()
                {
                    constructionProjectBillDelaysVM = constructionProjectBillDelaysVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToConstructionProjectBillDelays(addToConstructionProjectBillDelaysPVM);


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
                                Id = jsonResultWithRecordObjectVM.Record,
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
        public IActionResult UpdateConstructionProjectBillDelays(ConstructionProjectBillDelaysVM constructionProjectBillDelaysVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectBillDelaysVM.EditEnDate = DateTime.Now;
                constructionProjectBillDelaysVM.EditTime = PersianDate.TimeNow;
                constructionProjectBillDelaysVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectBillDelaysManagement/UpdateConstructionProjectBillDelays";

                UpdateConstructionProjectBillDelaysPVM updateConstructionProjectBillDelaysPVM = new UpdateConstructionProjectBillDelaysPVM()
                {
                    constructionProjectBillDelaysVM = constructionProjectBillDelaysVM,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateConstructionProjectBillDelays(updateConstructionProjectBillDelaysPVM);


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

    }
}

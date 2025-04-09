using ApiCallers.TeniacoApiCaller;
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
using System;
using System.Collections.Generic;
using System.Linq;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class EvaluationSubjectsManagementController : ExtraAdminController
    {
        public EvaluationSubjectsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست موضوعات ارزیابی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }



        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllEvaluationSubjectsList(
          int jtStartIndex = 0,
          int jtPageSize = 10,
          string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationSubjectsManagement/GetAllEvaluationSubjectsList";

                GetAllEvaluationSubjectsListPVM getAllEvaluationSubjectsListPVM = new GetAllEvaluationSubjectsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationSubjectsList(getAllEvaluationSubjectsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationSubjectsVM>>();

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
                                jsonResultWithRecordsObjectVM.Result,
                                jsonResultWithRecordsObjectVM.Records,
                                jsonResultWithRecordsObjectVM.TotalRecordCount
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
    }
}

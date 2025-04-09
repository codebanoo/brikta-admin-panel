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
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class EvaluationChartManagementController : ExtraAdminController
    {
        public EvaluationChartManagementController(IHostEnvironment _hostEnvironment,
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



        public IActionResult Index(long Id = 0, string parentType = "")
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "EvaluationsManagement");


            ViewData["Title"] = "چارت ارزیابی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            //EvaluationList
            if (ViewData["EvaluationsList"] == null)
            {
                List<EvaluationsVM> evaluationsVMList = new List<EvaluationsVM>();

                GetAllEvaluationsListPVM getAllEvaluationsListPVM = new GetAllEvaluationsListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetAllEvaluationsList";

                    if (parentType == "Properties")
                    {
                        getAllEvaluationsListPVM.EvaluationSubjectId = 2;
                    }
                    else if (parentType == "Projects")
                    {
                        getAllEvaluationsListPVM.EvaluationSubjectId = 3;
                    }
                    else if (parentType == "Investors")
                    {

                        getAllEvaluationsListPVM.EvaluationSubjectId = 4;

                    }
                    else if (parentType == "Operation")
                    {
                        getAllEvaluationsListPVM.EvaluationSubjectId = 5;
                    }


                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationsList(getAllEvaluationsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                evaluationsVMList = jArray.ToObject<List<EvaluationsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["EvaluationsList"] = evaluationsVMList;
            }

            ViewData["ParentId"] = Id;

            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllEvalCategoriesList(int evaluationId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllEvaluationCategoriesList";

                GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM = new GetAllEvaluationCategoriesListPVM()
                {
                    EvaluationId = evaluationId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationCategoriesList(getAllEvaluationCategoriesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
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
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }



        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllDivisionOfEvaluationsListByParentId(int? parentId, int? evaluationId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllDivisionOfEvaluationsListByParentId";


                GetAllDivisionOfEvaluationsListByParentIdPVM getAllDivisionOfEvaluationsListByParentIdPVM =
                     new GetAllDivisionOfEvaluationsListByParentIdPVM
                     {
                         ParentId = parentId,
                         ParentType = "Investors",
                         EvaluationId = evaluationId,
                         ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                     };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllDivisionOfEvaluationsListByParentId(getAllDivisionOfEvaluationsListByParentIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JObject jobject = jsonResultWithRecordsObjectVM.Records;
                            var records = jobject.ToObject<AnswerSheetEvaluationVM>();

                            return Json(new { Result = "OK", records = records });



                        }
                    }
                }
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = "خطا"
                });
            }

            return Json(new { Result = "Err" });
        }



    }
}

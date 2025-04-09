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
    public class EvaluationTreeManagementController : ExtraAdminController
    {
        public EvaluationTreeManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("Index", "EvaluationsManagement");


            ViewData["Title"] = "نمایش درختی گروه های ارزیابی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["Evaluation"] == null)
            {
                EvaluationsVM evaluationsVM = new EvaluationsVM();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetEvaluationsWithEvaluationId";

                    GetEvaluationsWithEvaluationIdPVM getEvaluationsWithEvaluationIdPVM = new GetEvaluationsWithEvaluationIdPVM()
                    {
                        EvaluationId = Id,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        // this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetEvaluationsWithEvaluationId(getEvaluationsWithEvaluationIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<EvaluationsVM>();


                                if (record != null)
                                {
                                    evaluationsVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["Evaluation"] = evaluationsVM;


            }

            if (ViewData["EvaluationCategoriesList"] == null)
            {
                List<EvaluationCategoriesVM> evaluationCategoriesVMList = new List<EvaluationCategoriesVM>();


                //EvaluationCategoriesVM evaluationCategoriesVM = new EvaluationCategoriesVM();

                //var answerSheetEvaluationVMList = new AnswerSheetEvaluationVM();


                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllExclusivelyEvaluationCategoriesList";

                    GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM = new GetAllEvaluationCategoriesListPVM()
                    {
                        EvaluationId = Id,
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
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                evaluationCategoriesVMList = jArray.ToObject<List<EvaluationCategoriesVM>>();

                                //JObject jobject = jsonResultWithRecordsObjectVM.Records;
                                //evaluationCategoriesVM = jobject.ToObject<EvaluationCategoriesVM>();


                                //JObject jObject = jsonResultWithRecordsObjectVM.Records;
                                //evaluationCategoriesVM = jObject.ToObject<EvaluationCategoriesVM>();


                                //JObject jObject = jsonResultWithRecordsObjectVM.Records;
                                //answerSheetEvaluationVMList = jObject.ToObject<AnswerSheetEvaluationVM>();
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["EvaluationCategoriesList"] = evaluationCategoriesVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/EvaluationsManagement/Index/";
            }

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllEvaluationCategoriesList(int evaluationId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllEvaluationCategoriesList";

                GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM = new GetAllEvaluationCategoriesListPVM()
                {
                    EvaluationId = evaluationId,
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

                            ViewData["EvaluationCategoriesList"] = jsonResultWithRecordsObjectVM.Records;
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


    }
}

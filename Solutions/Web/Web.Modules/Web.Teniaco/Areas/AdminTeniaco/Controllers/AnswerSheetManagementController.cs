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
using System.Text.Json.Nodes;
using System.Xml.Linq;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class AnswerSheetManagementController : ExtraAdminController
    {
        public AnswerSheetManagementController(IHostEnvironment _hostEnvironment,
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



        public IActionResult Index(int Id = 0, string parentType = "")
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "InvestorsManagement");


            ViewData["Title"] = "فرم پاسخنامه";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            //EvaluationList
            if (ViewData["EvaluationsList"] == null)
            {
                List<EvaluationsVM> evaluationsVMList = new List<EvaluationsVM>();

                GetAllEvaluationsListPVM getAllEvaluationsListPVM = new GetAllEvaluationsListPVM()
                {
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
                    else if (parentType == "HumanResources")
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


        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllEvaluationCategoriesList(int EvaluationId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllEvaluationCategoriesList";


                GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM =
                    new GetAllEvaluationCategoriesListPVM()
                    {

                        EvaluationId = EvaluationId,
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
                            JObject jobject = jsonResultWithRecordsObjectVM.Records;

                            //JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            //var records = jArray.ToObject<List<AnswerSheetEvaluationVM>>();
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



        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateEvaluationItemValuesList(List<EvaluationItemValuesVM> evaluationItemValues)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                foreach (var evalItems in evaluationItemValues)
                {
                    evalItems.CreateEnDate = DateTime.Now;
                    evalItems.CreateTime = PersianDate.TimeNow;
                    evalItems.UserIdCreator = this.userId.Value;
                    evalItems.IsActivated = true;
                    evalItems.IsDeleted = false;
                }

                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemValuesManagement/UpdateEvaluationItemValuesList";

                UpdateEvaluationItemValuesListPVM updateEvaluationItemValuesListPVM = new UpdateEvaluationItemValuesListPVM()
                {
                    EvaluationItemValuesVM = evaluationItemValues,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateEvaluationItemValuesList(updateEvaluationItemValuesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "آپلود انجام شد",
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

    }
}

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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VM.Base;
using VM.Console;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class EvaluationOfHumanResourcesManagementController : ExtraAdminController
    {
        public EvaluationOfHumanResourcesManagementController(IHostEnvironment _hostEnvironment,
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


        #region EvaluationOfHumanResources
        public IActionResult Index()
        {
            ViewData["Title"] = "فرم ارزیابی نیروی انسانی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (ViewData["EvaluationsList"] == null)
            {
                List<EvaluationsVM> evaluationsVMList = new List<EvaluationsVM>();

                GetAllEvaluationsListPVM getAllEvaluationsListPVM = new GetAllEvaluationsListPVM();
                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetAllEvaluationsList";

                    getAllEvaluationsListPVM.EvaluationSubjectId = 5;

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

            if (ViewData["CustomUsersList"] == null)
            {
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();

                //var ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //        this.domainsSettings.DomainSettingId);

                var ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);
                try
                {
                    customUsersVMList = consoleBusiness.GetUsersWithUserIds(ChildsUsersIds);

                }
                catch (Exception exc)
                { }

                ViewData["CustomUsersList"] = customUsersVMList;
            }

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
                         ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                         ParentId = parentId,
                         ParentType = "HumanResources",
                         EvaluationId = evaluationId
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

                UpdateEvaluationItemValuesListPVM  updateEvaluationItemValuesListPVM = new UpdateEvaluationItemValuesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    EvaluationItemValuesVM = evaluationItemValues
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


        #endregion

        #region EvaluationChartOfHumanResources


        public IActionResult EvaluationChartOfHumaneResourcesManagement()
        {
            ViewData["Title"] = "چارت ارزیابی نیروی انسانی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


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

                    getAllEvaluationsListPVM.EvaluationSubjectId = 5;

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

            if (ViewData["CustomUsersList"] == null)
            {
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();

                //var ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //        this.domainsSettings.DomainSettingId);

                var ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                          this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);
                try
                {
                    customUsersVMList = consoleBusiness.GetUsersWithUserIds(ChildsUsersIds);

                }
                catch (Exception exc)
                { }

                ViewData["CustomUsersList"] = customUsersVMList;
            }



            return View("Index");
        }

        #endregion
    }
}

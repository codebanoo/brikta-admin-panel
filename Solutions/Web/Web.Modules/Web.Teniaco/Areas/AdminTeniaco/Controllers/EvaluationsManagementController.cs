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
using System.Linq;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class EvaluationsManagementController : ExtraAdminController
    {
        public EvaluationsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست ارزیابی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["EvaluationSubjectsList"] == null)
            {
                List<EvaluationSubjectsVM> evaluationSubjectsVMList = new List<EvaluationSubjectsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationSubjectsManagement/GetAllEvaluationSubjectsList";
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
                                evaluationSubjectsVMList = jArray.ToObject<List<EvaluationSubjectsVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["EvaluationSubjectsList"] = evaluationSubjectsVMList;
            }

            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllEvaluationsList(
             int? EvaluationSubjectId,
             string? EvaluationTitle)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetAllEvaluationsList";

                GetAllEvaluationsListPVM getAllEvaluationsListPVM = new GetAllEvaluationsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    EvaluationSubjectId = EvaluationSubjectId,
                    EvaluationTitle = EvaluationTitle,
                };

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
                            var records = jArray.ToObject<List<EvaluationsVM>>();

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


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfEvaluations(
            int jtStartIndex = 0,
            int jtPageSize = 10,
            string evaluationTitle = "",
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetListOfEvaluations";

                GetListOfEvaluationsPVM getListOfEvaluationsPVM = new GetListOfEvaluationsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    EvaluationTitle = (!string.IsNullOrEmpty(evaluationTitle) ? evaluationTitle.Trim() : ""),
                    jtSorting = jtSorting
                };
                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfEvaluations(getListOfEvaluationsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationsVM>>();

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



        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToEvaluations(EvaluationsVM evaluationsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            AddToEvaluationsPVM addToEvaluationsPVM = new AddToEvaluationsPVM();
            try
            {
                addToEvaluationsPVM.EvaluationsVM = evaluationsVM;
                addToEvaluationsPVM.EvaluationsVM.CreateEnDate = DateTime.Now;
                addToEvaluationsPVM.EvaluationsVM.CreateTime = PersianDate.TimeNow;
                addToEvaluationsPVM.EvaluationsVM.UserIdCreator = this.userId.Value;
                addToEvaluationsPVM.EvaluationsVM.IsActivated = true;
                addToEvaluationsPVM.EvaluationsVM.IsDeleted = false;

                addToEvaluationsPVM.EvaluationsVM.PlanDate = PersianDate.ToGregorianDate(FrameWork.PersianType.ToEnglishDigits(evaluationsVM.PPlanDate));

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/AddToEvaluations";

                    // AddToEvaluationCategoriesPVM addToEvaluationCategoriesPVM = new AddToEvaluationCategoriesPVM() { EvaluationCategoriesVM = evaluationCategoriesVM };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToEvaluations(addToEvaluationsPVM);


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
                                    #region Fill UserCreatorName

                                    if (record.UserIdCreator.HasValue)
                                    {
                                        var customUser = consoleBusiness.GetCustomUser(record.UserIdCreator.Value);

                                        if (customUser != null)
                                        {
                                            record.UserCreatorName = customUser.UserName;

                                            if (!string.IsNullOrEmpty(customUser.Name))
                                                record.UserCreatorName += " " + customUser.Name;

                                            if (!string.IsNullOrEmpty(customUser.Family))
                                                record.UserCreatorName += " " + customUser.Family;
                                        }
                                    }

                                    #endregion

                                    //addToEvaluationsPVM.EvaluationsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = record,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateEvaluationCategory"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
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
        public IActionResult UpdateEvaluations(EvaluationsVM evaluationsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                evaluationsVM.EditEnDate = DateTime.Now;
                evaluationsVM.EditTime = PersianDate.TimeNow;
                evaluationsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/UpdateEvaluations";

                    UpdateEvaluationsPVM updateEvaluationsPVM = new UpdateEvaluationsPVM()
                    {
                        EvaluationsVM = evaluationsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateEvaluations(updateEvaluationsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            evaluationsVM = jObject.ToObject<EvaluationsVM>();

                            #region Fill UserCreatorName

                            if (evaluationsVM.UserIdCreator.HasValue)
                            {
                                var customUser = consoleBusiness.GetCustomUser(evaluationsVM.UserIdCreator.Value);

                                if (customUser != null)
                                {
                                    evaluationsVM.UserCreatorName = customUser.UserName;

                                    if (!string.IsNullOrEmpty(customUser.Name))
                                        evaluationsVM.UserCreatorName += " " + customUser.Name;

                                    if (!string.IsNullOrEmpty(customUser.Family))
                                        evaluationsVM.UserCreatorName += " " + customUser.Family;
                                }
                            }

                            #endregion

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = evaluationsVM,
                            });
                        }
                        else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateEvaluationCategory"))
                        {
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "رکورد تکراری است"
                            });
                        }
                    }

                }
                return Json(new
                {
                    Result = jsonResultWithRecordObjectVM.Result,
                    Record = evaluationsVM,
                });
            }
            catch (Exception)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }




        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationEvaluations(int evaluationId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/ToggleActivationEvaluations";

                ToggleActivationEvaluationsPVM toggleActivationEvaluationsPVM =
                    new ToggleActivationEvaluationsPVM()
                    {
                        EvaluationId = evaluationId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationEvaluations(toggleActivationEvaluationsPVM);

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
        public IActionResult TemporaryDeleteEvaluations(int evaluationId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/TemporaryDeleteEvaluations";

                TemporaryDeleteEvaluationsPVM temporaryDeleteEvaluationsPVM =
                    new TemporaryDeleteEvaluationsPVM()
                    {
                        EvaluationId = evaluationId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteEvaluations(temporaryDeleteEvaluationsPVM);

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
        public IActionResult CompleteDeleteEvaluations(int evaluationId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/CompleteDeleteEvaluations";

                CompleteDeleteEvaluationsPVM completeDeleteEvaluationsPVM =
                    new CompleteDeleteEvaluationsPVM()
                    {
                        EvaluationId = evaluationId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteEvaluations(completeDeleteEvaluationsPVM);

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

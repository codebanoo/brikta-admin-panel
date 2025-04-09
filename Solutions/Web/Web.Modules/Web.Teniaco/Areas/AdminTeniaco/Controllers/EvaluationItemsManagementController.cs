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
    public class EvaluationItemsManagementController : ExtraAdminController
    {
        public EvaluationItemsManagementController(IHostEnvironment _hostEnvironment,
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


        public IActionResult Index(int id = 0)
        {
            if (id.Equals(0))
            {
                return RedirectToAction("Index", "EvaluationCategoriesManagement");
            }
            ViewData["Title"] = "لیست پاسخها ";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });


            EvaluationQuestionsVM evaluationQuestionsVM = new EvaluationQuestionsVM();

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationQuestionsManagement/GetEvaluationQuestionWithEvaluationQuestionId";

                GetEvaluationQuestionWithEvaluationQuestionIdPVM getEvaluationQuestionWithEvaluationQuestionIdPVM = new GetEvaluationQuestionWithEvaluationQuestionIdPVM()
                {
                    EvaluationQuestionId = id,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetEvaluationQuestionWithEvaluationQuestionId(getEvaluationQuestionWithEvaluationQuestionIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<EvaluationQuestionsVM>();


                            if (record != null)
                            {
                                evaluationQuestionsVM = record;

                                #region Fill UserCreatorName

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["EvaluationQuestionsVM"] = evaluationQuestionsVM;

            //if (ViewData["DataBackUrl"] == null)
            //{
            //    ViewData["DataBackUrl"] = "/AdminTeniaco/EvaluationQuestionsManagement/Index/" + evaluationQuestionsVM.EvaluationCategoryId.Value;
            //}

            //ViewData["SearchTitle"] = "OK";

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllEvaluationItemsList(
             int? evaluationQuestionId = null,
             string? evaluationAnswer = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/GetAllEvaluationItemsList";

                GetAllEvaluationItemsListPVM getAllEvaluationItemsListPVM = new GetAllEvaluationItemsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    EvaluationQuestionId = evaluationQuestionId.Value,
                    EvaluationAnswerSearch = evaluationAnswer,
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationItemsList(getAllEvaluationItemsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationItemsVM>>();

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
        public IActionResult GetListOfEvaluationItems(
            int jtStartIndex = 0,
            int jtPageSize = 10,
            int evaluationQuestionId = 0,
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/GetListOfEvaluationItems";

                GetListOfEvaluationItemsPVM getListOfEvaluationItemsPVM = new GetListOfEvaluationItemsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    EvaluationQuestionId = evaluationQuestionId,
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfEvaluationItems(getListOfEvaluationItemsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationItemsVM>>();

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
        public IActionResult AddToEvaluationItems(EvaluationItemsVM evaluationItemsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                evaluationItemsVM.CreateEnDate = DateTime.Now;
                evaluationItemsVM.CreateTime = PersianDate.TimeNow;
                evaluationItemsVM.UserIdCreator = this.userId.Value;
                evaluationItemsVM.IsActivated = true;
                evaluationItemsVM.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/AddToEvaluationItems";

                    AddToEvaluationItemsPVM addToEvaluationItemsPVM = new AddToEvaluationItemsPVM()
                    {
                        EvaluationItemsVM = evaluationItemsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToEvaluationItems(addToEvaluationItemsPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<EvaluationItemsVM>();

                                if (record != null)
                                {
                                    evaluationItemsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = evaluationItemsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateEvaluationItem"))
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
        public IActionResult UpdateEvaluationItems(EvaluationItemsVM evaluationItemsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                evaluationItemsVM.EditEnDate = DateTime.Now;
                evaluationItemsVM.EditTime = PersianDate.TimeNow;
                evaluationItemsVM.UserIdEditor = this.userId.Value;



                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/UpdateEvaluationItems";

                    UpdateEvaluationItemsPVM updateEvaluationItemsPVM = new UpdateEvaluationItemsPVM()
                    {
                        EvaluationItemsVM = evaluationItemsVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateEvaluationItems(updateEvaluationItemsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            evaluationItemsVM = jObject.ToObject<EvaluationItemsVM>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = evaluationItemsVM,
                            });
                        }
                        else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateEvaluationItem"))
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
                    Record = evaluationItemsVM,
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
        public IActionResult ToggleActivationEvaluationItems(int evaluationItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/ToggleActivationEvaluationItems";

                ToggleActivationEvaluationItemsPVM toggleActivationEvaluationItemsPVM =
                    new ToggleActivationEvaluationItemsPVM()
                    {
                        EvaluationItemId = evaluationItemId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationEvaluationItems(toggleActivationEvaluationItemsPVM);

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
        public IActionResult TemporaryDeleteEvaluationItems(int evaluationItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/TemporaryDeleteEvaluationItems";

                TemporaryDeleteEvaluationItemsPVM temporaryDeleteEvaluationItemsPVM =
                    new TemporaryDeleteEvaluationItemsPVM()
                    {
                        EvaluationItemId = evaluationItemId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteEvaluationItems(temporaryDeleteEvaluationItemsPVM);

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
        public IActionResult CompleteDeleteEvaluationItems(int evaluationItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/CompleteDeleteEvaluationItems";

                CompleteDeleteEvaluationItemsPVM completeDeleteEvaluationItemsPVM =
                  new CompleteDeleteEvaluationItemsPVM()
                  {
                      EvaluationItemId = evaluationItemId,
                      //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                      //  this.domainsSettings.DomainSettingId),
                      ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                      UserId = this.userId.Value
                  };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteEvaluationItems(completeDeleteEvaluationItemsPVM);

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

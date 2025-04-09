using ApiCallers.PublicApiCaller;
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
using VM.Public;
using VM.PVM.Public;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class ConstructionSubItemsManagementController : ExtraAdminController
    {
        public ConstructionSubItemsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(long Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "ConstructionItemsManagement");

            ViewData["Title"] = "لیست زیر آیتمهای برآورد هزینه ساخت";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["UnitsOfMeasurementList"] == null)
            {
                List<UnitsOfMeasurementVM> unitsOfMeasurementVMList = new List<UnitsOfMeasurementVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/UnitsOfMeasurementManagement/GetAllUnitsOfMeasurementList";

                    GetAllUnitsOfMeasurementListPVM getAllUnitsOfMeasurementListPVM = new GetAllUnitsOfMeasurementListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllUnitsOfMeasurementList(getAllUnitsOfMeasurementListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                unitsOfMeasurementVMList = jArray.ToObject<List<UnitsOfMeasurementVM>>();


                                if (unitsOfMeasurementVMList != null)
                                    if (unitsOfMeasurementVMList.Count > 0)
                                    {

                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["UnitsOfMeasurementList"] = unitsOfMeasurementVMList;
            }

            if (ViewData["ConstructionItem"] == null)
            {
                ConstructionItemsVM constructionItemsVM = new ConstructionItemsVM();

                try
                {
                    serviceUrl = publicApiUrl + "/api/ConstructionItemsManagement/GetConstructionItemWithConstructionItemId";

                    GetConstructionItemWithConstructionItemIdPVM getConstructionItemWithConstructionItemIdPVM = new GetConstructionItemWithConstructionItemIdPVM()
                    {
                        ConstructionItemId = Id,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetConstructionItemWithConstructionItemId(getConstructionItemWithConstructionItemIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<ConstructionItemsVM>();

                                constructionItemsVM = record;

                                if (record != null)
                                {
                                    //if (constructionItemsVM.UserIdCreator.HasValue)
                                    //{
                                    //    var customUser = consoleBusiness.GetCustomUser(constructionItemsVM.UserIdCreator.Value);

                                    //    if (customUser != null)
                                    //    {
                                    //        constructionItemsVM.UserCreatorName = customUser.UserName;

                                    //        if (!string.IsNullOrEmpty(customUser.Name))
                                    //            constructionItemsVM.UserCreatorName += " " + customUser.Name;

                                    //        if (!string.IsNullOrEmpty(customUser.Family))
                                    //            constructionItemsVM.UserCreatorName += " " + customUser.Family;
                                    //    }
                                    //}
                                }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ConstructionItem"] = constructionItemsVM;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminPublic/ConstructionItemsManagement/Index/";
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllConstructionSubItemsList(
            long? ConstructionItemId,
            string ConstructionSubItemTitle)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/GetAllConstructionSubItemsList";

                GetAllConstructionSubItemsListPVM getAllConstructionSubItemsListPVM = new GetAllConstructionSubItemsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    ConstructionItemId  = ConstructionItemId,
                    ConstructionSubItemTitle = ConstructionSubItemTitle,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetAllConstructionSubItemsList(getAllConstructionSubItemsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ConstructionSubItemsVM>>();

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
        public IActionResult GetListOfConstructionSubItems(int jtStartIndex = 0,
            int jtPageSize = 10,
            long? constructionItemId = null,
            string constructionSubItemTitle = null,
            string jtSorting = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/GetListOfConstructionSubItems";

                GetListOfConstructionSubItemsPVM getListOfConstructionSubItemsPVM = new GetListOfConstructionSubItemsPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    ConstructionItemId = ((constructionItemId.HasValue) ? constructionItemId.Value : (long?)0),
                    ConstructionSubItemTitle = (!string.IsNullOrEmpty(constructionSubItemTitle) ? constructionSubItemTitle.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfConstructionSubItems(getListOfConstructionSubItemsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ConstructionSubItemsVM>>();

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
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToConstructionSubItems(ConstructionSubItemsVM constructionItemsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionItemsVM.CreateEnDate = DateTime.Now;
                constructionItemsVM.CreateTime = PersianDate.TimeNow;
                constructionItemsVM.UserIdCreator = this.userId.Value;

                //constructionItemsVM.ConstructionSubItemsType = "text";

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/AddToConstructionSubItems";

                    AddToConstructionSubItemsPVM addToFormPVM = new AddToConstructionSubItemsPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        ConstructionSubItemsVM = constructionItemsVM
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).AddToConstructionSubItems(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<ConstructionSubItemsVM>();

                                if (record != null)
                                {
                                    constructionItemsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = constructionItemsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateConstructionSubItem"))
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
        public IActionResult UpdateConstructionSubItems(ConstructionSubItemsVM constructionItemsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionItemsVM.EditEnDate = DateTime.Now;
                constructionItemsVM.EditTime = PersianDate.TimeNow;
                constructionItemsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/UpdateConstructionSubItems";

                    UpdateConstructionSubItemsPVM updateFormPVM = new UpdateConstructionSubItemsPVM()
                    {
                        ConstructionSubItemsVM = constructionItemsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).UpdateConstructionSubItems(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                constructionItemsVM = jObject.ToObject<ConstructionSubItemsVM>();

                                #region Fill UserCreatorName

                                //if (constructionItemsVM.UserIdCreator.HasValue)
                                //{
                                //    var customUser = consoleBusiness.GetCustomUser(constructionItemsVM.UserIdCreator.Value);

                                //    if (customUser != null)
                                //    {
                                //        constructionItemsVM.UserCreatorName = customUser.UserName;

                                //        if (!string.IsNullOrEmpty(customUser.Name))
                                //            constructionItemsVM.UserCreatorName += " " + customUser.Name;

                                //        if (!string.IsNullOrEmpty(customUser.Family))
                                //            constructionItemsVM.UserCreatorName += " " + customUser.Family;
                                //    }
                                //}

                                #endregion
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateConstructionSubItem"))
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
                        Record = constructionItemsVM,
                    });
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
        public IActionResult ToggleActivationConstructionSubItems(int ConstructionSubItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/ToggleActivationConstructionSubItems";

                ToggleActivationConstructionSubItemsPVM toggleActivationConstructionSubItemsPVM =
                new ToggleActivationConstructionSubItemsPVM()
                {
                    ConstructionSubItemId = ConstructionSubItemId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationConstructionSubItems(toggleActivationConstructionSubItemsPVM);

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
        public IActionResult TemporaryDeleteConstructionSubItems(int ConstructionSubItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/TemporaryDeleteConstructionSubItems";

                TemporaryDeleteConstructionSubItemsPVM toggleActivationConstructionSubItemsPVM =
                new TemporaryDeleteConstructionSubItemsPVM()
                {
                    ConstructionSubItemId = ConstructionSubItemId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).TemporaryDeleteConstructionSubItems(toggleActivationConstructionSubItemsPVM);

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
        public IActionResult CompleteDeleteConstructionSubItems(int ConstructionSubItemId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/CompleteDeleteConstructionSubItems";

                CompleteDeleteConstructionSubItemsPVM deleteConstructionSubItemsPVM =
                new CompleteDeleteConstructionSubItemsPVM()
                {
                    ConstructionSubItemId = ConstructionSubItemId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeleteConstructionSubItems(deleteConstructionSubItemsPVM);

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

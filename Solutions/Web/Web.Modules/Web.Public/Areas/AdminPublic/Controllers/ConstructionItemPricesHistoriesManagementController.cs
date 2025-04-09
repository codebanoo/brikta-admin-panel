using ApiCallers.PublicApiCaller;
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
using VM.Public;
using VM.PVM.Public;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class ConstructionItemPricesHistoriesManagementController : ExtraAdminController
    {
        public ConstructionItemPricesHistoriesManagementController(IHostEnvironment _hostEnvironment,
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




        public IActionResult Index(long Id, string parentType)
        {
            ViewData["Title"] = "تاریخچه قیمت";

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["ParentType"] == null)
                ViewData["ParentType"] = parentType;

            switch (parentType)
            {
                case "item":
                    if (ViewData["Item"] == null)
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

                                        }

                                        #endregion
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        { }

                        ViewData["Item"] = constructionItemsVM;
                    }
                    if (ViewData["DataBackUrl"] == null)
                    {
                        ViewData["DataBackUrl"] = "/AdminPublic/ConstructionItemsManagement/Index/";
                    }
                    break;
                case "subItem":
                    ConstructionSubItemsVM constructionSubItemsVM = new ConstructionSubItemsVM();
                    if (ViewData["Item"] == null)
                    {
                        try
                        {
                            serviceUrl = publicApiUrl + "/api/ConstructionSubItemsManagement/GetConstructionSubItemWithConstructionSubItemId";

                            GetConstructionSubItemWithConstructionSubItemIdPVM getConstructionSubItemWithConstructionSubItemIdPVM = new GetConstructionSubItemWithConstructionSubItemIdPVM()
                            {
                                ConstructionSubItemId = Id,
                                ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            };

                            responseApiCaller = new PublicApiCaller(serviceUrl).GetConstructionSubItemWithConstructionSubItemId(getConstructionSubItemWithConstructionSubItemIdPVM);

                            if (responseApiCaller.IsSuccessStatusCode)
                            {
                                jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                                if (jsonResultWithRecordObjectVM != null)
                                {
                                    if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                                    {
                                        #region Fill UserCreatorName

                                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                                        var record = jObject.ToObject<ConstructionSubItemsVM>();

                                        constructionSubItemsVM = record;

                                        if (record != null)
                                        {

                                        }

                                        #endregion
                                    }
                                }
                            }
                        }
                        catch (Exception exc)
                        { }

                        ViewData["Item"] = constructionSubItemsVM;
                    }
                    if (ViewData["DataBackUrl"] == null)
                    {
                        ViewData["DataBackUrl"] = "/AdminPublic/ConstructionSubItemsManagement/Index/" + constructionSubItemsVM.ConstructionItemId;
                    }
                    break;
            }

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllConstructionItemPricesHistoriesList(
            string parentType,
            long parentId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionItemPricesHistoriesManagement/GetAllConstructionItemPricesHistoriesList";

                GetAllConstructionItemPricesHistoriesListPVM getAllConstructionItemPricesHistoriesListPVM= new GetAllConstructionItemPricesHistoriesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    ParentId = parentId,
                    ParentType = parentType,

                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetAllConstructionItemPricesHistoriesList(getAllConstructionItemPricesHistoriesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ConstructionItemPricesHistoriesVM>>();

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
        public IActionResult GetListOfConstructionItemPricesHistories(int jtStartIndex = 0,
            int jtPageSize = 10,
            long? parentId = null,
            string parentType = null,
            string jtSorting = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/ConstructionItemsManagement/GetListOfConstructionItemPricesHistories";

                GetListOfConstructionItemPricesHistoriesPVM getListOfConstructionItemPricesHistoriesPVM = new GetListOfConstructionItemPricesHistoriesPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    //ConstructionItemParentId = ((constructionItemParentId.HasValue) ? constructionItemParentId.Value : (long?)0),
                    //ConstructionItemTitle = (!string.IsNullOrEmpty(constructionItemTitle) ? constructionItemTitle.Trim() : ""),
                    jtSorting = jtSorting
                };

                //responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfConstructionItems(getListOfConstructionItemPricesHistoriesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<ConstructionItemPricesHistoriesVM>>();

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
    }
}

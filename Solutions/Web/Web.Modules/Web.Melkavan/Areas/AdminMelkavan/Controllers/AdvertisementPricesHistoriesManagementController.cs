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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;
using VM.Melkavan;
using VM.PVM.Melkavan;
using ApiCallers.MelkavanApiCaller;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class AdvertisementPricesHistoriesManagementController : ExtraAdminController
    {
        public AdvertisementPricesHistoriesManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("Index", "PropertiesManagement");
            }
            ViewData["Title"] = "تاریخچه قیمت ها";

            if (ViewData["DomainName"] == null)
                ViewData["DomainName"] = this.domainsSettings.DomainName;
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            if (ViewData["Advertisement"] == null)
            {
                AdvertisementVM advertisementVM = new AdvertisementVM();

                try
                {
                    string serviceUrl = melkavanApiUrl + "/api/AdvertisementManagement/GetAdvertisementWithAdvertisementId";

                    GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM = new GetAdvertisementWithAdvertisementIdPVM()
                    {
                        AdvertisementId = id,
                        Type = "Advertisement",
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAdvertisementWithAdvertisementId(getAdvertisementWithAdvertisementIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<AdvertisementVM>();


                                if (record != null)
                                {
                                    advertisementVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["Advertisement"] = advertisementVM;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesManagement/Index/";
            }
            return View("Index");
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfAdvertisementPricesHistories(
        int jtStartIndex = 0,
        int jtPageSize = 10,
        int advertisementId = 0,
        string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = melkavanApiUrl + "/api/AdvertisementPricesHistoriesManagement/GetListOfAdvertisementPricesHistories";

                GetListOfAdvertisementPricesHistoriesPVM getListOfAdvertisementPricesHistoriesPVM = new GetListOfAdvertisementPricesHistoriesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    AdvertisementId = advertisementId,
                    jtSorting = jtSorting
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetListOfAdvertisementsPricesHistories(getListOfAdvertisementPricesHistoriesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<AdvertisementPricesHistoriesVM>>();

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


                                    #region MyRegion

                                    PersianCalendar persianCalendar = new PersianCalendar();
                                    int persianYear = persianCalendar.GetYear(record.CreateEnDate.Value);
                                    int persianMonth = persianCalendar.GetMonth(record.CreateEnDate.Value);
                                    int persianDay = persianCalendar.GetDayOfMonth(record.CreateEnDate.Value);
                                    record.CreateEnDate = DateTime.Parse(persianYear + "/" + persianMonth + "/" + persianDay);

                                    #endregion
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

    }
}

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
using System.Linq;
using System;
using VM.Base;
using Web.Core.Controllers;
using VM.PVM.Automation;
using ApiCallers.AutomationApiCaller;
using VM.Automation;
using FrameWork;
using ApiCallers.TeniacoApiCaller;
using VM.PVM.Teniaco;
using VM.Teniaco;
using ApiCallers.PublicApiCaller;
using VM.Public;
using VM.PVM.Public;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class OrganizationalPositionsManagementController : ExtraAdminController
    {
        public OrganizationalPositionsManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست پست سازمانی";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            //if (ViewData["OrganizationalPositionsList"] == null)
            //{
            //    List<OrganizationalPositionsVM> organizationalPositionsList = new List<OrganizationalPositionsVM>();

            //    try
            //    {
            //        serviceUrl = teniacoApiUrl + "/api/OrganizationalPositionsManagement/GetListOfOrganizationalPositions";
            //        GetListOfOrganizationalPositionsPVM getListOfOrganizationalPositionsPVM = new GetListOfOrganizationalPositionsPVM();

            //        responseApiCaller = new AutomationApiCaller(serviceUrl).GetListOfOrganizationalPositions(getListOfOrganizationalPositionsPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {
            //                    #region Fill UserCreatorName

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    organizationalPositionsList = jArray.ToObject<List<OrganizationalPositionsVM>>();

            //                    #endregion
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["OrganizationalPositionsList"] = organizationalPositionsList;
            //}


            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }




        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllOrganizationalPositionsList(
            string organizationalPositionName = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/GetAllOrganizationalPositionsList";

                GetAllOrganizationalPositionsListPVM getAllOrganizationalPositionsListPVM = new GetAllOrganizationalPositionsListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    

                    OrganizationalPositionName = organizationalPositionName,
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllOrganizationalPositionsList(getAllOrganizationalPositionsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<OrganizationalPositionsVM>>();

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



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfOrganizationalPositions(
            int jtStartIndex,
            int jtPageSize,
            string organizationalPositionName = "",
            string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/GetListOfOrganizationalPositions";

                GetListOfOrganizationalPositionsPVM getListOfOrganizationalPositionsPVM = new GetListOfOrganizationalPositionsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    OrganizationalPositionName = (!string.IsNullOrEmpty(organizationalPositionName) ? organizationalPositionName.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).GetListOfOrganizationalPositions(getListOfOrganizationalPositionsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<OrganizationalPositionsVM>>();

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
        public IActionResult AddToOrganizationalPositions(OrganizationalPositionsVM organizationalPositionsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
               organizationalPositionsVM.CreateEnDate = DateTime.Now;
               organizationalPositionsVM.CreateTime = PersianDate.TimeNow;
               organizationalPositionsVM.UserIdCreator = this.userId.Value;
               organizationalPositionsVM.IsActivated = true;
               organizationalPositionsVM.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/AddToOrganizationalPositions";

                    AddToOrganizationalPositionsPVM addToOrganizationalPositionsPVM = new AddToOrganizationalPositionsPVM() { 
                        OrganizationalPositionsVM = organizationalPositionsVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).AddToOrganizationalPositions(addToOrganizationalPositionsPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<OrganizationalPositionsVM>();

                                if (record != null)
                                {
                                    organizationalPositionsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = organizationalPositionsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateOrganizationalPositions"))
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
        public IActionResult UpdateOrganizationalPositions(OrganizationalPositionsVM organizationalPositionsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                organizationalPositionsVM.EditEnDate = DateTime.Now;
                organizationalPositionsVM.EditTime = PersianDate.TimeNow;
                organizationalPositionsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/UpdateOrganizationalPositions";

                    UpdateOrganizationalPositionsPVM updateOrganizationalPositionsPVM = new UpdateOrganizationalPositionsPVM()
                    {
                        OrganizationalPositionsVM = organizationalPositionsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateOrganizationalPositions(updateOrganizationalPositionsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            organizationalPositionsVM = jObject.ToObject<OrganizationalPositionsVM>();

                            return Json(new
                            {
                                Result = jsonResultWithRecordObjectVM.Result,
                                Record = organizationalPositionsVM,
                            });
                        }
                        else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateorganizationalPositions"))
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
                    Record = organizationalPositionsVM,
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
        public IActionResult ToggleActivationOrganizationalPositions(int organizationalPositionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/ToggleActivationOrganizationalPositions";

                ToggleActivationOrganizationalPositionsPVM toggleActivationOrganizationalPositionsPVM =
                    new ToggleActivationOrganizationalPositionsPVM()
                    {
                        OrganizationalPositionId = organizationalPositionId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new AutomationApiCaller(serviceUrl).ToggleActivationOrganizationalPositions(toggleActivationOrganizationalPositionsPVM);

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
        public IActionResult TemporaryDeleteOrganizationalPositions(int organizationalPositionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/TemporaryDeleteOrganizationalPositions";

                TemporaryDeleteOrganizationalPositionsPVM temporaryDeleteOrganizationalPositionsPVM =
                    new TemporaryDeleteOrganizationalPositionsPVM()
                    {
                        OrganizationalPositionId = organizationalPositionId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new AutomationApiCaller(serviceUrl).TemporaryDeleteOrganizationalPositions(temporaryDeleteOrganizationalPositionsPVM);

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
        public IActionResult CompleteDeleteOrganizationalPositions(int organizationalPositionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrganizationalPositionsManagement/CompleteDeleteOrganizationalPositions";

                CompleteDeleteOrganizationalPositionsPVM completeDeleteOrganizationalPositionsPVM =
                    new CompleteDeleteOrganizationalPositionsPVM()
                    {
                        OrganizationalPositionId = organizationalPositionId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new AutomationApiCaller(serviceUrl).CompleteDeleteOrganizationalPositions(completeDeleteOrganizationalPositionsPVM);

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

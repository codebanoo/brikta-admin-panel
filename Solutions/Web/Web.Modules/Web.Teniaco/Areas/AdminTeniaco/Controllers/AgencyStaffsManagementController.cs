using ApiCallers.PublicApiCaller;
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
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class AgencyStaffsManagementController : ExtraAdminController
    {
        public AgencyStaffsManagementController(IHostEnvironment _hostEnvironment,
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
                return RedirectToAction("ListOfAgencies", "ColleaguesManagement");
            }
            //ViewData["Title"] = "لیست کارمندان بنگاه";

            ViewData["Title"] = "لیست مشاورین";

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["Agency"] == null)
            {
                AgenciesVM agenciesVM = new AgenciesVM();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/AgenciesManagement/GetAgencyWithAgencyId";

                    GetAgencyWithAgencyIdPVM getAgencyWithAgencyIdPVM = new GetAgencyWithAgencyIdPVM()
                    {
                        AgencyId = id,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "AgenciesManagement", "GetAgencyWithAgencyId", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAgencyWithAgencyId(getAgencyWithAgencyIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<AgenciesVM>();


                                if (record != null)
                                {
                                    agenciesVM = record;


                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["Agency"] = agenciesVM;


            }

            if (ViewData["PositionsList"] == null)
            {
                List<PositionsVM> positionsVMs = new List<PositionsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/PositionsManagement/GetAllPositionsList";
                    GetAllPositionsListPVM getAllPositionsListPVM = new GetAllPositionsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PositionsManagement", "GetAllPositionsList", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPositionsList(getAllPositionsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                positionsVMs = jArray.ToObject<List<PositionsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PositionsList"] = positionsVMs;
            }

            if (ViewData["AgencyStaffs"] == null)
            {
                List<AgencyStaffsVM> agencyStaffsVM = new List<AgencyStaffsVM>();

                try
                {
                    string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/GetListOfAgencyStaffs";

                    GetListOfAgencyStaffsPVM getListOfAgencyStaffsPVM = new GetListOfAgencyStaffsPVM()
                    {
                        AgencyId = id,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "AgencyStaffsManagement", "GetListOfAgencyStaffs", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfAgencyStaffs(getListOfAgencyStaffsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                agencyStaffsVM = jArray.ToObject<List<AgencyStaffsVM>>();

                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["AgencyStaffs"] = agencyStaffsVM;


            }



            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/ColleaguesManagement/ListOfAgencies/";
            }

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllAgencyStaffsList(
           int? agencyId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/GetAllAgencyStaffsList";

                GetAllAgencyStaffsListPVM getAllAgencyStaffsListPVM = new GetAllAgencyStaffsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    AgencyId = agencyId.Value
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllAgencyStaffsList(getAllAgencyStaffsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<AgencyStaffsVM>>();

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
        public IActionResult GetListOfAgencyStaffs(
          int jtStartIndex = 0,
          int jtPageSize = 10,
          int agencyId = 0,
          string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/GetListOfAgencyStaffs";

                GetListOfAgencyStaffsPVM getListOfAgencyStaffsPVM = new GetListOfAgencyStaffsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    AgencyId = agencyId,
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfAgencyStaffs(getListOfAgencyStaffsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<AgencyStaffsVM>>();

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
        public IActionResult AddToAgencyStaffs(AgencyStaffsVM agencyStaffsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                agencyStaffsVM.CreateEnDate = DateTime.Now;
                agencyStaffsVM.CreateTime = PersianDate.TimeNow;
                agencyStaffsVM.UserIdCreator = this.userId.Value;
                agencyStaffsVM.IsActivated = true;
                agencyStaffsVM.IsDeleted = false;
                //agencyStaffsVM.CustomUsersVM.ParentUserId = 2;
                agencyStaffsVM.CustomUsersVM.ParentUserId = agencyStaffsVM.ParentId;
                agencyStaffsVM.CustomUsersVM.RoleIds = new List<int>();
                agencyStaffsVM.CustomUsersVM.LevelIds = new List<int>();
                agencyStaffsVM.CustomUsersVM.ReplyPassword = agencyStaffsVM.CustomUsersVM.Password;
                agencyStaffsVM.CustomUsersVM.DomainSettingId = this.domainsSettings.DomainSettingId;

                ModelState.Remove("CustomUsersVM.RoleIds");
                ModelState.Remove("CustomUsersVM.LevelIds");
                ModelState.Remove("CustomUsersVM.ReplyPassword");
                ModelState.Remove("CustomUsersVM.Password");
                ModelState.Remove("CustomUsersVM.ParentUserId");
                ModelState.Remove("CustomUsersVM.Name");
                ModelState.Remove("CustomUsersVM.Family");
                ModelState.Remove("CustomUsersVM.Mobile");


                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/AddToAgencyStaffs";

                    AddToAgencyStaffsPVM addToAgencyStaffsPVM = new AddToAgencyStaffsPVM()
                    {
                        AgencyStaffsVM = agencyStaffsVM,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToAgencyStaffs(addToAgencyStaffsPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<AgencyStaffsVM>();

                                if (record != null)
                                {
                                    agencyStaffsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = agencyStaffsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateAgency"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
                            else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("AgentManager"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "این کاربر در یک آژانس املاک دیگری به عنوان مدیر املاک شناخته شده است."
                                });
                            }
                            else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("AnotherAgent"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "این کاربر در یک آژانس املاک دیگری به عنوان مشاور املاک شناخته شده است."
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
        public IActionResult UpdateAgencyStaffs(AgencyStaffsVM agencyStaffsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                agencyStaffsVM.EditEnDate = DateTime.Now;
                agencyStaffsVM.EditTime = PersianDate.TimeNow;
                agencyStaffsVM.UserIdEditor = this.userId.Value;
                //agencyStaffsVM.CustomUsersVM.ParentUserId = agencyStaffsVM.ParentId;
                //agencyStaffsVM.CustomUsersVM.RoleIds = new List<int>();
                //agencyStaffsVM.CustomUsersVM.LevelIds = new List<int>();

                ModelState.Remove("CustomUsersVM.ParentUserId");
                //ModelState.Remove("CustomUsersVM.RoleIds");
                //ModelState.Remove("CustomUsersVM.LevelIds");


                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/UpdateAgencyStaffs";

                    UpdateAgencyStaffsPVM updateAgencyStaffsPVM = new UpdateAgencyStaffsPVM()
                    {
                        AgencyStaffsVM = agencyStaffsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateAgencyStaffs(updateAgencyStaffsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                agencyStaffsVM = jObject.ToObject<AgencyStaffsVM>();

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = agencyStaffsVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateAgencyStaffs"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
                            else if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("ParentUserError"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "والد نمیتواند همان مشاور باشد."
                                });
                            }

                        }
                        
                    }

                }
                return Json(new
                {
                    Result = jsonResultWithRecordObjectVM.Result,
                    Record = agencyStaffsVM,
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
        public IActionResult ToggleActivationAgencyStaffs(int AgencyStaffsId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/ToggleActivationAgencyStaffs";

                ToggleActivationAgencyStaffsPVM toggleActivationAgencyStaffsPVM =
                    new ToggleActivationAgencyStaffsPVM()
                    {
                        AgencyStaffsId = AgencyStaffsId,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationAgencyStaffs(toggleActivationAgencyStaffsPVM);

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
        public IActionResult TemporaryDeleteAgencyStaffs(int AgencyStaffsId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/TemporaryDeleteAgencyStaffs";

                TemporaryDeleteAgencyStaffsPVM temporaryDeleteAgencyStaffsPVM =
                    new TemporaryDeleteAgencyStaffsPVM()
                    {
                        AgencyStaffsId = AgencyStaffsId,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteAgencyStaffs(temporaryDeleteAgencyStaffsPVM);

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
        public IActionResult CompleteDeleteAgencyStaffs(int AgencyStaffsId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/AgencyStaffsManagement/CompleteDeleteAgencyStaffs";

                CompleteDeleteAgencyStaffsPVM completeDeleteAgencyStaffsPVM =
                    new CompleteDeleteAgencyStaffsPVM()
                    {
                        AgencyStaffsId = AgencyStaffsId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteAgencyStaffs(completeDeleteAgencyStaffsPVM);

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




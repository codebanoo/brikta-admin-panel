using ApiCallers.PublicApiCaller;
using AutoMapper;
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
using VM.Public;
using VM.PVM.Public;
using Web.Core.Controllers;
using VM.Automation;
using VM.PVM.Automation;
using ApiCallers.AutomationApiCaller;
using VM.Teniaco;
using AutoMapper.Features;
using ApiCallers.TeniacoApiCaller;
using CustomAttributes;
using FrameWork;
using VM.PVM.Teniaco;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class OrgChartManagementController : ExtraAdminController
    {
        public OrgChartManagementController(IHostEnvironment _hostEnvironment,
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
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ViewData["Title"] = "ساختار سازمانی";

            if (ViewData["NodeTypesList"] == null)
            {
                //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

                List<NodeTypesVM> nodeTypesVMList = new List<NodeTypesVM>();

                try
                {
                    string serviceUrl = automationApiUrl + "/api/NodeTypesManagement/GetAllNodeTypesList";

                    GetAllNodeTypesListPVM getAllNodeTypesListPVM =
                        new GetAllNodeTypesListPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };
                    responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllNodeTypesList(getAllNodeTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<NodeTypesVM>>();

                                if (records.Count > 0)
                                {

                                }

                                nodeTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["NodeTypesList"] = nodeTypesVMList;
            }

            List<long> orgChartUserIds = new List<long>();

            if (ViewData["HierarchyOfOrgChartNodes"] == null)
            {
                OrgChartNodeWithDataVM orgChartNodeWithDataVM = new OrgChartNodeWithDataVM();

                try
                {
                    string serviceUrl = automationApiUrl + "/api/OrgChartNodesManagement/GetHierarchyOfOrgChartNodes";

                    GetHierarchyOfOrgChartNodesPVM getHierarchyOfOrgChartNodesPVM =
                        new GetHierarchyOfOrgChartNodesPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            OrgChartNodeId = 0,
                            UserId = this.userId.Value
                        };
                    responseApiCaller = new AutomationApiCaller(serviceUrl).GetHierarchyOfOrgChartNodes(getHierarchyOfOrgChartNodesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {

                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<OrgChartNodeWithDataVM>();

                                if (record != null)
                                {
                                    orgChartNodeWithDataVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                if (orgChartNodeWithDataVM != null)
                    if (orgChartNodeWithDataVM.UserIds != null)
                        if (orgChartNodeWithDataVM.UserIds.Count > 0)
                        {
                            orgChartUserIds = orgChartNodeWithDataVM.UserIds;
                        }

                ViewData["HierarchyOfOrgChartNodes"] = orgChartNodeWithDataVM;
            }

            if (ViewData["CustomUsers"] == null)
            {
                //var users = consoleBusiness.GetUsersListWithMultiRoleNames(this.userId.Value, "Admins;Users", null/*this.domainsSettings.DomainSettingId*/);

                var users = consoleBusiness.GetUsersListWithMultiRoleNames(this.userId.Value, "Admins;Users", null/*this.domainsSettings.DomainSettingId*/);

                ViewData["CustomUsers"] = users.Where(u => !orgChartUserIds.Contains(u.UserId)).ToList();
            }

            //if (ViewData["BoardMembersList"] == null)
            //{
            //    //ViewData["EducationLevelsList"] = institutionsBusiness.GetAllEducationLevelsList(this.userId.Value);

            //    List<BoardMembersVM> boardMembersVMList = new List<BoardMembersVM>();

            //    try
            //    {
            //        string serviceUrl = automationApiUrl + "/api/NodeTypesManagement/GetAllNodeTypesList";

            //        GetAllNodeTypesListPVM getAllNodeTypesListPVM =
            //            new GetAllNodeTypesListPVM()
            //            {
            //                ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
            //            };
            //        responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllNodeTypesList(getAllNodeTypesListPVM);

            //        if (responseApiCaller.IsSuccessStatusCode)
            //        {
            //            jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

            //            if (jsonResultWithRecordsObjectVM != null)
            //            {
            //                if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
            //                {

            //                    JArray jArray = jsonResultWithRecordsObjectVM.Records;
            //                    var records = jArray.ToObject<List<NodeTypesVM>>();

            //                    if (records.Count > 0)
            //                    {

            //                    }

            //                    nodeTypesVMList = records;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception exc)
            //    { }

            //    ViewData["NodeTypesList"] = nodeTypesVMList;
            //}

            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult EditBoardMembers(List<BoardMembersVM> boardMembersVMList)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                foreach (var boardMembersVM in boardMembersVMList)
                {
                    boardMembersVM.CreateEnDate = DateTime.Now;
                    boardMembersVM.CreateTime = PersianDate.TimeNow;
                    boardMembersVM.UserIdCreator = this.userId.Value;
                }

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/BoardMembersManagement/EditBoardMembers";

                    EditBoardMembersPVM editBoardMembersPVM = new EditBoardMembersPVM()
                    {
                        BoardMembersVMList = boardMembersVMList,
                        OrgChartNodeId = 2,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        //OrgChartNodeId = boardMembersVMList.FirstOrDefault().OrgChartNodeId
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).EditBoardMembers(editBoardMembersPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                        if (jsonResultObjectVM != null)
                        {
                            if (jsonResultObjectVM.Result.Equals("OK"))
                            {
                                return Json(new
                                {
                                    Result = jsonResultObjectVM.Result,
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
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToOrgChartNodes(OrgChartNodesVM orgChartNodesVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                orgChartNodesVM.CreateEnDate = DateTime.Now;
                orgChartNodesVM.CreateTime = PersianDate.TimeNow;
                //orgChartNodesVM.UserIdCreator = this.userId.Value;
                orgChartNodesVM.IsActivated = true;
                orgChartNodesVM.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/OrgChartNodesManagement/AddToOrgChartNodes";

                    AddToOrgChartNodesPVM addToOrgChartNodesPVM = new AddToOrgChartNodesPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        OrgChartNodesVM = orgChartNodesVM
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).AddToOrgChartNodes(addToOrgChartNodesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                        if (jsonResultObjectVM != null)
                        {
                            if (jsonResultObjectVM.Result.Equals("OK"))
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                    Message = jsonResultObjectVM.Message
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
        public IActionResult UpdateOrgChartNodes(OrgChartNodesVM orgChartNodesVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                orgChartNodesVM.EditEnDate = DateTime.Now;
                orgChartNodesVM.EditTime = PersianDate.TimeNow;
                orgChartNodesVM.UserIdEditor = orgChartNodesVM.UserIdCreator.HasValue ? orgChartNodesVM.UserIdCreator.Value : 0;
                orgChartNodesVM.IsActivated = true;
                orgChartNodesVM.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/OrgChartNodesManagement/UpdateOrgChartNodes";

                    UpdateOrgChartNodesPVM updateOrgChartNodesPVM = new UpdateOrgChartNodesPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        OrgChartNodesVM = orgChartNodesVM
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateOrgChartNodes(updateOrgChartNodesPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                        if (jsonResultObjectVM != null)
                        {
                            if (jsonResultObjectVM.Result.Equals("OK"))
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                    Message = jsonResultObjectVM.Message
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
        public IActionResult CompleteDeleteOrgChartNodes(int OrgChartNodeId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/OrgChartNodesManagement/CompleteDeleteOrgChartNodes";

                CompleteDeleteOrgChartNodesPVM completeDeleteOrgChartNodesPVM = new CompleteDeleteOrgChartNodesPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    OrgChartNodeId = OrgChartNodeId,
                    UserId = this.userId.Value
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).CompleteDeleteOrgChartNodes(completeDeleteOrgChartNodesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = jsonResultObjectVM.Result,
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
                Message = "ErrorMessage"
            });
        }
    }
}

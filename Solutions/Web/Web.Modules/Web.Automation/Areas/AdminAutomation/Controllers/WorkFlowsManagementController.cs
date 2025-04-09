using ApiCallers.AutomationApiCaller;
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
using System;
using VM.Automation;
using VM.Base;
using VM.PVM.Automation;
using Web.Core.Controllers;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class WorkFlowsManagementController : ExtraAdminController
    {
        public WorkFlowsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult WorkFlowDesign()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ViewData["Title"] = "طراحی گردش کار";

            if (ViewData["AllOrgChartNodesList"] == null)
            {
                //List<OrgChartNodesVM> orgChartNodesVMList = new List<OrgChartNodesVM>();

                //try
                //{
                //    string serviceUrl = automationApiUrl + "/api/NodeTypesManagement/GetAllOrgChartNodesList";

                //    GetAllOrgChartNodesListPVM getAllOrgChartNodesListPVM =
                //        new GetAllOrgChartNodesListPVM()
                //        {
                //            ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                //        };
                //    responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllOrgChartNodesList(getAllOrgChartNodesListPVM);

                //    if (responseApiCaller.IsSuccessStatusCode)
                //    {
                //        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //        if (jsonResultWithRecordsObjectVM != null)
                //        {
                //            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //            {

                //                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //                var records = jArray.ToObject<List<OrgChartNodesVM>>();

                //                if (records.Count > 0)
                //                {

                //                }

                //                orgChartNodesVMList = records;
                //            }
                //        }
                //    }
                //}
                //catch (Exception exc)
                //{ }

                //ViewData["AllOrgChartNodesList"] = orgChartNodesVMList;
            }

            if (ViewData["HierarchyOfOrgChartNodesForTreeView"] == null)
            {
                string strData = "";

                try
                {
                    string serviceUrl = automationApiUrl + "/api/OrgChartNodesManagement/GetHierarchyOfOrgChartNodesForTreeView";

                    GetHierarchyOfOrgChartNodesForTreeViewPVM getHierarchyOfOrgChartNodesForTreeViewPVM =
                        new GetHierarchyOfOrgChartNodesForTreeViewPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                            OrgChartNodeId = 0,
                            UserId = this.userId.Value
                        };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).GetHierarchyOfOrgChartNodesForTreeView(getHierarchyOfOrgChartNodesForTreeViewPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                if (!string.IsNullOrEmpty(jsonResultWithRecordObjectVM.Record))
                                {
                                    strData = jsonResultWithRecordObjectVM.Record;
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["HierarchyOfOrgChartNodesForTreeView"] = strData;
            }

            return View("Index");
        }
    }
}

using APIs.Automation.Models.Business;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.Teniaco.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Automation;
using APIs.CustomAttributes;
using APIs.Core.Controllers;
using VM.Automation;
using System.Linq;
using System.Collections.Generic;
using APIs.Automation.Models.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// OrgChartNodesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class OrgChartNodesManagementController : ApiBaseController
    {
        /// <summary>
        /// OrgChartNodesManagement
        /// </summary>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <param name="_actionContextAccessor"></param>
        /// <param name="_configurationRoot"></param>
        /// <param name="_consoleBusiness"></param>
        /// <param name="_automationApiBusiness"></param>
        /// <param name="_publicApiBusiness"></param>
        /// <param name="_teniacoApiBusiness"></param>
        /// <param name="_melkavanApiBusiness"></param>
        /// <param name="_projectsApiBusiness"></param>
        /// <param name="_telegramBotApiBusiness"></param>
        /// <param name="_appSettingsSection"></param>
        public OrgChartNodesManagementController(IHostEnvironment _hostingEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IConsoleBusiness _consoleBusiness,
            IAutomationApiBusiness _automationApiBusiness,
            IPublicApiBusiness _publicApiBusiness,
            ITeniacoApiBusiness _teniacoApiBusiness,
            IMelkavanApiBusiness _melkavanApiBusiness,
            IProjectsApiBusiness _projectsApiBusiness,
            ITelegramBotApiBusiness _telegramBotApiBusiness,
            IOptions<AppSettings> _appSettingsSection) :
            base(
                _hostingEnvironment,
                _httpContextAccessor,
                _actionContextAccessor,
                _configurationRoot,
                _consoleBusiness,
                _automationApiBusiness,
                _publicApiBusiness,
                _teniacoApiBusiness,
                _melkavanApiBusiness,
                _projectsApiBusiness,
                _telegramBotApiBusiness,
                _appSettingsSection)
        {

        }

        /// <summary>
        /// GetHierarchyOfOrgChartNodes
        /// </summary>
        /// <param name="getHierarchyOfOrgChartNodesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetHierarchyOfOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetHierarchyOfOrgChartNodes(GetHierarchyOfOrgChartNodesPVM getHierarchyOfOrgChartNodesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                OrgChartNodesVM rootOrgChartNodesVM = new OrgChartNodesVM();

                if (getHierarchyOfOrgChartNodesPVM.OrgChartNodeId.Equals(0))
                {
                    rootOrgChartNodesVM = automationApiBusiness.GetFirstOrgChartNode(getHierarchyOfOrgChartNodesPVM.UserId.Value);
                }

                var listOfOrgChartNodes = automationApiBusiness.GetHierarchyOfOrgChartNodes(getHierarchyOfOrgChartNodesPVM.ChildsUsersIds,
                    getHierarchyOfOrgChartNodesPVM.OrgChartNodeId,
                    getHierarchyOfOrgChartNodesPVM.UserId.Value,
                    ref rootOrgChartNodesVM);

                //string strNodes = "";

                var nodeTypesList = automationApiBusiness.GetAllNodeTypesList(true);

                List<BoardMembersVM> boardMembersVMList = new List<BoardMembersVM>();

                if (rootOrgChartNodesVM.NodeTypeId.Equals(2))
                {
                    boardMembersVMList = automationApiBusiness.GetAllBoardMembersList(rootOrgChartNodesVM.OrgChartNodeId);
                }

                var user = consoleBusiness.GetUserWithUserId(getHierarchyOfOrgChartNodesPVM.UserId.Value);

                //userId = user.UserId;
                //parentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 0;

                string staffName = "";

                if (!string.IsNullOrEmpty(user.Name))
                    staffName = user.Name;

                if (!string.IsNullOrEmpty(user.Family))
                    staffName += " " + user.Family;

                List<long> userIds = new List<long>();

                //userIds.Add(user.UserId);

                var jsonData = new OrgChartNodeWithDataVM
                {
                    UserIds = new List<long>(),
                    name = rootOrgChartNodesVM.NodeTitle,
                    id = rootOrgChartNodesVM.OrgChartNodeId.ToString(),
                    data = new NodeDataVM
                    {
                        code = "",
                        type_id = rootOrgChartNodesVM.NodeTypeId,
                        type = nodeTypesList.Where(t => t.NodeTypeId.Equals(rootOrgChartNodesVM.NodeTypeId)).FirstOrDefault().NodeTypeTitle,
                        description = rootOrgChartNodesVM.NodeDescription,
                        userId = user.UserId,
                        parentUserId = user.ParentUserId.Value,
                        staffName = staffName,
                        BoardMembersVMList = boardMembersVMList,
                        OrgChartNodeId = rootOrgChartNodesVM.OrgChartNodeId
                    },
                    children = GetChildNode(rootOrgChartNodesVM, listOfOrgChartNodes, nodeTypesList, userIds)
                };

                jsonData.UserIds = userIds;

                //var jsonData = GetChildNode(rootOrgChartNodesVM, listOfOrgChartNodes, nodeTypesList);

                //strNodes = JsonConvert.SerializeObject(jsonData);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = jsonData;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// GetHierarchyOfOrgChartNodesForTreeView
        /// </summary>
        /// <param name="getHierarchyOfOrgChartNodesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetHierarchyOfOrgChartNodesForTreeView")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetHierarchyOfOrgChartNodesForTreeView(GetHierarchyOfOrgChartNodesForTreeViewPVM getHierarchyOfOrgChartNodesForTreeViewPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                OrgChartNodesVM rootOrgChartNodesVM = new OrgChartNodesVM();

                if (getHierarchyOfOrgChartNodesForTreeViewPVM.OrgChartNodeId.Equals(0))
                {
                    rootOrgChartNodesVM = automationApiBusiness.GetFirstOrgChartNode(getHierarchyOfOrgChartNodesForTreeViewPVM.UserId.Value);
                }

                var listOfOrgChartNodes = automationApiBusiness.GetHierarchyOfOrgChartNodes(getHierarchyOfOrgChartNodesForTreeViewPVM.ChildsUsersIds,
                    getHierarchyOfOrgChartNodesForTreeViewPVM.OrgChartNodeId,
                    getHierarchyOfOrgChartNodesForTreeViewPVM.UserId.Value,
                    ref rootOrgChartNodesVM);

                string strData = @" {                     
                    id: """ + rootOrgChartNodesVM.OrgChartNodeId + @""", 
                    key: """ + rootOrgChartNodesVM.OrgChartNodeId + @""",
                    title: """ + rootOrgChartNodesVM.NodeTitle + @""", 
                    /*folder: true,*/ 
                    expanded: true, 
                    selected: true,
                    children: [ " + ((listOfOrgChartNodes.Where(n => n.ParentOrgChartNodeId.HasValue).Where(n => n.ParentOrgChartNodeId.Value.Equals(rootOrgChartNodesVM.OrgChartNodeId)).Any()) ?
                        GetChildNodeForTreeView(rootOrgChartNodesVM, listOfOrgChartNodes/*, nodeTypesList*/) :
                        @"") + @" ], 
                } ";
                //var jsonData = new JsTreeNodeVM
                //{
                //    title = "",
                //    folder = true,
                //    expanded = true,
                //    id = 0,
                //    key = 0,
                //    children = GetChildNodeForTreeView(rootOrgChartNodesVM, listOfOrgChartNodes, nodeTypesList)
                //};

                //var jsonData = GetChildNode(rootOrgChartNodesVM, listOfOrgChartNodes, nodeTypesList);

                //strNodes = JsonConvert.SerializeObject(jsonData);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = strData;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// GetAllOrgChartNodesList
        /// </summary>
        /// <param name="getAllOrgChartNodesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllOrgChartNodesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllOrgChartNodesList(GetAllOrgChartNodesListPVM getAllOrgChartNodesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                var listOfOrgChartNodes = automationApiBusiness.GetAllOrgChartNodesList(getAllOrgChartNodesListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfOrgChartNodes;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfOrgChartNodes);

                return new JsonResult(jsonResultWithRecordsObjectVM);
                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// GetListOfOrgChartNodes
        /// </summary>
        /// <param name="getListOfOrgChartNodesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfOrgChartNodes(GetListOfOrgChartNodesPVM getListOfOrgChartNodesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfOrgChartNodes = automationApiBusiness.GetListOfOrgChartNodes(getListOfOrgChartNodesPVM.jtStartIndex.Value,
                    getListOfOrgChartNodesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfOrgChartNodesPVM.ChildsUsersIds,
                    getListOfOrgChartNodesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfOrgChartNodes;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfOrgChartNodes);

                return new JsonResult(jsonResultWithRecordsObjectVM);
                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// AddToOrgChartNodes
        /// </summary>
        /// <param name="addToOrgChartNodesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToOrgChartNodes([FromBody] AddToOrgChartNodesPVM
            addToOrgChartNodesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            string returnMessage = "";

            try
            {
                if (automationApiBusiness.AddToOrgChartNodes(
                    addToOrgChartNodesPVM.OrgChartNodesVM,
                    ref returnMessage,
                    consoleBusiness
                    /*,
                    addToOrgChartNodesPVM.ChildsUsersIds*/))
                {
                    //addToOrgChartNodesPVM.OrgChartNodesVM.OrgChartNodeId = orgChartNodeId;

                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            if (!string.IsNullOrEmpty(returnMessage))
                jsonResultObjectVM.Message = returnMessage;
            else
                jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// ExistOrgChartNodeWithUserId
        /// </summary>
        /// <param name="existOrgChartNodeWithUserIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("ExistOrgChartNodeWithUserId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ExistOrgChartNodeWithUserId([FromBody] ExistOrgChartNodeWithUserIdPVM
            existOrgChartNodeWithUserIdPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.ExistOrgChartNodeWithUserId(
                    existOrgChartNodeWithUserIdPVM.UserId.Value))
                {
                    //addToOrgChartNodesPVM.OrgChartNodesVM.OrgChartNodeId = orgChartNodeId;

                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// GetOrgChartNodeWithOrgChartNodeId
        /// </summary>
        /// <param name="getOrgChartNodeWithOrgChartNodeIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetOrgChartNodeWithOrgChartNodeId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrgChartNodeWithOrgChartNodeId([FromBody] GetOrgChartNodeWithOrgChartNodeIdPVM
            getOrgChartNodeWithOrgChartNodeIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var orgChartNode = automationApiBusiness.GetOrgChartNodeWithOrgChartNodeId(
                    getOrgChartNodeWithOrgChartNodeIdPVM.OrgChartNodeId,
                    getOrgChartNodeWithOrgChartNodeIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = orgChartNode;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateOrgChartNodes
        /// </summary>
        /// <param name="updateOrgChartNodesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateOrgChartNodes([FromBody] UpdateOrgChartNodesPVM
            updateOrgChartNodesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            string returnMessage = "";

            try
            {
                if (automationApiBusiness.UpdateOrgChartNodes(
                    updateOrgChartNodesPVM.OrgChartNodesVM,
                    ref returnMessage,
                    updateOrgChartNodesPVM.ChildsUsersIds,
                    consoleBusiness))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                }

                jsonResultObjectVM.Result = "OK";

                return new JsonResult(jsonResultObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            if (!string.IsNullOrEmpty(returnMessage))
                jsonResultObjectVM.Message = returnMessage;
            else
                jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// ToggleActivationOrgChartNodes
        /// </summary>
        /// <param name="toggleActivationOrgChartNodesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationOrgChartNodes([FromBody] ToggleActivationOrgChartNodesPVM
            toggleActivationOrgChartNodesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.ToggleActivationOrgChartNodes(
                    toggleActivationOrgChartNodesPVM.OrgChartNodeId,
                    toggleActivationOrgChartNodesPVM.UserId.Value,
                    toggleActivationOrgChartNodesPVM.ChildsUsersIds))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                        jsonResultObjectVM.Result = returnMessage;
                    else
                        jsonResultObjectVM.Result = "OK";
                }

                return new JsonResult(jsonResultObjectVM);
                //return jsonResultObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultObjectVM;
        }

        /// <summary>
        /// TemporaryDeleteOrgChartNodes
        /// </summary>
        /// <param name="temporaryDeleteOrgChartNodesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteOrgChartNodes([FromBody] TemporaryDeleteOrgChartNodesPVM
            temporaryDeleteOrgChartNodesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (automationApiBusiness.TemporaryDeleteOrgChartNodes(
                    temporaryDeleteOrgChartNodesPVM.OrgChartNodeId,
                    temporaryDeleteOrgChartNodesPVM.UserId.Value,
                    temporaryDeleteOrgChartNodesPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                    //return jsonResultObjectVM;
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultObjectVM;
        }

        /// <summary>
        /// CompleteDeleteOrgChartNodes
        /// </summary>
        /// <param name="completeDeleteOrgChartNodesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteOrgChartNodes")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteOrgChartNodes([FromBody] CompleteDeleteOrgChartNodesPVM completeDeleteOrgChartNodesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.CompleteDeleteOrgChartNodes(
                    completeDeleteOrgChartNodesPVM.OrgChartNodeId,
                    completeDeleteOrgChartNodesPVM.ChildsUsersIds,
                    completeDeleteOrgChartNodesPVM.UserId.Value,
                    consoleBusiness))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        #region needed method

        private List<OrgChartNodeWithDataVM> GetChildNode(OrgChartNodesVM parentOrgChartNodesVM, List<OrgChartNodesVM> orgChartNodesVMList, List<NodeTypesVM> nodeTypesVMList, List<long> userIds)
        {
            List<OrgChartNodeWithDataVM> OrgChartNodeWithDataList = new List<OrgChartNodeWithDataVM>();

            try
            {
                if (orgChartNodesVMList.Where(n => n.ParentOrgChartNodeId.HasValue).Any(n => n.ParentOrgChartNodeId.Value.Equals(parentOrgChartNodesVM.OrgChartNodeId)))
                {
                    var childs = orgChartNodesVMList.Where(n => n.ParentOrgChartNodeId.HasValue).Where(n =>
                        n.ParentOrgChartNodeId.Value.Equals(parentOrgChartNodesVM.OrgChartNodeId)).ToList();

                    foreach (var child in childs)
                    {
                        string staffName = "";

                        long parentUserId = 0;
                        long userId = 0;

                        //switch (parentOrgChartNodesVM.NodeTypeId)
                        //{
                        //    case 1:
                        //        break;
                        //    case 2:
                        //        break;
                        //    case 4:
                        //        break;
                        //    case 5:
                        //        break;
                        //    case 6:
                        //        break;
                        //    case 7:
                        //        break;
                        //    case 8:
                        //        parentUserId = parentOrgChartNodesVM.UserIdCreator.Value;
                        //        break;
                        //}

                        switch (child.NodeTypeId)
                        {
                            case 1://ساختار سازمانی
                                break;
                            case 2://هیئت مدیره
                                break;
                            case 4://گروه
                            case 5://شرکت
                            case 6://معاونت
                            case 7://واحد سازمانی
                            case 9://پروژه
                            case 8://شخص
                                var user = consoleBusiness.GetUserWithUserId(child.UserIdCreator.Value);

                                userId = user.UserId;

                                //if (userId.Equals(0))
                                //{
                                //    int i = 0;
                                //}

                                parentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 0;

                                if (!string.IsNullOrEmpty(user.Name))
                                    staffName = user.Name;

                                if (!string.IsNullOrEmpty(user.Family))
                                    staffName += " " + user.Family;
                                break;
                        }

                        List<BoardMembersVM> boardMembersVMList = new List<BoardMembersVM>();

                        if (child.NodeTypeId.Equals(2))
                        {
                            boardMembersVMList = automationApiBusiness.GetAllBoardMembersList(child.OrgChartNodeId);
                        }

                        if (!userIds.Where(u => u.Equals(userId)).Any())
                            userIds.Add(userId);

                        var orgChartNodeWithDataVM = new OrgChartNodeWithDataVM
                        {
                            //UserIds = new List<long>(),
                            name = child.NodeTitle,
                            id = child.OrgChartNodeId.ToString(),
                            data = new NodeDataVM
                            {
                                code = "",
                                type_id = child.NodeTypeId,
                                type = nodeTypesVMList.Where(t => t.NodeTypeId.Equals(child.NodeTypeId)).FirstOrDefault().NodeTypeTitle,
                                description = child.NodeDescription,
                                userId = userId,
                                parentUserId = parentUserId,
                                OrgChartNodeId = child.OrgChartNodeId,
                                ParentOrgChartNodeId = parentOrgChartNodesVM.OrgChartNodeId,
                                staffName = staffName,
                                BoardMembersVMList = boardMembersVMList
                            },
                            children = GetChildNode(child, orgChartNodesVMList, nodeTypesVMList, userIds)
                        };

                        if (!orgChartNodeWithDataVM.UserIds.Where(u => u.Equals(userId)).Any())
                            orgChartNodeWithDataVM.UserIds.Add(userId);

                        OrgChartNodeWithDataList.Add(orgChartNodeWithDataVM);
                    }
                }
            }
            catch (Exception exc)
            { }

            return OrgChartNodeWithDataList;
        }

        private string GetChildNodeForTreeView(OrgChartNodesVM parentOrgChartNodesVM, List<OrgChartNodesVM> orgChartNodesVMList/*, List<NodeTypesVM> nodeTypesVMList*/)
        {
            //List<OrgChartNodeWithDataVM> OrgChartNodeWithDataList = new List<OrgChartNodeWithDataVM>();

            string strData = "";

            try
            {
                if (orgChartNodesVMList.Where(n => n.ParentOrgChartNodeId.HasValue).Any(n => n.ParentOrgChartNodeId.Value.Equals(parentOrgChartNodesVM.OrgChartNodeId)))
                {
                    var childs = orgChartNodesVMList.Where(n => n.ParentOrgChartNodeId.HasValue).Where(n =>
                        n.ParentOrgChartNodeId.Value.Equals(parentOrgChartNodesVM.OrgChartNodeId)).ToList();

                    foreach (var child in childs)
                    {
                        string staffName = "";

                        long parentUserId = 0;
                        long userId = 0;

                        //switch (parentOrgChartNodesVM.NodeTypeId)
                        //{
                        //    case 1:
                        //        break;
                        //    case 2:
                        //        break;
                        //    case 4:
                        //        break;
                        //    case 5:
                        //        break;
                        //    case 6:
                        //        break;
                        //    case 7:
                        //        break;
                        //    case 8:
                        //        parentUserId = parentOrgChartNodesVM.UserIdCreator.Value;
                        //        break;
                        //}

                        switch (child.NodeTypeId)
                        {
                            case 1://ساختار سازمانی
                                break;
                            case 2://هیئت مدیره
                                break;
                            case 4://گروه
                            case 5://شرکت
                            case 6://معاونت
                            case 7://واحد سازمانی
                            case 9://پروژه
                            case 8://شخص
                                var user = consoleBusiness.GetUserWithUserId(child.UserIdCreator.Value);

                                userId = user.UserId;
                                parentUserId = user.ParentUserId.HasValue ? user.ParentUserId.Value : 0;

                                if (!string.IsNullOrEmpty(user.Name))
                                    staffName = user.Name;

                                if (!string.IsNullOrEmpty(user.Family))
                                    staffName += " " + user.Family;
                                break;
                        }

                        strData += @" {                     
                            id: """ + child.OrgChartNodeId + @""", 
                            key: """ + child.OrgChartNodeId + @""",
                            title: """ + child.NodeTitle + @""", 
                            /*folder: true,*/ 
                            expanded: true, 
                            selected: true,
                            children: [ " + ((orgChartNodesVMList.Where(n => n.ParentOrgChartNodeId.HasValue).Where(n => n.ParentOrgChartNodeId.Value.Equals(parentOrgChartNodesVM.OrgChartNodeId)).Any()) ?
                                                GetChildNodeForTreeView(child, orgChartNodesVMList/*, nodeTypesList*/) :
                                                @"") + @" ], 
                        }, ";
                    }
                }
            }
            catch (Exception exc)
            { }

            return strData;

            //return OrgChartNodeWithDataList;
        }

        #endregion
    }
}

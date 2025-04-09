using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
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
using System.Linq;
using FrameWork;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// OrganizationalPositionsManagement
    /// </summary>
    /// 

    [CustomApiAuthentication]
    public class OrganizationalPositionsManagementController : ApiBaseController
    {
        /// <summary>
        /// OrganizationalPositionsManagement
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
        public OrganizationalPositionsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllOrganizationalPositionsList
        /// </summary>
        /// <param name="getAllOrganizationalPositionsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllOrganizationalPositionsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllOrganizationalPositionsList([FromBody] GetAllOrganizationalPositionsListPVM getAllOrganizationalPositionsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllOrganizationalPositionsListPVM.ChildsUsersIds == null)
                    {
                        getAllOrganizationalPositionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllOrganizationalPositionsListPVM.ChildsUsersIds.Count == 0)
                        getAllOrganizationalPositionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllOrganizationalPositionsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllOrganizationalPositionsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllOrganizationalPositionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfOrganizationalPositions = automationApiBusiness.GetAllOrganizationalPositionsList(
                     ref listCount,
                     getAllOrganizationalPositionsListPVM.ChildsUsersIds,
                     getAllOrganizationalPositionsListPVM.OrganizationalPositionName);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfOrganizationalPositions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetListOfOrganizationalPositions
        /// </summary>
        /// <param name="getListOfOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfOrganizationalPositions([FromBody] GetListOfOrganizationalPositionsPVM getListOfOrganizationalPositionsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfOrganizationalPositionsPVM.ChildsUsersIds == null)
                    {
                        getListOfOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfOrganizationalPositionsPVM.ChildsUsersIds.Count == 0)
                        getListOfOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfOrganizationalPositionsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfOrganizationalPositionsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfOrganizationalPositions = automationApiBusiness.GetListOfOrganizationalPositions(
                   getListOfOrganizationalPositionsPVM.jtStartIndex.Value,
                   getListOfOrganizationalPositionsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfOrganizationalPositionsPVM.OrganizationalPositionName,
                   getListOfOrganizationalPositionsPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfOrganizationalPositions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// AddToOrganizationalPositions
        /// </summary>
        /// <param name="addToOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToOrganizationalPositions([FromBody] AddToOrganizationalPositionsPVM addToOrganizationalPositionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToOrganizationalPositionsPVM.ChildsUsersIds == null)
                    {
                        addToOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToOrganizationalPositionsPVM.ChildsUsersIds.Count == 0)
                        {
                            addToOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToOrganizationalPositionsPVM.ChildsUsersIds.Count == 1)
                                if (addToOrganizationalPositionsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToOrganizationalPositionsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.CreateEnDate = DateTime.Now;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.CreateTime = PersianDate.TimeNow;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.UserIdCreator = this.userId.Value;

                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.CreateEnDate = DateTime.Now;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.CreateTime = PersianDate.TimeNow;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.UserIdCreator = this.userId.Value;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.IsActivated = true;
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.IsDeleted = false;
                }

                int organizationalPositionId = automationApiBusiness.AddToOrganizationalPositions(
                   addToOrganizationalPositionsPVM.OrganizationalPositionsVM);


                if (organizationalPositionId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (organizationalPositionId > 0)
                {
                    addToOrganizationalPositionsPVM.OrganizationalPositionsVM.OrganizationalPositionId = organizationalPositionId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToOrganizationalPositionsPVM.OrganizationalPositionsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateOrganizationalPositions
        /// </summary>
        /// <param name="updateOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("UpdateOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateOrganizationalPositions([FromBody] UpdateOrganizationalPositionsPVM updateOrganizationalPositionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var organizationalPositionsVM = updateOrganizationalPositionsPVM.OrganizationalPositionsVM;

                int organizationalPositionId= automationApiBusiness.UpdateOrganizationalPositions(
                    ref organizationalPositionsVM,
                    updateOrganizationalPositionsPVM.ChildsUsersIds);

                if (organizationalPositionId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (organizationalPositionId > 0)
                {
                    updateOrganizationalPositionsPVM.OrganizationalPositionsVM.OrganizationalPositionId = organizationalPositionId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateOrganizationalPositionsPVM.OrganizationalPositionsVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// ToggleActivationOrganizationalPositions
        /// </summary>
        /// <param name="toggleActivationOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationOrganizationalPositions([FromBody] ToggleActivationOrganizationalPositionsPVM toggleActivationOrganizationalPositionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (automationApiBusiness.ToggleActivationOrganizationalPositions(

                    toggleActivationOrganizationalPositionsPVM.OrganizationalPositionId,
                    toggleActivationOrganizationalPositionsPVM.UserId.Value,
                    toggleActivationOrganizationalPositionsPVM.ChildsUsersIds))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                    {
                        jsonResultObjectVM.Result = returnMessage;
                    }
                    else
                    {
                        jsonResultObjectVM.Result = "OK";
                    }

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);

        }


        /// <summary>
        /// TemporaryDeleteOrganizationalPositions
        /// </summary>
        /// <param name="temporaryDeleteOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteOrganizationalPositions([FromBody] TemporaryDeleteOrganizationalPositionsPVM temporaryDeleteOrganizationalPositionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (automationApiBusiness.TemporaryDeleteOrganizationalPositions(
                    temporaryDeleteOrganizationalPositionsPVM.OrganizationalPositionId,
                    temporaryDeleteOrganizationalPositionsPVM.UserId.Value,
                    temporaryDeleteOrganizationalPositionsPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);

                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }



        /// <summary>
        /// CompleteDeleteOrganizationalPositions
        /// </summary>
        /// <param name="completeDeleteOrganizationalPositionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteOrganizationalPositions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult CompleteDeleteOrganizationalPositions([FromBody] CompleteDeleteOrganizationalPositionsPVM completeDeleteOrganizationalPositionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.CompleteDeleteOrganizationalPositions(
                    completeDeleteOrganizationalPositionsPVM.OrganizationalPositionId,
                    completeDeleteOrganizationalPositionsPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }


        /// <summary>
        /// GetOrganizationalPositionWithOrganizationalPositionId
        /// </summary>
        /// <param name="getOrganizationalPositionWithOrganizationalPositionIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetOrganizationalPositionWithOrganizationalPositionId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrganizationalPositionWithOrganizationalPositionId([FromBody] GetOrganizationalPositionWithOrganizationalPositionIdPVM
            getOrganizationalPositionWithOrganizationalPositionIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds == null)
                    {
                        getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds.Count == 0)
                        getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds.Count == 1)
                        if (getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var organizationalPosition = automationApiBusiness.GetOrganizationalPositionWithOrganizationalPositionId(
                    getOrganizationalPositionWithOrganizationalPositionIdPVM.OrganizationalPositionId,
                    getOrganizationalPositionWithOrganizationalPositionIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = organizationalPosition;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }
    }
}

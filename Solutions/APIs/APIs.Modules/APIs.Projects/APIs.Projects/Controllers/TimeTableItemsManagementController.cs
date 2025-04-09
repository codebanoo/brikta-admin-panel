using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
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
using VM.PVM.Projects;
using System.Linq;
using APIs.TelegramBot.Models.Business;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// TimeTableItemsManagement
    /// </summary>
    public class TimeTableItemsManagementController : ApiBaseController
    {
        /// <summary>
        /// TimeTableItemsManagement
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
        public TimeTableItemsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllTimeTableItemsList
        /// </summary>
        /// <param name="getAllTimeTableItemsList"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllTimeTableItemsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllTimeTableItemsList([FromBody] GetAllTimeTableItemsListPVM getAllTimeTableItemsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllTimeTableItemsListPVM.ChildsUsersIds == null)
                    {
                        getAllTimeTableItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllTimeTableItemsListPVM.ChildsUsersIds.Count == 0)
                        getAllTimeTableItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllTimeTableItemsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllTimeTableItemsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllTimeTableItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfTimeTableItems = projectsApiBusiness.GetAllTimeTableItemsList(
                    getAllTimeTableItemsListPVM.ChildsUsersIds,
                    getAllTimeTableItemsListPVM.TimeTableId,
                    getAllTimeTableItemsListPVM.TimeTableItemParentId,
                    getAllTimeTableItemsListPVM.TimeTableItemTitle
                   );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfTimeTableItems;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                //
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// GetListOfTimeTableItems
        /// </summary>
        /// <param name="getListOfTimeTableItemsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetListOfTimeTableItems([FromBody] GetListOfTimeTableItemsPVM getListOfTimeTableItemsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfTimeTableItemsPVM.ChildsUsersIds == null)
                    {
                        getListOfTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfTimeTableItemsPVM.ChildsUsersIds.Count == 0)
                        getListOfTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfTimeTableItemsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfTimeTableItemsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfTimeTableItems = projectsApiBusiness.GetListOfTimeTableItems(
                    getListOfTimeTableItemsPVM.jtStartIndex.Value,
                    getListOfTimeTableItemsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfTimeTableItemsPVM.ChildsUsersIds,
                    getListOfTimeTableItemsPVM.jtSorting,
                    getListOfTimeTableItemsPVM.TimeTableId,
                    getListOfTimeTableItemsPVM.TimeTableItemParentId,
                    getListOfTimeTableItemsPVM.TimeTableItemTitle
                   );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfTimeTableItems;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// AddToTimeTableItems
        /// </summary>
        /// <param name="addToTimeTableItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToTimeTableItems([FromBody] AddToTimeTableItemsPVM addToTimeTableItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToTimeTableItemsPVM.ChildsUsersIds == null)
                    {
                        addToTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToTimeTableItemsPVM.ChildsUsersIds.Count == 0)
                        addToTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToTimeTableItemsPVM.ChildsUsersIds.Count == 1)
                        if (addToTimeTableItemsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }



                if (projectsApiBusiness.AddToTimeTableItems(addToTimeTableItemsPVM.TimeTableItemsVM))
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToTimeTableItemsPVM.TimeTableItemsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception ex)
            {
                
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        [HttpPost("UpdateTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateTimeTableItems([FromBody] UpdateTimeTableItemsPVM updateTimeTableItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateTimeTableItemsPVM.ChildsUsersIds == null)
                    {
                        updateTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateTimeTableItemsPVM.ChildsUsersIds.Count == 0)
                        updateTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateTimeTableItemsPVM.ChildsUsersIds.Count == 1)
                        if (updateTimeTableItemsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateTimeTableItemsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }



                if (projectsApiBusiness.UpdateTimeTableItems(
                        updateTimeTableItemsPVM.ChildsUsersIds,
                        updateTimeTableItemsPVM.TimeTableItemsVM))
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateTimeTableItemsPVM.TimeTableItemsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception ex)
            {
                
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        [HttpPost("ToggleActivationTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationTimeTableItems(ToggleActivationTimeTableItemsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationTimeTableItems(
                    toggleActivationInvestorsPVM.TimeTableItemId,
                   toggleActivationInvestorsPVM.UserId.Value,
                   toggleActivationInvestorsPVM.ChildsUsersIds
                    ))
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


        [HttpPost("TemporaryDeleteTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteTimeTableItems(TemporaryDeleteTimeTableItemsPVM temporaryDeleteTimeTableItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteTimeTableItems(
                    temporaryDeleteTimeTableItemsPVM.TimeTableItemId,
                   temporaryDeleteTimeTableItemsPVM.UserId.Value,
                   temporaryDeleteTimeTableItemsPVM.ChildsUsersIds
                    ))
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


        [HttpPost("CompleteDeleteTimeTableItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteTimeTableItems(CompleteDeleteTimeTableItemsPVM completeDeleteTimeTableItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            string result = "ErrorInService";

            try
            {
                result = projectsApiBusiness.CompleteDeleteTimeTableItems(
                    completeDeleteTimeTableItemsPVM.TimeTableItemId,
                   completeDeleteTimeTableItemsPVM.UserId.Value,
                   completeDeleteTimeTableItemsPVM.ChildsUsersIds
                    );
                if (result.Equals("OK"))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = result;

            return new JsonResult(jsonResultObjectVM);
        }
    }
}

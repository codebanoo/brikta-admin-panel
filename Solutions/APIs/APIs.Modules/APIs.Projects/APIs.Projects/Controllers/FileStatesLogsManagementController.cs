using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
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

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// FileStatesLogsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class FileStatesLogsManagementController : ApiBaseController
    {
        /// <summary>
        /// FileStatesLogsManagement
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
        /// <param name="_telegramBotApiBusiness"></param>
        /// <param name="_appSettingsSection"></param>
        public FileStatesLogsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFileStatesLogsList
        /// </summary>
        /// <param name="getListOfFileStatesLogsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllFileStatesLogsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllFileStatesLogsList([FromBody] GetAllFileStatesLogsListPVM getAllFileStatesLogsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllFileStatesLogsListPVM.ChildsUsersIds == null)
                    {
                        getAllFileStatesLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllFileStatesLogsListPVM.ChildsUsersIds.Count == 0)
                        getAllFileStatesLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllFileStatesLogsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllFileStatesLogsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllFileStatesLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfFileStatesLogs = projectsApiBusiness.GetAllFileStatesLogsList(
                    getAllFileStatesLogsListPVM.ChildsUsersIds,
                    getAllFileStatesLogsListPVM.TableTitle,
                    getAllFileStatesLogsListPVM.RecordId,
                    getAllFileStatesLogsListPVM.FileStateId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFileStatesLogs;
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
        /// GetListOfFileStatesLogs
        /// </summary>
        /// <param name="getListOfFileStatesLogsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfFileStatesLogs([FromBody] GetListOfFileStatesLogsPVM getListOfFileStatesLogsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfFileStatesLogsPVM.ChildsUsersIds == null)
                    {
                        getListOfFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfFileStatesLogsPVM.ChildsUsersIds.Count == 0)
                        getListOfFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfFileStatesLogsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfFileStatesLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfFileStatesLogs = projectsApiBusiness.GetListOfFileStatesLogs(
                   getListOfFileStatesLogsPVM.jtStartIndex.Value,
                   getListOfFileStatesLogsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfFileStatesLogsPVM.ChildsUsersIds,
                   getListOfFileStatesLogsPVM.TableTitle,
                   getListOfFileStatesLogsPVM.RecordId,
                   getListOfFileStatesLogsPVM.FileStateId,
                   getListOfFileStatesLogsPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFileStatesLogs;
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
        /// AddToFileStatesLogs
        /// </summary>
        /// <param name="addToFileStatesLogsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToFileStatesLogs([FromBody] AddToFileStatesLogsPVM addToFileStatesLogsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToFileStatesLogsPVM.ChildsUsersIds == null)
                    {
                        addToFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToFileStatesLogsPVM.ChildsUsersIds.Count == 0)
                        addToFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToFileStatesLogsPVM.ChildsUsersIds.Count == 1)
                        if (addToFileStatesLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var fileStateLogId = projectsApiBusiness.AddToFileStatesLogs(addToFileStatesLogsPVM.FileStatesLogsVM);
                if (fileStateLogId != 0)
                {
                    addToFileStatesLogsPVM.FileStatesLogsVM.FileStateLogId = fileStateLogId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToFileStatesLogsPVM.FileStatesLogsVM;

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

        /// <summary>
        /// UpdateFileStatesLogs
        /// </summary>
        /// <param name="updateFileStatesLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("UpdateFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateFileStatesLogs([FromBody] UpdateFileStatesLogsPVM updateFileStatesLogsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateFileStatesLogsPVM.ChildsUsersIds == null)
                    {
                        updateFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateFileStatesLogsPVM.ChildsUsersIds.Count == 0)
                        updateFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateFileStatesLogsPVM.ChildsUsersIds.Count == 1)
                        if (updateFileStatesLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateFileStatesLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedFileStatesLogsVM = projectsApiBusiness.UpdateFileStatesLogs(
                    updateFileStatesLogsPVM.ChildsUsersIds,
                    updateFileStatesLogsPVM.FileStatesLogsVM);
                if (updatedFileStatesLogsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedFileStatesLogsVM;

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

        /// <summary>
        /// ToggleActivationFileStatesLogs
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns></returns>
        [HttpPost("ToggleActivationFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationFileStatesLogs(ToggleActivationFileStatesLogsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationFileStatesLogs(
                    toggleActivationInvestorsPVM.FileStateLogId,
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

        /// <summary>
        /// TemporaryDeleteFileStatesLogs
        /// </summary>
        /// <param name="temporaryDeleteFileStatesLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("TemporaryDeleteFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteFileStatesLogs(TemporaryDeleteFileStatesLogsPVM temporaryDeleteFileStatesLogsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteFileStatesLogs(
                    temporaryDeleteFileStatesLogsPVM.FileStateLogId,
                   temporaryDeleteFileStatesLogsPVM.UserId.Value,
                   temporaryDeleteFileStatesLogsPVM.ChildsUsersIds
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

        /// <summary>
        /// CompleteDeleteFileStatesLogs
        /// </summary>
        /// <param name="completeDeleteFileStatesLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("CompleteDeleteFileStatesLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteFileStatesLogs(CompleteDeleteFileStatesLogsPVM completeDeleteFileStatesLogsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;
            try
            {
                result = projectsApiBusiness.CompleteDeleteFileStatesLogs(
                    completeDeleteFileStatesLogsPVM.FileStateLogId,
                   completeDeleteFileStatesLogsPVM.UserId.Value,
                   completeDeleteFileStatesLogsPVM.ChildsUsersIds);
                if (result.HasValue && result.Value == true)
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            if (result == null)
            {
                jsonResultObjectVM.Message = "ERROR_DEPENDENCY";
            }
            else
                jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// GetFileStateLogWithFileStateLogId
        /// </summary>
        /// <param name="getFileStateLogWithFileStateLogIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetFileStateLogWithFileStateLogId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetFileStateLogWithFileStateLogId(
            [FromBody] GetFileStateLogWithFileStateLogIdPVM getFileStateLogWithFileStateLogIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds == null)
                    {
                        getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds.Count == 0)
                        getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds.Count == 1)
                        if (getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfFileStatesLogs = projectsApiBusiness.GetFileStateLogWithFileStateLogId(
                        getFileStateLogWithFileStateLogIdPVM.FileStateLogId,
                        getFileStateLogWithFileStateLogIdPVM.UserId.Value,
                        getFileStateLogWithFileStateLogIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfFileStatesLogs;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }
    }
}

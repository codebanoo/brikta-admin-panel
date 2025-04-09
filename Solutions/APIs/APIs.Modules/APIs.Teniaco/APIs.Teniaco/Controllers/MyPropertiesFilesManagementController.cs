using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Business;
using FrameWork;
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
using VM.PVM.Teniaco;
using System.Linq;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesFilesManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesFilesManagement
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
        public MyPropertiesFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMyPropertyFilesList
        /// </summary>
        /// <param name="getAllMyPropertyFilesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyPropertyFilesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyPropertyFilesList([FromBody] GetAllMyPropertyFilesListPVM getAllMyPropertyFilesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyPropertyFilesListPVM.ChildsUsersIds == null)
                    {
                        getAllMyPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyPropertyFilesListPVM.ChildsUsersIds.Count == 0)
                        getAllMyPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyPropertyFilesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyPropertyFilesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfMyPropertyFiles = teniacoApiBusiness.GetAllMyPropertyFilesList(
                    ref listCount,
                    getAllMyPropertyFilesListPVM.ChildsUsersIds,
                    getAllMyPropertyFilesListPVM.PropertyId,
                    getAllMyPropertyFilesListPVM.PropertyFileTitle,
                    getAllMyPropertyFilesListPVM.PropertyFileType,
                    getAllMyPropertyFilesListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyPropertyFiles;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// GetListOfMyPropertyFiles
        /// </summary>
        /// <param name="getListOfMyPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyPropertyFiles([FromBody] GetListOfMyPropertyFilesPVM
            getListOfMyPropertyFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfPropertyFiles = teniacoApiBusiness.GetListOfMyPropertyFiles(
                    getListOfMyPropertyFilesPVM.jtStartIndex.Value,
                    getListOfMyPropertyFilesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyPropertyFilesPVM.ChildsUsersIds,
                    getListOfMyPropertyFilesPVM.PropertyId,
                    getListOfMyPropertyFilesPVM.PropertyFileTitle,
                    getListOfMyPropertyFilesPVM.PropertyFileType,
                    getListOfMyPropertyFilesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPropertyFiles;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }
        [HttpPost("AddToMyPropertyFilesReg")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyPropertyFilesReg([FromBody] AddToMyPropertyFilesPVM
           addToMyPropertyFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMyPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToMyPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToMyPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToMyPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var propertyFilesVM in addToMyPropertyFilesPVM.MyPropertyFilesVMList)
                    {
                        propertyFilesVM.CreateEnDate = DateTime.Now;
                        propertyFilesVM.CreateTime = PersianDate.TimeNow;
                        propertyFilesVM.UserIdCreator = this.userId.Value;
                        propertyFilesVM.IsActivated = true;
                        propertyFilesVM.IsDeleted = false;
                    }
                }

                var _result = teniacoApiBusiness.AddToMyPropertyFilesReg(addToMyPropertyFilesPVM.MyPropertyFilesVMList, addToMyPropertyFilesPVM.CurrentUserId.Value);
                if (_result.IsSuccess)
                {
                    jsonResultWithRecordsObjectVM.Records = _result.finalResult;
                    jsonResultWithRecordsObjectVM.Result = "OK";
                    return new JsonResult(jsonResultWithRecordsObjectVM);
                };

                
                
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// AddToMyPropertyFiles
        /// </summary>
        /// <param name="addToMyPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyPropertyFiles([FromBody] AddToMyPropertyFilesPVM
            addToMyPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMyPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToMyPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToMyPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToMyPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var propertyFilesVM in addToMyPropertyFilesPVM.MyPropertyFilesVMList)
                    {
                        propertyFilesVM.CreateEnDate = DateTime.Now;
                        propertyFilesVM.CreateTime = PersianDate.TimeNow;
                        propertyFilesVM.UserIdCreator = this.userId.Value;
                        propertyFilesVM.IsActivated = true;
                        propertyFilesVM.IsDeleted = false;
                    }
                }

                if (teniacoApiBusiness.AddToMyPropertyFiles(
                    addToMyPropertyFilesPVM.MyPropertyFilesVMList))
                {
                    jsonResultObjectVM.Result = "OK";

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
        /// GetMyPropertyFileWithMyPropertyFileId
        /// </summary>
        /// <param name="getMyPropertyFileWithMyPropertyFileIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetMyPropertyFileWithMyPropertyFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyPropertyFileWithMyPropertyFileId([FromBody] GetMyPropertyFileWithMyPropertyFileIdPVM
            getMyPropertyFileWithMyPropertyFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds == null)
                    {
                        getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds.Count == 0)
                        getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFiles = teniacoApiBusiness.GetMyPropertyFileWithMyPropertyFileId(
                   getMyPropertyFileWithMyPropertyFileIdPVM.PropertyFileId,
                   getMyPropertyFileWithMyPropertyFileIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = propertyFiles;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// UpdateMyPropertyFiles
        /// </summary>
        /// <param name="updateMyPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyPropertyFiles([FromBody] UpdateMyPropertyFilesPVM
            updateMyPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMyPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        updateMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateMyPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        updateMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateMyPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (updateMyPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMyPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                   updateMyPropertyFilesPVM.MyPropertyFilesVM.EditEnDate = DateTime.Now;
                   updateMyPropertyFilesPVM.MyPropertyFilesVM.EditTime = PersianDate.TimeNow;
                   updateMyPropertyFilesPVM.MyPropertyFilesVM.UserIdEditor = this.userId.Value;
                }

                var propertyFilesVM = updateMyPropertyFilesPVM.MyPropertyFilesVM;

                if (teniacoApiBusiness.UpdateMyPropertyFiles(
                    ref propertyFilesVM,
                    updateMyPropertyFilesPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";
                }

                return new JsonResult(jsonResultObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// ToggleActivationMyPropertyFiles
        /// </summary>
        /// <param name="toggleActivationMyPropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMyPropertyFiles([FromBody] ToggleActivationMyPropertyFilesPVM
            toggleActivationMyPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationMyPropertyFiles(
                    toggleActivationMyPropertyFilesPVM.PropertyFileId,
                    toggleActivationMyPropertyFilesPVM.UserId.Value,
                    toggleActivationMyPropertyFilesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteMyPropertyFiles
        /// </summary>
        /// <param name="temporaryDeleteMyPropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMyPropertyFiles([FromBody] TemporaryDeleteMyPropertyFilesPVM
            temporaryDeleteMyPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteMyPropertyFiles(
                    temporaryDeleteMyPropertyFilesPVM.PropertyFileId,
                    temporaryDeleteMyPropertyFilesPVM.UserId.Value,
                    temporaryDeleteMyPropertyFilesPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

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
        /// CompleteDeleteMyPropertyFilesPVM
        /// </summary>
        /// <param name="completeDeleteMyPropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyPropertyFiles([FromBody] CompleteDeleteMyPropertyFilesPVM completeDeleteMyPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteMyPropertyFiles(
                    completeDeleteMyPropertyFilesPVM.PropertyFileId,
                    completeDeleteMyPropertyFilesPVM.ChildsUsersIds))
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
    }
}

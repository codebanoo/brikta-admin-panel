using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
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
using FrameWork;
using System.Linq;
using VM.PVM.Public;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{


    /// <summary>
    /// ZoneFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ZoneFilesManagementController : ApiBaseController
    {

        /// <summary>
        /// ZoneFilesManagement
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
        public ZoneFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfZoneFiles
        /// </summary>
        /// <param name="getListOfZoneFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfZoneFiles([FromBody] GetListOfZoneFilesPVM
            getListOfZoneFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfZoneFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfZoneFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfZoneFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfZoneFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfZoneFiles = publicApiBusiness.GetListOfZoneFiles(
                    getListOfZoneFilesPVM.jtStartIndex.Value,
                    getListOfZoneFilesPVM.jtPageSize.Value,
                    ref listCount,
                   getListOfZoneFilesPVM.ChildsUsersIds,
                   getListOfZoneFilesPVM.ZoneId,
                   getListOfZoneFilesPVM.ZoneFileTitle,
                   getListOfZoneFilesPVM.ZoneFileType,
                   getListOfZoneFilesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfZoneFiles;
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
        /// AddToZoneFiles
        /// </summary>
        /// <param name="addToZoneFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        /// 

        [HttpPost("AddToZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToZoneFiles([FromBody] AddToZoneFilesPVM
            addToZoneFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToZoneFilesPVM.ChildsUsersIds == null)
                    {
                        addToZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToZoneFilesPVM.ChildsUsersIds.Count == 0)
                        addToZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToZoneFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToZoneFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var ZoneFilesVM in addToZoneFilesPVM.ZoneFilesVM)
                    {
                        ZoneFilesVM.CreateEnDate = DateTime.Now;
                        ZoneFilesVM.CreateTime = PersianDate.TimeNow;
                        ZoneFilesVM.UserIdCreator = this.userId.Value;
                        ZoneFilesVM.IsActivated = true;
                        ZoneFilesVM.IsDeleted = false;
                    }
                }

                if (publicApiBusiness.AddToZoneFiles(
                    addToZoneFilesPVM.ZoneFilesVM))
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
        /// GetZoneFileWithZoneFileId
        /// </summary>
        /// <param name="getZoneFileWithZoneFileIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetZoneFileWithZoneFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetZoneFileWithZoneFileId([FromBody] GetZoneFileWithZoneFileIdPVM
            getZoneFileWithZoneFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getZoneFileWithZoneFileIdPVM.ChildsUsersIds == null)
                    {
                        getZoneFileWithZoneFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getZoneFileWithZoneFileIdPVM.ChildsUsersIds.Count == 0)
                        getZoneFileWithZoneFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getZoneFileWithZoneFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getZoneFileWithZoneFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getZoneFileWithZoneFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFiles = publicApiBusiness.GetZoneFileWithZoneFileId(
                    getZoneFileWithZoneFileIdPVM.ZoneFileId,
                    getZoneFileWithZoneFileIdPVM.ChildsUsersIds);

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
        /// UpdateZoneFiles
        /// </summary>
        /// <param name="updateZoneFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateZoneFiles([FromBody] UpdateZoneFilesPVM
            updateZoneFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateZoneFilesPVM.ChildsUsersIds == null)
                    {
                        updateZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateZoneFilesPVM.ChildsUsersIds.Count == 0)
                        updateZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateZoneFilesPVM.ChildsUsersIds.Count == 1)
                        if (updateZoneFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateZoneFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateZoneFilesPVM.ZoneFilesVM.EditEnDate = DateTime.Now;
                    updateZoneFilesPVM.ZoneFilesVM.EditTime = PersianDate.TimeNow;
                    updateZoneFilesPVM.ZoneFilesVM.UserIdEditor = this.userId.Value;
                }

                var ZoneFilesVM = updateZoneFilesPVM.ZoneFilesVM;

                if (publicApiBusiness.UpdateZoneFiles(
                    ref ZoneFilesVM,
                    updateZoneFilesPVM.ChildsUsersIds))
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
        /// ToggleActivationZoneFiles
        /// </summary>
        /// <param name="toggleActivationZoneFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationZoneFiles([FromBody] ToggleActivationZoneFilesPVM
            toggleActivationZoneFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationZoneFiles(
                    toggleActivationZoneFilesPVM.ZoneFileId,
                    toggleActivationZoneFilesPVM.UserId.Value,
                    toggleActivationZoneFilesPVM.ChildsUsersIds))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                        jsonResultObjectVM.Result = returnMessage;
                    else
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
        /// TemporaryDeleteZoneFiles
        /// </summary>
        /// <param name="temporaryDeleteZoneFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteZoneFiles([FromBody] TemporaryDeleteZoneFilesPVM
            temporaryDeleteZoneFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteZoneFiles(
                    temporaryDeleteZoneFilesPVM.ZoneFileId,
                    temporaryDeleteZoneFilesPVM.UserId.Value,
                    temporaryDeleteZoneFilesPVM.ChildsUsersIds))
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
        /// CompleteDeleteZoneFiles
        /// </summary>
        /// <param name="completeDeleteZoneFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteZoneFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteZoneFiles([FromBody] CompleteDeleteZoneFilesPVM completeDeleteZoneFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteZoneFiles(
                    completeDeleteZoneFilesPVM.ZoneFileId,
                    completeDeleteZoneFilesPVM.ChildsUsersIds))
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

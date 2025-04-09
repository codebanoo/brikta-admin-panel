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
    /// DistrictFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class DistrictFilesManagementController : ApiBaseController
    {

        /// <summary>
        /// DistrictFilesManagement
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
        public DistrictFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfDistrictFiles
        /// </summary>
        /// <param name="getListOfDistrictFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfDistrictFiles([FromBody] GetListOfDistrictFilesPVM
            getListOfDistrictFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfDistrictFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfDistrictFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfDistrictFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfDistrictFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfDistrictFiles = publicApiBusiness.GetListOfDistrictFiles(
                    getListOfDistrictFilesPVM.jtStartIndex.Value,
                    getListOfDistrictFilesPVM.jtPageSize.Value,
                    ref listCount,
                   getListOfDistrictFilesPVM.ChildsUsersIds,
                   getListOfDistrictFilesPVM.DistrictId,
                   getListOfDistrictFilesPVM.DistrictFileTitle,
                   getListOfDistrictFilesPVM.DistrictFileType,
                   getListOfDistrictFilesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDistrictFiles;
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
        /// AddToDistrictFiles
        /// </summary>
        /// <param name="addToDistrictFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        /// 

        [HttpPost("AddToDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToDistrictFiles([FromBody] AddToDistrictFilesPVM
            addToDistrictFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToDistrictFilesPVM.ChildsUsersIds == null)
                    {
                        addToDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToDistrictFilesPVM.ChildsUsersIds.Count == 0)
                        addToDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToDistrictFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToDistrictFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var districtFilesVM in addToDistrictFilesPVM.DistrictFilesVM)
                    {
                        districtFilesVM.CreateEnDate = DateTime.Now;
                        districtFilesVM.CreateTime = PersianDate.TimeNow;
                        districtFilesVM.UserIdCreator = this.userId.Value;
                        districtFilesVM.IsActivated = true;
                        districtFilesVM.IsDeleted = false;
                    }
                }

                if (publicApiBusiness.AddToDistrictFiles(
                    addToDistrictFilesPVM.DistrictFilesVM))
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
        /// GetDistrictFileWithDistrictFileId
        /// </summary>
        /// <param name="getDistrictFileWithDistrictFileIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetDistrictFileWithDistrictFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetDistrictFileWithDistrictFileId([FromBody] GetDistrictFileWithDistrictFileIdPVM
            getDistrictFileWithDistrictFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds == null)
                    {
                        getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds.Count == 0)
                        getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFiles = publicApiBusiness.GetDistrictFileWithDistrictFileId(
                    getDistrictFileWithDistrictFileIdPVM.DistrictFileId,
                    getDistrictFileWithDistrictFileIdPVM.ChildsUsersIds);

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
        /// UpdateDistrictFiles
        /// </summary>
        /// <param name="updateDistrictFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateDistrictFiles([FromBody] UpdateDistrictFilesPVM
            updateDistrictFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateDistrictFilesPVM.ChildsUsersIds == null)
                    {
                        updateDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateDistrictFilesPVM.ChildsUsersIds.Count == 0)
                        updateDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateDistrictFilesPVM.ChildsUsersIds.Count == 1)
                        if (updateDistrictFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateDistrictFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateDistrictFilesPVM.DistrictFilesVM.EditEnDate = DateTime.Now;
                    updateDistrictFilesPVM.DistrictFilesVM.EditTime = PersianDate.TimeNow;
                    updateDistrictFilesPVM.DistrictFilesVM.UserIdEditor = this.userId.Value;
                }

                var districtFilesVM = updateDistrictFilesPVM.DistrictFilesVM;

                if (publicApiBusiness.UpdateDistrictFiles(
                    ref districtFilesVM,
                    updateDistrictFilesPVM.ChildsUsersIds))
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
        /// ToggleActivationDistrictFiles
        /// </summary>
        /// <param name="toggleActivationDistrictFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationDistrictFiles([FromBody] ToggleActivationDistrictFilesPVM
            toggleActivationDistrictFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationDistrictFiles(
                    toggleActivationDistrictFilesPVM.DistrictFileId,
                    toggleActivationDistrictFilesPVM.UserId.Value,
                    toggleActivationDistrictFilesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteDistrictFiles
        /// </summary>
        /// <param name="temporaryDeleteDistrictFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteDistrictFiles([FromBody] TemporaryDeleteDistrictFilesPVM
            temporaryDeleteDistrictFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteDistrictFiles(
                    temporaryDeleteDistrictFilesPVM.DistrictFileId,
                    temporaryDeleteDistrictFilesPVM.UserId.Value,
                    temporaryDeleteDistrictFilesPVM.ChildsUsersIds))
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
        /// CompleteDeleteDistrictFiles
        /// </summary>
        /// <param name="completeDeleteDistrictFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteDistrictFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteDistrictFiles([FromBody] CompleteDeleteDistrictFilesPVM completeDeleteDistrictFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteDistrictFiles(
                    completeDeleteDistrictFilesPVM.DistrictFileId,
                    completeDeleteDistrictFilesPVM.ChildsUsersIds))
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

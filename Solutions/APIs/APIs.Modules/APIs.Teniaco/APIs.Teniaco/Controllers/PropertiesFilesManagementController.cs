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
using System.Collections.Generic;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// PropertiesFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PropertiesFilesManagementController : ApiBaseController
    {
        /// <summary>
        /// PropertiesFilesManagement
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
        public PropertiesFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllPropertyFilesList
        /// </summary>
        /// <param name="getAllPropertyFilesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPropertyFilesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPropertyFilesList([FromBody] GetAllPropertyFilesListPVM getAllPropertyFilesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPropertyFilesListPVM.ChildsUsersIds == null)
                    {
                        getAllPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPropertyFilesListPVM.ChildsUsersIds.Count == 0)
                        getAllPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPropertyFilesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPropertyFilesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPropertyFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfPropertyFiles = teniacoApiBusiness.GetAllPropertyFilesList(
                    ref listCount,
                    getAllPropertyFilesListPVM.ChildsUsersIds,
                    getAllPropertyFilesListPVM.PropertyId,
                    getAllPropertyFilesListPVM.PropertyFileTitle,
                    getAllPropertyFilesListPVM.PropertyFileType,
                    getAllPropertyFilesListPVM.jtSorting);

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
            //return jsonResultWithRecordsObjectVM;
        }


        /// <summary>
        /// GetListOfPropertyFiles
        /// </summary>
        /// <param name="getListOfPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPropertyFiles([FromBody] GetListOfPropertyFilesPVM
            getListOfPropertyFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfPropertyFiles = teniacoApiBusiness.GetListOfPropertyFiles(
                    getListOfPropertyFilesPVM.jtStartIndex.Value,
                    getListOfPropertyFilesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfPropertyFilesPVM.ChildsUsersIds,
                    getListOfPropertyFilesPVM.PropertyId,
                    getListOfPropertyFilesPVM.PropertyFileTitle,
                    getListOfPropertyFilesPVM.PropertyFileType,
                    getListOfPropertyFilesPVM.jtSorting);

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

        /// <summary>
        /// AddToPropertyFiles
        /// </summary>
        /// <param name="addToPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToPropertyFiles([FromBody] AddToPropertyFilesPVM
            addToPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToPropertyFilesPVM.ChildsUsersIds == null)
                    {
                        addToPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToPropertyFilesPVM.ChildsUsersIds.Count == 0)
                        addToPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToPropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToPropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToPropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var propertyFilesVM in addToPropertyFilesPVM.PropertyFilesVMList)
                    {
                        propertyFilesVM.CreateEnDate = DateTime.Now;
                        propertyFilesVM.CreateTime = PersianDate.TimeNow;
                        propertyFilesVM.UserIdCreator = this.userId.Value;
                        propertyFilesVM.IsActivated = true;
                        propertyFilesVM.IsDeleted = false;
                    }
                }

                if (teniacoApiBusiness.AddToPropertyFiles(
                    addToPropertyFilesPVM.PropertyFilesVMList,
                    addToPropertyFilesPVM.DeletedPhotosIDs,
                    addToPropertyFilesPVM.IsMainChanged,
                    addToPropertyFilesPVM.PropertyId))
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
        /// GetPropertyFileWithPropertyFileId
        /// </summary>
        /// <param name="getPropertyFileWithPropertyFileIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetPropertyFileWithPropertyFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPropertyFileWithPropertyFileId([FromBody] GetPropertyFileWithPropertyFileIdPVM
            getPropertyFileWithPropertyFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds == null)
                    {
                        getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds.Count == 0)
                        getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFiles = teniacoApiBusiness.GetPropertyFileWithPropertyFileId(
                    getPropertyFileWithPropertyFileIdPVM.PropertyFileId,
                    getPropertyFileWithPropertyFileIdPVM.ChildsUsersIds);

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
        [HttpPost("UpdateListPropertyFilesTitle")]
        [ProducesResponseType(typeof(JsonResultObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateListPropertyFilesTitle([FromBody] List<UpdateListPropertyFilesTitlePVM> lst)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            UpdateListPropertyFilesTitlePVM d = new UpdateListPropertyFilesTitlePVM();
            try
            {
                
                if (teniacoApiBusiness.UpdateListPropertyFilesTitle(lst, this.userId.Value, PersianDate.TimeNow))
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
        /// UpdatePropertyFiles
        /// </summary>
        /// <param name="updatePropertiesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdatePropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdatePropertyFiles([FromBody] UpdatePropertyFilesPVM
            updatePropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updatePropertyFilesPVM.ChildsUsersIds == null)
                    {
                        updatePropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updatePropertyFilesPVM.ChildsUsersIds.Count == 0)
                        updatePropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updatePropertyFilesPVM.ChildsUsersIds.Count == 1)
                        if (updatePropertyFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updatePropertyFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updatePropertyFilesPVM.PropertyFilesVM.EditEnDate = DateTime.Now;
                    updatePropertyFilesPVM.PropertyFilesVM.EditTime = PersianDate.TimeNow;
                    updatePropertyFilesPVM.PropertyFilesVM.UserIdEditor = this.userId.Value;
                }

                var propertyFilesVM = updatePropertyFilesPVM.PropertyFilesVM;

                if (teniacoApiBusiness.UpdatePropertyFiles(
                    ref propertyFilesVM,
                    updatePropertyFilesPVM.ChildsUsersIds))
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
        /// ToggleActivationPropertyFiles
        /// </summary>
        /// <param name="toggleActivationPropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationPropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationPropertyFiles([FromBody] ToggleActivationPropertyFilesPVM
            toggleActivationPropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationPropertyFiles(
                    toggleActivationPropertyFilesPVM.PropertyFileId,
                    toggleActivationPropertyFilesPVM.UserId.Value,
                    toggleActivationPropertyFilesPVM.ChildsUsersIds))
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
        /// TemporaryDeletePropertyFiles
        /// </summary>
        /// <param name="temporaryDeletePropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeletePropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePropertyFiles([FromBody] TemporaryDeletePropertyFilesPVM
            temporaryDeletePropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeletePropertyFiles(
                    temporaryDeletePropertyFilesPVM.PropertyFileId,
                    temporaryDeletePropertyFilesPVM.UserId.Value,
                    temporaryDeletePropertyFilesPVM.ChildsUsersIds))
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
        /// CompleteDeletePropertyFiles
        /// </summary>
        /// <param name="completeDeletePropertyFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeletePropertyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeletePropertyFiles([FromBody] CompleteDeletePropertyFilesPVM completeDeletePropertyFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeletePropertyFiles(
                    completeDeletePropertyFilesPVM.PropertyFileId,
                    completeDeletePropertyFilesPVM.ChildsUsersIds))
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

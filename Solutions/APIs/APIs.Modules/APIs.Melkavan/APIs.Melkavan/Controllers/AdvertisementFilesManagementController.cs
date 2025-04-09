using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
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

using System.Linq;
using System.Collections.Generic;
using APIs.Melkavan.Models.Business;
using VM.PVM.Melkavan;
using APIs.Teniaco.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementFilesManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementFilesManagement
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
        public AdvertisementFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllAdvertisementFilesList
        /// </summary>
        /// <param name="getAllAdvertisementFilesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllAdvertisementFilesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAdvertisementFilesList([FromBody] GetAllAdvertisementFilesListPVM getAllAdvertisementFilesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAdvertisementFilesListPVM.ChildsUsersIds == null)
                    {
                        getAllAdvertisementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllAdvertisementFilesListPVM.ChildsUsersIds.Count == 0)
                        getAllAdvertisementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllAdvertisementFilesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllAdvertisementFilesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAdvertisementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAdvertisementFiles = melkavanApiBusiness.GetAllAdvertisementFilesList(
                    ref listCount,
                    getAllAdvertisementFilesListPVM.ChildsUsersIds,
                    getAllAdvertisementFilesListPVM.AdvertisementId,
                    getAllAdvertisementFilesListPVM.AdvertisementFileTitle,
                    getAllAdvertisementFilesListPVM.AdvertisementFileType,
                    getAllAdvertisementFilesListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisementFiles;
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
        /// GetListOfAdvertisementFiles
        /// </summary>
        /// <param name="getListOfAdvertisementFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAdvertisementFiles([FromBody] GetListOfAdvertisementFilesPVM
            getListOfAdvertisementFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAdvertisementFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAdvertisementFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAdvertisementFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAdvertisementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAdvertisementFiles = melkavanApiBusiness.GetListOfAdvertisementFiles(
                    getListOfAdvertisementFilesPVM.jtStartIndex.Value,
                    getListOfAdvertisementFilesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfAdvertisementFilesPVM.ChildsUsersIds,
                    getListOfAdvertisementFilesPVM.AdvertisementId,
                    getListOfAdvertisementFilesPVM.AdvertisementFileTitle,
                    getListOfAdvertisementFilesPVM.AdvertisementFileType,
                    getListOfAdvertisementFilesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisementFiles;
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
        /// AddToAdvertisementFiles
        /// </summary>
        /// <param name="addToAdvertisementFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAdvertisementFiles([FromBody] AddToAdvertisementFilesPVM
            addToAdvertisementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAdvertisementFilesPVM.ChildsUsersIds == null)
                    {
                        addToAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToAdvertisementFilesPVM.ChildsUsersIds.Count == 0)
                        addToAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToAdvertisementFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToAdvertisementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var AdvertisementFilesVM in addToAdvertisementFilesPVM.AdvertisementFilesVMList)
                    {
                        AdvertisementFilesVM.CreateEnDate = DateTime.Now;
                        AdvertisementFilesVM.CreateTime = PersianDate.TimeNow;
                        AdvertisementFilesVM.UserIdCreator = this.userId.Value;
                        AdvertisementFilesVM.IsActivated = true;
                        AdvertisementFilesVM.IsDeleted = false;
                    }
                }

                if (melkavanApiBusiness.AddToAdvertisementFiles(
                    addToAdvertisementFilesPVM.AdvertisementFilesVMList,
                    addToAdvertisementFilesPVM.DeletedPhotosIDs,
                    addToAdvertisementFilesPVM.IsMainChanged,
                    addToAdvertisementFilesPVM.AdvertisementId,
                    addToAdvertisementFilesPVM.ChildsUsersIds))
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
        /// UpdateAdvertisementFiles
        /// </summary>
        /// <param name="updateAdvertisementFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisementFiles([FromBody] UpdateAdvertisementFilesPVM
            updateAdvertisementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAdvertisementFilesPVM.ChildsUsersIds == null)
                    {
                        updateAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateAdvertisementFilesPVM.ChildsUsersIds.Count == 0)
                        updateAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateAdvertisementFilesPVM.ChildsUsersIds.Count == 1)
                        if (updateAdvertisementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAdvertisementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateAdvertisementFilesPVM.AdvertisementFilesVM.EditEnDate = DateTime.Now;
                    updateAdvertisementFilesPVM.AdvertisementFilesVM.EditTime = PersianDate.TimeNow;
                    updateAdvertisementFilesPVM.AdvertisementFilesVM.UserIdEditor = this.userId.Value;
                }

                var AdvertisementFilesVM = updateAdvertisementFilesPVM.AdvertisementFilesVM;

                if (melkavanApiBusiness.UpdateAdvertisementFiles(
                    ref AdvertisementFilesVM,
                    updateAdvertisementFilesPVM.ChildsUsersIds))
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
        /// ToggleActivationAdvertisementFiles
        /// </summary>
        /// <param name="toggleActivationAdvertisementFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationAdvertisementFiles([FromBody] ToggleActivationAdvertisementFilesPVM
            toggleActivationAdvertisementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (melkavanApiBusiness.ToggleActivationAdvertisementFiles(
                    toggleActivationAdvertisementFilesPVM.AdvertisementFileId,
                    toggleActivationAdvertisementFilesPVM.UserId.Value,
                    toggleActivationAdvertisementFilesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteAdvertisementFiles
        /// </summary>
        /// <param name="temporaryDeleteAdvertisementFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteAdvertisementFiles([FromBody] TemporaryDeleteAdvertisementFilesPVM
            temporaryDeleteAdvertisementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (melkavanApiBusiness.TemporaryDeleteAdvertisementFiles(
                    temporaryDeleteAdvertisementFilesPVM.AdvertisementFileId,
                    temporaryDeleteAdvertisementFilesPVM.UserId.Value,
                    temporaryDeleteAdvertisementFilesPVM.ChildsUsersIds))
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
        /// CompleteDeleteAdvertisementFiles
        /// </summary>
        /// <param name="completeDeleteAdvertisementFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteAdvertisementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteAdvertisementFiles([FromBody] CompleteDeleteAdvertisementFilesPVM completeDeleteAdvertisementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (melkavanApiBusiness.CompleteDeleteAdvertisementFiles(
                    completeDeleteAdvertisementFilesPVM.AdvertisementFileId,
                    completeDeleteAdvertisementFilesPVM.ChildsUsersIds))
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
        /// GetAdvertisementFileWithAdvertisementFileId
        /// </summary>
        /// <param name="getAdvertisementFileWithAdvertisementFileIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetAdvertisementFileWithAdvertisementFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertisementFileWithAdvertisementFileId([FromBody] GetAdvertisementFileWithAdvertisementFileIdPVM
            getAdvertisementFileWithAdvertisementFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var AdvertisementFiles = melkavanApiBusiness.GetAdvertisementFileWithAdvertisementFileId(
                    getAdvertisementFileWithAdvertisementFileIdPVM.AdvertisementFileId,
                    getAdvertisementFileWithAdvertisementFileIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = AdvertisementFiles;

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

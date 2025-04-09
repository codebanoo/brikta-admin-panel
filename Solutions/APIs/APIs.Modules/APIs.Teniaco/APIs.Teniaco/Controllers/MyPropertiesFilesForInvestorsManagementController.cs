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
using VM.PVM.Teniaco;
using System.Linq;
using FrameWork;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesFilesForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesFilesForInvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesFilesForInvestorsManagement
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
        public MyPropertiesFilesForInvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMyPropertyFilesForInvestorsList
        /// </summary>
        /// <param name="getAllMyPropertyFilesForInvestorsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyPropertyFilesForInvestorsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyPropertyFilesList([FromBody] GetAllMyPropertyFilesForInvestorsListPVM getAllMyPropertyFilesForInvestorsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds == null)
                    {
                        getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds.Count == 0)
                        getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfMyPropertyFiles = teniacoApiBusiness.GetAllMyPropertyFilesList(
                    ref listCount,
                    getAllMyPropertyFilesForInvestorsListPVM.ChildsUsersIds,
                    getAllMyPropertyFilesForInvestorsListPVM.PropertyId,
                    getAllMyPropertyFilesForInvestorsListPVM.PropertyFileTitle,
                    getAllMyPropertyFilesForInvestorsListPVM.PropertyFileType,
                    getAllMyPropertyFilesForInvestorsListPVM.jtSorting);

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
        /// GetListOfMyPropertyFilesForInvestors
        /// </summary>
        /// <param name="getListOfMyPropertyFilesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyPropertyFilesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyPropertyFilesForInvestors([FromBody] GetListOfMyPropertyFilesForInvestorsPVM
            getListOfMyPropertyFilesForInvestorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfPropertyFiles = teniacoApiBusiness.GetListOfMyPropertyFilesForInvestors(
                    getListOfMyPropertyFilesForInvestorsPVM.jtStartIndex.Value,
                    getListOfMyPropertyFilesForInvestorsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyPropertyFilesForInvestorsPVM.ChildsUsersIds,
                    getListOfMyPropertyFilesForInvestorsPVM.PropertyId,
                    getListOfMyPropertyFilesForInvestorsPVM.PropertyFileTitle,
                    getListOfMyPropertyFilesForInvestorsPVM.PropertyFileType);

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
        /// AddToMyPropertyFilesForInvestors
        /// </summary>
        /// <param name="addToMyPropertyFilesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyPropertyFilesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyPropertyFilesForInvestors([FromBody] AddToMyPropertyFilesForInvestorsPVM
            addToMyPropertyFilesForInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    foreach (var propertyFilesVM in addToMyPropertyFilesForInvestorsPVM.PropertyFilesVMList)
                    {
                        propertyFilesVM.CreateEnDate = DateTime.Now;
                        propertyFilesVM.CreateTime = PersianDate.TimeNow;
                        propertyFilesVM.UserIdCreator = this.userId.Value;
                        propertyFilesVM.IsActivated = true;
                        propertyFilesVM.IsDeleted = false;
                    }
                }

                if (teniacoApiBusiness.AddToMyPropertyFilesForInvestors(
                    addToMyPropertyFilesForInvestorsPVM.PropertyFilesVMList,
                    addToMyPropertyFilesForInvestorsPVM.PropertyId))
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
        /// AddMediaToPropertyFilesForInvestors
        /// </summary>
        /// <param name="addToPropertyFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddMediaToPropertyFilesForInvestors")]
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

                if (teniacoApiBusiness.AddMediaToPropertyFilesForInvestors(
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



        ///// <summary>
        ///// UpdateMyPropertyFilesForInvestors
        ///// </summary>
        ///// <param name="updateMyPropertyFilesForInvestorsPVM"></param>
        ///// <returns>JsonResultWithRecordObjectVM</returns>
        //[HttpPost("UpdateMyPropertyFilesForInvestors")]
        //[ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        //public IActionResult UpdateMyPropertyFilesForInvestors([FromBody] UpdateMyPropertyFilesForInvestorsPVM
        //    updateMyPropertyFilesForInvestorsPVM)
        //{
        //    JsonResultObjectVM jsonResultObjectVM =
        //        new JsonResultObjectVM(new object() { });

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            if (updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds == null)
        //            {
        //                updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            }
        //            else
        //            if (updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 0)
        //                updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            else
        //            if (updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds.Count == 1)
        //                if (updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
        //                    updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

        //            updateMyPropertyFilesForInvestorsPVM.PropertyFilesVM.EditEnDate = DateTime.Now;
        //            updateMyPropertyFilesForInvestorsPVM.PropertyFilesVM.EditTime = PersianDate.TimeNow;
        //            updateMyPropertyFilesForInvestorsPVM.PropertyFilesVM.UserIdEditor = this.userId.Value;
        //        }

        //        var propertyFilesForInvestorsVM = updateMyPropertyFilesForInvestorsPVM.PropertyFilesVM;

        //        if (teniacoApiBusiness.UpdateMyPropertyFiles(
        //            ref propertyFilesForInvestorsVM,
        //            updateMyPropertyFilesForInvestorsPVM.ChildsUsersIds))
        //        {
        //            jsonResultObjectVM.Result = "OK";
        //        }

        //        return new JsonResult(jsonResultObjectVM);
        //    }
        //    catch (Exception exc)
        //    { }

        //    jsonResultObjectVM.Result = "ERROR";
        //    jsonResultObjectVM.Message = "ErrorInService";

        //    return new JsonResult(jsonResultObjectVM);
        //}



        /// <summary>
        /// CompleteDeleteMyPropertyFilesForInvestorsPVM
        /// </summary>
        /// <param name="completeDeleteMyPropertyFilesForInvestorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyPropertForInvestorsyFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyPropertyFiles([FromBody] CompleteDeleteMyPropertyFilesForInvestorsPVM completeDeleteMyPropertyFilesForInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteMyPropertyFiles(
                    completeDeleteMyPropertyFilesForInvestorsPVM.PropertyFileId,
                    completeDeleteMyPropertyFilesForInvestorsPVM.ChildsUsersIds))
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

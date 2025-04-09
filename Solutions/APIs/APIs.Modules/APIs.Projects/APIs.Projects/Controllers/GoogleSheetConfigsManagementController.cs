using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Projects.Models.Business;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using System;
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Projects;
//using APIs.InitialPlans.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// GoogleSheetConfigsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class GoogleSheetConfigsManagementController : ApiBaseController
    {

        /// <summary>
        /// GoogleSheetConfigsManagement
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
        public GoogleSheetConfigsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllGoogleSheetConfigsList
        /// </summary>
        /// <param name="getListOfGoogleSheetConfigsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllGoogleSheetConfigsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllGoogleSheetConfigsList([FromBody] GetAllGoogleSheetConfigsListPVM getAllGoogleSheetConfigsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllGoogleSheetConfigsListPVM.ChildsUsersIds == null)
                    {
                        getAllGoogleSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllGoogleSheetConfigsListPVM.ChildsUsersIds.Count == 0)
                        getAllGoogleSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllGoogleSheetConfigsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllGoogleSheetConfigsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllGoogleSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfGoogleSheetConfigs = projectsApiBusiness.GetAllGoogleSheetConfigsList(
                    //getAllGoogleSheetConfigsListPVM.ChildsUsersIds,
                   //getAllGoogleSheetConfigsListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfGoogleSheetConfigs;
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
        /// GetListOfGoogleSheetConfigs
        /// </summary>
        /// <param name="getListOfGoogleSheetConfigsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfGoogleSheetConfigs([FromBody] GetListOfGoogleSheetConfigsPVM getListOfGoogleSheetConfigsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfGoogleSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        getListOfGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfGoogleSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        getListOfGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfGoogleSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfGoogleSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfGoogleSheetConfigs = projectsApiBusiness.GetListOfGoogleSheetConfigs(
                   getListOfGoogleSheetConfigsPVM.jtStartIndex.Value,
                   getListOfGoogleSheetConfigsPVM.jtPageSize.Value,
                   ref listCount,
                   //getListOfGoogleSheetConfigsPVM.ChildsUsersIds,
                   //getListOfGoogleSheetConfigsPVM.ConstructionProjectId,
                   getListOfGoogleSheetConfigsPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfGoogleSheetConfigs;
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
        /// AddToGoogleSheetConfigs
        /// </summary>
        /// <param name="addToGoogleSheetConfigsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToGoogleSheetConfigs([FromBody] AddToGoogleSheetConfigsPVM addToGoogleSheetConfigsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToGoogleSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        addToGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToGoogleSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        addToGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToGoogleSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (addToGoogleSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var googleSheetId = projectsApiBusiness.AddToGoogleSheetConfigs(addToGoogleSheetConfigsPVM.GoogleSheetConfigsVM);
                if (googleSheetId != 0)
                {
                    addToGoogleSheetConfigsPVM.GoogleSheetConfigsVM.GoogleSheetConfigId = googleSheetId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToGoogleSheetConfigsPVM.GoogleSheetConfigsVM;

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


        [HttpPost("UpdateGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateGoogleSheetConfigs([FromBody] UpdateGoogleSheetConfigsPVM updateGoogleSheetConfigsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateGoogleSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        updateGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateGoogleSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        updateGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateGoogleSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (updateGoogleSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateGoogleSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var googleSheetId = projectsApiBusiness.UpdateGoogleSheetConfigs(
                    //updateGoogleSheetConfigsPVM.ChildsUsersIds,
                    updateGoogleSheetConfigsPVM.GoogleSheetConfigsVM);
                if (googleSheetId != 0)
                {
                    updateGoogleSheetConfigsPVM.GoogleSheetConfigsVM.GoogleSheetConfigId = googleSheetId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateGoogleSheetConfigsPVM.GoogleSheetConfigsVM;

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

        [HttpPost("ToggleActivationGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationGoogleSheetConfigs(ToggleActivationGoogleSheetConfigsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationGoogleSheetConfigs(
                    toggleActivationInvestorsPVM.GoogleSheetConfigId,
                   toggleActivationInvestorsPVM.UserId.Value
                   //toggleActivationInvestorsPVM.ChildsUsersIds
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


        [HttpPost("TemporaryDeleteGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteGoogleSheetConfigs(TemporaryDeleteGoogleSheetConfigsPVM temporaryDeleteGoogleSheetConfigsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteGoogleSheetConfigs(
                    temporaryDeleteGoogleSheetConfigsPVM.GoogleSheetConfigId,
                   temporaryDeleteGoogleSheetConfigsPVM.UserId.Value
                   //temporaryDeleteGoogleSheetConfigsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteGoogleSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteGoogleSheetConfigs(CompleteDeleteGoogleSheetConfigsPVM completeDeleteGoogleSheetConfigsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            
            try
            {
                if (projectsApiBusiness.CompleteDeleteGoogleSheetConfigs(
                    completeDeleteGoogleSheetConfigsPVM.GoogleSheetConfigId
                   //completeDeleteGoogleSheetConfigsPVM.UserId.Value
                   //completeDeleteGoogleSheetConfigsPVM.ChildsUsersIds
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
        /// GetGoogleSheetConfigsWithGoogleSheetConfigId
        /// </summary>
        /// <param name="getGoogleSheetConfigsWithGoogleSheetConfigIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetGoogleSheetConfigsWithGoogleSheetConfigId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetGoogleSheetConfigsWithGoogleSheetConfigId(
            [FromBody] GetGoogleSheetConfigsWithGoogleSheetConfigIdPVM getGoogleSheetConfigsWithGoogleSheetConfigIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds == null)
                    {
                        getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds.Count == 0)
                        getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds.Count == 1)
                        if (getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfGoogleSheetConfigs = projectsApiBusiness.GetGoogleSheetConfigsWithGoogleSheetConfigId(
                        getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.GoogleSheetConfigId
                        //getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.UserId.Value
                        //getGoogleSheetConfigsWithGoogleSheetConfigIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfGoogleSheetConfigs;

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

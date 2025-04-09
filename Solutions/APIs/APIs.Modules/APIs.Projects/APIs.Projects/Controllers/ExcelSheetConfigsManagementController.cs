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
    /// ExcelSheetConfigsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ExcelSheetConfigsManagementController : ApiBaseController
    {

        /// <summary>
        /// ExcelSheetConfigsManagement
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
        public ExcelSheetConfigsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllExcelSheetConfigsList
        /// </summary>
        /// <param name="getListOfExcelSheetConfigsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllExcelSheetConfigsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllExcelSheetConfigsList([FromBody] GetAllExcelSheetConfigsListPVM getAllExcelSheetConfigsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllExcelSheetConfigsListPVM.ChildsUsersIds == null)
                    {
                        getAllExcelSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllExcelSheetConfigsListPVM.ChildsUsersIds.Count == 0)
                        getAllExcelSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllExcelSheetConfigsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllExcelSheetConfigsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllExcelSheetConfigsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfExcelSheetConfigs = projectsApiBusiness.GetAllExcelSheetConfigsList(
                    //getAllExcelSheetConfigsListPVM.ChildsUsersIds,
                    getAllExcelSheetConfigsListPVM.ExcelSheetConfigName,
                   getAllExcelSheetConfigsListPVM.GoogleSheetConfigId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfExcelSheetConfigs;
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
        /// GetListOfExcelSheetConfigs
        /// </summary>
        /// <param name="getListOfExcelSheetConfigsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfExcelSheetConfigs([FromBody] GetListOfExcelSheetConfigsPVM getListOfExcelSheetConfigsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfExcelSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        getListOfExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfExcelSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        getListOfExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfExcelSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfExcelSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfExcelSheetConfigs = projectsApiBusiness.GetListOfExcelSheetConfigs(
                   getListOfExcelSheetConfigsPVM.jtStartIndex.Value,
                   getListOfExcelSheetConfigsPVM.jtPageSize.Value,
                   ref listCount,
                   // getListOfExcelSheetConfigsPVM.ChildsUsersIds,
                   getListOfExcelSheetConfigsPVM.ExcelSheetConfigName,
                   getListOfExcelSheetConfigsPVM.GoogleSheetConfigId,
                   getListOfExcelSheetConfigsPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfExcelSheetConfigs;
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
        /// AddToExcelSheetConfigs
        /// </summary>
        /// <param name="addToExcelSheetConfigsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToExcelSheetConfigs([FromBody] AddToExcelSheetConfigsPVM addToExcelSheetConfigsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToExcelSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        addToExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToExcelSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        addToExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToExcelSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (addToExcelSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var excelSheetId = projectsApiBusiness.AddToExcelSheetConfigs(addToExcelSheetConfigsPVM.ExcelSheetConfigsVM);
                if (excelSheetId != 0)
                {
                    addToExcelSheetConfigsPVM.ExcelSheetConfigsVM.ExcelSheetConfigId = excelSheetId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToExcelSheetConfigsPVM.ExcelSheetConfigsVM;

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


        [HttpPost("UpdateExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateExcelSheetConfigs([FromBody] UpdateExcelSheetConfigsPVM updateExcelSheetConfigsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateExcelSheetConfigsPVM.ChildsUsersIds == null)
                    {
                        updateExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateExcelSheetConfigsPVM.ChildsUsersIds.Count == 0)
                        updateExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateExcelSheetConfigsPVM.ChildsUsersIds.Count == 1)
                        if (updateExcelSheetConfigsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateExcelSheetConfigsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var excelSheetId = projectsApiBusiness.UpdateExcelSheetConfigs(
                    //updateExcelSheetConfigsPVM.ChildsUsersIds,
                    updateExcelSheetConfigsPVM.ExcelSheetConfigsVM);
                if (excelSheetId != 0)
                {
                    updateExcelSheetConfigsPVM.ExcelSheetConfigsVM.ExcelSheetConfigId = excelSheetId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateExcelSheetConfigsPVM.ExcelSheetConfigsVM;

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

        [HttpPost("ToggleActivationExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationExcelSheetConfigs(ToggleActivationExcelSheetConfigsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationExcelSheetConfigs(
                    toggleActivationInvestorsPVM.ExcelSheetConfigId,
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


        [HttpPost("TemporaryDeleteExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteExcelSheetConfigs(TemporaryDeleteExcelSheetConfigsPVM temporaryDeleteExcelSheetConfigsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteExcelSheetConfigs(
                    temporaryDeleteExcelSheetConfigsPVM.ExcelSheetConfigId,
                   temporaryDeleteExcelSheetConfigsPVM.UserId.Value
                    //temporaryDeleteExcelSheetConfigsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteExcelSheetConfigs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteExcelSheetConfigs(CompleteDeleteExcelSheetConfigsPVM completeDeleteExcelSheetConfigsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            try
            {
                if (projectsApiBusiness.CompleteDeleteExcelSheetConfigs(
                    completeDeleteExcelSheetConfigsPVM.ExcelSheetConfigId,
                   completeDeleteExcelSheetConfigsPVM.UserId.Value
                   //completeDeleteExcelSheetConfigsPVM.ChildsUsersIds
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
        /// GetExcelSheetConfigsWithExcelSheetConfigId
        /// </summary>
        /// <param name="getExcelSheetConfigsWithExcelSheetConfigIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetExcelSheetConfigsWithExcelSheetConfigId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetExcelSheetConfigsWithExcelSheetConfigId(
            [FromBody] GetExcelSheetConfigsWithExcelSheetConfigIdPVM getExcelSheetConfigsWithExcelSheetConfigIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds == null)
                    {
                        getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds.Count == 0)
                        getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds.Count == 1)
                        if (getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfExcelSheetConfigs = projectsApiBusiness.GetExcelSheetConfigsWithExcelSheetConfigId(
                        getExcelSheetConfigsWithExcelSheetConfigIdPVM.ExcelSheetConfigId,
                        getExcelSheetConfigsWithExcelSheetConfigIdPVM.UserId.Value
                   //getExcelSheetConfigsWithExcelSheetConfigIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfExcelSheetConfigs;

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

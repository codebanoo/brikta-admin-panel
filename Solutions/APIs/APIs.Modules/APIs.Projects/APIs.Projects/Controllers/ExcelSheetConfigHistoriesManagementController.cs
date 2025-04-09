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
    /// ExcelSheetConfigHistoriesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ExcelSheetConfigHistoriesManagementController : ApiBaseController
    {

        /// <summary>
        /// ExcelSheetConfigHistoriesManagement
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
        public ExcelSheetConfigHistoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllExcelSheetConfigHistoriesList
        /// </summary>
        /// <param name="getListOfExcelSheetConfigHistoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllExcelSheetConfigHistoriesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllExcelSheetConfigHistoriesList([FromBody] GetAllExcelSheetConfigHistoriesListPVM getAllExcelSheetConfigHistoriesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds == null)
                    {
                        getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds.Count == 0)
                        getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfExcelSheetConfigHistories = projectsApiBusiness.GetAllExcelSheetConfigHistoriesList(
                    //getAllExcelSheetConfigHistoriesListPVM.ChildsUsersIds,
                   getAllExcelSheetConfigHistoriesListPVM.ExcelSheetConfigId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfExcelSheetConfigHistories;
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
        /// GetListOfExcelSheetConfigHistories
        /// </summary>
        /// <param name="getListOfExcelSheetConfigHistoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfExcelSheetConfigHistories([FromBody] GetListOfExcelSheetConfigHistoriesPVM getListOfExcelSheetConfigHistoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfExcelSheetConfigHistories = projectsApiBusiness.GetListOfExcelSheetConfigHistories(
                   getListOfExcelSheetConfigHistoriesPVM.jtStartIndex.Value,
                   getListOfExcelSheetConfigHistoriesPVM.jtPageSize.Value,
                   ref listCount,
                  // getListOfExcelSheetConfigHistoriesPVM.ChildsUsersIds,
                   getListOfExcelSheetConfigHistoriesPVM.ExcelSheetConfigId,
                   getListOfExcelSheetConfigHistoriesPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfExcelSheetConfigHistories;
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
        /// AddToExcelSheetConfigHistories
        /// </summary>
        /// <param name="addToExcelSheetConfigHistoriesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToExcelSheetConfigHistories([FromBody] AddToExcelSheetConfigHistoriesPVM addToExcelSheetConfigHistoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToExcelSheetConfigHistoriesPVM.ChildsUsersIds == null)
                    {
                        addToExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 0)
                        addToExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (addToExcelSheetConfigHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var excelHistoryId = projectsApiBusiness.AddToExcelSheetConfigHistories(addToExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM);
                if (excelHistoryId != 0)
                {
                    addToExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM.ExcelSheetConfigHistoryId = excelHistoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM;

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


        [HttpPost("UpdateExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateExcelSheetConfigHistories([FromBody] UpdateExcelSheetConfigHistoriesPVM updateExcelSheetConfigHistoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateExcelSheetConfigHistoriesPVM.ChildsUsersIds == null)
                    {
                        updateExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 0)
                        updateExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateExcelSheetConfigHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (updateExcelSheetConfigHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateExcelSheetConfigHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var excelHistoryId = projectsApiBusiness.UpdateExcelSheetConfigHistories(
                    //updateExcelSheetConfigHistoriesPVM.ChildsUsersIds,
                    updateExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM);
                if (excelHistoryId != 0)
                {
                    updateExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM.ExcelSheetConfigHistoryId = excelHistoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoriesVM;

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

        [HttpPost("ToggleActivationExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationExcelSheetConfigHistories(ToggleActivationExcelSheetConfigHistoriesPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationExcelSheetConfigHistories(
                    toggleActivationInvestorsPVM.ExcelSheetConfigHistoryId,
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


        [HttpPost("TemporaryDeleteExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteExcelSheetConfigHistories(TemporaryDeleteExcelSheetConfigHistoriesPVM temporaryDeleteExcelSheetConfigHistoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteExcelSheetConfigHistories(
                    temporaryDeleteExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoryId,
                   temporaryDeleteExcelSheetConfigHistoriesPVM.UserId.Value
                   //temporaryDeleteExcelSheetConfigHistoriesPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteExcelSheetConfigHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteExcelSheetConfigHistories(CompleteDeleteExcelSheetConfigHistoriesPVM completeDeleteExcelSheetConfigHistoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            try
            {
                if (projectsApiBusiness.CompleteDeleteExcelSheetConfigHistories(
                    completeDeleteExcelSheetConfigHistoriesPVM.ExcelSheetConfigHistoryId,
                   completeDeleteExcelSheetConfigHistoriesPVM.UserId.Value
                   //completeDeleteExcelSheetConfigHistoriesPVM.ChildsUsersIds
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
        /// GetExcelSheetConfigHistoriesWithExcelSheetConfigId
        /// </summary>
        /// <param name="getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetExcelSheetConfigHistoriesWithExcelSheetConfigId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetExcelSheetConfigHistoriesWithExcelSheetConfigId(
            [FromBody] GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryIdPVM getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds == null)
                    {
                        getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds.Count == 0)
                        getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds.Count == 1)
                        if (getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfExcelSheetConfigHistories = projectsApiBusiness.GetExcelSheetConfigHistoriesWithExcelSheetConfigHistoryId(
                        getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ExcelSheetConfigHistoryId,
                        getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.UserId.Value
                        //getExcelSheetConfigHistoriesWithExcelSheetConfigIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfExcelSheetConfigHistories;

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

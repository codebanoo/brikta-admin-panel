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
using System;
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Projects;

namespace APIs.Projects.Controllers
{

    /// <summary>
    /// ConstructionProjectBillDelaysManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ConstructionProjectBillDelaysManagementController : ApiBaseController
    {
        /// <summary>
        /// ConstructionProjectTypesManagement
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
        public ConstructionProjectBillDelaysManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConstructionProjectTypesList
        /// </summary>
        /// <param name="getAllConstructionProjectBillDelaysListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllConstructionProjectBillDelaysList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConstructionProjectBillDelaysList([FromBody] GetAllConstructionProjectBillDelaysListPVM getAllConstructionProjectBillDelaysListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds == null)
                    {
                        getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds.Count == 0)
                        getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds.Count == 1)
                        if (getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfConstructionProjectBillDelays = projectsApiBusiness.GetAllConstructionProjectBillDelaysList(
                    getAllConstructionProjectBillDelaysListPVM.ChildsUsersIds);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjectBillDelays;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// AddToConstructionProjectBillDelays
        /// </summary>
        /// <param name="addToConstructionProjectBillDelaysPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToConstructionProjectBillDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConstructionProjectBillDelays([FromBody] AddToConstructionProjectBillDelaysPVM addToConstructionProjectBillDelaysPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToConstructionProjectBillDelaysPVM.ChildsUsersIds == null)
                    {
                        addToConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToConstructionProjectBillDelaysPVM.ChildsUsersIds.Count == 0)
                        addToConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToConstructionProjectBillDelaysPVM.ChildsUsersIds.Count == 1)
                        if (addToConstructionProjectBillDelaysPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var constructionProjectBillDelayId = projectsApiBusiness.AddToConstructionProjectBillDelays(addToConstructionProjectBillDelaysPVM.ChildsUsersIds, addToConstructionProjectBillDelaysPVM.constructionProjectBillDelaysVM);
                if (constructionProjectBillDelayId != 0)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = constructionProjectBillDelayId;
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
        /// UpdateConstructionProjectBillDelays
        /// </summary>
        /// <param name="updateConstructionProjectBillDelaysPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("UpdateConstructionProjectBillDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConstructionProjectBillDelays([FromBody] UpdateConstructionProjectBillDelaysPVM updateConstructionProjectBillDelaysPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateConstructionProjectBillDelaysPVM.ChildsUsersIds == null)
                    {
                        updateConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateConstructionProjectBillDelaysPVM.ChildsUsersIds.Count == 0)
                        updateConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateConstructionProjectBillDelaysPVM.ChildsUsersIds.Count == 1)
                        if (updateConstructionProjectBillDelaysPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateConstructionProjectBillDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var result = projectsApiBusiness.UpdateConstructionProjectBillDelays(updateConstructionProjectBillDelaysPVM.ChildsUsersIds, updateConstructionProjectBillDelaysPVM.constructionProjectBillDelaysVM);
                if (result != false)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";

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
        /// GetListOfBillDelaysForOuterDashboard
        /// </summary>
        /// <param name="getListOfBillDelaysForOuterDashboardPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfBillDelaysForOuterDashboard")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfBillDelaysForOuterDashboard([FromBody] GetListOfBillDelaysForOuterDashboardPVM getListOfBillDelaysForOuterDashboardPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds == null)
                    {
                        getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds.Count == 0)
                        getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds.Count == 1)
                        if (getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfConstructionProjectBillDelays = projectsApiBusiness.GetListOfBillDelaysForOuterDashboard(
                    getListOfBillDelaysForOuterDashboardPVM.ConstructionProjectId,
                    getListOfBillDelaysForOuterDashboardPVM.ChildsUsersIds);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjectBillDelays;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


    }
}

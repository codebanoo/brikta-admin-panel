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
//using APIs.ContractorsAgreements.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// ContractorsAgreementsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ContractorsAgreementsManagementController : ApiBaseController
    {

        /// <summary>
        /// ContractorsAgreementsManagement
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
        public ContractorsAgreementsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllContractorsAgreementsList
        /// </summary>
        /// <param name="getListOfContractorsAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllContractorsAgreementsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllContractorsAgreementsList([FromBody] GetAllContractorsAgreementsListPVM getAllContractorsAgreementsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllContractorsAgreementsListPVM.ChildsUsersIds == null)
                    {
                        getAllContractorsAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllContractorsAgreementsListPVM.ChildsUsersIds.Count == 0)
                        getAllContractorsAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllContractorsAgreementsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllContractorsAgreementsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllContractorsAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractorsAgreements = projectsApiBusiness.GetAllContractorsAgreementsList(
                    getAllContractorsAgreementsListPVM.ChildsUsersIds,
                   getAllContractorsAgreementsListPVM.ContractorsAgreementTitle,
                   getAllContractorsAgreementsListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractorsAgreements;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

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
        /// GetListOfContractorsAgreements
        /// </summary>
        /// <param name="getListOfContractorsAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfContractorsAgreements([FromBody] GetListOfContractorsAgreementsPVM getListOfContractorsAgreementsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfContractorsAgreementsPVM.ChildsUsersIds == null)
                    {
                        getListOfContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfContractorsAgreementsPVM.ChildsUsersIds.Count == 0)
                        getListOfContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfContractorsAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfContractorsAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractorsAgreements = projectsApiBusiness.GetListOfContractorsAgreements(
                   getListOfContractorsAgreementsPVM.jtStartIndex.Value,
                   getListOfContractorsAgreementsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfContractorsAgreementsPVM.ChildsUsersIds,
                   getListOfContractorsAgreementsPVM.ContractorsAgreementTitle,
                   getListOfContractorsAgreementsPVM.ConstructionProjectId,
                   getListOfContractorsAgreementsPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractorsAgreements;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

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
        /// AddToContractorsAgreements
        /// </summary>
        /// <param name="addToContractorsAgreementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToContractorsAgreements([FromBody] AddToContractorsAgreementsPVM addToContractorsAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToContractorsAgreementsPVM.ChildsUsersIds == null)
                    {
                        addToContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToContractorsAgreementsPVM.ChildsUsersIds.Count == 0)
                        addToContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToContractorsAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (addToContractorsAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var contractorsAgreementId = projectsApiBusiness.AddToContractorsAgreements(addToContractorsAgreementsPVM.ContractorsAgreementsVM);
                if (contractorsAgreementId != 0)
                {
                    addToContractorsAgreementsPVM.ContractorsAgreementsVM.ContractorsAgreementId = contractorsAgreementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToContractorsAgreementsPVM.ContractorsAgreementsVM;

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


        [HttpPost("UpdateContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateContractorsAgreements([FromBody] UpdateContractorsAgreementsPVM updateContractorsAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateContractorsAgreementsPVM.ChildsUsersIds == null)
                    {
                        updateContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateContractorsAgreementsPVM.ChildsUsersIds.Count == 0)
                        updateContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateContractorsAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (updateContractorsAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateContractorsAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedContractorsAgreementsVM = projectsApiBusiness.UpdateContractorsAgreements(
                    updateContractorsAgreementsPVM.ChildsUsersIds,
                    updateContractorsAgreementsPVM.ContractorsAgreementsVM);
                if (updatedContractorsAgreementsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedContractorsAgreementsVM;

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

        [HttpPost("ToggleActivationContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationContractorsAgreements(ToggleActivationContractorsAgreementsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationContractorsAgreements(
                    toggleActivationInvestorsPVM.ContractorsAgreementId,
                   toggleActivationInvestorsPVM.UserId.Value,
                   toggleActivationInvestorsPVM.ChildsUsersIds
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


        [HttpPost("TemporaryDeleteContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteContractorsAgreements(TemporaryDeleteContractorsAgreementsPVM temporaryDeleteContractorsAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteContractorsAgreements(
                    temporaryDeleteContractorsAgreementsPVM.ContractorsAgreementId,
                   temporaryDeleteContractorsAgreementsPVM.UserId.Value,
                   temporaryDeleteContractorsAgreementsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteContractorsAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteContractorsAgreements(CompleteDeleteContractorsAgreementsPVM completeDeleteContractorsAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteContractorsAgreements(
                    completeDeleteContractorsAgreementsPVM.ContractorsAgreementId,
                   completeDeleteContractorsAgreementsPVM.UserId.Value,
                   completeDeleteContractorsAgreementsPVM.ChildsUsersIds);
                if (result.HasValue && result.Value == true)
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            if (result == null)
            {
                jsonResultObjectVM.Message = "ERROR_DEPENDENCY";
            }
            else
                jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

        /// <summary>
        /// GetContractorsAgreementsWithContractorsAgreementId
        /// </summary>
        /// <param name="getContractorsAgreementsWithContractorsAgreementIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetContractorsAgreementsWithContractorsAgreementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetContractorsAgreementsWithContractorsAgreementId(
            [FromBody] GetContractorsAgreementsWithContractorsAgreementIdPVM getContractorsAgreementsWithContractorsAgreementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds == null)
                    {
                        getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds.Count == 0)
                        getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds.Count == 1)
                        if (getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfContractorsAgreements = projectsApiBusiness.GetContractorsAgreementsWithContractorsAgreementId(
                        getContractorsAgreementsWithContractorsAgreementIdPVM.ContractorsAgreementId,
                        getContractorsAgreementsWithContractorsAgreementIdPVM.UserId.Value,
                        getContractorsAgreementsWithContractorsAgreementIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfContractorsAgreements;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

using APIs.Projects.Models.Business;
using Microsoft.AspNetCore.Mvc;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Projects;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// ConstructionProjectFinancialDataManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ConstructionProjectFinancialDataManagementController : ApiBaseController
    {
        /// <summary>
        /// ConstructionProjectFinancialDataManagement
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
        public ConstructionProjectFinancialDataManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetConstructionProjectFinancialDataByConstructionProjectId
        /// </summary>
        /// <param name="getConstructionProjectFinancialDataPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetConstructionProjectFinancialDataByConstructionProjectId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConstructionProjectFinancialDataByConstructionProjectId([FromBody] GetConstructionProjectFinancialDataByConstructionProjectIdPVM getConstructionProjectFinancialDataPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectFinancialDataPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectFinancialDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConstructionProjectFinancialDataPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectFinancialDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConstructionProjectFinancialDataPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectFinancialDataPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectFinancialDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var List = projectsApiBusiness.GetConstructionProjectFinancialDataByConstructionProjectId(
                getConstructionProjectFinancialDataPVM.ConstructionProjectId,
                getConstructionProjectFinancialDataPVM.Type,
                getConstructionProjectFinancialDataPVM.UserId.Value,
                getConstructionProjectFinancialDataPVM.OwnerUserId
                );
                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = List;
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
        /// GetPaymentsAndCostsList
        /// </summary>
        /// <param name="getPaymentsAndCostsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetPaymentsAndCostsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPaymentsAndCostsList([FromBody] GetPaymentsAndCostsListPVM getPaymentsAndCostsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPaymentsAndCostsListPVM.ChildsUsersIds == null)
                    {
                        getPaymentsAndCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getPaymentsAndCostsListPVM.ChildsUsersIds.Count == 0)
                        getPaymentsAndCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getPaymentsAndCostsListPVM.ChildsUsersIds.Count == 1)
                        if (getPaymentsAndCostsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPaymentsAndCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                //int listCount = 0;

                var paymentsAndCostsVMList = projectsApiBusiness.GetPaymentsAndCostsList(
                    getPaymentsAndCostsListPVM.ConstructionProjectId,
                    getPaymentsAndCostsListPVM.UserId.Value,
                    getPaymentsAndCostsListPVM.Type,
                    getPaymentsAndCostsListPVM.RepresentativeId.HasValue ? getPaymentsAndCostsListPVM.RepresentativeId : null
                );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = paymentsAndCostsVMList;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        #region MyProjects

        

        /// <summary>
        /// GetSumOfPrivateCostsList
        /// </summary>
        /// <param name="getPaymentsAndCostsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetSumOfPrivateCostsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetSumOfPrivateCostsList([FromBody] GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getSumOfPrivateCostsListPVM.ChildsUsersIds == null)
                    {
                        getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getSumOfPrivateCostsListPVM.ChildsUsersIds.Count == 0)
                        getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getSumOfPrivateCostsListPVM.ChildsUsersIds.Count == 1)
                        if (getSumOfPrivateCostsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                //int listCount = 0;

                var sumOfPrivateCostsList = projectsApiBusiness.GetSumOfPrivateCostsList(
                    getSumOfPrivateCostsListPVM.ConstructionProjectId

                );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = sumOfPrivateCostsList;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        #endregion

        #region RepresentativesProjects



        /// <summary>
        /// GetSumOfPrivateCostsList
        /// </summary>
        /// <param name="getPaymentsAndCostsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetSumOfPrivateCostsListForRepresantative")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetSumOfPrivateCostsListForRepresantative([FromBody] GetSumOfPrivateCostsListPVM getSumOfPrivateCostsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getSumOfPrivateCostsListPVM.ChildsUsersIds == null)
                    {
                        getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getSumOfPrivateCostsListPVM.ChildsUsersIds.Count == 0)
                        getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getSumOfPrivateCostsListPVM.ChildsUsersIds.Count == 1)
                        if (getSumOfPrivateCostsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getSumOfPrivateCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                //int listCount = 0;

                var sumOfPrivateCostsList = projectsApiBusiness.GetSumOfPrivateCostsListForRepresantative(
                    getSumOfPrivateCostsListPVM.ConstructionProjectId,
                    getSumOfPrivateCostsListPVM.Type,
                    getSumOfPrivateCostsListPVM.RepresentativeId.HasValue ? getSumOfPrivateCostsListPVM.RepresentativeId.Value : null

                );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = sumOfPrivateCostsList;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        #endregion

        /// <summary>
        /// GetSumOfPublicCostsList
        /// </summary>
        /// <param name="getPaymentsAndCostsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetSumOfPublicCostsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetSumOfPublicCostsList([FromBody] GetSumOfPublicCostsListPVM getSumOfPublicCostsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getSumOfPublicCostsListPVM.ChildsUsersIds == null)
                    {
                        getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getSumOfPublicCostsListPVM.ChildsUsersIds.Count == 0)
                        getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getSumOfPublicCostsListPVM.ChildsUsersIds.Count == 1)
                        if (getSumOfPublicCostsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                //int listCount = 0;

                var sumOfPublicCostsList = projectsApiBusiness.GetSumOfPublicCostsList(
                    getSumOfPublicCostsListPVM.ConstructionProjectId
                );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = sumOfPublicCostsList;

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
        /// GetSumOfPublicCostsListForRepresentative
        /// </summary>
        /// <param name="getPaymentsAndCostsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 
        [HttpPost("GetSumOfPublicCostsListForRepresentative")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetSumOfPublicCostsListForRepresentative([FromBody] GetSumOfPublicCostsForRepresentativeListPVM getSumOfPublicCostsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getSumOfPublicCostsListPVM.ChildsUsersIds == null)
                    {
                        getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getSumOfPublicCostsListPVM.ChildsUsersIds.Count == 0)
                        getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getSumOfPublicCostsListPVM.ChildsUsersIds.Count == 1)
                        if (getSumOfPublicCostsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getSumOfPublicCostsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                //int listCount = 0;

                var sumOfPublicCostsList = projectsApiBusiness.GetSumOfPublicCostsListForRepresentative(
                    getSumOfPublicCostsListPVM.ConstructionProjectId,
                    getSumOfPublicCostsListPVM.Type,
                    getSumOfPublicCostsListPVM.RepresentativeId
                );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = sumOfPublicCostsList;

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

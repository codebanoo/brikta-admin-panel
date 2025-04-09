using APIs.Projects.Models.Business;
using APIs.Teniaco.Models.Business;
using Microsoft.AspNetCore.Mvc;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using APIs.CustomAttributes;
using System.Linq;
using System.Collections.Generic;
using APIs.Teniaco.Models.Entities;
using System.Threading.Tasks;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// TeniacoPanel
    /// </summary>
    [CustomApiAuthentication]
    public class TeniacoPanelController : ApiBaseController
    {
        /// <summary>
        /// TeniacoPanel
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
        public TeniacoPanelController(IHostEnvironment _hostingEnvironment,
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
        /// GetOutterDashboardPricesBlock
        /// </summary>
        /// <param name="addPropertiesInMelkavanPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetOutterDashboardPricesBlock")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetOutterDashboardPricesBlock([FromBody] GetOutterDashboardPricesBlockPVM
            getOutterDashboardPricesBlockPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getOutterDashboardPricesBlockPVM.ChildsUsersIds == null)
                    {
                        getOutterDashboardPricesBlockPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getOutterDashboardPricesBlockPVM.ChildsUsersIds.Count == 0)
                        getOutterDashboardPricesBlockPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getOutterDashboardPricesBlockPVM.ChildsUsersIds.Count == 1)
                        if (getOutterDashboardPricesBlockPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getOutterDashboardPricesBlockPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                long personId = 0;

                var userProfile = consoleBusiness.GetUsersProfile(getOutterDashboardPricesBlockPVM.UserId.Value);

                if (userProfile != null)
                {
                    string phoneNumber = userProfile.Mobile;

                    if (publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).Any())
                        personId = publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).FirstOrDefault().PersonId;

                }

                OutterDashboardPricesBlockVM outterDashboardPricesBlockVM = teniacoApiBusiness.GetOutterDashboardPricesBlock(getOutterDashboardPricesBlockPVM.UserId.Value,
                    personId,
                    projectsApiBusiness);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = outterDashboardPricesBlockVM;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetListOfMyFundsDispersion
        /// </summary>
        /// <param name="getListOfMyFundsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetListOfMyFundsDispersion")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyFundsDispersion([FromBody] GetListOfMyFundsDispersionPVM
            getListOfMyFundsDispersionPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyFundsDispersionPVM.ChildsUsersIds == null)
                    {
                        getListOfMyFundsDispersionPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyFundsDispersionPVM.ChildsUsersIds.Count == 0)
                        getListOfMyFundsDispersionPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyFundsDispersionPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyFundsDispersionPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyFundsDispersionPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                long personId = 0;

                var userProfile = consoleBusiness.GetUsersProfile(getListOfMyFundsDispersionPVM.UserId.Value);

                if (userProfile != null)
                {
                    string phoneNumber = userProfile.Mobile;

                    if (publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).Any())
                        personId = publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).FirstOrDefault().PersonId;
                }

                List<MyFundsVM> myFundsVMList = teniacoApiBusiness.GetListOfMyFundsDispersion(getListOfMyFundsDispersionPVM.UserId.Value,
                    personId,
                    projectsApiBusiness);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = myFundsVMList;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// GetListOfMyFundsGrowth
        /// </summary>
        /// <param name="getListOfMyFundsGrowthPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetListOfMyFundsGrowth")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyFundsGrowth([FromBody] GetListOfMyFundsGrowthPVM
            getListOfMyFundsGrowthPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyFundsGrowthPVM.ChildsUsersIds == null)
                    {
                        getListOfMyFundsGrowthPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyFundsGrowthPVM.ChildsUsersIds.Count == 0)
                        getListOfMyFundsGrowthPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyFundsGrowthPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyFundsGrowthPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyFundsGrowthPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                long personId = 0;

                var userProfile = consoleBusiness.GetUsersProfile(getListOfMyFundsGrowthPVM.UserId.Value);

                if (userProfile != null)
                {
                    string phoneNumber = userProfile.Mobile;

                    if (publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).Any())
                        personId = publicApiBusiness.PublicApiDb.Persons.Where(p => p.Mobail.Equals(phoneNumber) || p.Phone.Equals(phoneNumber)).FirstOrDefault().PersonId;
                }

                List<MyFundsVM> myFundsVMList = teniacoApiBusiness.GetListOfMyFundsGrowth(getListOfMyFundsGrowthPVM.UserId.Value,
                    personId,
                    projectsApiBusiness);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = myFundsVMList;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// GetDetailsForOuterDashboard
        /// </summary>
        /// <param name="getCountOfUnverifiedFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetDetailsForOuterDashboard")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCountOfUnverifiedFiles([FromBody] GetDetailsForOuterDashboardPVM
            getDetailsForOuterDashboardPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getDetailsForOuterDashboardPVM.ChildsUsersIds == null)
                    {
                        getDetailsForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getDetailsForOuterDashboardPVM.ChildsUsersIds.Count == 0)
                        getDetailsForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getDetailsForOuterDashboardPVM.ChildsUsersIds.Count == 1)
                        if (getDetailsForOuterDashboardPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getDetailsForOuterDashboardPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                    DetailsForOuterDashboardVM detailsForOuterDashboardVM = await teniacoApiBusiness.GetDetailsForOuterDashboard(
                    getDetailsForOuterDashboardPVM.UserId.Value,
                    projectsApiBusiness,
                    teniacoApiBusiness,
                    melkavanApiBusiness);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = detailsForOuterDashboardVM;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// GetUnreadConversationsAndUnverifiedFilesCount
        /// </summary>
        /// <param name="getUnreadConversationsAndUnverifiedFilesCountPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetUnreadConversationsAndUnverifiedFilesCount")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUnreadConversationsAndUnverifiedFilesCount([FromBody] GetUnreadConversationsAndUnverifiedFilesCountPVM
            getUnreadConversationsAndUnverifiedFilesCountPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds == null)
                    {
                        getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds.Count == 0)
                        getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds.Count == 1)
                        if (getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getUnreadConversationsAndUnverifiedFilesCountPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                GetUnreadConversationsAndUnverifiedFilesCountVM getUnreadConversationsAndUnverifiedFilesCountVM = await teniacoApiBusiness.GetUnreadConversationsAndUnverifiedFilesCount(
                getUnreadConversationsAndUnverifiedFilesCountPVM.UserId.Value,
                projectsApiBusiness);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = getUnreadConversationsAndUnverifiedFilesCountVM;

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

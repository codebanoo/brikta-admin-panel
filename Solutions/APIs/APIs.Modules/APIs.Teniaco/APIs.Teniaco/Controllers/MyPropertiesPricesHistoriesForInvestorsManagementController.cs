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
using System.Linq;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesPricesHistoriesForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesPricesHistoriesForInvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesPricesHistoriesManagement
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
        public MyPropertiesPricesHistoriesForInvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfMyPropertiesPricesHistoriesForInvestors
        /// </summary>
        /// <param name="getListOfMyPropertiesPricesHistoriesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfMyPropertiesPricesHistoriesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyPropertiesPricesHistoriesForInvestors([FromBody] GetListOfMyPropertiesPricesHistoriesForInvestorsPVM getListOfMyPropertiesPricesHistoriesForInvestorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var ListOfPropertiesPricesHistories = teniacoApiBusiness.GetListOfMyPropertiesPricesHistoriesForInvestors(
                 getListOfMyPropertiesPricesHistoriesForInvestorsPVM.jtStartIndex.Value,
                 getListOfMyPropertiesPricesHistoriesForInvestorsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfMyPropertiesPricesHistoriesForInvestorsPVM.ChildsUsersIds,
                   getListOfMyPropertiesPricesHistoriesForInvestorsPVM.PropertyId);


                var userId = ListOfPropertiesPricesHistories.Where(u => u.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).FirstOrDefault();
                var userName = consoleBusiness.GetUserName(userId);

                foreach (var priceHistoryUsers in ListOfPropertiesPricesHistories)
                {
                    priceHistoryUsers.UserCreatorName = userName;
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = ListOfPropertiesPricesHistories;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




    }
}

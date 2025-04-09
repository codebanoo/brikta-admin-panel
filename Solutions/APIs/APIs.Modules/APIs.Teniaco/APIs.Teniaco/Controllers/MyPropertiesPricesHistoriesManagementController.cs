using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Business;
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
using VM.PVM.Teniaco;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesPricesHistoriesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesPricesHistoriesManagementController : ApiBaseController
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
        public MyPropertiesPricesHistoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfMyPropertiesPricesHistories
        /// </summary>
        /// <param name="getListOfMyPropertiesPricesHistoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfMyPropertiesPricesHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyPropertiesPricesHistories([FromBody] GetListOfMyPropertiesPricesHistoriesPVM getListOfMyPropertiesPricesHistoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var ListOfPropertiesPricesHistories = teniacoApiBusiness.GetListOfMyPropertiesPricesHistories(
                 getListOfMyPropertiesPricesHistoriesPVM.jtStartIndex.Value,
                 getListOfMyPropertiesPricesHistoriesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfMyPropertiesPricesHistoriesPVM.ChildsUsersIds,
                   getListOfMyPropertiesPricesHistoriesPVM.PropertyId,
                   getListOfMyPropertiesPricesHistoriesPVM.jtSorting);


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

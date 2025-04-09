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
    /// PropertiesPricesHistoriesManagement
    /// </summary>
    [CustomApiAuthentication]

    public class PropertiesPricesHistoriesManagementController : ApiBaseController
    {
        /// <summary>
        /// PropertiesPricesHistoriesManagement
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

        public PropertiesPricesHistoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfPropertiesPricesHistories
        /// </summary>
        /// <param name="getListOfPropertiesPricesHistoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfPropertiesPricesHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPropertiesPricesHistories([FromBody] GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var ListOfPropertiesPricesHistories = teniacoApiBusiness.GetListOfPropertiesPricesHistories(
                  getListOfPropertiesPricesHistoriesPVM.jtStartIndex.Value,
                  getListOfPropertiesPricesHistoriesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds,
                   getListOfPropertiesPricesHistoriesPVM.PropertyId,
                   getListOfPropertiesPricesHistoriesPVM.jtSorting);


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

        /// <summary>
        /// GetLastPropertiesPriceHistoryByPropertyId
        /// </summary>
        /// <param name="getListOfPropertiesPricesHistoriesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetLastPropertiesPriceHistoryByPropertyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetLastPropertiesPriceHistoryByPropertyId([FromBody] GetListOfPropertiesPricesHistoriesPVM
            getListOfPropertiesPricesHistoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var property = teniacoApiBusiness.GetLastPropertiesPriceHistoryByPropertyId(
                getListOfPropertiesPricesHistoriesPVM.ChildsUsersIds,
                getListOfPropertiesPricesHistoriesPVM.PropertyId);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = property;

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

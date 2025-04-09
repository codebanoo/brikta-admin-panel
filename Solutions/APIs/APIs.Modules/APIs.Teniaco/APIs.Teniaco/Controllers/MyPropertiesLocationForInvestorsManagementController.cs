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
using System.Net;
using System;
using VM.Base;
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesLocationForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesLocationForInvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesLocationForInvestorsManagement
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
        public MyPropertiesLocationForInvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// UpdateMyPropertyLocationForInvestors
        /// </summary>
        /// <param name="updateMyPropertyLocationForInvestorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("UpdateMyPropertyLocationForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyPropertyLocation([FromBody] UpdateMyPropertyLocationForInvestorsPVM
            updateMyPropertyLocationForInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.UpdateMyPropertyLocationForInvestors(
                    updateMyPropertyLocationForInvestorsPVM.UserId.Value,
                    updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.PropertyId,
                          updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.StateId,
                     updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.CityId,
                     updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.ZoneId,
                     updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.DistrictId,
                     updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.Address,
                    updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.LocationLat.HasValue ? updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.LocationLat.Value : null,
                    updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.LocationLon.HasValue ? updateMyPropertyLocationForInvestorsPVM.PropertyLocationVM.LocationLon.Value : null))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);

                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);

        }


    }
}

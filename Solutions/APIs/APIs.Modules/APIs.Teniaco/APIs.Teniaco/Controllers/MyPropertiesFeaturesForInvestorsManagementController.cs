using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using FrameWork;
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
using System.Linq;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesFeaturesForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesFeaturesForInvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesFeaturesManagement
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
        public MyPropertiesFeaturesForInvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetMyPropertyFeaturesValuesForInvestors
        /// </summary>
        /// <param name="getPMyPropertyFeaturesValuesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetMyPropertyFeaturesValuesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyPropertyFeaturesValuesForInvestors([FromBody] GetMyPropertyFeaturesValuesForInvestorsPVM getPMyPropertyFeaturesValuesForInvestorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPMyPropertyFeaturesValuesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFeaturesValues = teniacoApiBusiness.GetMyPropertyFeaturesValuesForInvestors(
                    getPMyPropertyFeaturesValuesForInvestorsPVM.PropertyId,
                    getPMyPropertyFeaturesValuesForInvestorsPVM.PropertyTypeId);

                propertyFeaturesValues.ElementTypesVMList = publicApiBusiness.GetAllElementTypesList();

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = propertyFeaturesValues;

                return new JsonResult(jsonResultWithRecordObjectVM);

            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// UpdateMyPropertyFeaturesForInvestors
        /// </summary>
        /// <param name="updateMyPropertyFeaturesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateMyPropertyFeaturesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyPropertyFeaturesForInvestors([FromBody] UpdateMyPropertyFeaturesForInvestorsPVM updateMyPropertyFeaturesForInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMyPropertyFeaturesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                foreach (var featuresValuesVM in updateMyPropertyFeaturesForInvestorsPVM.FeaturesValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.PropertyId = updateMyPropertyFeaturesForInvestorsPVM.PropertyId;
                }

                if (teniacoApiBusiness.UpdateMyPropertyFeaturesForInvestors(updateMyPropertyFeaturesForInvestorsPVM.PropertyId, updateMyPropertyFeaturesForInvestorsPVM.FeaturesValuesVMList))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                }

            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";

            return new JsonResult(jsonResultObjectVM);
        }



    }
}

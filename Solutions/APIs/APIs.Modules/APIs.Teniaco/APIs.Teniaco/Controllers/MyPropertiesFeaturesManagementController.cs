using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
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
using System.Linq;
using FrameWork;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesFeaturesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesFeaturesManagementController : ApiBaseController
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
        public MyPropertiesFeaturesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetMyPropertyFeaturesValues
        /// </summary>
        /// <param name="getPMyPropertyFeaturesValuesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetMyPropertyFeaturesValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyPropertyFeaturesValues([FromBody] GetMyPropertyFeaturesValuesPVM getPMyPropertyFeaturesValuesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPMyPropertyFeaturesValuesPVM.ChildsUsersIds == null)
                    {
                        getPMyPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPMyPropertyFeaturesValuesPVM.ChildsUsersIds.Count == 0)
                        getPMyPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPMyPropertyFeaturesValuesPVM.ChildsUsersIds.Count == 1)
                        if (getPMyPropertyFeaturesValuesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPMyPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFeaturesValues = teniacoApiBusiness.GetMyPropertyFeaturesValues(
                    getPMyPropertyFeaturesValuesPVM.PropertyId,
                    getPMyPropertyFeaturesValuesPVM.PropertyTypeId);

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
        /// UpdateMyPropertyFeatures
        /// </summary>
        /// <param name="updateMyPropertyFeaturesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateMyPropertyFeatures")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyPropertyFeatures([FromBody] UpdateMyPropertyFeaturesPVM updateMyPropertyFeaturesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMyPropertyFeaturesPVM.ChildsUsersIds == null)
                    {
                        updateMyPropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateMyPropertyFeaturesPVM.ChildsUsersIds.Count == 0)
                        updateMyPropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateMyPropertyFeaturesPVM.ChildsUsersIds.Count == 1)
                        if (updateMyPropertyFeaturesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMyPropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                foreach (var featuresValuesVM in updateMyPropertyFeaturesPVM.FeaturesValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.PropertyId = updateMyPropertyFeaturesPVM.PropertyId;
                }

                if (teniacoApiBusiness.UpdateMyPropertyFeatures(updateMyPropertyFeaturesPVM.PropertyId, updateMyPropertyFeaturesPVM.FeaturesValuesVMList))
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

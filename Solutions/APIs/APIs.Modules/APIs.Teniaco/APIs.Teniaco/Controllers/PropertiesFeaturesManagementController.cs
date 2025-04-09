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
    /// PropertiesFeaturesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PropertiesFeaturesManagementController : ApiBaseController
    {
        /// <summary>
        /// PropertiesFeaturesManagement
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
        public PropertiesFeaturesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetPropertyFeaturesValues
        /// </summary>
        /// <param name="getAllPropertyFeaturesValuesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetPropertyFeaturesValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPropertyFeaturesValues([FromBody] GetPropertyFeaturesValuesPVM getPropertyFeaturesValuesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPropertyFeaturesValuesPVM.ChildsUsersIds == null)
                    {
                        getPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPropertyFeaturesValuesPVM.ChildsUsersIds.Count == 0)
                        getPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPropertyFeaturesValuesPVM.ChildsUsersIds.Count == 1)
                        if (getPropertyFeaturesValuesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPropertyFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertyFeaturesValues = teniacoApiBusiness.GetPropertyFeaturesValues(
                    getPropertyFeaturesValuesPVM.PropertyId,
                    getPropertyFeaturesValuesPVM.PropertyTypeId);

                propertyFeaturesValues.ElementTypesVMList = publicApiBusiness.GetAllElementTypesList();

                jsonResultWithRecordObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordObjectVM.Record = propertyFeaturesValues;

                return new JsonResult(jsonResultWithRecordObjectVM);

                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// UpdatePropertyFeatures
        /// </summary>
        /// <param name="updatePropertyFeaturesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdatePropertyFeatures")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdatePropertyFeatures([FromBody] UpdatePropertyFeaturesPVM updatePropertyFeaturesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updatePropertyFeaturesPVM.ChildsUsersIds == null)
                    {
                        updatePropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updatePropertyFeaturesPVM.ChildsUsersIds.Count == 0)
                        updatePropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updatePropertyFeaturesPVM.ChildsUsersIds.Count == 1)
                        if (updatePropertyFeaturesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updatePropertyFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                foreach (var featuresValuesVM in updatePropertyFeaturesPVM.FeaturesValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.PropertyId = updatePropertyFeaturesPVM.PropertyId;
                }

                if (teniacoApiBusiness.UpdatePropertyFeatures(updatePropertyFeaturesPVM.PropertyId, updatePropertyFeaturesPVM.FeaturesValuesVMList))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                }

                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }
    }
}

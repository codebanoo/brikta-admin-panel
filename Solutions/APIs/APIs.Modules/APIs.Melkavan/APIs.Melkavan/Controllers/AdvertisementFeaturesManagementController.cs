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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Melkavan;
using VM.Teniaco;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementFeaturesManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementFeaturesManagement
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
        public AdvertisementFeaturesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfFeaturesByPropertyTypeId
        /// </summary>
        /// <param name="getListOfFeaturesByPropertyTypeIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfFeaturesByPropertyTypeId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfFeaturesByPropertyTypeId(GetListOfFeaturesByPropertyTypeIdPVM getListOfFeaturesByPropertyTypeIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var manageAdvertisementFeaturesValuesVM = melkavanApiBusiness.GetListOfFeaturesByPropertyTypeId(
                    getListOfFeaturesByPropertyTypeIdPVM.PropertyTypeId.Value,
                    teniacoApiBusiness.TeniacoApiDb);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = manageAdvertisementFeaturesValuesVM;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }





        /// <summary>
        /// UpdateAdvertisementFeatures
        /// </summary>
        /// <param name="updateAdvertisementFeaturesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateAdvertisementFeatures")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisementFeatures(UpdateAdvertisementFeaturesPVM updateAdvertisementFeaturesPVM)

        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAdvertisementFeaturesPVM.ChildsUsersIds == null)
                    {
                        updateAdvertisementFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateAdvertisementFeaturesPVM.ChildsUsersIds.Count == 0)
                        updateAdvertisementFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateAdvertisementFeaturesPVM.ChildsUsersIds.Count == 1)
                        if (updateAdvertisementFeaturesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAdvertisementFeaturesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int featureListCount = 0;
                List<FeaturesVM> features = teniacoApiBusiness.GetListOfFeatures(0, 9999, ref featureListCount, updateAdvertisementFeaturesPVM.PropertyTypeId);
                List<int> featureIds = features.Select(a => a.FeatureId).ToList();
                if (melkavanApiBusiness.UpdateAdvertisementFeatures(
                    featureIds,
                    updateAdvertisementFeaturesPVM.AdvertisementFeaturesVMs,
                    updateAdvertisementFeaturesPVM.ChildsUsersIds
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
    }
}


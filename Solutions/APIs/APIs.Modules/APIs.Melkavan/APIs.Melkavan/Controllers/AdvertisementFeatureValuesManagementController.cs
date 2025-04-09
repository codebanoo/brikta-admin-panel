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
using System.Linq;
using FrameWork;
using APIs.Melkavan.Models.Business;
using VM.PVM.Melkavan;
using System.Collections.Generic;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementFeatureValuesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementFeatureValuesManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementFeatureValuesManagement
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
        public AdvertisementFeatureValuesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAdvertisementFeaturesValues
        /// </summary>
        /// <param name="getAdvertisementFeaturesValuesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAdvertisementFeaturesValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertisementFeaturesValues([FromBody] GetAdvertisementFeaturesValuesPVM getAdvertisementFeaturesValuesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementFeaturesValuesPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertisementFeaturesValuesPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertisementFeaturesValuesPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementFeaturesValuesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementFeaturesValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var advertisementFeaturesValues = melkavanApiBusiness.GetAdvertisementFeaturesValues(
                    getAdvertisementFeaturesValuesPVM.AdvertisementId,
                    getAdvertisementFeaturesValuesPVM.PropertyTypeId,
                    teniacoApiBusiness.TeniacoApiDb);

                advertisementFeaturesValues.ElementTypesVMList = publicApiBusiness.GetAllElementTypesList();


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = advertisementFeaturesValues;

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
        [HttpPost("UpdateAdvertisementFeatureValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisementFeatureValues([FromBody] UpdateAdvertisementFeatureValuesPVM updateAdvertisementFeaturesPVM)
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

                foreach (var featuresValuesVM in updateAdvertisementFeaturesPVM.AdvertisementFeaturesValuesVMList)
                {
                    featuresValuesVM.CreateEnDate = DateTime.Now;
                    featuresValuesVM.CreateTime = PersianDate.TimeNow;
                    featuresValuesVM.UserIdCreator = this.userId.Value;

                    featuresValuesVM.AdvertisementId = updateAdvertisementFeaturesPVM.AdvertisementId;
                }

                if (melkavanApiBusiness.UpdateAdvertisementFeatureValues(updateAdvertisementFeaturesPVM.AdvertisementId,
                    updateAdvertisementFeaturesPVM.AdvertisementFeaturesValuesVMList,
                    updateAdvertisementFeaturesPVM.ChildsUsersIds))
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





        /// <summary>
        /// GetAllAdvertisementFeatureValues
        /// </summary>
        /// <param name="getAllAdvertisementFeatureValuesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllAdvertisementFeatureValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAdvertisementFeatureValues([FromBody] GetAllAdvertisementFeatureValuesPVM getAllAdvertisementFeatureValuesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAdvertisementFeatureValuesPVM.ChildsUsersIds == null)
                    {
                        getAllAdvertisementFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllAdvertisementFeatureValuesPVM.ChildsUsersIds.Count == 0)
                        getAllAdvertisementFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllAdvertisementFeatureValuesPVM.ChildsUsersIds.Count == 1)
                        if (getAllAdvertisementFeatureValuesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAdvertisementFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var advertisementFeaturesValues = melkavanApiBusiness.GetAllAdvertisementFeatureValues(
                    getAllAdvertisementFeatureValuesPVM.AdvertisementTypeId,
                    getAllAdvertisementFeatureValuesPVM.PropertyTypeId,
                    teniacoApiBusiness.TeniacoApiDb);

                advertisementFeaturesValues.ElementTypesVMList = publicApiBusiness.GetAllElementTypesList();


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = advertisementFeaturesValues;

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIs.Public.Models;
using Microsoft.AspNetCore.Authorization;
using APIs.Core.Controllers;
using APIs.Public.Models.Business;
using VM.PVM.Teniaco;
using APIs.Automation.Models.Business;
using VM.Teniaco;
using Models.Business.ConsoleBusiness;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using APIs.CustomAttributes.Helper;
using Microsoft.Extensions.Options;
using VM.Base;
using APIs.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using APIs.Teniaco.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// FeaturesOptionsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class FeaturesOptionsManagementController : ApiBaseController
    {
        /// <summary>
        /// FeaturesOptionsManagement
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
        public FeaturesOptionsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFeaturesOptionsList
        /// </summary>
        /// <param name="getAllFeaturesOptionsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllFeaturesOptionsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllFeaturesOptionsList([FromBody] GetAllFeaturesOptionsListPVM getAllFeaturesOptionsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllFeaturesOptionsListPVM.ChildsUsersIds == null)
                    {
                        getAllFeaturesOptionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllFeaturesOptionsListPVM.ChildsUsersIds.Count == 0)
                        getAllFeaturesOptionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllFeaturesOptionsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllFeaturesOptionsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllFeaturesOptionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfFeaturesOptions = teniacoApiBusiness.GetAllFeaturesOptionsList(
                    ref listCount,
                    getAllFeaturesOptionsListPVM.FeatureId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfFeaturesOptions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);

                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }


        /// <summary>
        /// GetListOfFeaturesOptions
        /// </summary>
        /// <param name="getListOfFeaturesOptionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfFeaturesOptions([FromBody] GetListOfFeaturesOptionsPVM getListOfFeaturesOptionsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfFeaturesOptions = teniacoApiBusiness.GetListOfFeaturesOptions(
                    getListOfFeaturesOptionsPVM.jtStartIndex.Value,
                    getListOfFeaturesOptionsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfFeaturesOptionsPVM.FeatureId);

                //var states = teniacoApiBusiness.GetListOfStates(null, getListOfFeaturesOptionsPVM.Lang);
                //var cities = teniacoApiBusiness.GetListOfCities(null, getListOfFeaturesOptionsPVM.Lang);

                //foreach (var feature in listOfFeaturesOptions)
                //{
                //    if (feature.StateId.HasValue)
                //        if (feature.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == feature.StateId.Value);
                //            feature.StateName = state.StateName;
                //        }

                //    if (feature.CityId.HasValue)
                //        if (feature.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == feature.CityId.Value);
                //            feature.CityName = city.CityName;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFeaturesOptions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfFeaturesOptions);

                return new JsonResult(jsonResultWithRecordsObjectVM);
                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }

        /// <summary>
        /// AddToFeaturesOptions
        /// </summary>
        /// <param name="addToFeaturesOptionsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToFeaturesOptions([FromBody] AddToFeaturesOptionsPVM addToFeaturesOptionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int featureId = teniacoApiBusiness.AddToFeaturesOptions(
                    addToFeaturesOptionsPVM.FeaturesOptionsVM);

                if (featureId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateFeatureOption";
                }
                else
                if (featureId > 0)
                {
                    addToFeaturesOptionsPVM.FeaturesOptionsVM.FeatureId = featureId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToFeaturesOptionsPVM.FeaturesOptionsVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }

        /// <summary>
        /// GetFeatureWithFeatureId
        /// </summary>
        /// <param name="GetFeatureWithFeatureId"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetFeatureWithFeatureId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetFeatureWithFeatureId([FromBody] GetFeatureWithFeatureIdPVM
            getFeatureWithFeatureIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var feature = teniacoApiBusiness.GetFeatureWithFeatureId(
                    getFeatureWithFeatureIdPVM.FeatureId);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = feature;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateFeaturesOptions
        /// </summary>
        /// <param name="updateFeaturesOptionsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateFeaturesOptions([FromBody] UpdateFeaturesOptionsPVM updateFeaturesOptionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var FeaturesOptionsVM = updateFeaturesOptionsPVM.FeaturesOptionsVM;

                int featureId = teniacoApiBusiness.UpdateFeaturesOptions(
                    ref FeaturesOptionsVM);

                if (featureId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateFeatureOption";
                }
                else
                if (featureId > 0)
                {
                    updateFeaturesOptionsPVM.FeaturesOptionsVM.FeatureId = featureId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateFeaturesOptionsPVM.FeaturesOptionsVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }

        /// <summary>
        /// ToggleActivationFeaturesOptions
        /// </summary>
        /// <param name="toggleActivationFeaturesOptionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationFeaturesOptions([FromBody] ToggleActivationFeaturesOptionsPVM toggleActivationFeaturesOptionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationFeaturesOptions(
                    toggleActivationFeaturesOptionsPVM.FeatureOptionId,
                    toggleActivationFeaturesOptionsPVM.UserId.Value))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                        jsonResultObjectVM.Result = returnMessage;
                    else
                        jsonResultObjectVM.Result = "OK";
                }

                return new JsonResult(jsonResultObjectVM);
                //return jsonResultObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultObjectVM;
        }

        /// <summary>
        /// TemporaryDeleteFeaturesOptions
        /// </summary>
        /// <param name="temporaryDeleteFeaturesOptionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteFeaturesOptions([FromBody] TemporaryDeleteFeaturesOptionsPVM
            temporaryDeleteFeaturesOptionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteFeaturesOptions(
                    temporaryDeleteFeaturesOptionsPVM.FeatureOptionId,
                    temporaryDeleteFeaturesOptionsPVM.UserId.Value))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                    //return jsonResultObjectVM;
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultObjectVM;
        }

        /// <summary>
        /// CompleteDeleteFeaturesOptions
        /// </summary>
        /// <param name="completeDeleteFeaturesOptionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteFeaturesOptions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteFeaturesOptions([FromBody] CompleteDeleteFeaturesOptionsPVM completeDeleteFeaturesOptionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteFeaturesOptions(completeDeleteFeaturesOptionsPVM.FeatureOptionId))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);
                    //return jsonResultObjectVM;
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
            //return jsonResultObjectVM;
        }
    }
}

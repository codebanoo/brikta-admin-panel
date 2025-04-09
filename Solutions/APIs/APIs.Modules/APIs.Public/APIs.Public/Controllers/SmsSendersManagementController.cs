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
using VM.PVM.Public;
using APIs.Automation.Models.Business;
using VM.Public;
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
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Public.Controllers
{
    /// <summary>
    /// SmsSendersManagement
    /// </summary>
    [CustomApiAuthentication]
    public class SmsSendersManagementController : ApiBaseController
    {
        /// <summary>
        /// SmsSendersManagement
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
        public SmsSendersManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllSmsSendersList
        /// </summary>
        /// <param name="getAllSmsSendersListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllSmsSendersList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllSmsSendersList([FromBody] GetAllSmsSendersListPVM getAllSmsSendersListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllSmsSendersListPVM.ChildsUsersIds == null)
                    {
                        getAllSmsSendersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllSmsSendersListPVM.ChildsUsersIds.Count == 0)
                        getAllSmsSendersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllSmsSendersListPVM.ChildsUsersIds.Count == 1)
                        if (getAllSmsSendersListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllSmsSendersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfSmsSenders = publicApiBusiness.GetAllSmsSendersList(
                    getAllSmsSendersListPVM.ChildsUsersIds,
                    ref listCount,
                    getAllSmsSendersListPVM.SearchText);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfSmsSenders;
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
        /// GetListOfSmsSenders
        /// </summary>
        /// <param name="getListOfSmsSendersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfSmsSenders([FromBody] GetListOfSmsSendersPVM getListOfSmsSendersPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfSmsSenders = publicApiBusiness.GetListOfSmsSenders(
                    getListOfSmsSendersPVM.ChildsUsersIds,
                    getListOfSmsSendersPVM.jtStartIndex.Value,
                    getListOfSmsSendersPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfSmsSendersPVM.SearchText);

                //var states = publicApiBusiness.GetListOfStates(null, getListOfSmsSendersPVM.Lang);
                //var cities = publicApiBusiness.GetListOfCities(null, getListOfSmsSendersPVM.Lang);

                //foreach (var smsSender in listOfSmsSenders)
                //{
                //    if (smsSender.StateId.HasValue)
                //        if (smsSender.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == smsSender.StateId.Value);
                //            smsSender.StateName = state.StateName;
                //        }

                //    if (smsSender.CityId.HasValue)
                //        if (smsSender.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == smsSender.CityId.Value);
                //            smsSender.CityName = city.CityName;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfSmsSenders;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfSmsSenders);

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
        /// AddToSmsSenders
        /// </summary>
        /// <param name="addToSmsSendersPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToSmsSenders([FromBody] AddToSmsSendersPVM addToSmsSendersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                long smsSenderId = publicApiBusiness.AddToSmsSenders(
                    addToSmsSendersPVM.SmsSendersVM,
                    addToSmsSendersPVM.ChildsUsersIds);

                if (smsSenderId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateSmsSender";
                }
                else
                if (smsSenderId > 0)
                {
                    addToSmsSendersPVM.SmsSendersVM.SmsSenderId = smsSenderId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToSmsSendersPVM.SmsSendersVM;
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
        /// GetDefaultSmsSender 
        /// </summary>
        /// <param></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetDefaultSmsSender")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetDefaultSmsSender()
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var smsSender = publicApiBusiness.GetDefaultSmsSender();

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = smsSender;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateSmsSenders
        /// </summary>
        /// <param name="updateSmsSendersPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateSmsSenders([FromBody] UpdateSmsSendersPVM updateSmsSendersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var smsSendersVM = updateSmsSendersPVM.SmsSendersVM;

                long smsSenderId = publicApiBusiness.UpdateSmsSenders(
                    ref smsSendersVM,
                    updateSmsSendersPVM.ChildsUsersIds);

                if (smsSenderId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateSmsSender";
                }
                else
                if (smsSenderId > 0)
                {
                    updateSmsSendersPVM.SmsSendersVM.SmsSenderId = smsSenderId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateSmsSendersPVM.SmsSendersVM;
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
        /// ToggleActivationSmsSenders
        /// </summary>
        /// <param name="toggleActivationSmsSendersPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationSmsSenders([FromBody] ToggleActivationSmsSendersPVM toggleActivationSmsSendersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationSmsSenders(
                    toggleActivationSmsSendersPVM.SmsSenderId,
                    toggleActivationSmsSendersPVM.UserId.Value,
                    toggleActivationSmsSendersPVM.ChildsUsersIds))
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
        /// TemporaryDeleteSmsSenders
        /// </summary>
        /// <param name="temporaryDeleteSmsSendersPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteSmsSenders([FromBody] TemporaryDeleteSmsSendersPVM
            temporaryDeleteSmsSendersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteSmsSenders(
                    temporaryDeleteSmsSendersPVM.SmsSenderId,
                    temporaryDeleteSmsSendersPVM.UserId.Value,
                    temporaryDeleteSmsSendersPVM.ChildsUsersIds))
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
        /// CompleteDeleteSmsSenders
        /// </summary>
        /// <param name="completeDeleteSmsSendersPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteSmsSenders")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteSmsSenders([FromBody] CompleteDeleteSmsSendersPVM completeDeleteSmsSendersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteSmsSenders(completeDeleteSmsSendersPVM.SmsSenderId,completeDeleteSmsSendersPVM.ChildsUsersIds))
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

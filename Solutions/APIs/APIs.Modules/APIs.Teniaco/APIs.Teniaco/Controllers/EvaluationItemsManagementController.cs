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
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Teniaco;
using APIs.Core.Controllers;
using System.Linq;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// EvaluationItemsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class EvaluationItemsManagementController : ApiBaseController
    {
        /// <summary>
        /// EvaluationItemsManagement
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
        public EvaluationItemsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllEvaluationItemsList
        /// </summary>
        /// <param name="getAllEvaluationItemsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllEvaluationItemsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllEvaluationItemsList([FromBody] GetAllEvaluationItemsListPVM getAllEvaluationItemsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllEvaluationItemsListPVM.ChildsUsersIds == null)
                    {
                        getAllEvaluationItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllEvaluationItemsListPVM.ChildsUsersIds.Count == 0)
                        getAllEvaluationItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllEvaluationItemsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllEvaluationItemsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllEvaluationItemsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfEvaluationItems = teniacoApiBusiness.GetAllEvaluationItemsList(
                    ref listCount,
                    getAllEvaluationItemsListPVM.ChildsUsersIds,
                    getAllEvaluationItemsListPVM.EvaluationQuestionId,
                    getAllEvaluationItemsListPVM.EvaluationAnswerSearch);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationItems;
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
        /// GetListOfEvaluationItems
        /// </summary>
        /// <param name="getListOfEvaluationItemsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfEvaluationItems([FromBody] GetListOfEvaluationItemsPVM getListOfEvaluationItemsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfEvaluationItems = teniacoApiBusiness.GetListOfEvaluationItems(
                    getListOfEvaluationItemsPVM.jtStartIndex.Value,
                    getListOfEvaluationItemsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfEvaluationItemsPVM.ChildsUsersIds,
                    getListOfEvaluationItemsPVM.EvaluationQuestionId,
                    getListOfEvaluationItemsPVM.jtSorting);

                //var states = teniacoApiBusiness.GetListOfStates(null, getListOfEvaluationItemsPVM.Lang);
                //var cities = teniacoApiBusiness.GetListOfCities(null, getListOfEvaluationItemsPVM.Lang);

                //foreach (var evaluationItem in listOfEvaluationItems)
                //{
                //    if (evaluationItem.StateId.HasValue)
                //        if (evaluationItem.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == evaluationItem.StateId.Value);
                //            evaluationItem.StateName = state.StateName;
                //        }

                //    if (evaluationItem.CityId.HasValue)
                //        if (evaluationItem.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == evaluationItem.CityId.Value);
                //            evaluationItem.CityName = city.CityName;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationItems;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfEvaluationItems);

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
        /// AddToEvaluationItems
        /// </summary>
        /// <param name="addToEvaluationItemsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToEvaluationItems([FromBody] AddToEvaluationItemsPVM addToEvaluationItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int evaluationItemId = teniacoApiBusiness.AddToEvaluationItems(
                    addToEvaluationItemsPVM.EvaluationItemsVM);

                if (evaluationItemId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationItem";
                }
                else
                if (evaluationItemId > 0)
                {
                    addToEvaluationItemsPVM.EvaluationItemsVM.EvaluationItemId = evaluationItemId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToEvaluationItemsPVM.EvaluationItemsVM;
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
        /// GetEvaluationItemWithEvaluationItemId
        /// </summary>
        /// <param name="GetEvaluationItemWithEvaluationItemId"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetEvaluationItemWithEvaluationItemId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetEvaluationItemWithEvaluationItemId([FromBody] GetEvaluationItemWithEvaluationItemIdPVM
            getEvaluationItemWithEvaluationItemIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var evaluationItem = teniacoApiBusiness.GetEvaluationItemWithEvaluationItemId(
                    getEvaluationItemWithEvaluationItemIdPVM.EvaluationItemId/*,
                    updateEvaluationItemsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = evaluationItem;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateEvaluationItems
        /// </summary>
        /// <param name="updateEvaluationItemsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateEvaluationItems([FromBody] UpdateEvaluationItemsPVM updateEvaluationItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var EvaluationItemsVM = updateEvaluationItemsPVM.EvaluationItemsVM;

                int evaluationItemId = teniacoApiBusiness.UpdateEvaluationItems(
                    ref EvaluationItemsVM,
                    updateEvaluationItemsPVM.ChildsUsersIds);

                if (evaluationItemId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationItem";
                }
                else
                if (evaluationItemId > 0)
                {
                    updateEvaluationItemsPVM.EvaluationItemsVM.EvaluationItemId = evaluationItemId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateEvaluationItemsPVM.EvaluationItemsVM;
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
        /// ToggleActivationEvaluationItems
        /// </summary>
        /// <param name="toggleActivationEvaluationItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationEvaluationItems([FromBody] ToggleActivationEvaluationItemsPVM toggleActivationEvaluationItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationEvaluationItems(
                    toggleActivationEvaluationItemsPVM.EvaluationItemId,
                    toggleActivationEvaluationItemsPVM.UserId.Value,
                    toggleActivationEvaluationItemsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteEvaluationItems
        /// </summary>
        /// <param name="temporaryDeleteEvaluationItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteEvaluationItems([FromBody] TemporaryDeleteEvaluationItemsPVM
            temporaryDeleteEvaluationItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteEvaluationItems(
                    temporaryDeleteEvaluationItemsPVM.EvaluationItemId,
                    temporaryDeleteEvaluationItemsPVM.UserId.Value,
                    temporaryDeleteEvaluationItemsPVM.ChildsUsersIds))
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
        /// CompleteDeleteEvaluationItems
        /// </summary>
        /// <param name="completeDeleteEvaluationItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteEvaluationItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteEvaluationItems([FromBody] CompleteDeleteEvaluationItemsPVM completeDeleteEvaluationItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteEvaluationItems(completeDeleteEvaluationItemsPVM.EvaluationItemId, completeDeleteEvaluationItemsPVM.ChildsUsersIds))
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

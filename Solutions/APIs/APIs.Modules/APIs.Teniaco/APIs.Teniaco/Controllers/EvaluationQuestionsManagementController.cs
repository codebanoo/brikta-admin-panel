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
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// EvaluationQuestionsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class EvaluationQuestionsManagementController : ApiBaseController
    {
        /// <summary>
        /// EvaluationQuestionsManagement
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
        public EvaluationQuestionsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllEvaluationQuestionsList
        /// </summary>
        /// <param name="getAllEvaluationQuestionsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllEvaluationQuestionsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllEvaluationQuestionsList([FromBody] GetAllEvaluationQuestionsListPVM getAllEvaluationQuestionsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllEvaluationQuestionsListPVM.ChildsUsersIds == null)
                    {
                        getAllEvaluationQuestionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllEvaluationQuestionsListPVM.ChildsUsersIds.Count == 0)
                        getAllEvaluationQuestionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllEvaluationQuestionsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllEvaluationQuestionsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllEvaluationQuestionsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfEvaluationQuestions = teniacoApiBusiness.GetAllEvaluationQuestionsList(
                    ref listCount,
                    getAllEvaluationQuestionsListPVM.ChildsUsersIds,
                    getAllEvaluationQuestionsListPVM.EvaluationCategoryId,
                    getAllEvaluationQuestionsListPVM.EvaluationQuestionSearch);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationQuestions;
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
        /// GetListOfEvaluationQuestions
        /// </summary>
        /// <param name="getListOfEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfEvaluationQuestions([FromBody] GetListOfEvaluationQuestionsPVM getListOfEvaluationQuestionsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfEvaluationQuestions = teniacoApiBusiness.GetListOfEvaluationQuestions(
                    getListOfEvaluationQuestionsPVM.jtStartIndex.Value,
                    getListOfEvaluationQuestionsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfEvaluationQuestionsPVM.ChildsUsersIds,
                    getListOfEvaluationQuestionsPVM.EvaluationCategoryId,
                    getListOfEvaluationQuestionsPVM.EvaluationQuestionSearch);

                #region Comments
                //var states = teniacoApiBusiness.GetListOfStates(null, getListOfEvaluationQuestionsPVM.Lang);
                //var cities = teniacoApiBusiness.GetListOfCities(null, getListOfEvaluationQuestionsPVM.Lang);

                //foreach (var evaluationQuestion in listOfEvaluationQuestions)
                //{
                //    if (evaluationQuestion.StateId.HasValue)
                //        if (evaluationQuestion.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == evaluationQuestion.StateId.Value);
                //            evaluationQuestion.StateName = state.StateName;
                //        }

                //    if (evaluationQuestion.CityId.HasValue)
                //        if (evaluationQuestion.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == evaluationQuestion.CityId.Value);
                //            evaluationQuestion.CityName = city.CityName;
                //        }
                //}
                #endregion


                foreach (var evaluationQuestion in listOfEvaluationQuestions)
                {

                    evaluationQuestion.CountOfEvaluationItems = teniacoApiBusiness.TeniacoApiDb.EvaluationItems.Where(f => f.EvaluationQuestionId.Equals(evaluationQuestion.EvaluationQuestionId)).Count();

                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationQuestions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                

                return new JsonResult(jsonResultWithRecordsObjectVM);
                
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
           
        }


        /// <summary>
        /// AddToEvaluationQuestions
        /// </summary>
        /// <param name="addToEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToEvaluationQuestions([FromBody] AddToEvaluationQuestionsPVM addToEvaluationQuestionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int evaluationQuestionId = teniacoApiBusiness.AddToEvaluationQuestions(
                    addToEvaluationQuestionsPVM.EvaluationQuestionsVM);

                if (evaluationQuestionId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationQuestion";
                }
                else
                if (evaluationQuestionId > 0)
                {
                    addToEvaluationQuestionsPVM.EvaluationQuestionsVM.EvaluationQuestionId = evaluationQuestionId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToEvaluationQuestionsPVM.EvaluationQuestionsVM;
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
        /// GetEvaluationQuestionWithEvaluationQuestionId
        /// </summary>
        /// <param name="GetEvaluationQuestionWithEvaluationQuestionId"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetEvaluationQuestionWithEvaluationQuestionId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetEvaluationQuestionWithEvaluationQuestionId([FromBody] GetEvaluationQuestionWithEvaluationQuestionIdPVM
            getEvaluationQuestionWithEvaluationQuestionIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var evaluationQuestion = teniacoApiBusiness.GetEvaluationQuestionWithEvaluationQuestionId(
                    getEvaluationQuestionWithEvaluationQuestionIdPVM.EvaluationQuestionId/*,
                    updateEvaluationQuestionsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = evaluationQuestion;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// UpdateEvaluationQuestions
        /// </summary>
        /// <param name="updateEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateEvaluationQuestions([FromBody] UpdateEvaluationQuestionsPVM updateEvaluationQuestionsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var EvaluationQuestionsVM = updateEvaluationQuestionsPVM.EvaluationQuestionsVM;

                int evaluationQuestionId = teniacoApiBusiness.UpdateEvaluationQuestions(
                    ref EvaluationQuestionsVM,
                    updateEvaluationQuestionsPVM.ChildsUsersIds);

                if (evaluationQuestionId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationQuestion";
                }
                else
                if (evaluationQuestionId > 0)
                {
                    updateEvaluationQuestionsPVM.EvaluationQuestionsVM.EvaluationQuestionId = evaluationQuestionId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateEvaluationQuestionsPVM.EvaluationQuestionsVM;
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
        /// ToggleActivationEvaluationQuestions
        /// </summary>
        /// <param name="toggleActivationEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationEvaluationQuestions([FromBody] ToggleActivationEvaluationQuestionsPVM toggleActivationEvaluationQuestionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationEvaluationQuestions(
                    toggleActivationEvaluationQuestionsPVM.EvaluationQuestionId,
                    toggleActivationEvaluationQuestionsPVM.UserId.Value,
                    toggleActivationEvaluationQuestionsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteEvaluationQuestions
        /// </summary>
        /// <param name="temporaryDeleteEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteEvaluationQuestions([FromBody] TemporaryDeleteEvaluationQuestionsPVM
            temporaryDeleteEvaluationQuestionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteEvaluationQuestions(
                    temporaryDeleteEvaluationQuestionsPVM.EvaluationQuestionId,
                    temporaryDeleteEvaluationQuestionsPVM.UserId.Value,
                    temporaryDeleteEvaluationQuestionsPVM.ChildsUsersIds))
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
        /// CompleteDeleteEvaluationQuestions
        /// </summary>
        /// <param name="completeDeleteEvaluationQuestionsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteEvaluationQuestions")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteEvaluationQuestions([FromBody] CompleteDeleteEvaluationQuestionsPVM completeDeleteEvaluationQuestionsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteEvaluationQuestions(completeDeleteEvaluationQuestionsPVM.EvaluationQuestionId, completeDeleteEvaluationQuestionsPVM.ChildsUsersIds))
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

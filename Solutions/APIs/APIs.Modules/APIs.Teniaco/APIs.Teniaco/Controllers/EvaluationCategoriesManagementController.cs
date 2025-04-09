using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.Public.Models.Entities;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using APIs.Teniaco.Models.Entities;
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
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// EvaluationCategoriesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class EvaluationCategoriesManagementController : ApiBaseController
    {
        /// <summary>
        /// EvaluationCategoriesManagement
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
        public EvaluationCategoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllExclusivelyEvaluationCategoriesList
        /// </summary>
        /// <param name="getAllEvaluationCategoriesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllExclusivelyEvaluationCategoriesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllExclusivelyEvaluationCategoriesList([FromBody] GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds == null)
                    {
                        getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.Count == 0)
                        getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfEvaluationCategories = teniacoApiBusiness.GetAllEvaluationCategoriesList(
                  ref listCount,
                  getAllEvaluationCategoriesListPVM.ChildsUsersIds,
                  getAllEvaluationCategoriesListPVM.EvaluationId,
                  getAllEvaluationCategoriesListPVM.EvaluationCategoryTitleSearch);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationCategories;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetAllEvaluationCategoriesList
        /// </summary>
        /// <param name="getAllEvaluationCategoriesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllEvaluationCategoriesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllEvaluationCategoriesList([FromBody] GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds == null)
                    {
                        getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.Count == 0)
                        getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllEvaluationCategoriesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllEvaluationCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var answerSheetEvaluationVMList = new AnswerSheetEvaluationVM();

                var listOfEvaluationCategories = teniacoApiBusiness.GetAllEvaluationCategoriesList(
                    ref listCount,
                    getAllEvaluationCategoriesListPVM.ChildsUsersIds,
                    getAllEvaluationCategoriesListPVM.EvaluationId,
                    getAllEvaluationCategoriesListPVM.EvaluationCategoryTitleSearch);


                var evalCategoriesIds = listOfEvaluationCategories.Select(p => p.EvaluationCategoryId).ToList();

                var evalQuestions = teniacoApiBusiness.GetEvaluationQuestionsWithEvalCategoriesIds(evalCategoriesIds);

                if (evalQuestions != null)
                    if (evalQuestions.Count > 0)
                    {
                        var evalQuestionIds = evalQuestions.Select(i => i.EvaluationQuestionId).ToList();

                        var evalItems = teniacoApiBusiness.GetEvaluationItemsWithEvalQuestionIds(evalQuestionIds);

                        if (evalItems != null)
                            if (evalItems.Count > 0)
                            {
                                var evalItemIds = evalItems.Select(v => v.EvaluationItemId).ToList();

                                var evalItemValues = teniacoApiBusiness.GetEvaluationItemValuesWithEvalItemIds(evalItemIds);


                                answerSheetEvaluationVMList.EvaluationCategoriesVMList = listOfEvaluationCategories;
                                answerSheetEvaluationVMList.EvaluationQuestionsVMList = evalQuestions;
                                answerSheetEvaluationVMList.EvaluationItemsVMList = evalItems;
                                answerSheetEvaluationVMList.EvaluationItemValuesVMList = evalItemValues;

                                //eval.EvaluationQuestionsVMList = new List<EvaluationQuestionsVM>();
                                //eval.EvaluationQuestionsVMList = evalQuestions;

                                //eval.EvaluationItemsVMList = new List<EvaluationItemsVM>();
                                //eval.EvaluationItemsVMList = evalItems;

                                //eval.EvaluationItemValuesVMList = new List<EvaluationItemValuesVM>();
                                //eval.EvaluationItemValuesVMList = evalItemValues;


                            }
                    }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = answerSheetEvaluationVMList;
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
        /// GetListOfEvaluationCategories
        /// </summary>
        /// <param name="getListOfEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfEvaluationCategories([FromBody] GetListOfEvaluationCategoriesPVM getListOfEvaluationCategoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfEvaluationCategories = teniacoApiBusiness.GetListOfEvaluationCategories(
                    getListOfEvaluationCategoriesPVM.jtStartIndex.Value,
                    getListOfEvaluationCategoriesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfEvaluationCategoriesPVM.ChildsUsersIds,
                    getListOfEvaluationCategoriesPVM.EvaluationId,
                    getListOfEvaluationCategoriesPVM.EvaluationCategoryTitleSearch,
                    getListOfEvaluationCategoriesPVM.jtSorting);

                #region Comments
                //var states = teniacoApiBusiness.GetListOfStates(null, getListOfEvaluationCategoriesPVM.Lang);
                //var cities = teniacoApiBusiness.GetListOfCities(null, getListOfEvaluationCategoriesPVM.Lang);

                //foreach (var evaluationCategory in listOfEvaluationCategories)
                //{
                //    if (evaluationCategory.StateId.HasValue)
                //        if (evaluationCategory.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == evaluationCategory.StateId.Value);
                //            evaluationCategory.StateName = state.StateName;
                //        }

                //    if (evaluationCategory.CityId.HasValue)
                //        if (evaluationCategory.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == evaluationCategory.CityId.Value);
                //            evaluationCategory.CityName = city.CityName;
                //        }
                //}
                #endregion


                foreach (var evaluationCategory in listOfEvaluationCategories)
                {

                    evaluationCategory.CountOfEvaluationQuestions = teniacoApiBusiness.TeniacoApiDb.EvaluationQuestions.Where(f => f.EvaluationCategoryId.Equals(evaluationCategory.EvaluationCategoryId)).Count();

                }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfEvaluationCategories;
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
        /// AddToEvaluationCategories
        /// </summary>
        /// <param name="addToEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToEvaluationCategories([FromBody] AddToEvaluationCategoriesPVM addToEvaluationCategoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int evaluationCategoryId = teniacoApiBusiness.AddToEvaluationCategories(
                    addToEvaluationCategoriesPVM.EvaluationCategoriesVM);

                if (evaluationCategoryId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationCategory";
                }
                else
                if (evaluationCategoryId > 0)
                {
                    addToEvaluationCategoriesPVM.EvaluationCategoriesVM.EvaluationCategoryId = evaluationCategoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToEvaluationCategoriesPVM.EvaluationCategoriesVM;
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
        /// UpdateEvaluationCategories
        /// </summary>
        /// <param name="updateEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateEvaluationCategories([FromBody] UpdateEvaluationCategoriesPVM updateEvaluationCategoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var EvaluationCategoriesVM = updateEvaluationCategoriesPVM.EvaluationCategoriesVM;

                int evaluationCategoryId = teniacoApiBusiness.UpdateEvaluationCategories(
                    ref EvaluationCategoriesVM,
                    updateEvaluationCategoriesPVM.ChildsUsersIds);

                if (evaluationCategoryId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluationCategory";
                }
                else
                if (evaluationCategoryId > 0)
                {
                    updateEvaluationCategoriesPVM.EvaluationCategoriesVM.EvaluationCategoryId = evaluationCategoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateEvaluationCategoriesPVM.EvaluationCategoriesVM;
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
        /// ToggleActivationEvaluationCategories
        /// </summary>
        /// <param name="toggleActivationEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationEvaluationCategories([FromBody] ToggleActivationEvaluationCategoriesPVM toggleActivationEvaluationCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationEvaluationCategories(
                    toggleActivationEvaluationCategoriesPVM.EvaluationCategoryId,
                    toggleActivationEvaluationCategoriesPVM.UserId.Value,
                    toggleActivationEvaluationCategoriesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteEvaluationCategories
        /// </summary>
        /// <param name="temporaryDeleteEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteEvaluationCategories([FromBody] TemporaryDeleteEvaluationCategoriesPVM
            temporaryDeleteEvaluationCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteEvaluationCategories(
                    temporaryDeleteEvaluationCategoriesPVM.EvaluationCategoryId,
                    temporaryDeleteEvaluationCategoriesPVM.UserId.Value,
                    temporaryDeleteEvaluationCategoriesPVM.ChildsUsersIds))
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
        /// CompleteDeleteEvaluationCategories
        /// </summary>
        /// <param name="completeDeleteEvaluationCategoriesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteEvaluationCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteEvaluationCategories([FromBody] CompleteDeleteEvaluationCategoriesPVM completeDeleteEvaluationCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteEvaluationCategories(completeDeleteEvaluationCategoriesPVM.EvaluationCategoryId, completeDeleteEvaluationCategoriesPVM.ChildsUsersIds))
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
        /// GetEvaluationCategoryWithEvaluationCategoryId
        /// </summary>
        /// <param name="GetEvaluationCategoryWithEvaluationCategoryId"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetEvaluationCategoryWithEvaluationCategoryId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetEvaluationCategoryWithEvaluationCategoryId([FromBody] GetEvaluationCategoryWithEvaluationCategoryIdPVM
            getEvaluationCategoryWithEvaluationCategoryIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var evaluationCategory = teniacoApiBusiness.GetEvaluationCategoryWithEvaluationCategoryId(
                    getEvaluationCategoryWithEvaluationCategoryIdPVM.EvaluationCategoryId/*,
                    updateEvaluationCategoriesPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = evaluationCategory;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetAllDivisionOfEvaluationsListByParentId
        /// </summary>
        /// <param name="getAllDivisionOfEvaluationsListByParentIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllDivisionOfEvaluationsListByParentId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllDivisionOfEvaluationsListByParentId([FromBody] GetAllDivisionOfEvaluationsListByParentIdPVM getAllDivisionOfEvaluationsListByParentIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds == null)
                    {
                        getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds.Count == 0)
                        getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds.Count == 1)
                        if (getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var answerSheetEvaluationVMList = new AnswerSheetEvaluationVM();


                var evalCategories = teniacoApiBusiness.GetAllDivisionOfEvaluationsListByParentId(
                   getAllDivisionOfEvaluationsListByParentIdPVM.ChildsUsersIds,
                   getAllDivisionOfEvaluationsListByParentIdPVM.EvaluationId);


                var evalCategoriesIds = evalCategories.Select(p => p.EvaluationCategoryId).ToList();

                var evalQuestions = teniacoApiBusiness.GetEvaluationQuestionsWithEvalCategoriesIds(evalCategoriesIds);



                if (evalQuestions != null)
                    if (evalQuestions.Count > 0)
                    {
                        var evalQuestionIds = evalQuestions.Select(i => i.EvaluationQuestionId).ToList();

                        var evalItems = teniacoApiBusiness.GetEvaluationItemsWithEvalQuestionIds(evalQuestionIds);

                        if (evalItems != null)
                            if (evalItems.Count > 0)
                            {
                                var evalItemIds = evalItems.Select(v => v.EvaluationItemId).ToList();

                                var evalItemValues = teniacoApiBusiness.GetEvaluationItemValuesWithEvalItemIds(evalItemIds);


                                answerSheetEvaluationVMList.EvaluationCategoriesVMList = evalCategories;
                                answerSheetEvaluationVMList.EvaluationQuestionsVMList = evalQuestions;
                                answerSheetEvaluationVMList.EvaluationItemsVMList = evalItems;

                                //answerSheetEvaluationVMList.EvaluationItemValuesVMList = evalItemValues;

                                //answerSheetEvaluationVMList.EvaluationItemValuesVMList = evalItemValues.Where(u => u.ParentId.Equals(getAllDivisionOfEvaluationsListByParentIdPVM.ParentId)).ToList();

                                answerSheetEvaluationVMList.EvaluationItemValuesVMList = evalItemValues.Where(u => u.ParentId.Equals(getAllDivisionOfEvaluationsListByParentIdPVM.ParentId) && u.ParentType.Equals(getAllDivisionOfEvaluationsListByParentIdPVM.ParentType)).ToList();


                            }

                    }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = answerSheetEvaluationVMList;
                //jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);


            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);

        }

    }
}

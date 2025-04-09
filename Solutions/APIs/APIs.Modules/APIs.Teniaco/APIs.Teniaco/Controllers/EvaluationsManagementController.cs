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
    /// EvaluationsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class EvaluationsManagementController : ApiBaseController
    {
        /// <summary>
        /// EvaluationsManagement
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
        public EvaluationsManagementController(
            IHostEnvironment _hostingEnvironment,
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
            base(_hostingEnvironment,
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
        /// GetAllEvaluationsList
        /// </summary>
        /// <param name="getAllEvaluationsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllEvaluationsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllEvaluationsList([FromBody] GetAllEvaluationsListPVM getAllEvaluationsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //if (!string.IsNullOrEmpty(token))
                //{
                //    if (getAllEvaluationsListPVM.ChildsUsersIds == null)
                //    {
                //        getAllEvaluationsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //    }
                //    else
                //    if (getAllEvaluationsListPVM.ChildsUsersIds.Count == 0)
                //        getAllEvaluationsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //    else
                //    if (getAllEvaluationsListPVM.ChildsUsersIds.Count == 1)
                //        if (getAllEvaluationsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                //            getAllEvaluationsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //}


                var listOfEvaluation = teniacoApiBusiness.GetAllEvaluationsList(getAllEvaluationsListPVM.EvaluationSubjectId,
                    getAllEvaluationsListPVM.EvaluationTitle);
                jsonResultWithRecordsObjectVM.Result = "OK";

                jsonResultWithRecordsObjectVM.Records = listOfEvaluation;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfEvaluation.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);

        }



        /// <summary>
        /// GetListOfEvaluations
        /// </summary>
        /// <param name="getListOfEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfEvaluations")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetListOfEvaluations([FromBody] GetListOfEvaluationsPVM getListOfEvaluationsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //if (!string.IsNullOrEmpty(token))
                //{
                //    if (getListOfInvestorsPVM.ChildsUsersIds == null)
                //    {
                //        getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //    }
                //    else
                //    if (getListOfInvestorsPVM.ChildsUsersIds.Count == 0)
                //        getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //    else
                //    if (getListOfInvestorsPVM.ChildsUsersIds.Count == 1)
                //        if (getListOfInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                //            getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                //}

                int listCount = 0;
                var listOfEvaluations = teniacoApiBusiness.GetListOfEvaluations(
                    getListOfEvaluationsPVM.jtStartIndex.Value,
                    getListOfEvaluationsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfEvaluationsPVM.ChildsUsersIds,
                    getListOfEvaluationsPVM.EvaluationTitle,
                    getListOfEvaluationsPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfEvaluations;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfEvaluations.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




        /// <summary>
        /// AddToEvaluations
        /// </summary>
        /// <param name="addToEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("AddToEvaluations")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult AddToEvaluations([FromBody] AddToEvaluationsPVM addToEvaluationsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                //if (!string.IsNullOrEmpty(token))
                //{
                //    if (addToInvestorsPVM.ChildsUsersIds == null)
                //    {
                //        addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //    }
                //    else
                //    {
                //        if (addToInvestorsPVM.ChildsUsersIds.Count == 0)
                //        {
                //            addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //        }
                //        else
                //        {
                //            if (addToInvestorsPVM.ChildsUsersIds.Count == 1)
                //                if (addToInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                //                    addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                //        }

                //    }
                //addToInvestorsPVM.InvestorsVM.CreateEnDate = DateTime.Now;
                //addToInvestorsPVM.InvestorsVM.CreateTime = PersianDate.TimeNow;
                //addToInvestorsPVM.InvestorsVM.UserIdCreator = this.userId.Value;
                //addToInvestorsPVM.InvestorsVM.IsActivated = true;
                //addToInvestorsPVM.InvestorsVM.IsDeleted = false;
                //}

                int EvaluationId = teniacoApiBusiness.AddToEvaluations(addToEvaluationsPVM.EvaluationsVM);



                if (EvaluationId > 0)
                {
                    addToEvaluationsPVM.EvaluationsVM.EvaluationId = EvaluationId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToEvaluationsPVM.EvaluationsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "خطا";
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }




        /// <summary>
        /// UpdateEvaluations
        /// </summary>
        /// <param name="updateEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateEvaluations")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateEvaluations([FromBody] UpdateEvaluationsPVM updateEvaluationsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });


            try
            {

                var evaluationsVM = updateEvaluationsPVM.EvaluationsVM;

                int evaluationId = teniacoApiBusiness.UpdateEvaluations(
                    ref evaluationsVM,
                    updateEvaluationsPVM.ChildsUsersIds);


                if (evaluationId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateEvaluation";
                }
                else
                if (evaluationId > 0)
                {
                    updateEvaluationsPVM.EvaluationsVM.EvaluationId = evaluationId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateEvaluationsPVM.EvaluationsVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);




            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }




        /// <summary>
        /// ToggleActivationEvaluations
        /// </summary>
        /// <param name="toggleActivationEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("ToggleActivationEvaluations")]
        [ProducesResponseType(typeof(JsonResultObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationEvaluations([FromBody] ToggleActivationEvaluationsPVM toggleActivationEvaluationsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.ToggleActivationEvaluations(toggleActivationEvaluationsPVM.EvaluationId,
                    toggleActivationEvaluationsPVM.UserId.Value,
                    toggleActivationEvaluationsPVM.ChildsUsersIds))
                    jsonResultObjectVM.Result = "OK";
                else
                    jsonResultObjectVM.Result = "ERROR";
                return new JsonResult(jsonResultObjectVM);

            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);

        }



        /// <summary>
        /// TemporaryDeleteEvaluations
        /// </summary>
        /// <param name="temporaryDeleteEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("TemporaryDeleteEvaluations")]
        [ProducesResponseType(typeof(JsonResultObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteEvaluations([FromBody] TemporaryDeleteEvaluationsPVM temporaryDeleteEvaluationsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteEvaluations(temporaryDeleteEvaluationsPVM.EvaluationId,
                    temporaryDeleteEvaluationsPVM.UserId.Value,
                    temporaryDeleteEvaluationsPVM.ChildsUsersIds))
                    jsonResultObjectVM.Result = "OK";

                return new JsonResult(jsonResultObjectVM);


            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }



        /// <summary>
        /// CompleteDeleteEvaluations
        /// </summary>
        /// <param name="completeDeleteEvaluationsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("CompleteDeleteEvaluations")]
        [ProducesResponseType(typeof(JsonResultObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult CompleteDeleteEvaluations([FromBody] CompleteDeleteEvaluationsPVM completeDeleteEvaluationsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteEvaluations(completeDeleteEvaluationsPVM.EvaluationId,
                    completeDeleteEvaluationsPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }


        /// <summary>
        /// GetEvaluationsWithEvaluationsId
        /// </summary>
        /// <param name="getEvaluationsWithEvaluationsIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetEvaluationsWithEvaluationId")]
        [ProducesResponseType(typeof(JsonResultWithRecordObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetEvaluationsWithEvaluationId([FromBody] GetEvaluationsWithEvaluationIdPVM getEvaluationsWithEvaluationsIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds == null)
                    {
                        getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds.Count == 0)
                        getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds.Count == 1)
                        if (getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var result = teniacoApiBusiness.GetEvaluationsWithEvaluationId(getEvaluationsWithEvaluationsIdPVM.EvaluationId, getEvaluationsWithEvaluationsIdPVM.ChildsUsersIds);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = result;

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

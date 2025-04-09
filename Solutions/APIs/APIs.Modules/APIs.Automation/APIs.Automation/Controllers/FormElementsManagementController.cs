using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
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
using VM.PVM.Public;
using VM.PVM.Automation;
using VM.Automation;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// FormElementsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class FormElementsManagementController : ApiBaseController
    {
        /// <summary>
        /// FormElementsManagement
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
        public FormElementsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFormElementsListPVM
        /// </summary>
        /// <param name="getAllFormElementsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllFormElementsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllFormElementsList(GetAllFormElementsListPVM getAllFormElementsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfFormElements = automationApiBusiness.GetAllFormElementsList(ref listCount,
                    getAllFormElementsListPVM.FormId,
                    getAllFormElementsListPVM.FormElementTitle);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFormElements;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfFormElements);

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
        /// GetListOfFormElements
        /// </summary>
        /// <param name="getListOfFormElementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfFormElements(GetListOfFormElementsPVM getListOfFormElementsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfFormElements = automationApiBusiness.GetListOfFormElements(getListOfFormElementsPVM.jtStartIndex.Value,
                    getListOfFormElementsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfFormElementsPVM.FormId,
                    getListOfFormElementsPVM.FormElementTitle);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFormElements;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfFormElements);

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
        /// AddToFormElements
        /// </summary>
        /// <param name="addToFormElementsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToFormElements([FromBody] AddToFormElementsPVM
            addToFormElementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int formElementId = automationApiBusiness.AddToFormElements(
                    addToFormElementsPVM.FormElementsVM,
                    addToFormElementsPVM.ChildsUsersIds);


                if (formElementId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateFormElement";
                }
                else
                if (formElementId > 0)
                {
                    addToFormElementsPVM.FormElementsVM.FormElementId = formElementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToFormElementsPVM.FormElementsVM;
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
        /// GetFormElementWithFormElementId
        /// </summary>
        /// <param name="updateFormElementsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetFormElementWithFormElementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetFormElementWithFormElementId([FromBody] GetFormElementWithFormElementIdPVM
            getFormElementWithFormElementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var formElement = automationApiBusiness.GetFormElementWithFormElementId(
                    getFormElementWithFormElementIdPVM.FormElementId/*,
                    updateFormElementsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = formElement;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateFormElements
        /// </summary>
        /// <param name="updateFormElementsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateFormElements([FromBody] UpdateFormElementsPVM
            updateFormElementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var formElementsVM = updateFormElementsPVM.FormElementsVM;

                int formElementId = automationApiBusiness.UpdateFormElements(
                    ref formElementsVM,
                    updateFormElementsPVM.ChildsUsersIds);

                if (formElementId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateFormElement";
                }
                else
                if (formElementId > 0)
                {
                    updateFormElementsPVM.FormElementsVM.FormElementId = formElementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateFormElementsPVM.FormElementsVM;
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
        /// ToggleActivationFormElements
        /// </summary>
        /// <param name="toggleActivationFormElementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationFormElements([FromBody] ToggleActivationFormElementsPVM
            toggleActivationFormElementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.ToggleActivationFormElements(
                    toggleActivationFormElementsPVM.FormElementId,
                    toggleActivationFormElementsPVM.UserId.Value,
                    toggleActivationFormElementsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteFormElements
        /// </summary>
        /// <param name="temporaryDeleteFormElementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteFormElements([FromBody] TemporaryDeleteFormElementsPVM
            temporaryDeleteFormElementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (automationApiBusiness.TemporaryDeleteFormElements(
                    temporaryDeleteFormElementsPVM.FormElementId,
                    temporaryDeleteFormElementsPVM.UserId.Value,
                    temporaryDeleteFormElementsPVM.ChildsUsersIds))
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
        /// CompleteDeleteFormElements
        /// </summary>
        /// <param name="completeDeleteFormElementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteFormElements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteFormElements([FromBody] CompleteDeleteFormElementsPVM completeDeleteFormElementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (automationApiBusiness.CompleteDeleteFormElements(
                    completeDeleteFormElementsPVM.FormElementId,
                    completeDeleteFormElementsPVM.ChildsUsersIds))
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

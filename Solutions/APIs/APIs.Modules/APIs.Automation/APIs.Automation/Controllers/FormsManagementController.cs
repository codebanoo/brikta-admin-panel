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
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// FormsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class FormsManagementController : ApiBaseController
    {
        /// <summary>
        /// FormsManagement
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
        public FormsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFormsListPVM
        /// </summary>
        /// <param name="getAllFormsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllFormsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllFormsList(GetAllFormsListPVM getAllFormsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfForms = automationApiBusiness.GetAllFormsList(ref listCount,
                    getAllFormsListPVM.FormName);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfForms;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfForms);

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
        /// GetListOfForms
        /// </summary>
        /// <param name="getListOfFormsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfForms(GetListOfFormsPVM getListOfFormsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfForms = automationApiBusiness.GetListOfForms(getListOfFormsPVM.jtStartIndex.Value,
                    getListOfFormsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfFormsPVM.FormName);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfForms;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfForms);

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
        /// AddToForms
        /// </summary>
        /// <param name="addToFormsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToForms([FromBody] AddToFormsPVM
            addToFormsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int formId = automationApiBusiness.AddToForms(
                    addToFormsPVM.FormsVM,
                    addToFormsPVM.ChildsUsersIds);


                if (formId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateForm";
                }
                else
                if (formId > 0)
                {
                    addToFormsPVM.FormsVM.FormId = formId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToFormsPVM.FormsVM;
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
        /// GetFormWithFormId
        /// </summary>
        /// <param name="getFormWithFormIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetFormWithFormId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetFormWithFormId([FromBody] GetFormWithFormIdPVM
            getFormWithFormIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var form = automationApiBusiness.GetFormWithFormId(
                    getFormWithFormIdPVM.FormId/*,
                    updateFormsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = form;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetFormAndFieldsWithFormId
        /// </summary>
        /// <param name="fetFormAndFieldsWithFormIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetFormAndFieldsWithFormId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetFormAndFieldsWithFormId([FromBody] GetFormAndFieldsWithFormIdPVM
            fetFormAndFieldsWithFormIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var form = automationApiBusiness.GetFormAndFieldsWithFormId(
                    fetFormAndFieldsWithFormIdPVM.FormId/*,
                    updateFormsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = form;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateForms
        /// </summary>
        /// <param name="updateFormsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateForms([FromBody] UpdateFormsPVM
            updateFormsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var formsVM = updateFormsPVM.FormsVM;

                int formId = automationApiBusiness.UpdateForms(
                    ref formsVM,
                    updateFormsPVM.ChildsUsersIds);

                if (formId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateForm";
                }
                else
                if (formId > 0)
                {
                    updateFormsPVM.FormsVM.FormId = formId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateFormsPVM.FormsVM;
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
        /// ToggleActivationForms
        /// </summary>
        /// <param name="toggleActivationFormsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationForms([FromBody] ToggleActivationFormsPVM
            toggleActivationFormsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.ToggleActivationForms(
                    toggleActivationFormsPVM.FormId,
                    toggleActivationFormsPVM.UserId.Value,
                    toggleActivationFormsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteForms
        /// </summary>
        /// <param name="temporaryDeleteFormsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteForms([FromBody] TemporaryDeleteFormsPVM
            temporaryDeleteFormsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (automationApiBusiness.TemporaryDeleteForms(
                    temporaryDeleteFormsPVM.FormId,
                    temporaryDeleteFormsPVM.UserId.Value,
                    temporaryDeleteFormsPVM.ChildsUsersIds))
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
        /// CompleteDeleteForms
        /// </summary>
        /// <param name="completeDeleteFormsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteForms")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteForms([FromBody] CompleteDeleteFormsPVM completeDeleteFormsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (automationApiBusiness.CompleteDeleteForms(
                    completeDeleteFormsPVM.FormId,
                    completeDeleteFormsPVM.ChildsUsersIds))
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

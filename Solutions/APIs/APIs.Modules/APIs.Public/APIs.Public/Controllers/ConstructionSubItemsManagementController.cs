using APIs.Automation.Models.Business;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
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
using APIs.CustomAttributes;
using APIs.Core.Controllers;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Public.Controllers
{
    /// <summary>
    /// ConstructionSubItemsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ConstructionSubItemsManagementController : ApiBaseController
    {
        /// <summary>
        /// ConstructionSubItemsManagement
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
        public ConstructionSubItemsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConstructionSubItemsListPVM
        /// </summary>
        /// <param name="getAllConstructionSubItemsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllConstructionSubItemsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConstructionSubItemsList(GetAllConstructionSubItemsListPVM getAllConstructionSubItemsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfConstructionSubItems = publicApiBusiness.GetAllConstructionSubItemsList(ref listCount,
                    getAllConstructionSubItemsListPVM.ConstructionItemId,
                    getAllConstructionSubItemsListPVM.ConstructionSubItemTitle);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionSubItems;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfConstructionSubItems);

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
        /// GetListOfConstructionSubItems
        /// </summary>
        /// <param name="getListOfConstructionSubItemsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConstructionSubItems(GetListOfConstructionSubItemsPVM getListOfConstructionSubItemsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfConstructionSubItems = publicApiBusiness.GetListOfConstructionSubItems(getListOfConstructionSubItemsPVM.jtStartIndex.Value,
                    getListOfConstructionSubItemsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfConstructionSubItemsPVM.ConstructionItemId,
                    getListOfConstructionSubItemsPVM.ConstructionSubItemTitle,
                    getListOfConstructionSubItemsPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionSubItems;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfConstructionSubItems);

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
        /// AddToConstructionSubItems
        /// </summary>
        /// <param name="addToConstructionSubItemsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConstructionSubItems([FromBody] AddToConstructionSubItemsPVM
            addToConstructionSubItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                long ConstructionSubItemId = publicApiBusiness.AddToConstructionSubItems(
                    addToConstructionSubItemsPVM.ConstructionSubItemsVM/*,
                    addToConstructionSubItemsPVM.ChildsUsersIds*/);


                if (ConstructionSubItemId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateConstructionSubItem";
                }
                else
                if (ConstructionSubItemId > 0)
                {
                    addToConstructionSubItemsPVM.ConstructionSubItemsVM.ConstructionSubItemId = ConstructionSubItemId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToConstructionSubItemsPVM.ConstructionSubItemsVM;
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
        /// GetConstructionSubItemWithConstructionSubItemId
        /// </summary>
        /// <param name="updateConstructionSubItemsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetConstructionSubItemWithConstructionSubItemId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConstructionSubItemWithConstructionSubItemId([FromBody] GetConstructionSubItemWithConstructionSubItemIdPVM
            getConstructionSubItemWithConstructionSubItemIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var ConstructionSubItem = publicApiBusiness.GetConstructionSubItemWithConstructionSubItemId(
                    getConstructionSubItemWithConstructionSubItemIdPVM.ConstructionSubItemId/*,
                    updateConstructionSubItemsPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = ConstructionSubItem;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateConstructionSubItems
        /// </summary>
        /// <param name="updateConstructionSubItemsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConstructionSubItems([FromBody] UpdateConstructionSubItemsPVM
            updateConstructionSubItemsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var ConstructionSubItemsVM = updateConstructionSubItemsPVM.ConstructionSubItemsVM;

                long ConstructionSubItemId = publicApiBusiness.UpdateConstructionSubItems(
                    ref ConstructionSubItemsVM/*,
                    updateConstructionSubItemsPVM.ChildsUsersIds*/);

                if (ConstructionSubItemId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateConstructionSubItem";
                }
                else
                if (ConstructionSubItemId > 0)
                {
                    updateConstructionSubItemsPVM.ConstructionSubItemsVM.ConstructionSubItemId = ConstructionSubItemId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateConstructionSubItemsPVM.ConstructionSubItemsVM;
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
        /// ToggleActivationConstructionSubItems
        /// </summary>
        /// <param name="toggleActivationConstructionSubItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationConstructionSubItems([FromBody] ToggleActivationConstructionSubItemsPVM
            toggleActivationConstructionSubItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationConstructionSubItems(
                    toggleActivationConstructionSubItemsPVM.ConstructionSubItemId,
                    toggleActivationConstructionSubItemsPVM.UserId.Value,
                    toggleActivationConstructionSubItemsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteConstructionSubItems
        /// </summary>
        /// <param name="temporaryDeleteConstructionSubItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteConstructionSubItems([FromBody] TemporaryDeleteConstructionSubItemsPVM
            temporaryDeleteConstructionSubItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteConstructionSubItems(
                    temporaryDeleteConstructionSubItemsPVM.ConstructionSubItemId,
                    temporaryDeleteConstructionSubItemsPVM.UserId.Value,
                    temporaryDeleteConstructionSubItemsPVM.ChildsUsersIds))
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
        /// CompleteDeleteConstructionSubItems
        /// </summary>
        /// <param name="completeDeleteConstructionSubItemsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteConstructionSubItems")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteConstructionSubItems([FromBody] CompleteDeleteConstructionSubItemsPVM completeDeleteConstructionSubItemsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteConstructionSubItems(
                    completeDeleteConstructionSubItemsPVM.ConstructionSubItemId,
                    completeDeleteConstructionSubItemsPVM.ChildsUsersIds))
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

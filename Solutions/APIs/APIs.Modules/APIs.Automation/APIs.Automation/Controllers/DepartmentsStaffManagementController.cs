using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using VM.Base;
using VM.PVM.Automation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using APIs.Teniaco.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// DepartmentsStaffManagement
    /// </summary>
    [CustomApiAuthentication]
    public class DepartmentsStaffManagementController : ApiBaseController
    {
        /// <summary>
        /// DepartmentsStaffManagement
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
        public DepartmentsStaffManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfDepartmentsStaff
        /// </summary>
        /// <param name="getListOfDepartmentsStaffPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfDepartmentsStaff([FromBody] GetListOfDepartmentsStaffPVM
            getListOfDepartmentsStaffPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfDepartmentsStaff = automationApiBusiness.GetListOfDepartmentsStaff(
                    getListOfDepartmentsStaffPVM.jtStartIndex.Value,
                    getListOfDepartmentsStaffPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfDepartmentsStaffPVM.ChildsUsersIds,
                    getListOfDepartmentsStaffPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDepartmentsStaff;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfDepartmentsStaff);
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
        /// GetAllDepartmentsStaffList
        /// </summary>
        /// <param name="getAllDepartmentsStaffListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllDepartmentsStaffList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllDepartmentsStaffList([FromBody] GetAllDepartmentsStaffListPVM
            getAllDepartmentsStaffListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                var listOfDepartmentsStaff = automationApiBusiness.GetAllDepartmentsStaffList(getAllDepartmentsStaffListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDepartmentsStaff;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// AddToDepartmentsStaff
        /// </summary>
        /// <param name="addToDepartmentsStaffPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToDepartmentsStaff([FromBody] AddToDepartmentsStaffPVM
            addToDepartmentsStaffPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                long deviceRfidCodeId = automationApiBusiness.AddToDepartmentsStaff(
                    addToDepartmentsStaffPVM.DepartmentsStaffVM,
                    addToDepartmentsStaffPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = deviceRfidCodeId;

                return new JsonResult(jsonResultWithRecordObjectVM);
                //return jsonResultWithRecordObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }

        /// <summary>
        /// GetDepartmentStaffWithDepartmentStaffId
        /// </summary>
        /// <param name="getDepartmentStaffWithDepartmentStaffIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetDepartmentStaffWithDepartmentStaffId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetDepartmentStaffWithDepartmentStaffId([FromBody] GetDepartmentStaffWithDepartmentStaffIdPVM
            getDepartmentStaffWithDepartmentStaffIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var departmentsStaffVM = automationApiBusiness.GetDepartmentStaffWithDepartmentStaffId(
                    getDepartmentStaffWithDepartmentStaffIdPVM.DepartmentStaffId,
                    getDepartmentStaffWithDepartmentStaffIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = departmentsStaffVM;

                return new JsonResult(jsonResultWithRecordObjectVM);
                //return jsonResultWithRecordObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }

        /// <summary>
        /// UpdateDepartmentsStaff
        /// </summary>
        /// <param name="updateDepartmentsStaffPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateDepartmentsStaff([FromBody] UpdateDepartmentsStaffPVM
            updateDepartmentsStaffPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var departmentsStaffVM = updateDepartmentsStaffPVM.DepartmentsStaffVM;
                if (automationApiBusiness.UpdateDepartmentsStaff(
                    ref departmentsStaffVM,
                    updateDepartmentsStaffPVM.ChildsUsersIds))
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = departmentsStaffVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                    //return jsonResultWithRecordObjectVM;
                }
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }

        /// <summary>
        /// ToggleActivationDepartmentsStaff
        /// </summary>
        /// <param name="toggleActivationDepartmentsStaffPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationDepartmentsStaff([FromBody]
            ToggleActivationDepartmentsStaffPVM toggleActivationDepartmentsStaffPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.ToggleActivationDepartmentsStaff(
                    toggleActivationDepartmentsStaffPVM.DepartmentStaffId,
                    toggleActivationDepartmentsStaffPVM.UserId.Value,
                    toggleActivationDepartmentsStaffPVM.ChildsUsersIds))
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
        /// TemporaryDeleteDepartmentsStaff
        /// </summary>
        /// <param name="temporaryDeleteDepartmentsStaffPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteDepartmentsStaff([FromBody]
            TemporaryDeleteDepartmentsStaffPVM temporaryDeleteDepartmentsStaffPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.TemporaryDeleteDepartmentsStaff(
                    temporaryDeleteDepartmentsStaffPVM.DepartmentStaffId,
                    temporaryDeleteDepartmentsStaffPVM.UserId.Value,
                    temporaryDeleteDepartmentsStaffPVM.ChildsUsersIds))
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
        /// CompleteDeleteDepartmentsStaff
        /// </summary>
        /// <param name="completeDeleteDepartmentsStaffPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteDepartmentsStaff")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteDepartmentsStaff([FromBody]
            CompleteDeleteDepartmentsStaffPVM completeDeleteDepartmentsStaffPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.CompleteDeleteDepartmentsStaff(
                    completeDeleteDepartmentsStaffPVM.DepartmentStaffId,
                    completeDeleteDepartmentsStaffPVM.ChildsUsersIds,
                    ref returnMessage))
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
    }
}

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

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// MyDepartmentsDirectorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyDepartmentsDirectorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyDepartmentsDirectorsManagement
        /// </summary>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="_httpContextAccessor"></param>
        /// <param name="_actionContextAccessor"></param>
        /// <param name="_configurationRoot"></param>
        /// <param name="_consoleBusiness"></param>
        /// <param name="_automationApiBusiness"></param>
        /// <param name="_publicApiBusiness"></param>
        /// <param name="_teniacoApiBusiness"></param>
        /// <param name="_appSettingsSection"></param>
        public MyDepartmentsDirectorsManagementController(IHostEnvironment _hostingEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IConsoleBusiness _consoleBusiness,
            IAutomationApiBusiness _automationApiBusiness,
            IPublicApiBusiness _publicApiBusiness,
            ITeniacoApiBusiness _teniacoApiBusiness,
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
                _appSettingsSection)
        {

        }

        /// <summary>
        /// GetListOfMyDepartmentsDirectors
        /// </summary>
        /// <param name="getListOfMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyDepartmentsDirectors([FromBody] GetListOfMyDepartmentsDirectorsPVM
            getListOfMyDepartmentsDirectorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfMyDepartmentsDirectors = automationApiBusiness.GetMyDepartmentsDirectorsList(
                    getListOfMyDepartmentsDirectorsPVM.jtStartIndex.Value,
                    getListOfMyDepartmentsDirectorsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyDepartmentsDirectorsPVM.jtSorting,
                    getListOfMyDepartmentsDirectorsPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyDepartmentsDirectors;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfMyDepartmentsDirectors);
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
        /// GetAllMyDepartmentsDirectorsList
        /// </summary>
        /// <param name="getAllMyDepartmentsDirectorsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyDepartmentsDirectorsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyDepartmentsDirectorsList([FromBody] GetAllMyDepartmentsDirectorsListPVM
            getAllMyDepartmentsDirectorsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                var listOfMyDepartmentsDirectors = automationApiBusiness.GetAllMyDepartmentsDirectorsList(getAllMyDepartmentsDirectorsListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyDepartmentsDirectors;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// AddToMyDepartmentsDirectors
        /// </summary>
        /// <param name="addToMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyDepartmentsDirectors([FromBody] AddToMyDepartmentsDirectorsPVM
            addToMyDepartmentsDirectorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.AddToMyDepartmentsDirectors(
                    addToMyDepartmentsDirectorsPVM.MyDepartmentsDirectorsVM,
                    addToMyDepartmentsDirectorsPVM.ChildsUsersIds))
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    //jsonResultWithRecordObjectVM.Record = myDepartmentDirectorId;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
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
        /// GetMyDepartmentDirectorWithMyDepartmentDirectorId
        /// </summary>
        /// <param name="getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetMyDepartmentDirectorWithMyDepartmentDirectorId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyDepartmentDirectorWithMyDepartmentDirectorId([FromBody] GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM
            getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var myDepartmentsDirectorsVM = automationApiBusiness.GetMyDepartmentsDirectorsWithMyDepartmentDirectorId(
                    getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM.ChildsUsersIds,
                    getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM.MyDepartmentId,
                    getMyDepartmentDirectorWithMyDepartmentDirectorIdPVM.UserId.Value);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = myDepartmentsDirectorsVM;

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
        /// UpdateMyDepartmentsDirectors
        /// </summary>
        /// <param name="updateMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyDepartmentsDirectors([FromBody] UpdateMyDepartmentsDirectorsPVM
            updateMyDepartmentsDirectorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var myDepartmentsDirectorsVM = updateMyDepartmentsDirectorsPVM.MyDepartmentsDirectorsVM;
                if (automationApiBusiness.UpdateMyDepartmentDirector(
                    ref myDepartmentsDirectorsVM,
                    updateMyDepartmentsDirectorsPVM.ChildsUsersIds))
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = myDepartmentsDirectorsVM;

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
        /// ToggleActivationMyDepartmentsDirectors
        /// </summary>
        /// <param name="toggleActivationMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMyDepartmentsDirectors([FromBody]
            ToggleActivationMyDepartmentsDirectorsPVM toggleActivationMyDepartmentsDirectorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.ToggleActivationMyDepartmentsDirectors(
                    toggleActivationMyDepartmentsDirectorsPVM.ChildsUsersIds,
                    toggleActivationMyDepartmentsDirectorsPVM.MyDepartmentId,
                    toggleActivationMyDepartmentsDirectorsPVM.UserId.Value))
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
        /// TemporaryDeleteMyDepartmentsDirectors
        /// </summary>
        /// <param name="temporaryDeleteMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMyDepartmentsDirectors([FromBody]
            TemporaryDeleteMyDepartmentsDirectorsPVM temporaryDeleteMyDepartmentsDirectorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.TemporaryDeleteMyDepartmentsDirectors(
                    temporaryDeleteMyDepartmentsDirectorsPVM.ChildsUsersIds,
                    temporaryDeleteMyDepartmentsDirectorsPVM.MyDepartmentId,
                    temporaryDeleteMyDepartmentsDirectorsPVM.UserId.Value))
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
        /// CompleteDeleteMyDepartmentsDirectors
        /// </summary>
        /// <param name="completeDeleteMyDepartmentsDirectorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyDepartmentsDirectors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyDepartmentsDirectors([FromBody]
            CompleteDeleteMyDepartmentsDirectorsPVM completeDeleteMyDepartmentsDirectorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.CompleteDeleteMyDepartmentsDirectors(
                    completeDeleteMyDepartmentsDirectorsPVM.ChildsUsersIds,
                    completeDeleteMyDepartmentsDirectorsPVM.MyDepartmentId,
                    completeDeleteMyDepartmentsDirectorsPVM.UserId.Value,
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

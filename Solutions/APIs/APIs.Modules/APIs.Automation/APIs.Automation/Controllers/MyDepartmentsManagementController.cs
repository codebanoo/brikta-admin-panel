using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.Public.Models.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json;

using VM.Automation;
using VM.PVM.Automation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using APIs.CustomAttributes.Helper;
using Microsoft.Extensions.Options;
using VM.Base;
using APIs.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using APIs.Teniaco.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// MyDepartmentsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyDepartmentsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyDepartmentsManagement
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
        public MyDepartmentsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMyDepartmentsList
        /// </summary>
        /// <param name="getAllMyDepartmentsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyDepartmentsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyDepartmentsList([FromBody] GetAllMyDepartmentsListPVM getAllMyDepartmentsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyDepartmentsListPVM.ChildsUsersIds == null)
                    {
                        getAllMyDepartmentsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyDepartmentsListPVM.ChildsUsersIds.Count == 0)
                        getAllMyDepartmentsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyDepartmentsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyDepartmentsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyDepartmentsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var allMyDepartmentsList = automationApiBusiness.GetAllMyDepartmentsList(getAllMyDepartmentsListPVM.ChildsUsersIds,
                    getAllMyDepartmentsListPVM.MyCompanyId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = allMyDepartmentsList;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(allMyDepartmentsList);
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);

                //return new JsonResult(new
                //{
                //    JsonResultWithRecordsObjectVM = jsonResultWithRecordsObjectVM
                //});

                //return new JsonResult(new
                //{
                //    JsonResultWithRecordsObjectVM = jsonResultWithRecordsObjectVM
                //});

                //jsonResultWithRecordsObjectVM.ContentType = "application/json;";
                //jsonResultWithRecordsObjectVM.StatusCode = 200;

                //return jsonResultWithRecordsObjectVM;

                //return new JsonResult(new
                //{
                //    JsonResultWithRecordsObjectVM = jsonResultWithRecordsObjectVM
                //});

                //return new JsonResult(new
                //{
                //    Result = "OK",
                //    Records = allMyDepartmentsList,
                //    TotalRecordCount = listCount
                //});

                //return new JsonResult(new
                //{
                //    Result = "OK",
                //    Records = allMyDepartmentsList,
                //    TotalRecordCount = listCount
                //});

                //return allMyDepartmentsList;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);

            //return jsonResultWithRecordsObjectVM;

            //return null;
        }

        /// <summary>
        /// GetListOfMyDepartments
        /// </summary>
        /// <param name="getListOfMyDepartmentsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyDepartments([FromBody] GetListOfMyDepartmentsPVM getListOfMyDepartmentsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfMyDepartments = automationApiBusiness.GetListOfMyDepartments(
                    getListOfMyDepartmentsPVM.jtStartIndex.Value,
                    getListOfMyDepartmentsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyDepartmentsPVM.jtSorting,
                    getListOfMyDepartmentsPVM.ChildsUsersIds);

                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);
                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //var departmentsDirectorsList = automationApiBusiness.GetAllMyDepartmentsDirectorsList(getListOfMyDepartmentsPVM.ChildsUsersIds);

                //foreach (var myDepartment in listOfMyDepartments)
                //{
                //    if (myDepartment.MyDepartmentDirectorId.HasValue)
                //        if (myDepartment.MyDepartmentDirectorId.Value > 0)
                //        {
                //            var departmentsDirector = departmentsDirectorsList.FirstOrDefault(x => x.MyDepartmentsDirectorId == myDepartment.MyDepartmentDirectorId.Value);
                //            myDepartment.UserId = departmentsDirector.UserId;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyDepartments;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfMyDepartments);
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
        /// AddToMyDepartments
        /// </summary>
        /// <param name="addToMyDepartmentsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyDepartments([FromBody] AddToMyDepartmentsPVM addToMyDepartmentsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int myCompanyId = automationApiBusiness.AddToMyDepartments(addToMyDepartmentsPVM.MyDepartmentsVM,addToMyDepartmentsPVM.ChildsUsersIds);

                if (myCompanyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "Duplicate";
                }
                else
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = myCompanyId;
                }

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
        /// UpdateMyDepartments
        /// </summary>
        /// <param name="updateMyDepartmentsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyDepartments([FromBody] UpdateMyDepartmentsPVM updateMyDepartmentsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                var myCompaniesVM = updateMyDepartmentsPVM.MyDepartmentsVM;
                if (automationApiBusiness.UpdateMyDepartments(myCompaniesVM, updateMyDepartmentsPVM.ChildsUsersIds, ref returnMessage))
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = myCompaniesVM;

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
        /// ToggleActivationMyDepartments
        /// </summary>
        /// <param name="toggleActivationMyDepartmentsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMyDepartments([FromBody] ToggleActivationMyDepartmentsPVM toggleActivationMyDepartmentsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.ToggleActivationMyDepartment(
                    toggleActivationMyDepartmentsPVM.MyDepartmentId,
                    toggleActivationMyDepartmentsPVM.UserId.Value,
                    toggleActivationMyDepartmentsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteMyDepartments
        /// </summary>
        /// <param name="temporaryDeleteMyDepartmentsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMyDepartments([FromBody] TemporaryDeleteMyDepartmentsPVM temporaryDeleteMyDepartmentsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (automationApiBusiness.TemporaryDeleteMyDepartments(
                     temporaryDeleteMyDepartmentsPVM.ChildsUsersIds,
                    temporaryDeleteMyDepartmentsPVM.MyDepartmentId,
                   temporaryDeleteMyDepartmentsPVM.UserId.Value))
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
        /// CompleteDeleteMyDepartments
        /// </summary>
        /// <param name="completeDeleteMyDepartmentsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyDepartments")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyDepartments([FromBody] CompleteDeleteMyDepartmentsPVM completeDeleteMyDepartmentsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.CompleteDeleteMyDepartments(
                    completeDeleteMyDepartmentsPVM.MyDepartmentId,
                    completeDeleteMyDepartmentsPVM.ChildsUsersIds,
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

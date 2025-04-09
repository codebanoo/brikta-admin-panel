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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Projects;

namespace APIs.Projects.Controllers
{
    [CustomApiAuthentication]
    public class ConstructionProjectDelaysManagementController : ApiBaseController
    {
        /// <summary>
        /// ConstructionProjectDelaysManagement
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
        public ConstructionProjectDelaysManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConstructionProjectDelays
        /// </summary>
        /// <param name="getAllConstructionProjectDelaysListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConstructionProjectDelays([FromBody] GetAllConstructionProjectDelaysListPVM getAllConstructionProjectDelaysListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllConstructionProjectDelaysListPVM.ChildsUsersIds == null)
                    {
                        getAllConstructionProjectDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllConstructionProjectDelaysListPVM.ChildsUsersIds.Count == 0)
                        getAllConstructionProjectDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllConstructionProjectDelaysListPVM.ChildsUsersIds.Count == 1)
                        if (getAllConstructionProjectDelaysListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllConstructionProjectDelaysListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var getAllConstructionProjectDelaysList = projectsApiBusiness.GetAllConstructionProjectDelays(
                    getAllConstructionProjectDelaysListPVM.ChildsUsersIds,
                    getAllConstructionProjectDelaysListPVM.ConstructionProjectBillDelayId,
                    getAllConstructionProjectDelaysListPVM.ConstructionProjectId);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = getAllConstructionProjectDelaysList;
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
        /// GetListOfConstructionProjectDelays
        /// </summary>
        /// <param name="getListOfConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConstructionProjectDelays([FromBody] GetListOfConstructionProjectDelaysPVM
            getListOfConstructionProjectDelaysPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfConstructionProjectDelaysPVM.ChildsUsersIds == null)
                    {
                        getListOfConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfConstructionProjectDelaysPVM.ChildsUsersIds.Count == 0)
                        getListOfConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfConstructionProjectDelaysPVM.ChildsUsersIds.Count == 1)
                        if (getListOfConstructionProjectDelaysPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfConstructionProjectDelays = projectsApiBusiness.GetListOfConstructionProjectDelays(
                    getListOfConstructionProjectDelaysPVM.jtStartIndex.Value,
                    getListOfConstructionProjectDelaysPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfConstructionProjectDelaysPVM.ChildsUsersIds,
                    getListOfConstructionProjectDelaysPVM.ConstructionProjectBillDelayId.Value,
                    getListOfConstructionProjectDelaysPVM.ConstructionProjectId,
                    getListOfConstructionProjectDelaysPVM.jtSorting);

                var ConstructionBillDelaysIds = listOfConstructionProjectDelays.Select(c => c.ConstructionProjectBillDelayId).Distinct().ToList();
                var ConstructionBillDelaysList = projectsApiBusiness.ProjectsApiDb.ConstructionProjectBillDelays.Where(c=> ConstructionBillDelaysIds.Contains(c.ConstructionProjectBillDelayId)).ToList();
                
                foreach (var item in listOfConstructionProjectDelays)
                {
                    if (ConstructionBillDelaysList.Where(c => c.ConstructionProjectBillDelayId.Equals(item.ConstructionProjectBillDelayId)).Any())
                    {
                        item.ConstructionProjectBillDelayTitle = ConstructionBillDelaysList.Where(c => c.ConstructionProjectBillDelayId.Equals(item.ConstructionProjectBillDelayId)).FirstOrDefault().ConstructionProjectBillDelayTitle;
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjectDelays;
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
        /// AddToConstructionProjectDelays
        /// </summary>
        /// <param name="addToConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConstructionProjectDelays([FromBody] AddToConstructionProjectDelaysPVM addToConstructionProjectDelaysPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToConstructionProjectDelaysPVM.ChildsUsersIds == null)
                    {
                        addToConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToConstructionProjectDelaysPVM.ChildsUsersIds.Count == 0)
                        addToConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToConstructionProjectDelaysPVM.ChildsUsersIds.Count == 1)
                        if (addToConstructionProjectDelaysPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var constructionProjectDelayId = projectsApiBusiness.AddToConstructionProjectDelays(addToConstructionProjectDelaysPVM.ChildsUsersIds, addToConstructionProjectDelaysPVM.constructionProjectDelaysVM);
                if (constructionProjectDelayId != 0)
                {
                    addToConstructionProjectDelaysPVM.constructionProjectDelaysVM.ConstructionProjectsDelayId = constructionProjectDelayId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToConstructionProjectDelaysPVM.constructionProjectDelaysVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateConstructionProjectDelays
        /// </summary>
        /// <param name="updateConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("UpdateConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConstructionProjectDelays([FromBody] UpdateConstructionProjectDelaysPVM updateConstructionProjectDelaysPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateConstructionProjectDelaysPVM.ChildsUsersIds == null)
                    {
                        updateConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateConstructionProjectDelaysPVM.ChildsUsersIds.Count == 0)
                        updateConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateConstructionProjectDelaysPVM.ChildsUsersIds.Count == 1)
                        if (updateConstructionProjectDelaysPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateConstructionProjectDelaysPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var result = projectsApiBusiness.UpdateConstructionProjectDelays(updateConstructionProjectDelaysPVM.ChildsUsersIds
                    , updateConstructionProjectDelaysPVM.constructionProjectDelaysVM);
                if (result == true)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }




        /// <summary>
        /// CompleteDeleteConstructionProjectDelay
        /// </summary>
        /// <param name="ToggleActivationConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("ToggleActivationConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationConstructionProjectDelays(ToggleActivationConstructionProjectDelaysPVM toggleActivationConstructionProjectDelaysPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationConstructionProjectDelays(
                    toggleActivationConstructionProjectDelaysPVM.ConstructionProjectDelayId,
                   toggleActivationConstructionProjectDelaysPVM.UserId.Value,
                   toggleActivationConstructionProjectDelaysPVM.ChildsUsersIds
                    ))
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



        /// <summary>
        /// TemporaryDeleteConstructionProjectDelays
        /// </summary>
        /// <param name="TemporaryDeleteConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("TemporaryDeleteConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteConstructionProjectDelays(TemporaryDeleteConstructionProjectDelaysPVM temporaryDeleteConstructionProjectDelaysPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteConstructionProjectDelays(
                    temporaryDeleteConstructionProjectDelaysPVM.ConstructionProjectDelayId,
                   temporaryDeleteConstructionProjectDelaysPVM.UserId.Value,
                   temporaryDeleteConstructionProjectDelaysPVM.ChildsUsersIds
                    ))
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



        /// <summary>
        /// CompleteDeleteConstructionProjectDelay
        /// </summary>
        /// <param name="completeDeleteConstructionProjectDelaysPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("CompleteDeleteConstructionProjectDelays")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteConstructionProjectDelays(CompleteDeleteConstructionProjectDelaysPVM completeDeleteConstructionProjectDelaysPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteConstructionProjectDelays(
                    completeDeleteConstructionProjectDelaysPVM.ConstructionProjectDelayId,
                   completeDeleteConstructionProjectDelaysPVM.UserId.Value,
                   completeDeleteConstructionProjectDelaysPVM.ChildsUsersIds
                    );
                if (result.HasValue && result.Value == true)
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            return new JsonResult(jsonResultObjectVM);
        }




        //GetConstructionProjectDelayById
        /// <summary>
        /// GetConstructionProjectDelayById
        /// </summary>
        /// <param name="getConstructionProjectDelayByIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetConstructionProjectDelayById")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConstructionProjectDelayById([FromBody] GetConstructionProjectDelayByIdPVM
            getConstructionProjectDelayByIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectDelayByIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectDelayByIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getConstructionProjectDelayByIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectDelayByIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getConstructionProjectDelayByIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectDelayByIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectDelayByIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var ConstructionProjectDelay = projectsApiBusiness.GetConstructionProjectDelayById(
                    getConstructionProjectDelayByIdPVM.ChildsUsersIds, getConstructionProjectDelayByIdPVM.ConstructionProjectsDelayId
                    );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = ConstructionProjectDelay;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

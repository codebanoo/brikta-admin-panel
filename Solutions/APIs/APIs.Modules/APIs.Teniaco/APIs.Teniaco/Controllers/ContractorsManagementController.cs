using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Teniaco.Models.Business;
using FrameWork;
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
using VM.PVM.Teniaco;
using System.Linq;
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Teniaco.Controllers
{

    /// <summary>
    /// ContractorsManagement
    /// </summary>
    [CustomApiAuthentication]


    public class ContractorsManagementController : ApiBaseController
    {


        /// <summary>
        /// ContractorsManagement
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
        public ContractorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllContractorsList
        /// </summary>
        /// <param name="getAllContractorsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllContractorsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllContractorsList([FromBody] GetAllContractorsListPVM getAllContractorsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllContractorsListPVM.ChildsUsersIds == null)
                    {
                        getAllContractorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllContractorsListPVM.ChildsUsersIds.Count == 0)
                        getAllContractorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllContractorsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllContractorsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllContractorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractors = teniacoApiBusiness.GetAllContractorsList(
                   publicApiBusiness.PublicApiDb,
                    ref listCount,
                     getAllContractorsListPVM.ChildsUsersIds,
                     getAllContractorsListPVM.ContractorName,
                     getAllContractorsListPVM.StateId,
                     getAllContractorsListPVM.CityId,
                     getAllContractorsListPVM.ZoneId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractors;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




        /// <summary>
        /// GetListOfContractors
        /// </summary>
        /// <param name="getListOfContractorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfContractors([FromBody] GetListOfContractorsPVM getListOfContractorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfContractorsPVM.ChildsUsersIds == null)
                    {
                        getListOfContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfContractorsPVM.ChildsUsersIds.Count == 0)
                        getListOfContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfContractorsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfContractorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfContractors = teniacoApiBusiness.GetListOfContractors(
                   publicApiBusiness.PublicApiDb,
                   getListOfContractorsPVM.jtStartIndex.Value,
                   getListOfContractorsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfContractorsPVM.ChildsUsersIds,
                   getListOfContractorsPVM.ContractorName,
                   getListOfContractorsPVM.StateId,
                   getListOfContractorsPVM.CityId,
                   getListOfContractorsPVM.ZoneId,
                   getListOfContractorsPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractors;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// AddToContractors
        /// </summary>
        /// <param name="addToContractorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToContractors([FromBody] AddToContractorsPVM addToContractorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToContractorsPVM.ChildsUsersIds == null)
                    {
                        addToContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToContractorsPVM.ChildsUsersIds.Count == 0)
                        {
                            addToContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToContractorsPVM.ChildsUsersIds.Count == 1)
                                if (addToContractorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToContractorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }

                    addToContractorsPVM.ContractorsVM.CreateEnDate = DateTime.Now;
                    addToContractorsPVM.ContractorsVM.CreateTime = PersianDate.TimeNow;
                    addToContractorsPVM.ContractorsVM.UserIdCreator = this.userId.Value;
                    
                    addToContractorsPVM.ContractorsVM.CreateEnDate = DateTime.Now;
                    addToContractorsPVM.ContractorsVM.CreateTime = PersianDate.TimeNow;
                    addToContractorsPVM.ContractorsVM.UserIdCreator = this.userId.Value;
                    addToContractorsPVM.ContractorsVM.IsActivated = true;
                    addToContractorsPVM.ContractorsVM.IsDeleted = false;
                }

                int contractorId = teniacoApiBusiness.AddToContractors(
                   addToContractorsPVM.ContractorsVM,
                   publicApiBusiness,
                     consoleBusiness,
                    this.domainsSettings.DomainName);


                if (contractorId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateContractor";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (contractorId > 0)
                {
                    addToContractorsPVM.ContractorsVM.ContractorId = contractorId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToContractorsPVM.ContractorsVM;

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
        /// UpdateContractors
        /// </summary>
        /// <param name="updateContractorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>


        [HttpPost("UpdateContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateContractors([FromBody] UpdateContractorsPVM updateContractorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var contractorsVM  = updateContractorsPVM.ContractorsVM;

                int contractorId = teniacoApiBusiness.UpdateContractors(
                    ref contractorsVM,
                    updateContractorsPVM.ChildsUsersIds);

                if (contractorId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (contractorId > 0)
                {
                    updateContractorsPVM.ContractorsVM.ContractorId = contractorId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateContractorsPVM.ContractorsVM;
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
        /// ToggleActivationContractors
        /// </summary>
        /// <param name="toggleActivationContractorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationContractors([FromBody] ToggleActivationContractorsPVM toggleActivationContractorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationContractors(

                    toggleActivationContractorsPVM.ContractorId,
                    toggleActivationContractorsPVM.UserId.Value,
                    consoleBusiness,
                    toggleActivationContractorsPVM.ChildsUsersIds))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                    {
                        jsonResultObjectVM.Result = returnMessage;
                    }
                    else
                    {
                        jsonResultObjectVM.Result = "OK";
                    }

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
        /// TemporaryDeletContractors
        /// </summary>
        /// <param name="temporaryDeleteContractorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteContractors([FromBody] TemporaryDeleteContractorsPVM temporaryDeleteContractorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteContractors(
                    temporaryDeleteContractorsPVM.ContractorId,
                    temporaryDeleteContractorsPVM.UserId.Value,
                    consoleBusiness,
                    temporaryDeleteContractorsPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

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
        /// CompleteDeleteContractors
        /// </summary>
        /// <param name="completeDeleteContractorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteContractors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteContractors([FromBody] CompleteDeleteContractorsPVM completeDeleteContractorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteContractors(
                    completeDeleteContractorsPVM.ContractorId,
                    completeDeleteContractorsPVM.ChildsUsersIds,
                    consoleBusiness))
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
        /// GetContractorWithContractorId
        /// </summary>
        /// <param name="getContractorWithContractorIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetContractorWithContractorId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetContractorWithContractorId([FromBody] GetContractorWithContractorIdPVM
            getContractorWithContractorIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getContractorWithContractorIdPVM.ChildsUsersIds == null)
                    {
                        getContractorWithContractorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getContractorWithContractorIdPVM.ChildsUsersIds.Count == 0)
                        getContractorWithContractorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getContractorWithContractorIdPVM.ChildsUsersIds.Count == 1)
                        if (getContractorWithContractorIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getContractorWithContractorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var agencyStaff = teniacoApiBusiness.GetContractorWithContractorId(
                    getContractorWithContractorIdPVM.ContractorId,
                    getContractorWithContractorIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = agencyStaff;

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

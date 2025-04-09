using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Projects.Models.Business;
using FrameWork;
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
using VM.PVM.Projects;
//using APIs.ContractSides.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// ContractSidesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ContractSidesManagementController : ApiBaseController
    {

        /// <summary>
        /// ContractSidesManagement
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
        public ContractSidesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllContractSidesList
        /// </summary>
        /// <param name="getListOfContractSidesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllContractSidesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllContractSidesList([FromBody] GetAllContractSidesListPVM getAllContractSidesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllContractSidesListPVM.ChildsUsersIds == null)
                    {
                        getAllContractSidesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllContractSidesListPVM.ChildsUsersIds.Count == 0)
                        getAllContractSidesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllContractSidesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllContractSidesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllContractSidesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractSides = projectsApiBusiness.GetAllContractSidesList(
                    getAllContractSidesListPVM.ChildsUsersIds,
                   getAllContractSidesListPVM.ParentId,
                   getAllContractSidesListPVM.TableId,
                   getAllContractSidesListPVM.TableTitle
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractSides;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




        /// <summary>
        /// GetListOfContractSides
        /// </summary>
        /// <param name="getListOfContractSidesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfContractSides([FromBody] GetListOfContractSidesPVM getListOfContractSidesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfContractSidesPVM.ChildsUsersIds == null)
                    {
                        getListOfContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfContractSidesPVM.ChildsUsersIds.Count == 0)
                        getListOfContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfContractSidesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfContractSidesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractSides = projectsApiBusiness.GetListOfContractSides(
                   getListOfContractSidesPVM.jtStartIndex.Value,
                   getListOfContractSidesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfContractSidesPVM.ChildsUsersIds,
                   getListOfContractSidesPVM.ParentId,
                   getListOfContractSidesPVM.TableId,
                   getListOfContractSidesPVM.TableTitle,
                   getListOfContractSidesPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractSides;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




        /// <summary>
        /// AddToContractSides
        /// </summary>
        /// <param name="addToContractSidesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToContractSides([FromBody] AddToContractSidesPVM addToContractSidesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToContractSidesPVM.ChildsUsersIds == null)
                    {
                        addToContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToContractSidesPVM.ChildsUsersIds.Count == 0)
                        addToContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToContractSidesPVM.ChildsUsersIds.Count == 1)
                        if (addToContractSidesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var contractSideId = projectsApiBusiness.AddToContractSides(addToContractSidesPVM.ContractSidesVM);
                if (contractSideId != 0)
                {
                    addToContractSidesPVM.ContractSidesVM.ContractSideId = contractSideId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToContractSidesPVM.ContractSidesVM;

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


        [HttpPost("UpdateContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateContractSides([FromBody] UpdateContractSidesPVM updateContractSidesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateContractSidesPVM.ChildsUsersIds == null)
                    {
                        updateContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateContractSidesPVM.ChildsUsersIds.Count == 0)
                        updateContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateContractSidesPVM.ChildsUsersIds.Count == 1)
                        if (updateContractSidesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateContractSidesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedContractSidesVM = projectsApiBusiness.UpdateContractSides(
                    updateContractSidesPVM.ChildsUsersIds,
                    updateContractSidesPVM.ContractSidesVM);
                if (updatedContractSidesVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedContractSidesVM;

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

        [HttpPost("ToggleActivationContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationContractSides(ToggleActivationContractSidesPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationContractSides(
                    toggleActivationInvestorsPVM.ContractSideId,
                   toggleActivationInvestorsPVM.UserId.Value,
                   toggleActivationInvestorsPVM.ChildsUsersIds
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


        [HttpPost("TemporaryDeleteContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteContractSides(TemporaryDeleteContractSidesPVM temporaryDeleteContractSidesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteContractSides(
                    temporaryDeleteContractSidesPVM.ContractSideId,
                   temporaryDeleteContractSidesPVM.UserId.Value,
                   temporaryDeleteContractSidesPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteContractSides")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteContractSides(CompleteDeleteContractSidesPVM completeDeleteContractSidesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            try
            {
                if (projectsApiBusiness.CompleteDeleteContractSides(
                    completeDeleteContractSidesPVM.ContractSidesId,
                   completeDeleteContractSidesPVM.UserId.Value,
                   completeDeleteContractSidesPVM.ChildsUsersIds))
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
        /// GetContractSidesWithContractSideId
        /// </summary>
        /// <param name="getContractSidesWithContractSideIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetContractSidesWithContractSideId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetContractSidesWithContractSideId(
            [FromBody] GetContractSidesWithContractSideIdPVM getContractSidesWithContractSideIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getContractSidesWithContractSideIdPVM.ChildsUsersIds == null)
                    {
                        getContractSidesWithContractSideIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getContractSidesWithContractSideIdPVM.ChildsUsersIds.Count == 0)
                        getContractSidesWithContractSideIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getContractSidesWithContractSideIdPVM.ChildsUsersIds.Count == 1)
                        if (getContractSidesWithContractSideIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getContractSidesWithContractSideIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfContractSides = projectsApiBusiness.GetContractSidesWithContractSideId(
                        getContractSidesWithContractSideIdPVM.ContractSideId,
                        getContractSidesWithContractSideIdPVM.UserId.Value,
                        getContractSidesWithContractSideIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfContractSides;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

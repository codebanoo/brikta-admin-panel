using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using System;
using System.Net;
using VM.Base;
using VM.PVM.Public;

namespace APIs.Public.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesListIndicesManagementController : ApiBaseController
    {
        /// <summary>
        /// PricesListIndicesManagement
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
        public PricesListIndicesManagementController(IHostEnvironment _hostEnvironment, 
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
            base(_hostEnvironment, 
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
         [HttpPost("CompleteDeletePricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeletePricesListIndices([FromBody] CompleteDeletePricesListIndicesPVM completeDeletePricesListIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (publicApiBusiness.CompleteDeletePricesListIndices(
                    completeDeletePricesListIndicesPVM.PricesListIndicesId,
                    completeDeletePricesListIndicesPVM.ChildsUsersIds
                    ))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exx)
            {
                jsonResultObjectVM.Result = "ERROR";
                jsonResultObjectVM.Message = "ErrorInService";

            }


            return new JsonResult(jsonResultObjectVM);
        }

        [HttpPost("TemporaryDeletePricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePricesListIndices([FromBody] TemporaryDeletePricesListIndicesPVM temporaryDeletePricesListIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {


                if (publicApiBusiness.TemporaryDeletePricesListIndices(temporaryDeletePricesListIndicesPVM.PricesListIndicesId,
                    temporaryDeletePricesListIndicesPVM.UserId.Value
                    , temporaryDeletePricesListIndicesPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);

                }
            }
            catch (Exception exx)
            {
                jsonResultObjectVM.Result = "ERROR";
                jsonResultObjectVM.Message = "ErrorInService";
            }
            return new JsonResult(jsonResultObjectVM);
        }

        [HttpPost("ToggleActivationPricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationPricesListIndices([FromBody] ToggleActivationPricesListIndicesPVM toggleActivationPricesListIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                if (publicApiBusiness.ToggleActivationPricesListIndices(

                    toggleActivationPricesListIndicesPVM.PricesListIndicesId,
                    toggleActivationPricesListIndicesPVM.UserId.Value,

                    toggleActivationPricesListIndicesPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";
                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            {
                jsonResultObjectVM.Result = "ERROR";
                jsonResultObjectVM.Message = "ErrorInService";
            }

            return new JsonResult(jsonResultObjectVM);

        }

        [HttpPost("UpdatePricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdatePricesListIndices([FromBody] UpdatePricesListIndicesPVM updatePricesListIndicesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var _PricesListIndicesVM = updatePricesListIndicesPVM.PricesListIndicesVM;

                int _PricesListIndicesId = publicApiBusiness.UpdatePricesListIndices(_PricesListIndicesVM, updatePricesListIndicesPVM.ChildsUsersIds);

                if (_PricesListIndicesId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (_PricesListIndicesId > 0)
                {
                    updatePricesListIndicesPVM.PricesListIndicesVM.PricesListIndicesId = _PricesListIndicesId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatePricesListIndicesPVM.PricesListIndicesVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            {
                jsonResultWithRecordObjectVM.Result = "ERROR";
                jsonResultWithRecordObjectVM.Message = "ErrorInService";
            }

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        
        [HttpPost("AddToPricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToPricesListIndices([FromBody] AddToPricesListIndicesPVM addToPricesListIndicesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToPricesListIndicesPVM.ChildsUsersIds == null)
                    {
                        addToPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToPricesListIndicesPVM.ChildsUsersIds.Count == 0)
                        {
                            addToPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToPricesListIndicesPVM.ChildsUsersIds.Count == 1)
                                if (addToPricesListIndicesPVM.ChildsUsersIds[0] == 0)
                                    addToPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToPricesListIndicesPVM.PricesListIndicesVM.CreateEnDate = DateTime.Now;
                    addToPricesListIndicesPVM.PricesListIndicesVM.CreateTime = PersianDate.TimeNow;
                    addToPricesListIndicesPVM.PricesListIndicesVM.UserIdCreator = this.userId.Value;

                    addToPricesListIndicesPVM.PricesListIndicesVM.CreateEnDate = DateTime.Now;
                    addToPricesListIndicesPVM.PricesListIndicesVM.CreateTime = PersianDate.TimeNow;
                    addToPricesListIndicesPVM.PricesListIndicesVM.UserIdCreator = this.userId.Value;
                    addToPricesListIndicesPVM.PricesListIndicesVM.IsActivated = true;
                    addToPricesListIndicesPVM.PricesListIndicesVM.IsDeleted = false;
                }

                int _PricesListIndicesId = publicApiBusiness.AddToPricesListIndices(addToPricesListIndicesPVM.PricesListIndicesVM);


                if (_PricesListIndicesId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (_PricesListIndicesId > 0)
                {
                    addToPricesListIndicesPVM.PricesListIndicesVM.PricesListIndicesId = _PricesListIndicesId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToPricesListIndicesPVM.PricesListIndicesVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            {
                jsonResultWithRecordObjectVM.Result = "ERROR";
                jsonResultWithRecordObjectVM.Message = "ErrorInService";
            }
            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        [HttpPost("GetListOfPricesListIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPricesListIndices([FromBody] GetListOfPricesListIndicesPVM getListOfPricesListIndicesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPricesListIndicesPVM.ChildsUsersIds == null)
                    {
                        getListOfPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPricesListIndicesPVM.ChildsUsersIds.Count == 0)
                        getListOfPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPricesListIndicesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPricesListIndicesPVM.ChildsUsersIds[0] == 0)
                            getListOfPricesListIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAgencies = publicApiBusiness.GetListOfPricesListIndices(
                    getListOfPricesListIndicesPVM.IndicesId ?? 0, getListOfPricesListIndicesPVM.Bdate,
                    getListOfPricesListIndicesPVM.EDate,

                   getListOfPricesListIndicesPVM.jtStartIndex.Value,
                   getListOfPricesListIndicesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfPricesListIndicesPVM.ChildsUsersIds,

                   getListOfPricesListIndicesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencies;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                jsonResultWithRecordsObjectVM.Result = "ERROR";
                jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            }


            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        [HttpPost("GetAllPricesListIndicesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllPricesListIndicesList([FromBody] GetAllPricesListIndicesListPVM getAllPricesListIndicesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPricesListIndicesListPVM.ChildsUsersIds == null)
                    {
                        getAllPricesListIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllPricesListIndicesListPVM.ChildsUsersIds.Count == 0)
                        getAllPricesListIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllPricesListIndicesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPricesListIndicesListPVM.ChildsUsersIds[0] == 0)
                            getAllPricesListIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                int listCount = 0;

                var listOfPricesListIndicesList = publicApiBusiness.GetAllPricesListIndicesList(getAllPricesListIndicesListPVM.PricesListIndicesId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPricesListIndicesList;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }




    }
}

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
    public class IndicesMamagementController : ApiBaseController
    {
        /// <summary>
        /// IndicesMamagement
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
        public IndicesMamagementController(IHostEnvironment _hostingEnvironment, 
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
            base(_hostingEnvironment, 
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
        [HttpPost("CompleteDeleteIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteIndices([FromBody] CompleteDeleteIndicesPVM completeDeleteIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (publicApiBusiness.CompleteDeleteIndices(
                    completeDeleteIndicesPVM.IndiceId,
                    completeDeleteIndicesPVM.ChildsUsersIds
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

        [HttpPost("TemporaryDeleteIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteIndices([FromBody] TemporaryDeleteIndicesPVM temporaryDeleteIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {


                if (publicApiBusiness.TemporaryDeleteIndices(temporaryDeleteIndicesPVM.IndiceId,
                    temporaryDeleteIndicesPVM.UserId.Value
                    , temporaryDeleteIndicesPVM.ChildsUsersIds))
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

        [HttpPost("ToggleActivationIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationIndices([FromBody] ToggleActivationIndicesPVM toggleActivationIndicesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                if (publicApiBusiness.ToggleActivationIndices(

                    toggleActivationIndicesPVM.IndiceId,
                    toggleActivationIndicesPVM.UserId.Value,
                    
                    toggleActivationIndicesPVM.ChildsUsersIds))
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

        [HttpPost("UpdateIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateIndices([FromBody] UpdateIndicesPVM updateIndicesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var _IndicesVM = updateIndicesPVM.IndicesVM;

                int _IndicesId = publicApiBusiness.UpdateIndices(_IndicesVM, updateIndicesPVM.ChildsUsersIds);

                if (_IndicesId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (_IndicesId > 0)
                {
                    updateIndicesPVM.IndicesVM.IndiceId = _IndicesId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateIndicesPVM.IndicesVM;
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

        [HttpPost("AddToIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToIndices([FromBody] AddToIndicesPVM addToIndicesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToIndicesPVM.ChildsUsersIds == null)
                    {
                        addToIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToIndicesPVM.ChildsUsersIds.Count == 0)
                        {
                            addToIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToIndicesPVM.ChildsUsersIds.Count == 1)
                                if (addToIndicesPVM.ChildsUsersIds[0] == 0)
                                    addToIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToIndicesPVM.IndicesVM.CreateEnDate = DateTime.Now;
                    addToIndicesPVM.IndicesVM.CreateTime = PersianDate.TimeNow;
                    addToIndicesPVM.IndicesVM.UserIdCreator = this.userId.Value;

                    addToIndicesPVM.IndicesVM.CreateEnDate = DateTime.Now;
                    addToIndicesPVM.IndicesVM.CreateTime = PersianDate.TimeNow;
                    addToIndicesPVM.IndicesVM.UserIdCreator = this.userId.Value;
                    addToIndicesPVM.IndicesVM.IsActivated = true;
                    addToIndicesPVM.IndicesVM.IsDeleted = false;
                }

                int _Indicesid = publicApiBusiness.AddToIndices(
                   addToIndicesPVM.IndicesVM);


                if (_Indicesid.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (_Indicesid > 0)
                {
                    addToIndicesPVM.IndicesVM.IndiceId = _Indicesid;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToIndicesPVM.IndicesVM;

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

        [HttpPost("GetListOfIndices")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfIndices([FromBody] GetListOfIndicesPVM getListOfIndicesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfIndicesPVM.ChildsUsersIds == null)
                    {
                        getListOfIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfIndicesPVM.ChildsUsersIds.Count == 0)
                        getListOfIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfIndicesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfIndicesPVM.ChildsUsersIds[0] == 0)
                            getListOfIndicesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAgencies = publicApiBusiness.GetListOfIndices(

                   getListOfIndicesPVM.jtStartIndex.Value,
                   getListOfIndicesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfIndicesPVM.ChildsUsersIds,

                   getListOfIndicesPVM.jtSorting);

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

        [HttpPost("GetAllIndicesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllIndicesList([FromBody] GetAllIndicesListPVM getAllIndicesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllIndicesListPVM.ChildsUsersIds == null)
                    {
                        getAllIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllIndicesListPVM.ChildsUsersIds.Count == 0)
                        getAllIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllIndicesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllIndicesListPVM.ChildsUsersIds[0] == 0)
                            getAllIndicesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                int listCount = 0;

                var listOfIndices = publicApiBusiness.GetAllIndicesList(
                 getAllIndicesListPVM.IndiceId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfIndices;
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

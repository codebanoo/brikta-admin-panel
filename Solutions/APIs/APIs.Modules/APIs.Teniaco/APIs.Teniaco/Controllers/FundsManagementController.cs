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
using System.Linq;
using System.Net;
using VM.Base;
using VM.Public;
using VM.PVM.Teniaco;


namespace APIs.Teniaco.Controllers
{
    public class FundsManagementController : ApiBaseController
    {
        /// <summary>
        /// FundsManagement
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

        public FundsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFundsList
        /// </summary>
        /// <param name="getAllFundsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllFundsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllFundsList([FromBody] GetAllFundsListPVM getAllFundsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllFundsListPVM.ChildsUsersIds == null)
                    {
                        getAllFundsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllFundsListPVM.ChildsUsersIds.Count == 0)
                        getAllFundsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllFundsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllFundsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllFundsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                int listCount = 0;

                var listOfFunds = teniacoApiBusiness.GetAllFundsList(
                 getAllFundsListPVM.FundId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFunds;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }



        [HttpPost("GetListOfFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfFunds([FromBody] GetListOfFundsPVM getListOfFundsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfFundsPVM.ChildsUsersIds == null)
                    {
                        getListOfFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfFundsPVM.ChildsUsersIds.Count == 0)
                        getListOfFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfFundsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfFundsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAgencies = teniacoApiBusiness.GetListOfFunds(

                   getListOfFundsPVM.jtStartIndex.Value,
                   getListOfFundsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfFundsPVM.ChildsUsersIds,

                   getListOfFundsPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencies;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }




        [HttpPost("AddToFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToFunds([FromBody] AddToFundsPVM addToFundsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToFundsPVM.ChildsUsersIds == null)
                    {
                        addToFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToFundsPVM.ChildsUsersIds.Count == 0)
                        {
                            addToFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToFundsPVM.ChildsUsersIds.Count == 1)
                                if (addToFundsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToFundsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToFundsPVM.FundsVM.CreateEnDate = DateTime.Now;
                    addToFundsPVM.FundsVM.CreateTime = PersianDate.TimeNow;
                    addToFundsPVM.FundsVM.UserIdCreator = this.userId.Value;

                    addToFundsPVM.FundsVM.CreateEnDate = DateTime.Now;
                    addToFundsPVM.FundsVM.CreateTime = PersianDate.TimeNow;
                    addToFundsPVM.FundsVM.UserIdCreator = this.userId.Value;
                    addToFundsPVM.FundsVM.IsActivated = true;
                    addToFundsPVM.FundsVM.IsDeleted = false;
                }

                int _Fundid = teniacoApiBusiness.AddToFunds(
                   addToFundsPVM.FundsVM);


                if (_Fundid.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (_Fundid > 0)
                {
                    addToFundsPVM.FundsVM.FundId = _Fundid;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToFundsPVM.FundsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }





        [HttpPost("UpdateFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateFunds([FromBody] UpdateFundPVM updateFundPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var _FundsVM = updateFundPVM.FundsVM;

                int _FundId = teniacoApiBusiness.UpdateFunds(ref _FundsVM, updateFundPVM.ChildsUsersIds);

                if (_FundId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (_FundId > 0)
                {
                    updateFundPVM.FundsVM.FundId = _FundId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateFundPVM.FundsVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }





        [HttpPost("ToggleActivationFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationFunds([FromBody] ToggleActivationFundPVM toggleActivationFundPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationFunds(

                    toggleActivationFundPVM.FundId,
                    toggleActivationFundPVM.UserId.Value,
                    toggleActivationFundPVM.ChildsUsersIds))
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





        [HttpPost("TemporaryDeleteFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteFunds([FromBody] TemporaryDeleteFundPVM temporaryDeleteFundPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteFunds(
                    temporaryDeleteFundPVM.FundId,
                    temporaryDeleteFundPVM.UserId.Value,
                    temporaryDeleteFundPVM.ChildsUsersIds))
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




        [HttpPost("CompleteDeleteFunds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteFunds([FromBody] CompleteDeleteFundsPVM completeDeleteFundsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteFunds(
                    completeDeleteFundsPVM.FundId,
                    completeDeleteFundsPVM.ChildsUsersIds))
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


    }
}

using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
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
using VM.Melkavan;
using VM.PVM.Melkavan;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// SmsesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class SmsesManagementController : ApiBaseController
    {
        /// <summary>
        /// SmsesManagement
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
        public SmsesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllSmsesList
        /// </summary>
        /// <param name="getAllSmsesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllSmsesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllSmsesList([FromBody] GetAllSmsesListPVM getAllSmsesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllSmsesListPVM.ChildsUsersIds == null)
                    {
                        getAllSmsesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllSmsesListPVM.ChildsUsersIds.Count == 0)
                        getAllSmsesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllSmsesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllSmsesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllSmsesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfSmses = melkavanApiBusiness.GetAllSmsesList(
                    getAllSmsesListPVM.ChildsUsersIds,
                    ref listCount,
                    getAllSmsesListPVM.SearchText);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfSmses;
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
        /// GetListOfSmses
        /// </summary>
        /// <param name="getListOfSmsesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfSmses([FromBody] GetListOfSmsesPVM getListOfSmsesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfSmses = melkavanApiBusiness.GetListOfSmses(
                    getListOfSmsesPVM.ChildsUsersIds,
                    getListOfSmsesPVM.jtStartIndex.Value,
                    getListOfSmsesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfSmsesPVM.SearchText);

                //var states = publicApiBusiness.GetListOfStates(null, getListOfSmsesPVM.Lang);
                //var cities = publicApiBusiness.GetListOfCities(null, getListOfSmsesPVM.Lang);

                //foreach (var sms in listOfSmses)
                //{
                //    if (sms.StateId.HasValue)
                //        if (sms.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == sms.StateId.Value);
                //            sms.StateName = state.StateName;
                //        }

                //    if (sms.CityId.HasValue)
                //        if (sms.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == sms.CityId.Value);
                //            sms.CityName = city.CityName;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfSmses;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfSmses);

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
        /// AddToSmses
        /// </summary>
        /// <param name="addToSmsesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToSmses([FromBody] AddToSmsesPVM addToSmsesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                addToSmsesPVM.SmsesVM = new SmsesVM
                {
                    SmsMessage = "ملکاوان" + " " + addToSmsesPVM.SmsesVM.SmsMessage,
                    UserId = addToSmsesPVM.SmsesVM.UserId,
                    IsActivated = true,
                    IsDeleted = false,
                    UserIdCreator = addToSmsesPVM.SmsesVM.UserId,
                    CreateEnDate = DateTime.Now,
                    CreateTime = PersianDate.TimeNow,
                };

                long smsId = melkavanApiBusiness.AddToSmses(

                    addToSmsesPVM.SmsesVM,
                    addToSmsesPVM.ChildsUsersIds,
                    publicApiBusiness,
                    consoleBusiness);


                if (smsId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateSms";
                }
                else
                if (smsId > 0)
                {
                    addToSmsesPVM.SmsesVM.SmsId = smsId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToSmsesPVM.SmsesVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }


        /// <summary>
        /// UpdateSmses
        /// </summary>
        /// <param name="updateSmsesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateSmses([FromBody] UpdateSmsesPVM updateSmsesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var smsesVM = updateSmsesPVM.SmsesVM;

                long smsId = melkavanApiBusiness.UpdateSmses(
                    ref smsesVM,
                    updateSmsesPVM.ChildsUsersIds);

                if (smsId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateSms";
                }
                else
                if (smsId > 0)
                {
                    updateSmsesPVM.SmsesVM.SmsId = smsId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateSmsesPVM.SmsesVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
            //return jsonResultWithRecordObjectVM;
        }



        /// <summary>
        /// ToggleActivationSmses
        /// </summary>
        /// <param name="toggleActivationSmsesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationSmses([FromBody] ToggleActivationSmsesPVM toggleActivationSmsesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (melkavanApiBusiness.ToggleActivationSmses(
                    toggleActivationSmsesPVM.SmsId,
                    toggleActivationSmsesPVM.UserId.Value,
                    toggleActivationSmsesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteSmses
        /// </summary>
        /// <param name="temporaryDeleteSmsesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteSmses([FromBody] TemporaryDeleteSmsesPVM
            temporaryDeleteSmsesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (melkavanApiBusiness.TemporaryDeleteSmses(
                    temporaryDeleteSmsesPVM.SmsId,
                    temporaryDeleteSmsesPVM.UserId.Value,
                    temporaryDeleteSmsesPVM.ChildsUsersIds))
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
        /// CompleteDeleteSmses
        /// </summary>
        /// <param name="completeDeleteSmsesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteSmses")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteSmses([FromBody] CompleteDeleteSmsesPVM completeDeleteSmsesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (melkavanApiBusiness.CompleteDeleteSmses(completeDeleteSmsesPVM.SmsId, completeDeleteSmsesPVM.ChildsUsersIds))
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



    }
}

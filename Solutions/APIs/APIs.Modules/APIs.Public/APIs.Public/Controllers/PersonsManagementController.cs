using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIs.Public.Models;
using Microsoft.AspNetCore.Authorization;
using APIs.Core.Controllers;
using APIs.Public.Models.Business;
using VM.PVM.Public;
using APIs.Automation.Models.Business;
using VM.Public;
using Models.Business.ConsoleBusiness;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using APIs.CustomAttributes.Helper;
using Microsoft.Extensions.Options;
using VM.Base;
using APIs.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using APIs.Teniaco.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Public.Controllers
{
    /// <summary>
    /// PersonsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PersonsManagementController : ApiBaseController
    {
        /// <summary>
        /// PersonsManagement
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
        public PersonsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllBuyersList
        /// </summary>
        /// <param name="getAllPersonsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetBuyersList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetBuyersList([FromBody] GetAllPersonsListPVM getAllPersonsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPersonsListPVM.ChildsUsersIds == null)
                    {
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 0)
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPersonsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfBuyers = publicApiBusiness.GetBuyersList(
                    getAllPersonsListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Records = listOfBuyers;
                jsonResultWithRecordsObjectVM.Result = "OK";

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetAllPersonsList
        /// </summary>
        /// <param name="getAllPersonsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPersonsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPersonsList([FromBody] GetAllPersonsListPVM getAllPersonsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPersonsListPVM.ChildsUsersIds == null)
                    {
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 0)
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPersonsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPersons = publicApiBusiness.GetAllPersonsList(
                    getAllPersonsListPVM.ChildsUsersIds,
                    ref listCount,
                    getAllPersonsListPVM.SearchText);

                jsonResultWithRecordsObjectVM.Result = "OK";
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfAgeCategories);
                jsonResultWithRecordsObjectVM.Records = listOfPersons;
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
        /// GetListOfPersons
        /// </summary>
        /// <param name="getListOfPersonsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfPersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPersons([FromBody] GetListOfPersonsPVM getListOfPersonsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfPersons = publicApiBusiness.GetListOfPersons(
                    getListOfPersonsPVM.ChildsUsersIds,
                    getListOfPersonsPVM.jtStartIndex.Value,
                    getListOfPersonsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfPersonsPVM.SearchText);

                //var states = publicApiBusiness.GetListOfStates(null, getListOfPersonsPVM.Lang);
                //var cities = publicApiBusiness.GetListOfCities(null, getListOfPersonsPVM.Lang);

                //foreach (var person in listOfPersons)
                //{
                //    if (person.StateId.HasValue)
                //        if (person.StateId.Value > 0)
                //        {
                //            var state = states.FirstOrDefault(x => x.StateId == person.StateId.Value);
                //            person.StateName = state.StateName;
                //        }

                //    if (person.CityId.HasValue)
                //        if (person.CityId.Value > 0)
                //        {
                //            var city = cities.FirstOrDefault(x => x.CityId == person.CityId.Value);
                //            person.CityName = city.CityName;
                //        }
                //}

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPersons;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfPersons);

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
        /// AddToPersons
        /// </summary>
        /// <param name="addToPersonsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToPersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToPersons([FromBody] AddToPersonsPVM addToPersonsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                long personId = publicApiBusiness.AddToPersons(
                    addToPersonsPVM.PersonsVM,
                    addToPersonsPVM.ChildsUsersIds);

                if (personId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicatePerson";
                }
                else
                if (personId > 0)
                {
                    addToPersonsPVM.PersonsVM.PersonId = personId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToPersonsPVM.PersonsVM;
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
        /// UpdatePersons
        /// </summary>
        /// <param name="updatePersonsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdatePersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdatePersons([FromBody] UpdatePersonsPVM updatePersonsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var personsVM = updatePersonsPVM.PersonsVM;

                long personId = publicApiBusiness.UpdatePersons(
                    ref personsVM,
                    updatePersonsPVM.ChildsUsersIds);

                if (personId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicatePerson";
                }
                else
                if (personId > 0)
                {
                    updatePersonsPVM.PersonsVM.PersonId = personId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatePersonsPVM.PersonsVM;
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
        /// ToggleActivationPersons
        /// </summary>
        /// <param name="toggleActivationPersonsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationPersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationPersons([FromBody] ToggleActivationPersonsPVM toggleActivationPersonsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationPersons(
                    toggleActivationPersonsPVM.PersonId,
                    toggleActivationPersonsPVM.UserId.Value,
                    toggleActivationPersonsPVM.ChildsUsersIds))
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
        /// TemporaryDeletePersons
        /// </summary>
        /// <param name="temporaryDeletePersonsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeletePersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePersons([FromBody] TemporaryDeletePersonsPVM
            temporaryDeletePersonsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeletePersons(
                    temporaryDeletePersonsPVM.PersonId,
                    temporaryDeletePersonsPVM.UserId.Value,
                    temporaryDeletePersonsPVM.ChildsUsersIds))
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
        /// CompleteDeletePersons
        /// </summary>
        /// <param name="completeDeletePersonsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeletePersons")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeletePersons([FromBody] CompleteDeletePersonsPVM completeDeletePersonsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeletePersons(completeDeletePersonsPVM.PersonId,completeDeletePersonsPVM.ChildsUsersIds))
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
        /// GetPersonWithPersonId
        /// </summary>
        /// <param name="getPersonWithMobileNumberPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetPersonWithMobileNumber")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPersonWithMobileNumber([FromBody] GetPersonWithMobileNumberPVM
            getPersonWithMobileNumberPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPersonWithMobileNumberPVM.ChildsUsersIds == null)
                    {
                        getPersonWithMobileNumberPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPersonWithMobileNumberPVM.ChildsUsersIds.Count == 0)
                        getPersonWithMobileNumberPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPersonWithMobileNumberPVM.ChildsUsersIds.Count == 1)
                        if (getPersonWithMobileNumberPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPersonWithMobileNumberPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var person = publicApiBusiness.GetPersonWithMobileNumber(
                    getPersonWithMobileNumberPVM.MobileNumber,
                    getPersonWithMobileNumberPVM.ChildsUsersIds);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = person;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetPersonWithUserId
        /// </summary>
        /// <param name="getPersonWithUserIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetPersonWithUserId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPersonWithUserId([FromBody] GetPersonWithUserIdPVM getPersonWithUserIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPersonWithUserIdPVM.ChildsUsersIds == null)
                    {
                        getPersonWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPersonWithUserIdPVM.ChildsUsersIds.Count == 0)
                        getPersonWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPersonWithUserIdPVM.ChildsUsersIds.Count == 1)
                        if (getPersonWithUserIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPersonWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var person = publicApiBusiness.GetPersonWithUserId(
                    getPersonWithUserIdPVM.UserId.Value,
                    getPersonWithUserIdPVM.ChildsUsersIds);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = person;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetAllPersonsListWithUsers
        /// </summary>
        /// <param name="getAllPersonsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPersonsListWithUsers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPersonsListWithUsers([FromBody] GetAllPersonsListPVM getAllPersonsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPersonsListPVM.ChildsUsersIds == null)
                    {
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 0)
                        getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPersonsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPersonsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPersonsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPersons = publicApiBusiness.GetAllPersonsListWithUsers(
                    getAllPersonsListPVM.ChildsUsersIds,
                    consoleBusiness);

                jsonResultWithRecordsObjectVM.Result = "OK";

                jsonResultWithRecordsObjectVM.Records = listOfPersons;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);


            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }
    }
}

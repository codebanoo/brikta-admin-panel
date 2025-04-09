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
using APIs.Projects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.TelegramBot.Models.Business;

namespace APIs.Automation.Controllers
{
    /// <summary>
    /// MyCompaniesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyCompaniesManagementController : ApiBaseController
    {
        /// <summary>
        /// MyCompaniesManagement
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
        public MyCompaniesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfMyCompanies
        /// </summary>
        /// <param name="getListOfMyCompaniesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyCompanies([FromBody] GetListOfMyCompaniesPVM getListOfMyCompaniesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfMyCompanies = automationApiBusiness.GetMyCompaniesList(
                    getListOfMyCompaniesPVM.jtStartIndex.Value,
                    getListOfMyCompaniesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyCompaniesPVM.jtSorting,
                    getListOfMyCompaniesPVM.ChildsUsersIds,
                    "fa",
                    getListOfMyCompaniesPVM.Address,
                    getListOfMyCompaniesPVM.CommercialCode,
                    getListOfMyCompaniesPVM.MyCompanyName,
                    getListOfMyCompaniesPVM.Phones,
                    getListOfMyCompaniesPVM.MyCompanyRealName,
                    getListOfMyCompaniesPVM.PostalCode,
                    getListOfMyCompaniesPVM.Faxes,
                    getListOfMyCompaniesPVM.RegisterNumber,
                    getListOfMyCompaniesPVM.NationalCode,
                    getListOfMyCompaniesPVM.CityId,
                    getListOfMyCompaniesPVM.StateId
                    );

                var states = publicApiBusiness.GetListOfStates(null);
                int lcount = 0;
                var cities = publicApiBusiness.GetAllCitiesList(ref lcount,null,null,null);

                foreach (var myCompany in listOfMyCompanies)
                {
                    if (myCompany.StateId.HasValue)
                        if (myCompany.StateId.Value > 0)
                        {
                            var state = states.FirstOrDefault(x=>x.StateId== myCompany.StateId.Value);
                            myCompany.StateName = state.StateName;
                        }

                    if (myCompany.CityId.HasValue)
                        if (myCompany.CityId.Value > 0)
                        {
                            var city = cities.FirstOrDefault(x=>x.CityId== myCompany.CityId.Value);
                            myCompany.CityName = city.CityName;

                        }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyCompanies;

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
        /// GetAllMyCompaniesList
        /// </summary>
        /// <param name="getAllMyCompaniesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyCompaniesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyCompaniesList([FromBody] GetAllMyCompaniesListPVM getAllMyCompaniesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                var listOfMyCompanies = automationApiBusiness.GetAllMyCompaniesList(getAllMyCompaniesListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyCompanies;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// AddToMyCompanies
        /// </summary>
        /// <param name="addToMyCompaniesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyCompanies([FromBody] AddToMyCompaniesPVM addToMyCompaniesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                int myCompanyId = automationApiBusiness.AddToMyCompanies(
                    addToMyCompaniesPVM.MyCompaniesVM,
                    addToMyCompaniesPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = myCompanyId;

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
        /// GetMyCompanyWithMyCompanyId
        /// </summary>
        /// <param name="getMyCompanyWithMyCompanyIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetMyCompanyWithMyCompanyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyCompanyWithMyCompanyId([FromBody] GetMyCompanyWithMyCompanyIdPVM  getMyCompanyWithMyCompanyIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var myCompaniesVM = automationApiBusiness.GetMyCompaniesWithMyCompanyId(
                    getMyCompanyWithMyCompanyIdPVM.ChildsUsersIds,
                    getMyCompanyWithMyCompanyIdPVM.MyCompanyId);

                if (myCompaniesVM.StateId.HasValue)
                    if (myCompaniesVM.StateId.Value > 0)
                    {
                        var state = publicApiBusiness.GetStateWithStateId(myCompaniesVM.StateId.Value,getMyCompanyWithMyCompanyIdPVM.ChildsUsersIds);
                        myCompaniesVM.StateName = state.StateName;
                    }

                if (myCompaniesVM.CityId.HasValue)
                    if (myCompaniesVM.CityId.Value > 0)
                    {
                        var city = publicApiBusiness.GetCityWithCityId(myCompaniesVM.CityId.Value);
                        myCompaniesVM.CityName = city.CityName;

                    }

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = myCompaniesVM;

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
        /// UpdateMyCompanies
        /// </summary>
        /// <param name="updateMyCompaniesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyCompanies([FromBody] UpdateMyCompaniesPVM updateMyCompaniesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var myCompaniesVM = updateMyCompaniesPVM.MyCompaniesVM;
                if (automationApiBusiness.UpdateMyCompany(
                    ref myCompaniesVM,
                    updateMyCompaniesPVM.ChildsUsersIds))
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
        /// ToggleActivationMyCompanies
        /// </summary>
        /// <param name="toggleActivationMyCompaniesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMyCompanies([FromBody] ToggleActivationMyCompaniesPVM toggleActivationMyCompaniesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.ToggleActivationMyCompanies(
                    toggleActivationMyCompaniesPVM.MyCompanyId,
                    toggleActivationMyCompaniesPVM.UserId.Value,
                    toggleActivationMyCompaniesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteMyCompanies
        /// </summary>
        /// <param name="temporaryDeleteMyCompaniesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMyCompanies([FromBody] TemporaryDeleteMyCompaniesPVM temporaryDeleteMyCompaniesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.TemporaryDeleteMyCompanies(
                    temporaryDeleteMyCompaniesPVM.MyCompanyId,
                    temporaryDeleteMyCompaniesPVM.UserId.Value,
                    temporaryDeleteMyCompaniesPVM.ChildsUsersIds))
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
        /// CompleteDeleteMyCompanies
        /// </summary>
        /// <param name="completeDeleteMyCompaniesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyCompanies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyCompanies([FromBody] CompleteDeleteMyCompaniesPVM completeDeleteMyCompaniesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (automationApiBusiness.CompleteDeleteMyCompanies(
                    completeDeleteMyCompaniesPVM.MyCompanyId,
                    completeDeleteMyCompaniesPVM.ChildsUsersIds,
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

        /// <summary>
        /// UpdateCompanyPictures
        /// </summary>
        /// <param name="updateCompanyPicturesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("UpdateCompanyPictures")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateCompanyPictures([FromBody] UpdateCompanyPicturesPVM updateCompanyPicturesPVM)
        {

            JsonResultObjectVM jsonResultObjectViewModel = new JsonResultObjectVM(new object() { });

            try
            {
                if (automationApiBusiness.UpdateCompanyPictures(
                        updateCompanyPicturesPVM.UserId.Value,
                        updateCompanyPicturesPVM.MyCompanyId,
                        null,
                        updateCompanyPicturesPVM.CompanyLogo,
                        updateCompanyPicturesPVM.WaterMarkImage
                   ))
                {
                    jsonResultObjectViewModel.Result = "OK";
                    return new JsonResult(jsonResultObjectViewModel);
                }

                return new JsonResult(jsonResultObjectViewModel);
            }
            catch (Exception exc)
            { }

            jsonResultObjectViewModel.Result = "ERROR";
            jsonResultObjectViewModel.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectViewModel);
            //return jsonResultObjectViewModel;
        }


        /// <summary>
        ///GetMyCompaniesImages
        /// </summary>
        /// <param name="getMyCompaniesImagesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetMyCompaniesImages")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyCompaniesImages([FromBody] GetMyCompaniesImagesPVM getMyCompaniesImagesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;
                var listOfArticlesFiles = automationApiBusiness.GetMyCompaniesImages(getMyCompaniesImagesPVM.ChildsUsersIds, getMyCompaniesImagesPVM.MyCompanyId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfArticlesFiles;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfArticlesFiles);
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



    }
}

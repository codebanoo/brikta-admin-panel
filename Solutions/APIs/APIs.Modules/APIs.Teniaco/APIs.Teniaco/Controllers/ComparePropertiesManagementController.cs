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
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// ComparePropertiesManagementController
    /// </summary>
    [CustomApiAuthentication]
    public class ComparePropertiesManagementController : ApiBaseController
    {
        /// <summary>
        /// ComparePropertiesManagementController
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
        public ComparePropertiesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllComparePropertiesListByPersonId
        /// </summary>
        /// <param name="getAllComparePropertiesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllComparePropertiesListByPersonId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllComparePropertiesListByPersonId([FromBody] GetAllComparePropertiesListByPersonIdPVM getAllComparePropertiesListByPersonId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllComparePropertiesListByPersonId.ChildsUsersIds == null)
                    {
                        getAllComparePropertiesListByPersonId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllComparePropertiesListByPersonId.ChildsUsersIds.Count == 0)
                        getAllComparePropertiesListByPersonId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllComparePropertiesListByPersonId.ChildsUsersIds.Count == 1)
                        if (getAllComparePropertiesListByPersonId.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllComparePropertiesListByPersonId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var ListOfCompareProperties = teniacoApiBusiness.GetAllComparePropertiesListByPersonId(
                    getAllComparePropertiesListByPersonId.ChildsUsersIds,
                     publicApiBusiness.PublicApiDb);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = ListOfCompareProperties;
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




        /// <summary>
        /// GetListOfComparePropertiesForBasicInfo
        /// </summary>
        /// <param name="getListOfComparePropertiesForBasicInfoPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetListOfComparePropertiesForBasicInfo")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfComparePropertiesForBasicInfo([FromBody] GetListOfComparePropertiesForBasicInfoPVM getListOfComparePropertiesForBasicInfoPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds == null)
                    {
                        getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds.Count == 0)
                        getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds.Count == 1)
                        if (getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                var listOfProperties = teniacoApiBusiness.GetListOfComparePropertiesForBasicInfo(
                    getListOfComparePropertiesForBasicInfoPVM.ChildsUsersIds,
                    getListOfComparePropertiesForBasicInfoPVM.PropertyId);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfProperties;


                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            {
                jsonResultWithRecordObjectVM.Result = "ERROR";
                jsonResultWithRecordObjectVM.Message = "ErrorInService";
            }
            return new JsonResult(jsonResultWithRecordObjectVM);

        }



        /// <summary>
        /// GetListOfCompareFeatureValues
        /// </summary>
        /// <param name="getListOfCompareFeatureValuesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetListOfCompareFeatureValues")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfCompareFeatureValues([FromBody] GetListOfCompareFeatureValuesPVM getListOfCompareFeatureValuesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfCompareFeatureValuesPVM.ChildsUsersIds == null)
                    {
                        getListOfCompareFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfCompareFeatureValuesPVM.ChildsUsersIds.Count == 0)
                        getListOfCompareFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfCompareFeatureValuesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfCompareFeatureValuesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfCompareFeatureValuesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfCompareFeatureValues = teniacoApiBusiness.GetListOfCompareFeatureValues(
                    getListOfCompareFeatureValuesPVM.ChildsUsersIds,
                    getListOfCompareFeatureValuesPVM.PropertyId);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfCompareFeatureValues;
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



        /// <summary>
        /// GetListOfComparePropertiesAddress
        /// </summary>
        /// <param name="getListOfComparePropertiesAddressPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetListOfComparePropertiesAddress")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfComparePropertiesAddress([FromBody] GetListOfComparePropertiesAddressPVM getListOfComparePropertiesAddressPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfComparePropertiesAddressPVM.ChildsUsersIds == null)
                    {
                        getListOfComparePropertiesAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfComparePropertiesAddressPVM.ChildsUsersIds.Count == 0)
                        getListOfComparePropertiesAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfComparePropertiesAddressPVM.ChildsUsersIds.Count == 1)
                        if (getListOfComparePropertiesAddressPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfComparePropertiesAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfComparePropertiesAddress = teniacoApiBusiness.GetListOfComparePropertiesAddress(
                    publicApiBusiness.PublicApiDb,
                    getListOfComparePropertiesAddressPVM.ChildsUsersIds,
                    getListOfComparePropertiesAddressPVM.PropertyId);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfComparePropertiesAddress;
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



        /// <summary>
        /// GetListOfComparePropertiesPricesHistories
        /// </summary>
        /// <param name="getListOfComparePropertiesPricesHistoriesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetListOfComparePropertiesPricesHistories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfComparePropertiesPricesHistories([FromBody] GetListOfComparePropertiesPricesHistoriesPVM getListOfComparePropertiesPricesHistoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfComparePropertiesPriceHistories = teniacoApiBusiness.GetListOfComparePropertiesPricesHistories(
                    getListOfComparePropertiesPricesHistoriesPVM.ChildsUsersIds,
                    getListOfComparePropertiesPricesHistoriesPVM.PropertyId);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfComparePropertiesPriceHistories;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }
    }
}

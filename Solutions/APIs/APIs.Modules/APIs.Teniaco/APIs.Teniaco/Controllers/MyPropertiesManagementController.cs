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
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesManagement
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
        public MyPropertiesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMyPropertiesList
        /// </summary>
        /// <param name="getAllMyPropertiesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyPropertiesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyPropertiesList([FromBody] GetAllMyPropertiesListPVM getAllMyPropertiesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyPropertiesListPVM.ChildsUsersIds == null)
                    {
                        getAllMyPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyPropertiesListPVM.ChildsUsersIds.Count == 0)
                        getAllMyPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyPropertiesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyPropertiesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetAllMyPropertiesList(
                    ref listCount,
                    getAllMyPropertiesListPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getAllMyPropertiesListPVM.PropertyTypeId,
                    getAllMyPropertiesListPVM.TypeOfUseId,
                    getAllMyPropertiesListPVM.DocumentTypeId,
                    getAllMyPropertiesListPVM.ConsultantUserId,
                    getAllMyPropertiesListPVM.OwnerId,
                    getAllMyPropertiesListPVM.PropertyCodeName,
                    getAllMyPropertiesListPVM.StateId,
                    getAllMyPropertiesListPVM.CityId,
                    getAllMyPropertiesListPVM.ZoneId,
                    getAllMyPropertiesListPVM.DistrictId,
                    getAllMyPropertiesListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfProperties;
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
        /// GetListOfMyProperties
        /// </summary>
        /// <param name="getListOfMyPropertiesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyProperties([FromBody] GetListOfMyPropertiesPVM
            getListOfMyPropertiesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertiesPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertiesPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertiesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetListOfMyProperties(
                    getListOfMyPropertiesPVM.jtStartIndex.Value,
                    getListOfMyPropertiesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyPropertiesPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getListOfMyPropertiesPVM.PropertyTypeId,
                    getListOfMyPropertiesPVM.TypeOfUseId,
                    getListOfMyPropertiesPVM.DocumentTypeId,
                    getListOfMyPropertiesPVM.ConsultantUserId,
                    getListOfMyPropertiesPVM.OwnerId,
                    getListOfMyPropertiesPVM.PropertyCodeName,
                    getListOfMyPropertiesPVM.StateId,
                    getListOfMyPropertiesPVM.CityId,
                    getListOfMyPropertiesPVM.ZoneId,
                    getListOfMyPropertiesPVM.DistrictId,
                    getListOfMyPropertiesPVM.jtSorting,
                    getListOfMyPropertiesPVM.UserId); // Passing the id of logged In User

                foreach (var property in listOfProperties)
                {
                    property.CountOfMaps = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("maps")).Count();
                    property.CountOfDocs = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("docs")).Count();
                    property.CountOfMedia = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("media")).Count();
                    property.CountOfPrices = teniacoApiBusiness.TeniacoApiDb.PropertiesPricesHistories.Where(f => f.PropertyId.Equals(property.PropertyId)).Count();
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfProperties;
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
        /// AddToMyProperties
        /// </summary>
        /// <param name="addToMyPropertiesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyProperties([FromBody] AddToMyPropertiesPVM
            addToMyPropertiesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMyPropertiesPVM.ChildsUsersIds == null)
                    {
                        addToMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToMyPropertiesPVM.ChildsUsersIds.Count == 0)
                        addToMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToMyPropertiesPVM.ChildsUsersIds.Count == 1)
                        if (addToMyPropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    addToMyPropertiesPVM.MyPropertiesVM.CreateEnDate = DateTime.Now;
                    addToMyPropertiesPVM.MyPropertiesVM.CreateTime = PersianDate.TimeNow;
                    addToMyPropertiesPVM.MyPropertiesVM.UserIdCreator = this.userId.Value;

                    addToMyPropertiesPVM.MyPropertiesVM.MyPropertyAddressVM.CreateEnDate = DateTime.Now;
                    addToMyPropertiesPVM.MyPropertiesVM.MyPropertyAddressVM.CreateTime = PersianDate.TimeNow;
                    addToMyPropertiesPVM.MyPropertiesVM.MyPropertyAddressVM.UserIdCreator = this.userId.Value;
                    addToMyPropertiesPVM.MyPropertiesVM.MyPropertyAddressVM.IsActivated = true;
                    addToMyPropertiesPVM.MyPropertiesVM.MyPropertyAddressVM.IsDeleted = false;
                }

                long propertyId = teniacoApiBusiness.AddToMyProperties(
                    addToMyPropertiesPVM.MyPropertiesVM,
                    publicApiBusiness/*,
                    addToPropertiesPVM.ChildsUsersIds*/);


                if (propertyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                if (propertyId > 0)
                {
                    addToMyPropertiesPVM.MyPropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToMyPropertiesPVM.MyPropertiesVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// GetMyPropertyWithMyPropertyId
        /// </summary>
        /// <param name="getMyPropertyWithMyPropertyIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetMyPropertyWithMyPropertyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMyPropertyWithMyPropertyId([FromBody] GetMyPropertyWithMyPropertyIdPVM
            getMyPropertyWithMyPropertyIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds == null)
                    {
                        getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds.Count == 0)
                        getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds.Count == 1)
                        if (getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var property = teniacoApiBusiness.GetMyPropertyWithMyPropertyId(
                   getMyPropertyWithMyPropertyIdPVM.PropertyId,
                   getMyPropertyWithMyPropertyIdPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb);


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = property;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateMyProperties
        /// </summary>
        /// <param name="updateMyPropertiesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyProperties([FromBody] UpdateMyPropertiesPVM
            updateMyPropertiesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMyPropertiesPVM.ChildsUsersIds == null)
                    {
                        updateMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateMyPropertiesPVM.ChildsUsersIds.Count == 0)
                        updateMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateMyPropertiesPVM.ChildsUsersIds.Count == 1)
                        if (updateMyPropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMyPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateMyPropertiesPVM.MyPropertiesVM.EditEnDate = DateTime.Now;
                    updateMyPropertiesPVM.MyPropertiesVM.EditTime = PersianDate.TimeNow;
                    updateMyPropertiesPVM.MyPropertiesVM.UserIdEditor = this.userId.Value;
                }

                var propertiesVM = updateMyPropertiesPVM.MyPropertiesVM;

                long propertyId = teniacoApiBusiness.UpdateMyProperties(
                    ref propertiesVM,
                    updateMyPropertiesPVM.ChildsUsersIds);

                if (propertyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";
                }
                else
                if (propertyId > 0)
                {
                    updateMyPropertiesPVM.MyPropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateMyPropertiesPVM.MyPropertiesVM;
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
        /// ToggleActivationMyProperties
        /// </summary>
        /// <param name="toggleActivationMyPropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMyProperties([FromBody] ToggleActivationMyPropertiesPVM
            toggleActivationMyPropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationMyProperties(
                   toggleActivationMyPropertiesPVM.PropertyId,
                   toggleActivationMyPropertiesPVM.UserId.Value,
                   toggleActivationMyPropertiesPVM.ChildsUsersIds))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                        jsonResultObjectVM.Result = returnMessage;
                    else
                        jsonResultObjectVM.Result = "OK";
                }

                return new JsonResult(jsonResultObjectVM);

            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);

        }


        /// <summary>
        /// TemporaryDeleteMyProperties
        /// </summary>
        /// <param name="temporaryDeleteMyPropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMyProperties([FromBody] TemporaryDeleteMyPropertiesPVM
            temporaryDeleteMyPropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteMyProperties(
                    temporaryDeleteMyPropertiesPVM.PropertyId,
                    temporaryDeleteMyPropertiesPVM.UserId.Value,
                    temporaryDeleteMyPropertiesPVM.ChildsUsersIds))
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
        /// CompleteDeleteMyProperties
        /// </summary>
        /// <param name="completeDeleteMyPropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyProperties([FromBody] CompleteDeleteMyPropertiesPVM completeDeleteMyPropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteMyProperties(
                   completeDeleteMyPropertiesPVM.PropertyId,
                   completeDeleteMyPropertiesPVM.ChildsUsersIds))
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
        /// GetMyPropertiesCompareBasicInfo
        /// </summary>
        /// <param name="getMyPropertiesCompareBasicInfoPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetMyPropertiesCompareBasicInfo")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]



        public IActionResult GetMyPropertiesCompareBasicInfo([FromBody] GetMyPropertiesCompareBasicInfoPVM getMyPropertiesCompareBasicInfoPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds == null)
                    {
                        getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds.Count == 0)
                        getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds.Count == 1)
                        if (getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMyPropertiesCompareBasicInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                var listOfMyProperties = teniacoApiBusiness.GetMyPropertiesCompareBasicInfo(publicApiBusiness.PublicApiDb, getMyPropertiesCompareBasicInfoPVM);
                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfMyProperties;


                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            {
                jsonResultWithRecordObjectVM.Result = "ERROR";
                jsonResultWithRecordObjectVM.Message = "ErrorInService";
            }
            return new JsonResult(jsonResultWithRecordObjectVM);

        }
        [HttpPost("GetAllMyPropertiesCompareTopic")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyPropertiesCompareTopic([FromBody] GetAllMyPropertiesCompareTopicPVM getAllMyPropertiesCompareTopicPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyPropertiesCompareTopicPVM.ChildsUsersIds == null)
                    {
                        getAllMyPropertiesCompareTopicPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyPropertiesCompareTopicPVM.ChildsUsersIds.Count == 0)
                        getAllMyPropertiesCompareTopicPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyPropertiesCompareTopicPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyPropertiesCompareTopicPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyPropertiesCompareTopicPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfMyProperties = teniacoApiBusiness.GetAllMyPropertiesCompareTopic(publicApiBusiness.PublicApiDb, getAllMyPropertiesCompareTopicPVM);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyProperties;
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
        /// GetAllMyFeaturesValuesCompare
        /// </summary>
        /// <param name="getMyFeaturesValuesComparePVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 
        [HttpPost("GetAllMyFeaturesValuesCompare")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyFeaturesValuesCompare([FromBody] GetMyFeaturesValuesComparePVM getMyFeaturesValuesComparePVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMyFeaturesValuesComparePVM.ChildsUsersIds == null)
                    {
                        getMyFeaturesValuesComparePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMyFeaturesValuesComparePVM.ChildsUsersIds.Count == 0)
                        getMyFeaturesValuesComparePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMyFeaturesValuesComparePVM.ChildsUsersIds.Count == 1)
                        if (getMyFeaturesValuesComparePVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMyFeaturesValuesComparePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfMyProperties = teniacoApiBusiness.GetAllMyFeaturesValuesCompare(getMyFeaturesValuesComparePVM);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyProperties;
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

        [HttpPost("GetAllPropertiesInfo")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPropertiesInfo([FromBody] GetAllPropertiesInfoPVM getAllPropertiesInfoPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPropertiesInfoPVM.ChildsUsersIds == null)
                    {
                        getAllPropertiesInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPropertiesInfoPVM.ChildsUsersIds.Count == 0)
                        getAllPropertiesInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPropertiesInfoPVM.ChildsUsersIds.Count == 1)
                        if (getAllPropertiesInfoPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPropertiesInfoPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

               

                var listOfMyProperties = teniacoApiBusiness.GetAllPropertiesInfo(getAllPropertiesInfoPVM);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMyProperties;
                //jsonResultWithRecordsObjectVM.TotalRecordCount = 0;

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

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
using VM.PVM.Public;

namespace APIs.Public.Controllers
{
    /// <summary>
    /// DistrictsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class DistrictsManagementController : ApiBaseController
    {
        /// <summary>
        /// DistrictsManagement
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
        public DistrictsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllDistrictsList
        /// </summary>
        /// <param name="getAllDistrictsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllDistrictsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllDistrictsList([FromBody] GetAllDistrictsListPVM getAllDistrictsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfDistricts = publicApiBusiness.GetAllDistrictsList(
                    getAllDistrictsListPVM.StateId,
                    getAllDistrictsListPVM.CityId,
                    getAllDistrictsListPVM.ZoneId,
                    getAllDistrictsListPVM.DistrictName);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDistricts;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);

        }

        /// <summary>
        /// GetAllDistrictsListWithOutStrPolygon
        /// </summary>
        /// <param name="getAllDistrictsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllDistrictsListWithOutStrPolygon")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllDistrictsListWithOutStrPolygon([FromBody] GetAllDistrictsListPVM getAllDistrictsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfDistricts = publicApiBusiness.GetAllDistrictsListWithOutStrPolygon(
                    getAllDistrictsListPVM.StateId,
                    getAllDistrictsListPVM.CityId,
                    getAllDistrictsListPVM.ZoneId,
                    getAllDistrictsListPVM.DistrictName);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDistricts;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);

        }

        /// <summary>
        /// GetListOfDistricts
        /// </summary>
        /// <param name="getListOfDistrictsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfDistricts([FromBody] GetListOfDistrictsPVM getListOfDistrictsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfDistricts = publicApiBusiness.GetListOfDistricts(
                   getListOfDistrictsPVM.jtStartIndex.Value,
                   getListOfDistrictsPVM.jtPageSize.Value,
                    ref listCount,
                   getListOfDistrictsPVM.StateId,
                   getListOfDistrictsPVM.CityId,
                   getListOfDistrictsPVM.ZoneId,
                   getListOfDistrictsPVM.DistrictName,
                   getListOfDistrictsPVM.jtSorting);

                foreach (var district in listOfDistricts)
                {
                  district.CountOfMaps = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("maps")).Count();
                  district.CountOfDocs = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("docs")).Count();
                  district.CountOfMedia = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("media")).Count();
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDistricts;
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
        /// GetListOfDistrictsWithOutStrPolygon
        /// </summary>
        /// <param name="getListOfDistrictsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfDistrictsWithOutStrPolygon")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfDistrictsWithOutStrPolygon([FromBody] GetListOfDistrictsPVM
            getListOfDistrictsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfDistricts = publicApiBusiness.GetListOfDistrictsWithOutStrPolygon(
                    getListOfDistrictsPVM.jtStartIndex.Value,
                    getListOfDistrictsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfDistrictsPVM.StateId,
                    getListOfDistrictsPVM.CityId,
                     getListOfDistrictsPVM.ZoneId,
                    getListOfDistrictsPVM.DistrictName,
                    getListOfDistrictsPVM.jtSorting);


                foreach (var district in listOfDistricts)
                {
                    district.CountOfMaps = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("maps")).Count();
                    district.CountOfDocs = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("docs")).Count();
                    district.CountOfMedia = publicApiBusiness.PublicApiDb.DistrictFiles.Where(f => f.DistrictId.Equals(district.DistrictId) && f.DistrictFileType.Equals("media")).Count();
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfDistricts;
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
        /// AddToDistricts
        /// </summary>
        /// <param name="addToDistrictsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToDistricts([FromBody] AddToDistrictsPVM
            addToDistrictsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if ((!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.DistrictName)) &&
                    (!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.VillageName)))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "Don'tFillAllFields";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if((!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.DistrictName)) &&
                         (!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.TownName))){
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "Don'tFillAllFields";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if ((!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.VillageName)) &&
                          (!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.TownName)))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "Don'tFillAllFields";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if ((!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.VillageName)) &&
                          (!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.TownName)) &&
                          (!string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.DistrictName)))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "Don'tFillAllFields";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if ((string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.VillageName)) &&
                          (string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.TownName)) &&
                          (string.IsNullOrEmpty(addToDistrictsPVM.DistrictsVM.DistrictName)))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "FillOneOfFields";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                {
                    long districtId = publicApiBusiness.AddToDistricts(
                  addToDistrictsPVM.DistrictsVM);
                    if (districtId.Equals(-1))
                    {
                        jsonResultWithRecordObjectVM.Result = "ERROR";
                        jsonResultWithRecordObjectVM.Message = "DuplicateDistrict";

                        return new JsonResult(jsonResultWithRecordObjectVM);
                    }
                    else
                      if (districtId > 0)
                    {
                        addToDistrictsPVM.DistrictsVM.DistrictId = districtId;
                        jsonResultWithRecordObjectVM.Result = "OK";
                        jsonResultWithRecordObjectVM.Record = addToDistrictsPVM.DistrictsVM;
                    }

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
        /// GetDistrictWithDistrictId
        /// </summary>
        /// <param name="getDistrictWithDistrictIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetDistrictWithDistrictId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetDistrictWithDistrictId([FromBody] GetDistrictWithDistrictIdPVM
            getDistrictWithDistrictIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var district = publicApiBusiness.GetDistrictWithDistrictId(
                    getDistrictWithDistrictIdPVM.DistrictId);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = district;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateDistricts
        /// </summary>
        /// <param name="updateDistrictsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateDistricts([FromBody] UpdateDistrictsPVM
            updateDistrictsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var districtsVM = updateDistrictsPVM.DistrictsVM;

                long districtId = publicApiBusiness.UpdateDistricts(
                    ref districtsVM,
                    updateDistrictsPVM.ChildsUsersIds);

                if (districtId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateZone";
                }
                else
                if (districtId > 0)
                {
                    updateDistrictsPVM.DistrictsVM.DistrictId = districtId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateDistrictsPVM.DistrictsVM;
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
        /// ToggleActivationDistricts
        /// </summary>
        /// <param name="toggleActivationDistrictsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationDistricts([FromBody] ToggleActivationDistrictsPVM
            toggleActivationDistrictsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationDistricts(
                   toggleActivationDistrictsPVM.DistrictId,
                   toggleActivationDistrictsPVM.UserId.Value,
                   toggleActivationDistrictsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteDistricts
        /// </summary>
        /// <param name="temporaryDeleteDistrictsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteDistricts([FromBody] TemporaryDeleteDistrictsPVM
            temporaryDeleteDistrictsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteDistricts(
                    temporaryDeleteDistrictsPVM.DistrictId,
                    temporaryDeleteDistrictsPVM.UserId.Value,
                    temporaryDeleteDistrictsPVM.ChildsUsersIds))
                {
                    jsonResultObjectVM.Result = "OK";

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
        /// CompleteDeleteDistricts
        /// </summary>
        /// <param name="completeDeleteDistrictsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteDistricts")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteDistricts([FromBody] CompleteDeleteDistrictsPVM completeDeleteDistrictsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteDistricts(
                   completeDeleteDistrictsPVM.DistrictId,
                   completeDeleteDistrictsPVM.ChildsUsersIds))
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
    }
}

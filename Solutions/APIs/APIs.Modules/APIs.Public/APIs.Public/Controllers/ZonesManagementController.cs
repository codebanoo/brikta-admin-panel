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
    /// ZonesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ZonesManagementController : ApiBaseController
    {
        /// <summary>
        /// ZonesManagement
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
        public ZonesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllZonesList
        /// </summary>
        /// <param name="getAllZonesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllZonesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllZonesList([FromBody] GetAllZonesListPVM getAllZonesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfZones = publicApiBusiness.GetAllZonesList(
                    ref listCount,
                    getAllZonesListPVM.StateId,
                    getAllZonesListPVM.CityId,
                    getAllZonesListPVM.SearchTitle,
                    getAllZonesListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfZones;

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
        /// GetAllZonesListWithOutStrPolygon
        /// </summary>
        /// <param name="getAllZonesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllZonesListWithOutStrPolygon")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllZonesListWithOutStrPolygon([FromBody] GetAllZonesListPVM getAllZonesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfZones = publicApiBusiness.GetAllZonesListWithOutStrPolygon(
                    ref listCount,
                    getAllZonesListPVM.StateId,
                    getAllZonesListPVM.CityId,
                    getAllZonesListPVM.SearchTitle,
                    getAllZonesListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfZones;

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
        /// GetListOfZones
        /// </summary>
        /// <param name="getListOfZonesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfZones([FromBody] GetListOfZonesPVM
            getListOfZonesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfZones = publicApiBusiness.GetListOfZones(
                    getListOfZonesPVM.jtStartIndex.Value,
                    getListOfZonesPVM.jtPageSize.Value,
                    ref listCount,
                    //getListOfZonesPVM.ChildsUsersIds,
                    getListOfZonesPVM.StateId,
                    getListOfZonesPVM.CityId,
                    getListOfZonesPVM.SearchTitle,
                    getListOfZonesPVM.jtSorting);


                foreach (var zone in listOfZones)
                {
                   zone.CountOfMaps = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("maps")).Count();
                   zone.CountOfDocs = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("docs")).Count();
                   zone.CountOfMedia = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("media")).Count();
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfZones;
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
        /// GetListOfZonesWithOutStrPolygon
        /// </summary>
        /// <param name="getListOfZonesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfZonesWithOutStrPolygon")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfZonesWithOutStrPolygon([FromBody] GetListOfZonesPVM
            getListOfZonesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfZones = publicApiBusiness.GetListOfZonesWithOutStrPolygon(
                    getListOfZonesPVM.jtStartIndex.Value,
                    getListOfZonesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfZonesPVM.StateId,
                    getListOfZonesPVM.CityId,
                    getListOfZonesPVM.SearchTitle,
                    getListOfZonesPVM.jtSorting);

                foreach (var zone in listOfZones)
                {
                    zone.CountOfMaps = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("maps")).Count();
                    zone.CountOfDocs = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("docs")).Count();
                    zone.CountOfMedia = publicApiBusiness.PublicApiDb.ZoneFiles.Where(f => f.ZoneId.Equals(zone.ZoneId) && f.ZoneFileType.Equals("media")).Count();
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfZones;
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
        /// AddToZones
        /// </summary>
        /// <param name="addToZonesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToZones([FromBody] AddToZonesPVM
            addToZonesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                long zoneId = publicApiBusiness.AddToZones(
                    addToZonesPVM.ZonesVM/*,
                    addToZonesPVM.ChildsUsersIds*/);


                if (zoneId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateZone";
                }
                else
                if (zoneId > 0)
                {
                    addToZonesPVM.ZonesVM.ZoneId = zoneId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToZonesPVM.ZonesVM;
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
        /// GetZoneWithZoneId
        /// </summary>
        /// <param name="getZoneWithZoneIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetZoneWithZoneId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetZoneWithZoneId([FromBody] GetZoneWithZoneIdPVM
            getZoneWithZoneIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                var zone = publicApiBusiness.GetZoneWithZoneId(
                    getZoneWithZoneIdPVM.ZoneId/*,
                    updateZonesPVM.ChildsUsersIds*/);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = zone;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// UpdateZones
        /// </summary>
        /// <param name="updateZonesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateZones([FromBody] UpdateZonesPVM
            updateZonesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var educationLevelsVM = updateZonesPVM.ZonesVM;

                long zoneId = publicApiBusiness.UpdateZones(
                    ref educationLevelsVM/*,
                    updateZonesPVM.ChildsUsersIds*/);

                if (zoneId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateZone";
                }
                else
                if (zoneId > 0)
                {
                    updateZonesPVM.ZonesVM.ZoneId = zoneId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateZonesPVM.ZonesVM;
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
        /// ToggleActivationZones
        /// </summary>
        /// <param name="toggleActivationZonesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationZones([FromBody] ToggleActivationZonesPVM
            toggleActivationZonesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (publicApiBusiness.ToggleActivationZones(
                    toggleActivationZonesPVM.ZoneId,
                    toggleActivationZonesPVM.UserId.Value,
                    toggleActivationZonesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteZones
        /// </summary>
        /// <param name="temporaryDeleteZonesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteZones([FromBody] TemporaryDeleteZonesPVM
            temporaryDeleteZonesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (publicApiBusiness.TemporaryDeleteZones(
                    temporaryDeleteZonesPVM.ZoneId,
                    temporaryDeleteZonesPVM.UserId.Value,
                    temporaryDeleteZonesPVM.ChildsUsersIds))
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
        /// CompleteDeleteZones
        /// </summary>
        /// <param name="completeDeleteZonesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteZones")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteZones([FromBody] CompleteDeleteZonesPVM completeDeleteZonesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (publicApiBusiness.CompleteDeleteZones(
                    completeDeleteZonesPVM.ZoneId,
                    completeDeleteZonesPVM.ChildsUsersIds))
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

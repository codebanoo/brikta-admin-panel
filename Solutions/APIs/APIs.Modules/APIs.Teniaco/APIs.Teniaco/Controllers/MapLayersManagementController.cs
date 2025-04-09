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
    /// MapLayersManagement
    /// </summary>
    /// 

    [CustomApiAuthentication]

    public class MapLayersManagementController : ApiBaseController
    {

        /// <summary>
        /// MapLayersManagement
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
        public MapLayersManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMapLayersList
        /// </summary>
        /// <param name="getAllMapLayersListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllMapLayersList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllMapLayersList([FromBody] GetAllMapLayersListPVM getAllMapLayersListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMapLayersListPVM.ChildsUsersIds == null)
                    {
                        getAllMapLayersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllMapLayersListPVM.ChildsUsersIds.Count == 0)
                        getAllMapLayersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllMapLayersListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMapLayersListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMapLayersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfMapLayers = teniacoApiBusiness.GetAllMapLayersList(
                    ref listCount,
                     getAllMapLayersListPVM.ChildsUsersIds,
                     getAllMapLayersListPVM.MapLayerCategoryId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMapLayers;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetListOfMapLayers
        /// </summary>
        /// <param name="getListOfMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMapLayers([FromBody] GetListOfMapLayersPVM getListOfMapLayersPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMapLayersPVM.ChildsUsersIds == null)
                    {
                        getListOfMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMapLayersPVM.ChildsUsersIds.Count == 0)
                        getListOfMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMapLayersPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMapLayersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }
               
                int listCount = 0;

                var listOfMapLayers = teniacoApiBusiness.GetListOfMapLayers(
                   getListOfMapLayersPVM.jtStartIndex.Value,
                   getListOfMapLayersPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfMapLayersPVM.ChildsUsersIds,
                   getListOfMapLayersPVM.MapLayerCategoryId,
                   getListOfMapLayersPVM.jtSorting);
               
                #region State
                bool _lst = listOfMapLayers.Any(x => x.DistrictId > 0);
                if (_lst)
                {
                    var cities = publicApiBusiness.PublicApiDb.Cities.ToList();
                    var stats = publicApiBusiness.PublicApiDb.States.ToList();
                    var zones = publicApiBusiness.PublicApiDb.Zones.ToList();
                    var Districts = publicApiBusiness.PublicApiDb.Districts.ToList();
                    listOfMapLayers.Where(x => x.DistrictId > 0).ToList().ForEach(a =>
                    {
                        int? _zonId = (int?)Districts.FirstOrDefault(d => d.DistrictId == a.DistrictId)?.ZoneId;
                        int? _cityid = (int?)zones.FirstOrDefault(z => z.ZoneId == _zonId)?.CityId;
                        int? _state = (int?)cities.FirstOrDefault(s => s.CityId == _cityid)?.StateId;
                        a.StateId = _state;
                        a.CityId = _cityid;
                        a.ZoneId = _zonId;
                    });
                }
                #endregion

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMapLayers;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        [HttpPost("AddToMapLayersWithJsonData")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMapLayersWithJsonData([FromBody] AddToMapLayersJsonDataPVM addToMapLayersJsonDataPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMapLayersJsonDataPVM.ChildsUsersIds == null)
                    {
                        addToMapLayersJsonDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToMapLayersJsonDataPVM.ChildsUsersIds.Count == 0)
                        {
                            addToMapLayersJsonDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToMapLayersJsonDataPVM.ChildsUsersIds.Count == 1)
                                if (addToMapLayersJsonDataPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToMapLayersJsonDataPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                    }

                    addToMapLayersJsonDataPVM.MapLayersVM.CreateEnDate = DateTime.Now;
                    addToMapLayersJsonDataPVM.MapLayersVM.CreateTime = PersianDate.TimeNow;
                    addToMapLayersJsonDataPVM.MapLayersVM.UserIdCreator = this.domainsSettings.UserIdCreator.Value;

                    addToMapLayersJsonDataPVM.MapLayersVM.IsActivated = true;
                    addToMapLayersJsonDataPVM.MapLayersVM.IsDeleted = false;
                }

                int mapLayerId = teniacoApiBusiness.AddToMapLayersWithJsonData(
                   addToMapLayersJsonDataPVM.MapLayersVM, addToMapLayersJsonDataPVM.StrPolygonList,
                   consoleBusiness,
                   this.domainsSettings.DomainName);


                if (mapLayerId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateMapLayer";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (mapLayerId > 0)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }




        /// <summary>
        /// AddToMapLayers
        /// </summary>
        /// <param name="addToMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMapLayers([FromBody] AddToMapLayersPVM addToMapLayersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMapLayersPVM.ChildsUsersIds == null)
                    {
                        addToMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToMapLayersPVM.ChildsUsersIds.Count == 0)
                        {
                            addToMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToMapLayersPVM.ChildsUsersIds.Count == 1)
                                if (addToMapLayersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToMapLayersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                    }

                    addToMapLayersPVM.MapLayersVM.CreateEnDate = DateTime.Now;
                    addToMapLayersPVM.MapLayersVM.CreateTime = PersianDate.TimeNow;
                    addToMapLayersPVM.MapLayersVM.UserIdCreator = this.userId;

                    addToMapLayersPVM.MapLayersVM.CreateEnDate = DateTime.Now;
                    addToMapLayersPVM.MapLayersVM.CreateTime = PersianDate.TimeNow;
                    addToMapLayersPVM.MapLayersVM.UserIdCreator = this.userId;
                    addToMapLayersPVM.MapLayersVM.IsActivated = true;
                    addToMapLayersPVM.MapLayersVM.IsDeleted = false;
                }

                int mapLayerId = teniacoApiBusiness.AddToMapLayers(
                   addToMapLayersPVM.MapLayersVM,
                   consoleBusiness,
                   this.domainsSettings.DomainName);


                if (mapLayerId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateMapLayer";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (mapLayerId > 0)
                {
                    addToMapLayersPVM.MapLayersVM.MapLayerId = mapLayerId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToMapLayersPVM.MapLayersVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateMapLayers
        /// </summary>
        /// <param name="updateMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("UpdateMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateMapLayers([FromBody] UpdateMapLayersPVM updateMapLayersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var mapLayersVM = updateMapLayersPVM.MapLayersVM;

                int mapLayerId = teniacoApiBusiness.UpdateMapLayers(
                    ref mapLayersVM,
                    updateMapLayersPVM.ChildsUsersIds);

                if (mapLayerId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateMapLayer";
                }
                else
                if (mapLayerId  > 0)
                {
                    updateMapLayersPVM.MapLayersVM.MapLayerId= mapLayerId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateMapLayersPVM.MapLayersVM;
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
        /// ToggleActivationMapLayers
        /// </summary>
        /// <param name="toggleActivationMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationMapLayers([FromBody] ToggleActivationMapLayersPVM toggleActivationMapLayersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationMapLayers(

                    toggleActivationMapLayersPVM.MapLayerId,
                    toggleActivationMapLayersPVM.UserId.Value,
                    toggleActivationMapLayersPVM.ChildsUsersIds))
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



        /// <summary>
        /// TemporaryDeleteMapLayers
        /// </summary>
        /// <param name="temporaryDeleteMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteMapLayers([FromBody] TemporaryDeleteMapLayersPVM temporaryDeleteMapLayersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteMapLayers(
                    temporaryDeleteMapLayersPVM.MapLayerId,
                    temporaryDeleteMapLayersPVM.UserId.Value,
                    temporaryDeleteMapLayersPVM.ChildsUsersIds))
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
        [HttpPost("CompleteDeleteMapLayersIds")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult CompleteDeleteMapLayersIds([FromBody] CompleteDeleteMapLayersIdsPVM completeDeleteMapLayersIdsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";
                if ( teniacoApiBusiness.CompleteDeleteMapLayersIds(completeDeleteMapLayersIdsPVM))
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



        /// <summary>
        /// CompleteDeleteMapLayers
        /// </summary>
        /// <param name="completeDeleteMapLayersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteMapLayers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult CompleteDeleteMapLayers([FromBody] CompleteDeleteMapLayersPVM completeDeleteMapLayersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteMapLayers(
                    completeDeleteMapLayersPVM.MapLayerId,
                    completeDeleteMapLayersPVM.ChildsUsersIds))
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


        /// <summary>
        /// GetMapLayerWithMapLayerId
        /// </summary>
        /// <param name="getMapLayerWithMapLayerIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetMapLayerWithMapLayerId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMapLayerWithMapLayerId([FromBody] GetMapLayerWithMapLayerIdPVM
            getMapLayerWithMapLayerIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMapLayerWithMapLayerIdPVM.ChildsUsersIds == null)
                    {
                        getMapLayerWithMapLayerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMapLayerWithMapLayerIdPVM.ChildsUsersIds.Count == 0)
                        getMapLayerWithMapLayerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMapLayerWithMapLayerIdPVM.ChildsUsersIds.Count == 1)
                        if (getMapLayerWithMapLayerIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMapLayerWithMapLayerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var MapLayers = teniacoApiBusiness.GetMapLayerWithMapLayerId(
                    getMapLayerWithMapLayerIdPVM.MapLayerId,
                    getMapLayerWithMapLayerIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = MapLayers;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }
    }
}

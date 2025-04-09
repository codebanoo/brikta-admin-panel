using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using APIs.Teniaco.Models.Entities;
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
    /// MapLayerCategoriesManagement
    /// </summary>
    /// 

    [CustomApiAuthentication]

    public class MapLayerCategoriesManagementController : ApiBaseController
    {
        /// <summary>
        /// MapLayerCategoriesManagement
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
        public MapLayerCategoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMapLayerCategoriesList
        /// </summary>
        /// <param name="getAllMapLayerCategoriesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllMapLayerCategoriesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllMapLayerCategoriesList([FromBody] GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMapLayerCategoriesListPVM.ChildsUsersIds == null)
                    {
                        getAllMapLayerCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllMapLayerCategoriesListPVM.ChildsUsersIds.Count == 0)
                        getAllMapLayerCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllMapLayerCategoriesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMapLayerCategoriesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMapLayerCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfMapLayerCategories = teniacoApiBusiness.GetAllMapLayerCategoriesList(
                    ref listCount,
                     getAllMapLayerCategoriesListPVM.ChildsUsersIds,
                     getAllMapLayerCategoriesListPVM.MapLayerCategoryTitle);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMapLayerCategories;
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
        /// GetListOfMapLayerCategories
        /// </summary>
        /// <param name="getListOfMapLayerCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMapLayerCategories([FromBody] GetListOfMapLayerCategoriesPVM getListOfMapCategoriesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMapCategoriesPVM.ChildsUsersIds == null)
                    {
                        getListOfMapCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMapCategoriesPVM.ChildsUsersIds.Count == 0)
                        getListOfMapCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMapCategoriesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMapCategoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMapCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfMapLayerCategories = teniacoApiBusiness.GetListOfMapLayerCategories(
                   getListOfMapCategoriesPVM.jtStartIndex.Value,
                   getListOfMapCategoriesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfMapCategoriesPVM.ChildsUsersIds,
                   getListOfMapCategoriesPVM.MapLayerCategoryTitle,
                   getListOfMapCategoriesPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMapLayerCategories;
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
        /// AddToMapLayerCategories
        /// </summary>
        /// <param name="addToMapLayerCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMapLayerCategories([FromBody] AddToMapLayerCategoriesPVM addToMapLayerCategoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMapLayerCategoriesPVM.ChildsUsersIds == null)
                    {
                        addToMapLayerCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToMapLayerCategoriesPVM.ChildsUsersIds.Count == 0)
                        {
                            addToMapLayerCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToMapLayerCategoriesPVM.ChildsUsersIds.Count == 1)
                                if (addToMapLayerCategoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToMapLayerCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }

                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.CreateEnDate = DateTime.Now;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.CreateTime = PersianDate.TimeNow;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.UserIdCreator = this.userId.Value;

                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.CreateEnDate = DateTime.Now;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.CreateTime = PersianDate.TimeNow;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.UserIdCreator = this.userId.Value;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.IsActivated = true;
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.IsDeleted = false;
                }

                int mapLayerCategoryId = teniacoApiBusiness.AddToMapLayerCategories(
                   addToMapLayerCategoriesPVM.MapLayerCategoriesVM,
                   consoleBusiness,
                   this.domainsSettings.DomainName);


                if (mapLayerCategoryId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateMapLayerCategory";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (mapLayerCategoryId > 0)
                {
                    addToMapLayerCategoriesPVM.MapLayerCategoriesVM.MapLayerCategoryId = mapLayerCategoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToMapLayerCategoriesPVM.MapLayerCategoriesVM;

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
        /// UpdateMapLayerCategories
        /// </summary>
        /// <param name="updateMapLayerCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>


        [HttpPost("UpdateMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateMapLayerCategories([FromBody] UpdateMapLayerCategoriesPVM updateMapLayerCategoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var MapCategoriesVM = updateMapLayerCategoriesPVM.MapLayerCategoriesVM;

                int MapLayerCatgoryId = teniacoApiBusiness.UpdateMapLayerCategories(
                    ref MapCategoriesVM,
                    updateMapLayerCategoriesPVM.ChildsUsersIds);

                if (MapLayerCatgoryId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateMapLayerCategory";
                }
                else
                if (MapLayerCatgoryId > 0)
                {
                    updateMapLayerCategoriesPVM.MapLayerCategoriesVM.MapLayerCategoryId = MapLayerCatgoryId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateMapLayerCategoriesPVM.MapLayerCategoriesVM;
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
        /// ToggleActivationMapLayerCategories
        /// </summary>
        /// <param name="toggleActivationMapCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationMapCategories([FromBody] ToggleActivationMapLayerCategoriesPVM toggleActivationMapLayerCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationMapLayerCategories(

                    toggleActivationMapLayerCategoriesPVM.MapLayerCategoryId,
                    toggleActivationMapLayerCategoriesPVM.UserId.Value,
                    toggleActivationMapLayerCategoriesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteMapLayerCategories
        /// </summary>
        /// <param name="temporaryDeleteMapLayerCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteMapLayerCategories([FromBody] TemporaryDeleteMapLayerCategoriesPVM temporaryDeleteMapLayerCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteMapLayerCategories(
                    temporaryDeleteMapLayerCategoriesPVM.MapLayerCategoryId,
                    temporaryDeleteMapLayerCategoriesPVM.UserId.Value,
                    temporaryDeleteMapLayerCategoriesPVM.ChildsUsersIds))
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
        /// CompleteDeleteMapLayerCategories
        /// </summary>
        /// <param name="completeDeleteMapLayerCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteMapLayerCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteMapLayerCategories([FromBody] CompleteDeleteMapLayerCategoriesPVM completeDeleteMapLayerCategoriesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteMapLayerCategories(
                    completeDeleteMapLayerCategoriesPVM.MapLayerCategoryId,
                    completeDeleteMapLayerCategoriesPVM.ChildsUsersIds))
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
        /// GetMapLayerCategoryWithMapLayerCategoryId
        /// </summary>
        /// <param name="getMapLayerCategoryWithMapLayerCategoryIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetMapLayerCategoryWithMapLayerCategoryId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMapLayerCategoryWithMapLayerCategoryId([FromBody] GetMapLayerCategoryWithMapLayerCategoryIdPVM
            getMapLayerCategoryWithMapLayerCategoryIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds == null)
                    {
                        getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds.Count == 0)
                        getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds.Count == 1)
                        if (getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var MapCategories = teniacoApiBusiness.GetMapLayerCategoryWithMapLayerCategoryId(
                    getMapLayerCategoryWithMapLayerCategoryIdPVM.MapLayerCategoryId,
                    getMapLayerCategoryWithMapLayerCategoryIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = MapCategories;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

        /// <summary>
        /// GetListOfPropertiesPricesForMap
        /// </summary>
        /// <param name="getMapLayerCategoryWithMapLayerCategoryIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetListOfPropertiesPricesForMap")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPropertiesPricesForMap([FromBody] GetListOfPropertiesPricesForMapPVM
            getListOfPropertiesPricesForMapPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertiesPricesForMapPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertiesPricesForMapPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertiesPricesForMapPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertiesPricesForMapPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertiesPricesForMapPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertiesPricesForMapPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertiesPricesForMapPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var propertiesPricesForMap = teniacoApiBusiness.GetListOfPropertiesPricesForMap(
                    getListOfPropertiesPricesForMapPVM.ChildsUsersIds,
                    getListOfPropertiesPricesForMapPVM.Platform,//all, teniaco, melkavan
                    getListOfPropertiesPricesForMapPVM.PriceFrom,
                    getListOfPropertiesPricesForMapPVM.PriceTo,
                    getListOfPropertiesPricesForMapPVM.StateId,
                    getListOfPropertiesPricesForMapPVM.CityId,
                    getListOfPropertiesPricesForMapPVM.ZoneId,
                    getListOfPropertiesPricesForMapPVM.DistrictId,
                    getListOfPropertiesPricesForMapPVM.TypeOfUseId,
                    getListOfPropertiesPricesForMapPVM.PropertyTypeId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = propertiesPricesForMap;

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




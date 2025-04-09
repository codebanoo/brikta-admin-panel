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
using VM.PVM.Public;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementManagement
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
        public AdvertisementManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllAdvertisementList
        /// </summary>
        /// <param name="getAllAdvertisementListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllAdvertisementList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAdvertisementList([FromBody] GetAllAdvertisementListPVM getAllAdvertisementListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAdvertisementListPVM.ChildsUsersIds == null)
                    {
                        getAllAdvertisementListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllAdvertisementListPVM.ChildsUsersIds.Count == 0)
                        getAllAdvertisementListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllAdvertisementListPVM.ChildsUsersIds.Count == 1)
                        if (getAllAdvertisementListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAdvertisementListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAdvertisement = melkavanApiBusiness.GetAllAdvertisementList(
                    ref listCount,
                    getAllAdvertisementListPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getAllAdvertisementListPVM.AdvertisementTypeId,
                    getAllAdvertisementListPVM.PropertyTypeId,
                    getAllAdvertisementListPVM.TypeOfUseId,
                    getAllAdvertisementListPVM.DocumentTypeId,
                    //getAllAdvertisementListPVM.IntermediaryPersonId,
                    //getAllAdvertisementListPVM.OwnerPersonId,
                    //getAllAdvertisementListPVM.DocumentOwnershipTypeId,
                    //getAllAdvertisementListPVM.DocumentRootTypeId,
                    getAllAdvertisementListPVM.PropertyCodeName,
                    getAllAdvertisementListPVM.StateId,
                    getAllAdvertisementListPVM.CityId,
                    getAllAdvertisementListPVM.ZoneId,
                    getAllAdvertisementListPVM.DistrictId,
                    //getAllAdvertisementListPVM.Intermediary,
                    //getAllAdvertisementListPVM.IsPrivate,
                    getAllAdvertisementListPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisement;
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
        /// GetListOfAdvertisement
        /// </summary>
        /// <param name="getListOfAdvertisementPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAdvertisement([FromBody] GetListOfAdvertisementPVM
            getListOfAdvertisementPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAdvertisementPVM.ChildsUsersIds == null)
                    {
                        getListOfAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAdvertisementPVM.ChildsUsersIds.Count == 0)
                        getListOfAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAdvertisementPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAdvertisementPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAdvertisement = melkavanApiBusiness.GetListOfAdvertisement(
                    getListOfAdvertisementPVM.jtStartIndex.Value,
                    getListOfAdvertisementPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfAdvertisementPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getListOfAdvertisementPVM.AdvertisementTypeId,
                    getListOfAdvertisementPVM.PropertyTypeId,
                    getListOfAdvertisementPVM.TypeOfUseId,
                    getListOfAdvertisementPVM.DocumentTypeId,
                    //getListOfAdvertisementPVM.IntermediaryPersonId,
                    //getListOfAdvertisementPVM.OwnerPersonId,
                    //getListOfAdvertisementPVM.DocumentOwnershipTypeId,
                    //getListOfAdvertisementPVM.DocumentRootTypeId,
                    getListOfAdvertisementPVM.PropertyCodeName,
                    getListOfAdvertisementPVM.StateId,
                    getListOfAdvertisementPVM.CityId,
                    getListOfAdvertisementPVM.ZoneId,
                    getListOfAdvertisementPVM.DistrictId,
                    //getListOfAdvertisementPVM.Intermediary,
                    //getListOfAdvertisementPVM.IsPrivate,
                    getListOfAdvertisementPVM.jtSorting,
                    getListOfAdvertisementPVM.OnlyFavorite,
                    getListOfAdvertisementPVM.UserId,
                    getListOfAdvertisementPVM.AdvertisementTitle);

                foreach (var Advertisement in listOfAdvertisement)
                {

                    Advertisement.CountOfMaps = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("maps")).Count();
                    Advertisement.CountOfDocs = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("docs")).Count();
                    Advertisement.CountOfMedia = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("media")).Count();
                    Advertisement.CountOfPrices = melkavanApiBusiness.MelkavanApiDb.AdvertisementPricesHistories.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId)).Count();
                }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisement;
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
        /// GetListOfAdvertisementAdvanceSearchPVM
        /// </summary>
        /// <param name="getListOfAdvertisementAdvanceSearchPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAdverisementsAdvanceSearch")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAdverisementsAdvanceSearch([FromBody] GetListOfAdvertisementAdvanceSearchPVM
            getListOfAdvertisementAdvanceSearchPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds == null)
                    {
                        getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds.Count == 0)
                        getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAdvertisements = melkavanApiBusiness.GetListOfAdverisementsAdvanceSearch(
                   jtStartIndex: getListOfAdvertisementAdvanceSearchPVM.jtStartIndex.Value,
                   jtPageSize: getListOfAdvertisementAdvanceSearchPVM.jtPageSize.Value,
                   listCount: ref listCount,
                   childsUsersIds: getListOfAdvertisementAdvanceSearchPVM.ChildsUsersIds,
                   publicApiDb: publicApiBusiness.PublicApiDb,
                   teniacoApiDb: teniacoApiBusiness.TeniacoApiDb,
                   jtSorting: getListOfAdvertisementAdvanceSearchPVM.jtSorting,
                   advertisementTitle: getListOfAdvertisementAdvanceSearchPVM.AdvertisementTitle,
                   onlyFavorite: getListOfAdvertisementAdvanceSearchPVM.OnlyFavorite,
                   userId: getListOfAdvertisementAdvanceSearchPVM.UserId,
                   IsFiltered: getListOfAdvertisementAdvanceSearchPVM.IsFiltered,
                   //Parameters for advanced filters
                   AdvertisementTypeId: getListOfAdvertisementAdvanceSearchPVM.AdvertisementTypeId,
                   PropertyTypeId: getListOfAdvertisementAdvanceSearchPVM.PropertyTypeId,
                   TypeOfUseId: getListOfAdvertisementAdvanceSearchPVM.TypeOfUseId,
                   StateId: getListOfAdvertisementAdvanceSearchPVM.StateId,
                   CityId: getListOfAdvertisementAdvanceSearchPVM.CityId,
                   ZoneId: getListOfAdvertisementAdvanceSearchPVM.ZoneId,
                   TownName: getListOfAdvertisementAdvanceSearchPVM.TownName,
                   FromArea: getListOfAdvertisementAdvanceSearchPVM.FromArea,
                   ToArea: getListOfAdvertisementAdvanceSearchPVM.ToArea,
                   FromFoundation: getListOfAdvertisementAdvanceSearchPVM.FromFoundation,
                   ToFoundation: getListOfAdvertisementAdvanceSearchPVM.ToFoundation,
                   FromPrice: getListOfAdvertisementAdvanceSearchPVM.FromPrice,
                   ToPrice: getListOfAdvertisementAdvanceSearchPVM.ToPrice,
                   BuildingLifeId: getListOfAdvertisementAdvanceSearchPVM.BuildingLifeId,
                   FromRebuiltInYear: getListOfAdvertisementAdvanceSearchPVM.FromRebuiltInYear,
                   ToRebuiltInYear: getListOfAdvertisementAdvanceSearchPVM.ToRebuiltInYear,
                   DocumentTypeId: getListOfAdvertisementAdvanceSearchPVM.DocumentTypeId,
                   DocumentRootTypeId: getListOfAdvertisementAdvanceSearchPVM.DocumentRootTypeId,
                   DocumentOwnershipTypeId: getListOfAdvertisementAdvanceSearchPVM.DocumentOwnershipTypeId,
                   DepositFromPrice: getListOfAdvertisementAdvanceSearchPVM.DepositFromPrice,
                    DepositToPrice: getListOfAdvertisementAdvanceSearchPVM.DepositToPrice,
                   RentFromPrice: getListOfAdvertisementAdvanceSearchPVM.RentFromPrice,
                   RentToPrice: getListOfAdvertisementAdvanceSearchPVM.RentToPrice,
                   MaritalStatusId: getListOfAdvertisementAdvanceSearchPVM.MaritalStatusId,
                    Convertable: getListOfAdvertisementAdvanceSearchPVM.Convertable,
                    Features: getListOfAdvertisementAdvanceSearchPVM.Features);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisements;
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
        /// AddToAdvertisement
        /// </summary>
        /// <param name="addToAdvertisementPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAdvertisement([FromBody] AddToAdvertisementPVM
            addToAdvertisementPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAdvertisementPVM.ChildsUsersIds == null)
                    {
                        addToAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToAdvertisementPVM.ChildsUsersIds.Count == 0)
                        addToAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToAdvertisementPVM.ChildsUsersIds.Count == 1)
                        if (addToAdvertisementPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    addToAdvertisementPVM.AdvertisementVM.CreateEnDate = DateTime.Now;
                    addToAdvertisementPVM.AdvertisementVM.CreateTime = PersianDate.TimeNow;
                    addToAdvertisementPVM.AdvertisementVM.UserIdCreator = this.userId.Value;

                    //addToAdvertisementPVM.AdvertisementVM.AdvertisementAddressVM.CreateEnDate = DateTime.Now;
                    //addToAdvertisementPVM.AdvertisementVM.AdvertisementAddressVM.CreateTime = PersianDate.TimeNow;
                    //addToAdvertisementPVM.AdvertisementVM.AdvertisementAddressVM.UserIdCreator = this.userId.Value;
                    //addToAdvertisementPVM.AdvertisementVM.AdvertisementAddressVM.IsActivated = true;
                    //addToAdvertisementPVM.AdvertisementVM.AdvertisementAddressVM.IsDeleted = false;

                }

                long AdvertisementId = melkavanApiBusiness.AddToAdvertisement(
                    addToAdvertisementPVM.AdvertisementVM,
                    publicApiBusiness,
                    consoleBusiness/*,
                    addToAdvertisementPVM.ChildsUsersIds*/);


                if (AdvertisementId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAdvertisement";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                if (AdvertisementId > 0)
                {
                    addToAdvertisementPVM.AdvertisementVM.AdvertisementId = AdvertisementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAdvertisementPVM.AdvertisementVM;

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
        /// GetAdvertisementWithAdvertisementId
        /// </summary>
        /// <param name="getAdvertisementWithAdvertisementIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetAdvertisementWithAdvertisementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertisementWithAdvertisementId([FromBody] GetAdvertisementWithAdvertisementIdPVM
            getAdvertisementWithAdvertisementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var Advertisement = melkavanApiBusiness.GetAdvertisementWithAdvertisementId(
                     getAdvertisementWithAdvertisementIdPVM.AdvertisementId,
                     getAdvertisementWithAdvertisementIdPVM.ChildsUsersIds,
                     consoleBusiness,
                     publicApiBusiness.PublicApiDb,
                     teniacoApiBusiness.TeniacoApiDb,
                     melkavanApiBusiness.MelkavanApiDb,
                     getAdvertisementWithAdvertisementIdPVM.UserId,
                     getAdvertisementWithAdvertisementIdPVM.Type);



                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = Advertisement;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// UpdateAdvertisement
        /// </summary>
        /// <param name="updateAdvertisementPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisement([FromBody] UpdateAdvertisementPVM
            updateAdvertisementPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAdvertisementPVM.ChildsUsersIds == null)
                    {
                        updateAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateAdvertisementPVM.ChildsUsersIds.Count == 0)
                        updateAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateAdvertisementPVM.ChildsUsersIds.Count == 1)
                        if (updateAdvertisementPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAdvertisementPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateAdvertisementPVM.AdvertisementVM.EditEnDate = DateTime.Now;
                    updateAdvertisementPVM.AdvertisementVM.EditTime = PersianDate.TimeNow;
                    updateAdvertisementPVM.AdvertisementVM.UserIdEditor = this.userId.Value;
                }

                var AdvertisementVM = updateAdvertisementPVM.AdvertisementVM;

                long AdvertisementId = melkavanApiBusiness.UpdateAdvertisement(
                    ref AdvertisementVM,
                    updateAdvertisementPVM.ChildsUsersIds,
                     teniacoApiBusiness.TeniacoApiDb);


                if (AdvertisementId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAdvertisement";
                }
                else
                if (AdvertisementId > 0)
                {
                    updateAdvertisementPVM.AdvertisementVM.AdvertisementId = AdvertisementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateAdvertisementPVM.AdvertisementVM;
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
        /// ToggleActivationAdvertisement
        /// </summary>
        /// <param name="toggleActivationAdvertisementPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationAdvertisement([FromBody] ToggleActivationAdvertisementPVM
            toggleActivationAdvertisementPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (melkavanApiBusiness.ToggleActivationAdvertisement(
                    toggleActivationAdvertisementPVM.AdvertisementId,
                    toggleActivationAdvertisementPVM.UserId.Value,
                    toggleActivationAdvertisementPVM.ChildsUsersIds))
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
        /// TemporaryDeleteAdvertisement
        /// </summary>
        /// <param name="temporaryDeleteAdvertisementPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteAdvertisement([FromBody] TemporaryDeleteAdvertisementPVM
            temporaryDeleteAdvertisementPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (melkavanApiBusiness.TemporaryDeleteAdvertisement(
                    temporaryDeleteAdvertisementPVM.AdvertisementId,
                    temporaryDeleteAdvertisementPVM.UserId.Value,
                    temporaryDeleteAdvertisementPVM.ChildsUsersIds))
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
        /// TemporaryDeleteAdvertisementWithChild
        /// </summary>
        /// <param name="temporaryDeleteAdvertisementWithChildPVM"></param>
        /// <returns></returns>
        [HttpPost("TemporaryDeleteAdvertisementWithChild")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteAdvertisementWithChild([FromBody] TemporaryDeleteAdvertisementWithChildPVM temporaryDeleteAdvertisementWithChildPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (melkavanApiBusiness.TemporaryDeleteAdvertisementWithChild(
                    temporaryDeleteAdvertisementWithChildPVM.AdvertisementId,
                    temporaryDeleteAdvertisementWithChildPVM.ChildsUsersIds))
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
        /// CompleteDeleteAdvertisement
        /// </summary>
        /// <param name="completeDeleteAdvertisementPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteAdvertisement")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteAdvertisement([FromBody] CompleteDeleteAdvertisementPVM completeDeleteAdvertisementPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (melkavanApiBusiness.CompleteDeleteAdvertisement(
                    completeDeleteAdvertisementPVM.AdvertisementId,
                    completeDeleteAdvertisementPVM.ChildsUsersIds,
                    completeDeleteAdvertisementPVM.UserId.Value,
                    completeDeleteAdvertisementPVM.Type, teniacoApiBusiness.TeniacoApiDb))
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
        /// GetListOfAdvertisementWithMoreDetail
        /// </summary>
        /// <param name="getListOfAdvertisementWithMoreDetailPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAdvertisementWithMoreDetail")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAdvertisementWithMoreDetail([FromBody] GetListOfAdvertisementWithMoreDetailPVM
             getListOfAdvertisementWithMoreDetailPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds == null)
                    {
                        getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds.Count == 0)
                        getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var listOfAdvertisement = melkavanApiBusiness.GetListOfAdvertisementWithMoreDetail(
                    getListOfAdvertisementWithMoreDetailPVM.jtStartIndex.Value,
                    getListOfAdvertisementWithMoreDetailPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfAdvertisementWithMoreDetailPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    teniacoApiBusiness.TeniacoApiDb,
                    getListOfAdvertisementWithMoreDetailPVM.HaveCallers,
                    getListOfAdvertisementWithMoreDetailPVM.HaveAddress,
                    getListOfAdvertisementWithMoreDetailPVM.HaveFeatures,
                    getListOfAdvertisementWithMoreDetailPVM.HaveFavorites,
                    getListOfAdvertisementWithMoreDetailPVM.HaveViewers,
                    getListOfAdvertisementWithMoreDetailPVM.HaveDetails,
                    getListOfAdvertisementWithMoreDetailPVM.HaveTags,
                    getListOfAdvertisementWithMoreDetailPVM.HaveFiles,
                    getListOfAdvertisementWithMoreDetailPVM.AdvertisementTypeId,
                    getListOfAdvertisementWithMoreDetailPVM.PropertyTypeId,
                    getListOfAdvertisementWithMoreDetailPVM.TypeOfUseId,
                    getListOfAdvertisementWithMoreDetailPVM.DocumentTypeId,
                    getListOfAdvertisementWithMoreDetailPVM.PropertyCodeName,
                    getListOfAdvertisementWithMoreDetailPVM.StateId,
                    getListOfAdvertisementWithMoreDetailPVM.CityId,
                    getListOfAdvertisementWithMoreDetailPVM.ZoneId,
                    getListOfAdvertisementWithMoreDetailPVM.DistrictId,
                    getListOfAdvertisementWithMoreDetailPVM.jtSorting,
                    getListOfAdvertisementWithMoreDetailPVM.UserId,
                   getListOfAdvertisementWithMoreDetailPVM.AdvertisementTitle);

                //foreach (var Advertisement in listOfAdvertisement)
                //{
                //    Advertisement.CountOfMaps = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("maps")).Count();
                //    Advertisement.CountOfDocs = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("docs")).Count();
                //    Advertisement.CountOfMedia = melkavanApiBusiness.MelkavanApiDb.AdvertisementFiles.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId) && f.AdvertisementFileType.Equals("media")).Count();
                //    Advertisement.CountOfPrices = melkavanApiBusiness.MelkavanApiDb.AdvertisementPricesHistories.Where(f => f.AdvertisementId.Equals(Advertisement.AdvertisementId)).Count();
                //}
                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisement;
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
        /// GetAdvertisementsWithOwnerId
        /// </summary>
        /// <param name="getAdvertisementsWithOwnerIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAdvertisementsWithOwnerId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertisementsWithOwnerId([FromBody] GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var listOfAdvertisement = melkavanApiBusiness.GetAdvertisementsWithOwnerId(
                    getAdvertisementsWithOwnerIdPVM.jtStartIndex.Value,
                    getAdvertisementsWithOwnerIdPVM.jtPageSize.Value,
                    ref listCount,
                    getAdvertisementsWithOwnerIdPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    teniacoApiBusiness.TeniacoApiDb,
                    getAdvertisementsWithOwnerIdPVM.jtSorting,
                    getAdvertisementsWithOwnerIdPVM.OwnerId
                    );
                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisement;
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
        /// GetWatchedAdvertisementsWithOwnerId
        /// </summary>
        /// <param name="getAdvertisementsWithOwnerIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetWatchedAdvertisementsWithOwnerId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetWatchedAdvertisementsWithOwnerId([FromBody] GetAdvertisementsWithOwnerIdPVM getAdvertisementsWithOwnerIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementsWithOwnerIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementsWithOwnerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var listOfAdvertisement = melkavanApiBusiness.GetWatchedAdvertisementsWithOwnerId(
                    getAdvertisementsWithOwnerIdPVM.jtStartIndex.Value,
                    getAdvertisementsWithOwnerIdPVM.jtPageSize.Value,
                    ref listCount,
                    getAdvertisementsWithOwnerIdPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    teniacoApiBusiness.TeniacoApiDb,
                    getAdvertisementsWithOwnerIdPVM.jtSorting,
                    getAdvertisementsWithOwnerIdPVM.OwnerId
                    );
                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisement;
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
        /// GetAdvertisementsViewersReportWithIdAndFilterType
        /// </summary>
        /// <param name="getAdvertisementsViewersReportWithIdAndFilterTypePVM"></param>
        /// <returns>jsonResultObjectVM</returns>
        [HttpPost("GetAdvertisementsViewersReportWithIdAndFilterType")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertisementsViewersReportWithIdAndFilterType([FromBody] GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterTypePVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
              new JsonResultWithRecordsObjectVM(new object() { });
            //GetAdvertisementsViewersReportWithIdAndFilterTypePVM getAdvertisementsViewersReportWithIdAndFilterType = new()
            //{
            //    AdvertisementId = getAdvertisementsViewersReportWithIdAndFilterTypePVM.AdvertisementId,
            //    FilterType = getAdvertisementsViewersReportWithIdAndFilterTypePVM.FilterType
            //};
            try
            {

                var result = melkavanApiBusiness.GetAdvertisementsViewersReportWithIdAndFilterType(
                      getAdvertisementsViewersReportWithIdAndFilterTypePVM,
                      teniacoApiBusiness.TeniacoApiDb);
                {
                    jsonResultWithRecordsObjectVM.Records = result;
                    jsonResultWithRecordsObjectVM.Result = "OK";
                    jsonResultWithRecordsObjectVM.Message = "Success";
                    jsonResultWithRecordsObjectVM.TotalRecordCount = result.Count;
                    return new JsonResult(jsonResultWithRecordsObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// AboutUs
        /// </summary>
        /// <param name="aboutUsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("AboutUs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AboutUs([FromBody] AboutUsPVM
             aboutUsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (aboutUsPVM.ChildsUsersIds == null)
                    {
                        aboutUsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (aboutUsPVM.ChildsUsersIds.Count == 0)
                        aboutUsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (aboutUsPVM.ChildsUsersIds.Count == 1)
                        if (aboutUsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            aboutUsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var aboutUs = melkavanApiBusiness.AboutUs(consoleBusiness,publicApiBusiness,teniacoApiBusiness);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = aboutUs;
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

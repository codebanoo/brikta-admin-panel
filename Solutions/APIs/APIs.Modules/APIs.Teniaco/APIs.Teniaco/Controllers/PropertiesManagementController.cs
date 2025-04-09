using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.Public.Models.Entities;
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VM.Base;
using VM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// PropertiesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PropertiesManagementController : ApiBaseController
    {
        /// <summary>
        /// PropertiesManagement
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
        public PropertiesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllPropertiesList
        /// </summary>
        /// <param name="getAllPropertiesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPropertiesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPropertiesList([FromBody] GetAllPropertiesListPVM getAllPropertiesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPropertiesListPVM.ChildsUsersIds == null)
                    {
                        getAllPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPropertiesListPVM.ChildsUsersIds.Count == 0)
                        getAllPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPropertiesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPropertiesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPropertiesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetAllPropertiesList(
                    ref listCount,
                    getAllPropertiesListPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getAllPropertiesListPVM.PropertyTypeId,
                    getAllPropertiesListPVM.TypeOfUseId,
                    getAllPropertiesListPVM.DocumentTypeId,
                    getAllPropertiesListPVM.ConsultantUserId,
                    getAllPropertiesListPVM.OwnerId,
                    //getAllPropertiesListPVM.DocumentOwnershipTypeId,
                    //getAllPropertiesListPVM.DocumentRootTypeId,
                    getAllPropertiesListPVM.PropertyCodeName,
                    getAllPropertiesListPVM.StateId,
                    getAllPropertiesListPVM.CityId,
                    getAllPropertiesListPVM.ZoneId,
                    getAllPropertiesListPVM.DistrictId,
                    getAllPropertiesListPVM.GetFiles,
                    getAllPropertiesListPVM.GetAddress,
                    getAllPropertiesListPVM.GetPrices,
                    getAllPropertiesListPVM.GetPrice,
                    getAllPropertiesListPVM.GetFeatures,
                    //getAllPropertiesListPVM.Intermediary,
                    //getAllPropertiesListPVM.IsPrivate,
                    getAllPropertiesListPVM.jtSorting);

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
        /// GetListOfProperties
        /// </summary>
        /// <param name="getListOfPropertiesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfProperties([FromBody] GetListOfPropertiesPVM
            getListOfPropertiesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertiesPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertiesPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertiesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetListOfProperties(getListOfPropertiesPVM.jtStartIndex.Value,
                    getListOfPropertiesPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfPropertiesPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getListOfPropertiesPVM.PropertyTypeId,
                    getListOfPropertiesPVM.TypeOfUseId,
                    getListOfPropertiesPVM.DocumentTypeId,
                    getListOfPropertiesPVM.ConsultantUserId,
                    getListOfPropertiesPVM.OwnerId,
                    //getListOfPropertiesPVM.DocumentOwnershipTypeId,
                    //getListOfPropertiesPVM.DocumentRootTypeId,
                    getListOfPropertiesPVM.PropertyCodeName,
                    getListOfPropertiesPVM.StateId,
                    getListOfPropertiesPVM.CityId,
                    getListOfPropertiesPVM.ZoneId,
                    getListOfPropertiesPVM.DistrictId,
                    //getListOfPropertiesPVM.Intermediary,
                    //getListOfPropertiesPVM.IsPrivate,
                    getListOfPropertiesPVM.jtSorting);




                var propertyIds = listOfProperties.Select(p => p.PropertyId).ToList();

                var PropertyOwners = teniacoApiBusiness.TeniacoApiDb.PropertyOwners.Where(p => propertyIds.Contains(p.PropertyId)).ToList();

                var personsIds = PropertyOwners.Select(c => c.OwnerId).Distinct().ToList();

                var persons = publicApiBusiness.PublicApiDb.Persons.Where(p => personsIds.Contains(p.PersonId)).ToList();


                var propertyDetails = teniacoApiBusiness.TeniacoApiDb.PropertiesDetails.Where(d => propertyIds.Contains(d.PropertyId)).ToList();

                var buildingLifeIds = propertyDetails.Select(b => b.BuildingLifeId).ToList();

                var buildingLifes = melkavanApiBusiness.MelkavanApiDb.BuildingLifes.Where(b => buildingLifeIds.Contains(b.BuildingLifeId)).ToList();

                foreach (var property in listOfProperties)
                {
                    //property.CountOfMaps = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("maps")).Count();
                    //property.CountOfDocs = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("docs")).Count();
                    //property.CountOfMedia = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("media")).Count();
                    //property.CountOfPrices = teniacoApiBusiness.TeniacoApiDb.PropertiesPricesHistories.Where(f => f.PropertyId.Equals(property.PropertyId)).Count();



                    if (PropertyOwners.Where(p => p.PropertyId.Equals(property.PropertyId)).Any())
                    {
                        property.PropertyOwnersVM = new List<PropertyOwnersVM>();


                        var propertyOwner = PropertyOwners.Where(p => p.PropertyId.Equals(property.PropertyId)).ToList();

                        property.PropertyOwnersVM = propertyOwner.Select(c => new PropertyOwnersVM
                        {
                            PropertyOwnerId = c.PropertyOwnerId,
                            OwnerId = c.OwnerId,
                            Share = c.Share,
                            SharePercent = c.SharePercent,
                            PropertyId = c.PropertyId,
                            OwnerPersonFamily = persons.Where(p => p.PersonId.Equals(c.OwnerId)).Any() ?
                            persons.Where(p => p.PersonId.Equals(c.OwnerId)).FirstOrDefault().Family : ""
                        }).ToList();
                    }


                    if (propertyDetails.Where(d => d.PropertyId.Equals(property.PropertyId)).Any())
                    {

                        var propertyDetail = propertyDetails.Where(d => d.PropertyId.Equals(property.PropertyId)).FirstOrDefault();

                        property.PropertiesDetailsVM = new PropertiesDetailsVM();
                        property.PropertiesDetailsVM.BuildingLifeId = propertyDetail.BuildingLifeId;
                        property.PropertiesDetailsVM.Foundation = propertyDetail.Foundation;
                        property.PropertiesDetailsVM.BuildingLifeTitle = buildingLifes.Where(b => b.BuildingLifeId.Equals(propertyDetail.BuildingLifeId)).Any() ?
                            buildingLifes.Where(b => b.BuildingLifeId.Equals(propertyDetail.BuildingLifeId)).FirstOrDefault().BuildingLifeTitle : "";
                    }


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
        /// GetListOfPropertiesAdvanceSearch
        /// </summary>
        /// <param name="getListOfPropertiesAdvanceSearchPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfPropertiesAdvanceSearch")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPropertiesAdvanceSearch([FromBody] GetListOfPropertiesAdvanceSearchPVM
            getListOfPropertiesAdvanceSearchPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds == null)
                    {
                        getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds.Count == 0)
                        getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetListOfPropertiesAdvanceSearch(getListOfPropertiesAdvanceSearchPVM.jtStartIndex.Value,
                    getListOfPropertiesAdvanceSearchPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    melkavanApiBusiness.MelkavanApiDb,
                    consoleBusiness,
                    getListOfPropertiesAdvanceSearchPVM.Platform,
                    getListOfPropertiesAdvanceSearchPVM.PropertyTypeId,
                    getListOfPropertiesAdvanceSearchPVM.Price,
                    getListOfPropertiesAdvanceSearchPVM.PriceFrom,
                    getListOfPropertiesAdvanceSearchPVM.PriceTo,
                    getListOfPropertiesAdvanceSearchPVM.Area,
                    getListOfPropertiesAdvanceSearchPVM.AreaFrom,
                    getListOfPropertiesAdvanceSearchPVM.AreaTo,
                    getListOfPropertiesAdvanceSearchPVM.Address,
                    getListOfPropertiesAdvanceSearchPVM.FeaturesAndDesc,
                    getListOfPropertiesAdvanceSearchPVM.TypeOfUseId,
                    getListOfPropertiesAdvanceSearchPVM.DocumentTypeId,
                    getListOfPropertiesAdvanceSearchPVM.DocumentRootTypeId,
                    getListOfPropertiesAdvanceSearchPVM.DocumentOwnershipTypeId,
                    getListOfPropertiesAdvanceSearchPVM.PropertyCodeName,
                    getListOfPropertiesAdvanceSearchPVM.ConsultantUserId,
                    getListOfPropertiesAdvanceSearchPVM.OwnerId,
                    getListOfPropertiesAdvanceSearchPVM.InvestorId,
                    getListOfPropertiesAdvanceSearchPVM.AdvertiserId,
                    getListOfPropertiesAdvanceSearchPVM.Features,
                    getListOfPropertiesAdvanceSearchPVM.StateId,
                    getListOfPropertiesAdvanceSearchPVM.CityId,
                    getListOfPropertiesAdvanceSearchPVM.ZoneId,
                    getListOfPropertiesAdvanceSearchPVM.DistrictId,
                    getListOfPropertiesAdvanceSearchPVM.ThisUserId,
                    getListOfPropertiesAdvanceSearchPVM.Participable,
                    getListOfPropertiesAdvanceSearchPVM.Exchangeable,
                    getListOfPropertiesAdvanceSearchPVM.jtSorting);


                #region comments
                //var propertyIds = listOfProperties.Select(p => p.PropertyId).ToList();

                //var PropertyOwners = teniacoApiBusiness.TeniacoApiDb.PropertyOwners.Where(p => propertyIds.Contains(p.PropertyId)).ToList();

                //var personsIds = PropertyOwners.Select(c => c.OwnerId).Distinct().ToList();

                //var persons = publicApiBusiness.PublicApiDb.Persons.Where(p => personsIds.Contains(p.PersonId)).ToList();


                //var propertyDetails = teniacoApiBusiness.TeniacoApiDb.PropertiesDetails.Where(d => propertyIds.Contains(d.PropertyId)).ToList();

                //var buildingLifeIds = propertyDetails.Select(b => b.BuildingLifeId).ToList();

                //var buildingLifes = melkavanApiBusiness.MelkavanApiDb.BuildingLifes.Where(b => buildingLifeIds.Contains(b.BuildingLifeId)).ToList();

                //foreach (var property in listOfProperties)
                //{
                //    //property.CountOfMaps = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("maps")).Count();
                //    //property.CountOfDocs = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("docs")).Count();
                //    //property.CountOfMedia = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(f => f.PropertyId.Equals(property.PropertyId) && f.PropertyFileType.Equals("media")).Count();
                //    //property.CountOfPrices = teniacoApiBusiness.TeniacoApiDb.PropertiesPricesHistories.Where(f => f.PropertyId.Equals(property.PropertyId)).Count();



                //    if (PropertyOwners.Where(p => p.PropertyId.Equals(property.PropertyId)).Any())
                //    {
                //        property.PropertyOwnersVM = new List<PropertyOwnersVM>();


                //        var propertyOwner = PropertyOwners.Where(p => p.PropertyId.Equals(property.PropertyId)).ToList();

                //        property.PropertyOwnersVM = propertyOwner.Select(c => new PropertyOwnersVM
                //        {
                //            PropertyOwnerId = c.PropertyOwnerId,
                //            OwnerId = c.OwnerId,
                //            Share = c.Share,
                //            SharePercent = c.SharePercent,
                //            PropertyId = c.PropertyId,
                //            OwnerPersonFamiy = persons.Where(p => p.PersonId.Equals(c.OwnerId)).Any() ?
                //            persons.Where(p => p.PersonId.Equals(c.OwnerId)).FirstOrDefault().Family : ""
                //        }).ToList();
                //    }


                //    if (propertyDetails.Where(d => d.PropertyId.Equals(property.PropertyId)).Any())
                //    {

                //        var propertyDetail = propertyDetails.Where(d => d.PropertyId.Equals(property.PropertyId)).FirstOrDefault();

                //        property.PropertiesDetailsVM = new PropertiesDetailsVM();
                //        property.PropertiesDetailsVM.BuildingLifeId = propertyDetail.BuildingLifeId;
                //        property.PropertiesDetailsVM.Foundation = propertyDetail.Foundation;
                //        property.PropertiesDetailsVM.BuildingLifeTitle = buildingLifes.Where(b => b.BuildingLifeId.Equals(propertyDetail.BuildingLifeId)).Any() ?
                //            buildingLifes.Where(b => b.BuildingLifeId.Equals(propertyDetail.BuildingLifeId)).FirstOrDefault().BuildingLifeTitle : "";
                //    }


                //}

                #endregion


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
        /// AddToProperties
        /// </summary>
        /// <param name="addToPropertiesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToProperties([FromBody] AddToPropertiesPVM
            addToPropertiesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToPropertiesPVM.ChildsUsersIds == null)
                    {
                        addToPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToPropertiesPVM.ChildsUsersIds.Count == 0)
                        addToPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToPropertiesPVM.ChildsUsersIds.Count == 1)
                        if (addToPropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToPropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    addToPropertiesPVM.PropertiesVM.CreateEnDate = DateTime.Now;
                    addToPropertiesPVM.PropertiesVM.CreateTime = PersianDate.TimeNow;
                    addToPropertiesPVM.PropertiesVM.UserIdCreator = this.userId.Value;

                    //addToPropertiesPVM.PropertiesVM.PropertyAddressVM.CreateEnDate = DateTime.Now;
                    //addToPropertiesPVM.PropertiesVM.PropertyAddressVM.CreateTime = PersianDate.TimeNow;
                    //addToPropertiesPVM.PropertiesVM.PropertyAddressVM.UserIdCreator = this.userId.Value;
                    //addToPropertiesPVM.PropertiesVM.PropertyAddressVM.IsActivated = true;
                    //addToPropertiesPVM.PropertiesVM.PropertyAddressVM.IsDeleted = false;

                }

                long propertyId = teniacoApiBusiness.AddToProperties(
                    addToPropertiesPVM.PropertiesVM,
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
                    addToPropertiesPVM.PropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToPropertiesPVM.PropertiesVM;

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
        /// GetPropertyWithPropertyId
        /// </summary>
        /// <param name="getPropertyWithPropertyIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetPropertyWithPropertyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPropertyWithPropertyId([FromBody] GetPropertyWithPropertyIdPVM
            getPropertyWithPropertyIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPropertyWithPropertyIdPVM.ChildsUsersIds == null)
                    {
                        getPropertyWithPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getPropertyWithPropertyIdPVM.ChildsUsersIds.Count == 0)
                        getPropertyWithPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getPropertyWithPropertyIdPVM.ChildsUsersIds.Count == 1)
                        if (getPropertyWithPropertyIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPropertyWithPropertyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                    var property = teniacoApiBusiness.GetPropertyWithPropertyId(
                    getPropertyWithPropertyIdPVM.PropertyId,
                    getPropertyWithPropertyIdPVM.ChildsUsersIds,
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
        /// UpdateProperties
        /// </summary>
        /// <param name="updatePropertiesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateProperties([FromBody] UpdatePropertiesPVM
            updatePropertiesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updatePropertiesPVM.ChildsUsersIds == null)
                    {
                        updatePropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updatePropertiesPVM.ChildsUsersIds.Count == 0)
                        updatePropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updatePropertiesPVM.ChildsUsersIds.Count == 1)
                        if (updatePropertiesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updatePropertiesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updatePropertiesPVM.PropertiesVM.EditEnDate = DateTime.Now;
                    updatePropertiesPVM.PropertiesVM.EditTime = PersianDate.TimeNow;
                    updatePropertiesPVM.PropertiesVM.UserIdEditor = this.userId.Value;
                }

                var propertiesVM = updatePropertiesPVM.PropertiesVM;

                long propertyId = teniacoApiBusiness.UpdateProperties(
                    ref propertiesVM,
                    updatePropertiesPVM.ChildsUsersIds);

                if (propertyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";
                }
                else
                if (propertyId > 0)
                {
                    updatePropertiesPVM.PropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatePropertiesPVM.PropertiesVM;
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
        /// ToggleActivationProperties
        /// </summary>
        /// <param name="toggleActivationPropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("ToggleActivationProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationProperties([FromBody] ToggleActivationPropertiesPVM
            toggleActivationPropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationProperties(
                    toggleActivationPropertiesPVM.PropertyId,
                    toggleActivationPropertiesPVM.UserId.Value,
                    toggleActivationPropertiesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteProperties
        /// </summary>
        /// <param name="temporaryDeletePropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("TemporaryDeleteProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteProperties([FromBody] TemporaryDeletePropertiesPVM
            temporaryDeletePropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteProperties(
                    temporaryDeletePropertiesPVM.PropertyId,
                    temporaryDeletePropertiesPVM.UserId.Value,
                    temporaryDeletePropertiesPVM.ChildsUsersIds))
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


        [HttpPost("TemporaryDeletePropertiesWithChild")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePropertiesWithChild([FromBody] TemporaryDeletePropertiesWithChildPVM temporaryDeletePropertiesWithChildPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.TemporaryDeletePropertiesWithChild(
                    temporaryDeletePropertiesWithChildPVM.PropertyId,
                    temporaryDeletePropertiesWithChildPVM.ChildsUsersIds))
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
        /// CompleteDeleteProperties
        /// </summary>
        /// <param name="completeDeletePropertiesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteProperties([FromBody] CompleteDeletePropertiesPVM completeDeletePropertiesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteProperties(
                    completeDeletePropertiesPVM.PropertyId,
                    completeDeletePropertiesPVM.ChildsUsersIds))
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
        /// GetAllPropertiesListWithoutAddress
        /// </summary>
        /// <param name="getAllPropertiesListWithoutAddressPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPropertiesListWithoutAddress")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPropertiesListWithoutAddress([FromBody] GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPropertiesListWithoutAddressPVM.ChildsUsersIds == null)
                    {
                        getAllPropertiesListWithoutAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllPropertiesListWithoutAddressPVM.ChildsUsersIds.Count == 0)
                        getAllPropertiesListWithoutAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllPropertiesListWithoutAddressPVM.ChildsUsersIds.Count == 1)
                        if (getAllPropertiesListWithoutAddressPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPropertiesListWithoutAddressPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetAllPropertiesListWithoutAddress(
                    getAllPropertiesListWithoutAddressPVM.ChildsUsersIds);

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
        /// ToggleActivationShowInMelkavan
        /// </summary>
        /// <param name="toggleActivationShowInMelkavanPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        /// 

        [HttpPost("ToggleActivationShowInMelkavan")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationShowInMelkavan([FromBody] ToggleActivationShowInMelkavanPVM
            toggleActivationShowInMelkavanPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";
                if (teniacoApiBusiness.ToggleActivationShowInMelkavan(
                    toggleActivationShowInMelkavanPVM.PropertyId,
                    toggleActivationShowInMelkavanPVM.UserId.Value))
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
        /// AddPropertiesInMelkavan
        /// </summary>
        /// <param name="addPropertiesInMelkavanPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddPropertiesInMelkavan")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddPropertiesInMelkavan([FromBody] AddPropertiesInMelkavanPVM
            addPropertiesInMelkavanPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addPropertiesInMelkavanPVM.ChildsUsersIds == null)
                    {
                        addPropertiesInMelkavanPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addPropertiesInMelkavanPVM.ChildsUsersIds.Count == 0)
                        addPropertiesInMelkavanPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addPropertiesInMelkavanPVM.ChildsUsersIds.Count == 1)
                        if (addPropertiesInMelkavanPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addPropertiesInMelkavanPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var propertiesVM = addPropertiesInMelkavanPVM.PropertiesInMelkavanVM;

                long propertyId = teniacoApiBusiness.AddPropertiesInMelkavan(
                    ref propertiesVM,
                    addPropertiesInMelkavanPVM.ChildsUsersIds,
                    consoleBusiness);

                if (propertyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";
                }
                else
                if (propertyId > 0)
                {
                    addPropertiesInMelkavanPVM.PropertiesInMelkavanVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addPropertiesInMelkavanPVM.PropertiesInMelkavanVM;
                }

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

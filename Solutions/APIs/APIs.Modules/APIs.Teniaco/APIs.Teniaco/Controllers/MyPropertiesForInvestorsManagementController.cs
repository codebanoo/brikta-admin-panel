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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MyPropertiesForInvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// MyPropertiesForInvestorsManagement
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
        public MyPropertiesForInvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMyPropertiesForInvestorsList
        /// </summary>
        /// <param name="getAllMyPropertiesForInvestorsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMyPropertiesForInvestorsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMyPropertiesForInvestorsList([FromBody] GetAllMyPropertiesForInvestorsListPVM getAllMyPropertiesForInvestorsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds == null)
                    {
                        getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds.Count == 0)
                        getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetAllMyPropertiesForInvestorsList(
                    ref listCount,
                    getAllMyPropertiesForInvestorsListPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getAllMyPropertiesForInvestorsListPVM.PropertyTypeId,
                    getAllMyPropertiesForInvestorsListPVM.TypeOfUseId,
                    getAllMyPropertiesForInvestorsListPVM.DocumentTypeId,
                    getAllMyPropertiesForInvestorsListPVM.ConsultantUserId,
                    getAllMyPropertiesForInvestorsListPVM.OwnerId,
                    getAllMyPropertiesForInvestorsListPVM.PropertyCodeName,
                    getAllMyPropertiesForInvestorsListPVM.StateId,
                    getAllMyPropertiesForInvestorsListPVM.CityId,
                    getAllMyPropertiesForInvestorsListPVM.ZoneId,
                    getAllMyPropertiesForInvestorsListPVM.DistrictId);

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
        /// GetListOfMyPropertiesForInvestors
        /// </summary>
        /// <param name="getListOfMyPropertiesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMyPropertiesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMyPropertiesForInvestors([FromBody] GetListOfMyPropertiesForInvestorsPVM
            getListOfMyPropertiesForInvestorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfProperties = teniacoApiBusiness.GetListOfMyPropertiesForInvestors(
                    getListOfMyPropertiesForInvestorsPVM.jtStartIndex.Value,
                    getListOfMyPropertiesForInvestorsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfMyPropertiesForInvestorsPVM.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    getListOfMyPropertiesForInvestorsPVM.pageNum,
                    getListOfMyPropertiesForInvestorsPVM.pageSize,
                    getListOfMyPropertiesForInvestorsPVM.PropertyTypeId,
                    getListOfMyPropertiesForInvestorsPVM.TypeOfUseId,
                    getListOfMyPropertiesForInvestorsPVM.DocumentTypeId,
                    getListOfMyPropertiesForInvestorsPVM.ConsultantUserId,
                    getListOfMyPropertiesForInvestorsPVM.OwnerId,
                    getListOfMyPropertiesForInvestorsPVM.PropertyCodeName,
                    getListOfMyPropertiesForInvestorsPVM.StateId,
                    getListOfMyPropertiesForInvestorsPVM.CityId,
                    getListOfMyPropertiesForInvestorsPVM.ZoneId,
                    getListOfMyPropertiesForInvestorsPVM.DistrictId,
                    getListOfMyPropertiesForInvestorsPVM.UserId); // Passing the id of logged In User


                var propertyIds = listOfProperties.Select(c => c.PropertyId).Distinct().ToList();
                var propertyFiles = teniacoApiBusiness.TeniacoApiDb.PropertyFiles.Where(pf => propertyIds.Contains(pf.PropertyId) && pf.PropertyFileType == "media").ToList();

                foreach (var property in listOfProperties)
                {
                    if (propertyFiles.Where(c => c.PropertyId.Equals(property.PropertyId)).Any())
                    {
                        var propertyFile = propertyFiles.Where(c => c.PropertyId.Equals(property.PropertyId)).ToList();

                        property.Photos = propertyFile.Select(file => new PropertyFilesVM
                        {

                            PropertyFileExt = file.PropertyFileExt,
                            PropertyFileId = file.PropertyFileId,
                            PropertyFileTitle = file.PropertyFileTitle,
                            PropertyFileOrder = file.PropertyFileOrder,
                            PropertyFileType = file.PropertyFileType,
                            PropertyId = property.PropertyId,
                            PropertyFilePath = file.PropertyFilePath,
                            CreateEnDate = file.CreateEnDate.Value,
                            UserIdCreator = file.UserIdCreator,
                        }).ToList();


                    }
                    else
                    {
                        property.Photos = new List<PropertyFilesVM>();

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
        /// AddToMyPropertiesForInvestors
        /// </summary>
        /// <param name="addToMyPropertiesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToMyPropertiesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMyPropertiesForInvestors([FromBody] AddToMyPropertiesForInvestorsPVM
            addToMyPropertiesForInvestorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMyPropertiesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        addToMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        addToMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (addToMyPropertiesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    addToMyPropertiesForInvestorsPVM.PropertiesVM.CreateEnDate = DateTime.Now;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.CreateTime = PersianDate.TimeNow;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.UserIdCreator = this.userId.Value;

                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyAddressVM.CreateEnDate = DateTime.Now;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyAddressVM.CreateTime = PersianDate.TimeNow;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyAddressVM.UserIdCreator = this.userId.Value;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyAddressVM.IsActivated = true;
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyAddressVM.IsDeleted = false;
                }

                long propertyId = teniacoApiBusiness.AddToMyPropertiesForInvestors(
                    addToMyPropertiesForInvestorsPVM.PropertiesVM,
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
                    addToMyPropertiesForInvestorsPVM.PropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToMyPropertiesForInvestorsPVM.PropertiesVM;

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
        /// UpdateMyPropertiesForInvestors
        /// </summary>
        /// <param name="updateMyPropertiesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("UpdateMyPropertiesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMyPropertiesForInvestors([FromBody] UpdateMyPropertiesForInvestorsPVM
            updateMyPropertiesForInvestorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMyPropertiesForInvestorsPVM.ChildsUsersIds == null)
                    {
                        updateMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 0)
                        updateMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateMyPropertiesForInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (updateMyPropertiesForInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMyPropertiesForInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateMyPropertiesForInvestorsPVM.PropertiesVM.EditEnDate = DateTime.Now;
                    updateMyPropertiesForInvestorsPVM.PropertiesVM.EditTime = PersianDate.TimeNow;
                    updateMyPropertiesForInvestorsPVM.PropertiesVM.UserIdEditor = this.userId.Value;
                }

                var propertiesVM = updateMyPropertiesForInvestorsPVM.PropertiesVM;

                long propertyId = teniacoApiBusiness.UpdateMyPropertiesForInvestors(
                    ref propertiesVM,
                    updateMyPropertiesForInvestorsPVM.ChildsUsersIds,
                    updateMyPropertiesForInvestorsPVM.UserId);

                if (propertyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";
                }
                else
                if (propertyId > 0)
                {
                    updateMyPropertiesForInvestorsPVM.PropertiesVM.PropertyId = propertyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateMyPropertiesForInvestorsPVM.PropertiesVM;
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
        /// CompleteDeleteMyPropertiesForInvestors
        /// </summary>
        /// <param name="completeDeleteMyPropertiesForInvestorsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteMyPropertiesForInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMyPropertiesForInvestors([FromBody] CompleteDeleteMyPropertiesForInvestorsPVM completeDeleteMyPropertiesForInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {

                if (teniacoApiBusiness.CompleteDeleteMyPropertiesForInvestors(
                   completeDeleteMyPropertiesForInvestorsPVM.PropertyId,
                   completeDeleteMyPropertiesForInvestorsPVM.ChildsUsersIds))
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

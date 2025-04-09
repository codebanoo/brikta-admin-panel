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
using System.Net;
using System;
using VM.Base;
using VM.PVM.Teniaco;
using FrameWork;
using System.Linq;
using System.Collections.Generic;
using VM.Teniaco;
using VM.PVM.Melkavan;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// MyPropertiesForInvestorsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MelkavanPropertiesManagementController : ApiBaseController
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
        public MelkavanPropertiesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfMelkavanProperties
        /// </summary>
        /// <param name="getListOfMyPropertiesForInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMelkavanProperties")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMelkavanProperties([FromBody] GetListOfMyPropertiesForInvestorsPVM
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

                var listOfProperties = teniacoApiBusiness.GetListOfMelkavanProperties(
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
        /// GetListOfNearAdvertisementsWithPropertyId
        /// </summary>
        /// <param name="getListOfNearAdvertisementsWithPropertyId"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfNearAdvertisementsWithPropertyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfNearAdvertisementsWithPropertyId([FromBody] GetListOfNearAdvertisementsWithPropertyIdPVM
             getListOfNearAdvertisementsWithPropertyId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds == null)
                    {
                        getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds.Count == 0)
                        getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds.Count == 1)
                        if (getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var listOfAdvertisement = teniacoApiBusiness.GetListOfNearAdvertisementsWithPropertyId(
                    getListOfNearAdvertisementsWithPropertyId.jtStartIndex.Value,
                    getListOfNearAdvertisementsWithPropertyId.jtPageSize.Value,
                    ref listCount,
                    getListOfNearAdvertisementsWithPropertyId.ChildsUsersIds,
                    publicApiBusiness.PublicApiDb,
                    teniacoApiBusiness.TeniacoApiDb,
                    melkavanApiBusiness.MelkavanApiDb,
                    getListOfNearAdvertisementsWithPropertyId.HaveCallers,
                    getListOfNearAdvertisementsWithPropertyId.HaveAddress,
                    getListOfNearAdvertisementsWithPropertyId.HaveFeatures,
                    getListOfNearAdvertisementsWithPropertyId.HaveViewers,
                    getListOfNearAdvertisementsWithPropertyId.HaveDetails,
                    getListOfNearAdvertisementsWithPropertyId.HaveTags,
                    getListOfNearAdvertisementsWithPropertyId.HaveFiles,
                    getListOfNearAdvertisementsWithPropertyId.PropertyId,
                    getListOfNearAdvertisementsWithPropertyId.RecordType,
                    getListOfNearAdvertisementsWithPropertyId.AdvertisementTypeId,
                    getListOfNearAdvertisementsWithPropertyId.PropertyTypeId,
                    getListOfNearAdvertisementsWithPropertyId.TypeOfUseId,
                    getListOfNearAdvertisementsWithPropertyId.DocumentTypeId,
                    getListOfNearAdvertisementsWithPropertyId.PropertyCodeName,
                    getListOfNearAdvertisementsWithPropertyId.StateId,
                    getListOfNearAdvertisementsWithPropertyId.CityId,
                    getListOfNearAdvertisementsWithPropertyId.ZoneId,
                    getListOfNearAdvertisementsWithPropertyId.DistrictId,
                    getListOfNearAdvertisementsWithPropertyId.jtSorting,
                    getListOfNearAdvertisementsWithPropertyId.UserId,
                   getListOfNearAdvertisementsWithPropertyId.AdvertisementTitle);


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


    }
}

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
using System.Linq;
using VM.PVM.Melkavan;
using VM.Melkavan.PVM.Melkavan.Advertisement;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementStatusManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementStatusManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementStatusManagement
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
        public AdvertisementStatusManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfPropertiesAdvanceSearch
        /// </summary>
        /// <param name="getListOfPropertiesAdvanceSearchPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAdvertisementsForStatus")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAdvertisementsForStatus([FromBody] GetListOfPropertiesAdvanceSearchPVM
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

                var listOfAdvertisements = melkavanApiBusiness.GetListOfAdvertisementsForStatus(getListOfPropertiesAdvanceSearchPVM.jtStartIndex.Value,
                    getListOfPropertiesAdvanceSearchPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfPropertiesAdvanceSearchPVM.ChildsUsersIds,
                    melkavanApiBusiness.MelkavanApiDb,
                    teniacoApiBusiness.TeniacoApiDb,
                    consoleBusiness,
                    getListOfPropertiesAdvanceSearchPVM.jtSorting,
                    getListOfPropertiesAdvanceSearchPVM.ThisUserId);


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
        /// UpdateAdvertisementStatusId
        /// </summary>
        /// <param name="updateAdvertisementStatusPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateAdvertisementStatusId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisementStatusId([FromBody] UpdateAdvertisementStatusPVM
            updateAdvertisementStatusPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAdvertisementStatusPVM.ChildsUsersIds == null)
                    {
                        updateAdvertisementStatusPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateAdvertisementStatusPVM.ChildsUsersIds.Count == 0)
                        updateAdvertisementStatusPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateAdvertisementStatusPVM.ChildsUsersIds.Count == 1)
                        if (updateAdvertisementStatusPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAdvertisementStatusPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }


                var result = melkavanApiBusiness.UpdateAdvertisementStatusId(updateAdvertisementStatusPVM.AdvertisementId,
                    updateAdvertisementStatusPVM.Type,
                    updateAdvertisementStatusPVM.StatusId,
                    updateAdvertisementStatusPVM.RejectionReason,
                    updateAdvertisementStatusPVM.ChildsUsersIds,
                    teniacoApiBusiness.TeniacoApiDb);

                if (result > 0)
                {
                    jsonResultWithRecordsObjectVM.Result = "OK";
                }
                else
                {
                    jsonResultWithRecordsObjectVM.Result = "ERROR";
                }

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// UpdateAdvertisementTagId
        /// </summary>
        /// <param name="updateAdvertisementTagIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateAdvertisementTagId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAdvertisementTagId([FromBody] UpdateAdvertisementTagIdPVM
            updateAdvertisementTagIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAdvertisementTagIdPVM.ChildsUsersIds == null)
                    {
                        updateAdvertisementTagIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateAdvertisementTagIdPVM.ChildsUsersIds.Count == 0)
                        updateAdvertisementTagIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateAdvertisementTagIdPVM.ChildsUsersIds.Count == 1)
                        if (updateAdvertisementTagIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAdvertisementTagIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }


                var result = melkavanApiBusiness.UpdateAdvertisementTagId(updateAdvertisementTagIdPVM.AdvertisementId,
                    updateAdvertisementTagIdPVM.Type,
                    updateAdvertisementTagIdPVM.TagId,
                    updateAdvertisementTagIdPVM.ChildsUsersIds,
                    teniacoApiBusiness.TeniacoApiDb);

                if (result > 0)
                {
                    jsonResultWithRecordsObjectVM.Result = "OK";
                }
                else
                {
                    jsonResultWithRecordsObjectVM.Result = "ERROR";
                }

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

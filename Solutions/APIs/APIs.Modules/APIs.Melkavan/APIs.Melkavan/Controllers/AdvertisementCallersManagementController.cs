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
using VM.Melkavan;
using VM.PVM.Melkavan;
using VM.Teniaco;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementCallersManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementCallersManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementCallersManagement
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
        public AdvertisementCallersManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAdvertisementCallersWithAdvertisementId
        /// </summary>
        /// <param name="getAdvertisementCallersWithAdvertisementIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAdvertisementCallersWithAdvertisementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAdvertisementCallers(GetAdvertisementCallersWithAdvertisementIdPVM getAdvertisementCallersWithAdvertisementIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementCallersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfAdvertisementCallers = melkavanApiBusiness.GetAdvertisementCallersWithAdvertisementId(
                    getAdvertisementCallersWithAdvertisementIdPVM.AdvertisementId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisementCallers;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfAdvertisementCallers.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// AddToAdvertisementCallers
        /// </summary>
        /// <param name="addToAdvertisementCallersPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToAdvertisementCallers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAdvertisementCallers([FromBody] AddToAdvertisementCallersPVM addToAdvertisementCallersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAdvertisementCallersPVM.ChildsUsersIds == null)
                    {
                        addToAdvertisementCallersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToAdvertisementCallersPVM.ChildsUsersIds.Count == 0)
                        {
                            addToAdvertisementCallersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToAdvertisementCallersPVM.ChildsUsersIds.Count == 1)
                                if (addToAdvertisementCallersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToAdvertisementCallersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }

                    addToAdvertisementCallersPVM.AdvertisementCallersVM.CreateEnDate = DateTime.Now;
                    addToAdvertisementCallersPVM.AdvertisementCallersVM.CreateTime = PersianDate.TimeNow;
                    addToAdvertisementCallersPVM.AdvertisementCallersVM.UserIdCreator = this.userId.Value;
                    addToAdvertisementCallersPVM.AdvertisementCallersVM.IsActivated = true;
                    addToAdvertisementCallersPVM.AdvertisementCallersVM.IsDeleted = false;
                }

                int advertisementCallersId = melkavanApiBusiness.AddToAdvertisementCallers(
                  addToAdvertisementCallersPVM.AdvertisementCallersVM);


                if (advertisementCallersId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAdvertisementCallers";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (advertisementCallersId > 0)
                {
                    addToAdvertisementCallersPVM.AdvertisementCallersVM.AdvertisementCallersId = advertisementCallersId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAdvertisementCallersPVM.AdvertisementCallersVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }

            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

    }
}


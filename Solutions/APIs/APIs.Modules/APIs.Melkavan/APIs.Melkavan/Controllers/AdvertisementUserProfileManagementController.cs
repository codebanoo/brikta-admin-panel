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
using VM.PVM.Melkavan;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementUserProfileManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementUserProfileManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementUserProfileManagement
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
        public AdvertisementUserProfileManagementController(IHostEnvironment _hostingEnvironment,
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


        [HttpPost("UpdateUserProfile")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateUserProfile(UpdateUserProfilePVM updateUserProfilePVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateUserProfilePVM.ChildsUsersIds == null)
                    {
                        updateUserProfilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateUserProfilePVM.ChildsUsersIds.Count == 0)
                        updateUserProfilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateUserProfilePVM.ChildsUsersIds.Count == 1)
                        if (updateUserProfilePVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateUserProfilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }



                if (melkavanApiBusiness.UpdateUserProfile(
                    updateUserProfilePVM.UserId,
                    updateUserProfilePVM.StateId,
                    updateUserProfilePVM.CityId,
                    updateUserProfilePVM.Name,
                    updateUserProfilePVM.Family,
                    updateUserProfilePVM.Email,
                    updateUserProfilePVM.PersonTypeId,
                    updateUserProfilePVM.ChildsUsersIds,
                   consoleBusiness,
                   publicApiBusiness))
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


        [HttpPost("GetAdvertiserProfileWithAdvertiserId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAdvertiserProfileWithAdvertiserId(GetAdvertiserProfileWithAdvertiserIdPVM getAdvertiserProfileWithAdvertiserIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }




                var userProfile = melkavanApiBusiness.GetAdvertiserProfileWithAdvertiserId(
                     getAdvertiserProfileWithAdvertiserIdPVM.AdvertiserId,
                     getAdvertiserProfileWithAdvertiserIdPVM.ChildsUsersIds,
                     consoleBusiness,
                     publicApiBusiness.PublicApiDb,
                     melkavanApiBusiness.MelkavanApiDb);


                if (userProfile!=null)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Message = "Success";
                    jsonResultWithRecordObjectVM.Record = userProfile;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

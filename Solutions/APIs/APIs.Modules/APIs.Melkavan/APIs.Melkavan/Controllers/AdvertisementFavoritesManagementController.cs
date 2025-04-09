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
    /// AdvertisementFavoritesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementFavoritesManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementFavoritesManagement
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
        public AdvertisementFavoritesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// AddToAdvertisementFavorites
        /// </summary>
        /// <param name="addToAdvertisementFavoritesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToAdvertisementFavorites")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAdvertisementFavorites(AddToAdvertisementFavoritesPVM addToAdvertisementFavoritesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAdvertisementFavoritesPVM.ChildsUsersIds == null)
                    {
                        addToAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToAdvertisementFavoritesPVM.ChildsUsersIds.Count == 0)
                        addToAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToAdvertisementFavoritesPVM.ChildsUsersIds.Count == 1)
                        if (addToAdvertisementFavoritesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                var advertisementFavoritesVM = melkavanApiBusiness.AddToAdvertisementFavorites(
                    addToAdvertisementFavoritesPVM.AdvertisementFavoritesVM, teniacoApiBusiness.TeniacoApiDb);

                if (advertisementFavoritesVM != null)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = advertisementFavoritesVM;
                    jsonResultWithRecordObjectVM.Message = "Success";

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
        /// CompleteDeleteAdvertisementFavorites
        /// </summary>
        /// <param name="completeDeleteAdvertisementFavoritesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>
        [HttpPost("CompleteDeleteAdvertisementFavorites")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteAdvertisementFavorites(CompleteDeleteAdvertisementFavoritesPVM completeDeleteAdvertisementFavoritesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds == null)
                    {
                        completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds.Count == 0)
                        completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds.Count == 1)
                        if (completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            completeDeleteAdvertisementFavoritesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfAdvertisementFavorites = melkavanApiBusiness.CompleteDeleteAdvertisementFavorites(
                    completeDeleteAdvertisementFavoritesPVM.AdvertisementId,
                    completeDeleteAdvertisementFavoritesPVM.UserId.Value);

                jsonResultObjectVM.Result = "OK";

                return new JsonResult(jsonResultObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }


    }
}


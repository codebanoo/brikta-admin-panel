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
    /// AdvertisementViewersManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementViewersManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementViewersManagement
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
        public AdvertisementViewersManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAdvertisementViewersWithAdvertisementId
        /// </summary>
        /// <param name="getAdvertisementViewersWithAdvertisementIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAdvertisementViewersWithAdvertisementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAdvertisementViewers(GetAdvertisementViewersWithAdvertisementIdPVM getAdvertisementViewersWithAdvertisementIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds == null)
                    {
                        getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds.Count == 0)
                        getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds.Count == 1)
                        if (getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAdvertisementViewersWithAdvertisementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfAdvertisementViewers = melkavanApiBusiness.GetAdvertisementViewersByAdvertisementId(
                    getAdvertisementViewersWithAdvertisementIdPVM.AdvertisementId,
                    getAdvertisementViewersWithAdvertisementIdPVM.RecordType,
                    teniacoApiBusiness.TeniacoApiDb);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAdvertisementViewers;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfAdvertisementViewers.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }





        /// <summary>
        ///             
        /// </summary>
        /// <param name="addToAdvertisementViewersPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToAdvertisementViewers")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAdvertisementViewers([FromBody] AddToAdvertisementViewersPVM addToAdvertisementViewersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAdvertisementViewersPVM.ChildsUsersIds == null)
                    {
                        addToAdvertisementViewersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToAdvertisementViewersPVM.ChildsUsersIds.Count == 0)
                        {
                            addToAdvertisementViewersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToAdvertisementViewersPVM.ChildsUsersIds.Count == 1)
                                if (addToAdvertisementViewersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToAdvertisementViewersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }

                    addToAdvertisementViewersPVM.AdvertisementViewersVM.CreateEnDate = DateTime.Now;
                    addToAdvertisementViewersPVM.AdvertisementViewersVM.CreateTime = PersianDate.TimeNow;
                    addToAdvertisementViewersPVM.AdvertisementViewersVM.UserIdCreator = this.userId.Value;
                    addToAdvertisementViewersPVM.AdvertisementViewersVM.IsActivated = true;
                    addToAdvertisementViewersPVM.AdvertisementViewersVM.IsDeleted = false;
                }

                int advertisementViewersId = melkavanApiBusiness.AddToAdvertisementViewers(
                   addToAdvertisementViewersPVM.AdvertisementViewersVM, teniacoApiBusiness.TeniacoApiDb);


                if (advertisementViewersId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAdvertisementViewers";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
               if (advertisementViewersId > 0)
                {
                    addToAdvertisementViewersPVM.AdvertisementViewersVM.AdvertisementViewersId = advertisementViewersId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAdvertisementViewersPVM.AdvertisementViewersVM;

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


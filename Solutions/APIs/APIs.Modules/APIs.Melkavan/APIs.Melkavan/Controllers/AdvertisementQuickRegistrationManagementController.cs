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
using VM.PVM.Public;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// AdvertisementQuickRegistrationManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AdvertisementQuickRegistrationManagementController : ApiBaseController
    {
        /// <summary>
        /// AdvertisementQuickRegistrationManagement
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
        public AdvertisementQuickRegistrationManagementController(IHostEnvironment _hostingEnvironment,
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
        /// AdvertisementQuickRegistration
        /// </summary>
        /// <param name="addToAdvertisementPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AdvertisementQuickRegistration")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AdvertisementQuickRegistration([FromBody] AddToAdvertisementPVM
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


                }

                long AdvertisementId = melkavanApiBusiness.AdvertisementQuickRegistration(
                    addToAdvertisementPVM.AdvertisementVM,
                    publicApiBusiness,
                    consoleBusiness);


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
        /// AddOwnerOrConcultant
        /// </summary>
        /// <param name="addOwnerOrConcultantPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("AddOwnerOrConcultant")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddOwnerOrConcultant(AddOwnerOrConcultantPVM addOwnerOrConcultantPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });



            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addOwnerOrConcultantPVM.ChildsUsersIds == null)
                    {
                        addOwnerOrConcultantPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addOwnerOrConcultantPVM.ChildsUsersIds.Count == 0)
                        addOwnerOrConcultantPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addOwnerOrConcultantPVM.ChildsUsersIds.Count == 1)
                        if (addOwnerOrConcultantPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addOwnerOrConcultantPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                long AddOwnerOrConcultant = melkavanApiBusiness.AddOwnerOrConcultant(addOwnerOrConcultantPVM, consoleBusiness);


                if (AddOwnerOrConcultant.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateUser";

                    return new JsonResult(jsonResultWithRecordObjectVM);


                }else if (AddOwnerOrConcultant.Equals(-3))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateOwner";

                    return new JsonResult(jsonResultWithRecordObjectVM);


                }else if (AddOwnerOrConcultant.Equals(-2))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "ERRORConsultant";
                    return new JsonResult(jsonResultWithRecordObjectVM);


                }else if (AddOwnerOrConcultant.Equals(-5))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DupliacateConsultant";
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }else if(AddOwnerOrConcultant > 0)
                {

                    var users = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(AddOwnerOrConcultant)).FirstOrDefault();

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Message = "Successful";
                    jsonResultWithRecordObjectVM.Record = users;
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "ERRORMessage";
                    return new JsonResult(jsonResultWithRecordObjectVM);
                }


            }
            catch (Exception exc)
            { }


            return new JsonResult(jsonResultWithRecordObjectVM);
        }
    }
}

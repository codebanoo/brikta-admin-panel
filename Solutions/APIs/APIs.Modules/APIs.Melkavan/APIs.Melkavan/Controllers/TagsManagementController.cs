using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes.Helper;
using APIs.CustomAttributes;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Public.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Teniaco.Models.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Business.ConsoleBusiness;
using System.Net;
using System;
using VM.Base;
using VM.PVM.Melkavan;
using VM.Melkavan.PVM.Melkavan.Tags;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// BuildingLifesManagementController
    /// </summary>
    [CustomApiAuthentication]
    public class TagsManagementController : ApiBaseController
    {
        /// <summary>
        /// BuildingLifesManagementController
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
        public TagsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllTagsList
        /// </summary>
        /// <param name="getAllTagsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllTagsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllTagsList([FromBody] GetAllTagsListPVM getAllTagsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                int listCount = 0;

                var listOfTags = melkavanApiBusiness.GetAllTagsList(
                    ref listCount,
                    getAllTagsListPVM.ChildsUsersIds);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfTags;

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

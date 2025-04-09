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

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// FeaturesCategoriesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class FeaturesCategoriesManagementController : ApiBaseController
    {
        /// <summary>
        /// FeaturesCategoriesManagement
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
        public FeaturesCategoriesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllFeaturesCategoriesList
        /// </summary>
        /// <param name="getAllFeaturesCategoriesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllFeaturesCategoriesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllFeaturesCategoriesList([FromBody] GetAllFeaturesCategoriesListPVM getAllFeaturesCategoriesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllFeaturesCategoriesListPVM.ChildsUsersIds == null)
                    {
                        getAllFeaturesCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAllFeaturesCategoriesListPVM.ChildsUsersIds.Count == 0)
                        getAllFeaturesCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAllFeaturesCategoriesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllFeaturesCategoriesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllFeaturesCategoriesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfFeaturesCategories = teniacoApiBusiness.GetAllFeaturesCategoriesList();

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfFeaturesCategories;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// CreateFeaturesCategories
        /// </summary>
        /// <param name="CreateFeaturesCategoriesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("CreateFeaturesCategories")]
        [ProducesResponseType(typeof(JsonResultWithRecordObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CreateFeaturesCategories([FromBody] CreateFeaturesCategoriesPVM CreateFeaturesCategoriesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (CreateFeaturesCategoriesPVM.ChildsUsersIds == null)
                    {
                        CreateFeaturesCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (CreateFeaturesCategoriesPVM.ChildsUsersIds.Count == 0)
                        CreateFeaturesCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (CreateFeaturesCategoriesPVM.ChildsUsersIds.Count == 1)
                        if (CreateFeaturesCategoriesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            CreateFeaturesCategoriesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int id = teniacoApiBusiness.CreateFeaturesCategories(CreateFeaturesCategoriesPVM.FeaturesCategoriesVM);

                if (id > 0)
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = id;
                }
                else
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            {           
            }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordObjectVM);
        }

    }
}

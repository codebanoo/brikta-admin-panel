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
using VM.PVM.Projects;
using PVM.Projects.TeniacoSuggestions;
using System.Linq;
using VM.Projects.TeniacoSuggestions;
using VM.Projects;
using APIs.Projects.Models.Entities;
using System.Collections.Generic;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// TeniacoSuggestionsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class TeniacoSuggestionsManagementController : ApiBaseController
    {
        /// <summary>
        /// TeniacoSuggestionsManagement
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
        public TeniacoSuggestionsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetListOfTeniacoSuggestionFilesWithConstructionProjectId
        /// </summary>
        /// <param name="getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfTeniacoSuggestionFilesWithConstructionProjectId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfTeniacoSuggestionFilesWithConstructionProjectId([FromBody] GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds == null)
                    {
                        getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds.Count == 0)
                        getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds.Count == 1)
                        if (getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfTeniacoSuggestions = projectsApiBusiness.GetListOfTeniacoSuggestionFilesWithConstructionProjectId(
                   getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ConstructionProjectId,
                   getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM.ChildsUsersIds
                   );

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfTeniacoSuggestions;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetListOfTeniacoSuggestedProjects
        /// </summary>
        /// <param name="getListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfTeniacoSuggestedProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfTeniacoSuggestedProjects([FromBody] GetListOfTeniacoSuggestedProjectsPVM getListOfTeniacoSuggestedProjectsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds == null)
                    {
                        getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds.Count == 0)
                        getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfSuggestedProjects = projectsApiBusiness.GetListOfTeniacoSuggestedProjects(
                   getListOfTeniacoSuggestedProjectsPVM.pageNum,
                   getListOfTeniacoSuggestedProjectsPVM.pageSize,
                   getListOfTeniacoSuggestedProjectsPVM.ChildsUsersIds,
                   teniacoApiBusiness,
                   publicApiBusiness
                   );

                // درصد پیشرفت پروژه
                #region Project Operation
                var projectIds = listOfSuggestedProjects.Select(p => p.ConstructionProjectId).ToList();
                var constructionProjectProgressDataVMList = projectsApiBusiness.GetProjectProgressDataVM(projectIds);
                foreach (var project in listOfSuggestedProjects)
                {
                    if (constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                    {
                        var constructionProjectProgressDataVM = constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                        project.Operation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(7)).FirstOrDefault().CellData;//عملکرد
                    }
                }
                #endregion


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfSuggestedProjects;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception)
            {
            }



            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// AddToTeniacoSuggestionFiles
        /// </summary>
        /// <param name="addToTeniacoSuggestionFilesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("AddToTeniacoSuggestionFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToTeniacoSuggestionFiles([FromBody] AddToTeniacoSuggestionFilesPVM addToTeniacoSuggestionFilesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToTeniacoSuggestionFilesPVM.ChildsUsersIds == null)
                    {
                        addToTeniacoSuggestionFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToTeniacoSuggestionFilesPVM.ChildsUsersIds.Count == 0)
                        addToTeniacoSuggestionFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToTeniacoSuggestionFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToTeniacoSuggestionFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToTeniacoSuggestionFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfTeniacoSuggestions = projectsApiBusiness.AddToTeniacoSuggestionFiles(
                   addToTeniacoSuggestionFilesPVM.teniacoSuggestionFilesVM,
                   addToTeniacoSuggestionFilesPVM.ChildsUsersIds,
                   addToTeniacoSuggestionFilesPVM.UserId.Value,
                   addToTeniacoSuggestionFilesPVM.SuggestionPageTitle
                   );


                jsonResultWithRecordObjectVM.Result = "OK";

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// DeleteTeniacoSuggestionFile
        /// </summary>
        /// <param name="deleteTeniacoSuggestionFilePVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("DeleteTeniacoSuggestionFile")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult DeleteTeniacoSuggestionFile([FromBody] DeleteTeniacoSuggestionFilePVM deleteTeniacoSuggestionFilePVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (deleteTeniacoSuggestionFilePVM.ChildsUsersIds == null)
                    {
                        deleteTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (deleteTeniacoSuggestionFilePVM.ChildsUsersIds.Count == 0)
                        deleteTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (deleteTeniacoSuggestionFilePVM.ChildsUsersIds.Count == 1)
                        if (deleteTeniacoSuggestionFilePVM.ChildsUsersIds.FirstOrDefault() == 0)
                            deleteTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfTeniacoSuggestions = projectsApiBusiness.DeleteTeniacoSuggestionFile(
                   deleteTeniacoSuggestionFilePVM.SuggestionFileId,
                   deleteTeniacoSuggestionFilePVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// EditTeniacoSuggestionFile
        /// </summary>
        /// <param name="editTeniacoSuggestionFilePVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("EditTeniacoSuggestionFile")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult EditTeniacoSuggestionFile([FromBody] EditTeniacoSuggestionFilePVM editTeniacoSuggestionFilePVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (editTeniacoSuggestionFilePVM.ChildsUsersIds == null)
                    {
                        editTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (editTeniacoSuggestionFilePVM.ChildsUsersIds.Count == 0)
                        editTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (editTeniacoSuggestionFilePVM.ChildsUsersIds.Count == 1)
                        if (editTeniacoSuggestionFilePVM.ChildsUsersIds.FirstOrDefault() == 0)
                            editTeniacoSuggestionFilePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfTeniacoSuggestions = projectsApiBusiness.EditTeniacoSuggestionFile(
                   editTeniacoSuggestionFilePVM.teniacoSuggestionFilesVM,
                   editTeniacoSuggestionFilePVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception)
            {
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

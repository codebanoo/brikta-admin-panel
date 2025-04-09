using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
//using APIs.ConstructionProjects.Models.Business;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
using APIs.Projects.Models.Entities;
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
using VM.Console;
using VM.Projects;
using VM.PVM.Projects;
using VM.Teniaco;

namespace APIs.Projects.Controllers
{    /// <summary>
     /// ConstructionProjectsManagement
     /// </summary>
    [CustomApiAuthentication]
    public class ConstructionProjectsManagementController : ApiBaseController
    {
        /// <summary>
        /// ConstructionProjectsManagement
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
        public ConstructionProjectsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConstructionProjectsList
        /// </summary>
        /// <param name="getAllConstructionProjectsList"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllConstructionProjectsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConstructionProjectsList([FromBody] GetAllConstructionProjectsListPVM getAllConstructionProjectsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllConstructionProjectsListPVM.ChildsUsersIds == null)
                    {
                        getAllConstructionProjectsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllConstructionProjectsListPVM.ChildsUsersIds.Count == 0)
                        getAllConstructionProjectsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllConstructionProjectsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllConstructionProjectsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllConstructionProjectsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConstructionProjects = projectsApiBusiness.GetAllConstructionProjectsList(
                    getAllConstructionProjectsListPVM.ChildsUsersIds,
                   getAllConstructionProjectsListPVM.ConstructionProjectTitle
                   );

                var propertyIds = listOfConstructionProjects.Select(p => p.PropertyId).ToList();
                var projectIds = listOfConstructionProjects.Select(p => p.ConstructionProjectId).ToList();

                var properties = teniacoApiBusiness.TeniacoApiDb.Properties.Where(p => propertyIds.Contains(p.PropertyId)).ToList();

                var contractSides = projectsApiBusiness.ProjectsApiDb.ContractSides
                 .Where(p => p.TableTitle == "ConstructionProjects" && projectIds.Contains((int)p.TableRecordId) && p.ContractSideTypeId == 1)
                 .ToList();


                var userIds = contractSides.Select(a => a.ParentId).ToList();

                var users = consoleBusiness.CmsDb.Users.Where(p => userIds.Contains(p.UserId)).ToList();

                var constructionProjectTypes = projectsApiBusiness.ProjectsApiDb.ConstructionProjectTypes.ToList();

                foreach (var constructionProject in listOfConstructionProjects)
                {
                    if (users.Where(p => contractSides.Where(a => a.TableRecordId == constructionProject.ConstructionProjectId)
                        .Select(a => a.ParentId).Contains(p.UserId)).Any())
                    {
                        var user = users.Where(p => contractSides.Where(a => a.TableRecordId == constructionProject.ConstructionProjectId)
                            .Select(a => a.ParentId).Contains(p.UserId)).FirstOrDefault();

                        constructionProject.CustomUsersVM = new CustomUsersVM();
                        constructionProject.CustomUsersVM.UserId = user.UserId;
                        constructionProject.CustomUsersVM.Name = user.UsersProfileUser.Name;
                        constructionProject.CustomUsersVM.Family = user.UsersProfileUser.Family;
                        constructionProject.CustomUsersVM.Phone = user.UsersProfileUser.Phone;
                        constructionProject.CustomUsersVM.Mobile = user.UsersProfileUser.Mobile;
                    }
                    if (properties.Where(p => p.PropertyId == constructionProject.PropertyId).Any())
                    {
                        var property = properties.Where(p => p.PropertyId == constructionProject.PropertyId).FirstOrDefault();

                        constructionProject.PropertiesVM = new PropertiesVM
                        {
                            Area = property.Area,
                            BuiltInYear = property.BuiltInYear.HasValue ? property.BuiltInYear.Value : (int?)0,
                            BuiltInYearFa = property.BuiltInYearFa.HasValue ? property.BuiltInYearFa.Value : (int?)0,
                            ConsultantUserId = property.ConsultantUserId.HasValue ? property.ConsultantUserId.Value : (long?)null,
                            //OwnerPersonId = property.OwnerPersonId.HasValue ? property.OwnerPersonId.Value : (long?)null,
                            //Intermediary = property.Intermediary,
                            //IntermediaryPhone = property.IntermediaryPhone,
                            //IsPrivate = property.IsPrivate,
                            PropertyCodeName = property.PropertyCodeName,
                            PropertyId = property.PropertyId,
                            PropertyTypeId = property.PropertyTypeId,
                            RebuiltInYear = property.BuiltInYear.HasValue ? property.BuiltInYear.Value : (int?)0,
                            RebuiltInYearFa = property.RebuiltInYearFa.HasValue ? property.RebuiltInYearFa.Value : (int?)0,
                            TypeOfUseId = property.TypeOfUseId.HasValue ? property.TypeOfUseId.Value : (int?)0,
                            DocumentTypeId = property.DocumentOwnershipTypeId.HasValue ? property.DocumentOwnershipTypeId.Value : (int?)0,
                            DocumentOwnershipTypeId = property.DocumentOwnershipTypeId.HasValue ? property.DocumentOwnershipTypeId.Value : (int?)0,
                            DocumentRootTypeId = property.DocumentRootTypeId.HasValue ? property.DocumentRootTypeId.Value : (int?)0,
                            UserIdCreator = property.UserIdCreator.Value,
                            CreateEnDate = property.CreateEnDate,
                            CreateTime = property.CreateTime,
                            EditEnDate = property.EditEnDate,
                            EditTime = property.EditTime,
                            UserIdEditor = property.UserIdEditor.HasValue ? property.UserIdEditor.Value : (int?)0,
                            RemoveEnDate = property.RemoveEnDate,
                            RemoveTime = property.EditTime,
                            UserIdRemover = property.UserIdRemover.HasValue ? property.UserIdRemover.Value : (int?)0,
                            IsActivated = property.IsActivated,
                            IsDeleted = property.IsDeleted,
                        };
                    }

                    if (constructionProjectTypes.Where(c => c.ConstructionProjectTypeId == constructionProject.ConstructionProjectTypeId).Any())
                    {
                        var constructionProjectType = constructionProjectTypes.Where(c => c.ConstructionProjectTypeId == constructionProject.ConstructionProjectTypeId).FirstOrDefault();

                        constructionProject.ConstructionProjectTypesVM = new ConstructionProjectTypesVM();
                        constructionProject.ConstructionProjectTypesVM.ConstructionProjectTypeId = constructionProject.ConstructionProjectTypeId;
                        constructionProject.ConstructionProjectTypesVM.ConstructionProjectTypeTitle = constructionProject.ConstructionProjectTitle;
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjects;
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
        /// GetListOfConstructionProjects
        /// </summary>
        /// <param name="getListOfConstructionProjectsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetListOfConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConstructionProjects([FromBody] GetListOfConstructionProjectsPVM getListOfConstructionProjectsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfConstructionProjectsPVM.ChildsUsersIds == null)
                    {
                        getListOfConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfConstructionProjectsPVM.ChildsUsersIds.Count == 0)
                        getListOfConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfConstructionProjectsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfConstructionProjectsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConstructionProjects = projectsApiBusiness.GetListOfConstructionProjects(
                   getListOfConstructionProjectsPVM.jtStartIndex.Value,
                   getListOfConstructionProjectsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfConstructionProjectsPVM.ChildsUsersIds,
                   getListOfConstructionProjectsPVM.ConstructionProjectTitle,
                   getListOfConstructionProjectsPVM.jtSorting);


                var projectIds = listOfConstructionProjects.Select(p => p.ConstructionProjectId).ToList();


                #region projectOwnerUsers ==> سهامداران

                var constructionProjectOwnerUsers = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers.Where(p => projectIds.Contains(p.ConstructionProjectId)).ToList();

                //سهامداران آیدی
                var ownerUserIds = constructionProjectOwnerUsers.Select(c => c.OwnerUserId).Distinct().ToList();
                // لیست سهامداران
                var Ownerusers = consoleBusiness.CmsDb.Users.Where(p => ownerUserIds.Contains(p.UserId)).ToList();
                //پروفایل سهامداران
                var OwnerusersProfile = consoleBusiness.CmsDb.UsersProfile.Where(p => ownerUserIds.Contains(p.UserId)).ToList();

                #endregion

                #region Properties

                //آیدی ملک
                var propertyIds = listOfConstructionProjects.Select(p => p.PropertyId).Distinct().ToList();

                //لیست املاک
                var properties = teniacoApiBusiness.TeniacoApiDb.Properties.Where(p => propertyIds.Contains(p.PropertyId)).ToList();

                #endregion

                #region constructionProjectTypes

                //نوع پروژه
                var constructionProjectTypes = projectsApiBusiness.ProjectsApiDb.ConstructionProjectTypes.ToList();
                #endregion

                #region users ==> نماینده

                //var userIds = listOfConstructionProjects.Select(u => u.UserId).Distinct().ToList();

                //var users = consoleBusiness.CmsDb.Users.Where(u => userIds.Contains(u.UserId)).ToList();

                //var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(p => userIds.Contains(p.UserId)).ToList();
                #endregion

                #region ConstructionProjectsHistories
                var constructionProjectPriceHistories = projectsApiBusiness.ProjectsApiDb.ConstructionProjectPriceHistories
                    .Where(p => projectIds.Contains(p.ConstructionProjectId)).OrderByDescending(c => c.CreateEnDate).ThenByDescending(c => c.CreateTime).ToList();

                #endregion

                #region constructionProjectDataTypes
                var constructionProjectDataTypeCountsList = projectsApiBusiness.GetListConstructionProjectDataTypeCounts(projectIds);
                #endregion


                #region projectRepresentatives ==> نمایندگان

                var constructionProjectRepresentatives = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives.Where(p => projectIds.Contains(p.ConstructionProjectId)).ToList();

                //نمایندگان آیدی
                var RepresentativeIds = constructionProjectRepresentatives.Select(c => c.RepresentativeId).Distinct().ToList();

                // لیست نمایندگان
                var Representatives = consoleBusiness.CmsDb.Users.Where(p => RepresentativeIds.Contains(p.UserId)).ToList();

                //پروفایل نمایندگان
                var RepresentativesProfile = consoleBusiness.CmsDb.UsersProfile.Where(p => RepresentativeIds.Contains(p.UserId)).ToList();

                #endregion


                #region TeniacoSuggestions

                var teniacoSuggestionsList = projectsApiBusiness.ProjectsApiDb.TeniacoSuggestionFiles.Where(c => projectIds.Contains(c.ConstructionProjectId)).ToList();

                #endregion


                foreach (var project in listOfConstructionProjects)
                {
                    try
                    {
                        if (constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                        {
                            var constructionProjectDataTypeCounts = constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();


                            if (teniacoSuggestionsList.Where(c => c.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                project.HasSuggestion = true;
                            }


                            project.ConfirmationAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ConfirmationAgreements")).FirstOrDefault()?.Count;

                            if (project.ConfirmationAgreementsCount == null)
                            {
                                project.ConfirmationAgreementsCount = 0;
                            }

                            project.ContractAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractAgreements")).FirstOrDefault()?.Count;

                            if (project.ContractAgreementsCount == null)
                            {
                                project.ContractAgreementsCount = 0;
                            }

                            project.ContractorsAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                            p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractorsAgreements")).FirstOrDefault()?.Count;

                            if (project.ContractorsAgreementsCount == null)
                            {
                                project.ContractorsAgreementsCount = 0;
                            }

                            project.InitialPlansCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("InitialPlans")).FirstOrDefault()?.Count;


                            if (project.InitialPlansCount == null)
                            {
                                project.InitialPlansCount = 0;
                            }


                            project.MeetingBoardsCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("MeetingBoards")).FirstOrDefault()?.Count;


                            if (project.MeetingBoardsCount == null)
                            {
                                project.MeetingBoardsCount = 0;
                            }


                            project.PartnershipAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PartnershipAgreements")).FirstOrDefault()?.Count;

                            if (project.PartnershipAgreementsCount == null)
                            {
                                project.PartnershipAgreementsCount = 0;
                            }


                            project.PitchDecksCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PitchDecks")).FirstOrDefault()?.Count;

                            if (project.PitchDecksCount == null)
                            {
                                project.PitchDecksCount = 0;
                            }


                            project.ProgressPicturesCount = constructionProjectDataTypeCounts.Where(p =>
                                p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ProgressPictures")).FirstOrDefault()?.Count;


                            if (project.ProgressPicturesCount == null)
                            {
                                project.ProgressPicturesCount = 0;
                            }


                            project.BillDelaysCount = constructionProjectDataTypeCounts.Where(p =>
                               p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("BillDelays")).FirstOrDefault()?.Count;


                            if (project.BillDelaysCount == null)
                            {
                                project.BillDelaysCount = 0;
                            }
                        }
                        else
                        {
                            project.ConfirmationAgreementsCount = 0;
                            project.ContractAgreementsCount = 0;
                            project.ContractorsAgreementsCount = 0;
                            project.InitialPlansCount = 0;
                            project.MeetingBoardsCount = 0;
                            project.PartnershipAgreementsCount = 0;
                            project.PitchDecksCount = 0;
                            project.ProgressPicturesCount = 0;
                            project.BillDelaysCount = 0;
                        }
                    }
                    catch (Exception exc)
                    { }
                    //project.ConfirmationAgreementsCount = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.ContractAgreementsCount = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.ContractorsAgreementsCount = projectsApiBusiness.ProjectsApiDb.ContractorsAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.InitialPlansCount = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.MeetingBoardsCount = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.PartnershipAgreementsCount = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.PitchDecksCount = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    //project.ProgressPicturesCount = projectsApiBusiness.ProjectsApiDb.ProgressPictures.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();

                    #region projectOwnerUsers ==> سهامداران

                    //projectOwnerUsers
                    if (constructionProjectOwnerUsers.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                    {
                        try
                        {
                            var projectOwner = constructionProjectOwnerUsers.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                            project.ConstructionProjectOwnerUsersVM = projectOwner.Select(c => new ConstructionProjectOwnerUsersVM
                            {
                                ConstructionProjectOwnerUserId = c.ConstructionProjectOwnerUserId,
                                OwnerUserId = c.OwnerUserId,
                                SharePercent = c.SharePercent,
                                ConstructionProjectId = c.ConstructionProjectId,
                                OwnerUserFamiy = OwnerusersProfile.Where(p => p.UserId.Equals(c.OwnerUserId)).FirstOrDefault().Family != "" ?
                                  OwnerusersProfile.Where(p => p.UserId.Equals(c.OwnerUserId)).FirstOrDefault().Family : "",
                                UserName = Ownerusers.Where(u => u.UserId.Equals(c.OwnerUserId)).FirstOrDefault().UserName != "" ?
                                Ownerusers.Where(u => u.UserId.Equals(c.OwnerUserId)).FirstOrDefault().UserName : ""


                            }).ToList();

                        }
                        catch (Exception exc)
                        { }
                    }
                    #endregion

                    #region properties

                    //properties
                    if (properties.Where(p => p.PropertyId.Equals(project.PropertyId)).Any())
                    {
                        var property = properties.Where(p => p.PropertyId.Equals(project.PropertyId)).ToList();

                        project.PropertiesVM = property.Select(c => new PropertiesVM
                        {
                            PropertyId = c.PropertyId,
                            PropertyCodeName = c.PropertyCodeName

                        }).FirstOrDefault();
                    }
                    #endregion

                    #region users ==> نماینده

                    //if (users.Where(p => p.UserId.Equals(project.UserId)).Any())
                    //{
                    //    //نماینده
                    //    var user = users.Where(p => p.UserId.Equals(project.UserId)).ToList();

                    //    project.CustomUsersVM = user.Select(c => new CustomUsersVM
                    //    {
                    //        UserId = c.UserId,
                    //        UserName = c.UserName,
                    //        Email = c.Email,
                    //        DomainSettingId = c.DomainSettingId,
                    //        Name = usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Name != "" ?
                    //          usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Name : "",
                    //        Family = usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Family != "" ?
                    //          usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Family : "",
                    //        Mobile = usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Mobile != "" ?
                    //          usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Mobile : "",
                    //        Phone = usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Phone != "" ?
                    //          usersProfile.Where(p => p.UserId.Equals(c.UserId)).FirstOrDefault().Phone : "",
                    //    }).FirstOrDefault();

                    //}

                    #endregion

                    #region constructionProjectTypes

                    if (constructionProjectTypes.Where(p => p.ConstructionProjectTypeId.Equals(project.ConstructionProjectTypeId)).Any())
                    {
                        var constructionProjectType = constructionProjectTypes.Where(p => p.ConstructionProjectTypeId.Equals(project.ConstructionProjectTypeId)).ToList();

                        project.ConstructionProjectTypesVM = constructionProjectType.Select(c => new ConstructionProjectTypesVM
                        {
                            ConstructionProjectTypeId = c.ConstructionProjectTypeId,
                            ConstructionProjectTypeTitle = c.ConstructionProjectTypeTitle

                        }).FirstOrDefault();
                    }
                    #endregion

                    #region ConstructionProjectHistories

                    if (constructionProjectPriceHistories.Where(p => p.ConstructionProjectId == project.ConstructionProjectId).Any())
                    {
                        var constructionProjectPriceHistory = constructionProjectPriceHistories.Where(p => p.ConstructionProjectId == project.ConstructionProjectId).FirstOrDefault();

                        project.PrevisionOfCost = constructionProjectPriceHistory.PrevisionOfCost;
                        project.CurrentValueOfProject = constructionProjectPriceHistory.CurrentValueOfProject;
                        project.ProjectEstimate = constructionProjectPriceHistory.ProjectEstimate;
                    }

                    #endregion


                    #region projectRepresentatives ==> نمایندگان

                    //projectRepresentatives
                    if (constructionProjectRepresentatives.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                    {
                        try
                        {
                            var projectRepresentor = constructionProjectRepresentatives.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                            project.ConstructionProjectRepresentativesVM = projectRepresentor.Select(c => new ConstructionProjectRepresentativesVM
                            {
                                ConstructionProjectRepresentativeId = c.ConstructionProjectRepresentativeId,
                                OwnerUserId = c.OwnerUserId,
                                RepresentativeId = c.RepresentativeId,
                                ConstructionProjectId = c.ConstructionProjectId,
                                RepresentativeFamiy = RepresentativesProfile.Where(p => p.UserId.Equals(c.RepresentativeId)).FirstOrDefault().Family ,
                                RepresentativeName = Representatives.Where(u => u.UserId.Equals(c.RepresentativeId)).FirstOrDefault().UserName 

                            }).ToList();


                            #region comments


                            //project.ConstructionProjectRepresentativesVM = projectOwner.Select(c => new ConstructionProjectRepresentativesVM
                            //{
                            //    ConstructionProjectRepresentativeId = c.ConstructionProjectRepresentativeId,
                            //    OwnerUserId = c.OwnerUserId,
                            //    RepresentativeId = c.RepresentativeId,
                            //    ConstructionProjectId = c.ConstructionProjectId,


                            //    RepresentativeFamiy = RepresentativesProfile.Where(p => p.UserId.Equals(c.RepresentativeId)).FirstOrDefault().Family != null ?
                            //      OwnerusersProfile.Where(p => p.UserId.Equals(c.RepresentativeId)).FirstOrDefault().Family : "",



                            //    RepresentativeName = Representatives.Where(u => u.UserId.Equals(c.RepresentativeId)).FirstOrDefault().UserName != null ?
                            //    Ownerusers.Where(u => u.UserId.Equals(c.RepresentativeId)).FirstOrDefault().UserName : ""


                            //}).ToList();

                            #endregion

                        }
                        catch (Exception exc)
                        { }
                    }
                    #endregion
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjects;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            {

            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// AddToConstructionProjects
        /// </summary>
        /// <param name="addToConstructionProjectsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConstructionProjects([FromBody] AddToConstructionProjectsPVM addToConstructionProjectsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToConstructionProjectsPVM.ChildsUsersIds == null)
                    {
                        addToConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToConstructionProjectsPVM.ChildsUsersIds.Count == 0)
                        addToConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToConstructionProjectsPVM.ChildsUsersIds.Count == 1)
                        if (addToConstructionProjectsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var currentPrice = teniacoApiBusiness.GetLastPropertiesPriceHistoryByPropertyId(
                    addToConstructionProjectsPVM.ChildsUsersIds,
                    addToConstructionProjectsPVM.ConstructionProjectsVM.PropertyId).CalculatedOfferPrice;


                addToConstructionProjectsPVM.ConstructionProjectsVM.CurrentValueOfProject = Convert.ToInt64(currentPrice);

                var constructionProjectId = projectsApiBusiness.AddToConstructionProjects(addToConstructionProjectsPVM.ConstructionProjectsVM
                    /*,addToConstructionProjectsPVM.PersonId*/);
                if (constructionProjectId != 0)
                {
                    addToConstructionProjectsPVM.ConstructionProjectsVM.ConstructionProjectId = constructionProjectId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToConstructionProjectsPVM.ConstructionProjectsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// UpdateConstructionProjects
        /// </summary>
        /// <param name="updateConstructionProjectsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("UpdateConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConstructionProjects([FromBody] UpdateConstructionProjectsPVM updateConstructionProjectsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateConstructionProjectsPVM.ChildsUsersIds == null)
                    {
                        updateConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateConstructionProjectsPVM.ChildsUsersIds.Count == 0)
                        updateConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateConstructionProjectsPVM.ChildsUsersIds.Count == 1)
                        if (updateConstructionProjectsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateConstructionProjectsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);



                    updateConstructionProjectsPVM.ConstructionProjectsVM.EditEnDate = DateTime.Now;
                    updateConstructionProjectsPVM.ConstructionProjectsVM.EditTime = PersianDate.TimeNow;
                    updateConstructionProjectsPVM.ConstructionProjectsVM.UserIdEditor = this.userId.Value;
                }



                var constructionProjectsVM = updateConstructionProjectsPVM.ConstructionProjectsVM;

                long constructionProjectId = projectsApiBusiness.UpdateConstructionProjects(
                            updateConstructionProjectsPVM.ChildsUsersIds,
                        updateConstructionProjectsPVM.ConstructionProjectsVM);

                if (constructionProjectId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateConstructionProject";
                }
                else
                if (constructionProjectId > 0)
                {
                    updateConstructionProjectsPVM.ConstructionProjectsVM.ConstructionProjectId = constructionProjectId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateConstructionProjectsPVM.ConstructionProjectsVM;
                }
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// ToggleActivationConstructionProjects
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("ToggleActivationConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationConstructionProjects(ToggleActivationConstructionProjectsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationConstructionProjects(
                    toggleActivationInvestorsPVM.ConstructionProjectId,
                   toggleActivationInvestorsPVM.UserId.Value,
                   toggleActivationInvestorsPVM.ChildsUsersIds
                    ))
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


        /// <summary>
        /// toggleShowInDashboardConstructionProjectsPVM
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("ToggleShowInDashboardConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleShowInDashboardConstructionProjects(ToggleShowInDashboardConstructionProjectsPVM toggleShowInDashboardConstructionProjectsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {

                string result = projectsApiBusiness.ToggleShowInDashboardConstructionProjects(
                    toggleShowInDashboardConstructionProjectsPVM.ConstructionProjectId,
                   toggleShowInDashboardConstructionProjectsPVM.UserId.Value,
                   toggleShowInDashboardConstructionProjectsPVM.ChildsUsersIds
                    );


                if(result != "error" && result != "ok")
                {
                    jsonResultWithRecordObjectVM.Result = "ReturnName";
                    jsonResultWithRecordObjectVM.Record = result;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if (result != "error")
                {
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Message = "Success";
                    jsonResultWithRecordObjectVM.Record = result;

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
        /// TemporaryDeleteConstructionProjects
        /// </summary>
        /// <param name="temporaryDeleteConstructionProjectsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        /// 

        [HttpPost("TemporaryDeleteConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteConstructionProjects(TemporaryDeleteConstructionProjectsPVM temporaryDeleteConstructionProjectsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteConstructionProjects(
                    temporaryDeleteConstructionProjectsPVM.ConstructionProjectId,
                   temporaryDeleteConstructionProjectsPVM.UserId.Value,
                   temporaryDeleteConstructionProjectsPVM.ChildsUsersIds
                    ))
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





        /// <summary>
        /// CompleteDeleteConstructionProjects
        /// </summary>
        /// <param name="completeDeleteConstructionProjectsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("CompleteDeleteConstructionProjects")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteConstructionProjects(CompleteDeleteConstructionProjectsPVM completeDeleteConstructionProjectsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteConstructionProjects(
                    completeDeleteConstructionProjectsPVM.ConstructionProjectId,
                   completeDeleteConstructionProjectsPVM.UserId.Value,
                   completeDeleteConstructionProjectsPVM.ChildsUsersIds
                    );
                if (result.HasValue && result.Value == true)
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultObjectVM.Result = "ERROR";
            if (result == null)
            {
                jsonResultObjectVM.Message = "ERROR_DEPENDENCY";
            }
            else
                jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }



        /// <summary>
        /// GetConstructionProjectWithConstructionProjectId
        /// </summary>
        /// <param name="getConstructionProjectWithConstructionProjectIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetConstructionProjectWithConstructionProjectId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConstructionProjectWithConstructionProjectId([FromBody] GetConstructionProjectWithConstructionProjectIdPVM
            getConstructionProjectWithConstructionProjectIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var constructionProject = projectsApiBusiness.GetConstructionProjectWithConstructionProjectId(
                    getConstructionProjectWithConstructionProjectIdPVM.ConstructionProjectId.Value,
                    getConstructionProjectWithConstructionProjectIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = constructionProject;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }




        //outterDashboard


        /// <summary>
        /// GetListOfConstructionProjectsWithUserId
        /// </summary>
        /// <param name="getConstructionProjectWithUserIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetListOfConstructionProjectsWithUserId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConstructionProjectsWithUserId([FromBody] GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectWithUserIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                //if (publicApiBusiness.PublicApiDb.Persons.Where(p => p.UserIdCreator.Equals(.PersonId)).Any())
                //{getConstructionProjectWithPersonIdPVM
                //    var person = publicApiBusiness.PublicApiDb.Persons.Where(p => p.UserIdCreator.Equals(getConstructionProjectWithPersonIdPVM.PersonId)).FirstOrDefault();
                //    getConstructionProjectWithPersonIdPVM.PersonId = person.PersonId;
                //}


                var listOfConstructionProjects = projectsApiBusiness.GetListOfConstructionProjectsWithUserId(getConstructionProjectWithUserIdPVM);

                if (listOfConstructionProjects.Count > 0)
                {
                    var projectIds = listOfConstructionProjects.Select(p => p.ConstructionProjectId).ToList();
                    var constructionProjectDataTypeCountsList = projectsApiBusiness.GetListConstructionProjectDataTypeCounts(projectIds);

                    var constructionProjectProgressDataVMList = projectsApiBusiness.GetProjectProgressDataVM(projectIds);

                    foreach (var project in listOfConstructionProjects)
                    {
                        try
                        {
                            if (constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                var constructionProjectDataTypeCounts = constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                                project.ConfirmationAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ConfirmationAgreements")).FirstOrDefault()?.Count;

                                project.ContractAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractAgreements")).FirstOrDefault()?.Count;

                                project.ContractorsAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractorsAgreements")).FirstOrDefault()?.Count;

                                project.InitialPlansCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("InitialPlans")).FirstOrDefault()?.Count;

                                project.MeetingBoardsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("MeetingBoards")).FirstOrDefault()?.Count;

                                project.PartnershipAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PartnershipAgreements")).FirstOrDefault()?.Count;

                                project.PartnershipAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PartnershipAgreements")).FirstOrDefault()?.Count;

                                project.PitchDecksCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PitchDecks")).FirstOrDefault()?.Count;

                                project.ProgressPicturesCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ProgressPictures")).FirstOrDefault()?.Count;
                            }
                        }
                        catch (Exception exc)
                        { }

                        try
                        {
                            if (constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                var constructionProjectProgressDataVM = constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                                project.ProjectProgressDataVM = new ProjectProgressDataVM();

                                project.ProjectProgressDataVM.Program = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(6)).FirstOrDefault().CellData;//برنامه
                                project.ProjectProgressDataVM.Operation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(7)).FirstOrDefault().CellData;//عملکرد
                                project.ProjectProgressDataVM.Deviation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(8)).FirstOrDefault().CellData;//انحراف
                                project.ProjectProgressDataVM.ProgramStart = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(10)).FirstOrDefault().CellData;//شروع برنامه
                                project.ProjectProgressDataVM.ProgramEnd = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(11)).FirstOrDefault().CellData;//پایان برنامه
                            }
                        }
                        catch (Exception exc)
                        { }

                        //project.ConfirmationAgreementsCount = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.ContractAgreementsCount = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.ContractorsAgreementsCount = projectsApiBusiness.ProjectsApiDb.ContractorsAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.InitialPlansCount = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.MeetingBoardsCount = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.PartnershipAgreementsCount = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.PitchDecksCount = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                        //project.ProgressPicturesCount = projectsApiBusiness.ProjectsApiDb.ProgressPictures.Where(f => f.ConstructionProjectId.Equals(project.ConstructionProjectId)).Count();
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjects;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfConstructionProjects.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception e)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }



        /// <summary>
        /// GetConstructionProjectsDailyDataByProjectId
        /// </summary>
        /// <param name="GetConstructionProjectsDailyDataByConstructionProjectId"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetConstructionProjectsDailyDataByConstructionProjectId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConstructionProjectsDailyDataByConstructionProjectId([FromBody] GetConstructionProjectsDailyDataByProjectIdPVM getConstructionProjectsDailyDataByProjectIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectsDailyDataByProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;
                var List = projectsApiBusiness.GetConstructionProjectsDailyDataByConstructionProjectId(getConstructionProjectsDailyDataByProjectIdPVM.jtStartIndex.Value, getConstructionProjectsDailyDataByProjectIdPVM.jtPageSize.Value, ref listCount,
                    getConstructionProjectsDailyDataByProjectIdPVM.ConstructionProjectId);



                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = List;
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
        /// GetAgreementDataByAgreementTypeAndConstructionProjectId
        /// </summary>
        /// <param name="GetAgreementDataByAgreementTypeAndConstructionProjectId"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetAgreementDataByAgreementTypeAndConstructionProjectId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAgreementDataByAgreementTypeAndConstructionProjectId([FromBody] GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM getAgreementDataByAgreementTypeAndConstructionProjectIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM agreementDataByAgreementTypeAndConstructionProjectIdPVM = new GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds == null)
                    {
                        getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds.Count == 0)
                        getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds.Count == 1)
                        if (getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var List = projectsApiBusiness.GetAgreementDataByAgreementTypeAndConstructionProjectId(
                getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ConstructionProjectId,
                getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.ContractType,
                getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.UserId.Value,
                getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.HaveAttachments,
                getAgreementDataByAgreementTypeAndConstructionProjectIdPVM.HaveConversations
                );
                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = List;
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
        /// GetListOfConstructionProjectsWithRepresentativeId
        /// </summary>
        /// <param name="getConstructionProjectWithUserIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetListOfConstructionProjectsWithRepresentativeId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConstructionProjectsWithRepresentativeId([FromBody] GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectWithUserIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


 


                var listOfConstructionProjects = projectsApiBusiness.GetListOfConstructionProjectsWithRepresentativeId(getConstructionProjectWithUserIdPVM, consoleBusiness);

                if (listOfConstructionProjects.Count > 0)
                {
                    var projectIds = listOfConstructionProjects.Select(p => p.ConstructionProjectId).ToList();
                    var constructionProjectDataTypeCountsList = projectsApiBusiness.GetListConstructionProjectDataTypeCounts(projectIds);

                    var constructionProjectProgressDataVMList = projectsApiBusiness.GetProjectProgressDataVM(projectIds);

                    foreach (var project in listOfConstructionProjects)
                    {
                        try
                        {
                            if (constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                var constructionProjectDataTypeCounts = constructionProjectDataTypeCountsList.Where(p => p.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                                project.ConfirmationAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ConfirmationAgreements")).FirstOrDefault()?.Count;

                                project.ContractAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractAgreements")).FirstOrDefault()?.Count;

                                project.ContractorsAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ContractorsAgreements")).FirstOrDefault()?.Count;

                                project.InitialPlansCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("InitialPlans")).FirstOrDefault()?.Count;

                                project.MeetingBoardsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("MeetingBoards")).FirstOrDefault()?.Count;

                                project.PartnershipAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PartnershipAgreements")).FirstOrDefault()?.Count;

                                project.PartnershipAgreementsCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PartnershipAgreements")).FirstOrDefault()?.Count;

                                project.PitchDecksCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("PitchDecks")).FirstOrDefault()?.Count;

                                project.ProgressPicturesCount = constructionProjectDataTypeCounts.Where(p =>
                                    p.ConstructionProjectId.Equals(project.ConstructionProjectId) && p.DataType.Equals("ProgressPictures")).FirstOrDefault()?.Count;
                            }
                        }
                        catch (Exception exc)
                        { }

                        try
                        {
                            if (constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                var constructionProjectProgressDataVM = constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();

                                project.ProjectProgressDataVM = new ProjectProgressDataVM();

                                project.ProjectProgressDataVM.Program = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(6)).FirstOrDefault().CellData;//برنامه
                                project.ProjectProgressDataVM.Operation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(7)).FirstOrDefault().CellData;//عملکرد
                                project.ProjectProgressDataVM.Deviation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(8)).FirstOrDefault().CellData;//انحراف
                                project.ProjectProgressDataVM.ProgramStart = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(10)).FirstOrDefault().CellData;//شروع برنامه
                                project.ProjectProgressDataVM.ProgramEnd = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(11)).FirstOrDefault().CellData;//پایان برنامه
                            }
                        }
                        catch (Exception exc)
                        { }

                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConstructionProjects;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listOfConstructionProjects.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception e)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }





        /// <summary>
        /// GetAllProgressDataList
        /// </summary>
        /// <param name="getConstructionProjectWithUserIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        /// 

        [HttpPost("GetAllProgressDataList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllProgressDataList([FromBody] GetConstructionProjectWithUserIdPVM getConstructionProjectWithUserIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            List<ProjectProgressDataVM> projectProgressDatasList = new List<ProjectProgressDataVM>();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConstructionProjectWithUserIdPVM.ChildsUsersIds == null)
                    {
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 0)
                        getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.Count == 1)
                        if (getConstructionProjectWithUserIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConstructionProjectWithUserIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }



                var projectOwnerUsers = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers.Where
                    (c => c.OwnerUserId.Equals(getConstructionProjectWithUserIdPVM.UserId)).ToList();

                var projectIds = projectOwnerUsers.Select(c=>c.ConstructionProjectId).ToList();

                var projects = projectsApiBusiness.ProjectsApiDb.ConstructionProjects.Where
                    (c => projectIds.Contains(c.ConstructionProjectId)).ToList();


                if (projectIds.Count > 0)
                {
                    var constructionProjectProgressDataVMList = projectsApiBusiness.GetAllProgressDataList(projectIds);

                    foreach (var project in projects)
                    {
                        try
                        {
                            if (constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).Any())
                            {
                                var constructionProjectProgressDataVM = constructionProjectProgressDataVMList.Where(d => d.ConstructionProjectId.Equals(project.ConstructionProjectId)).ToList();
                                
                                projectProgressDatasList.Add(new ProjectProgressDataVM()
                                {
                                    ConstructionProjectId = project.ConstructionProjectId,
                                    ConstructionProjectTitle = project.ConstructionProjectTitle,
                                    //Program = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(6)).FirstOrDefault().CellData,
                                    Operation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(7)).FirstOrDefault().CellData,
                                    Deviation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(8)).FirstOrDefault().CellData,
                                    ProgramStart = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(10)).FirstOrDefault().CellData,
                                    ProgramEnd = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(11)).FirstOrDefault().CellData,
                                });

                                //projectProgressDatasList.Select(c => c.Program = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(6)).FirstOrDefault().CellData); //برنامه
                                //projectProgressDatasList.Select(c => c.Operation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(7)).FirstOrDefault().CellData);//عملکرد
                                //projectProgressDatasList.Select(c => c.Deviation = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(8)).FirstOrDefault().CellData);//انحراف
                                //projectProgressDatasList.Select(c => c.ProgramStart = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(10)).FirstOrDefault().CellData);//شروع برنامه
                                //projectProgressDatasList.Select(c => c.ProgramEnd = constructionProjectProgressDataVM.Where(c => c.CellIndex.Equals(11)).FirstOrDefault().CellData);//پایان برنامه
                            }
                        }
                        catch (Exception exc)
                        { }

                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = projectProgressDatasList;
                jsonResultWithRecordsObjectVM.TotalRecordCount = projectProgressDatasList.Count;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception e)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

    }
}

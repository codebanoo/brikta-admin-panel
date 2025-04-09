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
using System.Linq;
using System.Collections.Generic;
using APIs.Projects.Models.Entities;
using VM.Console;
using VM.Projects;
using Models.Entities.Console;
using Models.Business.AutoMapper;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// ConversationLogsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ConversationLogsManagementController : ApiBaseController
    {
        /// <summary>
        /// ConversationLogsManagement
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
        /// <param name="_telegramBotApiBusiness"></param>
        /// <param name="_appSettingsSection"></param>
        public ConversationLogsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConversationLogsList
        /// </summary>
        /// <param name="getListOfConversationLogsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllConversationLogsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConversationLogsList([FromBody] GetAllConversationLogsListPVM getAllConversationLogsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllConversationLogsListPVM.ChildsUsersIds == null)
                    {
                        getAllConversationLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllConversationLogsListPVM.ChildsUsersIds.Count == 0)
                        getAllConversationLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllConversationLogsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllConversationLogsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllConversationLogsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConversationLogs = projectsApiBusiness.GetAllConversationLogsList(
                    getAllConversationLogsListPVM.ChildsUsersIds,
                    getAllConversationLogsListPVM.ConversationLogTitle,
                    getAllConversationLogsListPVM.ConversationLogDescription,
                    getAllConversationLogsListPVM.TableTitle,
                    getAllConversationLogsListPVM.RecordId);


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConversationLogs;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// GetListOfConversationLogs
        /// </summary>
        /// <param name="getListOfConversationLogsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConversationLogs([FromBody] GetListOfConversationLogsPVM getListOfConversationLogsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfConversationLogsPVM.ChildsUsersIds == null)
                    {
                        getListOfConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfConversationLogsPVM.ChildsUsersIds.Count == 0)
                        getListOfConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfConversationLogsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfConversationLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConversationLogs = projectsApiBusiness.GetListOfConversationLogs(
                    getListOfConversationLogsPVM.jtStartIndex.Value,
                    getListOfConversationLogsPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfConversationLogsPVM.ChildsUsersIds,
                    getListOfConversationLogsPVM.ConversationLogTitle,
                    getListOfConversationLogsPVM.ConversationLogDescription,
                    getListOfConversationLogsPVM.TableTitle,
                    getListOfConversationLogsPVM.RecordId,
                    getListOfConversationLogsPVM.jtSorting
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConversationLogs;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

        /// <summary>
        /// AddToConversationLogs
        /// </summary>
        /// <param name="addToConversationLogsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConversationLogs([FromBody] AddToConversationLogsPVM addToConversationLogsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToConversationLogsPVM.ChildsUsersIds == null)
                    {
                        addToConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToConversationLogsPVM.ChildsUsersIds.Count == 0)
                        addToConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToConversationLogsPVM.ChildsUsersIds.Count == 1)
                        if (addToConversationLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var conversationLogId = projectsApiBusiness.AddToConversationLogs(addToConversationLogsPVM.ConversationLogsVM);


              var userIdCreator =   projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => c.ConversationLogId.Equals(conversationLogId)).Select(c => c.UserIdCreator).FirstOrDefault();



                var userId = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(userIdCreator)).Select(c => c.UserId).FirstOrDefault();
                var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(userId)).FirstOrDefault();

                if(userProfile != null)
                {

                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM = new CustomUsersVM();
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.Family = userProfile.Family;
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.Name = userProfile.Name;
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.Phone = userProfile.Phone;
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.Mobile = userProfile.Mobile;
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.UserId = userId;
                    addToConversationLogsPVM.ConversationLogsVM.CustomUsersVM.Picture = userProfile.Picture;
                }

                if (conversationLogId != 0)
                {
                    //ارسال اس ام اس به نمایندگان
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل نمایندگان
                    long ConstructionProjectId = 0;
                    switch (addToConversationLogsPVM.ConversationLogsVM.TableTitle)
                    {
                        case "ContractAgreement":
                            var ContractAgreement = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = ContractAgreement.ConstructionProjectId;
                            break;

                        case "PartnershipAgreement":
                            var PartnershipAgreement = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(c => c.PartnershipAgreementId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = PartnershipAgreement.ConstructionProjectId;
                            break;

                        case "MeetingBoard":
                            var MeetingBoards = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = MeetingBoards.ConstructionProjectId;
                            break;

                        case "ConfirmationAgreement":
                            var ConfirmationAgreements = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = ConfirmationAgreements.ConstructionProjectId;
                            break;

                        case "InitialPlan":
                            var InitialPlan = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(c => c.InitialPlanId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = InitialPlan.ConstructionProjectId;
                            break;

                        case "PitchDeck": 
                            var PitchDeck = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(c => c.PitchDeckId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            ConstructionProjectId = PitchDeck.ConstructionProjectId;
                            break;

                        case "Attachments": 
                            var AttachmentFile = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(c => c.AttachementId == addToConversationLogsPVM.ConversationLogsVM.RecordId).FirstOrDefault();
                            switch (AttachmentFile.AttachementTableTitle)
                            {
                                case "ContractAgreements":
                                    var ContractAgreement2 = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = ContractAgreement2.ConstructionProjectId;
                                    break;

                                case "PartnershipAgreements":
                                    var PartnershipAgreement2 = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(c => c.PartnershipAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = PartnershipAgreement2.ConstructionProjectId;
                                    break;

                                case "MeetingBoards":
                                    var MeetingBoards2 = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = MeetingBoards2.ConstructionProjectId;
                                    break;

                                case "ConfirmationAgreements":
                                    var ConfirmationAgreements2 = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = ConfirmationAgreements2.ConstructionProjectId;
                                    break;

                                case "InitialPlans":
                                    var InitialPlan2 = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(c => c.InitialPlanId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = InitialPlan2.ConstructionProjectId;
                                    break;

                                case "PitchDecks":
                                    var PitchDeck2 = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(c => c.PitchDeckId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    ConstructionProjectId = PitchDeck2.ConstructionProjectId;
                                    break;
                            }
                            break;
                    }

                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == ConstructionProjectId && c.OwnerUserId!= addToConversationLogsPVM.ConversationLogsVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == ConstructionProjectId)
                        .Select(c => c.ConstructionProjectTitle).FirstOrDefault();


                    // ارسال اس ام اس به سرمایه گذاران
                    if (smsSender != null && OwnerPhoneNumbers.Count != 0)
                    {
                        for (int i = 0; i < OwnerPhoneNumbers.Count; i++)
                        {
                            string format = "{0} عزیز \n یک پیام در پلتفرم تنیاکو در پروژه {1} برای شما بارگذاری شده است. لطفا ملاحظه فرمایید\n{2}.";
                            string Message = String.Format(format, OwnerFamilyNames[i], ConstructionProjectTitle, "https://my.teniaco.com/home/login");
                            sendSmsService.SendSms(OwnerPhoneNumbers[i], Message, smsSender);
                        }
                    }

                    // ارسال اس ام اس به نمایندگان
                    if (smsSender != null && RepresentativePhoneNumbers.Count != 0)
                    {
                        for (int i = 0; i < RepresentativePhoneNumbers.Count; i++)
                        {
                            string format = "{0} عزیز \n یک پیام در پلتفرم تنیاکو در پروژه {1} برای شما بارگذاری شده است. لطفا ملاحظه فرمایید\n{2}.";
                            string Message = String.Format(format, RepresentativeFamilyNames[i], ConstructionProjectTitle, "https://my.teniaco.com/home/login");
                            sendSmsService.SendSms(RepresentativePhoneNumbers[i], Message, smsSender);
                        }
                    }
                    // پایان عملیات ارسال اس ام اس




                    
                    addToConversationLogsPVM.ConversationLogsVM.ConversationLogId = conversationLogId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToConversationLogsPVM.ConversationLogsVM;

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
        /// UpdateConversationLogs
        /// </summary>
        /// <param name="updateConversationLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("UpdateConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConversationLogs([FromBody] UpdateConversationLogsPVM updateConversationLogsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateConversationLogsPVM.ChildsUsersIds == null)
                    {
                        updateConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateConversationLogsPVM.ChildsUsersIds.Count == 0)
                        updateConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateConversationLogsPVM.ChildsUsersIds.Count == 1)
                        if (updateConversationLogsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateConversationLogsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedConversationLogsVM = projectsApiBusiness.UpdateConversationLogs(
                    updateConversationLogsPVM.ChildsUsersIds,
                    updateConversationLogsPVM.ConversationLogsVM);
                if (updatedConversationLogsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedConversationLogsVM;

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
        /// ToggleActivationConversationLogs
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns></returns>
        [HttpPost("ToggleActivationConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationConversationLogs(ToggleActivationConversationLogsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationConversationLogs(
                    toggleActivationInvestorsPVM.ConversationLogId,
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
        /// TemporaryDeleteConversationLogs
        /// </summary>
        /// <param name="temporaryDeleteConversationLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("TemporaryDeleteConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteConversationLogs(TemporaryDeleteConversationLogsPVM temporaryDeleteConversationLogsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteConversationLogs(
                    temporaryDeleteConversationLogsPVM.ConversationLogId,
                   temporaryDeleteConversationLogsPVM.UserId.Value,
                   temporaryDeleteConversationLogsPVM.ChildsUsersIds
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
        /// CompleteDeleteConversationLogs
        /// </summary>
        /// <param name="completeDeleteConversationLogsPVM"></param>
        /// <returns></returns>
        [HttpPost("CompleteDeleteConversationLogs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteConversationLogs(CompleteDeleteConversationLogsPVM completeDeleteConversationLogsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;
            try
            {
                result = projectsApiBusiness.CompleteDeleteConversationLogs(
                    completeDeleteConversationLogsPVM.ConversationLogId,
                   completeDeleteConversationLogsPVM.UserId.Value,
                   completeDeleteConversationLogsPVM.ChildsUsersIds);
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
        /// GetConversationLogWithConversationLogId
        /// </summary>
        /// <param name="getConversationLogWithConversationLogIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetConversationLogWithConversationLogId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConversationLogWithConversationLogId(
            [FromBody] GetConversationLogWithConversationLogIdPVM getConversationLogWithConversationLogIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConversationLogWithConversationLogIdPVM.ChildsUsersIds == null)
                    {
                        getConversationLogWithConversationLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConversationLogWithConversationLogIdPVM.ChildsUsersIds.Count == 0)
                        getConversationLogWithConversationLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConversationLogWithConversationLogIdPVM.ChildsUsersIds.Count == 1)
                        if (getConversationLogWithConversationLogIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConversationLogWithConversationLogIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfConversationLogs = projectsApiBusiness.GetConversationLogWithConversationLogId(
                        getConversationLogWithConversationLogIdPVM.ConversationLogId,
                        getConversationLogWithConversationLogIdPVM.UserId.Value,
                        getConversationLogWithConversationLogIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfConversationLogs;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// GetConversationDataByAgreementTypeAndRecordId
        /// </summary>
        /// <param name="getConversationDataByAgreementTypeAndRecordIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetConversationDataByAgreementTypeAndRecordId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConversationDataByAgreementTypeAndRecordId(
            [FromBody] GetConversationDataByAgreementTypeAndRecordIdPVM getConversationDataByAgreementTypeAndRecordIdPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds == null)
                    {
                        getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds.Count == 0)
                        getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds.Count == 1)
                        if (getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConversationDataByAgreementTypeAndRecordIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfConversationLogs = projectsApiBusiness.GetConversationDataByAgreementTypeAndRecordId(
                    getConversationDataByAgreementTypeAndRecordIdPVM.RecordId,
                    getConversationDataByAgreementTypeAndRecordIdPVM.ContractType,
                    getConversationDataByAgreementTypeAndRecordIdPVM.UserId.Value
                    );

                var UserIds = listOfConversationLogs.Select(c => c.UserIdCreator).Distinct().ToList();
                var Users = consoleBusiness.CmsDb.Users.Where(c => UserIds.Contains(c.UserId)).ToList();


                var type = listOfConversationLogs.Select(c => c.TableTitle).Distinct().FirstOrDefault();
             

                foreach (var item in listOfConversationLogs)
                {

                    #region Users

                    var user = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(item.UserIdCreator)).FirstOrDefault();

                    if (user != null)
                    {
                        item.CustomUsersVM = new CustomUsersVM();
                        var UserProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(user.UserId)).FirstOrDefault();

                        item.CustomUsersVM.Family = UserProfile.Family;
                        item.CustomUsersVM.Name = UserProfile.Name;
                        item.CustomUsersVM.Phone = UserProfile.Phone;
                        item.CustomUsersVM.Mobile = UserProfile.Mobile;
                        item.CustomUsersVM.UserId = user.UserId;
                        item.CustomUsersVM.Picture = UserProfile.Picture;


                    }
                    else
                    {
                        item.CustomUsersVM = new CustomUsersVM();
                        item.CustomUsersVM.Family = "تنیاکو";
                        item.CustomUsersVM.Name = "کاربر";
                    }

                    #endregion

                    #region Contracts

                    switch (type)
                    {
                        case "ContractAgreement":
                            var ContractAgreement = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = ContractAgreement.ContractAgreementTitle;
                            break;

                        case "PartnershipAgreement":
                            var PartnershipAgreement = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(c => c.PartnershipAgreementId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = PartnershipAgreement.PartnershipAgreementTitle;
                            break;

                        case "MeetingBoard":
                            var MeetingBoards = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = MeetingBoards.MeetingBoardTitle;
                            break;

                        case "ConfirmationAgreement":
                            var ConfirmationAgreements = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = ConfirmationAgreements.ConfirmationAgreementTitle;
                            break;

                        case "InitialPlan":
                            var InitialPlan = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(c => c.InitialPlanId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = InitialPlan.InitialPlanTitle;
                            break;

                        case "PitchDeck":
                            var PitchDeck = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(c => c.PitchDeckId == item.RecordId).FirstOrDefault();
                            item.RecordTitle = PitchDeck.PitchDeckTitle;
                            break;

                        case "Attachments":
                            var AttachmentFile = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(c => c.AttachementId == item.RecordId).FirstOrDefault();
                            switch (AttachmentFile.AttachementTableTitle)
                            {
                                case "ContractAgreements":
                                    var ContractAgreement2 = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = ContractAgreement2.ContractAgreementTitle;
                                    break;

                                case "PartnershipAgreements":
                                    var PartnershipAgreement2 = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(c => c.PartnershipAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = PartnershipAgreement2.PartnershipAgreementTitle;
                                    break;

                                case "MeetingBoards":
                                    var MeetingBoards2 = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = MeetingBoards2.MeetingBoardTitle;
                                    break;

                                case "ConfirmationAgreements":
                                    var ConfirmationAgreements2 = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = ConfirmationAgreements2.ConfirmationAgreementTitle;
                                    break;

                                case "InitialPlans":
                                    var InitialPlan2 = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(c => c.InitialPlanId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = InitialPlan2.InitialPlanTitle;
                                    break;

                                case "PitchDecks":
                                    var PitchDeck2 = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(c => c.PitchDeckId == AttachmentFile.AttachementParentId).FirstOrDefault();
                                    item.RecordTitle = PitchDeck2.PitchDeckTitle;
                                    break;
                            }
                            break;
                    }
                    #endregion
                }



                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConversationLogs;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {

            }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";
            return new JsonResult(jsonResultWithRecordsObjectVM);
        }

    }
}

using APIs.Core.Controllers;
using APIs.Automation.Models.Business;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Public.Models.Business;
using APIs.Projects.Models.Business;
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
using VM.PVM.Projects;
//using APIs.AttachementFiles.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Projects.Models.Entities;
using System.Collections.Generic;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// AttachementFilesManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AttachementFilesManagementController : ApiBaseController
    {

        /// <summary>
        /// AttachementFilesManagement
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
        public AttachementFilesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllAttachementFilesList
        /// </summary>
        /// <param name="getAllAttachementFilesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllAttachementFilesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllAttachementFilesList([FromBody] GetAllAttachementFilesListPVM getAllAttachementFilesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAttachementFilesListPVM.ChildsUsersIds == null)
                    {
                        getAllAttachementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllAttachementFilesListPVM.ChildsUsersIds.Count == 0)
                        getAllAttachementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllAttachementFilesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllAttachementFilesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAttachementFilesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfAttachementFiles = projectsApiBusiness.GetAllAttachementFilesList(
                    getAllAttachementFilesListPVM.ChildsUsersIds,
                   getAllAttachementFilesListPVM.AttachementParentId,
                   getAllAttachementFilesListPVM.AttachementTableTitle
                   );




                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAttachementFiles;
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
        /// GetListOfAttachementFiles
        /// </summary>
        /// <param name="getListOfAttachementFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAttachementFiles([FromBody] GetListOfAttachementFilesPVM getListOfAttachementFilesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAttachementFilesPVM.ChildsUsersIds == null)
                    {
                        getListOfAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfAttachementFilesPVM.ChildsUsersIds.Count == 0)
                        getListOfAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfAttachementFilesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAttachementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfAttachementFiles = projectsApiBusiness.GetListOfAttachementFiles(
                   getListOfAttachementFilesPVM.jtStartIndex.Value,
                   getListOfAttachementFilesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfAttachementFilesPVM.ChildsUsersIds,
                   getListOfAttachementFilesPVM.AttachementTableTitle,
                   getListOfAttachementFilesPVM.AttachementParentId,
                   getListOfAttachementFilesPVM.jtSorting
                   );

                var attachementIds = listOfAttachementFiles.Select(c => c.AttachementId).ToList();

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && attachementIds.Contains(f.RecordId) && f.TableTitle.Equals("Attachments")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && attachementIds.Contains(f.RecordId) && f.TableTitle.Equals("Attachments")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => attachementIds.Contains(c.RecordId) && c.TableTitle.Equals("Attachments") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfAttachementFilesPVM.UserId.Value)).ToList();

                #endregion


                foreach (var item in listOfAttachementFiles)
                {

                    #region IsView

                    //دیده شده
                    if (IsView.Where(c => c.RecordId.Equals(item.AttachementId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.AttachementId)).FirstOrDefault();
                        if (isView != null)
                        {
                            if (isView.FileStateId.Equals(3)) //دیده شده
                            {
                                item.IsView = true;
                            }
                            else
                            {
                                item.IsView = false;
                            }

                        }

                    }
                    #endregion

                    #region IsConfirm

                    //تایید شده
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.AttachementId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.AttachementId)).FirstOrDefault();
                        if (isConfirm != null)
                        {
                            if (isConfirm.FileStateId.Equals(4)) //تایید شده
                            {
                                item.IsConfirm = true;
                            }
                            else
                            {
                                item.IsConfirm = false;
                            }

                        }

                    }

                    #endregion

                    #region Conversation

                    //مکاتبه

                    if (converstions.Where(f => f.RecordId.Equals(item.AttachementId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.AttachementId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAttachementFiles;
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
        /// AddToAttachementFiles
        /// </summary>
        /// <param name="addToAttachementFilesPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAttachementFiles([FromBody] AddToAttachementFilesPVM addToAttachementFilesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAttachementFilesPVM.ChildsUsersIds == null)
                    {
                        addToAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToAttachementFilesPVM.ChildsUsersIds.Count == 0)
                        addToAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToAttachementFilesPVM.ChildsUsersIds.Count == 1)
                        if (addToAttachementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var attachementFileId = projectsApiBusiness.AddToAttachementFiles(addToAttachementFilesPVM.AttachementFilesVM);
                if (attachementFileId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "Attachments",
                        RecordId = attachementFileId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();


                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    var AttachmentFile = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(a => a.AttachementId == attachementFileId).FirstOrDefault();
                    //string AgreementTitle = "";
                    long ConstructionProjectId = 0;
                    switch (AttachmentFile.AttachementTableTitle)
                    {
                        case "ContractAgreements": 
                            var ContractAgreement = projectsApiBusiness.ProjectsApiDb.ContractAgreements.Where(c => c.ContractAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = ContractAgreement.ContractAgreementTitle;
                            ConstructionProjectId = ContractAgreement.ConstructionProjectId;
                            break;

                        case "PartnershipAgreements": 
                            var PartnershipAgreement = projectsApiBusiness.ProjectsApiDb.PartnershipAgreements.Where(c => c.PartnershipAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = PartnershipAgreement.PartnershipAgreementTitle;
                            ConstructionProjectId = PartnershipAgreement.ConstructionProjectId;
                            break;

                        case "MeetingBoards":
                            var MeetingBoards = projectsApiBusiness.ProjectsApiDb.MeetingBoards.Where(c => c.MeetingBoardId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = MeetingBoards.MeetingBoardTitle;
                            ConstructionProjectId = MeetingBoards.ConstructionProjectId;
                            break;

                        case "ConfirmationAgreements":
                            var ConfirmationAgreements = projectsApiBusiness.ProjectsApiDb.ConfirmationAgreements.Where(c => c.ConfirmationAgreementId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = ConfirmationAgreements.ConfirmationAgreementTitle;
                            ConstructionProjectId = ConfirmationAgreements.ConstructionProjectId;
                            break;

                        case "InitialPlans":
                            var InitialPlan = projectsApiBusiness.ProjectsApiDb.InitialPlans.Where(c => c.InitialPlanId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = InitialPlan.InitialPlanTitle;
                            ConstructionProjectId = InitialPlan.ConstructionProjectId;
                            break;

                        case "PitchDecks":
                            var PitchDeck = projectsApiBusiness.ProjectsApiDb.PitchDecks.Where(c => c.PitchDeckId == AttachmentFile.AttachementParentId).FirstOrDefault();
                            //AgreementTitle = PitchDeck.PitchDeckTitle;
                            ConstructionProjectId = PitchDeck.ConstructionProjectId;
                            break;
                    }

                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == ConstructionProjectId && c.OwnerUserId != addToAttachementFilesPVM.AttachementFilesVM.UserIdCreator)
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
                            string format = "{0} عزیز \n یک فایل در پلتفرم تنیاکو در پروژه {1} برای شما بارگذاری شده است. لطفا ملاحظه و تایید فرمایید\n{2}.";
                            string Message = String.Format(format, OwnerFamilyNames[i], ConstructionProjectTitle, "https://my.teniaco.com/home/login");
                            sendSmsService.SendSms(OwnerPhoneNumbers[i], Message, smsSender);
                        }
                    }

                    // ارسال اس ام اس به نمایندگان
                    if (smsSender != null && RepresentativePhoneNumbers.Count != 0)
                    {
                        for (int i = 0; i < RepresentativePhoneNumbers.Count; i++)
                        {
                            string format = "{0} عزیز \n یک فایل در پلتفرم تنیاکو در پروژه {1} برای شما بارگذاری شده است. لطفا ملاحظه و تایید فرمایید\n{2}.";
                            string Message = String.Format(format, RepresentativeFamilyNames[i], ConstructionProjectTitle, "https://my.teniaco.com/home/login");
                            sendSmsService.SendSms(RepresentativePhoneNumbers[i], Message, smsSender);
                        }
                    }
                    // پایان عملیات ارسال اس ام اس


                    addToAttachementFilesPVM.AttachementFilesVM.AttachementId = attachementFileId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAttachementFilesPVM.AttachementFilesVM;

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
        /// UpdateAttachementFiles
        /// </summary>
        /// <param name="updateAttachementFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateAttachementFiles([FromBody] UpdateAttachementFilesPVM updateAttachementFilesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateAttachementFilesPVM.ChildsUsersIds == null)
                    {
                        updateAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateAttachementFilesPVM.ChildsUsersIds.Count == 0)
                        updateAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateAttachementFilesPVM.ChildsUsersIds.Count == 1)
                        if (updateAttachementFilesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateAttachementFilesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var attachementFilesVM = projectsApiBusiness.UpdateAttachementFiles(
                    updateAttachementFilesPVM.ChildsUsersIds,
                    updateAttachementFilesPVM.AttachementFilesVM);




                var attachementId = attachementFilesVM.AttachementId;

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => attachementId.Equals(c.RecordId) && c.TableTitle.Equals("Attachments") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updateAttachementFilesPVM.UserId.Value)).ToList();

                attachementFilesVM.ConversationIsReadCount = converstions.Count;
                #endregion




                if (attachementFilesVM != null)
                {
                    //updateAttachementFilesPVM.AttachementFilesVM.AttachementId = attachementFilesId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = attachementFilesVM;

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
        /// ToggleActivationAttachementFiles
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("ToggleActivationAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationAttachementFiles(ToggleActivationAttachementFilesPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationAttachementFiles(
                    toggleActivationInvestorsPVM.AttachementFileId,
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
        /// TemporaryDeleteAttachementFiles
        /// </summary>
        /// <param name="temporaryDeleteAttachementFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("TemporaryDeleteAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteAttachementFiles(TemporaryDeleteAttachementFilesPVM temporaryDeleteAttachementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteAttachementFiles(
                    temporaryDeleteAttachementFilesPVM.AttachementFileId,
                   temporaryDeleteAttachementFilesPVM.UserId.Value,
                   temporaryDeleteAttachementFilesPVM.ChildsUsersIds
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
        /// CompleteDeleteAttachementFiles
        /// </summary>
        /// <param name="completeDeleteAttachementFilesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("CompleteDeleteAttachementFiles")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteAttachementFiles(CompleteDeleteAttachementFilesPVM completeDeleteAttachementFilesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.CompleteDeleteAttachementFiles(
                    completeDeleteAttachementFilesPVM.AttachementFilesId,
                   completeDeleteAttachementFilesPVM.UserId.Value,
                   completeDeleteAttachementFilesPVM.ChildsUsersIds
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
        /// GetAttachementFilesWithAttachementFileId
        /// </summary>
        /// <param name="getAttachementFilesWithAttachementFileIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAttachementFilesWithAttachementFileId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAttachementFilesWithAttachementFileId(
            [FromBody] GetAttachementFilesWithAttachementFileIdPVM getAttachementFilesWithAttachementFileIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds == null)
                    {
                        getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds.Count == 0)
                        getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds.Count == 1)
                        if (getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfAttachementFiles = projectsApiBusiness.GetAttachementFilesWithAttachementFileId(
                        getAttachementFilesWithAttachementFileIdPVM.AttachementFileId,
                        getAttachementFilesWithAttachementFileIdPVM.UserId.Value,
                        getAttachementFilesWithAttachementFileIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfAttachementFiles;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {
                //
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// GetAttachementsByAgreementIdAndTableTitle
        /// </summary>
        /// <param name="getAttachementsByAgreementIdAndTableTitlePVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAttachementsByAgreementIdAndTableTitle")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAttachementsByAgreementIdAndTableTitle(
            [FromBody] GetAttachementsByAgreementIdAndTableTitlePVM getAttachementsByAgreementIdAndTableTitlePVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds == null)
                    {
                        getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds.Count == 0)
                        getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds.Count == 1)
                        if (getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAttachementsByAgreementIdAndTableTitlePVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfAttachementFiles = projectsApiBusiness.GetAttachementsByAgreementIdAndTableTitle(
                       getAttachementsByAgreementIdAndTableTitlePVM.AgreeemntId,
                       getAttachementsByAgreementIdAndTableTitlePVM.TableTitle,
                       getAttachementsByAgreementIdAndTableTitlePVM.UserId.Value
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAttachementFiles;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }


            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }
    }
}

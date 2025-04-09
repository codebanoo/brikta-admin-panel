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
//using APIs.ConfirmationAgreements.Models.Business;
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
    /// ConfirmationAgreementsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ConfirmationAgreementsManagementController : ApiBaseController
    {

        /// <summary>
        /// ConfirmationAgreementsManagement
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
        public ConfirmationAgreementsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllConfirmationAgreementsList
        /// </summary>
        /// <param name="getListOfConfirmationAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllConfirmationAgreementsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllConfirmationAgreementsList([FromBody] GetAllConfirmationAgreementsListPVM getAllConfirmationAgreementsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllConfirmationAgreementsListPVM.ChildsUsersIds == null)
                    {
                        getAllConfirmationAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllConfirmationAgreementsListPVM.ChildsUsersIds.Count == 0)
                        getAllConfirmationAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllConfirmationAgreementsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllConfirmationAgreementsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllConfirmationAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConfirmationAgreements = projectsApiBusiness.GetAllConfirmationAgreementsList(
                    getAllConfirmationAgreementsListPVM.ChildsUsersIds,
                   getAllConfirmationAgreementsListPVM.ConfirmationAgreementTitle,
                   getAllConfirmationAgreementsListPVM.ConstructionProjectId
                   );

                var confirmationTypeIds = listOfConfirmationAgreements.Select(p => p.ConfirmationTypeId).ToList();
                var confirmationTypes = projectsApiBusiness.ProjectsApiDb.ConfirmationTypes.Where(p => confirmationTypeIds.Contains(p.ConfirmationTypeId)).ToList();
                //var personIds = listOfConfirmationAgreements.Select(p => p.UserId).ToList();
                //var persons = publicApiBusiness.PublicApiDb.Persons.Where(p => personIds.Contains(p.PersonId)).ToList();

                foreach (var confirmationAgreementsVM in listOfConfirmationAgreements)
                {
                    //if (persons.Where(p => p.PersonId == confirmationAgreementsVM.UserId).Any())
                    //{
                    //    var person = persons.Where(p => p.PersonId == confirmationAgreementsVM.UserId).FirstOrDefault();

                    //    confirmationAgreementsVM.PersonsVM = new PersonsVM();
                    //    confirmationAgreementsVM.PersonsVM.PersonId = person.PersonId;
                    //    confirmationAgreementsVM.PersonsVM.Name = person.Name;
                    //    confirmationAgreementsVM.PersonsVM.Family = person.Family;
                    //    //constructionProject.PersonsVM.Phone = person.Phone;
                    //}
                    if (confirmationTypes.Where(p => p.ConfirmationTypeId == confirmationAgreementsVM.ConfirmationTypeId).Any())
                    {
                        var confirmationType = confirmationTypes.Where(p => p.ConfirmationTypeId == confirmationAgreementsVM.ConfirmationTypeId).FirstOrDefault();

                        confirmationAgreementsVM.ConfirmationTypesVM = new ConfirmationTypesVM
                        {
                            ConfirmationTypeId = confirmationType.ConfirmationTypeId,
                            ConfirmationTypeTitle = confirmationType.ConfirmationTypeTitle
                        };
                    }

                }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConfirmationAgreements;
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
        /// GetListOfConfirmationAgreements
        /// </summary>
        /// <param name="getListOfConfirmationAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfConfirmationAgreements([FromBody] GetListOfConfirmationAgreementsPVM getListOfConfirmationAgreementsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfConfirmationAgreementsPVM.ChildsUsersIds == null)
                    {
                        getListOfConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfConfirmationAgreementsPVM.ChildsUsersIds.Count == 0)
                        getListOfConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfConfirmationAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfConfirmationAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfConfirmationAgreements = projectsApiBusiness.GetListOfConfirmationAgreements(
                   getListOfConfirmationAgreementsPVM.jtStartIndex.Value,
                   getListOfConfirmationAgreementsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfConfirmationAgreementsPVM.ChildsUsersIds,
                   getListOfConfirmationAgreementsPVM.ConfirmationAgreementTitle,
                   getListOfConfirmationAgreementsPVM.ConstructionProjectId,
                   getListOfConfirmationAgreementsPVM.jtSorting);

                var confirmationTypeIds = listOfConfirmationAgreements.Select(p => p.ConfirmationTypeId).ToList();
                var confirmationTypes = projectsApiBusiness.ProjectsApiDb.ConfirmationTypes.Where(p => confirmationTypeIds.Contains(p.ConfirmationTypeId)).ToList();


                var confirmationAgreementIds = listOfConfirmationAgreements.Select(c => c.ConfirmationAgreementId).ToList();



                #region Load the count of attachements

                var attachementsOfMeetingBoards = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => confirmationAgreementIds.Contains(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("ConfirmationAgreements")).ToList();

                #endregion

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && confirmationAgreementIds.Contains(f.RecordId) && f.TableTitle.Equals("ConfirmationAgreement")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && confirmationAgreementIds.Contains(f.RecordId) && f.TableTitle.Equals("ConfirmationAgreement")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => confirmationAgreementIds.Contains(c.RecordId) && c.TableTitle.Equals("ConfirmationAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfConfirmationAgreementsPVM.UserId.Value)).ToList();

                #endregion


                foreach (var item in listOfConfirmationAgreements)
                {


                    #region ConfirmationTypes

                    if (confirmationTypes.Where(p => p.ConfirmationTypeId == item.ConfirmationTypeId).Any())
                    {
                        var confirmationType = confirmationTypes.Where(p => p.ConfirmationTypeId == item.ConfirmationTypeId).FirstOrDefault();

                        item.ConfirmationTypesVM = new ConfirmationTypesVM
                        {
                            ConfirmationTypeId = confirmationType.ConfirmationTypeId,
                            ConfirmationTypeTitle = confirmationType.ConfirmationTypeTitle
                        };
                    }
                    #endregion


                    #region count of attachements

                    if (attachementsOfMeetingBoards.Where(c => c.AttachementParentId.Equals(item.ConfirmationAgreementId) && c.AttachementTableTitle.Equals("ConfirmationAgreements")).Any())
                    {
                        var attachementCount = attachementsOfMeetingBoards.Where(c => c.AttachementParentId.Equals(item.ConfirmationAgreementId) && c.AttachementTableTitle.Equals("ConfirmationAgreements")).Count();

                        if (attachementCount != null)
                        {

                            if (attachementCount > 0)
                            {
                                item.AttachementsCount = attachementCount;
                            }
                            else
                            {
                                item.AttachementsCount = 0;
                            }
                        }
                        else
                        {
                            item.AttachementsCount = 0;
                        }


                    }
                    else
                    {
                        item.AttachementsCount = 0;
                    }

                    #endregion


                    #region IsView

                    //دیده شده
                    if (IsView.Where(c => c.RecordId.Equals(item.ConfirmationAgreementId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.ConfirmationAgreementId)).FirstOrDefault();
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
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.ConfirmationAgreementId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.ConfirmationAgreementId)).FirstOrDefault();
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

                    if (converstions.Where(f => f.RecordId.Equals(item.ConfirmationAgreementId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.ConfirmationAgreementId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfConfirmationAgreements;
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
        /// AddToConfirmationAgreements
        /// </summary>
        /// <param name="addToConfirmationAgreementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToConfirmationAgreements([FromBody] AddToConfirmationAgreementsPVM addToConfirmationAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToConfirmationAgreementsPVM.ChildsUsersIds == null)
                    {
                        addToConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToConfirmationAgreementsPVM.ChildsUsersIds.Count == 0)
                        addToConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToConfirmationAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (addToConfirmationAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var confirmationAgreementId = projectsApiBusiness.AddToConfirmationAgreements(addToConfirmationAgreementsPVM.ConfirmationAgreementsVM);
                if (confirmationAgreementId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "ConfirmationAgreement",
                        RecordId = confirmationAgreementId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();



                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConstructionProjectId && c.OwnerUserId != addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConstructionProjectId)
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


                    addToConfirmationAgreementsPVM.ConfirmationAgreementsVM.ConfirmationAgreementId = confirmationAgreementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToConfirmationAgreementsPVM.ConfirmationAgreementsVM;

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


        [HttpPost("UpdateConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateConfirmationAgreements([FromBody] UpdateConfirmationAgreementsPVM updateConfirmationAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateConfirmationAgreementsPVM.ChildsUsersIds == null)
                    {
                        updateConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateConfirmationAgreementsPVM.ChildsUsersIds.Count == 0)
                        updateConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateConfirmationAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (updateConfirmationAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateConfirmationAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedConfirmationAgreementsVM = projectsApiBusiness.UpdateConfirmationAgreements(
                    updateConfirmationAgreementsPVM.ChildsUsersIds,
                    updateConfirmationAgreementsPVM.ConfirmationAgreementsVM);

                var confirmationAgreementId = updatedConfirmationAgreementsVM.ConfirmationAgreementId;

                #region Load the count of attachements

                var attachementsOfMeetingBoards = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => confirmationAgreementId.Equals(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("ConfirmationAgreements")).ToList();

                updatedConfirmationAgreementsVM.AttachementsCount = attachementsOfMeetingBoards.Count;
                #endregion


                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => confirmationAgreementId.Equals(c.RecordId) && c.TableTitle.Equals("ConfirmationAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updateConfirmationAgreementsPVM.UserId.Value)).ToList();

                updatedConfirmationAgreementsVM.ConversationIsReadCount = converstions.Count;
                #endregion

                if (updatedConfirmationAgreementsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedConfirmationAgreementsVM;

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

        [HttpPost("ToggleActivationConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationConfirmationAgreements(ToggleActivationConfirmationAgreementsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationConfirmationAgreements(
                    toggleActivationInvestorsPVM.ConfirmationAgreementId,
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


        [HttpPost("TemporaryDeleteConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteConfirmationAgreements(TemporaryDeleteConfirmationAgreementsPVM temporaryDeleteConfirmationAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteConfirmationAgreements(
                    temporaryDeleteConfirmationAgreementsPVM.ConfirmationAgreementId,
                   temporaryDeleteConfirmationAgreementsPVM.UserId.Value,
                   temporaryDeleteConfirmationAgreementsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteConfirmationAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteConfirmationAgreements(CompleteDeleteConfirmationAgreementsPVM completeDeleteConfirmationAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;


            try
            {
                result = projectsApiBusiness.CompleteDeleteConfirmationAgreements(
                    completeDeleteConfirmationAgreementsPVM.ConfirmationAgreementId,
                   completeDeleteConfirmationAgreementsPVM.UserId.Value,
                   completeDeleteConfirmationAgreementsPVM.ChildsUsersIds
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
        /// GetConfirmationAgreementsWithConfirmationAgreementId
        /// </summary>
        /// <param name="getConfirmationAgreementsWithConfirmationAgreementIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetConfirmationAgreementsWithConfirmationAgreementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetConfirmationAgreementsWithConfirmationAgreementId(
            [FromBody] GetConfirmationAgreementsWithConfirmationAgreementIdPVM getConfirmationAgreementsWithConfirmationAgreementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds == null)
                    {
                        getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds.Count == 0)
                        getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds.Count == 1)
                        if (getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfConfirmationAgreements = projectsApiBusiness.GetConfirmationAgreementsWithConfirmationAgreementId(
                        getConfirmationAgreementsWithConfirmationAgreementIdPVM.ConfirmationAgreementId,
                        getConfirmationAgreementsWithConfirmationAgreementIdPVM.UserId.Value,
                        getConfirmationAgreementsWithConfirmationAgreementIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfConfirmationAgreements;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {
                
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        //[HttpPost("ConfirmConfirmationAgreements")]
        //[ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        //public IActionResult ConfirmConfirmationAgreements(ConfirmConfirmationAgreementsPVM confirmConfirmationAgreementsPVM)
        //{
        //    JsonResultObjectVM jsonResultObjectVM =
        //        new JsonResultObjectVM(new object() { });

        //    try
        //    {

        //        if (projectsApiBusiness.ConfirmConfirmationAgreements(
        //            confirmConfirmationAgreementsPVM.ConfirmationAgreementId,
        //           confirmConfirmationAgreementsPVM.UserId.Value,
        //           confirmConfirmationAgreementsPVM.ChildsUsersIds
        //            ))
        //        {
        //            jsonResultObjectVM.Result = "OK";
        //            jsonResultObjectVM.Message = "Success";

        //            return new JsonResult(jsonResultObjectVM);
        //        }
        //    }
        //    catch (Exception exc)
        //    { }

        //    jsonResultObjectVM.Result = "ERROR";
        //    jsonResultObjectVM.Message = "ErrorInService";

        //    return new JsonResult(jsonResultObjectVM);
        //}
    }
}

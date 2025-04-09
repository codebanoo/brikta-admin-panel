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
//using APIs.PitchDecks.Models.Business;
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
    /// PitchDecksManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PitchDecksManagementController : ApiBaseController
    {

        /// <summary>
        /// PitchDecksManagement
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
        public PitchDecksManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllPitchDecksList
        /// </summary>
        /// <param name="getListOfPitchDecksPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPitchDecksList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPitchDecksList([FromBody] GetAllPitchDecksListPVM getAllPitchDecksListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPitchDecksListPVM.ChildsUsersIds == null)
                    {
                        getAllPitchDecksListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllPitchDecksListPVM.ChildsUsersIds.Count == 0)
                        getAllPitchDecksListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllPitchDecksListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPitchDecksListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPitchDecksListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPitchDecks = projectsApiBusiness.GetAllPitchDecksList(
                    getAllPitchDecksListPVM.ChildsUsersIds,
                   getAllPitchDecksListPVM.PitchDeckTitle,
                   getAllPitchDecksListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPitchDecks;
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
        /// GetListOfPitchDecks
        /// </summary>
        /// <param name="getListOfPitchDecksPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfPitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPitchDecks([FromBody] GetListOfPitchDecksPVM getListOfPitchDecksPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPitchDecksPVM.ChildsUsersIds == null)
                    {
                        getListOfPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfPitchDecksPVM.ChildsUsersIds.Count == 0)
                        getListOfPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfPitchDecksPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPitchDecksPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPitchDecks = projectsApiBusiness.GetListOfPitchDecks(
                   getListOfPitchDecksPVM.jtStartIndex.Value,
                   getListOfPitchDecksPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfPitchDecksPVM.ChildsUsersIds,
                   getListOfPitchDecksPVM.PitchDeckTitle,
                   getListOfPitchDecksPVM.ConstructionProjectId,
                   getListOfPitchDecksPVM.jtSorting);




                var pitchDeckIds = listOfPitchDecks.Select(f => f.PitchDeckId).ToList();

                #region Load the count of attachements

                var attachementsOfPitchDecks = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => pitchDeckIds.Contains(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("PitchDecks")).ToList();

                #endregion

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && pitchDeckIds.Contains(f.RecordId) && f.TableTitle.Equals("PitchDeck")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && pitchDeckIds.Contains(f.RecordId) && f.TableTitle.Equals("PitchDeck")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => pitchDeckIds.Contains(c.RecordId) && c.TableTitle.Equals("PitchDeck") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfPitchDecksPVM.UserId.Value)).ToList();

                #endregion


                foreach (var item in listOfPitchDecks)
                {
                    #region count of attachements

                    if (attachementsOfPitchDecks.Where(c => c.AttachementParentId.Equals(item.PitchDeckId) && c.AttachementTableTitle.Equals("PitchDecks")).Any())
                    {
                        var attachementCount = attachementsOfPitchDecks.Where(c => c.AttachementParentId.Equals(item.PitchDeckId) && c.AttachementTableTitle.Equals("PitchDecks")).Count();

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
                    if (IsView.Where(c => c.RecordId.Equals(item.PitchDeckId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.PitchDeckId)).FirstOrDefault();
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
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.PitchDeckId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.PitchDeckId)).FirstOrDefault();
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

                    if (converstions.Where(f => f.RecordId.Equals(item.PitchDeckId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.PitchDeckId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPitchDecks;
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
        /// AddToPitchDecks
        /// </summary>
        /// <param name="addToPitchDecksPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToPitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToPitchDecks([FromBody] AddToPitchDecksPVM addToPitchDecksPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToPitchDecksPVM.ChildsUsersIds == null)
                    {
                        addToPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToPitchDecksPVM.ChildsUsersIds.Count == 0)
                        addToPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToPitchDecksPVM.ChildsUsersIds.Count == 1)
                        if (addToPitchDecksPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToPitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var pitchDeckId = projectsApiBusiness.AddToPitchDecks(addToPitchDecksPVM.PitchDecksVM);
                if (pitchDeckId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "PitchDeck",
                        RecordId = pitchDeckId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();



                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToPitchDecksPVM.PitchDecksVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToPitchDecksPVM.PitchDecksVM.ConstructionProjectId && c.OwnerUserId != addToPitchDecksPVM.PitchDecksVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToPitchDecksPVM.PitchDecksVM.ConstructionProjectId)
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


                    addToPitchDecksPVM.PitchDecksVM.PitchDeckId = pitchDeckId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToPitchDecksPVM.PitchDecksVM;

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


        [HttpPost("UpdatePitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdatePitchDecks([FromBody] UpdatePitchDecksPVM updatePitchDecksPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updatePitchDecksPVM.ChildsUsersIds == null)
                    {
                        updatePitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updatePitchDecksPVM.ChildsUsersIds.Count == 0)
                        updatePitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updatePitchDecksPVM.ChildsUsersIds.Count == 1)
                        if (updatePitchDecksPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updatePitchDecksPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedPitchDecksVM = projectsApiBusiness.UpdatePitchDecks(
                    updatePitchDecksPVM.ChildsUsersIds,
                    updatePitchDecksPVM.PitchDecksVM);


                var pitchDeckId = updatedPitchDecksVM.PitchDeckId;

                #region Load the count of attachements

                var attachementsOfPitchDecks = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => pitchDeckId.Equals(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("PitchDecks")).ToList();

                updatedPitchDecksVM.AttachementsCount = attachementsOfPitchDecks.Count;
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => pitchDeckId.Equals(c.RecordId) && c.TableTitle.Equals("PitchDeck") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updatePitchDecksPVM.UserId.Value)).ToList();

                updatedPitchDecksVM.ConversationIsReadCount = converstions.Count;
                #endregion

                if (updatedPitchDecksVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedPitchDecksVM;

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

        [HttpPost("ToggleActivationPitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationPitchDecks(ToggleActivationPitchDecksPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationPitchDecks(
                    toggleActivationInvestorsPVM.PitchDeckId,
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


        [HttpPost("TemporaryDeletePitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePitchDecks(TemporaryDeletePitchDecksPVM temporaryDeletePitchDecksPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeletePitchDecks(
                    temporaryDeletePitchDecksPVM.PitchDeckId,
                   temporaryDeletePitchDecksPVM.UserId.Value,
                   temporaryDeletePitchDecksPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeletePitchDecks")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeletePitchDecks(CompleteDeletePitchDecksPVM completeDeletePitchDecksPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;
            try
            {
                result = projectsApiBusiness.CompleteDeletePitchDecks(
                    completeDeletePitchDecksPVM.PitchDeckId,
                   completeDeletePitchDecksPVM.UserId.Value,
                   completeDeletePitchDecksPVM.ChildsUsersIds);
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
        /// GetPitchDecksWithPitchDeckId
        /// </summary>
        /// <param name="getPitchDecksWithPitchDeckIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetPitchDecksWithPitchDeckId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPitchDecksWithPitchDeckId(
            [FromBody] GetPitchDecksWithPitchDeckIdPVM getPitchDecksWithPitchDeckIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds == null)
                    {
                        getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds.Count == 0)
                        getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds.Count == 1)
                        if (getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfPitchDecks = projectsApiBusiness.GetPitchDecksWithPitchDeckId(
                        getPitchDecksWithPitchDeckIdPVM.PitchDeckId,
                        getPitchDecksWithPitchDeckIdPVM.UserId.Value,
                        getPitchDecksWithPitchDeckIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfPitchDecks;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {
                
            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


    }
}

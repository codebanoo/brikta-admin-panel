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
//using APIs.InitialPlans.Models.Business;
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
    /// MeetingBoardsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class MeetingBoardsManagementController : ApiBaseController
    {

        /// <summary>
        /// MeetingBoardsManagement
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
        public MeetingBoardsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllMeetingBoardsList
        /// </summary>
        /// <param name="getListOfMeetingBoardsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllMeetingBoardsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllMeetingBoardsList([FromBody] GetAllMeetingBoardsListPVM getAllMeetingBoardsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllMeetingBoardsListPVM.ChildsUsersIds == null)
                    {
                        getAllMeetingBoardsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllMeetingBoardsListPVM.ChildsUsersIds.Count == 0)
                        getAllMeetingBoardsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllMeetingBoardsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllMeetingBoardsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllMeetingBoardsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfMeetingBoards = projectsApiBusiness.GetAllMeetingBoardsList(
                    getAllMeetingBoardsListPVM.ChildsUsersIds,
                   getAllMeetingBoardsListPVM.MeetingBoardTitle,
                   getAllMeetingBoardsListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMeetingBoards;
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
        /// GetListOfMeetingBoards
        /// </summary>
        /// <param name="getListOfMeetingBoardsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfMeetingBoards([FromBody] GetListOfMeetingBoardsPVM getListOfMeetingBoardsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfMeetingBoardsPVM.ChildsUsersIds == null)
                    {
                        getListOfMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfMeetingBoardsPVM.ChildsUsersIds.Count == 0)
                        getListOfMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfMeetingBoardsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfMeetingBoardsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfMeetingBoards = projectsApiBusiness.GetListOfMeetingBoards(
                   getListOfMeetingBoardsPVM.jtStartIndex.Value,
                   getListOfMeetingBoardsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfMeetingBoardsPVM.ChildsUsersIds,
                   getListOfMeetingBoardsPVM.MeetingBoardTitle,
                   getListOfMeetingBoardsPVM.ConstructionProjectId,
                   getListOfMeetingBoardsPVM.jtSorting);



                var meetingBoardIds = listOfMeetingBoards.Select(f => f.MeetingBoardId).ToList();

                #region Load the count of attachements

                var attachementsOfMeetingBoards = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => meetingBoardIds.Contains(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("MeetingBoards")).ToList();

                #endregion

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && meetingBoardIds.Contains(f.RecordId) && f.TableTitle.Equals("MeetingBoard")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && meetingBoardIds.Contains(f.RecordId) && f.TableTitle.Equals("MeetingBoard")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => meetingBoardIds.Contains(c.RecordId) && c.TableTitle.Equals("MeetingBoard") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfMeetingBoardsPVM.UserId.Value)).ToList();

                #endregion


                foreach (var item in listOfMeetingBoards)
                {
                    #region count of attachements

                    if (attachementsOfMeetingBoards.Where(c => c.AttachementParentId.Equals(item.MeetingBoardId) && c.AttachementTableTitle.Equals("MeetingBoards")).Any())
                    {
                        var attachementCount = attachementsOfMeetingBoards.Where(c => c.AttachementParentId.Equals(item.MeetingBoardId) && c.AttachementTableTitle.Equals("MeetingBoards")).Count();

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
                    if (IsView.Where(c => c.RecordId.Equals(item.MeetingBoardId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.MeetingBoardId)).FirstOrDefault();
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
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.MeetingBoardId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.MeetingBoardId)).FirstOrDefault();
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

                    if (converstions.Where(f => f.RecordId.Equals(item.MeetingBoardId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.MeetingBoardId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfMeetingBoards;
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
        /// AddToMeetingBoards
        /// </summary>
        /// <param name="addToMeetingBoardsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToMeetingBoards([FromBody] AddToMeetingBoardsPVM addToMeetingBoardsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToMeetingBoardsPVM.ChildsUsersIds == null)
                    {
                        addToMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToMeetingBoardsPVM.ChildsUsersIds.Count == 0)
                        addToMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToMeetingBoardsPVM.ChildsUsersIds.Count == 1)
                        if (addToMeetingBoardsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var meetingBoardId = projectsApiBusiness.AddToMeetingBoards(addToMeetingBoardsPVM.MeetingBoardsVM);
                if (meetingBoardId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "MeetingBoard",
                        RecordId = meetingBoardId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();



                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToMeetingBoardsPVM.MeetingBoardsVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToMeetingBoardsPVM.MeetingBoardsVM.ConstructionProjectId && c.OwnerUserId != addToMeetingBoardsPVM.MeetingBoardsVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToMeetingBoardsPVM.MeetingBoardsVM.ConstructionProjectId)
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



                    addToMeetingBoardsPVM.MeetingBoardsVM.MeetingBoardId = meetingBoardId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToMeetingBoardsPVM.MeetingBoardsVM;

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


        [HttpPost("UpdateMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateMeetingBoards([FromBody] UpdateMeetingBoardsPVM updateMeetingBoardsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateMeetingBoardsPVM.ChildsUsersIds == null)
                    {
                        updateMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateMeetingBoardsPVM.ChildsUsersIds.Count == 0)
                        updateMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateMeetingBoardsPVM.ChildsUsersIds.Count == 1)
                        if (updateMeetingBoardsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateMeetingBoardsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedMeetingBoardsVM = projectsApiBusiness.UpdateMeetingBoards(
                    updateMeetingBoardsPVM.ChildsUsersIds,
                    updateMeetingBoardsPVM.MeetingBoardsVM);


                var meetingBoardId = updatedMeetingBoardsVM.MeetingBoardId;

                #region Load the count of attachements

                var attachementsOfMeetingBoards = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => meetingBoardId.Equals(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("MeetingBoards")).ToList();

                updatedMeetingBoardsVM.AttachementsCount = attachementsOfMeetingBoards.Count;
                #endregion


                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => meetingBoardId.Equals(c.RecordId) && c.TableTitle.Equals("MeetingBoard") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updateMeetingBoardsPVM.UserId.Value)).ToList();

                updatedMeetingBoardsVM.ConversationIsReadCount = converstions.Count;
                #endregion


                if (updatedMeetingBoardsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedMeetingBoardsVM;

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

        [HttpPost("ToggleActivationMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationMeetingBoards(ToggleActivationMeetingBoardsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationMeetingBoards(
                    toggleActivationInvestorsPVM.MeetingBoardId,
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


        [HttpPost("TemporaryDeleteMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteMeetingBoards(TemporaryDeleteMeetingBoardsPVM temporaryDeleteMeetingBoardsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteMeetingBoards(
                    temporaryDeleteMeetingBoardsPVM.MeetingBoardId,
                   temporaryDeleteMeetingBoardsPVM.UserId.Value,
                   temporaryDeleteMeetingBoardsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteMeetingBoards")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteMeetingBoards(CompleteDeleteMeetingBoardsPVM completeDeleteMeetingBoardsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteMeetingBoards(
                    completeDeleteMeetingBoardsPVM.MeetingBoardId,
                   completeDeleteMeetingBoardsPVM.UserId.Value,
                   completeDeleteMeetingBoardsPVM.ChildsUsersIds);
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
        /// GetMeetingBoardsWithMeetingBoardId
        /// </summary>
        /// <param name="getMeetingBoardsWithMeetingBoardIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetMeetingBoardsWithMeetingBoardId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetMeetingBoardsWithMeetingBoardId(
            [FromBody] GetMeetingBoardsWithMeetingBoardIdPVM getMeetingBoardsWithMeetingBoardIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds == null)
                    {
                        getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds.Count == 0)
                        getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds.Count == 1)
                        if (getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfMeetingBoards = projectsApiBusiness.GetMeetingBoardsWithMeetingBoardId(
                        getMeetingBoardsWithMeetingBoardIdPVM.MeetingBoardId,
                        getMeetingBoardsWithMeetingBoardIdPVM.UserId.Value,
                        getMeetingBoardsWithMeetingBoardIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfMeetingBoards;

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

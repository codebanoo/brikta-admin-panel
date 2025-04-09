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
    /// InitialPlansManagement
    /// </summary>
    [CustomApiAuthentication]
    public class InitialPlansManagementController : ApiBaseController
    {

        /// <summary>
        /// InitialPlansManagement
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
        public InitialPlansManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllInitialPlansList
        /// </summary>
        /// <param name="getListOfInitialPlansPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllInitialPlansList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllInitialPlansList([FromBody] GetAllInitialPlansListPVM getAllInitialPlansListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllInitialPlansListPVM.ChildsUsersIds == null)
                    {
                        getAllInitialPlansListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllInitialPlansListPVM.ChildsUsersIds.Count == 0)
                        getAllInitialPlansListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllInitialPlansListPVM.ChildsUsersIds.Count == 1)
                        if (getAllInitialPlansListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllInitialPlansListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfInitialPlans = projectsApiBusiness.GetAllInitialPlansList(
                    getAllInitialPlansListPVM.ChildsUsersIds,
                   getAllInitialPlansListPVM.InitialPlanTitle,
                   getAllInitialPlansListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfInitialPlans;
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
        /// GetListOfInitialPlans
        /// </summary>
        /// <param name="getListOfInitialPlansPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfInitialPlans([FromBody] GetListOfInitialPlansPVM getListOfInitialPlansPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfInitialPlansPVM.ChildsUsersIds == null)
                    {
                        getListOfInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfInitialPlansPVM.ChildsUsersIds.Count == 0)
                        getListOfInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfInitialPlansPVM.ChildsUsersIds.Count == 1)
                        if (getListOfInitialPlansPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfInitialPlans = projectsApiBusiness.GetListOfInitialPlans(
                   getListOfInitialPlansPVM.jtStartIndex.Value,
                   getListOfInitialPlansPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfInitialPlansPVM.ChildsUsersIds,
                   getListOfInitialPlansPVM.InitialPlanTitle,
                   getListOfInitialPlansPVM.ConstructionProjectId,
                   getListOfInitialPlansPVM.jtSorting
                   );



                var initialPlanIds = listOfInitialPlans.Select(f => f.InitialPlanId).ToList();

                #region Load the count of attachements

                var attachementsOfInitialPlans = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => initialPlanIds.Contains(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("InitialPlans")).ToList();

                #endregion

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && initialPlanIds.Contains(f.RecordId) && f.TableTitle.Equals("InitialPlan")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && initialPlanIds.Contains(f.RecordId) && f.TableTitle.Equals("InitialPlan")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => initialPlanIds.Contains(c.RecordId) && c.TableTitle.Equals("InitialPlan") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfInitialPlansPVM.UserId.Value)).ToList();

                #endregion


                foreach (var item in listOfInitialPlans)
                {
                    #region count of attachements

                    if (attachementsOfInitialPlans.Where(c => c.AttachementParentId.Equals(item.InitialPlanId) && c.AttachementTableTitle.Equals("InitialPlans")).Any())
                    {
                        var attachementCount = attachementsOfInitialPlans.Where(c => c.AttachementParentId.Equals(item.InitialPlanId) && c.AttachementTableTitle.Equals("InitialPlans")).Count();

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
                    if (IsView.Where(c => c.RecordId.Equals(item.InitialPlanId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.InitialPlanId)).FirstOrDefault();
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
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.InitialPlanId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.InitialPlanId)).FirstOrDefault();
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

                    if (converstions.Where(f => f.RecordId.Equals(item.InitialPlanId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.InitialPlanId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfInitialPlans;
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
        /// AddToInitialPlans
        /// </summary>
        /// <param name="addToInitialPlansPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToInitialPlans([FromBody] AddToInitialPlansPVM addToInitialPlansPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToInitialPlansPVM.ChildsUsersIds == null)
                    {
                        addToInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToInitialPlansPVM.ChildsUsersIds.Count == 0)
                        addToInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToInitialPlansPVM.ChildsUsersIds.Count == 1)
                        if (addToInitialPlansPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var initialPlanId = projectsApiBusiness.AddToInitialPlans(addToInitialPlansPVM.InitialPlansVM);
                if (initialPlanId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "InitialPlan",
                        RecordId = initialPlanId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();



                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToInitialPlansPVM.InitialPlansVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToInitialPlansPVM.InitialPlansVM.ConstructionProjectId && c.OwnerUserId != addToInitialPlansPVM.InitialPlansVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToInitialPlansPVM.InitialPlansVM.ConstructionProjectId)
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


                    addToInitialPlansPVM.InitialPlansVM.InitialPlanId = initialPlanId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToInitialPlansPVM.InitialPlansVM;

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


        [HttpPost("UpdateInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateInitialPlans([FromBody] UpdateInitialPlansPVM updateInitialPlansPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateInitialPlansPVM.ChildsUsersIds == null)
                    {
                        updateInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateInitialPlansPVM.ChildsUsersIds.Count == 0)
                        updateInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateInitialPlansPVM.ChildsUsersIds.Count == 1)
                        if (updateInitialPlansPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateInitialPlansPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedInitialPlansVM = projectsApiBusiness.UpdateInitialPlans(
                    updateInitialPlansPVM.ChildsUsersIds,
                    updateInitialPlansPVM.InitialPlansVM);


                var initialPlanId = updatedInitialPlansVM.InitialPlanId;

                #region Load the count of attachements

                var attachementsOfInitialPlans = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => initialPlanId.Equals(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("InitialPlans")).ToList();

                updatedInitialPlansVM.AttachementsCount = attachementsOfInitialPlans.Count;
                #endregion


                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => initialPlanId.Equals(c.RecordId) && c.TableTitle.Equals("InitialPlan") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updateInitialPlansPVM.UserId.Value)).ToList();

                updatedInitialPlansVM.ConversationIsReadCount = converstions.Count;
                #endregion

                if (updatedInitialPlansVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedInitialPlansVM;

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

        [HttpPost("ToggleActivationInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationInitialPlans(ToggleActivationInitialPlansPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationInitialPlans(
                    toggleActivationInvestorsPVM.InitialPlanId,
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


        [HttpPost("TemporaryDeleteInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteInitialPlans(TemporaryDeleteInitialPlansPVM temporaryDeleteInitialPlansPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteInitialPlans(
                    temporaryDeleteInitialPlansPVM.InitialPlanId,
                   temporaryDeleteInitialPlansPVM.UserId.Value,
                   temporaryDeleteInitialPlansPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteInitialPlans")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteInitialPlans(CompleteDeleteInitialPlansPVM completeDeleteInitialPlansPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteInitialPlans(
                    completeDeleteInitialPlansPVM.InitialPlanId,
                   completeDeleteInitialPlansPVM.UserId.Value,
                   completeDeleteInitialPlansPVM.ChildsUsersIds);
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
        /// GetInitialPlansWithInitialPlanId
        /// </summary>
        /// <param name="getInitialPlansWithInitialPlanIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetInitialPlansWithInitialPlanId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetInitialPlansWithInitialPlanId(
            [FromBody] GetInitialPlansWithInitialPlanIdPVM getInitialPlansWithInitialPlanIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds == null)
                    {
                        getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds.Count == 0)
                        getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds.Count == 1)
                        if (getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfInitialPlans = projectsApiBusiness.GetInitialPlansWithInitialPlanId(
                        getInitialPlansWithInitialPlanIdPVM.InitialPlanId,
                        getInitialPlansWithInitialPlanIdPVM.UserId.Value,
                        getInitialPlansWithInitialPlanIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfInitialPlans;

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

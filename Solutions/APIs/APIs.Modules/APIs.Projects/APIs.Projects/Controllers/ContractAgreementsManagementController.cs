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
//using APIs.ContractAgreements.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Projects.Models.Entities;
using Castle.Core.Internal;
using System.Collections.Generic;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// ContractAgreementsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class ContractAgreementsManagementController : ApiBaseController
    {

        /// <summary>
        /// ContractAgreementsManagement
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
        public ContractAgreementsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllContractAgreementsList
        /// </summary>
        /// <param name="getListOfContractAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllContractAgreementsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllContractAgreementsList([FromBody] GetAllContractAgreementsListPVM getAllContractAgreementsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllContractAgreementsListPVM.ChildsUsersIds == null)
                    {
                        getAllContractAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllContractAgreementsListPVM.ChildsUsersIds.Count == 0)
                        getAllContractAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllContractAgreementsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllContractAgreementsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllContractAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractAgreements = projectsApiBusiness.GetAllContractAgreementsList(
                    getAllContractAgreementsListPVM.ChildsUsersIds,
                   getAllContractAgreementsListPVM.ContractAgreementTitle,
                   getAllContractAgreementsListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractAgreements;
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
        /// GetListOfContractAgreements
        /// </summary>
        /// <param name="getListOfContractAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfContractAgreements([FromBody] GetListOfContractAgreementsPVM getListOfContractAgreementsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfContractAgreementsPVM.ChildsUsersIds == null)
                    {
                        getListOfContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfContractAgreementsPVM.ChildsUsersIds.Count == 0)
                        getListOfContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfContractAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfContractAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfContractAgreements = projectsApiBusiness.GetListOfContractAgreements(
                   getListOfContractAgreementsPVM.jtStartIndex.Value,
                   getListOfContractAgreementsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfContractAgreementsPVM.ChildsUsersIds,
                   getListOfContractAgreementsPVM.ContractAgreementTitle,
                   getListOfContractAgreementsPVM.ConstructionProjectId,
                   getListOfContractAgreementsPVM.jtSorting
                   );



                var contractIds = listOfContractAgreements.Select(f => f.ContractAgreementId).ToList();

                #region Load the count of attachements

                var attachementsOfContractAgreements = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => contractIds.Contains(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("ContractAgreements")).ToList();

                #endregion

                #region IsView
                //دیده شده
                //isView

                var IsView = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(3) && contractIds.Contains(f.RecordId) && f.TableTitle.Equals("ContractAgreement")).ToList();

                #endregion

                #region IsConfirm

                //تاییده شده
                //isConfirm

                var IsConfirm = projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Where(f => f.FileStateId.Equals(4) && contractIds.Contains(f.RecordId) && f.TableTitle.Equals("ContractAgreement")).ToList();
                #endregion

                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => contractIds.Contains(c.RecordId) && c.TableTitle.Equals("ContractAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfContractAgreementsPVM.UserId.Value)).ToList();

                #endregion
                foreach (var item in listOfContractAgreements)
                {
                    #region count of attachements

                    if (attachementsOfContractAgreements.Where(c => c.AttachementParentId.Equals(item.ContractAgreementId) && c.AttachementTableTitle.Equals("ContractAgreements")).Any())
                    {
                        var attachementCount = attachementsOfContractAgreements.Where(c => c.AttachementParentId.Equals(item.ContractAgreementId) && c.AttachementTableTitle.Equals("ContractAgreements")).Count();

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
                    if (IsView.Where(c => c.RecordId.Equals(item.ContractAgreementId)).Any())
                    {
                        var isView = IsView.Where(c => c.RecordId.Equals(item.ContractAgreementId)).FirstOrDefault();
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
                    if (IsConfirm.Where(c => c.RecordId.Equals(item.ContractAgreementId)).Any())
                    {
                        var isConfirm = IsConfirm.Where(c => c.RecordId.Equals(item.ContractAgreementId)).FirstOrDefault();
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

                    if (converstions.Where(f => f.RecordId.Equals(item.ContractAgreementId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.ContractAgreementId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }


                    #endregion

                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfContractAgreements;
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
        /// AddToContractAgreements
        /// </summary>
        /// <param name="addToContractAgreementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToContractAgreements([FromBody] AddToContractAgreementsPVM addToContractAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToContractAgreementsPVM.ChildsUsersIds == null)
                    {
                        addToContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToContractAgreementsPVM.ChildsUsersIds.Count == 0)
                        addToContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToContractAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (addToContractAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var contractAgreementId = projectsApiBusiness.AddToContractAgreements(addToContractAgreementsPVM.ContractAgreementsVM);
                if (contractAgreementId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "ContractAgreement",
                        RecordId = contractAgreementId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();



                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToContractAgreementsPVM.ContractAgreementsVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToContractAgreementsPVM.ContractAgreementsVM.ConstructionProjectId && c.OwnerUserId != addToContractAgreementsPVM.ContractAgreementsVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToContractAgreementsPVM.ContractAgreementsVM.ConstructionProjectId)
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


                    addToContractAgreementsPVM.ContractAgreementsVM.ContractAgreementId = contractAgreementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToContractAgreementsPVM.ContractAgreementsVM;

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


        [HttpPost("UpdateContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateContractAgreements([FromBody] UpdateContractAgreementsPVM updateContractAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateContractAgreementsPVM.ChildsUsersIds == null)
                    {
                        updateContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updateContractAgreementsPVM.ChildsUsersIds.Count == 0)
                        updateContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updateContractAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (updateContractAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateContractAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedContractAgreementsVM = projectsApiBusiness.UpdateContractAgreements(
                    updateContractAgreementsPVM.ChildsUsersIds,
                    updateContractAgreementsPVM.ContractAgreementsVM);


                var contractId = updatedContractAgreementsVM.ContractAgreementId;

                #region Load the count of attachements

                var attachementsOfContractAgreements = projectsApiBusiness.ProjectsApiDb.AttachementFiles.Where(f => contractId.Equals(f.AttachementParentId.Value) && f.AttachementTableTitle.Equals("ContractAgreements")).ToList();

                updatedContractAgreementsVM.AttachementsCount = attachementsOfContractAgreements.Count;

                #endregion


                #region Conversations

                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => contractId.Equals(c.RecordId) && c.TableTitle.Equals("ContractAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updateContractAgreementsPVM.UserId.Value)).ToList();

                updatedContractAgreementsVM.ConversationIsReadCount = converstions.Count;

                #endregion

                if (updatedContractAgreementsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedContractAgreementsVM;

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

        [HttpPost("ToggleActivationContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationContractAgreements(ToggleActivationContractAgreementsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationContractAgreements(
                    toggleActivationInvestorsPVM.ContractAgreementId,
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


        [HttpPost("TemporaryDeleteContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteContractAgreements(TemporaryDeleteContractAgreementsPVM temporaryDeleteContractAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeleteContractAgreements(
                    temporaryDeleteContractAgreementsPVM.ContractAgreementId,
                   temporaryDeleteContractAgreementsPVM.UserId.Value,
                   temporaryDeleteContractAgreementsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeleteContractAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteContractAgreements(CompleteDeleteContractAgreementsPVM completeDeleteContractAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;

            try
            {
                result = projectsApiBusiness.CompleteDeleteContractAgreements(
                    completeDeleteContractAgreementsPVM.ContractAgreementId,
                   completeDeleteContractAgreementsPVM.UserId.Value,
                   completeDeleteContractAgreementsPVM.ChildsUsersIds);
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
        /// GetContractAgreementsWithContractAgreementId
        /// </summary>
        /// <param name="getContractAgreementsWithContractAgreementIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetContractAgreementsWithContractAgreementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetContractAgreementsWithContractAgreementId(
            [FromBody] GetContractAgreementsWithContractAgreementIdPVM getContractAgreementsWithContractAgreementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds == null)
                    {
                        getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds.Count == 0)
                        getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds.Count == 1)
                        if (getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfContractAgreements = projectsApiBusiness.GetContractAgreementsWithContractAgreementId(
                        getContractAgreementsWithContractAgreementIdPVM.ContractAgreementId,
                        getContractAgreementsWithContractAgreementIdPVM.UserId.Value,
                        getContractAgreementsWithContractAgreementIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfContractAgreements;

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

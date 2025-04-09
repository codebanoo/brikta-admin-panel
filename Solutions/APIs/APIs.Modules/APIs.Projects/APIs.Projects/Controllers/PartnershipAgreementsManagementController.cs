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
//using APIs.PartnershipAgreements.Models.Business;
using APIs.Melkavan.Models.Business;
using VM.Public;
using VM.Projects;
using APIs.Teniaco.Models.Business;
using APIs.TelegramBot.Models.Business;
using APIs.Public.Models.Entities;
using Castle.Components.DictionaryAdapter;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json.Nodes;
using System.Text;
using APIs.Projects.Models.Entities;
using Models;
using Castle.Core.Internal;
using System.Collections.Generic;

namespace APIs.Projects.Controllers
{
    /// <summary>
    /// PartnershipAgreementsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class PartnershipAgreementsManagementController : ApiBaseController
    {

        /// <summary>
        /// PartnershipAgreementsManagement
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
        public PartnershipAgreementsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllPartnershipAgreementsList
        /// </summary>
        /// <param name="getAllPartnershipAgreementsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllPartnershipAgreementsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllPartnershipAgreementsList([FromBody] GetAllPartnershipAgreementsListPVM getAllPartnershipAgreementsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllPartnershipAgreementsListPVM.ChildsUsersIds == null)
                    {
                        getAllPartnershipAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllPartnershipAgreementsListPVM.ChildsUsersIds.Count == 0)
                        getAllPartnershipAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllPartnershipAgreementsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllPartnershipAgreementsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllPartnershipAgreementsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPartnershipAgreements = projectsApiBusiness.GetAllPartnershipAgreementsList(
                    getAllPartnershipAgreementsListPVM.ChildsUsersIds,

                   getAllPartnershipAgreementsListPVM.PartnershipAgreementTitle,
                   getAllPartnershipAgreementsListPVM.ConstructionProjectId
                   );


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPartnershipAgreements;
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
        /// GetListOfPartnershipAgreements
        /// </summary>
        /// <param name="getListOfPartnershipAgreementsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfPartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfPartnershipAgreements([FromBody] GetListOfPartnershipAgreementsPVM getListOfPartnershipAgreementsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfPartnershipAgreementsPVM.ChildsUsersIds == null)
                    {
                        getListOfPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfPartnershipAgreementsPVM.ChildsUsersIds.Count == 0)
                        getListOfPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfPartnershipAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfPartnershipAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfPartnershipAgreements = projectsApiBusiness.GetListOfPartnershipAgreements(
                   getListOfPartnershipAgreementsPVM.jtStartIndex.Value,
                   getListOfPartnershipAgreementsPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfPartnershipAgreementsPVM.ChildsUsersIds,
                   getListOfPartnershipAgreementsPVM.UserId.Value,
                   getListOfPartnershipAgreementsPVM.PartnershipAgreementTitle,
                   getListOfPartnershipAgreementsPVM.ConstructionProjectId,
                   getListOfPartnershipAgreementsPVM.jtSorting
                   );

                var partnershipIds = listOfPartnershipAgreements.Select(c => c.PartnershipAgreementId).ToList();


                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => partnershipIds.Contains(c.RecordId) && c.TableTitle.Equals("PartnershipAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(getListOfPartnershipAgreementsPVM.UserId.Value)).ToList();


                foreach (var item in listOfPartnershipAgreements)
                {

                    if (converstions.Where(f => f.RecordId.Equals(item.PartnershipAgreementId)).Any())
                    {
                        item.ConversationIsReadCount = converstions.Where(f => f.RecordId.Equals(item.PartnershipAgreementId)).Count();
                    }
                    else
                    {
                        item.ConversationIsReadCount = 0;
                    }



                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfPartnershipAgreements;
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



        ///// <summary>
        ///// GetListOfPartnershipAgreementsWithNoParent
        ///// </summary>
        ///// <param name="getListOfPartnershipAgreementsWithNoParentPVM"></param>
        ///// <returns>JsonResultWithRecordsObjectVM</returns>
        //[HttpPost("GetListOfPartnershipAgreementsWithNoParent")]
        //[ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        //public IActionResult GetListOfPartnershipAgreementsWithNoParent([FromBody] GetListOfPartnershipAgreementsWithNoParentPVM getListOfPartnershipAgreementsWithNoParentPVM)
        //{
        //    JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            if (getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds == null)
        //            {
        //                getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            }
        //            else
        //          if (getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds.Count == 0)
        //                getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            else
        //          if (getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds.Count == 1)
        //                if (getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds.FirstOrDefault() == 0)
        //                    getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //        }

        //        int listCount = 0;

        //        var listOfPartnershipAgreements = projectsApiBusiness.GetListOfPartnershipAgreementsWithNoParent(
        //           getListOfPartnershipAgreementsWithNoParentPVM.jtStartIndex.Value,
        //           getListOfPartnershipAgreementsWithNoParentPVM.jtPageSize.Value,
        //           ref listCount,
        //           getListOfPartnershipAgreementsWithNoParentPVM.ChildsUsersIds,
        //           getListOfPartnershipAgreementsWithNoParentPVM.PartnershipAgreementTitle,
        //           getListOfPartnershipAgreementsWithNoParentPVM.ConstructionProjectId,
        //           getListOfPartnershipAgreementsWithNoParentPVM.jtSorting
        //           );


        //        jsonResultWithRecordsObjectVM.Result = "OK";
        //        jsonResultWithRecordsObjectVM.Records = listOfPartnershipAgreements;
        //        jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

        //        return new JsonResult(jsonResultWithRecordsObjectVM);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //    }


        //    jsonResultWithRecordsObjectVM.Result = "ERROR";
        //    jsonResultWithRecordsObjectVM.Message = "ErrorInService";

        //    return new JsonResult(jsonResultWithRecordsObjectVM);
        //}



        /// <summary>
        /// AddToPartnershipAgreements
        /// </summary>
        /// <param name="addToPartnershipAgreementsPVM"></param>
        /// <returns>JsonResultObjectVM</returns>

        [HttpPost("AddToPartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToPartnershipAgreements([FromBody] AddToPartnershipAgreementsPVM addToPartnershipAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToPartnershipAgreementsPVM.ChildsUsersIds == null)
                    {
                        addToPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (addToPartnershipAgreementsPVM.ChildsUsersIds.Count == 0)
                        addToPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (addToPartnershipAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (addToPartnershipAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToPartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var partnershipAgreementId = projectsApiBusiness.AddToPartnershipAgreements(addToPartnershipAgreementsPVM.PartnershipAgreementsVM);
                if (partnershipAgreementId != 0)
                {
                    // Adding To FilesStatesLogs
                    FileStatesLogs fileStatesLogs = new FileStatesLogs()
                    {
                        TableTitle = "PartnershipAgreement",
                        RecordId = partnershipAgreementId,
                        FileStateId = 1
                    };

                    projectsApiBusiness.ProjectsApiDb.FileStatesLogs.Add(fileStatesLogs);
                    projectsApiBusiness.ProjectsApiDb.SaveChanges();

                    //ارسال اس ام اس به سرمایه گذار
                    SendSmsService sendSmsService = new SendSmsService();
                    var smsSender = publicApiBusiness.PublicApiDb.SmsSenders.Where(c => c.IsDefault == true && c.IsActivated.Value.Equals(true) && c.IsDeleted.Value.Equals(false)).FirstOrDefault();

                    // گرفتن شماره موبایل سرمایه گذار
                    List<long> RepresentativeIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectRepresentatives // آی دی نمایندگان
                        .Where(c => c.ConstructionProjectId == addToPartnershipAgreementsPVM.PartnershipAgreementsVM.ConstructionProjectId)
                        .Select(c => c.RepresentativeId).ToList();

                    List<long> OtherOwnerIds = projectsApiBusiness.ProjectsApiDb.ConstructionProjectOwnerUsers  // آی دی دیگر سرمایه گذاران
                        .Where(c => c.ConstructionProjectId == addToPartnershipAgreementsPVM.PartnershipAgreementsVM.ConstructionProjectId && c.OwnerUserId != addToPartnershipAgreementsPVM.PartnershipAgreementsVM.UserIdCreator)
                        .Select(c => c.OwnerUserId).ToList();

                    List<string> RepresentativePhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل نمایندگان
                    List<string> OwnerPhoneNumbers = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Mobile).ToList();  // شماره موبایل سرمایه گذاران

                    List<string> RepresentativeFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => RepresentativeIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی نمایندگان
                    List<string> OwnerFamilyNames = consoleBusiness.CmsDb.UsersProfile.Where(u => OtherOwnerIds.Contains(u.UserId)).Select(u => u.Family).ToList(); // فامیلی سرماه گذاران


                    string ConstructionProjectTitle = projectsApiBusiness.ProjectsApiDb.ConstructionProjects
                        .Where(c => c.ConstructionProjectId == addToPartnershipAgreementsPVM.PartnershipAgreementsVM.ConstructionProjectId)
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

                    addToPartnershipAgreementsPVM.PartnershipAgreementsVM.PartnershipAgreementId = partnershipAgreementId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToPartnershipAgreementsPVM.PartnershipAgreementsVM;

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


        [HttpPost("UpdatePartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdatePartnershipAgreements([FromBody] UpdatePartnershipAgreementsPVM updatePartnershipAgreementsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updatePartnershipAgreementsPVM.ChildsUsersIds == null)
                    {
                        updatePartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (updatePartnershipAgreementsPVM.ChildsUsersIds.Count == 0)
                        updatePartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (updatePartnershipAgreementsPVM.ChildsUsersIds.Count == 1)
                        if (updatePartnershipAgreementsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updatePartnershipAgreementsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                var updatedPartnershipAgreementsVM = projectsApiBusiness.UpdatePartnershipAgreements(
                    updatePartnershipAgreementsPVM.ChildsUsersIds,
                    updatePartnershipAgreementsPVM.PartnershipAgreementsVM);



                #region Count of convesations
                var partnerShipId = updatedPartnershipAgreementsVM.PartnershipAgreementId;
                var converstions = projectsApiBusiness.ProjectsApiDb.ConversationLogs.Where(c => partnerShipId.Equals(c.RecordId) && c.TableTitle.Equals("PartnershipAgreement") && c.IsRead.Equals(false) && !c.UserIdCreator.Equals(updatePartnershipAgreementsPVM.UserId.Value)).ToList();
                updatedPartnershipAgreementsVM.ConversationIsReadCount = converstions.Count;
                #endregion


                if (updatedPartnershipAgreementsVM != null)
                {

                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updatedPartnershipAgreementsVM;

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

        [HttpPost("ToggleActivationPartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationPartnershipAgreements(ToggleActivationPartnershipAgreementsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.ToggleActivationPartnershipAgreements(
                    toggleActivationInvestorsPVM.PartnershipAgreementId,
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


        [HttpPost("TemporaryDeletePartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeletePartnershipAgreements(TemporaryDeletePartnershipAgreementsPVM temporaryDeletePartnershipAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (projectsApiBusiness.TemporaryDeletePartnershipAgreements(
                    temporaryDeletePartnershipAgreementsPVM.PartnershipAgreementId,
                   temporaryDeletePartnershipAgreementsPVM.UserId.Value,
                   temporaryDeletePartnershipAgreementsPVM.ChildsUsersIds
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


        [HttpPost("CompleteDeletePartnershipAgreements")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeletePartnershipAgreements(CompleteDeletePartnershipAgreementsPVM completeDeletePartnershipAgreementsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });
            bool? result = false;


            try
            {
                result = projectsApiBusiness.CompleteDeletePartnershipAgreements(
                    completeDeletePartnershipAgreementsPVM.PartnershipAgreementId,
                   completeDeletePartnershipAgreementsPVM.UserId.Value,
                   completeDeletePartnershipAgreementsPVM.ChildsUsersIds);
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
        /// GetPartnershipAgreementsWithPartnershipAgreementId
        /// </summary>
        /// <param name="getPartnershipAgreementsWithPartnershipAgreementIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>
        [HttpPost("GetPartnershipAgreementsWithPartnershipAgreementId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetPartnershipAgreementsWithPartnershipAgreementId(
            [FromBody] GetPartnershipAgreementsWithPartnershipAgreementIdPVM getPartnershipAgreementsWithPartnershipAgreementIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds == null)
                    {
                        getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds.Count == 0)
                        getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds.Count == 1)
                        if (getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var listOfPartnershipAgreements = projectsApiBusiness.GetPartnershipAgreementsWithPartnershipAgreementId(
                        getPartnershipAgreementsWithPartnershipAgreementIdPVM.PartnershipAgreementId,
                        getPartnershipAgreementsWithPartnershipAgreementIdPVM.UserId.Value,
                        getPartnershipAgreementsWithPartnershipAgreementIdPVM.ChildsUsersIds
                   );


                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = listOfPartnershipAgreements;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception ex)
            {

            }


            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        ///// <summary>
        ///// GetPartnershipAgreementsWithParentId
        ///// </summary>
        ///// <param name="getPartnershipAgreementsWithPartnershipAgreementIdPVM"></param>
        ///// <returns>JsonResultWithRecordsObjectVM</returns>
        //[HttpPost("GetPartnershipAgreementsWithParentId")]
        //[ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        //public IActionResult GetPartnershipAgreementsWithParentId(
        //    [FromBody] GetPartnershipAgreementsWithParentIdPVM getPartnershipAgreementsWithParentIdPVM)
        //{
        //    JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

        //    try
        //    {
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            if (getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds == null)
        //            {
        //                getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            }
        //            else
        //          if (getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds.Count == 0)
        //                getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //            else
        //          if (getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds.Count == 1)
        //                if (getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
        //                    getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
        //        }

        //        var listOfPartnershipAgreements = projectsApiBusiness.GetPartnershipAgreementsWithParentId(
        //                getPartnershipAgreementsWithParentIdPVM.ParentPartnershipAgreementId.Value,
        //                getPartnershipAgreementsWithParentIdPVM.UserId.Value,
        //                getPartnershipAgreementsWithParentIdPVM.ChildsUsersIds
        //           );


        //        jsonResultWithRecordsObjectVM.Result = "OK";
        //        jsonResultWithRecordsObjectVM.Records = listOfPartnershipAgreements;
        //        //jsonResultWithRecordsObjectVM.TotalRecordCount = 
        //        return new JsonResult(jsonResultWithRecordsObjectVM);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //    }


        //    jsonResultWithRecordsObjectVM.Result = "ERROR";
        //    jsonResultWithRecordsObjectVM.Message = "ErrorInService";

        //    return new JsonResult(jsonResultWithRecordsObjectVM);
        //}


    }

    [NonController]
    public class SendSmsService
    {
        public void SendSms(string phoneNumber, string Message, SmsSenders smsSender)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://sms.safironline.ir/webservice/rest/sms_send?");

            var MobileNumber = phoneNumber;
            var postData = "note_arr= " + Message + "&login_username=" + smsSender.UserName + "& login_password=" + smsSender.Password + "&receiver_number=" + MobileNumber + "&sender_number=" + smsSender.SmsSenderNumber;

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}

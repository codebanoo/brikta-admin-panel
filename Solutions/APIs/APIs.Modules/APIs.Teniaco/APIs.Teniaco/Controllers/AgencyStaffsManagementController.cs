using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
using APIs.CustomAttributes;
using APIs.CustomAttributes.Helper;
using APIs.Melkavan.Models.Business;
using APIs.Projects.Models.Business;
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
using System.Linq;
using System.Net;
using VM.Base;
using VM.Console;
using VM.Public;
using VM.PVM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// AgencyStaffsManagement
    /// </summary>
    [CustomApiAuthentication]
    public class AgencyStaffsManagementController : ApiBaseController
    {
        /// <summary>
        /// AgencyStaffsManagement
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
        public AgencyStaffsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllAgencyStaffsList
        /// </summary>
        /// <param name="getAllAgencyStaffsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllAgencyStaffsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllAgencyStaffsList([FromBody] GetAllAgencyStaffsListPVM getAllAgencyStaffsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAgencyStaffsListPVM.ChildsUsersIds == null)
                    {
                        getAllAgencyStaffsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllAgencyStaffsListPVM.ChildsUsersIds.Count == 0)
                        getAllAgencyStaffsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllAgencyStaffsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllAgencyStaffsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAgencyStaffsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfAgencyStaffs = teniacoApiBusiness.GetAllAgencyStaffsList(
                    //publicApiBusiness.PublicApiDb,
                    //consoleBusiness.CmsDb,
                    ref listCount,
                    getAllAgencyStaffsListPVM.ChildsUsersIds,
                    getAllAgencyStaffsListPVM.AgencyId);

                var userIds = listOfAgencyStaffs.Where(p => p.UserId.HasValue).Select(p => p.UserId.Value).ToList();
                var users = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).ToList();

                var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => userIds.Contains(c.UserId)).ToList();

                foreach (var staff in listOfAgencyStaffs)
                {
                    var userProfile = usersProfile.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();
                    if (users.Where(u => u.UserId.Equals(staff.UserId)).Any())
                    {
                        var user = users.Where(u => u.UserId.Equals(staff.UserId)).FirstOrDefault();

                        staff.CustomUsersVM = new CustomUsersVM();
                        staff.CustomUsersVM.UserId = user.UserId;
                        staff.CustomUsersVM.UserName = user.UserName;
                        staff.CustomUsersVM.Password = user.Password;
                        staff.CustomUsersVM.Name = userProfile.Name;
                        staff.CustomUsersVM.Family = userProfile.Family;
                        staff.CustomUsersVM.Mobile = userProfile.Mobile;
                        staff.CustomUsersVM.Email = user.Email;
                        staff.CustomUsersVM.ParentUserId = user.ParentUserId;
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencyStaffs;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

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



        /// <summary>
        /// GetListOfAgencyStaffs
        /// </summary>
        /// <param name="getListOfAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAgencyStaffs([FromBody] GetListOfAgencyStaffsPVM getListOfAgencyStaffsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAgencyStaffsPVM.ChildsUsersIds == null)
                    {
                        getListOfAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAgencyStaffsPVM.ChildsUsersIds.Count == 0)
                        getListOfAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAgencyStaffsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAgencyStaffsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAgencyStaffs = teniacoApiBusiness.GetListOfAgencyStaffs(
                 getListOfAgencyStaffsPVM.jtStartIndex.Value,
                 getListOfAgencyStaffsPVM.jtPageSize.Value,
                 ref listCount,
                 getListOfAgencyStaffsPVM.ChildsUsersIds,
                 getListOfAgencyStaffsPVM.AgencyId,
                 getListOfAgencyStaffsPVM.jtSorting);


                var userIds = listOfAgencyStaffs.Where(p => p.UserId.HasValue).Select(p => p.UserId.Value).ToList();
                var users = consoleBusiness.CmsDb.Users.Where(c => userIds.Contains(c.UserId)).ToList();

                //var users = consoleBusiness.GetUsersWithUserIds(userIds);

                var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => userIds.Contains(c.UserId)).ToList();

                foreach (var staff in listOfAgencyStaffs)
                {
                    var userProfile = usersProfile.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();
                    if (users.Where(u => u.UserId.Equals(staff.UserId)).Any())
                    {
                        var user = users.Where(u => u.UserId.Equals(staff.UserId)).FirstOrDefault();

                        staff.CustomUsersVM = new CustomUsersVM();
                        staff.CustomUsersVM.UserId = user.UserId;
                        staff.CustomUsersVM.UserName = user.UserName;
                        staff.CustomUsersVM.Password = user.Password;
                        staff.CustomUsersVM.Name = userProfile.Name;
                        staff.CustomUsersVM.Family = userProfile.Family;
                        staff.CustomUsersVM.Mobile = userProfile.Mobile;
                        staff.CustomUsersVM.Email = user.Email;
                        staff.CustomUsersVM.ParentUserId = user.ParentUserId;
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencyStaffs;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception ex)
            { }
            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
        }


        /// <summary>
        /// AddToAgencyStaffs
        /// </summary>
        /// <param name="addToAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAgencyStaffs([FromBody] AddToAgencyStaffsPVM addToAgencyStaffsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAgencyStaffsPVM.ChildsUsersIds == null)
                    {
                        addToAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToAgencyStaffsPVM.ChildsUsersIds.Count == 0)
                        {
                            addToAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToAgencyStaffsPVM.ChildsUsersIds.Count == 1)
                                if (addToAgencyStaffsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToAgencyStaffsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToAgencyStaffsPVM.AgencyStaffsVM.CreateEnDate = DateTime.Now;
                    addToAgencyStaffsPVM.AgencyStaffsVM.CreateTime = PersianDate.TimeNow;
                    addToAgencyStaffsPVM.AgencyStaffsVM.UserIdCreator = this.userId.Value;
                    addToAgencyStaffsPVM.AgencyStaffsVM.IsActivated = true;
                    addToAgencyStaffsPVM.AgencyStaffsVM.IsDeleted = false;



                }


                int agencyStuffId = teniacoApiBusiness.AddToAgencyStaffs(
                    addToAgencyStaffsPVM.AgencyStaffsVM,
                    consoleBusiness,
                    this.domainsSettings.DomainName);

                if (agencyStuffId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                if (agencyStuffId > 0)
                {
                    addToAgencyStaffsPVM.AgencyStaffsVM.AgencyStaffsId = agencyStuffId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAgencyStaffsPVM.AgencyStaffsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);

                }
                else if (agencyStuffId.Equals(-2))// مدیر املاک وجود دارد با همین شماره موبایل
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "AgentManager";
                }
                else if (agencyStuffId.Equals(-3)) //با این شماره موبایل یک مشاور دیگر در یک آژانس املاک میباشد
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "AnotherAgent";
                }


            }
            catch (Exception)
            { }

            //jsonResultWithRecordObjectVM.Result = "ERROR";
            //jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// UpdateAgencyStaffs
        /// </summary>
        /// <param name="updateAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("UpdateAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateAgencyStaffs([FromBody] UpdateAgencyStaffsPVM updateAgencyStaffsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });


            try
            {
                var agenciesStaffsVM = updateAgencyStaffsPVM.AgencyStaffsVM;

                int agencyStuffsId = teniacoApiBusiness.UpdateAgencyStaffs(
                   ref agenciesStaffsVM,
                   updateAgencyStaffsPVM.ChildsUsersIds,
                   consoleBusiness);


                if (agencyStuffsId.Equals(0))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "ErrorInService";
                }
                else
                if (agencyStuffsId > 0)
                {
                    updateAgencyStaffsPVM.AgencyStaffsVM.AgencyStaffsId = agencyStuffsId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateAgencyStaffsPVM.AgencyStaffsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if (agencyStuffsId.Equals(-3))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "ParentUserError";
                }

              


            }
            catch (Exception exc)
            { }

            //jsonResultWithRecordObjectVM.Result = "ERROR";
            //jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }


        /// <summary>
        /// GetAgencyStaffWithAgencyStaffId
        /// </summary>
        /// <param name="getAgencyStaffWithAgencyStaffIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetAgencyStaffWithAgencyStaffId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAgencyStaffWithAgencyStaffId([FromBody] GetAgencyStaffWithAgencyStaffIdPVM
            getAgencyStaffWithAgencyStaffIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds == null)
                    {
                        getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds.Count == 0)
                        getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds.Count == 1)
                        if (getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var agencyStaff = teniacoApiBusiness.GetAgencyStaffWithAgencyStaffId(
                    getAgencyStaffWithAgencyStaffIdPVM.AgencyStaffsId,
                    getAgencyStaffWithAgencyStaffIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = agencyStaff;

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// ToggleActivationAgencyStaffs
        /// </summary>
        /// <param name="toggleActivationAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationAgencyStaffs([FromBody] ToggleActivationAgencyStaffsPVM toggleActivationAgencyStaffsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationAgencyStaffs(

                   toggleActivationAgencyStaffsPVM.AgencyStaffsId,
                   toggleActivationAgencyStaffsPVM.UserId.Value,
                   toggleActivationAgencyStaffsPVM.ChildsUsersIds,
                    consoleBusiness))
                {
                    if (!string.IsNullOrEmpty(returnMessage))
                    {
                        jsonResultObjectVM.Result = returnMessage;
                    }
                    else
                    {
                        jsonResultObjectVM.Result = "OK";
                    }

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);

        }


        /// <summary>
        /// TemporaryDeleteAgencyStaffs
        /// </summary>
        /// <param name="temporaryDeleteAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteAgencyStaffs([FromBody] TemporaryDeleteAgencyStaffsPVM temporaryDeleteAgencyStaffsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteAgencyStaffs(
                    temporaryDeleteAgencyStaffsPVM.AgencyStaffsId,
                    temporaryDeleteAgencyStaffsPVM.UserId.Value,
                    temporaryDeleteAgencyStaffsPVM.ChildsUsersIds,
                    consoleBusiness))
                {
                    jsonResultObjectVM.Result = "OK";

                    return new JsonResult(jsonResultObjectVM);

                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }



        /// <summary>
        /// CompleteDeleteAgencyStaffs
        /// </summary>
        /// <param name="completeDeleteAgencyStaffsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteAgencyStaffs")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteAgencyStaffs([FromBody] CompleteDeleteAgencyStaffsPVM completeDeleteAgencyStaffsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteAgencyStaffs(
                   completeDeleteAgencyStaffsPVM.AgencyStaffsId,
                   completeDeleteAgencyStaffsPVM.ChildsUsersIds,
                   consoleBusiness))
                {
                    jsonResultObjectVM.Result = "OK";
                    jsonResultObjectVM.Message = "Success";

                    return new JsonResult(jsonResultObjectVM);
                }
            }
            catch (Exception)
            { }

            jsonResultObjectVM.Result = "ERROR";
            jsonResultObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultObjectVM);
        }

    }
}

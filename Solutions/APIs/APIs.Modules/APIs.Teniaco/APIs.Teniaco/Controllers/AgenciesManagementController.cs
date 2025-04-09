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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VM.Base;
using VM.Console;
using VM.PVM.Teniaco;
using VM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// AgenciesManagement
    /// </summary>
    [CustomApiAuthentication]

    public class AgenciesManagementController : ApiBaseController
    {
        /// <summary>
        /// AgenciesManagement
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
        public AgenciesManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllAgenciesList
        /// </summary>
        /// <param name="getAllAgenciesListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllAgenciesList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllAgenciesList([FromBody] GetAllAgenciesListPVM getAllAgenciesListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllAgenciesListPVM.ChildsUsersIds == null)
                    {
                        getAllAgenciesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllAgenciesListPVM.ChildsUsersIds.Count == 0)
                        getAllAgenciesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllAgenciesListPVM.ChildsUsersIds.Count == 1)
                        if (getAllAgenciesListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllAgenciesListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                int listCount = 0;

                var listOfAgencies = teniacoApiBusiness.GetAllAgenciesList(
                   publicApiBusiness.PublicApiDb,
                    ref listCount,
                     getAllAgenciesListPVM.ChildsUsersIds,
                     //getAllAgenciesListPVM.AgencyId,
                     getAllAgenciesListPVM.AgencyName,
                     getAllAgenciesListPVM.StateId,
                     getAllAgenciesListPVM.CityId,
                     getAllAgenciesListPVM.ZoneId);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencies;
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
        /// GetListOfAgencies
        /// </summary>
        /// <param name="getListOfAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfAgencies([FromBody] GetListOfAgenciesPVM getListOfAgenciesPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfAgenciesPVM.ChildsUsersIds == null)
                    {
                        getListOfAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfAgenciesPVM.ChildsUsersIds.Count == 0)
                        getListOfAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfAgenciesPVM.ChildsUsersIds.Count == 1)
                        if (getListOfAgenciesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;

                var listOfAgencies = teniacoApiBusiness.GetListOfAgencies(
                   publicApiBusiness.PublicApiDb,
                   getListOfAgenciesPVM.jtStartIndex.Value,
                   getListOfAgenciesPVM.jtPageSize.Value,
                   ref listCount,
                   getListOfAgenciesPVM.ChildsUsersIds,
                   // getListOfAgenciesPVM.AgencyId,
                   getListOfAgenciesPVM.AgencyName,
                   getListOfAgenciesPVM.StateId,
                   getListOfAgenciesPVM.CityId,
                   getListOfAgenciesPVM.ZoneId,
                   getListOfAgenciesPVM.jtSorting);


                #region users



                var agencyIds = listOfAgencies.Select(c => c.AgencyId).ToList();

                //var userIdsOfAgencyStaffs = teniacoApiBusiness.TeniacoApiDb.AgencyStaffs.Where(c => agencyIds.Contains(c.AgencyId)).Select(c => c.UserId).ToList().Distinct();
                //var users = consoleBusiness.CmsDb.Users.Where(c => userIdsOfAgencyStaffs.Contains(c.UserId)).ToList();
                //var usersProfiles = consoleBusiness.CmsDb.UsersProfile.Where(c => userIdsOfAgencyStaffs.Contains(c.UserId)).ToList();

                foreach (var item in listOfAgencies)
                {
                    //لیست تمامی کارمندان بنگاه
                    //برای یک رکورد

                    var agencyStaffs = teniacoApiBusiness.TeniacoApiDb.AgencyStaffs.Where(c => c.AgencyId.Equals(item.AgencyId)).ToList();
                    //var userIds = agencyStaffs.Select(c => c.UserId).Distinct().ToList();

                    //var userLevels = consoleBusiness.CmsDb.UsersLevels.Where(c => userIds.Contains(c.UserId)).Distinct().ToList();
                    //var userProfiles = consoleBusiness.CmsDb.UsersProfile.Where(c => userIds.Contains(c.UserId)).ToList();

                    if (agencyStaffs != null)
                    {
                        foreach (var staff in agencyStaffs)
                        {

                            var levelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(staff.UserId)).ToList().Select(c => c.LevelId);
                            var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIds.Contains(c.LevelId)).ToList().Select(c=>c.LevelName);
                            var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();

                            if (levelNames.Contains("مدیر املاک"))
                            {


                                var agencyStaff = agencyStaffs.Where(c => c.AgencyStaffsId.Equals(staff.AgencyStaffsId)).ToList();

                                item.AgencyStaffsVMList = agencyStaff.Select(c => new AgencyStaffsVM()
                                {
                                    AgencyId = staff.AgencyId,
                                    UserId = staff.UserId,
                                    CustomUsersVM = new CustomUsersVM()
                                    {
                                        Mobile = userProfile.Mobile,
                                        Name = userProfile.Name,
                                        Family = userProfile.Family,
                                    }
                                }).ToList();

                            }
                           

                            #region comments

                            //var levelIds = userLevels.Select(c => c.LevelId).ToList();
                            //var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIds.Contains(c.LevelId)).Select(c => c.LevelName).ToList();
                            //var userProfile = userProfiles.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();

                            //if (levelNames.Contains("مدیر املاک"))
                            //{
                            //    item.AgencyStaffsVMList = agencyStaffs.Select(c => new AgencyStaffsVM()
                            //    {
                            //        AgencyId = item.AgencyId,
                            //        UserId = staff.UserId,
                            //        CustomUsersVM = new CustomUsersVM()
                            //        {
                            //            Mobile = userProfile.Mobile,
                            //            Name = userProfile.Name,
                            //            Family = userProfile.Family,
                            //        }

                            //    }).ToList();
                            //}
                            #endregion

                        }
                    }



                    #region old code
                    //if (agencyStaffs.Where(c => c.AgencyId.Equals(item.AgencyId)).Any())
                    //{

                    //    var userIds = agencyStaffs.Where(c => c.AgencyId.Equals(item.AgencyId)).Select(c => c.UserId).Distinct().ToList();



                    //    var levelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => userIds.Equals(c.UserId)).ToList().Select(c=>c.LevelId);



                    //    //var staff = agencyStaffs.Where(c => c.AgencyId.Equals(item.AgencyId)).FirstOrDefault();

                    //    //var user = consoleBusiness.CmsDb.Users.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();
                    //    var levelIds = consoleBusiness.CmsDb.UsersLevels.Where(c => c.UserId.Equals(staff.UserId)).Select(c=>c.LevelId);

                    //    var levelNames = consoleBusiness.CmsDb.Levels.Where(c => levelIds.Contains(c.LevelId)).ToList().Select(c=>c.LevelName);

                    //    //var level = consoleBusiness.CmsDb.Levels.Where(c => c.LevelId.Equals(userLevel.LevelId)).FirstOrDefault();

                    //    var userProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(staff.UserId)).FirstOrDefault();

                    //    if(levelNames.Contains("مدیر املاک"))
                    //    {
                    //        item.AgencyStaffsVMList = agencyStaffs.Select(c => new AgencyStaffsVM()
                    //        {
                    //            AgencyId = item.AgencyId,
                    //            UserId = staff.UserId,
                    //            CustomUsersVM = new CustomUsersVM()
                    //            {
                    //                //UserName = user.UserName,
                    //                //UserId = user.UserId,
                    //                Mobile = userProfile.Mobile,
                    //                Name = userProfile.Name,
                    //                Family = userProfile.Family,
                    //            }

                    //        }).ToList();
                    //    }



                    //}
                    #endregion


                }

                #endregion


                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfAgencies;
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
        /// AddToAgencies
        /// </summary>
        /// <param name="addToAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToAgencies([FromBody] AddToAgenciesPVM addToAgenciesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToAgenciesPVM.ChildsUsersIds == null)
                    {
                        addToAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToAgenciesPVM.ChildsUsersIds.Count == 0)
                        {
                            addToAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToAgenciesPVM.ChildsUsersIds.Count == 1)
                                if (addToAgenciesPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToAgenciesPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }

                    addToAgenciesPVM.AgenciesVM.CreateEnDate = DateTime.Now;
                    addToAgenciesPVM.AgenciesVM.CreateTime = PersianDate.TimeNow;
                    addToAgenciesPVM.AgenciesVM.UserIdCreator = this.userId.Value;
                    addToAgenciesPVM.AgenciesVM.IsActivated = true;
                    addToAgenciesPVM.AgenciesVM.IsDeleted = false;
                }

                int agencyId = teniacoApiBusiness.AddToAgencies(
                   addToAgenciesPVM.AgenciesVM,
                   consoleBusiness);


                if (agencyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if (agencyId > 0)
                {
                    addToAgenciesPVM.AgenciesVM.AgencyId = agencyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToAgenciesPVM.AgenciesVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else if (agencyId.Equals(0)) // با این شماره موبایل فقط میتواند یک رکورد ثبت کند
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "OnRecordOnly";

                }
                else if (agencyId.Equals(-2)) // مدیر املاک وجود دارد با همین شماره موبایل
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "AgentManager";

                }
                else if (agencyId.Equals(-3)) //با این شماره موبایل یک مشاور دیگر در یک آژانس املاک میباشد
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
        /// UpdateAgencies
        /// </summary>
        /// <param name="updateAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>


        [HttpPost("UpdateAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateAgencies([FromBody] UpdateAgenciesPVM updateAgenciesPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                var agenciesVM = updateAgenciesPVM.AgenciesVM;

                int agencyId = teniacoApiBusiness.UpdateAgencies(
                    ref agenciesVM,
                    updateAgenciesPVM.ChildsUsersIds);

                if (agencyId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateAgency";
                }
                else
                if (agencyId > 0)
                {
                    updateAgenciesPVM.AgenciesVM.AgencyId = agencyId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateAgenciesPVM.AgenciesVM;
                }

                return new JsonResult(jsonResultWithRecordObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }



        /// <summary>
        /// ToggleActivationAgencies
        /// </summary>
        /// <param name="toggleActivationAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationAgencies([FromBody] ToggleActivationAgenciesPVM toggleActivationAgenciesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationAgencies(

                    toggleActivationAgenciesPVM.AgencyId,
                    toggleActivationAgenciesPVM.UserId.Value,
                    consoleBusiness,
                    toggleActivationAgenciesPVM.ChildsUsersIds))
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
        /// TemporaryDeleteAgencies
        /// </summary>
        /// <param name="temporaryDeleteAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteAgencies([FromBody] TemporaryDeleteAgenciesPVM temporaryDeleteAgenciesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteAgencies(
                    temporaryDeleteAgenciesPVM.AgencyId,
                    temporaryDeleteAgenciesPVM.UserId.Value,
                    consoleBusiness,
                    temporaryDeleteAgenciesPVM.ChildsUsersIds))
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
        /// CompleteDeleteAgencies
        /// </summary>
        /// <param name="completeDeleteAgenciesPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteAgencies")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteAgencies([FromBody] CompleteDeleteAgenciesPVM completeDeleteAgenciesPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteAgencies(
                    completeDeleteAgenciesPVM.AgencyId,
                    completeDeleteAgenciesPVM.UserId.Value,
                    completeDeleteAgenciesPVM.ChildsUsersIds,
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



        /// <summary>
        /// GetAgencyWithAgencyId
        /// </summary>
        /// <param name="getAgencyStaffWithAgencyStaffIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetAgencyWithAgencyId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAgencyWithAgencyId([FromBody] GetAgencyWithAgencyIdPVM
            getAgencyWithAgencyIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAgencyWithAgencyIdPVM.ChildsUsersIds == null)
                    {
                        getAgencyWithAgencyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getAgencyWithAgencyIdPVM.ChildsUsersIds.Count == 0)
                        getAgencyWithAgencyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getAgencyWithAgencyIdPVM.ChildsUsersIds.Count == 1)
                        if (getAgencyWithAgencyIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAgencyWithAgencyIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var agencyStaff = teniacoApiBusiness.GetAgencyWithAgencyId(
                    getAgencyWithAgencyIdPVM.AgencyId,
                    getAgencyWithAgencyIdPVM.ChildsUsersIds);

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
    }
}




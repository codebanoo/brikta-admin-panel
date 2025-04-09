using APIs.Automation.Models.Business;
using APIs.Core.Controllers;
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
using VM.Teniaco;

namespace APIs.Teniaco.Controllers
{
    /// <summary>
    /// InvestorsManagement
    /// </summary>
    /// [CustomApiAuthentication]
    /// 

    public class InvestorsManagementController : ApiBaseController
    {
        /// <summary>
        /// InvestorsManagement
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
        public InvestorsManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllInvestorsList
        /// </summary>
        /// <param name="getAllInvestorsListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetAllInvestorsList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult GetAllInvestorsList([FromBody] GetAllInvestorsListPVM getAllInvestorsListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllInvestorsListPVM.ChildsUsersIds == null)
                    {
                        getAllInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllInvestorsListPVM.ChildsUsersIds.Count == 0)
                        getAllInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllInvestorsListPVM.ChildsUsersIds.Count == 1)
                        if (getAllInvestorsListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllInvestorsListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }


                int listCount = 0;

                var listOfInvestors = teniacoApiBusiness.GetAllInvestorsList(
                 getAllInvestorsListPVM.ChildsUsersIds,
                 getAllInvestorsListPVM.UserId,
                 getAllInvestorsListPVM.CompanyName,
                 getAllInvestorsListPVM.FundId);


                //var persons = publicApiBusiness.PublicApiDb.Persons.Where(p => personIds.Contains(p.PersonId)).Distinct().ToList();
                var userIds = listOfInvestors.Select(p => p.UserId).ToList();
                var users = consoleBusiness.CmsDb.UsersProfile.Where(c => userIds.Contains(c.UserId)).ToList();


                foreach (var invest in listOfInvestors)
                {
                    //if (persons.Where(p => p.PersonId.Equals(invest.PersonId)).Any())
                    //{
                    //    var person = persons.Where(p => p.PersonId.Equals(invest.PersonId)).FirstOrDefault();

                    //    invest.PersonsVM = new PersonsVM();
                    //    invest.PersonsVM.PersonId = person.PersonId;
                    //    invest.PersonsVM.Name = person.Name;
                    //    invest.PersonsVM.Family = person.Family;
                    //    invest.PersonsVM.Phone = person.Phone;
                    //    invest.PersonsVM.Mobail = person.Mobail;

                    //}


                    if (users.Where(u => u.UserId.Equals(invest.UserId)).Any())
                    {
                        var user = users.Where(u => u.UserId.Equals(invest.UserId)).FirstOrDefault();

                        invest.CustomUsersVM = new CustomUsersVM();
                        invest.CustomUsersVM.UserId = user.UserId;
                        invest.CustomUsersVM.Name = user.Name;
                        invest.CustomUsersVM.Family = user.Family;
                        invest.CustomUsersVM.Mobile = user.Mobile;

                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfInvestors;
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
        /// GetListOfInvestors
        /// </summary>
        /// <param name="getListOfInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("GetListOfInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfInvestors([FromBody] GetListOfInvestorsPVM getListOfInvestorsPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM =
                 new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfInvestorsPVM.ChildsUsersIds == null)
                    {
                        getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getListOfInvestorsPVM.ChildsUsersIds.Count == 0)
                        getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getListOfInvestorsPVM.ChildsUsersIds.Count == 1)
                        if (getListOfInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                }

                int listCount = 0;


                var listOfInvestors = teniacoApiBusiness.GetListOfInvestors(
                getListOfInvestorsPVM.jtStartIndex.Value,
                getListOfInvestorsPVM.jtPageSize.Value,
                ref listCount,
                getListOfInvestorsPVM.ChildsUsersIds,
                getListOfInvestorsPVM.UserId,
                getListOfInvestorsPVM.CompanyName,
                getListOfInvestorsPVM.FundId,
                getListOfInvestorsPVM.jtSorting);


                var guildCategories = publicApiBusiness.PublicApiDb.GuildCategories.Where(c => c.GuildCategoryId.Equals(c.GuildCategoryId)).ToList();

                var guildCategoryIds = listOfInvestors.Select(c => c.GuildCategoryId).ToList();

                //var personIds = listOfInvestors.Select(p => p.PersonId).ToList();
                //var persons = publicApiBusiness.PublicApiDb.Persons.Where(p => personIds.Contains(p.PersonId)).ToList();

                var funds = teniacoApiBusiness.TeniacoApiDb.Funds.Where(f => f.FundId.Equals(f.FundId)).ToList();

                var fundIds = listOfInvestors.Select(f => f.FundId).ToList();

                var userIds = listOfInvestors.Where(p => p.UserId.HasValue).Select(p => p.UserId.Value).ToList();

                var users = consoleBusiness.GetUsersWithUserIds(userIds);
                var usersProfiles = consoleBusiness.CmsDb.UsersProfile.Where(c => userIds.Contains(c.UserId)).ToList();


                foreach (var invest in listOfInvestors)
                {


                    if (users.Where(u => u.UserId.Equals(invest.UserId)).Any())
                    {
                        var user = users.Where(u => u.UserId.Equals(invest.UserId)).FirstOrDefault();
                        var userProfile = usersProfiles.Where(u => u.UserId.Equals(invest.UserId)).FirstOrDefault();


                        invest.CustomUsersVM = new CustomUsersVM();
                        invest.CustomUsersVM.UserId = user.UserId;
                        invest.CustomUsersVM.UserName = user.UserName;
                        invest.CustomUsersVM.Password = user.Password;
                        invest.CustomUsersVM.Email = user.Email;
                        invest.CustomUsersVM.Name = userProfile.Name;
                        invest.CustomUsersVM.Family = userProfile.Family;
                        invest.CustomUsersVM.Mobile = userProfile.Mobile;
                        invest.CustomUsersVM.NationalCode = userProfile.NationalCode;
                    }

                    if (funds.Where(f => f.FundId.Equals(invest.FundId)).Any())
                    {
                        var fund = funds.Where(f => f.FundId.Equals(invest.FundId)).FirstOrDefault();

                        invest.FundsVM = new FundsVM();
                        invest.FundsVM.FundId = fund.FundId;
                        invest.FundsVM.FundName = fund.FundName;
                    }

                    if (guildCategories.Where(c => c.GuildCategoryId.Equals(invest.GuildCategoryId)).Any())
                    {
                        var guildCategory = guildCategories.Where(c => c.GuildCategoryId.Equals(invest.GuildCategoryId)).FirstOrDefault();

                        invest.GuildCategoriesVM = new GuildCategoriesVM();
                        invest.GuildCategoriesVM.GuildCategoryId = guildCategory.GuildCategoryId;
                        invest.GuildCategoriesVM.GuildCategoryName = guildCategory.GuildCategoryName;
                    }
                }

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfInvestors;
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
        /// AddToInvestors
        /// </summary>
        /// <param name="addToInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("AddToInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToInvestors([FromBody] AddToInvestorsPVM addToInvestorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToInvestorsPVM.ChildsUsersIds == null)
                    {
                        addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    {
                        if (addToInvestorsPVM.ChildsUsersIds.Count == 0)
                        {
                            addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }
                        else
                        {
                            if (addToInvestorsPVM.ChildsUsersIds.Count == 1)
                                if (addToInvestorsPVM.ChildsUsersIds.FirstOrDefault() == 0)
                                    addToInvestorsPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                        }

                    }
                    addToInvestorsPVM.InvestorsVM.CreateEnDate = DateTime.Now;
                    addToInvestorsPVM.InvestorsVM.CreateTime = PersianDate.TimeNow;
                    addToInvestorsPVM.InvestorsVM.UserIdCreator = this.userId.Value;
                    addToInvestorsPVM.InvestorsVM.IsActivated = true;
                    addToInvestorsPVM.InvestorsVM.IsDeleted = false;
                }


                
                int investorId = teniacoApiBusiness.AddToInvestors(
                    addToInvestorsPVM.InvestorsVM,
                    consoleBusiness,
                    "my.teniaco.com"
                     /*this.domainsSettings.DomainName*/);

                if (investorId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
                else
                if (investorId > 0)
                {
                    addToInvestorsPVM.InvestorsVM.InvestorId = investorId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = addToInvestorsPVM.InvestorsVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }


                #region comments


                //var person = publicApiBusiness.PublicApiDb.Persons.Where(p => p.PersonId.Equals(addToInvestorsPVM.InvestorsVM.PersonId)).FirstOrDefault();

                //if (person != null)
                //{
                //    addToInvestorsPVM.InvestorsVM.PersonsVM.Name = person.Name;
                //    addToInvestorsPVM.InvestorsVM.PersonsVM.Family = person.Family;
                //    addToInvestorsPVM.InvestorsVM.PersonsVM.Mobail = person.Mobail;
                //    addToInvestorsPVM.InvestorsVM.PersonsVM.Phone = person.Phone;


                //}



                //if (!string.IsNullOrEmpty(addToInvestorsPVM.InvestorsVM.CustomUsersVM.UserName) &&
                //!string.IsNullOrEmpty(addToInvestorsPVM.InvestorsVM.CustomUsersVM.Password) &&
                //!string.IsNullOrEmpty(addToInvestorsPVM.InvestorsVM.CustomUsersVM.ReplyPassword) &&
                //(addToInvestorsPVM.InvestorsVM.CustomUsersVM.Password == addToInvestorsPVM.InvestorsVM.CustomUsersVM.ReplyPassword) &&
                //!consoleBusiness.ExistUserWithUserName(addToInvestorsPVM.InvestorsVM.CustomUsersVM.UserName, this.domainsSettings.DomainName))
                //{

                //    int investorId = teniacoApiBusiness.AddToInvestors(
                //    addToInvestorsPVM.InvestorsVM,
                //    consoleBusiness,
                //     this.domainsSettings.DomainName);

                //    if (investorId.Equals(-1))
                //    {
                //        jsonResultWithRecordObjectVM.Result = "ERROR";
                //        jsonResultWithRecordObjectVM.Message = "DuplicateProperty";

                //        return new JsonResult(jsonResultWithRecordObjectVM);
                //    }
                //    else
                //    if (investorId > 0)
                //    {
                //        addToInvestorsPVM.InvestorsVM.InvestorId = investorId;
                //        jsonResultWithRecordObjectVM.Result = "OK";
                //        jsonResultWithRecordObjectVM.Record = addToInvestorsPVM.InvestorsVM;

                //        return new JsonResult(jsonResultWithRecordObjectVM);
                //    }

                // }

                #endregion




            }
            catch (Exception)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }

     


        /// <summary>
        /// UpdateInvestors
        /// </summary>
        /// <param name="updateInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("UpdateInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult UpdateInvestors([FromBody] UpdateInvestorsPVM updateInvestorsPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });


            try
            {
                var investorsVM = updateInvestorsPVM.InvestorsVM;

                int investorsId = teniacoApiBusiness.UpdateInvestors(
                   ref investorsVM,
                   updateInvestorsPVM.ChildsUsersIds);


                if (investorsId.Equals(-1))
                {
                    jsonResultWithRecordObjectVM.Result = "ERROR";
                    jsonResultWithRecordObjectVM.Message = "DuplicateInvestor";
                }
                else
                if (investorsId > 0)
                {
                    updateInvestorsPVM.InvestorsVM.InvestorId = investorsId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Record = updateInvestorsPVM.InvestorsVM;
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
        /// ToggleActivationInvestors
        /// </summary>
        /// <param name="toggleActivationInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("ToggleActivationInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult ToggleActivationInvestors([FromBody] ToggleActivationInvestorsPVM toggleActivationInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string returnMessage = "";

                if (teniacoApiBusiness.ToggleActivationInvestors(

                   toggleActivationInvestorsPVM.InvestorId,
                   toggleActivationInvestorsPVM.UserId.Value,
                   consoleBusiness,
                   toggleActivationInvestorsPVM.ChildsUsersIds))
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
        /// TemporaryDeleteInvestors
        /// </summary>
        /// <param name="temporaryDeleteInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("TemporaryDeleteInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]

        public IActionResult TemporaryDeleteInvestors([FromBody] TemporaryDeleteInvestorsPVM temporaryDeleteInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                jsonResultObjectVM.Result = "ERROR";

                if (teniacoApiBusiness.TemporaryDeleteInvestors(
                    temporaryDeleteInvestorsPVM.InvestorId,
                    temporaryDeleteInvestorsPVM.UserId.Value,
                    temporaryDeleteInvestorsPVM.ChildsUsersIds))
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
        /// CompleteDeleteInvestors
        /// </summary>
        /// <param name="completeDeleteInvestorsPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>

        [HttpPost("CompleteDeleteInvestors")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]


        public IActionResult CompleteDeleteInvestors([FromBody] CompleteDeleteInvestorsPVM completeDeleteInvestorsPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (teniacoApiBusiness.CompleteDeleteInvestors(
                   completeDeleteInvestorsPVM.InvestorId,
                   completeDeleteInvestorsPVM.ChildsUsersIds))
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
        /// GetInvestorWithInvestorId
        /// </summary>
        /// <param name="getInvestorWithInvestorIdPVM"></param>
        /// <returns>JsonResultWithRecordObjectVM</returns>

        [HttpPost("GetInvestorWithInvestorId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetInvestorWithInvestorId([FromBody] GetInvestorWithInvestorIdPVM
            getInvestorWithInvestorIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getInvestorWithInvestorIdPVM.ChildsUsersIds == null)
                    {
                        getInvestorWithInvestorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getInvestorWithInvestorIdPVM.ChildsUsersIds.Count == 0)
                        getInvestorWithInvestorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getInvestorWithInvestorIdPVM.ChildsUsersIds.Count == 1)
                        if (getInvestorWithInvestorIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getInvestorWithInvestorIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var investor = teniacoApiBusiness.GetInvestorWithInvestorId(
                    getInvestorWithInvestorIdPVM.InvestorId,
                    getInvestorWithInvestorIdPVM.ChildsUsersIds);



                var userId = investor.UserId;
                var user = consoleBusiness.GetUserWithUserId(userId.Value);
                var usersProfile = consoleBusiness.CmsDb.UsersProfile.Where(c => c.UserId.Equals(userId)).FirstOrDefault();


                CustomUsersVM customUsersVM = new CustomUsersVM();


                customUsersVM.UserName = user.UserName;
                customUsersVM.Email = user.Email;

                customUsersVM.Name = usersProfile.Name;
                customUsersVM.Family = usersProfile.Family;
                customUsersVM.Mobile = usersProfile.Mobile;
                customUsersVM.NationalCode = usersProfile.NationalCode;

                investor.CustomUsersVM = customUsersVM;

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = investor;

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

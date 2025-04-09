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
using VM.PVM.Melkavan;

namespace APIs.Melkavan.Controllers
{
    /// <summary>
    /// BannersManagement
    /// </summary>
    [CustomApiAuthentication]
    public class BannersManagementController : ApiBaseController
    {
        /// <summary>
        /// BannersManagement
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
        public BannersManagementController(IHostEnvironment _hostingEnvironment,
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
        /// GetAllBanners
        /// </summary>
        /// <param name="getAllBannersListPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetAllBannersList")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllBannersList(GetAllBannersListPVM getAllBannersListPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getAllBannersListPVM.ChildsUsersIds == null)
                    {
                        getAllBannersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getAllBannersListPVM.ChildsUsersIds.Count == 0)
                        getAllBannersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getAllBannersListPVM.ChildsUsersIds.Count == 1)
                        if (getAllBannersListPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getAllBannersListPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                var listOfBanners = melkavanApiBusiness.GetAllBannersList(getAllBannersListPVM.BannerTitle);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfBanners;
                //jsonResultWithRecordsObjectVM.Records = JsonConvert.SerializeObject(listOfCities);

                return new JsonResult(jsonResultWithRecordsObjectVM);
                //return jsonResultWithRecordsObjectVM;
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }







        /// <summary>
        /// GetListOfBanners
        /// </summary>
        /// <param name="getListOfBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetListOfBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetListOfBanners(GetListOfBannersPVM getListOfBannersPVM)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getListOfBannersPVM.ChildsUsersIds == null)
                    {
                        getListOfBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                  if (getListOfBannersPVM.ChildsUsersIds.Count == 0)
                        getListOfBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                  if (getListOfBannersPVM.ChildsUsersIds.Count == 1)
                        if (getListOfBannersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getListOfBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }
                int listCount = 0;
                var listOfBanners = melkavanApiBusiness.GetListOfBanners(
                    getListOfBannersPVM.jtStartIndex.Value,
                    getListOfBannersPVM.jtPageSize.Value,
                    ref listCount,
                    getListOfBannersPVM.BannerTitle,
                    getListOfBannersPVM.jtSorting);

                jsonResultWithRecordsObjectVM.Result = "OK";
                jsonResultWithRecordsObjectVM.Records = listOfBanners;
                jsonResultWithRecordsObjectVM.TotalRecordCount = listCount;

                return new JsonResult(jsonResultWithRecordsObjectVM);
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordsObjectVM.Result = "ERROR";
            jsonResultWithRecordsObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordsObjectVM);
            //return jsonResultWithRecordsObjectVM;
        }





        /// <summary>
        /// AddToBanners
        /// </summary>
        /// <param name="addToBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("AddToBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult AddToBanners(AddToBannersPVM addToBannersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (addToBannersPVM.ChildsUsersIds == null)
                    {
                        addToBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (addToBannersPVM.ChildsUsersIds.Count == 0)
                        addToBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (addToBannersPVM.ChildsUsersIds.Count == 1)
                        if (addToBannersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            addToBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                addToBannersPVM.BannersVM.CreateEnDate = DateTime.Now;
                addToBannersPVM.BannersVM.CreateTime = PersianDate.TimeNow;
                //addToBannersPVM.BannersVM.UserIdCreator = this.userId.Value;
                addToBannersPVM.BannersVM.IsActivated = true;
                addToBannersPVM.BannersVM.IsDeleted = false;

                int bannerId = melkavanApiBusiness.AddToBanners(addToBannersPVM.BannersVM);
                if (bannerId != 0)
                {
                    addToBannersPVM.BannersVM.BannerId = bannerId;
                    jsonResultWithRecordObjectVM.Result = "OK";
                    jsonResultWithRecordObjectVM.Message = "Success";
                    jsonResultWithRecordObjectVM.Record = addToBannersPVM.BannersVM;

                    return new JsonResult(jsonResultWithRecordObjectVM);
                }
            }
            catch (Exception exc)
            { }

            jsonResultWithRecordObjectVM.Result = "ERROR";
            jsonResultWithRecordObjectVM.Message = "ErrorInService";

            return new JsonResult(jsonResultWithRecordObjectVM);
        }





        /// <summary>
        /// UpdateBanners
        /// </summary>
        /// <param name="updateBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("UpdateBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateBanners(UpdateBannersPVM updateBannersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (updateBannersPVM.ChildsUsersIds == null)
                    {
                        updateBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (updateBannersPVM.ChildsUsersIds.Count == 0)
                        updateBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (updateBannersPVM.ChildsUsersIds.Count == 1)
                        if (updateBannersPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            updateBannersPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);

                    updateBannersPVM.BannersVM.EditEnDate = DateTime.Now;
                    updateBannersPVM.BannersVM.EditTime = PersianDate.TimeNow;
                    updateBannersPVM.BannersVM.UserIdEditor = this.userId.Value;
                }

                //updateBannersPVM.BannersVM.UserIdEditor = this.userId.Value;

                if (melkavanApiBusiness.UpdateBanners(
                    updateBannersPVM.BannersVM,
                    updateBannersPVM.ChildsUsersIds))
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
        /// ToggleActivationBanners
        /// </summary>
        /// <param name="toggleActivationBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("ToggleActivationBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult ToggleActivationBanners(ToggleActivationBannersPVM toggleActivationBannersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {

                if (melkavanApiBusiness.ToggleActivationBanners(
                    toggleActivationBannersPVM.BannerId,
                    toggleActivationBannersPVM.UserId.Value,
                    toggleActivationBannersPVM.ChildsUsersIds
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
        /// TemporaryDeleteBanners
        /// </summary>
        /// <param name="temporaryDeleteBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("TemporaryDeleteBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult TemporaryDeleteBanners(TemporaryDeleteBannersPVM temporaryDeleteBannersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (melkavanApiBusiness.TemporaryDeleteBanners(
                    temporaryDeleteBannersPVM.BannerId,
                    temporaryDeleteBannersPVM.UserId.Value,
                    temporaryDeleteBannersPVM.ChildsUsersIds
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
        /// CompleteDeleteBanners
        /// </summary>
        /// <param name="completeDeleteBannersPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("CompleteDeleteBanners")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult CompleteDeleteBanners(CompleteDeleteBannersPVM completeDeleteBannersPVM)
        {
            JsonResultObjectVM jsonResultObjectVM =
                new JsonResultObjectVM(new object() { });

            try
            {
                if (melkavanApiBusiness.CompleteDeleteBanners(
                    completeDeleteBannersPVM.BannerId,
                    completeDeleteBannersPVM.UserId.Value,
                    completeDeleteBannersPVM.ChildsUsersIds
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
        /// GetBannersByBannerId
        /// </summary>
        /// <param name="getBannerWithBannerIdPVM"></param>
        /// <returns>JsonResultWithRecordsObjectVM</returns>
        [HttpPost("GetBannersByBannerId")]
        [ProducesResponseType(typeof(JsonResultWithRecordsObjectVM), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public IActionResult GetBannersByBannerId(GetBannersWithBannerIdPVM getBannerWithBannerIdPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (getBannerWithBannerIdPVM.ChildsUsersIds == null)
                    {
                        getBannerWithBannerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    }
                    else
                    if (getBannerWithBannerIdPVM.ChildsUsersIds.Count == 0)
                        getBannerWithBannerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                    else
                    if (getBannerWithBannerIdPVM.ChildsUsersIds.Count == 1)
                        if (getBannerWithBannerIdPVM.ChildsUsersIds.FirstOrDefault() == 0)
                            getBannerWithBannerIdPVM.ChildsUsersIds = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainAdminId.Value);
                }

                var banner = melkavanApiBusiness.GetBannersByBannerId(
                    getBannerWithBannerIdPVM.BannerId,
                    getBannerWithBannerIdPVM.ChildsUsersIds);

                jsonResultWithRecordObjectVM.Result = "OK";
                jsonResultWithRecordObjectVM.Record = banner;

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

using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;
using VM.Projects;
using VM.PVM.Projects;
using ApiCallers.ProjectsApiCaller;
using System.IO;
using System.Dynamic;
using VM.Console;

namespace Web.Projects.Areas.AdminProjects.Controllers
{
    [Area("AdminProjects")]
    public class ConstructionProjectsManagementController : ExtraAdminController
    {
        public ConstructionProjectsManagementController(IHostEnvironment _hostEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConfigurationRoot _configurationRoot,
            IMapper _mapper,
            IConsoleBusiness _consoleBusiness,
            IPublicServicesBusiness _publicServicesBusiness,
            IMemoryCache _memoryCache,
            IDistributedCache _distributedCache) :
            base(_hostEnvironment,
            _httpContextAccessor,
            _actionContextAccessor,
            _configurationRoot,
            _mapper,
            _consoleBusiness,
            _publicServicesBusiness,
            _memoryCache,
            _distributedCache)
        {
        }
        public IActionResult Index()
        {
            ViewData["Title"] = "پروژه های ساخت";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            ///<summary>
            ///کمبوی ملک
            ///</summary>

            if (ViewData["PropertiesList"] == null)
            {
                List<PropertiesVM> PropertiesVMList = new List<PropertiesVM>();

                try
                {
                    GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM = new GetAllPropertiesListWithoutAddressPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PropertiesManagement", "GetAllPropertiesListWithoutAddress", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                    serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetAllPropertiesListWithoutAddress";
                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertiesListWithoutAddress(getAllPropertiesListWithoutAddressPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                PropertiesVMList = jArray.ToObject<List<PropertiesVM>>();


                                if (PropertiesVMList != null)
                                    if (PropertiesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertiesList"] = PropertiesVMList;
            }

            /// <summary>
            ///کمبوی نماینده
            ///لیستی از کاربران
            ///دسترسی نماینده هم شامل میشود
            /// </summary>

            if (ViewData["CustomUsersList"] == null)
            {
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
                try
                {
                    List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                    var getLevelId = consoleBusiness.GetLevelDetailWithLevelName("نماینده");
                    var usersIdsInUserLevels = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId.LevelId)).Select(f => f.UserId).ToList();


                    /// <summary>
                    /// کاربرانی که دسترسی نماینده دارند
                    /// و نقش user
                    ///و با دامنه ی تنیاکو وارد شدند
                    /// </summary>


                    var usersList = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                    usersIdsInUserLevels.Contains(u.UserId)).AsQueryable();

                    if (childsUsersIds != null)
                    {
                        if (childsUsersIds.Count > 1)
                        {
                            usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                        }
                        else
                        {
                            if (childsUsersIds.Count == 1)
                            {
                                if (childsUsersIds.FirstOrDefault() > 0)
                                {
                                    usersList = usersList.Where(c => childsUsersIds.Contains(c.UserIdCreator.Value));
                                }
                            }
                        }
                    }


                    ViewData["CustomUsersList"] = usersList.Select(s => new CustomUsersVM
                    {
                        UserId = s.UserId,
                        Name = s.UsersProfileUser.Name,
                        Family = s.UsersProfileUser.Family,
                        UserName = s.UserName,
                        Phone = s.UsersProfileUser.Phone,
                        Mobile = s.UsersProfileUser.Mobile,

                    }).ToList();
                }

                catch (Exception exc)
                { }


            }

            //نوع پروژه
            if (ViewData["ConstructionProjectTypesList"] == null)
            {
                List<ConstructionProjectTypesVM> constructionProjectVMList = new List<ConstructionProjectTypesVM>();

                GetAllConstructionProjectTypesListPVM getAllConstructionProjectTypesListPVM = new GetAllConstructionProjectTypesListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminProjects", "ConstructionProjectTypesManagement", "GetAllConstructionProjectTypesList", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectTypesManagement/GetAllConstructionProjectTypesList";

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectTypesList(getAllConstructionProjectTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectVMList = jArray.ToObject<List<ConstructionProjectTypesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ConstructionProjectTypesList"] = constructionProjectVMList;
            }

            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }





        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllConstructionProjectsList(
             string? constructionProjectTitle = "")
        {

            try
            {
                List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/GetAllConstructionProjectsList";

                    GetAllConstructionProjectsListPVM getAllConstructionProjectsListPVM = new GetAllConstructionProjectsListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                        ConstructionProjectTitle = constructionProjectTitle,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectsList(getAllConstructionProjectsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectsVMList = jArray.ToObject<List<ConstructionProjectsVM>>();


                                if (constructionProjectsVMList != null)
                                    if (constructionProjectsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConstructionProjectsVM>>();

                                        if (records.Count > 0)
                                        {
                                            var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                            var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                            foreach (var record in records)
                                            {
                                                if (record.UserIdCreator.HasValue)
                                                {
                                                    var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                                    if (customUser != null)
                                                    {
                                                        record.UserCreatorName = customUser.UserName;

                                                        if (!string.IsNullOrEmpty(customUser.Name))
                                                            record.UserCreatorName += " " + customUser.Name;

                                                        if (!string.IsNullOrEmpty(customUser.Family))
                                                            record.UserCreatorName += " " + customUser.Family;
                                                    }
                                                }
                                            }
                                        }

                                        #endregion




                                        return Json(new
                                        {
                                            Result = jsonResultWithRecordsObjectVM.Result,
                                            Records = records,//jsonResultWithRecordsObjectVM.Records,
                                            TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                                        });
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfConstructionProjects(
        int jtStartIndex = 0,
        int jtPageSize = 10,
        string constructionProjectTitle = "",
        string jtSorting = null)
        {

            try
            {
                List<ConstructionProjectsVM> constructionProjectsVMList = new List<ConstructionProjectsVM>();

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/GetListOfConstructionProjects";
                    GetListOfConstructionProjectsPVM getListOfConstructionProjectsPVM = new GetListOfConstructionProjectsPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        ConstructionProjectTitle = constructionProjectTitle,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetListOfConstructionProjects(getListOfConstructionProjectsPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectsVMList = jArray.ToObject<List<ConstructionProjectsVM>>();


                                if (constructionProjectsVMList != null)
                                    if (constructionProjectsVMList.Count >= 0)
                                    {
                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<ConstructionProjectsVM>>();

                                        if (records.Count > 0)
                                        {
                                            var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                            var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                            foreach (var record in records)
                                            {
                                                if (record.UserIdCreator.HasValue)
                                                {
                                                    var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                                    if (customUser != null)
                                                    {
                                                        record.UserCreatorName = customUser.UserName;

                                                        if (!string.IsNullOrEmpty(customUser.Name))
                                                            record.UserCreatorName += " " + customUser.Name;

                                                        if (!string.IsNullOrEmpty(customUser.Family))
                                                            record.UserCreatorName += " " + customUser.Family;
                                                    }
                                                }
                                            }
                                        }

                                        #endregion




                                        return Json(new
                                        {
                                            Result = jsonResultWithRecordsObjectVM.Result,
                                            Records = records,//jsonResultWithRecordsObjectVM.Records,
                                            TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                                        });
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }
            }
            catch (Exception)
            {

                throw;
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }




        public IActionResult AddToConstructionProjects()
        {
            ViewData["Title"] = "تعریف پروژه ساخت";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            /// <summary>
            ///کمبوی سهامداران
            ///لیستی از سرمایه گذاران
            ///شرط دسترسی دارد
            /// </summary>

            if (ViewData["InvestorsList"] == null)
            {
                List<InvestorsVM> investorsVMList = new List<InvestorsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                    GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                    {

                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetAllInvestorsList", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                investorsVMList = jArray.ToObject<List<InvestorsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["InvestorsList"] = investorsVMList;
            }



            /// <summary>
            ///کمبوی نماینده
            ///لیستی از کاربران
            ///دسترسی نماینده هم شامل میشود
            /// </summary>

            if (ViewData["CustomUsersList"] == null)
            {
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
                try
                {
                    //List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                    var getLevelId = consoleBusiness.GetLevelDetailWithLevelName("نماینده");
                    var usersIdsInUserLevels = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId.LevelId)).Select(f => f.UserId).ToList();


                    /// <summary>
                    /// کاربرانی که دسترسی نماینده دارند
                    /// و نقش user
                    ///و با دامنه ی تنیاکو وارد شدند
                    /// </summary>


                    var usersList = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                    usersIdsInUserLevels.Contains(u.UserId)).AsQueryable();


                    #region comments - old codes

                    //if (childsUsersIds != null)
                    //{
                    //    if (childsUsersIds.Count > 1)
                    //    {
                    //        usersList = usersList.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
                    //    }
                    //    else
                    //    {
                    //        if (childsUsersIds.Count == 1)
                    //        {
                    //            if (childsUsersIds.FirstOrDefault() > 0)
                    //            {
                    //                usersList = usersList.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion


                    ViewData["CustomUsersList"] = usersList.Select(s => new CustomUsersVM
                    {
                        UserId = s.UserId,
                        Name = s.UsersProfileUser.Name,
                        Family = s.UsersProfileUser.Family,
                        UserName = s.UserName,
                        Phone = s.UsersProfileUser.Phone,
                        Mobile = s.UsersProfileUser.Mobile,

                    }).ToList();
                }

                catch (Exception exc)
                { }


            }


            ///<summary>
            ///کمبوی ملک
            ///</summary>

            if (ViewData["PropertiesList"] == null)
            {
                List<PropertiesVM> PropertiesVMList = new List<PropertiesVM>();

                try
                {
                    GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM = new GetAllPropertiesListWithoutAddressPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)


                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PropertiesManagement", "GetAllPropertiesListWithoutAddress", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };


                    serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetAllPropertiesListWithoutAddress";
                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertiesListWithoutAddress(getAllPropertiesListWithoutAddressPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                PropertiesVMList = jArray.ToObject<List<PropertiesVM>>();


                                if (PropertiesVMList != null)
                                    if (PropertiesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertiesList"] = PropertiesVMList;
            }

            ///<summary>
            ///نوع پروژه
            ///</summary>

            if (ViewData["ConstructionProjectTypesList"] == null)
            {
                List<ConstructionProjectTypesVM> constructionProjectVMList = new List<ConstructionProjectTypesVM>();

                GetAllConstructionProjectTypesListPVM getAllConstructionProjectTypesListPVM = new GetAllConstructionProjectTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminProjects", "ConstructionProjectTypesManagement", "GetAllConstructionProjectTypesList", this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectTypesManagement/GetAllConstructionProjectTypesList";

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectTypesList(getAllConstructionProjectTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectVMList = jArray.ToObject<List<ConstructionProjectTypesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ConstructionProjectTypesList"] = constructionProjectVMList;
            }



            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/ConstructionProjectsManagement/Index";
            }

            ConstructionProjectsVM constructionProjectsVM = new ConstructionProjectsVM();


            dynamic expando = new ExpandoObject();
            expando = constructionProjectsVM;

            return View("AddTo", expando);
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToConstructionProjects(ConstructionProjectsVM constructionProjectsVM/*, int? PersonId = 0*/)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectsVM.CreateEnDate = DateTime.Now;
                constructionProjectsVM.CreateTime = PersianDate.TimeNow;
                constructionProjectsVM.UserIdCreator = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/AddToConstructionProjects";

                AddToConstructionProjectsPVM addToConstructionProjectsPVM = new AddToConstructionProjectsPVM()
                {
                    ConstructionProjectsVM = constructionProjectsVM,
                    //PersonId = PersonId,
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).AddToConstructionProjects(addToConstructionProjectsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ConstructionProjectsVM>();

                        if (record != null)
                        {
                            var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                            #region Crreate folders

                            string constructionProjectsFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles";
                            if (!Directory.Exists(constructionProjectsFolder))
                            {
                                Directory.CreateDirectory(constructionProjectsFolder);
                            }


                            string thisConstructionProjectsFolder = constructionProjectsFolder + "\\" + domainSettings.DomainName + "\\" + record.ConstructionProjectId;
                            Directory.CreateDirectory(thisConstructionProjectsFolder);

                            string initialPlanFolder = thisConstructionProjectsFolder + "\\InitialPlanFiles";
                            Directory.CreateDirectory(initialPlanFolder);

                            string attachementsFolder = thisConstructionProjectsFolder + "\\AttachementFiles";
                            Directory.CreateDirectory(attachementsFolder);

                            string confirmationAgreementFolder = thisConstructionProjectsFolder + "\\ConfirmationAgreementFiles";
                            Directory.CreateDirectory(confirmationAgreementFolder);

                            string contractAgreementFolder = thisConstructionProjectsFolder + "\\ContractAgreementFiles";
                            Directory.CreateDirectory(contractAgreementFolder);

                            string contractorsAgreementFolder = thisConstructionProjectsFolder + "\\ContractorsAgreementFiles";
                            Directory.CreateDirectory(contractorsAgreementFolder);

                            string meetingBoardFolder = thisConstructionProjectsFolder + "\\MeetingBoardFiles";
                            Directory.CreateDirectory(meetingBoardFolder);

                            string partnershipAgreementFolder = thisConstructionProjectsFolder + "\\PartnershipAgreementFiles";
                            Directory.CreateDirectory(partnershipAgreementFolder);

                            string pitchDeckFolder = thisConstructionProjectsFolder + "\\PitchDeckFiles";
                            Directory.CreateDirectory(pitchDeckFolder);

                            string progressPictureFolder = thisConstructionProjectsFolder + "\\ProgressPictureFiles";
                            Directory.CreateDirectory(progressPictureFolder);


                            #endregion

                            return Json(new
                            {
                                Result = "OK",
                                Record = record,
                                Message = "تعریف انجام شد",
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            { }


            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        public IActionResult UpdateConstructionProjects(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index");

            ViewData["Title"] = "ویرایش پروژه ساخت";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            /// <summary>
            ///کمبوی سهامداران
            ///لیستی از سرمایه گذاران
            ///شرط دسترسی دارد
            /// </summary>

            if (ViewData["InvestorsList"] == null)
            {
                List<InvestorsVM> investorsVMList = new List<InvestorsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                    GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                    {

                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetAllInvestorsList", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                investorsVMList = jArray.ToObject<List<InvestorsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["InvestorsList"] = investorsVMList;
            }



            /// <summary>
            ///کمبوی نماینده
            ///لیستی از کاربران
            ///دسترسی نماینده هم شامل میشود
            /// </summary>

            if (ViewData["CustomUsersList"] == null)
            {
                List<CustomUsersVM> customUsersVMList = new List<CustomUsersVM>();
                try
                {
                    //List<long> childsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("Admin", "UsersManagement", "GetListOfUsers", this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds);

                    var getLevelId = consoleBusiness.GetLevelDetailWithLevelName("نماینده");
                    var usersIdsInUserLevels = consoleBusiness.CmsDb.UsersLevels.Where(u => u.LevelId.Equals(getLevelId.LevelId)).Select(f => f.UserId).ToList();


                    /// <summary>
                    /// کاربرانی که دسترسی نماینده دارند
                    /// و نقش user
                    ///و با دامنه ی تنیاکو وارد شدند
                    /// </summary>


                    var usersList = consoleBusiness.CmsDb.Users.Where(u => u.DomainSettingId.Equals(this.domainsSettings.DomainSettingId) &&
                    usersIdsInUserLevels.Contains(u.UserId)).AsQueryable();

                    #region comments - old codes

                    //if (childsUsersIds != null)
                    //{
                    //    if (childsUsersIds.Count > 1)
                    //    {
                    //        usersList = usersList.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
                    //    }
                    //    else
                    //    {
                    //        if (childsUsersIds.Count == 1)
                    //        {
                    //            if (childsUsersIds.FirstOrDefault() > 0)
                    //            {
                    //                usersList = usersList.Where(c => childsUsersIds.Contains(c.ParentUserId.Value));
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion

                    
                    ViewData["CustomUsersList"] = usersList.Select(s => new CustomUsersVM
                    {
                        UserId = s.UserId,
                        Name = s.UsersProfileUser.Name,
                        Family = s.UsersProfileUser.Family,
                        UserName = s.UserName,
                        Phone = s.UsersProfileUser.Phone,
                        Mobile = s.UsersProfileUser.Mobile,

                    }).ToList();
                }

                catch (Exception exc)
                { }


            }


            ///<summary>
            ///کمبوی ملک
            ///</summary>

            if (ViewData["PropertiesList"] == null)
            {
                List<PropertiesVM> PropertiesVMList = new List<PropertiesVM>();

                try
                {
                    GetAllPropertiesListWithoutAddressPVM getAllPropertiesListWithoutAddressPVM = new GetAllPropertiesListWithoutAddressPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)


                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "PropertiesManagement", "GetAllPropertiesListWithoutAddress", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                    };

                    serviceUrl = teniacoApiUrl + "/api/PropertiesManagement/GetAllPropertiesListWithoutAddress";
                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllPropertiesListWithoutAddress(getAllPropertiesListWithoutAddressPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                PropertiesVMList = jArray.ToObject<List<PropertiesVM>>();


                                if (PropertiesVMList != null)
                                    if (PropertiesVMList.Count > 0)
                                    {
                                    }

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PropertiesList"] = PropertiesVMList;
            }


            ///<summary>
            ///نوع پروژه
            ///</summary>

            if (ViewData["ConstructionProjectTypesList"] == null)
            {
                List<ConstructionProjectTypesVM> constructionProjectVMList = new List<ConstructionProjectTypesVM>();

                GetAllConstructionProjectTypesListPVM getAllConstructionProjectTypesListPVM = new GetAllConstructionProjectTypesListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminProjects", "ConstructionProjectTypesManagement", "GetAllConstructionProjectTypesList", this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                try
                {
                    serviceUrl = projectsApiUrl + "/api/ConstructionProjectTypesManagement/GetAllConstructionProjectTypesList";

                    responseApiCaller = new ProjectsApiCaller(serviceUrl).GetAllConstructionProjectTypesList(getAllConstructionProjectTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                constructionProjectVMList = jArray.ToObject<List<ConstructionProjectTypesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["ConstructionProjectTypesList"] = constructionProjectVMList;
            }


            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminProjects/ConstructionProjectsManagement/Index";
            }

            ConstructionProjectsVM constructionProjectsVM = new ConstructionProjectsVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                GetConstructionProjectWithConstructionProjectIdPVM getConstructionProjectWithConstructionProjectIdPVM = new GetConstructionProjectWithConstructionProjectIdPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),

                    ConstructionProjectId = Id,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminProjects", "ConstructionProjectsManagement", "GetConstructionProjectWithConstructionProjectId", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/GetConstructionProjectWithConstructionProjectId";

                responseApiCaller = new ProjectsApiCaller(serviceUrl).GetConstructionProjectWithConstructionProjectId(getConstructionProjectWithConstructionProjectIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<ConstructionProjectsVM>();


                            if (record != null)
                            {
                                constructionProjectsVM = record;

                                ViewData["constructionProjectsVM"] = constructionProjectsVM;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            dynamic expando = new ExpandoObject();
            expando = constructionProjectsVM;

            return View("Update", expando);
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateConstructionProjects(ConstructionProjectsVM constructionProjectsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                constructionProjectsVM.EditEnDate = DateTime.Now;
                constructionProjectsVM.EditTime = PersianDate.TimeNow;
                constructionProjectsVM.UserIdEditor = this.userId.Value;

                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/UpdateConstructionProjects";

                UpdateConstructionProjectsPVM updateConstructionProjectsPVM = new UpdateConstructionProjectsPVM()
                {
                    ConstructionProjectsVM = constructionProjectsVM,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    //        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value,
                    //PersonId = PersonId,
                    ContractSideTypeId = 1
                };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).UpdateConstructionProjects(updateConstructionProjectsPVM);


                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        JObject jObject = jsonResultWithRecordObjectVM.Record;
                        var record = jObject.ToObject<ConstructionProjectsVM>();

                        if (record != null)
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Record = record,
                                Message = "ویرایش انجام شد",
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            { }


            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleShowInDashboardConstructionProjects(int constructionProjectId = 0)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/ToggleShowInDashboardConstructionProjects";

                ToggleShowInDashboardConstructionProjectsPVM toggleShowInDashboardConstructionProjectsPVM =
                    new ToggleShowInDashboardConstructionProjectsPVM()
                    {
                        ConstructionProjectId = constructionProjectId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleShowInDashboardConstructionProjects(toggleShowInDashboardConstructionProjectsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if(jsonResultWithRecordObjectVM.Result== "ReturnName")
                        {
                            return Json(new { Result = "ReturnName", Record = jsonResultWithRecordObjectVM.Record });
                        }
                        else if(jsonResultWithRecordObjectVM.Result!= "ERROR")
                        {
                            return Json(new { Result = "OK" });
                        }

                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationConstructionProjects(int constructionProjectId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/ToggleActivationConstructionProjects";

                ToggleActivationConstructionProjectsPVM toggleActivationConstructionProjectsPVM =
                    new ToggleActivationConstructionProjectsPVM()
                    {
                        ConstructionProjectId = constructionProjectId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).ToggleActivationConstructionProjects(toggleActivationConstructionProjectsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult TemporaryDeleteConstructionProjects(int constructionProjectId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/TemporaryDeleteConstructionProjects";

                TemporaryDeleteConstructionProjectsPVM temporaryDeleteConstructionProjectsPVM =
                    new TemporaryDeleteConstructionProjectsPVM()
                    {
                        ConstructionProjectId = constructionProjectId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).TemporaryDeleteConstructionProjects(temporaryDeleteConstructionProjectsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new { Result = "OK" });
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }



        [AjaxOnly]
        [HttpPost]
        public IActionResult CompleteDeleteConstructionProjects(int constructionProjectId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = projectsApiUrl + "/api/ConstructionProjectsManagement/CompleteDeleteConstructionProjects";

                CompleteDeleteConstructionProjectsPVM completeDeleteConstructionProjectsPVM =
                    new CompleteDeleteConstructionProjectsPVM()
                    {
                        ConstructionProjectId = constructionProjectId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new ProjectsApiCaller(serviceUrl).CompleteDeleteConstructionProjects(completeDeleteConstructionProjectsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);
                            string constructionProjectsFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\ConstructionProjectFiles";
                            string thisConstructionProjectsFolder = constructionProjectsFolder + "\\" + domainSettings.DomainName + "\\" + constructionProjectId;
                            Directory.Delete(thisConstructionProjectsFolder, true);
                            System.Threading.Thread.Sleep(100);

                            return Json(new { Result = "OK" });
                        }
                        else
                        {
                            if (jsonResultObjectVM.Message == "ERROR_DEPENDENCY")
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "لطفا ابتدا قرارداد های مربوط به این پروژه را حذف کنید."
                                });
                            return Json(new
                            {
                                Result = "ERROR",
                                Message = "خطا"
                            });
                        }
                    }

                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }


        #region persons management

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToPersons(PersonsVM personsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                personsVM.CreateEnDate = DateTime.Now;
                personsVM.CreateTime = PersianDate.TimeNow;
                personsVM.UserIdCreator = this.userId.Value;
                personsVM.IsActivated = true;
                personsVM.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/PersonsManagement/AddToPersons";

                    AddToPersonsPVM addToFormPVM = new AddToPersonsPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        PersonsVM = personsVM
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).AddToPersons(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                personsVM = jObject.ToObject<PersonsVM>();

                                #region Fill UserCreatorName

                                //if (zonesVM.UserIdCreator.HasValue)
                                //{
                                //    var customUser = consoleBusiness.GetCustomUser(zonesVM.UserIdCreator.Value);

                                //    if (customUser != null)
                                //    {
                                //        zonesVM.UserCreatorName = customUser.UserName;

                                //        if (!string.IsNullOrEmpty(customUser.Name))
                                //            zonesVM.UserCreatorName += " " + customUser.Name;

                                //        if (!string.IsNullOrEmpty(customUser.Family))
                                //            zonesVM.UserCreatorName += " " + customUser.Family;
                                //    }
                                //}

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = personsVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicatePerson"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllPersonsList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                jsonResultWithRecordsObjectVM.Result,
                                jsonResultWithRecordsObjectVM.Records,
                                jsonResultWithRecordsObjectVM.TotalRecordCount
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }

        #endregion


        #region PropertyPriceHistories

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetLastPropertiesPriceHistoryByPropertyId(int PropertyId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            PropertiesPricesHistoriesVM propertiesPricesHistoriesVM = new PropertiesPricesHistoriesVM();


            try
            {
                string serviceUrl = teniacoApiUrl + "/api/PropertiesPricesHistoriesManagement/GetLastPropertiesPriceHistoryByPropertyId";

                GetListOfPropertiesPricesHistoriesPVM getListOfPropertiesPricesHistoriesPVM = new GetListOfPropertiesPricesHistoriesPVM()
                {
                    PropertyId = PropertyId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),

                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetLastPropertiesPriceHistoryByPropertyId(getListOfPropertiesPricesHistoriesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<PropertiesPricesHistoriesVM>();


                            if (record != null)
                            {
                                
                                var propertyYear = record.CreateEnDate.Value.Year;
                                var propertyMonth = record.CreateEnDate.Value.Month;
                                var propertyDay = record.CreateEnDate.Value.Day;

                                var todayYear = DateTime.Now.Year;
                                var todayMonth = DateTime.Now.Month;
                                var todayDay = DateTime.Now.Day;


                                if (((todayYear - propertyYear) * 12) + ((todayMonth - propertyMonth) * 30) + (todayDay - propertyDay) <= 30)
                                {
                                    propertiesPricesHistoriesVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Message = "صحیح",
                                        record = record
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        Result = "ERROR",
                                        Message = "خطا"
                                    });
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }




        #endregion

    }
}

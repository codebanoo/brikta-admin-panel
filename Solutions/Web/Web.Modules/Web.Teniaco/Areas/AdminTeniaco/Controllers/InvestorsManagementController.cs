using ApiCallers.PublicApiCaller;
using ApiCallers.TeniacoApiCaller;
using AutoMapper;
using CustomAttributes;
using FrameWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Models.Business.ConsoleBusiness;
using Newtonsoft.Json.Linq;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class InvestorsManagementController : ExtraAdminController
    {
        public InvestorsManagementController(IHostEnvironment _hostEnvironment,
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

        #region Investors
        public IActionResult Index()
        {
            ViewData["Title"] = "لیست سرمایه گذاران";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["FundsList"] == null)
            {
                List<FundsVM> fundsVMList = new List<FundsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/FundsManagement/GetAllFundsList";
                    GetAllFundsListPVM getAllFundsListPVM = new GetAllFundsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "FundsManagement", "GetAllFundsList", this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllFundsList(getAllFundsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                fundsVMList = jArray.ToObject<List<FundsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["FundsList"] = fundsVMList;
            }


            if (ViewData["GuildCategoriesList"] == null)
            {
                List<GuildCategoriesVM> categoriesVMList = new List<GuildCategoriesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/GuildCategoriesManagement/GetAllGuildCategoriesList";

                    GetAllGuildCategoriesListPVM getAllCategoriesListPVM = new GetAllGuildCategoriesListPVM() 
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllGuildCategoriesList(getAllCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                categoriesVMList = jArray.ToObject<List<GuildCategoriesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["GuildCategoriesList"] = categoriesVMList;
            }


            //کمبوی معرف
            // لیستی از اشخاص بدون شرط دسترسی
            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    //serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsListWithUsers";
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {

                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonsList"] = personsVMList;
            }

            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllInvestorsList(
            long? userId = null,
            string companyName = "",
            int? fundId = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId  = userId,
                    CompanyName = companyName,
                    FundId = fundId,
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
                            var records = jArray.ToObject<List<InvestorsVM>>();

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
                                Records = records,
                                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                            });
                        }
                    }
                }
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
        public IActionResult GetListOfInvestors(
           int jtStartIndex,
          int jtPageSize,
          long? userId = null,
          string companyName = "",
          int? fundId = null,
          string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetListOfInvestors";

                GetListOfInvestorsPVM getListOfInvestorsPVM = new GetListOfInvestorsPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    UserId = userId,
                    FundId = ((fundId.HasValue) ? fundId.Value : (int?)0),
                    CompanyName = (!string.IsNullOrEmpty(companyName) ? companyName.Trim() : ""),
                    jtSorting = jtSorting
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetListOfInvestors(getListOfInvestorsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<InvestorsVM>>();

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
                    }
                }
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



        public IActionResult AddToInvestors()
        {
            ViewData["Title"] = "تعریف سرمایه گذار";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["FundsList"] == null)
            {
                List<FundsVM> fundsVMList = new List<FundsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/FundsManagement/GetAllFundsList";
                    GetAllFundsListPVM getAllFundsListPVM = new GetAllFundsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllFundsList(getAllFundsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                fundsVMList = jArray.ToObject<List<FundsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["FundsList"] = fundsVMList;
            }

            if (ViewData["GuildCategoriesList"] == null)
            {
                List<GuildCategoriesVM> categoriesVMList = new List<GuildCategoriesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/GuildCategoriesManagement/GetAllGuildCategoriesList";

                    GetAllGuildCategoriesListPVM getAllCategoriesListPVM = new GetAllGuildCategoriesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllGuildCategoriesList(getAllCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                categoriesVMList = jArray.ToObject<List<GuildCategoriesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["GuildCategoriesList"] = categoriesVMList;
            }


            //کمبوی معرف
            // لیستی از اشخاص  بدون شرط دسترسی
            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    //serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsListWithUsers";
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {

                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonsList"] = personsVMList;
            }

            if (ViewData["PersonTypesList"] == null)
            {
                List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/PersonTypesManagement/GetAllPersonTypesList";

                    GetAllPersonTypesListPVM getAllPersonTypesListPVM =
                        new GetAllPersonTypesListPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonTypesList(getAllPersonTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<PersonTypesVM>>();

                                personTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonTypesList"] = personTypesVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/InvestorsManagement/Index";
            }

            InvestorsVM investorsVM = new InvestorsVM();

            dynamic expando = new ExpandoObject();
            expando = investorsVM;

            return View("AddTo", expando);
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToInvestors(InvestorsVM investorsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                investorsVM.CreateEnDate = DateTime.Now;
                investorsVM.CreateTime = PersianDate.TimeNow;
                investorsVM.UserIdCreator = this.userId.Value;
                investorsVM.IsActivated = true;
                investorsVM.IsDeleted = false;
                investorsVM.CustomUsersVM.ParentUserId = 2;
                investorsVM.CustomUsersVM.RoleIds = new List<int>();
                investorsVM.CustomUsersVM.LevelIds = new List<int>();
                investorsVM.CustomUsersVM.ReplyPassword = investorsVM.CustomUsersVM.Password;
                investorsVM.CustomUsersVM.DomainSettingId = this.domainsSettings.DomainSettingId;
                investorsVM.CustomUsersVM.UserIdCreator = this.userId.Value;

                ModelState.Remove("CustomUsersVM.RoleIds");
                ModelState.Remove("CustomUsersVM.LevelIds");
                ModelState.Remove("CustomUsersVM.ReplyPassword");
                ModelState.Remove("CustomUsersVM.Password");
                ModelState.Remove("CustomUsersVM.Name");
                ModelState.Remove("CustomUsersVM.Family");
                ModelState.Remove("CustomUsersVM.Mobile");
                ModelState.Remove("CustomUsersVM.ParentUserId");

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/AddToInvestors";

                    AddToInvestorsPVM addToInvestorsPVM = new AddToInvestorsPVM() 
                    {
                        InvestorsVM = investorsVM ,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).AddToInvestors(addToInvestorsPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<InvestorsVM>();

                                if (record != null)
                                {
                                    investorsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = investorsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateInvestor"))
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
            catch (Exception ex)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }
        

        public IActionResult UpdateInvestors(int Id = 0)
        {
            ViewData["Title"] = "ویرایش سرمایه گذار";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            if (ViewData["FundsList"] == null)
            {
                List<FundsVM> fundsVMList = new List<FundsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/FundsManagement/GetAllFundsList";
                    GetAllFundsListPVM getAllFundsListPVM = new GetAllFundsListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllFundsList(getAllFundsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                fundsVMList = jArray.ToObject<List<FundsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["FundsList"] = fundsVMList;
            }

            if (ViewData["GuildCategoriesList"] == null)
            {
                List<GuildCategoriesVM> categoriesVMList = new List<GuildCategoriesVM>();

                try
                {
                    serviceUrl = publicApiUrl + "/api/GuildCategoriesManagement/GetAllGuildCategoriesList";

                    GetAllGuildCategoriesListPVM getAllCategoriesListPVM = new GetAllGuildCategoriesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllGuildCategoriesList(getAllCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                categoriesVMList = jArray.ToObject<List<GuildCategoriesVM>>();

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["GuildCategoriesList"] = categoriesVMList;
            }


            //کمبوی معرف
            // لیستی از اشخاص  بدون شرط دسترسی
            if (ViewData["PersonsList"] == null)
            {
                List<PersonsVM> personsVMList = new List<PersonsVM>();

                try
                {
                    //serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsListWithUsers";
                    serviceUrl = publicApiUrl + "/api/PersonsManagement/GetAllPersonsList";

                    GetAllPersonsListPVM getAllPersonsListPVM = new GetAllPersonsListPVM()
                    {

                        //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                        //    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonsList(getAllPersonsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                personsVMList = jArray.ToObject<List<PersonsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonsList"] = personsVMList;
            }

            if (ViewData["PersonTypesList"] == null)
            {
                List<PersonTypesVM> personTypesVMList = new List<PersonTypesVM>();

                try
                {
                    string serviceUrl = publicApiUrl + "/api/PersonTypesManagement/GetAllPersonTypesList";

                    GetAllPersonTypesListPVM getAllPersonTypesListPVM =
                        new GetAllPersonTypesListPVM()
                        {
                            //ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            //this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        };

                    responseApiCaller = new PublicApiCaller(serviceUrl).GetAllPersonTypesList(getAllPersonTypesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<PersonTypesVM>>();

                                personTypesVMList = records;
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["PersonTypesList"] = personTypesVMList;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/InvestorsManagement/Index";
            }

            InvestorsVM investorsVM = new InvestorsVM();
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetInvestorWithInvestorId";

                GetInvestorWithInvestorIdPVM getInvestorWithInvestorIdPVM = new GetInvestorWithInvestorIdPVM()
                {
                    InvestorId = Id,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds("AdminTeniaco", "InvestorsManagement", "GetInvestorWithInvestorId", this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetInvestorWithInvestorId(getInvestorWithInvestorIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<InvestorsVM>();


                            if (record != null)
                            {
                                investorsVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }


            dynamic expando = new ExpandoObject();
            expando = investorsVM;

            return View("Update", expando);
        }


        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateInvestors(InvestorsVM investorsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                investorsVM.EditEnDate = DateTime.Now;
                investorsVM.EditTime = PersianDate.TimeNow;
                investorsVM.UserIdEditor = this.userId.Value;
                investorsVM.CustomUsersVM.ParentUserId = 1;
                investorsVM.CustomUsersVM.RoleIds = new List<int>();
                investorsVM.CustomUsersVM.LevelIds = new List<int>();
                investorsVM.CustomUsersVM.UserName = "string";
                investorsVM.CustomUsersVM.Family = string.Empty;
                investorsVM.CustomUsersVM.Mobile = "123";
                investorsVM.CustomUsersVM.Name = string.Empty;
                investorsVM.CustomUsersVM.NationalCode = string.Empty;



                ModelState.Remove("CustomUsersVM.RoleIds");
                ModelState.Remove("CustomUsersVM.LevelIds");
                ModelState.Remove("CustomUsersVM.ParentUserId");
                ModelState.Remove("CustomUsersVM.UserName");
                //ModelState.Remove("CustomUsersVM.Email");
                //ModelState.Remove("CustomUsersVM.Family");
                //ModelState.Remove("CustomUsersVM.Mobile");
                //ModelState.Remove("CustomUsersVM.Name");
                //ModelState.Remove("CustomUsersVM.NationalCode"); 

                if (ModelState.IsValid)
                {
                    string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/UpdateInvestors";

                    UpdateInvestorsPVM updateInvestorsPVM = new UpdateInvestorsPVM()
                    {
                        InvestorsVM = investorsVM,
                        
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).UpdateInvestors(updateInvestorsPVM);


                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                investorsVM = jObject.ToObject<InvestorsVM>();

                                #region Fill UserCreatorName

                                if (investorsVM.UserIdCreator.HasValue)
                                {
                                    var customUser = consoleBusiness.GetCustomUser(investorsVM.UserIdCreator.Value);

                                    if (customUser != null)
                                    {
                                        investorsVM.UserCreatorName = customUser.UserName;

                                        if (!string.IsNullOrEmpty(customUser.Name))
                                            investorsVM.UserCreatorName += " " + customUser.Name;

                                        if (!string.IsNullOrEmpty(customUser.Family))
                                            investorsVM.UserCreatorName += " " + customUser.Family;
                                    }
                                }

                                #endregion

                                return Json(new
                                {
                                    Result = jsonResultWithRecordObjectVM.Result,
                                    Record = investorsVM,
                                    Message = "ویرایش انجام شد",
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateInvestor"))
                            {
                                return Json(new
                                {
                                    Result = "ERROR",
                                    Message = "رکورد تکراری است"
                                });
                            }
                        }
                    }

                    return Json(new
                    {
                        Result = jsonResultWithRecordObjectVM.Result,
                        Record = investorsVM,
                    });

                }
                return Json(new
                {
                    Result = jsonResultWithRecordObjectVM.Result,
                    Record = investorsVM,
                });
            }
            catch (Exception)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationInvestors(int investorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/ToggleActivationInvestors";

                ToggleActivationInvestorsPVM toggleActivationInvestorsPVM =
                    new ToggleActivationInvestorsPVM()
                    {
                        InvestorId = investorId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).ToggleActivationInvestors(toggleActivationInvestorsPVM);

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
        public IActionResult TemporaryDeleteInvestors(int investorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/TemporaryDeleteInvestors";

                TemporaryDeleteInvestorsPVM temporaryDeleteInvestorsPVM =
                    new TemporaryDeleteInvestorsPVM()
                    {
                        InvestorId = investorId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).TemporaryDeleteInvestors(temporaryDeleteInvestorsPVM);

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


        [HttpPost]
        [AjaxOnly]
        public IActionResult CompleteDeleteInvestors(int investorId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/CompleteDeleteInvestors";

                CompleteDeleteInvestorsPVM completeDeleteInvestorsPVM =
                    new CompleteDeleteInvestorsPVM()
                    {
                        InvestorId = investorId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).CompleteDeleteInvestors(completeDeleteInvestorsPVM);

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
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "خطا"
            });
        }
        #endregion

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

        #region Evaluation Investors Value

       
        public IActionResult EvaluationOfInvestors(int Id = 0)
        {
            ViewData["Title"] = "چارت ارزیابی سرمایه گذار";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (Id.Equals(0))
            {
                return RedirectToAction("Index", "InvestorsManagement");
            }

            if (ViewData["EvaluationsList"] == null)
            {
                List<EvaluationsVM> evaluationsVMList = new List<EvaluationsVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationsManagement/GetAllEvaluationsList";
                    GetAllEvaluationsListPVM getAllEvaluationsListPVM = new GetAllEvaluationsListPVM()
                    {
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationsList(getAllEvaluationsListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                evaluationsVMList  = jArray.ToObject<List<EvaluationsVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["EvaluationsList"] = evaluationsVMList;
            }

            if (ViewData["EvaluationItemValuesList"] == null)
            {
                List<EvaluationItemValuesVM> evaluationItemValuesVMList = new List<EvaluationItemValuesVM>();

                try
                {
                    serviceUrl = teniacoApiUrl + "/api/EvaluationItemValuesManagement/GetEvaluationItemValuesByParentId";

                    GetEvaluationItemValuesByParentIdPVM getEvaluationItemValuesByParentIdPVM= new GetEvaluationItemValuesByParentIdPVM()
                    {
                        ParentId = Id,
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetEvaluationItemValuesByParentId(getEvaluationItemValuesByParentIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                evaluationItemValuesVMList = jArray.ToObject<List<EvaluationItemValuesVM>>();


                                #endregion
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }

                ViewData["EvaluationItemValuesList"] = evaluationItemValuesVMList;
            }

            ViewData["SearchTitle"] = "OK";

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllEvalCategoriesList(int evaluationId)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllEvaluationCategoriesList";

                GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM = new GetAllEvaluationCategoriesListPVM()
                {
                    EvaluationId = evaluationId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationCategoriesList(getAllEvaluationCategoriesListPVM);

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


        #region Norouzi's code

        private (List<EvaluationCategoriesVM> lst, string err) GetEvalCategoriesList(int evaluationId = 0)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            try
            {
                serviceUrl = teniacoApiUrl + "/api/EvaluationCategoriesManagement/GetAllEvaluationCategoriesList";

                GetAllEvaluationCategoriesListPVM getAllEvaluationCategoriesListPVM = new GetAllEvaluationCategoriesListPVM()
                {
                    EvaluationId = evaluationId,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationCategoriesList(getAllEvaluationCategoriesListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationCategoriesVM>>();
                            if (records.Count > 0)
                            {
                                return (lst: records, err: string.Empty);

                            }
                        }
                    }
                }
            }
            catch
            {

                //throw;
                return (null, "خطا در واکشی اطلاعات");
            }
            return (null, string.Empty);

        }
        private List<InvestorsVM> GetInvestorList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            try
            {
                string serviceUrl = teniacoApiUrl + "/api/InvestorsManagement/GetAllInvestorsList";

                GetAllInvestorsListPVM getAllInvestorsListPVM = new GetAllInvestorsListPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId)
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllInvestorsList(getAllInvestorsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<InvestorsVM>>();

                            if (records.Count > 0)
                            {
                                return records;
                            }
                        }
                    }
                }
            }

            catch
            {

            }
            return null;
        }
        private List<EvaluationQuestionsVM> GetAllEvaluationQuestions()
        {
            var res = new List<EvaluationQuestionsVM>();
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/EvaluationQuestionsManagement/GetAllEvaluationQuestionsList";
                GetAllEvaluationQuestionsListPVM getAllEvaluationQuestionsListPVM = new GetAllEvaluationQuestionsListPVM()
                {
                    EvaluationQuestionSearch = "",
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationQuestionsList(getAllEvaluationQuestionsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationQuestionsVM>>();
                            res = records;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return res;


        }
        private List<EvaluationItemsVM> EvalquestionItems()
        {

            var res = new List<EvaluationItemsVM>();
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                serviceUrl = teniacoApiUrl + "/api/EvaluationItemsManagement/GetAllEvaluationItemsList";
                GetAllEvaluationItemsListPVM getAllEvaluationItemsListPVM = new GetAllEvaluationItemsListPVM()
                {
                    EvaluationAnswerSearch = "",
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllEvaluationItemsList(getAllEvaluationItemsListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<EvaluationItemsVM>>();
                            res = records;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return res;

        }

        #endregion


        #endregion

    }
}

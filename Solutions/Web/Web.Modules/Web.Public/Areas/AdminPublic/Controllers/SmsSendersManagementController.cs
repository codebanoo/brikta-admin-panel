using ApiCallers.PublicApiCaller;
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
using Web.Core.Controllers;

namespace Web.Public.Areas.AdminPublic.Controllers
{
    [Area("AdminPublic")]
    public class SmsSendersManagementController : ExtraAdminController
    {
        public SmsSendersManagementController(IHostEnvironment _hostEnvironment,
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
            ViewData["Title"] = "لیست مالکین/واسطه ها";

            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (ViewData["SearchTitle"] == null)
            {
                ViewData["SearchTitle"] = "OK";
            }

            return View("Index");
        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllSmsSendersList(
                 string searchText)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/GetAllSmsSendersList";

                GetAllSmsSendersListPVM getAllSmsSendersListPVM = new GetAllSmsSendersListPVM()
                {
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                    this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),


                    SearchText = searchText,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetAllSmsSendersList(getAllSmsSendersListPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<SmsSendersVM>>();

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
        public IActionResult GetListOfSmsSenders(string searchText, int jtStartIndex = 0, int jtPageSize = 0)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/GetListOfSmsSenders";

                GetListOfSmsSendersPVM getListOfSmsSendersPVM = new GetListOfSmsSendersPVM()
                {
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtPageSize = jtPageSize,
                    jtStartIndex = jtStartIndex,
                    SearchText = searchText,
                };

                responseApiCaller = new PublicApiCaller(serviceUrl).GetListOfSmsSenders(getListOfSmsSendersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            //var records = jsonResultWithRecordsObjectVM.Records as List<SmsSenderssVM>;
                            //var records = JsonConvert.DeserializeObject<List<SmsSenderssVM>>(jsonResultWithRecordsObjectVM.Records);

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<SmsSendersVM>>();

                            //JObject jObject = jsonResultWithRecordsObjectVM.Records;
                            //var records = jObject.ToObject<List<SmsSendersVM>>();

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
            catch (Exception exc)
            {
            }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToSmsSenders(SmsSendersVM smsSendersVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                smsSendersVM.CreateEnDate = DateTime.Now;
                smsSendersVM.CreateTime = PersianDate.TimeNow;
                smsSendersVM.UserIdCreator = this.userId.Value;
                //menusVM.Lang = this.currentLanguage;

                //menusVM.SmsSendersType = "text";

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/AddToSmsSenders";

                    AddToSmsSendersPVM addToFormPVM = new AddToSmsSendersPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        SmsSendersVM = smsSendersVM
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).AddToSmsSenders(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                smsSendersVM = jObject.ToObject<SmsSendersVM>();

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
                                    Record = smsSendersVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateSmsSender"))
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
        public IActionResult UpdateSmsSenders(SmsSendersVM smsSendersVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                smsSendersVM.EditEnDate = DateTime.Now;
                smsSendersVM.EditTime = PersianDate.TimeNow;
                smsSendersVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/UpdateSmsSenders";

                    UpdateSmsSendersPVM updateFormPVM = new UpdateSmsSendersPVM()
                    {
                        SmsSendersVM = smsSendersVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new PublicApiCaller(serviceUrl).UpdateSmsSenders(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                smsSendersVM = jObject.ToObject<SmsSendersVM>();

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
                                    Record = smsSendersVM,
                                });
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateSmsSender"))
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
                        Record = smsSendersVM,
                    });
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

        [AjaxOnly]
        [HttpPost]
        public IActionResult ToggleActivationSmsSenders(int SmsSenderId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/ToggleActivationSmsSenders";

                ToggleActivationSmsSendersPVM toggleActivationSmsSendersPVM =
                    new ToggleActivationSmsSendersPVM()
                    {
                        SmsSenderId = SmsSenderId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).ToggleActivationSmsSenders(toggleActivationSmsSendersPVM);

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
                Message = "ErrorMessage"
            });
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult TemporaryDeleteSmsSenders(int SmsSenderId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //if (institutionsBusiness.CompleteDeleteSmsSenders(EducationLevelId, this.userId.Value))
                //{
                //    return Json(new
                //    {
                //        Result = "OK",
                //    });
                //}
                //else
                //{
                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentSmsSendersErrorMessage").FirstOrDefault().Value
                //    });
                //}

                string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/TemporaryDeleteSmsSenders";

                TemporaryDeleteSmsSendersPVM temporaryDeleteSmsSendersPVM =
                    new TemporaryDeleteSmsSendersPVM()
                    {
                        SmsSenderId = SmsSenderId,
                        UserId = this.userId.Value,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).
                    TemporaryDeleteSmsSenders(temporaryDeleteSmsSendersPVM);

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

                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "DeleteCurrentSmsSendersErrorMessage"
                    });
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
        public IActionResult CompleteDeleteSmsSenders(int SmsSenderId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = publicApiUrl + "/api/SmsSendersManagement/CompleteDeleteSmsSenders";

                CompleteDeleteSmsSendersPVM deleteSmsSendersPVM =
                    new CompleteDeleteSmsSendersPVM()
                    {
                        SmsSenderId = SmsSenderId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                responseApiCaller = new PublicApiCaller(serviceUrl).CompleteDeleteSmsSenders(deleteSmsSendersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
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
                Message = "ErrorMessage"
            });
        }
    }
}

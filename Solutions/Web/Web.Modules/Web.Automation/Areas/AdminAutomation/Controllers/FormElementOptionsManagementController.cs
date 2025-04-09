using ApiCallers.BaseApiCaller;
using ApiCallers.AutomationApiCaller;
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
using System.Collections.Generic;
using System;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using Web.Core.Controllers;
using System.Linq;
using VM.Automation;
using VM.PVM.Automation;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class FormElementOptionsManagementController : ExtraAdminController
    {
        public FormElementOptionsManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult Index(int id = 0)
        {
            if (id.Equals(0))
            {
                return RedirectToAction("Index", "FormElementsManagement");
            }

            ViewData["Title"] = "لیست آیتمهای فیلد";

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            FormElementsVM formElementsVM = new FormElementsVM();

            if (ViewData["FormElementsVM"] == null)
            {
                try
                {
                    string serviceUrl = automationApiUrl + "/api/FormElementsManagement/GetFormElementWithFormElementId";

                    GetFormElementWithFormElementIdPVM getFormElementWithFormElementIdPVM = new GetFormElementWithFormElementIdPVM()
                    {
                        FormElementId = id,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).GetFormElementWithFormElementId(getFormElementWithFormElementIdPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<FormElementsVM>();

                                if (record != null)
                                {
                                    formElementsVM = record;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }

                ViewData["FormElementsVM"] = formElementsVM;
            }

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminAutomation/FormElementsManagement/Index/" + formElementsVM.FormId.ToString();
            }

            return View("Index");

        }



        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllFormElementOptionsList(
            int formElementId = 0)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/GetAllFormElementOptionsList";

                GetAllFormElementOptionsListPVM getListOfFormElementOptionsPVM = new GetAllFormElementOptionsListPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    
                    FormElementId = formElementId,

                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).GetAllFormElementOptionsList(getListOfFormElementOptionsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<FormElementOptionsVM>>();

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
                Message = "خطا"
            });
        }


        [HttpPost]
        [AjaxOnly]
        public IActionResult GetListOfFormElementOptions(int jtStartIndex = 0,
            int jtPageSize = 10,
            int formElementId = 0,
            string jtSorting = "")
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/GetListOfFormElementOptions";

                GetListOfFormElementOptionsPVM getListOfFormElementOptionsPVM = new GetListOfFormElementOptionsPVM()
                {
                    /*/ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, 
                        this.domainsSettings.DomainSettingId),*/
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    jtStartIndex = jtStartIndex,
                    jtPageSize = jtPageSize,
                    FormElementId = formElementId,
                    jtSorting = jtSorting
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).GetListOfFormElementOptions(getListOfFormElementOptionsPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                    if (jsonResultWithRecordsObjectVM != null)
                    {
                        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                        {
                            #region Fill UserCreatorName

                            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                            var records = jArray.ToObject<List<FormElementOptionsVM>>();

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
                Message = "خطا"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult AddToFormElementOptions(FormElementOptionsVM FormElementOptionsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                FormElementOptionsVM.CreateEnDate = DateTime.Now;
                FormElementOptionsVM.CreateTime = PersianDate.TimeNow;
                FormElementOptionsVM.UserIdCreator = this.userId.Value;

                //FormElementOptionsVM.FormElementOptionsType = "text";

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/AddToFormElementOptions";

                    AddToFormElementOptionsPVM addToFormPVM = new AddToFormElementOptionsPVM()
                    {
                        /*ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        this.domainsSettings.DomainSettingId),*/
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        FormElementOptionsVM = FormElementOptionsVM
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).AddToFormElementOptions(addToFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                var record = jObject.ToObject<FormElementOptionsVM>();

                                if (record != null)
                                {
                                    FormElementOptionsVM = record;
                                    return Json(new
                                    {
                                        Result = "OK",
                                        Record = FormElementOptionsVM,
                                        Message = "تعریف انجام شد",
                                    });
                                }
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateformElementOption"))
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
                Message = "خطا"
            });
        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult UpdateFormElementOptions(FormElementOptionsVM FormElementOptionsVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                FormElementOptionsVM.EditEnDate = DateTime.Now;
                FormElementOptionsVM.EditTime = PersianDate.TimeNow;
                FormElementOptionsVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/UpdateFormElementOptions";

                    UpdateFormElementOptionsPVM updateFormPVM = new UpdateFormElementOptionsPVM()
                    {
                        FormElementOptionsVM = FormElementOptionsVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateFormElementOptions(updateFormPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                FormElementOptionsVM = jObject.ToObject<FormElementOptionsVM>();

                                #region Fill UserCreatorName

                                if (FormElementOptionsVM.UserIdCreator.HasValue)
                                {
                                    var customUser = consoleBusiness.GetCustomUser(FormElementOptionsVM.UserIdCreator.Value);

                                    if (customUser != null)
                                    {
                                        FormElementOptionsVM.UserCreatorName = customUser.UserName;

                                        if (!string.IsNullOrEmpty(customUser.Name))
                                            FormElementOptionsVM.UserCreatorName += " " + customUser.Name;

                                        if (!string.IsNullOrEmpty(customUser.Family))
                                            FormElementOptionsVM.UserCreatorName += " " + customUser.Family;
                                    }
                                }

                                #endregion
                            }
                            else
                                if (jsonResultWithRecordObjectVM.Result.Equals("ERROR") &&
                                jsonResultWithRecordObjectVM.Message.Equals("DuplicateformElementOption"))
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
                        Record = FormElementOptionsVM,
                    });
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
        public IActionResult ToggleActivationFormElementOptions(int formElementOptionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/ToggleActivationFormElementOptions";

                ToggleActivationFormElementOptionsPVM toggleActivationFormElementOptionsPVM =
                new ToggleActivationFormElementOptionsPVM()
                {
                    FormElementOptionId = formElementOptionId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).ToggleActivationFormElementOptions(toggleActivationFormElementOptionsPVM);

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
        public IActionResult TemporaryDeleteFormElementOptions(int formElementOptionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/TemporaryDeleteFormElementOptions";

                TemporaryDeleteFormElementOptionsPVM toggleActivationFormElementOptionsPVM =
                new TemporaryDeleteFormElementOptionsPVM()
                {
                    FormElementOptionId = formElementOptionId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).TemporaryDeleteFormElementOptions(toggleActivationFormElementOptionsPVM);

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
        public IActionResult CompleteDeleteFormElementOptions(int formElementOptionId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                string serviceUrl = automationApiUrl + "/api/FormElementOptionsManagement/CompleteDeleteFormElementOptions";

                CompleteDeleteFormElementOptionsPVM deleteFormElementOptionsPVM =
                new CompleteDeleteFormElementOptionsPVM()
                {
                    FormElementOptionId = formElementOptionId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new AutomationApiCaller(serviceUrl).CompleteDeleteFormElementOptions(deleteFormElementOptionsPVM);

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
    }
}

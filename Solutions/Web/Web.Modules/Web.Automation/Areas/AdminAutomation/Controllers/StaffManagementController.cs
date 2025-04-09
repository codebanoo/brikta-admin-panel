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
//using ApiCallers.AutomationApiCaller;
using Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VM.Console;
using VM.Automation;
using VM.PVM.Automation;
using Web.Core.Controllers;
using VM.Base;
using System.IO;
using System.Dynamic;

namespace Web.Automation.Areas.AdminAutomation.Controllers
{
    [Area("AdminAutomation")]
    public class StaffManagementController : ExtraAdminController
    {
        public StaffManagementController(IHostEnvironment _hostEnvironment,
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
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });


            if (ViewData["UsersList"] == null)
            {
                
                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }

            return View("Index");
        }
        [AjaxOnly]
        [HttpPost]
        public IActionResult GetAllStaffList()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //List<StaffVM> staffVMList = new List<StaffVM>();

                //serviceUrl = crmApiUrl + "/api/StaffManagement/GetAllStaffList";

                //GetAllStaffListPVM getAllStaffListPVM =
                //    new GetAllStaffListPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId),
                //        
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetAllStaffList(getAllStaffListPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<StaffVM>>();

                //            if (records != null)
                //            {
                //                if (records.Count > 0)
                //                {
                //                    //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                //                    //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //                    //foreach (var record in records)
                //                    //{
                //                    //    if (record.UserIdCreator.HasValue)
                //                    //    {
                //                    //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                //                    //        if (customUser != null)
                //                    //        {
                //                    //            record.UserCreatorName = customUser.UserName;

                //                    //            if (!string.IsNullOrEmpty(customUser.Name))
                //                    //                record.UserCreatorName += " " + customUser.Name;

                //                    //            if (!string.IsNullOrEmpty(customUser.Family))
                //                    //                record.UserCreatorName += " " + customUser.Family;
                //                    //        }
                //                    //    }
                //                    //}
                //                }

                //                staffVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = staffVMList,
                //            });
                //        }
                //    }
                //}
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
        public IActionResult GetListOfStaff(int jtStartIndex, int jtPageSize, string jtSorting = null)
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //List<StaffVM> staffVMList = new List<StaffVM>();

                //serviceUrl = crmApiUrl + "/api/StaffManagement/GetListOfStaff";

                //GetListOfStaffPVM getListOfStaffPVM =
                //    new GetListOfStaffPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId),
                //        jtStartIndex = jtStartIndex,
                //        jtPageSize = jtPageSize,
                //        jtSorting = jtSorting,
                //        
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetListOfStaff(getListOfStaffPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                //    if (jsonResultWithRecordsObjectVM != null)
                //    {
                //        if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            JArray jArray = jsonResultWithRecordsObjectVM.Records;
                //            var records = jArray.ToObject<List<StaffVM>>();

                //            if (records != null)
                //            {
                //                if (records.Count > 0)
                //                {
                //                    var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();

                //                    var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                //                    foreach (var record in records)
                //                    {
                //                        if (record.UserIdCreator.HasValue)
                //                        {
                //                            var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                //                            if (customUser != null)
                //                            {
                //                                record.UserCreatorName = customUser.UserName;

                //                                if (!string.IsNullOrEmpty(customUser.Name))
                //                                    record.UserCreatorName += " " + customUser.Name;

                //                                if (!string.IsNullOrEmpty(customUser.Family))
                //                                    record.UserCreatorName += " " + customUser.Family;
                //                            }
                //                        }

                //                    }
                //                }

                //                staffVMList = records;
                //            }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Records = staffVMList,
                //                TotalRecordCount = jsonResultWithRecordsObjectVM.TotalRecordCount
                //            });
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult AddToStaff()
        {
            if (ViewData["UsersList"] == null)
            {
               
                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }


            if (ViewData["DomainName"] == null)
            {
                ViewData["DomainName"] = this.domain;
            }

            StaffVM StaffVM = new StaffVM();
            StaffVM.IsActivated = true;
            StaffVM.IsDeleted = false;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminAutomation/StaffManagement/Index";
            }
            dynamic expando = new ExpandoObject();
            expando = StaffVM;

            return View("AddTo", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult AddToStaff(StaffVM staffVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                //staffVM.CreateEnDate = DateTime.Now;
                //staffVM.CreateTime = PersianDate.TimeNow;
                //staffVM.UserIdCreator = this.userId.Value;

                //serviceUrl = crmApiUrl + "/api/StaffManagement/AddToStaff";

                //AddToStaffPVM addToStaffPVM =
                //    new AddToStaffPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        StaffVM = staffVM
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    AddToStaff(addToStaffPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            int? record = (int?)jsonResultWithRecordObjectVM.Record;

                //            if (record != null)
                //                if (record.Value > 0)
                //                {
                //                    staffVM.StaffId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                StaffId = staffVM.StaffId
                //            });
                //        }
                //    }
                //}
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
        public IActionResult GetStaffWithUserId(int userId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM =
                new JsonResultWithRecordObjectVM(new object() { });

            StaffVM staffVM = new StaffVM();

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/GetUserIdWithUserId";

                //GetStaffWithUserIdPVM getStaffWithUserIdPVM =
                //    new GetStaffWithUserIdPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        UserId = userId,
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    GetStaffWithUserId(getStaffWithUserIdPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            #region Fill UserCreatorName

                //            int? record = (int?)jsonResultWithRecordObjectVM.Record;

                //            if (record != null)
                //                if (record.Value > 0)
                //                {
                //                    staffVM.UserId = record.Value;
                //                }

                //            #endregion

                //            return Json(new
                //            {
                //                Result = "OK",
                //                Message = "Success",
                //                StaffVM = staffVM
                //            }); ;
                //        }
                //    }
                //}
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
        public IActionResult GetStaffWithStaffId(int StaffId)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            StaffVM StaffVM = new StaffVM();

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/GetStaffWithStaffId";

                //GetStaffWithStaffIdPVM getStaffWithStaffIdPVM =
                //    new GetStaffWithStaffIdPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        StaffId = StaffId,
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetStaffWithStaffId(getStaffWithStaffIdPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                //            var record = jObject.ToObject<StaffVM>();

                //            if (record != null)
                //            {
                //                StaffVM = record;

                //                #region Fill UserCreatorName

                //                if (StaffVM.UserIdCreator.HasValue)
                //                {
                //                    var customUser = consoleBusiness.GetCustomUser(StaffVM.UserIdCreator.Value);

                //                    if (customUser != null)
                //                    {
                //                        StaffVM.UserCreatorName = customUser.UserName;

                //                        if (!string.IsNullOrEmpty(customUser.Name))
                //                            StaffVM.UserCreatorName += " " + customUser.Name;

                //                        if (!string.IsNullOrEmpty(customUser.Family))
                //                            StaffVM.UserCreatorName += " " + customUser.Family;
                //                    }
                //                }

                //                #endregion
                //            }
                //            return Json(new
                //            {
                //                Result = "OK",
                //                StaffVM = StaffVM
                //            }); ;
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }

        public IActionResult UpdateStaff(int Id = 0)
        {
            StaffVM staffVM = new StaffVM();
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/GetStaffWithStaffId";

                //GetStaffWithStaffIdPVM getStaffWithStaffIdPVM =
                //    new GetStaffWithStaffIdPVM()
                //    {
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                //                this.domainsSettings.DomainSettingId),
                //        
                //        StaffId = Id,
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).GetStaffWithStaffId(getStaffWithStaffIdPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                //    if (jsonResultWithRecordObjectVM != null)
                //    {
                //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                //        {
                //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                //            var record = jObject.ToObject<StaffVM>();

                //            if (record != null)
                //            {
                //                staffVM = record;

                //                #region Fill UserCreatorName

                //                if (staffVM.UserIdCreator.HasValue)
                //                {
                //                    var customUser = consoleBusiness.GetCustomUser(staffVM.UserIdCreator.Value);

                //                    if (customUser != null)
                //                    {
                //                        staffVM.UserCreatorName = customUser.UserName;

                //                        if (!string.IsNullOrEmpty(customUser.Name))
                //                            staffVM.UserCreatorName += " " + customUser.Name;

                //                        if (!string.IsNullOrEmpty(customUser.Family))
                //                            staffVM.UserCreatorName += " " + customUser.Family;
                //                    }
                //                }

                //                #endregion
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            { }


            if (ViewData["UsersList"] == null)
            {
               
                var userIdCreators = consoleBusiness.GetChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value);

                var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                ViewData["UsersList"] = customUsers;
            }

            if (ViewData["DomainName"] == null)
            {
                ViewData["DomainName"] = this.domain;
            }
            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminAutomation/StaffManagement/Index";
            }
            dynamic expando = new ExpandoObject();
            expando = staffVM;

            return View("Update", expando);

            //return View(themeName /*this.theme.ThemeName*/ + direction + "UpdateNewsCategory", expando);
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult UpdateStaff(StaffVM staffVM)
        {
            try
            {
                staffVM.EditEnDate = DateTime.Now;
                staffVM.EditTime = PersianDate.TimeNow;
                staffVM.UserIdEditor = this.userId.Value;

                if (ModelState.IsValid)
                {
                    //serviceUrl = crmApiUrl + "/api/StaffManagement/UpdateStaff";

                    //UpdateStaffPVM updateStaffPVM =
                    //    new UpdateStaffPVM()
                    //    {
                    //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //        this.domainsSettings.DomainSettingId),
                    //        StaffVM = staffVM,
                    //    };

                    //responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateStaff(updateStaffPVM);

                    //if (responseApiCaller.IsSuccessStatusCode)
                    //{
                    //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    //    if (jsonResultWithRecordObjectVM != null)
                    //    {
                    //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                    //        {
                    //            //var record = jsonResultWithRecordObjectVM.Record as AlumnusCoursesVM;
                    //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                    //            var record = jObject.ToObject<StaffVM>();

                    //            if (record != null)
                    //            {
                    //                staffVM = record;
                    //                return Json(new
                    //                {
                    //                    Result = "OK",
                    //                    Message = "Success",
                    //                    StaffId = staffVM.StaffId
                    //                });
                    //            }
                    //        }
                    //    }
                    //}
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
        public IActionResult ToggleActivationStaff(int StaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/ToggleActivationStaff";

                //ToggleActivationStaffPVM toggleActivationStaffPVM =
                //    new ToggleActivationStaffPVM()
                //    {
                //        StaffId = StaffId,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    ToggleActivationStaff(toggleActivationStaffPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }
                //}
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
        public IActionResult TemporaryDeleteStaff(int StaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/TemporaryDeleteStaff";

                //TemporaryDeleteStaffPVM temporaryDeleteStaffPVM =
                //    new TemporaryDeleteStaffPVM()
                //    {
                //        StaffId = StaffId,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    TemporaryDeleteStaff(temporaryDeleteStaffPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }

                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentAlumnusCoursesErrorMessage").FirstOrDefault().Value
                //    });
                //}
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
        public IActionResult CompleteDeleteStaff(int StaffId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {
                //serviceUrl = crmApiUrl + "/api/StaffManagement/CompleteDeleteStaff";

                //CompleteDeleteStaffPVM completeDeleteStaffPVM =
                //    new CompleteDeleteStaffPVM()
                //    {
                //        StaffId = StaffId,
                //        ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                //        this.domainsSettings.DomainSettingId)
                //    };

                //responseApiCaller = new AutomationApiCaller(serviceUrl).
                //    CompleteDeleteStaff(completeDeleteStaffPVM);

                //if (responseApiCaller.IsSuccessStatusCode)
                //{
                //    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                //    if (jsonResultObjectVM != null)
                //    {
                //        if (jsonResultObjectVM.Result.Equals("OK"))
                //        {
                //            return Json(new { Result = "OK" });
                //        }
                //    }

                //    return Json(new
                //    {
                //        Result = "ERROR",
                //        Message = pageTexts.Where(t => t.PropertyName == "DeleteCurrentStaffErrorMessage").FirstOrDefault().Value
                //    });
                //}
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
        public async Task<ActionResult> UploadFile(IFormFile ContractImage, IFormFile CertificateImage, IFormFile NationalCodeImage, int StaffId)
        {
            try
            {
                if (ContractImage != null || CertificateImage != null || NationalCodeImage != null)
                {
                    string contractImageName = "";
                    string certificateImageName = "";
                    string nationalCodeImageName = "";
                    string ext = "";

                    string StaffFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\Staff\\";

                    string oldContractImageName = "";
                    string oldCertificateImageName = "";
                    string oldNationalCodeImageName = "";

                    try
                    {   //TODO
                        //serviceUrl = crmApiUrl + "/api/StaffManagement/GetStaffImages";

                        //GetStaffImagesPVM getStaffImagesPVM = new GetStaffImagesPVM()
                        //{
                        //    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                        //    StaffId = StaffId
                        //};

                        //responseApiCaller = new AutomationApiCaller(serviceUrl).GetStaffImages(getStaffImagesPVM);

                        //if (responseApiCaller.IsSuccessStatusCode)
                        //{
                        //    var jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        //    if (jsonResultWithRecordObjectVM != null)
                        //    {
                        //        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        //        {

                        //            JObject jObject = jsonResultWithRecordObjectVM.Record;
                        //            var record = jObject.ToObject<StaffVM>();

                        //            oldContractImageName = record.ContractImage;
                        //            oldCertificateImageName = record.CertificateImage;
                        //            oldNationalCodeImageName = record.NationalCodeImage;
                        //        }
                        //    }
                        //}
                    }
                    catch (Exception exc)
                    { }

                    if (ContractImage != null)
                        if (ContractImage.Length > 0)
                        {
                            #region Remove Old Contract Image

                            if (ContractImage != null)
                            {
                                if (!string.IsNullOrEmpty(oldContractImageName))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" + StaffId + "\\" + oldContractImageName))
                                        {
                                            System.IO.File.Delete(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                StaffId + "\\" + oldContractImageName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }

                                    try
                                    {
                                        if (System.IO.File.Exists(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" + StaffId + "\\thumb_" + oldContractImageName))
                                        {
                                            System.IO.File.Delete(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                StaffId + "\\thumb_" + oldContractImageName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }
                                }
                            }

                            #endregion

                            #region image
                            string path = StaffFolder + this.domainsSettings.DomainName + "\\" + StaffId + "\\";

                            ext = Path.GetExtension(ContractImage.FileName);
                            contractImageName = Guid.NewGuid().ToString() + ext;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(path + contractImageName, FileMode.Create))
                            {
                                await ContractImage.CopyToAsync(fileStream);
                                System.Threading.Thread.Sleep(100);
                            }

                            ImageModify.ResizeImage(path, contractImageName, 40, 40);

                            #endregion
                        }


                    if (CertificateImage != null)
                        if (CertificateImage.Length > 0)
                        {
                            #region Remove Old Certificate Image

                            if (CertificateImage != null)
                            {
                                if (!string.IsNullOrEmpty(oldCertificateImageName))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" + StaffId + "\\" + oldCertificateImageName))
                                        {
                                            System.IO.File.Delete(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                StaffId + "\\" + oldCertificateImageName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }

                                }
                            }

                            #endregion

                            #region files

                            string path = StaffFolder + this.domainsSettings.DomainName + "\\" + StaffId + "\\";

                            ext = Path.GetExtension(CertificateImage.FileName);
                            certificateImageName = Guid.NewGuid().ToString() + ext;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(path + certificateImageName, FileMode.Create))
                            {
                                await CertificateImage.CopyToAsync(fileStream);
                                System.Threading.Thread.Sleep(100);
                            }

                            ImageModify.ResizeImage(path, certificateImageName, 40, 40);

                            #endregion
                        }

                    if (NationalCodeImage != null)
                        if (NationalCodeImage.Length > 0)
                        {
                            #region Remove Old NationalCode Image

                            if (NationalCodeImage != null)
                            {
                                if (!string.IsNullOrEmpty(oldNationalCodeImageName))
                                {
                                    try
                                    {
                                        if (System.IO.File.Exists(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" + StaffId + "\\" + oldNationalCodeImageName))
                                        {
                                            System.IO.File.Delete(StaffFolder + "\\" + this.domainsSettings.DomainName + "\\" +
                                                StaffId + "\\" + oldNationalCodeImageName);
                                            System.Threading.Thread.Sleep(100);
                                        }
                                    }
                                    catch (Exception exc)
                                    { }

                                }
                            }

                            #endregion

                            #region files

                            string path = StaffFolder + this.domainsSettings.DomainName + "\\" + StaffId + "\\";

                            ext = Path.GetExtension(NationalCodeImage.FileName);
                            nationalCodeImageName = Guid.NewGuid().ToString() + ext;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            using (var fileStream = new FileStream(path + nationalCodeImageName, FileMode.Create))
                            {
                                await NationalCodeImage.CopyToAsync(fileStream);
                                System.Threading.Thread.Sleep(100);
                            }

                            ImageModify.ResizeImage(path, nationalCodeImageName, 40, 40);

                            #endregion
                        }


                    if (!string.IsNullOrEmpty(contractImageName) || !string.IsNullOrEmpty(certificateImageName) || !string.IsNullOrEmpty(nationalCodeImageName))
                    {
                        try
                        {
                            //serviceUrl = crmApiUrl + "/api/StaffManagement/UpdateStaffImages";

                            //UpdateStaffImagesPVM updateStaffImagesPVM = new UpdateStaffImagesPVM()
                            //{
                            //    ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value, this.domainsSettings.DomainSettingId),
                            //    StaffId = StaffId,
                            //    ContractImage = contractImageName,
                            //    CertificateImage = certificateImageName,
                            //    NationalCodeImage = nationalCodeImageName
                            //};

                            //responseApiCaller = new AutomationApiCaller(serviceUrl).UpdateStaffImages(updateStaffImagesPVM);

                            //if (responseApiCaller.IsSuccessStatusCode)
                            //{
                            //    var jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                            //    if (jsonResultObjectVM != null)
                            //    {
                            //        if (jsonResultObjectVM.Result.Equals("OK"))
                            //        {
                            //            return Json(new
                            //            {
                            //                Result = "OK",
                            //                ContractImage = contractImageName,
                            //                Message = "Success"
                            //            });
                            //        }
                            //    }
                            //}

                        }
                        catch (Exception exc)
                        { }
                    }
                }
                return Json(new
                {
                    Result = "ERROR",
                    Message = ""
                });
            }
            catch (Exception exc)
            { }

            return Json(new
            {
                Result = "ERROR",
                Message = "ErrorMessage"
            });
        }
    }
}

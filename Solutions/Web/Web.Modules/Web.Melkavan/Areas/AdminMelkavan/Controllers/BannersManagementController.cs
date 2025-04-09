using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Core.Ext;
using Web.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Models.Business;
using AutoMapper;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using CustomAttributes;
using Services.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using VM.Console;
using Models.Business.ConsoleBusiness;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using VM.Base;
using VM.Public;
using VM.PVM.Public;
using ApiCallers.MelkavanApiCaller;
using Newtonsoft.Json.Linq;
using FrameWork;
using VM.Melkavan;
using VM.PVM.Melkavan;
using System.IO;
using VM.Teniaco;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class BannersManagementController : ExtraAdminController
    {
        public BannersManagementController(IHostEnvironment _hostEnvironment,
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

        //برای یکسان کردن دامین نیم در ملکاوان و تنیاکو
        //private readonly string hardcodedDomainName = "localhost";

        //برای یکسان کردن دامین نیم در ملکاوان و تنیاکو
        private readonly string hardcodedDomainName = "melkavan.com";

        public IActionResult Index()
        {
            ViewData["DomainName"] = hardcodedDomainName;
            ViewData["Title"] = "لیست بنرها";
            return View("Index");

        }

        [HttpPost]
        [AjaxOnly]
        public IActionResult GetAllBannersList(
        string bannerTitle)
        {

            try
            {
                List<BannersVM> bannersVMList = new List<BannersVM>();

                try
                {
                    serviceUrl = melkavanApiUrl + "/api/BannersManagement/GetAllBannersList";

                    GetAllBannersListPVM getAllBannersListPVM = new GetAllBannersListPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),


                        BannerTitle = bannerTitle
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAllBannersList(getAllBannersListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                bannersVMList = jArray.ToObject<List<BannersVM>>();


                                if (bannersVMList != null)
                                    if (bannersVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<BannersVM>>();

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
        public IActionResult GetListOfBanners(
        int jtStartIndex = 0,
        int jtPageSize = 10,
        string bannerTitle = "",
        string jtSorting = null)
        {

            try
            {
                List<BannersVM> bannersVMList = new List<BannersVM>();

                try
                {
                    serviceUrl = melkavanApiUrl + "/api/BannersManagement/GetListOfBanners";
                    GetListOfBannersPVM getListOfBannersPVM = new GetListOfBannersPVM
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        jtStartIndex = jtStartIndex,
                        jtPageSize = jtPageSize,
                        BannerTitle = bannerTitle,
                        jtSorting = jtSorting,
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).GetListOfBanners(getListOfBannersPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        var jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                bannersVMList = jArray.ToObject<List<BannersVM>>();


                                if (bannersVMList != null)
                                    if (bannersVMList.Count >= 0)
                                    {

                                        #region Fill UserCreatorName

                                        var records = jArray.ToObject<List<BannersVM>>();

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

        public IActionResult AddToBanners()
        {
            ViewData["Title"] = "آپلود بنر";

            return View("AddTo");
        }


        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToBanners(AddToBannersPVM addToBannersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            //BannersVM bannersVM = new BannersVM();

            try
            {
                if (addToBannersPVM != null)
                {
                    var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);


                    string fileName = "";

                    string ext = Path.GetExtension(addToBannersPVM.File.FileName);
                    fileName = Guid.NewGuid().ToString() + ext;

                    BannersVM bannersVM = new BannersVM()
                    {
                        CreateEnDate = DateTime.Now,
                        CreateTime = PersianDate.TimeNow,
                        UserIdCreator = this.userId.Value,
                        IsActivated = true,
                        IsDeleted = false,
                        BannerFileExt = ext,
                        BannerFilePath = fileName,
                        BannerFileOrder = addToBannersPVM.BannersVM.BannerFileOrder,
                        BannerLink = addToBannersPVM.BannersVM.BannerLink,
                        BannerTitle = addToBannersPVM.BannersVM.BannerTitle,

                    };

                    string serviceUrl = melkavanApiUrl + "/api/BannersManagement/AddToBanners";

                    AddToBannersPVM addToBannersPVM1 = new AddToBannersPVM()
                    {
                        BannersVM = bannersVM,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).AddToBanners(addToBannersPVM1);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                JObject jObject = jsonResultWithRecordObjectVM.Record;
                                bannersVM.BannerId = jObject.ToObject<BannersVM>().BannerId;
                            }
                        }
                    }

                    try
                    {
                        string bannerFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\BannerFiles\\" + hardcodedDomainName + "\\" + bannersVM.BannerId + "\\Image";
                        if (!Directory.Exists(bannerFolder))
                        {
                            Directory.CreateDirectory(bannerFolder);
                        }
                        using (var fileStream = new FileStream(bannerFolder + "\\" + fileName, FileMode.Create))
                        {
                            await addToBannersPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (ext.Equals(".jpeg") ||
                            ext.Equals(".jpg") ||
                            ext.Equals(".png") ||
                            ext.Equals(".gif") ||
                            ext.Equals(".bmp"))
                        {
                            ImageModify.ResizeImage(bannerFolder + "\\",
                                fileName,
                                120,
                                120);
                        }
                        return Json(new
                        {
                            Result = "OK",
                            Message = "آپلود انجام شد",
                        });

                    }
                    catch (Exception exc)
                    { }
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


        public IActionResult UpdateBanners(int Id)
        {
            ViewData["Title"] = "ویرایش بنر";
            if (Id.Equals(0))
                return RedirectToAction("Index", "BannersManagement");


            BannersVM bannersVM = new BannersVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = melkavanApiUrl + "/api/BannersManagement/GetBannersByBannerId";

                GetBannersWithBannerIdPVM getBannersWithBannerIdPVM = new GetBannersWithBannerIdPVM()
                {
                    BannerId = Id,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetBannersWithBannerId(getBannersWithBannerIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<BannersVM>();


                            if (record != null)
                            {
                                bannersVM = record;
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["BannersVM"] = bannersVM;
            var domainName = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value).DomainName;
            ViewData["DomainName"] = domainName;
            return View("Update");
        }





        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> UpdateBanners(UpdateBannersPVM updateBannersPVM)
        {
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);
            //BannersVM bannersVM = new BannersVM();
            //BannersVM oldBannersVM = new BannersVM();
            string serviceUrl = "";
            string newExt = "";
            string newFileName = "";
            //bool filesChanged = false;

            try
            {
                if (updateBannersPVM != null)
                {
                    if (updateBannersPVM.File != null && updateBannersPVM.File.Length > 0)
                    {


                        #region removing old files 
                        string bannerFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\BannerFiles\\" + hardcodedDomainName + "\\" + updateBannersPVM.BannersVM.BannerId + "\\Image";


                        if (!string.IsNullOrEmpty(updateBannersPVM.BannersVM.BannerFilePath))
                        {
                            if (System.IO.File.Exists(bannerFolder + "\\" + updateBannersPVM.BannersVM.BannerFilePath))
                            {
                                System.IO.File.Delete(bannerFolder + "\\" + updateBannersPVM.BannersVM.BannerFilePath);
                                System.Threading.Thread.Sleep(100);
                            }

                            if (updateBannersPVM.BannersVM.BannerFileExt.ToLower().Equals(".jpg") ||
                                updateBannersPVM.BannersVM.BannerFileExt.ToLower().Equals(".jpeg") ||
                                updateBannersPVM.BannersVM.BannerFileExt.ToLower().Equals(".png") ||
                                updateBannersPVM.BannersVM.BannerFileExt.ToLower().Equals(".bmp"))
                            {
                                if (System.IO.File.Exists(bannerFolder + "\\thumb_" + updateBannersPVM.BannersVM.BannerFilePath))
                                {
                                    System.IO.File.Delete(bannerFolder + "\\thumb_" + updateBannersPVM.BannersVM.BannerFilePath);
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                        }

                        #endregion

                        #region making new files
                        newExt = Path.GetExtension(updateBannersPVM.File.FileName);
                        newFileName = Guid.NewGuid().ToString() + newExt;
                        if (!Directory.Exists(bannerFolder))
                        {
                            Directory.CreateDirectory(bannerFolder);
                        }
                        using (var fileStream = new FileStream(bannerFolder + "\\" + newFileName, FileMode.Create))
                        {
                            await updateBannersPVM.File.CopyToAsync(fileStream);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (newExt.Equals(".jpeg") ||
                            newExt.Equals(".jpg") ||
                            newExt.Equals(".png") ||
                            newExt.Equals(".gif") ||
                            newExt.Equals(".bmp"))
                        {
                            ImageModify.ResizeImage(bannerFolder + "\\",
                                newFileName,
                                120,
                                120);
                        }
                        #endregion

                        //filesChanged = true;
                    }
                    else
                    {
                        newFileName = updateBannersPVM.BannersVM.BannerFilePath;// + updateBannersPVM.BannersVM.BannerFileExt;
                        newExt = updateBannersPVM.BannersVM.BannerFileExt;
                    }



                    BannersVM bannersVM = new BannersVM()
                    {
                        BannerId = updateBannersPVM.BannersVM.BannerId,
                        EditEnDate = DateTime.Now,
                        EditTime = PersianDate.TimeNow,
                        UserIdEditor = this.userId.Value,
                        IsActivated = updateBannersPVM.BannersVM.IsActivated,
                        IsDeleted = updateBannersPVM.BannersVM.IsDeleted,
                        BannerFileExt = newExt,
                        BannerFilePath = newFileName,
                        BannerFileOrder = updateBannersPVM.BannersVM.BannerFileOrder,
                        BannerLink = updateBannersPVM.BannersVM.BannerLink,
                        BannerTitle = updateBannersPVM.BannersVM.BannerTitle,

                    };

                    serviceUrl = melkavanApiUrl + "/api/BannersManagement/UpdateBanners";

                    updateBannersPVM = new UpdateBannersPVM()
                    {
                        BannersVM = bannersVM,

                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                    responseApiCaller = new MelkavanApiCaller(serviceUrl).UpdateBanners(updateBannersPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                        if (jsonResultWithRecordObjectVM != null)
                        {
                            if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                            {
                                return Json(new
                                {
                                    Result = "OK",
                                    Message = "ویرایش انجام شد",
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
       
        public IActionResult ToggleActivationBanners(int BannerId = 0)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            try
            {


                string serviceUrl = melkavanApiUrl + "/api/BannersManagement/ToggleActivationBanners";

                ToggleActivationBannersPVM toggleActivationBannersPVM =
                    new ToggleActivationBannersPVM()
                    {
                        BannerId = BannerId,
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //this.domainsSettings.DomainSettingId),
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                        UserId = this.userId.Value
                    };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).ToggleActivationBanners(toggleActivationBannersPVM);

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
        public IActionResult TemporaryDeleteBanners(int BannerId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });


            try
            {
                serviceUrl = melkavanApiUrl + "/api/BannersManagement/TemporaryDeleteBanners";
                TemporaryDeleteBannersPVM temporaryDeleteBannersPVM = new TemporaryDeleteBannersPVM
                {
                    BannerId = BannerId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).TemporaryDeleteBanners(temporaryDeleteBannersPVM);

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
        public IActionResult CompleteDeleteBanners(int BannerId)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new { });
            BannersVM bannersVM = null;
            try
            {
                string serviceUrl = melkavanApiUrl + "/api/BannersManagement/GetBannersByBannerId";

                GetBannersWithBannerIdPVM getBannersWithBannerIdPVM = new GetBannersWithBannerIdPVM()
                {
                    BannerId = BannerId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetBannersWithBannerId(getBannersWithBannerIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<BannersVM>();


                            if (record != null)
                            {
                                bannersVM = record;

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            try
            {


                serviceUrl = melkavanApiUrl + "/api/BannersManagement/CompleteDeleteBanners";
                CompleteDeleteBannersPVM completeDeleteBannersPVM = new CompleteDeleteBannersPVM
                {
                    BannerId = BannerId,
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    UserId = this.userId.Value
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).CompleteDeleteBanners(completeDeleteBannersPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
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


                var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.userId.Value);

                string bannerFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\BannerFiles\\" + hardcodedDomainName + "\\" + bannersVM.BannerId + "\\Image";

                if (bannersVM != null)
                {
                    if (!string.IsNullOrEmpty(bannersVM.BannerFilePath))
                    {
                        if (System.IO.File.Exists(bannerFolder + "\\" + bannersVM.BannerFilePath))
                        {
                            System.IO.File.Delete(bannerFolder + "\\" + bannersVM.BannerFilePath);
                            System.Threading.Thread.Sleep(100);
                        }

                        if (bannersVM.BannerFileExt.ToLower().Equals(".jpg") ||
                            bannersVM.BannerFileExt.ToLower().Equals(".jpeg") ||
                            bannersVM.BannerFileExt.ToLower().Equals(".png") ||
                            bannersVM.BannerFileExt.ToLower().Equals(".bmp"))
                        {
                            if (System.IO.File.Exists(bannerFolder + "\\thumb_" + bannersVM.BannerFilePath))
                            {
                                System.IO.File.Delete(bannerFolder + "\\thumb_" + bannersVM.BannerFilePath);
                                System.Threading.Thread.Sleep(100);
                            }
                            Directory.Delete(bannerFolder);
                            System.Threading.Thread.Sleep(100);
                            Directory.Delete(bannerFolder + "\\..");
                        }
                    }
                }
            }

            catch (Exception exc)
            { }

            return Json(new { Result = "OK" });
        }





    }
}

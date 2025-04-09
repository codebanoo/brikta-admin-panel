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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;
using VM.Melkavan;
using VM.PVM.Melkavan;
using ApiCallers.MelkavanApiCaller;

namespace Web.Melkavan.Areas.AdminMelkavan.Controllers
{
    [Area("AdminMelkavan")]
    public class AdvertisementMediaManagementController : ExtraAdminController
    {
        public AdvertisementMediaManagementController(IHostEnvironment _hostEnvironment,
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

        public IActionResult AddToAdvertisementFiles(int Id = 0)
        {
            if (Id.Equals(0))
                return RedirectToAction("Index", "PropertiesManagement");

            ViewData["Title"] = "آپلود فیلم/عکس";

            AdvertisementVM advertisementVM = new AdvertisementVM();
            advertisementVM.AdvertisementAddressVM = new AdvertisementAddressVM();

            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            try
            {
                string serviceUrl = melkavanApiUrl + "/api/AdvertisementManagement/GetAdvertisementWithAdvertisementId";

                GetAdvertisementWithAdvertisementIdPVM getAdvertisementWithAdvertisementIdPVM = new GetAdvertisementWithAdvertisementIdPVM()
                {
                    AdvertisementId = Id,
                    Type = "Advertisement",
                    //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.domainsSettings.UserIdCreator.Value,
                    //    this.domainsSettings.DomainSettingId),
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).GetAdvertisementWithAdvertisementId(getAdvertisementWithAdvertisementIdPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultWithRecordObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultWithRecordObjectVM != null)
                    {
                        if (jsonResultWithRecordObjectVM.Result.Equals("OK"))
                        {
                            JObject jObject = jsonResultWithRecordObjectVM.Record;
                            var record = jObject.ToObject<AdvertisementVM>();


                            if (record != null)
                            {
                                advertisementVM = record;

                                #region Fill UserCreatorName

                                //var userIdCreators = records.Where(i => i.UserIdCreator.HasValue).Select(u => u.UserIdCreator.Value).ToList();
                                //var customUsers = consoleBusiness.GetCustomUsers(userIdCreators);

                                //foreach (var record in records)
                                //{
                                //    if (record.UserIdCreator.HasValue)
                                //    {
                                //        var customUser = customUsers.FirstOrDefault(c => c.UserId.Equals(record.UserIdCreator.Value));
                                //        if (customUser != null)
                                //        {
                                //            record.UserCreatorName = customUser.UserName;

                                //            if (!string.IsNullOrEmpty(customUser.Name))
                                //                record.UserCreatorName += " " + customUser.Name;

                                //            if (!string.IsNullOrEmpty(customUser.Family))
                                //                record.UserCreatorName += " " + customUser.Family;
                                //        }
                                //    }
                                //}

                                //statesVMList = records;

                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            { }

            ViewData["AdvertisementVM"] = advertisementVM;

            if (ViewData["DataBackUrl"] == null)
            {
                ViewData["DataBackUrl"] = "/AdminTeniaco/PropertiesMediaManagement/Index";
            }

            return View("AddTo");
        }

        //[DisableRequestSizeLimit]
        [AjaxOnly]
        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<ActionResult> AddToAdvertisementFiles(List<AdvertisementFileUploadPVM> AdvertisementFileUploadPVMList,
            long AdvertisementId,
            List<int>? DeletedPhotosIDs,
            List<string>? DeletedPhotosPaths,
            bool? IsMainChanged)
        {
            JsonResultObjectVM jsonResultObjectVM = new JsonResultObjectVM(new object() { });

            List<AdvertisementFilesVM> advertisementFilesVMList = new List<AdvertisementFilesVM>();

            try
            {
                if (AdvertisementFileUploadPVMList != null)
                {
                    if (AdvertisementFileUploadPVMList.Count > 0)
                    {
                        var domainSettings = consoleBusiness.GetDomainsSettingsWithUserId(this.domainsSettings.UserIdCreator.Value);

                        string advertisementFolder = hostEnvironment.ContentRootPath + "\\wwwroot\\Files\\AdvertisementsFiles\\melkavan.com\\" + AdvertisementId + "\\Media";

                        foreach (var advertisementFileUploadPVM in AdvertisementFileUploadPVMList)
                        {
                            try
                            {
                                string fileName = "";

                                string ext = Path.GetExtension(advertisementFileUploadPVM.File.FileName).ToLower();
                                fileName = Guid.NewGuid().ToString() + ext;
                                using (var fileStream = new FileStream(advertisementFolder + "\\" + fileName, FileMode.Create))
                                {
                                    await advertisementFileUploadPVM.File.CopyToAsync(fileStream);
                                    System.Threading.Thread.Sleep(100);
                                }

                                if (ext.Equals(".jpeg") ||
                                    ext.Equals(".jpg") ||
                                    ext.Equals(".webp") ||
                                    ext.Equals(".png") ||
                                    ext.Equals(".gif") ||
                                    ext.Equals(".bmp"))
                                {
                                    ImageModify.ResizeImage(advertisementFolder + "\\",
                                        fileName,
                                        120,
                                        120);
                                }
                                //else
                                //    if (ext.Equals(".mp4"))
                                //{

                                //}

                                var advertisementFilesVM = new AdvertisementFilesVM()
                                {
                                    CreateEnDate = DateTime.Now,
                                    CreateTime = PersianDate.TimeNow,
                                    UserIdCreator = this.userId.Value,
                                    IsActivated = true,
                                    IsDeleted = false,
                                    AdvertisementFileExt = ext,
                                    AdvertisementFilePath = fileName,
                                    AdvertisementFileTitle = advertisementFileUploadPVM.AdvertisementFileTitle,
                                    AdvertisementFileOrder = advertisementFileUploadPVM.AdvertisementFileOrder,
                                    AdvertisementFileType = "media",
                                    AdvertisementId = AdvertisementId,
                                };
                                //propertyFilesVM.PropertiesVM = new PropertiesVM();
                                //propertyFilesVM.PropertiesVM.PropertyAddressVM = new PropertyAddressVM();

                                advertisementFilesVMList.Add(advertisementFilesVM);
                            }
                            catch (Exception exc)
                            { }
                        }
                    }
                }



                //for removing photos
                if (DeletedPhotosPaths != null)
                {
                    foreach (string path in DeletedPhotosPaths)
                    {
                        string fullPath = hostEnvironment.ContentRootPath + "\\wwwroot" + path.Replace("/", "\\");

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        string thumbPath = fullPath.Replace("\\Media\\", "\\Media\\thumb_");

                        if (System.IO.File.Exists(thumbPath))
                        {
                            System.IO.File.Delete(thumbPath);
                        }
                    }
                }

                string serviceUrl = melkavanApiUrl + "/api/AdvertisementFilesManagement/AddToAdvertisementFiles";

                AddToAdvertisementFilesPVM addToAdvertisementFilesPVM = new AddToAdvertisementFilesPVM()
                {
                    AdvertisementFilesVMList = advertisementFilesVMList,
                    ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    DeletedPhotosIDs = DeletedPhotosIDs,
                    IsMainChanged = IsMainChanged,
                    AdvertisementId = AdvertisementId
                };

                responseApiCaller = new MelkavanApiCaller(serviceUrl).AddToAdvertisementFiles(addToAdvertisementFilesPVM);

                if (responseApiCaller.IsSuccessStatusCode)
                {
                    jsonResultObjectVM = responseApiCaller.JsonResultWithRecordObjectVM;

                    if (jsonResultObjectVM != null)
                    {
                        if (jsonResultObjectVM.Result.Equals("OK"))
                        {
                            return Json(new
                            {
                                Result = "OK",
                                Message = "تغییرات انجام شد",
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
    }
}

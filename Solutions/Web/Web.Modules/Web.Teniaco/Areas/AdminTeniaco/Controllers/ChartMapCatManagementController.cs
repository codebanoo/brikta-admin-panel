using ApiCallers.TeniacoApiCaller;
using AutoMapper;
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
using System.Linq;
using VM.Base;
using VM.PVM.Teniaco;
using VM.Teniaco;
using Web.Core.Controllers;

namespace Web.Teniaco.Areas.AdminTeniaco.Controllers
{
    [Area("AdminTeniaco")]
    public class ChartMapCatManagementController : ExtraAdminController
    {
        public ChartMapCatManagementController(IHostEnvironment _hostEnvironment, IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor, IConfigurationRoot _configurationRoot,
            IMapper _mapper, IConsoleBusiness _consoleBusiness, IPublicServicesBusiness _publicServicesBusiness, IMemoryCache _memoryCache,
            IDistributedCache _distributedCache) : base(_hostEnvironment, _httpContextAccessor,
                _actionContextAccessor, _configurationRoot, _mapper, _consoleBusiness, _publicServicesBusiness, _memoryCache,
                _distributedCache)
        {
        }
        private List<ChartMapCatNodeWithDataVM> result = new List<ChartMapCatNodeWithDataVM>();
        public List<ChartMapCatNodeWithDataVM> GetHierarchicalData(List<MapLayerCategoriesVM> list)
        {
            result = list.Select(s => new ChartMapCatNodeWithDataVM { id = s.MapLayerCategoryId.ToString(), parent = s.ParentMapLayerCategoryId == null ? "#" : s.ParentMapLayerCategoryId.ToString(), name = s.MapLayerCategoryTitle }).ToList();
            List<ChartMapCatNodeWithDataVM> res = new List<ChartMapCatNodeWithDataVM>();
            foreach (var item in result.Where(w => w.parent == "#"))
            {
                item.children = GetNodesCategory(item.id);
                res.Add(item);
            }

            return res;
        }
        private List<ChartMapCatNodeWithDataVM> GetNodesCategory(string id)
        {
            var res = result.Where(w => w.parent == id).ToList();
            if (res.Any())
                foreach (var item in res)
                {
                    item.children = GetNodesCategory(item.id);
                }
            else
                res = null;
            return res;
        }

        public IActionResult Index()
        {
            JsonResultWithRecordsObjectVM jsonResultWithRecordsObjectVM = new JsonResultWithRecordsObjectVM(new object() { });
            JsonResultWithRecordObjectVM jsonResultWithRecordObjectVM = new JsonResultWithRecordObjectVM(new object() { });

            ViewData["Title"] = "ساختار نقشه";
            try
            {
                if (ViewData["CatMapData"] == null)
                {

                    string serviceUrl = teniacoApiUrl + "/api/MapLayerCategoriesManagement/GetAllMapLayerCategoriesList";
                    GetAllMapLayerCategoriesListPVM getAllMapLayerCategoriesListPVM = new GetAllMapLayerCategoriesListPVM()
                    {
                        //ChildsUsersIds = consoleBusiness.GetDomainChildUserIdsWithStoredProcedure(this.userId.Value,
                        //    this.domainsSettings.DomainSettingId)
                        ChildsUsersIds = consoleBusiness.GetChildUserIdsMatchWithLevelIds(this.areaName, this.controllerName, this.actionName, this.userId.Value, this.parentUserId.Value,
                            this.domainsSettings.UserIdCreator.Value, this.roleIds, this.levelIds),
                    };

                    responseApiCaller = new TeniacoApiCaller(serviceUrl).GetAllMapLayerCategoriesList(getAllMapLayerCategoriesListPVM);

                    if (responseApiCaller.IsSuccessStatusCode)
                    {
                        jsonResultWithRecordsObjectVM = responseApiCaller.JsonResultWithRecordsObjectVM;

                        if (jsonResultWithRecordsObjectVM != null)
                        {
                            if (jsonResultWithRecordsObjectVM.Result.Equals("OK"))
                            {
                                #region Fill UserCreatorName

                                JArray jArray = jsonResultWithRecordsObjectVM.Records;
                                var records = jArray.ToObject<List<MapLayerCategoriesVM>>();

                                if (records.Count > 0)
                                {
                                    MapLayerCategoriesVM root = records.SingleOrDefault(a => a.ParentMapLayerCategoryId == null);
                                    var _children = GetHierarchicalData(records);
                                    var final = new ChartMapCatNodeWithDataVM()
                                    {
                                        id = root.MapLayerCategoryId.ToString(),
                                        /// data= new NodeDataVM { code=""},b
                                        name = root.MapLayerCategoryTitle,
                                        children = _children
                                    };
                                    ViewData["CatMapData"] = final;
                                }
                                #endregion


                            }
                        }
                    }
                }
            }
            catch
            {

                return Json(new
                {
                    Result = "ERROR",
                    Message = "خطا"
                });
            }
            return View();
        }


    }
}

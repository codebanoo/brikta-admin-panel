using Models.Business.ConsoleBusiness;
using Web.Core.Ext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
//using Microsoft.Aspnet.Core;

namespace Web.Automation.Areas.AdminAutomation.ViewComponents
{
    public class AdminAutomationDashboardViewComponent : ViewComponent
    {
        IHostEnvironment hostEnvironment;
        IHttpContextAccessor httpContextAccessor;
        IActionContextAccessor actionContextAccessor;
        IConsoleBusiness consoleBusiness;
        long? userId = 0;

        public AdminAutomationDashboardViewComponent(IHostEnvironment _hostEnvironment,
            IHttpContextAccessor _httpContextAccessor,
            IActionContextAccessor _actionContextAccessor,
            IConsoleBusiness _consoleBusiness)
        {
            hostEnvironment = _hostEnvironment;
            httpContextAccessor = _httpContextAccessor;
            actionContextAccessor = _actionContextAccessor;
            consoleBusiness = _consoleBusiness;
        }

        public IViewComponentResult Invoke()
        {
            //string areaName = "";
            //string controllerName = "";
            //string actionName = "";

            //BaseAuthentication.GetUserId(httpContextAccessor,
            //    ref userId);

            //#region get area name from route data

            //if (actionContextAccessor.ActionContext.RouteData.Values["area"] != null)
            //    areaName = actionContextAccessor.ActionContext.RouteData.Values["area"].ToString();
            //else
            //    if (actionContextAccessor.ActionContext.RouteData.DataTokens["area"] != null)
            //    areaName = actionContextAccessor.ActionContext.RouteData.DataTokens["area"].ToString();

            //#endregion

            //#region get controller name from route data

            //if (actionContextAccessor.ActionContext.RouteData.Values["controller"] != null)
            //    controllerName = actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString();
            //else
            //    if (actionContextAccessor.ActionContext.RouteData.DataTokens["controller"] != null)
            //    controllerName = actionContextAccessor.ActionContext.RouteData.DataTokens["controller"].ToString();

            //#endregion

            //#region get action name from route data

            //if (actionContextAccessor.ActionContext.RouteData.Values["action"] != null)
            //    actionName = actionContextAccessor.ActionContext.RouteData.Values["action"].ToString();
            //else
            //    if (actionContextAccessor.ActionContext.RouteData.DataTokens["action"] != null)
            //    actionName = actionContextAccessor.ActionContext.RouteData.DataTokens["action"].ToString();

            //#endregion

            //try
            //{
            //    //////////ViewData["SendedMessage"] = publicBusiness.GetCallUsCount();
            //    //////////ViewData["Pending"] = storeBusiness.GetNotVerifiedOrderCount(this.userId.Value);
            //    ViewData["UnverifiedUsers"] = consoleBusiness.GetUnverifiedUsersCount(this.userId.Value);
            //    //ViewData["UnapprovedTransactions"] = business.GetNotVerifiedTransactionsCount(this.userId.Value);
            //    //////////ViewData["VerifiedOrders"] = storeBusiness.GetVerifiedOrderCount(this.userId.Value);
            //    //////////ViewData["Observed"] = storeBusiness.GetSeenOrdersCount(this.userId.Value);
            //}
            //catch (Exception exc)
            //{
            //}

            return View("AdminAutomationDashboard");
        }
    }
}

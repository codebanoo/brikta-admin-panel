using Services.Xml;
using Services.Xml.Entities;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Web.Console.Areas.Users.ViewComponents
{
    public class UserDashboardViewComponent : ViewComponent
    {
        IHostEnvironment hostEnvironment;
        IActionContextAccessor actionContextAccessor;

        public UserDashboardViewComponent(IHostEnvironment _hostEnvironment,
            IActionContextAccessor _actionContextAccessor)
        {
            hostEnvironment = _hostEnvironment;
            actionContextAccessor = _actionContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            string areaName = "";
            string controllerName = "";
            string actionName = "";

            #region get area name from route data

            if (actionContextAccessor.ActionContext.RouteData.Values["area"] != null)
                areaName = actionContextAccessor.ActionContext.RouteData.Values["area"].ToString();
            else
                if (actionContextAccessor.ActionContext.RouteData.DataTokens["area"] != null)
                areaName = actionContextAccessor.ActionContext.RouteData.DataTokens["area"].ToString();

            #endregion

            #region get controller name from route data

            if (actionContextAccessor.ActionContext.RouteData.Values["controller"] != null)
                controllerName = actionContextAccessor.ActionContext.RouteData.Values["controller"].ToString();
            else
                if (actionContextAccessor.ActionContext.RouteData.DataTokens["controller"] != null)
                controllerName = actionContextAccessor.ActionContext.RouteData.DataTokens["controller"].ToString();

            #endregion

            #region get action name from route data

            if (actionContextAccessor.ActionContext.RouteData.Values["action"] != null)
                actionName = actionContextAccessor.ActionContext.RouteData.Values["action"].ToString();
            else
                if (actionContextAccessor.ActionContext.RouteData.DataTokens["action"] != null)
                actionName = actionContextAccessor.ActionContext.RouteData.DataTokens["action"].ToString();

            #endregion

            return View("UserDashboard");
        }
    }
}

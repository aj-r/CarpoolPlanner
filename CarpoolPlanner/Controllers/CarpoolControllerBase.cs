using System;
using System.Web;
using System.Web.Mvc;

namespace CarpoolPlanner.Controllers
{
    public class CarpoolControllerBase : NgController
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                base.OnException(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Result = Content(filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
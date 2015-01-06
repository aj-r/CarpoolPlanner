using System;
using System.Web;
using System.Web.Mvc;

namespace CarpoolPlanner.Controllers
{
    public class CarpoolControllerBase : NgController
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            MvcApplication.HandleError(filterContext.Exception);
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                base.OnException(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                var message = filterContext.Exception.Message;
                var ex = filterContext.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += "\nInner Exception: " + ex.Message;
                }
                filterContext.Result = Content(message);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
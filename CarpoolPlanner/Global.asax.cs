using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CarpoolPlanner.Model;
using log4net;
using log4net.Config;

namespace CarpoolPlanner
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Remove the default JSON provider and use a special model binder instead.
            // This allows us to use strongly typed deserialization.
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ModelBinders.Binders.DefaultBinder = new JsonNetModelBinder();

            JsonSerializerFactory.Current = new CarpoolSerializerFactory();
            log.Info("Application started");
        }

        public void Application_End(object sender, EventArgs e)
        {
            log.Info("Application ended");
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            if (AppUtils.CurrentUser != null)
                ThreadContext.Properties["UserId"] = AppUtils.CurrentUser.Id;
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            // For some reason, the first error in this event handler does not get logged, but the rest do.
            // This is meant to be a dummy first error to make sure the rest get logged.
            log.Error("An error occurred");
            HandleError(ex);
        }

        public static void HandleError(Exception ex)
        {
            try
            {
                if (ex != null)
                {
                    log.Error(ex.ToString());
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        log.Error("Inner Exception: " + ex.ToString());
                    }
                }
                else
                {
                    log.Error("An unhandled exception occurred");
                }
            }
            catch { }
        }
    }
}
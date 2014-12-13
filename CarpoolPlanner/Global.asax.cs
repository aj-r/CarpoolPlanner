using System;
using System.Web;
using log4net;
using log4net.Config;

namespace CarpoolPlanner
{
    public class Global : HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));

        private void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            log.Info("Application started");
        }

        public void Application_End(object sender, EventArgs e)
        {
            log.Info("Application ended");
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(App.CurrentUserId))
                ThreadContext.Properties["UserId"] = App.CurrentUserId;
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            log.Error(ex);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Core;

namespace CarpoolPlanner.Model.Appender
{
    public class CarpoolPlannerAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                using (var context = ApplicationDbContext.Create())
                {
                    var userId = ThreadContext.Properties["UserId"];
                    var ndc = ThreadContext.Stacks["NDC"];
                    var log = new Log
                    {
                        Message = (string)loggingEvent.MessageObject,
                        Level = loggingEvent.Level.Name,
                        Logger = loggingEvent.LoggerName,
                        UserId = (long?)userId,
                        Date = DateTime.UtcNow,
                        Ndc = ndc != null ? ndc.ToString() : null
                    };
                    context.Logs.Add(log);
                    context.SaveChanges();
                }
            }
            catch
            { }
        }
    }
}
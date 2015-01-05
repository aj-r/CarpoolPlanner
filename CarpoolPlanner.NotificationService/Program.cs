using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarpoolPlanner.Model;
using log4net;
using log4net.Config;
using TextNow.Net;

namespace CarpoolPlanner.NotificationService
{
    public static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            NotificationManager manager;
            XmlConfigurator.Configure();

            if (args.Length == 1)
            {
                string userId = args[0];
                manager = NotificationManager.GetInstance();
                using (var context = ApplicationDbContext.Create())
                {
                    var user = context.Users.Find(userId);
                    if (user == null)
                    {
                        Console.WriteLine("User '" + userId + "' does not exist.");
                        return;
                    }
                    Console.WriteLine("Sending test notification to " + userId + "...");
                    manager.SendNotification(user, "test message").Wait();
                    Console.WriteLine("Test notification sent; exiting.");
                    log.Info("Test notification sent to " + userId + "; exiting");
                }
#if DEBUG
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
#endif
                return;
            }

            log.Info("CarpoolPlanner notification service started");
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            manager = NotificationManager.GetInstance();
            manager.Init();
            var listener = new ChangeListener();
            listener.Start();

            Console.WriteLine("CarpoolPlanner notification service started.");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            log.Info("CarpoolPlanner notification service stopped");
            listener.Stop();
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        public static void HandleException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            log.Error(ex.ToString());
            Console.Error.WriteLine(ex.ToString());
        }
    }
}

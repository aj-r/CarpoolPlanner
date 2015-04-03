using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarpoolPlanner.Model;
using log4net;
using log4net.Config;

namespace CarpoolPlanner.NotificationService
{
    public static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static bool Verbose = false;

        static void Main(string[] args)
        {
            GeneralExceptionHandler.ExceptionEncountered += ex => HandleException(ex);

            AppDomain.CurrentDomain.UnhandledException += GeneralExceptionHandler.Raise;
            // Attempt to catch exceptions that occur in unobserved tasks. This is a last resort and is not guaranteed to work,
            // so you should try to never have unobserved tasks. If you don't want to await the task, use the WithErrorHandler() extension instead.
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                GeneralExceptionHandler.Raise(e.Exception);
                e.SetObserved();
            };
            
            XmlConfigurator.Configure();

            if (args.Length >= 1)
            {
                switch (args[0])
                {
                    case "-v":
                    case "--verbose":
                        Verbose = true;
                        break;
                    case "-t":
                    case "--test":
                        // Argument was specified -> this is for testing.
                        // Send a test notification to the user identified by the specified login name.
                        if (args.Length < 2)
                        {
                            Console.WriteLine("Test user email not specified.");
                            Console.WriteLine("Usage: CarpoolPlanner.NotificationService.exe --test email@example.com");
                            return;
                        }
                        string email = args[1];
                        var manager = NotificationManager.GetInstance();
                        var contextProvider = new CarpoolPlannerDbContextProvider();
                        using (var context = contextProvider.GetContext())
                        {
                            var user = context.Users.FirstOrDefault(u => u.Email == email);
                            if (user == null)
                            {
                                Console.WriteLine("User '" + email + "' does not exist.");
                                return;
                            }
                            log.Info("Sending test notification to " + email + "...");
                            Console.WriteLine("Sending test notification to " + email + "...");
                            if (manager.SendNotification(user, "Test message", "test message").Result)
                            {
                                Console.WriteLine("Test notification sent; exiting.");
                                log.Info("Test notification sent to " + email + "; exiting");
                            }
                            else
                            {
                                Console.WriteLine("Failed to send test notification; exiting.");
                                log.Info("Failed to send test notification to " + email + "; exiting");
                            }
                        }
#if DEBUG
                        Console.WriteLine("Press enter to exit.");
                        Console.ReadLine();
#endif
                        return;
                }
            }

            log.Info("CarpoolPlanner notification service started");

            NotificationManager.GetInstance().InitializeNotificationTimes().WithErrorHandler();
            var listener = new ChangeListener();
            listener.Start();
            if (Verbose)
                Console.WriteLine("CarpoolPlanner notification service started.");
            if (Console.In is StreamReader)
            {
                if (Verbose)
                    Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
            else
            {
                // When running in the background on a UNIX system, Console.ReadLine() returns immediately.
                // In this case, just block the thread until the process is killed.
                if (Verbose)
                    Console.WriteLine("Running in background. Send SIGTERM to end process.");
                Thread.Sleep(Timeout.Infinite);
            }
            if (Verbose)
                Console.WriteLine("CarpoolPlanner notification service stopping...");
            log.Info("CarpoolPlanner notification service stopping");
            listener.Stop();
        }

        private static void HandleException(Exception ex)
        {
            log.Error(ex.ToString());
            Console.Error.WriteLine(ex.ToString());
        }
    }
}

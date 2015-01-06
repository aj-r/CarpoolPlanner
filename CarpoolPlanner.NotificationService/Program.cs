using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static bool Verbose = false;

        static void Main(string[] args)
        {
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
                            Console.WriteLine("Test user not specified.");
                            Console.WriteLine("Usage: CarpoolPlanner.NotificationService.exe --test username");
                            return;
                        }
                        string loginName = args[1];
                        var manager = NotificationManager.GetInstance();
                        using (var context = ApplicationDbContext.Create())
                        {
                            var user = context.Users.FirstOrDefault(u => u.LoginName == loginName);
                            if (user == null)
                            {
                                Console.WriteLine("User '" + loginName + "' does not exist.");
                                return;
                            }
                            Console.WriteLine("Sending test notification to " + loginName + "...");
                            manager.SendNotification(user, "test message").Wait();
                            Console.WriteLine("Test notification sent; exiting.");
                            log.Info("Test notification sent to " + loginName + "; exiting");
                        }
#if DEBUG
                        Console.WriteLine("Press enter to exit.");
                        Console.ReadLine();
#endif
                        return;
                }
            }

            log.Info("CarpoolPlanner notification service started");
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            NotificationManager.GetInstance().Init();
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

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject);
        }

        public static void HandleException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            log.Error(ex.ToString());
            if (Verbose)
                Console.Error.WriteLine(ex.ToString());
            // TODO: notify admin
        }
    }
}

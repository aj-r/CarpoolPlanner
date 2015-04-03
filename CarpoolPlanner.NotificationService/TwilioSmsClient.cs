using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Twilio;

namespace CarpoolPlanner.NotificationService
{
    public class TwilioSmsClient : ISmsClient
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TwilioSmsClient));
        private static readonly TimeSpan sendNotificationTimeout = TimeSpan.FromSeconds(5);

        private readonly TwilioRestClient innerClient;
        private readonly string from;
        private readonly string statusCallbackUrl;
        // TODO: For debugging only; remove
        private readonly string accountSid;

        public TwilioSmsClient()
            : this(ConfigurationManager.AppSettings["TwilioAccountSid"],
                ConfigurationManager.AppSettings["TwilioAuthToken"],
                ConfigurationManager.AppSettings["TwilioPhoneNumber"],
                ConfigurationManager.AppSettings["TwilioStatusCallbackUrl"])
        { }

        public TwilioSmsClient(string accountSid, string authToken, string from, string statusCallbackUrl)
        {
            innerClient = new TwilioRestClient(accountSid, authToken);
            this.from = from;
            this.statusCallbackUrl = statusCallbackUrl;

            this.accountSid = accountSid;
        }

        public string From { get { return from; } }

        public string StatusCallbackUrl { get { return statusCallbackUrl; } }

        public async Task<bool> SendMessage(string to, string message, CancellationToken token)
        {
            var tasks = new List<Task<bool>>();
            var messages = SplitMessage(message, 160);
            foreach (var subMessage in messages)
            {
                // Set a timeout
                var tcs = new TaskCompletionSource<bool>();
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter((int)sendNotificationTimeout.TotalMilliseconds);
                cts.Token.Register(() =>
                {
                    if (!tcs.TrySetResult(false))
                        return;
                    var logMessage = "Timed out while sending SMS to " + to;
                    log.ErrorFormat(logMessage);
                    Console.WriteLine(logMessage);
                }, false);

                if (Program.Verbose)
                    Console.WriteLine("AccointSid: " + accountSid);

                // Send the message
                innerClient.SendSmsMessage(From, to, subMessage, statusCallbackUrl, m =>
                {
                    // Log the result and mark the task as completed
                    if (m == null)
                    {
                        log.Warn("Twilio returned a null message.");
                        tcs.TrySetResult(false);
                        return;
                    }
                    try
                    {
                        // Status should be "queued" at this point. If null, that probably indicates an authentication problem.
                        // I believe you need to set a callback URL to get the sent/error statuses, and that doesn't work well for local testing (or with Tasks).
                        if (m.Status != null)
                        {
                            log.Debug("SMS message sent.");
                            tcs.TrySetResult(true);
                        }
                        else
                        {
                            log.ErrorFormat("Failed to send SMS to " + to);
                            Console.WriteLine("Failed to send SMS to " + to);
                            tcs.TrySetResult(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error sending SMS to " + to + ": " + ex);
                        tcs.TrySetResult(false);
                    }
                });
                tasks.Add(tcs.Task);
            }
            var results = await Task.WhenAll(tasks);
            return results.All(r => r);
        }

        public static string[] SplitMessage(string message, int maxLength)
        {
            if (message == null)
                return new string[0];
            var messages = new string[(message.Length + maxLength - 1) / maxLength];
            for (var i = 0; i < messages.Length; ++i)
                messages[i] = message.Substring(maxLength * i, Math.Min(maxLength, message.Length - maxLength * i));
            return messages;
        }
    }
}

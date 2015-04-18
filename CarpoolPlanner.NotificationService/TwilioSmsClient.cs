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
        }

        public string From { get { return from; } }

        public string StatusCallbackUrl { get { return statusCallbackUrl; } }

        public async Task<bool> SendMessage(string to, string message, CancellationToken token)
        {
            var success = true;

            // HACK: certain special characters reduce the message size limit to 70.
            // Replace those characters to get the 160 character message limit.
            message = message.Replace("ë", "e");

            // Twilio is supposed to automatically split messages greater than 160 characters.
            // However, it instead seems to fail silently for those messages, so we need to split them manually.
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
                // Wait for the message to be sent before continuing.
                // This helps messages to be received in the correct order.
                success = await tcs.Task && success;
            }
            return success;
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

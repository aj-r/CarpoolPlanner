using System.Configuration;
using Twilio;

namespace CarpoolPlanner.NotificationService
{
    public class TwilioRestClientImpl : TwilioRestClient, ITwilioRestClient
    {
        public TwilioRestClientImpl()
            : this(ConfigurationManager.AppSettings["TwilioAccountSid"], ConfigurationManager.AppSettings["TwilioAuthToken"])
        { }

        public TwilioRestClientImpl(string accountSid, string authToken)
            : this(accountSid, authToken, null)
        { }

        public TwilioRestClientImpl(string accountSid, string authToken, string accountResourceSid)
            : base(accountSid, authToken, accountResourceSid)
        { }
    }
}

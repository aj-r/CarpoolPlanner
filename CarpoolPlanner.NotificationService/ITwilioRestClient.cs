using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace CarpoolPlanner.NotificationService
{
    public interface ITwilioRestClient
    {
        // Summary:
        //     Create a new Address.
        //
        // Parameters:
        //   friendlyName:
        //     Friendly name (optional)
        //
        //   customerName:
        //     Customer name
        //
        //   street:
        //     Number and street of the address
        //
        //   city:
        //     City name for the address
        //
        //   region:
        //     The state or region
        //
        //   postalCode:
        //     Postal code
        //
        //   isoCountry:
        //     ISO 3166-1 2-letter country code
        //
        // Returns:
        //     The address.
        Address AddAddress(string friendlyName, string customerName, string street, string city, string region, string postalCode, string isoCountry);
        //
        // Summary:
        //     Create a new Address.
        //
        // Parameters:
        //   friendlyName:
        //     Friendly name (optional)
        //
        //   customerName:
        //     Customer name.
        //
        //   street:
        //     Street.
        //
        //   city:
        //     City.
        //
        //   region:
        //     Region.
        //
        //   postalCode:
        //     Postal code.
        //
        //   isoCountry:
        //     Iso country.
        //
        //   callback:
        //     Method to call upon successful completion.
        void AddAddress(string friendlyName, string customerName, string street, string city, string region, string postalCode, string isoCountry, Action<Address> callback);
        //
        // Summary:
        //     Create a new application
        //
        // Parameters:
        //   friendlyName:
        //     The friendly name to name the application
        //
        //   options:
        //     Optional parameters to use when purchasing number
        Application AddApplication(string friendlyName, ApplicationOptions options);
        //
        // Summary:
        //     Create a new application
        //
        // Parameters:
        //   friendlyName:
        //     The friendly name to name the application
        //
        //   options:
        //     Optional parameters to use when purchasing number
        //
        //   callback:
        //     Method to call upon successful completion
        void AddApplication(string friendlyName, ApplicationOptions options, Action<Application> callback);
        //
        // Summary:
        //     Purchase/provision a local phone number
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        IncomingPhoneNumber AddIncomingLocalPhoneNumber(PhoneNumberOptions options);
        //
        // Summary:
        //     Purchase/provision a local phone number
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        //
        //   callback:
        //     Method to call upon successful completion
        void AddIncomingLocalPhoneNumber(PhoneNumberOptions options, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Purchase/provision a phone number
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        IncomingPhoneNumber AddIncomingPhoneNumber(PhoneNumberOptions options);
        //
        // Summary:
        //     Purchase/provision a phone number.
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        //
        //   callback:
        //     Method to call upon successful completion
        void AddIncomingPhoneNumber(PhoneNumberOptions options, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Purchase/provision a toll free phone number
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        IncomingPhoneNumber AddIncomingTollFreePhoneNumber(PhoneNumberOptions options);
        //
        // Summary:
        //     Purchase/provision a toll free phone number
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when purchasing number
        //
        //   callback:
        //     Method to call upon successful completion
        void AddIncomingTollFreePhoneNumber(PhoneNumberOptions options, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Adds a new validated CallerID to your account. After making this request,
        //     Twilio will return to you a validation code and dial the phone number given
        //     to perform validation. The code returned must be entered via the phone before
        //     the CallerID will be added to your account.  Makes a POST request to an OutgoingCallerIds
        //     List resource.
        //
        // Parameters:
        //   phoneNumber:
        //     The phone number to verify. Should be formatted with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Twilio will also accept unformatted US
        //     numbers e.g., (415) 555-1212, 415-555-1212.
        //
        //   options:
        //     Optional parameters to use when purchasing number
        ValidationRequestResult AddOutgoingCallerId(string phoneNumber, OutgoingCallerIdOptions options);
        //
        // Summary:
        //     Adds a new validated CallerID to your account. After making this request,
        //     Twilio will return to you a validation code and dial the phone number given
        //     to perform validation. The code returned must be entered via the phone before
        //     the CallerID will be added to your account.
        //
        // Parameters:
        //   phoneNumber:
        //     The phone number to verify. Should be formatted with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Twilio will also accept unformatted US
        //     numbers e.g., (415) 555-1212, 415-555-1212.
        //
        //   options:
        //     Optional parameters to use when purchasing number
        //
        //   callback:
        //     Method to call upon successful completion
        void AddOutgoingCallerId(string phoneNumber, OutgoingCallerIdOptions options, Action<ValidationRequestResult> callback);
        //
        // Summary:
        //     Adds a new validated CallerID to your account. After making this request,
        //     Twilio will return to you a validation code and dial the phone number given
        //     to perform validation. The code returned must be entered via the phone before
        //     the CallerID will be added to your account.  Makes a POST request to an OutgoingCallerIds
        //     List resource.
        //
        // Parameters:
        //   phoneNumber:
        //     The phone number to verify. Should be formatted with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Twilio will also accept unformatted US
        //     numbers e.g., (415) 555-1212, 415-555-1212.
        //
        //   friendlyName:
        //     A human readable description for the new caller ID with maximum length 64
        //     characters. Defaults to a nicely formatted version of the number.
        //
        //   callDelay:
        //     The number of seconds, between 0 and 60, to delay before initiating the validation
        //     call. Defaults to 0.
        //
        //   extension:
        //     Digits to dial after connecting the validation call.
        ValidationRequestResult AddOutgoingCallerId(string phoneNumber, string friendlyName, int? callDelay, string extension);
        //
        // Summary:
        //     Adds a new validated CallerID to your account. After making this request,
        //     Twilio will return to you a validation code and dial the phone number given
        //     to perform validation. The code returned must be entered via the phone before
        //     the CallerID will be added to your account.
        //
        // Parameters:
        //   phoneNumber:
        //     The phone number to verify. Should be formatted with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Twilio will also accept unformatted US
        //     numbers e.g., (415) 555-1212, 415-555-1212.
        //
        //   friendlyName:
        //     A human readable description for the new caller ID with maximum length 64
        //     characters. Defaults to a nicely formatted version of the number.
        //
        //   callDelay:
        //     The number of seconds, between 0 and 60, to delay before initiating the validation
        //     call. Defaults to 0.
        //
        //   extension:
        //     Digits to dial after connecting the validation call.
        //
        //   callback:
        //     Method to call upon successful completion
        void AddOutgoingCallerId(string phoneNumber, string friendlyName, int? callDelay, string extension, Action<ValidationRequestResult> callback);
        //
        // Summary:
        //     Changes the status of a subaccount. You must be authenticated as the master
        //     account to call this method on a subaccount.  WARNING: When closing an account,
        //     Twilio will release all phone numbers assigned to it and shut it down completely.
        //     You can't ever use a closed account to make and receive phone calls or send
        //     and receive SMS messages. It's closed, gone, kaput. It will still appear
        //     in your accounts list, and you will still have access to historical data
        //     for that subaccount, but you cannot reopen a closed account.
        //
        // Parameters:
        //   subAccountSid:
        //     The subaccount to change the status on
        //
        //   status:
        //     The status to change the subaccount to
        Account ChangeSubAccountStatus(string subAccountSid, AccountStatus status);
        //
        // Summary:
        //     Changes the status of a subaccount. You must be authenticated as the master
        //     account to call this method on a subaccount.  WARNING: When closing an account,
        //     Twilio will release all phone numbers assigned to it and shut it down completely.
        //     You can't ever use a closed account to make and receive phone calls or send
        //     and receive SMS messages. It's closed, gone, kaput. It will still appear
        //     in your accounts list, and you will still have access to historical data
        //     for that subaccount, but you cannot reopen a closed account.  ///
        //
        // Parameters:
        //   subAccountSid:
        //     The subaccount to change the status on
        //
        //   status:
        //     The status to change the subaccount to
        //
        //   callback:
        //     Method to call upon successful completion
        void ChangeSubAccountStatus(string subAccountSid, AccountStatus status, Action<Account> callback);
        //
        // Summary:
        //     Create a new Credential resource in a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to add the new Credential to
        //
        //   username:
        //     The Credential Username
        //
        //   password:
        //     The Credential Password
        Credential CreateCredential(string credentialListSid, string username, string password);
        //
        // Summary:
        //     Create a new Credential resource in a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to add the new Credential to
        //
        //   username:
        //     The Credential Username
        //
        //   password:
        //     The Credential Password
        void CreateCredential(string credentialListSid, string username, string password, Action<Credential> callback);
        //
        // Summary:
        //     Creates a new CredentialList resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the CredentialList to create.
        CredentialList CreateCredentialList(string friendlyName);
        //
        // Summary:
        //     Creates a new CredentialList resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the CredentialList to create.
        void CreateCredentialList(string friendlyName, Action<CredentialList> callback);
        //
        // Summary:
        //     Maps a CredentialList to a specific SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to map to
        //
        //   credentialListSid:
        //     The Sid of the CredentialList to map to
        CredentialListMapping CreateCredentialListMapping(string domainSid, string credentialListSid);
        //
        // Summary:
        //     Maps a CredentialList to a specific SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to map to
        //
        //   credentialListSid:
        //     The Sid of the CredentialList to map to
        void CreateCredentialListMapping(string domainSid, string credentialListSid, Action<IpAccessControlListMapping> callback);
        //
        // Summary:
        //     Creates a new SIP Domain resource
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when creating a new SIP domain. DomainName is
        //     required and you must pick a unique domain name that ends in ".sip.twilio.com"
        Domain CreateDomain(DomainOptions options);
        //
        // Summary:
        //     Creates a new SIP Domain resource
        //
        // Parameters:
        //   domainName:
        //     The name of the SIP Domain to create. You must pick a unique domain name
        //     that ends in ".sip.twilio.com"
        Domain CreateDomain(string domainName);
        //
        // Summary:
        //     Creates a new SIP Domain resource
        //
        // Parameters:
        //   options:
        //     Optional parameters to use when creating a new SIP domain. DomainName is
        //     required and you must pick a unique domain name that ends in ".sip.twilio.com"
        void CreateDomain(DomainOptions options, Action<Domain> callback);
        //
        // Summary:
        //     Creates a new SIP Domain resource
        //
        // Parameters:
        //   domainName:
        //     The name of the SIP Domain to create. You must pick a unique domain name
        //     that ends in ".sip.twilio.com"
        void CreateDomain(string domainName, Action<Domain> callback);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        Feedback CreateFeedback(string callSid, int qualityScore);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        void CreateFeedback(string callSid, int qualityScore, Action<Feedback> callback);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        Feedback CreateFeedback(string callSid, int qualityScore, List<string> issues);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        Feedback CreateFeedback(string callSid, int qualityScore, string issue);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        void CreateFeedback(string callSid, int qualityScore, List<string> issues, Action<Feedback> callback);
        //
        // Summary:
        //     Creates a new feedback entry for a specific CallSid.
        void CreateFeedback(string callSid, int qualityScore, string issue, Action<Feedback> callback);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        // Returns:
        //     A feedback summary.
        FeedbackSummary CreateFeedbackSummary(DateTime startDate, DateTime endDate);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        //   callback:
        //     Asynchronous callback.
        void CreateFeedbackSummary(DateTime startDate, DateTime endDate, Action<FeedbackSummary> callback);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        //   includeSubaccounts:
        //     If set to true include subaccounts.
        //
        // Returns:
        //     A feedback summary.
        FeedbackSummary CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        //   includeSubaccounts:
        //     If set to true include subaccounts.
        //
        //   callback:
        //     Asynchronous callback.
        void CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts, Action<FeedbackSummary> callback);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        //   includeSubaccounts:
        //     If set to true include subaccounts.
        //
        //   statusCallback:
        //     Status callback URL.
        //
        //   statusCallbackMethod:
        //     Status callback URL method. Either GET or POST.
        //
        // Returns:
        //     A feedback summary.
        FeedbackSummary CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts, string statusCallback, string statusCallbackMethod);
        //
        // Summary:
        //     Creates a feedback summary.
        //
        // Parameters:
        //   startDate:
        //     Start date.
        //
        //   endDate:
        //     End date.
        //
        //   includeSubaccounts:
        //     If set to true include subaccounts.
        //
        //   statusCallback:
        //     Status callback.
        //
        //   statusCallbackMethod:
        //     Status callback method.
        //
        //   callback:
        //     Asynchronous callback.
        void CreateFeedbackSummary(DateTime startDate, DateTime endDate, bool includeSubaccounts, string statusCallback, string statusCallbackMethod, Action<FeedbackSummary> callback);
        //
        // Summary:
        //     Creates a new IpAccessControlList resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the IpAccessControlList to create.
        IpAccessControlList CreateIpAccessControlList(string friendlyName);
        //
        // Summary:
        //     Creates a new IpAccessControlList resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the IpAccessControlList to create.
        void CreateIpAccessControlList(string friendlyName, Action<IpAccessControlList> callback);
        //
        // Summary:
        //     Maps an IpAccessControlList to a specific SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to map to
        //
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList to map to
        IpAccessControlListMapping CreateIpAccessControlListMapping(string domainSid, string ipAccessControlListSid);
        //
        // Summary:
        //     Maps an IpAccessControlList to a specific SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to map to
        //
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList to map to
        void CreateIpAccessControlListMapping(string domainSid, string ipAccessControlListSid, Action<IpAccessControlListMapping> callback);
        //
        // Summary:
        //     Creates a new IpAddress resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControList to add the IpAddress to
        //
        //   friendlyName:
        //     The name of the IpAddress to create.
        //
        //   ipAddress:
        //     The address value of the IpAddress
        IpAddress CreateIpAddress(string ipAccessControlListSid, string friendlyName, string ipAddress);
        //
        // Summary:
        //     Creates a new IpAddress resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControList to add the IpAddress to
        //
        //   friendlyName:
        //     The name of the IpAddress to create.
        //
        //   ipAddress:
        //     The address value of the IpAddress
        void CreateIpAddress(string ipAccessControlListSid, string friendlyName, string ipAddress, Action<IpAddress> callback);
        //
        // Summary:
        //     Creates a new Queue resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the Queue
        Queue CreateQueue(string friendlyName);
        //
        // Summary:
        //     Creates a new Queue resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the Queue
        //
        //   callback:
        //     Method to call upon successful completion
        void CreateQueue(string friendlyName, Action<Queue> callback);
        //
        // Summary:
        //     Creates a new Queue resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the Queue
        //
        //   maxSize:
        //     The maximum number of calls allowed in the queue
        Queue CreateQueue(string friendlyName, int maxSize);
        //
        // Summary:
        //     Creates a new Queue resource
        //
        // Parameters:
        //   friendlyName:
        //     The name of the Queue
        //
        //   maxSize:
        //     The maximum number of calls allowed in the queue
        //
        //   callback:
        //     Method to call upon successful completion
        void CreateQueue(string friendlyName, int maxSize, Action<Queue> callback);
        //
        // Summary:
        //     Creates a new subaccount under the authenticated account. Makes a POST request
        //     to the Account List resource.
        //
        // Parameters:
        //   friendlyName:
        //     Name associated with this account for your own reference (can be empty string)
        Account CreateSubAccount(string friendlyName);
        //
        // Summary:
        //     Creates a new subaccount under the authenticated account
        //
        // Parameters:
        //   friendlyName:
        //     Name associated with this account for your own reference (can be empty string)
        //
        //   callback:
        //     Method to call upon successful completion
        void CreateSubAccount(string friendlyName, Action<Account> callback);
        //
        // Summary:
        //     Create a new token
        Token CreateToken();
        //
        // Summary:
        //     Create a new token
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void CreateToken(Action<Token> callback);
        //
        // Summary:
        //     Create a new token
        //
        // Parameters:
        //   ttl:
        //     The friendly name to name the application
        Token CreateToken(int ttl);
        //
        // Summary:
        //     Create a new token
        //
        // Parameters:
        //   ttl:
        //     The time in seconds before this token expires
        //
        //   callback:
        //     Method to call upon successful completion
        void CreateToken(int ttl, Action<Token> callback);
        //
        // Summary:
        //     Creates a new Usage Trigger resource
        //
        // Parameters:
        //   options:
        //     A UsageTriggerOption object that defines the different trigger options
        UsageTrigger CreateUsageTrigger(UsageTriggerOptions options);
        //
        // Summary:
        //     Creates a new Usage Trigger resource
        //
        // Parameters:
        //   options:
        //     A UsageTriggerOption object that defines the different trigger options
        //
        //   callback:
        void CreateUsageTrigger(UsageTriggerOptions options, Action<UsageTrigger> callback);
        //
        // Summary:
        //     Creates a new Usage Trigger resource
        //
        // Parameters:
        //   usageCategory:
        //     The usage category to trigger on
        //
        //   triggerValue:
        //     The value to trigger on
        //
        //   callbackUrl:
        //     The URL to call once a trigger value has been met
        UsageTrigger CreateUsageTrigger(string usageCategory, string triggerValue, string callbackUrl);
        //
        // Summary:
        //     Creates a new Usage Trigger resource
        //
        // Parameters:
        //   usageCategory:
        //     The usage category to trigger on
        //
        //   triggerValue:
        //     The the value to trigger on
        //
        //   callbackUrl:
        //     The URL to call once a trigger value has been met
        //
        //   callback:
        void CreateUsageTrigger(string usageCategory, string triggerValue, string callbackUrl, Action<UsageTrigger> callback);
        //
        // Summary:
        //     Delete an Address.
        //
        // Parameters:
        //   addressSid:
        //     The sid of the Address to be deleted.
        //
        // Returns:
        //     A DeleteStatus object indicating whether the request succeeded.
        DeleteStatus DeleteAddress(string addressSid);
        //
        // Summary:
        //     Delete an address from the current account.
        //
        // Parameters:
        //   addressSid:
        //     Sid of the address to delete
        //
        //   callback:
        //     Method to call upon completion; should accept a DeleteStatus object indicating
        //     whether the request succeeded.
        void DeleteAddress(string addressSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Delete this application. If this application's sid is assigned to any IncomingPhoneNumber
        //     resources as a VoiceApplicationSid or SmsApplicationSid it will be removed.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the number to remove
        DeleteStatus DeleteApplication(string applicationSid);
        //
        // Summary:
        //     Delete this application. If this application's sid is assigned to any IncomingPhoneNumber
        //     resources as a VoiceApplicationSid or SmsApplicationSid it will be removed.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the number to remove
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteApplication(string applicationSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes the single Call resource identified by {callSid}.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call resource to delete.
        DeleteStatus DeleteCall(string callSid);
        //
        // Summary:
        //     Deletes a single Call resource identified by {CallSid}.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call resource to delete.
        //
        //   callback:
        //     Method to call upon completion.
        void DeleteCall(string callSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a Credential from a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to delete from
        //
        //   credentialSid:
        //     The Sid of the Credential to delete
        DeleteStatus DeleteCredential(string credentialListSid, string credentialSid);
        //
        // Summary:
        //     Deletes a Credential from a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to delete from
        //
        //   credentialSid:
        //     The Sid of the Credential to delete
        void DeleteCredential(string credentialListSid, string credentialSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a specific CredentialList resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the CredentialList to delete
        DeleteStatus DeleteCredentialList(string credentialListSid);
        //
        // Summary:
        //     Deletes a specific CredentialList resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the CredentialList to delete
        void DeleteCredentialList(string credentialListSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a CredentialListMapping from a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain
        //
        //   credentialListMappingSid:
        //     The Sid of the CredentialListMapping to delete
        DeleteStatus DeleteCredentialListMapping(string domainSid, string credentialListMappingSid);
        //
        // Summary:
        //     Deletes a CredentialListMapping from a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain
        //
        //   credentialListMappingSid:
        //     The Sid of the CredentialListMapping to delete
        void DeleteCredentialListMapping(string domainSid, string credentialListMappingSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to delete
        DeleteStatus DeleteDomain(string domainSid);
        //
        // Summary:
        //     Deletes a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to delete
        void DeleteDomain(string domainSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a Feedback entry for a specific call
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call to delete the Feedback entry from
        DeleteStatus DeleteFeedback(string callSid);
        //
        // Summary:
        //     Deletes a Feedback entry for a specific call
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call to delete the Feedback entry from
        void DeleteFeedback(string callSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a feedback summary.
        //
        // Parameters:
        //   feedbackSummarySid:
        //     Feedback summary sid.
        //
        // Returns:
        //     Deletion status.
        DeleteStatus DeleteFeedbackSummary(string feedbackSummarySid);
        //
        // Summary:
        //     Deletes a feedback summary.
        //
        // Parameters:
        //   feedbackSummarySid:
        //     Feedback summary sid.
        //
        //   callback:
        //     Asynchronous callback.
        void DeleteFeedbackSummary(string feedbackSummarySid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Remove (deprovision) a phone number from the current account
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the number to remove
        DeleteStatus DeleteIncomingPhoneNumber(string incomingPhoneNumberSid);
        //
        // Summary:
        //     Remove (deprovision) a phone number from the current account.
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the number to remove
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteIncomingPhoneNumber(string incomingPhoneNumberSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList Domain to delete
        DeleteStatus DeleteIpAccessControlList(string ipAccessControlListSid);
        //
        // Summary:
        //     Deletes a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList Domain to delete
        void DeleteIpAccessControlList(string ipAccessControlListSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a IpAccessControlListMapping from a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain
        //
        //   ipAccessControlListMappingSid:
        //     The Sid of the IpAccessControlListMapping to delete
        DeleteStatus DeleteIpAccessControlListMapping(string domainSid, string ipAccessControlListMappingSid);
        //
        // Summary:
        //     Deletes a IpAccessControlListMapping from a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain
        //
        //   ipAccessControlListMappingSid:
        //     The Sid of the IpAccessControlListMapping to delete
        void DeleteIpAccessControlListMapping(string domainSid, string ipAccessControlListMappingSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a specific IpAddress resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the IpAddress to delete
        DeleteStatus DeleteIpAddress(string ipAccessControlListSid, string ipAddressSid);
        //
        // Summary:
        //     Deletes a specific IpAddress resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the IpAddress to delete
        void DeleteIpAddress(string ipAccessControlListSid, string ipAddressSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Delete the specified media instance. Makes a DELETE request to a Media Instance
        //     resource.
        //
        // Parameters:
        //   mediaSid:
        //     The Sid of the media to delete
        DeleteStatus DeleteMedia(string messageSid, string mediaSid);
        //
        // Summary:
        //     Delete the specified media instance. Makes a DELETE request to a Media Instance
        //     resource.
        //
        // Parameters:
        //   mediaSid:
        //     The Sid of the media to delete
        void DeleteMedia(string messageSid, string mediaSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes the single Message resource specified by messageSid.
        //
        // Parameters:
        //   messageSid:
        //     The Sid of the message to delete
        DeleteStatus DeleteMessage(string messageSid);
        //
        // Summary:
        //     Deletes a single Message instance
        //
        // Parameters:
        //   messageSid:
        //     The Sid of the Message to delete
        //
        //   callback:
        //     Method to be called on completion
        void DeleteMessage(string messageSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a notification from your account. Makes a DELETE request to a Notification
        //     Instance resource.
        //
        // Parameters:
        //   notificationSid:
        //     The Sid of the notification to delete
        DeleteStatus DeleteNotification(string notificationSid);
        //
        // Summary:
        //     Deletes a notification from your account
        //
        // Parameters:
        //   notificationSid:
        //     The Sid of the notification to delete
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteNotification(string notificationSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Remove a validated outgoing caller ID from the current account.  Makes a
        //     DELETE request to an OutgoingCallerId Instance resource.
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid to remove
        DeleteStatus DeleteOutgoingCallerId(string outgoingCallerIdSid);
        //
        // Summary:
        //     Remove a validated outgoing caller ID from the current account
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid to remove
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteOutgoingCallerId(string outgoingCallerIdSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Deletes a Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to delete
        DeleteStatus DeleteQueue(string queueSid);
        //
        // Summary:
        //     Deletes a Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to delete
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteQueue(string queueSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Delete the specified recording instance. Makes a DELETE request to a Recording
        //     Instance resource.
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to delete
        DeleteStatus DeleteRecording(string recordingSid);
        //
        // Summary:
        //     Delete the specified recording instance
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to delete
        //
        //   callback:
        //     Method to call upon successful completion
        void DeleteRecording(string recordingSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Delete the specified transcription instance. Makes a DELETE request to a
        //     Transcription Instance resource.
        DeleteStatus DeleteTranscription(string transcriptionSid);
        //
        // Summary:
        //     Deletes a UsageTrigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the UsageTrigger to delete
        DeleteStatus DeleteUsageTrigger(string usageTriggerSid);
        //
        // Summary:
        //     Deletes a UsageTrigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the UsageTrigger to delete
        //
        //   callback:
        void DeleteUsageTrigger(string usageTriggerSid, Action<DeleteStatus> callback);
        //
        // Summary:
        //     Dequeues the Caller in the first wait position for the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        DequeueStatus DequeueFirstQueueMember(string queueSid, string url);
        //
        // Summary:
        //     Dequeues the Caller in the first wait position for the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   callback:
        //     Method to call upon successful completion
        void DequeueFirstQueueMember(string queueSid, string url, Action<DequeueStatus> callback);
        //
        // Summary:
        //     Dequeues the Caller in the first wait position for the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   method:
        //     The method to use to request the Url
        DequeueStatus DequeueFirstQueueMember(string queueSid, string url, string method);
        //
        // Summary:
        //     Dequeues the Caller in the first wait position for the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   method:
        //     The method to use to request the Url
        //
        //   callback:
        //     Method to call upon successful completion
        void DequeueFirstQueueMember(string queueSid, string url, string method, Action<DequeueStatus> callback);
        //
        // Summary:
        //     Dequeues a specific Caller in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callSid:
        //     The Sid of the Caller to dequeue
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        DequeueStatus DequeueQueueMember(string queueSid, string callSid, string url);
        //
        // Summary:
        //     Dequeues a specific Caller in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callSid:
        //     The Sid of the Caller to dequeue
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   callback:
        //     Method to call upon successful completion
        void DequeueQueueMember(string queueSid, string callSid, string url, Action<DequeueStatus> callback);
        //
        // Summary:
        //     Dequeues a specific Caller in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callSid:
        //     The Sid of the Caller to dequeue
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   method:
        //     The method to use to request the Url
        DequeueStatus DequeueQueueMember(string queueSid, string callSid, string url, string method);
        //
        // Summary:
        //     Dequeues a specific Caller in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callSid:
        //     The Sid of the Caller to dequeue
        //
        //   url:
        //     A Url containing TwiML intructions to execute when the call is dequeued
        //
        //   method:
        //     The method to use to request the Url
        //
        //   callback:
        //     Method to call upon successful completion
        void DequeueQueueMember(string queueSid, string callSid, string url, string method, Action<DequeueStatus> callback);
        //
        // Summary:
        //     Retrieve the account details for the currently authenticated account. Makes
        //     a GET request to an Account Instance resource.
        Account GetAccount();
        //
        // Summary:
        //     Retrieve the account details for the currently authenticated account
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void GetAccount(Action<Account> callback);
        //
        // Summary:
        //     Retrieve the account details for a subaccount. Makes a GET request to an
        //     Account Instance resource.
        //
        // Parameters:
        //   accountSid:
        //     The Sid of the subaccount to retrieve
        Account GetAccount(string accountSid);
        //
        // Summary:
        //     Retrieve the account details for a subaccount
        //
        // Parameters:
        //   accountSid:
        //     The Sid of the subaccount to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetAccount(string accountSid, Action<Account> callback);
        //
        // Summary:
        //     Retrieve the details for an address instance. Makes a GET request to an Address
        //     Instance resource.
        //
        // Parameters:
        //   addressSid:
        //     The Sid of the address to retrieve
        Address GetAddress(string addressSid);
        //
        // Summary:
        //     Retrieve the details for an address instance. Makes a GET request to an Address
        //     Instance resource.
        //
        // Parameters:
        //   addressSid:
        //     The Sid of the application to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetAddress(string addressSid, Action<Address> callback);
        //
        // Summary:
        //     Retrieve the details for an application instance. Makes a GET request to
        //     an Application Instance resource.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the application to retrieve
        Application GetApplication(string applicationSid);
        //
        // Summary:
        //     Retrieve the details for an application instance. Makes a GET request to
        //     an Application Instance resource.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the application to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetApplication(string applicationSid, Action<Application> callback);
        //
        // Summary:
        //     Retrieve the details for an AuthorizedConnectApp instance. Makes a GET request
        //     to an AuthorizedConnectApp Instance resource.
        //
        // Parameters:
        //   authorizedConnectAppSid:
        //     The Sid of the AuthorizedConnectApp to retrieve
        AuthorizedConnectApp GetAuthorizedConnectApp(string authorizedConnectAppSid);
        //
        // Summary:
        //     Retrieve the details for an AuthorizedConnectApp instance. Makes a GET request
        //     to an AuthorizedConnectApp Instance resource.
        //
        // Parameters:
        //   authorizedConnectAppSid:
        //     The Sid of the AuthorizedConnectApp to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetAuthorizedConnectApp(string authorizedConnectAppSid, Action<AuthorizedConnectApp> callback);
        //
        // Summary:
        //     Returns the single Call resource identified by {CallSid} Makes a GET request
        //     to a Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call resource to retrieve
        Call GetCall(string callSid);
        //
        // Summary:
        //     Returns the single Call resource identified by {CallSid}
        //
        // Parameters:
        //   callSid:
        //     The Sid of the Call resource to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetCall(string callSid, Action<Call> callback);
        //
        // Summary:
        //     Retrieve details for specific conference. Makes a GET request to a Conference
        //     Instance resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference to retrieve
        Conference GetConference(string conferenceSid);
        //
        // Summary:
        //     Retrieve details for specific conference
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetConference(string conferenceSid, Action<Conference> callback);
        //
        // Summary:
        //     Retrieve a single conference participant by their CallSid. Makes a GET request
        //     to a Conference Participant Instance resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call instance
        Participant GetConferenceParticipant(string conferenceSid, string callSid);
        //
        // Summary:
        //     Retrieve a single conference participant by their CallSid
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call instance
        //
        //   callback:
        //     Method to call upon successful completion
        void GetConferenceParticipant(string conferenceSid, string callSid, Action<Participant> callback);
        //
        // Summary:
        //     Retrieve the details for an ConnectApp instance. Makes a GET request to an
        //     ConnectApp Instance resource.
        //
        // Parameters:
        //   connectAppSid:
        //     The Sid of the ConnectApp to retrieve
        ConnectApp GetConnectApp(string connectAppSid);
        //
        // Summary:
        //     Retrieve the details for an ConnectApp instance. Makes a GET request to an
        //     ConnectApp Instance resource.
        //
        // Parameters:
        //   connectAppSid:
        //     The Sid of the ConnectApp to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetConnectApp(string connectAppSid, Action<ConnectApp> callback);
        //
        // Summary:
        //     Locates and returns a specific Credential in a CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   credentialSid:
        //     The Sid of the Credential to locate
        Credential GetCredential(string credentialListSid, string credentialSid);
        //
        // Summary:
        //     Locates and returns a specific Credential in a CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   credentialSid:
        //     The Sid of the Credential to locate
        void GetCredential(string credentialListSid, string credentialSid, Action<Credential> callback);
        //
        // Summary:
        //     Locates and returns a specific CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to locate
        CredentialList GetCredentialList(string credentialListSid);
        //
        // Summary:
        //     Locates and returns a specific CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList to locate
        void GetCredentialList(string credentialListSid, Action<CredentialList> callback);
        //
        // Summary:
        //     Gets a specific CredentialList mapping for a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the mapped SIP Domain
        //
        //   credentialListMappingSid:
        //     The Sid of the mapped CredentialList
        CredentialListMapping GetCredentialListMapping(string domainSid, string credentialListMappingSid);
        //
        // Summary:
        //     Gets a specific CredentialList mapping for a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the mapped SIP Domain
        //
        //   credentialListMappingSid:
        //     The Sid of the mapped CredentialList
        void GetCredentialListMapping(string domainSid, string credentialListMappingSid, Action<CredentialListMapping> callback);
        //
        // Summary:
        //     Locates and returns a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to locate
        Domain GetDomain(string domainSid);
        //
        // Summary:
        //     Locates and returns a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to locate
        void GetDomain(string domainSid, Action<Domain> callback);
        //
        // Summary:
        //     Retrieve the Feedback for a specific CallSid.
        Feedback GetFeedback(string callSid);
        //
        // Summary:
        //     Retrieve the Feedback for a specific CallSid.
        void GetFeedback(string callSid, Action<Feedback> callback);
        //
        // Summary:
        //     Gets a feedback summary.
        //
        // Parameters:
        //   feedbackSummarySid:
        //     Feedback summary sid.
        //
        // Returns:
        //     A feedback summary.
        FeedbackSummary GetFeedbackSummary(string feedbackSummarySid);
        //
        // Summary:
        //     Gets a feedback summary.
        //
        // Parameters:
        //   feedbackSummarySid:
        //     Feedback summary sid.
        //
        //   callback:
        //     Asynchronous callback.
        void GetFeedbackSummary(string feedbackSummarySid, Action<FeedbackSummary> callback);
        //
        // Summary:
        //     Returns the Call in the first position of the wait Queue for the specified
        //     Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        QueueMember GetFirstQueueMember(string queueSid);
        //
        // Summary:
        //     Returns the Call in the first position of the wait Queue for the specified
        //     Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callback:
        //     Method to call upon successful completion
        void GetFirstQueueMember(string queueSid, Action<QueueMember> callback);
        //
        // Summary:
        //     Retrieve the details for an incoming phone number
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the number to retrieve
        IncomingPhoneNumber GetIncomingPhoneNumber(string incomingPhoneNumberSid);
        //
        // Summary:
        //     Retrieve the details for an incoming phone number. Makes a GET request to
        //     a IncomingPhoneNumber instance resource.
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the number to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetIncomingPhoneNumber(string incomingPhoneNumberSid, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Gets a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList resource
        IpAccessControlList GetIpAccessControlList(string ipAccessControlListSid);
        //
        // Summary:
        //     Gets a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList resource
        void GetIpAccessControlList(string ipAccessControlListSid, Action<IpAccessControlList> callback);
        //
        // Summary:
        //     Gets a specific IpAccessControlList mapping for a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the mapped SIP Domain
        //
        //   ipAccessControlListMappingSid:
        //     The Sid of the mapped IpAccessControlList
        IpAccessControlListMapping GetIpAccessControlListMapping(string domainSid, string ipAccessControlListMappingSid);
        //
        // Summary:
        //     Gets a specific IpAccessControlList mapping for a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the mapped SIP Domain
        //
        //   ipAccessControlListMappingSid:
        //     The Sid of the mapped IpAccessControlList
        void GetIpAccessControlListMapping(string domainSid, string ipAccessControlListMappingSid, Action<IpAccessControlListMapping> callback);
        //
        // Summary:
        //     Locates and returns a specific IpAddress resource located in an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   ipAddressSid:
        //     The Sid of the IpAddress to locate
        IpAddress GetIpAddress(string ipAccessControlListSid, string ipAddressSid);
        //
        // Summary:
        //     Locates and returns a specific IpAddress resource located in an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   ipAddressSid:
        //     The Sid of the IpAddress to locate
        void GetIpAddress(string ipAccessControlListSid, string ipAddressSid, Action<IpAddress> callback);
        //
        // Summary:
        //     Get the details for a specific Media instance.
        void GetMedia(string mediaSid, Action<Media> callback);
        //
        // Summary:
        //     Get the details for a specific Media instance.
        //
        // Parameters:
        //   mediaSid:
        //     The Sid of the media resource
        Media GetMedia(string messageSid, string mediaSid);
        //
        // Summary:
        //     Retrieve the details for a specific Message instance.  Makes a GET request
        //     to an Message Instance resource.
        //
        // Parameters:
        //   messageSid:
        //     The Sid of the message to retrieve
        Message GetMessage(string messageSid);
        //
        // Summary:
        //     Retrieve the details for a specific Message instance.  Makes a GET request
        //     to an Message Instance resource.
        //
        // Parameters:
        //   messageSid:
        //     The Sid of the message to retrieve
        void GetMessage(string messageSid, Action<Message> callback);
        //
        // Summary:
        //     Retrieve the details of a specific notification. Makes a GET request to a
        //     Notification Instance resource.
        //
        // Parameters:
        //   notificationSid:
        //     The Sid of the notification to retrieve
        Notification GetNotification(string notificationSid);
        //
        // Summary:
        //     Retrieve the details of a specific notification
        //
        // Parameters:
        //   notificationSid:
        //     The Sid of the notification to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetNotification(string notificationSid, Action<Notification> callback);
        //
        // Summary:
        //     Retrieve the details for an existing validated Outgoing Caller ID entry.
        //      Makes a GET request to an OutgoingCallerId Instance resource.
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid of the entry to retrieve
        OutgoingCallerId GetOutgoingCallerId(string outgoingCallerIdSid);
        //
        // Summary:
        //     Retrieve the details for an existing validated Outgoing Caller ID entry
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid of the entry to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetOutgoingCallerId(string outgoingCallerIdSid, Action<OutgoingCallerId> callback);
        //
        // Summary:
        //     Locates and returns a specific Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        Queue GetQueue(string queueSid);
        //
        // Summary:
        //     Locates and returns a specific Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callback:
        //     Method to call upon successful completion
        void GetQueue(string queueSid, Action<Queue> callback);
        //
        // Summary:
        //     Locate and return a specific Call in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to search
        //
        //   callSid:
        //     The Sid of the Call to locate
        QueueMember GetQueueMember(string queueSid, string callSid);
        //
        // Summary:
        //     Locate and return a specific Call in the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to search
        //
        //   callSid:
        //     The Sid of the Call to locate
        //
        //   callback:
        //     Method to call upon successful completion
        void GetQueueMember(string queueSid, string callSid, Action<QueueMember> callback);
        //
        // Summary:
        //     Retrieve the details for the specified recording instance.  Makes a GET request
        //     to a Recording Instance resource.
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retrieve
        Recording GetRecording(string recordingSid);
        //
        // Summary:
        //     Retrieve the details for the specified recording instance
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetRecording(string recordingSid, Action<Recording> callback);
        //
        // Summary:
        //     Retrieves the transcription text for the specified recording, if it was transcribed.
        //     Makes a GET request to a Recording Instance resource.
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retreive the transcription for
        string GetRecordingText(string recordingSid);
        //
        // Summary:
        //     Retrieves the transcription text for the specified recording, if it was transcribed
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retreive the transcription for
        //
        //   callback:
        //     Method to call upon successful completion
        void GetRecordingText(string recordingSid, Action<string> callback);
        //
        // Summary:
        //     Returns the Sandbox resource associated with the account identified by {YourAccountSid}.
        //     Twilio accounts upgraded prior to February 2010 may not have a Sandbox resource,
        //     and in this case you will receive a 404 (Not Found) response.  Makes a GET
        //     request to the Sandbox Instance resource.
        Sandbox GetSandbox();
        //
        // Summary:
        //     Returns the Sandbox resource associated with the account identified by {YourAccountSid}.
        //     Twilio accounts upgraded prior to February 2010 may not have a Sandbox resource,
        //     and in this case you will receive a 404 (Not Found) response.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void GetSandbox(Action<Sandbox> callback);
        //
        // Summary:
        //     Retrieve the details for a specific ShortCode instance.  Makes a GET request
        //     to a ShortCode Instance resource.
        //
        // Parameters:
        //   shortCodeSid:
        //     The Sid of the ShortCode resource to return
        SMSShortCode GetShortCode(string shortCodeSid);
        //
        // Summary:
        //     Retrieve the details for a specific ShortCode instance.  Makes a GET request
        //     to a ShortCode Instance resource.
        //
        // Parameters:
        //   shortCodeSid:
        //     The Sid of the ShortCode resource to return
        //
        //   callback:
        //     Method to call upon successful completion
        void GetShortCode(string shortCodeSid, Action<SMSShortCode> callback);
        //
        // Summary:
        //     Retrieve the details for a specific SMS message instance.  Makes a GET request
        //     to an SMSMessage Instance resource.
        //
        // Parameters:
        //   smsMessageSid:
        //     The Sid of the message to retrieve
        SMSMessage GetSmsMessage(string smsMessageSid);
        //
        // Summary:
        //     Retrieve the details for a specific SMS message instance
        //
        // Parameters:
        //   smsMessageSid:
        //     The Sid of the message to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void GetSmsMessage(string smsMessageSid, Action<SMSMessage> callback);
        //
        // Summary:
        //     Retrieve the details of a single transcription. Makes a GET request to a
        //     Transcription Instance resource.
        //
        // Parameters:
        //   transcriptionSid:
        //     The Sid of the transcription to retrieve
        Transcription GetTranscription(string transcriptionSid);
        //
        // Summary:
        //     Retrieve the details of a single transcription
        //
        // Parameters:
        //   transcriptionSid:
        //     The Sid of the transcription to retrieve
        //
        //   callback:
        //     Method to call upon completion of request
        void GetTranscription(string transcriptionSid, Action<Transcription> callback);
        //
        // Summary:
        //     Retrieve the text of a single transcription.  Makes a GET request to a Transcription
        //     Instance resource.
        //
        // Parameters:
        //   transcriptionSid:
        //     The Sid of the transcription to retrieve
        string GetTranscriptionText(string transcriptionSid);
        //
        // Summary:
        //     Retrieve the text of a single transcription
        //
        // Parameters:
        //   transcriptionSid:
        //     The Sid of the transcription to retrieve
        //
        //   callback:
        //     Method to call upon completion of the request
        void GetTranscriptionText(string transcriptionSid, Action<string> callback);
        //
        // Summary:
        //     Locates and returns a specific Usage Trigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the Usage Trigger to locate
        UsageTrigger GetUsageTrigger(string usageTriggerSid);
        //
        // Summary:
        //     Locates and returns a specific Usage Trigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the Usage Trigger to locate
        //
        //   callback:
        void GetUsageTrigger(string usageTriggerSid, Action<UsageTrigger> callback);
        //
        // Summary:
        //     Hangs up a call in progress. Makes a POST request to a Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to hang up.
        //
        //   style:
        //     'Canceled' will attempt to hangup calls that are queued or ringing but not
        //     affect calls already in progress. 'Completed' will attempt to hang up a call
        //     even if it's already in progress.
        Call HangupCall(string callSid, HangupStyle style);
        //
        // Summary:
        //     Hangs up a call in progress.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to hang up.
        //
        //   style:
        //     'Canceled' will attempt to hangup calls that are queued or ringing but not
        //     affect calls already in progress. 'Completed' will attempt to hang up a call
        //     even if it's already in progress.
        //
        //   callback:
        //     Method to call upon successful completion
        void HangupCall(string callSid, HangupStyle style, Action<Call> callback);
        //
        // Summary:
        //     Initiates a new phone call. Makes a POST request to the Calls List resource.
        //
        // Parameters:
        //   options:
        //     Call settings. Only properties with values set will be used.
        Call InitiateOutboundCall(CallOptions options);
        //
        // Summary:
        //     Initiates a new phone call.
        //
        // Parameters:
        //   options:
        //     Call settings. Only properties with values set will be used.
        //
        //   callback:
        //     Method to call upon successful completion
        void InitiateOutboundCall(CallOptions options, Action<Call> callback);
        //
        // Summary:
        //     Initiates a new phone call. Makes a POST request to the Calls List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to use as the caller id. Format with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Must be a Twilio number or a valid outgoing
        //     caller id for your account.
        //
        //   to:
        //     The number to call formatted with a '+' and country code e.g., +16175551212
        //     (E.164 format). Twilio will also accept unformatted US numbers e.g., (415)
        //     555-1212, 415-555-1212.
        //
        //   url:
        //     The fully qualified URL that should be consulted when the call connects.
        //     Just like when you set a URL for your inbound calls. URL should return TwiML.
        Call InitiateOutboundCall(string from, string to, string url);
        //
        // Summary:
        //     Initiates a new phone call.
        //
        // Parameters:
        //   from:
        //     The phone number to use as the caller id. Format with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Must be a Twilio number or a valid outgoing
        //     caller id for your account.
        //
        //   to:
        //     The number to call formatted with a '+' and country code e.g., +16175551212
        //     (E.164 format). Twilio will also accept unformatted US numbers e.g., (415)
        //     555-1212, 415-555-1212.
        //
        //   url:
        //     The fully qualified URL that should be consulted when the call connects.
        //     Just like when you set a URL for your inbound calls. URL should return TwiML.
        //
        //   callback:
        //     Method to call upon successful completion
        void InitiateOutboundCall(string from, string to, string url, Action<Call> callback);
        //
        // Summary:
        //     Initiates a new phone call. Makes a POST request to the Calls List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to use as the caller id. Format with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Must be a Twilio number or a valid outgoing
        //     caller id for your account.
        //
        //   to:
        //     The number to call formatted with a '+' and country code e.g., +16175551212
        //     (E.164 format). Twilio will also accept unformatted US numbers e.g., (415)
        //     555-1212, 415-555-1212.
        //
        //   url:
        //     The fully qualified URL that should be consulted when the call connects.
        //     Just like when you set a URL for your inbound calls. URL should return TwiML.
        //
        //   statusCallback:
        //     A URL that Twilio will request when the call ends to notify your app.
        Call InitiateOutboundCall(string from, string to, string url, string statusCallback);
        //
        // Summary:
        //     Initiates a new phone call.
        //
        // Parameters:
        //   from:
        //     The phone number to use as the caller id. Format with a '+' and country code
        //     e.g., +16175551212 (E.164 format). Must be a Twilio number or a valid outgoing
        //     caller id for your account.
        //
        //   to:
        //     The number to call formatted with a '+' and country code e.g., +16175551212
        //     (E.164 format). Twilio will also accept unformatted US numbers e.g., (415)
        //     555-1212, 415-555-1212.
        //
        //   url:
        //     The fully qualified URL that should be consulted when the call connects.
        //     Just like when you set a URL for your inbound calls. URL should return TwiML.
        //
        //   statusCallback:
        //     A URL that Twilio will request when the call ends to notify your app.
        //
        //   callback:
        //     Method to call upon successful completion
        void InitiateOutboundCall(string from, string to, string url, string statusCallback, Action<Call> callback);
        //
        // Summary:
        //     Remove a caller from a conference. Makes a DELETE request to a Conference
        //     Participant Instance resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to remove
        bool KickConferenceParticipant(string conferenceSid, string callSid);
        //
        // Summary:
        //     Remove a caller from a conference
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to remove
        //
        //   callback:
        //     Method to call upon successful completion
        void KickConferenceParticipant(string conferenceSid, string callSid, Action<bool> callback);
        //
        // Summary:
        //     List Addresses on the current account.
        AddressResult ListAddresses();
        //
        // Summary:
        //     List addresses on the current account.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListAddresses(Action<AddressResult> callback);
        //
        // Summary:
        //     List Addresses on the current account, with filters.
        //
        // Parameters:
        //   options:
        //     Filters to be applied to the request.
        AddressResult ListAddresses(AddressListRequest options);
        //
        // Summary:
        //     List addresses on the current account, with filters.
        //
        // Parameters:
        //   options:
        //     Filters to pass into the list request
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAddresses(AddressListRequest options, Action<AddressResult> callback);
        //
        // Summary:
        //     List applications on current account
        ApplicationResult ListApplications();
        //
        // Summary:
        //     List applications on current account
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListApplications(Action<ApplicationResult> callback);
        //
        // Summary:
        //     List applications on current account with filters
        //
        // Parameters:
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        ApplicationResult ListApplications(string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     List applications on current account with filters
        //
        // Parameters:
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        //
        //   callback:
        //     Method to call upon successful completion
        void ListApplications(string friendlyName, int? pageNumber, int? count, Action<ApplicationResult> callback);
        //
        // Summary:
        //     List AuthorizedConnectApps on current account
        AuthorizedConnectAppResult ListAuthorizedConnectApps();
        //
        // Summary:
        //     List AuthorizedConnectApps on current account
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListAuthorizedConnectApps(Action<AuthorizedConnectAppResult> callback);
        //
        // Summary:
        //     List AuthorizedConnectApps on current account with filters
        //
        // Parameters:
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        AuthorizedConnectAppResult ListAuthorizedConnectApps(int? pageNumber, int? count);
        //
        // Summary:
        //     List AuthorizedConnectApps on current account with filters
        //
        // Parameters:
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAuthorizedConnectApps(int? pageNumber, int? count, Action<AuthorizedConnectAppResult> callback);
        //
        // Summary:
        //     Search available local phone numbers. Makes a GET request to the AvailablePhoneNumber
        //     List resource.
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   options:
        //     Search filter options. Only properties with values set will be used.
        AvailablePhoneNumberResult ListAvailableLocalPhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options);
        //
        // Summary:
        //     Search available local phone numbers
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   options:
        //     Search filter options. Only properties with values set will be used.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAvailableLocalPhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options, Action<AvailablePhoneNumberResult> callback);
        //
        // Summary:
        //     Search available mobile phone numbers. Makes a GET request to the AvailablePhoneNumber
        //     List resource.
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   options:
        //     Search filter options. Only properties with values set will be used.
        AvailablePhoneNumberResult ListAvailableMobilePhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options);
        //
        // Summary:
        //     Search available mobile phone numbers. Makes a GET request to the AvailablePhoneNumber
        //     List resource.
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   options:
        //     Search filter options. Only properties with values set will be used.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAvailableMobilePhoneNumbers(string isoCountryCode, AvailablePhoneNumberListRequest options, Action<AvailablePhoneNumberResult> callback);
        //
        // Summary:
        //     Search available toll-free phone numbers. Makes a GET request to the AvailablePhoneNumber
        //     List resource.
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        AvailablePhoneNumberResult ListAvailableTollFreePhoneNumbers(string isoCountryCode);
        //
        // Summary:
        //     Search available toll-free phone numbers
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAvailableTollFreePhoneNumbers(string isoCountryCode, Action<AvailablePhoneNumberResult> callback);
        //
        // Summary:
        //     Search available toll-free phone numbers. Makes a GET request to the AvailablePhoneNumber
        //     List resource.
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   contains:
        //     Value to use when filtering search. Accepts numbers or characters.
        AvailablePhoneNumberResult ListAvailableTollFreePhoneNumbers(string isoCountryCode, string contains);
        //
        // Summary:
        //     Search available toll-free phone numbers
        //
        // Parameters:
        //   isoCountryCode:
        //     Two-character ISO country code (US or CA)
        //
        //   contains:
        //     Value to use when filtering search. Accepts numbers or characters.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListAvailableTollFreePhoneNumbers(string isoCountryCode, string contains, Action<AvailablePhoneNumberResult> callback);
        //
        // Summary:
        //     Returns a paged list of phone calls made to and from the account.  Makes
        //     a GET request to the Calls List resource.
        CallResult ListCalls();
        //
        // Summary:
        //     Returns a paged list of phone calls made to and from the account.  Sorted
        //     by DateUpdated with most-recent calls first.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListCalls(Action<CallResult> callback);
        //
        // Summary:
        //     Returns a paged list of phone calls made to and from the account.  Makes
        //     a GET request to the Calls List resource.
        //
        // Parameters:
        //   options:
        //     List filter options. If an property is set the list will be filtered by that
        //     value.
        CallResult ListCalls(CallListRequest options);
        //
        // Summary:
        //     Returns a paged list of phone calls made to and from the account.  Sorted
        //     by DateUpdated with most-recent calls first.
        //
        // Parameters:
        //   options:
        //     List filter options. If an property is set the list will be filtered by that
        //     value.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListCalls(CallListRequest options, Action<CallResult> callback);
        //
        // Summary:
        //     Retrieve a list of conference participants. Makes a GET request to a Conference
        //     Participants List resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   muted:
        //     Set to null to retrieve all, true to retrieve muted, false to retrieve unmuted
        ParticipantResult ListConferenceParticipants(string conferenceSid, bool? muted);
        //
        // Summary:
        //     Retrieve a list of conference participants
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   muted:
        //     Set to null to retrieve all, true to retrieve muted, false to retrieve unmuted
        //
        //   callback:
        //     Method to call upon successful completion
        void ListConferenceParticipants(string conferenceSid, bool? muted, Action<ParticipantResult> callback);
        //
        // Summary:
        //     Retrieve a list of conference participants. Makes a GET request to a Conference
        //     Participants List resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   muted:
        //     Set to null to retrieve all, true to retrieve muted, false to retrieve unmuted
        //
        //   pageNumber:
        //     Which page number to start retrieving from
        //
        //   count:
        //     How many participants to retrieve
        ParticipantResult ListConferenceParticipants(string conferenceSid, bool? muted, int? pageNumber, int? count);
        //
        // Summary:
        //     Retrieve a list of conference participants
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   muted:
        //     Set to null to retrieve all, true to retrieve muted, false to retrieve unmuted
        //
        //   pageNumber:
        //     Which page number to start retrieving from
        //
        //   count:
        //     How many participants to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void ListConferenceParticipants(string conferenceSid, bool? muted, int? pageNumber, int? count, Action<ParticipantResult> callback);
        //
        // Summary:
        //     Returns a list of conferences within an account. The list includes paging
        //     information.  Makes a GET request to the Conferences List resource.
        ConferenceResult ListConferences();
        //
        // Summary:
        //     Returns a list of conferences within an account. The list includes paging
        //     information and is sorted by DateUpdated, with most recent conferences first.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListConferences(Action<ConferenceResult> callback);
        //
        // Summary:
        //     Returns a list of conferences within an account. The list includes paging
        //     information.  Makes a POST request to the Conferences List resource.
        //
        // Parameters:
        //   options:
        //     List filter options. Only properties with values are included in request.
        ConferenceResult ListConferences(ConferenceListRequest options);
        //
        // Summary:
        //     Returns a list of conferences within an account. The list includes paging
        //     information and is sorted by DateUpdated, with most recent conferences first.
        //
        // Parameters:
        //   options:
        //     List filter options. Only properties with values are included in request.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListConferences(ConferenceListRequest options, Action<ConferenceResult> callback);
        //
        // Summary:
        //     List ConnectApps on current account
        ConnectAppResult ListConnectApps();
        //
        // Summary:
        //     List ConnectApps on current account
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListConnectApps(Action<ConnectAppResult> callback);
        //
        // Summary:
        //     List ConnectApps on current account with filters
        //
        // Parameters:
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        ConnectAppResult ListConnectApps(int? pageNumber, int? count);
        //
        // Summary:
        //     List ConnectApps on current account with filters
        //
        // Parameters:
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        //
        //   callback:
        //     Method to call upon successful completion
        void ListConnectApps(int? pageNumber, int? count, Action<ConnectAppResult> callback);
        //
        // Summary:
        //     Lists all CredentialLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        CredentialListMappingResult ListCredentialListMappings(string domainSid);
        //
        // Summary:
        //     Lists all CredentialLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        void ListCredentialListMappings(string domainSid, Action<CredentialListMappingResult> callback);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        //
        //   pageNumber:
        //
        //   count:
        CredentialListMappingResult ListCredentialListMappings(string domainSid, int? pageNumber, int? count);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        //
        //   pageNumber:
        //
        //   count:
        void ListCredentialListMappings(string domainSid, int? pageNumber, int? count, Action<CredentialListMappingResult> callback);
        //
        // Summary:
        //     Return a list all CredentialsList resources
        CredentialListResult ListCredentialLists();
        //
        // Summary:
        //     Return a list all CredentialsList resources
        void ListCredentialLists(Action<CredentialListResult> callback);
        //
        // Summary:
        //     Return a list all CredentialsList resources
        //
        // Parameters:
        //   pageNumber:
        //
        //   count:
        CredentialListResult ListCredentialLists(int? pageNumber, int? count);
        //
        // Summary:
        //     Return a list all CredentialsList resources
        //
        // Parameters:
        //   pageNumber:
        //
        //   count:
        void ListCredentialLists(int? pageNumber, int? count, Action<CredentialListResult> callback);
        //
        // Summary:
        //     List all Credentials for a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        CredentialResult ListCredentials(string credentialListSid);
        //
        // Summary:
        //     List all Credentials for a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        void ListCredentials(string credentialListSid, Action<CredentialResult> callback);
        //
        // Summary:
        //     List all Credentials for a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   pageNumber:
        //
        //   count:
        CredentialResult ListCredentials(string credentialListSid, int? pageNumber, int? count);
        //
        // Summary:
        //     List all Credentials for a CredentialList
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   pageNumber:
        //
        //   count:
        void ListCredentials(string credentialListSid, int? pageNumber, int? count, Action<CredentialResult> callback);
        //
        // Summary:
        //     List the current account's incoming phone numbers that depend on a specific
        //     address.
        //
        // Parameters:
        //   addressSid:
        //     Sid of the address to retrieve dependent phone numbers for.
        DependentPhoneNumberResult ListDependentPhoneNumbers(string addressSid);
        //
        // Summary:
        //     List incoming phone numbers on the current account that depend on the specified
        //     Address.
        //
        // Parameters:
        //   addressSid:
        //     Sid of the address to retrieve numbers for
        //
        //   callback:
        //     Method to call upon successful completion
        void ListDependentPhoneNumbers(string addressSid, Action<DependentPhoneNumberResult> callback);
        //
        // Summary:
        //     Return a list of all SIP Domain resources
        DomainResult ListDomains();
        //
        // Summary:
        //     Return a list of all SIP Domain resources
        void ListDomains(Action<DomainResult> callback);
        //
        // Summary:
        //     Return a list of all SIP Domain resources
        //
        // Parameters:
        //   pageNumber:
        //
        //   count:
        DomainResult ListDomains(int? pageNumber, int? count);
        //
        //
        // Parameters:
        //   pageNumber:
        //
        //   count:
        void ListDomains(int? pageNumber, int? count, Action<DomainResult> callback);
        //
        // Summary:
        //     List all incoming local phone numbers on current account
        IncomingPhoneNumberResult ListIncomingLocalPhoneNumbers();
        //
        // Summary:
        //     List all incoming local phone numbers on current account
        void ListIncomingLocalPhoneNumbers(Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List incoming local phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        IncomingPhoneNumberResult ListIncomingLocalPhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     List incoming local phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        void ListIncomingLocalPhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count, Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List all incoming mobile phone numbers on current account
        IncomingPhoneNumberResult ListIncomingMobilePhoneNumbers();
        //
        // Summary:
        //     List all incoming mobile phone numbers on current account
        void ListIncomingMobilePhoneNumbers(Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List incoming mobile phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        IncomingPhoneNumberResult ListIncomingMobilePhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     List incoming mobile phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        void ListIncomingMobilePhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count, Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List all incoming phone numbers on current account
        IncomingPhoneNumberResult ListIncomingPhoneNumbers();
        //
        // Summary:
        //     List all incoming phone numbers on current account. Makes a GET request to
        //     the IncomingPhoneNumber List resource.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListIncomingPhoneNumbers(Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List incoming phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        IncomingPhoneNumberResult ListIncomingPhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     List incoming phone numbers on current account with filters. Makes a GET
        //     request to the IncomingPhoneNumber List resource.
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        //
        //   callback:
        //     Method to call upon successful completion
        void ListIncomingPhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count, Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List all incoming toll free phone numbers on current account
        IncomingPhoneNumberResult ListIncomingTollFreePhoneNumbers();
        //
        // Summary:
        //     List all incoming toll free phone numbers on current account
        void ListIncomingTollFreePhoneNumbers(Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     List incoming toll free phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        IncomingPhoneNumberResult ListIncomingTollFreePhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     List incoming toll free phone numbers on current account with filters
        //
        // Parameters:
        //   phoneNumber:
        //     Optional phone number to match
        //
        //   friendlyName:
        //     Optional friendly name to match
        //
        //   pageNumber:
        //     Page number to start retrieving results from
        //
        //   count:
        //     How many results to return
        void ListIncomingTollFreePhoneNumbers(string phoneNumber, string friendlyName, int? pageNumber, int? count, Action<IncomingPhoneNumberResult> callback);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        IpAccessControlListMappingResult ListIpAccessControlListMappings(string domainSid);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        void ListIpAccessControlListMappings(string domainSid, Action<IpAccessControlListMappingResult> callback);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        //
        //   pageNumber:
        //
        //   count:
        IpAccessControlListMappingResult ListIpAccessControlListMappings(string domainSid, int? pageNumber, int? count);
        //
        // Summary:
        //     Lists all IpAccessControlLists mapped to a SIP Domain
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to list mappings for
        //
        //   pageNumber:
        //
        //   count:
        void ListIpAccessControlListMappings(string domainSid, int? pageNumber, int? count, Action<IpAccessControlListMappingResult> callback);
        //
        // Summary:
        //     Lists all IpAccessControlLists for this account
        IpAccessControlListResult ListIpAccessControlLists();
        //
        // Summary:
        //     Lists all IpAccessControlLists for this account
        void ListIpAccessControlLists(Action<IpAccessControlListResult> callback);
        //
        // Summary:
        //     Lists all IpAccessControlLists for this account
        //
        // Parameters:
        //   pageNumber:
        //
        //   count:
        IpAccessControlListResult ListIpAccessControlLists(int? pageNumber, int? count);
        //
        //
        // Parameters:
        //   count:
        void ListIpAccessControlLists(int? pageNumber, int? count, Action<IpAccessControlListResult> callback);
        //
        // Summary:
        //     Return a lists all IpAddresses associated with an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        IpAddressResult ListIpAddresses(string ipAccessControlListSid);
        //
        // Summary:
        //     Return a lists all IpAddresses associated with an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        void ListIpAddresses(string ipAccessControlListSid, Action<IpAddressResult> callback);
        //
        // Summary:
        //     Return a lists all IpAddresses associated with an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   pageNumber:
        //
        //   count:
        IpAddressResult ListIpAddresses(string ipAccessControlListSid, int? pageNumber, int? count);
        //
        // Summary:
        //     Return a lists all IpAddresses associated with an IpAccessControlList
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   pageNumber:
        //
        //   count:
        void ListIpAddresses(string ipAccessControlListSid, int? pageNumber, int? count, Action<IpAddressResult> callback);
        //
        // Summary:
        //     Retrieve a list of Media objects with no list filters
        MediaResult ListMedia(string messageSid);
        //
        // Summary:
        //     Retrieve a list of Media objects with no list filters
        void ListMedia(string messageSid, Action<MediaResult> callback);
        //
        // Summary:
        //     Return a filtered list of Media objects. The list includes paging information.
        MediaResult ListMedia(string messageSid, MediaListRequest options);
        //
        // Summary:
        //     Return a filtered list of Media objects. The list includes paging information.
        void ListMedia(string messageSid, MediaListRequest options, Action<MediaResult> callback);
        //
        // Summary:
        //     List all media for a particular message
        //
        // Parameters:
        //   messageSid:
        //     The message sid to filter on
        MediaResult ListMessageMedia(string messageSid, MediaListRequest options);
        //
        // Summary:
        //     List all media for a particular message
        //
        // Parameters:
        //   messageSid:
        //     The message sid to filter on
        void ListMessageMedia(string messageSid, MediaListRequest options, Action<MediaResult> callback);
        //
        // Summary:
        //     Returns a list of Messages.  The list includes paging information.  Makes
        //     a GET request to the Message List resource.
        MessageResult ListMessages();
        //
        // Summary:
        //     Returns a list of Messages. The list includes paging information.  Makes
        //     a GET request to the Message List resource.
        void ListMessages(Action<MessageResult> callback);
        //
        // Summary:
        //     Returns a filtered list of Messages. The list includes paging information.
        //      Makes a GET request to the Messages List resource.
        //
        // Parameters:
        //   options:
        //     The list filters for the request
        MessageResult ListMessages(MessageListRequest options);
        //
        // Summary:
        //     Returns a filtered list of Messages. The list includes paging information.
        //      Makes a GET request to the Messages List resource.
        //
        // Parameters:
        //   options:
        //     The list filters for the request
        void ListMessages(MessageListRequest options, Action<MessageResult> callback);
        //
        // Summary:
        //     Returns a list of notifications generated for an account. The list includes
        //     paging information.  Makes a GET request to a Notifications List resource.
        NotificationResult ListNotifications();
        //
        // Summary:
        //     Returns a list of notifications generated for an account. The list includes
        //     paging information and is sorted by DateUpdated, with most recent notifications
        //     first.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListNotifications(Action<NotificationResult> callback);
        //
        // Summary:
        //     Returns a filtered list of notifications generated for an account. The list
        //     includes paging information.  Makes a GET request to a Notifications List
        //     resource.
        //
        // Parameters:
        //   log:
        //     Only show notifications for this log, using the integer log values: 0 is
        //     ERROR, 1 is WARNING
        //
        //   messageDate:
        //     Only show notifications for this date (in GMT)
        //
        //   pageNumber:
        //     The page number to start retrieving results from
        //
        //   count:
        //     How many notifications to return
        NotificationResult ListNotifications(int? log, DateTime? messageDate, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a filtered list of notifications generated for an account. The list
        //     includes paging information and is sorted by DateUpdated, with most recent
        //     notifications first.
        //
        // Parameters:
        //   log:
        //     Only show notifications for this log, using the integer log values: 0 is
        //     ERROR, 1 is WARNING
        //
        //   messageDate:
        //     Only show notifications for this date (in GMT)
        //
        //   pageNumber:
        //     The page number to start retrieving results from
        //
        //   count:
        //     How many notifications to return
        //
        //   callback:
        //     Method to call upon successful completion
        void ListNotifications(int? log, DateTime? messageDate, int? pageNumber, int? count, Action<NotificationResult> callback);
        //
        // Summary:
        //     Returns a list of validated outgoing caller IDs. The list includes paging
        //     information.  Makes a GET request to an OutgoingCallerIds List resource.
        OutgoingCallerIdResult ListOutgoingCallerIds();
        //
        // Summary:
        //     Returns a list of validated outgoing caller IDs. The list includes paging
        //     information.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListOutgoingCallerIds(Action<OutgoingCallerIdResult> callback);
        //
        // Summary:
        //     Returns a filtered list of validated outgoing caller IDs. The list includes
        //     paging information.  Makes a GET request to an OutgoingCallerIds List resource.
        //
        // Parameters:
        //   phoneNumber:
        //     If present, filter the list by the value provided
        //
        //   friendlyName:
        //     If present, filter the list by the value provided
        //
        //   pageNumber:
        //     If present, start the results from the specified page
        //
        //   count:
        //     If present, return the specified number of results, up to 1000
        OutgoingCallerIdResult ListOutgoingCallerIds(string phoneNumber, string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a filtered list of validated outgoing caller IDs. The list includes
        //     paging information.
        //
        // Parameters:
        //   phoneNumber:
        //     If present, filter the list by the value provided
        //
        //   friendlyName:
        //     If present, filter the list by the value provided
        //
        //   pageNumber:
        //     If present, start the results from the specified page
        //
        //   count:
        //     If present, return the specified number of results, up to 1000
        //
        //   callback:
        //     Method to call upon successful completion
        void ListOutgoingCallerIds(string phoneNumber, string friendlyName, int? pageNumber, int? count, Action<OutgoingCallerIdResult> callback);
        //
        // Summary:
        //     Return a List of all Calls currently in the the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        QueueMemberResult ListQueueMembers(string queueSid);
        //
        // Summary:
        //     Return a List of all Calls currently in the the specified Queue
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to locate
        //
        //   callback:
        //     Method to call upon successful completion
        void ListQueueMembers(string queueSid, Action<QueueMemberResult> callback);
        //
        // Summary:
        //     Return a list of all Queue resources
        QueueResult ListQueues();
        //
        // Summary:
        //     Return a list of all Queue resources
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListQueues(Action<QueueResult> callback);
        //
        // Summary:
        //     Returns a list of Recordings, each representing a recording generated during
        //     the course of a phone call. The list includes paging information.  Makes
        //     a GET request to the Recordings List resource.
        RecordingResult ListRecordings();
        //
        // Summary:
        //     Returns a list of Recordings, each representing a recording generated during
        //     the course of a phone call. The list includes paging information.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListRecordings(Action<RecordingResult> callback);
        //
        // Summary:
        //     Returns a filtered list of Recordings, each representing a recording generated
        //     during the course of a phone call. The list includes paging information.
        //      Makes a GET request to the Recordings List resource.
        //
        // Parameters:
        //   callSid:
        //     (Optional) The CallSid to retrieve recordings for
        //
        //   dateCreated:
        //     (Optional) The date the recording was created (GMT)
        //
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     How many results to retrieve
        RecordingResult ListRecordings(string callSid, DateTime? dateCreated, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a filtered list of Recordings, each representing a recording generated
        //     during the course of a phone call. The list includes paging information.
        //
        // Parameters:
        //   callSid:
        //     (Optional) The CallSid to retrieve recordings for
        //
        //   dateCreated:
        //     (Optional) The date the recording was created (GMT)
        //
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     How many results to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void ListRecordings(string callSid, DateTime? dateCreated, int? pageNumber, int? count, Action<RecordingResult> callback);
        //
        // Summary:
        //     Returns a list of ShortCode resource representations, each representing a
        //     short code within your account.
        //
        // Parameters:
        //   shortCode:
        //     Only show the ShortCode resources that match this pattern. You can specify
        //     partial numbers and use '*' as a wildcard for any digit.
        //
        //   friendlyName:
        //     Only show the ShortCode resources with friendly names that exactly match
        //     this name.
        SmsShortCodeResult ListShortCodes(string shortCode, string friendlyName);
        //
        // Summary:
        //     Returns a list of ShortCode resource representations, each representing a
        //     short code within your account.
        //
        // Parameters:
        //   shortCode:
        //     Only show the ShortCode resources that match this pattern. You can specify
        //     partial numbers and use '*' as a wildcard for any digit.
        //
        //   friendlyName:
        //     Only show the ShortCode resources with friendly names that exactly match
        //     this name.
        //
        //   callback:
        //     Method to call upon successful completion
        void ListShortCodes(string shortCode, string friendlyName, Action<SmsShortCodeResult> callback);
        //
        // Summary:
        //     Returns a list of SMS messages. The list includes paging information.  Makes
        //     a GET request to the SMSMessage List resource.
        SmsMessageResult ListSmsMessages();
        //
        // Summary:
        //     Returns a list of SMS messages. The list includes paging information and
        //     is sorted by DateSent, with most recent messages first.
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListSmsMessages(Action<SmsMessageResult> callback);
        //
        // Summary:
        //     Returns a filtered list of SMS messages. The list includes paging information.
        //      Makes a GET request to the SMSMessages List resource.
        //
        // Parameters:
        //   to:
        //     (Optional) The phone number of the message recipient
        //
        //   from:
        //     (Optional) The phone number of the message sender
        //
        //   dateSent:
        //     (Optional) The date the message was sent (GMT)
        //
        //   pageNumber:
        //     (Optional) The page to start retrieving results from
        //
        //   count:
        //     (Optional) The number of results to retrieve
        SmsMessageResult ListSmsMessages(string to, string from, DateTime? dateSent, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a filtered list of SMS messages. The list includes paging information
        //     and is sorted by DateSent, with most recent messages first.
        //
        // Parameters:
        //   to:
        //     (Optional) The phone number of the message recipient
        //
        //   from:
        //     (Optional) The phone number of the message sender
        //
        //   dateSent:
        //     (Optional) The date the message was sent (GMT)
        //
        //   pageNumber:
        //     (Optional) The page to start retrieving results from
        //
        //   count:
        //     (Optional) The number of results to retrieve
        //
        //   callback:
        //     Method to call upon successful completion
        void ListSmsMessages(string to, string from, DateTime? dateSent, int? pageNumber, int? count, Action<SmsMessageResult> callback);
        //
        // Summary:
        //     List all subaccounts created for the authenticated account. Makes a GET request
        //     to the Account List resource.
        AccountResult ListSubAccounts();
        //
        // Summary:
        //     List all subaccounts created for the authenticated account
        //
        // Parameters:
        //   callback:
        //     Method to call upon successful completion
        void ListSubAccounts(Action<AccountResult> callback);
        //
        // Summary:
        //     List all subaccounts created for the authenticated account. Makes a GET request
        //     to the Account List resource.
        AccountResult ListSubAccounts(string friendlyName);
        //
        // Summary:
        //     List all subaccounts created for the authenticated account. Makes a GET request
        //     to the Account List resource.
        AccountResult ListSubAccounts(int? pageNumber, int? count);
        //
        // Summary:
        //     List subaccounts that match the provided FriendlyName for the authenticated
        //     account. Makes a GET request to the Account List resource.
        //
        // Parameters:
        //   friendlyName:
        //     Name associated with this account
        AccountResult ListSubAccounts(string friendlyName, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a set of Transcriptions that includes paging information.  Makes
        //     a GET request to the Transcriptions List resource.
        TranscriptionResult ListTranscriptions();
        //
        // Summary:
        //     Returns a set of Transcriptions that includes paging information, sorted
        //     by 'DateUpdated', with most recent transcripts first.
        //
        // Parameters:
        //   callback:
        //     The method to call upon the completion of the request
        void ListTranscriptions(Action<TranscriptionResult> callback);
        //
        // Summary:
        //     Returns a paged set of Transcriptions that includes paging information. 
        //     Makes a GET request to the Transcriptions List resource.
        //
        // Parameters:
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     The number of results to retrieve
        TranscriptionResult ListTranscriptions(int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a paged set of Transcriptions that includes paging information, sorted
        //     by 'DateUpdated', with most recent transcripts first.
        //
        // Parameters:
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     The number of results to retrieve
        //
        //   callback:
        //     The method to call upon the completion of the request
        void ListTranscriptions(int? pageNumber, int? count, Action<TranscriptionResult> callback);
        //
        // Summary:
        //     Returns a set of Transcriptions for a specific recording that includes paging
        //     information.  Makes a GET request to a Recording Transcriptions List resource.
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retrieve transcriptions for
        //
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     The number of results to retrieve
        TranscriptionResult ListTranscriptions(string recordingSid, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a set of Transcriptions for a specific recording that includes paging
        //     information, sorted by 'DateUpdated', with most recent transcripts first.
        //
        // Parameters:
        //   recordingSid:
        //     The Sid of the recording to retrieve transcriptions for
        //
        //   pageNumber:
        //     The page to start retrieving results from
        //
        //   count:
        //     The number of results to retrieve
        //
        //   callback:
        //     Method to call upon completion of request
        void ListTranscriptions(string recordingSid, int? pageNumber, int? count, Action<TranscriptionResult> callback);
        //
        // Summary:
        //     Returns a list of all usage resources
        UsageResult ListUsage();
        //
        // Summary:
        //     Returns a list of all usage resources
        //
        // Parameters:
        //   callback:
        void ListUsage(Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage resources for a specific usage category
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        UsageResult ListUsage(string category);
        //
        // Summary:
        //     Returns a list of usage resources for a specific usage category
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   callback:
        void ListUsage(string category, Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, where the data
        //     grouped using a specific interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        UsageResult ListUsage(string category, string interval);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        UsageResult ListUsage(string category, DateTime? startDate, DateTime? endDate);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, where the data
        //     grouped using a specific interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        //
        //   callback:
        void ListUsage(string category, string interval, Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        //
        //   callback:
        void ListUsage(string category, DateTime? startDate, DateTime? endDate, Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range, grouped by a specific time interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        UsageResult ListUsage(string category, string interval, DateTime? startDate, DateTime? endDate);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range, grouped by a specific time interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        //
        //   callback:
        void ListUsage(string category, string interval, DateTime? startDate, DateTime? endDate, Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range, grouped by a specific time interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        //
        //   pageNumber:
        //     (Optional) The page to start retrieving results from
        //
        //   count:
        //     (Optional) The number of results to retrieve
        UsageResult ListUsage(string category, string interval, DateTime? startDate, DateTime? endDate, int? pageNumber, int? count);
        //
        // Summary:
        //     Returns a list of usage resources for a specific category, within a specific
        //     date range, grouped by a specific time interval
        //
        // Parameters:
        //   category:
        //     The category used to filter the usage data
        //
        //   interval:
        //     The time interval used to group the usage data
        //
        //   startDate:
        //     The start date of the filter range
        //
        //   endDate:
        //     The end date of the filter range
        //
        //   pageNumber:
        //     (Optional) The page to start retrieving results from
        //
        //   count:
        //     (Optional) The number of results to retrieve
        //
        //   callback:
        void ListUsage(string category, string interval, DateTime? startDate, DateTime? endDate, int? pageNumber, int? count, Action<UsageResult> callback);
        //
        // Summary:
        //     Returns a list of usage triggers
        UsageTriggerResult ListUsageTriggers();
        //
        // Summary:
        //     Returns a list of usage triggers
        //
        // Parameters:
        //   callback:
        void ListUsageTriggers(Action<UsageTriggerResult> callback);
        //
        // Summary:
        //     Returns a list of usage triggers
        //
        // Parameters:
        //   recurring:
        //     A string defining the recurrance interval for this trigger
        //
        //   usageCategory:
        //     The usage category this trigger watches
        //
        //   triggerBy:
        //     The value at which the trigger will fire
        UsageTriggerResult ListUsageTriggers(string recurring, string usageCategory, string triggerBy);
        //
        // Summary:
        //     Returns a list of usage triggers
        //
        // Parameters:
        //   recurring:
        //     A string defining the recurrance interval for this trigger
        //
        //   usageCategory:
        //     The usage category this trigger watches
        //
        //   triggerBy:
        //     The value at which the trigger will fire
        //
        //   callback:
        void ListUsageTriggers(string recurring, string usageCategory, string triggerBy, Action<UsageTriggerResult> callback);
        //
        // Summary:
        //     Change a participant of a conference to be muted. Makes a GET request to
        //     a Conference Participant Instance resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to mute
        Participant MuteConferenceParticipant(string conferenceSid, string callSid);
        //
        // Summary:
        //     Change a participant of a conference to be muted
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to mute
        //
        //   callback:
        //     Method to call upon successful completion
        void MuteConferenceParticipant(string conferenceSid, string callSid, Action<Participant> callback);
        Message RedactMessage(string messageSid);
        void RedactMessage(string messageSid, Action<Message> callback);
        //
        // Summary:
        //     Redirect a call in progress to a new TwiML URL. Makes a POST request to a
        //     Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to redirect
        //
        //   options:
        //     Call settings. Only Url, Method, FallbackUrl, FallbackMethod, StatusCallback
        //     and StatusCallbackMethod properties with values set will be used.
        Call RedirectCall(string callSid, CallOptions options);
        //
        // Summary:
        //     Redirect a call in progress to a new TwiML URL. Makes a POST request to a
        //     Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to redirect
        //
        //   options:
        //     Call settings. Only Url, Method, FallbackUrl, FallbackMethod, StatusCallback
        //     and StatusCallbackMethod properties with values set will be used.
        //
        //   callback:
        //     Method to call upon successful completion
        void RedirectCall(string callSid, CallOptions options, Action<Call> callback);
        //
        // Summary:
        //     Redirect a call in progress to a new TwiML URL. Makes a POST request to a
        //     Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to redirect
        //
        //   redirectUrl:
        //     The URL to redirect the call to.
        //
        //   redirectMethod:
        //     The HTTP method to use when requesting the redirectUrl
        Call RedirectCall(string callSid, string redirectUrl, string redirectMethod);
        //
        // Summary:
        //     Redirect a call in progress to a new TwiML URL. Makes a POST request to a
        //     Call Instance resource.
        //
        // Parameters:
        //   callSid:
        //     The Sid of the call to redirect
        //
        //   redirectUrl:
        //     The URL to redirect the call to.
        //
        //   redirectMethod:
        //     The HTTP method to use when requesting the redirectUrl
        //
        //   callback:
        //     Method to call upon successful completion
        void RedirectCall(string callSid, string redirectUrl, string redirectMethod, Action<Call> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        Message SendMessage(string from, string to, string body);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        Message SendMessage(string from, string to, string[] mediaUrls);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        void SendMessage(string from, string to, string body, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the MessageSid as well as MessageStatus=sent or MessageStatus=failed
        Message SendMessage(string from, string to, string body, string statusCallback);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        Message SendMessage(string from, string to, string body, string[] mediaUrls);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        void SendMessage(string from, string to, string[] mediaUrls, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to.
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the MessageSid as well as MessageStatus=sent or MessageStatus=failed
        void SendMessage(string from, string to, string body, string statusCallback, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients.  Makes a POST request to
        //     the Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        void SendMessage(string from, string to, string body, string[] mediaUrls, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the MessageSid as well as MessageStatus=sent or MessageStatus=failed
        Message SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        void SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the MessageSid as well as MessageStatus=sent or MessageStatus=failed
        //
        //   applicationSid:
        Message SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback, string applicationSid);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        //
        //   applicationSid:
        void SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback, string applicationSid, Action<Message> callback);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   mediaUrls:
        //     An array of URLs where each member of the array points to a media file to
        //     be sent with the message. You can include a maximum of 10 media URLs
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the MessageSid as well as MessageStatus=sent or MessageStatus=failed
        //
        //   applicationSid:
        //
        //   mmsOnly:
        //     Doesn't fallback to SMS if set to true
        Message SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback, string applicationSid, bool? mmsOnly);
        //
        // Summary:
        //     Send a new Message to the specified recipients Makes a POST request to the
        //     Messages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        //
        //   applicationSid:
        //
        //   mmsOnly:
        //     Doesn't fallback to SMS if set to true
        void SendMessage(string from, string to, string body, string[] mediaUrls, string statusCallback, string applicationSid, bool? mmsOnly, Action<Message> callback);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients.  Makes a POST request
        //     to the SMSMessages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        SMSMessage SendSmsMessage(string from, string to, string body);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   callback:
        //     Method to call upon successful completion
        void SendSmsMessage(string from, string to, string body, Action<SMSMessage> callback);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients Makes a POST request to
        //     the SMSMessages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        SMSMessage SendSmsMessage(string from, string to, string body, string statusCallback);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        //
        //   callback:
        //     Method to call upon successful completion
        void SendSmsMessage(string from, string to, string body, string statusCallback, Action<SMSMessage> callback);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients Makes a POST request to
        //     the SMSMessages List resource.
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        //
        //   applicationSid:
        //     Twilio will POST SmsSid as well as SmsStatus=sent or SmsStatus=failed to
        //     the URL in the SmsStatusCallback property of this Application. If the StatusCallback
        //     parameter above is also passed, the Application's SmsStatusCallback parameter
        //     will take precedence.
        SMSMessage SendSmsMessage(string from, string to, string body, string statusCallback, string applicationSid);
        //
        // Summary:
        //     Send a new SMS message to the specified recipients
        //
        // Parameters:
        //   from:
        //     The phone number to send the message from. Must be a Twilio-provided or ported
        //     local (not toll-free) number. Validated outgoing caller IDs cannot be used.
        //
        //   to:
        //     The phone number to send the message to. If using the Sandbox, this number
        //     must be a validated outgoing caller ID
        //
        //   body:
        //     The message to send. Must be 160 characters or less.
        //
        //   statusCallback:
        //     A URL that Twilio will POST to when your message is processed. Twilio will
        //     POST the SmsSid as well as SmsStatus=sent or SmsStatus=failed
        //
        //   callback:
        //     Method to call upon successful completion
        //
        //   applicationSid:
        //     Twilio will POST SmsSid as well as SmsStatus=sent or SmsStatus=failed to
        //     the URL in the SmsStatusCallback property of this Application. If the StatusCallback
        //     parameter above is also passed, the Application's SmsStatusCallback parameter
        //     will take precedence.
        void SendSmsMessage(string from, string to, string body, string statusCallback, string applicationSid, Action<SMSMessage> callback);
        //
        // Summary:
        //     Transfer phone numbes between master and sub accounts
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the phone number to move
        //
        //   sourceAccountSid:
        //     The AccountSid of the current owning account to move the phone number from
        //
        //   targetAccountSid:
        //     The AccountSid of the account to move the phone number to
        IncomingPhoneNumber TransferIncomingPhoneNumber(string incomingPhoneNumberSid, string sourceAccountSid, string targetAccountSid);
        //
        // Summary:
        //     Transfer phone numbes between master and sub accounts
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the phone number to move
        //
        //   sourceAccountSid:
        //     The AccountSid of the current owning account to move the phone number from
        //
        //   targetAccountSid:
        //     The AccountSid of the account to move the phone number to
        //
        //   callback:
        //     Method to call upon successful completion
        void TransferIncomingPhoneNumber(string incomingPhoneNumberSid, string sourceAccountSid, string targetAccountSid, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Change a participant of a conference to be unmuted. Makes a GET request to
        //     a Conference Participant Instance resource.
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to unmute
        Participant UnmuteConferenceParticipant(string conferenceSid, string callSid);
        //
        // Summary:
        //     Change a participant of a conference to be unmuted
        //
        // Parameters:
        //   conferenceSid:
        //     The Sid of the conference
        //
        //   callSid:
        //     The Sid of the call to unmute
        //
        //   callback:
        //     Method to call upon successful completion
        void UnmuteConferenceParticipant(string conferenceSid, string callSid, Action<Participant> callback);
        //
        // Summary:
        //     Update the friendly name associated with the currently authenticated account.
        //     Makes a POST request to an Account Instance resource.
        //
        // Parameters:
        //   friendlyName:
        //     Name to use when updating
        Account UpdateAccountName(string friendlyName);
        //
        // Summary:
        //     Update the friendly name associated with the currently authenticated account
        //
        // Parameters:
        //   friendlyName:
        //     Name to use when updating
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateAccountName(string friendlyName, Action<Account> callback);
        //
        // Summary:
        //     Update an Address. All attributes of the address except for the IsoCountry
        //     can be updated.
        //
        // Parameters:
        //   addressSid:
        //     The sid of the Address to update.
        //
        //   options:
        //     Which properties to update. Only properties with values set will be updated.
        //
        // Returns:
        //     Updated Address object.
        Address UpdateAddress(string addressSid, AddressOptions options);
        //
        // Summary:
        //     Update an address on the current account.
        //
        // Parameters:
        //   addressSid:
        //     Sid of the address to update
        //
        //   options:
        //     Attributes to update. Only properties with a value set will be updated.
        //
        //   callback:
        //     Method to call upon successful completion.
        void UpdateAddress(string addressSid, AddressOptions options, Action<Address> callback);
        //
        // Summary:
        //     Tries to update the application's properties, and returns the updated resource
        //     representation if successful.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the application to update
        //
        //   friendlyName:
        //     The friendly name to rename the application to (optional, null to leave as-is)
        //
        //   options:
        //     Which settings to update. Only properties with values set will be updated.
        Application UpdateApplication(string applicationSid, string friendlyName, ApplicationOptions options);
        //
        // Summary:
        //     Tries to update the application's properties, and returns the updated resource
        //     representation if successful.
        //
        // Parameters:
        //   applicationSid:
        //     The Sid of the application to update
        //
        //   friendlyName:
        //     The friendly name to rename the application to (optional, null to leave as-is)
        //
        //   options:
        //     Which settings to update. Only properties with values set will be updated.
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateApplication(string applicationSid, string friendlyName, ApplicationOptions options, Action<Application> callback);
        //
        // Summary:
        //     Tries to update the ConnectApp's properties, and returns the updated resource
        //     representation if successful.
        //
        // Parameters:
        //   connectAppSid:
        //     The Sid of the ConnectApp to update
        //
        //   friendlyName:
        //     A human readable description of the Connect App, with maximum length 64 characters.
        //     (optional, null to leave as-is)
        //
        //   authorizeRedirectUrl:
        //     The URL the user's browser will redirect to after Twilio authenticates the
        //     user and obtains authorization for this Connect App. (optional, null to leave
        //     as-is)
        //
        //   deauthorizeCallbackUrl:
        //     The URL to which Twilio will send a request when a user de-authorizes this
        //     Connect App. (optional, null to leave as-is)
        //
        //   deauthorizeCallbackMethod:
        //     The HTTP method to be used when making a request to the DeauthorizeCallbackUrl.
        //     Either GET or POST. (optional, null to leave as-is)
        //
        //   permissions:
        //     A comma-separated list of permssions you will request from users of this
        //     ConnectApp. Valid permssions are get-all or post-all. (optional, null to
        //     leave as-is)
        //
        //   description:
        //     A more detailed human readable description of the Connect App. (optional,
        //     null to leave as-is)
        //
        //   companyName:
        //     The company name for this Connect App. (optional, null to leave as-is)
        //
        //   homepageUrl:
        //     The URL where users can obtain more information about this Connect
        //     App. (optional, null to leave as-is)
        ConnectApp UpdateConnectApp(string connectAppSid, string friendlyName, string authorizeRedirectUrl, string deauthorizeCallbackUrl, string deauthorizeCallbackMethod, string permissions, string description, string companyName, string homepageUrl);
        //
        // Summary:
        //     Tries to update the ConnectApp's properties, and returns the updated resource
        //     representation if successful.
        //
        // Parameters:
        //   connectAppSid:
        //     The Sid of the ConnectApp to update
        //
        //   friendlyName:
        //     A human readable description of the Connect App, with maximum length 64 characters.
        //     (optional, null to leave as-is)
        //
        //   authorizeRedirectUrl:
        //     The URL the user's browser will redirect to after Twilio authenticates the
        //     user and obtains authorization for this Connect App. (optional, null to leave
        //     as-is)
        //
        //   deauthorizeCallbackUrl:
        //     The URL to which Twilio will send a request when a user de-authorizes this
        //     Connect App. (optional, null to leave as-is)
        //
        //   deauthorizeCallbackMethod:
        //     The HTTP method to be used when making a request to the DeauthorizeCallbackUrl.
        //     Either GET or POST. (optional, null to leave as-is)
        //
        //   permissions:
        //     A comma-separated list of permssions you will request from users of this
        //     ConnectApp. Valid permssions are get-all or post-all. (optional, null to
        //     leave as-is)
        //
        //   description:
        //     A more detailed human readable description of the Connect App. (optional,
        //     null to leave as-is)
        //
        //   companyName:
        //     The company name for this Connect App. (optional, null to leave as-is)
        //
        //   homepageUrl:
        //     The URL where users can obtain more information about this Connect
        //     App. (optional, null to leave as-is)
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateConnectApp(string connectAppSid, string friendlyName, string authorizeRedirectUrl, string deauthorizeCallbackUrl, string deauthorizeCallbackMethod, string permissions, string description, string companyName, string homepageUrl, Action<ConnectApp> callback);
        //
        // Summary:
        //     Updates a specific Credential resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList that contains the Credential
        //
        //   credentialSid:
        //     The Sid of the Credential to update
        //
        //   username:
        //
        //   password:
        Credential UpdateCredential(string credentialListSid, string credentialSid, string username, string password);
        //
        // Summary:
        //     Updates a specific Credential resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList that contains the Credential
        //
        //   credentialSid:
        //     The Sid of the Credential to update
        //
        //   username:
        //
        //   password:
        void UpdateCredential(string credentialListSid, string credentialSid, string username, string password, Action<Credential> callback);
        //
        // Summary:
        //     Updates a specific CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   friendlyName:
        //     The name of the CredentialList
        CredentialList UpdateCredentialList(string credentialListSid, string friendlyName);
        //
        // Summary:
        //     Updates a specific CredentialList resource
        //
        // Parameters:
        //   credentialListSid:
        //     The Sid of the CredentialList
        //
        //   friendlyName:
        //     The name of the CredentialList
        void UpdateCredentialList(string credentialListSid, string friendlyName, Action<CredentialList> callback);
        //
        // Summary:
        //     Updates a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to update
        //
        //   options:
        //     Optional parameters for a SIP domain
        Domain UpdateDomain(string domainSid, DomainOptions options);
        //
        // Summary:
        //     Updates a specific SIP Domain resource
        //
        // Parameters:
        //   domainSid:
        //     The Sid of the SIP Domain to update
        //
        //   options:
        //     Optional parameters for a SIP domain
        void UpdateDomain(string domainSid, DomainOptions options, Action<Domain> callback);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        Feedback UpdateFeedback(string callSid, int qualityScore);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        void UpdateFeedback(string callSid, int qualityScore, Action<Feedback> callback);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        Feedback UpdateFeedback(string callSid, int qualityScore, List<string> issues);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        Feedback UpdateFeedback(string callSid, int qualityScore, string issue);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        void UpdateFeedback(string callSid, int qualityScore, List<string> issues, Action<Feedback> callback);
        //
        // Summary:
        //     Updates the current Feedback entry for a specific CallSid.
        void UpdateFeedback(string callSid, int qualityScore, string issue, Action<Feedback> callback);
        //
        // Summary:
        //     Update the settings of an incoming phone number
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the phone number to update
        //
        //   options:
        //     Which settings to update. Only properties with values set will be updated.
        IncomingPhoneNumber UpdateIncomingPhoneNumber(string incomingPhoneNumberSid, PhoneNumberOptions options);
        //
        // Summary:
        //     Update the settings of an incoming phone number.
        //
        // Parameters:
        //   incomingPhoneNumberSid:
        //     The Sid of the phone number to update
        //
        //   options:
        //     Which settings to update. Only properties with values set will be updated.
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateIncomingPhoneNumber(string incomingPhoneNumberSid, PhoneNumberOptions options, Action<IncomingPhoneNumber> callback);
        //
        // Summary:
        //     Updates a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList to update
        //
        //   friendlyName:
        //     The name of the IpAccessControlList
        IpAccessControlList UpdateIpAccessControlList(string ipAccessControlListSid, string friendlyName);
        //
        // Summary:
        //     Updates a specific IpAccessControlList resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList to update
        //
        //   friendlyName:
        //     The name of the IpAccessControlList
        void UpdateIpAccessControlList(string ipAccessControlListSid, string friendlyName, Action<IpAccessControlList> callback);
        //
        // Summary:
        //     Updates a specific IpAddress resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   ipAddressSid:
        //     The Sid of the IpAddress to update
        //
        //   friendlyName:
        //     The name of the IpAddress
        //
        //   ipAddress:
        //     The address value of the IpAddress
        IpAddress UpdateIpAddress(string ipAccessControlListSid, string ipAddressSid, string friendlyName, string ipAddress);
        //
        // Summary:
        //     Updates a specific IpAddress resource
        //
        // Parameters:
        //   ipAccessControlListSid:
        //     The Sid of the IpAccessControlList
        //
        //   ipAddressSid:
        //     The Sid of the IpAddress to update
        //
        //   friendlyName:
        //     The name of the IpAddress
        //
        //   ipAddress:
        //     The address value of the IpAddress
        void UpdateIpAddress(string ipAccessControlListSid, string ipAddressSid, string friendlyName, string ipAddress, Action<IpAddress> callback);
        //
        // Summary:
        //     Update the FriendlyName associated with a validated outgoing caller ID entry.
        //      Makes a POST request to an OutgoingCallerId Instance resource.
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid of the outgoing caller ID entry
        //
        //   friendlyName:
        //     The name to update the FriendlyName to
        OutgoingCallerId UpdateOutgoingCallerIdName(string outgoingCallerIdSid, string friendlyName);
        //
        // Summary:
        //     Update the FriendlyName associated with a validated outgoing caller ID entry
        //
        // Parameters:
        //   outgoingCallerIdSid:
        //     The Sid of the outgoing caller ID entry
        //
        //   friendlyName:
        //     The name to update the FriendlyName to
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateOutgoingCallerIdName(string outgoingCallerIdSid, string friendlyName, Action<OutgoingCallerId> callback);
        //
        // Summary:
        //     Updates a specific Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to update
        //
        //   friendlyName:
        //     The name of the Queue
        //
        //   maxSize:
        //     The maximum number of calls allowed in the queue
        Queue UpdateQueue(string queueSid, string friendlyName, int maxSize);
        //
        // Summary:
        //     Updates a specific Queue resource
        //
        // Parameters:
        //   queueSid:
        //     The Sid of the Queue to update
        //
        //   friendlyName:
        //     The name of the Queue
        //
        //   maxSize:
        //     The maximum number of calls allowed in the queue
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateQueue(string queueSid, string friendlyName, int maxSize, Action<Queue> callback);
        //
        // Summary:
        //     Update the TwiML voice and SMS URLs associated with the sandbox number. 
        //     Makes a POST request to the Sandbox Instance resource.
        //
        // Parameters:
        //   voiceUrl:
        //     The URL to use for incoming calls to your sandbox number.
        //
        //   voiceMethod:
        //     The HTTP method to use for incoming calls to your sandbox number.
        //
        //   smsUrl:
        //     The URL to use for incoming SMS text messages sent to your sandbox number.
        //
        //   smsMethod:
        //     The HTTP method to use for incoming text messages sent to your sandbox number.
        Sandbox UpdateSandbox(string voiceUrl, string voiceMethod, string smsUrl, string smsMethod);
        //
        // Summary:
        //     Update the TwiML voice and SMS URLs associated with the sandbox number
        //
        // Parameters:
        //   voiceUrl:
        //     The URL to use for incoming calls to your sandbox number.
        //
        //   voiceMethod:
        //     The HTTP method to use for incoming calls to your sandbox number.
        //
        //   smsUrl:
        //     The URL to use for incoming SMS text messages sent to your sandbox number.
        //
        //   smsMethod:
        //     The HTTP method to use for incoming text messages sent to your sandbox number.
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateSandbox(string voiceUrl, string voiceMethod, string smsUrl, string smsMethod, Action<Sandbox> callback);
        //
        // Summary:
        //     Tries to update the shortcode's properties, and returns the updated resource
        //     representation if successful.  Makes a POST request to the ShortCode instance
        //     resource.
        //
        // Parameters:
        //   shortCodeSid:
        //     The Sid of the ShortCode instance to update
        //
        //   friendlyName:
        //     A human readable description of the short code, with maximum length 64 characters.
        //
        //   apiVersion:
        //     SMSs to this short code will start a new TwiML session with this API version.
        //
        //   smsUrl:
        //     The URL that Twilio should request when somebody sends an SMS to the short
        //     code.
        //
        //   smsMethod:
        //     The HTTP method that should be used to request the SmsUrl. Either GET or
        //     POST.
        //
        //   smsFallbackUrl:
        //     A URL that Twilio will request if an error occurs requesting or executing
        //     the TwiML at the SmsUrl.
        //
        //   smsFallbackMethod:
        //     The HTTP method that should be used to request the SmsFallbackUrl. Either
        //     GET or POST.
        SMSShortCode UpdateShortCode(string shortCodeSid, string friendlyName, string apiVersion, string smsUrl, string smsMethod, string smsFallbackUrl, string smsFallbackMethod);
        //
        // Summary:
        //     Tries to update the shortcode's properties, and returns the updated resource
        //     representation if successful.  Makes a POST request to the ShortCode instance
        //     resource.
        //
        // Parameters:
        //   shortCodeSid:
        //     The Sid of the ShortCode instance to update
        //
        //   friendlyName:
        //     A human readable description of the short code, with maximum length 64 characters.
        //
        //   apiVersion:
        //     SMSs to this short code will start a new TwiML session with this API version.
        //
        //   smsUrl:
        //     The URL that Twilio should request when somebody sends an SMS to the short
        //     code.
        //
        //   smsMethod:
        //     The HTTP method that should be used to request the SmsUrl. Either GET or
        //     POST.
        //
        //   smsFallbackUrl:
        //     A URL that Twilio will request if an error occurs requesting or executing
        //     the TwiML at the SmsUrl.
        //
        //   smsFallbackMethod:
        //     The HTTP method that should be used to request the SmsFallbackUrl. Either
        //     GET or POST.
        //
        //   callback:
        //     Method to call upon successful completion
        void UpdateShortCode(string shortCodeSid, string friendlyName, string apiVersion, string smsUrl, string smsMethod, string smsFallbackUrl, string smsFallbackMethod, Action<SMSShortCode> callback);
        //
        // Summary:
        //     Updates a specific UsageTrigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the UsageTrigger to update
        //
        //   friendlyName:
        //     The friendly name of the trigger
        //
        //   callbackUrl:
        //     The URL to call once a trigger value has been met
        //
        //   callbackMethod:
        //     The HTTP method used when requesting the callback URL
        UsageTrigger UpdateUsageTrigger(string usageTriggerSid, string friendlyName, string callbackUrl, string callbackMethod);
        //
        // Summary:
        //     Updates a specific UsageTrigger resource
        //
        // Parameters:
        //   usageTriggerSid:
        //     The Sid of the UsageTrigger to update
        //
        //   friendlyName:
        //     The friendly name of the trigger
        //
        //   callbackUrl:
        //     The URL to call once a trigger value has been met
        //
        //   callbackMethod:
        //     The HTTP method used when requesting the callback URL
        //
        //   callback:
        void UpdateUsageTrigger(string usageTriggerSid, string friendlyName, string callbackUrl, string callbackMethod, Action<UsageTrigger> callback);
    
    }
}

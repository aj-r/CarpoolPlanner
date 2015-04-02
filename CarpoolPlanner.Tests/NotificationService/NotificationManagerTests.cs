using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarpoolPlanner.Model;
using CarpoolPlanner.NotificationService;
using Moq;
using NUnit.Framework;
using Twilio;

namespace CarpoolPlanner.Tests.NotificationService
{
    [TestFixture]
    public class NotificationManagerTests
    {
        [Test]
        public async Task SendNotification_ShouldSendSMS()
        {
            var sent = false;
            var user = GetTestUser();
            var mockTwilioClient = new Mock<ITwilioRestClient>();
            SetupForSmsNotifications(mockTwilioClient, user.Phone, "Test message", () => sent = true);
            var manager = GetTestNotificationManager(Mock.Of<IDbContextProvider>(), mockTwilioClient.Object);

            var success = await manager.SendNotification(user, "Test subject", "Test message");

            Assert.That(success, Is.True, "SendNotification failed.");
            Assert.That(sent, Is.True, "Failed to send message.");
        }

        [Test]
        public async Task SendNotification_ShouldNotSend()
        {
            var sent = false;
            var mockTwilioClient = new Mock<ITwilioRestClient>();
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sent = true);
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sent = true);
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => sent = true);
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<SMSMessage>>()))
                .Callback(() => sent = true);
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<SMSMessage>>()))
                .Callback(() => sent = true);
            mockTwilioClient.Setup(t => t.SendSmsMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<SMSMessage>>()))
                .Callback(() => sent = true);
            var manager = GetTestNotificationManager(Mock.Of<IDbContextProvider>(), mockTwilioClient.Object);
            var user = GetTestUser();
            user.PhoneNotify = false;

            var success = await manager.SendNotification(user, "Test subject", "Test message");

            Assert.That(success, Is.True, "SendNotification failed.");
            Assert.That(sent, Is.False, "Should not have sent message.");
        }

        [Test]
        public async Task SendInitialNotification_ShouldSend()
        {
            var sendCount = 0;
            var saved = false;
            var mockTwilioClient = new Mock<ITwilioRestClient>();
            SetupForSmsNotifications(mockTwilioClient, () => ++sendCount);

            var tripInstance = GetTestTripInstance();
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.GetTripInstanceById(2)).Returns(tripInstance);
            mockContext.Setup(c => c.SaveChanges()).Callback(() => saved = true);
            var mockContextProvider = Mock.Of<IDbContextProvider>(p => p.GetContext() == mockContext.Object);

            var manager = GetTestNotificationManager(mockContextProvider, mockTwilioClient.Object);

            var success = await manager.SendInitialNotification(2);

            Assert.That(success, Is.True, "SendNotification failed.");
            Assert.That(sendCount, Is.EqualTo(3), "Sent wrong numer of messages.");
            Assert.That(saved, Is.True, "SendNotification did not save initial notification times.");
            foreach (var userTripInstnace in tripInstance.UserTripInstances)
            {
                Assert.That(userTripInstnace.InitialNotificationTime, Is.Not.Null);
                Assert.That(userTripInstnace.InitialNotificationTime, Is.AtMost(DateTime.UtcNow));
            }
        }

        [Test]
        public async Task SendInitialNotification_ShouldNotSend_IfAlreadySent()
        {
            var sendCount = 0;
            var mockTwilioClient = new Mock<ITwilioRestClient>();
            SetupForSmsNotifications(mockTwilioClient, () => ++sendCount);

            var tripInstance = GetTestTripInstance();
            foreach (var userTripInstnace in tripInstance.UserTripInstances)
                userTripInstnace.InitialNotificationTime = DateTime.UtcNow;
            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.Setup(c => c.GetTripInstanceById(2)).Returns(tripInstance);
            var mockContextProvider = Mock.Of<IDbContextProvider>(p => p.GetContext() == mockContext.Object);

            var manager = GetTestNotificationManager(mockContextProvider, mockTwilioClient.Object);

            var success = await manager.SendInitialNotification(2);

            Assert.That(success, Is.True, "SendNotification failed.");
            Assert.That(sendCount, Is.EqualTo(0), "Sent wrong numer of messages.");
        }
        //TODO: test with inactive users
        //TODO: test with non-null status
        //TODO: test when notification was already sent


        [Test]
        public async Task SetNextNotificationTimes_ShouldSendInitialNotification()
        {
            var sendCount = 0;
            var mockTwilioClient = new Mock<ITwilioRestClient>();
            SetupForSmsNotifications(mockTwilioClient, () => ++sendCount);

            var tripInstance = GetTestTripInstance();
            tripInstance.Date = DateTime.UtcNow + TimeSpan.FromHours(4);
            var mockContext = Mock.Of<IApplicationDbContext>((c => c.GetTripInstanceById(2) == tripInstance));
            var mockContextProvider = Mock.Of<IDbContextProvider>(p => p.GetContext() == mockContext);

            var manager = GetTestNotificationManager(mockContextProvider, mockTwilioClient.Object);

            await manager.SetNextNotificationTimes(tripInstance, 0);

            //await Task.Delay(10000);

            Assert.That(sendCount, Is.EqualTo(3), "Sent wrong numer of messages.");
        }
        //TODO: test with reminder notification, final notification.
        //TODO: after the event is over.
        //TODO: test before initial notification with timers somehow?



        private void SetupForSmsNotifications(Mock<ITwilioRestClient> mockTwilioClient, Action callback)
        {
            mockTwilioClient.Setup(t => t.SendSmsMessage("+15555555555", It.IsNotNull<string>(), It.IsNotNull<string>(), It.IsAny<string>(), It.IsNotNull<Action<SMSMessage>>()))
                .Callback((string from, string to2, string m, string s, Action<SMSMessage> a) =>
                {
                    a(new SMSMessage { Status = "queued" });
                    if (callback != null)
                        callback();
                });
        }

        private void SetupForSmsNotifications(Mock<ITwilioRestClient> mockTwilioClient, string to, string message, Action callback)
        {
            mockTwilioClient.Setup(t => t.SendSmsMessage("+15555555555", to, message, It.IsAny<string>(), It.IsNotNull<Action<SMSMessage>>()))
                .Callback((string from, string to2, string m, string s, Action<SMSMessage> a) =>
                {
                    a(new SMSMessage { Status = "queued" });
                    if (callback != null)
                        callback();
                });
        }

        private NotificationManager GetTestNotificationManager(IDbContextProvider contextProvider, ITwilioRestClient twilioClient)
        {
            return new NotificationManager(contextProvider, twilioClient)
            {
                SmsNumber = "+15555555555",
                InitialAdvanceNotificationTime = TimeSpan.FromHours(7),
                ReminderAdvanceNotificationTime = TimeSpan.FromHours(2),
                FinalAdvanceNotificationTime = TimeSpan.FromHours(1)
            };
        }

        private User GetTestUser()
        {
            return new User { Id = 1, Phone = "+15555556666", PhoneNotify = true, CommuteMethod = CommuteMethod.NeedRide, Status = UserStatus.Active };
        }

        private TripInstance GetTestTripInstance()
        {
            var tripInstance = new TripInstance
            {
                Id = 2,
                Date = new DateTime(2015, 12, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                Trip = new Trip { TimeZone = "America/New_York" }
            };
            tripInstance.UserTripInstances.Add(new UserTripInstance
            {
                UserId = 1,
                CommuteMethod = CommuteMethod.NeedRide,
                User = GetTestUser()
            });

            var user2 = GetTestUser();
            user2.Id = 2;
            user2.Phone = "+15555557777";
            tripInstance.UserTripInstances.Add(new UserTripInstance
            {
                UserId = 2,
                CommuteMethod = CommuteMethod.HaveRide,
                CanDriveIfNeeded = true,
                User = user2
            });

            var user3 = GetTestUser();
            user3.Id = 3;
            user3.Phone = "+15555558888";
            tripInstance.UserTripInstances.Add(new UserTripInstance
            {
                UserId = 3,
                CommuteMethod = CommuteMethod.Driver,
                User = user3
            });

            return tripInstance;
        }
    }
}

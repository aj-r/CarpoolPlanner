using CarpoolPlanner.NotificationService;
using NUnit.Framework;

namespace CarpoolPlanner.Tests.NotificationService
{
    [TestFixture]
    public class TwilioSmsClientTests
    {
        [Test]
        public void SplitMessage_ShouldNotSplit()
        {
            var messages = TwilioSmsClient.SplitMessage(@"Test message.", 160);

            foreach (var message in messages)
                Assert.That(message.Length, Is.AtMost(160), "Message is too long.");
            Assert.That(messages.Length, Is.EqualTo(1), "Wrong number of messages.");
        }

        [Test]
        public void SplitMessage_ShouldSplit_IfTooLong()
        {
            var messages = TwilioSmsClient.SplitMessage(@"Test very long message that is greater than 160 characters.
Lorum ipsum dolor sit amet consectetor adipiscing elit. Praesent commodo nisi nec turpis finibus, et interdum libero sollicitudin. Vestibulum ante ipsum metus.", 160);

            foreach (var message in messages)
                Assert.That(message.Length, Is.AtMost(160), "Message is too long.");
            Assert.That(messages.Length, Is.EqualTo(2), "Wrong number of messages.");
        }
    }
}

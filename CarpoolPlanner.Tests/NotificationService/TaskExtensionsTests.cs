using System;
using System.Threading.Tasks;
using CarpoolPlanner.NotificationService;
using NUnit.Framework;

namespace CarpoolPlanner.Tests.NotificationService
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        [Test]
        public async Task TaskWithErrorHandler_ShouldCallHandler()
        {
            var exceptionCount = 0;
            GeneralExceptionHandler.ExceptionEncountered += ex => ++exceptionCount;
            var tcs = new TaskCompletionSource<bool>();
            tcs.Task.WithErrorHandler();

            tcs.SetException(new Exception());

            await Task.Yield();

            Assert.That(exceptionCount, Is.EqualTo(1));
        }
    }
}

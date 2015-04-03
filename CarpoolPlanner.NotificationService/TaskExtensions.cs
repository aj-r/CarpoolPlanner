using System.Threading.Tasks;

namespace CarpoolPlanner.NotificationService
{
    /// <summary>
    /// Contains extensions for Tasks.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Executes a task and sends any exceptions to the general exception handler.
        /// </summary>
        /// <param name="task"></param>
        public static void WithErrorHandler(this Task task)
        {
            task.ContinueWith(t => GeneralExceptionHandler.Raise(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}

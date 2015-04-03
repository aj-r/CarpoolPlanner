using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpoolPlanner.NotificationService
{
    public static class GeneralExceptionHandler
    {
        public static event Action<Exception> ExceptionEncountered;

        /// <summary>
        /// Raises the ExceptionEncountered event.
        /// </summary>
        /// <param name="ex">The exception that was thrown.</param>
        public static void Raise(Exception ex)
        {
            if (ExceptionEncountered != null)
                ExceptionEncountered(ex);
        }

        /// <summary>
        /// Raises the ExceptionEncountered event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Raise(object sender, UnhandledExceptionEventArgs e)
        {
            Raise(e.ExceptionObject as Exception);
        }
    }
}

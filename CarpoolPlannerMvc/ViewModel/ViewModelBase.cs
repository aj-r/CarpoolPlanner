using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarpoolPlanner.ViewModel
{
    public enum MessageType
    {
        Info,
        Success,
        Error
    }

    public class ViewModelBase
    {
        private static readonly string[] classNameMappings = { "", "text-success", "text-danger" };

        private string message = string.Empty;
        private MessageType messageType;

        public void SetMessage(string message, MessageType type)
        {
            this.message = message;
            messageType = type;
        }

        public void ClearMessage()
        {
            message = string.Empty;
        }

        /// <summary>
        /// Gets the HTML-rendered message (if any) that should be displayed to the user.
        /// </summary>
        /// <returns>The HTML-rendered message.</returns>
        public MvcHtmlString RenderMessage()
        {
            // TODO: use angular instead of id/class?
            // Also, is there a better way to do this? It seems like we are putting view code in the model...
            var index = (int)messageType;
            var result = string.Format("<p id=\"message\" class=\"{0}\">{1}</p>",
                classNameMappings[index],
                !string.IsNullOrWhiteSpace(message) ? message : "&nbsp;");
            return MvcHtmlString.Create(result);
        }
    }
}
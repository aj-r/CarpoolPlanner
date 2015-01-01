using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

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

        public ViewModelBase()
        {
            Message = string.Empty;
            MessageType = MessageType.Info;
        }

        public string Message { get; set; }
        public MessageType MessageType { get; set; }

        public long? UserId
        {
            get
            {
                var user = AppUtils.CurrentUser;
                return user != null ? (int?)user.Id : null;
            }
        }

        public string LoginName
        {
            get
            {
                var user = AppUtils.CurrentUser;
                return user != null ? user.LoginName : null;
            }
        }

        public void SetMessage(string message, MessageType type)
        {
            Message = message;
            MessageType = type;
        }

        public void ClearMessage()
        {
            Message = string.Empty;
        }

        /// <summary>
        /// Gets the HTML-rendered message (if any) that should be displayed to the user.
        /// </summary>
        /// <returns>The HTML-rendered message.</returns>
        public MvcHtmlString RenderMessage()
        {
            // TODO: use angular instead of id/class?
            // Also, is there a better way to do this? It seems like we are putting view code in the model...
            var index = (int)MessageType;
            var result = string.Format("<p id=\"message\" class=\"{0}\">{1}</p>",
                classNameMappings[index],
                !string.IsNullOrWhiteSpace(Message) ? Message : "&nbsp;");
            return MvcHtmlString.Create(result);
        }
    }
}
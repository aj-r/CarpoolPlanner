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
    }
}
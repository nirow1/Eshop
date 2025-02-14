namespace Eshop.Classes
{
    public enum FlashMessageType
    {
        Info,
        Success,
        Warning,
        Danger
    }
    public class FlashMessage
    {
        public string Message { get; set; }
        public FlashMessageType MessageType { get; set; }
        public FlashMessage (string message, FlashMessageType messageType)
        {
            Message = message;
            MessageType = messageType;
        }
    }
}

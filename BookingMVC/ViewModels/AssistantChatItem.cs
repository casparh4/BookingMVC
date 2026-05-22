using BookingMVC.Models;

namespace BookingMVC.ViewModels
{
    public class AssistantChatItem
    {
        public string Message { get; set; } = string.Empty;
        public Hotel? Hotel { get; set; }

        public AssistantChatItem() { }
        public AssistantChatItem(string message)
        {
            Message = message;
        }
    }
}

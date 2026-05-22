using BookingMVC.Models;
using OpenAI.Chat;

namespace BookingMVC.ViewModels
{
    public class ChatBotViewModel
    {
        public string UserMessage { get; set; } = string.Empty;
        public List<AssistantChatItem> AssistantChatMessages { get; set; } = new();
        public List<string> UserChatMessages { get; set; } = new List<string>();


        public Hotel? Hotel { get; set; }

        public ChatBotViewModel() { }
        public ChatBotViewModel(List<AssistantChatItem> assistantChatMessage,List<string> userChatMessage, string userMessage)
        {
            AssistantChatMessages = assistantChatMessage;
            UserChatMessages = userChatMessage;
            UserMessage = userMessage;
            
        }
    }//when a user asks for hotel respond with HOTELPLACEHOLDER to indicate you want a hotel sent along with a chat
}

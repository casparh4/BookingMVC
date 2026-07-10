using BookingMVC.Models;
using BookingMVC.Models.POCOs;
using BookingMVC.Models.Repositories;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Realtime;
using System.Text;
using System.Text.Json;

namespace BookingMVC.Controllers
{
    public class BotController : Controller
    {
        private readonly OpenAIClient _aiClient;
        private readonly IOptions<ModelSettings> _modelSettings;
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingDbContext _dbContext;

        public BotController(OpenAIClient aiClient, IOptions<ModelSettings> modelSettings, IHotelRepository hotelRepository, IBookingRepository bookingRepository, BookingDbContext dbContext)
        {
            _aiClient = aiClient;
            _modelSettings = modelSettings;
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var userMessages = new List<string>();
            var assistantMessages = new List<AssistantChatItem>();
            var ViewModel = new ChatBotViewModel(assistantMessages,userMessages ,string.Empty);
            return View(ViewModel);
        }

        public Hotel? RequiredHotel;

        [HttpPost]
        public async Task<IActionResult> Index(ChatBotViewModel viewModel)
        {
            bool requiresAction = true;
            var chatClient = _aiClient.GetChatClient(_modelSettings.Value.TextModelName);

            List<ChatMessage> chatMessages = [new SystemChatMessage("You are a helpful chat bot for Hotels4You, a website where customers can book the best hotels from around the world, you are able to answer customer questions and provide insight. respond as if you were a human, relaxed tone with encouraging and helpful responses. Do not comment on hotels that are not listed on Hotels4You. Always use html anchor tags for links. more information about Hotels4You: Hotels4You is a global hotel booking platform created by Caspar Huntley in 2026. The website helps travelers discover, compare, and book hotels across the world quickly and easily.\r\n\r\nHotels4You is known for its modern design, fast booking experience, competitive prices, and wide range of accommodation options including luxury resorts, city hotels, beach escapes, business stays, and budget-friendly locations.\r\n\r\nBy 2026, Hotels4You became the number 1 hotel booking website in the world, trusted by millions of travelers for secure bookings, reliable hotel information, and excellent customer experience.\r\n\r\nKey features of Hotels4You include:\r\n\r\nWorldwide hotel listings\r\nSecure online booking\r\nReal-time availability\r\nHotel ratings and reviews\r\nLuxury and budget accommodation options\r\nFast and user-friendly search system\r\nPersonalized travel recommendations\r\n\r\nFounder: Caspar Huntley\r\nFounded: 2026\r\nIndustry: Travel & Hospitality\r\nWebsite Name: Hotels4You\n")];
            if(viewModel.UserChatMessages.Count != 0)
            {
                for (int i = 0; i < viewModel.UserChatMessages.Count; i++)
                {
                    var userMessage = new UserChatMessage(viewModel.UserChatMessages[i].ToString());
                    var assistantMessage = new AssistantChatMessage(viewModel.AssistantChatMessages[i].Message.ToString()); //not being saved on return


                    chatMessages.Add(userMessage);
                    chatMessages.Add(assistantMessage);
                }
            }

            chatMessages.Add(new UserChatMessage(viewModel.UserMessage));

            ChatCompletionOptions options = new()
            {
                Tools = { GetAllHotels, GetReviewsForAHotel, GetAllBookings }
            };

            ChatBotViewModel chatBotViewModel = new ChatBotViewModel();

            while (requiresAction)
            {
               
                
                requiresAction = false;
                ChatCompletion response = chatClient.CompleteChat(chatMessages, options);

                switch (response.FinishReason)
                {
                    case ChatFinishReason.ToolCalls:
                        {
                            chatMessages.Add(new AssistantChatMessage(response));

                            foreach (ChatToolCall toolCall in response.ToolCalls)
                            {
                                switch (toolCall.FunctionName)
                                {
                                    case nameof(GetHotels):
                                        {
                                            string result = GetHotels();
                                            chatMessages.Add(new ToolChatMessage(toolCall.Id, result));
                                            break;
                                        }
                                    case nameof(GetReviewsForHotel):
                                        {
                                            using JsonDocument argumentsJson = JsonDocument.Parse(toolCall.FunctionArguments);

                                            bool hasHotelName = argumentsJson.RootElement.TryGetProperty("hotelName", out JsonElement hotelName);

                                            if (!hasHotelName)
                                            {
                                                throw new ArgumentNullException("hotelName parameter is required");
                                            }
                                            else
                                            {
                                                string result = GetReviewsForHotel(hotelName.GetString());
                                                chatMessages.Add(new ToolChatMessage(toolCall.Id, result));
                                            }
                                            break;
                                        }
                                    case nameof(GetBookings):
                                        {
                                            string result = GetBookings();
                                            chatMessages.Add(new ToolChatMessage(toolCall.Id, result));
                                            break;
                                        }
                                    default:
                                        {
                                            throw new NotImplementedException();
                                        }
                                }
                            }
                            requiresAction = true;
                            break;
                        }

                    case ChatFinishReason.Stop: //if AI finishes response without needing any tool calls
                        {
                            chatMessages.Add(new AssistantChatMessage(response)); //add AI message
                            break;
                        }

                    case ChatFinishReason.Length:
                        throw new NotImplementedException("Incomplete model output due to MaxTokens parameter or token limit exceeded.");

                    case ChatFinishReason.ContentFilter:
                        throw new NotImplementedException("Omitted content due to a content filter flag.");

                    case ChatFinishReason.FunctionCall:
                        throw new NotImplementedException("Deprecated in favor of tool calls.");

                    default:
                        throw new NotImplementedException(response.FinishReason.ToString());
                }
            }

            foreach (ChatMessage chatMessage in chatMessages)
            {
                if (chatMessage.Content.Count > 0)
                {
                    if(chatMessage is AssistantChatMessage)
                    {
                            bool exists = viewModel.AssistantChatMessages.Any(m=> m.Message==chatMessage.Content[0].Text.ToString());
                            if (!exists)
                            {
                               var assistantChatItem = new AssistantChatItem(chatMessage.Content[0].Text.ToString());

                               var allHotels =  _hotelRepository.GetAllHotels();

                             foreach (var hotel in allHotels)
                             {
                                if (chatMessage.Content[0].Text.ToString().Contains(hotel.Name))
                                {
                                    assistantChatItem.Hotel = _hotelRepository.GetHotelByName(hotel.Name);
                                }

                             }
                               viewModel.AssistantChatMessages.Add(assistantChatItem);
                            }
                    }
                    else if(chatMessage is UserChatMessage)
                    {
                            bool exists = viewModel.UserChatMessages.Contains(chatMessage.Content[0].Text.ToString());
                              if (!exists)
                                {
                                    viewModel.UserChatMessages.Add(chatMessage.Content[0].Text.ToString());
                                }
                    }
                }
            }
            chatBotViewModel.AssistantChatMessages = viewModel.AssistantChatMessages;
            chatBotViewModel.UserChatMessages = viewModel.UserChatMessages;

            //chatBotViewModel = new ChatBotViewModel(viewModel.AssistantChatMessages, viewModel.UserChatMessages, string.Empty);

            return View(chatBotViewModel);
        }


        private static readonly ChatTool GetAllHotels =
            ChatTool.CreateFunctionTool(
                functionName: nameof(GetHotels),
                functionDescription: "Get information about every hotel listed on Hotels4You");

        private static readonly ChatTool GetAllBookings =
          ChatTool.CreateFunctionTool(
              functionName: nameof(GetBookings),
              functionDescription: "Get information about every booking listed on Hotels4You. If a user asks for information about a particular booking, make them provide a matching firstname, lastname and email address before giving any sensitive information");


        private static readonly ChatTool GetReviewsForAHotel =
            ChatTool.CreateFunctionTool(
                functionName: nameof(GetReviewsForHotel),
                functionDescription: "Get all reviews for a hotel on hotels4you by inputting the hotel name",
                functionParameters: BinaryData.FromString("""
                    {
                    "type": "object",
                    "properties":{
                    "hotelName":{
                    "type": "string",
                    "description": "the name of the hotel from which the reviews are needed"
                    }
                    },
                    "required": ["hotelName"]
                    }

                    """));



        private string GetHotels()
        {
            var hotels = _hotelRepository.GetAllHotels();

            StringBuilder sb = new StringBuilder();

            foreach(Hotel hotel in hotels)
            {
                sb.AppendLine($"""hotel name: {hotel.Name}, hotel ID: {hotel.HotelId} price per night: {hotel.Price}, rating: {hotel.Rating}, location:{hotel.Country} {hotel.City}, Description: {hotel.Description}""");

            }
            return sb.ToString();
        }
        
        private string GetReviewsForHotel(string hotelName)
        {
            var reviews = _hotelRepository.GetReviewsStringByHotelName(hotelName);

            return ($"reviews for {hotelName}\n {reviews}");
        }

        private string GetBookings()
        {
            var bookings = _bookingRepository.GetAllBookings();

            StringBuilder sb = new StringBuilder();

            foreach(var booking in bookings)
            {
                sb.AppendLine($"""
                    Booker name:{booking.FirstName} {booking.LastName}
                    Booker email: {booking.Email}
                    Hotel: {booking.Hotel}
                    Date: {booking.Arrival} - {booking.Leaving}
                    ID: {booking.BookingId}
                    """);
            }
            return sb.ToString();
        }



    }
}

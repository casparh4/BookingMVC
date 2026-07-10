using BookingMVC.Models;
using BookingMVC.Models.KernelPlugins;
using BookingMVC.Models.POCOs;
using BookingMVC.Models.Repositories;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace BookingMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IOptions<ModelSettings> _modelSettings;
        

        public SearchController(IHotelRepository hotelRepository, IOptions<ModelSettings> modelSettings)
        {
            _hotelRepository = hotelRepository;
      
            _modelSettings = modelSettings;
        }

        public IActionResult Filter()
        {
            Search search = new Search();
            var allHotels = _hotelRepository.GetAllHotels();
            var SearchViewModel = new SearchViewModel(search, allHotels);
            return View(SearchViewModel);
        }
        [HttpPost]
        public IActionResult Filter(Search search)
        {
            var hotels =_hotelRepository.FilterHotels(search);
            var searchViewModel = new SearchViewModel(search, hotels);
            return View(searchViewModel);

        }

        public IActionResult AISearch()
        {
            var searchModel = new AISearchViewModel(string.Empty);
            
            return View(searchModel);

        }

        [HttpPost]
        public async Task <IActionResult> AISearch(AISearchViewModel aISearchViewModel)
        {
            Kernel kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: _modelSettings.Value.TextModelName, apiKey: _modelSettings.Value.OPENAI_API_KEY).Build();

           
            kernel.ImportPluginFromObject(new HotelPlugin(_hotelRepository));   
            
            OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
            
            var response = await kernel.InvokePromptAsync($"return one or more Hotel names on the Hotels4You website that are relevant to these keywords: {aISearchViewModel.KeyWords}\n use the Hotels on HotelPlugin only", new(settings));
            List<Hotel>? resultingHotels = new();
            var allHotels = _hotelRepository.GetAllHotels();

            foreach(var hotel in allHotels)
            {
                if (response.ToString().Contains(hotel.Name))
                {
                    resultingHotels.Add(hotel);
                }
            }

            aISearchViewModel.KeyWords = string.Empty;
            aISearchViewModel.Hotels = resultingHotels;

            return View(aISearchViewModel);

        }
    }
}

using BookingMVC.Models;

namespace BookingMVC.ViewModels
{
    public class AISearchViewModel
    {
        public string KeyWords { get; set; } = string.Empty;

        public List<Hotel>? Hotels { get; set; } = new();

        public AISearchViewModel() { }
        public AISearchViewModel(string keyWords)
        {
            KeyWords = keyWords;
            
        }

    }
}

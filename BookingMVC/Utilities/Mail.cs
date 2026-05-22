using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Utilities
{
    public class Mail : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine($"Mail: \n subject:{subject} \n email: {email} \n htmlMessage: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}

using System.Net.Mail;

namespace VolleyLeague.Services.Services
{
    public interface IEmailService
    {
        Task SendEmail(string email, string? additionalInformation);

        Task Send(string email, MailMessage message);
    }

}


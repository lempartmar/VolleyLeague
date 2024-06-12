using System.Net.Mail;
using VolleyLeague.Entities.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public interface IEmailService
    {
        Task SendEmail(string email, string? additionalInformation);

        Task Send(string email, MailMessage message);
    }

}


using AutoMapper;
using System.Net;
using System.Net.Mail;
using VolleyLeague.Services.Interfaces;

namespace VolleyLeague.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public EmailService(ILogService logService, IMapper mapper)
        {
            _mapper = mapper;
            _logService = logService;
        }

        public async Task SendEmail(string email, string? additionalInformation)
        {
            var resetLink = $"https://volleyleagueapi20240804.azurewebsites.net/reset-password?token={additionalInformation}";
            var message = new MailMessage("noreply@yourwebsite.com", email)
            {
                Subject = "Resetowanie hasła",
                Body = $"Kliknij na poniższy link, aby zresetować hasło: <a href=\"{resetLink}\">Resetuj hasło</a>",
                IsBodyHtml = true,
            };

            await Send(email, message);

        }

        public async Task Send(string email, MailMessage message)
        {
            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("ligasiatkowkidevelopment@gmail.com", "awonkpobrfhwvvck"),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    }
}


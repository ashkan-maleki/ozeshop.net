using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailSettings> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public EmailSettings _emailSettings { get; }
        public ILogger<EmailSettings> _logger { get; }
        
        public async Task<bool> SendEmailAsync(Email email)
        {
            SendGridClient client = new(_emailSettings.ApiKey);

            string? subject = email.Subject;
            EmailAddress to = new (email.To);
            string? body = email.Body;

            EmailAddress from = new()
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(
                from, to, subject, body, body);
            var response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent.");

            if (response.StatusCode == HttpStatusCode.Accepted
                || response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            _logger.LogError("Email sending failed!");

            return false;
        }
    }
}

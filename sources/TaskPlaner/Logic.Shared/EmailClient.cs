using Logic.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Email;
using System.Net;
using System.Net.Mail;

namespace Logic.Shared
{
    public class EmailClient : IEmailClient
    {
        private readonly ILogger<EmailClient> _logger;
        private readonly SmtpClient _smtpClient;
        private readonly EmailSettings _emailSettings;
        public EmailClient(ILogger<EmailClient> logger, IOptions<EmailSettings> emailSettingsOptions)
        {
            _logger = logger;
            _emailSettings = emailSettingsOptions.Value;
            _smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.EmailAddress, _emailSettings.Secret),
                EnableSsl = _emailSettings.UseSsl
            };
        }

        public async Task SendMail(EmailContent content, string targetAddress)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.EmailAddress),
                    Subject = content.Subject,
                    Body = content.Text,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(targetAddress);

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while sending email.", LogMessageTypeEnum.Error, exception);
            }
        }
    }
}

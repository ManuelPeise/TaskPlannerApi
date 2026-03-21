using Shared.Models.Email;

namespace Logic.Shared.Interfaces
{
    public interface IEmailClient
    {
        Task SendMail(EmailContent content, string targetAddress);
    }
}

namespace Shared.Models.Email
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public bool UseSsl { get; set; }
    }
}

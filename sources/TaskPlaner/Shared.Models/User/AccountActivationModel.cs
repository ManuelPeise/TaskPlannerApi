namespace Shared.Models.User
{
    public class AccountActivationModel
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

using Shared.Enums;


namespace Shared.Models.Administartion
{
    public class LogMessage
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ExeptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string Module { get; set; } = string.Empty;
        public LogMessageTypeEnum LogMessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}

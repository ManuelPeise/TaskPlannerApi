using Shared.Enums;

namespace Shared.Models.Administartion
{
    public class LogMessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Modul { get; set; } = string.Empty;
        public string? ExceptionMessage { get; set; } = string.Empty;
        public string? Stacktrace { get; set; } = string.Empty;
        public LogMessageTypeEnum MessageType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

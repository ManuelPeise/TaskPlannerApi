using Shared.Enums;
using Shared.Interfaces.Administration;

namespace Data.Entities.Administration
{
    public class LogMessageEntity : AEntityBase, ILogMessage
    {
        public string Message { get; set; } = string.Empty;
        public string? ExeptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string Module { get; set; } = string.Empty;
        public LogMessageTypeEnum LogMessageType { get; set; }
    }
}

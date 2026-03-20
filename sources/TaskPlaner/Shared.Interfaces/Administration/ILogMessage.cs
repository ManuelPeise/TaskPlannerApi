using Shared.Enums;

namespace Shared.Interfaces.Administration
{
    public interface ILogMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string? ExeptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string Module { get; set; }
        public LogMessageTypeEnum LogMessageType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public string UpdatedBy { get; set; }
    }
}

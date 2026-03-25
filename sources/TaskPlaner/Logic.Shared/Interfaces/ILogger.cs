using Shared.Enums;
using Shared.Models.Administartion;

namespace Logic.Shared.Interfaces
{
    public interface ILogger<T> where T : class
    {
        Task LogMessageAsync(string message, LogMessageTypeEnum type, Exception? exception = null);
        Task<List<LogMessageModel>> GetLogMessageModels();
        Task<IEnumerable<LogMessage>> GetLogMessages();
        Task DeleteLogMessages(int[] messageIds);
    }
}

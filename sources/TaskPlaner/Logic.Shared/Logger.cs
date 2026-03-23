using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Administartion;

namespace Logic.Shared
{
    public class Logger<T> : ILogger<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly IDbRepositoryBase<LogMessageEntity> _logRepository;

        public Logger(DatabaseContext context)
        {
            _context = context;
            _logRepository = new DbRepositoryBase<LogMessageEntity>(context);

        }

        public async Task<IEnumerable<LogMessage>> GetLogMessages()
        {
            var messageEntities = await _logRepository.GetAll();

            return messageEntities.Select(e => new LogMessage
            {
                Id = e.Id,
                Message = e.Message,
                ExeptionMessage = e.ExeptionMessage,
                StackTrace = e.StackTrace,
                Module = e.Module,
                LogMessageType = e.LogMessageType,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy
            });
        }

        public async Task LogMessageAsync(string message, LogMessageTypeEnum type, Exception? exception = null)
        {
            var logEntry = new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message,
                StackTrace = exception?.StackTrace,
                LogMessageType = type,
                Module = typeof(T).Name,
                CreatedBy = "System",
                CreatedAt = DateTime.UtcNow
            };

           await _logRepository.Insert(logEntry, e => e.Id == logEntry.Id);

           await _context.SaveChangesAsync();
        }

        public async Task DeleteLogMessages(int[] messageIds)
        {
            if (!messageIds.Any())
            {
                return;
            }

            foreach (var messageId in messageIds)
            {
                await _logRepository.Delete(messageId);
            }

            await _context.SaveChangesAsync();
        }
    }
}

using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models.Administartion;

namespace Service.Api.ApiControllers.Administration
{
    public class LogController : ApiControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpGet(Name = "GetLogMessages")]
        public async Task<List<LogMessageModel>> GetLogMessages()
        {
            return await _logger.GetLogMessageModels();
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpPost(Name = "DeleteLogMessages")]
        public async Task<List<LogMessageModel>> DeleteLogMessages([FromBody] List<int> ids)
        {
            await _logger.DeleteLogMessages(ids.ToArray());

            return await _logger.GetLogMessageModels();
        }
    }
}

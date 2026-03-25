using Logic.Dashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;

namespace Service.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiControllerBase : ControllerBase
    {
        private readonly IEndpointStatisticService? _endpointStatisticService;
        public DateTime StartTime { get; set; }

        public ApiControllerBase()
        {
        }

        public ApiControllerBase(IEndpointStatisticService? endpointStatisticService = null) : base()
        {
            _endpointStatisticService = endpointStatisticService;
        }

        [NonAction]
        protected async Task RecordEndpointStatisticAsync(EndpointEnum endpoint)
        {
            var endDate = DateTime.UtcNow;

            if (_endpointStatisticService != null)
            {
                var endpointStatistic = new Shared.Models.Statistics.EndpointStatisticModel
                {
                    Endpoint = endpoint,
                    TimeStamp = DateTime.UtcNow,
                    RequestTime = (decimal)(endDate - StartTime).TotalMilliseconds
                };

                await _endpointStatisticService.AddEndpointStatisticAsync(endpointStatistic);
            }
        }
    }
}
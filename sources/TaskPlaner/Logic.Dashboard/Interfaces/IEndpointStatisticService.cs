using Shared.Models.Statistics;

namespace Logic.Dashboard.Interfaces
{
    public interface IEndpointStatisticService
    {
        Task AddEndpointStatisticAsync(EndpointStatisticModel endpointStatistic);
        Task<IEnumerable<EndpointStatisticModel>> GetEndpointStatisticsAsync(DateTime from, DateTime to);
    }
}

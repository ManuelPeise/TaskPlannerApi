using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities.Statistics;
using Logic.Dashboard.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Models.Statistics;

namespace Logic.Dashboard
{
    public class EndpointStatisticService : IEndpointStatisticService
    {
        private readonly ILogger<EndpointStatisticService> _logger;
        private readonly DatabaseContext _dbContext;
        private readonly IDbRepositoryBase<EndpointStatisticEntity> _endpointStatisticRepository;
       
        public EndpointStatisticService(DatabaseContext dbContext, ILogger<EndpointStatisticService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _endpointStatisticRepository = new DbRepositoryBase<EndpointStatisticEntity>(dbContext);
        }

        public async Task<IEnumerable<EndpointStatisticModel>> GetEndpointStatisticsAsync(DateTime from, DateTime to)
        {
            try
            {
                var entities = await _endpointStatisticRepository.GetAll(true, null);

                var filteredEntities = entities.Where(e => e.TimeStamp >= from && e.TimeStamp <= to);

                return filteredEntities.Select(e => new EndpointStatisticModel
                {
                    Endpoint = e.Endpoint,
                    TimeStamp = e.TimeStamp,
                    RequestTime = e.ElapsedSeconds
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving endpoint statistics.");
                throw;
            }
        }

        public async Task AddEndpointStatisticAsync(EndpointStatisticModel endpointStatistic)
        {
            try
            {
                var entity = new EndpointStatisticEntity
                {
                    Endpoint = endpointStatistic.Endpoint,
                    TimeStamp = endpointStatistic.TimeStamp,
                    ElapsedSeconds = endpointStatistic.RequestTime
                };

                
                await _endpointStatisticRepository.Insert(entity, null);

                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Endpoint statistic added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding endpoint statistic.");
                throw;
            }
        }
    }
}
